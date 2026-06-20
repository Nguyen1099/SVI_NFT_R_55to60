using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormStatisticsTactTime : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 유닛 택타임 칼럼 정의
        /// </summary>
        private enum EUnitTactTimeListColumn
        {
            UNIT = 0,
            WAIT_LOADING,
            PURE_CYCLE_TACT,
            WAIT_UNLOADING,
        };
        /// <summary>
        /// 유닛 택타임 상세내용 칼럼 정의
        /// </summary>
        private enum EUnitTactTimeUnitDetailColumn
        {
            TYPE = 0,
            VALUE,
        };
        /// <summary>
        /// 배출 택타임 칼럼 정의
        /// </summary>
        private enum EOutputTactTimeListColumn
        {
            Index = 0,
            UnitType,
            WaitLoading,
            WaitUnloading,
            Inspection,
            OutTact
        };
        /// <summary>
        /// 배출 택타임 최대 표시
        /// </summary>
        private int m_iTactTimeMaxCount;
        private TactTime.Tact[] mOutputTactTimeList;
        private const int SIMPLE_OUT_TACT_ROW_COUNT = 30;
        private const int SIMPLE_OUT_TACT_COLUMN_COUNT = 4;
        private bool IsSimpleOutTactTimeView
        {
            get => m_objDocument.m_objConfig.GetSystemParameter().bIsOutTactTimeSimpleView;
            set
            {
                m_objDocument.m_objConfig.GetSystemParameter().bIsOutTactTimeSimpleView = value;
                try
                {
                    m_objDocument.m_objConfig.SaveSystemParameter(m_objDocument.m_objConfig.GetSystemParameter());
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormStatisticsTactTime(CDocument objDocument)
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
        private void CFormStatisticsTactTime_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormStatisticsTactTime_FormClosed(object sender, FormClosedEventArgs e)
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
                // 배출 택타임 최대 표시 갯수
                m_iTactTimeMaxCount = m_objDocument.m_objConfig.GetSystemParameter().iTactTimeMaxCount;
                // 유닛 택타임 리스트 그리드 뷰 초기화
                string[] strUnitTactTimeList = Enum.GetNames(typeof(EUnitTactTimeListColumn));
                if (false == InitializeGridView(this.GridViewTactTimeUnit, strUnitTactTimeList))
                {
                    break;
                }
                // 유닛 택타임 리스트 상세내용 그리드 뷰 초기화
                string[] strUnitTactTimeUnitDetail = Enum.GetNames(typeof(EUnitTactTimeUnitDetailColumn));
                if (false == InitializeGridView(this.GridViewTactTimeUnitDetail, strUnitTactTimeUnitDetail))
                {
                    break;
                }
                // 배출 택타임 리스트 그리드 뷰 초기화
                string[] strOutputTactTimeList = Enum.GetNames(typeof(EOutputTactTimeListColumn));
                if (false == InitializeGridView(this.GridViewTactTimeOutput, strOutputTactTimeList))
                {
                    break;
                }
                GridViewTactTimeOutput.Columns[(int)EOutputTactTimeListColumn.Index].Width = 55;
                GridViewTactTimeOutput.Columns[(int)EOutputTactTimeListColumn.UnitType].Width = 120;
                GridViewTactTimeUnit.Columns[(int)EUnitTactTimeListColumn.UNIT].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                InitializeSimpleOutTactGridView();
                // 유닛 Row 추가
                string[] strUnitName = Enum.GetNames(typeof(ETactTime));
                AddUnitRow(strUnitName);
                // 인덱스 Row 추가
                AddIndexRow(m_iTactTimeMaxCount);
                SetSimpleOutTactCount(m_iTactTimeMaxCount);
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
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            base.SetButtonBackColor(this.BtnTitleTactTimeUnit, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleTactTimeUnitDetail, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleTactTimeOutput, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleSlowUnit, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleFastUnit, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnSlowUnit, m_colorLabelData);
            base.SetButtonBackColor(this.BtnFastUnit, m_colorLabelData);
            base.SetButtonBackColor(this.BtnClearUnit, m_colorNormal);
            base.SetButtonBackColor(this.BtnTitleAverage, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnAverage, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTitleMaxLength, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnMaxLength, m_colorLabelData);
            base.SetButtonBackColor(this.BtnClearOutput, m_colorNormal);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnMaxLength.Name)
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
                SetButtonChangeLanguage(this.BtnTitleTactTimeUnit);
                SetButtonChangeLanguage(this.BtnTitleTactTimeUnitDetail);
                SetButtonChangeLanguage(this.BtnTitleTactTimeOutput);
                SetButtonChangeLanguage(this.BtnTitleSlowUnit);
                SetButtonChangeLanguage(this.BtnTitleFastUnit);
                SetButtonChangeLanguage(this.BtnClearUnit);
                SetButtonChangeLanguage(this.BtnTitleAverage);
                SetButtonChangeLanguage(this.BtnTitleMaxLength);
                SetButtonChangeLanguage(this.BtnClearOutput);
                SetButtonChangeLanguage(BtnOutTactSimpleView);

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
                if (MtbiDataCollector.IsStarted == true)
                {
                    // MTBi 데이터 수집기가 동작중일 때 초기 파라미터 설정
                    IsSimpleOutTactTimeView = true;
                }

                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                m_objDocument.GetMainFrame().SetCurrentForm(this);
                // 업데이트
                SetUpdateTactTimeDetailData();
                SetSimpleOutTactCount(m_iTactTimeMaxCount);
            }
        }

        private void InitializeSimpleOutTactGridView()
        {
            DgvSimpleOutTactTime.VirtualMode = true;
            DgvSimpleOutTactTime.BackgroundColor = Color.White;
            DgvSimpleOutTactTime.ColumnHeadersVisible = false;
            DgvSimpleOutTactTime.RowHeadersVisible = false;
            DgvSimpleOutTactTime.ReadOnly = true;
            DgvSimpleOutTactTime.CellValueNeeded += (sender, e) =>
            {
                if (null == mOutputTactTimeList)
                {
                    return;
                }

                int itemIndex = e.RowIndex + e.ColumnIndex / 2 * SIMPLE_OUT_TACT_ROW_COUNT;
                if (itemIndex >= m_iTactTimeMaxCount)
                {
                    e.Value = "";
                    return;
                }
                if (e.ColumnIndex % 2 == 0)
                {
                    e.Value = $"{itemIndex + 1:000}";
                }
                else
                {
                    e.Value = itemIndex < mOutputTactTimeList.Length ? $" {mOutputTactTimeList[itemIndex].OutTact.Value.TotalSeconds:0.000} ( sec )" : "";
                }
            };
        }

        private void SetSimpleOutTactCount(int count)
        {
            int columnCount = count / SIMPLE_OUT_TACT_ROW_COUNT + (count % SIMPLE_OUT_TACT_ROW_COUNT != 0 ? 1 : 0);
            int rowCount = count > SIMPLE_OUT_TACT_ROW_COUNT ? SIMPLE_OUT_TACT_ROW_COUNT : count;
            DgvSimpleOutTactTime.ColumnCount = columnCount * 2;
            DgvSimpleOutTactTime.RowCount = rowCount + 1;
            int cellWidth = DgvSimpleOutTactTime.Width / SIMPLE_OUT_TACT_COLUMN_COUNT;
            cellWidth--;
            for (int columnIndex = 0; columnIndex < DgvSimpleOutTactTime.ColumnCount; ++columnIndex)
            {
                DgvSimpleOutTactTime.Columns[columnIndex].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvSimpleOutTactTime.Columns[columnIndex].Resizable = DataGridViewTriState.False;
                DgvSimpleOutTactTime.Columns[columnIndex].DefaultCellStyle.BackColor = columnIndex % 2 == 0 ? Color.LightGray : Color.White;
                DgvSimpleOutTactTime.Columns[columnIndex].DefaultCellStyle.Alignment = columnIndex % 2 == 0 ? DataGridViewContentAlignment.MiddleCenter : DataGridViewContentAlignment.MiddleRight;
                DgvSimpleOutTactTime.Columns[columnIndex].Width = columnIndex % 2 == 0 ? Convert.ToInt32(cellWidth * 0.3) : Convert.ToInt32(cellWidth * 0.7);
            }
            int cellHeight = DgvSimpleOutTactTime.Height / SIMPLE_OUT_TACT_ROW_COUNT;
            cellHeight--;
            for (int rowIndex = 0; rowIndex < DgvSimpleOutTactTime.RowCount; ++rowIndex)
            {
                DgvSimpleOutTactTime.Rows[rowIndex].Resizable = DataGridViewTriState.False;
                DgvSimpleOutTactTime.Rows[rowIndex].Height = cellHeight;
            }
            DgvSimpleOutTactTime.Rows[DgvSimpleOutTactTime.RowCount - 1].Height = -1;
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
                // 그리드 뷰 칼럼 추가
                for (int iLoopColumn = 0; iLoopColumn < strColumnName.Length; iLoopColumn++)
                {
                    objGridView.Columns.Add(string.Format("{0}", iLoopColumn), strColumnName[iLoopColumn]);
                    // 칼럼 정렬 기능 x
                    objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, 9.0);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 그리드 뷰에 유닛 이름 항목 표시
        /// </summary>
        /// <param name="strUnitName"></param>
        private void AddUnitRow(string[] strUnitName)
        {
            DataGridView objGridView = this.GridViewTactTimeUnit;

            for (int iLoopUnit = 0; iLoopUnit < strUnitName.Length; iLoopUnit++)
            {
                string[] strRowData = new string[Enum.GetNames(typeof(EUnitTactTimeListColumn)).Length];
                strRowData[(int)EUnitTactTimeListColumn.UNIT] = strUnitName[iLoopUnit];
                objGridView.Rows.Add(strRowData);
            }
        }

        /// <summary>
        /// 그리드 뷰에 인덱스 항목 표시
        /// </summary>
        /// <param name="iMaxLength"></param>
        private void AddIndexRow(int iMaxLength)
        {
            DataGridView objGridView = this.GridViewTactTimeOutput;

            objGridView.RowCount = iMaxLength;

            // 첫 행 포커스 해제
            objGridView.ClearSelection();
        }

        /// <summary>
        /// 유닛 택타임 목록 갱신
        /// 설명 : 해당 택타임 데이터가 어떤 구조로 올지 아직 미정
        /// </summary>
        private void SetUnitTactTimeListGridView()
        {
            DataGridView objGridView = this.GridViewTactTimeUnit;

            for (int iLoopTactTime = 0; iLoopTactTime < m_objDocument.m_objTactTime.CycleTactTime.Count; iLoopTactTime++)
            {
                base.SetGridViewCellData(objGridView, (int)EUnitTactTimeListColumn.WAIT_LOADING, iLoopTactTime, string.Format("{0,8:F3} ( sec )", m_objDocument.m_objTactTime.LoadingWaitTime[(ETactTime)iLoopTactTime]));
                base.SetGridViewCellData(objGridView, (int)EUnitTactTimeListColumn.PURE_CYCLE_TACT, iLoopTactTime, string.Format("{0,8:F3} ( sec )", m_objDocument.m_objTactTime.GetPureCycleTactTime((ETactTime)iLoopTactTime)));
                base.SetGridViewCellData(objGridView, (int)EUnitTactTimeListColumn.WAIT_UNLOADING, iLoopTactTime, string.Format("{0,8:F3} ( sec )", m_objDocument.m_objTactTime.UnloadingWaitTime[(ETactTime)iLoopTactTime]));
            }
        }

        /// <summary>
        /// 유닛 택타임 목록 갱신
        /// </summary>
        private void SetUnitTactTimeDetailGridView()
        {
            if (0 >= GridViewTactTimeUnit.SelectedRows.Count)
            {
                return;
            }

            DataGridView objGridView = this.GridViewTactTimeUnitDetail;

            var selectRowIndex = (ETactTime)GridViewTactTimeUnit.SelectedRows[0].Index;
            var selectData = m_objDocument.m_objTactTime.UnitTactTimeDetailInformation[selectRowIndex];
            int rowIndex = 0;

            if (selectData.Count != GridViewTactTimeUnitDetail.Rows.Count)
            {
                SetUpdateTactTimeDetailData();
            }

            foreach (var item in selectData)
            {
                base.SetGridViewCellData(objGridView, (int)EUnitTactTimeUnitDetailColumn.TYPE, rowIndex, item.Key);
                base.SetGridViewCellData(objGridView, (int)EUnitTactTimeUnitDetailColumn.VALUE, rowIndex, item.Value);
                rowIndex++;
            }
        }

        /// <summary>
        /// 유닛 택타임 가운데 가장 느린 유닛명 리턴
        /// </summary>
        /// <returns></returns>
        private string GetUnitTactTimeSlow()
        {
            string strReturn = "";
            DataGridView objGridView = this.GridViewTactTimeUnit;

            do
            {
                double dMaxTactTime = double.MinValue;
                int iMaxIndex = 0;
                int iLoopRow = 0;
                foreach (var item in m_objDocument.m_objTactTime.GetAllPureCycleTactTime())
                {
                    if (
                        (int)ETactTime.JAVAS_INSPECTION != iLoopRow
                        && dMaxTactTime < item
                        )
                    {
                        dMaxTactTime = item;
                        iMaxIndex = iLoopRow;
                    }
                    iLoopRow++;
                }

                // 그리드 뷰에서 해당하는 인덱스에 유닛명을 뽑아냄
                try
                {
                    strReturn = objGridView[(int)EUnitTactTimeListColumn.UNIT, iMaxIndex].Value.ToString();
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);

            return strReturn;
        }

        /// <summary>
        /// 유닛 택타임 가운데 가장 빠른 유닛명 리턴
        /// </summary>
        /// <returns></returns>
        private string GetUnitTactTimeFast()
        {
            string strReturn = "";
            DataGridView objGridView = this.GridViewTactTimeUnit;

            do
            {
                double dMinTactTime = double.MaxValue;
                int iMinIndex = 0;
                int iLoopRow = 0;
                foreach (var item in m_objDocument.m_objTactTime.GetAllPureCycleTactTime())
                {
                    if (
                        (int)ETactTime.JAVAS_INSPECTION != iLoopRow
                        && dMinTactTime > item
                        )
                    {
                        dMinTactTime = item;
                        iMinIndex = iLoopRow;
                    }
                    iLoopRow++;
                }

                // 그리드 뷰에서 해당하는 인덱스에 유닛명을 뽑아냄
                try
                {
                    strReturn = objGridView[(int)EUnitTactTimeListColumn.UNIT, iMinIndex].Value.ToString();
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);

            return strReturn;
        }

        /// <summary>
        /// 평균 배출 택타임 문자열 리턴
        /// </summary>
        /// <returns></returns>
        private string GetOutputTactTimeAverage()
        {
            string strReturn = "";

            do
            {
                // 택타임 뿌려줌
                double dTotalTactTime = 0;
                var dOutputTactTimeList = m_objDocument.m_objTactTime.GetOutputTactTime();
                int outCellCount = 0;
                int outTactTimeCount = dOutputTactTimeList.Length;
                for (int iLoopRow = 0; iLoopRow < outTactTimeCount; iLoopRow++)
                {
                    dTotalTactTime += dOutputTactTimeList[iLoopRow].OutTact.Value.TotalSeconds;
                    outCellCount += dOutputTactTimeList[iLoopRow].CellCount;
                }

                double dAverage = 0.0;
                if (0 != outTactTimeCount)
                {
                    dAverage = dTotalTactTime / outTactTimeCount;
                }

                strReturn = string.Format("{0,8:F3} ( sec )\n[{1,4}/{2,4} ( cycle ), {3,4} ( pcs. )]", dAverage, outTactTimeCount, m_iTactTimeMaxCount, outCellCount);
            } while (false);

            return strReturn;
        }

        /// <summary>
        /// 배출 택타임 목록 갱신
        /// </summary>
        private void SetOutputTactTimeListGridView()
        {
            if (DgvSimpleOutTactTime.Visible != IsSimpleOutTactTimeView)
            {
                DgvSimpleOutTactTime.Visible = IsSimpleOutTactTimeView;
            }
            SetButtonBackColor(BtnOutTactSimpleView, IsSimpleOutTactTimeView ? m_colorClick : m_colorNormal);

            DataGridView objGridView = this.GridViewTactTimeOutput;

            do
            {
                // Max Length 에 따라 테이블이 가변적으로 변동되어야 함
                if (objGridView.Rows.Count == m_iTactTimeMaxCount)
                {
                    break;
                }
                // 테이블 row 비교해서 다르면 클리어하고 새로 row 생성해줌
                objGridView.Rows.Clear();
                // 인덱스 Row 추가
                AddIndexRow(m_iTactTimeMaxCount);
                SetSimpleOutTactCount(m_iTactTimeMaxCount);
                // 택타임 최대 개수 설정
                m_objDocument.m_objTactTime.SetOutputTactTimeMaxCount(m_iTactTimeMaxCount);
                // Config에 저장
                CConfig.CSystemParameter objSystemParameter = m_objDocument.m_objConfig.GetSystemParameter();
                objSystemParameter.iTactTimeMaxCount = m_iTactTimeMaxCount;
                m_objDocument.m_objConfig.SaveSystemParameter(objSystemParameter);

            } while (false);

            do
            {
                var beforeData = mOutputTactTimeList;

                // 택타임 뿌려줌
                mOutputTactTimeList = m_objDocument.m_objTactTime.GetOutputTactTime().ToArray();

                // 그리드 다시 그림
                if (
                    null == beforeData
                    || beforeData.Length != mOutputTactTimeList.Length
                    )
                {
                    objGridView.Invalidate();
                    DgvSimpleOutTactTime.Invalidate();
                }
                else
                {
                    for (int i = 0; i < beforeData.Length; i++)
                    {
                        if (
                            beforeData[i].CellCount != mOutputTactTimeList[i].CellCount
                            || beforeData[i].OutTact != mOutputTactTimeList[i].OutTact
                            )
                        {
                            objGridView.Invalidate();
                            DgvSimpleOutTactTime.Invalidate();
                            break;
                        }
                    }
                }
            } while (false);
        }

        /// <summary>
        /// 유닛 택타임 데이터 클리어
        /// </summary>
        private void SetUnitTactTimeDataClear()
        {
            m_objDocument.m_objTactTime.ClearCycleTactTime();
        }

        /// <summary>
        /// 배출 택타임 데이터 클리어
        /// </summary>
        private void SetOutputTactTimeDataClear()
        {
            m_objDocument.m_objTactTime.SetOutputTactTimeClear();
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // 유닛 택타임 테이블 갱신
            SetUnitTactTimeListGridView();
            // 유닛 택타임 테이블 갱신
            SetUnitTactTimeDetailGridView();
            // 유닛 택타임 가운데 가장 느린 유닛명 리턴
            base.SetButtonText(this.BtnSlowUnit, GetUnitTactTimeSlow());
            // 유닛 택타임 가운데 가장 빠른 유닛명 리턴
            base.SetButtonText(this.BtnFastUnit, GetUnitTactTimeFast());
            // 배출 택타임 테이블 갱신
            SetOutputTactTimeListGridView();
            // 배출 택타임 평균 산출
            base.SetButtonText(this.BtnAverage, GetOutputTactTimeAverage());
            // 배출 택타임 최대 표시 값 표시
            base.SetButtonText(this.BtnMaxLength, string.Format("{0}", m_iTactTimeMaxCount));
        }

        /// <summary>
        /// 유닛 택타임 클리어
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearUnit_Click(object sender, EventArgs e)
        {
            SetUnitTactTimeDataClear();
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnClearUnit_Click", "Unit Tact Time Data Clear");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 출력 택타임 클리어
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearOutput_Click(object sender, EventArgs e)
        {
            SetOutputTactTimeDataClear();
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnClearOutput_Click", "Output Tact Time Data Clear");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 최대 길이값 갱신
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMaxLength_Click(object sender, EventArgs e)
        {
            try
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnMaxLength_Click", true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                using (FormKeyPad objKeyPad = new FormKeyPad(Convert.ToDouble((sender as Button).Text)))
                {
                    if (System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog())
                    {
                        m_iTactTimeMaxCount = (0 < (int)objKeyPad.m_dResultValue) ? (int)objKeyPad.m_dResultValue : 0;
                    }
                }
                // 버튼 로그 추가
                strLog = string.Format("[{0}] [TactTimeMaxCount : {1:D}] [{2}]", "BtnMaxLength_Click", m_iTactTimeMaxCount, false);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        private void SetUpdateTactTimeDetailData()
        {
            DataGridView objGridView = this.GridViewTactTimeUnitDetail;
            objGridView.Rows.Clear();
            var selectRowIndex = (ETactTime)GridViewTactTimeUnit.SelectedRows[0].Index;
            var selectData = m_objDocument.m_objTactTime.UnitTactTimeDetailInformation[selectRowIndex];
            int rowIndex = 0;
            foreach (var item in selectData.Keys)
            {
                objGridView.Rows.Add(item);
                rowIndex++;
            }
        }

        private void GridViewTactTimeUnit_SelectionChanged(object sender, EventArgs e)
        {
            SetUpdateTactTimeDetailData();
        }

        private void GridViewTactTimeOutput_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (null == mOutputTactTimeList)
            {
                return;
            }

            if (mOutputTactTimeList.Length <= e.RowIndex)
            {
                switch (e.ColumnIndex)
                {
                    case (int)EOutputTactTimeListColumn.Index:
                        e.Value = string.Format("{0,4}", e.RowIndex + 1);
                        break;
                    default:
                        e.Value = "";
                        break;
                }
                return;
            }
            else
            {
                switch (e.ColumnIndex)
                {
                    case (int)EOutputTactTimeListColumn.Index:
                        e.Value = string.Format("{0,4}", e.RowIndex + 1);
                        break;
                    case (int)EOutputTactTimeListColumn.UnitType:
                        {
                            e.Value = string.Format("{0}", ETactTime.INSP_STAGE);
                        }
                        break;
                    case (int)EOutputTactTimeListColumn.OutTact:
                        e.Value = string.Format("{0,8:F3} ( sec )", mOutputTactTimeList[e.RowIndex].OutTact.Value.TotalSeconds);
                        break;
                    case (int)EOutputTactTimeListColumn.WaitLoading:
                        e.Value = string.Format("{0,8:F3} ( sec )", mOutputTactTimeList[e.RowIndex].InputDelayTime.Value.TotalSeconds);
                        break;
                    case (int)EOutputTactTimeListColumn.WaitUnloading:
                        e.Value = string.Format("{0,8:F3} ( sec )", mOutputTactTimeList[e.RowIndex].OutputDelayTime.Value.TotalSeconds);
                        break;
                    case (int)EOutputTactTimeListColumn.Inspection:
                        e.Value = string.Format("{0,8:F3} ( sec )", mOutputTactTimeList[e.RowIndex].InspectionTact.Value.TotalSeconds);
                        break;
                }
            }
        }

        private void BtnOutTactSimpleView_Click(object sender, EventArgs e)
        {
            IsSimpleOutTactTimeView = !IsSimpleOutTactTimeView;
        }
    }
}