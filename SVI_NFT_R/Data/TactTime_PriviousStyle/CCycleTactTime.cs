using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SVI_NFT_R
{
    [Serializable]
    public class CTimeElement
    {
        public DateTime m_time;
        public string m_strTime;

        public CTimeElement()
        {
            m_time = default(DateTime);
            m_strTime = string.Empty;
        }

        public bool IsEmpty()
        {
            bool bResult = false;

            if (
                true == string.IsNullOrEmpty(m_strTime)
                && true == m_time.Equals(default(DateTime))
                )
            {
                bResult = true;
            }

            return bResult;
        }

        public void Clear()
        {
            m_time = default(DateTime);
            m_strTime = string.Empty;
        }

        public void SetTime()
        {
            if (true == string.IsNullOrEmpty(m_strTime))
            {
                m_time = DateTime.Now;
                m_strTime = m_time.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
        }

        public DateTime GetTime()
        {
            return m_time;
        }
    }

    [Serializable]
    public class CCycleTact<enumHeader> where enumHeader : struct
    {
        public CDefine.ELogType LogType;
        public Dictionary<enumHeader, CTimeElement> dicTactTime = new Dictionary<enumHeader, CTimeElement>();
        public string strHeader;
        public string strInnerID;
        public string strCellID;
        public string strInspection_Class;
        public string strInspection_Defect;
        public DateTime InstanceCreateTime { get; private set; }

        public CCycleTact(CDefine.ELogType eLogType)
        {
            InstanceCreateTime = DateTime.Now;
            LogType = eLogType;
            var sbHeader = new StringBuilder();
            foreach (enumHeader key in Enum.GetValues(typeof(enumHeader)))
            {
                dicTactTime.Add(key, new CTimeElement());
                if ("RAWDATA" != key.ToString())
                {
                    sbHeader.AppendFormat("{0},", key.ToString());
                }
                else
                {
                    sbHeader.AppendFormat("{0},", string.Empty);
                }
            }
            strHeader = sbHeader.ToString();
        }
    }

    [Serializable]
    public class CCycleTactTime
    {
        public CCycleTact<CDefine.ECycleTactInShuttle> m_tactInShuttle = new CCycleTact<CDefine.ECycleTactInShuttle>(CDefine.ELogType.LOG_CYCLE_TACT_01_IN_SHUTTLE);
        public CCycleTact<CDefine.ECycleTactInRobot> m_tactInRobot = new CCycleTact<CDefine.ECycleTactInRobot>(CDefine.ELogType.LOG_CYCLE_TACT_02_IN_ROBOT);
        public CCycleTact<CDefine.ECycleTactInspStage> m_tactInspStage = new CCycleTact<CDefine.ECycleTactInspStage>(CDefine.ELogType.LOG_CYCLE_TACT_03_INSP_STAGE);
        public CCycleTact<CDefine.ECycleTactOutRobot> m_tactOutRobot = new CCycleTact<CDefine.ECycleTactOutRobot>(CDefine.ELogType.LOG_CYCLE_TACT_04_OUT_ROBOT);
        public CCycleTact<CDefine.ECycleTactOutFlip> m_TactOutFlip = new CCycleTact<CDefine.ECycleTactOutFlip>(CDefine.ELogType.LOG_CYCLE_TACT_05_OUT_FLIP);
        // m_tactJavasInspection은 로그 초기화시 참조용으로 사용함
        public CCycleTact<CDefine.ECycleTactJavasInspection> m_tactJavasInspection = new CCycleTact<CDefine.ECycleTactJavasInspection>(CDefine.ELogType.LOG_CYCLE_TACT_90_JAVAS_INSPECTION);
        // JavasInspection은 로그는 CellID를 기준으로 로그를 기록하는 방식으로 딕셔너리를 사용함
        private Dictionary<string, CCycleTact<CDefine.ECycleTactJavasInspection>> m_dicJavasInspectionItems = new Dictionary<string, CCycleTact<CDefine.ECycleTactJavasInspection>>();

        /// <summary>
        /// 자바스 택타임 아이템을 불러옴 (없으면 인스턴스를 새로 할당하여 추가)
        /// </summary>
        /// <param name="strCellID">Cell ID</param>
        /// <returns>자바스 택타임 아이템</returns>
        public CCycleTact<CDefine.ECycleTactJavasInspection> GetJavasCycleTactItem(string strCellID)
        {
            CCycleTact<CDefine.ECycleTactJavasInspection> result = null;

            lock (m_dicJavasInspectionItems)
            {
                if (true == m_dicJavasInspectionItems.ContainsKey(strCellID))
                {
                    result = m_dicJavasInspectionItems[strCellID];
                }
                else
                {
                    result = new CCycleTact<CDefine.ECycleTactJavasInspection>(m_tactJavasInspection.LogType);
                    m_dicJavasInspectionItems.Add(strCellID, result);

                    // 5시간 이상 처리되지 않은 오래된 데이터 삭제 (일반적으로 데이터는 쌓이지 않으나 예외 상황 발생시 메모리 Leak를 방지하기 위함)
                    {
                        TimeSpan overTimeSpan = new TimeSpan(5, 0, 0);
                        var removeTargets = m_dicJavasInspectionItems.Where(item => (DateTime.Now - item.Value.InstanceCreateTime) > overTimeSpan).ToList();
                        foreach (var item in removeTargets)
                        {
                            m_dicJavasInspectionItems.Remove(item.Key);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 입력된 Cell ID의 자바스 택타임 아이템에 대한 로그를 남기고 데이터를 삭제함 (오래된 데이터 삭제)
        /// </summary>
        /// <param name="objDocument">CDocument의 객체</param>
        /// <param name="strCellID">Cell ID</param>
        public void SetJavasCycleTactLog(CDocument objDocument, string strCellID)
        {
            lock (m_dicJavasInspectionItems)
            {
                if (true == m_dicJavasInspectionItems.ContainsKey(strCellID))
                {
                    // Calulate Wait Time
                    {
                        DateTime StartTime, EndTime;
                        double TactTime, WaitTime;

                        // INSPECTION TACT TIME
                        StartTime = m_dicJavasInspectionItems[strCellID].dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_START_START_TIME].GetTime();
                        EndTime = m_dicJavasInspectionItems[strCellID].dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_RESULT_TIME].GetTime();
                        TactTime = objDocument.CountTactTime(StartTime, EndTime);
                        m_dicJavasInspectionItems[strCellID].dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB].m_strTime = TactTime.ToString();

                        // GRAB START WAITING TACT
                        StartTime = m_dicJavasInspectionItems[strCellID].dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_START_START_TIME].GetTime();
                        EndTime = m_dicJavasInspectionItems[strCellID].dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_START_END_TIME].GetTime();
                        WaitTime = objDocument.CountTactTime(StartTime, EndTime);
                        m_dicJavasInspectionItems[strCellID].dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_START_WAITING].m_strTime = WaitTime.ToString();

                        // GRAB END WAITING TACT
                        StartTime = m_dicJavasInspectionItems[strCellID].dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_END_START_TIME].GetTime();
                        EndTime = m_dicJavasInspectionItems[strCellID].dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_END_END_TIME].GetTime();
                        WaitTime = objDocument.CountTactTime(StartTime, EndTime);
                        m_dicJavasInspectionItems[strCellID].dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_END_WAITING].m_strTime = WaitTime.ToString();

                        // RESULT WAITING TACT
                        StartTime = m_dicJavasInspectionItems[strCellID].dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_END_END_TIME].GetTime();
                        EndTime = m_dicJavasInspectionItems[strCellID].dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_RESULT_TIME].GetTime();
                        WaitTime = objDocument.CountTactTime(StartTime, EndTime);
                        m_dicJavasInspectionItems[strCellID].dicTactTime[CDefine.ECycleTactJavasInspection.INSP_RESULT_WAITING].m_strTime = WaitTime.ToString();
                    }
                    // Write Log
                    {
                        Dictionary<string, string> tactTimeDetail = new Dictionary<string, string>();
                        var sbLog = new StringBuilder();
                        foreach (CDefine.ECycleTactJavasInspection column in Enum.GetValues(typeof(CDefine.ECycleTactJavasInspection)))
                        {
                            switch (column)
                            {
                                case CDefine.ECycleTactJavasInspection.INNER_ID:
                                    sbLog.AppendFormat("{0},", m_dicJavasInspectionItems[strCellID].strInnerID);
                                    tactTimeDetail[column.ToString()] = m_dicJavasInspectionItems[strCellID].strInnerID;
                                    break;
                                case CDefine.ECycleTactJavasInspection.CELL_ID:
                                    sbLog.AppendFormat("{0},", m_dicJavasInspectionItems[strCellID].strCellID);
                                    tactTimeDetail[column.ToString()] = m_dicJavasInspectionItems[strCellID].strCellID;
                                    break;
                                case CDefine.ECycleTactJavasInspection.INSP_GRAB_START_START_TIME:
                                case CDefine.ECycleTactJavasInspection.INSP_GRAB_START_END_TIME:
                                case CDefine.ECycleTactJavasInspection.INSP_GRAB_END_START_TIME:
                                case CDefine.ECycleTactJavasInspection.INSP_GRAB_END_END_TIME:
                                case CDefine.ECycleTactJavasInspection.INSP_GRAB_RESULT_TIME:
                                case CDefine.ECycleTactJavasInspection.INSP_GRAB:
                                case CDefine.ECycleTactJavasInspection.INSP_GRAB_START_WAITING:
                                case CDefine.ECycleTactJavasInspection.INSP_GRAB_END_WAITING:
                                case CDefine.ECycleTactJavasInspection.INSP_RESULT_WAITING:
                                    sbLog.AppendFormat("{0},", m_dicJavasInspectionItems[strCellID].dicTactTime[column].m_strTime);
                                    tactTimeDetail[column.ToString()] = m_dicJavasInspectionItems[strCellID].dicTactTime[column].m_strTime;
                                    break;
                                case CDefine.ECycleTactJavasInspection.INSP_CLASS:
                                    sbLog.AppendFormat("{0},", m_dicJavasInspectionItems[strCellID].strInspection_Class);
                                    tactTimeDetail[column.ToString()] = m_dicJavasInspectionItems[strCellID].strInspection_Class;
                                    break;
                                case CDefine.ECycleTactJavasInspection.INSP_DEFECT:
                                    sbLog.AppendFormat("{0},", m_dicJavasInspectionItems[strCellID].strInspection_Defect);
                                    tactTimeDetail[column.ToString()] = m_dicJavasInspectionItems[strCellID].strInspection_Defect;
                                    break;
                                case CDefine.ECycleTactJavasInspection.RAWDATA:
                                    sbLog.AppendFormat(",");
                                    break;
                                default:
                                    Debug.Assert(false);
                                    break;
                            }
                        }
                        objDocument.SetUpdateLog(m_tactJavasInspection.LogType, sbLog.ToString());
                        objDocument.m_objTactTime.UnitTactTimeDetailInformation[ETactTime.JAVAS_INSPECTION] = tactTimeDetail;
                    }
                    // 로그 저장을 마친 아이템은 딕셔너리에서 삭제
                    m_dicJavasInspectionItems.Remove(strCellID);
                }
            }
        }
    }
}