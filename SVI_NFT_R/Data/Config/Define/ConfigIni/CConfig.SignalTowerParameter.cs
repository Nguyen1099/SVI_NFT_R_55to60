using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// Signal Tower Parameter
        /// </summary>
        [Serializable]
        public class CSignalTowerParameter
        {
            /// <summary>
            /// 램프 상태값
            /// </summary>
            public CDefine.ELampValue[,] eLampValue = new CDefine.ELampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_FINAL, (int)CDefine.ELampColor.LAMP_COLOR_FINAL];

            /// <summary>
            /// 부저 상태값
            /// </summary>
            public CDefine.EBuzzerValue[] eBuzzerValue = new CDefine.EBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_FINAL];

            /// <summary>
            /// 시나리오 사용 여부
            /// </summary>
            public bool[] bEnableScenario = new bool[(int)CDefine.ELampSituation.LAMP_SITUATION_FINAL];
        }

        /// <summary>
        /// 시그널 타워 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CSignalTowerParameter GetSignalTowerParameter() => m_objSignalTowerParameter;

        /// <summary>
        /// 시그널 타워 파라미터 선언
        /// </summary>
        private CSignalTowerParameter m_objSignalTowerParameter = new CSignalTowerParameter();

        /// <summary>
        /// 시그널 타워 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadSignalTowerParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"SIGNAL_TOWER";
                for (int iLoopLampSituation = 0; iLoopLampSituation < m_objSignalTowerParameter.eLampValue.GetLength(0); iLoopLampSituation++)
                {
                    m_objSignalTowerParameter.bEnableScenario[iLoopLampSituation] = objINI.GetBool(sectionName, string.Format("bEnableScenario[{0}]", iLoopLampSituation), true);
                    m_objSignalTowerParameter.eBuzzerValue[iLoopLampSituation] = (CDefine.EBuzzerValue)objINI.GetInt32(sectionName, string.Format("eBuzzerValue[{0}]", iLoopLampSituation), 0);

                    for (int iLoopLampColor = 0; iLoopLampColor < m_objSignalTowerParameter.eLampValue.GetLength(1); iLoopLampColor++)
                    {
                        m_objSignalTowerParameter.eLampValue[iLoopLampSituation, iLoopLampColor] = (CDefine.ELampValue)objINI.GetInt32(sectionName, string.Format("eLampValue[{0},{1}]", iLoopLampSituation, iLoopLampColor), 0);
                    }
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 시그널 타워 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveSignalTowerParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"SIGNAL_TOWER";
                for (int iLoopLampSituation = 0; iLoopLampSituation < m_objSignalTowerParameter.eLampValue.GetLength(0); iLoopLampSituation++)
                {
                    objINI.WriteValue(sectionName, string.Format("bEnableScenario[{0}]", iLoopLampSituation), m_objSignalTowerParameter.bEnableScenario[iLoopLampSituation]);
                    objINI.WriteValue(sectionName, string.Format("eBuzzerValue[{0}]", iLoopLampSituation), (int)m_objSignalTowerParameter.eBuzzerValue[iLoopLampSituation]);

                    for (int iLoopLampColor = 0; iLoopLampColor < m_objSignalTowerParameter.eLampValue.GetLength(1); iLoopLampColor++)
                    {
                        objINI.WriteValue(sectionName, string.Format("eLampValue[{0},{1}]", iLoopLampSituation, iLoopLampColor), (int)m_objSignalTowerParameter.eLampValue[iLoopLampSituation, iLoopLampColor]);
                    }
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 시그널 타워 파라미터 저장
        /// </summary>
        /// <param name="objSignalTowerParameter"></param>
        /// <returns></returns>
        public bool SaveSignalTowerParameter(CSignalTowerParameter objSignalTowerParameter)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);

                Constants.SaveBackupFile(strPath);

                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"SIGNAL_TOWER";
                for (int iLoopLampSituation = 0; iLoopLampSituation < objSignalTowerParameter.eLampValue.GetLength(0); iLoopLampSituation++)
                {
                    objINI.WriteValue(sectionName, string.Format("bEnableScenario[{0}]", iLoopLampSituation), objSignalTowerParameter.bEnableScenario[iLoopLampSituation]);
                    objINI.WriteValue(sectionName, string.Format("eBuzzerValue[{0}]", iLoopLampSituation), (int)objSignalTowerParameter.eBuzzerValue[iLoopLampSituation]);

                    for (int iLoopLampColor = 0; iLoopLampColor < objSignalTowerParameter.eLampValue.GetLength(1); iLoopLampColor++)
                    {
                        objINI.WriteValue(sectionName, string.Format("eLampValue[{0},{1}]", iLoopLampSituation, iLoopLampColor), (int)objSignalTowerParameter.eLampValue[iLoopLampSituation, iLoopLampColor]);
                    }
                }
                m_objSignalTowerParameter = objSignalTowerParameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}