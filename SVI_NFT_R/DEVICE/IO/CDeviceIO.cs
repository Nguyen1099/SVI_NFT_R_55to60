using HLDevice.Abstract;
using SVI_NFT_R;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLDevice
{
    public class CDeviceIO
    {
        private CDeviceIOAbstract m_objIO;
        private CDocument m_objDocument;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public bool HLInitialize(CDocument document, CConfig.CIOInitializeParameter objConfigIOinitializeParameter)
        {
            bool bReturn = false;

            do
            {
                m_objDocument = document;

                if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
                {
                    m_objIO = new IO.CDeviceIOVirtual();
                }
                else
                {
                    m_objIO = new IO.CDeviceIOAjin();
                }

                // IO 객체 생성
                CDeviceIOAbstract.CIOInitializeParameter objInitializeParameter = new CDeviceIOAbstract.CIOInitializeParameter();
                objInitializeParameter.objIOParameter = new Dictionary<string, CDeviceIOAbstract.CIOParameter>();
                objInitializeParameter.iInputModuleCount = objConfigIOinitializeParameter.iInputModuleCount;
                objInitializeParameter.iOutPutModuleCount = objConfigIOinitializeParameter.iOutPutModuleCount;
                foreach (KeyValuePair<string, CConfig.CIOInitializeParameter.CIOParameter> pair in objConfigIOinitializeParameter.objIOParameter)
                {
                    CDeviceIOAbstract.CIOParameter objIOParameter = new CDeviceIOAbstract.CIOParameter();
                    objIOParameter.strIndex = pair.Value.strIndex;
                    objIOParameter.eIOType = (CDeviceIOAbstract.EIOType)pair.Value.eIOType;
                    objIOParameter.strAddress = pair.Value.strAddress;
                    objIOParameter.strIOName = pair.Value.strIOName;
                    objIOParameter.nModuleIndex = pair.Value.nModuleIndex;

                    if (objInitializeParameter.objIOParameter.ContainsKey(objIOParameter.strIOName) == false)
                    {
                        objInitializeParameter.objIOParameter.Add(objIOParameter.strIOName, objIOParameter);
                    }
                }

                // IO List Validation
                checkIoListValidation(objConfigIOinitializeParameter);

                // IO 객체 초기화
                if (false == m_objIO.HLInitialize(objInitializeParameter))
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void HLDeInitialize()
        {
            m_objIO.HLDeInitialize();
        }

        /// <summary>
        /// 버전 정보
        /// </summary>
        /// <returns></returns>
        public string HLGetVersion()
        {
            return m_objIO.HLGetVersion();
        }

        /// <summary>
        /// DI, DO 입력 신호를 Bit로 읽어온다.
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public bool HLGetDigitalBit(string strIOName, ref bool bResult)
        {
            return m_objIO.HLGetDigitalBit(strIOName, ref bResult);
        }

        public bool HLGetDigitalBit(CDeviceIODefine.EDigitalInput eIOName, ref bool bResult)
        {
            return m_objIO.HLGetDigitalBit(eIOName.ToString(), ref bResult);
        }

        public bool HLGetDigitalBit(CDeviceIODefine.EDigitalOutput eIOName, ref bool bResult)
        {
            return m_objIO.HLGetDigitalBit(eIOName.ToString(), ref bResult);
        }

        /// <summary>
        /// DIO 출력 신호 Bit 입력
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public bool HLSetDigitalBit(string strIOName, bool bResult)
        {
            return m_objIO.HLSetDigitalBit(strIOName, bResult);
        }

        /// <summary>
        /// DO 출력 신호 Bit 입력
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public bool HLSetDigitalBit(CDeviceIODefine.EDigitalOutput eIOName, bool bResult)
        {
            return m_objIO.HLSetDigitalBit(eIOName.ToString(), bResult);
        }

        /// <summary>
        /// DI 출력 신호 Bit 입력
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public bool HLSetDigitalBit(CDeviceIODefine.EDigitalInput eIOName, bool bResult)
        {
            return m_objIO.HLSetDigitalBit(eIOName.ToString(), bResult);
        }

        /// <summary>
        /// AIO 지정한 입력 채널의 아날로그 입력값을 전압으로 반환 한다.
        /// </summary>
        /// <param name="strChannelName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public bool HLGetAnalog(CDeviceIODefine.EAnalogInput eIOName, ref double dValue)
        {
            return m_objIO.HLGetAnalog(eIOName.ToString(), ref dValue);
        }
        public bool HLGetAnalog(string strIOName, ref double dValue)
        {
            return m_objIO.HLGetAnalog(strIOName, ref dValue);
        }
        /// <summary>
        /// AIO 지정한 입력 채널의 아날로그 입력값을 전압으로 반환 한다.
        /// </summary>
        /// <param name="strChannelName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public bool HLSetAnalog(string strChannelName, double dValue)
        {
            return m_objIO.HLSetAnalog(strChannelName, dValue);
        }

        /// <summary>
        /// 알람 객체를 반환 한다.
        /// </summary>
        /// <returns></returns>
        public object HLGetErrorCode()
        {
            return m_objIO.HLGetErrorCode();
        }

        /// <summary>
        /// 추상화 객체 반환
        /// </summary>
        /// <returns></returns>
        public CDeviceIOAbstract HLGetAbstractReference()
        {
            return m_objIO;
        }

        /// <summary>
        /// 프로그램 최초 구동시 소스코드에 등록된 IO 리스트와 파일에 등록된 IO 리스트를 비교하여 누락된 항목이 있으면 예외를 발생 시킴
        /// </summary>
        /// <param name="objIoInitializeParameter"></param>
        /// <exception cref="Exception">소스코드에 등록된 IO 리스트와 파일에 등록된 IO 리스트를 비교하여 누락된 항목이 있을 때 발생하는 예외</exception>
        private void checkIoListValidation(CConfig.CIOInitializeParameter objIoInitializeParameter)
        {
            IEnumerable<string> allIoNames = Enum.GetNames(typeof(CDeviceIODefine.EDigitalInput))
                .Concat(Enum.GetNames(typeof(CDeviceIODefine.EAnalogInput)))
                .Concat(Enum.GetNames(typeof(CDeviceIODefine.EDigitalOutput)));
            HashSet<string> enumIoNames = new HashSet<string>();
            foreach (string ioName in allIoNames)
            {
                enumIoNames.Add(ioName);
            }
            HashSet<string> notFoundIoNames = new HashSet<string>();
            foreach (KeyValuePair<string, CConfig.CIOInitializeParameter.CIOParameter> pair in objIoInitializeParameter.objIOParameter)
            {
                if (true == enumIoNames.Contains(pair.Value.strIOName))
                {
                    enumIoNames.Remove(pair.Value.strIOName);
                }
                else
                {
                    notFoundIoNames.Add(pair.Value.strIOName);
                }
            }

            if (
                0 != enumIoNames.Count
                || 0 < notFoundIoNames.Count
                )
            {
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.Append(" IO list validation failed. please check the 'IO.dat' file.");
                if (0 != enumIoNames.Count)
                {
                    errorMessage
                        .AppendLine()
                        .AppendLine()
                        .Append("'IO.dat' 파일에서 누락된 항목 [")
                        .AppendLine()
                        .Append(string.Join(", ", enumIoNames.ToArray()))
                        .Append("]");
                }
                if (0 < notFoundIoNames.Count)
                {
                    errorMessage
                        .AppendLine()
                        .AppendLine()
                        .Append("'CDeviceIODefine'에 등록되지 않은 항목 [")
                        .AppendLine()
                        .Append(string.Join(", ", notFoundIoNames.ToArray()))
                        .Append("]");
                }
                errorMessage.AppendLine();
                throw new Exception(errorMessage.ToString());
            }
        }
    }
}
