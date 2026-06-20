using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SVI_NFT_R
{
    public class CModel
    {
        public const int RANGE_PPID_START_NUMBER = 1;
        public const int RANGE_PPID_END_NUMBER = 200;
        public const int RANGE_PPID_RMS_ONLY_START_NUMBER = RANGE_PPID_START_NUMBER;
        public const int RANGE_PPID_RMS_ONLY_END_NUMBER = 90;
        public const int RANGE_PPID_SHARE_START_NUMBER = RANGE_PPID_RMS_ONLY_END_NUMBER + 1;
        public const int RANGE_PPID_SHARE_END_NUMBER = RANGE_PPID_END_NUMBER;

        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        private readonly HashSet<string> mJobChangeLossCodes = new HashSet<string>
        {
            $"{(int)CDialogLossCode.CodeNumber.CODE_CHANGE_SAME_MODEL}",
            $"{(int)CDialogLossCode.CodeNumber.CODE_CHANGE_DIFFERENT_MODEL}"
        };
        /// <summary>
        /// Model Class에 Recipe가 추가 되었다는 Flag
        /// </summary>
        public bool IsRecipeChanged = false;
        public bool IsOffsetChanged = false;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CModel(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool bReturn = false;

            do
            {
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
        }

        /// <summary>
        /// 해당 폴더 리스트 검색
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public List<string> GetDirectoryList(string strFilePath)
        {
            List<string> objReturn = new List<string>();

            do
            {
                DirectoryInfo objDir = new DirectoryInfo(strFilePath);
                // 지정된 경로에 폴더 없으면 빠져나감
                if (false == objDir.Exists)
                    break;
                // 전체 검색
                DirectoryInfo[] objDirArr = objDir.GetDirectories("*", SearchOption.TopDirectoryOnly);
                for (int iLoopDir = 0; iLoopDir < objDirArr.Length; iLoopDir++)
                {
                    objReturn.Add(objDirArr[iLoopDir].Name);
                }

            } while (false);

            return objReturn;
        }

        /// <summary>
        /// 폴더 삭제
        /// </summary>
        /// <param name="strFilePath"></param>
        public void SetDirectoryDelete(string strFilePath)
        {
            DirectoryInfo objDir = new DirectoryInfo(strFilePath);
            // 하위 폴더까지 삭제
            try
            {
                objDir.Delete(true);
            }
            catch (IOException ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 모델 파라미터 리스트 받음
        /// </summary>
        /// <returns></returns>
        public List<CConfig.CModelParameter> GetModelParameterList()
        {
            List<string> allPpids = GetDirectoryList(m_objDocument.m_objConfig.GetModelPath());
            return allPpids
                .Select(ppid => m_objDocument.m_objConfig.GetModelParameter(ppid))
                .OrderBy(model => model.Index)
                .ToList();
        }

        /// <summary>
        /// 모델 파라미터 받음
        /// </summary>
        /// <param name="strPPID"></param>
        /// <returns></returns>
        public CConfig.CModelParameter GetModelParameter(string strPPID)
        {
            return m_objDocument.m_objConfig.GetModelParameter(strPPID);
        }

        /// <summary>
        /// PPID가 중복되는지 체크
        /// </summary>
        /// <param name="strPPID"></param>
        /// <returns></returns>
        public bool GetPPIDOverlap(string strPPID)
        {
            List<string> allPpids = GetDirectoryList(m_objDocument.m_objConfig.GetModelPath());
            return allPpids
                .Any(ppid => ppid == strPPID);
        }

        /// <summary>
        /// 해당 PPID 폴더 내 PPID 변수 일치시킴
        /// </summary>
        /// <param name="strPPID"></param>
        /// <param name="strName"></param>
        public void SetPPIDMatch(string strPPID, int ppidNumber)
        {
            var model = m_objDocument.m_objConfig.GetModelParameter(strPPID);
            model.Index = ppidNumber;
            model.strSoftRevision = "1";
            model.UpdateTime = DateTime.Now;
            m_objDocument.m_objConfig.SaveModelParameter(model, strPPID);
        }

        /// <summary>
        /// 모델이 등록되지 않은 가장 빠른 인덱스를 찾음
        /// </summary>
        /// <returns></returns>
        public int GetEmptyIndex(int ppidNoStartIndex)
        {
            var indexs = GetModelParameterList()
                .Select(model => model.Index)
                .Where(index => index >= ppidNoStartIndex)
                .OrderBy(idx => idx)
                .Distinct();
            int i = ppidNoStartIndex;
            foreach (var index in indexs)
            {
                if (index != i)
                {
                    return i;
                }
                i++;
            }
            return i;
        }

        /// <summary>
        /// 등록된 인덱스 인지 확인함
        /// </summary>
        /// <param name="index">인덱스</param>
        /// <returns>true = 등록된 인덱스, false = 등록 안된 인덱스</returns>
        public bool IsDuiplicateIndex(int index)
        {
            return GetModelParameterList().Any(model => model.Index == index);
        }

        /// <summary>
        /// 마지막에 선택한 TPM LossCode가 JobChange인지 반환함
        /// </summary>
        /// <returns></returns>
        public bool IsSelectJobChangeTpmLossCode()
        {
            //JobChange LossCode 미사용
            return true;
            //string lastTpmLossCode = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.GetLastTpmLossCode();
            //return mJobChangeLossCodes.Contains(lastTpmLossCode);
        }

        /// <summary>
        /// 모델 생성 가능 영역 확인
        /// </summary>
        /// <param name="ppidNumber"></param>
        /// <returns></returns>
        public bool InRangeModelCapacity(int ppidNumber)
        {
            return ppidNumber >= RANGE_PPID_START_NUMBER && ppidNumber <= RANGE_PPID_END_NUMBER;
        }

        /// <summary>
        /// RMS 전용 영역 확인
        /// </summary>
        /// <param name="ppidNumber"></param>
        /// <returns></returns>
        public bool InRangeRmsOnly(int ppidNumber)
        {
            return ppidNumber >= RANGE_PPID_RMS_ONLY_START_NUMBER && ppidNumber <= RANGE_PPID_RMS_ONLY_END_NUMBER;
        }

        /// <summary>
        /// RMS & 설비 공용 영역 확인 (이 영역에서는 PPID가 "TT_"로 시작해야되는 규칙이 있음)
        /// </summary>
        /// <param name="ppidNumber"></param>
        /// <returns></returns>
        public bool InRangeShare(int ppidNumber)
        {
            return ppidNumber >= RANGE_PPID_SHARE_START_NUMBER && ppidNumber <= RANGE_PPID_SHARE_END_NUMBER;
        }
    }
}