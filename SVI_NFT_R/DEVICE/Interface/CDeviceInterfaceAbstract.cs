using System;
using System.Collections.Generic;

namespace HLDevice.Abstract
{
    public abstract class CDeviceInterfaceAbstract
    {


        /// <summary>
        /// CCLink 초기화 파라미터 객체
        /// </summary>
        public class CInitializeParameter : ICloneable
        {
            /// <summary>
            /// 사용되는 데이터 타입
            /// </summary>
            public enum EDataType
            {
                DATA_TYPE_BIT_IN = 0,
                DATA_TYPE_BIT_OUT,
                DATA_TYPE_BINARY_IN,
                DATA_TYPE_BINARY_OUT,
                DATA_TYPE_INT_IN,
                DATA_TYPE_INT_OUT,
                DATA_TYPE_DOUBLE_IN,
                DATA_TYPE_DOUBLE_OUT,
                DATA_TYPE_BCD_IN,
                DATA_TYPE_BCD_OUT,
                DATA_TYPE_ASCII_IN,
                DATA_TYPE_ASCII_OUT,
                DATA_TYPE_UNKWON,
                DATA_TYPE_FINAL
            };

            /// <summary>
            /// CCLink 운용 파라미터 객체
            /// </summary>
            public class CInterfaceParameter
            {
                /// <summary>
                /// 주소
                /// </summary>
                public string strDataAddress;
                /// <summary>
                /// 주소
                /// </summary>
                public int iDataAddress;
                /// <summary>
                /// 데이터 길이
                /// </summary>
                public uint iDataSize;
                /// <summary>
                /// 접근 이름
                /// </summary>
                public string strDataName;
                /// <summary>
                /// 데이터 타입
                /// </summary>
                public EDataType eDataType;
                /// <summary>
                /// 나찌 로봇 옵셋 기준 표기 이름
                /// </summary>
                public string strOffsetName;
                /// <summary>
                /// 데이터 시작 옵셋
                /// </summary>
                public uint iStartOffset;
            }

            public string strInterfaceChannel;
            public string strInterfaceNetworkNumber;
            public string strInterfaceStationNumber;

            public Dictionary<string, CInterfaceParameter> objInterfaceParameterDigital = new Dictionary<string, CInterfaceParameter>();
            public Dictionary<string, CInterfaceParameter> objInterfaceParameterAnalog = new Dictionary<string, CInterfaceParameter>();
            public object Clone()
            {
                CInitializeParameter objInitializeParameter = new CInitializeParameter();
                objInitializeParameter.strInterfaceChannel = strInterfaceChannel;
                objInitializeParameter.strInterfaceNetworkNumber = strInterfaceNetworkNumber;
                objInitializeParameter.strInterfaceStationNumber = strInterfaceStationNumber;
                objInitializeParameter.objInterfaceParameterDigital = new Dictionary<string, CInterfaceParameter>(objInterfaceParameterDigital);
                objInitializeParameter.objInterfaceParameterAnalog = new Dictionary<string, CInterfaceParameter>(objInterfaceParameterAnalog);

                return objInitializeParameter;
            }
        }

        /// <summary>
        /// 알람 발생 확인 클래스
        /// </summary>
        public class CInterfaceError : ICloneable
        {
            /// <summary>
            /// 이벤트 발생 시간
            /// </summary>
            public string strEventTime;
            /// <summary>
            /// 수행된 함수 이름
            /// </summary>
            public string strFunctionName;
            /// <summary>
            /// 알람 리턴 결과
            /// </summary>
            public int iReturnCode;
            /// <summary>
            /// 알람 메세지
            /// </summary>
            public string strMessage;

            public object Clone()
            {
                CInterfaceError objError = new CInterfaceError();
                objError.strEventTime = strEventTime;
                objError.strFunctionName = strFunctionName;
                objError.iReturnCode = iReturnCode;
                objError.strMessage = strMessage;

                return objError;
            }
        }

        /// <summary>
        /// 초기화 추상객체
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public abstract bool HLInitialize(CInitializeParameter objInitializeParameter);

        /// <summary>
        /// 해제 추상객체
        /// </summary>
        public abstract void HLDeInitialize();

        public virtual bool HLInitializeChannelShare(CInitializeParameter objInitializeParameter)
        {
            throw new NotImplementedException();
        }
        public virtual void HLDeInitializeChannelShare()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 버전 추상객체
        /// </summary>
        /// <returns></returns>
        public abstract string HLGetVersion();

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public virtual bool HLIsConnected()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// BIT영역 Write
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public virtual bool HLSetInterfaceBit(string strName, bool bData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WORD영역 Write - WORD 영역을 BIT단위 Write
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bValue"></param>
        /// <param name="iIndex"></param>
        /// <returns></returns>
        public virtual bool HLSetInterfaceValue(string strName, bool bValue, int iIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WORD영역 Write - WORD 영역을 BIT단위 Write
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="pValue"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public virtual bool HLSetInterfaceValue(string strName, bool[] pValue, int iCount = 16)
        {
            throw new NotImplementedException();
        }
        public virtual bool HLSetInterfaceValueEx(string strName, bool[] pValue)
        {
            throw new NotImplementedException();
        }
        public virtual bool HLSetInterfaceValueEx(string name, int[] values)
        {
            throw new NotImplementedException();
        }
        public virtual bool HLSetInterfaceValueEx(string name, int values, int wordCount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WORD영역 Write - int
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public virtual bool HLSetInterfaceValue(string strName, int iValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WORD영역 Write - double
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public virtual bool HLSetInterfaceValue(string strName, double dValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WORD영역 Write - string
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public virtual bool HLSetInterfaceValue(string strName, string strValue)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// BIT영역 Read
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public virtual bool HLGetInterfaceBit(string strName, out bool bData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WORD영역 Write - WORD 영역을 BIT단위 Read
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="bData"></param>
        /// <param name="iIndex"></param>
        /// <returns></returns>
        public virtual bool HLGetInterfaceValue(string strName, out bool bData, int iIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WORD영역 Write - WORD 영역을 BIT단위 Read
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="pValue"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public virtual bool HLGetInterfaceValue(string strName, out bool[] pValue, out int iCount)
        {
            throw new NotImplementedException();
        }
        public virtual bool HLGetInterfaceValueEx(string strName, out bool[] pValue)
        {
            throw new NotImplementedException();
        }
        public virtual bool HLGetInterfaceValueEx(string name, out int[] values)
        {
            throw new NotImplementedException();
        }
        public virtual bool HLGetInterfaceValueEx(string name, out int value, int wordCount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WORD영역 Read - int
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public virtual bool HLGetInterfaceValue(string strName, out int iValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WORD영역 Read - double
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public virtual bool HLGetInterfaceValue(string strName, out double dValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WORD영역 Read - string
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public virtual bool HLGetInterfaceValue(string strName, out string strValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public virtual CInterfaceError HLGetErrorCode()
        {
            CInterfaceError objError = new CInterfaceError();
            return objError;
        }
    }

}
