using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormStatisticsEQtoEQLoss : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// EQ to EQ Loss 리스트 칼럼 정의
        /// </summary>
        private enum EEQtoEQLossListColumn
        {
            SET = 0,
            CLEAR,
            DURATION,
            LOSS_TYPE,
            EQ_TO_EQ_LOSS_LIST_COLUMN_FINAL
        }
        /// <summary>
        /// 해당 날짜로 검색된 데이터를 갖고 있음
        /// </summary>
        private DataTable m_objDataTable;
        private DataRow[] m_objDataRow;
        private string m_strLastAlign;
        /// <summary>
        /// EQ to EQ Loss 구조체
        /// </summary>
        private struct structureEQtoEQLoss
        {
            /// <summary>
            /// SET 시간
            /// </summary>
            public string strSet;
            /// <summary>
            /// CLEAR 시간
            /// </summary>
            public string strClear;
            /// <summary>
            /// CLEAR - SET 기한
            /// </summary>
            public string strDuration;

            public structureEQtoEQLoss(string set, string clear, string duration)
            {
                strSet = set;
                strClear = clear;
                strDuration = duration;
            }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormStatisticsEQtoEQLoss(CDocument objDocument)
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
        private void CFormStatisticsEQtoEQLoss_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormStatisticsEQtoEQLoss_FormClosed(object sender, FormClosedEventArgs e)
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
                // 데이터베이스 현재 날짜로 초기화 & 데이터베이스 연동
                if (false == InitializeDatabase())
                {
                    break;
                }
                // Alarm 리스트 그리드 뷰 초기화
                if (false == InitializeGridView(this.GridViewEQtoEQLossList))
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
        private void GridViewEQtoEQLossList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            do
            {
                try
                {
                    if (0 == m_objDataRow.Length)
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
            base.SetButtonBackColor(this.BtnTitleEQtoEQLossList, m_colorLabel);
            base.SetButtonBackColor(this.BtnSelectAsc, m_colorNormal);
            base.SetButtonBackColor(this.BtnSelectDesc, m_colorNormal);
            base.SetButtonBackColor(this.BtnSaveToCsv, m_colorNormal);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    //&& false == btn.Name.Equals("")        
                    )
                {
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                    btn.BackColorChanged += NonClickableButton_BackColorChanged;
                    btn.Cursor = Cursors.Default;
                }
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
                SetButtonChangeLanguage(this.BtnTitleEQtoEQLossList);
                SetButtonChangeLanguage(this.BtnSelectAsc);
                SetButtonChangeLanguage(this.BtnSelectDesc);
                SetButtonChangeLanguage(this.BtnSaveToCsv);

                bReturn = true;
            } while (false);

            return bReturn;
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
                this.DateTimeFrom.Value = DateTime.Today;
                this.DateTimeTo.Value = DateTime.Today;
                // EQ to EQ Loss History 데이터베이스 연동
                SetEQtoEQLossHistoryConnect(this.DateTimeFrom.Value, this.DateTimeTo.Value);
                // DB 리스트 그리드 뷰 초기화
                InitializeGridView(this.GridViewEQtoEQLossList);

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
                    break;
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

                // 전체 행 수를 잡아줌
                objGridView.RowCount = m_objDataRow.Length;
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, 9.0);

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
        /// EQ to EQ Loss 이력 데이터베이스 연동
        /// </summary>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        private void SetEQtoEQLossHistoryConnect(DateTime objFrom, DateTime objTo)
        {
            // 디폴트 내림차순
            SetEQtoEQLossHistoryConnect(objFrom, objTo, m_strLastAlign);
        }

        /// <summary>
        /// EQ to EQ Loss 이력 데이터베이스 연동
        /// </summary>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        /// <param name="strOrder"></param>
        private void SetEQtoEQLossHistoryConnect(DateTime objFrom, DateTime objTo, string strOrder)
        {
            // Equipment Upper Loss Time Event
            CManagerTable objEquipmentUpperLossTimeEvent = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentUpperLossTimeEvent;
            // Equipment Lower Loss Time Event
            CManagerTable objEquipmentLowerLossTimeEvent = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentLowerLossTimeEvent;

            try
            {
                // 앞 단 설비 손실 타임 이벤트 목록을 만들어서 받아옴
                List<structureEQtoEQLoss> objUpperEQtoEQLoss = GetEventList(objEquipmentUpperLossTimeEvent, (int)CDatabaseDefine.EHistoryEquipmentUpperLossTimeEvent.DATE, objFrom, objTo);
                // 뒷 단 설비 손실 타임 이벤트 목록을 만들어서 받아옴
                List<structureEQtoEQLoss> objLowerEQtoEQLoss = GetEventList(objEquipmentLowerLossTimeEvent, (int)CDatabaseDefine.EHistoryEquipmentLowerLossTimeEvent.DATE, objFrom, objTo);
                // 임시 데이터 테이블
                DataTable objDataTableEQtoEQLoss = new DataTable();
                objDataTableEQtoEQLoss.Columns.Add(EEQtoEQLossListColumn.SET.ToString());
                objDataTableEQtoEQLoss.Columns.Add(EEQtoEQLossListColumn.CLEAR.ToString());
                objDataTableEQtoEQLoss.Columns.Add(EEQtoEQLossListColumn.DURATION.ToString());
                objDataTableEQtoEQLoss.Columns.Add(EEQtoEQLossListColumn.LOSS_TYPE.ToString());
                // 임시 데이터 테이블에 레코드를 삽입
                for (int iLoopRow = 0; iLoopRow < objUpperEQtoEQLoss.Count; iLoopRow++)
                {
                    // 레코드 삽입
                    objDataTableEQtoEQLoss.Rows.Add(
                        objUpperEQtoEQLoss[iLoopRow].strSet,
                        objUpperEQtoEQLoss[iLoopRow].strClear,
                        objUpperEQtoEQLoss[iLoopRow].strDuration,
                        "UPPER");
                }
                for (int iLoopRow = 0; iLoopRow < objLowerEQtoEQLoss.Count; iLoopRow++)
                {
                    // 레코드 삽입
                    objDataTableEQtoEQLoss.Rows.Add(
                        objLowerEQtoEQLoss[iLoopRow].strSet,
                        objLowerEQtoEQLoss[iLoopRow].strClear,
                        objLowerEQtoEQLoss[iLoopRow].strDuration,
                        "LOWER");
                }
                // DATE 칼럼으로 정렬
                objDataTableEQtoEQLoss.DefaultView.Sort = EEQtoEQLossListColumn.SET.ToString() + " ASC";
                objDataTableEQtoEQLoss = objDataTableEQtoEQLoss.DefaultView.ToTable();
                // 복사
                m_objDataTable = new DataTable();
                m_objDataTable = objDataTableEQtoEQLoss.Copy();
                m_objDataRow = m_objDataTable.Select("", EEQtoEQLossListColumn.SET.ToString() + " " + strOrder);
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// SET / CLEAR / CLEAR - SET 리스트로 리턴시켜줌
        /// </summary>
        /// <param name="objManagerTable"></param>
        /// <param name="iDateIndex"></param>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        /// <returns></returns>
        private List<structureEQtoEQLoss> GetEventList(CManagerTable objManagerTable, int iDateIndex, DateTime objFrom, DateTime objTo)
        {
            List<structureEQtoEQLoss> objList = new List<structureEQtoEQLoss>();
            string strQuery = null;
            DataTable objDataTable = new DataTable();
            DataRow[] objDataRow = null;

            try
            {
                strQuery = string.Format("SELECT * FROM {0} ", objManagerTable.HLGetTableName());
                strQuery += string.Format("WHERE {0} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{1}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{2}') {3};",
                    objManagerTable.HLGetTableSchemaName()[iDateIndex],
                        string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                        string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                        CDatabaseDefine.DEF_RECORD_READ_LIMIT);
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(strQuery, ref objDataTable);
                objDataRow = objDataTable.Select();
                // 이벤트는 SET - CLEAR 관계로 온다
                // 레코드가 짝수인 경우
                if (0 == objDataRow.Length % 2)
                {
                    for (int iLoopRow = 0; iLoopRow < objDataRow.Length; iLoopRow += 2)
                    {
                        DateTime objDateSet = DateTime.ParseExact(objDataRow[iLoopRow].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        DateTime objDateClear = DateTime.ParseExact(objDataRow[iLoopRow + 1].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        objList.Add(new structureEQtoEQLoss(
                            objDateSet.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                            objDateClear.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                            (objDateClear - objDateSet).ToString(CDatabaseDefine.DEF_TACT_TIME_FORMAT)));
                    }
                }
                // 홀수인 경우 하나 남는 SET은 현재 시간이랑 뺌
                else
                {
                    for (int iLoopRow = 0; iLoopRow < objDataRow.Length - 1; iLoopRow += 2)
                    {
                        DateTime objDateSet = DateTime.ParseExact(objDataRow[iLoopRow].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        DateTime objDateClear = DateTime.ParseExact(objDataRow[iLoopRow + 1].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        objList.Add(new structureEQtoEQLoss(
                            objDateSet.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                            objDateClear.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                            (objDateClear - objDateSet).ToString(CDatabaseDefine.DEF_TACT_TIME_FORMAT)));
                    }
                    DateTime objSoloSet = DateTime.ParseExact(objDataRow[objDataRow.Length - 1].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                    objList.Add(new structureEQtoEQLoss(
                        objSoloSet.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                        DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                        (DateTime.Now - objSoloSet).ToString(CDatabaseDefine.DEF_TACT_TIME_FORMAT)));
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }

            return objList;
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
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
        /// From 날짜 변경
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
        /// To 날짜 변경
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
            strPath = string.Format(@"{0}\{1}_{2}.csv", strPath, "EQ_TO_EQ_LOSS", string.Format("{0:yyyyMMdd_HHmm}", DateTime.Now));
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

            // EQ to EQ Loss History 데이터베이스 연동
            SetEQtoEQLossHistoryConnect(this.DateTimeFrom.Value, this.DateTimeTo.Value);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(this.GridViewEQtoEQLossList);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [From : {1}] [To : {2}]", "BtnSelectAsc_Click", this.DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), this.DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT));
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

            // EQ to EQ Loss History 데이터베이스 연동
            SetEQtoEQLossHistoryConnect(this.DateTimeFrom.Value, this.DateTimeTo.Value);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(this.GridViewEQtoEQLossList);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [From : {1}] [To : {2}]", "BtnSelectDesc_Click", this.DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), this.DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT));
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }
    }
}