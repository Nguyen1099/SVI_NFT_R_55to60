using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// Option Parameter
        /// </summary>
        [Serializable]
        public class COptionParameter
        {
            /// <summary>
            /// 언어설정
            /// </summary>
            public CDefine.ELanguage eLanguage;
            /// <summary>
            /// 부저 사용
            /// </summary>
            public bool bUseBuzzer;
            /// <summary>
            /// MCR 사용
            /// </summary>
            public bool bUseAutoMCR;
            /// <summary>
            /// CIM 사용
            /// </summary>
            public bool bUseCIM;
            /// <summary>
            /// 나찌 오프셋 인터락
            /// </summary>
            public double dNahciOffsetInterlockX;
            public double dNahciOffsetInterlockY;
            public double dNahciOffsetInterlockZ;
            public double dNahciOffsetInterlockRx;
            public double dNahciOffsetInterlockRy;
            public double dNahciOffsetInterlockRz;
            /// <summary>
            /// MCR 리트라이 사용 유무
            /// </summary>
            public bool bUseMCRRetry;
            /// <summary>
            /// MCR 리트라이 카운트
            /// </summary>
            public int iMCRRetryCount;
            /// <summary>
            /// MCR 연속 알람 횟수 ( 연속 알람 발생시 중알람 발생 )
            /// </summary>
            public int iMCRAlarmCount;
            /// <summary>
            /// MCR 연속 알람 초과시 중알람 사용 유무
            /// </summary>
            public bool bUseMCRHeavyAlarm;
            /// <summary>
            /// 검사 결과 타임아웃이 연속으로 n회 발생하면 중알람 발생
            /// </summary>
            public int iResultWaitAlarmCount;
            /// <summary>
            /// FanAlarm 무시 시간
            /// </summary>
            public int iFanAlarmTime;
            /// <summary>
            /// 자재 관리 사용
            /// </summary>
            public bool bUseMaterialsManagement;
            /// <summary>
            /// 자재 관리 알람 발생 시간
            /// </summary>
            public int iMaterialsManagementTime;
            /// <summary>
            /// CIM - Cell Information Download 사용 여부
            /// </summary>     
            public bool bUseCellInfomationResultData;
            /// <summary>
            /// CIM - Job Process 사용 여부
            /// </summary>
            public bool bUseJobProcess;
            /// <summary>
            /// CIM - Recipe Validation 사용 여부
            /// </summary>
            public bool bUseRecipeValidation;
            /// <summary>
            /// GrabStart 명령을 인스펙션 스테이지에 로드 하기 전에 전송하여 택타임을 줄이는 옵션 사용 여부
            /// </summary>
            public bool bUseGrabStartSendBeforeLoading;
            /// <summary>
            /// GrabEnd 명령을 전송한 뒤 바로 언로드를 진행 하고, 로드 위치로 되돌아간 뒤 GrabEnd 명령의 결과를 대기하여 택타임을 줄이는 옵션 사용 여부
            /// </summary>
            public bool bUseGrabEndWaitAfterReturn;
            /// <summary>
            /// CIM에서 트랙인 전에 Cell Lot Information을 요청하여 STEPID를 체크하여 똑같을 경우에만 트랙인 하는 기능 (LOTINFO)
            /// </summary>
            public bool bUseCellLotInformation;
            /// <summary>
            /// 아웃 컨베이어 투입 확인 사용 여부
            /// </summary>
            public bool bUseOutConveyorInputCheck;
            /// <summary>
            /// 아웃 컨베이어 배출 확인 사용 여부
            /// </summary>
            public bool bUseOutConveyorOutputCheck;
            /// <summary>
            /// 아웃 컨베이어 배출부 쌓임 확인 사용 여부
            /// </summary>
            public bool bUseOutConveyorBlockedCheck;
            /// <summary>
            /// 나찌 로봇 팬던트 원격 제어 사용 여부
            /// </summary>
            public bool bUseNachiPendantRemoteControl;
            /// <summary>
            /// 저전력 모드 사용 여부
            /// </summary>
            public bool UseLowEnergyMode;
            /// <summary>
            /// 일반 모드 FFU 팬 속도
            /// </summary>
            public int LowEnergyModeFfuNormalRpm;
            /// <summary>
            /// 저전력 모드 FFU 팬 속도
            /// </summary>
            public int LowEnergyModeFfuSlowRpm;
            /// <summary>
            /// 아웃 로봇 양품자재 Cylinder Turn 사용여부
            /// </summary>
            public bool bUseOutRobotCylinderTurnOk;
            /// <summary>
            /// 아웃 로봇 불량자재 Cylinder Turn 사용여부
            /// </summary>
            public bool bUseOutRobotCylinderTurnNg;
        }

        /// <summary>
        /// 설정 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public COptionParameter GetOptionParameter() => m_objOptionParameter;

        /// <summary>
        /// 설정 파라미터 선언
        /// </summary>
        private COptionParameter m_objOptionParameter = new COptionParameter();

        /// <summary>
        /// 설정 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadOptionParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"OPTION";
                m_objOptionParameter.eLanguage = (CDefine.ELanguage)objINI.GetInt32(sectionName, nameof(COptionParameter.eLanguage), (int)CDefine.ELanguage.LANGUAGE_KOREA);
                m_objOptionParameter.bUseBuzzer = objINI.GetBool(sectionName, nameof(COptionParameter.bUseBuzzer), true);
                m_objOptionParameter.bUseAutoMCR = true;
                m_objOptionParameter.bUseCIM = objINI.GetBool(sectionName, nameof(COptionParameter.bUseCIM), true);
                m_objOptionParameter.dNahciOffsetInterlockX = objINI.GetDouble(sectionName, nameof(COptionParameter.dNahciOffsetInterlockX), 0.0);
                m_objOptionParameter.dNahciOffsetInterlockY = objINI.GetDouble(sectionName, nameof(COptionParameter.dNahciOffsetInterlockY), 0.0);
                m_objOptionParameter.dNahciOffsetInterlockZ = objINI.GetDouble(sectionName, nameof(COptionParameter.dNahciOffsetInterlockZ), 0.0);
                m_objOptionParameter.dNahciOffsetInterlockRx = objINI.GetDouble(sectionName, nameof(COptionParameter.dNahciOffsetInterlockRx), 0.0);
                m_objOptionParameter.dNahciOffsetInterlockRy = objINI.GetDouble(sectionName, nameof(COptionParameter.dNahciOffsetInterlockRy), 0.0);
                m_objOptionParameter.dNahciOffsetInterlockRz = objINI.GetDouble(sectionName, nameof(COptionParameter.dNahciOffsetInterlockRz), 0.0);
                m_objOptionParameter.bUseMCRRetry = objINI.GetBool(sectionName, nameof(COptionParameter.bUseMCRRetry), false);
                m_objOptionParameter.iMCRRetryCount = objINI.GetInt32(sectionName, nameof(COptionParameter.iMCRRetryCount), 0);
                m_objOptionParameter.iMCRAlarmCount = objINI.GetInt32(sectionName, nameof(COptionParameter.iMCRAlarmCount), 0);
                m_objOptionParameter.bUseMCRHeavyAlarm = objINI.GetBool(sectionName, nameof(COptionParameter.bUseMCRHeavyAlarm), false);
                m_objOptionParameter.iResultWaitAlarmCount = objINI.GetInt32(sectionName, nameof(COptionParameter.iResultWaitAlarmCount), 0);
                m_objOptionParameter.iFanAlarmTime = objINI.GetInt32(sectionName, nameof(COptionParameter.iFanAlarmTime), 30);
                m_objOptionParameter.bUseMaterialsManagement = objINI.GetBool(sectionName, nameof(COptionParameter.bUseMaterialsManagement), false);
                m_objOptionParameter.iMaterialsManagementTime = objINI.GetInt32(sectionName, nameof(COptionParameter.iMaterialsManagementTime), 30);
                m_objOptionParameter.bUseCellInfomationResultData = objINI.GetBool(sectionName, nameof(COptionParameter.bUseCellInfomationResultData), false);
                m_objOptionParameter.bUseJobProcess = objINI.GetBool(sectionName, nameof(COptionParameter.bUseJobProcess), true);
                m_objOptionParameter.bUseRecipeValidation = objINI.GetBool(sectionName, nameof(COptionParameter.bUseRecipeValidation), true);
                m_objOptionParameter.bUseGrabStartSendBeforeLoading = objINI.GetBool(sectionName, nameof(COptionParameter.bUseGrabStartSendBeforeLoading), false);
                m_objOptionParameter.bUseGrabEndWaitAfterReturn = objINI.GetBool(sectionName, nameof(COptionParameter.bUseGrabEndWaitAfterReturn), false);
                m_objOptionParameter.bUseCellLotInformation = objINI.GetBool(sectionName, nameof(COptionParameter.bUseCellLotInformation), false);
                m_objOptionParameter.bUseOutConveyorInputCheck = objINI.GetBool(sectionName, nameof(COptionParameter.bUseOutConveyorInputCheck), true);
                m_objOptionParameter.bUseOutConveyorOutputCheck = objINI.GetBool(sectionName, nameof(COptionParameter.bUseOutConveyorOutputCheck), true);
                m_objOptionParameter.bUseOutConveyorBlockedCheck = objINI.GetBool(sectionName, nameof(COptionParameter.bUseOutConveyorBlockedCheck), true);
                m_objOptionParameter.bUseNachiPendantRemoteControl = objINI.GetBool(sectionName, nameof(COptionParameter.bUseNachiPendantRemoteControl), false);
                m_objOptionParameter.UseLowEnergyMode = objINI.GetBool(sectionName, nameof(COptionParameter.UseLowEnergyMode), false);
                m_objOptionParameter.LowEnergyModeFfuNormalRpm = objINI.GetInt32(sectionName, nameof(COptionParameter.LowEnergyModeFfuNormalRpm), 200);
                m_objOptionParameter.LowEnergyModeFfuSlowRpm = objINI.GetInt32(sectionName, nameof(COptionParameter.LowEnergyModeFfuSlowRpm), 100);
                m_objOptionParameter.bUseOutRobotCylinderTurnOk = objINI.GetBool(sectionName, nameof(COptionParameter.bUseOutRobotCylinderTurnOk), true);
                m_objOptionParameter.bUseOutRobotCylinderTurnNg = objINI.GetBool(sectionName, nameof(COptionParameter.bUseOutRobotCylinderTurnNg), false);

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 설정 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveOptionParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"OPTION";
                objINI.WriteValue(sectionName, nameof(COptionParameter.eLanguage), (int)m_objOptionParameter.eLanguage);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseBuzzer), m_objOptionParameter.bUseBuzzer);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseAutoMCR), m_objOptionParameter.bUseAutoMCR);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseCIM), m_objOptionParameter.bUseCIM);
                objINI.WriteValue(sectionName, nameof(COptionParameter.dNahciOffsetInterlockX), m_objOptionParameter.dNahciOffsetInterlockX);
                objINI.WriteValue(sectionName, nameof(COptionParameter.dNahciOffsetInterlockY), m_objOptionParameter.dNahciOffsetInterlockY);
                objINI.WriteValue(sectionName, nameof(COptionParameter.dNahciOffsetInterlockZ), m_objOptionParameter.dNahciOffsetInterlockZ);
                objINI.WriteValue(sectionName, nameof(COptionParameter.dNahciOffsetInterlockRx), m_objOptionParameter.dNahciOffsetInterlockRx);
                objINI.WriteValue(sectionName, nameof(COptionParameter.dNahciOffsetInterlockRy), m_objOptionParameter.dNahciOffsetInterlockRy);
                objINI.WriteValue(sectionName, nameof(COptionParameter.dNahciOffsetInterlockRz), m_objOptionParameter.dNahciOffsetInterlockRz);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseMCRRetry), m_objOptionParameter.bUseMCRRetry);
                objINI.WriteValue(sectionName, nameof(COptionParameter.iMCRAlarmCount), m_objOptionParameter.iMCRAlarmCount);
                objINI.WriteValue(sectionName, nameof(COptionParameter.iMCRRetryCount), m_objOptionParameter.iMCRRetryCount);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseMCRHeavyAlarm), m_objOptionParameter.bUseMCRHeavyAlarm);
                objINI.WriteValue(sectionName, nameof(COptionParameter.iResultWaitAlarmCount), m_objOptionParameter.iResultWaitAlarmCount);
                objINI.WriteValue(sectionName, nameof(COptionParameter.iFanAlarmTime), m_objOptionParameter.iFanAlarmTime);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseMaterialsManagement), m_objOptionParameter.bUseMaterialsManagement);
                objINI.WriteValue(sectionName, nameof(COptionParameter.iMaterialsManagementTime), m_objOptionParameter.iMaterialsManagementTime);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseCellInfomationResultData), m_objOptionParameter.bUseCellInfomationResultData);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseJobProcess), m_objOptionParameter.bUseJobProcess);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseRecipeValidation), m_objOptionParameter.bUseRecipeValidation);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseGrabStartSendBeforeLoading), m_objOptionParameter.bUseGrabStartSendBeforeLoading);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseGrabEndWaitAfterReturn), m_objOptionParameter.bUseGrabEndWaitAfterReturn);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseCellLotInformation), m_objOptionParameter.bUseCellLotInformation);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseOutConveyorInputCheck), m_objOptionParameter.bUseOutConveyorInputCheck);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseOutConveyorOutputCheck), m_objOptionParameter.bUseOutConveyorOutputCheck);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseOutConveyorBlockedCheck), m_objOptionParameter.bUseOutConveyorBlockedCheck);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseNachiPendantRemoteControl), m_objOptionParameter.bUseNachiPendantRemoteControl);
                objINI.WriteValue(sectionName, nameof(COptionParameter.UseLowEnergyMode), m_objOptionParameter.UseLowEnergyMode);
                objINI.WriteValue(sectionName, nameof(COptionParameter.LowEnergyModeFfuNormalRpm), m_objOptionParameter.LowEnergyModeFfuNormalRpm);
                objINI.WriteValue(sectionName, nameof(COptionParameter.LowEnergyModeFfuSlowRpm), m_objOptionParameter.LowEnergyModeFfuSlowRpm);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseOutRobotCylinderTurnOk), m_objOptionParameter.bUseOutRobotCylinderTurnOk);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseOutRobotCylinderTurnNg), m_objOptionParameter.bUseOutRobotCylinderTurnNg);

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 설정 파라미터 저장
        /// </summary>
        /// <param name="objOptionParameter"></param>
        /// <returns></returns>
        public bool SaveOptionParameter(COptionParameter objOptionParameter)
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);

                Constants.SaveBackupFile(strPath);

                ClassINI objINI = new ClassINI(strPath);
                string sectionName = $"OPTION";
                objINI.WriteValue(sectionName, nameof(COptionParameter.eLanguage), (int)objOptionParameter.eLanguage);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseBuzzer), objOptionParameter.bUseBuzzer);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseAutoMCR), objOptionParameter.bUseAutoMCR);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseCIM), objOptionParameter.bUseCIM);
                objINI.WriteValue(sectionName, nameof(COptionParameter.dNahciOffsetInterlockX), objOptionParameter.dNahciOffsetInterlockX);
                objINI.WriteValue(sectionName, nameof(COptionParameter.dNahciOffsetInterlockY), objOptionParameter.dNahciOffsetInterlockY);
                objINI.WriteValue(sectionName, nameof(COptionParameter.dNahciOffsetInterlockZ), objOptionParameter.dNahciOffsetInterlockZ);
                objINI.WriteValue(sectionName, nameof(COptionParameter.dNahciOffsetInterlockRx), objOptionParameter.dNahciOffsetInterlockRx);
                objINI.WriteValue(sectionName, nameof(COptionParameter.dNahciOffsetInterlockRy), objOptionParameter.dNahciOffsetInterlockRy);
                objINI.WriteValue(sectionName, nameof(COptionParameter.dNahciOffsetInterlockRz), objOptionParameter.dNahciOffsetInterlockRz);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseMCRRetry), objOptionParameter.bUseMCRRetry);
                objINI.WriteValue(sectionName, nameof(COptionParameter.iMCRAlarmCount), objOptionParameter.iMCRAlarmCount);
                objINI.WriteValue(sectionName, nameof(COptionParameter.iMCRRetryCount), objOptionParameter.iMCRRetryCount);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseMCRHeavyAlarm), objOptionParameter.bUseMCRHeavyAlarm);
                objINI.WriteValue(sectionName, nameof(COptionParameter.iResultWaitAlarmCount), objOptionParameter.iResultWaitAlarmCount);
                objINI.WriteValue(sectionName, nameof(COptionParameter.iFanAlarmTime), objOptionParameter.iFanAlarmTime);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseMaterialsManagement), objOptionParameter.bUseMaterialsManagement);
                objINI.WriteValue(sectionName, nameof(COptionParameter.iMaterialsManagementTime), objOptionParameter.iMaterialsManagementTime);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseCellInfomationResultData), objOptionParameter.bUseCellInfomationResultData);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseJobProcess), objOptionParameter.bUseJobProcess);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseRecipeValidation), objOptionParameter.bUseRecipeValidation);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseGrabStartSendBeforeLoading), objOptionParameter.bUseGrabStartSendBeforeLoading);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseGrabEndWaitAfterReturn), objOptionParameter.bUseGrabEndWaitAfterReturn);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseCellLotInformation), objOptionParameter.bUseCellLotInformation);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseOutConveyorInputCheck), objOptionParameter.bUseOutConveyorInputCheck);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseOutConveyorOutputCheck), objOptionParameter.bUseOutConveyorOutputCheck);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseOutConveyorBlockedCheck), objOptionParameter.bUseOutConveyorBlockedCheck);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseNachiPendantRemoteControl), objOptionParameter.bUseNachiPendantRemoteControl);
                objINI.WriteValue(sectionName, nameof(COptionParameter.UseLowEnergyMode), objOptionParameter.UseLowEnergyMode);
                objINI.WriteValue(sectionName, nameof(COptionParameter.LowEnergyModeFfuNormalRpm), objOptionParameter.LowEnergyModeFfuNormalRpm);
                objINI.WriteValue(sectionName, nameof(COptionParameter.LowEnergyModeFfuSlowRpm), objOptionParameter.LowEnergyModeFfuSlowRpm);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseOutRobotCylinderTurnOk), objOptionParameter.bUseOutRobotCylinderTurnOk);
                objINI.WriteValue(sectionName, nameof(COptionParameter.bUseOutRobotCylinderTurnNg), objOptionParameter.bUseOutRobotCylinderTurnNg);
                m_objOptionParameter = objOptionParameter.DeepClone();

                bReturn = true;
            } while (false);
            return bReturn;
        }
    }
}