using System.Collections.Generic;

namespace HLDevice.Interface
{
    public class CDeviceInterfaceCCLinkVer2 : Abstract.CDeviceInterfaceAbstract
    {
        private readonly CInterfaceError m_objError = new CInterfaceError();
        private readonly HLDeviceDLL.Interface.CCLink.HLDeviceInterfaceCCLink m_objInterface = new HLDeviceDLL.Interface.CCLink.HLDeviceInterfaceCCLink();

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public override bool HLInitialize(CInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {
                HLDeviceDLL.Interface.CCLink.CDeviceInterfaceCCLinkDefine.CInitializeParameter objParameter = new HLDeviceDLL.Interface.CCLink.CDeviceInterfaceCCLinkDefine.CInitializeParameter();
                objParameter.strCCLinkChannel = objInitializeParameter.strInterfaceChannel;
                objParameter.strCCLinkNetworkNumber = objInitializeParameter.strInterfaceNetworkNumber;
                objParameter.strCCLinkStationNumber = objInitializeParameter.strInterfaceStationNumber;

                foreach (KeyValuePair<string, CInitializeParameter.CInterfaceParameter> item in objInitializeParameter.objInterfaceParameterDigital)
                {
                    HLDeviceDLL.Interface.CCLink.CDeviceInterfaceCCLinkDefine.CCCLinkParameter objCCLinkParameter = new HLDeviceDLL.Interface.CCLink.CDeviceInterfaceCCLinkDefine.CCCLinkParameter();

                    objCCLinkParameter.eDataType = (HLDeviceDLL.Interface.CCLink.CDeviceInterfaceCCLinkDefine.enumDataType)item.Value.eDataType;
                    objCCLinkParameter.iDataAddress = item.Value.iDataAddress;
                    objCCLinkParameter.iDataSize = item.Value.iDataSize;
                    objCCLinkParameter.strDataAddress = item.Value.strDataAddress;
                    objCCLinkParameter.strDataName = item.Value.strDataName;

                    objParameter.objCCLinkParameterDigital.Add(item.Key.ToString(), objCCLinkParameter);
                }

                foreach (KeyValuePair<string, CInitializeParameter.CInterfaceParameter> item in objInitializeParameter.objInterfaceParameterAnalog)
                {
                    HLDeviceDLL.Interface.CCLink.CDeviceInterfaceCCLinkDefine.CCCLinkParameter objCCLinkParameter = new HLDeviceDLL.Interface.CCLink.CDeviceInterfaceCCLinkDefine.CCCLinkParameter();

                    objCCLinkParameter.eDataType = (HLDeviceDLL.Interface.CCLink.CDeviceInterfaceCCLinkDefine.enumDataType)item.Value.eDataType;
                    objCCLinkParameter.iDataAddress = item.Value.iDataAddress;
                    objCCLinkParameter.iDataSize = item.Value.iDataSize;
                    objCCLinkParameter.strDataAddress = item.Value.strDataAddress;
                    objCCLinkParameter.strDataName = item.Value.strDataName;

                    objParameter.objCCLinkParameterAnalog.Add(item.Key.ToString(), objCCLinkParameter);
                }

                if (false == m_objInterface.HLInitialize(objParameter))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void HLDeInitialize()
        {
            m_objInterface.HLDeInitialize();
        }

        /// <summary>
        /// 버전
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
            ;
        }

        /// <summary>
        /// BIT영역 Write
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
        /// WORD영역 Write - WORD 영역을 BIT단위 Write
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
        /// WORD영역 Write - WORD 영역을 BIT단위 Write
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

        /// <summary>
        /// WORD영역 Write - int
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
        /// WORD영역 Write - double
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
        /// WORD영역 Write - string
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
        /// BIT영역 Read
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public override bool HLGetInterfaceBit(string strName, out bool bData)
        {
            bool bReturn = false;
            bool bResutlData = false;
            do
            {
                if (false == m_objInterface.HLGetInterfaceBit(strName, out bResutlData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            bData = bResutlData;
            return bReturn;
        }

        /// <summary>
        /// WORD영역 Write - WORD 영역을 BIT단위 Read
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bData"></param>
        /// <param name="iIndex"></param>
        /// <returns></returns>
        public override bool HLGetInterfaceValue(string strName, out bool bData, int iIndex)
        {
            bool bReturn = false;
            bool bResutlData = false;
            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out bResutlData, iIndex))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);
            bData = bResutlData;
            return bReturn;
        }

        /// <summary>
        /// WORD영역 Write - WORD 영역을 BIT단위 Read
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="pValue"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public override bool HLGetInterfaceValue(string strName, out bool[] pValue, out int iCount)
        {
            bool bReturn = false;
            iCount = 16;
            bool[] pResultValue = new bool[iCount];

            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out pResultValue, out iCount))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);
            pValue = pResultValue;

            return bReturn;
        }

        /// <summary>
        /// WORD영역 Read - int
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public override bool HLGetInterfaceValue(string strName, out int iValue)
        {
            bool bReturn = false;
            int iResultValue = 0;
            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out iResultValue))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            iValue = iResultValue;
            return bReturn;
        }

        /// <summary>
        /// WORD영역 Read - double
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public override bool HLGetInterfaceValue(string strName, out double dValue)
        {
            bool bReturn = false;
            double dResultValue = 0;

            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out dResultValue))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            dValue = dResultValue;
            return bReturn;
        }

        /// <summary>
        /// WORD영역 Read - string
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public override bool HLGetInterfaceValue(string strName, out string strValue)
        {

            bool bReturn = false;
            string strResutlValue = "";
            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out strResutlValue))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            strValue = strResutlValue;
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

        /// <summary>
        /// DLL에서 나온 에러를 현재 클래스에 맞게 변환한다
        /// </summary>
        private void MakeError()
        {
            HLDeviceDLL.Interface.CCLink.CDeviceInterfaceCCLinkDefine.CCCLinkError objError = m_objInterface.HLGetErrorCode();
            m_objError.iReturnCode = objError.iReturnCode;
            m_objError.strEventTime = objError.strEventTime;
            m_objError.strFunctionName = objError.strFunctionName;
            m_objError.strMessage = objError.strMessage;
        }

    }
}
