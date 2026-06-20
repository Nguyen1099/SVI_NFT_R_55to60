using System;
using System.Collections.Generic;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// [공용] 인터페이스 파라미터 ( CCLinkIE, CCLinkVer2 )
        /// </summary>
        [Serializable]
        public class CInterfaceParameter
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
            /// 멜섹 운용 파라미터 객체
            /// </summary>
            [Serializable]
            public class CInterfaceDataParameter
            {
                /// <summary>
                /// 인덱스
                /// </summary>
                public string strIndex;

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
                public uint iStartOffset = 0;
            }

            public string strInterfaceChannel;
            public string strInterfaceNetworkNumber;
            public bool bRunSimulationMode;

            public Dictionary<string, CInterfaceDataParameter> objInterfaceParameterDigital = new Dictionary<string, CInterfaceDataParameter>();
            public Dictionary<string, CInterfaceDataParameter> objInterfaceParameterAnalog = new Dictionary<string, CInterfaceDataParameter>();
        }
    }
}