using HLDeviceDLL.Interface.CCLink;
using System.Collections.Generic;

namespace HLDevice.Interface
{
    public class CDeviceInterfaceCCLinkVer2Virtual : Abstract.CDeviceInterfaceAbstract
    {
        private readonly CInterfaceError m_objError = new CInterfaceError();
        private readonly CDeviceInterfaceCCLinkVirtual m_objInterface = new CDeviceInterfaceCCLinkVirtual();

        /// <summary>
        /// 초기화 추상객체
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public override bool HLInitialize(CInitializeParameter objInitializeParameter)
        {
            CDeviceInterfaceCCLinkVirtualDefine.CInitializeParameter objParameter = GetInitializeParameter(objInitializeParameter);
            if (false == m_objInterface.HLInitialize(objParameter, SVI_NFT_R.Program.ID))
            {
                MakeError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 해제 추상객체
        /// </summary>
        public override void HLDeInitialize()
        {
            m_objInterface.HLDeInitialize();
        }

        public override bool HLInitializeChannelShare(CInitializeParameter objInitializeParameter)
        {
            CDeviceInterfaceCCLinkVirtualDefine.CInitializeParameter objParameter = GetInitializeParameter(objInitializeParameter);
            if (false == m_objInterface.HLInitializeChannelShare(objParameter))
            {
                MakeError();
                return false;
            }
            return true;
        }

        public override void HLDeInitializeChannelShare()
        {
            m_objInterface.HLDeInitializeChannelShare();
        }

        /// <summary>
        /// 버전 추상객체
        /// </summary>
        /// <returns></returns>
        public override string HLGetVersion()
        {
            return m_objInterface.GetVersion();
        }

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public override bool HLIsConnected()
        {
            return m_objInterface.HLIsConnected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public override bool HLSetInterfaceBit(string strName, bool bData)
        {
            bool bReturn = false;

            do
            {
                if (false == m_objInterface.HLSetInterfaceBit(strName, bData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bValue"></param>
        /// <param name="iIndex"></param>
        /// <returns></returns>
        public override bool HLSetInterfaceValue(string strName, bool bValue, int iIndex)
        {
            bool bReturn = false;

            do
            {
                if (false == m_objInterface.HLSetInterfaceValue(strName, bValue, iIndex))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="pValue"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public override bool HLSetInterfaceValue(string strName, bool[] pValue, int iCount = 16)
        {
            bool bReturn = false;

            do
            {
                if (false == m_objInterface.HLSetInterfaceValue(strName, pValue, iCount))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLSetInterfaceValueEx(string strName, bool[] pValue)
        {
            if (!m_objInterface.HLSetInterfaceValueEx(strName, pValue))
            {
                MakeError();
                return false;
            }
            return true;
        }

        public override bool HLSetInterfaceValueEx(string strName, int[] pValue)
        {
            if (!m_objInterface.HLSetInterfaceValueEx(strName, pValue))
            {
                MakeError();
                return false;
            }
            return true;
        }

        public override bool HLSetInterfaceValueEx(string strName, int pValue, int wordCount)
        {
            if (!m_objInterface.HLSetInterfaceValueEx(strName, pValue, wordCount))
            {
                MakeError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public override bool HLSetInterfaceValue(string strName, int iValue)
        {
            bool bReturn = false;

            do
            {
                if (false == m_objInterface.HLSetInterfaceValue(strName, iValue))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public override bool HLSetInterfaceValue(string strName, double dValue)
        {
            bool bReturn = false;

            do
            {
                if (false == m_objInterface.HLSetInterfaceValue(strName, dValue))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public override bool HLSetInterfaceValue(string strName, string strValue)
        {
            bool bReturn = false;

            do
            {
                if (false == m_objInterface.HLSetInterfaceValue(strName, strValue))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public override bool HLGetInterfaceBit(string strName, out bool bData)
        {
            bool bReturn = false;
            bData = false;
            do
            {
                if (false == m_objInterface.HLGetInterfaceBit(strName, out bData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bData"></param>
        /// <param name="iIndex"></param>
        /// <returns></returns>
        public override bool HLGetInterfaceValue(string strName, out bool bData, int iIndex)
        {
            bool bReturn = false;
            bData = false;
            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out bData, iIndex))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="pValue"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public override bool HLGetInterfaceValue(string strName, out bool[] pValue, out int iCount)
        {
            bool bReturn = false;
            iCount = 16;
            pValue = new bool[iCount];

            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out pValue, out iCount))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLGetInterfaceValueEx(string strName, out bool[] pValue)
        {
            if (!m_objInterface.HLGetInterfaceValueEx(strName, out pValue))
            {
                MakeError();
                return false;
            }
            return true;
        }

        public override bool HLGetInterfaceValueEx(string strName, out int[] pValue)
        {
            if (!m_objInterface.HLGetInterfaceValueEx(strName, out pValue))
            {
                MakeError();
                return false;
            }
            return true;
        }

        public override bool HLGetInterfaceValueEx(string strName, out int pValue, int wordCount)
        {
            if (!m_objInterface.HLGetInterfaceValueEx(strName, out pValue, wordCount))
            {
                MakeError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public override bool HLGetInterfaceValue(string strName, out int iValue)
        {
            bool bReturn = false;
            iValue = 0;
            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out iValue))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public override bool HLGetInterfaceValue(string strName, out double dValue)
        {
            bool bReturn = false;
            dValue = 0;

            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out dValue))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public override bool HLGetInterfaceValue(string strName, out string strValue)
        {
            bool bReturn = false;
            strValue = "";
            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out strValue))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public override CInterfaceError HLGetErrorCode()
        {
            return (CInterfaceError)m_objError.Clone();
        }

        private CDeviceInterfaceCCLinkVirtualDefine.CInitializeParameter GetInitializeParameter(CInitializeParameter objInitializeParameter)
        {
            CDeviceInterfaceCCLinkVirtualDefine.CInitializeParameter objParameter = new CDeviceInterfaceCCLinkVirtualDefine.CInitializeParameter();
            objParameter.strCCLinkChannel = objInitializeParameter.strInterfaceChannel;
            objParameter.strCCLinkNetworkNumber = objInitializeParameter.strInterfaceNetworkNumber;
            objParameter.strCCLinkStationNumber = objInitializeParameter.strInterfaceStationNumber;


            foreach (KeyValuePair<string, CInitializeParameter.CInterfaceParameter> item in objInitializeParameter.objInterfaceParameterDigital)
            {
                CDeviceInterfaceCCLinkVirtualDefine.CCCLinkParameter objMelsecParameter = new CDeviceInterfaceCCLinkVirtualDefine.CCCLinkParameter();

                objMelsecParameter.eDataType = (CDeviceInterfaceCCLinkVirtualDefine.enumDataType)item.Value.eDataType;
                objMelsecParameter.iDataAddress = item.Value.iDataAddress;
                objMelsecParameter.iDataSize = item.Value.iDataSize;
                objMelsecParameter.strDataAddress = item.Value.strDataAddress;
                objMelsecParameter.strDataName = item.Value.strDataName;

                objParameter.objCCLinkParameterDigital.Add(item.Key.ToString(), objMelsecParameter);
            }

            foreach (KeyValuePair<string, CInitializeParameter.CInterfaceParameter> item in objInitializeParameter.objInterfaceParameterAnalog)
            {
                CDeviceInterfaceCCLinkVirtualDefine.CCCLinkParameter objMelsecParameter = new CDeviceInterfaceCCLinkVirtualDefine.CCCLinkParameter();

                objMelsecParameter.eDataType = (CDeviceInterfaceCCLinkVirtualDefine.enumDataType)item.Value.eDataType;
                objMelsecParameter.iDataAddress = item.Value.iDataAddress;
                objMelsecParameter.iDataSize = item.Value.iDataSize;
                objMelsecParameter.strDataAddress = item.Value.strDataAddress;
                objMelsecParameter.strDataName = item.Value.strDataName;

                objParameter.objCCLinkParameterAnalog.Add(item.Key.ToString(), objMelsecParameter);
            }
            return objParameter;
        }

        /// <summary>
        /// DLL에서 나온 에러를 현재 클래스에 맞게 변환한다
        /// </summary>
        private void MakeError()
        {
            CDeviceInterfaceCCLinkVirtualDefine.CCCLinkError objError = m_objInterface.HLGetErrorCode();
            m_objError.iReturnCode = objError.iReturnCode;
            m_objError.strEventTime = objError.strEventTime;
            m_objError.strFunctionName = objError.strFunctionName;
            m_objError.strMessage = objError.strMessage;
        }
    }

}
