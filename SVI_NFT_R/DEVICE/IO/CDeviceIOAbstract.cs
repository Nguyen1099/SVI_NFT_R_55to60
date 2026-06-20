using System;
using System.Collections.Generic;

namespace HLDevice.Abstract
{
    public abstract class CDeviceIOAbstract
    {
        /// <summary>
        /// IO 타입 정의
        /// </summary>
        public enum EIOType
        {
            IO_TYPE_DI = 0,
            IO_TYPE_DO,
            IO_TYPE_AI,
            IO_TYPE_AO,
            IO_TYPE_FINAL
        };

        /// <summary>
        /// 초기화 파라미터
        /// </summary>
        public class CIOParameter : ICloneable
        {
            public EIOType eIOType;
            public string strAddress;
            public string strIOName;
            public string strIndex;
            public int nModuleIndex;

            public object Clone()
            {
                CIOParameter objIOParameter = new CIOParameter();
                objIOParameter.eIOType = eIOType;
                objIOParameter.strAddress = strAddress;
                objIOParameter.strIOName = strIOName;
                objIOParameter.strIndex = strIndex;
                objIOParameter.nModuleIndex = nModuleIndex;

                return objIOParameter;
            }
        }

        public class CIOInitializeParameter : ICloneable
        {
            /// <summary>
            /// Input IO 모듈 갯수
            /// </summary>
            public int iInputModuleCount;
            /// <summary>
            /// Output IO 모듈 갯수
            /// </summary>
            public int iOutPutModuleCount;
            /// <summary>
            /// 개별 IO 정보 map
            /// </summary>
            public Dictionary<string, CIOParameter> objIOParameter;

            public object Clone()
            {
                CIOInitializeParameter objInitializeParameter = new CIOInitializeParameter();
                objInitializeParameter.objIOParameter = new Dictionary<string, CIOParameter>(objIOParameter);
                return objInitializeParameter;
            }
        }

        /// <summary>
        /// 초기화 추상 함수
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public abstract bool HLInitialize(CIOInitializeParameter objInitializeParameter);

        /// <summary>
        /// 해제 추상 함수
        /// </summary>
        public abstract void HLDeInitialize();

        /// <summary>
        /// 버전 정보 추상 함수
        /// </summary>
        /// <returns></returns>
        public abstract string HLGetVersion();

        /// <summary>
        /// DI, DO 입력 신호를 Bit로 읽어온다.
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public abstract bool HLGetDigitalBit(string strIOName, ref bool bResult);

        /// <summary>
        /// DO 출력 신호 Bit 입력
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public abstract bool HLSetDigitalBit(string strIOName, bool bResult);

        /// <summary>
        /// DI, DO 입력 신호를 Byte로 읽어온다,
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public abstract bool HLGetDigitalByte(string strIOName, ref bool[] bResult);

        /// <summary>
        /// DI, DO 입력 신호를 Byte로 입력
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public abstract bool HLSetDigitalByte(string strIOName, bool[] bResult);

        /// <summary>
        /// DI, DO 입력 신호를 Word로 읽어온다,
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public abstract bool HLGetDigitalWord(string strIOName, ref bool[] bResult);

        /// <summary>
        /// DI, DO 입력 신호를 Word로 입력
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public abstract bool HLSetDigitalWord(string strIOName, bool[] bResult);

        /// <summary>
        /// DI, DO 입력 신호를 DoubleWord로 읽어온다,
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public abstract bool HLGetDigitalDoubleWord(string strIOName, ref bool[] bResult);

        /// <summary>
        /// DI, DO 입력 신호를 DoubleWord로 입력
        /// </summary>
        /// <param name="strIOName"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        public abstract bool HLSetDigitalDoubleWord(string strIOName, bool[] bResult);

        /// <summary>
        /// AIO 지정한 입력 채널의 아날로그 입력값을 전압으로 반환 한다.
        /// </summary>
        /// <param name="strChannelName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public abstract bool HLGetAnalog(string strChannelName, ref double dValue);

        /// <summary>
        /// AIO 지정한 입력 채널의 아날로그 입력값을 전압으로 반환 한다.
        /// </summary>
        /// <param name="strChannelName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public abstract bool HLSetAnalog(string strChannelName, double dValue);

        /// <summary>
        /// 알람 객체를 반환 한다.
        /// </summary>
        /// <returns></returns>
        public abstract object HLGetErrorCode();
    }
}
