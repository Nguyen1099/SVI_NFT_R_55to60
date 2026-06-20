using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SVI_NFT_R
{
    public class CProcessTowerLamp
    {
        /// <summary>
        /// 타워램프 부저 상태
        /// </summary>
        public enum ETowerLamp
        {
            Fault,
            PM,
            OpCall,
            Interlock,
            UnitInterlock,
        }
        /// <summary>
        /// 타워램프 히스토리
        /// </summary>
        private readonly List<ETowerLamp> mTowerLampHistory = new List<ETowerLamp>();
        /// <summary>
        /// Document
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 램프 변경 쓰레드
        /// </summary>
        private Thread m_ThreadSetTowerLamp;
        /// <summary>
        /// 램프 상태 체크 쓰레드
        /// </summary>
        private Thread m_ThreadTowerLampState;
        /// <summary>
        /// 쓰레드 종료 Flag
        /// </summary>
        private bool m_bThreadExit;
        /// <summary>
        /// 램프 상태
        /// </summary>
        private CDefine.ELampSituation m_eLampSituation;
        private CDefine.ELampSituation m_eLampSituationPrev;
        private CDefine.ELampSituation m_eBuzzerSituation;
        private CDefine.ELampSituation m_eBuzzerSituationPrev;
        /// <summary>
        /// IO 객체 선언
        /// </summary>
        private HLDevice.CDeviceIO m_objIO;
        /// <summary>
        /// 램프 IO 이름
        /// </summary>
        private CDeviceIODefine.EDigitalOutput[] mTowerLampIO;
        private CDeviceIODefine.EDigitalOutput[] mBuzzerIO;
        /// <summary>
        /// 시간 체크
        /// </summary>
        private DateTime m_objCurrentTime;
        private TimeSpan m_objDurationTime;
        /// <summary>
        /// 부져 상태
        /// </summary>
        private CDefine.EBuzzerValue m_eBuzzerStatus;
        private bool m_bBlinkSignal = false;
        private CDefine.EBuzzerVoice m_eBuzzerVoiceStatus = CDefine.EBuzzerVoice.NONE;
        private Stopwatch mOperatorCheckPlayStopwatch = Stopwatch.StartNew();
        private readonly TimeSpan mOperatorCheckPlayTime = TimeSpan.FromSeconds(7);
        private Stopwatch mRunDelayCheckStopwatch = Stopwatch.StartNew();
        private readonly TimeSpan mRunDelayCheckTime = TimeSpan.FromMinutes(5);

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public bool Initialize(CDocument objDocument)
        {
            bool bResult = false;
            do
            {
                m_objDocument = objDocument;
                // IO 
                m_objIO = m_objDocument.m_objProcessMain.m_objIO;

                m_eLampSituation = CDefine.ELampSituation.LAMP_SITUATION_STAND_BY;
                m_eLampSituationPrev = CDefine.ELampSituation.LAMP_SITUATION_FINAL;
                m_eBuzzerSituation = CDefine.ELampSituation.LAMP_SITUATION_STAND_BY;
                m_eBuzzerSituationPrev = CDefine.ELampSituation.LAMP_SITUATION_FINAL;
                m_eBuzzerStatus = CDefine.EBuzzerValue.BUZZER_VALUE_OFF;
                m_eBuzzerVoiceStatus = CDefine.EBuzzerVoice.NONE;

                mTowerLampIO = new CDeviceIODefine.EDigitalOutput[(int)CDefine.ELampColor.LAMP_COLOR_FINAL];
                mTowerLampIO[(int)CDefine.ELampColor.LAMP_COLOR_RED] = CDeviceIODefine.EDigitalOutput.Y_TOWER_LAMP_RED;
                mTowerLampIO[(int)CDefine.ELampColor.LAMP_COLOR_YELLOW] = CDeviceIODefine.EDigitalOutput.Y_TOWER_LAMP_YELLOW;
                mTowerLampIO[(int)CDefine.ELampColor.LAMP_COLOR_GREEN] = CDeviceIODefine.EDigitalOutput.Y_TOWER_LAMP_GREEN;
                // HW에서 멜로디 버저를 지원하지 않음
                //mBuzzerIO = new CDeviceIODefine.EDigitalOutput[5];
                //mBuzzerIO[0] = CDeviceIODefine.EDigitalOutput.Y_MELODY_BUZZER_1;
                //mBuzzerIO[1] = CDeviceIODefine.EDigitalOutput.Y_MELODY_BUZZER_2;
                //mBuzzerIO[2] = CDeviceIODefine.EDigitalOutput.Y_MELODY_BUZZER_3;
                //mBuzzerIO[3] = CDeviceIODefine.EDigitalOutput.Y_MELODY_BUZZER_4;
                //mBuzzerIO[4] = CDeviceIODefine.EDigitalOutput.Y_MELODY_BUZZER_5;
                mBuzzerIO = new CDeviceIODefine.EDigitalOutput[1]
                {
                    CDeviceIODefine.EDigitalOutput.Y_TOWER_LAMP_BUZZER
                };

                m_objCurrentTime = new DateTime();
                m_objDurationTime = new TimeSpan();

                m_bThreadExit = false;
                m_ThreadSetTowerLamp = new Thread(ThreadSetTowerLamp);
                m_ThreadSetTowerLamp.IsBackground = true;
                m_ThreadSetTowerLamp.Start();

                m_ThreadTowerLampState = new Thread(ThreadTowerLampState);
                m_ThreadTowerLampState.IsBackground = true;
                m_ThreadTowerLampState.Start();

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            m_bThreadExit = true;
            m_ThreadTowerLampState.Join();
            m_ThreadSetTowerLamp.Join();
        }

        /// <summary>
        /// 마지막에 발생한 상황을 리스트 최상위로 올린다.
        /// </summary>
        /// <param name="item"></param>
        public void AddHistory(ETowerLamp item)
        {
            lock (mTowerLampHistory)
            {
                if (true == mTowerLampHistory.Contains(item))
                {
                    mTowerLampHistory.Remove(item);
                }
                mTowerLampHistory.Insert(0, item);
            }
        }

        /// <summary>
        /// 해당 아이템을 히스토리에서 클리어 한다.
        /// </summary>
        /// <param name="item"></param>
        public void ClearHistory(ETowerLamp item)
        {
            lock (mTowerLampHistory)
            {
                if (true == mTowerLampHistory.Contains(item))
                {
                    mTowerLampHistory.Remove(item);
                }
            }
        }

        /// <summary>
        /// 램프 상태 내보내기.
        /// </summary>
        /// <returns></returns>
        public CDefine.ELampSituation GetLampSituation()
        {
            return m_eLampSituation;
        }

        /// <summary>
        /// 램프 상태 변경
        /// </summary>
        /// <param name="eLampSituation"></param>
        public bool SetLampSituation(CDefine.ELampSituation eLampSituation, bool bUseBuzzerScenario = false)
        {
            bool result = false;
            if (true == m_objDocument.m_objConfig.GetSignalTowerParameter().bEnableScenario[(int)eLampSituation])
            {
                m_eLampSituation = eLampSituation;
                result = true;
            }
            if (bUseBuzzerScenario == true)
            {
                m_eBuzzerSituation = eLampSituation;
            }
            return result;
        }

        public void SetBuzzerVoiceStatus(CDefine.EBuzzerVoice eBuzzerVoice)
        {
            if (m_eBuzzerVoiceStatus != eBuzzerVoice)
            {
                m_eBuzzerVoiceStatus = eBuzzerVoice;
                mOperatorCheckPlayStopwatch.Reset();
                mOperatorCheckPlayStopwatch.Start();
            }
        }

        /// <summary>
        /// 부저 상태
        /// </summary>
        /// <returns></returns>
        public CDefine.EBuzzerValue GetBuzzerStatus()
        {
            return m_eBuzzerStatus;
        }

        /// <summary>
        /// 부저 정지
        /// </summary>
        public void SetBuzzerStop()
        {
            m_eBuzzerStatus = CDefine.EBuzzerValue.BUZZER_VALUE_OFF;
        }

        /// <summary>
        /// 부저 시작
        /// </summary>
        public void SetBuzzerStart()
        {
            m_eBuzzerStatus = CDefine.EBuzzerValue.BUZZER_VALUE_ON;
        }

        /// <summary>
        /// 타워 램프 동작
        /// </summary>                                    
        private void ThreadSetTowerLamp()
        {
            while (false == m_bThreadExit)
            {
                // 타워램프 상태가 변경이 되면...
                if (m_eLampSituation != m_eLampSituationPrev)
                {
                    // Blink 있는 타워 램프 상태들은 처음 한번 즉시 램프랑 Buzzer 상태를 변경해줘야 함.
                    // 램프와 Buzzer에 Blink 있는지 확인하자.
                    for (int iLoopTowerLampColor = 0; iLoopTowerLampColor < (int)CDefine.ELampColor.LAMP_COLOR_FINAL; iLoopTowerLampColor++)
                    {
                        if (CDefine.ELampValue.LAMP_VALUE_BLINK == m_objDocument.m_objConfig.GetSignalTowerParameter().eLampValue[(int)m_eLampSituation, iLoopTowerLampColor])
                        {
                            m_objIO.HLSetDigitalBit(mTowerLampIO[iLoopTowerLampColor], Convert.ToBoolean((int)CDefine.ELampValue.LAMP_VALUE_ON));
                        }
                    }

                    m_objCurrentTime = DateTime.Now;
                    m_eLampSituationPrev = m_eLampSituation;
                }
                if (m_eBuzzerSituation != m_eBuzzerSituationPrev)
                {
                    m_eBuzzerSituationPrev = m_eBuzzerSituation;
                    if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_objDocument.m_objConfig.GetSignalTowerParameter().eBuzzerValue[(int)m_eBuzzerSituation])
                    {
                        m_eBuzzerStatus = CDefine.EBuzzerValue.BUZZER_VALUE_ON;
                    }
                }

                // Blink 기능 구현
                m_objDurationTime = DateTime.Now - m_objCurrentTime;
                if (m_objDurationTime >= new TimeSpan(0, 0, 1))
                {
                    m_bBlinkSignal = !m_bBlinkSignal;
                    // Yellow Lamp
                    if (CDefine.ELampValue.LAMP_VALUE_BLINK == m_objDocument.m_objConfig.GetSignalTowerParameter().eLampValue[(int)m_eLampSituation, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW])
                    {
                        m_objIO.HLSetDigitalBit(mTowerLampIO[(int)CDefine.ELampColor.LAMP_COLOR_YELLOW], m_bBlinkSignal);
                    }

                    // Green Lamp
                    if (CDefine.ELampValue.LAMP_VALUE_BLINK == m_objDocument.m_objConfig.GetSignalTowerParameter().eLampValue[(int)m_eLampSituation, (int)CDefine.ELampColor.LAMP_COLOR_GREEN])
                    {
                        m_objIO.HLSetDigitalBit(mTowerLampIO[(int)CDefine.ELampColor.LAMP_COLOR_GREEN], m_bBlinkSignal);
                    }

                    // Red Lamp
                    if (CDefine.ELampValue.LAMP_VALUE_BLINK == m_objDocument.m_objConfig.GetSignalTowerParameter().eLampValue[(int)m_eLampSituation, (int)CDefine.ELampColor.LAMP_COLOR_RED])
                    {
                        m_objIO.HLSetDigitalBit(mTowerLampIO[(int)CDefine.ELampColor.LAMP_COLOR_RED], m_bBlinkSignal);
                    }

                    // Buzzer
                    if (true == m_objDocument.m_objConfig.GetOptionParameter().bUseBuzzer)
                    {
                        if (
                            CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_objDocument.m_objConfig.GetSignalTowerParameter().eBuzzerValue[(int)m_eBuzzerSituation]
                            && CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_eBuzzerStatus
                            )
                        {
                            SetBuzzerVoice(CDefine.EBuzzerVoice.BUZZER, m_bBlinkSignal);
                        }
                        else
                        {
                            SetBuzzerVoice(m_eBuzzerVoiceStatus, true);
                        }
                    }
                    else
                    {
                        SetBuzzerVoice(m_eBuzzerVoiceStatus, true);
                    }

                    m_objCurrentTime = DateTime.Now;
                }

                // Yellow Lamp
                if (CDefine.ELampValue.LAMP_VALUE_BLINK != m_objDocument.m_objConfig.GetSignalTowerParameter().eLampValue[(int)m_eLampSituation, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW])
                {
                    if (CDefine.ELampValue.LAMP_VALUE_ON == m_objDocument.m_objConfig.GetSignalTowerParameter().eLampValue[(int)m_eLampSituation, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW])
                    {
                        m_objIO.HLSetDigitalBit(mTowerLampIO[(int)CDefine.ELampColor.LAMP_COLOR_YELLOW], Convert.ToBoolean((int)CDefine.ELampValue.LAMP_VALUE_ON));
                    }
                    else
                    {
                        m_objIO.HLSetDigitalBit(mTowerLampIO[(int)CDefine.ELampColor.LAMP_COLOR_YELLOW], Convert.ToBoolean((int)CDefine.ELampValue.LAMP_VALUE_OFF));
                    }
                }
                // Green Lamp
                if (CDefine.ELampValue.LAMP_VALUE_BLINK != m_objDocument.m_objConfig.GetSignalTowerParameter().eLampValue[(int)m_eLampSituation, (int)CDefine.ELampColor.LAMP_COLOR_GREEN])
                {
                    if (CDefine.ELampValue.LAMP_VALUE_ON == m_objDocument.m_objConfig.GetSignalTowerParameter().eLampValue[(int)m_eLampSituation, (int)CDefine.ELampColor.LAMP_COLOR_GREEN])
                    {
                        m_objIO.HLSetDigitalBit(mTowerLampIO[(int)CDefine.ELampColor.LAMP_COLOR_GREEN], Convert.ToBoolean((int)CDefine.ELampValue.LAMP_VALUE_ON));
                    }
                    else
                    {
                        m_objIO.HLSetDigitalBit(mTowerLampIO[(int)CDefine.ELampColor.LAMP_COLOR_GREEN], Convert.ToBoolean((int)CDefine.ELampValue.LAMP_VALUE_OFF));
                    }
                }
                // Red Lamp
                if (CDefine.ELampValue.LAMP_VALUE_BLINK != m_objDocument.m_objConfig.GetSignalTowerParameter().eLampValue[(int)m_eLampSituation, (int)CDefine.ELampColor.LAMP_COLOR_RED])
                {
                    if (CDefine.ELampValue.LAMP_VALUE_ON == m_objDocument.m_objConfig.GetSignalTowerParameter().eLampValue[(int)m_eLampSituation, (int)CDefine.ELampColor.LAMP_COLOR_RED])
                    {
                        m_objIO.HLSetDigitalBit(mTowerLampIO[(int)CDefine.ELampColor.LAMP_COLOR_RED], Convert.ToBoolean((int)CDefine.ELampValue.LAMP_VALUE_ON));
                    }
                    else
                    {
                        m_objIO.HLSetDigitalBit(mTowerLampIO[(int)CDefine.ELampColor.LAMP_COLOR_RED], Convert.ToBoolean((int)CDefine.ELampValue.LAMP_VALUE_OFF));
                    }
                }
                Thread.Sleep(100);
            }
        }

        private void SetBuzzerVoice(CDefine.EBuzzerVoice eBuzzerType, bool bOnOff)
        {
            if (
                true == m_objDocument.IsMasterLogin
                && CDefine.EBuzzerVoice.BUZZER != eBuzzerType
                )
            {
                bOnOff = false;
            }

            switch (eBuzzerType)
            {
                case CDefine.EBuzzerVoice.NONE:
                    if (mBuzzerIO.Length < 5)
                    {
                        break;
                    }
                    m_objIO.HLSetDigitalBit(mBuzzerIO[0], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[1], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[2], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[3], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[4], false);
                    break;
                case CDefine.EBuzzerVoice.DOOR_UNLOCK:
                    if (mBuzzerIO.Length < 5)
                    {
                        break;
                    }
                    m_objIO.HLSetDigitalBit(mBuzzerIO[0], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[1], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[2], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[3], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[4], bOnOff);
                    break;
                case CDefine.EBuzzerVoice.KEY_NO_OWN:
                    if (mBuzzerIO.Length < 5)
                    {
                        break;
                    }
                    m_objIO.HLSetDigitalBit(mBuzzerIO[0], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[1], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[2], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[3], bOnOff);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[4], false);
                    break;
                case CDefine.EBuzzerVoice.DOOR_LOCK:
                    if (mBuzzerIO.Length < 5)
                    {
                        break;
                    }
                    m_objIO.HLSetDigitalBit(mBuzzerIO[0], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[1], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[2], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[3], bOnOff);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[4], bOnOff);
                    break;
                case CDefine.EBuzzerVoice.OPERATOR_CHECK:
                    if (mBuzzerIO.Length < 5)
                    {
                        break;
                    }
                    m_objIO.HLSetDigitalBit(mBuzzerIO[0], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[1], false);
                    if (mOperatorCheckPlayTime > mOperatorCheckPlayStopwatch.Elapsed)
                    {
                        m_objIO.HLSetDigitalBit(mBuzzerIO[2], bOnOff);
                    }
                    else
                    {
                        m_objIO.HLSetDigitalBit(mBuzzerIO[2], false);
                        mOperatorCheckPlayStopwatch.Stop();
                    }
                    m_objIO.HLSetDigitalBit(mBuzzerIO[3], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[4], false);
                    break;
                case CDefine.EBuzzerVoice.BUZZER:
                    if (mBuzzerIO.Length < 5)
                    {
                        m_objIO.HLSetDigitalBit(mBuzzerIO[0], bOnOff);
                        break;
                    }
                    m_objIO.HLSetDigitalBit(mBuzzerIO[0], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[1], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[2], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[3], false);
                    m_objIO.HLSetDigitalBit(mBuzzerIO[4], bOnOff);
                    break;
            }
        }

        /// <summary>
        /// 타워 램프 동작 스레드
        /// </summary>
        private void ThreadTowerLampState()
        {
            while (false == m_bThreadExit)
            {
                var history = mTowerLampHistory.ToArray();
                bool bCheckNormalSituation = false;

                // 공유메모리에서 프로그램 종료 상태 쓰기.
                ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus objMachineStatus = new ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus();
                var objMMFManagerMachineStatus = ENC.MemoryMap.Manager.CMMFManagerMachineStatus.Instance;
                objMMFManagerMachineStatus[0].GetMachineStatus(out objMachineStatus);
                // 프로그램 종료시
                if (CDefine.EProgramExitStatus.PROGRAM_EXIT_STATUS_ON == objMachineStatus.m_eProgramExitStatus)
                {
                    SetLampSituation(CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN, true);
                }
                else if (0 < history.Length)
                {
                    // Abnormal State (가장 최근에 발생한 상황을 타워램프에 표시한다.)
                    switch (history[0])
                    {
                        case ETowerLamp.Fault:
                            if (true == m_objDocument.GetIsHeavyAlarm())
                            {
                                bCheckNormalSituation = !SetLampSituation(CDefine.ELampSituation.LAMP_SITUATION_FAULT, true);
                            }
                            else
                            {
                                bCheckNormalSituation = !SetLampSituation(CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM, true);
                            }
                            break;
                        case ETowerLamp.PM:
                            bCheckNormalSituation = !SetLampSituation(CDefine.ELampSituation.LAMP_SITUATION_PM, true);
                            break;
                        case ETowerLamp.OpCall:
                            bCheckNormalSituation = !SetLampSituation(CDefine.ELampSituation.LAMP_SITUATION_OP_CALL, true);
                            break;
                        case ETowerLamp.Interlock:
                        case ETowerLamp.UnitInterlock:
                            bCheckNormalSituation = !SetLampSituation(CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK, true);
                            break;
                        default:
                            Debug.Assert(false, "정의되지 않은 타워램프 상태");
                            break;
                    }
                }
                else
                {
                    bCheckNormalSituation = true;
                }

                if (true == bCheckNormalSituation)
                {
                    // 설비 상태가 Run 일 경우
                    if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                    {
                        if (true == m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip.IsUnloadPending)
                        {
                            mRunDelayCheckStopwatch.Start();
                        }
                        else
                        {
                            mRunDelayCheckStopwatch.Stop();
                            mRunDelayCheckStopwatch.Reset();
                        }

                        // 설비내 자재가 없을 경우
                        if (CCIMDefine.ERunState.RUN_STATE_IDLE == m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                        {
                            SetLampSituation(CDefine.ELampSituation.LAMP_SITUATION_IDLE);
                        }
                        // 설비내 자재가 있을 경우
                        else
                        {
                            if (
                                mRunDelayCheckStopwatch.Elapsed > mRunDelayCheckTime
                                && true == m_objDocument.m_objConfig.GetSignalTowerParameter().bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY]
                                )
                            {
                                SetLampSituation(CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY);
                            }
                            else
                            {
                                SetLampSituation(CDefine.ELampSituation.LAMP_SITUATION_RUNNING);
                            }
                        }
                    }
                    // 설비 상태가 Stop 일 경우
                    else if (CCIMDefine.EMoveState.MOVE_STATE_PAUSE == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                    {
                        mRunDelayCheckStopwatch.Stop();
                        mRunDelayCheckStopwatch.Reset();
                        if (
                            CDefine.ERunStatus.Initialize == m_objDocument.GetRunStatus()
                            && true == m_objDocument.m_objConfig.GetSignalTowerParameter().bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY]
                            )
                        {
                            SetLampSituation(CDefine.ELampSituation.LAMP_SITUATION_STAND_BY);
                        }
                        else
                        {
                            SetLampSituation(CDefine.ELampSituation.LAMP_SITUATION_STOP);
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}
