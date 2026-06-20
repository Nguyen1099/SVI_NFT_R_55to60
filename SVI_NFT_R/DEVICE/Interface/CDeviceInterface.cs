using HLDevice.Abstract;
using SVI_NFT_R;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace HLDevice
{
    public class CDeviceInterface
    {
        public CDeviceInterfaceAbstract.CInitializeParameter GetInitializeParameter => mInitializeParameter;
        private CDeviceInterfaceAbstract m_objInterface;
        private CDocument m_objDocument;
        private CDeviceInterfaceAbstract.CInitializeParameter mInitializeParameter;

        /// <summary>
        /// 에러타입
        /// </summary>
        public enum EError
        {
            ERROR_INTERFACE,
            ERROR_NONE,
            ERROR_INTERFACE_NAME,
            ERROR_LICENSE,
            ERROR_FINAL
        };

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public bool HLInitialize(CDocument document, CConfig.CInterfaceParameter objCCLinkParameter)
        {
            m_objDocument = document;
            if (
                CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode
                || objCCLinkParameter.bRunSimulationMode == true
                )
            {
                m_objInterface = new Interface.CDeviceInterfaceCCLinkIEVirtual();
            }
            switch (objCCLinkParameter.strInterfaceChannel)
            {
                case "81":
                case "82":
                case "83":
                case "84":
                    // 로봇 통신용 인터페이스용
                    if (
                        CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode
                        || objCCLinkParameter.bRunSimulationMode == true
                        )
                    {
                        m_objInterface = new Interface.CDeviceInterfaceCCLinkVer2Virtual();
                    }
                    else
                    {
                        m_objInterface = new Interface.CDeviceInterfaceCCLinkVer2();
                    }
                    break;
                case "151":
                case "152":
                case "153":
                case "154":
                    // 설비간 인터페이스용
                    if (
                        CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode
                        || objCCLinkParameter.bRunSimulationMode == true
                        )
                    {
                        m_objInterface = new Interface.CDeviceInterfaceCCLinkIEVirtual();
                    }
                    else
                    {
                        m_objInterface = new Interface.CDeviceInterfaceCCLinkIE();
                    }
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            parseInitializeParameter(objCCLinkParameter);

            return m_objInterface.HLInitialize(mInitializeParameter);
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void HLDeInitialize()
        {
            m_objInterface.HLDeInitialize();
        }

        public bool HLInitializeChannelShare(CDocument document, CConfig.CInterfaceParameter objCCLinkParameter, CConfig.CInterfaceParameter hostParameter)
        {
            m_objDocument = document;
            if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                m_objInterface = new Interface.CDeviceInterfaceCCLinkIEVirtual();
            }
            else
            {
                m_objInterface = new Interface.CDeviceInterfaceCCLinkIE();
            }
            parseInitializeParameter(objCCLinkParameter, hostParameter);

            return m_objInterface.HLInitializeChannelShare(mInitializeParameter);
        }

        public void HLDeInitializeChannelShare()
        {
            m_objInterface.HLDeInitializeChannelShare();
        }

        /// <summary>
        /// 버전
        /// </summary>
        /// <returns></returns>
        public string HLGetVersion()
        {
            return m_objInterface.HLGetVersion();
        }

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public bool HLIsConnected()
        {
            return m_objInterface.HLIsConnected();
        }

        /// <summary>
        /// BIT영역 Write
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public EError HLSetInterfaceBit(string strName, bool bData)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLSetInterfaceBit(strName, bData))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        /// <summary>
        /// WORD영역 Write - WORD 영역을 BIT단위 Write
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bValue"></param>
        /// <param name="iIndex"></param>
        /// <returns></returns>
        public EError HLSetInterfaceValue(string strName, bool bValue, int iIndex)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLSetInterfaceValue(strName, bValue, iIndex))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        /// <summary>
        /// WORD영역 Write - WORD 영역을 BIT단위 Write
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="pValue"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public EError HLSetInterfaceValue(string strName, bool[] pValue, int iCount = 16)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLSetInterfaceValue(strName, pValue, iCount))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        /// <summary>
        /// WORD영역 Write - int
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public EError HLSetInterfaceValue(string strName, int iValue)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLSetInterfaceValue(strName, iValue))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        /// <summary>
        /// WORD영역 Write - double
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public EError HLSetInterfaceValue(string strName, double dValue)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLSetInterfaceValue(strName, dValue))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        /// <summary>
        /// WORD영역 Write - string
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public EError HLSetInterfaceValue(string strName, string strValue)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLSetInterfaceValue(strName, strValue))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }


        /// <summary>
        /// BIT영역 Read
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public EError HLGetInterfaceBit(string strName, out bool bData)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLGetInterfaceBit(strName, out bData))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        /// <summary>
        /// WORD영역 Write - WORD 영역을 BIT단위 Read
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bData"></param>
        /// <param name="iIndex"></param>
        /// <returns></returns>
        public EError HLGetInterfaceValue(string strName, out bool bData, int iIndex)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out bData, iIndex))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        /// <summary>
        /// WORD영역 Write - WORD 영역을 BIT단위 Read
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="pValue"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public EError HLGetInterfaceValue(string strName, out bool[] pValue, out int iCount)
        {
            iCount = 16;
            pValue = new bool[iCount];
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out pValue, out iCount))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        public EError HLSetInterfaceValueEx(string strName, bool[] pValue)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLSetInterfaceValueEx(strName, pValue))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        public EError HLSetInterfaceValueEx(string strName, int[] pValue)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLSetInterfaceValueEx(strName, pValue))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        public EError HLSetInterfaceValueEx(string strName, int pValue, int wordCount)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLSetInterfaceValueEx(strName, pValue, wordCount))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        public EError HLGetInterfaceValueEx(string strName, out bool[] pValue)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLGetInterfaceValueEx(strName, out pValue))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        public EError HLGetInterfaceValueEx(string strName, out int[] pValue)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLGetInterfaceValueEx(strName, out pValue))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        public EError HLGetInterfaceValueEx(string strName, out int pValue, int wordCount)
        {
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLGetInterfaceValueEx(strName, out pValue, wordCount))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        /// <summary>
        /// WORD영역 Read - int
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public EError HLGetInterfaceValue(string strName, out int iValue)
        {
            iValue = 0;
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out iValue))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        /// <summary>
        /// WORD영역 Read - double
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public EError HLGetInterfaceValue(string strName, out double dValue)
        {
            dValue = 0;
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out dValue))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        /// <summary>
        /// WORD영역 Read - string
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public EError HLGetInterfaceValue(string strName, out string strValue)
        {
            strValue = "";
            EError eError = EError.ERROR_INTERFACE;
            do
            {
                if (false == m_objInterface.HLGetInterfaceValue(strName, out strValue))
                {
                    eError = (EError)m_objInterface.HLGetErrorCode().iReturnCode;
                    break;
                }

                eError = EError.ERROR_NONE;
            } while (false);

            return eError;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public CDeviceInterfaceAbstract.CInterfaceError HLGetErrorCode()
        {
            return m_objInterface.HLGetErrorCode();
        }

        public bool SetInterfaceBit(string strAddressName, bool setSignal, CDefine.ELogType logType, bool logEnable = true, string commentMessage = "", [CallerMemberName] string callerMemberName = "")
        {
            bool bReturn = false;
            var sbLogMssage = new StringBuilder();
            sbLogMssage.Append(callerMemberName);
            sbLogMssage.Append(" -> ");
            sbLogMssage.Append("SetInterfaceBit");
            sbLogMssage.Append(" : ");
            sbLogMssage.Append(strAddressName);
            sbLogMssage.Append(" : ");
            string signalName = setSignal == true ? "ON" : "OFF";
            sbLogMssage.Append(signalName);
            if (false == string.IsNullOrEmpty(commentMessage))
            {
                sbLogMssage.Append(" // ");
                sbLogMssage.Append(commentMessage);
            }
            if (true == logEnable)
            {
                m_objDocument.SetUpdateLog(logType, string.Format(sbLogMssage.ToString()));
            }

            EError functionResult = EError.ERROR_NONE;
            do
            {
                functionResult = HLSetInterfaceBit(strAddressName, setSignal);
                if (EError.ERROR_NONE != functionResult)
                {
                    break;
                }

                bReturn = true;
            } while (false);

            if (false == bReturn)
            {
                if (true == logEnable)
                {
                    m_objDocument.SetUpdateLog(logType, string.Format("[FAILED] {0}", sbLogMssage.ToString()));
                }
                if (EError.ERROR_LICENSE == functionResult)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.LISENCE_KEY_RECOGNITION_FAILED);
                }
            }
            return bReturn;
        }

        private void parseInitializeParameter(CConfig.CInterfaceParameter objCCLinkParameter, CConfig.CInterfaceParameter hostParameter = null)
        {
            mInitializeParameter = new CDeviceInterfaceAbstract.CInitializeParameter();
            mInitializeParameter.strInterfaceStationNumber = "255";
            if (hostParameter == null)
            {
                mInitializeParameter.strInterfaceChannel = objCCLinkParameter.strInterfaceChannel;
                mInitializeParameter.strInterfaceNetworkNumber = objCCLinkParameter.strInterfaceNetworkNumber;
            }
            else
            {
                mInitializeParameter.strInterfaceChannel = hostParameter.strInterfaceChannel;
                mInitializeParameter.strInterfaceNetworkNumber = hostParameter.strInterfaceNetworkNumber;
            }
            foreach (KeyValuePair<string, CConfig.CInterfaceParameter.CInterfaceDataParameter> item in objCCLinkParameter.objInterfaceParameterDigital)
            {
                CDeviceInterfaceAbstract.CInitializeParameter.CInterfaceParameter objCCLinkData = new CDeviceInterfaceAbstract.CInitializeParameter.CInterfaceParameter();
                objCCLinkData.eDataType = (CDeviceInterfaceAbstract.CInitializeParameter.EDataType)item.Value.eDataType;
                objCCLinkData.iDataAddress = item.Value.iDataAddress;
                objCCLinkData.iDataSize = item.Value.iDataSize;
                objCCLinkData.strDataAddress = item.Value.strDataAddress;
                objCCLinkData.strDataName = item.Value.strDataName;
                objCCLinkData.strOffsetName = item.Value.strOffsetName;
                objCCLinkData.iStartOffset = item.Value.iStartOffset;

                if (mInitializeParameter.objInterfaceParameterDigital.ContainsKey(item.Value.strDataName))
                {
                    throw new System.Exception($"CCLink_Digital \nName[{item.Value.strDataName}]. Duplicated");
                }
                mInitializeParameter.objInterfaceParameterDigital.Add(item.Value.strDataName, objCCLinkData);
            }
            foreach (KeyValuePair<string, CConfig.CInterfaceParameter.CInterfaceDataParameter> item in objCCLinkParameter.objInterfaceParameterAnalog)
            {
                CDeviceInterfaceAbstract.CInitializeParameter.CInterfaceParameter objCCLinkData = new CDeviceInterfaceAbstract.CInitializeParameter.CInterfaceParameter();
                objCCLinkData.eDataType = (CDeviceInterfaceAbstract.CInitializeParameter.EDataType)item.Value.eDataType;
                objCCLinkData.iDataAddress = item.Value.iDataAddress;
                objCCLinkData.iDataSize = item.Value.iDataSize;
                objCCLinkData.strDataAddress = item.Value.strDataAddress;
                objCCLinkData.strDataName = item.Value.strDataName;
                objCCLinkData.strOffsetName = item.Value.strOffsetName;
                objCCLinkData.iStartOffset = item.Value.iStartOffset;

                if (mInitializeParameter.objInterfaceParameterDigital.ContainsKey(item.Value.strDataName))
                {
                    throw new System.Exception($"CCLink_Analog \nName[{item.Value.strDataName}]. Duplicated");

                }
                mInitializeParameter.objInterfaceParameterAnalog.Add(objCCLinkData.strDataName, objCCLinkData);
            }
        }
    }

}
