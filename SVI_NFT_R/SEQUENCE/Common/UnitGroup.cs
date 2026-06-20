using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using static SVI_NFT_R.CCIMDefine;

namespace SVI_NFT_R
{
    public partial class UnitGroup
    {
        public enum ECommand
        {
            /// <summary>
            /// 사용
            /// </summary>
            Use = 0,
            /// <summary>
            /// 미사용
            /// </summary>
            Pause,
            /// <summary>
            /// 인터락
            /// </summary>
            Interlock
        }
        public enum EStatus
        {
            Unknown = 0,
            /// <summary>
            /// 사용 상태 전환중
            /// </summary>
            ChangingUse,
            /// <summary>
            /// 사용 상태 보고 완료
            /// </summary>
            Use,
            /// <summary>
            /// 미사용 상태 전환중
            /// </summary>
            ChangingPause,
            /// <summary>
            /// 미사용 상태 보고 완료
            /// </summary>
            Pause,
            /// <summary>
            /// 인터락 상태 전환중
            /// </summary>
            ChangingInterlock,
            /// <summary>
            /// 인터락 상태 보고 완료
            /// </summary>
            Interlock
        }
        public bool IsInitialize { get; private set; } = false;
        public EUnit UnitIndex { get; private set; }
        public IInputUnitNode InputUnitNode { get; private set; }
        public IReadOnlyList<IUnitNode> UnitNodes { get; private set; }
        private CDocument mDocument;
        private bool mbShouldStop = false;
        private Thread mThreadProcess;
        private bool mbShouldCycleStop = false;
        private bool mbShouldInterlockReport = false;
        private ManualResetEvent mOneCycleUnlock;
        private ManualResetEvent mOneCycleEnd;
        private readonly object mSyncRoot = new object();
        private static readonly string REGISTRY_PATH = Path.Combine("ENC", Program.ID, "SEQUENCE", nameof(UnitGroup));
        private readonly Settings.RegistryComponent mBackupData = new Settings.RegistryComponent(REGISTRY_PATH);

        public bool Initialize(CDocument document, EUnit unitIndex, List<IUnitNode> unitNodes)
        {
            if (IsInitialize == true)
            {
                return false;
            }

            mDocument = document;
            UnitIndex = unitIndex;
            UnitNodes = unitNodes;
            InputUnitNode = (IInputUnitNode)unitNodes.First(i => i is IInputUnitNode);
            Debug.Assert(InputUnitNode != null, "InputUnitNode를 찾을 수 없습니다. UnitNodes에 InputUnitNode이 반드시 존재해야 합니다.");

            Restore();

            mOneCycleUnlock = new ManualResetEvent(true);
            mOneCycleEnd = new ManualResetEvent(true);

            mbShouldStop = false;
            mThreadProcess = new Thread(ThreadProcess);
            mThreadProcess.IsBackground = true;
            mThreadProcess.Start();

            IsInitialize = true;
            return true;
        }

        public void DeInitialize()
        {
            if (IsInitialize == false)
            {
                return;
            }

            mbShouldStop = true;
            mThreadProcess.Join();

            mOneCycleUnlock.Dispose();
            mOneCycleEnd.Dispose();

            Backup();

            IsInitialize = false;
        }

        public bool SetCommand(ECommand command)
        {
            LockOneCycle();
            try
            {
                Func<bool> commandAction;
                switch (command)
                {
                    case ECommand.Use:
                        commandAction = DoProcessSetCommandUse;
                        break;

                    case ECommand.Pause:
                        commandAction = DoProcessSetCommandPause;
                        break;

                    case ECommand.Interlock:
                        commandAction = DoProcessSetCommandInterlock;
                        break;

                    default:
                        commandAction = DoProcessNothing;
                        Debug.Assert(false);
                        break;
                }

                lock (mSyncRoot)
                {
                    return commandAction.Invoke();
                }
            }
            finally
            {
                UnlockOneCycle();
            }
        }

        public EStatus GetStatus()
        {
            if (mbShouldCycleStop == false
                && mbShouldInterlockReport == false
                )
            {
                if (InputUnitNode.CanInput == true
                    && GetCimReportedUseStatus() == true
                    )
                {
                    return EStatus.Use;
                }
                else
                {
                    return EStatus.ChangingUse;
                }
            }
            if (mbShouldCycleStop == true
                && mbShouldInterlockReport == false
                )
            {
                if (InputUnitNode.CanInput == false
                    && GetCimReportedPauseStatus() == true
                    )
                {
                    return EStatus.Pause;
                }
                else
                {
                    return EStatus.ChangingPause;
                }
            }
            if (mbShouldCycleStop == true
                && mbShouldInterlockReport == true
                )
            {
                if (InputUnitNode.CanInput == false
                    && GetCimReportedInterlockStatus() == true
                    )
                {
                    return EStatus.Interlock;
                }
                else
                {
                    return EStatus.ChangingInterlock;
                }
            }
            return EStatus.Unknown;
        }

        private void Restore()
        {
            mbShouldCycleStop = Convert.ToBoolean(mBackupData.GetValue($"{UnitIndex}.{nameof(mbShouldCycleStop)}", 0));
            mbShouldInterlockReport = Convert.ToBoolean(mBackupData.GetValue($"{UnitIndex}.{nameof(mbShouldInterlockReport)}", 0));
        }

        private void Backup()
        {
            mBackupData.SetValue($"{UnitIndex}.{nameof(mbShouldCycleStop)}", Convert.ToInt32(mbShouldCycleStop));
            mBackupData.SetValue($"{UnitIndex}.{nameof(mbShouldInterlockReport)}", Convert.ToInt32(mbShouldInterlockReport));
        }

