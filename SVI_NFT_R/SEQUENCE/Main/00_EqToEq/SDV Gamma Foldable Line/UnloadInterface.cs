using EqToEq;
using Mcc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SVI_NFT_R.EqToEq.SdvGammaFoldableLine
{
    public partial class UnloadInterface : IProcessManagerUnloadInterface, IActiveVacuumRequest, ICheckPending
    {
        public bool IsPending
        {
            get
            {
                return (
                    mDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE] == CCIMDefine.EMoveState.MOVE_STATE_RUNNING
                    && mDefine.SelfBits.SendAble.Value == true
                    && IsHandShaking == false
                    );
            }
        }
        public bool IsLowerHeartBeat
        {
            get
            {
                return (
                    mbLowerIsSurvival == true
                    );
            }
        }
        public bool IsLowerEmergency
        {
            get
            {
                return mDefine.LowerBits.EmsSafe.Value == false;
            }
        }
        public bool IsLowerAbnormal
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsLowerPause
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsLowerInterlock
        {
            get
            {
                return mDefine.LowerBits.InterlockSafe.Value == false;
            }
        }
        public bool IsLowerDoorOpen
        {
            get
            {
                return mDefine.LowerBits.InterlockDoorClosed.Value == false;
            }
        }
        public bool IsLowerMcStatus
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsLowerHeavyAlarm
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsLowerLightAlarm
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsLowerInitialStatus
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsLowerStartSignalStatus
        {
            get
            {
                return (
                    mDefine.LowerBits.ReceiveStart.Value == true
                    );
            }
        }
        public bool IsHandShaking
        {
            get
            {
                return (
                    GetIsHandShaking() == true
                    );
            }
        }
        public bool IsSelfHeartBeat
        {
            get
            {
                return (
                    mDefine.SelfBits.Heartbeat.Value == true
                    );
            }
        }
        public bool IsSelfEmergency
        {
            get
            {
                return mDefine.SelfBits.EmsSafe.Value == false;
            }
        }
        public bool IsSelfAbnormal
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsSelfPause
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsSelfInterlock
        {
            get
            {
                return mDefine.SelfBits.InterlockSafe.Value == false;
            }
        }
        public bool IsSelfDoorOpen
        {
            get
            {
                return mDefine.SelfBits.InterlockDoorClosed.Value == false;
            }
        }
        public bool IsSelfMcStatus
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsSelfHeavyAlarm
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsSelfLightAlarm
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsSelfInitialStatus
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsUnloadCompleted { get; set; }
        public string[] LowerSignalNames
        {
            get
            {
                return new string[0];
            }
        }
        public string[] SelfSignalNames
        {
            get
            {
                return new string[0];
            }
        }
        public bool IsInitialized
        {
            get
            {
                return mbInitialized;
            }
        }
        public object[] SignalControllers => mSignalControllers;
        private object[] mSignalControllers;
        private const int SIMULATION_DELAY = 200;
        private readonly int mPreActionCount = 0;
        private readonly int mMidActionCount = 0;
        private readonly int mPostActionCount = 0;
        private CDocument mDocument;
        private Thread mThreadProcess;
        private bool mbInitialized = false;
        private bool mbThreadExit = false;
        private bool mbLowerIsSurvival = false;
        private bool mbLastLowerHeartBeat = false;
        private bool mbLastSelfHeartBeat = false;
        private readonly Stopwatch mLowerHeartBeatTimeoutStopwatch = Stopwatch.StartNew();
        private readonly TimeSpan mLowerHeartBeatTimeout = TimeSpan.FromSeconds(5);
        private readonly Stopwatch mHeartBeatUpdateStopwatch = Stopwatch.StartNew();
        private readonly TimeSpan mHeartBeatPeriod = TimeSpan.FromMilliseconds(500);
        private readonly Utils.DigitalSignalObserver mLowerSignalObserver = new Utils.DigitalSignalObserver();
        private int mPositionIndex = 0;
        private DateTime mLastSignalClearDateTime = DateTime.Now;
        private DateTime mLastPendingDateTime = DateTime.Now;

        public bool Initialize(CDocument document, int positionIndex)
        {
            if (mbInitialized == true)
            {
                return true;
            }

            mDocument = document;
            mPositionIndex = positionIndex;
            AttachRealSignalDevice();
            mbThreadExit = false;
            mThreadProcess = new Thread(ThreadProcess);
            mThreadProcess.Start();

            var mObserverList = new List<Utils.DigitalSignalObserverOption>()
            {
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.SendAble), mDefine.Name, () => mDefine.SelfBits.SendAble.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.SendStart), mDefine.Name, () => mDefine.SelfBits.SendStart.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.SendComplete), mDefine.Name, () => mDefine.SelfBits.SendComplete.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.SendVacuumOnP1), mDefine.Name, () => mDefine.SelfBits.SendVacuumOnP1.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.SendVacuumOnP2), mDefine.Name, () => mDefine.SelfBits.SendVacuumOnP2.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.SendCellP1), mDefine.Name, () => mDefine.SelfBits.SendCellP1.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.SendCellP2), mDefine.Name, () => mDefine.SelfBits.SendCellP2.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.EmsSafe), mDefine.Name, () => mDefine.SelfBits.EmsSafe.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.InterlockDoorClosed), mDefine.Name, () => mDefine.SelfBits.InterlockDoorClosed.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.InterlockSafe), mDefine.Name, () => mDefine.SelfBits.InterlockSafe.Value),

                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.LowerBits.ReceiveAble), mDefine.Name, () => mDefine.LowerBits.ReceiveAble.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.LowerBits.ReceiveStart), mDefine.Name, () => mDefine.LowerBits.ReceiveStart.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.LowerBits.ReceiveComplete), mDefine.Name, () => mDefine.LowerBits.ReceiveComplete.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.LowerBits.ReceiveVacuumOnP1), mDefine.Name, () => mDefine.LowerBits.ReceiveVacuumOnP1.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.LowerBits.ReceiveVacuumOnP2), mDefine.Name, () => mDefine.LowerBits.ReceiveVacuumOnP2.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.LowerBits.ReceiveCellP1), mDefine.Name, () => mDefine.LowerBits.ReceiveCellP1.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.LowerBits.ReceiveCellP2), mDefine.Name, () => mDefine.LowerBits.ReceiveCellP2.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.LowerBits.EmsSafe), mDefine.Name, () => mDefine.LowerBits.EmsSafe.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.LowerBits.InterlockDoorClosed), mDefine.Name, () => mDefine.LowerBits.InterlockDoorClosed.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.LowerBits.InterlockSafe), mDefine.Name, () => mDefine.LowerBits.InterlockSafe.Value),
            };
            mLowerSignalObserver.SignalChanged += LowerSignalObserver_SignalChanged;
            mLowerSignalObserver.Initialize(mObserverList);

            IsUnloadCompleted = false;
            mbInitialized = true;
            return true;
        }

        public void DeInitialize()
        {
            mLowerSignalObserver.DeInitialize();
            mLowerSignalObserver.SignalChanged -= LowerSignalObserver_SignalChanged;

            mbThreadExit = true;
            mThreadProcess.Join();

            mbInitialized = false;
        }

        public void AttachRealSignalDevice()
        {
            Address.DeInitialize();
            var option = new AddressOptionSet()
            {
                CcLinkIeControlNetworkDevB = mDocument.m_objProcessMain.IoHardware.CcLinkIeControlNetworkDevB,
                CcLinkIeControlNetworkDevW = mDocument.m_objProcessMain.IoHardware.CcLinkIeControlNetworkDevW
            };
            Address.Initialize(option);
            mDefine = new Define(mPositionIndex);
            mSignalControllers = new UnloadInterfaceSignalController[] { mDefine };
        }

        public void AttachVirtualSignalDevice()
        {
            Address.DeInitialize();
            var option = new AddressOptionSet()
            {
                CcLinkIeControlNetworkDevB = mDocument.m_objProcessMain.IoHardware.VirtualCcLinkIeControlNetworkDevB,
                CcLinkIeControlNetworkDevW = mDocument.m_objProcessMain.IoHardware.VirtualCcLinkIeControlNetworkDevW
            };
            Address.Initialize(option);
            mDefine = new Define(mPositionIndex);
            mSignalControllers = new UnloadInterfaceSignalController[] { mDefine };
        }

        public void UpdateTickHeartbeatSignals()
        {
            // Update Self HeartBeat
            if (mHeartBeatPeriod < mHeartBeatUpdateStopwatch.Elapsed)
            {
                mbLastSelfHeartBeat = !mbLastSelfHeartBeat;
                mDefine.SelfBits.Heartbeat.Value = mbLastSelfHeartBeat;
                // 가상 모드 처리
                mDefine.LowerBits.Heartbeat.Value = mbLastSelfHeartBeat;
                mHeartBeatUpdateStopwatch.Restart();
            }

            // Check Lower HeartBeat
            if (mbLastLowerHeartBeat == mDefine.LowerBits.Heartbeat.Value)
            {
                if (mLowerHeartBeatTimeout < mLowerHeartBeatTimeoutStopwatch.Elapsed)
                {
                    mbLowerIsSurvival = false;
                }
            }
            else
            {
                mbLowerIsSurvival = true;
                mbLastLowerHeartBeat = mDefine.LowerBits.Heartbeat.Value;
                mLowerHeartBeatTimeoutStopwatch.Restart();
            }
        }

        public void SetEmergency(bool bSignal)
        {
            mDefine.SelfBits.EmsSafe.Value = bSignal == false;
        }

        public void SetAbnormal(bool bSignal)
        {
            // 미사용 신호
        }

        public void SetPause(bool bSignal)
        {
            // 미사용 신호
        }

        public void SetInterlock(int position, bool bSignal)
        {
            mDefine.SelfBits.InterlockSafe.Value = bSignal == false;
        }

        public bool GetSelfInterlock(int position)
        {
            return mDefine.SelfBits.InterlockSafe.Value == false;
        }

        public bool GetLowerInterlock(int position)
        {
            return mDefine.LowerBits.InterlockSafe.Value == false;
        }

        public void SetDoorOpen(bool bSignal)
        {
            mDefine.SelfBits.InterlockDoorClosed.Value = bSignal == false;
        }

        public void SetMcStatus(bool bSignal)
        {
            // 미사용 신호
        }

        public void SetHeavyAlarm(bool bSignal)
        {
            // 미사용 신호
        }

        public void SetLightAlarm(bool bSignal)
        {
            // 미사용 신호
        }

        public void SetInitialStatus(bool bSignal)
        {
            // 미사용 신호
        }

        public void DryRunSignalClear()
        {
            mDefine.DryRunSignalClear();
            IsUnloadCompleted = false;
        }

        public Task<bool> TaskVacuumOnRequest(EPosition placement, TimeSpan timeout)
        {
            return Task.Run(() =>
            {
                if (placement.HasFlag(EPosition.P1) && placement.HasFlag(EPosition.P2))
                {
                    mDefine.SelfBits.SendVacuumOnP1.Value = true;
                    mDefine.SelfBits.SendVacuumOnP2.Value = true;
                    if (mDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
                    {
                        Thread.Sleep(SIMULATION_DELAY);
                        mDefine.LowerBits.ReceiveVacuumOnP1.Value = true;
                        mDefine.LowerBits.ReceiveVacuumOnP2.Value = true;
                    }
                    if (SpinWait.SpinUntil(() => mDefine.LowerBits.ReceiveVacuumOnP1.Value == true && mDefine.LowerBits.ReceiveVacuumOnP2.Value == true, timeout) == false)
                    {
                        return false;
                    }
                }
                else if (placement.HasFlag(EPosition.P1))
                {
                    mDefine.SelfBits.SendVacuumOnP1.Value = true;
                    if (mDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
                    {
                        Thread.Sleep(SIMULATION_DELAY);
                        mDefine.LowerBits.ReceiveVacuumOnP1.Value = true;
                    }
                    if (SpinWait.SpinUntil(() => mDefine.LowerBits.ReceiveVacuumOnP1.Value == true, timeout) == false)
                    {
                        return false;
                    }
                }
                else if (placement.HasFlag(EPosition.P2))
                {
                    mDefine.SelfBits.SendVacuumOnP2.Value = true;
                    if (mDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
                    {
                        Thread.Sleep(SIMULATION_DELAY);
                        mDefine.LowerBits.ReceiveVacuumOnP2.Value = true;
                    }
                    if (SpinWait.SpinUntil(() => mDefine.LowerBits.ReceiveVacuumOnP2.Value == true, timeout) == false)
                    {
                        return false;
                    }
                }
                return true;
            });
        }

        public bool IsOtherEquipmentPendingTimeout()
        {
            // true=타임아웃 발생, false=정상
            if (IsPending == true)
            {
                if (DateTime.Now - mLastPendingDateTime < Config.WaitTime.Equipment.UnloadInterfacePendingTime.ToTimeSpan())
                {
                    return false;
                }
            }
            else
            {
                mLastPendingDateTime = DateTime.Now;
                return false;
            }
            return true;
        }

        public bool IsOtherEquipmentSingalClearTimeout()
        {
            // true=타임아웃 발생, false=정상
            if (mDefine.LowerBits.ReceiveComplete.Value == true
                && mDefine.SelfBits.SendAble.Value == false
                && mDefine.SelfBits.SendStart.Value == false
                && mDefine.SelfBits.SendComplete.Value == false
                )
            {
                if (DateTime.Now - mLastSignalClearDateTime < Config.WaitTime.Equipment.UnloadInterfacePendingTime.ToTimeSpan())
                {
                    return false;
                }
            }
            else
            {
                mLastSignalClearDateTime = DateTime.Now;
                return false;
            }
            return true;
        }

        public EPosition GetOtherEquipmentSelectPosition()
        {
            EPosition selectPosition = 0;
            if (mDefine.LowerBits.ReceiveCellP1.Value == true)
            {
                selectPosition |= EPosition.P1;
            }
            if (mDefine.LowerBits.ReceiveCellP2.Value == true)
            {
                selectPosition |= EPosition.P2;
            }
            if (selectPosition == 0)
            {
                selectPosition = EPosition.Empty;
            }
            return selectPosition;
        }

        public static string SIGNAL_DUMP_LOG_CSV_HEADER()
        {
            return string.Join(",",
                "DateTime",
                "Function",
                "Comment",

                "SelfSendAble",
                "SelfSendStart",
                "SelfSendComplete",
                "SelfSendVacuumOn",
                "SelfSendCellP1",
                "SelfSendCellP2",
                "SelfEmsSafe",
                "SelfDoorClose",
                "SelfInterlockSafe",

                "LowerSurvival",
                "LowerSendAble",
                "LowerSendStart",
                "LowerSendComplete",
                "LowerSendVacuumOn",
                "LowerSendCellP1",
                "LowerSendCellP2",
                "LowerEmsSafe",
                "LowerDoorClose",
                "LowerInterlockSafe"
                );
        }

        private void WriteLogSignalDump(string comment, [CallerMemberName] string callerMemberName = "")
        {
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_SIGNAL_INTERFACE_UNLOAD, string.Join(",",
                $"\t{DateTime.Now:yyyy-MM-dd HH:mm:ss:fff}",
                callerMemberName,
                comment,

                $"{Convert.ToInt32(mDefine.SelfBits.SendAble.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.SendStart.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.SendComplete.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.SendVacuumOnP1.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.SendVacuumOnP2.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.SendCellP1.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.SendCellP2.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.EmsSafe.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.InterlockDoorClosed.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.InterlockSafe.Value)}",

                $"{Convert.ToInt32(mbLowerIsSurvival)}",
                $"{Convert.ToInt32(mDefine.LowerBits.ReceiveAble.Value)}",
                $"{Convert.ToInt32(mDefine.LowerBits.ReceiveStart.Value)}",
                $"{Convert.ToInt32(mDefine.LowerBits.ReceiveComplete.Value)}",
                $"{Convert.ToInt32(mDefine.LowerBits.ReceiveVacuumOnP1.Value)}",
                $"{Convert.ToInt32(mDefine.LowerBits.ReceiveVacuumOnP2.Value)}",
                $"{Convert.ToInt32(mDefine.LowerBits.ReceiveCellP1.Value)}",
                $"{Convert.ToInt32(mDefine.LowerBits.ReceiveCellP2.Value)}",
                $"{Convert.ToInt32(mDefine.LowerBits.EmsSafe.Value)}",
                $"{Convert.ToInt32(mDefine.LowerBits.InterlockDoorClosed.Value)}",
                $"{Convert.ToInt32(mDefine.LowerBits.InterlockSafe.Value)}"
                ));
        }

        private void LowerSignalObserver_SignalChanged(object sender, Utils.IDigitalSignalObserverItem e)
        {
            switch (e.Name)
            {
                case nameof(mDefine.SelfBits.SendAble):
                    break;

                case nameof(mDefine.SelfBits.SendStart):
                    break;

                case nameof(mDefine.SelfBits.SendComplete):
                    break;

                case nameof(mDefine.LowerBits.ReceiveAble):
                    break;

                case nameof(mDefine.LowerBits.ReceiveStart):
                    break;

                case nameof(mDefine.LowerBits.ReceiveComplete):
                    break;
            }

            string onOff = e.LastSignal ? "ON" : "OFF";
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_UNLOAD, $"{e.Comment}\t{e.Name}\t{onOff}");
        }

        private void DoProcessPreAction(int actionIndex = 0)
        {
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_UNLOAD, $"DoProcessPreAction({mDefine.Name},{actionIndex})");
            // Pre Action 없음
        }

        private void DoProcessAble()
        {
            WriteLogSignalDump("BEGIN");
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_UNLOAD, $"DoProcessAble({mDefine.Name})");

            var outRobot = mDocument.m_objProcessMain.m_objProcessMotion.OutRobot;
            mDefine.SelfBits.SendCellP1.Value = outRobot.CellContainer[0].IsCellExist();
            mDefine.SelfBits.SendCellP2.Value = outRobot.CellContainer[1].IsCellExist();

            // 언로드 완료 플래그 초기화
            IsUnloadCompleted = false;

            // Self의 Able 신호 ON
            mDefine.SelfBits.SendAble.Value = true;

            // 언링크 드라이런 처리
            if (mDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
            {
                Thread.Sleep(SIMULATION_DELAY);
                mDefine.LowerBits.ReceiveCellP1.Value = mDefine.SelfBits.SendCellP1.Value;
                mDefine.LowerBits.ReceiveCellP2.Value = mDefine.SelfBits.SendCellP2.Value;
                mDefine.LowerBits.ReceiveAble.Value = true;
                mDefine.LowerBits.ReceiveStart.Value = true;
                Thread.Sleep(SIMULATION_DELAY);
            }

            WriteLogSignalDump("END");
        }

        private void DoProcessStart()
        {
            WriteLogSignalDump("BEGIN");
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_UNLOAD, $"DoProcessStart({mDefine.Name})");

            // 언로드 완료 플래그 초기화
            IsUnloadCompleted = false;

            // Self의 Start 신호 ON
            mDefine.SelfBits.SendStart.Value = true;

            // 언링크 드라이런 처리
            if (mDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
            {
                Thread.Sleep(SIMULATION_DELAY);
            }

            WriteLogSignalDump("END");
        }

        private void DoProcessMidAction(int actionIndex = 0)
        {
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_UNLOAD, $"DoProcessMidAction({mDefine.Name},{actionIndex})");
            // Mid Action 없음
        }

        private void DoProcessComplete()
        {
            WriteLogSignalDump("BEGIN");
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_UNLOAD, $"DoProcessComplete({mDefine.Name})");

            // Self의 Complete 신호 ON
            mDefine.SelfBits.SendComplete.Value = true;

            // 언링크 드라이런 처리
            if (mDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
            {
                Thread.Sleep(SIMULATION_DELAY);
                mDefine.LowerBits.ReceiveComplete.Value = true;
            }

            WriteLogSignalDump("END");
        }

        private void DoProcessPostAction(int actionIndex = 0)
        {
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_UNLOAD, $"DoProcessPostAction({mDefine.Name},{actionIndex})");
            // Post Action 없음
        }

        private void DoProcessClearAllInterfaceSignals()
        {
            WriteLogSignalDump("BEGIN");
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_UNLOAD, $"DoProcessClearAllInterfaceSignals({mDefine.Name})");

            // 모든 인터페이스 신호 OFF
            mDefine.SelfBits.SendAble.Value = false;
            mDefine.SelfBits.SendStart.Value = false;
            mDefine.SelfBits.SendComplete.Value = false;
            mDefine.SelfBits.SendVacuumOnP1.Value = false;
            mDefine.SelfBits.SendVacuumOnP2.Value = false;
            mDefine.SelfBits.SendCellP1.Value = false;
            mDefine.SelfBits.SendCellP2.Value = false;

            if (mDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
            {
                Thread.Sleep(SIMULATION_DELAY);
                mDefine.LowerBits.ReceiveAble.Value = false;
                mDefine.LowerBits.ReceiveStart.Value = false;
                mDefine.LowerBits.ReceiveComplete.Value = false;
                mDefine.LowerBits.ReceiveVacuumOnP1.Value = false;
                mDefine.LowerBits.ReceiveVacuumOnP2.Value = false;
                mDefine.LowerBits.ReceiveCellP1.Value = false;
                mDefine.LowerBits.ReceiveCellP2.Value = false;
                Thread.Sleep(SIMULATION_DELAY);
            }
            WriteLogSignalDump("END");
        }

        private bool GetIsHandShaking()
        {
            // 개별 채널별로 H/S 진행중을 체크하기 위해 작성한 함수
            bool bResult = false;

            do
            {
                if (IsInitialized == false)
                {
                    break;
                }
                // Complete 신호가 하나라도 켜져있으면 H/S 진행중임
                bool bCompleteState = (
                    mDefine.SelfBits.SendComplete.Value == true
                    || mDefine.LowerBits.ReceiveComplete.Value == true
                    );
                // Able 신호가 모두 켜진 상태에서 Upper의 Start 신호가 켜져 있으면 H/S 진행중임
                bool bHandshakeState = (
                    mDefine.SelfBits.SendAble.Value == true
                    && mDefine.LowerBits.ReceiveAble.Value == true
                    && mDefine.SelfBits.SendStart.Value == true
                    && mDefine.LowerBits.ReceiveStart.Value == true
                    );

                if (
                    bCompleteState == false
                    && bHandshakeState == false
                    )
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        private bool IsHandShake(EInterface action, int actionIndex = 0)
        {
            bool bResult = false;

            switch (action)
            {
                case EInterface.PreAction:
                    {
                        // Pre Action 없음
                    }
                    break;

                case EInterface.Able:
                    {
                        // Manual Output 모드다
                        if (mDocument.IsManualInputMode == true
                            && mDocument.GetRunMode() == CDefine.ERunMode.RealRun
                            )
                        {
                            break;
                        }
                        // 인터페이스 진행중이다
                        if (IsHandShaking == true)
                        {
                            break;
                        }
                        // Out Robot에 셀 정보가 없다.
                        int existCellCount = CellDataManager.ProcessCells[CellData.EProcess.OutRobot].GetExistCellCount();
                        if (existCellCount == 0)
                        {
                            break;
                        }
                        // Out Robot이 Lower Process에 진입하지 않았다.
                        if (mDocument.m_objProcessMain.m_objProcessMotion.OutRobot.Nachi.GetStatus() != OutRobotNachi.EStatus.UnloadProcessBegin)
                        {
                            break;
                        }
                        // 설비가 런상태가 아니다 (RUN OR LOT_END가 아니면 진행하지 않음)
                        if (
                            mDocument.GetRunStatus() != CDefine.ERunStatus.Start
                            && mDocument.GetRunStatus() != CDefine.ERunStatus.LoadingStop
                            )
                        {
                            // Run 상태가 아닐 때는 Receive Able 신호 false로 변환
                            if (
                                mDefine.SelfBits.SendAble.Value == true
                                && mDefine.SelfBits.SendStart.Value == false
                                )
                            {
                                mDefine.SelfBits.SendAble.Value = false;
                                mDefine.SelfBits.SendCellP1.Value = false;
                                mDefine.SelfBits.SendCellP2.Value = false;
                            }
                            break;
                        }
                        // Self Able 신호가 ON이다
                        if (mDefine.SelfBits.SendAble.Value == true)
                        {
                            break;
                        }

                        bResult = true;
                    }
                    break;

                case EInterface.Start:
                    {
                        // 설비가 런상태가 아니다 (RUN OR LOT_END가 아니면 진행하지 않음)
                        if (
                            mDocument.GetRunStatus() != CDefine.ERunStatus.Start
                            && mDocument.GetRunStatus() != CDefine.ERunStatus.LoadingStop
                            )
                        {
                            break;
                        }
                        if (
                            // 인터페이스 진행중이다
                            IsHandShaking == true
                            // 하류 설비 인터페이스 가능 상태 확인
                            || mbLowerIsSurvival == false
                            // Self와 하류 설비의 ABLE 신호가 모두 OFF다
                            || mDefine.LowerBits.ReceiveAble.Value == false
                            || mDefine.SelfBits.SendAble.Value == false
                            // 하류 설비의 Start가 OFF 이다
                            || mDefine.LowerBits.ReceiveStart.Value == false
                            // Self Start 신호가 ON이다
                            || mDefine.SelfBits.SendStart.Value == true
                            )
                        {
                            break;
                        }

                        bResult = true;
                    }
                    break;

                case EInterface.MidAction:
                    {
                        // Mid Action 없음
                    }
                    break;

                case EInterface.Complete:
                    {
                        if (
                            // 인터페이스 미진행중이다
                            IsHandShaking == false
                            // Self Start 신호가 OFF다
                            || mDefine.SelfBits.SendStart.Value == false
                            // Self Complete 신호가 ON이다
                            || mDefine.SelfBits.SendComplete.Value == true
                            // 하류 설비 Start 신호가 OFF다
                            || mDefine.LowerBits.ReceiveStart.Value == false
                            // 하류 설비 Complete 신호가 ON이다
                            || mDefine.LowerBits.ReceiveComplete.Value == true
                            // 언로드 미완료 상태다
                            || IsUnloadCompleted == false
                            )
                        {
                            break;
                        }


                        bResult = true;
                    }
                    break;

                case EInterface.PostAction:
                    {
                        // Post Action 없음
                    }
                    break;

                case EInterface.ClearAllInterfaceSignals:
                    {
                        if (
                            // 인터페이스 미진행중이다
                            IsHandShaking == false
                            // 언로드 미완료 상태다
                            || IsUnloadCompleted == false
                            // 하류 설비 Complete 신호가 OFF다
                            || mDefine.LowerBits.ReceiveComplete.Value == false
                            // Self Complete 신호가 OFF다
                            || mDefine.SelfBits.SendComplete.Value == false
                            )
                        {
                            break;
                        }

                        bResult = true;
                    }
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            return bResult;
        }

        private void ThreadProcess()
        {
            while (mDocument.IsInitialized == false)
            {
                Thread.Sleep(100);
            }

            while (mbThreadExit == false)
            {
                Thread.Sleep(10);
                // Pre Action
                for (int i = 0; i < mPreActionCount; i++)
                {
                    if (IsHandShake(EInterface.PreAction, i) == true)
                    {
                        DoProcessPreAction(i);
                    }
                }
                // Able
                if (IsHandShake(EInterface.Able) == true)
                {
                    DoProcessAble();
                }
                // Start
                if (IsHandShake(EInterface.Start) == true)
                {
                    DoProcessStart();
                }
                // Mid Action
                for (int i = 0; i < mMidActionCount; i++)
                {
                    if (IsHandShake(EInterface.MidAction, i) == true)
                    {
                        DoProcessMidAction(i);
                    }
                }
                // Complete
                if (IsHandShake(EInterface.Complete) == true)
                {
                    DoProcessComplete();
                }
                // Post Action
                for (int i = 0; i < mPostActionCount; i++)
                {
                    if (IsHandShake(EInterface.PostAction, i) == true)
                    {
                        DoProcessPostAction(i);
                    }
                }
                // Clear All
                if (IsHandShake(EInterface.ClearAllInterfaceSignals) == true)
                {
                    DoProcessClearAllInterfaceSignals();
                }
            }
        }
    }
}