using System;
using System.Collections.Generic;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// SpecificationVersionStateParameter
        /// </summary>
        [Serializable]
        public class CSpecificationVersionStateParameter
        {
            public Dictionary<string, string> SpecList { get; set; } = new Dictionary<string, string>();
        }

        /// <summary>
        /// 설정 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CSpecificationVersionStateParameter GetSpecificationVersionStateParameter() => m_objSpecificationVersionStateParameter;

        /// <summary>
        /// 설정 파라미터 선언
        /// </summary>
        private CSpecificationVersionStateParameter m_objSpecificationVersionStateParameter = new CSpecificationVersionStateParameter();

        /// <summary>
        /// 설정 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadSpecificationVersionStateParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", GetIniPath(), CDefine.DEF_SPECIFICATION_VERSIONSTATE_PARAMETER_INI);
                ClassINI objINI = new ClassINI(strPath);
                string sectionName = $"SPECIFICATIONVERSIONSTATEREQUEST";
                var keyName = objINI.GetKeyNames(sectionName);
                for (int iLoopCount = 0; iLoopCount < keyName.Length; iLoopCount++)
                {
                    m_objSpecificationVersionStateParameter.SpecList[keyName[iLoopCount]] = objINI.GetString(sectionName, keyName[iLoopCount], "");
                }

                bReturn = true;
            } while (false);
            return bReturn;
        }
    }
}