using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// IO 초기화 객체
        /// </summary>
        [Serializable]
        public class CIOInitializeParameter
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
            /// IO 파라미터 정의
            /// </summary>
            [Serializable]
            public class CIOParameter
            {
                public EIOType eIOType;
                public string strAddress;
                public string strIOName;
                public string strIndex;
                public int nModuleIndex;
            }

            /// <summary>
            /// Input IO 모듈 갯수
            /// </summary>
            public int iInputModuleCount;

            /// <summary>
            /// Output IO 모듈 갯수
            /// </summary>
            public int iOutPutModuleCount;

            /// <summary>
            /// Analog Input 모듈 갯수
            /// </summary>
            public int iAnalogInputModuleCount;

            /// <summary>
            /// Analog Ouput 모듈 갯수
            /// </summary>
            public int iAnalogOutputModuleCount;

            /// <summary>
            /// 개벌 IO 정보 map
            /// </summary>
            public Dictionary<string, CIOParameter> objIOParameter = new Dictionary<string, CIOParameter>();
        }

        /// <summary>
        /// IO 경로 리턴
        /// </summary>
        /// <returns></returns>
        public CIOInitializeParameter GetIOParameter() => m_objIOInitializeParameter;

        /// <summary>
        /// IO 초기화 파라미터 선언
        /// </summary>
        private readonly CIOInitializeParameter m_objIOInitializeParameter = new CIOInitializeParameter();

        /// <summary>
        /// IO Map 데이터를 읽어온다.
        /// </summary>
        /// <returns></returns>
        public bool LoadIOParameter()
        {
            bool bReturn = false;
            do
            {
                // IO MapData 정보 로드
                string strPath = string.Format(@"{0}\{1}", GetIniPath(), CDefine.DEF_IO_DAT);
                if (false == File.Exists(strPath))
                    break;

                int nDIO_X_Count = 0;
                int nAIO_X_Count = 0;
                int nDIO_Y_Count = 0;

                int nDIO_X_Page = 0;
                int nAIO_X_Page = 0;
                int nDIO_Y_Page = 0;
                FileStream fs = File.Open(strPath, FileMode.Open);
                StreamReader sr = new StreamReader(fs, Encoding.Default);

                while (!sr.EndOfStream)
                {
                    string strData = sr.ReadLine();
                    strData = strData.Replace("\"", "");
                    strData = strData.Replace(" ", "");
                    strData.Trim();
                    strData = strData.Replace("\t", "");

                    string[] strIOParameter = strData.Split(',');
                    if (strData.Length < 1)
                        continue;

                    CIOInitializeParameter.CIOParameter IOParameter = new CIOInitializeParameter.CIOParameter();
                    IOParameter.strIndex = strIOParameter[0];
                    IOParameter.strAddress = strIOParameter[1];
                    if (strIOParameter[2].Length == 0)
                    {
                        IOParameter.strIOName = strIOParameter[0];
                    }
                    else
                    {
                        IOParameter.strIOName = strIOParameter[2];
                    }
                    IOParameter.eIOType = (CIOInitializeParameter.EIOType)Convert.ToInt32(strIOParameter[3]);
                    if (IOParameter.eIOType == CIOInitializeParameter.EIOType.IO_TYPE_DI)
                    {
                        nDIO_X_Count++;
                        if (nDIO_X_Page <= Convert.ToInt32(IOParameter.strAddress))
                            nDIO_X_Page = Convert.ToInt32(IOParameter.strAddress);
                    }
                    else if (IOParameter.eIOType == CIOInitializeParameter.EIOType.IO_TYPE_DO)
                    {
                        nDIO_Y_Count++;
                        if (nDIO_Y_Page <= Convert.ToInt32(IOParameter.strAddress))
                            nDIO_Y_Page = Convert.ToInt32(IOParameter.strAddress);
                    }
                    else if (IOParameter.eIOType == CIOInitializeParameter.EIOType.IO_TYPE_AI)
                    {
                        nAIO_X_Count++;
                        if (nAIO_X_Page <= Convert.ToInt32(IOParameter.strAddress))
                            nAIO_X_Page = Convert.ToInt32(IOParameter.strAddress);
                    }
                    IOParameter.nModuleIndex = Convert.ToInt32(strIOParameter[4]);
                    m_objIOInitializeParameter.objIOParameter.Add(IOParameter.strIndex, IOParameter);
                }
                fs.Close();

                m_objIOInitializeParameter.iInputModuleCount = m_objIOInitializeParameter.objIOParameter.Values
                    .Where(i => i.eIOType == CIOInitializeParameter.EIOType.IO_TYPE_DI)
                    .GroupBy(i => i.nModuleIndex)
                    .Count();
                m_objIOInitializeParameter.iOutPutModuleCount = m_objIOInitializeParameter.objIOParameter.Values
                    .Where(i => i.eIOType == CIOInitializeParameter.EIOType.IO_TYPE_DO)
                    .GroupBy(i => i.nModuleIndex)
                    .Count();
                m_objIOInitializeParameter.iAnalogInputModuleCount = m_objIOInitializeParameter.objIOParameter.Values
                    .Where(i => i.eIOType == CIOInitializeParameter.EIOType.IO_TYPE_AI)
                    .GroupBy(i => i.nModuleIndex)
                    .Count();
                m_objIOInitializeParameter.iAnalogOutputModuleCount = m_objIOInitializeParameter.objIOParameter.Values
                    .Where(i => i.eIOType == CIOInitializeParameter.EIOType.IO_TYPE_AO)
                    .GroupBy(i => i.nModuleIndex)
                    .Count();

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// IO 보드 수량 정보 저장
        /// </summary>
        /// <returns></returns>
        public bool SaveIOParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"IO";
                objINI.WriteValue(sectionName, "iInputModuleCount", m_objIOInitializeParameter.iInputModuleCount);
                objINI.WriteValue(sectionName, "iOutPutModuleCount", m_objIOInitializeParameter.iOutPutModuleCount);
                objINI.WriteValue(sectionName, "iAnalogInputModuleCount", m_objIOInitializeParameter.iAnalogInputModuleCount);
                objINI.WriteValue(sectionName, "iAnalogOutputModuleCount", m_objIOInitializeParameter.iAnalogOutputModuleCount);

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}