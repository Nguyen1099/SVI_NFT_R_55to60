using EqToEq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SVI_NFT_R.EqToEq.SdvGammaFoldableLine
{
    public partial class LoadInterface : IProcessManagerLoadInterface, IPassiveVacuumAction
    {
        public bool IsPending
        {
            get
            {
                return (
                    mDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE] == CCIMDefine.EMoveState.MOVE_STATE_RUNNING
                    && mDefine.SelfBits.ReceiveAble.Value == true
                    && IsHandShaking == false
                    );
            }
        }
        public bool IsUpperHeartbeat
        {
            get
            {
                return mbUpperIsSurvival == true;
            }
        }
        public bool IsUpperEmergency
        {
            get
            {
                return mDefine.UpperBits.EmsSafe.Value == false;
            }
        }
        public bool IsUpperAbnormal
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsUpperPause
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsUpperInterlock
        {
            get
            {
                return mDefine.UpperBits.InterlockSafe.Value == false;
            }
        }
        public bool IsUpperDoorOpen
        {
            get
            {
                return mDefine.UpperBits.InterlockDoorClosed.Value == false;
            }
        }
        public bool IsUpperMcStatus
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsUpperHeavyAlarm
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsUpperLightAlarm
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsUpperInitialStatus
        {
            get
            {
                // 신호 미사용으로 항상 정상 상태 반환
                return false;
            }
        }
        public bool IsUpperStartSignalStatus
        {
            get
            {
                return (
                    mDefine.UpperBits.SendStart.Value == true
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
        public string[] UpperSignalNames
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
        public bool IsLoadCompleted { get; set; }
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
        private bool mbUpperIsSurvival = false;
        private bool mbLastUpperHeartBeat = false;
        private bool mbLastSelfHeartBeat = false;
        private readonly Stopwatch mUpperHeartBeatTimeoutStopwatch = Stopwatch.StartNew();
        private readonly TimeSpan mUpperHeartBeatTimeout = TimeSpan.FromSeconds(5);
        private readonly Stopwatch mHeartBeatUpdateStopwatch = Stopwatch.StartNew();
        private readonly TimeSpan mHeartBeatPeriod = TimeSpan.FromMilliseconds(500);
        private readonly Utils.DigitalSignalObserver mUpperSignalObserver = new Utils.DigitalSignalObserver();
        public event EventHandler<ReceiveVacuumEventArgs> ReceiveActiveVacuumRequest;
        private int mPositionIndex = 0;
        private int _dryrunCycleCount = 0;

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
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.UpperBits.SendAble), mDefine.Name, () => mDefine.UpperBits.SendAble.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.UpperBits.SendStart), mDefine.Name, () => mDefine.UpperBits.SendStart.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.UpperBits.SendComplete), mDefine.Name, () => mDefine.UpperBits.SendComplete.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.UpperBits.SendVacuumOnP1), mDefine.Name, () => mDefine.UpperBits.SendVacuumOnP1.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.UpperBits.SendVacuumOnP2), mDefine.Name, () => mDefine.UpperBits.SendVacuumOnP2.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.UpperBits.SendCellP1), mDefine.Name, () => mDefine.UpperBits.SendCellP1.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.UpperBits.SendCellP2), mDefine.Name, () => mDefine.UpperBits.SendCellP2.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.UpperBits.EmsSafe), mDefine.Name, () => mDefine.UpperBits.EmsSafe.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.UpperBits.InterlockDoorClosed), mDefine.Name, () => mDefine.UpperBits.InterlockDoorClosed.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.UpperBits.InterlockSafe), mDefine.Name, () => mDefine.UpperBits.InterlockSafe.Value),

                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.ReceiveAble), mDefine.Name, () => mDefine.SelfBits.ReceiveAble.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.ReceiveStart), mDefine.Name, () => mDefine.SelfBits.ReceiveStart.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.ReceiveComplete), mDefine.Name, () => mDefine.SelfBits.ReceiveComplete.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.ReceiveVacuumOnP1), mDefine.Name, () => mDefine.SelfBits.ReceiveVacuumOnP1.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.ReceiveVacuumOnP2), mDefine.Name, () => mDefine.SelfBits.ReceiveVacuumOnP2.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.ReceiveCellP1), mDefine.Name, () => mDefine.SelfBits.ReceiveCellP1.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.ReceiveCellP2), mDefine.Name, () => mDefine.SelfBits.ReceiveCellP2.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.EmsSafe), mDefine.Name, () => mDefine.SelfBits.EmsSafe.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.InterlockDoorClosed), mDefine.Name, () => mDefine.SelfBits.InterlockDoorClosed.Value),
                new Utils.DigitalSignalObserverOption($"", nameof(mDefine.SelfBits.InterlockSafe), mDefine.Name, () => mDefine.SelfBits.InterlockSafe.Value),
            };
            mUpperSignalObserver.SignalChanged += UpperSignalObserver_SignalChanged;
            mUpperSignalObserver.Initialize(mObserverList);

            mbInitialized = true;
            return true;
        }

        public void DeInitialize()
        {
            mUpperSignalObserver.DeInitialize();
            mUpperSignalObserver.SignalChanged -= UpperSignalObserver_SignalChanged;

            mbThreadExit = true;
            mThreadProcess.Join();
            mbInitialized = false;
            Address.DeInitialize();
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
            mSignalControllers = new LoadInterfaceSignalController[] { mDefine };
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
            mSignalControllers = new LoadInterfaceSignalController[] { mDefine };
        }

        public void UpdateTickHeartbeatSignals()
        {
            // Update Self HeartBeat
            if (mHeartBeatPeriod < mHeartBeatUpdateStopwatch.Elapsed)
            {
                mbLastSelfHeartBeat = !mbLastSelfHeartBeat;
                mDefine.SelfBits.Heartbeat.Value = mbLastSelfHeartBeat;
                // 가상 모드 처리
                mDefine.UpperBits.Heartbeat.Value = mbLastSelfHeartBeat;
                mHeartBeatUpdateStopwatch.Restart();
            }

            // Check Upper HeartBeat
            if (mbLastUpperHeartBeat == mDefine.UpperBits.Heartbeat.Value)
            {
                if (mUpperHeartBeatTimeout < mUpperHeartBeatTimeoutStopwatch.Elapsed)
                {
                    mbUpperIsSurvival = false;
                }
            }
            else
            {
                mbUpperIsSurvival = true;
                mbLastUpperHeartBeat = mDefine.UpperBits.Heartbeat.Value;
                mUpperHeartBeatTimeoutStopwatch.Restart();
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

        public bool GetUpperInterlock(int position)
        {
            return mDefine.UpperBits.InterlockSafe.Value == false;
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
        }

        public EPosition GetOtherEquipmentSelectPosition()
        {
            // ! 미사용
            return EPosition.None;
        }

        public static string SIGNAL_DUMP_LOG_CSV_HEADER()
        {
            return string.Join(",",
                "DateTime",
                "Function",
                "Comment",

                "UpperSurvival",
                "UpperSendAble",
                "UpperSendStart",
                "UpperSendComplete",
                "UpperSendVacuumOn",
                "UpperSendCellP1",
                "UpperSendCellP2",
                "UpperEmsSafe",
                "UpperDoorClose",
                "UpperInterlockSafe",

                "SelfSendAble",
                "SelfSendStart",
                "SelfSendComplete",
                "SelfSendVacuumOn",
                "SelfSendCellP1",
                "SelfSendCellP2",
                "SelfEmsSafe",
                "SelfDoorClose",
                "SelfInterlockSafe"
                );
        }

        private void WriteLogSignalDump(string comment, [CallerMemberName] string callerMemberName = "")
        {
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_SIGNAL_INTERFACE_LOAD, string.Join(",",
                $"\t{DateTime.Now:yyyy-MM-dd HH:mm:ss:fff}",
                callerMemberName,
                comment,

                $"{Convert.ToInt32(mbUpperIsSurvival)}",
                $"{Convert.ToInt32(mDefine.UpperBits.SendAble.Value)}",
                $"{Convert.ToInt32(mDefine.UpperBits.SendStart.Value)}",
                $"{Convert.ToInt32(mDefine.UpperBits.SendComplete.Value)}",
                $"{Convert.ToInt32(mDefine.UpperBits.SendVacuumOnP1.Value)}",
                $"{Convert.ToInt32(mDefine.UpperBits.SendVacuumOnP2.Value)}",
                $"{Convert.ToInt32(mDefine.UpperBits.SendCellP1.Value)}",
                $"{Convert.ToInt32(mDefine.UpperBits.SendCellP2.Value)}",
                $"{Convert.ToInt32(mDefine.UpperBits.EmsSafe.Value)}",
                $"{Convert.ToInt32(mDefine.UpperBits.InterlockDoorClosed.Value)}",
                $"{Convert.ToInt32(mDefine.UpperBits.InterlockSafe.Value)}",

                $"{Convert.ToInt32(mDefine.SelfBits.ReceiveAble.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.ReceiveStart.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.ReceiveComplete.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.ReceiveVacuumOnP1.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.ReceiveVacuumOnP2.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.ReceiveCellP1.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.ReceiveCellP2.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.EmsSafe.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.InterlockDoorClosed.Value)}",
                $"{Convert.ToInt32(mDefine.SelfBits.InterlockSafe.Value)}"
                ));
        }

        private void UpperSignalObserver_SignalChanged(object sender, Utils.IDigitalSignalObserverItem e)
        {
            switch (e.Name)
            {
                case nameof(mDefine.UpperBits.SendAble):
                    break;

                case nameof(mDefine.UpperBits.SendStart):
                    break;

                case nameof(mDefine.UpperBits.SendComplete):
                    break;

                case nameof(mDefine.SelfBits.ReceiveAble):
                    break;

                case nameof(mDefine.SelfBits.ReceiveStart):
                    break;

                case nameof(mDefine.SelfBits.ReceiveComplete):
                    break;
            }

            switch (e.Name)
            {
                case nameof(mDefine.UpperBits.SendVacuumOnP1):
                    if (e.LastSignal)
                    {
                        // !! H/S 도중에 동작하는게 좋아보이나, Blow가 아니므로 그냥 처리함
                        RaiseReceiveActiveVacuumRequest(EVacuumCommand.Vacuum, EPosition.P1);
                    }
                    else
                    {
                        mDefine.SelfBits.ReceiveVacuumOnP1.Value = false;
                    }
                    break;
                case nameof(mDefine.UpperBits.SendVacuumOnP2):
                    if (e.LastSignal)
                    {
                        // !! H/S 도중에 동작하는게 좋아보이나, Blow가 아니므로 그냥 처리함
                        RaiseReceiveActiveVacuumRequest(EVacuumCommand.Vacuum, EPosition.P2);
                    }
                    else
                    {
                        mDefine.SelfBits.ReceiveVacuumOnP2.Value = false;
                    }
                    break;
            }

            string onOff = e.LastSignal ? "ON" : "OFF";
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_LOAD, $"{e.Comment}\t{e.Name}\t{onOff}");
        }

        private void RaiseReceiveActiveVacuumRequest(EVacuumCommand command, EPosition placement)
        {
            // !!! 긴 작업으로 비동기 처리
            // + 이벤트 핸들러에 연결된 Picker에 커맨드를 날리도록 연결함
            ReceiveVacuumEventArgs args = new ReceiveVacuumEventArgs(command, placement);
            ReceiveActiveVacuumRequest?.BeginInvoke(this, args, ReceiveActiveVacuumRequestEventFinished, args);
        }

        private void ReceiveActiveVacuumRequestEventFinished(IAsyncResult ar)
        {
            // + Picker에 커맨드 완료 후 Active설비에 응답
            ReceiveVacuumEventArgs args = (ReceiveVacuumEventArgs)ar.AsyncState;
            if (args.Placement.HasFlag(EPosition.P1) == true)
            {
                mDefine.SelfBits.ReceiveVacuumOnP1.Value = args.Command == EVacuumCommand.Vacuum;
            }
            if (args.Placement.HasFlag(EPosition.P2) == true)
            {
                mDefine.SelfBits.ReceiveVacuumOnP2.Value = args.Command == EVacuumCommand.Vacuum;
            }
        }

        private void DoProcessPreAction(int actionIndex = 0)
        {
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_LOAD, $"DoProcessPreAction({mDefine.Name},{actionIndex})");
            // Pre Action 없음
        }

        private void DoProcessAble()
        {
            WriteLogSignalDump("BEGIN");
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_LOAD, $"DoProcessAble({mDefine.Name})");

            // Able 신호 ON   
            mDefine.SelfBits.ReceiveAble.Value = true;

            if (mDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
            {
                Thread.Sleep(SIMULATION_DELAY);
                mDefine.UpperBits.SendCellP1.Value = true;
                mDefine.UpperBits.SendCellP2.Value = true;
                //switch (_dryrunCycleCount % 3)
                //{
                //    case 0:
                //        mDefine.UpperBits.SendCellP1.Value = true;
                //        mDefine.UpperBits.SendCellP2.Value = true;
                //        break;
                //    case 1:
                //        mDefine.UpperBits.SendCellP1.Value = true;
                //        mDefine.UpperBits.SendCellP2.Value = false;
                //        break;
                //    case 2:
                //        mDefine.UpperBits.SendCellP1.Value = false;
                //        mDefine.UpperBits.SendCellP2.Value = true;
                //        break;
                //}
                _dryrunCycleCount++;
                mDefine.UpperBits.SendAble.Value = true;
                Thread.Sleep(SIMULATION_DELAY);
            }

            WriteLogSignalDump("END");
        }

        private void DoProcessStart()
        {
            WriteLogSignalDump("BEGIN");
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_LOAD, $"DoProcessStart({mDefine.Name})");

            // 앞공정 Cell Position 정보 수신
            mDefine.SelfBits.ReceiveCellP1.Value = mDefine.UpperBits.SendCellP1.Value;
            mDefine.SelfBits.ReceiveCellP2.Value = mDefine.UpperBits.SendCellP2.Value;

            // Start 신호 ON
            mDefine.SelfBits.ReceiveStart.Value = true;

            if (mDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
            {
                Thread.Sleep(SIMULATION_DELAY);
                mDefine.UpperBits.SendStart.Value = true;
                Thread.Sleep(SIMULATION_DELAY);
                mDefine.UpperBits.SendVacuumOnP1.Value = mDefine.UpperBits.SendCellP1.Value;
                Thread.Sleep(SIMULATION_DELAY);
                mDefine.UpperBits.SendVacuumOnP2.Value = mDefine.UpperBits.SendCellP2.Value;
                Thread.Sleep(SIMULATION_DELAY);
                mDefine.UpperBits.SendComplete.Value = true;
            }

            WriteLogSignalDump("END");
        }

        private void DoProcessMidAction(int actionIndex = 0)
        {
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_LOAD, $"DoProcessMidAction({mDefine.Name},{actionIndex})");
            // Mid Action 없음
        }

        private void DoProcessComplete()
        {
            WriteLogSignalDump("BEGIN");
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_LOAD, $"DoProcessComplete({mDefine.Name})");

            // Complete 신호 ON       
            mDefine.SelfBits.ReceiveComplete.Value = true;

            var inShuttle = mDocument.m_objProcessMain.m_objProcessMotion.InShuttle;
            if (mDefine.SelfBits.ReceiveCellP1.Value == true)
            {
                inShuttle.CreateCellData(0);
            }
            if (mDefine.SelfBits.ReceiveCellP2.Value == true)
            {
                inShuttle.CreateCellData(1);
            }

            if (mDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
            {
                Thread.Sleep(SIMULATION_DELAY);
                mDefine.UpperBits.SendAble.Value = false;
                mDefine.UpperBits.SendStart.Value = false;
                mDefine.UpperBits.SendComplete.Value = false;
                mDefine.UpperBits.SendVacuumOnP1.Value = false;
                mDefine.UpperBits.SendVacuumOnP2.Value = false;
                mDefine.UpperBits.SendCellP1.Value = false;
                mDefine.UpperBits.SendCellP2.Value = false;
                Thread.Sleep(SIMULATION_DELAY);
            }

            WriteLogSignalDump("END");
        }

        private void DoProcessPostAction(int actionIndex = 0)
        {
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_LOAD, $"DoProcessPostAction({mDefine.Name},{actionIndex})");
            // Post Action 없음
        }

        private void DoProcessClearAllInterfaceSignals()
        {
            WriteLogSignalDump("BEGIN");
            mDocument.SetUpdateLog(CDefine.ELogType.LOG_PROCESS_INTERFACE_LOAD, $"DoProcessClearAllInterfaceSignals({mDefine.Name})");

            var inShuttle = mDocument.m_objProcessMain.m_objProcessMotion.InShuttle;
            if (mDefine.SelfBits.ReceiveCellP1.Value == true)
            {
                MccLogManager.InShuttle.COMPONENT_IN[0].WriteEnd();
            }
            if (mDefine.SelfBits.ReceiveCellP2.Value == true)
            {
                MccLogManager.InShuttle.COMPONENT_IN[1].WriteEnd();
            }

            // 모든 인터페이스 신호 OFF
            mDefine.SelfBits.ReceiveAble.Value = false;
            mDefine.SelfBits.ReceiveStart.Value = false;
            mDefine.SelfBits.ReceiveComplete.Value = false;
            mDefine.SelfBits.ReceiveVacuumOnP1.Value = false;
            mDefine.SelfBits.ReceiveVacuumOnP2.Value = false;
            mDefine.SelfBits.ReceiveCellP1.Value = false;
            mDefine.SelfBits.ReceiveCellP2.Value = false;

            if (mDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
            {
                Thread.Sleep(SIMULATION_DELAY);
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
                    mDefine.SelfBits.ReceiveComplete.Value == true
                    || mDefine.UpperBits.SendComplete.Value == true
                    );
                // Able 신호가 모두 켜진 상태에서 Upper의 Start 신호가 켜져 있으면 H/S 진행중임
                bool bHandshakeState = (
                    mDefine.SelfBits.ReceiveAble.Value == true
                    && mDefine.UpperBits.SendAble.Value == true
                    && mDefine.SelfBits.ReceiveStart.Value == true
                    //&& mDefine.UpperBits.SendStart.Value == true
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
                        // Manual Input 모드다
                        if (mDocument.IsManualInputMode == true
                            && mDocument.GetRunMode() == CDefine.ERunMode.RealRun
                            )
                        {
                            break;
                        }

                        // 인터페이스 진행 상태다
                        if (IsHandShaking == true)
                        {
                            break;
                        }

                        if (
                            mDefine.SelfBits.ReceiveAble.Value == true
                            && mDefine.SelfBits.ReceiveStart.Value == true
                            && mDefine.SelfBits.ReceiveComplete.Value == false
                            && mDefine.UpperBits.SendAble.Value == false
                            && mDefine.UpperBits.SendStart.Value == false
                            && mDefine.UpperBits.SendComplete.Value == false
                            )
                        {
                            mDefine.SelfBits.ReceiveStart.Value = false;
                            mDefine.SelfBits.ReceiveCellP1.Value = false;
                            mDefine.SelfBits.ReceiveCellP2.Value = false;
                            mDocument.m_objProcessMain.m_objProcessMotion.InShuttle.IsStartLoadInterface = false;
                        }

                        if (mDocument.GetRunStatus() != CDefine.ERunStatus.Start)
                        {
                            // Run 상태가 아닐 때는 Receive Able 신호 false로 변환
                            if (mDefine.SelfBits.ReceiveAble.Value == true
                                && mDefine.SelfBits.ReceiveStart.Value == false
                                )
                            {
                                mDefine.SelfBits.ReceiveAble.Value = false;
                            }
                            break;
                        }

                        // In Shuttle 셀 정보가 없다
                        var inShuttle = mDocument.m_objProcessMain.m_objProcessMotion.InShuttle;
                        if (inShuttle.CellContainer.IsCellExistFromList() == true)
                        {
                            break;
                        }

                        // 로드 위치가 아닐때
                        if (inShuttle.IsStageMotorXStatusAll(InShuttleMotorX.EStatus.LoadPosition) == false)
                        {
                            return false;
                        }

                        // Able 신호가 ON이다
                        if (mDefine.SelfBits.ReceiveAble.Value == true)
                        {
                            break;
                        }

                        bResult = true;
                    }
                    break;

                case EInterface.Start:
                    {
                        // 설비가 런상태가 아니다 (RUN이 아니면 진행하지 않음)
                        if (mDocument.GetRunStatus() != CDefine.ERunStatus.Start)
                        {
                            break;
                        }
                        // 인터페이스 진행 상태다
                        if (IsHandShaking == true)
                        {
                            break;
                        }

                        if (
                            // 상류 설비 인터페이스 가능 상태 확인
                            mbUpperIsSurvival == false
                            // 상, 하류 설비의 Able 신호가 둘다 OFF다
                            || mDefine.UpperBits.SendAble.Value == false
                            || mDefine.SelfBits.ReceiveAble.Value == false
                            // 상류 설비의 Start 신호가 ON이다
                            || mDefine.UpperBits.SendStart.Value == true
                            // Self Start 신호가 ON이다
                            || mDefine.SelfBits.ReceiveStart.Value == true
                            )
                        {
                            break;
                        }

                        bResult = true;
                    }
                    break;

                case EInterface.MidAction:
                    {
                        // !! 옵저버에서 신호를 감시하여 Event 처리
                        // Mid Action 없음
                    }
                    break;

                case EInterface.Complete:
                    {
                        if (
                            // 인터페이스 미진행 상태다
                            IsHandShaking == false
                            // 상류 Start 신호가 OFF다
                            || mDefine.UpperBits.SendStart.Value == false
                            // 상류 Complete 신호가 OFF이다
                            || mDefine.UpperBits.SendComplete.Value == false
                            // Self 설비 Start 신호가 OFF다
                            || mDefine.SelfBits.ReceiveStart.Value == false
                            // Self 설비 Complete 신호가 ON이다
                            || mDefine.SelfBits.ReceiveComplete.Value == true
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
                            // 인터페이스 미진행 상태다
                            IsHandShaking == false
                            // 상류 설비 Complete 신호가 ON이다
                            || mDefine.UpperBits.SendComplete.Value == true
                            // Self Complete 신호가 OFF다
                            || mDefine.SelfBits.ReceiveComplete.Value == false
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

            while (false == mbThreadExit)
            {
                Thread.Sleep(10);
                // Pre Action  (미사용)
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
