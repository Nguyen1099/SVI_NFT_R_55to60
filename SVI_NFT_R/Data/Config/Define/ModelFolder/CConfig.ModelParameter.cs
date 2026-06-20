using System;
using System.IO;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// Model Parameter
        /// </summary>
        [Serializable]
        public class CModelParameter
        {
            /// <summary>
            /// PPID
            /// </summary>
            public string strPPID;

            /// <summary>
            /// 모델 번호
            /// </summary>
            public int Index;

            /// <summary>
            /// 레시피 넓이
            /// </summary>
            public double dWidth;

            /// <summary>
            /// 레시피 높이
            /// </summary>
            public double dHeight;

            /// <summary>
            /// 모델 SOFTREV
            /// </summary>
            public string strSoftRevision;

            /// <summary>
            /// 업데이트 시간
            /// </summary>
            public DateTime UpdateTime;

            public string this[CCIMDefine.EPpidParamList idx]
            {
                set
                {
                    switch (idx)
                    {
                        case CCIMDefine.EPpidParamList.CELL_WIDTH:
                            double.TryParse(value, out dWidth);
                            break;

                        case CCIMDefine.EPpidParamList.CELL_HEIGHT:
                            double.TryParse(value, out dHeight);
                            break;
                    }
                }
                get
                {
                    string result = null;
                    switch (idx)
                    {
                        case CCIMDefine.EPpidParamList.CELL_WIDTH:
                            result = string.Format("{0:0.000}", dWidth);
                            break;

                        case CCIMDefine.EPpidParamList.CELL_HEIGHT:
                            result = string.Format("{0:0.000}", dHeight);
                            break;
                    }
                    return result;
                }
            }
        }

        /// <summary>
        /// 모델 파라미터 선언
        /// </summary>
        private CModelParameter m_objModelParameter = new CModelParameter();

        /// <summary>
        /// 모델 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CModelParameter GetModelParameter(string ppidOrNull = null)
        {
            if (ppidOrNull == null)
            {
                ppidOrNull = m_objSystemParameter.strPPID;
            }

            string strPath = string.Format(@"{0}\{1}\{2}", GetModelPath(), ppidOrNull, CDefine.DEF_MODEL_INI);
            ClassINI objINI = new ClassINI(strPath);

            string sectionName = $"MODEL";
            CModelParameter objModelParameter = new CModelParameter();
            objModelParameter.strPPID = ppidOrNull;
            objModelParameter.Index = objINI.GetInt32(sectionName, "Index", CModel.RANGE_PPID_SHARE_START_NUMBER);
            objModelParameter.dWidth = objINI.GetDouble(sectionName, "dWidth", 0d);
            objModelParameter.dHeight = objINI.GetDouble(sectionName, "dHeight", 0d);
            objModelParameter.strSoftRevision = objINI.GetString(sectionName, "strSoftRevision", "0");
            string readTime = objINI.GetString(sectionName, "UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            objModelParameter.UpdateTime = (DateTime)Convert.ChangeType(readTime, typeof(DateTime));

            return objModelParameter;
        }

        /// <summary>
        /// 모델 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadModelParameter(string ppidOrNull = null)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}\{2}", GetModelPath(), m_objSystemParameter.strPPID, CDefine.DEF_MODEL_INI);
                string PPID = m_objSystemParameter.strPPID;
                if (ppidOrNull != null)
                {
                    strPath = string.Format(@"{0}\{1}\{2}", GetModelPath(), ppidOrNull, CDefine.DEF_MODEL_INI);
                    PPID = ppidOrNull;
                }
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"MODEL";
                m_objModelParameter.strPPID = PPID;
                m_objModelParameter.Index = objINI.GetInt32(sectionName, "Index", CModel.RANGE_PPID_SHARE_START_NUMBER);
                m_objModelParameter.dWidth = objINI.GetDouble(sectionName, "dWidth", 0.0);
                m_objModelParameter.dHeight = objINI.GetDouble(sectionName, "dHeight", 0.0);
                m_objModelParameter.strSoftRevision = objINI.GetString(sectionName, "strSoftRevision", "0");
                string readTime = objINI.GetString(sectionName, "UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                m_objModelParameter.UpdateTime = (DateTime)Convert.ChangeType(readTime, typeof(DateTime));

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모델 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveModelParameter(string ppidOrNull = null)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", GetModelPath(), m_objSystemParameter.strPPID);
                string PPID = m_objSystemParameter.strPPID;
                if (ppidOrNull != null)
                {
                    strPath = string.Format(@"{0}\{1}", GetModelPath(), ppidOrNull);
                    PPID = ppidOrNull;
                }
                if (false == Directory.Exists(strPath))
                {
                    // 폴더 생성
                    Directory.CreateDirectory(strPath);
                }

                strPath = string.Format(@"{0}\{1}\{2}", GetModelPath(), PPID, CDefine.DEF_MODEL_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"MODEL";
                objINI.WriteValue(sectionName, "strPPID", PPID);
                objINI.WriteValue(sectionName, "Index", m_objModelParameter.Index);
                objINI.WriteValue(sectionName, "dWidth", m_objModelParameter.dWidth);
                objINI.WriteValue(sectionName, "dHeight", m_objModelParameter.dHeight);
                objINI.WriteValue(sectionName, "strSoftRevision", m_objModelParameter.strSoftRevision);
                objINI.WriteValue(sectionName, "UpdateTime", m_objModelParameter.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"));

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public void SaveBackupDir()
        {
            Constants.SaveBackupDir(m_objSystemParameter.strPPID, $@"{GetModelPath()}\{m_objSystemParameter.strPPID}");
        }

        /// <summary>
        /// 모델 파라미터 저장
        /// </summary>
        /// <param name="objModelParameter"></param>
        /// <returns></returns>
        public bool SaveModelParameter(CModelParameter objModelParameter, string ppidOrNull = null)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}\{2}", GetModelPath(), m_objSystemParameter.strPPID, CDefine.DEF_MODEL_INI);
                string PPID = m_objSystemParameter.strPPID;
                if (ppidOrNull != null)
                {
                    strPath = string.Format(@"{0}\{1}\{2}", GetModelPath(), ppidOrNull, CDefine.DEF_MODEL_INI);
                    PPID = ppidOrNull;
                }

                SaveBackupDir();

                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"MODEL";
                objINI.WriteValue(sectionName, "strPPID", PPID);
                objINI.WriteValue(sectionName, "Index", objModelParameter.Index);
                objINI.WriteValue(sectionName, "dWidth", objModelParameter.dWidth);
                objINI.WriteValue(sectionName, "dHeight", objModelParameter.dHeight);
                objINI.WriteValue(sectionName, "strSoftRevision", objModelParameter.strSoftRevision);
                objINI.WriteValue(sectionName, "UpdateTime", objModelParameter.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                m_objModelParameter = objModelParameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}