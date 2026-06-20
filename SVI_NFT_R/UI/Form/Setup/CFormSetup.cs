using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    public partial class CFormSetup : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private readonly CDocument m_objDocument;
        /// <summary>
        /// 현재 폼
        /// </summary>
        private CDefine.EFormViewSetup m_eCurrentForm = CDefine.EFormViewSetup.FORM_VIEW_SETUP_FINAL;
        /// <summary>
        /// 폼 (화면이 전환되는 폼)
        /// </summary>
        private struct structureForm
        {
            private readonly CFormCommon _objForm;
            public CFormCommon m_objForm
            {
                get { return _objForm; }
            }
            private readonly CFormInterface _IForm;
            public CFormInterface m_IForm
            {
                get { return _IForm; }
            }

            public structureForm(CFormInterface form)
            {
                _IForm = form;
                _objForm = _IForm as CFormCommon;
            }
        }

        private structureForm[] m_stForm;

        /// <summary>
        /// 폼에 사이즈에 맞춰 버튼을 동적 할당해줌
        /// 동적 생성될 버튼
        /// </summary>
        private Button[] m_btnMenu;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormSetup(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeAutoScaleByControl(panelFormMenu);
        }


        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormSetup_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormSetup_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 해제
            DeInitialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        private bool Initialize()
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
                int iMenuCount = (int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_FINAL;
                // 버튼 여백 & 넓이 설정
                int iWhiteSpace = 2;
                int iButtonWidth = (panelFormMenu.Width / iMenuCount);
                // 버튼 개수가 적어서 base 버튼보다 넓이가 크면 base 버튼 넓이로 고정
                if (iButtonWidth > BtnBase.Width)
                {
                    iButtonWidth = BtnBase.Width;
                }
                string[] strButtonName = new string[iMenuCount];
                m_btnMenu = new Button[iMenuCount];
                for (int iLoopButton = 0; iLoopButton < strButtonName.Length; iLoopButton++)
                {
                    strButtonName[iLoopButton] = ((CDefine.EFormViewSetup)iLoopButton).ToString();
                }
                // 버튼 동적 생성
                base.SetDynamicMenuButton(m_btnMenu, panelFormMenu, strButtonName, iButtonWidth, iWhiteSpace, new EventHandler(ButtonMenu_Click));
                // 버튼 이름 설정
                for (int iLoopMenu = 0; iLoopMenu < m_btnMenu.Length; iLoopMenu++)
                {
                    m_btnMenu[iLoopMenu].Name = string.Format("BtnSetupMenu[{0}]", iLoopMenu);
                }
                // 폼 생성
                m_stForm = new structureForm[iMenuCount];
                // 화면 전환
                SetChangeForm(CDefine.EFormViewSetup.FORM_VIEW_SETUP_INITIALIZE);

                // 타이머 설정.
                timer.Interval = 100;
                timer.Enabled = true;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 폼 변경
        /// </summary>
        /// <param name="eForm"></param>
        public void SetChangeForm(CDefine.EFormViewSetup eForm)
        {
            do
            {
                // 현재 폼이 이전 폼과 같으면 건너뜀
                if (m_eCurrentForm == eForm)
                {
                    break;
                }
                // 현재 폼 Invisible
                if (CDefine.EFormViewSetup.FORM_VIEW_SETUP_FINAL != m_eCurrentForm)
                {
                    m_stForm[(int)m_eCurrentForm].m_IForm.SetVisible(false);
                    m_stForm[(int)m_eCurrentForm].m_IForm.SetTimer(false);
                }

                // 생성이 되어 있지 않으면 생성
                if (null == m_stForm[(int)eForm].m_IForm)
                {
                    switch (eForm)
                    {
                        case CDefine.EFormViewSetup.FORM_VIEW_SETUP_INITIALIZE:
                            m_stForm[(int)eForm] = new structureForm(new CFormSetupInitialize(m_objDocument));
                            break;
                        case CDefine.EFormViewSetup.FORM_VIEW_SETUP_IO:
                            m_stForm[(int)eForm] = new structureForm(new CFormSetupIO(m_objDocument));
                            break;
                        case CDefine.EFormViewSetup.FORM_VIEW_SETUP_MOTION:
                            m_stForm[(int)eForm] = new structureForm(new CFormSetupMotion(m_objDocument));
                            break;
                        case CDefine.EFormViewSetup.FORM_VIEW_SETUP_NACHI:
                            m_stForm[(int)eForm] = new structureForm(new CFormSetupNachi(m_objDocument));
                            break;
                        case CDefine.EFormViewSetup.FORM_VIEW_SETUP_NACHI_OFFSET:
                            m_stForm[(int)eForm] = new structureForm(new CFormSetupNachiOffsetData(m_objDocument));
                            break;
                        case CDefine.EFormViewSetup.FORM_VIEW_SETUP_MONITOR:
                            m_stForm[(int)eForm] = new structureForm(new CFormSetupMonitor(m_objDocument));
                            break;
                        case CDefine.EFormViewSetup.FORM_VIEW_SETUP_DB:
                            m_stForm[(int)eForm] = new structureForm(new CFormSetupDB(m_objDocument));
                            break;
                        case CDefine.EFormViewSetup.FORM_VIEW_SETUP_VISION:
                            m_stForm[(int)eForm] = new structureForm(new CFormSetupVision(m_objDocument));
                            break;
                        case CDefine.EFormViewSetup.FORM_VIEW_SETUP_ALIGN_VISION:
                            m_stForm[(int)eForm] = new structureForm(new CFormSetupAlignVision(m_objDocument));
                            break;
                        case CDefine.EFormViewSetup.FORM_VIEW_SETUP_EQ_TO_EQ:
                            m_stForm[(int)eForm] = new structureForm(new CFormSetupEqToEq(m_objDocument));
                            break;
                        case CDefine.EFormViewSetup.FORM_VIEW_SETUP_DEVICE:
                            m_stForm[(int)eForm] = new structureForm(new CFormSetupDevice(m_objDocument));
                            break;
                        default:
                            break;
                    }
                    // 패널에 생성된 화면 붙임
                    SetFormDockStyle(m_stForm[(int)eForm].m_objForm, panelFormView);
                    m_stForm[(int)eForm].m_IForm.SetVisible(true);
                    m_stForm[(int)eForm].m_IForm.SetTimer(true);
                    m_stForm[(int)eForm].m_IForm.SetChangeLanguage();
                }
                // 생성 되어 있으면 Visible
                else
                {
                    m_stForm[(int)eForm].m_IForm.SetVisible(true);
                    m_stForm[(int)eForm].m_IForm.SetTimer(true);
                    m_stForm[(int)eForm].m_IForm.SetChangeLanguage();
                }

                m_eCurrentForm = eForm;
            } while (false);
        }

        /// <summary>
        /// 폼 스타일 달라붙음
        /// </summary>
        /// <param name="objForm"></param>
        /// <param name="objPanel"></param>
        public void SetFormDockStyle(Form objForm, Panel objPanel)
        {
            objForm.Owner = this;
            objForm.TopLevel = false;
            objForm.Dock = DockStyle.Fill;
            objPanel.Controls.Add(objForm);
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
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_INITIALIZE]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_IO]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_MOTION]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_NACHI]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_NACHI_OFFSET]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_DB]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_VISION]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_ALIGN_VISION]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_DEVICE]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_EQ_TO_EQ]);
                SetButtonChangeLanguage(m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_MONITOR]);

                m_stForm[(int)m_eCurrentForm].m_IForm.SetChangeLanguage();

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
        /// 타이머 유무
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            m_stForm[(int)m_eCurrentForm].m_IForm.SetTimer(bTimer);
        }

        /// <summary>
        /// Visible 유무
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
            this.Visible = bVisible;
            m_stForm[(int)m_eCurrentForm].m_IForm.SetVisible(bVisible);
        }

        /// <summary>
        /// 버튼 클릭 이벤트 정의
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonMenu_Click(object sender, EventArgs e)
        {
            Button objButton = sender as Button;
            try
            {
                var objRegex = new Regex(@"\[(.+)\]");
                if (true == objRegex.IsMatch(objButton.Name))
                {
                    string strText = objRegex.Match(objButton.Name).Groups[1].Value;
                    CDefine.EFormViewSetup eFormView = (CDefine.EFormViewSetup)Convert.ToInt32(strText);

                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Form Change : {1} -> {2}]", "ButtonMenu_Click", m_eCurrentForm.ToString(), eFormView.ToString());
                    m_objDocument.SetUpdateButtonLog(this, strLog);

                    SetChangeForm(eFormView);
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 타이머.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            for (int iLoopMenu = 0; iLoopMenu < (int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_FINAL; iLoopMenu++)
            {
                if (iLoopMenu == (int)m_eCurrentForm)
                {
                    base.SetButtonBackColor(m_btnMenu[iLoopMenu], m_colorClick);
                }
                else
                {
                    base.SetButtonBackColor(m_btnMenu[iLoopMenu], m_colorNormal);
                }
            }

            if (m_objDocument.GetUserInformation().m_eAuthorityLevel == CDefine.EUserAuthorityLevel.MASTER)
            {
                m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_DEVICE].Visible = true;
                m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_MONITOR].Visible = true;
            }
            else
            {
                if (
                    true == m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_DEVICE].Visible
                    || true == m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_MONITOR].Visible
                    )
                {
                    m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_DEVICE].Visible = false;
                    m_btnMenu[(int)CDefine.EFormViewSetup.FORM_VIEW_SETUP_MONITOR].Visible = false;
                    if (
                        m_eCurrentForm == CDefine.EFormViewSetup.FORM_VIEW_SETUP_DEVICE
                        || m_eCurrentForm == CDefine.EFormViewSetup.FORM_VIEW_SETUP_MONITOR
                        )
                    {
                        SetChangeForm(CDefine.EFormViewSetup.FORM_VIEW_SETUP_INITIALIZE);
                    }
                }
            }
        }
    }
}