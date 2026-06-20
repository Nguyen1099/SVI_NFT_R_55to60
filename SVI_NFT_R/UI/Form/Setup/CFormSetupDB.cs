using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormSetupDB : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 편집 DB 칼럼 정의
        /// </summary>
        private enum EEditDBListColumn
        {
            NAME = 0,
            VALUE,
            EDIT_DB_LIST_COLUMN_FINAL
        }

        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 현재 선택 메뉴
        /// </summary>
        private CDefine.ESetupDB m_eCurrentSetupDB = CDefine.ESetupDB.SETUP_DB_ALARM;
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
        /// 트랜잭션
        /// </summary>
        private SQLiteTransaction m_objSQLiteTransaction;
        /// <summary>
        /// 트랜잭션 사용 유무 체크
        /// </summary>
        private bool m_bSQLiteTransaction = false;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormSetupDB(CDocument objDocument)
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
        private void CFormSetupDB_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormSetupDB_FormClosed(object sender, FormClosedEventArgs e)
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
                int iMenuCount = (int)CDefine.ESetupDB.SETUP_DB_FINAL;
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
                    strButtonName[iLoopButton] = ((CDefine.ESetupDB)iLoopButton).ToString();
                }
                // 버튼 동적 생성
                base.SetDynamicMenuButton(m_btnMenu, this.panelFormMenu, strButtonName, iButtonWidth, iWhiteSpace, new EventHandler(ButtonMenu_Click));
                // 버튼 이름 설정
                for (int iLoopMenu = 0; iLoopMenu < m_btnMenu.Length; iLoopMenu++)
                {
                    m_btnMenu[iLoopMenu].Name = string.Format("BtnSetupDB[{0}]", iLoopMenu);
                }
                // 편집 DB 그리드 뷰 초기화
                string[] strEditDBList = { EEditDBListColumn.NAME.ToString(), EEditDBListColumn.VALUE.ToString() };
                if (false == InitializeGridViewEditDB(strEditDBList))
                    break;
                // 그리드 뷰에 알람으로 데이터베이스 연동
                SetDatabaseConnect(m_eCurrentSetupDB);
                // DB 리스트 그리드 뷰 초기화
                if (false == InitializeGridViewDB())
                    break;
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
            // 버튼 색 변경
            base.SetButtonBackColor(this.m_btnMenu[(int)CDefine.ESetupDB.SETUP_DB_ALARM], m_colorNormal);
            base.SetButtonBackColor(this.m_btnMenu[(int)CDefine.ESetupDB.SETUP_DB_UI_TEXT], m_colorNormal);
            base.SetButtonBackColor(this.m_btnMenu[(int)CDefine.ESetupDB.SETUP_DB_USER_MESSAGE], m_colorNormal);
            base.SetButtonBackColor(this.m_btnMenu[(int)CDefine.ESetupDB.SETUP_DB_USER], m_colorNormal);
            base.SetButtonBackColor(this.BtnInputPosition, m_colorNormal);
            base.SetButtonBackColor(this.BtnInsert, m_colorNormal);
            base.SetButtonBackColor(this.BtnUpdate, m_colorNormal);
            base.SetButtonBackColor(this.BtnDelete, m_colorNormal);
            base.SetButtonBackColor(this.BtnSaveToTxt, m_colorNormal);
            base.SetButtonBackColor(this.BtnCommit, m_colorNormal);
            base.SetButtonBackColor(this.BtnRollback, m_colorNormal);
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
                GridViewEditDB.Enabled = false;
                foreach (ImageButton btn in m_btnMenu)
                {
                    btn.Enabled = true;
                }
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        base.SetControlButtonEnable(this.Controls, false);
                        GridViewEditDB.Enabled = false;
                        foreach (ImageButton btn in m_btnMenu)
                        {
                            btn.Enabled = true;
                        }
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        base.SetControlButtonEnable(this.Controls, false);
                        GridViewEditDB.Enabled = false;
                        foreach (ImageButton btn in m_btnMenu)
                        {
                            btn.Enabled = true;
                        }
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        base.SetControlButtonEnable(this.Controls, true);
                        GridViewEditDB.Enabled = true;
                        break;
                    default:
                        break;
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
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.ESetupDB.SETUP_DB_ALARM]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.ESetupDB.SETUP_DB_UI_TEXT]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.ESetupDB.SETUP_DB_USER_MESSAGE]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.ESetupDB.SETUP_DB_USER]);
                SetButtonChangeLanguage(this.BtnInsert);
                SetButtonChangeLanguage(this.BtnInputPosition);
                SetButtonChangeLanguage(this.BtnUpdate);
                SetButtonChangeLanguage(this.BtnDelete);
                SetButtonChangeLanguage(this.BtnSaveToTxt);
                SetButtonChangeLanguage(this.BtnCommit);
                SetButtonChangeLanguage(this.BtnRollback);

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
                SetDatabaseConnect(m_eCurrentSetupDB);
                // DB 리스트 그리드 뷰 초기화
                InitializeGridViewDB();
                // 편집 DB 그리드 뷰 데이터 클리어
                SetEditDBGridViewClear();
                // 트랜잭션 시작 상태
                m_objSQLiteTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLBeginTransaction();
            }
            else
            {
                // Visible false이면 폼이 전환된 상태
                // 트랜잭션 유무 확인해서 커밋할건지 롤백할건지 메세지 창 띄워줌
                if (true == m_bSQLiteTransaction)
                {
                    // Msg: 아직 수정된 결과를 커밋하지 않았습니다. 커밋하시겠습니까? (아니요를 누르면 롤백)
                    if (DialogResult.Yes == m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.YOU_HAVE_NOT_COMMITTED_THE_RESULTS_YET_DO_YOU_WANT_TO_COMMIT))
                    {
                        // DML 언어 커밋
                        m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLCommit(m_objSQLiteTransaction);
                    }
                    else
                    {
                        // DML 언어 롤백
                        m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLRollback(m_objSQLiteTransaction);
                    }
                    // 데이터 테이블 갱신 & 그리드 뷰 갱신
                    SetDataTableUpdate();
                    // 트랜잭션 유무 false
                    m_bSQLiteTransaction = false;
                }
                else
                {
                    // DML 언어 커밋
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLCommit(m_objSQLiteTransaction);
                }
            }
        }

        /// <summary>
        /// 항목에 맞는 DB 테이블 연결
        /// </summary>
        /// <param name="eSetupDB"></param>
        private void SetDatabaseConnect(CDefine.ESetupDB eSetupDB)
        {
            DataGridView objGridView = this.GridViewDB;

            switch (eSetupDB)
            {
                case CDefine.ESetupDB.SETUP_DB_ALARM:
                    m_objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationAlarm;
                    break;
                case CDefine.ESetupDB.SETUP_DB_UI_TEXT:
                    m_objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationUIText;
                    break;
                case CDefine.ESetupDB.SETUP_DB_USER_MESSAGE:
                    m_objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationUserMessage;
                    break;
                case CDefine.ESetupDB.SETUP_DB_USER:
                    m_objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationUser;
                    break;
                default:
                    break;
            }
            objGridView.DataSource = null;
            objGridView.DataSource = m_objManagerTable.HLGetDataTable();
            // 현재 메뉴 갱신
            m_eCurrentSetupDB = eSetupDB;

            if (m_eCurrentSetupDB == CDefine.ESetupDB.SETUP_DB_ALARM)
            {
                GridViewEditDB.ScrollBars = ScrollBars.Vertical;
                GridViewEditDB.Height = 357;
            }
            else
            {
                GridViewEditDB.ScrollBars = ScrollBars.None;
                GridViewEditDB.Height = 410;
            }
        }

        /// <summary>
        /// 현재 데이터베이스 테이블에 전체 값을 보여주는 그리드 뷰
        /// 설명 : 열 클릭만 되고 나머지 조작 x
        /// </summary>
        /// <returns></returns>
        private bool InitializeGridViewDB()
        {
            bool bReturn = false;

            do
            {
                DataGridView objGridView = this.GridViewDB;
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
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                // 그리드 뷰 칼럼 정렬 x
                for (int iLoopColumn = 0; iLoopColumn < objGridView.Columns.Count; iLoopColumn++)
                {
                    objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                // 컬럼 사이즈 재조정
                SetColumnResize();

                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, 8.0);
                // 타이머 설정
                timer.Interval = 100;
                timer.Enabled = true;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 현재 데이터베이스 테이블에 한 레코드 값만을 보여주는 그리드 뷰
        /// 설명 : 셀 더블 클릭해서 편집 가능하게 해야 함
        /// </summary>
        /// <param name="strColumnName"></param>
        /// <returns></returns>
        private bool InitializeGridViewEditDB(string[] strColumnName)
        {
            bool bReturn = false;

            do
            {
                DataGridView objGridView = this.GridViewEditDB;
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
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, 9.0);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 컬럼 사이즈 재조정
        /// </summary>
        private void SetColumnResize()
        {
            DataGridView objGridView = this.GridViewDB;

            // 해당 데이터 그리드 뷰는 성능상의 문제로 AutoSize를 껏기 때문에 사이즈를 임의로 고정하기로 함.
            switch (m_eCurrentSetupDB)
            {
                case CDefine.ESetupDB.SETUP_DB_ALARM:
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.ID].Width = 50;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_KOREA].Width = 300;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_CHINA].Width = 300;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_ENGLISH].Width = 300;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_VIETNAM].Width = 300;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_KOREA].Width = 300;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_CHINA].Width = 300;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_ENGLISH].Width = 300;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_VIETNAM].Width = 300;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.LEVEL].Width = 50;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.UNIT].Width = 100;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.BOX_TYPE].Width = 50;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.BOX_RECTANGLE_X].Width = 50;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.BOX_RECTANGLE_Y].Width = 50;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.BOX_RECTANGLE_WIDTH].Width = 50;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationAlarm.BOX_RECTANGLE_HEIGHT].Width = 50;
                    GridViewEditDB.Columns[(int)EEditDBListColumn.NAME].Width = 200;
                    break;
                case CDefine.ESetupDB.SETUP_DB_UI_TEXT:
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUIText.IDX].Width = 50;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUIText.ID].Width = 300;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUIText.FORM_NAME].Width = 100;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUIText.TEXT_KOREA].Width = 100;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUIText.TEXT_CHINA].Width = 100;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUIText.TEXT_ENGLISH].Width = 100;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUIText.TEXT_VIETNAM].Width = 100;
                    GridViewEditDB.Columns[(int)EEditDBListColumn.NAME].Width = 120;
                    break;
                case CDefine.ESetupDB.SETUP_DB_USER_MESSAGE:
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUserMessage.ID].Width = 50;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUserMessage.TEXT_KOREA].Width = 200;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUserMessage.TEXT_CHINA].Width = 200;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUserMessage.TEXT_ENGLISH].Width = 200;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUserMessage.TEXT_VIETNAM].Width = 200;
                    GridViewEditDB.Columns[(int)EEditDBListColumn.NAME].Width = 120;
                    break;
                case CDefine.ESetupDB.SETUP_DB_USER:
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUser.ID].Width = 50;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUser.PASSWORD].Width = 1;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUser.NAME].Width = 100;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUser.DEPARTMENT].Width = 100;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUser.POSITION].Width = 100;
                    objGridView.Columns[(int)CDatabaseDefine.EInformationUser.AUTHORITY_LEVEL].Width = 300;
                    GridViewEditDB.Columns[(int)EEditDBListColumn.NAME].Width = 120;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 그리드 뷰에 Row값을 행열 변환해서 집어넣어줌
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="iRow"></param>
        private void SetGridViewEditDB(DataGridView objGridView, int iRow)
        {
            // 기존에 있는 편집 DB 그리드 뷰 정보 클리어
            this.GridViewEditDB.Rows.Clear();
            for (int iLoopColumn = 0; iLoopColumn < objGridView.Rows[iRow].Cells.Count; iLoopColumn++)
            {
                string[] strRowData = new string[(int)EEditDBListColumn.EDIT_DB_LIST_COLUMN_FINAL];
                strRowData[(int)EEditDBListColumn.NAME] = objGridView.Columns[iLoopColumn].Name;
                strRowData[(int)EEditDBListColumn.VALUE] = objGridView.Rows[iRow].Cells[iLoopColumn].Value.ToString();
                this.GridViewEditDB.Rows.Add(strRowData);
            }
            // 첫 행 포커스 해제
            this.GridViewEditDB.ClearSelection();
        }

        /// <summary>
        /// 편집 DB 그리드 뷰 데이터 클리어
        /// </summary>
        private void SetEditDBGridViewClear()
        {
            DataGridView objGridView = this.GridViewEditDB;
            objGridView.Rows.Clear();
        }

        /// <summary>
        /// 기본 INSERT 쿼리문 수행
        /// </summary>
        /// <param name="objTable"></param>
        /// <returns></returns>
        private bool SetDefaultQuery(CManagerTable objTable)
        {
            bool bReturn = false;

            do
            {
                try
                {
                    DataGridView objGridViewDB = this.GridViewDB;

                    string strQuery = string.Format("insert into {0} values", objTable.HLGetTableName());
                    string strValues = "";
                    object[] objData = new object[objTable.HLGetTableSchemaType().Length];
                    for (int iLoopSchema = 0; iLoopSchema < objTable.HLGetTableSchemaType().Length; iLoopSchema++)
                    {
                        // 타입 검사
                        if ("INTEGER" == objTable.HLGetTableSchemaType()[iLoopSchema])
                        {
                            strValues += string.Format("{0}", 0);
                            objData[iLoopSchema] = string.Format("{0}", 0);
                        }
                        else if (true == objTable.HLGetTableSchemaType()[iLoopSchema].Contains("VARCHAR"))
                        {
                            // 유저 탭이고 권한 레벨 레코드인 경우
                            if (CDefine.ESetupDB.SETUP_DB_USER == m_eCurrentSetupDB && (int)CDatabaseDefine.EInformationUser.AUTHORITY_LEVEL == iLoopSchema)
                            {
                                strValues += string.Format("'{0}'", CDefine.EUserAuthorityLevel.OPERATOR.ToString());
                                objData[iLoopSchema] = string.Format("{0}", CDefine.EUserAuthorityLevel.OPERATOR.ToString());
                            }
                            else
                            {
                                strValues += string.Format("'{0}'", "0");
                                objData[iLoopSchema] = string.Format("{0}", "0");
                            }
                        }
                        else if ("REAL" == objTable.HLGetTableSchemaType()[iLoopSchema] || "DOUBLE" == objTable.HLGetTableSchemaType()[iLoopSchema])
                        {
                            strValues += string.Format("{0}", 0.0);
                            objData[iLoopSchema] = string.Format("{0}", 0.0);
                        }

                        if (iLoopSchema != objTable.HLGetTableSchemaType().Length - 1)
                        {
                            strValues += ",";
                        }
                    }
                    // INSERT 쿼리문
                    strQuery = string.Format("{0} ({1})", strQuery, strValues);
                    // 쿼리문 수행
                    CErrorReturn objReturn = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLExecute(strQuery);
                    // 쿼리문 에러 나면 메세지 박스 띄워줌
                    if (true == objReturn.m_bError)
                    {
                        MessageBox.Show(string.Format("Class : {0} Function : {1} Message {2}", objReturn.m_strClassName, objReturn.m_strFunctionName, objReturn.m_strErrorMessage));
                        break;
                    }
                    // GridView DB에 해당 INSERT 추가 반영
                    DataTable objDataTable = objGridViewDB.DataSource as DataTable;
                    objDataTable.Rows.Add(objData);
                    objGridViewDB.DataSource = null;
                    objGridViewDB.DataSource = objDataTable;
                    // 그리드 뷰 칼럼 정렬 x
                    for (int iLoopColumn = 0; iLoopColumn < objGridViewDB.Columns.Count; iLoopColumn++)
                    {
                        objGridViewDB.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    // 컬럼 사이즈 재조정
                    SetColumnResize();
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// INSERT 쿼리문 수행
        /// </summary>
        /// <param name="objTable"></param>
        /// <returns></returns>
        private bool SetInsertQuery(CManagerTable objTable)
        {
            bool bReturn = false;

            do
            {
                try
                {
                    DataGridView objGridViewDB = this.GridViewDB;
                    DataGridView objGridViewEditDB = this.GridViewEditDB;

                    string strQuery = string.Format("insert into {0} values", objTable.HLGetTableName());
                    string strValues = "";
                    object[] objData = new object[objTable.HLGetTableSchemaType().Length];
                    for (int iLoopSchema = 0; iLoopSchema < objTable.HLGetTableSchemaType().Length; iLoopSchema++)
                    {
                        // 타입 검사
                        if ("INTEGER" == objTable.HLGetTableSchemaType()[iLoopSchema])
                        {
                            strValues += string.Format("{0}", Convert.ToInt32(objGridViewEditDB[(int)EEditDBListColumn.VALUE, iLoopSchema].Value));
                        }
                        else if (true == objTable.HLGetTableSchemaType()[iLoopSchema].Contains("VARCHAR"))
                        {
                            strValues += string.Format("'{0}'", objGridViewEditDB[(int)EEditDBListColumn.VALUE, iLoopSchema].Value.ToString());
                        }
                        else if ("REAL" == objTable.HLGetTableSchemaType()[iLoopSchema] || "DOUBLE" == objTable.HLGetTableSchemaType()[iLoopSchema])
                        {
                            strValues += string.Format("{0}", Convert.ToDouble(objGridViewEditDB[(int)EEditDBListColumn.VALUE, iLoopSchema].Value));
                        }
                        objData[iLoopSchema] = objGridViewEditDB[(int)EEditDBListColumn.VALUE, iLoopSchema].Value;

                        if (iLoopSchema != objTable.HLGetTableSchemaType().Length - 1)
                        {
                            strValues += ",";
                        }
                    }
                    // INSERT 쿼리문
                    strQuery = string.Format("{0} ({1})", strQuery, strValues);
                    // 쿼리문 수행
                    CErrorReturn objReturn = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLExecute(strQuery);
                    // 쿼리문 에러 나면 메세지 박스 띄워줌
                    if (true == objReturn.m_bError)
                    {
                        MessageBox.Show(string.Format("Class : {0} Function : {1} Message {2}", objReturn.m_strClassName, objReturn.m_strFunctionName, objReturn.m_strErrorMessage));
                        break;
                    }
                    // GridView DB에 해당 INSERT 추가 반영
                    DataTable objDataTable = objGridViewDB.DataSource as DataTable;
                    objDataTable.Rows.Add(objData);
                    objGridViewDB.DataSource = null;
                    objGridViewDB.DataSource = objDataTable;
                    // 그리드 뷰 칼럼 정렬 x
                    for (int iLoopColumn = 0; iLoopColumn < objGridViewDB.Columns.Count; iLoopColumn++)
                    {
                        objGridViewDB.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    // 컬럼 사이즈 재조정
                    SetColumnResize();
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// UPDATE 쿼리문 만들어서 뱉음
        /// </summary>
        /// <param name="objTable"></param>
        /// <returns></returns>
        private bool SetUpdateQuery(CManagerTable objTable)
        {
            bool bReturn = false;

            do
            {
                try
                {
                    DataGridView objGridViewDB = this.GridViewDB;
                    DataGridView objGridViewEditDB = this.GridViewEditDB;

                    // pk에 해당하는 인덱스 설정
                    int iIndexPK = m_objManagerTable.HLGetPrimaryKey();
                    // 쿼리문
                    string strQuery = string.Format("update {0}", objTable.HLGetTableName());
                    string strSet = "set ";
                    string strWhere = "";
                    for (int iLoopSchema = 0; iLoopSchema < objTable.HLGetTableSchemaType().Length; iLoopSchema++)
                    {
                        // 타입 검사
                        if ("INTEGER" == objTable.HLGetTableSchemaType()[iLoopSchema])
                        {
                            strSet += string.Format("{0} = {1}", objTable.HLGetTableSchemaName()[iLoopSchema], Convert.ToInt32(objGridViewEditDB[(int)EEditDBListColumn.VALUE, iLoopSchema].Value));
                        }
                        else if (true == objTable.HLGetTableSchemaType()[iLoopSchema].Contains("VARCHAR"))
                        {
                            strSet += string.Format("{0} = '{1}'", objTable.HLGetTableSchemaName()[iLoopSchema], objGridViewEditDB[(int)EEditDBListColumn.VALUE, iLoopSchema].Value.ToString());
                        }
                        else if ("REAL" == objTable.HLGetTableSchemaType()[iLoopSchema] || "DOUBLE" == objTable.HLGetTableSchemaType()[iLoopSchema])
                        {
                            strSet += string.Format("{0} = {1}", objTable.HLGetTableSchemaName()[iLoopSchema], Convert.ToDouble(objGridViewEditDB[(int)EEditDBListColumn.VALUE, iLoopSchema].Value));
                        }

                        if (iLoopSchema != objGridViewEditDB.Rows.Count - 1)
                        {
                            strSet += ",";
                        }
                    }
                    // Where 문
                    if ("INTEGER" == objTable.HLGetTableSchemaType()[iIndexPK])
                    {
                        strWhere = string.Format("where {0} = {1}", objTable.HLGetTableSchemaName()[iIndexPK], Convert.ToInt32(objGridViewDB[iIndexPK, objGridViewDB.CurrentCell.RowIndex].Value));
                    }
                    else if (true == objTable.HLGetTableSchemaType()[iIndexPK].Contains("VARCHAR"))
                    {
                        strWhere = string.Format("where {0} = '{1}'", objTable.HLGetTableSchemaName()[iIndexPK], objGridViewDB[iIndexPK, objGridViewDB.CurrentCell.RowIndex].Value.ToString());
                    }
                    else if ("REAL" == objTable.HLGetTableSchemaType()[iIndexPK] || "DOUBLE" == objTable.HLGetTableSchemaType()[iIndexPK])
                    {
                        strWhere = string.Format("where {0} = {1}", objTable.HLGetTableSchemaName()[iIndexPK], Convert.ToDouble(objGridViewDB[iIndexPK, objGridViewDB.CurrentCell.RowIndex].Value));
                    }

                    // UPDATE 쿼리문
                    strQuery = string.Format("{0} {1} {2}", strQuery, strSet, strWhere);
                    // 쿼리문 수행
                    CErrorReturn objReturn = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLExecute(strQuery);
                    // 쿼리문 에러 나면 메세지 박스 띄워줌
                    if (true == objReturn.m_bError)
                    {
                        MessageBox.Show(string.Format("Class : {0} Function : {1} Message {2}", objReturn.m_strClassName, objReturn.m_strFunctionName, objReturn.m_strErrorMessage));
                        break;
                    }
                    // GridView DB에 해당 UPDATE 추가 반영
                    DataTable objDataTable = objGridViewDB.DataSource as DataTable;
                    for (int iLoopSchema = 0; iLoopSchema < objTable.HLGetTableSchemaType().Length; iLoopSchema++)
                    {
                        // 타입 검사
                        if ("INTEGER" == objTable.HLGetTableSchemaType()[iLoopSchema])
                        {
                            objDataTable.Rows[objGridViewDB.CurrentCell.RowIndex][objTable.HLGetTableSchemaName()[iLoopSchema]] = Convert.ToInt32(objGridViewEditDB[(int)EEditDBListColumn.VALUE, iLoopSchema].Value);
                        }
                        else if (true == objTable.HLGetTableSchemaType()[iLoopSchema].Contains("VARCHAR"))
                        {
                            objDataTable.Rows[objGridViewDB.CurrentCell.RowIndex][objTable.HLGetTableSchemaName()[iLoopSchema]] = objGridViewEditDB[(int)EEditDBListColumn.VALUE, iLoopSchema].Value.ToString();
                        }
                        else if ("REAL" == objTable.HLGetTableSchemaType()[iLoopSchema] || "DOUBLE" == objTable.HLGetTableSchemaType()[iLoopSchema])
                        {
                            objDataTable.Rows[objGridViewDB.CurrentCell.RowIndex][objTable.HLGetTableSchemaName()[iLoopSchema]] = Convert.ToDouble(objGridViewEditDB[(int)EEditDBListColumn.VALUE, iLoopSchema].Value);
                        }
                    }
                    objGridViewDB.DataSource = null;
                    objGridViewDB.DataSource = objDataTable;
                    // 그리드 뷰 칼럼 정렬 x
                    for (int iLoopColumn = 0; iLoopColumn < objGridViewDB.Columns.Count; iLoopColumn++)
                    {
                        objGridViewDB.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    // 컬럼 사이즈 재조정
                    SetColumnResize();
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// DELETE 쿼리문 만들어서 뱉음
        /// </summary>
        /// <param name="objTable"></param>
        /// <returns></returns>
        private bool SetDeleteQuery(CManagerTable objTable)
        {
            bool bReturn = false;

            do
            {
                try
                {
                    DataGridView objGridViewDB = this.GridViewDB;

                    string strQuery = string.Format("delete from {0}", objTable.HLGetTableName());
                    string strWhere = "";
                    // pk에 해당하는 인덱스 설정
                    int iIndexPK = m_objManagerTable.HLGetPrimaryKey();
                    // Where 문
                    if ("INTEGER" == objTable.HLGetTableSchemaType()[iIndexPK])
                    {
                        strWhere = string.Format("where {0} = {1}", objTable.HLGetTableSchemaName()[iIndexPK], Convert.ToInt32(objGridViewDB[iIndexPK, objGridViewDB.CurrentCell.RowIndex].Value));
                    }
                    else if (true == objTable.HLGetTableSchemaType()[iIndexPK].Contains("VARCHAR"))
                    {
                        strWhere = string.Format("where {0} = '{1}'", objTable.HLGetTableSchemaName()[iIndexPK], objGridViewDB[iIndexPK, objGridViewDB.CurrentCell.RowIndex].Value.ToString());
                    }
                    else if ("REAL" == objTable.HLGetTableSchemaType()[iIndexPK] || "DOUBLE" == objTable.HLGetTableSchemaType()[iIndexPK])
                    {
                        strWhere = string.Format("where {0} = {1}", objTable.HLGetTableSchemaName()[iIndexPK], Convert.ToDouble(objGridViewDB[iIndexPK, objGridViewDB.CurrentCell.RowIndex].Value));
                    }
                    // DELETE 쿼리문
                    strQuery = string.Format("{0} {1}", strQuery, strWhere);
                    // 쿼리문 수행
                    CErrorReturn objReturn = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLExecute(strQuery);
                    // 쿼리문 에러 나면 메세지 박스 띄워줌
                    if (true == objReturn.m_bError)
                    {
                        MessageBox.Show(string.Format("Class : {0} Function : {1} Message {2}", objReturn.m_strClassName, objReturn.m_strFunctionName, objReturn.m_strErrorMessage));
                        break;
                    }
                    // GridView DB에 해당 DELETE 추가 반영
                    DataTable objDataTable = objGridViewDB.DataSource as DataTable;
                    objDataTable.Rows.Remove(objDataTable.Rows[objGridViewDB.CurrentCell.RowIndex]);
                    objGridViewDB.DataSource = null;
                    objGridViewDB.DataSource = objDataTable;
                    // 그리드 뷰 칼럼 정렬 x
                    for (int iLoopColumn = 0; iLoopColumn < objGridViewDB.Columns.Count; iLoopColumn++)
                    {
                        objGridViewDB.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    // 컬럼 사이즈 재조정
                    SetColumnResize();
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 데이터 테이블 갱신
        /// </summary>
        private void SetDataTableUpdate()
        {
            DataGridView objGridView = this.GridViewDB;

            do
            {
                // 문제 없으면 그리드 뷰를 새롭게 갱신 시켜줌
                string strSelectQuery = string.Format("select * from {0}", m_objManagerTable.HLGetTableName());
                DataTable objTable = new DataTable();
                // SELECT 쿼리문으로 데이터 테이블 뽑아서 기존 테이블에서 갱신하고
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLReload(strSelectQuery, ref objTable);
                // 데이터 테이블 갱신
                m_objManagerTable.HLSetDataTable(objTable);
                // 그리드 뷰도 갱신시켜주자
                objGridView.DataSource = null;
                objGridView.DataSource = objTable;
                // DB 리스트 그리드 뷰 초기화
                InitializeGridViewDB();
                // 편집 DB 그리드 뷰 데이터 클리어
                SetEditDBGridViewClear();
            } while (false);
        }

        /// <summary>
        /// 버튼 클릭 이벤트 정의
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonMenu_Click(object sender, EventArgs e)
        {
            ImageButton objButton = sender as ImageButton;
            CDefine.ESetupDB eSetupDB = 0;
            try
            {
                var objRegex = new Regex(@"\[(.+)\]");
                if (true == objRegex.IsMatch(objButton.Name))
                {
                    string strText = objRegex.Match(objButton.Name).Groups[1].Value;
                    eSetupDB = (CDefine.ESetupDB)Convert.ToInt32(strText);

                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Menu : {1} -> {2}]", "ButtonMenu_Click", m_eCurrentSetupDB.ToString(), eSetupDB.ToString());
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
            // DB 내용을 변경해서 그리드 뷰에 뿌려줌
            SetDatabaseConnect(eSetupDB);
            // DB 리스트 그리드 뷰 초기화
            InitializeGridViewDB();
            // 편집 DB 그리드 뷰 데이터 클리어
            SetEditDBGridViewClear();
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            for (int iLoopMenu = 0; iLoopMenu < (int)CDefine.ESetupDB.SETUP_DB_FINAL; iLoopMenu++)
            {

                if (iLoopMenu == (int)m_eCurrentSetupDB)
                    base.SetButtonBackColor(m_btnMenu[iLoopMenu], m_colorClick);
                else
                    base.SetButtonBackColor(m_btnMenu[iLoopMenu], m_colorNormal);
            }

            bool hideSetupDBSettingUI = true;
            GridViewEditDB.Visible = hideSetupDBSettingUI;
            BtnInputPosition.Visible = (hideSetupDBSettingUI && (m_eCurrentSetupDB == CDefine.ESetupDB.SETUP_DB_ALARM));
            BtnInsert.Visible = hideSetupDBSettingUI;
            BtnUpdate.Visible = hideSetupDBSettingUI;
            BtnDelete.Visible = hideSetupDBSettingUI;
            BtnSaveToTxt.Visible = hideSetupDBSettingUI;
            BtnCommit.Visible = hideSetupDBSettingUI;
            BtnRollback.Visible = hideSetupDBSettingUI;
        }

        /// <summary>
        /// EDIT DB 셀 선택 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewEditDB_CellClick(object sender, DataGridViewCellEventArgs e)
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
                    if ((int)EEditDBListColumn.NAME == iColumn)
                        break;

                    // 스키마 타입 검사해서 INTEGER 키패드
                    if ("INTEGER" == m_objManagerTable.HLGetTableSchemaType()[iRow])
                    {
                        // 키패드
                        using (var objKeypad = new FormKeyPad(Convert.ToInt32(objGridView.CurrentCell.Value)))
                        {
                            if (DialogResult.OK == objKeypad.ShowDialog())
                            {
                                base.SetGridViewCellData(objGridView, iColumn, iRow, string.Format("{0:D}", (int)objKeypad.m_dResultValue));
                            }
                        }
                    }
                    // 스키마 타입 검사해서 VARCHAR 키보드
                    else if (true == m_objManagerTable.HLGetTableSchemaType()[iRow].Contains("VARCHAR"))
                    {
                        // 해당 탭이 알람 정보이고 LEVEL, UNIT, BOX_TYPE 레코드이면 클릭할 때마다 enum string 값을 변경해야함
                        if (CDefine.ESetupDB.SETUP_DB_ALARM == m_eCurrentSetupDB)
                        {
                            // LEVEL, UNIT, BOX_TYPE 레코드인 경우
                            if ((int)CDatabaseDefine.EInformationAlarm.LEVEL == iRow)
                            {
                                // enum string 변경
                                string strEnum = objGridView[iColumn, iRow].Value.ToString();
                                int iValue = (int)m_objDocument.m_objProcessDatabase.GetAlarmLevel(strEnum);
                                iValue = (iValue + 1) % (int)CDatabaseDefine.EAlarmLevelList.ALARM_LEVEL_LIST_FINAL;
                                base.SetGridViewCellData(objGridView, iColumn, iRow, ((CDatabaseDefine.EAlarmLevelList)iValue).ToString());
                            }
                            else if ((int)CDatabaseDefine.EInformationAlarm.BOX_TYPE == iRow)
                            {
                                // enum string 변경
                                string strEnum = objGridView[iColumn, iRow].Value.ToString();
                                int iValue = (int)m_objDocument.m_objProcessDatabase.GetAlarmBoxType(strEnum);
                                iValue = (iValue + 1) % (int)CDatabaseDefine.EAlarmBoxTypeList.ALARM_BOX_TYPE_LIST_FINAL;
                                base.SetGridViewCellData(objGridView, iColumn, iRow, ((CDatabaseDefine.EAlarmBoxTypeList)iValue).ToString());
                            }
                            // 그 외 레코드인 경우
                            else
                            {
                                using (var objKeyboard = new FormKeyBoard(objGridView.CurrentCell.Value.ToString()))
                                {
                                    if (DialogResult.OK == objKeyboard.ShowDialog())
                                    {
                                        base.SetGridViewCellData(objGridView, iColumn, iRow, string.Format("{0}", objKeyboard.m_strReturnValue));
                                    }
                                }
                            }
                        }
                        // 해당 탭이 유저 정보이고 권한 레벨 레코드이면 클릭할 때마다 enum string 값을 변경해야함
                        else if (CDefine.ESetupDB.SETUP_DB_USER == m_eCurrentSetupDB)
                        {
                            // 암호 레코드인 경우
                            if ((int)CDatabaseDefine.EInformationUser.PASSWORD == iRow)
                            {
                                string password = null;
#if LOGIN_VER2
                                using (var keypad = new FormKeyPad(0d))
                                {
                                    if (DialogResult.OK == keypad.ShowDialog())
                                    {
                                        password = Convert.ToInt32(keypad.m_dResultValue).ToString();
                                    }
                                }
#else
                                using (var keyboard = new FormKeyBoard(objGridView.CurrentCell.Value.ToString()))
                                {
                                    if (DialogResult.OK == keyboard.ShowDialog())
                                    {
                                        password = keyboard.m_strReturnValue;
                                    }
                                }
#endif
                                if (string.IsNullOrEmpty(password) == false)
                                {
                                    CCryptography objCryptography = new CCryptography();
                                    base.SetGridViewCellData(objGridView, iColumn, iRow, objCryptography.SetEncoding(password));
                                }
                            }
                            else if ((int)CDatabaseDefine.EInformationUser.AUTHORITY_LEVEL == iRow)
                            {
                                // enum string 변경
                                string strEnum = objGridView[iColumn, iRow].Value.ToString();
                                int iValue = (int)m_objDocument.m_objProcessDatabase.GetUserAuthorityLevel(strEnum);
                                iValue = (iValue + 1) % (int)CDefine.EUserAuthorityLevel.USER_AUTHORITY_LEVEL_FINAL;
                                base.SetGridViewCellData(objGridView, iColumn, iRow, ((CDefine.EUserAuthorityLevel)iValue).ToString());
                            }
                            // 그 외 레코드인 경우
                            else
                            {
                                using (var objKeyboard = new FormKeyBoard(objGridView.CurrentCell.Value.ToString()))
                                {
                                    if (DialogResult.OK == objKeyboard.ShowDialog())
                                    {
                                        base.SetGridViewCellData(objGridView, iColumn, iRow, string.Format("{0}", objKeyboard.m_strReturnValue));
                                    }
                                }
                            }
                        }
                        // 알람, 유저를 제외한 탭인 경우
                        else
                        {
                            using (var objKeyboard = new FormKeyBoard(objGridView.CurrentCell.Value.ToString()))
                            {
                                if (DialogResult.OK == objKeyboard.ShowDialog())
                                {
                                    base.SetGridViewCellData(objGridView, iColumn, iRow, string.Format("{0}", objKeyboard.m_strReturnValue));
                                }
                            }
                        }
                    }
                    // 스키마 타입 검사해서 DOUBLE 키패드
                    else if ("REAL" == m_objManagerTable.HLGetTableSchemaType()[iRow] || "DOUBLE" == m_objManagerTable.HLGetTableSchemaType()[iRow])
                    {
                        // 키패드
                        using (var objKeypad = new FormKeyPad(Convert.ToDouble(objGridView.CurrentCell.Value)))
                        {
                            if (DialogResult.OK == objKeypad.ShowDialog())
                            {
                                base.SetGridViewCellData(objGridView, iColumn, iRow, string.Format("{0:F3}", objKeypad.m_dResultValue));
                            }
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
        /// INSERT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInsert_Click(object sender, EventArgs e)
        {
            DataGridView objGridViewDB = this.GridViewDB;
            DataGridView objGridViewEditDB = this.GridViewEditDB;

            do
            {
                // DB에 데이터가 없는 경우 Default Insert를 해줘야함
                if (0 == objGridViewDB.Rows.Count)
                {
                    // 기본 INSERT 쿼리문
                    if (true == SetDefaultQuery(m_objManagerTable))
                    {
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [{1}]", "BtnInsert_Click", "Insert Query true");
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                        m_bSQLiteTransaction = true;
                    }
                    else
                    {
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [{1}]", "BtnInsert_Click", "Insert Query false");
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }

                    break;
                }
                // 편집 DB에 그리드 뷰가 클리어면 튕김
                else if (0 == objGridViewEditDB.Rows.Count)
                {
                    break;
                }

                // INSERT 쿼리문 실행 & 그리드 뷰에 해당 쿼리 추가
                if (true == SetInsertQuery(m_objManagerTable))
                {
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [{1}]", "BtnInsert_Click", "Insert Query true");
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                    // 쿼리문이 성공적이면 트랜잭션 유무 true
                    m_bSQLiteTransaction = true;
                }
                else
                {
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [{1}]", "BtnInsert_Click", "Insert Query false");
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }

            } while (false);
        }

        /// <summary>
        /// UPDATE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            DataGridView objGridView = this.GridViewEditDB;

            do
            {
                // 편집 DB에 그리드 뷰가 클리어면 튕김
                if (0 == objGridView.Rows.Count)
                {
                    break;
                }
                // UPDATE 쿼리문 실행 & 그리드 뷰에 해당 쿼리 추가
                if (true == SetUpdateQuery(m_objManagerTable))
                {
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [{1}]", "BtnUpdate_Click", "Update Query true");
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                    // 쿼리문이 성공적이면 트랜잭션 유무 true
                    m_bSQLiteTransaction = true;
                }
                else
                {
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [{1}]", "BtnUpdate_Click", "Update Query false");
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }

            } while (false);
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DataGridView objGridView = this.GridViewEditDB;

            do
            {
                // 편집 DB에 그리드 뷰가 클리어면 튕김
                if (0 == objGridView.Rows.Count)
                {
                    break;
                }
                // DELETE 쿼리문 실행 & 그리드 뷰에 해당 쿼리 추가
                if (true == SetDeleteQuery(m_objManagerTable))
                {
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [{1}]", "BtnDelete_Click", "Delete Query true");
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                    // 쿼리문이 성공적이면 트랜잭션 유무 true
                    m_bSQLiteTransaction = true;
                }
                else
                {
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [{1}]", "BtnDelete_Click", "Delete Query false");
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }

            } while (false);
        }

        /// <summary>
        /// SAVE TO TXT FILE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveToTxt_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE))
            {
                return;
            }

            // DataTable 받아서 Csv로 저장
            CTxtFile objTxtFile = new CTxtFile();
            // 현재 경로 폴더 확인 & 생성
            string strPath = string.Format(@"{0}\Database", m_objDocument.m_objConfig.GetLogPath());
            DirectoryInfo objDirectory = new DirectoryInfo(strPath);
            if (false == objDirectory.Exists)
            {
                objDirectory.Create();
            }
            // 현재 경로에 테이블 이름.txt 로 파일 생성
            strPath = string.Format(@"{0}\{1}_{2}.txt", strPath, m_btnMenu[(int)m_eCurrentSetupDB].Text, string.Format("{0:yyyyMMdd_HHmm}", DateTime.Now));
            objTxtFile.SetDataTableToTxt(strPath, m_objManagerTable.HLGetDataTable());
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Path : {1}]", "BtnSaveToCsv_Click", strPath);
            m_objDocument.SetUpdateButtonLog(this, strLog);
            MessageBox.Show(string.Format("Path : {0}", strPath));

            m_objDocument.RunCellLogToExcel(strPath);
        }

        /// <summary>
        /// COMMIT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCommit_Click(object sender, EventArgs e)
        {
            DataGridView objGridView = this.GridViewDB;
            // DML 언어 커밋
            m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLCommit(m_objSQLiteTransaction);
            // 데이터 테이블 갱신 & 그리드 뷰 갱신
            SetDataTableUpdate();
            // 커밋하고 바로 트랜잭션 시작으로
            m_objSQLiteTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLBeginTransaction();
            // 커밋했으니까 트랜잭션 유무 false
            m_bSQLiteTransaction = false;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnCommit_Click", "Database Transaction Commit");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// ROLLBACK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRollback_Click(object sender, EventArgs e)
        {
            DataGridView objGridView = this.GridViewDB;
            // DML 언어 롤백
            m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLRollback(m_objSQLiteTransaction);
            // 데이터 테이블 갱신 & 그리드 뷰 갱신
            SetDataTableUpdate();
            // 롤백하고 바로 트랜잭션 시작으로
            m_objSQLiteTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objSQLite.HLBeginTransaction();
            // 롤백했으니까 트랜잭션 유무 false
            m_bSQLiteTransaction = false;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnRollback_Click", "Database Transaction Rollback");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 현재 셀 상태 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewDB_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridView objGridView = sender as DataGridView;

            do
            {
                try
                {
                    if (null == objGridView.CurrentCell)
                    {
                        break;
                    }
                    int iSelectedRow = objGridView.CurrentCell.RowIndex;
                    // Edit DB 그리드 뷰에 데이터 뿌려줌
                    SetGridViewEditDB(objGridView, iSelectedRow);
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        private void btnInputPosition_Click(object sender, EventArgs e)
        {
            if (0 == GridViewEditDB.Rows.Count)
            {
                return;
            }

            if (m_eCurrentSetupDB != CDefine.ESetupDB.SETUP_DB_ALARM)
            {
                return;
            }
            if (
                string.IsNullOrWhiteSpace(Convert.ToString(GridViewEditDB[(int)EEditDBListColumn.VALUE, 13].Value)) ||
                string.IsNullOrWhiteSpace(Convert.ToString(GridViewEditDB[(int)EEditDBListColumn.VALUE, 14].Value == DBNull.Value)) ||
                string.IsNullOrWhiteSpace(Convert.ToString(GridViewEditDB[(int)EEditDBListColumn.VALUE, 15].Value == DBNull.Value)) ||
                string.IsNullOrWhiteSpace(Convert.ToString(GridViewEditDB[(int)EEditDBListColumn.VALUE, 16].Value == DBNull.Value))
                )
            {
                return;
            }
            var x = Convert.ToSingle(GridViewEditDB[(int)EEditDBListColumn.VALUE, 13].Value); //x
            var y = Convert.ToSingle(GridViewEditDB[(int)EEditDBListColumn.VALUE, 14].Value); //y
            var width = Convert.ToSingle(GridViewEditDB[(int)EEditDBListColumn.VALUE, 15].Value); //width
            var hight = Convert.ToSingle(GridViewEditDB[(int)EEditDBListColumn.VALUE, 16].Value); //hight
            if (true == ShowAlarmPosition(ref x, ref y, ref width, ref hight))
            {
                GridViewEditDB[(int)EEditDBListColumn.VALUE, 13].Value = x;
                GridViewEditDB[(int)EEditDBListColumn.VALUE, 14].Value = y;
                GridViewEditDB[(int)EEditDBListColumn.VALUE, 15].Value = width;
                GridViewEditDB[(int)EEditDBListColumn.VALUE, 16].Value = hight;
            }
        }

        public bool ShowAlarmPosition(ref float x, ref float y, ref float width, ref float hight)
        {
            bool bResult = false;
            // LossCode 
            using (CDialogAlarmPosition objDialogAlarmPosition = new CDialogAlarmPosition(x, y, width, hight))
            {
                objDialogAlarmPosition.StartPosition = FormStartPosition.CenterParent;
                if (objDialogAlarmPosition.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    bResult = true;
                }


                if (true == bResult)
                {
                    x = objDialogAlarmPosition.drawPoint.X;
                    y = objDialogAlarmPosition.drawPoint.Y;
                    width = objDialogAlarmPosition.drawPoint.width;
                    hight = objDialogAlarmPosition.drawPoint.hight;

                    Clipboard.SetText($"{x}\t,\t{y}\t,\t{width}\t,\t{hight}");
                }
            }
            return bResult;
        }
    }
}