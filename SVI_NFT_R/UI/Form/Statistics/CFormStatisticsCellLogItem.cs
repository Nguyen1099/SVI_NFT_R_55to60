using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormStatisticsCellLogItem : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 자재 로그 리스트 칼럼 정의
        /// </summary>
        private enum ECellLogItemListColumn
        {
            DATE = 0,
            INNER_ID,
            CELL_ID,
            CH_NAME,
            PPID,
            EQ_RESULT,
            CIM_RESULT,
            MACHINE_RESULT,
            JUDGE,
            REASON_CODE,
            DESCRIPTION,
            INPUT_TIME,
            OUTPUT_TIME,
            UNIT_TACT_TIME,
            IN_SHUTTLE_START_TIME,
            IN_SHUTTLE_END_TIME,
            IN_SHUTTLE_UNIT_TACT_TIME,
            ALIGN_STAGE_START_TIME,
            ALIGN_STAGE_END_TIME,
            ALIGN_STAGE_UNIT_TACT_TIME,
            ALIGN_GRAB_START_TIME,
            ALIGN_GRAB_END_TIME,
            ALIGN_GRAB_UNIT_TACT_TIME,
            ALIGN_COUNT,
            NACHI_IN_ROBOT_START_TIME,
            NACHI_IN_ROBOT_END_TIME,
            NACHI_IN_ROBOT_UNIT_TACT_TIME,
            MCR_COUNT,
            INSPECTION_STAGE_START_TIME,
            INSPECTION_STAGE_END_TIME,
            INSPECTION_STAGE_UNIT_TACT_TIME,
            INSPECTION_START_TIME,
            INSPECTION_END_TIME,
            INSPECTION_UNIT_TACT_TIME,
            VISION_RESULT,
            VISION_DEFECT,
            NACHI_OUT_ROBOT_START_TIME,
            NACHI_OUT_ROBOT_END_TIME,
            NACHI_OUT_ROBOT_UNIT_TACT_TIME,
            REVIEW_RESULT,
            REVIEW_DEFECT,
            CELL_LOG_ITEM_LIST_COLUMN_FINAL
        }
        // 해당 날짜로 검색된 데이터를 갖고 있음
        private DataTable m_objDataTable;
        private DataRow[] m_objDataRow;
        private string m_strLastAlign;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormStatisticsCellLogItem(CDocument objDocument)
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
        private void CFormStatisticsCellLogItem_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormStatisticsCellLogItem_FormClosed(object sender, FormClosedEventArgs e)
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
                if (false == InitializeGridView(this.GridViewCellLogItemList))
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
        private void GridViewCellLogItemList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
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
            base.SetButtonBackColor(this.BtnTitleCellLogItemList, m_colorLabel);
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
                SetButtonChangeLanguage(this.BtnTitleCellLogItemList);
                SetButtonChangeLanguage(this.BtnSelectAsc);
                SetButtonChangeLanguage(this.BtnSelectDesc);
                SetButtonChangeLanguage(this.BtnSaveToCsv);

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
                // Cell Log Item History 데이터베이스 연동
                SetCellLogItemHistoryConnect(this.DateTimeFrom.Value, this.DateTimeTo.Value);
                // DB 리스트 그리드 뷰 초기화
                InitializeGridView(this.GridViewCellLogItemList);

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
                // 그리드 뷰 칼럼 추가
                objGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (m_objDataTable != null)
                {
                    if (objGridView.Columns.Count != m_objDataTable.Columns.Count)
                    {
                        objGridView.Columns.Clear();
                        for (int iLoopColumn = 0; iLoopColumn < m_objDataTable.Columns.Count; iLoopColumn++)
                        {
                            objGridView.Columns.Add(m_objDataTable.Columns[iLoopColumn].ToString(), m_objDataTable.Columns[iLoopColumn].ToString());
                        }
                    }
                    // 그리드 뷰 칼럼 정렬 x
                    for (int iLoopColumn = 0; iLoopColumn < objGridView.Columns.Count; iLoopColumn++)
                    {
                        objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                        objGridView.Columns[iLoopColumn].Width = (int)(objGridView.Width * 0.2);
                        objGridView.Columns[iLoopColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    objGridView.Columns[(int)ECellLogItemListColumn.CELL_ID].Width = 200;

                    // 전체 행 수를 잡아줌
                    objGridView.RowCount = m_objDataRow.Length;
                    // 그리드 뷰 크기 변경
                    base.SetGridViewFont(objGridView, 9.0);
                }

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
        /// 자재 로그 이력 데이터베이스 연동
        /// </summary>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        private void SetCellLogItemHistoryConnect(DateTime objFrom, DateTime objTo)
        {
            // 디폴트 내림차순
            SetCellLogItemHistoryConnect(objFrom, objTo, m_strLastAlign);
        }

        /// <summary>
        /// 자재 로그 이력 데이터베이스 연동
        /// </summary>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        /// <param name="strOrder"></param>
        private void SetCellLogItemHistoryConnect(DateTime objFrom, DateTime objTo, string strOrder)
        {
            var sbQuery = new StringBuilder();
            // Process Data Cell
            CManagerTable objProcessDataCell = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataCell;
            // Process Data Judgement
            CManagerTable objProcessDataJudgement = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataJudgement;
            // Process Data Reader
            CManagerTable objProcessDataReader = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataReader;

            try
            {
                // 셀 데이터 테이블이랑 택타임 테이블 조인함
                // SELECT
                sbQuery.Clear();
                sbQuery.Append("SELECT ");
                string[] selectColumns = new string[]
                {
                    string.Format("t1.{0}", objProcessDataCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.DATE]),
                    string.Format("t1.{0}", objProcessDataCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.INNER_ID]),
                    string.Format("t1.{0}", objProcessDataCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.CELL_ID]),
                    string.Format("t1.{0}", objProcessDataCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.CH_NAME]),
                    string.Format("t1.{0}", objProcessDataCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.PPID]),
                    string.Format("t1.{0}", objProcessDataCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.CIM_RESULT]),
                    string.Format("t1.{0}", objProcessDataCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.MACHINE_RESULT]),
                    string.Format("t3.{0}", objProcessDataJudgement.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataJudgement.JUDGE]),
                    string.Format("t3.{0}", objProcessDataJudgement.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataJudgement.REASON_CODE]),
                    string.Format("t3.{0}", objProcessDataJudgement.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataJudgement.DESCRIPTION]),
                    string.Format("t1.{0} AS 'ALIGN_RETRY_COUNT'", objProcessDataCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.ALIGN_COUNT]),
                    string.Format("t4.{0} AS 'MCR_RETRY_COUNT'", objProcessDataReader.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataReader.RETRY_COUNT]),
                    string.Format("t1.{0} AS 'INSP_RESULT'", objProcessDataCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.FRONT_INSP_RESULT]),
                    string.Format("t1.{0} AS 'INSP_REASONCODES'", objProcessDataCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.FRONT_INSP_REASONCODES]),
                };
                for (int i = 0; i < selectColumns.Length; i++)
                {
                    if ((i + 1) != selectColumns.Length)
                    {
                        sbQuery.AppendFormat("{0}, ", selectColumns[i]);
                    }
                    else
                    {
                        sbQuery.AppendFormat("{0}", selectColumns[i]);
                    }
                }
                // FROM (INNER JOIN)
                sbQuery.Append(" FROM");
                sbQuery.AppendFormat(" {0} AS t1", objProcessDataCell.HLGetTableName());
                sbQuery.AppendFormat(" INNER JOIN {0} AS t3 USING({1})", objProcessDataJudgement.HLGetTableName(), objProcessDataJudgement.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataJudgement.INNER_ID]);
                sbQuery.AppendFormat(" INNER JOIN {0} AS t4 USING({1})", objProcessDataReader.HLGetTableName(), objProcessDataReader.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataReader.INNER_ID]);
                // WHERE
                sbQuery.Append(" WHERE");
                sbQuery.AppendFormat(
                    " t1.{0} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{1}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{2}') {3};",
                    objProcessDataCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.DATE],
                    string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                    string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                    CDatabaseDefine.DEF_RECORD_READ_LIMIT
                    );

                m_objDataTable = new DataTable();
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(sbQuery.ToString(), ref m_objDataTable);
                m_objDataRow = m_objDataTable.Select("", objProcessDataCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.DATE] + " " + strOrder);
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
            strPath = string.Format(@"{0}\{1}_{2}.csv", strPath, "CELL_LOG_ITEM", string.Format("{0:yyyyMMdd_HHmm}", DateTime.Now));
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

            // Cell Log Item History 데이터베이스 연동
            SetCellLogItemHistoryConnect(this.DateTimeFrom.Value, this.DateTimeTo.Value);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(this.GridViewCellLogItemList);
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

            // Cell Log Item History 데이터베이스 연동
            SetCellLogItemHistoryConnect(this.DateTimeFrom.Value, this.DateTimeTo.Value);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridView(this.GridViewCellLogItemList);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [From : {1}] [To : {2}]", "BtnSelectDesc_Click", this.DateTimeFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), this.DateTimeTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT));
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }
    }
}