        private void LockOneCycle()
        {
            mOneCycleUnlock.Reset();
            mOneCycleEnd.WaitOne();
        }

        private void UnlockOneCycle()
        {
            mOneCycleUnlock.Set();
        }

        private bool DoProcessNothing() => false;

        private bool DoProcessSetCommandUse()
        {
            EStatus status = GetStatus();
            switch (status)
            {
                // 상태 전환 불가 case
                case EStatus.ChangingUse:
                case EStatus.ChangingInterlock:
                case EStatus.Use:
                    return false;

                // 상태 전환 가능 case
                case EStatus.ChangingPause:
                case EStatus.Pause:
                case EStatus.Interlock:
                    break;

                case EStatus.Unknown:
                default:
                    Debug.Assert(false);
                    return false;
            }

            mbShouldCycleStop = false;
            mbShouldInterlockReport = false;
            Backup();
            return true;
        }

        private bool DoProcessSetCommandPause()
        {
            EStatus status = GetStatus();
            switch (status)
            {
                // 상태 전환 불가 case
                case EStatus.ChangingPause:
                case EStatus.ChangingInterlock:
                case EStatus.Pause:
                case EStatus.Interlock:
                    return false;

                // 상태 전환 가능 case
                case EStatus.ChangingUse:
                case EStatus.Use:
                    break;

                case EStatus.Unknown:
                default:
                    Debug.Assert(false);
                    return false;
            }

            mbShouldCycleStop = true;
            mbShouldInterlockReport = false;
            Backup();
            return true;
        }

        private bool DoProcessSetCommandInterlock()
        {
            EStatus status = GetStatus();
            switch (status)
            {
                // 상태 전환 불가 case
                case EStatus.ChangingInterlock:
                case EStatus.Interlock:
                case EStatus.Pause:
                    return false;

                // 상태 전환 가능 case
                case EStatus.ChangingUse:
                case EStatus.ChangingPause:
                case EStatus.Use:
                    break;

                case EStatus.Unknown:
                default:
                    Debug.Assert(false);
                    return false;
            }

            mbShouldCycleStop = true;
            mbShouldInterlockReport = true;
            Backup();
            return true;
        }

        private bool DoProcessStopInput()
        {
            InputUnitNode.StopInput();
            return true;
        }

        private bool DoProcessResumeInput()
        {
            InputUnitNode.ResumeInput();
            return true;
        }

        private bool DoProcessCimReportUseStatus()
        {
            int unitIndex = (int)UnitIndex;
            int currentStateIndex = (int)EPresentState.CURRENT_STATE;
            mDocument.m_eUnitMoveState[unitIndex][currentStateIndex] = EMoveState.MOVE_STATE_RUNNING;
            mDocument.m_eUnitInterlockState[unitIndex][currentStateIndex] = EInterlockState.INTERLOCK_STATE_OFF;
            mDocument.m_objProcessCIM.UnitStatusChangeReport(UnitIndex);
            return true;
        }

        private bool DoProcessCimReportPauseStatus()
        {
            int unitIndex = (int)UnitIndex;
            int currentStateIndex = (int)EPresentState.CURRENT_STATE;
            mDocument.m_eUnitMoveState[unitIndex][currentStateIndex] = EMoveState.MOVE_STATE_PAUSE;
            mDocument.m_eUnitInterlockState[unitIndex][currentStateIndex] = EInterlockState.INTERLOCK_STATE_OFF;
            mDocument.m_objProcessCIM.UnitStatusChangeReport(UnitIndex);
            return true;
        }

        private bool DoProcessCimReportInterlockStatus()
        {
            int unitIndex = (int)UnitIndex;
            int currentStateIndex = (int)EPresentState.CURRENT_STATE;
            mDocument.m_eUnitMoveState[unitIndex][currentStateIndex] = EMoveState.MOVE_STATE_PAUSE;
            mDocument.m_eUnitInterlockState[unitIndex][currentStateIndex] = EInterlockState.INTERLOCK_STATE_ON;
            mDocument.m_objProcessCIM.UnitStatusChangeReport(UnitIndex);
            return true;
        }

        private void ThreadProcess()
        {
            // CDocument 초기화 완료 대기
            while (mDocument.IsInitialized == false)
            {
                Thread.Sleep(500);
            }

            int sequenceIndex = 0;
            EAutoSequence[] sequenceItems = Enum.GetValues(typeof(EAutoSequence)).Cast<EAutoSequence>().ToArray();

            while (mbShouldStop == false)
            {
                // END ONE CYCLE
                mOneCycleEnd.Set();

                // BEGIN ONE CYCLE
                mOneCycleUnlock.WaitOne();
                mOneCycleEnd.Reset();

                Thread.Sleep(33);

                EAutoSequence sequencePointer = sequenceItems[sequenceIndex];

                // 다음 시퀀스 인덱스 선택
                sequenceIndex = (sequenceItems.Length != sequenceIndex + 1) ? sequenceIndex + 1 : 0;

                // 시퀀스 조건 확인
                if (IsUnitGroup(sequencePointer) == false)
                {
                    continue;
                }

                switch (sequencePointer)
                {
                    case EAutoSequence.StopInput:
                        DoProcessStopInput();
                        break;

                    case EAutoSequence.ResumeInput:
                        DoProcessResumeInput();
                        break;

                    case EAutoSequence.CimReportUseStatus:
                        DoProcessCimReportUseStatus();
                        break;

                    case EAutoSequence.CimReportPauseStatus:
                        DoProcessCimReportPauseStatus();
                        break;

                    case EAutoSequence.CimReportInterlockStatus:
                        DoProcessCimReportInterlockStatus();
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }
        }
    }
}