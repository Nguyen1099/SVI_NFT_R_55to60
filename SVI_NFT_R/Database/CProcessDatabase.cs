using System;
using System.Globalization;

namespace SVI_NFT_R
{
    public class CProcessDatabase
    {
        /// <summary>도큐먼트 클래스</summary>
        private CDocument m_objDocument;
        /// <summary>데이터베이스 이력 클래스</summary>
        public CProcessDatabaseHistory m_objProcessDatabaseHistory;
        /// <summary>데이터베이스 정보 클래스</summary>
        public CProcessDatabaseInformation m_objProcessDatabaseInformation;
        /// <summary>데이터베이스 쿼리 메세지</summary>
        public CDatabaseSendMessage m_objDatabaseSendMessage;

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public bool Initialize(CDocument objDocument)
        {
            try
            {
                // 도큐먼트 클래스 이어줌
                m_objDocument = objDocument;
                // 데이터베이스 정보 초기화
                m_objProcessDatabaseInformation = new CProcessDatabaseInformation();
                if (false == m_objProcessDatabaseInformation.Initialize(this))
                    return false;
                // 데이터베이스 이력 초기화
                m_objProcessDatabaseHistory = new CProcessDatabaseHistory();
                if (false == m_objProcessDatabaseHistory.Initialize(this))
                    return false;
                // 데이터베이스 히스토리 메세지
                CDatabaseImplementHistory objDatabaseImplementHistory = new CDatabaseImplementHistory(m_objDocument);
                m_objDatabaseSendMessage = new CDatabaseSendMessage(objDatabaseImplementHistory);
            }
            catch (Exception e)
            {
                ExceptionTool.ShowExceptionDialog(e, "Database Initialize fail");
                Environment.Exit(0);
            }

            return true;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            // 데이터베이스 히스토리 메세지 해제
            m_objDatabaseSendMessage.DeInitialize();
            // 데이터베이스 이력 해제
            m_objProcessDatabaseHistory.Deinitialize();
            // 데이터베이스 정보 해제
            m_objProcessDatabaseInformation.Deinitialize();
        }

        /// <summary>
        /// 설정 클래스 얻음
        /// </summary>
        /// <returns></returns>
        public CDocument GetDocument()
        {
            return m_objDocument;
        }

        /// <summary>
        /// 데이터베이스에서 string으로 받은 알람 레벨을 enum으로 변경
        /// </summary>
        /// <param name="strAlarmLevel"></param>
        /// <returns></returns>
        public CDatabaseDefine.EAlarmLevelList GetAlarmLevel(string strAlarmLevel)
        {
            CDatabaseDefine.EAlarmLevelList eAlarmLevelList = CDatabaseDefine.EAlarmLevelList.LIGHT;

            if (CDatabaseDefine.EAlarmLevelList.LIGHT.ToString() == strAlarmLevel)
            {
                eAlarmLevelList = CDatabaseDefine.EAlarmLevelList.LIGHT;
            }
            else if (CDatabaseDefine.EAlarmLevelList.HEAVY.ToString() == strAlarmLevel)
            {
                eAlarmLevelList = CDatabaseDefine.EAlarmLevelList.HEAVY;
            }
            else
            {
                eAlarmLevelList = CDatabaseDefine.EAlarmLevelList.LIGHT;
            }

            return eAlarmLevelList;
        }

        /// <summary>
        /// 데이터베이스에서 string으로 받은 알람 박스 타입을 enum으로 변경
        /// </summary>
        /// <param name="strAlarmBoxType"></param>
        /// <returns></returns>
        public CDatabaseDefine.EAlarmBoxTypeList GetAlarmBoxType(string strAlarmBoxType)
        {
            CDatabaseDefine.EAlarmBoxTypeList eAlarmBoxType = CDatabaseDefine.EAlarmBoxTypeList.SQUARE;

            if (CDatabaseDefine.EAlarmBoxTypeList.SQUARE.ToString() == strAlarmBoxType)
            {
                eAlarmBoxType = CDatabaseDefine.EAlarmBoxTypeList.SQUARE;
            }
            else
            {
                eAlarmBoxType = CDatabaseDefine.EAlarmBoxTypeList.SQUARE;
            }

            return eAlarmBoxType;
        }

        /// <summary>
        /// 데이터베이스에서 string으로 받은 유저 권한 레벨 타입을 enum으로 변경
        /// </summary>
        /// <param name="strUserAuthorityLevel"></param>
        /// <returns></returns>
        public CDefine.EUserAuthorityLevel GetUserAuthorityLevel(string strUserAuthorityLevel)
        {
            CDefine.EUserAuthorityLevel eUserAuthorityLevel = CDefine.EUserAuthorityLevel.OPERATOR;

            if (CDefine.EUserAuthorityLevel.OPERATOR.ToString() == strUserAuthorityLevel)
            {
                eUserAuthorityLevel = CDefine.EUserAuthorityLevel.OPERATOR;
            }
            else if (CDefine.EUserAuthorityLevel.ENGINEER.ToString() == strUserAuthorityLevel)
            {
                eUserAuthorityLevel = CDefine.EUserAuthorityLevel.ENGINEER;
            }
            else if (CDefine.EUserAuthorityLevel.MASTER.ToString() == strUserAuthorityLevel)
            {
                eUserAuthorityLevel = CDefine.EUserAuthorityLevel.MASTER;
            }

            return eUserAuthorityLevel;
        }

        /// <summary>
        /// 시작 시간과 완료 시간 차를 구해서 TimeSpan으로 반환
        /// </summary>
        /// <param name="strStartTime"></param>
        /// <param name="strEndTime"></param>
        /// <returns></returns>
        public string GetDuration(string strStartTime, string strEndTime)
        {
            string strDuration = "";

            do
            {
                if ("" == strStartTime || "" == strEndTime)
                    break;

                DateTime objDateStart = DateTime.ParseExact(strStartTime, CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                DateTime objDateEnd = DateTime.ParseExact(strEndTime, CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);

                strDuration = (objDateEnd - objDateStart).ToString(CDatabaseDefine.DEF_TACT_TIME_FORMAT);
            } while (false);

            return strDuration;
        }

        /// <summary>
        /// DB에 데이터 삽입 조건
        /// </summary>
        /// <returns></returns>
        public bool IsInsertHistory()
        {
            bool bReturn = false;

            do
            {
                //// 시뮬레이션 모드거나 실제런 모드가 아니면 DB에 데이터를 남기지 않는다.
                //if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
                //{
                //    // CIM 사용 모드에는 가상모드에서도 데이터베이스에 자료를 남긴다.
                //    if (false == m_objDocument.m_objConfig.GetOptionParameter().bUseCIM)
                //    {
                //        break;
                //    }
                //}
                //if (CDefine.ERunMode.UnlinkDryrun == m_objDocument.GetRunMode())
                //{
                //    break;
                //}

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}