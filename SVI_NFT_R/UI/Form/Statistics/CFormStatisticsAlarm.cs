using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormStatisticsAlarm : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private readonly CDocument m_objDocument;
        /// <summary>
        /// 알람 리스트 칼럼 정의
        /// </summary>
        private enum EAlarmListColumn
        {
            DATE = 0,
            ID,
            UNIT,
            LEVEL,
            ALARM_TEXT,
            ALARM_LIST_COLUMN_FINAL
        }
        /// <summary>
        /// 알람 필터 정의
        /// </summary>
        private enum EAlarmFilter
        {
            HEAVY,
            LIGHT,
            SILENCE,
            ALL
        }
        /// <summary>
        /// 해당 날짜로 검색된 데이터를 갖고 있음
        /// </summary>
        private DataTable m_objDataTable;
        private DataTable m_objDataGroupTable;
        private DataRow[] m_objDataRow;
        private DataRow[] m_objDataGroupRow;
        /// <summary>
        /// 알람 랭킹 보기 모드인지를 확인하기 위한 변수
        /// </summary>
        private bool m_bViewRanking;
        /// <summary>
        /// 알람 필터링 옵션
        /// </summary>
        private EAlarmFilter m_eAlarmFilter;
        private string m_strLastAlign;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        public CFormStatisticsAlarm(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeAutoScale();
        }


        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormStatisticsAlarm_Load(object sender, EventArgs e)
        {
            // 내부 옵션 초기화
            m_bViewRanking = true;
            m_eAlarmFilter = EAlarmFilter.HEAVY;
            m_strLastAlign = CDatabaseDefine.DEF_DESC;

            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormStatisticsAlarm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 해제
            DeInitialize();
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
                // 폼 초기화
                if (false == InitializeForm())
                {
                    break;
                }

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
        /// 폼 초기화
        /// </summary>
        /// <returns></returns>
        public bool InitializeForm()
        {
            bool bReturn = false;

            do
            {
                // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
                base.m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
                // 데이터베이스 현재 날짜로 초기화 & 데이터베이스 연동
                if (false == InitializeDatabase())
                {
                    break;
                }
                // Alarm 리스트 그리드 뷰 초기화
                if (false == InitializeGridView(GridViewAlarmList))
                {
                    break;
                }
                // From Date 초기화
                if (false == InitializeDateTime(DateTimeFrom))
                {
                    break;
                }
                // To Date 초기화
                if (false == InitializeDateTime(DateTimeTo))
                {
                    break;
                }
                // 버튼 색상 정의
                SetButtonColor();
                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 셀의 값을 한 페이지에서만 보여지도록 처리하기 위해..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewAlarmList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            do
            {
                var captureData = m_objDataRow;
                if (captureData == null)
                {
                    return;
                }

                try
                {
                    if (
                        0 == captureData.Length
                        || captureData.Length <= e.RowIndex
                        )
                    {
                        break;
                    }
                    e.Value = captureData[e.RowIndex].ItemArray[e.ColumnIndex];
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            SetButtonBackColor(BtnSelectAsc, m_colorNormal);
            SetButtonBackColor(BtnSelectDesc, m_colorNormal);
            SetButtonBackColor(BtnSaveToCsv, m_colorNormal);
            SetButtonBackColor(BtnOptionAllAlarm, m_colorNormal);
            SetButtonBackColor(BtnOptionHeavyAlarm, m_colorNormal);
            SetButtonBackColor(BtnOptionLightAlarm, m_colorNormal);
            SetButtonBackColor(BtnOptionSilenceAlarm, m_colorNormal);
            SetButtonBackColor(BtnOptionViewRanking, m_colorNormal);
            SetButtonBackColor(BtnOptionViewGrouping, m_colorNormal);
        }

        /// <summary>
        /// 설비 상태 or 권한 레벨에 따라 자원 상태 변경
        /// </summary>
        private void SetResourceControl()
        {
            // 현재 유저 권한 레벨 받아옴
            CUserInformation objUserInformation = m_objDocument.GetUserInformation();

            // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
            // 권한 레벨에 따라 원하는 방향으로 사용
            switch (objUserInformation.m_eAuthorityLevel)
            {
                case CDefine.EUserAuthorityLevel.OPERATOR:
                    base.SetControlButtonEnable(Controls, true);
                    break;
                case CDefine.EUserAuthorityLevel.ENGINEER:
                    base.SetControlButtonEnable(Controls, true);
                    break;
                case CDefine.EUserAuthorityLevel.MASTER:
                    base.SetControlButtonEnable(Controls, true);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <returns></returns>
        public bool SetChangeLanguage()
        {
            bool bReturn = false;

            do
            {
                // 데이터 테이블에서 일치하는 ID에 해당하는 TEXT를 불러옴
                SetButtonChangeLanguage(BtnSelectAsc);
                SetButtonChangeLanguage(BtnSelectDesc);
                SetButtonChangeLanguage(BtnSaveToCsv);
                SetButtonChangeLanguage(BtnOptionAllAlarm);
                SetButtonChangeLanguage(BtnOptionHeavyAlarm);
                SetButtonChangeLanguage(BtnOptionLightAlarm);
                SetButtonChangeLanguage(BtnOptionSilenceAlarm);
                SetButtonChangeLanguage(BtnOptionViewRanking);
                SetButtonChangeLanguage(BtnOptionViewGrouping);

                // Alarm History 데이터베이스 연동
                SetAlarmHistoryConnect(DateTimeFrom.Value, DateTimeTo.Value);
                // DB 리스트 그리드 뷰 초기화
                InitializeGridView(GridViewAlarmList);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(Button objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        /// <summary>
        /// 타이머 유무
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            timer.Enabled = bTimer;
        }

        /// <summary>
        /// Visible 유무
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
            Visible = bVisible;

            if (true == bVisible)
            {
                if (MtbiDataCollector.IsStarted == true)
                {
                    // MTBi 데이터 수집기가 동작중일 때 초기 파라미터 설정
                    m_bViewRanking = false;
                    m_eAlarmFilter = EAlarmFilter.ALL;
                    m_strLastAlign = CDatabaseDefine.DEF_DESC;
                }

                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                m_objDocument.GetMainFrame().SetCurrentForm(this);
                // 데이터베이스 현재 날짜로 초기화 & 데이터베이스 연동
                InitializeDatabase();
            }
            else
            {
                // 가비지 컬렉터에서 메모리를 회수해 갈 수 있도록 참조를 해제한다.
                m_objDataTable = null;
                m_objDataRow = null;
            }
        }

        /// <summary>
        /// 데이터베이스 현재 날짜로 초기화 & 데이터베이스 연동
        /// </summary>
        /// <returns></returns>
        private bool InitializeDatabase()
        {
            bool bReturn = false;

            do
            {
                // 날짜 선택 컨트롤을 현재 날짜로 설정
                DateTimeFrom.Value = DateTime.Today;
                DateTimeTo.Value = DateTime.Today;
                // Alarm History 데이터베이스 연동
                SetAlarmHistoryConnect(DateTimeFrom.Value, DateTimeTo.Value);
                // DB 리스트 그리드 뷰 초기화
                InitializeGridView(GridViewAlarmList);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 그리드 뷰 초기화
        /// </summary>
        /// <param name="objGridView"></param>
        /// <returns></returns>
        private new bool InitializeGridView(DataGridView objGridView)
        {
            bool bReturn = false;

            do
            {
                // 그리드 뷰 행 열 값 날려줌
                objGridView.Rows.Clear();
                objGridView.Columns.Clear();
                // 그리드 뷰 기본 스타일 초기화
                if (false == base.InitializeGridView(objGridView))
                {
                    break;
                }
                // 그리드 뷰 칼럼 사이즈 (성능상 문제가 있어서 사이즈는 고정으로 픽스)
                objGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                // 그리드 뷰 ReadOnly
                objGridView.ReadOnly = true;
                // 그리드 뷰 다중 선택 x
                objGridView.MultiSelect = false;
                // 가상 모드로 사용해서 빠른 처리
                objGridView.VirtualMode = true;
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                objGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                // 그리드 뷰 칼럼 추가
                for (int iLoopColumn = 0; iLoopColumn < m_objDataTable.Columns.Count; iLoopColumn++)
                {
                    objGridView.Columns.Add(m_objDataTable.Columns[iLoopColumn].ToString(), m_objDataTable.Columns[iLoopColumn].ToString());
                }
                // 그리드 뷰 칼럼 정렬 x
                for (int iLoopColumn = 0; iLoopColumn < objGridView.Columns.Count; iLoopColumn++)
                {
                    objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                    objGridView.Columns[iLoopColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                // DATE, ID 사이즈 조정
                objGridView.Columns[(int)EAlarmListColumn.DATE].Width = (int)(objGridView.Width * 0.12);
                objGridView.Columns[(int)EAlarmListColumn.ID].Width = (int)(objGridView.Width * 0.05);
                objGridView.Columns[(int)EAlarmListColumn.UNIT].Width = (int)(objGridView.Width * 0.1);
                objGridView.Columns[(int)EAlarmListColumn.LEVEL].Width = (int)(objGridView.Width * 0.05);
                objGridView.Columns[(int)EAlarmListColumn.ALARM_TEXT].Width = (int)(objGridView.Width * 0.7);
                objGridView.Columns[(int)EAlarmListColumn.ALARM_TEXT].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                // 전체 행 수를 잡아줌
                objGridView.RowCount = m_objDataRow.Length;
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, 9.0);

                // 알람을 그룹화해서 표시한다
                // (알람이 리셋된 시간을 받아와서 비교하여 그룹으로 나눈다)
                if (
                    false == m_bViewRanking
                    && EAlarmFilter.LIGHT != m_eAlarmFilter
                    )
                {
                    DateTime eventDateTime = DateTime.Now;
                    if (CDatabaseDefine.DEF_ASC == m_strLastAlign)
                    {
                        int eventRowIndex = 0;
                        int eventSkipRowCount = 0;
                        if (0 != m_objDataGroupRow.Length)
                        {
                            // 클리어 리스트가 없을 때에대한 예외처리
                            eventDateTime = Convert.ToDateTime(m_objDataGroupRow[eventRowIndex][(int)CDatabaseDefine.EHistoryAlarmEvent.DATE]);
                        }
                        for (int iRowIndex = 0; iRowIndex < m_objDataRow.Length; iRowIndex++)
                        {
                            DateTime alarmDateTime = Convert.ToDateTime(m_objDataRow[iRowIndex][(int)EAlarmListColumn.DATE]);
                            if (alarmDateTime > eventDateTime)
                            {
                                if (++eventRowIndex >= m_objDataGroupRow.Length)
                                {
                                    // 알람 클리어를 하지 않았을 때에대한 예외처리
                                    eventDateTime = DateTime.Now;
                                }
                                else
                                {
                                    eventDateTime = Convert.ToDateTime(m_objDataGroupRow[eventRowIndex][(int)CDatabaseDefine.EHistoryAlarmEvent.DATE]);
                                    if (0 < eventRowIndex)
                                    {
                                        // 날짜가 넘어 갈때 이벤트에대한 예외처리
                                        if ("23:59:59.999" == Convert.ToDateTime(m_objDataGroupRow[eventRowIndex][(int)CDatabaseDefine.EHistoryAlarmEvent.DATE]).ToString("HH:mm:ss.fff"))
                                        {
                                            eventSkipRowCount++;
                                        }
                                    }
                                }
                            }
                            Color backColor = ((eventRowIndex - eventSkipRowCount) % 2) == 0 ? Color.LightSteelBlue : Color.White; // AntiqueWhite
                            objGridView.Rows[iRowIndex].DefaultCellStyle.BackColor = backColor;
                        }
                    }
                    else
                    {
                        int eventRowIndex = m_objDataGroupRow.Length - 1;
                        int eventSkipRowCount = 0;
                        if (0 != m_objDataGroupRow.Length)
                        {
                            // 클리어 리스트가 없을 때에대한 예외처리
                            eventDateTime = Convert.ToDateTime(m_objDataGroupRow[eventRowIndex][(int)CDatabaseDefine.EHistoryAlarmEvent.DATE]);
                        }
                        for (int iRowIndex = m_objDataRow.Length - 1; iRowIndex >= 0; iRowIndex--)
                        {
                            DateTime alarmDateTime = Convert.ToDateTime(m_objDataRow[iRowIndex][(int)EAlarmListColumn.DATE]);
                            if (alarmDateTime > eventDateTime)
                            {
                                if (--eventRowIndex < 0)
                                {
                                    // 알람 클리어를 하지 않았을 때에대한 예외처리
                                    eventDateTime = DateTime.Now;
                                }
                                else
                                {
                                    eventDateTime = Convert.ToDateTime(m_objDataGroupRow[eventRowIndex][(int)CDatabaseDefine.EHistoryAlarmEvent.DATE]);
                                    if ((m_objDataRow.Length - 1) > iRowIndex)
                                    {
                                        // 날짜가 넘어 갈때 이벤트에대한 예외처리                 
                                        if ("23:59:59.999" == Convert.ToDateTime(m_objDataGroupRow[eventRowIndex][(int)CDatabaseDefine.EHistoryAlarmEvent.DATE]).ToString("HH:mm:ss.fff"))
                                        {
                                            eventSkipRowCount++;
                                        }
                                    }
                                }
                            }
                            Color backColor = ((m_objDataGroupRow.Length - 1 - eventRowIndex - eventSkipRowCount) % 2) == 0 ? Color.LightSteelBlue : Color.White;
                            objGridView.Rows[iRowIndex].DefaultCellStyle.BackColor = backColor;
                        }
                    }
                }

                // 그리드 선택 클리어
                objGridView.ClearSelection();

                bReturn = true;
            } while (false);

            return bReturn;
        }


        /// <summary>
        /// 데이트 타임 피커 초기화
        /// </summary>
        /// <param name="objDateTime"></param>
        /// <returns></returns>
        private bool InitializeDateTime(DateTimePicker objDateTime)
        {
            bool bReturn = false;

            do
            {
                // 커스텀 포멧 설정
                objDateTime.CustomFormat = CDatabaseDefine.DEF_DATE_FORMAT;
                objDateTime.Format = DateTimePickerFormat.Custom;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 알람 이력 데이터베이스 연동
        /// </summary>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        private void SetAlarmHistoryConnect(DateTime objFrom, DateTime objTo)
        {
            if (string.IsNullOrEmpty(m_strLastAlign))
            {
                m_strLastAlign = CDatabaseDefine.DEF_DESC;
            }
            // 디폴트 내림차순
            SetAlarmHistoryConnect(objFrom, objTo, m_strLastAlign);
        }

        /// <summary>
        /// 알람 이력 데이터베이스 연동
        /// </summary>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        /// <param name="strOrder"></param>
        private void SetAlarmHistoryConnect(DateTime objFrom, DateTime objTo, string strOrder)
        {
            string strQuery = null;
            string strAlarmEventQuery = null;
            string strFilterExpression = null;
            string strSort = null;
            string strAlarmEventSort = null;
            // History Database
            CProcessDatabaseHistory objProcessDatabaseHistory = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory;
            // Information Database
            CProcessDatabaseInformation objProcessDatabaseInformation = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation;

            try
            {
                // 언어별로 알람 텍스트가 바뀌어야함
                int iAlarmTextColumnIndex = 0;
                if (CDefine.ELanguage.LANGUAGE_KOREA == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
                {
                    iAlarmTextColumnIndex = (int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_KOREA;
                }
                else if (CDefine.ELanguage.LANGUAGE_CHINA == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
                {
                    iAlarmTextColumnIndex = (int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_CHINA;
                }
                else if (CDefine.ELanguage.LANGUAGE_ENGLISH == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
                {
                    iAlarmTextColumnIndex = (int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_ENGLISH;
                }
                else if (CDefine.ELanguage.LANGUAGE_VIETNAM == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
                {
                    iAlarmTextColumnIndex = (int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_VIETNAM;
                }

                if (false == m_bViewRanking)
                {
                    strQuery = string.Format("select {0}, {1} from {2} where {0} between strftime('%Y-%m-%d %H:%M:%S.%f','{3}') and strftime('%Y-%m-%d %H:%M:%S.%f','{4}')",
                        objProcessDatabaseHistory.m_objManagerTableHistoryAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryAlarm.DATE],
                        objProcessDatabaseHistory.m_objManagerTableHistoryAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryAlarm.ID],
                        objProcessDatabaseHistory.m_objManagerTableHistoryAlarm.HLGetTableName(),
                        string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                        string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT)));

                    strAlarmEventQuery = string.Format("select * from {0} where {1} between strftime('%Y-%m-%d %H:%M:%S.%f','{2}') and strftime('%Y-%m-%d %H:%M:%S.%f','{3}') and {4} = 'CLEAR';",
                        objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent.HLGetTableName(),
                        objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryAlarmEvent.DATE],
                        string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                        string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                        objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryAlarmEvent.EVENT]);
                }
                else
                {
                    strQuery = string.Format("select {0}, COUNT(*) count from {1} where DATE between strftime('%Y-%m-%d %H:%M:%S.%f','{2}') and strftime('%Y-%m-%d %H:%M:%S.%f','{3}') group by {4} order by count {5}",
                        objProcessDatabaseHistory.m_objManagerTableHistoryAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryAlarm.ID],
                        objProcessDatabaseHistory.m_objManagerTableHistoryAlarm.HLGetTableName(),
                        string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                        string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                        objProcessDatabaseHistory.m_objManagerTableHistoryAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryAlarm.ID],
                        strOrder);
                }
                // History Alarm Select 연산으로 꺼낸 DataTable
                DataTable objDataTableHistoryAlarm = new DataTable();
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(strQuery, ref objDataTableHistoryAlarm);
                // Information Alarm 전체 Select 연산으로 갖고 있는 DataTable
                DataTable objDataTableInformationAlarm = objProcessDatabaseInformation.m_objManagerTableInformationAlarm.HLGetDataTable();
                // 새로운 DataTable 생성 (inner Join 연산)
                // 현재 서로 다른 데이터베이스에 조인 연산이 안되는 관계로... shell 환경에서는 되는데 프로그램에서는 왜 안되는지 모르겠음.....
                // 그래서 부득이하게 서로 다른 데이터베이스 내 데이터 테이블을 Join 하여 사용
                DataTable objDataTableJoin = new DataTable();
                // 데이터 테이블에 칼럼명을 정의
                objDataTableJoin.Columns.Add(objDataTableHistoryAlarm.Columns[(int)CDatabaseDefine.EHistoryAlarm.DATE].ColumnName.ToUpper());
                objDataTableJoin.Columns.Add(objDataTableHistoryAlarm.Columns[(int)CDatabaseDefine.EHistoryAlarm.ID].ColumnName.ToUpper());
                objDataTableJoin.Columns.Add(objDataTableInformationAlarm.Columns[(int)CDatabaseDefine.EInformationAlarm.UNIT].ColumnName.ToUpper());
                objDataTableJoin.Columns.Add(objDataTableInformationAlarm.Columns[(int)CDatabaseDefine.EInformationAlarm.LEVEL].ColumnName.ToUpper());
                objDataTableJoin.Columns.Add(objDataTableInformationAlarm.Columns[iAlarmTextColumnIndex].ColumnName.ToUpper());
                // 데이터 테이블에 레코드를 삽입
                for (int iLoopRow = 0; iLoopRow < objDataTableHistoryAlarm.Rows.Count; iLoopRow++)
                {
                    // Information Alarm 테이블에서 History Alarm에 ID값을 Key로 삼아서 해당 레코드를 뽑아냄    
                    DataRow[] objDataRow = null;
                    if (false == m_bViewRanking)
                    {
                        objDataRow = objDataTableInformationAlarm.Select(
                            string.Format("{0} = {1}",
                                objProcessDatabaseInformation.m_objManagerTableInformationAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationAlarm.ID],
                                objDataTableHistoryAlarm.Rows[iLoopRow].ItemArray[(int)CDatabaseDefine.EHistoryAlarm.ID]));
                    }
                    else
                    {
                        // 알람 랭킹의 컬럼은 ID, count 임으로 맞춰줌
                        objDataRow = objDataTableInformationAlarm.Select(
                            string.Format("{0} = {1}",
                                objProcessDatabaseInformation.m_objManagerTableInformationAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationAlarm.ID],
                                objDataTableHistoryAlarm.Rows[iLoopRow].ItemArray[0]));
                    }
                    // 원하는 칼럼에 해당하는 레코드 값만 삽입 (등록되지 않은 알람 코드 예외처리)
                    if (0 != objDataRow.Length)
                    {
                        objDataTableJoin.Rows.Add(
                        objDataTableHistoryAlarm.Rows[iLoopRow].ItemArray[(int)CDatabaseDefine.EHistoryAlarm.DATE],
                        objDataTableHistoryAlarm.Rows[iLoopRow].ItemArray[(int)CDatabaseDefine.EHistoryAlarm.ID],
                        objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationAlarm.UNIT],
                        objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationAlarm.LEVEL],
                        $"[{objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationAlarm.ALTX]}]");
                        //$"[{objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationAlarm.ALTX]}] {objDataRow[0].ItemArray[iAlarmTextColumnIndex]}");
                    }
                    else
                    {
                        objDataTableJoin.Rows.Add(
                        objDataTableHistoryAlarm.Rows[iLoopRow].ItemArray[(int)CDatabaseDefine.EHistoryAlarm.DATE],
                        objDataTableHistoryAlarm.Rows[iLoopRow].ItemArray[(int)CDatabaseDefine.EHistoryAlarm.ID],
                        "UNREGISTED ALARM CODE",
                        "UNREGISTED ALARM CODE",
                        "UNREGISTED ALARM CODE");
                    }
                }

                // 알람 필터링
                switch (m_eAlarmFilter)
                {
                    case EAlarmFilter.HEAVY:
                    case EAlarmFilter.LIGHT:
                        strFilterExpression = string.Format("[{0}] = '{1}'", objProcessDatabaseInformation.m_objManagerTableInformationAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationAlarm.LEVEL], m_eAlarmFilter.ToString());
                        break;
                    case EAlarmFilter.SILENCE:
                        strFilterExpression = string.Format("{0} = '{1}'", objProcessDatabaseInformation.m_objManagerTableInformationAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationAlarm.ID], (int)CAlarmDefine.EAlarmList.EF_PREFORM4_MAIN_EQUIPMENT_SILENT_STOP);
                        break;
                    case EAlarmFilter.ALL:
                        strFilterExpression = string.Empty;
                        break;
                    default:
                        break;
                }
                // 알람 정렬
                if (false == m_bViewRanking)
                {
                    strSort = string.Format("{0} {1}", objProcessDatabaseHistory.m_objManagerTableHistoryAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryAlarm.DATE], strOrder);
                    strAlarmEventSort = string.Format("{0} {1}", objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryAlarmEvent.DATE], strOrder);
                }

                // 알람 필터링 & 정렬 적용
                m_objDataTable = new DataTable();
                m_objDataTable = objDataTableJoin.Copy();
                if (false == m_bViewRanking)
                {
                    m_objDataRow = m_objDataTable.Select(strFilterExpression, strSort);
                }
                else
                {
                    m_objDataRow = m_objDataTable.Select(strFilterExpression);
                }

                if (false == m_bViewRanking)
                {
                    // 연속 알람 그룹화 설정 
                    m_objDataGroupTable = new DataTable();
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(strAlarmEventQuery, ref m_objDataGroupTable);
                    m_objDataGroupRow = m_objDataGroupTable.Select(string.Empty, strAlarmEventSort);
                }
                else
                {
                    // 그룹화 해제
                    m_objDataGroupTable = null;
                    m_objDataGroupRow = null;
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }
        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            switch (m_eAlarmFilter)
            {
                case EAlarmFilter.HEAVY:
                    SetButtonBackColor(BtnOptionAllAlarm, m_colorNormal);
                    SetButtonBackColor(BtnOptionHeavyAlarm, m_colorClick);
                    SetButtonBackColor(BtnOptionLightAlarm, m_colorNormal);
                    SetButtonBackColor(BtnOptionSilenceAlarm, m_colorNormal);
                    break;
                case EAlarmFilter.LIGHT:
                    SetButtonBackColor(BtnOptionAllAlarm, m_colorNormal);
                    SetButtonBackColor(BtnOptionHeavyAlarm, m_colorNormal);
                    SetButtonBackColor(BtnOptionLightAlarm, m_colorClick);
                    SetButtonBackColor(BtnOptionSilenceAlarm, m_colorNormal);
                    break;
                case EAlarmFilter.SILENCE:
                    SetButtonBackColor(BtnOptionAllAlarm, m_colorNormal);
                    SetButtonBackColor(BtnOptionHeavyAlarm, m_colorNormal);
                    SetButtonBackColor(BtnOptionLightAlarm, m_colorNormal);
                    SetButtonBackColor(BtnOptionSilenceAlarm, m_colorClick);
                    break;
                case EAlarmFilter.ALL:
                    SetButtonBackColor(BtnOptionAllAlarm, m_colorClick);
                    SetButtonBackColor(BtnOptionHeavyAlarm, m_colorNormal);
                    SetButtonBackColor(BtnOptionLightAlarm, m_colorNormal);
                    SetButtonBackColor(BtnOptionSilenceAlarm, m_colorNormal);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            if (m_bViewRanking)
            {
                SetButtonBackColor(BtnOptionViewRanking, m_colorClick);
            }
            else
            {
                SetButtonBackColor(BtnOptionViewRanking, m_colorNormal);
            }

            if (CDatabaseDefine.DEF_ASC == m_strLastAlign)
            {
                SetButtonBackColor(BtnSelectAsc, m_colorClick);
                SetButtonBackColor(BtnSelectDesc, m_colorNormal);
            }
            else
            {
                SetButtonBackColor(BtnSelectAsc, m_colorNormal);
                SetButtonBackColor(BtnSelectDesc, m_colorClick);
            }

            if (
                false == m_bViewRanking
                && EAlarmFilter.LIGHT != m_eAlarmFilter
                )
            {
                SetButtonBackColor(BtnOptionViewGrouping, m_colorClick);
            }
            else
            {
                SetButtonBackColor(BtnOptionViewGrouping, m_colorNormal);
            }
        }

        /// <summary>
        /// From 날짜 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTimeFrom_ValueChanged(object sender, EventArgs e)
        {
            // Form 날짜는 To 날짜보다 이후 출력이 되면 안됨
            // To 날짜도 갱신되도록 함
            DateTimePicker objFrom = DateTimeFrom;
            DateTimePicker objTo = DateTimeTo;
            // From 날짜가 To 날짜보다 더 크면 To 날짜 갱신
            if (objFrom.Value > objTo.Value)
            {
                objTo.Value = objFrom.Value;
            }

            // Alarm History 데이터베이스 연동
            SetAlarmHistoryConnect(DateTimeFrom.Value, DateTimeTo.Value, m_strLastAlign);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(GridViewAlarmList);
        }

        /// <summary>
        /// To 날짜 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTimeTo_ValueChanged(object sender, EventArgs e)
        {
            // To 날짜는 From 날짜보다 이전 출력이 되면 안됨
            // From 날짜도 갱신되도록 함
            DateTimePicker objFrom = DateTimeFrom;
            DateTimePicker objTo = DateTimeTo;
            // To 날짜가 From 날짜보다 적으면 From 날짜 갱신
            if (objTo.Value < objFrom.Value)
            {
                objFrom.Value = objTo.Value;
            }

            // Alarm History 데이터베이스 연동
            SetAlarmHistoryConnect(DateTimeFrom.Value, DateTimeTo.Value, m_strLastAlign);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(GridViewAlarmList);
        }

        /// <summary>
        /// SAVE TO CSV FILE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveToCsv_Click(object sender, EventArgs e)
        {
            // DataTable 받아서 Csv로 저장
            CCsvFile objCsvFile = new CCsvFile();
            // 현재 경로 폴더 확인 & 생성
            string strPath = string.Format(@"{0}\Database", m_objDocument.m_objConfig.GetLogPath());
            DirectoryInfo objDirectory = new DirectoryInfo(strPath);
            if (false == objDirectory.Exists)
            {
                objDirectory.Create();
            }
            // 현재 경로에 테이블 이름.csv 로 파일 생성
            if (false == m_bViewRanking)
            {
                strPath = string.Format(@"{0}\{1}_{2}_{3}.csv", strPath, m_eAlarmFilter.ToString(), "ALARM", string.Format("{0:yyyyMMdd_HHmm}", DateTime.Now));
            }
            else
            {
                strPath = string.Format(@"{0}\{1}_{2}_{3}.csv", strPath, m_eAlarmFilter.ToString(), "ALARM_RANKING", string.Format("{0:yyyyMMdd_HHmm}", DateTime.Now));
            }
            objCsvFile.SetDataTableToCsv(strPath, m_objDataTable);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Path : {1}]", "BtnSaveToCsv_Click", strPath);
            m_objDocument.SetUpdateButtonLog(this, strLog);
            MessageBox.Show(string.Format("Path : {0}", strPath));

            m_objDocument.RunCellLogToExcel(strPath);
        }

        /// <summary>
        /// SELECT (ASC)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelectAsc_Click(object sender, EventArgs e)
        {
            m_strLastAlign = CDatabaseDefine.DEF_ASC;

            // Alarm History 데이터베이스 연동
            SetAlarmHistoryConnect(DateTimeFrom.Value, DateTimeTo.Value, m_strLastAlign);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(GridViewAlarmList);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [From : {1}] [To : {2}] [Align : {3}]", "BtnSelectAsc_Click", DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), m_strLastAlign);
            m_objDocument.SetUpdateButtonLog(this, strLog);

        }

        /// <summary>
        /// SELECT (DESC)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelectDesc_Click(object sender, EventArgs e)
        {
            m_strLastAlign = CDatabaseDefine.DEF_DESC;

            // Alarm History 데이터베이스 연동
            SetAlarmHistoryConnect(DateTimeFrom.Value, DateTimeTo.Value, m_strLastAlign);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(GridViewAlarmList);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [From : {1}] [To : {2}] [Align : {3}]", "BtnSelectDesc_Click", DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), m_strLastAlign);
            m_objDocument.SetUpdateButtonLog(this, strLog);

        }

        /// <summary>
        /// 알람 순위 작성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlarmRanking_Click(object sender, EventArgs e)
        {
            m_bViewRanking = !m_bViewRanking;

            // Alarm History 데이터베이스 연동
            SetAlarmHistoryConnect(DateTimeFrom.Value, DateTimeTo.Value, m_strLastAlign);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(GridViewAlarmList);

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [From : {1}] [To : {2}] [Align : {3}] [RankingMode : {4}]", "BtnAlarmRanking_Click", DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), m_strLastAlign, m_bViewRanking);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnOptionHeavyAlarm_Click(object sender, EventArgs e)
        {
            m_eAlarmFilter = EAlarmFilter.HEAVY;

            // Alarm History 데이터베이스 연동
            SetAlarmHistoryConnect(DateTimeFrom.Value, DateTimeTo.Value, m_strLastAlign);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(GridViewAlarmList);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [From : {1}] [To : {2}] [Align : {3}]", "BtnOptionHeavyAlarm_Click", DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), m_strLastAlign);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnOptionLightAlarm_Click(object sender, EventArgs e)
        {
            m_eAlarmFilter = EAlarmFilter.LIGHT;

            // Alarm History 데이터베이스 연동
            SetAlarmHistoryConnect(DateTimeFrom.Value, DateTimeTo.Value, m_strLastAlign);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(GridViewAlarmList);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [From : {1}] [To : {2}] [Align : {3}]", "BtnOptionLightAlarm_Click", DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), m_strLastAlign);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnOptionSilenceAlarm_Click(object sender, EventArgs e)
        {
            m_eAlarmFilter = EAlarmFilter.SILENCE;

            // Alarm History 데이터베이스 연동
            SetAlarmHistoryConnect(DateTimeFrom.Value, DateTimeTo.Value, m_strLastAlign);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(GridViewAlarmList);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [From : {1}] [To : {2}] [Align : {3}]", "BtnOptionSilenceAlarm_Click", DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), m_strLastAlign);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnOptionAllAlarm_Click(object sender, EventArgs e)
        {
            m_eAlarmFilter = EAlarmFilter.ALL;

            // Alarm History 데이터베이스 연동
            SetAlarmHistoryConnect(DateTimeFrom.Value, DateTimeTo.Value, m_strLastAlign);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(GridViewAlarmList);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [From : {1}] [To : {2}] [Align : {3}]", "BtnOptionAllAlarm_Click", DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), m_strLastAlign);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 그리드 클릭시 선택을 없앰
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewAlarmList_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            GridViewAlarmList.ClearSelection();
        }

        /// <summary>
        /// 그룹화 해서 보기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnViewAlarmGrouping_Click(object sender, EventArgs e)
        {
            if (true == m_bViewRanking)
            {
                m_bViewRanking = false;

                // Alarm History 데이터베이스 연동
                SetAlarmHistoryConnect(DateTimeFrom.Value, DateTimeTo.Value, m_strLastAlign);
                // DB 리스트 그리드 뷰 초기화
                InitializeGridView(GridViewAlarmList);
            }

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [From : {1}] [To : {2}] [Align : {3}] [RankingMode : {4}]", "BtnViewAlarmGrouping_Click", DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), m_strLastAlign, m_bViewRanking);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }
    }
}