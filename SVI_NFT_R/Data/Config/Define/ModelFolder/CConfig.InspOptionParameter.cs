using System;
using System.IO;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// SDV 삼성 광학계 설정 파라미터
        /// </summary>
        [Serializable]
        public class CInspOptionParameter
        {
            /// <summary>
            /// 비전 검사 모드
            /// </summary>
            public enum EInspectionMode
            {
                INSPECT_USE = 0,
                INSPECT_NOT_USE,
                INSPECT_BY_PASS,
                INSPECT_ERROR_SKIP,
                INSPECT_FINAL
            }

            /// <summary>
            /// 비전 검사 모드
            /// </summary>
            public EInspectionMode eInspectionMode;

            /// <summary>
            /// 출력 펄스 간격 (간이 광학계)
            /// </summary>
            public double dPeriod;

            /// <summary>
            /// 출력 벌스 시간 (간이 광학계)
            /// </summary>
            public double dPulseWidth;
        }

        /// <summary>
        /// SDV 옵션 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CInspOptionParameter GetInspOptionParameter(CDefine.EInspectType index) => m_objInspOptionParameter[(int)index];

        /// <summary>
        /// SDV 광학 옵션 파라미터 선언
        /// </summary>
        private readonly CInspOptionParameter[] m_objInspOptionParameter = new CInspOptionParameter[Enum.GetNames(typeof(CDefine.EInspectType)).Length];

        /// <summary>
        /// SDV 비전 옵션 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadInspOptionParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\DATA\{1}\{2}", m_strModelPath, m_objSystemParameter.strPPID, CDefine.DEF_VISION_DAT);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EInspectType AmoIndex in Enum.GetValues(typeof(CDefine.EInspectType)))
                {
                    int index = (int)AmoIndex;
                    if (m_objInspOptionParameter[index] == null)
                    {
                        m_objInspOptionParameter[index] = new CInspOptionParameter();
                    }
                    string sectionName = $"SVI_OPTION_{AmoIndex}";
                    m_objInspOptionParameter[index].eInspectionMode = (CInspOptionParameter.EInspectionMode)objINI.GetInt32(sectionName, "eInspectionMode", 0);
                    m_objInspOptionParameter[index].dPeriod = objINI.GetDouble(sectionName, "dPeriod", 615);
                    m_objInspOptionParameter[index].dPulseWidth = objINI.GetDouble(sectionName, "dPulseWidth", 500);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// SDV 비전 옵션 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveInspOptionParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\DATA\{1}", m_strModelPath, m_objSystemParameter.strPPID);
                if (false == Directory.Exists(strPath))
                {
                    // 폴더 생성
                    Directory.CreateDirectory(strPath);
                }

                strPath = string.Format(@"{0}\DATA\{1}\{2}", m_strModelPath, m_objSystemParameter.strPPID, CDefine.DEF_VISION_DAT);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EInspectType AmoIndex in Enum.GetValues(typeof(CDefine.EInspectType)))
                {
                    int index = (int)AmoIndex;
                    if (m_objInspOptionParameter[index] == null)
                    {
                        m_objInspOptionParameter[index] = new CInspOptionParameter();
                    }
                    string sectionName = $"SVI_OPTION_{AmoIndex}";
                    objINI.WriteValue(sectionName, "eInspectionMode", (int)m_objInspOptionParameter[index].eInspectionMode);
                    objINI.WriteValue(sectionName, "dPeriod", m_objInspOptionParameter[index].dPeriod);
                    objINI.WriteValue(sectionName, "dPulseWidth", m_objInspOptionParameter[index].dPulseWidth);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// SDV 비전 옵션 파라미터 저장
        /// </summary>
        /// <param name="objSDVOptionParameter"></param>
        /// <param name="eChannel"></param>
        /// <returns></returns>
        public bool SaveInspOptionParameter(CInspOptionParameter objSDVOptionParameter, CDefine.EInspectType index)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\DATA\{1}", m_strModelPath, m_objSystemParameter.strPPID);
                if (false == Directory.Exists(strPath))
                {
                    // 폴더 생성
                    Directory.CreateDirectory(strPath);
                }

                strPath = string.Format(@"{0}\DATA\{1}\{2}", m_strModelPath, m_objSystemParameter.strPPID, CDefine.DEF_VISION_DAT);
                Constants.SaveBackupFile(strPath);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"SVI_OPTION_{index}";
                objINI.WriteValue(sectionName, "eInspectionMode", (int)objSDVOptionParameter.eInspectionMode);
                objINI.WriteValue(sectionName, "dPeriod", objSDVOptionParameter.dPeriod);
                objINI.WriteValue(sectionName, "dPulseWidth", objSDVOptionParameter.dPulseWidth);
                m_objInspOptionParameter[(int)index] = objSDVOptionParameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}