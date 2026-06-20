using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormMainCIM : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// SV 리스트 칼럼 정의
        /// </summary>
        private enum ESVListColumn
        {
            INDEX_SORT = 0,
            SVID,
            SVNAME,
            UNIT_RAW,
            MIN_RAW,
            MAX_RAW,
            VALUE_RAW,

            VALUE,//측정값 
            MIN,
            MAX,
            UNIT_NAME,
            SV_LIST_COLUMN_FINAL
        };

        /// <summary>
        /// EC 리스트 칼럼 정의
        /// </summary>
        private enum EECListColumn
        {
            INDEX_SORT = 0,
            ECID,
            EC_ITEM_NAME,
            ECDEF,
            ECSLL,
            ECSUL,
            ECWLL,
            ECWUL,
        }

        /// <summary>
        /// EC 수정 필요 항목 정의
        /// </summary>
        private enum EEChangeDefine
        {
            ECSLL = 0,
            ECSUL,
            ECWLL,
            ECWUL
        }

        /// <summary>
        /// EC 편집 칼럼 정의
        /// </summary>
        private enum EEditECListColumn
        {
            NAME = 0,
            VALUE,
            EDIT_EC_LIST_COLUMN_FINAL
        }

        /// <summary>
        /// 폼에 사이즈에 맞춰 버튼을 동적 할당해줌
        /// 동적 생성될 버튼
        /// </summary>
        private ImageButton[] m_btnMenu;
        /// <summary>
        /// 현재 선택 메뉴
        /// </summary>
        private CDefine.ECimSystemMenu m_eCurrentSystemMenu = CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_FDC;
        private int m_iMaxDisplayItemCount;
        private int m_iCurrentPage;
        private int m_iLastPage;
        private readonly HashSet<int> mStringTypeDataIndex = new HashSet<int>();
        /// <summary>
        /// 인자값인 SVID List 구조체에 값으로 그리드 뷰 갱신
        /// </summary>
        public delegate void DelegateSetSVIDListUpdate(List<object> objSVIDList);
        public DelegateSetSVIDListUpdate m_delegateSetSVIDListUpdate;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormMainCIM(CDocument objDocument)
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
        private void CFormMainCIM_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormMainCIM_FormClosed(object sender, FormClosedEventArgs e)
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
                    break;

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
                int iMenuCount = (int)CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_FINAL;
                // 버튼 여백 & 넓이 설정
                int iWhiteSpace = 2;
                int iButtonWidth = (this.PnlFormMenu.Width / iMenuCount);
                // 버튼 개수가 적어서 base 버튼보다 넓이가 크면 base 버튼 넓이로 고정
                if (iButtonWidth > this.BtnBase.Width)
                {
                    iButtonWidth = this.BtnBase.Width;
                }
                string[] strButtonName = new string[iMenuCount];
                m_btnMenu = new ImageButton[iMenuCount];
                for (int iLoopButton = 0; iLoopButton < strButtonName.Length; iLoopButton++)
                {
                    strButtonName[iLoopButton] = ((CDefine.ECimSystemMenu)iLoopButton).ToString();
                }
                // 버튼 동적 생성
                base.SetDynamicMenuButton(m_btnMenu, this.PnlFormMenu, strButtonName, iButtonWidth, iWhiteSpace, new EventHandler(ButtonMenu_Click));
                // 버튼 이름 설정
                for (int iLoopMenu = 0; iLoopMenu < m_btnMenu.Length; iLoopMenu++)
                {
                    m_btnMenu[iLoopMenu].Name = string.Format("BtnCimSystemMenu[{0}]", iLoopMenu);
                }
                // GridViewEdit 위치 설정   
                PnlGridViewEditBase.Location = PnlCimMessageBase.Location;
                // 편집 DB 그리드 뷰 초기화
                string[] strEditECList = { EEditECListColumn.NAME.ToString(), EEditECListColumn.VALUE.ToString() };
                if (false == InitializeGridViewEdit(strEditECList))
                    break;
                SetDataTableConnect(m_eCurrentSystemMenu);
                // SV 리스트 그리드 뷰 초기화
                if (false == InitializeGridView())
                    break;
                // 알람 리스트 초기화
                int idx = 0;
                var alarmList = Enum.GetValues(typeof(CAlarmDefine.EAlarmList)).Cast<CAlarmDefine.EAlarmList>()
                    .Select(i => m_objDocument.m_objAlarmList.GetIsHeavyAlarmCode(i) ? $"[{idx++:000}] {i}" : $"({idx++:000}) {i}")
                    .ToArray();
                CbbAlarmIndex.Items.AddRange(alarmList);
                CbbAlarmIndex.SelectedIndex = 1;
                Console.WriteLine(string.Join(Environment.NewLine, alarmList));
                // 버튼 색상 정의
                SetButtonColor();
                // SVID 갱신 델리게이트 생성
                m_delegateSetSVIDListUpdate = new DelegateSetSVIDListUpdate(SetSVIDListUpdate);
                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                // ! 페이지 초기 로딩시 Cell Merge가 안되는 현상을 해결하기 위해 추가함
                SetSVIDListUpdate(m_objDocument.GetSVIDList());

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            base.SetButtonBackColor(this.m_btnMenu[(int)CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_FDC], m_colorNormal);
            base.SetButtonBackColor(this.m_btnMenu[(int)CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_EC], m_colorNormal);
            base.SetButtonBackColor(this.BtnTitle, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleCIMTestMessage, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleParameterEditView, m_colorLabel);
            base.SetButtonBackColor(this.BtnCurrentPosition, m_colorLabelData);

            base.SetButtonBackColor(this.BtnCIMTestMessageCellInfo, m_colorNormal);
            base.SetButtonBackColor(this.BtnCIMTestMessage01, m_colorNormal);
            base.SetButtonBackColor(this.BtnCIMTestMessage02, m_colorNormal);
            base.SetButtonBackColor(this.BtnCIMTestMessage03, m_colorNormal);
            base.SetButtonBackColor(this.BtnCIMTestMessage04, m_colorNormal);
            base.SetButtonBackColor(this.BtnCIMTestMessage05, m_colorNormal);
            base.SetButtonBackColor(this.BtnCIMTestMessage06, m_colorNormal);
            base.SetButtonBackColor(this.BtnCIMTestMessage07, m_colorNormal);
            base.SetButtonBackColor(this.BtnCIMTestMessage08, m_colorNormal);
            base.SetButtonBackColor(this.BtnCIMTestMessage09, m_colorNormal);
            base.SetButtonBackColor(this.BtnCIMTestMessage10, m_colorNormal);
            base.SetButtonBackColor(this.BtnCIMTestMessage11, m_colorNormal);

            base.SetButtonBackColor(this.BtnForceAlarm, m_colorNormal);

            base.SetButtonBackColor(this.BtnFirst, m_colorNormal);
            base.SetButtonBackColor(this.BtnLast, m_colorNormal);
            base.SetButtonBackColor(this.BtnNext, m_colorNormal);
            base.SetButtonBackColor(this.BtnPrivious, m_colorNormal);

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
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                base.SetControlButtonEnable(this.Controls, false);
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        base.SetControlButtonEnable(this.Controls, false);
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        base.SetControlButtonEnable(this.Controls, false);
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        base.SetControlButtonEnable(this.Controls, true);
                        break;
                    default:
                        break;
                }
            }

            // 네비게이션 버튼은 권한에 상관없이 활성화 한다
            SetButtonEnable(BtnFirst, true);
            SetButtonEnable(BtnPrivious, true);
            SetButtonEnable(BtnNext, true);
            SetButtonEnable(BtnLast, true);
            SetButtonEnable(m_btnMenu[(int)CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_FDC], true);
            SetButtonEnable(m_btnMenu[(int)CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_EC], true);

            // 마스터 모드로 로그인시 강제 알람 기능을 활성화 시킨다
            if (objUserInformation.m_eAuthorityLevel == CDefine.EUserAuthorityLevel.MASTER)
            {
                if (true != BtnForceAlarm.Visible)
                {
                    BtnForceAlarm.Visible = true;
                }
                if (true != CbbAlarmIndex.Visible)
                {
                    CbbAlarmIndex.Visible = true;
                }
                SetButtonEnable(BtnForceAlarm, true);
            }
            else
            {
                if (false != BtnForceAlarm.Visible)
                {
                    BtnForceAlarm.Visible = false;
                }
                if (false != CbbAlarmIndex.Visible)
                {
                    CbbAlarmIndex.Visible = false;
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
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_FDC]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_EC]);
                SetButtonChangeLanguage(this.BtnTitle);
                SetButtonChangeLanguage(this.BtnTitleCIMTestMessage);
                SetButtonChangeLanguage(this.BtnTitleParameterEditView);
                SetButtonChangeLanguage(this.BtnFirst);
                SetButtonChangeLanguage(this.BtnLast);
                SetButtonChangeLanguage(this.BtnNext);
                SetButtonChangeLanguage(this.BtnPrivious);
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
                // 그리드 뷰에 현재 메뉴로 데이터테이블 연동
                SetDataTableConnect(m_eCurrentSystemMenu);
                // 그리드 뷰 초기화
                InitializeGridView();
                // 편집 그리드 뷰 데이터 클리어
                SetEditGridViewClear();
                // SVID 리스트 업데이트
                SetSVIDListUpdate(m_objDocument.GetSVIDList());
            }
        }

        /// <summary>
        /// 그리드 뷰 초기화
        /// </summary>
        /// <param name="objGridView"></param>
        /// <returns></returns>
        private bool InitializeGridView()
        {
            bool bReturn = false;

            do
            {
                DataGridView objGridView = this.GridView;
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
                // 칼럼 정렬 기능 x
                for (int iLoopColumn = 0; iLoopColumn < objGridView.Columns.Count; iLoopColumn++)
                {
                    objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                    objGridView.Columns[iLoopColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                // 컬럼 사이즈 재조정
                SetColumnResize();
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, 10.0);

                // 페이지 계산
                double maxDisplayItemCount = (objGridView.Height - objGridView.ColumnHeadersHeight) / objGridView.Rows[0].Height;
                m_iMaxDisplayItemCount = Convert.ToInt32(maxDisplayItemCount);
                m_iLastPage = (objGridView.Rows.Count - 1) / m_iMaxDisplayItemCount;
                m_iCurrentPage = 0;
                SetPageView();
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 현재 테이블에 한 레코드 값만을 보여주는 그리드 뷰
        /// 설명 : 셀 더블 클릭해서 편집 가능하게 해야 함
        /// </summary>
        /// <param name="strColumnName"></param>
        /// <returns></returns>
        private bool InitializeGridViewEdit(string[] strColumnName)
        {
            bool bReturn = false;

            do
            {
                DataGridView objGridView = this.GridViewEdit;
                // 그리드 뷰 기본 스타일 초기화
                if (false == base.InitializeGridView(objGridView))
                {
                    break;
                }
                // 그리드 뷰 ReadOnly
                objGridView.ReadOnly = true;
                // 그리드 뷰 다중 선택 x
                objGridView.MultiSelect = false;
                // 그리드 뷰 스크롤바 x
                objGridView.ScrollBars = ScrollBars.None;
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                // 그리드 뷰 칼럼 추가
                for (int iLoopColumn = 0; iLoopColumn < strColumnName.Length; iLoopColumn++)
                {
                    objGridView.Columns.Add(string.Format("{0}", iLoopColumn), strColumnName[iLoopColumn]);
                    // 칼럼 정렬 기능 x
                    objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                objGridView.Columns[(int)EEditECListColumn.NAME].Width = (int)(objGridView.Width * 0.3);
                objGridView.Columns[(int)EEditECListColumn.VALUE].Width = (int)(objGridView.Width * 0.7);

                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, 9.0);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        private void SetColumnResize()
        {
            DataGridView objGridView = this.GridView;

            switch (m_eCurrentSystemMenu)
            {
                case CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_FDC:
                    {
                        objGridView.Columns[(int)ESVListColumn.INDEX_SORT].Width = (int)(objGridView.Width * 0.01);
                        objGridView.Columns[(int)ESVListColumn.INDEX_SORT].HeaderText = "NO.";
                        objGridView.Columns[(int)ESVListColumn.SVID].Width = (int)(objGridView.Width * 0.02);
                        objGridView.Columns[(int)ESVListColumn.SVID].HeaderText = "SVID";
                        objGridView.Columns[(int)ESVListColumn.SVNAME].HeaderText = "SVNAME";
                        objGridView.Columns[(int)ESVListColumn.UNIT_RAW].Visible = false;
                        objGridView.Columns[(int)ESVListColumn.MIN_RAW].Visible = false;
                        objGridView.Columns[(int)ESVListColumn.MAX_RAW].Visible = false;
                        objGridView.Columns[(int)ESVListColumn.VALUE_RAW].Visible = false;
                        objGridView.Columns[(int)ESVListColumn.VALUE].Width = (int)(objGridView.Width * 0.07);
                        objGridView.Columns[(int)ESVListColumn.VALUE].HeaderText = "INPUT";
                        objGridView.Columns[(int)ESVListColumn.MIN].Width = (int)(objGridView.Width * 0.07);
                        objGridView.Columns[(int)ESVListColumn.MIN].HeaderText = "MIDDLE";
                        objGridView.Columns[(int)ESVListColumn.MAX].Width = (int)(objGridView.Width * 0.07);
                        objGridView.Columns[(int)ESVListColumn.MAX].HeaderText = "OUTPUT";
                        objGridView.Columns[(int)ESVListColumn.UNIT_NAME].Width = (int)(objGridView.Width * 0.05);
                        objGridView.Columns[(int)ESVListColumn.UNIT_NAME].HeaderText = "UNIT";
                    }
                    break;
                case CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_EC:
                    {
                        objGridView.Columns[(int)ESVListColumn.INDEX_SORT].Width = (int)(objGridView.Width * 0.015);
                        objGridView.Columns[(int)ESVListColumn.INDEX_SORT].HeaderText = "NO.";
                        objGridView.Columns[(int)EECListColumn.ECID].Width = (int)(objGridView.Width * 0.035);
                        objGridView.Columns[(int)EECListColumn.ECID].HeaderText = "ECID";
                        objGridView.Columns[(int)EECListColumn.EC_ITEM_NAME].HeaderText = "EC_ITEM_NAME";
                        objGridView.Columns[(int)EECListColumn.ECDEF].Width = (int)(objGridView.Width * 0.05);
                        objGridView.Columns[(int)EECListColumn.ECDEF].HeaderText = "ECDEF";
                        objGridView.Columns[(int)EECListColumn.ECSLL].Width = (int)(objGridView.Width * 0.05);
                        objGridView.Columns[(int)EECListColumn.ECSLL].HeaderText = "ECSLL";
                        objGridView.Columns[(int)EECListColumn.ECSUL].Width = (int)(objGridView.Width * 0.05);
                        objGridView.Columns[(int)EECListColumn.ECSUL].HeaderText = "ECSUL";
                        objGridView.Columns[(int)EECListColumn.ECWLL].Width = (int)(objGridView.Width * 0.05);
                        objGridView.Columns[(int)EECListColumn.ECWLL].HeaderText = "ECWLL";
                        objGridView.Columns[(int)EECListColumn.ECWUL].Width = (int)(objGridView.Width * 0.1);
                        objGridView.Columns[(int)EECListColumn.ECWUL].HeaderText = "ECWUL";
                    }
                    break;
            }
        }

        /// <summary>
        /// 그리드 뷰에 Row값을 행열 변환해서 집어넣어줌
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="iRow"></param>
        private void SetGridViewEdit(DataGridView objGridView, int iRow)
        {
            // 기존에 있는 편집 DB 그리드 뷰 정보 클리어
            this.GridViewEdit.Rows.Clear();
            for (int iLoopColumn = 0; iLoopColumn < objGridView.Rows[iRow].Cells.Count; iLoopColumn++)
            {
                string[] strRowData = new string[(int)EEditECListColumn.EDIT_EC_LIST_COLUMN_FINAL];
                strRowData[(int)EEditECListColumn.NAME] = objGridView.Columns[iLoopColumn].Name;
                strRowData[(int)EEditECListColumn.VALUE] = objGridView.Rows[iRow].Cells[iLoopColumn].Value.ToString();
                this.GridViewEdit.Rows.Add(strRowData);
            }
            // 첫 행 포커스 해제
            this.GridViewEdit.ClearSelection();
        }

        /// <summary>
        /// 편집 DB 그리드 뷰 데이터 클리어
        /// </summary>
        private void SetEditGridViewClear()
        {
            DataGridView objGridView = this.GridViewEdit;
            objGridView.Rows.Clear();
        }

        /// <summary>
        /// SVID 데이터베이스 연동
        /// </summary>
        private void SetDataTableConnect(CDefine.ECimSystemMenu eSystemMenu)
        {
            DataGridView objGridView = this.GridView;
            DataTable objTable = new DataTable();
            switch (eSystemMenu)
            {
                case CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_FDC:
                    {
                        string strQuery = "";

                        strQuery = string.Format("select {0},{1},{2},{3},{4} from {5}",
                            m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationSVID.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationSvid.SVID],
                            m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationSVID.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationSvid.SVNAME],
                            m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationSVID.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationSvid.UNIT],
                            m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationSVID.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationSvid.MIN_VALUE],
                            m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationSVID.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationSvid.MAX_VALUE],
                            m_objDocument.m_objConfig.GetDatabaseParameter().strTableInformationSVID);

                        objTable.Columns.Add(ESVListColumn.INDEX_SORT.ToString());
                        m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLReload(strQuery, ref objTable);
                        // VALUE 칼럼 추가
                        objTable.Columns.Add(ESVListColumn.VALUE_RAW.ToString());
                        objTable.Columns.Add(ESVListColumn.VALUE.ToString());
                        objTable.Columns.Add(ESVListColumn.MIN.ToString());
                        objTable.Columns.Add(ESVListColumn.MAX.ToString());
                        objTable.Columns.Add(ESVListColumn.UNIT_NAME.ToString());
                        // MIN, MAX 컬럼에 데이터 추가
                        int index = 0;
                        foreach (DataRow row in objTable.Rows)
                        {
                            row[(int)ESVListColumn.INDEX_SORT] = ++index;

                            string minRaw = (string)row[(int)ESVListColumn.MIN_RAW];
                            if (minRaw == "" || minRaw == "N/A")
                            {
                                row[(int)ESVListColumn.MIN] = string.Format("{0}", row[(int)ESVListColumn.MIN_RAW]);
                            }
                            else
                            {
                                row[(int)ESVListColumn.MIN] = string.Format("{0} {1}", row[(int)ESVListColumn.MIN_RAW], row[(int)ESVListColumn.UNIT_RAW]);
                            }

                            string maxRaw = (string)row[(int)ESVListColumn.MAX_RAW];
                            if (maxRaw == "" || maxRaw == "N/A")
                            {
                                row[(int)ESVListColumn.MAX] = string.Format("{0}", row[(int)ESVListColumn.MAX_RAW]);
                            }
                            else
                            {
                                row[(int)ESVListColumn.MAX] = string.Format("{0} {1}", row[(int)ESVListColumn.MAX_RAW], row[(int)ESVListColumn.UNIT_RAW]);
                            }
                            row[(int)ESVListColumn.UNIT_NAME] = string.Format("{0}", row[(int)ESVListColumn.UNIT_RAW]);
                        }
                    }
                    break;
                case CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_EC:
                    {
                        foreach (var item in Enum.GetNames(typeof(EECListColumn)))
                        {
                            objTable.Columns.Add(item);
                        }
                        var objEcmParameter = m_objDocument.m_objConfig.GetEcmParameter();
                        int index = 0;
                        foreach (CConfig.EEquipmentEcmList ecmIndex in Enum.GetValues(typeof(CConfig.EEquipmentEcmList)))
                        {
                            object[] obj = new object[objTable.Columns.Count];
                            obj[(int)EECListColumn.INDEX_SORT] = ++index;
                            obj[(int)EECListColumn.ECID] = objEcmParameter.ECID[ecmIndex].ToString();
                            obj[(int)EECListColumn.EC_ITEM_NAME] = objEcmParameter.ECItemName[ecmIndex];
                            obj[(int)EECListColumn.ECDEF] = objEcmParameter.ECDef[ecmIndex].ToString("F3");
                            obj[(int)EECListColumn.ECSLL] = objEcmParameter.ECSll[ecmIndex].ToString("F3");
                            obj[(int)EECListColumn.ECSUL] = objEcmParameter.ECSul[ecmIndex].ToString("F3");
                            obj[(int)EECListColumn.ECWLL] = objEcmParameter.ECWll[ecmIndex].ToString("F3");
                            obj[(int)EECListColumn.ECWUL] = objEcmParameter.ECWul[ecmIndex].ToString("F3");
                            objTable.Rows.Add(obj);
                        }
                    }
                    break;
                default:
                    break;
            }
            objGridView.DataSource = null;
            objGridView.DataSource = objTable;
            m_eCurrentSystemMenu = eSystemMenu;
        }

        /// <summary>
        /// SVID 리스트 값 갱신
        /// </summary>
        /// <param name="objSVIDList"></param>
        private void SetSVIDListUpdate(List<object> objSVIDList)
        {
            if (m_eCurrentSystemMenu != CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_FDC)
            {
                return;
            }

            DataGridView objGridView = this.GridView;
            DataTable objDataTable = objGridView.DataSource as DataTable;

            try
            {
                mStringTypeDataIndex.Clear();
                for (int iLoopSVIDList = 0; iLoopSVIDList < objSVIDList.Count; iLoopSVIDList++)
                {
                    string strType = (objSVIDList[iLoopSVIDList].GetType()).Name;
                    string unitName = Convert.ToString(objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.UNIT_RAW]);
                    unitName = unitName != "N/A" ? unitName : "";
                    if (strType == "Double")
                    {
                        objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.MAX] = objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.MIN] = objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.VALUE] = string.Format("{0:F03}", objSVIDList[iLoopSVIDList]);
                        objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.VALUE_RAW] = (double)objSVIDList[iLoopSVIDList];
                    }
                    else if (strType == "Int32")
                    {
                        objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.MAX] = objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.MIN] = objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.VALUE] = string.Format("{0}", objSVIDList[iLoopSVIDList]);
                        objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.VALUE_RAW] = (int)objSVIDList[iLoopSVIDList];
                    }
                    else if (strType == "Int16")
                    {
                        objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.MAX] = objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.MIN] = objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.VALUE] = string.Format("{0}", objSVIDList[iLoopSVIDList]);
                        objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.VALUE_RAW] = (short)objSVIDList[iLoopSVIDList];
                    }
                    else
                    {
                        objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.MAX] = objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.MIN] = objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.VALUE] = string.Format("{0}", objSVIDList[iLoopSVIDList]);
                        objDataTable.Rows[iLoopSVIDList][(int)ESVListColumn.VALUE_RAW] = objSVIDList[iLoopSVIDList];
                        // 스트링형 데이터 표시시 Column을 Merge 처리해야됨
                        mStringTypeDataIndex.Add(iLoopSVIDList + 1);
                    }
                }

                // 첫번째 행이 Merge된 셀일경우 값이 업데이트 되지 않는 버그가 있어서 강제로 Refresh 함수를 호출함
                objGridView.Refresh();
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 현재 페이지를 디스플레이 한다
        /// </summary>
        private void SetPageView()
        {
            DataGridView objGridView = this.GridView;
            DataTable objDataTable = objGridView.DataSource as DataTable;
            string expression = string.Format("[{0}] > {1} AND [{2}] <= {3}", ESVListColumn.INDEX_SORT.ToString(), m_iCurrentPage * m_iMaxDisplayItemCount, ESVListColumn.INDEX_SORT.ToString(), (m_iCurrentPage + 1) * m_iMaxDisplayItemCount);
            objDataTable.DefaultView.RowFilter = expression;
            objGridView.ClearSelection();

            switch (m_eCurrentSystemMenu)
            {
                case CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_FDC:
                    {
                        // 문자열 데이터 셀 병합
                        for (int rowIndex = 0; rowIndex < objGridView.Rows.Count; rowIndex++)
                        {
                            int index = Convert.ToInt32(objGridView[(int)ESVListColumn.INDEX_SORT, rowIndex].Value);
                            if (
                                true == mStringTypeDataIndex.Contains(index)
                                && false == (objGridView[(int)ESVListColumn.VALUE, rowIndex] is HMergedCell)
                                )
                            {
                                var oldValue = objGridView[(int)ESVListColumn.VALUE, rowIndex].Value;
                                for (int i = (int)ESVListColumn.VALUE; i <= (int)ESVListColumn.MAX; i++)
                                {
                                    objGridView[i, rowIndex] = new HMergedCell();
                                    var newCell = objGridView[i, rowIndex] as HMergedCell;
                                    newCell.LeftColumn = (int)ESVListColumn.VALUE;
                                    newCell.RightColumn = (int)ESVListColumn.MAX;
                                    newCell.Value = oldValue;
                                }
                            }
                        }
                    }
                    break;
                case CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_EC:
                    {
                        // 편집 그리드 뷰 데이터 클리어
                        SetEditGridViewClear();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 현재 셀 상태 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridView objGridView = sender as DataGridView;

            do
            {
                try
                {
                    if (m_eCurrentSystemMenu != CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_EC)
                    {
                        break;
                    }

                    if (null == objGridView.CurrentCell)
                    {
                        break;
                    }

                    int iSelectedRow = objGridView.CurrentCell.RowIndex;
                    // Edit DB 그리드 뷰에 데이터 뿌려줌
                    SetGridViewEdit(objGridView, iSelectedRow);
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // 현재 페이지 갱신
            base.SetButtonText(this.BtnCurrentPosition, string.Format("PAGE ( {0} / {1} )", m_iCurrentPage + 1, m_iLastPage + 1));
            for (int iLoopMenu = 0; iLoopMenu < (int)CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_FINAL; iLoopMenu++)
            {
                if (iLoopMenu == (int)m_eCurrentSystemMenu)
                    base.SetButtonBackColor(m_btnMenu[iLoopMenu], m_colorClick);
                else
                    base.SetButtonBackColor(m_btnMenu[iLoopMenu], m_colorNormal);
            }
        }

        /// <summary>
        /// EDIT DB 셀 선택 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewEdit_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView objGridView = sender as DataGridView;

            do
            {
                try
                {
                    // 클릭하면 키보드 띄워서 데이터 받아서 다시 해당 셀에 집어넣음
                    int iRow = objGridView.CurrentCell.RowIndex;
                    int iColumn = objGridView.CurrentCell.ColumnIndex;
                    // NAME 변경 안됨
                    if ((int)EEditECListColumn.NAME == iColumn)
                        break;

                    // 변경이 필요한 항목만 체크하여 Key Pad 창 띄워줌
                    bool bChangeValue = false;
                    foreach (var item in Enum.GetValues(typeof(EEChangeDefine)))
                    {
                        if (objGridView.Rows[iRow].Cells[(int)EEditECListColumn.NAME].Value.ToString() == item.ToString())
                        {
                            bChangeValue = true;
                            break;
                        }
                    }
                    if (bChangeValue == false)
                    {
                        break;
                    }

                    // 키패드
                    using (var objKeypad = new FormKeyPad(Convert.ToDouble(objGridView.CurrentCell.Value)))
                    {
                        if (DialogResult.OK == objKeypad.ShowDialog())
                        {
                            base.SetGridViewCellData(objGridView, iColumn, iRow, string.Format("{0:F3}", objKeypad.m_dResultValue));
                        }
                    }
                    objGridView.ClearSelection();
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }
        /// <summary>
        /// 심 테스트 메세지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCIMTestMessage01_Click(object sender, EventArgs e)
        {
            // Run 상태 일경우
            if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                return;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage01_Click", CDialogCIMMessageTest.EMessageType.CELL_IN_EVENT.ToString(), true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            // 인자값 받아서 심 테스트 함수 호출
            using (CDialogCIMMessageTest objDialogCIMMessageTest = new CDialogCIMMessageTest(m_objDocument, CDialogCIMMessageTest.EMessageType.CELL_IN_EVENT))
            {
                objDialogCIMMessageTest.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage01_Click", CDialogCIMMessageTest.EMessageType.CELL_IN_EVENT.ToString(), false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 심 테스트 메세지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCIMTestMessageCellInfo_Click(object sender, EventArgs e)
        {
            // Run 상태 일경우
            if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                return;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessageCellInfo_Click", CDialogCIMMessageTest.EMessageType.CELL_INFOMATION_EVENT.ToString(), true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            // 인자값 받아서 심 테스트 함수 호출
            using (CDialogCIMMessageTest objDialogCIMMessageTest = new CDialogCIMMessageTest(m_objDocument, CDialogCIMMessageTest.EMessageType.CELL_INFOMATION_EVENT))
            {
                objDialogCIMMessageTest.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessageCellInfo_Click", CDialogCIMMessageTest.EMessageType.CELL_INFOMATION_EVENT.ToString(), false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 심 테스트 메세지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCIMTestMessage02_Click(object sender, EventArgs e)
        {
            // Run 상태 일경우
            if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                return;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage02_Click", CDialogCIMMessageTest.EMessageType.INSP_RESULT_EVENT.ToString(), true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (CDialogCIMMessageTest objDialogCIMMessageTest = new CDialogCIMMessageTest(m_objDocument, CDialogCIMMessageTest.EMessageType.INSP_RESULT_EVENT))
            {
                objDialogCIMMessageTest.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage02_Click", CDialogCIMMessageTest.EMessageType.INSP_RESULT_EVENT.ToString(), false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 심 테스트 메세지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCIMTestMessage03_Click(object sender, EventArgs e)
        {
            // Run 상태 일경우
            if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                return;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage03_Click", CDialogCIMMessageTest.EMessageType.ALARM_REPORT.ToString(), true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (CDialogCIMMessageTest objDialogCIMMessageTest = new CDialogCIMMessageTest(m_objDocument, CDialogCIMMessageTest.EMessageType.ALARM_REPORT))
            {
                objDialogCIMMessageTest.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage03_Click", CDialogCIMMessageTest.EMessageType.ALARM_REPORT.ToString(), false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 심 테스트 메세지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCIMTestMessage04_Click(object sender, EventArgs e)
        {
            // Run 상태 일경우
            if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                return;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage04_Click", CDialogCIMMessageTest.EMessageType.TACT_EVENT.ToString(), true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (CDialogCIMMessageTest objDialogCIMMessageTest = new CDialogCIMMessageTest(m_objDocument, CDialogCIMMessageTest.EMessageType.TACT_EVENT))
            {
                objDialogCIMMessageTest.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage04_Click", CDialogCIMMessageTest.EMessageType.TACT_EVENT.ToString(), false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 심 테스트 메세지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCIMTestMessage05_Click(object sender, EventArgs e)
        {
            // Run 상태 일경우
            if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                return;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage05_Click", CDialogCIMMessageTest.EMessageType.EQ_STATE_CHANGE_EVENT.ToString(), true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (CDialogCIMMessageTest objDialogCIMMessageTest = new CDialogCIMMessageTest(m_objDocument, CDialogCIMMessageTest.EMessageType.EQ_STATE_CHANGE_EVENT))
            {
                objDialogCIMMessageTest.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage05_Click", CDialogCIMMessageTest.EMessageType.EQ_STATE_CHANGE_EVENT.ToString(), false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 심 테스트 메세지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCIMTestMessage06_Click(object sender, EventArgs e)
        {
            // Run 상태 일경우
            if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                return;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage06_Click", CDialogCIMMessageTest.EMessageType.CONVEYOR_MOVE_STATE_CHANGE_EVENT.ToString(), true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (CDialogCIMMessageTest objDialogCIMMessageTest = new CDialogCIMMessageTest(m_objDocument, CDialogCIMMessageTest.EMessageType.CONVEYOR_MOVE_STATE_CHANGE_EVENT))
            {
                objDialogCIMMessageTest.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage06_Click", CDialogCIMMessageTest.EMessageType.CONVEYOR_MOVE_STATE_CHANGE_EVENT.ToString(), false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 심 테스트 메세지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCIMTestMessage07_Click(object sender, EventArgs e)
        {
            // Run 상태 일경우
            if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                return;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage07_Click", CDialogCIMMessageTest.EMessageType.CURRENT_PPID_CHANGE_EVENT.ToString(), true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (CDialogCIMMessageTest objDialogCIMMessageTest = new CDialogCIMMessageTest(m_objDocument, CDialogCIMMessageTest.EMessageType.CURRENT_PPID_CHANGE_EVENT))
            {
                objDialogCIMMessageTest.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage07_Click", CDialogCIMMessageTest.EMessageType.CURRENT_PPID_CHANGE_EVENT.ToString(), false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 심 테스트 메세지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCIMTestMessage08_Click(object sender, EventArgs e)
        {
            // Run 상태 일경우
            if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                return;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage08_Click", CDialogCIMMessageTest.EMessageType.CURRENT_PPID_CHANGE_EVENT.ToString(), true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (CDialogCIMMessageTest objDialogCIMMessageTest = new CDialogCIMMessageTest(m_objDocument, CDialogCIMMessageTest.EMessageType.CURRENT_PPID_CHANGE_EVENT))
            {
                objDialogCIMMessageTest.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage08_Click", CDialogCIMMessageTest.EMessageType.CURRENT_PPID_CHANGE_EVENT.ToString(), false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 심 테스트 메세지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCIMTestMessage10_Click(object sender, EventArgs e)
        {
            // Run 상태 일경우
            if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                return;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage10_Click", CDialogCIMMessageTest.EMessageType.EC_CHANGE_EVENT.ToString(), true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (CDialogCIMMessageTest objDialogCIMMessageTest = new CDialogCIMMessageTest(m_objDocument, CDialogCIMMessageTest.EMessageType.EC_CHANGE_EVENT))
            {
                objDialogCIMMessageTest.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage10_Click", CDialogCIMMessageTest.EMessageType.EC_CHANGE_EVENT.ToString(), false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 심 테스트 메세지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCIMTestMessage09_Click(object sender, EventArgs e)
        {
            // Run 상태 일경우
            if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                return;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage09_Click", CDialogCIMMessageTest.EMessageType.CURRENT_PPID_PARAM_CHANGE_EVENT.ToString(), true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (CDialogCIMMessageTest objDialogCIMMessageTest = new CDialogCIMMessageTest(m_objDocument, CDialogCIMMessageTest.EMessageType.CURRENT_PPID_PARAM_CHANGE_EVENT))
            {
                objDialogCIMMessageTest.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage09_Click", CDialogCIMMessageTest.EMessageType.CURRENT_PPID_PARAM_CHANGE_EVENT.ToString(), false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 심 테스트 메세지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCIMTestMessage11_Click(object sender, EventArgs e)
        {
            // Run 상태 일경우
            if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                return;

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage11_Click", CDialogCIMMessageTest.EMessageType.CELL_OUT_EVENT.ToString(), true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            // 인자값 받아서 심 테스트 함수 호출
            using (CDialogCIMMessageTest objDialogCIMMessageTest = new CDialogCIMMessageTest(m_objDocument, CDialogCIMMessageTest.EMessageType.CELL_OUT_EVENT))
            {
                objDialogCIMMessageTest.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [MessageType : {1}] [{2}]", "BtnCIMTestMessage11_Click", CDialogCIMMessageTest.EMessageType.CELL_OUT_EVENT.ToString(), false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 테스트 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageButton1_Click(object sender, EventArgs e)
        {
            //// 심 연결 안되어 있으면 연결하자.
            //if (false == m_objDocument.m_bCIMConnected)
            //{
            //    // 시뮬레이션 모드 가 아니고 MFQ 모드가 아닐 경우.
            //    if (
            //        CDefine.enumSimulationMode.SIMULATION_MODE_OFF == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode
            //        && false == m_objDocument.m_bMFQMode
            //        && CDefine.enumRunModeType.RUN_MODE_TYPE_REAL_RUN == m_objDocument.GetRunModeType()
            //        )
            //    {
            //        //m_objDocument.GetMainFrame().ShowCIMDisConnect( true );
            //        m_objDocument.m_objProcessCIM.DisConnect();
            //        m_objDocument.m_objProcessCIM.Connect();
            //    }
            //}
            string alarmItemName = (string)CbbAlarmIndex.SelectedItem;
            Regex regex = new Regex(@"^(?:\[(\d{3})\]|\((\d{3})\))\s(.+)$");
            var match = regex.Match(alarmItemName);
            var alarmIndex = (CAlarmDefine.EAlarmList)Enum.Parse(typeof(CAlarmDefine.EAlarmList), match.Result("$3"));
            m_objDocument.SetForcedAlarm(alarmIndex);
            //m_objDocument.SetRunMode(CDefine.enumRunMode.RUN_MODE_STOP);
        }

        /// <summary>
        /// 첫번째 페이지로 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFirst_Click(object sender, EventArgs e)
        {
            m_iCurrentPage = 0;
            SetPageView();

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [CurrentPage : {1}]", "BtnFirst_Click", m_iCurrentPage + 1);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 마지막 페이지로 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLast_Click(object sender, EventArgs e)
        {
            m_iCurrentPage = m_iLastPage;
            SetPageView();

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [CurrentPage : {1}]", "BtnLast_Click", m_iCurrentPage + 1);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 이전 페이지로 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPrivious_Click(object sender, EventArgs e)
        {
            if (m_iCurrentPage > 0)
            {
                m_iCurrentPage--;
            }
            SetPageView();

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [CurrentPage : {1}]", "BtnPrivious_Click", m_iCurrentPage + 1);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 다음 페이지로 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (m_iCurrentPage < m_iLastPage)
            {
                m_iCurrentPage++;
            }
            SetPageView();

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [CurrentPage : {1}]", "BtnNext_Click", m_iCurrentPage + 1);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 버튼 클릭 이벤트 정의
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonMenu_Click(object sender, EventArgs e)
        {
            ImageButton objButton = sender as ImageButton;
            CDefine.ECimSystemMenu eCimSystemMenu = 0;
            try
            {
                var objRegex = new Regex(@"\[(.+)\]");
                if (true == objRegex.IsMatch(objButton.Name))
                {
                    string strText = objRegex.Match(objButton.Name).Groups[1].Value;
                    eCimSystemMenu = (CDefine.ECimSystemMenu)Convert.ToInt32(strText);

                    if (m_eCurrentSystemMenu == eCimSystemMenu)
                    {
                        return;
                    }

                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Menu : {1} -> {2}]", "ButtonMenu_Click", m_eCurrentSystemMenu.ToString(), eCimSystemMenu.ToString());
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }

                switch (eCimSystemMenu)
                {
                    case CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_FDC:
                        {
                            PnlCimMessageBase.Visible = true;
                            PnlAlarmBase.Visible = true;
                            PnlGridViewEditBase.Visible = false;
                        }
                        break;
                    case CDefine.ECimSystemMenu.FORM_VIEW_MAIN_CIM_EC:
                        {
                            PnlCimMessageBase.Visible = false;
                            PnlAlarmBase.Visible = true;
                            PnlGridViewEditBase.Visible = true;
                        }
                        break;
                    default:
                        break;
                }

                // DataTable 내용을 변경해서 그리드 뷰에 뿌려줌
                SetDataTableConnect(eCimSystemMenu);
                // 그리드 뷰 초기화
                InitializeGridView();
                // 편집 그리드 뷰 데이터 클리어
                SetEditGridViewClear();
                // SVID 리스트 업데이트
                SetSVIDListUpdate(m_objDocument.GetSVIDList());
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataGridView objGridView = this.GridView;
            DataGridView objGridViewEdit = this.GridViewEdit;

            try
            {
                // 편집 그리드 뷰가 클리어면 튕김
                if (0 == objGridViewEdit.Rows.Count)
                {
                    return;
                }

                // 그리드뷰 선택 Cell과 편집 그리드 뷰 List가 다르면 튕김
                int iRow = objGridView.CurrentCell.RowIndex;
                if (objGridView.Rows[iRow].Cells[(int)EECListColumn.ECID].Value.ToString() != objGridViewEdit.Rows[(int)EECListColumn.ECID].Cells[(int)EEditECListColumn.VALUE].Value.ToString())
                {
                    return;
                }

                if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE))
                {
                    return;
                }

                CConfig.CEcmParameter objEcmParameter = m_objDocument.m_objConfig.GetEcmParameter().DeepClone();
                int index = Array.IndexOf(objEcmParameter.ECItemName.Values.ToArray(), objGridViewEdit.Rows[(int)EECListColumn.EC_ITEM_NAME].Cells[(int)EEditECListColumn.VALUE].Value.ToString());
                CConfig.EEquipmentEcmList eEcmParam = (CConfig.EEquipmentEcmList)index;

                // 버튼 로그 추가
                string strLog =
                    $"[BtnSave_Click] [Menu : {m_eCurrentSystemMenu}] " +
                    $"[Before Value -> ECID : {objEcmParameter.ECID[eEcmParam]} " +
                    $"ECNAME : {objEcmParameter.ECItemName[eEcmParam]} " +
                    $"ECSll : {objEcmParameter.ECSll[eEcmParam]} " +
                    $"ECSul : {objEcmParameter.ECSul[eEcmParam]} " +
                    $"ECWll : {objEcmParameter.ECWll[eEcmParam]} " +
                    $"ECWul : {objEcmParameter.ECWul[eEcmParam]} ]";
                m_objDocument.SetUpdateButtonLog(this, strLog);

                // 저장
                objEcmParameter.ECSll[eEcmParam] = Convert.ToDouble(objGridViewEdit.Rows[(int)EECListColumn.ECSLL].Cells[(int)EEditECListColumn.VALUE].Value);
                objEcmParameter.ECSul[eEcmParam] = Convert.ToDouble(objGridViewEdit.Rows[(int)EECListColumn.ECSUL].Cells[(int)EEditECListColumn.VALUE].Value);
                objEcmParameter.ECWll[eEcmParam] = Convert.ToDouble(objGridViewEdit.Rows[(int)EECListColumn.ECWLL].Cells[(int)EEditECListColumn.VALUE].Value);
                objEcmParameter.ECWul[eEcmParam] = Convert.ToDouble(objGridViewEdit.Rows[(int)EECListColumn.ECWUL].Cells[(int)EEditECListColumn.VALUE].Value);

                // EC 변경사항있는경우 보고됨
                m_objDocument.SetECList(objEcmParameter);

                strLog =
                    $"[BtnSave_Click] [Menu : {m_eCurrentSystemMenu}] " +
                    $"[After Value -> ECID : {objEcmParameter.ECID[eEcmParam]}, " +
                    $"ECNAME : {objEcmParameter.ECItemName[eEcmParam]}, " +
                    $"ECSll : {objEcmParameter.ECSll[eEcmParam]}, " +
                    $"ECSul : {objEcmParameter.ECSul[eEcmParam]}, " +
                    $"ECWll : {objEcmParameter.ECWll[eEcmParam]}, " +
                    $"ECWul : {objEcmParameter.ECWul[eEcmParam]}, ]";
                m_objDocument.SetUpdateButtonLog(this, strLog);

                // DataTable 내용을 변경해서 그리드 뷰에 뿌려줌
                SetDataTableConnect(m_eCurrentSystemMenu);
                // 그리드 뷰 초기화
                InitializeGridView();
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 폼이 처음 보여질 때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormMainCIM_Shown(object sender, EventArgs e)
        {
            GridView.ClearSelection();
        }
    }
}