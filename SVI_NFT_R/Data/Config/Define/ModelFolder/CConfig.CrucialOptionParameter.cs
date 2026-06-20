using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// Crucial Option Parameter
        /// </summary>
        [Serializable]
        public class CCrucialOptionParameter
        {
            /// <summary>
            /// BIN PRIME MODE
            /// </summary>
            public string strBinPrimeMode;
        }

        /// <summary>
        /// 설정 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CCrucialOptionParameter GetCrucialOptionParameter() => m_objCrucialOptionParameter;

        /// <summary>
        /// 설정 파라미터 선언
        /// </summary>
        private CCrucialOptionParameter m_objCrucialOptionParameter = new CCrucialOptionParameter();

        /// <summary>
        /// 설정 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadCrucialOptionParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format(@"{0}\{1}", GetModelPath(), CDefine.DEF_CRUCIAL_OPTION_INI);
                ClassINI objINI = new ClassINI(strPath);

                m_objCrucialOptionParameter.strBinPrimeMode = objINI.GetString("OPTION", "BINPRIME MODE", "OFF");

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 설정 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveCrucialOptionParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format(@"{0}\{1}", GetModelPath(), CDefine.DEF_CRUCIAL_OPTION_INI);
                ClassINI objINI = new ClassINI(strPath);

                objINI.WriteValue("OPTION", "BINPRIME MODE", m_objCrucialOptionParameter.strBinPrimeMode);

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 설정 파라미터 저장
        /// </summary>
        /// <param name="objCrucialOptionParameter"></param>
        /// <returns></returns>
        public bool SaveCrucialOptionParameter(CCrucialOptionParameter objCrucialOptionParameter)
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format(@"{0}\{1}", GetModelPath(), CDefine.DEF_CRUCIAL_OPTION_INI);

                Constants.SaveBackupFile(strPath);

                ClassINI objINI = new ClassINI(strPath);
                objINI.WriteValue("OPTION", "BINPRIME MODE", objCrucialOptionParameter.strBinPrimeMode);

                m_objCrucialOptionParameter = objCrucialOptionParameter.DeepClone();

                bReturn = true;
            } while (false);
            return bReturn;
        }
    }
}