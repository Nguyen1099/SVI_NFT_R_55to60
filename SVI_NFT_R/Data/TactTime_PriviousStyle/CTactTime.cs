using System;
using System.Collections.Generic;
using System.Text;

namespace SVI_NFT_R
{
    public class CTactTime
    {
        /// <summary>
        /// 유닛 택타임 상세정보
        /// </summary>
        public Dictionary<ETactTime, Dictionary<string, string>> UnitTactTimeDetailInformation { get; private set; }
        /// <summary>
        /// 유닛 사이클 택타임
        /// </summary>
        public Dictionary<ETactTime, double> CycleTactTime { get; private set; }
        /// <summary>
        /// 유닛 투입 대기 시간
        /// </summary>
        public Dictionary<ETactTime, double> LoadingWaitTime { get; private set; }
        /// <summary>
        /// 유닛 배출 대기 시간
        /// </summary>
        public Dictionary<ETactTime, double> UnloadingWaitTime { get; private set; }
        /// <summary>
        /// 유닛 배출 대기 시간
        /// </summary>
        public Dictionary<ETactTime, double> UserStopTime { get; private set; }
        /// <summary>
        /// 사용자 정의 순수 택타임 (미설정시 동일한 규칙 적용)
        /// </summary>
        public Dictionary<ETactTime, double> CustomPureCycleTactTime { get; private set; }
        /// <summary>
        /// 배출 택타임
        /// </summary>
        private Queue<TactTime.Tact> m_queueOutputTactTime;
        /// <summary>
        /// 배출 택타임 최대 개수 (기본 30)
        /// </summary>
        private int m_iMaxQueueCount = 30;
        private CDocument m_objDocument;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize(CDocument document)
        {
            bool bReturn = false;

            do
            {
                m_objDocument = document;
                m_queueOutputTactTime = new Queue<TactTime.Tact>();

                ClearCycleTactTime();

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
        /// 해당 유닛에 대기시간을 제외한 순수 택타임
        /// </summary>
        /// <param name="unitIndex"></param>
        /// <returns></returns>
        public double GetPureCycleTactTime(ETactTime unitIndex)
        {
            double result = CustomPureCycleTactTime[unitIndex];
            if (double.MinValue == result)
            {
                result = CycleTactTime[unitIndex] - LoadingWaitTime[unitIndex] - UnloadingWaitTime[unitIndex] - UserStopTime[unitIndex];
            }
            return result;
        }

        /// <summary>
        /// 모든 유닛에 대기시간을 제외한 순수 택타임
        /// </summary>
        /// <param name="unitIndex"></param>
        /// <returns></returns>
        public double[] GetAllPureCycleTactTime()
        {
            double[] result = new double[CycleTactTime.Count];
            int index = 0;
            foreach (ETactTime unitIndex in Enum.GetValues(typeof(ETactTime)))
            {
                double captureValue = CustomPureCycleTactTime[unitIndex];
                if (double.MinValue == captureValue)
                {
                    captureValue = CycleTactTime[unitIndex] - LoadingWaitTime[unitIndex] - UnloadingWaitTime[unitIndex] - UserStopTime[unitIndex];
                }
                result[index] = captureValue;
                index++;
            }
            return result;
        }

        /// <summary>
        /// 사이클 택타임 데이터 클리어
        /// </summary>
        public void ClearCycleTactTime()
        {
            var newUnitTactTimeDetailInformation = new Dictionary<ETactTime, Dictionary<string, string>>();
            var newCycleTactTime = new Dictionary<ETactTime, double>();
            var newLoadingWaitTime = new Dictionary<ETactTime, double>();
            var newUnloadingWaitTime = new Dictionary<ETactTime, double>();
            var newUserStopTime = new Dictionary<ETactTime, double>();
            var newCustomPureCycleTactTime = new Dictionary<ETactTime, double>();

            foreach (ETactTime item in Enum.GetValues(typeof(ETactTime)))
            {
                newUnitTactTimeDetailInformation[item] = new Dictionary<string, string>();
                newCycleTactTime[item] = 0d;
                newLoadingWaitTime[item] = 0d;
                newUnloadingWaitTime[item] = 0d;
                newUserStopTime[item] = 0d;
                newCustomPureCycleTactTime[item] = double.MinValue;
            }

            UnitTactTimeDetailInformation = newUnitTactTimeDetailInformation;
            CycleTactTime = newCycleTactTime;
            LoadingWaitTime = newLoadingWaitTime;
            UnloadingWaitTime = newUnloadingWaitTime;
            UserStopTime = newUserStopTime;
            CustomPureCycleTactTime = newCustomPureCycleTactTime;
        }

        /// <summary>
        /// 배출 택타임 값 Enqueue
        /// </summary>
        /// <param name="dValue"></param>
        public void SetOutputTactTime(TactTime.Tact tactTime)
        {
            do
            {
                if (true == tactTime.IsIncomplete)
                {
                    break;
                }

                // 큐에 저장된 값 요소가 최대 큐 개수보다 크면 Dequeue 해줘야 함
                if (m_iMaxQueueCount <= m_queueOutputTactTime.Count)
                {
                    try
                    {
                        m_queueOutputTactTime.Dequeue();
                    }
                    catch (Exception ex)
                    {
                        LogWrite.Exception(ex);
                    }
                    SetOutputTactTime(tactTime);
                    break;
                }
                m_queueOutputTactTime.Enqueue(tactTime);

                // TactTime Log
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}],", DateTime.Now);
                sb.AppendFormat("{0},", tactTime.CellCount);
                foreach (var item in tactTime.InnerID)
                {
                    sb.AppendFormat("{0},", item);
                }
                foreach (var item in tactTime.CellID)
                {
                    sb.AppendFormat("{0},", item);
                }
                sb.AppendFormat("{0:0.000},", tactTime.OutTact.Value.TotalSeconds);
                sb.AppendFormat("{0:0.000},", tactTime.InspectionTact.Value.TotalSeconds);
                sb.AppendFormat("{0:0.000},", tactTime.InputDelayTime.Value.TotalSeconds);
                sb.AppendFormat("{0:0.000},", tactTime.OutputDelayTime.Value.TotalSeconds);
                sb.AppendFormat("{0},{1:0.000},", tactTime.SlowPureTactUnitName, tactTime.SlowPureTactUnitTime.TotalSeconds);
                sb.AppendFormat("{0},{1:0.000}", tactTime.FastPureTactUnitName, tactTime.FastPureTactUnitTime.TotalSeconds);
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_TACT_TIME, sb.ToString());
                TactTime.Manager.Delete(tactTime.InnerID);

            } while (false);
        }

