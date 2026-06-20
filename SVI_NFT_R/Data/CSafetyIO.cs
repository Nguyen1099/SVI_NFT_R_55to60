using System.Collections.Generic;
using System.Linq;

namespace SVI_NFT_R
{
    public class CSafetyIO
    {
        public IReadOnlyList<CDeviceIODefine.EDigitalInput> HumanDetectSensers => mHumanDetectSensers;
        public IReadOnlyList<CDeviceIODefine.EDigitalInput> EnableSwitchGrips => mEnableSwitchGrips;
        public IReadOnlyList<CDeviceIODefine.EDigitalInput> EnableSwitchEmergencys => mEnableSwitchEmergencys;
        public IReadOnlyList<CDeviceIODefine.EDigitalInput> MaintKeyCheckers => mMaintKeyCheckers;
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 세이프티 모드 키 스위치 INPUT
        /// </summary>
        private Dictionary<CDefine.EMachineFrontRear, CDeviceIODefine.EDigitalInput> m_dicInputSafetyModeKeySwitch;
        /// <summary>
        /// 세이프티 모드 키 언락 OUTPUT
        /// </summary>
        private Dictionary<CDefine.EMachineFrontRear, CDeviceIODefine.EDigitalOutput> m_dicOutputSafetyModeKeyUnlock;
        private readonly List<CDeviceIODefine.EDigitalInput> mHumanDetectSensers = new List<CDeviceIODefine.EDigitalInput>();
        private readonly List<CDeviceIODefine.EDigitalInput> mEnableSwitchGrips = new List<CDeviceIODefine.EDigitalInput>();
        private readonly List<CDeviceIODefine.EDigitalInput> mEnableSwitchEmergencys = new List<CDeviceIODefine.EDigitalInput>();
        private readonly List<CDeviceIODefine.EDigitalInput> mMaintKeyCheckers = new List<CDeviceIODefine.EDigitalInput>();

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CSafetyIO(CDocument objDocument)
        {
            m_objDocument = objDocument;
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool bReturn = false;

            do
            {
                // IO 매칭
                // 세이프티 키 AUTO, TEACH 센서
                // 세이프티 키가 AUTO이면 OFF, TEACH이면 ON
                // ! 초기 Gamma 프로젝트 설비를 베이스로 개조해서 Mode 키 상태 감지 신호가 없어서 세이프티 컨트롤러 티치 모드 상태 신호로 대체함.
                m_dicInputSafetyModeKeySwitch = new Dictionary<CDefine.EMachineFrontRear, CDeviceIODefine.EDigitalInput>();
                m_dicInputSafetyModeKeySwitch.Add(CDefine.EMachineFrontRear.MACHINE_FRONT, CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH);
                m_dicInputSafetyModeKeySwitch.Add(CDefine.EMachineFrontRear.MACHINE_REAR, CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH);

                // 세이프티 키 열림, 잠금
                // ON이면 세이프티 키 잠금 해제
                m_dicOutputSafetyModeKeyUnlock = new Dictionary<CDefine.EMachineFrontRear, CDeviceIODefine.EDigitalOutput>();
                m_dicOutputSafetyModeKeyUnlock.Add(CDefine.EMachineFrontRear.MACHINE_FRONT, CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_MODE_KEY_UNLOCK);
                m_dicOutputSafetyModeKeyUnlock.Add(CDefine.EMachineFrontRear.MACHINE_REAR, CDeviceIODefine.EDigitalOutput.Y_REAR_SAFETY_MODE_KEY_UNLOCK);

                // 인체 감지 센서
                {
                    //mHumanDetectSensers.Add(CDeviceIODefine.EDigitalInput.X_HUMAN_DETECT_SENSOR1);
                }

                // 인에이블 스위치
                {
                    //mEnableSwitchGrips.Add(CDeviceIODefine.EDigitalInput.X_ENABLE_SWITCH_1_GRIP);
                    //mEnableSwitchEmergencys.Add(CDeviceIODefine.EDigitalInput.X_ENABLE_SWITCH_1_EMERGENCY);
                }

                // 유지보수 키 체크
                {
                    //mMaintKeyCheckers.Add(CDeviceIODefine.EDigitalInput.X_FRONT_MAINT_KEY_CHECKER_DETECT);
                }

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void Deinitialize()
        {
        }

        /// <summary>
        /// IO 비트 확인
        /// </summary>
        /// <param name="strIOName"></param>
        /// <returns></returns>
        private bool GetIO(CDeviceIODefine.EDigitalOutput strIOName)
        {
            bool bReturn = false;
            m_objDocument.m_objProcessMain.m_objIO.HLGetDigitalBit(strIOName, ref bReturn);
            return bReturn;
        }

        /// <summary>
        /// IO 비트 확인
        /// </summary>
        /// <param name="strIOName"></param>
        /// <returns></returns>
        private bool GetIO(CDeviceIODefine.EDigitalInput strIOName)
        {
            bool bReturn = false;
            m_objDocument.m_objProcessMain.m_objIO.HLGetDigitalBit(strIOName, ref bReturn);
            return bReturn;
        }

        /// <summary>
        /// IO 비트 설정
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bIO"></param>
        private void SetIO(CDeviceIODefine.EDigitalOutput strIOName, bool bIO)
        {
            m_objDocument.m_objProcessMain.m_objIO.HLSetDigitalBit(strIOName, bIO);
        }

        /// <summary>
        /// IO 비트 설정
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bIO"></param>
        private void SetIO(CDeviceIODefine.EDigitalInput strIOName, bool bIO)
        {
            m_objDocument.m_objProcessMain.m_objIO.HLSetDigitalBit(strIOName, bIO);
        }

        /// <summary>
        /// 세이프티 키 상태 리턴
        /// </summary>
        /// <param name="eFrontRear"></param>
        /// <returns></returns>
        public CDefine.ESafetyKey GetInputSafetyKey(CDefine.EMachineFrontRear eFrontRear)
        {
            CDefine.ESafetyKey eKey = CDefine.ESafetyKey.SAFETY_KEY_UNKNOWN;

            if (false == GetIO(m_dicInputSafetyModeKeySwitch[eFrontRear]))
            {
                eKey = CDefine.ESafetyKey.SAFETY_KEY_AUTO;
            }
            else
            {
                eKey = CDefine.ESafetyKey.SAFETY_KEY_TEACH;
            }

            return eKey;
        }

        /// <summary>
        /// 세이프티 키 상태 설정
        /// </summary>
        /// <param name="eFrontRear"></param>
        /// <param name="eKey"></param>
        public void SetInputSafetyKey(CDefine.EMachineFrontRear eFrontRear, CDefine.ESafetyKey eKey)
        {
            if (CDefine.ESafetyKey.SAFETY_KEY_AUTO == eKey)
            {
                SetIO(m_dicInputSafetyModeKeySwitch[eFrontRear], CDefine.OFF);
            }
            else if (CDefine.ESafetyKey.SAFETY_KEY_TEACH == eKey)
            {
                SetIO(m_dicInputSafetyModeKeySwitch[eFrontRear], CDefine.ON);
            }
        }

        /// <summary>
        /// 세이프티 키 잠그는 상태 리턴
        /// </summary>
        /// <param name="eFrontRear"></param>
        /// <returns></returns>
        public CDefine.ESafetyKeyUnlock GetOutputSafetyKeyUnlock(CDefine.EMachineFrontRear eFrontRear)
        {
            CDefine.ESafetyKeyUnlock eUnlock = CDefine.ESafetyKeyUnlock.SAFETY_KEY_UNLOCK_UNKNOWN;
            // AUTO로 돌릴 수 있음
            if (false == GetIO(m_dicOutputSafetyModeKeyUnlock[eFrontRear]))
            {
                eUnlock = CDefine.ESafetyKeyUnlock.SAFETY_KEY_UNLOCK_ON;
            }
            else
            {
                eUnlock = CDefine.ESafetyKeyUnlock.SAFETY_KEY_UNLOCK_OFF;
            }

            return eUnlock;
        }

        /// <summary>
        /// 세이프티 키 잠그는 상태 설정
        /// </summary>
        /// <param name="eFrontRear"></param>
        /// <param name="eUnlock"></param>
        public void SetOutputSafetyKeyUnlock(CDefine.EMachineFrontRear eFrontRear, CDefine.ESafetyKeyUnlock eUnlock)
        {
            if (CDefine.ESafetyKeyUnlock.SAFETY_KEY_UNLOCK_OFF == eUnlock)
            {
                // 세이프티 키 잠금 설정 AUTO 돌릴 수 없음
                SetIO(m_dicOutputSafetyModeKeyUnlock[eFrontRear], CDefine.ON);
            }
            else if (CDefine.ESafetyKeyUnlock.SAFETY_KEY_UNLOCK_ON == eUnlock)
            {
                // 세이프티 키 잠금 해제 AUTO 돌릴 수 있음
                SetIO(m_dicOutputSafetyModeKeyUnlock[eFrontRear], CDefine.OFF);
            }
        }

        public bool IsAnyEnableSwitchGripped()
        {
            if (mEnableSwitchGrips.Count == 0)
            {
                return true;
            }

            foreach (var io in mEnableSwitchGrips)
            {
                if (!GetIO(io))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsSafetyPlcNormal()
        {
            // ! 구형 하드웨어로 X_SAFETY_PLC_STATUS_SIGNAL_NORMAL 신호를 제공하지 않음으로 아래 신호들을 통해서 대체함.
            if (GetIO(CDeviceIODefine.EDigitalInput.X_SAFETY_STATUS_EMS_NORMAL) == false
                || GetIO(CDeviceIODefine.EDigitalInput.X_SAFETY_STATUS_DOOR_FORCE_OPEN_NORMAL) == false
                )
            {
                return false;
            }
            return true;
        }

        public bool IsSafetyAllDoorClosed()
        {
            // ! 구형 하드웨어로 X_SAFETY_DOOR_CLOSE_CHECK 신호를 제공하지 않음으로 아래 신호들을 통해서 대체함.
            return DoorManager.Doors.All(door => door.Value.IsOpened == false);
        }

        public void SetSimulationSafetyAllDoorClosed(bool _/*bIsAllDoorClosed*/)
        {
            // ! 구형 하드웨어로 X_SAFETY_DOOR_CLOSE_CHECK 신호를 제공하지 않음으로 구현부 없음.
        }

        public bool IsAnyMaintKeyChecked()
        {
            if (mMaintKeyCheckers.Count == 0)
            {
                return false;
            }

            foreach (var io in mMaintKeyCheckers)
            {
                if (GetIO(io))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
