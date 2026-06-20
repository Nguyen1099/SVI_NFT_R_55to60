using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormStatisticsDBProcessData : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 현재 선택 메뉴
        /// </summary>
        private CDefine.EStatisticsDBProcessData m_eCurrentStatisticsDB = CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_CELL;
        /// <summary>
        /// 현재 매니저 테이블
        /// </summary>
        private CManagerTable m_objManagerTable;
        /// <summary>
        /// 폼에 사이즈에 맞춰 버튼을 동적 할당해줌
        /// 동적 생성될 버튼
        /// </summary>
        private ImageButton[] m_btnMenu;
        /// <summary>
        /// 해당 날짜로 검색된 데이터를 갖고 있음
        /// </summary>
        private DataTable m_objDataTable;
        private DataRow[] m_objDataRow;
        private string m_strLastAlign;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormStatisticsDBProcessData(CDocument objDocument)
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
        private void CFormStatisticsDBProcessData_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormStatisticsDBProcessData_FormClosed(object sender, FormClosedEventArgs e)
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
                m_strLastAlign = CDatabaseDefine.DEF_DESC;

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
                int iMenuCount = (int)CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_FINAL;
                // 버튼 여백 & 넓이 설정
                int iWhiteSpace = 2;
                int iButtonWidth = (this.panelFormMenu.Width / iMenuCount);
                // 버튼 개수가 적어서 base 버튼보다 넓이가 크면 base 버튼 넓이로 고정
                if (iButtonWidth > this.BtnBase.Width)
                {
                    iButtonWidth = this.BtnBase.Width;
                }
                string[] strButtonName = new string[iMenuCount];
                m_btnMenu = new ImageButton[iMenuCount];
                for (int iLoopButton = 0; iLoopButton < strButtonName.Length; iLoopButton++)
                {
                    strButtonName[iLoopButton] = ((CDefine.EStatisticsDBProcessData)iLoopButton).ToString();
                }
                // 버튼 동적 생성
                base.SetDynamicMenuButton(m_btnMenu, this.panelFormMenu, strButtonName, iButtonWidth, iWhiteSpace, new EventHandler(ButtonMenu_Click));
                // 버튼 이름 설정
                for (int iLoopMenu = 0; iLoopMenu < m_btnMenu.Length; iLoopMenu++)
                {
                    m_btnMenu[iLoopMenu].Name = string.Format("BtnStatisticsDBProcessData[{0}]", iLoopMenu);
                }
                // 날짜 선택 컨트롤을 현재 날짜로 설정
                this.DateTimeFrom.Value = DateTime.Today;
                this.DateTimeTo.Value = DateTime.Today;
                // 그리드 뷰에 알람으로 데이터베이스 연동
                SetDatabaseConnect(m_eCurrentStatisticsDB, this.DateTimeFrom.Value, this.DateTimeTo.Value);
                // DB 리스트 그리드 뷰 초기화
                if (false == InitializeGridView(this.GridViewList))
                {
                    break;
                }
                // From Date 초기화
                if (false == InitializeDateTime(this.DateTimeFrom))
                {
                    break;
                }
                // To Date 초기화
                if (false == InitializeDateTime(this.DateTimeTo))
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
        private void GridViewList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            do
            {
                try
                {
                    if (
                        null == m_objDataRow
                        || 0 == m_objDataRow.Length
                        )
                    {
                        break;
                    }
                    if (m_objDataRow.Length <= e.RowIndex)
                    {
                        break;
                    }
                    e.Value = m_objDataRow[e.RowIndex].ItemArray[e.ColumnIndex];
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
            base.SetButtonBackColor(this.BtnSaveToCsv, m_colorNormal);
            base.SetButtonBackColor(this.BtnSelectAsc, m_colorNormal);
            base.SetButtonBackColor(this.BtnSelectDesc, m_colorNormal);
            for (int iLoopMenu = 0; iLoopMenu < m_btnMenu.Length; iLoopMenu++)
            {
                base.SetButtonBackColor(m_btnMenu[iLoopMenu], m_colorNormal);
            }
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
                    base.SetControlButtonEnable(this.Controls, true);
                    break;
                case CDefine.EUserAuthorityLevel.ENGINEER:
                    base.SetControlButtonEnable(this.Controls, true);
                    break;
                case CDefine.EUserAuthorityLevel.MASTER:
                    base.SetControlButtonEnable(this.Controls, true);
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
                for (int iLoopMenu = 0; iLoopMenu < m_btnMenu.Length; iLoopMenu++)
                {
                    SetButtonChangeLanguage(m_btnMenu[iLoopMenu]);
                }
                SetButtonChangeLanguage(this.BtnSaveToCsv);
                SetButtonChangeLanguage(this.BtnSelectAsc);
                SetButtonChangeLanguage(this.BtnSelectDesc);

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
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, this.Name));
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, this.Name));
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
            this.Visible = bVisible;

            if (true == bVisible)
            {
                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                m_objDocument.GetMainFrame().SetCurrentForm(this);
                // 그리드 뷰에 현재 메뉴로 데이터베이스 연동
                SetDatabaseConnect(m_eCurrentStatisticsDB, this.DateTimeFrom.Value, this.DateTimeTo.Value);
                // DB 리스트 그리드 뷰 초기화
                InitializeGridView(this.GridViewList);
            }
            else
            {
                // 가비지 컬렉터에서 메모리를 회수해 갈 수 있도록 참조를 해제한다.
                m_objDataTable = null;
                m_objDataRow = null;
            }
        }

        /// <summary>
        /// 현재 데이터베이스 테이블에 전체 값을 보여주는 그리드 뷰
        /// 설명 : 열 클릭만 되고 나머지 조작 x
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
                    objGridView.Columns[iLoopColumn].Width = (int)(objGridView.Width * 0.12);
                    objGridView.Columns[iLoopColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                // 선택한 테이블마다 칼럼 사이즈 다르게 고정으로 설정해줘야 함
                switch (m_eCurrentStatisticsDB)
                {
                    case CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_CELL:
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataCell.DATE].Width = 150;
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataCell.INNER_ID].Width = 200;
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataCell.CELL_ID].Width = 270;
                        break;
                    case CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_JUDGEMENT:
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataJudgement.DATE].Width = 150;
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataJudgement.INNER_ID].Width = 200;
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataJudgement.CELL_ID].Width = 270;
                        break;
                    case CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_READER:
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataReader.DATE].Width = 150;
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataReader.INNER_ID].Width = 200;
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataReader.CELL_ID].Width = 270;
                        break;
                    case CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_VISION_RESULT:
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.DATE].Width = 150;
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.INNER_ID].Width = 200;
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.CELL_ID].Width = 270;
                        break;
                    case CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_WORK_ORDER:
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataWorkOrder.DATE].Width = 150;
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataWorkOrder.INNER_ID].Width = 200;
                        objGridView.Columns[(int)CDatabaseDefine.EHistoryProcessDataWorkOrder.CELL_ID].Width = 270;
                        break;
                    //case CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_TACT_TIME:
                    //    for (int iLoopColumn = 0; iLoopColumn < objGridView.Columns.Count; iLoopColumn++)
                    //    {
                    //        objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //        objGridView.Columns[iLoopColumn].Width = (int)(objGridView.Width * 0.2);
                    //        objGridView.Columns[iLoopColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    //    }
                    //    break;
                    default:
                        break;
                }
                // 전체 행 수를 잡아줌
                objGridView.RowCount = m_objDataRow.Length;
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, 8.0);

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
        /// 항목에 맞는 DB 테이블 연결
        /// </summary>
        /// <param name="eStatisticsDB"></param>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        private void SetDatabaseConnect(CDefine.EStatisticsDBProcessData eStatisticsDB, DateTime objFrom, DateTime objTo)
        {
            // 디폴트 내림차순
            SetDatabaseConnect(eStatisticsDB, objFrom, objTo, m_strLastAlign);
        }

        /// <summary>
        /// 항목에 맞는 DB 테이블 연결
        /// </summary>
        /// <param name="eStatisticsDB"></param>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        /// <param name="strOrder"></param>
        private void SetDatabaseConnect(CDefine.EStatisticsDBProcessData eStatisticsDB, DateTime objFrom, DateTime objTo, string strOrder)
        {
            DataGridView objGridView = this.GridViewList;
            int iIndexDate = 0;

            switch (eStatisticsDB)
            {
                case CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_CELL:
                    m_objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataCell;
                    iIndexDate = (int)CDatabaseDefine.EHistoryProcessDataCell.DATE;
                    break;
                case CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_JUDGEMENT:
                    m_objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataJudgement;
                    iIndexDate = (int)CDatabaseDefine.EHistoryProcessDataJudgement.DATE;
                    break;
                case CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_READER:
                    m_objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataReader;
                    iIndexDate = (int)CDatabaseDefine.EHistoryProcessDataReader.DATE;
                    break;
                case CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_VISION_RESULT:
                    m_objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataVisionResult;
                    iIndexDate = (int)CDatabaseDefine.EHistoryProcessDataVisionResult.DATE;
                    break;
                case CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_WORK_ORDER:
                    m_objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataWorkOrder;
                    iIndexDate = (int)CDatabaseDefine.EHistoryProcessDataWorkOrder.DATE;
                    break;
                default:
                    break;
            }

            string strQuery = string.Format("SELECT * FROM {0} WHERE {1} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{2}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{3}') {4};",
                m_objManagerTable.HLGetTableName(),
                m_objManagerTable.HLGetTableSchemaName()[iIndexDate],
                string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                CDatabaseDefine.DEF_RECORD_READ_LIMIT);

            m_objDataTable = new DataTable();
            m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(strQuery, ref m_objDataTable);
            m_objDataRow = m_objDataTable.Select("", m_objManagerTable.HLGetTableSchemaName()[iIndexDate] + " " + strOrder);
            // 현재 메뉴 갱신
            m_eCurrentStatisticsDB = eStatisticsDB;
        }

        /// <summary>
        /// 버튼 클릭 이벤트 정의
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonMenu_Click(object sender, EventArgs e)
        {
            ImageButton objButton = sender as ImageButton;
            CDefine.EStatisticsDBProcessData eStatisticsDB = 0;
            try
            {
                var objRegex = new Regex(@"\[(.+)\]");
                if (true == objRegex.IsMatch(objButton.Name))
                {
                    string strText = objRegex.Match(objButton.Name).Groups[1].Value;
                    eStatisticsDB = (CDefine.EStatisticsDBProcessData)Convert.ToInt32(strText);

                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Menu : {1} -> {2}]", "ButtonMenu_Click", m_eCurrentStatisticsDB.ToString(), eStatisticsDB.ToString());
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
            // 날짜 선택 컨트롤을 현재 날짜로 설정
            this.DateTimeFrom.Value = DateTime.Today;
            this.DateTimeTo.Value = DateTime.Today;
            // DB 내용을 변경해서 그리드 뷰에 뿌려줌
            SetDatabaseConnect(eStatisticsDB, this.DateTimeFrom.Value, this.DateTimeTo.Value);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(this.GridViewList);
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            for (int iLoopMenu = 0; iLoopMenu < (int)CDefine.EStatisticsDBProcessData.STATISTICS_DB_PROCESS_DATA_FINAL; iLoopMenu++)
            {

                if (iLoopMenu == (int)m_eCurrentStatisticsDB)
                {
                    base.SetButtonBackColor(m_btnMenu[iLoopMenu], m_colorClick);
                }
                else
                {
                    base.SetButtonBackColor(m_btnMenu[iLoopMenu], m_colorNormal);
                }
            }

            if (CDatabaseDefine.DEF_ASC == m_strLastAlign)
            {
                base.SetButtonBackColor(BtnSelectAsc, m_colorClick);
                base.SetButtonBackColor(BtnSelectDesc, m_colorNormal);
            }
            else
            {
                base.SetButtonBackColor(BtnSelectAsc, m_colorNormal);
                base.SetButtonBackColor(BtnSelectDesc, m_colorClick);
            }
        }

        /// <summary>
        /// From DateTime 값이 변경된 경우 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTimeFrom_ValueChanged(object sender, EventArgs e)
        {
            // Form 날짜는 To 날짜보다 이후 출력이 되면 안됨
            // To 날짜도 갱신되도록 함
            DateTimePicker objFrom = this.DateTimeFrom;
            DateTimePicker objTo = this.DateTimeTo;
            // From 날짜가 To 날짜보다 더 크면 To 날짜 갱신
            if (objFrom.Value > objTo.Value)
            {
                objTo.Value = objFrom.Value;
            }
        }

        /// <summary>
        /// To DateTime 값이 변경된 경우 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTimeTo_ValueChanged(object sender, EventArgs e)
        {
            // To 날짜는 From 날짜보다 이전 출력이 되면 안됨
            // From 날짜도 갱신되도록 함
            DateTimePicker objFrom = this.DateTimeFrom;
            DateTimePicker objTo = this.DateTimeTo;
            // To 날짜가 From 날짜보다 적으면 From 날짜 갱신
            if (objTo.Value < objFrom.Value)
            {
                objFrom.Value = objTo.Value;
            }
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
            strPath = string.Format(@"{0}\{1}_{2}.csv", strPath, m_btnMenu[(int)m_eCurrentStatisticsDB].Text, string.Format("{0:yyyyMMdd_HHmm}", DateTime.Now));
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

            // 날짜 ~ 날짜 테이블을 검색해서 그리드 뷰에 뿌려줌
            SetDatabaseConnect(m_eCurrentStatisticsDB, this.DateTimeFrom.Value, this.DateTimeTo.Value);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(this.GridViewList);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Table : {1}] [From : {2}] [To : {3}]", "BtnSelectAsc_Click", m_eCurrentStatisticsDB.ToString(), this.DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), this.DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT));
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

            // 날짜 ~ 날짜 테이블을 검색해서 그리드 뷰에 뿌려줌
            SetDatabaseConnect(m_eCurrentStatisticsDB, this.DateTimeFrom.Value, this.DateTimeTo.Value);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(this.GridViewList);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Table : {1}] [From : {2}] [To : {3}]", "BtnSelectDesc_Click", m_eCurrentStatisticsDB.ToString(), this.DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), this.DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT));
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }
    }
}