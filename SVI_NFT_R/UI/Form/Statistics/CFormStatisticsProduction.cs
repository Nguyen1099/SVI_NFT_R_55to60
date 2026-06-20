using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormStatisticsProduction : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 생산량 리스트 칼럼 정의
        /// </summary>
        private enum EProductionListColumn
        {
            TIME = 0,
            INPUT,
            OUTPUT,
            NG,
            NG_RATE,
            OK,
            OK_RATE,
            MCR_READING,
            MCR_READING_RATE,
            PRODUCTION_LIST_COLUMN_FINAL
        };
        private class ProductionData
        {
            public string TimeRange;
            public string InputCount = "0";
            public string OutputCount = "0";
            public string NGCount = "0";
            public string NGRate = "0.0%";
            public string OKCount = "0";
            public string OKRate = "0.0%";
            public string McrReadingCount = "0";
            public string McrReadingRate = "0.0%";
        }
        private List<ProductionData> mDayShiftProductionData = new List<ProductionData>();
        private List<ProductionData> mNightShiftProductionData = new List<ProductionData>();
        private DataTable mDayShiftProductionTable;
        private DataTable mNightShiftProductionTable;
        private readonly string[] mProductionColumnList =
        {
            EProductionListColumn.TIME.ToString(),
            EProductionListColumn.INPUT.ToString(),
            EProductionListColumn.OUTPUT.ToString(),
            EProductionListColumn.NG.ToString(),
            "NG (%)",
            EProductionListColumn.OK.ToString(),
            "OK (%)",
            "MCR",
            "MCR (%)"
        };
        /// <summary>
        /// 갱신 날짜 설정
        /// </summary>
        private DateTime m_objDateTime = default(DateTime);
        /// <summary>
        /// 마지막 갱신 날짜 저장
        /// </summary>
        private DateTime mLastUpdateDateTime = default(DateTime);

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormStatisticsProduction(CDocument objDocument)
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
        private void CFormStatisticsProduction_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormStatisticsProduction_FormClosed(object sender, FormClosedEventArgs e)
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
            timerUpdate.Enabled = false;
            timer.Enabled = false;

            // 작업이 완료 될 때까지 대기한다
            while (true == backgroundWorkerUpdate.IsBusy)
            {
                Thread.Sleep(10);
            }
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
                // 그리드 데이터 초기화
                InitializeGridData();
                SetUpdateGridViewFromRawdata();
                // 주간 그룹 생산량 리스트 그리드 뷰 초기화
                if (false == InitializeGridView(this.GridViewWeeklyGroup, mProductionColumnList))
                {
                    break;
                }
                // 야간 그룹 생산량 리스트 그리드 뷰 초기화
                if (false == InitializeGridView(this.GridViewNightGroup, mProductionColumnList))
                {
                    break;
                }

                // From Date 초기화
                if (false == InitializeDateTime(this.DateTimeFrom))
                {
                    break;
                }
                // 버튼 색상 정의
                SetButtonColor();
                // 타이머 외부에서 제어
                timerUpdate.Interval = 4000;
                timerUpdate.Enabled = false;
                timer.Interval = 100;
                timer.Enabled = false;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 그리드 Raw데이터를 초기화한다.
        /// </summary>
        private void InitializeGridData()
        {
            mDayShiftProductionData.Clear();
            mNightShiftProductionData.Clear();

            CConfig.CSystemParameter objSystemParameter = m_objDocument.m_objConfig.GetSystemParameter();
            TimeSpan objWeeklyTimeStart = new TimeSpan(objSystemParameter.iWorkerTimeWeeklyStart, 0, 0);
            TimeSpan objWeeklyTimeEnd = new TimeSpan(objSystemParameter.iWorkerTimeWeeklyEnd - 1, 0, 0);
            TimeSpan objTime = objWeeklyTimeStart;
            for (int iLoopHour = 0; iLoopHour <= (objWeeklyTimeEnd - objWeeklyTimeStart).Hours; iLoopHour++)
            {
                mDayShiftProductionData.Add(new ProductionData() { TimeRange = string.Format("{0:00}:00 ~ {1:00}:00", objTime.Hours, objTime.Hours + 1) });
                objTime = objTime.Add(new TimeSpan(1, 0, 0));
            }
            TimeSpan objNightTimeStart = new TimeSpan(objSystemParameter.iWorkerTimeNightStart, 0, 0);
            TimeSpan objNightTimeEnd = new TimeSpan(1, objSystemParameter.iWorkerTimeNightEnd - 1, 0, 0);
            objTime = objNightTimeStart;
            for (int iLoopHour = 0; iLoopHour <= (objNightTimeEnd - objNightTimeStart).Hours; iLoopHour++)
            {
                mNightShiftProductionData.Add(new ProductionData() { TimeRange = string.Format("{0:00}:00 ~ {1:00}:00", objTime.Hours, objTime.Hours + 1) });
                objTime = objTime.Add(new TimeSpan(1, 0, 0));
            }
        }

        /// <summary>
        /// 그리드 Raw데이터를 그리드뷰에 업데이트한다. 
        /// </summary>
        private void SetUpdateGridViewFromRawdata()
        {
            if (
                null == mDayShiftProductionTable
                && null == mNightShiftProductionTable
                )
            {
                mDayShiftProductionTable = new DataTable();
                for (int iLoopColumn = 0; iLoopColumn < mProductionColumnList.Length; iLoopColumn++)
                {
                    mDayShiftProductionTable.Columns.Add(mProductionColumnList[iLoopColumn], typeof(string));
                }
                mNightShiftProductionTable = new DataTable();
                for (int iLoopColumn = 0; iLoopColumn < mProductionColumnList.Length; iLoopColumn++)
                {
                    mNightShiftProductionTable.Columns.Add(mProductionColumnList[iLoopColumn], typeof(string));
                }
                foreach (ProductionData item in mDayShiftProductionData)
                {
                    mDayShiftProductionTable.Rows.Add(item.TimeRange, item.InputCount, item.OutputCount, item.NGCount, item.NGRate, item.OKCount, item.OKRate, item.McrReadingCount, item.McrReadingRate);
                }
                foreach (ProductionData item in mNightShiftProductionData)
                {
                    mNightShiftProductionTable.Rows.Add(item.TimeRange, item.InputCount, item.OutputCount, item.NGCount, item.NGRate, item.OKCount, item.OKRate, item.McrReadingCount, item.McrReadingRate);
                }
            }

            if (true == GridViewWeeklyGroup.InvokeRequired)
            {
                Invoke(new Action(UpdateGridView));
            }
            else
            {
                UpdateGridView();
            }
        }

        /// <summary>
        /// 그리드뷰를 업데이트 한다
        /// </summary>         
        public void UpdateGridView()
        {
            if (
                null == GridViewWeeklyGroup.DataSource
                && null == GridViewNightGroup.DataSource
                )
            {
                GridViewWeeklyGroup.DataSource = mDayShiftProductionTable;
                GridViewNightGroup.DataSource = mNightShiftProductionTable;
            }
            else
            {
                int rowIndex = 0;
                foreach (ProductionData item in mDayShiftProductionData)
                {
                    string currentData = (string)mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.INPUT];
                    if (currentData != item.InputCount) mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.INPUT] = item.InputCount;
                    currentData = (string)mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.OUTPUT];
                    if (currentData != item.OutputCount) mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.OUTPUT] = item.OutputCount;
                    currentData = (string)mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.OK];
                    if (currentData != item.OKCount) mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.OK] = item.OKCount;
                    currentData = (string)mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.OK_RATE];
                    if (currentData != item.OKRate) mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.OK_RATE] = item.OKRate;
                    currentData = (string)mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.NG];
                    if (currentData != item.NGCount) mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.NG] = item.NGCount;
                    currentData = (string)mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.NG_RATE];
                    if (currentData != item.NGRate) mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.NG_RATE] = item.NGRate;
                    currentData = (string)mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.MCR_READING];
                    if (currentData != item.McrReadingCount) mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.MCR_READING] = item.McrReadingCount;
                    currentData = (string)mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.MCR_READING_RATE];
                    if (currentData != item.McrReadingRate) mDayShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.MCR_READING_RATE] = item.McrReadingRate;
                    rowIndex++;
                }
                rowIndex = 0;
                foreach (ProductionData item in mNightShiftProductionData)
                {
                    string currentData = (string)mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.INPUT];
                    if (currentData != item.InputCount) mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.INPUT] = item.InputCount;
                    currentData = (string)mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.OUTPUT];
                    if (currentData != item.OutputCount) mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.OUTPUT] = item.OutputCount;
                    currentData = (string)mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.OK];
                    if (currentData != item.OKCount) mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.OK] = item.OKCount;
                    currentData = (string)mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.OK_RATE];
                    if (currentData != item.OKRate) mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.OK_RATE] = item.OKRate;
                    currentData = (string)mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.NG];
                    if (currentData != item.NGCount) mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.NG] = item.NGCount;
                    currentData = (string)mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.NG_RATE];
                    if (currentData != item.NGRate) mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.NG_RATE] = item.NGRate;
                    currentData = (string)mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.MCR_READING];
                    if (currentData != item.McrReadingCount) mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.MCR_READING] = item.McrReadingCount;
                    currentData = (string)mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.MCR_READING_RATE];
                    if (currentData != item.McrReadingRate) mNightShiftProductionTable.Rows[rowIndex][(int)EProductionListColumn.MCR_READING_RATE] = item.McrReadingRate;
                    rowIndex++;
                }
                GridViewWeeklyGroup.ClearSelection();
                GridViewNightGroup.ClearSelection();
            }
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            base.SetButtonBackColor(this.BtnTitleProductionList, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleTotalInputWeekly, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleTotalOutputWeekly, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleTotalInputNight, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleTotalOutputNight, m_colorLabelSub);

            base.SetButtonBackColor(this.BtnTotalInputWeekly, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTotalOutputWeekly, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTotalInputNight, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTotalOutputNight, m_colorLabelData);

            base.SetButtonBackColor(this.BtnPrevious, m_colorNormal);
            base.SetButtonBackColor(this.BtnToday, m_colorNormal);
            base.SetButtonBackColor(this.BtnNext, m_colorNormal);

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
                SetButtonChangeLanguage(this.BtnTitleProductionList);
                SetButtonChangeLanguage(this.BtnTitleTotalInputWeekly);
                SetButtonChangeLanguage(this.BtnTitleTotalOutputWeekly);
                SetButtonChangeLanguage(this.BtnTitleTotalInputNight);
                SetButtonChangeLanguage(this.BtnTitleTotalOutputNight);

                SetButtonChangeLanguage(this.BtnPrevious);
                SetButtonChangeLanguage(this.BtnToday);
                SetButtonChangeLanguage(this.BtnNext);

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
            timerUpdate.Enabled = bTimer;
            timer.Enabled = bTimer;
        }

        /// <summary>
        /// Visible 유무
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
            do
            {
                this.Visible = bVisible;

                if (true == bVisible)
                {
                    // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                    SetResourceControl();
                    // 해당 폼을 말단으로 설정
                    m_objDocument.GetMainFrame().SetCurrentForm(this);
                    // 폼 로드시 오늘 날짜로 초기화
                    DateTimeFrom.Value = DateTime.Now;
                    // 데이터베이스 현재 날짜로 초기화 & 데이터베이스 연동
                    if (false == backgroundWorkerUpdate.IsBusy)
                    {
                        backgroundWorkerUpdate.RunWorkerAsync();
                    }
                }
                else
                {
                    mLastUpdateDateTime = default(DateTime);
                }
            } while (false);
        }

        /// <summary>
        /// 그리드 뷰 초기화
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="strColumnName"></param>
        /// <returns></returns>
        private bool InitializeGridView(DataGridView objGridView, string[] strColumnName)
        {
            bool bReturn = false;

            do
            {
                // 그리드 뷰 기본 스타일 초기화
                if (false == base.InitializeGridView(objGridView))
                {
                    break;
                }
                // 그리드 뷰 ReadOnly
                objGridView.ReadOnly = true;
                // 그리드 뷰 다중 선택 x
                objGridView.MultiSelect = false;
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                objGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                // 그리드 뷰 칼럼 추가
                for (int iLoopColumn = 0; iLoopColumn < strColumnName.Length; iLoopColumn++)
                {
                    // 칼럼 정렬 기능 x
                    objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                    objGridView.Columns[iLoopColumn].Width = (int)((objGridView.Width - 100) * 0.13);
                    objGridView.Columns[iLoopColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                // 사이즈 조정
                objGridView.Columns[(int)EProductionListColumn.TIME].Width = 100;
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
        /// 데이터베이스 현재 날짜로 초기화 & 데이터베이스 연동
        /// </summary>
        /// <returns></returns>
        private bool InitializeDatabase()
        {
            bool bReturn = false;

            do
            {
                // 날짜 선택 컨트롤을 현재 날짜로 설정
                // 날짜 조건 08:00 ~ 23:59 분까지는 현재 날짜로 00:00 ~ 07:59 분까지는 하루 이전 날짜로 설정해줘야 함
                // jht 20180525 수정
                DateTime objCurrentTime = DateTime.Now;
                DateTime objCurrentDay = this.DateTimeFrom.Value;
                objCurrentDay = new DateTime(objCurrentDay.Year, objCurrentDay.Month, objCurrentDay.Day, objCurrentTime.Hour, objCurrentTime.Minute, objCurrentTime.Second);
                DateTime objTime1 = new DateTime(objCurrentDay.Year, objCurrentDay.Month, objCurrentDay.Day, 7, 0, 0);
                DateTime objTime2 = new DateTime(objCurrentDay.Year, objCurrentDay.Month, objCurrentDay.Day, 23, 59, 59, 999);
                DateTime objTime3 = new DateTime(objCurrentDay.Year, objCurrentDay.Month, objCurrentDay.Day, 0, 0, 0);
                DateTime objTime4 = new DateTime(objCurrentDay.Year, objCurrentDay.Month, objCurrentDay.Day, 6, 59, 59, 999);
                // 08:00 ~ 23:59분 사이인지 확인
                if (0 <= DateTime.Compare(objCurrentDay, objTime1) && 0 >= DateTime.Compare(objCurrentDay, objTime2))
                {
                    //m_objDateTime = DateTime.Today;
                    m_objDateTime = new DateTime(objCurrentDay.Year, objCurrentDay.Month, objCurrentDay.Day, 12, 0, 0);
                }
                // 00:00 ~ 07:59분 사이인지 확인
                else if (0 <= DateTime.Compare(objCurrentDay, objTime3) && 0 >= DateTime.Compare(objCurrentDay, objTime4))
                {
                    //m_objDateTime = DateTime.Today.AddDays( -1 );
                    m_objDateTime = new DateTime(objCurrentDay.Year, objCurrentDay.Month, objCurrentDay.Day, 12, 0, 0);
                    m_objDateTime = m_objDateTime.AddDays(-1);
                }

                // 생산량 데이터베이스 연동
                SetProductionHistoryConnect(m_objDateTime);

                mLastUpdateDateTime = new DateTime(DateTimeFrom.Value.Year, DateTimeFrom.Value.Month, DateTimeFrom.Value.Day);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 그리드 뷰에 시간 항목 표시
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="objStart"></param>
        /// <param name="objEnd"></param>
        private void AddTimeRow(DataGridView objGridView, TimeSpan objStart, TimeSpan objEnd)
        {
            TimeSpan objTime = objStart;
            for (int iLoopHour = 0; iLoopHour <= (objEnd - objStart).Hours; iLoopHour++)
            {
                string[] strRowData = new string[(int)EProductionListColumn.PRODUCTION_LIST_COLUMN_FINAL];
                strRowData[(int)EProductionListColumn.TIME] = string.Format("{0:00}:00 : {1:00}:00", objTime.Hours, objTime.Hours + 1);
                objGridView.Rows.Add(strRowData);
                objTime = objTime.Add(new TimeSpan(1, 0, 0));
            }

            // 첫 행 포커스 해제
            objGridView.ClearSelection();
        }

        /// <summary>
        /// DB에서 생산량 표시 Rawdata 업데이트한다.
        /// </summary>
        /// <param name="objDate"></param>
        private void SetProductionHistoryConnect(DateTime objDate)
        {
            InitializeGridData();
            // 투입량 - Cell Input 테이블에 특정 시간대 데이터를 구해서 시간별 카운트를 누적
            SetCellInput(objDate);
            // 배출량 - Cell Output 테이블 시간기준 Output, Scrap, OK, NG, MCR OK 등을 구함
            SetCellDatas(objDate);
            //// 배출량 - Cell Output 테이블에 특정 시간대 데이터를 구해서 시간별 카운트를 누적
            //SetCellOutput(objDate);
            //// 설비 최종 결과 NG, OK 수량 & 비율 - Process Data Cell 테이블에 특정 시간대 데이터를 구해서 시간별 카운트를 누적
            //SetNgOkResult(objDate);
            //// MCR 리딩 수량 - MCR 테이블에 특정 시간대 데이터를 구해서 시간별 카운트를 누적
            //SetMCRRate(objDate);
        }

        /// <summary>
        /// 투입량 - Cell Input 테이블에 특정 시간대 데이터를 구해서 시간별 카운트를 누적
        /// </summary>
        /// <param name="objDate"></param>
        private void SetCellInput(DateTime objDate)
        {
            StringBuilder sbQuery = new StringBuilder();
            // Table History Cell Input
            CManagerTable objManagerTableCellInput = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellInput;

            try
            {
                // 시작 시간
                int iWorkerTime = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
                DateTime objDateTimeFrom = new DateTime(objDate.Year, objDate.Month, objDate.Day, iWorkerTime, 0, 0);
                DateTime objDateTimeTo = new DateTime(objDate.Year, objDate.Month, objDate.Day, iWorkerTime, 0, 0).AddDays(1d);
                // Day / Hour / Count
                sbQuery.AppendFormat(
                    "SELECT strftime('%d', {0}) AS Day, strftime('%H', {0}) AS Hour, count(*) AS Count FROM {1} WHERE {0} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{2}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{3}') GROUP BY Day, Hour;",
                    objManagerTableCellInput.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryCellInput.DATE],
                    objManagerTableCellInput.HLGetTableName(),
                    objDateTimeFrom.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                    objDateTimeTo.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT)
                    );
                // 데이터 테이블에 데이터베이스 자료 넣음
                DataTable objDataTable = new DataTable();
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(sbQuery.ToString(), ref objDataTable);
                DataRow[] objDataRow = objDataTable.Select();
                int rowIndex = 0;
                int nextTime = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
                int dayShiftDuration = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyEnd - m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
                for (int i = 0; i < 24; i++)
                {
                    if (
                        rowIndex < objDataRow.Length
                        && nextTime == Convert.ToInt32(objDataRow[rowIndex][1])
                        )
                    {
                        string count = Convert.ToString(objDataRow[rowIndex][2]);
                        if (i < dayShiftDuration)
                        {
                            // 낮
                            mDayShiftProductionData[i].InputCount = count;
                        }
                        else
                        {
                            // 밤
                            mNightShiftProductionData[i - dayShiftDuration].InputCount = count;
                        }
                        rowIndex++;
                    }
                    else
                    {
                        if (i < dayShiftDuration)
                        {
                            // 낮
                            mDayShiftProductionData[i].InputCount = "0";
                        }
                        else
                        {
                            // 밤
                            mNightShiftProductionData[i - dayShiftDuration].InputCount = "0";
                        }
                    }
                    nextTime++;
                    nextTime %= 24;
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        private void SetCellDatas(DateTime selectedDate)
        {
            StringBuilder query = new StringBuilder();
            DataGridView gridDay = this.GridViewWeeklyGroup;
            DataGridView gridNight = this.GridViewNightGroup;
            int dayStartHour = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
            int dayEndHour = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyEnd;
            int nightStartHour = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeNightStart;
            int nightEndHour = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeNightEnd;
            // Table History Cell Input
            CManagerTable cellOutManager = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellOutput;
            CManagerTable machineResultManager = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryMachineResult;
            CManagerTable readerManger = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataReader;

            try
            {
                #region(1. DB 검색 시작/ 끝 시간 구하기)
                DateTime searchStartTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, dayStartHour, 0, 0);
                if (selectedDate.Hour < dayStartHour)
                {
                    searchStartTime = searchStartTime.AddDays(-1);
                }
                DateTime searchEndTime = searchStartTime.AddDays(1).AddMilliseconds(-1);
                #endregion(1. DB 검색 시작/ 끝 시간 구하기)
                #region (2. Query)
                query
                    .Append("select").Append(" ")
                            .Append("strftime('%Y-%m-%d %H', t.date) as hour,").Append(" ")
                            .Append("count(case when t.out_result = 'OK' then 1 end) as OK,").Append(" ")
                            .Append("count(case when t.out_result = 'NG' then 1 end) as NG,").Append(" ")
                            .Append("count(case when ifnull(t.out_result, 1) then 1 end) as SCRAP,").Append(" ")
                            .Append("count(case when t.mcr = '0' then 1 end) as MCR_OK,").Append(" ")
                            .Append("count(case when t.mcr = '1' then 1 end) as MCR_NG").Append(" ")
                    .Append("from").Append(" ")
                            .Append("(select t1.date, t1.inner_id, t2.result as out_result, t3.reader_result_code as mcr").Append(" ")
                            .Append("from table_history_cell_output as t1").Append(" ")
                            .Append("left outer join table_history_machine_result as t2").Append(" ")
                            .Append("on t1.inner_id = t2.inner_id").Append(" ")
                            .Append("left outer join table_history_process_data_reader as t3").Append(" ")
                            .Append("on t1.inner_id = t3.inner_id) t").Append(" ")
                    .Append("where").Append(" ")
                            .AppendFormat("t.date between strftime('%Y-%m-%d %H:%M:%S.%f','{0}') and strftime('%Y-%m-%d %H:%M:%S.%f','{1}')", searchStartTime.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), searchEndTime.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT)).Append(" ")
                    .Append("group by strftime('%Y-%m-%d %H', t.date)");

                DataTable dataTable = new DataTable();
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(query.ToString(), ref dataTable);
                DataRow[] dataRow = dataTable.Select();
                #endregion (2. Query)
                #region(3. UI 표시)
                int dayCount = Math.Abs(dayEndHour - dayStartHour);
                int nightCount = Math.Abs(nightEndHour - nightStartHour);
                int okCount = 0;
                int ngCount = 0;
                double okRate = 0.0;
                double ngRate = 0.0;
                int scrapCount = 0;
                int mcrOkCount = 0;
                int mcrNgCount = 0;
                double mcrOkRate = 0.0;
                int index = 0;
                List<ProductionData> grid;
                // 3. UI 출력
                for (int i = 0; i < dataRow.Length; i++)
                {
                    DateTime rowTime = DateTime.ParseExact(dataRow[i].ItemArray[0].ToString(), "yyyy-MM-dd HH", CultureInfo.InvariantCulture);

                    if (rowTime.Hour >= dayStartHour && rowTime.Hour < dayEndHour)
                    {
                        index = rowTime.Hour - dayStartHour;
                        grid = mDayShiftProductionData;
                    }
                    else
                    {
                        if (rowTime.Hour >= nightStartHour)
                        {
                            index = rowTime.Hour - nightStartHour;
                        }
                        else
                        {
                            index = rowTime.Hour + 24 - nightStartHour;
                        }
                        grid = mNightShiftProductionData;
                    }
                    okCount = Int32.Parse(dataRow[i]["OK"].ToString());
                    ngCount = Int32.Parse(dataRow[i]["NG"].ToString());
                    scrapCount = Int32.Parse(dataRow[i]["SCRAP"].ToString());
                    mcrOkCount = Int32.Parse(dataRow[i]["MCR_OK"].ToString());
                    mcrNgCount = Int32.Parse(dataRow[i]["MCR_NG"].ToString());

                    grid[index].OutputCount = (okCount + ngCount).ToString();
                    //grid[index].ScrapCount = (scrapCount).ToString();
                    grid[index].OKCount = (okCount).ToString();
                    grid[index].NGCount = (ngCount).ToString();
                    grid[index].McrReadingCount = (mcrOkCount + mcrNgCount).ToString();
                    if ((okCount + ngCount) == 0)
                    {
                        okRate = 0;
                        ngRate = 0;
                    }
                    else
                    {
                        okRate = okCount / (double)(okCount + ngCount) * 100;
                        ngRate = ngCount / (double)(okCount + ngCount) * 100;
                    }
                    if ((mcrOkCount + mcrNgCount) == 0)
                    {
                        mcrOkRate = 0;
                    }
                    else
                    {
                        mcrOkRate = mcrOkCount / (double)(mcrOkCount + mcrNgCount) * 100;
                    }
                    grid[index].OKRate = string.Format("{0:F2}%", okRate);
                    grid[index].NGRate = string.Format("{0:F2}%", ngRate);
                    grid[index].McrReadingRate = string.Format("{0:F2}%", mcrOkRate);
                }
                #endregion(3. UI 표시)
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 배출량 - Cell Output 테이블에 특정 시간대 데이터를 구해서 시간별 카운트를 누적
        /// </summary>
        /// <param name="objDate"></param>
        private void SetCellOutput(DateTime objDate)
        {
            StringBuilder sbQuery = new StringBuilder();
            // Table History Cell Output
            CManagerTable objManagerTableCellOutput = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellOutput;

            try
            {
                // 시작 시간
                int iWorkerTime = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
                DateTime objDateTimeFrom = new DateTime(objDate.Year, objDate.Month, objDate.Day, iWorkerTime, 0, 0);
                DateTime objDateTimeTo = new DateTime(objDate.Year, objDate.Month, objDate.Day, iWorkerTime, 0, 0).AddDays(1d);
                // Day / Hour / Count
                sbQuery.AppendFormat(
                    "SELECT strftime('%d', {0}) AS Day, strftime('%H', {0}) AS Hour, count(*) AS Count FROM {1} WHERE {0} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{2}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{3}') GROUP BY Day, Hour;",
                    objManagerTableCellOutput.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryCellOutput.DATE],
                    objManagerTableCellOutput.HLGetTableName(),
                    objDateTimeFrom.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                    objDateTimeTo.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT)
                    );
                // 데이터 테이블에 데이터베이스 자료 넣음
                DataTable objDataTable = new DataTable();
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(sbQuery.ToString(), ref objDataTable);
                DataRow[] objDataRow = objDataTable.Select();
                int rowIndex = 0;
                int nextTime = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
                int dayShiftDuration = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyEnd - m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
                for (int i = 0; i < 24; i++)
                {
                    if (
                        rowIndex < objDataRow.Length
                        && nextTime == Convert.ToInt32(objDataRow[rowIndex][1])
                        )
                    {
                        string count = Convert.ToString(objDataRow[rowIndex][2]);
                        if (i < dayShiftDuration)
                        {
                            // 낮
                            mDayShiftProductionData[i].OutputCount = count;
                        }
                        else
                        {
                            // 밤
                            mNightShiftProductionData[i - dayShiftDuration].OutputCount = count;
                        }
                        rowIndex++;
                    }
                    else
                    {
                        if (i < dayShiftDuration)
                        {
                            // 낮
                            mDayShiftProductionData[i].OutputCount = "0";
                        }
                        else
                        {
                            // 밤
                            mNightShiftProductionData[i - dayShiftDuration].OutputCount = "0";
                        }
                    }
                    nextTime++;
                    nextTime %= 24;
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 설비 최종 결과 NG, OK 수량 & 비율 - Process Data Cell 테이블에 특정 시간대 데이터를 구해서 시간별 카운트를 누적
        /// </summary>
        /// <param name="objDate"></param>
        private void SetNgOkResult(DateTime objDate)
        {
            StringBuilder sbQuery = new StringBuilder();
            // Table History Machine Result
            CManagerTable objManagerTableMachineResult = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryMachineResult;

            try
            {
                // 시작 시간
                int iWorkerTime = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
                DateTime objDateTimeFrom = new DateTime(objDate.Year, objDate.Month, objDate.Day, iWorkerTime, 0, 0);
                DateTime objDateTimeTo = new DateTime(objDate.Year, objDate.Month, objDate.Day, iWorkerTime, 0, 0).AddDays(1d);
                // Day / Hour / RESULT / Count
                sbQuery.AppendFormat(
                    "SELECT strftime('%d', {0}) AS Day, strftime('%H', {0}) AS Hour, {1}, count(*) AS Count FROM {2} WHERE {0} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{3}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{4}') GROUP BY Day, Hour, {1};",
                    objManagerTableMachineResult.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryMachineResult.DATE],
                    objManagerTableMachineResult.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryMachineResult.RESULT],
                    objManagerTableMachineResult.HLGetTableName(),
                    objDateTimeFrom.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                    objDateTimeTo.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT)
                    );
                // 데이터 테이블에 데이터베이스 자료 넣음
                DataTable objDataTable = new DataTable();
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(sbQuery.ToString(), ref objDataTable);
                DataRow[] objDataRow = objDataTable.Select();
                int rowIndex = 0;
                int nextTime = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
                int dayShiftDuration = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyEnd - m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
                int okCount;
                int ngCount;
                for (int i = 0; i < 24; i++)
                {
                    okCount = 0;
                    ngCount = 0;

                    if (
                        rowIndex < objDataRow.Length
                        && nextTime == Convert.ToInt32(objDataRow[rowIndex][1])
                        )
                    {
                        while (
                            rowIndex < objDataRow.Length
                            && nextTime == Convert.ToInt32(objDataRow[rowIndex][1])
                            )
                        {
                            string machineResult = Convert.ToString(objDataRow[rowIndex][2]);
                            if (true == string.IsNullOrWhiteSpace(machineResult))
                            {
                            }
                            else if ("OK" == machineResult)
                            {
                                okCount += Convert.ToInt32(objDataRow[rowIndex][3]);
                            }
                            else
                            {
                                ngCount += Convert.ToInt32(objDataRow[rowIndex][3]);
                            }
                            rowIndex++;
                        }

                        int totalCount = okCount + ngCount;
                        if (i < dayShiftDuration)
                        {
                            // 낮
                            mDayShiftProductionData[i].OKCount = okCount.ToString();
                            mDayShiftProductionData[i].NGCount = ngCount.ToString();
                            mDayShiftProductionData[i].OKRate = string.Format("{0:F2}%", okCount / (double)totalCount * 100d);
                            mDayShiftProductionData[i].NGRate = string.Format("{0:F2}%", ngCount / (double)totalCount * 100d);
                        }
                        else
                        {
                            // 밤
                            mNightShiftProductionData[i - dayShiftDuration].OKCount = okCount.ToString();
                            mNightShiftProductionData[i - dayShiftDuration].NGCount = ngCount.ToString();
                            mNightShiftProductionData[i - dayShiftDuration].OKRate = string.Format("{0:F2}%", okCount / (double)totalCount * 100d);
                            mNightShiftProductionData[i - dayShiftDuration].NGRate = string.Format("{0:F2}%", ngCount / (double)totalCount * 100d);
                        }
                    }
                    else
                    {
                        if (i < dayShiftDuration)
                        {
                            // 낮
                            mDayShiftProductionData[i].OKCount = "0";
                            mDayShiftProductionData[i].NGCount = "0";
                            mDayShiftProductionData[i].OKRate = "0.00%";
                            mDayShiftProductionData[i].NGRate = "0.00%";
                        }
                        else
                        {
                            // 밤
                            mNightShiftProductionData[i - dayShiftDuration].OKCount = "0";
                            mNightShiftProductionData[i - dayShiftDuration].NGCount = "0";
                            mNightShiftProductionData[i - dayShiftDuration].OKRate = "0.00%";
                            mNightShiftProductionData[i - dayShiftDuration].NGRate = "0.00%";
                        }
                    }
                    nextTime++;
                    nextTime %= 24;
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// MCR 리딩 수량 - MCR 테이블에 특정 시간대 데이터를 구해서 시간별 카운트를 누적
        /// </summary>
        /// <param name="objDate"></param>
        private void SetMCRRate(DateTime objDate)
        {
            StringBuilder sbQuery = new StringBuilder();
            // Table History MCR Status
            CManagerTable objManagerTableDataReader = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataReader;
            string strReaderResultCodeOK = CCIMDefine.ReaderResultCode.OK;

            try
            {
                // 시작 시간
                int iWorkerTime = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
                DateTime objDateTimeFrom = new DateTime(objDate.Year, objDate.Month, objDate.Day, iWorkerTime, 0, 0);
                DateTime objDateTimeTo = new DateTime(objDate.Year, objDate.Month, objDate.Day, iWorkerTime, 0, 0).AddDays(1d);
                // Day / Hour / ResultCode / Count
                sbQuery.AppendFormat(
                    "SELECT strftime('%d', {0}) AS Day, strftime('%H', {0}) AS Hour, {1}, count(*) AS Count FROM {2} WHERE {0} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{3}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{4}') GROUP BY Day, Hour, {1};",
                    objManagerTableDataReader.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataReader.DATE],
                    objManagerTableDataReader.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataReader.READER_RESULT_CODE],
                    objManagerTableDataReader.HLGetTableName(),
                    objDateTimeFrom.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                    objDateTimeTo.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT)
                    );
                // 데이터 테이블에 데이터베이스 자료 넣음
                DataTable objDataTable = new DataTable();
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(sbQuery.ToString(), ref objDataTable);
                DataRow[] objDataRow = objDataTable.Select();
                int rowIndex = 0;
                int nextTime = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
                int dayShiftDuration = m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyEnd - m_objDocument.m_objConfig.GetSystemParameter().iWorkerTimeWeeklyStart;
                int okCount;
                int ngCount;
                for (int i = 0; i < 24; i++)
                {
                    okCount = 0;
                    ngCount = 0;

                    if (
                        rowIndex < objDataRow.Length
                        && nextTime == Convert.ToInt32(objDataRow[rowIndex][1])
                        )
                    {
                        while (
                            rowIndex < objDataRow.Length
                            && nextTime == Convert.ToInt32(objDataRow[rowIndex][1])
                            )
                        {
                            string resultCode = Convert.ToString(objDataRow[rowIndex][2]);
                            if (true == string.IsNullOrWhiteSpace(resultCode))
                            {
                            }
                            else if (strReaderResultCodeOK == resultCode)
                            {
                                okCount += Convert.ToInt32(objDataRow[rowIndex][3]);
                            }
                            else
                            {
                                ngCount += Convert.ToInt32(objDataRow[rowIndex][3]);
                            }

                            rowIndex++;
                        }

                        int totalCount = okCount + ngCount;
                        if (i < dayShiftDuration)
                        {
                            // 낮
                            mDayShiftProductionData[i].McrReadingCount = okCount.ToString();
                            mDayShiftProductionData[i].McrReadingRate = string.Format("{0:F2}%", (double)okCount / totalCount * 100d);
                        }
                        else
                        {
                            // 밤
                            mNightShiftProductionData[i - dayShiftDuration].McrReadingCount = okCount.ToString();
                            mNightShiftProductionData[i - dayShiftDuration].McrReadingRate = string.Format("{0:F2}%", (double)okCount / totalCount * 100d);
                        }
                    }
                    else
                    {
                        if (i < dayShiftDuration)
                        {
                            // 낮
                            mDayShiftProductionData[i].McrReadingCount = "0";
                            mDayShiftProductionData[i].McrReadingRate = "0.00%";
                        }
                        else
                        {
                            // 밤
                            mNightShiftProductionData[i - dayShiftDuration].McrReadingCount = "0";
                            mNightShiftProductionData[i - dayShiftDuration].McrReadingRate = "0.00%";
                        }
                    }
                    nextTime++;
                    nextTime %= 24;
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 그리드 뷰에 데이터 합계를 내줌
        /// </summary>
        /// <param name="objGridView"></param>
        /// <returns></returns>
        private int GetTotalInput(DataGridView objGridView)
        {
            int iReturn = 0;

            do
            {
                int iRowCount = 0;
                for (int iLoopRow = 0; iLoopRow < objGridView.Rows.Count; iLoopRow++)
                {
                    try
                    {
                        iRowCount += Convert.ToInt32(objGridView[(int)EProductionListColumn.INPUT, iLoopRow].Value);
                    }
                    catch (Exception ex)
                    {
                        LogWrite.Exception(ex);
                    }
                }
                iReturn = iRowCount;

            } while (false);

            return iReturn;
        }

        /// <summary>
        /// 그리드 뷰에 데이터 합계를 내줌
        /// </summary>
        /// <param name="objGridView"></param>
        /// <returns></returns>
        private int GetTotalOutput(DataGridView objGridView)
        {
            int iReturn = 0;

            do
            {
                int iRowCount = 0;
                for (int iLoopRow = 0; iLoopRow < objGridView.Rows.Count; iLoopRow++)
                {
                    try
                    {
                        iRowCount += Convert.ToInt32(objGridView[(int)EProductionListColumn.OUTPUT, iLoopRow].Value);
                    }
                    catch (Exception ex)
                    {
                        LogWrite.Exception(ex);
                    }
                }
                iReturn = iRowCount;

            } while (false);

            return iReturn;
        }

        /// <summary>
        /// UI 상태를 업데이트하는 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (DateTimeFrom.Value.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd"))
            {
                SetButtonBackColor(BtnToday, m_colorClick);
            }
            else
            {
                SetButtonBackColor(BtnToday, m_colorNormal);
            }
        }

        private void backgroundWorkerUpdate_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // 오늘 날짜일 때에는 실시간으로 업데이트하고, 아니면 최초 한번만 업데이트 한다.
            DateTime selectDay = new DateTime(DateTimeFrom.Value.Year, DateTimeFrom.Value.Month, DateTimeFrom.Value.Day);
            if (
                DateTime.Today != selectDay
                && mLastUpdateDateTime == selectDay
                )
            {
                return;
            }

            // 데이터베이스 현재 날짜로 초기화 & 데이터베이스 연동
            InitializeDatabase();
            SetUpdateGridViewFromRawdata();

            if (true == BtnTotalInputWeekly.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    // 그리드 뷰에 표시되는 생산량 토탈 표시해줌
                    // 주간 그룹
                    base.SetButtonText(this.BtnTotalInputWeekly, string.Format("{0}", GetTotalInput(this.GridViewWeeklyGroup)));
                    base.SetButtonText(this.BtnTotalOutputWeekly, string.Format("{0}", GetTotalOutput(this.GridViewWeeklyGroup)));
                    // 야간 그룹
                    base.SetButtonText(this.BtnTotalInputNight, string.Format("{0}", GetTotalInput(this.GridViewNightGroup)));
                    base.SetButtonText(this.BtnTotalOutputNight, string.Format("{0}", GetTotalOutput(this.GridViewNightGroup)));
                }));
            }
        }

        /// <summary>
        /// 테이블을 업데이트 하는 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            // 데이터베이스 현재 날짜로 초기화 & 데이터베이스 연동
            if (false == backgroundWorkerUpdate.IsBusy)
            {
                backgroundWorkerUpdate.RunWorkerAsync();
            }
        }

        /// <summary>
        /// 오늘 통계 보기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnToday_Click(object sender, EventArgs e)
        {
            DateTimeFrom.Value = DateTime.Now;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [DateTime : {1:yyyyMMdd}]", "BtnToday_Click", DateTimeFrom.Value);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 이전날 통계 보기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            DateTimeFrom.Value = DateTimeFrom.Value.AddDays(-1);

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [DateTime : {1:yyyyMMdd}]", "BtnPrevious_Click", DateTimeFrom.Value);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 다음날 통계 보기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNext_Click(object sender, EventArgs e)
        {
            DateTimeFrom.Value = DateTimeFrom.Value.AddDays(1);

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [DateTime : {1:yyyyMMdd}]", "BtnNext_Click", DateTimeFrom.Value);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 날짜 변경시 그리드 업데이트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTimeFrom_ValueChanged(object sender, EventArgs e)
        {
            // 데이터베이스 현재 날짜로 초기화 & 데이터베이스 연동
            if (false == backgroundWorkerUpdate.IsBusy)
            {
                backgroundWorkerUpdate.RunWorkerAsync();
            }
        }
    }
}