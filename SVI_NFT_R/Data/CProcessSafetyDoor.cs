using System;
using System.Linq;
using System.Threading;

namespace SVI_NFT_R
{
    public class CProcessSafetyDoor
    {
        public bool IsForceDoorBuzzerOffRequest { get; set; } = false;
        public readonly int DoorCount = Enum.GetNames(typeof(Door.EDoor)).Length;
        private readonly CDefine.EMachineFrontRear[] mAllInsideKeys = Enum.GetValues(typeof(CDefine.EMachineFrontRear)).Cast<CDefine.EMachineFrontRear>().ToArray();
        /// <summary>
        /// Document
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 스레드 종료 플래그
        /// </summary>
        private bool m_bThreadExit;
        /// <summary>
        /// 세이프티 키 스레드
        /// </summary>
        private Thread m_ThreadSafetyKey;
        /// <summary>
        /// 도어 키 상태 스레드
        /// </summary>
        private Thread m_ThreadDoorKeyState;
        /// <summary>
        /// 세이프티 IO 클래스
        /// </summary>
        private CSafetyIO m_objSafetyIO;

        public bool m_bDoorDialogClose = true;
        /// <summary>
        /// 세이프티 도어 시나리오
        /// </summary>
        public enum ESafetyDoorScenario
        {
            DOOR_LOCK = 0,
            DOOR_UNLOCK,
            KEY_NO_OWN,
            INNER_OPERATOR_CHECK
        };
        private ESafetyDoorScenario[] m_eSafetyDoorScenario;
        private readonly long KEY_NO_OWN_VOICE_OFF_DELAY = TimeSpan.FromSeconds(2).Ticks;
        private long mKeyNoWonVoiceOffDelayStartTime = Utils.GetTimestamp();

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public bool Initialize(CDocument objDocument)
        {
            bool bReturn = false;

            do
            {
                // 도큐먼트 이어줌
                m_objDocument = objDocument;
                // 세이프티 IO 클래스 초기화
                m_objSafetyIO = new CSafetyIO(m_objDocument);
                if (false == m_objSafetyIO.Initialize())
                    break;

                // 세이프티 도어 시나리오 초기화
                m_eSafetyDoorScenario = new ESafetyDoorScenario[DoorCount];

                // 세이프티 키 스레드
                m_bThreadExit = false;
                m_ThreadSafetyKey = new Thread(ThreadSafetyKey);
                m_ThreadSafetyKey.IsBackground = true;
                m_ThreadSafetyKey.Start(this);

                // 도어 키 상태 스레드
                m_ThreadDoorKeyState = new Thread(ThreadDoorKeyState);
                m_ThreadDoorKeyState.IsBackground = true;
                m_ThreadDoorKeyState.Start();

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            // 세이프티 IO 클래스 해제
            m_objSafetyIO.Deinitialize();
            m_bThreadExit = true;
            if (m_ThreadSafetyKey != null)
                m_ThreadSafetyKey.Join();
            if (m_ThreadDoorKeyState != null)
                m_ThreadDoorKeyState.Join();
        }

        /// <summary>
        /// 세이프티 도어 시나리어 상태 받음
        /// </summary>
        /// <param name="eFrontRear"></param>
        /// <returns></returns>
        public ESafetyDoorScenario GetSafetyDoorScenario(Door.EDoor eFrontRear/*, CDefine.enumDoorIndex eDoorIndex*/)
        {
            return m_eSafetyDoorScenario[(int)eFrontRear];
        }

        /// <summary>
        /// 도어 상태 초기화
        /// </summary>
        /// <param name="eFrontRear"></param>
        /// <param name="eSafetyDoorScenario"></param>
        public void SetSafetyDoorScenario(CDefine.EMachineFrontRear eFrontRear/*, CDefine.enumDoorIndex eDoorIndex*/, ESafetyDoorScenario eSafetyDoorScenario)
        {
            m_eSafetyDoorScenario[(int)eFrontRear] = eSafetyDoorScenario;
        }

        /// <summary>
        /// 세이프티 키 조건을 파악해서 잠금, 해제
        /// </summary>
        /// <param name="state"></param>
        private void ThreadSafetyKey(object state)
        {
            int changeDelayTick = 0;
            while (false == m_bThreadExit)
            {
                bool bAllUnlockPermitOff = DoorManager.Doors.Values.All(item => item.IsUnlockPermit == false);
                bool bAllDoorLocked = DoorManager.Doors.Values.All(item => item.IsLocked == true);
                bool bAllDoorClosed = DoorManager.Doors.Values.All(item => item.IsOpened == false);
                bool bAnyHasInsideKey = DoorManager.Doors.Values.Any(item => item.HasInsideKey == true);
                bool bCheckDoorClosedFromSafetyPLC = m_objSafetyIO.IsSafetyAllDoorClosed();

                // 세이프티 모드 키 언락 조건
                // + 모든 도어가 언락 요청 OFF 상태여야 함
                // + 모든 내부 보관함 키 감지 센서가 OFF 상태여야 함
                // + 모든 도어가 잠겨야 함
                // + 모든 다인수 스위치가 미감지 상태여야 함
                // + 세이프티 PLC에서 모든 문 닫힘 확인 함 -> Lami 후 외관검사기(E2104-SI501-096) 프로젝트부터 추가된 신호
                if (
                    bAllUnlockPermitOff == true
                    && bAllDoorClosed == true
                    && bAllDoorLocked == true
                    && bAnyHasInsideKey == false
                    && bCheckDoorClosedFromSafetyPLC == true
                    )
                {
                    // 세이프티 키 언락 ON 아니면
                    if (
                        changeDelayTick > 50
                        && mAllInsideKeys.Any(item => m_objSafetyIO.GetOutputSafetyKeyUnlock(item) != CDefine.ESafetyKeyUnlock.SAFETY_KEY_UNLOCK_ON) == true
                        )
                    {
                        // 세이프티 키 언락 ON
                        Array.ForEach(mAllInsideKeys, item => m_objSafetyIO.SetOutputSafetyKeyUnlock(item, CDefine.ESafetyKeyUnlock.SAFETY_KEY_UNLOCK_ON));
                    }
                    changeDelayTick++;
                }
                // 그 외 상태에서는 세이프티 키 언락 OFF
                else
                {
                    // 세이프티 키 언락 OFF 아니면
                    if (mAllInsideKeys.Any(item => m_objSafetyIO.GetOutputSafetyKeyUnlock(item) != CDefine.ESafetyKeyUnlock.SAFETY_KEY_UNLOCK_OFF) == true)
                    {
                        // 세이프티 키 언락 OFF
                        Array.ForEach(mAllInsideKeys, item => m_objSafetyIO.SetOutputSafetyKeyUnlock(item, CDefine.ESafetyKeyUnlock.SAFETY_KEY_UNLOCK_OFF));
                    }
                    changeDelayTick = 0;
                }
                // 소프트웨이어에서 도어 오픈을 허가할경우 외부 열쇠락이 해제됨
                foreach (var group in DoorManager.Groups)
                {
                    if (group.Value[0].IsUnlockPermit == true)
                    {
                        foreach (var door in group.Value)
                        {
                            door.DoorOutsideKeyLockSol = false;
                        }
                    }
                    else
                    {
                        foreach (var door in group.Value)
                        {
                            door.DoorOutsideKeyLockSol = true;
                        }
                    }
                }
                // 소프트웨이어에서 도어 오픈을 허가하고 외부 열쇠가 빠지면 해당하는 그룹에 도어락이 해제됨
                foreach (var group in DoorManager.Groups)
                {
                    if (group.Value[0].CanUnlock == true)
                    {
                        foreach (var door in group.Value)
                        {
                            door.DoorLockSol = false;
                        }
                    }
                    else
                    {
                        foreach (var door in group.Value)
                        {
                            door.DoorLockSol = true;
                        }
                    }
                }
                //// 도어 오픈 부저 ON/OFF (마더라인 미사용)
                //bool bFrontDoorOpenBuzzerOn = (
                //    m_objDocument.m_objConfig.GetOptionParameter().bUseBuzzer == true
                //    && m_objDocument.IsMasterLogin == false
                //    && IsForceDoorBuzzerOffRequest == false
                //    && (
                //        DoorManager.Groups[Door.EGroup.SlideFront].Any(item => item.IsOpened == true)
                //    )
                //    && bAnyHasInsideKey == false
                //    );
                //bool bRearDoorOpenBuzzerOn = (
                //    m_objDocument.m_objConfig.GetOptionParameter().bUseBuzzer == true
                //    && m_objDocument.IsMasterLogin == false
                //    && IsForceDoorBuzzerOffRequest == false
                //    && (
                //        DoorManager.Groups[Door.EGroup.SlideRear].Any(item => item.IsOpened == true)
                //    )
                //    && bAnyHasInsideKey == false
                //    );
                //m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_FRONT_DOOR_OPEN_BUZZER, bFrontDoorOpenBuzzerOn);
                //m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_REAR_DOOR_OPEN_BUZZER, bRearDoorOpenBuzzerOn);
                if (bAllDoorClosed == true)
                {
                    IsForceDoorBuzzerOffRequest = false;
                }
                // 다인수 키 거치대 언락 상태 유지 (딱히 잠글 이유가 없음)
                //m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_FRONT_MAINT_KEY_CHECKER_KEY_UNLOCK, true);
                //m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_REAR_MAINT_KEY_CHECKER_KEY_UNLOCK, true);
                Thread.Sleep(10);
                changeDelayTick++;
            }
        }

        /// <summary>
        /// 도어 키 상태를 파악해서 내부 작업자 알람 유무를 변경해줌
        /// </summary>                                  
        private void ThreadDoorKeyState()
        {
            while (false == m_bThreadExit)
            {
                // 각각 도어의 현재 상태를 갱신한다
                bool bAnyInsideKeyPlug = GetInsideKeyPlug();
                foreach (var item in DoorManager.Doors.Values)
                {
                    if (true == IsDoorKeyScenario(ESafetyDoorScenario.INNER_OPERATOR_CHECK, item.DoorIndex, bAnyInsideKeyPlug))
                    {
                        m_eSafetyDoorScenario[(int)item.DoorIndex] = ESafetyDoorScenario.INNER_OPERATOR_CHECK;
                    }
                    else if (true == IsDoorKeyScenario(ESafetyDoorScenario.KEY_NO_OWN, item.DoorIndex, bAnyInsideKeyPlug))
                    {
                        m_eSafetyDoorScenario[(int)item.DoorIndex] = ESafetyDoorScenario.KEY_NO_OWN;
                    }
                    else if (true == IsDoorKeyScenario(ESafetyDoorScenario.DOOR_LOCK, item.DoorIndex, bAnyInsideKeyPlug))
                    {
                        m_eSafetyDoorScenario[(int)item.DoorIndex] = ESafetyDoorScenario.DOOR_LOCK;
                    }
                    else if (true == IsDoorKeyScenario(ESafetyDoorScenario.DOOR_UNLOCK, item.DoorIndex, bAnyInsideKeyPlug))
                    {
                        m_eSafetyDoorScenario[(int)item.DoorIndex] = ESafetyDoorScenario.DOOR_UNLOCK;
                    }
                    else
                    {
                        m_eSafetyDoorScenario[(int)item.DoorIndex] = ESafetyDoorScenario.DOOR_UNLOCK;
                    }
                }

                // 도어 상태를 조합하여 재생 목소리를 변경한다
                {
                    if (m_eSafetyDoorScenario.Any(item => item == ESafetyDoorScenario.INNER_OPERATOR_CHECK) == true)
                    {
                        // 2024-01-16 환경안전 Qual 변경: 도어부저 다인수 키꽃으면 음성 한 사이클 처리 (임성주P)
                        if (Utils.GetTimestamp() - mKeyNoWonVoiceOffDelayStartTime > KEY_NO_OWN_VOICE_OFF_DELAY)
                        {
                            // 내부에 작업자가 있는지 확인하세요 (1회 재생, 1회 재생기능이 따로 없어서 3.5초 후 꺼짐) (미사용)
                            //m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerVoiceStatus(CDefine.EBuzzerVoice.OPERATOR_CHECK);
                            m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerVoiceStatus(CDefine.EBuzzerVoice.NONE);
                        }
                    }
                    else if (m_eSafetyDoorScenario.Any(item => item == ESafetyDoorScenario.KEY_NO_OWN) == true)
                    {
                        // 2023-05-24 환경안전 Qual 변경
                        // 내부 Maint Key를 꽂아주세요
                        // !!! 구형HW 베이스 개조건으로 멜로디 버저HW가 없어 바로 NONE 처리
                        m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerVoiceStatus(CDefine.EBuzzerVoice.NONE); //m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerVoiceStatus(CDefine.EBuzzerVoice.KEY_NO_OWN);

                        // 열쇠를 소지하여 주십시오
                        //m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerVoiceStatus(CDefine.EBuzzerVoice.KEY_NO_OWN);

                        // 2024-01-16 환경안전 Qual 변경: 도어부저 다인수 키꽃으면 음성 한 사이클 처리 (임성주P)
                        mKeyNoWonVoiceOffDelayStartTime = Utils.GetTimestamp();
                    }
                    else if (m_eSafetyDoorScenario.All(item => item == ESafetyDoorScenario.DOOR_UNLOCK) == true)
                    {
                        // 잠금이 해제되었습니다 (미사용)
                        //m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerVoiceStatus(CDefine.EBuzzerVoice.DOOR_UNLOCK);
                        m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerVoiceStatus(CDefine.EBuzzerVoice.NONE);
                    }
                    else if (m_eSafetyDoorScenario.All(item => item == ESafetyDoorScenario.DOOR_LOCK) == true)
                    {
                        // 잠금이 설정되었습니다 (미사용)
                        //m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerVoiceStatus(CDefine.EBuzzerVoice.DOOR_LOCK);
                        m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerVoiceStatus(CDefine.EBuzzerVoice.NONE);
                    }
                    else
                    {
                        m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerVoiceStatus(CDefine.EBuzzerVoice.NONE);
                    }
                }

                Thread.Sleep(10);
            }
        }

        private bool IsDoorKeyScenario(ESafetyDoorScenario doorScenario, Door.EDoor doorIndex, bool bAnyInsideKeyPlug)
        {
            var selectDoor = DoorManager.Doors[doorIndex];
            if (selectDoor.DoorType == Door.EType.Key)
            {
                // !!! 구형HW 베이스 개조건 기준으로 변경
                if (ESafetyDoorScenario.DOOR_LOCK == doorScenario)
                {
                    if (selectDoor.IsLocked == false) return false;
                    if (selectDoor.IsOpened == true) return false;
                }
                else if (ESafetyDoorScenario.DOOR_UNLOCK == doorScenario)
                {
                    if (selectDoor.IsLocked == true) return false;
                    if (selectDoor.IsOpened == true) return false;
                }
                else if (ESafetyDoorScenario.KEY_NO_OWN == doorScenario)
                {
                    if (selectDoor.IsOpened == false) return false;
                }
                else
                {
                    return false;
                }
            }
            else if (selectDoor.DoorType == Door.EType.Slide)
            {
                if (ESafetyDoorScenario.DOOR_LOCK == doorScenario)
                {
                    if (selectDoor.IsLocked == false) return false;
                }
                else if (ESafetyDoorScenario.KEY_NO_OWN == doorScenario)
                {
                    if (selectDoor.IsUnlockPermit == false) return false;
                    if (selectDoor.IsLocked == true) return false;
                    if (bAnyInsideKeyPlug == true) return false;
                }
                else if (ESafetyDoorScenario.INNER_OPERATOR_CHECK == doorScenario)
                {
                    if (selectDoor.IsUnlockPermit == false) return false;
                    if (selectDoor.IsLocked == true) return false;
                    if (bAnyInsideKeyPlug == false) return false;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private bool GetInsideKeyPlug() => DoorManager.Doors.Values.Any(item => item.HasInsideKey == true);
    }
}