using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        [Serializable]
        public class CCLinkIEParameter : CInterfaceParameter
        {
        }

        /// <summary>
        /// 멜섹 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CCLinkIEParameter GetCCLinkIEParameter() => m_objCCLinkIEParameter;

        /// <summary>
        /// CCLink IE 파라미터 선언 (설비간 인터페이스 용)
        /// </summary>
        private CCLinkIEParameter m_objCCLinkIEParameter = new CCLinkIEParameter();

        /// <summary>
        /// MELSEC 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadCCLinkIEParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"CCLINK_IE";
                m_objCCLinkIEParameter.strInterfaceChannel = objINI.GetString(sectionName, "strInterfaceChannel", "151");
                m_objCCLinkIEParameter.strInterfaceNetworkNumber = objINI.GetString(sectionName, "strInterfaceNetworkNumber", "1");
                m_objCCLinkIEParameter.bRunSimulationMode = objINI.GetBool(sectionName, "bRunSimulationMode", false);

                //// 파일로드
                //{
                //    strPath = string.Format(@"{0}\{1}", GetIniPath(), CDefine.DEF_CCLINK_IE_DIGITAL_DAT);
                //    FileStream fs = File.Open(strPath, FileMode.Open);
                //    StreamReader sr = new StreamReader(fs, Encoding.Default);
                //    while (!sr.EndOfStream)
                //    {
                //        string strData = sr.ReadLine();
                //        strData.Trim();
                //        strData = strData.Replace("\t", "");
                //        string[] strParameter = strData.Split(',');

                //        if (5 <= strParameter.Length)
                //        {
                //            CInterfaceParameter.CInterfaceDataParameter objData = new CInterfaceParameter.CInterfaceDataParameter();
                //            objData.strIndex = strParameter[0];
                //            objData.strDataAddress = strParameter[1];
                //            objData.iDataAddress = Convert.ToInt32(objData.strDataAddress, 16);
                //            objData.iDataSize = Convert.ToUInt32(strParameter[2]);
                //            objData.strDataName = strParameter[3];
                //            if ("BIT_IN" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BIT_IN;
                //            else if ("BIT_OUT" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BIT_OUT;
                //            else if ("BINARY_IN" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BINARY_IN;
                //            else if ("BINARY_OUT" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BINARY_OUT;
                //            else if ("INT_IN" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_INT_IN;
                //            else if ("INT_OUT" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_INT_OUT;
                //            else if ("DOUBLE_IN" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_DOUBLE_IN;
                //            else if ("DOUBLE_OUT" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_DOUBLE_OUT;
                //            else if ("BCD_IN" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BCD_IN;
                //            else if ("BCD_OUT" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BCD_OUT;
                //            else if ("ASCII_IN" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_ASCII_IN;
                //            else if ("ASCII_OUT" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_ASCII_OUT;
                //            else
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_UNKWON;
                //            m_objCCLinkIEParameter.objInterfaceParameterDigital.Add(objData.strDataName, objData);
                //        }
                //    }
                //}

                //// 파일로드
                //{
                //    strPath = string.Format(@"{0}\{1}", GetIniPath(), CDefine.DEF_CCLINK_IE_ANALOG_DAT);
                //    FileStream fs = File.Open(strPath, FileMode.Open);
                //    StreamReader sr = new StreamReader(fs, Encoding.Default);
                //    while (!sr.EndOfStream)
                //    {
                //        string strData = sr.ReadLine();
                //        strData.Trim();
                //        strData = strData.Replace("\t", "");
                //        string[] strParameter = strData.Split(',');

                //        if (5 <= strParameter.Length)
                //        {
                //            CInterfaceParameter.CInterfaceDataParameter objData = new CInterfaceParameter.CInterfaceDataParameter();
                //            objData.strIndex = strParameter[0];
                //            objData.strDataAddress = strParameter[1];
                //            objData.iDataAddress = Convert.ToInt32(objData.strDataAddress, 16);
                //            objData.iDataSize = Convert.ToUInt32(strParameter[2]);
                //            objData.strDataName = strParameter[3];
                //            if ("BIT_IN" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BIT_IN;
                //            else if ("BIT_OUT" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BIT_OUT;
                //            else if ("BINARY_IN" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BINARY_IN;
                //            else if ("BINARY_OUT" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BINARY_OUT;
                //            else if ("INT_IN" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_INT_IN;
                //            else if ("INT_OUT" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_INT_OUT;
                //            else if ("DOUBLE_IN" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_DOUBLE_IN;
                //            else if ("DOUBLE_OUT" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_DOUBLE_OUT;
                //            else if ("BCD_IN" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BCD_IN;
                //            else if ("BCD_OUT" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BCD_OUT;
                //            else if ("ASCII_IN" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_ASCII_IN;
                //            else if ("ASCII_OUT" == strParameter[4])
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_ASCII_OUT;
                //            else
                //                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_UNKWON;
                //            m_objCCLinkIEParameter.objInterfaceParameterAnalog.Add(objData.strDataName, objData);
                //        }
                //    }
                //}

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// MELSEC 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveCCLinkIEParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"CCLINK_IE";
                objINI.WriteValue(sectionName, "strInterfaceChannel", m_objCCLinkIEParameter.strInterfaceChannel);
                objINI.WriteValue(sectionName, "strInterfaceNetworkNumber", m_objCCLinkIEParameter.strInterfaceNetworkNumber);
                objINI.WriteValue(sectionName, "bRunSimulationMode", m_objCCLinkIEParameter.bRunSimulationMode);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// MELSEC 파라미터 저장
        /// </summary>
        /// <param name="objCCLinkIEParameter"></param>
        /// <returns></returns>
        public bool SaveCCLinkIEParameter(CCLinkIEParameter objCCLinkIEParameter)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"CCLINK_IE";
                objINI.WriteValue(sectionName, "strInterfaceChannel", m_objCCLinkIEParameter.strInterfaceChannel);
                objINI.WriteValue(sectionName, "strInterfaceNetworkNumber", m_objCCLinkIEParameter.strInterfaceNetworkNumber);
                objINI.WriteValue(sectionName, "bRunSimulationMode", m_objCCLinkIEParameter.bRunSimulationMode);
                m_objCCLinkIEParameter = objCCLinkIEParameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}