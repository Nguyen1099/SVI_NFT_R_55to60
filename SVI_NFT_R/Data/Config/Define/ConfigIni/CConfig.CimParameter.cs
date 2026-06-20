using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// CIMParameter
        /// </summary>
        [Serializable]
        public class CCimParameter
        {
            /// <summary>
            /// IP Address
            /// </summary>
            public string strIPAddress;

            /// <summary>
            /// Port Number
            /// </summary>
            public int iPortNo;

            /// <summary>
            /// Equipment Function Buffer (보고 전에 저장할 버퍼. 설비 내부에 CELL이 없으면 업데이트함.)
            /// </summary>
            public string[] strEFValueBuffer = new string[(int)CDialogEQPFunctionList.EFName.EF_NAME_FINAL];

            /// <summary>
            /// Equipment Function을 최종으로 변경한 주체
            /// </summary>
            public string strEFValueChangeByWho;

            /// <summary>
            /// Equipment Function (CIM에 보고용, 현재 실제로 적용중인 기능)
            /// </summary>
            public string[] strEFValue = new string[(int)CDialogEQPFunctionList.EFName.EF_NAME_FINAL];
        }

        /// <summary>
        /// 심 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CCimParameter GetCimParameter() => m_objCimParameter;

        /// <summary>
        /// 심 관련 파라미터 선언
        /// </summary>
        private CCimParameter m_objCimParameter = new CCimParameter();

        /// <summary>
        /// 심 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadCimParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"CIM";
                m_objCimParameter.strIPAddress = objINI.GetString(sectionName, "strIPAddress", "127.0.0.1");
                m_objCimParameter.iPortNo = objINI.GetInt32(sectionName, "iPortNo", 9601);
                for (int iLoopCount = (int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_TRACKING; iLoopCount < (int)CDialogEQPFunctionList.EFName.EF_NAME_FINAL; iLoopCount++)
                {
                    CDialogEQPFunctionList.EFName efid = (CDialogEQPFunctionList.EFName)iLoopCount;
                    string defaultValue = CDialogEQPFunctionList.GetEfstDefaultValue(efid);
                    m_objCimParameter.strEFValueBuffer[iLoopCount] = objINI.GetString(sectionName, string.Format("strEFValueBuffer[{0}]", iLoopCount), defaultValue);
                }
                for (int iLoopCount = (int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_TRACKING; iLoopCount < (int)CDialogEQPFunctionList.EFName.EF_NAME_FINAL; iLoopCount++)
                {
                    CDialogEQPFunctionList.EFName efid = (CDialogEQPFunctionList.EFName)iLoopCount;
                    string defaultValue = CDialogEQPFunctionList.GetEfstDefaultValue(efid);
                    m_objCimParameter.strEFValue[iLoopCount] = objINI.GetString(sectionName, string.Format("strEFValue[{0}]", iLoopCount), defaultValue);
                }
                m_objCimParameter.strEFValueChangeByWho = objINI.GetString(sectionName, "strEFValueChangeByWho", "EQP");

                // 설비에서 지원하지 않는 조합으로 값이 설정되어 있는 경우 기본값으로 바꾸고 전이도를 적용한다
                for (int iLoopCount = (int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_TRACKING; iLoopCount < (int)CDialogEQPFunctionList.EFName.EF_NAME_FINAL; iLoopCount++)
                {
                    CDialogEQPFunctionList.EFName efid = (CDialogEQPFunctionList.EFName)iLoopCount;
                    string defaultValue = CDialogEQPFunctionList.GetEfstDefaultValue(efid);
                    if (CDialogEQPFunctionList.CanEditEfstCombination(efid, m_objCimParameter.strEFValueBuffer[iLoopCount]) == false)
                    {
                        CDialogEQPFunctionList.UpdateEfstValue(efid, defaultValue, ref m_objCimParameter.strEFValueBuffer);
                    }
                    if (CDialogEQPFunctionList.CanEditEfstCombination(efid, m_objCimParameter.strEFValue[iLoopCount]) == false)
                    {
                        CDialogEQPFunctionList.UpdateEfstValue(efid, defaultValue, ref m_objCimParameter.strEFValue);
                    }
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 심 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveCimParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"CIM";
                objINI.WriteValue(sectionName, "strIPAddress", m_objCimParameter.strIPAddress);
                objINI.WriteValue(sectionName, "iPortNo", m_objCimParameter.iPortNo);
                for (int iLoopCount = (int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_TRACKING; iLoopCount < (int)CDialogEQPFunctionList.EFName.EF_NAME_FINAL; iLoopCount++)
                {
                    objINI.WriteValue(sectionName, string.Format("strEFValueBuffer[{0}]", iLoopCount), m_objCimParameter.strEFValueBuffer[iLoopCount]);
                }
                for (int iLoopCount = (int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_TRACKING; iLoopCount < (int)CDialogEQPFunctionList.EFName.EF_NAME_FINAL; iLoopCount++)
                {
                    objINI.WriteValue(sectionName, string.Format("strEFValue[{0}]", iLoopCount), m_objCimParameter.strEFValue[iLoopCount]);
                }
                objINI.WriteValue(sectionName, "strEFValueChangeByWho", m_objCimParameter.strEFValueChangeByWho);

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 심 파라미터 저장
        /// </summary>
        /// <param name="objCIMParameter"></param>
        /// <returns></returns>
        public bool SaveCimParameter(CCimParameter objCIMParameter)
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"CIM";
                objINI.WriteValue(sectionName, "strIPAddress", objCIMParameter.strIPAddress);
                objINI.WriteValue(sectionName, "iPortNo", objCIMParameter.iPortNo);
                for (int iLoopCount = (int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_TRACKING; iLoopCount < (int)CDialogEQPFunctionList.EFName.EF_NAME_FINAL; iLoopCount++)
                {
                    objINI.WriteValue(sectionName, string.Format("strEFValueBuffer[{0}]", iLoopCount), objCIMParameter.strEFValueBuffer[iLoopCount]);
                }
                for (int iLoopCount = (int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_TRACKING; iLoopCount < (int)CDialogEQPFunctionList.EFName.EF_NAME_FINAL; iLoopCount++)
                {
                    objINI.WriteValue(sectionName, string.Format("strEFValue[{0}]", iLoopCount), objCIMParameter.strEFValue[iLoopCount]);
                }
                objINI.WriteValue(sectionName, "strEFValueChangeByWho", objCIMParameter.strEFValueChangeByWho);
                m_objCimParameter = objCIMParameter.DeepClone();

                bReturn = true;
            } while (false);
            return bReturn;
        }
    }
}