        /// <summary>
        /// 배출 택타임 최대 값 설정
        /// </summary>
        /// <param name="iCount"></param>
        public void SetOutputTactTimeMaxCount(int iCount)
        {
            do
            {
                if (0 >= iCount) break;
                // 큐에 저장된 값 요소가 설정 개수보다 크면 Dequeue 해줘야 함
                if (iCount < m_queueOutputTactTime.Count)
                {
                    try
                    {
                        m_queueOutputTactTime.Dequeue();
                    }
                    catch (Exception ex)
                    {
                        LogWrite.Exception(ex);
                    }
                    SetOutputTactTimeMaxCount(iCount);
                    break;
                }
                m_iMaxQueueCount = iCount;
            } while (false);
        }

        /// <summary>
        /// 배출 택타임 배열 리스트 반환
        /// </summary>
        /// <returns></returns>
        public TactTime.Tact[] GetOutputTactTime()
        {
            TactTime.Tact[] outputTactTime = m_queueOutputTactTime.ToArray();
            // 반전
            Array.Reverse(outputTactTime);
            return outputTactTime;
        }

        /// <summary>
        /// 배출 택타임 값 클리어
        /// </summary>
        public void SetOutputTactTimeClear()
        {
            m_queueOutputTactTime.Clear();
        }
    }
}