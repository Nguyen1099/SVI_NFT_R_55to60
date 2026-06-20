using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// 모델 옵셋 파라미터
        /// </summary>
        [Serializable]
        public class CModelOffsetParameter
        {
            /// <summary>
            /// 더미 데이터
            /// </summary>
            [DisplayInfo("DUMMY_DATA", " ( mm )")]
            public double DummyData;
        }

        public class DisplayInfoAttribute : Attribute
        {
            public string Name { get; set; } = string.Empty;
            public string Unit { get; set; } = string.Empty;
            public DisplayInfoAttribute(string name, string unit)
            {
                Name = name;
                Unit = unit;
            }
        }

        /// <summary>
        /// 모든 모델 옵셋 파라미터 반환
        /// </summary>
        public Dictionary<int, CModelOffsetParameter> GetModelOffsetParameters()
        {
            string strPath = Path.Combine(m_strModelPath, "DATA");
            if (false == Directory.Exists(strPath))
            {
                // 폴더 생성
                Directory.CreateDirectory(strPath);
            }
            strPath = Path.Combine(m_strModelPath, "DATA", CDefine.DEF_MODEL_OFFSET_DAT);
            ClassINI objINI = new ClassINI(strPath);

            return objINI
                .GetSectionNames()
                .OrderBy(sectionName => sectionName)
                .ToDictionary(sectionName => Convert.ToInt32(sectionName), sectionName =>
                {
                    CModelOffsetParameter parameter = new CModelOffsetParameter()
                    {
                        DummyData = objINI.GetDouble(sectionName, nameof(CModelOffsetParameter.DummyData), 1d)
                    };
                    return parameter;
                });
        }

        /// <summary>
        /// 모델 옵셋 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CModelOffsetParameter GetModelOffsetParameter() => m_objModelOffsetParameter;

        /// <summary>
        /// 모델 옵셋 파라미터 선언
        /// </summary>
        private CModelOffsetParameter m_objModelOffsetParameter = new CModelOffsetParameter();

        /// <summary>
        /// 모델 옵셋 파라미터 로드
        /// </summary>
        /// <param name="modelIndex"></param>
        /// <returns></returns>
        public bool LoadModelOffsetParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = Path.Combine(m_strModelPath, "DATA", CDefine.DEF_MODEL_OFFSET_DAT);
                ClassINI objINI = new ClassINI(strPath);

                int modelIndex = m_objModelParameter.Index;
                string sectionName = $"{modelIndex}";
                m_objModelOffsetParameter.DummyData = objINI.GetDouble(sectionName, nameof(CModelOffsetParameter.DummyData), 1d);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모델 옵셋 파라미터 저장
        /// </summary>
        /// <param name="modelIndex"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SaveModelOffsetParameter(CModelOffsetParameter data)
        {
            if (SaveModelOffsetParameter(data, m_objModelParameter.Index) == false)
            {
                return false;
            }
            m_objModelOffsetParameter = data.DeepClone();
            return true;
        }

        /// <summary>
        /// 모델 옵셋 파라미터 저장
        /// </summary>
        public bool SaveModelOffsetParameter(CModelOffsetParameter data, int modelIndex)
        {
            bool bReturn = false;

            do
            {
                string strPath = Path.Combine(m_strModelPath, "DATA");
                if (false == Directory.Exists(strPath))
                {
                    // 폴더 생성
                    Directory.CreateDirectory(strPath);
                }

                strPath = Path.Combine(m_strModelPath, "DATA", CDefine.DEF_MODEL_OFFSET_DAT);
                Constants.SaveBackupFile(strPath);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"{modelIndex}";
                objINI.WriteValue(sectionName, nameof(CModelOffsetParameter.DummyData), data.DummyData);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모델 옵셋 파라미터 리셋
        /// </summary>
        public void ResetModelOffsetParameter(int modelIndex)
        {
            string strPath = Path.Combine(m_strModelPath, "DATA");
            if (false == Directory.Exists(strPath))
            {
                // 폴더 생성
                Directory.CreateDirectory(strPath);
            }
            strPath = Path.Combine(m_strModelPath, "DATA", CDefine.DEF_MODEL_OFFSET_DAT);
            ClassINI objINI = new ClassINI(strPath);

            string sectionName = $"{modelIndex}";
            objINI.DeleteSection(sectionName);
        }

        /// <summary>
        /// 저장된 모델 옵셋 파라미터가 있는지 확인함
        /// </summary>
        public bool ExistModelOffsetParameter(int modelIndex)
        {
            string strPath = Path.Combine(m_strModelPath, "DATA");
            if (false == Directory.Exists(strPath))
            {
                // 폴더 생성
                Directory.CreateDirectory(strPath);
            }
            strPath = Path.Combine(m_strModelPath, "DATA", CDefine.DEF_MODEL_OFFSET_DAT);
            ClassINI objINI = new ClassINI(strPath);

            string sectionName = $"{modelIndex}";
            return objINI.GetSectionNames().Any(item => item == sectionName);
        }
    }
}