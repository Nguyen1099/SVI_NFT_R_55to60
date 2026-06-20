using HLDevice.Abstract;
using System.Collections.Generic;

namespace HLDevice.IO
{
    public class CDeviceIOAjin : CDeviceIOAbstract
    {
        /// <summary>
        /// IO 초기화 객체 선언
        /// </summary>
        private HLDeviceDLL.IO.Ajin.CDeviceIOAjinDefine.CInitializeParameter m_objIOInitializeParameter;

        /// <summary>
        /// IO 객체 선언
        /// </summary>
        public HLDeviceDLL.IO.Ajin.CDeviceIOAjin m_objIOAjin;
        /// <summary>IO 
        /// 알람 객체 선언
        /// </summary>
        public HLDeviceDLL.IO.Ajin.CDeviceIOAjinDefine.CIOError m_objIOError;

        /// <summary>
        /// 초기화 파라미터
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public override bool HLInitialize(CDeviceIOAbstract.CIOInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {
                // IO 알람 객체 생성
                m_objIOError = new HLDeviceDLL.IO.Ajin.CDeviceIOAjinDefine.CIOError();
                // IO 초기화 객체 생성
                m_objIOInitializeParameter = new HLDeviceDLL.IO.Ajin.CDeviceIOAjinDefine.CInitializeParameter();
                m_objIOInitializeParameter.objIOParameter = new Dictionary<string, HLDeviceDLL.IO.Ajin.CDeviceIOAjinDefine.CIOParameter>();
                m_objIOInitializeParameter.iInputModuleCount = objInitializeParameter.iInputModuleCount;
                m_objIOInitializeParameter.iOutPutModuleCount = objInitializeParameter.iOutPutModuleCount;

                foreach (KeyValuePair<string, CIOParameter> pair in objInitializeParameter.objIOParameter)
                {
                    HLDeviceDLL.IO.Ajin.CDeviceIOAjinDefine.CIOParameter objIOParameter = new HLDeviceDLL.IO.Ajin.CDeviceIOAjinDefine.CIOParameter();
                    objIOParameter.strIndex = pair.Value.strIndex;
                    objIOParameter.eIOType = (HLDeviceDLL.IO.Ajin.CDeviceIOAjinDefine.enumIOType)pair.Value.eIOType;
                    objIOParameter.strAddress = pair.Value.strAddress;
                    objIOParameter.strIOName = pair.Value.strIOName;
                    objIOParameter.nModuleIndex = pair.Value.nModuleIndex;
                    m_objIOInitializeParameter.objIOParameter.Add(objIOParameter.strIOName, objIOParameter);
                }

                // IO 객체 생성 및 초기화
                m_objIOAjin = new HLDeviceDLL.IO.Ajin.CDeviceIOAjin();
                if (false == m_objIOAjin.HLInitialize(m_objIOInitializeParameter))
                {
                    m_objIOAjin.HLGetErrorCode();
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
            m_objIOAjin.HLDeInitialize();
        }

        /// <summary>
        /// 버전 정보
        /// </summary>
        /// <returns></returns>
        public override string HLGetVersion()
        {
            return m_objIOAjin.GetVersion();
        }

        /// <summary>
        /// DI, DO 입력 신호를 Bit로 읽어온다.
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public override bool HLGetDigitalBit(string strIOName, ref bool bResult)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objIOAjin.HLGetDigitalBit(strIOName, ref bResult))
                {
                    m_objIOAjin.HLGetErrorCode();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// DO 출력 신호 Bit 입력
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public override bool HLSetDigitalBit(string strIOName, bool bResult)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objIOAjin.HLSetDigitalBit(strIOName, bResult))
                {
                    m_objIOAjin.HLGetErrorCode();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// DI, DO 입력 신호를 Byte로 읽어온다,
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public override bool HLGetDigitalByte(string strIOName, ref bool[] bResult)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objIOAjin.HLGetDigitalByte(strIOName, ref bResult))
                {
                    m_objIOAjin.HLGetErrorCode();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// DI, DO 입력 신호를 Byte로 입력
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public override bool HLSetDigitalByte(string strIOName, bool[] bResult)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objIOAjin.HLSetDigitalByte(strIOName, bResult))
                {
                    m_objIOAjin.HLGetErrorCode();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// DI, DO 입력 신호를 Word로 읽어온다,
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public override bool HLGetDigitalWord(string strIOName, ref bool[] bResult)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objIOAjin.HLGetDigitalWord(strIOName, ref bResult))
                {
                    m_objIOAjin.HLGetErrorCode();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// DI, DO 입력 신호를 Word로 입력
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public override bool HLSetDigitalWord(string strIOName, bool[] bResult)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objIOAjin.HLSetDigitalWord(strIOName, bResult))
                {
                    m_objIOAjin.HLGetErrorCode();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// DI, DO 입력 신호를 DoubleWord로 읽어온다,
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public override bool HLGetDigitalDoubleWord(string strIOName, ref bool[] bResult)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objIOAjin.HLGetDigitalDoubleWord(strIOName, ref bResult))
                {
                    m_objIOAjin.HLGetErrorCode();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// DI, DO 입력 신호를 DoubleWord로 입력
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public override bool HLSetDigitalDoubleWord(string strIOName, bool[] bResult)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objIOAjin.HLSetDigitalDoubleWord(strIOName, bResult))
                {
                    m_objIOAjin.HLGetErrorCode();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// AIO 지정한 입력 채널의 아날로그 입력값을 전압으로 반환 한다.
        /// </summary>
        /// <param name="strChannelName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public override bool HLGetAnalog(string strChannelName, ref double dValue)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objIOAjin.HLGetAnalog(strChannelName, ref dValue))
                {
                    m_objIOAjin.HLGetErrorCode();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// AIO 지정한 입력 채널의 아날로그 입력값을 전압으로 반환 한다.
        /// </summary>
        /// <param name="strChannelName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public override bool HLSetAnalog(string strChannelName, double dValue)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objIOAjin.HLSetAnalog(strChannelName, dValue))
                {
                    m_objIOAjin.HLGetErrorCode();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 알람 객체를 반환 한다.
        /// </summary>
        /// <returns></returns>
        public override object HLGetErrorCode()
        {
            return m_objIOAjin.HLGetErrorCode();
        }

    }
}
