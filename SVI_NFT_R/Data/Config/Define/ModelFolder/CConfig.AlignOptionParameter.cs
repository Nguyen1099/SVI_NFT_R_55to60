using System;
using System.IO;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        [Serializable]
        public class CAlignOptionParameter
        {
            /// <summary>
            /// 비전 사용 유무
            /// </summary>
            public bool bUseVision;

            /// <summary>
            /// 얼라인 X 보정량 인터락
            /// </summary>
            public double dAlignInterlockX;

            /// <summary>
            /// 얼라인 Y 보정량 인터락
            /// </summary>
            public double dAlignInterlockY;

            /// <summary>
            /// 얼라인 T 보정량 인터락
            /// </summary>
            public double dAlignInterlockT;

            /// <summary>
            /// 비전 매칭률
            /// </summary>
            public double iVisionScore;

            /// <summary>
            /// 재시도 기능을 사용할 수 있는지 여부
            /// </summary>
            public bool CanUsingRetry;

            /// <summary>
            /// 재시도 횟수 (0으로 설정시 체크 시퀀스 진행 안함)
            /// </summary>
            public int RetryCount;

            /// <summary>
            /// 얼라인 X 보정량 체크 시퀀스 Pass 기준
            /// </summary>
            public double ToleranceX;

            /// <summary>
            /// 얼라인 Y 보정량 체크 시퀀스 Pass 기준
            /// </summary>
            public double ToleranceY;

            /// <summary>
            /// 얼라인 T 보정량 체크 시퀀스 Pass 기준
            /// </summary>
            public double ToleranceT;
        }

        /// <summary>
        /// 비전 옵션 파라미터 리턴
        /// </summary>
        /// <param name="eCamera"></param>
        /// <returns></returns>
        public CAlignOptionParameter GetAlignOptionParameter(CDefine.EAlign eCamera) => m_objAlignOptionParameter[(int)eCamera];

        /// <summary>
        /// 비전 옵션 데이터 파라미터 선언
        /// </summary>
        private readonly CAlignOptionParameter[] m_objAlignOptionParameter = new CAlignOptionParameter[Enum.GetNames(typeof(CDefine.EAlign)).Length];

        /// <summary>
        /// 비전 옵션 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadAlignOptionParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}\{2}", GetModelPath(), m_objSystemParameter.strPPID, CDefine.DEF_ALIGN_DAT);
                ClassINI objINI = new ClassINI(strPath);

                int cameraCount = Enum.GetNames(typeof(CDefine.EAlign)).Length;
                for (int iLoopCount = 0; iLoopCount < cameraCount; iLoopCount++)
                {
                    if (m_objAlignOptionParameter[iLoopCount] == null)
                    {
                        m_objAlignOptionParameter[iLoopCount] = new CAlignOptionParameter();
                    }
                    string sectionName = $"VISION_{iLoopCount}";
                    m_objAlignOptionParameter[iLoopCount].bUseVision = objINI.GetBool(sectionName, "bUseVision", true);
                    m_objAlignOptionParameter[iLoopCount].dAlignInterlockX = objINI.GetDouble(sectionName, "dAlignInterlockX", 3d);
                    m_objAlignOptionParameter[iLoopCount].dAlignInterlockY = objINI.GetDouble(sectionName, "dAlignInterlockY", 3d);
                    m_objAlignOptionParameter[iLoopCount].dAlignInterlockT = objINI.GetDouble(sectionName, "dAlignInterlockT", 3d);
                    m_objAlignOptionParameter[iLoopCount].iVisionScore = objINI.GetDouble(sectionName, "iVisionScore", 0d);
                    m_objAlignOptionParameter[iLoopCount].RetryCount = objINI.GetInt32(sectionName, "RetryCount", 0);
                    m_objAlignOptionParameter[iLoopCount].ToleranceX = objINI.GetDouble(sectionName, "ToleranceX", 0.01d);
                    m_objAlignOptionParameter[iLoopCount].ToleranceY = objINI.GetDouble(sectionName, "ToleranceY", 0.01d);
                    m_objAlignOptionParameter[iLoopCount].ToleranceT = objINI.GetDouble(sectionName, "ToleranceT", 0.01d);
                }

                // ! PreAlign은 하드웨어 구조상 리트라이 기능을 적용 할 수 없음
                m_objAlignOptionParameter[(int)CDefine.EAlign.PreAlign].CanUsingRetry = false;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 비전 옵션 파라미터 저장
        /// </summary>
        /// <param name="objVisionParameter"></param>
        /// <param name="eCamera"></param>
        /// <returns></returns>
        public bool SaveAlignOptionParameter(CAlignOptionParameter objVisionParameter, CDefine.EAlign eCamera)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", GetModelPath(), m_objSystemParameter.strPPID);
                if (false == Directory.Exists(strPath))
                {
                    // 폴더 생성
                    Directory.CreateDirectory(strPath);
                }

                strPath = string.Format(@"{0}\{1}\{2}", GetModelPath(), m_objSystemParameter.strPPID, CDefine.DEF_ALIGN_DAT);

                Constants.SaveBackupFile(strPath);

                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"VISION_{(int)eCamera}";
                objINI.WriteValue(sectionName, "bUseVision", objVisionParameter.bUseVision);
                objINI.WriteValue(sectionName, "dAlignInterlockX", objVisionParameter.dAlignInterlockX);
                objINI.WriteValue(sectionName, "dAlignInterlockY", objVisionParameter.dAlignInterlockY);
                objINI.WriteValue(sectionName, "dAlignInterlockT", objVisionParameter.dAlignInterlockT);
                objINI.WriteValue(sectionName, "iVisionScore", objVisionParameter.iVisionScore);
                objINI.WriteValue(sectionName, "RetryCount", objVisionParameter.RetryCount);
                objINI.WriteValue(sectionName, "ToleranceX", objVisionParameter.ToleranceX);
                objINI.WriteValue(sectionName, "ToleranceY", objVisionParameter.ToleranceY);
                objINI.WriteValue(sectionName, "ToleranceT", objVisionParameter.ToleranceT);
                m_objAlignOptionParameter[(int)eCamera] = objVisionParameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 비전 옵션 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveAlignOptionParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", GetModelPath(), m_objSystemParameter.strPPID);
                if (false == Directory.Exists(strPath))
                {
                    // 폴더 생성
                    Directory.CreateDirectory(strPath);
                }

                strPath = string.Format(@"{0}\{1}\{2}", GetModelPath(), m_objSystemParameter.strPPID, CDefine.DEF_ALIGN_DAT);
                ClassINI objINI = new ClassINI(strPath);

                int cameraCount = Enum.GetNames(typeof(CDefine.EAlign)).Length;
                for (int iLoopCount = 0; iLoopCount < cameraCount; iLoopCount++)
                {
                    string sectionName = $"VISION_{iLoopCount}";
                    objINI.WriteValue(sectionName, "bUseVision", m_objAlignOptionParameter[iLoopCount].bUseVision);
                    objINI.WriteValue(sectionName, "dAlignInterlockX", m_objAlignOptionParameter[iLoopCount].dAlignInterlockX);
                    objINI.WriteValue(sectionName, "dAlignInterlockY", m_objAlignOptionParameter[iLoopCount].dAlignInterlockY);
                    objINI.WriteValue(sectionName, "dAlignInterlockT", m_objAlignOptionParameter[iLoopCount].dAlignInterlockT);
                    objINI.WriteValue(sectionName, "iVisionScore", m_objAlignOptionParameter[iLoopCount].iVisionScore);
                    objINI.WriteValue(sectionName, "RetryCount", m_objAlignOptionParameter[iLoopCount].RetryCount);
                    objINI.WriteValue(sectionName, "ToleranceX", m_objAlignOptionParameter[iLoopCount].ToleranceX);
                    objINI.WriteValue(sectionName, "ToleranceY", m_objAlignOptionParameter[iLoopCount].ToleranceY);
                    objINI.WriteValue(sectionName, "ToleranceT", m_objAlignOptionParameter[iLoopCount].ToleranceT);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}