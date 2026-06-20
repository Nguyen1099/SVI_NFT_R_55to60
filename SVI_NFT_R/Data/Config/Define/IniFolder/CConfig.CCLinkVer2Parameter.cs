using System;
using System.IO;
using System.Text;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        [Serializable]
        public class CCLinkVer2Parameter : CInterfaceParameter
        {
        }

        /// <summary>
        /// CCLink 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CCLinkVer2Parameter GetCCLinkVer2Parameter() => m_objCCLinkVer2Parameter;

        /// <summary>
        /// CCLink Ver2 파라미터 선언 (로봇 인터페이스 용)
        /// </summary>
        private CCLinkVer2Parameter m_objCCLinkVer2Parameter = new CCLinkVer2Parameter();

        /// <summary>
        /// CCLink 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadCCLinkVer2Parameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"CCLINK_VER2";
                m_objCCLinkVer2Parameter.strInterfaceChannel = objINI.GetString(sectionName, "strInterfaceChannel", "81");
                m_objCCLinkVer2Parameter.strInterfaceNetworkNumber = objINI.GetString(sectionName, "strInterfaceNetworkNumber", "1");
                m_objCCLinkVer2Parameter.bRunSimulationMode = objINI.GetBool(sectionName, "bRunSimulationMode", false);

                // 파일로드 X
                {
                    strPath = string.Format(@"{0}\{1}", GetIniPath(), CDefine.DEF_CCLINK_VER2_DIGITAL_DAT);
                    FileStream fs = File.Open(strPath, FileMode.Open);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    while (!sr.EndOfStream)
                    {
                        string strData = sr.ReadLine();
                        strData.Trim();
                        strData = strData.Replace("\t", "");
                        string[] strParameter = strData.Split(',');

                        if (6 <= strParameter.Length)
                        {
                            CInterfaceParameter.CInterfaceDataParameter objData = new CInterfaceParameter.CInterfaceDataParameter();
                            objData.strIndex = strParameter[0];
                            objData.strDataAddress = strParameter[1];
                            objData.iDataAddress = Convert.ToInt32(objData.strDataAddress, 16);
                            objData.iDataSize = Convert.ToUInt32(strParameter[2]);
                            objData.strDataName = strParameter[3];
                            if ("BIT_IN" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BIT_IN;
                            else if ("BIT_OUT" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BIT_OUT;
                            else if ("BINARY_IN" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BINARY_IN;
                            else if ("BINARY_OUT" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BINARY_OUT;
                            else if ("INT_IN" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_INT_IN;
                            else if ("INT_OUT" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_INT_OUT;
                            else if ("DOUBLE_IN" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_DOUBLE_IN;
                            else if ("DOUBLE_OUT" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_DOUBLE_OUT;
                            else if ("BCD_IN" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BCD_IN;
                            else if ("BCD_OUT" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BCD_OUT;
                            else if ("ASCII_IN" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_ASCII_IN;
                            else if ("ASCII_OUT" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_ASCII_OUT;
                            else
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_UNKWON;
                            objData.strOffsetName = strParameter[5];
                            m_objCCLinkVer2Parameter.objInterfaceParameterDigital.Add(objData.strIndex, objData);
                        }
                    }
                }

                // 파일로드
                {
                    strPath = string.Format(@"{0}\{1}", GetIniPath(), CDefine.DEF_CCLINK_VER2_ANALOG_DAT);
                    FileStream fs = File.Open(strPath, FileMode.Open);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    while (!sr.EndOfStream)
                    {
                        string strData = sr.ReadLine();
                        strData.Trim();
                        strData = strData.Replace("\t", "");
                        string[] strParameter = strData.Split(',');

                        if (7 <= strParameter.Length)
                        {
                            CInterfaceParameter.CInterfaceDataParameter objData = new CInterfaceParameter.CInterfaceDataParameter();
                            objData.strIndex = strParameter[0];
                            objData.strDataAddress = strParameter[1];
                            objData.iDataAddress = Convert.ToInt32(objData.strDataAddress, 16);
                            objData.iDataSize = Convert.ToUInt32(strParameter[2]);
                            objData.strDataName = strParameter[3];
                            if ("BIT_IN" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BIT_IN;
                            else if ("BIT_OUT" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BIT_OUT;
                            else if ("BINARY_IN" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BINARY_IN;
                            else if ("BINARY_OUT" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BINARY_OUT;
                            else if ("INT_IN" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_INT_IN;
                            else if ("INT_OUT" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_INT_OUT;
                            else if ("DOUBLE_IN" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_DOUBLE_IN;
                            else if ("DOUBLE_OUT" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_DOUBLE_OUT;
                            else if ("BCD_IN" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BCD_IN;
                            else if ("BCD_OUT" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_BCD_OUT;
                            else if ("ASCII_IN" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_ASCII_IN;
                            else if ("ASCII_OUT" == strParameter[4])
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_ASCII_OUT;
                            else
                                objData.eDataType = CInterfaceParameter.EDataType.DATA_TYPE_UNKWON;
                            objData.strOffsetName = strParameter[5];
                            objData.iStartOffset = Convert.ToUInt32(strParameter[6]);
                            m_objCCLinkVer2Parameter.objInterfaceParameterAnalog.Add(objData.strIndex, objData);
                        }
                    }
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// CCLink 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveCCLinkVer2Parameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"CCLINK_VER2";
                objINI.WriteValue(sectionName, "strInterfaceChannel", m_objCCLinkVer2Parameter.strInterfaceChannel);
                objINI.WriteValue(sectionName, "strInterfaceNetworkNumber", m_objCCLinkVer2Parameter.strInterfaceNetworkNumber);
                objINI.WriteValue(sectionName, "bRunSimulationMode", m_objCCLinkVer2Parameter.bRunSimulationMode);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// CCLink 파라미터 저장
        /// </summary>
        /// <param name="objCCLinkVer2Parameter"></param>
        /// <returns></returns>
        public bool SaveCCLinkVer2Parameter(CCLinkVer2Parameter objCCLinkVer2Parameter)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"CCLINK_VER2";
                objINI.WriteValue(sectionName, "strInterfaceChannel", m_objCCLinkVer2Parameter.strInterfaceChannel);
                objINI.WriteValue(sectionName, "strInterfaceNetworkNumber", m_objCCLinkVer2Parameter.strInterfaceNetworkNumber);
                objINI.WriteValue(sectionName, "bRunSimulationMode", m_objCCLinkVer2Parameter.bRunSimulationMode);
                m_objCCLinkVer2Parameter = objCCLinkVer2Parameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}