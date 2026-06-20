using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// System
        /// </summary>
        [Serializable]
        public class CSystemParameter
        {
            /// <summary>
            /// 시뮬레이션 모드
            /// </summary>
            public CDefine.ESimulationMode eSimulationMode;

            /// <summary>
            /// PPID
            /// </summary>
            public string strPPID;

            /// <summary>
            /// Equipment ID
            /// </summary>
            public string strEquipmentID;

            /// <summary>
            /// Equipment Serial Number
            /// </summary>
            public string strEquipmentSN;

            /// <summary>
            /// Step ID
            /// </summary>
            public string strStepID;

            /// <summary>
            /// 배출 택타임 최대 표시
            /// </summary>
            public int iTactTimeMaxCount;

            /// <summary>
            /// 배출 택타임 간단히 표시 여부
            /// </summary>
            public bool bIsOutTactTimeSimpleView;

            /// <summary>
            /// 주간 작업자 근무 시작 시간
            /// </summary>
            public int iWorkerTimeWeeklyStart;

            /// <summary>
            /// 주간 작업자 근무 종료 시간
            /// </summary>
            public int iWorkerTimeWeeklyEnd;

            /// <summary>
            /// 야간 작업자 근무 시작 시간
            /// </summary>
            public int iWorkerTimeNightStart;

            /// <summary>
            /// 야간 작업자 근무 종료 시간
            /// </summary>
            public int iWorkerTimeNightEnd;
        };

        /// <summary>
        /// 시스템 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CSystemParameter GetSystemParameter() => m_objSystemParameter;

        /// <summary>
        /// 시스템 파라미터 선언
        /// </summary>
        private CSystemParameter m_objSystemParameter = new CSystemParameter();

        /// <summary>
        /// 시스템 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadSystemParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"SYSTEM";
                m_objSystemParameter.eSimulationMode = (CDefine.ESimulationMode)objINI.GetInt32(sectionName, "eSimulationMode", (int)CDefine.ESimulationMode.SIMULATION_MODE_ON);
                m_objSystemParameter.strPPID = objINI.GetString(sectionName, "strPPID", "TT_1000");
                m_objSystemParameter.strEquipmentID = objINI.GetString(sectionName, "strEquipmentID", "B3INE03_RS01");
                m_objSystemParameter.strEquipmentSN = objINI.GetString(sectionName, "strEquipmentSN", "E2508A-115");
                m_objSystemParameter.strStepID = objINI.GetString(sectionName, "strStepID", "ME540");
                m_objSystemParameter.iTactTimeMaxCount = objINI.GetInt32(sectionName, "iTactTimeMaxCount", 30);
                m_objSystemParameter.bIsOutTactTimeSimpleView = objINI.GetBool(sectionName, "bIsOutTactTimeSimpleView", false);
                m_objSystemParameter.iWorkerTimeWeeklyStart = objINI.GetInt32(sectionName, "iWorkerTimeWeeklyStart", 8);
                m_objSystemParameter.iWorkerTimeWeeklyEnd = objINI.GetInt32(sectionName, "iWorkerTimeWeeklyEnd", 20);
                m_objSystemParameter.iWorkerTimeNightStart = objINI.GetInt32(sectionName, "iWorkerTimeNightStart", 20);
                m_objSystemParameter.iWorkerTimeNightEnd = objINI.GetInt32(sectionName, "iWorkerTimeNightEnd", 8);
                foreach (string password in objINI.GetString(sectionName, nameof(CDefine.EquipmentPasswordNumbers), "1,3333").Split(','))
                {
                    CDefine.EquipmentPasswordNumbers.Add(password);
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 시스템 파라미터 저장
        /// </summary>
        /// <param name="objSystemParameter"></param>
        /// <returns></returns>
        public bool SaveSystemParameter(CSystemParameter objSystemParameter)
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"SYSTEM";
                objINI.WriteValue(sectionName, "eSimulationMode", (int)objSystemParameter.eSimulationMode);
                objINI.WriteValue(sectionName, "strPPID", objSystemParameter.strPPID);
                objINI.WriteValue(sectionName, "strEquipmentID", objSystemParameter.strEquipmentID);
                objINI.WriteValue(sectionName, "strEquipmentSN", objSystemParameter.strEquipmentSN);
                objINI.WriteValue(sectionName, "strStepID", objSystemParameter.strStepID);
                objINI.WriteValue(sectionName, "iTactTimeMaxCount", objSystemParameter.iTactTimeMaxCount);
                objINI.WriteValue(sectionName, "bIsOutTactTimeSimpleView", objSystemParameter.bIsOutTactTimeSimpleView);
                objINI.WriteValue(sectionName, "iWorkerTimeWeeklyStart", objSystemParameter.iWorkerTimeWeeklyStart);
                objINI.WriteValue(sectionName, "iWorkerTimeWeeklyEnd", objSystemParameter.iWorkerTimeWeeklyEnd);
                objINI.WriteValue(sectionName, "iWorkerTimeNightStart", objSystemParameter.iWorkerTimeNightStart);
                objINI.WriteValue(sectionName, "iWorkerTimeNightEnd", objSystemParameter.iWorkerTimeNightEnd);
                m_objSystemParameter = objSystemParameter.DeepClone();

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 시스템 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveSystemParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"SYSTEM";
                objINI.WriteValue(sectionName, "eSimulationMode", (int)m_objSystemParameter.eSimulationMode);
                objINI.WriteValue(sectionName, "strPPID", m_objSystemParameter.strPPID);
                objINI.WriteValue(sectionName, "strEquipmentID", m_objSystemParameter.strEquipmentID);
                objINI.WriteValue(sectionName, "strEquipmentSN", m_objSystemParameter.strEquipmentSN);
                objINI.WriteValue(sectionName, "strStepID", m_objSystemParameter.strStepID);
                objINI.WriteValue(sectionName, "iTactTimeMaxCount", m_objSystemParameter.iTactTimeMaxCount);
                objINI.WriteValue(sectionName, "bIsOutTactTimeSimpleView", m_objSystemParameter.bIsOutTactTimeSimpleView);
                objINI.WriteValue(sectionName, "iWorkerTimeWeeklyStart", m_objSystemParameter.iWorkerTimeWeeklyStart);
                objINI.WriteValue(sectionName, "iWorkerTimeWeeklyEnd", m_objSystemParameter.iWorkerTimeWeeklyEnd);
                objINI.WriteValue(sectionName, "iWorkerTimeNightStart", m_objSystemParameter.iWorkerTimeNightStart);
                objINI.WriteValue(sectionName, "iWorkerTimeNightEnd", m_objSystemParameter.iWorkerTimeNightEnd);

                bReturn = true;
            } while (false);
            return bReturn;
        }
    }
}