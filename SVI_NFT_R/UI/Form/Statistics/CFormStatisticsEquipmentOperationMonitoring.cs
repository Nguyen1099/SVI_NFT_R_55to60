using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormStatisticsEquipmentOperationMonitoring : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 현황 리스트 칼럼 정의
        /// </summary>
        private enum EStatusListColumn
        {
            ITEM = 0,
            VALUE,
            STATUS_LIST_COLUMN_FINAL
        }
        /// <summary>
        /// 현재 폼에서 사용하기 위한 클래스 데이터 정의
        /// </summary>
        private class CEquipmentOperationMonitoringData
        {
            /// <summary>
            /// 총 진행한 수량 (OUT)
            /// </summary>
            public int m_iOutput;
            /// <summary>
            /// BIN1 수량
            /// </summary>
            public int m_iBin1;
            /// <summary>
            /// BIN2 수량
            /// </summary>
            public int m_iBin2;
            /// <summary>
            /// BIN3 수량
            /// </summary>
            public int m_iBin3;
            /// <summary>
            /// Reject 수량
            /// </summary>
            public int m_iReject;
            /// <summary>
            /// 가동 시간
            /// </summary>
            public TimeSpan m_objOperationTime;
            /// <summary>
            /// 설비 다운 시간
            /// </summary>
            public TimeSpan m_objEquipmentDownTime;
            /// <summary>
            /// 표준 시간
            /// </summary>
            public TimeSpan m_objStandardTime;
            /// <summary>
            /// 실 생산 시간
            /// </summary>
            public TimeSpan m_objRealProductionTime;
            /// <summary>
            /// 표준 생산 시간
            /// </summary>
            public TimeSpan m_objStandardProductionTime;
            /// <summary>
            /// 시간 가동율
            /// </summary>
            public int m_iTimeOperationRate;
            /// <summary>성능 가동율
            /// </summary>
            public int m_iPerformanceUtilizationRate;
            /// <summary>
            /// [비가동] 횟수 (종합)
            /// </summary>
            public int m_iNonOperationCount;
            /// <summary>
            /// [비가동] 시간 (종합)
            /// </summary>
            public TimeSpan m_objNonOperationTime;
            /// <summary>
            /// [비가동] 알람 횟수
            /// </summary>
            public int m_iNonOperationAlarmCount;
            /// <summary>
            /// [비가동] 알람 시간
            /// </summary>
            public TimeSpan m_objNonOperationAlarmTime;
            /// <summary>
            /// [비가동] 설비 정지 횟수
            /// </summary>
            public int m_iNonOperationEquipmentStopCount;
            /// <summary>
            /// [비가동] 설비 정지 시간
            /// </summary>
            public TimeSpan m_objNonOperationEquipmentStopTime;
            /// <summary>
            /// [비가동] 앞 설비로 인해 손실된 횟수
            /// </summary>
            public int m_iNonOperationEquipmentLowerLossCount;
            /// <summary>
            /// [비가동] 앞 설비로 인해 손실된 시간
            /// </summary>
            public TimeSpan m_objNonOperationEquipmentLowerLossTime;
            /// <summary>
            /// [비가동] 뒷 설비로 인해 손실된 횟수
            /// </summary>
            public int m_iNonOperationEquipmentUpperLossCount;
            /// <summary>
            /// [비가동] 뒷 설비로 인해 손실된 시간
            /// </summary>
            public TimeSpan m_objNonOperationEquipmentUpperLossTime;
            /// <summary>
            /// [순간 정지] 횟수 (종합)
            /// </summary>
            public int m_iMomentaryStopCount;
            /// <summary>
            /// [순간 정지] 시간 (종합)
            /// </summary>
            public TimeSpan m_objMomentaryStopTime;
            /// <summary>
            /// [순간 정지] 알람 횟수
            /// </summary>
            public int m_iMomentaryStopAlarmCount;
            /// <summary>
            /// [순간 정지] 알람 시간
            /// </summary>
            public TimeSpan m_objMomentaryStopAlarmTime;
            /// <summary>
            /// [순간 정지] 설비 정지 횟수
            /// </summary>
            public int m_iMomentaryStopEquipmentStopCount;
            /// <summary>
            /// [순간 정지] 설비 정지 시간
            /// </summary>
            public TimeSpan m_objMomentaryStopEquipmentStopTime;
            /// <summary>
            /// [순간 정지] 앞 설비로 인해 손실된 횟수
            /// </summary>
            public int m_iMomentaryStopEquipmentLowerLossCount;
            /// <summary>
            /// [순간 정지] 앞 설비로 인해 손실된 시간
            /// </summary>
            public TimeSpan m_objMomentaryStopEquipmentLowerLossTime;
            /// <summary>
            /// [순간 정지] 뒷 설비로 인해 손실된 횟수
            /// </summary>
            public int m_iMomentaryStopEquipmentUpperLossCount;
            /// <summary>
            /// [순간 정지] 뒷 설비로 인해 손실된 시간
            /// </summary>
            public TimeSpan m_objMomentaryStopEquipmentUpperLossTime;
            /// <summary>
            /// MTBF (Mean Time Between Failure)
            /// </summary>
            public TimeSpan m_objMeanTimeBetweenFailure;
            /// <summary>
            /// MTTR (Mean Time To Refair)
            /// </summary>
            public TimeSpan m_objMeanTimeToRefair;
            /// <summary>
            /// MCR 진행 수량
            /// </summary>
            public int m_iTotalMCRCount;
            /// <summary>
            /// MCR 리딩 성공 수량
            /// </summary>
            public int m_iMCRReadingOKCount;
            /// <summary>
            /// MCR 리딩율
            /// </summary>
            public int m_iMCRReadingRate;

            public CEquipmentOperationMonitoringData()
            {
                m_iOutput = 0;
                m_iBin1 = 0;
                m_iBin2 = 0;
                m_iBin3 = 0;
                m_iReject = 0;
                m_objOperationTime = new TimeSpan(0, 0, 0, 0);
                m_objEquipmentDownTime = new TimeSpan(0, 0, 0, 0);
                m_objStandardTime = new TimeSpan(0, 0, 0, 0);
                m_objRealProductionTime = new TimeSpan(0, 0, 0, 0);
                m_objStandardProductionTime = new TimeSpan(0, 0, 0, 0);
                m_iTimeOperationRate = 0;
                m_iPerformanceUtilizationRate = 0;
                m_iNonOperationCount = 0;
                m_objNonOperationTime = new TimeSpan(0, 0, 0, 0);
                m_iNonOperationAlarmCount = 0;
                m_objNonOperationAlarmTime = new TimeSpan(0, 0, 0, 0);
                m_iNonOperationEquipmentStopCount = 0;
                m_objNonOperationEquipmentStopTime = new TimeSpan(0, 0, 0, 0);
                m_iNonOperationEquipmentLowerLossCount = 0;
                m_objNonOperationEquipmentLowerLossTime = new TimeSpan(0, 0, 0, 0);
                m_iNonOperationEquipmentUpperLossCount = 0;
                m_objNonOperationEquipmentUpperLossTime = new TimeSpan(0, 0, 0, 0);
                m_iMomentaryStopCount = 0;
                m_objMomentaryStopTime = new TimeSpan(0, 0, 0, 0);
                m_iMomentaryStopAlarmCount = 0;
                m_objMomentaryStopAlarmTime = new TimeSpan(0, 0, 0, 0);
                m_iMomentaryStopEquipmentStopCount = 0;
                m_objMomentaryStopEquipmentStopTime = new TimeSpan(0, 0, 0, 0);
                m_iMomentaryStopEquipmentLowerLossCount = 0;
                m_objMomentaryStopEquipmentLowerLossTime = new TimeSpan(0, 0, 0, 0);
                m_iMomentaryStopEquipmentUpperLossCount = 0;
                m_objMomentaryStopEquipmentUpperLossTime = new TimeSpan(0, 0, 0, 0);
                m_objMeanTimeBetweenFailure = new TimeSpan(0, 0, 0, 0);
                m_objMeanTimeToRefair = new TimeSpan(0, 0, 0, 0);
                m_iTotalMCRCount = 0;
                m_iMCRReadingOKCount = 0;
                m_iMCRReadingRate = 0;
            }
        }
        /// <summary>
        /// Set Clear 데이터
        /// </summary>
        public class CSetClearData
        {
            /// <summary>
            /// Set 시점
            /// </summary>
            public DateTime m_objSetTime;
            /// <summary>
            /// Clear 시점
            /// </summary>
            public DateTime m_objClearTime;

            public CSetClearData()
            {
                m_objSetTime = DateTime.Now;
                m_objClearTime = DateTime.Now;
            }

            public CSetClearData(DateTime objSet, DateTime objClear)
            {
                m_objSetTime = objSet;
                m_objClearTime = objClear;
            }
        }

        private CEquipmentOperationMonitoringData m_objMonitoringData;
        private List<TimeSpan> m_objAlarmDurationList;
        private List<TimeSpan> m_objEquipmentStopDurationList;
        private List<TimeSpan> m_objEquipmentUpperLossDurationList;
        private List<TimeSpan> m_objEquipmentLowerLossDurationList;
        private List<CSetClearData> m_objSetClearDataList;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormStatisticsEquipmentOperationMonitoring(CDocument objDocument)
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
        private void CFormStatisticsEquipmentOperationMonitoring_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormStatisticsEquipmentOperationMonitoring_FormClosed(object sender, FormClosedEventArgs e)
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
                // 데이터 클래스 초기화
                m_objMonitoringData = new CEquipmentOperationMonitoringData();
                // 현황 리스트
                string[] strStatusList = { EStatusListColumn.ITEM.ToString(), EStatusListColumn.VALUE.ToString() };
                // 생산 현황 리스트 그리드 뷰 초기화
                if (false == InitializeGridView(this.GridViewProductionStatus, strStatusList))
                {
                    break;
                }
                // 가동 현황 리스트 그리드 뷰 초기화
                if (false == InitializeGridView(this.GridViewOperationStatus, strStatusList))
                {
                    break;
                }
                // 비가동 현황 리스트 그리드 뷰 초기화
                if (false == InitializeGridView(this.GridViewNonOperationStatus, strStatusList))
                {
                    break;
                }
                // 순간 정지 현황 리스트 그리드 뷰 초기화
                if (false == InitializeGridView(this.GridViewMomentaryStopStatus, strStatusList))
                {
                    break;
                }
                // 장애 조치 현황 리스트 그리드 뷰 초기화
                if (false == InitializeGridView(this.GridViewTimeToRefair, strStatusList))
                {
                    break;
                }
                // MCR 현황 리스트 그리드 뷰 초기화
                if (false == InitializeGridView(this.GridViewMCRStatus, strStatusList))
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
                DrawGridView();
                // 버튼 색상 정의
                SetButtonColor();
                // 날짜 선택 컨트롤을 현재 날짜로 설정
                this.DateTimeFrom.Value = DateTime.Today;
                this.DateTimeTo.Value = DateTime.Today;
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
            base.SetButtonBackColor(this.BtnSelect, m_colorNormal);

            base.SetButtonBackColor(this.BtnTitleEquipmentOperationMonitoring, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleProductionStatus, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleOperationStatus, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNonOperationStatus, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleMomentaryStop, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleTimeToRefair, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleMCRStatus, m_colorLabelSub);

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
                SetButtonChangeLanguage(this.BtnTitleEquipmentOperationMonitoring);
                SetButtonChangeLanguage(this.BtnSelect);
                SetButtonChangeLanguage(this.BtnTitleProductionStatus);
                SetButtonChangeLanguage(this.BtnTitleOperationStatus);
                SetButtonChangeLanguage(this.BtnTitleNonOperationStatus);
                SetButtonChangeLanguage(this.BtnTitleMomentaryStop);
                SetButtonChangeLanguage(this.BtnTitleTimeToRefair);
                SetButtonChangeLanguage(this.BtnTitleMCRStatus);

                if (false == backgroundWorkerUpdate.IsBusy)
                {
                    backgroundWorkerUpdate.RunWorkerAsync();
                }
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

                if (false == backgroundWorkerUpdate.IsBusy)
                {
                    backgroundWorkerUpdate.RunWorkerAsync();
                }

                bReturn = true;
            } while (false);

            return bReturn;
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
                    break;
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
                    objGridView.Columns.Add(string.Format("{0}", iLoopColumn), strColumnName[iLoopColumn]);
                    // 칼럼 정렬 기능 x
                    objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                    objGridView.Columns[iLoopColumn].Width = (int)(objGridView.Width * 0.12);
                    objGridView.Columns[iLoopColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                // VALUE 사이즈 조정
                objGridView.Columns[(int)EStatusListColumn.VALUE].Width = (int)(objGridView.Width * 0.4);
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
        /// 데이터베이스 연동 (생산량 현황 리스트에 뿌려줌)
        /// </summary>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        private void SetProductionHistoryConnect(DateTime objFrom, DateTime objTo)
        {
            string strQuery = null;
            DataTable objDataTable = new DataTable();
            DataRow[] objDataRow = null;
            CManagerTable objManagerTableCellOutput = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellOutput;
            CManagerTable objManagerTableProcessCell = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataCell;

            try
            {
                #region 총 진행한 수량 (OUT)
                // From ~ To 날짜에 해당하는 Cell Output 레코드를 뽑아냄
                strQuery = string.Format(
                    "SELECT count(*) FROM {0} WHERE {1} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{2}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{3}');",
                    objManagerTableCellOutput.HLGetTableName(),
                    objManagerTableCellOutput.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryCellOutput.DATE],
                    string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                    string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT))
                    );
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(strQuery, ref objDataTable);
                // 전체 항목을 루프 돌면서 개수를 누적시킴
                objDataRow = objDataTable.Select();
                m_objMonitoringData.m_iOutput = Convert.ToInt32(objDataRow[0][0]);
                #endregion

                #region SDV 비전 검사 수량 (BIN1, BIN2, BIN3, REJECT)
                // From ~ To 날짜에 해당하는 Process Cell 레코드를 뽑아냄
                strQuery = string.Format(
                    "SELECT {4}, count(*) FROM {0} WHERE {1} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{2}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{3}') GROUP BY {4};",
                    objManagerTableProcessCell.HLGetTableName(),
                    objManagerTableProcessCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.DATE],
                    string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                    string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                    objManagerTableProcessCell.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataCell.FRONT_INSP_RESULT]
                    );
                objDataTable = new DataTable();
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(strQuery, ref objDataTable);
                // Vistion Result 데이터를 판단해서 카운터 누적
                objDataRow = objDataTable.Select();
                m_objMonitoringData.m_iBin1 = 0;
                m_objMonitoringData.m_iBin2 = 0;
                m_objMonitoringData.m_iBin3 = 0;
                m_objMonitoringData.m_iReject = 0;
                foreach (var row in objDataRow)
                {
                    string inspectionResult = Convert.ToString(row[0]);
                    if ("BIN1" == inspectionResult)
                    {
                        m_objMonitoringData.m_iBin1 += Convert.ToInt32(row[1]);
                    }
                    else if ("BIN2" == inspectionResult)
                    {
                        m_objMonitoringData.m_iBin2 += Convert.ToInt32(row[1]);
                    }
                    else if ("BIN3" == inspectionResult)
                    {
                        m_objMonitoringData.m_iBin3 += Convert.ToInt32(row[1]);
                    }
                    else if ("REJECT" == inspectionResult)
                    {
                        m_objMonitoringData.m_iReject += Convert.ToInt32(row[1]);
                    }
                    else if ("NG" == inspectionResult)
                    {
                        m_objMonitoringData.m_iReject += Convert.ToInt32(row[1]);
                    }
                    else
                    {
                        m_objMonitoringData.m_iBin2 += Convert.ToInt32(row[1]);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 데이터베이스 연동 (가동 현황 리스트에 뿌려줌)
        /// </summary>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        private void SetOperationHistoryConnect(DateTime objFrom, DateTime objTo)
        {
            string strQuery = null;
            DataTable objDataTable = new DataTable();
            DataRow[] objDataRow = null;
            CManagerTable objManagerTableCellTrackIn = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellTrackIn;
            CManagerTable objManagerTableCellTrackOut = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellTrackOut;

            try
            {
                #region 가동 시간이 현재 시간에 따라 변경됨
                if (objTo.DayOfYear == DateTime.Now.DayOfYear)
                {
                    m_objMonitoringData.m_objOperationTime = DateTime.Now - objFrom;
                }
                else
                {
                    m_objMonitoringData.m_objOperationTime = objTo - new DateTime(objFrom.Ticks).AddDays(-1);
                }
                string strOperationTime = GetTimeToHourMinuteString(m_objMonitoringData.m_objOperationTime);
                #endregion

                #region 설비 다운 시간 (비가동 및 순간 정지된 시간의 누적)
                // 알람 이벤트에 SET - CLEAR 사이 시간을 누적
                TimeSpan objAlarmDuration = new TimeSpan(0, 0, 0, 0);
                for (int iLoopList = 0; iLoopList < m_objAlarmDurationList.Count; iLoopList++)
                {
                    objAlarmDuration += m_objAlarmDurationList[iLoopList];
                }
                // 정지 이벤트에 SET - CLEAR 사이 시간을 누적
                TimeSpan objEquipmentStopDuration = new TimeSpan(0, 0, 0, 0);
                for (int iLoopList = 0; iLoopList < m_objEquipmentStopDurationList.Count; iLoopList++)
                {
                    objEquipmentStopDuration += m_objEquipmentStopDurationList[iLoopList];
                }
                // 앞 설비 지연 시간 이벤트에 SET - CLEAR 사이 시간을 누적
                TimeSpan objEquipmentUpperLossDuration = new TimeSpan(0, 0, 0, 0);
                for (int iLoopList = 0; iLoopList < m_objEquipmentUpperLossDurationList.Count; iLoopList++)
                {
                    objEquipmentUpperLossDuration += m_objEquipmentUpperLossDurationList[iLoopList];
                }
                // 뒷 설비 지연 시간 이벤트에 SET - CLEAR 사이 시간을 누적
                TimeSpan objEquipmentLowerLossDuration = new TimeSpan(0, 0, 0, 0);
                for (int iLoopList = 0; iLoopList < m_objEquipmentLowerLossDurationList.Count; iLoopList++)
                {
                    objEquipmentLowerLossDuration += m_objEquipmentLowerLossDurationList[iLoopList];
                }
                // 설비 다운 시간 구함
                m_objMonitoringData.m_objEquipmentDownTime = objAlarmDuration + objEquipmentStopDuration + objEquipmentLowerLossDuration + objEquipmentUpperLossDuration;
                #endregion

                #region 표준 시간 : ST (제품 투입부터 배출까지의 시간)
                // 인스펙션 스테이지의 유닛 택타임 (아마 인스펙션 스테이지가 시간이 가장 길다...)
                // Cell Input & Output 테이블에 Inner ID 로 묶음
                // Track In - Out 사이 시간을 구함
                // Track In Out 테이블을 Join 함
                strQuery = string.Format(
                   //"SELECT {0} FROM {1} WHERE {2} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{3}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{4}');",
                   //objManagerTableDataTactTime.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataTactTime.TACT_TIME_DATA_START + (int)CDefine.ETactTimeItems.INSPECTION_STAGE_UNIT_TACT_TIME],
                   //objManagerTableDataTactTime.HLGetTableName(),
                   //objManagerTableDataTactTime.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryProcessDataTactTime.DATE],
                   //string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                   //string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT))
                   "SELECT t1.{2}, t1.{3}, t2.{3} FROM {0} AS t1 INNER JOIN {1} AS t2 ON t1.{2} = t2.{2} WHERE t1.{3} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{4}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{5}');",
                   objManagerTableCellTrackIn.HLGetTableName(),
                   objManagerTableCellTrackOut.HLGetTableName(),
                   objManagerTableCellTrackIn.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryCellTrackIn.INNER_ID],
                   objManagerTableCellTrackIn.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryCellTrackIn.DATE],
                   string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                   string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT))
                   );
                objDataTable = new DataTable();
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(strQuery, ref objDataTable);
                // 전체 항목을 루프 돌면서 간격을 뺌
                objDataRow = objDataTable.Select();
                m_objMonitoringData.m_objStandardTime = new TimeSpan(0, 0, 0, 0);
                for (int iLoopRow = 0; iLoopRow < objDataRow.Length; iLoopRow++)
                {
                    DateTime objDateTrackIn = DateTime.ParseExact(objDataRow[iLoopRow].ItemArray[1].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                    DateTime objDateTrackOut = DateTime.ParseExact(objDataRow[iLoopRow].ItemArray[2].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                    m_objMonitoringData.m_objStandardTime += objDateTrackOut - objDateTrackIn;
                    //if (false == string.IsNullOrWhiteSpace(objDataRow[iLoopRow].ItemArray[0].ToString()))
                    //{
                    //    m_objMonitoringData.m_objStandardTime += TimeSpan.ParseExact(objDataRow[iLoopRow].ItemArray[0].ToString(), @"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture);
                    //}
                }
                // 제품 배출 - 투입 레코드에 평균 시간을 구함
                long lTick = m_objMonitoringData.m_objStandardTime.Ticks;
                if (0 != lTick)
                {
                    lTick /= objDataRow.Length;
                }
                // 표준 시간 구함
                m_objMonitoringData.m_objStandardTime = new TimeSpan(lTick);
                #endregion

                #region 실 생산 시간 (가동시간 - 설비 다운 시간)
                // 실 생산 시간 구함
                m_objMonitoringData.m_objRealProductionTime = m_objMonitoringData.m_objOperationTime - m_objMonitoringData.m_objEquipmentDownTime;
                #endregion

                #region 표준 생산 시간 (생산량 * 표준 시간 : ST)
                lTick = m_objMonitoringData.m_objStandardTime.Ticks;
                lTick *= m_objMonitoringData.m_iOutput;
                // 표준 생산 시간 구함
                m_objMonitoringData.m_objStandardProductionTime = new TimeSpan(lTick);
                #endregion

                #region 시간 가동율 (실 생산 시간 / 가동 시간 * 100)
                double dTimeOperationRate = 0.0;
                dTimeOperationRate = (double)m_objMonitoringData.m_objRealProductionTime.Ticks;
                if (0.0 != dTimeOperationRate)
                {
                    dTimeOperationRate /= (double)m_objMonitoringData.m_objOperationTime.Ticks;
                    dTimeOperationRate *= 100.0;
                }
                m_objMonitoringData.m_iTimeOperationRate = (int)dTimeOperationRate;
                #endregion

                #region 성능 가동율 (표준 생산 시간 / 가동 시간 * 100)
                double dPerformanceUtilizationRate = 0.0;
                dPerformanceUtilizationRate = (double)m_objMonitoringData.m_objStandardProductionTime.Ticks;
                if (0.0 != dPerformanceUtilizationRate)
                {
                    dPerformanceUtilizationRate /= (double)m_objMonitoringData.m_objOperationTime.Ticks;
                    dPerformanceUtilizationRate *= 100;
                }
                m_objMonitoringData.m_iPerformanceUtilizationRate = (int)dPerformanceUtilizationRate;
                #endregion
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// TimeSpan 받아서 분, 초 문자열로 변환
        /// </summary>
        /// <param name="objTime"></param>
        /// <returns></returns>
        private string GetTimeToMinuteSecondString(TimeSpan objTime)
        {
            int iTotalSecond = (int)objTime.TotalSeconds;
            int iMinute = iTotalSecond / 60;
            int iSecond = iTotalSecond % 60;
            return string.Format("{0} m {1} s", iMinute, iSecond);
        }

        /// <summary>
        /// TimeSpan 받아서 시, 분 문자열로 변환
        /// </summary>
        /// <param name="objTime"></param>
        /// <returns></returns>
        private string GetTimeToHourMinuteString(TimeSpan objTime)
        {
            int iTotalSecond = (int)objTime.TotalSeconds;
            int iHour = iTotalSecond / 3600;
            int iMinute = (iTotalSecond % 3600) / 60;
            return string.Format("{0} h {1} m", iHour, iMinute);
        }

        /// <summary>
        /// SET - CLEAR 이벤트 사이 시간 리스트로 리턴시켜줌
        /// </summary>
        /// <param name="objManagerTable"></param>
        /// <param name="iDateIndex"></param>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        /// <returns></returns>
        private List<TimeSpan> GetEventDuration(CManagerTable objManagerTable, int iDateIndex, DateTime objFrom, DateTime objTo)
        {
            List<TimeSpan> objDurationList = new List<TimeSpan>();

            try
            {
                string strQuery = string.Format(
                    "SELECT * FROM {0} WHERE {1} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{2}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{3}');",
                    objManagerTable.HLGetTableName(),
                    objManagerTable.HLGetTableSchemaName()[iDateIndex],
                    string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                    string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT))
                    );
                DataTable objDataTable = new DataTable();
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(strQuery, ref objDataTable);
                // 전체 항목을 루프 돌면서 개수를 누적시킴
                DataRow[] objDataRow = objDataTable.Select();
                // 레코드가 없는 경우 CLEAR 상태임
                if (0 == objDataRow.Length)
                {
                }
                // 이벤트는 SET - CLEAR 관계로 온다
                // 레코드가 짝수인 경우
                else if (0 == objDataRow.Length % 2)
                {
                    for (int iLoopRow = 0; iLoopRow < objDataRow.Length; iLoopRow += 2)
                    {
                        DateTime objDateSet = DateTime.ParseExact(objDataRow[iLoopRow].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        DateTime objDateClear = DateTime.ParseExact(objDataRow[iLoopRow + 1].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        objDurationList.Add(objDateClear - objDateSet);
                    }
                }
                // 홀수인 경우 하나 남는 SET은 현재 시간이랑 뺌 (Today)
                else
                {
                    for (int iLoopRow = 0; iLoopRow < objDataRow.Length - 1; iLoopRow += 2)
                    {
                        DateTime objDateSet = DateTime.ParseExact(objDataRow[iLoopRow].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        DateTime objDateClear = DateTime.ParseExact(objDataRow[iLoopRow + 1].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        objDurationList.Add(objDateClear - objDateSet);
                    }
                    DateTime objSoloSet = DateTime.ParseExact(objDataRow[objDataRow.Length - 1].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                    objDurationList.Add(DateTime.Now - objSoloSet);
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }

            return objDurationList;
        }

        /// <summary>
        /// SET - CLEAR 이벤트 리스트 리턴
        /// </summary>
        /// <param name="objManagerTable"></param>
        /// <param name="iDateIndex"></param>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        /// <returns></returns>
        private List<CSetClearData> GetEventSetClearTime(CManagerTable objManagerTable, int iDateIndex, DateTime objFrom, DateTime objTo)
        {
            List<CSetClearData> objSetClearDataList = new List<CSetClearData>();

            try
            {
                string strQuery = string.Format(
                    "SELECT * FROM {0} WHERE {1} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{2}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{3}');",
                    objManagerTable.HLGetTableName(),
                    objManagerTable.HLGetTableSchemaName()[iDateIndex],
                    string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                    string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT))
                    );
                DataTable objDataTable = new DataTable();
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(strQuery, ref objDataTable);
                // 전체 항목을 루프 돌면서 개수를 누적시킴
                DataRow[] objDataRow = objDataTable.Select();
                // 레코드가 없는 경우 CLEAR 상태임
                if (0 == objDataRow.Length)
                {
                }
                // 이벤트는 SET - CLEAR 관계로 온다
                // 레코드가 짝수인 경우
                else if (0 == objDataRow.Length % 2)
                {
                    for (int iLoopRow = 0; iLoopRow < objDataRow.Length; iLoopRow += 2)
                    {
                        DateTime objDateSet = DateTime.ParseExact(objDataRow[iLoopRow].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        DateTime objDateClear = DateTime.ParseExact(objDataRow[iLoopRow + 1].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        objSetClearDataList.Add(new CSetClearData(objDateSet, objDateClear));
                    }
                }
                // 홀수인 경우 하나 남는 SET은 현재 시간이랑 뺌
                else
                {
                    for (int iLoopRow = 0; iLoopRow < objDataRow.Length - 1; iLoopRow += 2)
                    {
                        DateTime objDateSet = DateTime.ParseExact(objDataRow[iLoopRow].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        DateTime objDateClear = DateTime.ParseExact(objDataRow[iLoopRow + 1].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        objSetClearDataList.Add(new CSetClearData(objDateSet, objDateClear));
                    }
                    DateTime objSoloSet = DateTime.ParseExact(objDataRow[objDataRow.Length - 1].ItemArray[iDateIndex].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                    objSetClearDataList.Add(new CSetClearData(objSoloSet, DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }

            return objSetClearDataList;
        }

        /// <summary>
        /// 데이터베이스 연동 (비가동 현황 리스트에 뿌려줌)
        /// </summary>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        private void SetNonOperationHistoryConnect(DateTime objFrom, DateTime objTo)
        {
            // 5분 이상 다운 시 비가동으로 구분
            TimeSpan objBaseTime = new TimeSpan(0, 5, 0);
            try
            {
                #region 알람 횟수 & 시간
                // 알람 이벤트에 SET - CLEAR 사이 시간 중에 5분 이상인 부분을 비가동으로 구분
                m_objMonitoringData.m_iNonOperationAlarmCount = 0;
                m_objMonitoringData.m_objNonOperationAlarmTime = new TimeSpan(0, 0, 0, 0);
                for (int iLoopList = 0; iLoopList < m_objAlarmDurationList.Count; iLoopList++)
                {
                    if (objBaseTime <= m_objAlarmDurationList[iLoopList])
                    {
                        // 5분 이상인 알람 이벤트 시간에 대해 횟수 증가
                        m_objMonitoringData.m_iNonOperationAlarmCount++;
                        // 5분 이상인 알람 이벤트 시간 누적
                        m_objMonitoringData.m_objNonOperationAlarmTime += m_objAlarmDurationList[iLoopList];
                    }
                }
                #endregion

                #region 설비 정지 횟수 & 시간
                // 설비 정지 이벤트에 SET - CLEAR 사이 시간 중에 5분 이상인 부분을 비가동으로 구분
                m_objMonitoringData.m_iNonOperationEquipmentStopCount = 0;
                m_objMonitoringData.m_objNonOperationEquipmentStopTime = new TimeSpan(0, 0, 0, 0);
                for (int iLoopList = 0; iLoopList < m_objEquipmentStopDurationList.Count; iLoopList++)
                {
                    if (objBaseTime <= m_objEquipmentStopDurationList[iLoopList])
                    {
                        // 5분 이상인 설비 정지 이벤트 시간에 대해 횟수 증가
                        m_objMonitoringData.m_iNonOperationEquipmentStopCount++;
                        // 5분 이상인 설비 정지 이벤트 시간 누적
                        m_objMonitoringData.m_objNonOperationEquipmentStopTime += m_objEquipmentStopDurationList[iLoopList];
                    }
                }
                #endregion

                #region 앞 설비로 인해 손실된 횟수 & 시간
                // 앞 설비 손실 시간 이벤트에 SET - CLEAR 사이 시간 중에 5분 이상인 부분을 비가동으로 구분
                m_objMonitoringData.m_iNonOperationEquipmentUpperLossCount = 0;
                m_objMonitoringData.m_objNonOperationEquipmentUpperLossTime = new TimeSpan(0, 0, 0, 0);
                for (int iLoopList = 0; iLoopList < m_objEquipmentUpperLossDurationList.Count; iLoopList++)
                {
                    if (objBaseTime <= m_objEquipmentUpperLossDurationList[iLoopList])
                    {
                        // 5분 이상인 앞 설비 손실 시간 이벤트 시간에 대해 횟수 증가
                        m_objMonitoringData.m_iNonOperationEquipmentUpperLossCount++;
                        // 5분 이상인 앞 설비 손실 시간 이벤트 시간 누적
                        m_objMonitoringData.m_objNonOperationEquipmentUpperLossTime += m_objEquipmentUpperLossDurationList[iLoopList];
                    }
                }
                #endregion

                #region 뒷 설비로 인해 손실된 횟수 & 시간
                // 뒷 설비 손실 시간 이벤트에 SET - CLEAR 사이 시간 중에 5분 이상인 부분을 비가동으로 구분
                m_objMonitoringData.m_iNonOperationEquipmentLowerLossCount = 0;
                m_objMonitoringData.m_objNonOperationEquipmentLowerLossTime = new TimeSpan(0, 0, 0, 0);
                for (int iLoopList = 0; iLoopList < m_objEquipmentLowerLossDurationList.Count; iLoopList++)
                {
                    if (objBaseTime <= m_objEquipmentLowerLossDurationList[iLoopList])
                    {
                        // 5분 이상인 뒷 설비 손실 시간 이벤트 시간에 대해 횟수 증가
                        m_objMonitoringData.m_iNonOperationEquipmentLowerLossCount++;
                        // 5분 이상인 뒷 설비 손실 시간 이벤트 시간 누적
                        m_objMonitoringData.m_objNonOperationEquipmentLowerLossTime += m_objEquipmentLowerLossDurationList[iLoopList];
                    }
                }
                #endregion

                #region 비가동 횟수 (모든 비가동 항목을 더해줌)
                m_objMonitoringData.m_iNonOperationCount =
                    m_objMonitoringData.m_iNonOperationAlarmCount +
                    m_objMonitoringData.m_iNonOperationEquipmentStopCount +
                    m_objMonitoringData.m_iNonOperationEquipmentLowerLossCount +
                    m_objMonitoringData.m_iNonOperationEquipmentUpperLossCount;
                #endregion

                #region 비가동 시간
                m_objMonitoringData.m_objNonOperationTime =
                    m_objMonitoringData.m_objNonOperationAlarmTime +
                    m_objMonitoringData.m_objNonOperationEquipmentStopTime +
                    m_objMonitoringData.m_objNonOperationEquipmentLowerLossTime +
                    m_objMonitoringData.m_objNonOperationEquipmentUpperLossTime;
                #endregion
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 데이터베이스 연동 (순간 정지 현황 리스트에 뿌려줌)
        /// </summary>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        private void SetMomentaryStopHistoryConnect(DateTime objFrom, DateTime objTo)
        {
            // 5분 미만 다운 시 순간 정지로 구분
            TimeSpan objBaseTime = new TimeSpan(0, 5, 0);
            try
            {
                #region 알람 횟수 & 시간
                // 알람 이벤트에 SET - CLEAR 사이 시간 중에 5분 미만인 부분을 순간 정지로 구분
                m_objMonitoringData.m_iMomentaryStopAlarmCount = 0;
                m_objMonitoringData.m_objMomentaryStopAlarmTime = new TimeSpan(0, 0, 0, 0);
                for (int iLoopList = 0; iLoopList < m_objAlarmDurationList.Count; iLoopList++)
                {
                    if (objBaseTime > m_objAlarmDurationList[iLoopList])
                    {
                        // 5분 미만인 알람 이벤트 시간에 대해 횟수 증가
                        m_objMonitoringData.m_iMomentaryStopAlarmCount++;
                        // 5분 미만인 알람 이벤트 시간 누적
                        m_objMonitoringData.m_objMomentaryStopAlarmTime += m_objAlarmDurationList[iLoopList];
                    }
                }
                #endregion

                #region 설비 정지 횟수 & 시간
                // 설비 정지 이벤트에 SET - CLEAR 사이 시간 중에 5분 미만인 부분을 순간 정지로 구분
                m_objMonitoringData.m_iMomentaryStopEquipmentStopCount = 0;
                m_objMonitoringData.m_objMomentaryStopEquipmentStopTime = new TimeSpan(0, 0, 0, 0);
                for (int iLoopList = 0; iLoopList < m_objEquipmentStopDurationList.Count; iLoopList++)
                {
                    if (objBaseTime > m_objEquipmentStopDurationList[iLoopList])
                    {
                        // 5분 미만인 설비 정지 이벤트 시간에 대해 횟수 증가
                        m_objMonitoringData.m_iMomentaryStopEquipmentStopCount++;
                        // 5분 미만인 설비 정지 이벤트 시간 누적
                        m_objMonitoringData.m_objMomentaryStopEquipmentStopTime += m_objEquipmentStopDurationList[iLoopList];
                    }
                }
                #endregion

                #region 앞 설비로 인해 손실된 횟수 & 시간
                // 앞 설비 손실 시간 이벤트에 SET - CLEAR 사이 시간 중에 5분 미만인 부분을 순간 정지로 구분
                m_objMonitoringData.m_iMomentaryStopEquipmentUpperLossCount = 0;
                m_objMonitoringData.m_objMomentaryStopEquipmentUpperLossTime = new TimeSpan(0, 0, 0, 0);
                for (int iLoopList = 0; iLoopList < m_objEquipmentUpperLossDurationList.Count; iLoopList++)
                {
                    if (objBaseTime > m_objEquipmentUpperLossDurationList[iLoopList])
                    {
                        // 5분 미만인 앞 설비 손실 시간 이벤트 시간에 대해 횟수 증가
                        m_objMonitoringData.m_iMomentaryStopEquipmentUpperLossCount++;
                        // 5분 미만인 앞 설비 손실 시간 이벤트 시간 누적
                        m_objMonitoringData.m_objMomentaryStopEquipmentUpperLossTime += m_objEquipmentUpperLossDurationList[iLoopList];
                    }
                }
                #endregion

                #region 뒷 설비로 인해 손실된 횟수 & 시간
                // 뒷 설비 손실 시간 이벤트에 SET - CLEAR 사이 시간 중에 5분 미만인 부분을 순간 정지로 구분
                m_objMonitoringData.m_iMomentaryStopEquipmentLowerLossCount = 0;
                m_objMonitoringData.m_objMomentaryStopEquipmentLowerLossTime = new TimeSpan(0, 0, 0, 0);
                for (int iLoopList = 0; iLoopList < m_objEquipmentLowerLossDurationList.Count; iLoopList++)
                {
                    if (objBaseTime > m_objEquipmentLowerLossDurationList[iLoopList])
                    {
                        // 5분 미만인 뒷 설비 손실 시간 이벤트 시간에 대해 횟수 증가
                        m_objMonitoringData.m_iMomentaryStopEquipmentLowerLossCount++;
                        // 5분 미만인 뒷 설비 손실 시간 이벤트 시간 누적
                        m_objMonitoringData.m_objMomentaryStopEquipmentLowerLossTime += m_objEquipmentLowerLossDurationList[iLoopList];
                    }
                }
                #endregion

                #region 순간 정지 횟수 (모든 비가동 항목을 더해줌)
                m_objMonitoringData.m_iMomentaryStopCount =
                    m_objMonitoringData.m_iMomentaryStopAlarmCount +
                    m_objMonitoringData.m_iMomentaryStopEquipmentStopCount +
                    m_objMonitoringData.m_iMomentaryStopEquipmentLowerLossCount +
                    m_objMonitoringData.m_iMomentaryStopEquipmentUpperLossCount;
                #endregion

                #region 순간 정지 시간
                m_objMonitoringData.m_objMomentaryStopTime =
                    m_objMonitoringData.m_objMomentaryStopAlarmTime +
                    m_objMonitoringData.m_objMomentaryStopEquipmentStopTime +
                    m_objMonitoringData.m_objMomentaryStopEquipmentLowerLossTime +
                    m_objMonitoringData.m_objMomentaryStopEquipmentUpperLossTime;
                #endregion
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 데이터베이스 연동 (장애 조치 현황 리스트에 뿌려줌)
        /// </summary>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        private void SetTimeToRepairHistoryConnect(DateTime objFrom, DateTime objTo)
        {
            CManagerTable objManagerTableAlarmEvent = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent;

            try
            {

                #region MTBF : Mean Time To Refair (평균 장애발생 간격시간 : 장애발생 후 다음 장애 발생이 일어날 때까지의 시간)
                m_objMonitoringData.m_objMeanTimeBetweenFailure = new TimeSpan(0, 0, 0, 0);
                for (int iLoopSetClearData = 0; iLoopSetClearData < m_objSetClearDataList.Count - 1; iLoopSetClearData++)
                {
                    // 현재 Set - 다음 Set 이벤트 시간을 뺀 시간 누적
                    m_objMonitoringData.m_objMeanTimeBetweenFailure += m_objSetClearDataList[iLoopSetClearData + 1].m_objSetTime - m_objSetClearDataList[iLoopSetClearData].m_objSetTime;
                }
                // 누적된 시간에 평균을 구함
                long lTick = m_objMonitoringData.m_objMeanTimeBetweenFailure.Ticks;
                if (0 != lTick)
                {
                    lTick /= m_objSetClearDataList.Count - 1;
                }
                // MTBF 구함
                m_objMonitoringData.m_objMeanTimeBetweenFailure = new TimeSpan(lTick);
                #endregion

                #region MTTR : Mean Time Between Failure (평균 장애 발생 후 장애조치까지 걸린 시간)
                m_objMonitoringData.m_objMeanTimeToRefair = new TimeSpan(0, 0, 0, 0);
                for (int iLoopSetClearData = 0; iLoopSetClearData < m_objSetClearDataList.Count; iLoopSetClearData++)
                {
                    // Clear - Set 이벤트 시간을 뺀 시간 누적
                    m_objMonitoringData.m_objMeanTimeToRefair += m_objSetClearDataList[iLoopSetClearData].m_objClearTime - m_objSetClearDataList[iLoopSetClearData].m_objSetTime;
                }
                // 누적된 시간에 평균을 구함
                lTick = m_objMonitoringData.m_objMeanTimeToRefair.Ticks;
                if (0 != lTick)
                {
                    lTick /= m_objSetClearDataList.Count;
                }
                // MTTR 구함
                m_objMonitoringData.m_objMeanTimeToRefair = new TimeSpan(lTick);
                #endregion
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 데이터베이스 연동 (MCR 현황 리스트에 뿌려줌)
        /// </summary>
        /// <param name="objFrom"></param>
        /// <param name="objTo"></param>
        private void SetMCRHistoryConnect(DateTime objFrom, DateTime objTo)
        {
            CManagerTable objManagerTableMCRStatus = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryMCRStatus;

            try
            {
                #region MCR 총 진행 수량
                string strQuery = string.Format(
                    "SELECT {4}, count(*) FROM {0} WHERE {1} BETWEEN strftime('%Y-%m-%d %H:%M:%S.%f','{2}') AND strftime('%Y-%m-%d %H:%M:%S.%f','{3}') GROUP BY {4};",
                    objManagerTableMCRStatus.HLGetTableName(),
                    objManagerTableMCRStatus.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryMCRStatus.DATE],
                    string.Format("{0} 00:00:00.000", objFrom.ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                    string.Format("{0} 00:00:00.000", objTo.AddDays(1).ToString(CDatabaseDefine.DEF_DATE_FORMAT)),
                    objManagerTableMCRStatus.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryMCRStatus.RESULT]
                    );
                DataTable objDataTable = new DataTable();
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(strQuery, ref objDataTable);
                // 전체 항목을 루프 돌면서 간격을 뺌
                DataRow[] objDataRow = objDataTable.Select();
                // 총 진행 수량 구함
                m_objMonitoringData.m_iTotalMCRCount = 0;
                #endregion

                #region 리딩 수량 (MCR 판독 성공 수량)
                m_objMonitoringData.m_iMCRReadingOKCount = 0;
                foreach (var row in objDataRow)
                {
                    string mcrResult = Convert.ToString(row[0]);
                    if ("OK" == mcrResult)
                    {
                        m_objMonitoringData.m_iMCRReadingOKCount += Convert.ToInt32(row[1]);
                    }
                    m_objMonitoringData.m_iTotalMCRCount += Convert.ToInt32(row[1]);
                }
                #endregion

                #region 리딩율 (리딩 수량 / 총 진행 수량 * 100)
                double dMCRReadingRate = (double)m_objMonitoringData.m_iMCRReadingOKCount;
                if (0 != m_objMonitoringData.m_iTotalMCRCount)
                {
                    dMCRReadingRate /= (double)m_objMonitoringData.m_iTotalMCRCount;
                    dMCRReadingRate *= 100.0;
                }
                m_objMonitoringData.m_iMCRReadingRate = (int)dMCRReadingRate;
                #endregion
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 그리드에 뿌릴 데이터를 연산한다
        /// </summary>
        private void UpdateGridViewData()
        {
            DateTimePicker objFrom = this.DateTimeFrom;
            DateTimePicker objTo = this.DateTimeTo;

            var tasks = new List<System.Threading.Tasks.Task>();
            tasks.Add(System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                SetProductionHistoryConnect(objFrom.Value, objTo.Value);
            }));
            tasks.Add(System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                SetOperationHistoryConnect(objFrom.Value, objTo.Value);
            }));
            tasks.Add(System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                SetNonOperationHistoryConnect(objFrom.Value, objTo.Value);
            }));
            tasks.Add(System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                SetMomentaryStopHistoryConnect(objFrom.Value, objTo.Value);
            }));
            tasks.Add(System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                SetTimeToRepairHistoryConnect(objFrom.Value, objTo.Value);
            }));
            tasks.Add(System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                SetMCRHistoryConnect(objFrom.Value, objTo.Value);
            }));

            while (true == tasks.Exists(item => false == item.IsCompleted))
            {
                System.Threading.Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 데이터 베이스에서 데이터를 읽어서 메모리에 셋업한다.
        /// </summary>
        private void SetupDatabaseData()
        {
            CManagerTable objManagerTableAlarmEvent = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent;
            CManagerTable objManagerTableEquipmentStopEvent = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentStopEvent;
            CManagerTable objManagerTableEquipmentUpperLossTimeEvent = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentUpperLossTimeEvent;
            CManagerTable objManagerTableEquipmentLowerLossTimeEvent = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentLowerLossTimeEvent;
            DateTime objFrom = this.DateTimeFrom.Value;
            DateTime objTo = this.DateTimeTo.Value;

            m_objAlarmDurationList = GetEventDuration(objManagerTableAlarmEvent, (int)CDatabaseDefine.EHistoryAlarmEvent.DATE, objFrom, objTo);
            m_objEquipmentStopDurationList = GetEventDuration(objManagerTableEquipmentStopEvent, (int)CDatabaseDefine.EHistoryEquipmentStopEvent.DATE, objFrom, objTo);
            m_objEquipmentUpperLossDurationList = GetEventDuration(objManagerTableEquipmentUpperLossTimeEvent, (int)CDatabaseDefine.EHistoryEquipmentUpperLossTimeEvent.DATE, objFrom, objTo);
            m_objEquipmentLowerLossDurationList = GetEventDuration(objManagerTableEquipmentLowerLossTimeEvent, (int)CDatabaseDefine.EHistoryEquipmentLowerLossTimeEvent.DATE, objFrom, objTo);
            m_objSetClearDataList = GetEventSetClearTime(objManagerTableAlarmEvent, (int)CDatabaseDefine.EHistoryAlarmEvent.DATE, objFrom, objTo);
        }

        /// <summary>
        /// 그리드에 데이터를 뿌린다
        /// </summary>
        private void DrawGridView()
        {
            #region Draw Grid - ProductionHistory
            {
                DataGridView objGridView = this.GridViewProductionStatus;
                // UI Text Key 지정
                string[] strProductionStatusKey = new string[(int)CDatabaseDefine.EProductionStatusList.PRODUCTION_STATUS_LIST_FINAL];
                string[] strProductionStatusToolTipKey = new string[(int)CDatabaseDefine.EProductionStatusList.PRODUCTION_STATUS_LIST_FINAL];
                for (int iLoopProduction = 0; iLoopProduction < strProductionStatusKey.Length; iLoopProduction++)
                {
                    strProductionStatusKey[iLoopProduction] = string.Format("ProductionStatusKey[{0}]", iLoopProduction);
                    strProductionStatusToolTipKey[iLoopProduction] = string.Format("ProductionStatusToolTipKey[{0}]", iLoopProduction);
                }
                // 데이터베이스에서 UI Text 뽑아옴
                string[] strProductionStatus = new string[(int)CDatabaseDefine.EProductionStatusList.PRODUCTION_STATUS_LIST_FINAL];
                string[] strProductionStatusToolTip = new string[(int)CDatabaseDefine.EProductionStatusList.PRODUCTION_STATUS_LIST_FINAL];
                for (int iLoopProduction = 0; iLoopProduction < strProductionStatus.Length; iLoopProduction++)
                {
                    strProductionStatus[iLoopProduction] = m_objDocument.GetDatabaseUIText(strProductionStatusKey[iLoopProduction], this.Name);
                    strProductionStatusToolTip[iLoopProduction] = m_objDocument.GetDatabaseUIText(strProductionStatusToolTipKey[iLoopProduction], this.Name);
                }
                // 테이블에 Row 추가
                objGridView.Rows.Clear();
                for (int iLoopProduction = 0; iLoopProduction < strProductionStatusKey.Length; iLoopProduction++)
                {
                    string[] strRowData = new string[(int)EStatusListColumn.STATUS_LIST_COLUMN_FINAL];
                    strRowData[(int)EStatusListColumn.ITEM] = strProductionStatus[iLoopProduction];
                    objGridView.Rows.Add(strRowData);
                    // Tool Tip 등록
                    objGridView[(int)EStatusListColumn.ITEM, iLoopProduction].ToolTipText = strProductionStatusToolTip[iLoopProduction];
                    objGridView[(int)EStatusListColumn.VALUE, iLoopProduction].ToolTipText = strProductionStatusToolTip[iLoopProduction];
                }
                // 그리드 뷰에 표시
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EProductionStatusList.OUT, m_objMonitoringData.m_iOutput);
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EProductionStatusList.BIN1, m_objMonitoringData.m_iBin1);
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EProductionStatusList.BIN2, m_objMonitoringData.m_iBin2);
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EProductionStatusList.BIN3, m_objMonitoringData.m_iBin3);
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EProductionStatusList.REJECT, m_objMonitoringData.m_iReject);
                // 첫 행 포커스 해제
                objGridView.ClearSelection();
            }
            #endregion

            #region Draw Grid - OperationHistory
            {
                DataGridView objGridView = this.GridViewOperationStatus;
                // UI Text Key 지정
                string[] strOperationStatusKey = new string[(int)CDatabaseDefine.EOperationStatusList.OPERATION_STATUS_LIST_FINAL];
                string[] strOperationStatusToolTipKey = new string[(int)CDatabaseDefine.EOperationStatusList.OPERATION_STATUS_LIST_FINAL];
                for (int iLoopOperation = 0; iLoopOperation < strOperationStatusKey.Length; iLoopOperation++)
                {
                    strOperationStatusKey[iLoopOperation] = string.Format("OperationStatusKey[{0}]", iLoopOperation);
                    strOperationStatusToolTipKey[iLoopOperation] = string.Format("OperationStatusToolTipKey[{0}]", iLoopOperation);
                }
                // 데이터베이스에서 UI Text 뽑아옴
                string[] strOperationStatus = new string[(int)CDatabaseDefine.EOperationStatusList.OPERATION_STATUS_LIST_FINAL];
                string[] strOperationStatusToolTip = new string[(int)CDatabaseDefine.EOperationStatusList.OPERATION_STATUS_LIST_FINAL];
                for (int iLoopOperation = 0; iLoopOperation < strOperationStatus.Length; iLoopOperation++)
                {
                    strOperationStatus[iLoopOperation] = m_objDocument.GetDatabaseUIText(strOperationStatusKey[iLoopOperation], this.Name);
                    strOperationStatusToolTip[iLoopOperation] = m_objDocument.GetDatabaseUIText(strOperationStatusToolTipKey[iLoopOperation], this.Name);
                }
                // 테이블에 Row 추가
                objGridView.Rows.Clear();
                for (int iLoopOperation = 0; iLoopOperation < strOperationStatus.Length; iLoopOperation++)
                {
                    string[] strRowData = new string[(int)EStatusListColumn.STATUS_LIST_COLUMN_FINAL];
                    strRowData[(int)EStatusListColumn.ITEM] = strOperationStatus[iLoopOperation];
                    objGridView.Rows.Add(strRowData);
                    // Tool Tip 등록
                    objGridView[(int)EStatusListColumn.ITEM, iLoopOperation].ToolTipText = strOperationStatusToolTip[iLoopOperation];
                    objGridView[(int)EStatusListColumn.VALUE, iLoopOperation].ToolTipText = strOperationStatusToolTip[iLoopOperation];
                }
                // 그리드 뷰에 표시
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EOperationStatusList.OPERATION_TIME, GetTimeToHourMinuteString(m_objMonitoringData.m_objOperationTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EOperationStatusList.EQUIPMENT_DOWN_TIME, GetTimeToMinuteSecondString(m_objMonitoringData.m_objEquipmentDownTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EOperationStatusList.STANDARD_TIME, GetTimeToMinuteSecondString(m_objMonitoringData.m_objStandardTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EOperationStatusList.REAL_PRODUCTION_TIME, GetTimeToHourMinuteString(m_objMonitoringData.m_objRealProductionTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EOperationStatusList.STANDARD_PRODUCTION_TIME, GetTimeToHourMinuteString(m_objMonitoringData.m_objStandardProductionTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EOperationStatusList.TIME_OPERATION_RATE, string.Format("{0} %", m_objMonitoringData.m_iTimeOperationRate));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EOperationStatusList.PERFORMANCE_UTILIZATION_RATE, string.Format("{0} %", m_objMonitoringData.m_iPerformanceUtilizationRate));
                // 첫 행 포커스 해제
                objGridView.ClearSelection();
            }
            #endregion

            #region Draw Grid - NonOperationHistory
            {
                DataGridView objGridView = this.GridViewNonOperationStatus;
                // UI Text Key 지정
                string[] strNonOperationStatusKey = new string[(int)CDatabaseDefine.ENonOperationStatusList.NON_OPERATION_STATUS_LIST_FINAL];
                string[] strNonOperationStatusToolTipKey = new string[(int)CDatabaseDefine.ENonOperationStatusList.NON_OPERATION_STATUS_LIST_FINAL];
                for (int iLoopNonOperation = 0; iLoopNonOperation < strNonOperationStatusKey.Length; iLoopNonOperation++)
                {
                    strNonOperationStatusKey[iLoopNonOperation] = string.Format("NonOperationStatusKey[{0}]", iLoopNonOperation);
                    strNonOperationStatusToolTipKey[iLoopNonOperation] = string.Format("NonOperationStatusToolTipKey[{0}]", iLoopNonOperation);
                }
                // 데이터베이스에서 UI Text 뽑아옴
                string[] strNonOperationStatus = new string[(int)CDatabaseDefine.ENonOperationStatusList.NON_OPERATION_STATUS_LIST_FINAL];
                string[] strNonOperationStatusToolTip = new string[(int)CDatabaseDefine.ENonOperationStatusList.NON_OPERATION_STATUS_LIST_FINAL];
                for (int iLoopNonOperation = 0; iLoopNonOperation < strNonOperationStatus.Length; iLoopNonOperation++)
                {
                    strNonOperationStatus[iLoopNonOperation] = m_objDocument.GetDatabaseUIText(strNonOperationStatusKey[iLoopNonOperation], this.Name);
                    strNonOperationStatusToolTip[iLoopNonOperation] = m_objDocument.GetDatabaseUIText(strNonOperationStatusToolTipKey[iLoopNonOperation], this.Name);
                }
                // 테이블에 Row 추가
                objGridView.Rows.Clear();
                for (int iLoopNonOperation = 0; iLoopNonOperation < strNonOperationStatus.Length; iLoopNonOperation++)
                {
                    string[] strRowData = new string[(int)EStatusListColumn.STATUS_LIST_COLUMN_FINAL];
                    strRowData[(int)EStatusListColumn.ITEM] = strNonOperationStatus[iLoopNonOperation];
                    objGridView.Rows.Add(strRowData);
                    // Tool Tip 등록
                    objGridView[(int)EStatusListColumn.ITEM, iLoopNonOperation].ToolTipText = strNonOperationStatusToolTip[iLoopNonOperation];
                    objGridView[(int)EStatusListColumn.VALUE, iLoopNonOperation].ToolTipText = strNonOperationStatusToolTip[iLoopNonOperation];
                }
                // 그리드 뷰에 표시
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.ENonOperationStatusList.ALARM_COUNT, string.Format("{0}", m_objMonitoringData.m_iNonOperationAlarmCount));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.ENonOperationStatusList.ALARM_TIME, GetTimeToMinuteSecondString(m_objMonitoringData.m_objNonOperationAlarmTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.ENonOperationStatusList.EQUIPMENT_STOP_COUNT, string.Format("{0}", m_objMonitoringData.m_iNonOperationEquipmentStopCount));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.ENonOperationStatusList.EQUIPMENT_STOP_TIME, GetTimeToMinuteSecondString(m_objMonitoringData.m_objNonOperationEquipmentStopTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.ENonOperationStatusList.EQUIPMENT_UPPER_LOSS_COUNT, string.Format("{0}", m_objMonitoringData.m_iNonOperationEquipmentUpperLossCount));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.ENonOperationStatusList.EQUIPMENT_UPPER_LOSS_TIME, GetTimeToMinuteSecondString(m_objMonitoringData.m_objNonOperationEquipmentUpperLossTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.ENonOperationStatusList.EQUIPMENT_LOWER_LOSS_COUNT, string.Format("{0}", m_objMonitoringData.m_iNonOperationEquipmentLowerLossCount));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.ENonOperationStatusList.EQUIPMENT_LOWER_LOSS_TIME, GetTimeToMinuteSecondString(m_objMonitoringData.m_objNonOperationEquipmentLowerLossTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.ENonOperationStatusList.COUNT, string.Format("{0}", m_objMonitoringData.m_iNonOperationCount));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.ENonOperationStatusList.TIME, GetTimeToMinuteSecondString(m_objMonitoringData.m_objNonOperationTime));
                // 첫 행 포커스 해제
                objGridView.ClearSelection();
            }
            #endregion

            #region Draw Grid - MomentaryStopHistory
            {
                DataGridView objGridView = this.GridViewMomentaryStopStatus;
                // UI Text Key 지정
                string[] strMomentaryStopStatusKey = new string[(int)CDatabaseDefine.EMomentaryStopStatusList.MOMENTARY_STOP_STATUS_LIST_FINAL];
                string[] strMomentaryStopStatusToolTipKey = new string[(int)CDatabaseDefine.EMomentaryStopStatusList.MOMENTARY_STOP_STATUS_LIST_FINAL];
                for (int iLoopMomentaryStop = 0; iLoopMomentaryStop < strMomentaryStopStatusKey.Length; iLoopMomentaryStop++)
                {
                    strMomentaryStopStatusKey[iLoopMomentaryStop] = string.Format("MomentaryStopStatusKey[{0}]", iLoopMomentaryStop);
                    strMomentaryStopStatusToolTipKey[iLoopMomentaryStop] = string.Format("MomentaryStopStatusToolTipKey[{0}]", iLoopMomentaryStop);
                }
                // 데이터베이스에서 UI Text 뽑아옴
                string[] strMomentaryStopStatus = new string[(int)CDatabaseDefine.EMomentaryStopStatusList.MOMENTARY_STOP_STATUS_LIST_FINAL];
                string[] strMomentaryStopStatusToolTip = new string[(int)CDatabaseDefine.EMomentaryStopStatusList.MOMENTARY_STOP_STATUS_LIST_FINAL];
                for (int iLoopMomentaryStop = 0; iLoopMomentaryStop < strMomentaryStopStatus.Length; iLoopMomentaryStop++)
                {
                    strMomentaryStopStatus[iLoopMomentaryStop] = m_objDocument.GetDatabaseUIText(strMomentaryStopStatusKey[iLoopMomentaryStop], this.Name);
                    strMomentaryStopStatusToolTip[iLoopMomentaryStop] = m_objDocument.GetDatabaseUIText(strMomentaryStopStatusToolTipKey[iLoopMomentaryStop], this.Name);
                }
                // 테이블에 Row 추가
                objGridView.Rows.Clear();
                for (int iLoopMomentaryStop = 0; iLoopMomentaryStop < strMomentaryStopStatus.Length; iLoopMomentaryStop++)
                {
                    string[] strRowData = new string[(int)EStatusListColumn.STATUS_LIST_COLUMN_FINAL];
                    strRowData[(int)EStatusListColumn.ITEM] = strMomentaryStopStatus[iLoopMomentaryStop];
                    objGridView.Rows.Add(strRowData);
                    // Tool Tip 등록
                    objGridView[(int)EStatusListColumn.ITEM, iLoopMomentaryStop].ToolTipText = strMomentaryStopStatusToolTip[iLoopMomentaryStop];
                    objGridView[(int)EStatusListColumn.VALUE, iLoopMomentaryStop].ToolTipText = strMomentaryStopStatusToolTip[iLoopMomentaryStop];
                }
                // 그리드 뷰에 표시
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMomentaryStopStatusList.ALARM_COUNT, string.Format("{0}", m_objMonitoringData.m_iMomentaryStopAlarmCount));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMomentaryStopStatusList.ALARM_TIME, GetTimeToMinuteSecondString(m_objMonitoringData.m_objMomentaryStopAlarmTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMomentaryStopStatusList.EQUIPMENT_STOP_COUNT, string.Format("{0}", m_objMonitoringData.m_iMomentaryStopEquipmentStopCount));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMomentaryStopStatusList.EQUIPMENT_STOP_TIME, GetTimeToMinuteSecondString(m_objMonitoringData.m_objMomentaryStopEquipmentStopTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMomentaryStopStatusList.EQUIPMENT_UPPER_LOSS_COUNT, string.Format("{0}", m_objMonitoringData.m_iMomentaryStopEquipmentUpperLossCount));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMomentaryStopStatusList.EQUIPMENT_UPPER_LOSS_TIME, GetTimeToMinuteSecondString(m_objMonitoringData.m_objMomentaryStopEquipmentUpperLossTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMomentaryStopStatusList.EQUIPMENT_LOWER_LOSS_COUNT, string.Format("{0}", m_objMonitoringData.m_iMomentaryStopEquipmentLowerLossCount));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMomentaryStopStatusList.EQUIPMENT_LOWER_LOSS_TIME, GetTimeToMinuteSecondString(m_objMonitoringData.m_objMomentaryStopEquipmentLowerLossTime));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMomentaryStopStatusList.COUNT, string.Format("{0}", m_objMonitoringData.m_iMomentaryStopCount));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMomentaryStopStatusList.TIME, GetTimeToMinuteSecondString(m_objMonitoringData.m_objMomentaryStopTime));
                // 첫 행 포커스 해제
                objGridView.ClearSelection();
            }
            #endregion

            #region Draw Grid - TimeToRepairHistory
            {
                DataGridView objGridView = this.GridViewTimeToRefair;
                // UI Text Key 지정
                string[] strFailoverStatusKey = new string[(int)CDatabaseDefine.ETimeToRefairList.TIME_TO_REFAIR_LIST_FINAL];
                string[] strFailoverStatusToolTipKey = new string[(int)CDatabaseDefine.ETimeToRefairList.TIME_TO_REFAIR_LIST_FINAL];
                for (int iLoopFailover = 0; iLoopFailover < strFailoverStatusKey.Length; iLoopFailover++)
                {
                    strFailoverStatusKey[iLoopFailover] = string.Format("FailoverStatusKey[{0}]", iLoopFailover);
                    strFailoverStatusToolTipKey[iLoopFailover] = string.Format("FailoverStatusToolTipKey[{0}]", iLoopFailover);
                }
                // 데이터베이스에서 UI Text 뽑아옴
                string[] strFailoverStatus = new string[(int)CDatabaseDefine.ETimeToRefairList.TIME_TO_REFAIR_LIST_FINAL];
                string[] strFailoverStatusToolTip = new string[(int)CDatabaseDefine.ETimeToRefairList.TIME_TO_REFAIR_LIST_FINAL];
                for (int iLoopFailover = 0; iLoopFailover < strFailoverStatus.Length; iLoopFailover++)
                {
                    strFailoverStatus[iLoopFailover] = m_objDocument.GetDatabaseUIText(strFailoverStatusKey[iLoopFailover], this.Name);
                    strFailoverStatusToolTip[iLoopFailover] = m_objDocument.GetDatabaseUIText(strFailoverStatusToolTipKey[iLoopFailover], this.Name);
                }
                // 테이블에 Row 추가
                objGridView.Rows.Clear();
                for (int iLoopFailover = 0; iLoopFailover < strFailoverStatus.Length; iLoopFailover++)
                {
                    string[] strRowData = new string[(int)EStatusListColumn.STATUS_LIST_COLUMN_FINAL];
                    strRowData[(int)EStatusListColumn.ITEM] = strFailoverStatus[iLoopFailover];
                    objGridView.Rows.Add(strRowData);
                    // Tool Tip 등록
                    objGridView[(int)EStatusListColumn.ITEM, iLoopFailover].ToolTipText = strFailoverStatusToolTip[iLoopFailover];
                    objGridView[(int)EStatusListColumn.VALUE, iLoopFailover].ToolTipText = strFailoverStatusToolTip[iLoopFailover];
                }
                // 그리드 뷰에 표시
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.ETimeToRefairList.MTBF, GetTimeToMinuteSecondString(m_objMonitoringData.m_objMeanTimeBetweenFailure));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.ETimeToRefairList.MTTR, GetTimeToMinuteSecondString(m_objMonitoringData.m_objMeanTimeToRefair));
                // 첫 행 포커스 해제
                objGridView.ClearSelection();
            }
            #endregion

            #region Draw Grid - MCRHistory
            {
                DataGridView objGridView = this.GridViewMCRStatus;
                // UI Text Key 지정
                string[] strMCRStatusKey = new string[(int)CDatabaseDefine.EMcrStatusList.MCR_STATUS_LIST_FINAL];
                string[] strMCRStatusToolTipKey = new string[(int)CDatabaseDefine.EMcrStatusList.MCR_STATUS_LIST_FINAL];
                for (int iLoopMCR = 0; iLoopMCR < strMCRStatusKey.Length; iLoopMCR++)
                {
                    strMCRStatusKey[iLoopMCR] = string.Format("MCRStatusKey[{0}]", iLoopMCR);
                    strMCRStatusToolTipKey[iLoopMCR] = string.Format("MCRStatusToolTipKey[{0}]", iLoopMCR);
                }
                // 데이터베이스에서 UI Text 뽑아옴
                string[] strMCRStatus = new string[(int)CDatabaseDefine.EMcrStatusList.MCR_STATUS_LIST_FINAL];
                string[] strMCRStatusToolTip = new string[(int)CDatabaseDefine.EMcrStatusList.MCR_STATUS_LIST_FINAL];
                for (int iLoopMCR = 0; iLoopMCR < strMCRStatus.Length; iLoopMCR++)
                {
                    strMCRStatus[iLoopMCR] = m_objDocument.GetDatabaseUIText(strMCRStatusKey[iLoopMCR], this.Name);
                    strMCRStatusToolTip[iLoopMCR] = m_objDocument.GetDatabaseUIText(strMCRStatusToolTipKey[iLoopMCR], this.Name);
                }
                // 테이블에 Row 추가
                objGridView.Rows.Clear();
                for (int iLoopMCR = 0; iLoopMCR < strMCRStatus.Length; iLoopMCR++)
                {
                    string[] strRowData = new string[(int)EStatusListColumn.STATUS_LIST_COLUMN_FINAL];
                    strRowData[(int)EStatusListColumn.ITEM] = strMCRStatus[iLoopMCR];
                    objGridView.Rows.Add(strRowData);
                    // Tool Tip 등록
                    objGridView[(int)EStatusListColumn.ITEM, iLoopMCR].ToolTipText = strMCRStatusToolTip[iLoopMCR];
                    objGridView[(int)EStatusListColumn.VALUE, iLoopMCR].ToolTipText = strMCRStatusToolTip[iLoopMCR];
                }
                // 그리드 뷰에 표시
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMcrStatusList.COUNT, string.Format("{0}", m_objMonitoringData.m_iTotalMCRCount));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMcrStatusList.READING_COUNT, string.Format("{0}", m_objMonitoringData.m_iMCRReadingOKCount));
                base.SetGridViewCellData(objGridView, (int)EStatusListColumn.VALUE, (int)CDatabaseDefine.EMcrStatusList.READING_RATE, string.Format("{0} %", m_objMonitoringData.m_iMCRReadingRate));
                // 첫 행 포커스 해제
                objGridView.ClearSelection();
            }
            #endregion
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {

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
            if (objFrom.Value > DateTime.Today)
            {
                objFrom.Value = DateTime.Today;
            }
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
            if (objTo.Value > DateTime.Today)
            {
                objTo.Value = DateTime.Today;
            }
            // To 날짜가 From 날짜보다 적으면 From 날짜 갱신
            if (objTo.Value < objFrom.Value)
            {
                objFrom.Value = objTo.Value;
            }
        }

        /// <summary>
        /// SELECT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            DateTimePicker objFrom = this.DateTimeFrom;
            DateTimePicker objTo = this.DateTimeTo;

            if (false == backgroundWorkerUpdate.IsBusy)
            {
                backgroundWorkerUpdate.RunWorkerAsync();
            }

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [From : {1}] [To : {2}]", "BtnSelect_Click", objFrom.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT), objTo.Value.ToString(CDatabaseDefine.DEF_DATE_FORMAT));
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void backgroundWorkerUpdate_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // 공용 데이터 셋업
            SetupDatabaseData();
            // 데이터 업데이트
            UpdateGridViewData();

            // UI 업데이트
            if (true == InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    DrawGridView();
                }));
            }
            else
            {
                DrawGridView();
            }
        }
    }
}