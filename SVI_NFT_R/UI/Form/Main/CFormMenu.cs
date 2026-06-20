using System;
using System.Drawing;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormMenu : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// CFormView
        /// </summary>
        private CFormView m_objView = null;
        /// <summary>
        /// Button Menu
        /// </summary>
        private ImageButton[] m_objMenu;

        /// <summary>
        /// 생성자
        /// 설명 :
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormMenu(CDocument objDocument)
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
        private void CFormMenu_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormMenu_FormClosed(object sender, FormClosedEventArgs e)
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
                // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
                base.m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
                CMainFrame objFormMain = Owner as CMainFrame;
                m_objView = objFormMain.GetFormView() as CFormView;
                // 버튼 이어줌
                m_objMenu = new ImageButton[(int)CDefine.EFormView.FORM_VIEW_FINAL];
                m_objMenu[(int)CDefine.EFormView.FORM_VIEW_MAIN] = this.BtnMain;
                m_objMenu[(int)CDefine.EFormView.FORM_VIEW_ALARM] = this.BtnAlarm;
                m_objMenu[(int)CDefine.EFormView.FORM_VIEW_STATISTICS] = this.BtnStatistics;
                m_objMenu[(int)CDefine.EFormView.FORM_VIEW_TEACH] = this.BtnTeach;
                m_objMenu[(int)CDefine.EFormView.FORM_VIEW_SETUP] = this.BtnSetup;
                m_objMenu[(int)CDefine.EFormView.FORM_VIEW_CONFIG] = this.BtnConfig;
                // 버튼 색상 정의
                SetButtonColor();
                // 타이머 시작
                timer.Interval = 100;
                timer.Enabled = true;

                bReturn = true;
            } while (false);

            return bReturn;
        }


        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            for (int iLoopCount = 0; iLoopCount < (int)CDefine.EFormView.FORM_VIEW_FINAL; iLoopCount++)
            {
                base.SetButtonBackColor(this.m_objMenu[iLoopCount], Color.FromArgb(224, 224, 224));
            }

            base.SetButtonBackColor(this.BtnPM, Color.FromArgb(224, 224, 224));
            base.SetButtonBackColor(this.BtnLanguage, Color.FromArgb(224, 224, 224));
            base.SetButtonBackColor(this.BtnExit, Color.FromArgb(224, 224, 224));
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
                SetButtonChangeLanguage(this.BtnMain);
                SetButtonChangeLanguage(this.BtnStatistics);
                SetButtonChangeLanguage(this.BtnConfig);
                SetButtonChangeLanguage(this.BtnTeach);
                SetButtonChangeLanguage(this.BtnAlarm);
                SetButtonChangeLanguage(this.BtnSetup);
                SetButtonChangeLanguage(this.BtnExit);

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
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                // 종료 버튼
                base.SetButtonEnable(this.BtnExit, false);
                // 언어 버튼
                base.SetButtonEnable(this.BtnLanguage, true);

                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_MAIN], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_ALARM], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_STATISTICS], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_TEACH], false);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_SETUP], false);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_CONFIG], false);
                        base.SetButtonEnable(BtnPM, false);
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_MAIN], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_ALARM], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_STATISTICS], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_TEACH], false);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_SETUP], false);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_CONFIG], true);
                        base.SetButtonEnable(BtnPM, false);
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_MAIN], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_ALARM], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_STATISTICS], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_TEACH], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_SETUP], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_CONFIG], true);
                        base.SetButtonEnable(BtnPM, false);
                        break;
                    default:
                        break;
                }
            }
            // 설비 정지 상태
            else
            {
                // 언어 버튼
                base.SetButtonEnable(this.BtnLanguage, true);

                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_MAIN], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_ALARM], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_STATISTICS], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_TEACH], false);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_SETUP], false);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_CONFIG], false);
                        base.SetButtonEnable(BtnPM, true);
                        base.SetButtonEnable(this.BtnExit, false);
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_MAIN], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_ALARM], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_STATISTICS], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_TEACH], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_SETUP], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_CONFIG], true);
                        base.SetButtonEnable(BtnPM, true);
                        base.SetButtonEnable(this.BtnExit, true);
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_MAIN], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_ALARM], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_STATISTICS], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_TEACH], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_SETUP], true);
                        base.SetButtonEnable(m_objMenu[(int)CDefine.EFormView.FORM_VIEW_CONFIG], true);
                        base.SetButtonEnable(BtnPM, true);
                        base.SetButtonEnable(this.BtnExit, true);
                        break;
                    default:
                        break;
                }
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
        /// 언어 UI 변경
        /// </summary>
        private void SetLanguageUI()
        {
            if (CDefine.ELanguage.LANGUAGE_KOREA == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
            {
                base.SetButtonText(this.BtnLanguage, "KOREA");
            }
            else if (CDefine.ELanguage.LANGUAGE_CHINA == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
            {
                base.SetButtonText(this.BtnLanguage, "CHINA");
            }
            else if (CDefine.ELanguage.LANGUAGE_ENGLISH == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
            {
                base.SetButtonText(this.BtnLanguage, "ENGLISH");
            }
            else if (CDefine.ELanguage.LANGUAGE_VIETNAM == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
            {
                base.SetButtonText(this.BtnLanguage, "VIETNAM");
            }
        }

        /// <summary>
        /// 현재 Form 색상 강조
        /// </summary>
        private void SetCurrentFormColor()
        {
            for (int iLoopFormView = 0; iLoopFormView < (int)CDefine.EFormView.FORM_VIEW_FINAL; iLoopFormView++)
            {
                base.SetButtonBackColor(m_objMenu[iLoopFormView], Color.FromArgb(224, 224, 224));
            }

            base.SetButtonBackColor(m_objMenu[(int)m_objView.GetCurrentForm()], m_colorClick);
        }

        /// <summary>
        /// 타이머 유무
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
        }

        /// <summary>
        /// Visible 유무
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // 언어 UI 변경
            SetLanguageUI();
            // 현재 Form View 색 강조
            SetCurrentFormColor();
        }

        /// <summary>
        /// 이전 폼이 PM이었다가 전환되는 경우 로그인 유무 체크
        /// </summary>
        /// <returns></returns>
        private bool GetLogin()
        {
            bool bReturn = false;

            do
            {
                //// 마스터 로그인 모드에서는 PM 창에서 다른창으로 전환 될 때 재-로그인을 하지 않는다
                //if (
                //    CDefine.EFormView.FORM_VIEW_PM == m_objView.GetCurrentForm()
                //    && false == m_objDocument.IsMasterLogin
                //    )
                //{
                //    // 로그인 다이얼로그 띄워줌
                //    m_objDocument.GetMainFrame().m_objDialogLogin.ShowDialog();
                //    if (true == m_objDocument.GetMainFrame().m_objDialogLogin.IsCanceled)
                //    {
                //        break;
                //    }
                //}

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 메인 메뉴 전환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMain_Click(object sender, EventArgs e)
        {
            do
            {
                // 이전 폼이 PM이었다가 전환되는 경우 로그인 유무 체크
                if (false == GetLogin())
                {
                    break;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Form Change : {1} -> {2}]", "BtnMain_Click", m_objView.GetCurrentForm().ToString(), CDefine.EFormView.FORM_VIEW_MAIN.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);

                m_objView.SetChangeForm(CDefine.EFormView.FORM_VIEW_MAIN);

            } while (false);
        }

        /// <summary>
        /// 알람 메뉴 전환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlarm_Click(object sender, EventArgs e)
        {
            do
            {
                // 이전 폼이 PM이었다가 전환되는 경우 로그인 유무 체크
                if (false == GetLogin())
                {
                    break;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Form Change : {1} -> {2}]", "BtnAlarm_Click", m_objView.GetCurrentForm().ToString(), CDefine.EFormView.FORM_VIEW_ALARM.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);

                m_objView.SetChangeForm(CDefine.EFormView.FORM_VIEW_ALARM);

            } while (false);
        }

        /// <summary>
        /// 통계 메뉴 전환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStatistics_Click(object sender, EventArgs e)
        {
            do
            {
                // 이전 폼이 PM이었다가 전환되는 경우 로그인 유무 체크
                if (false == GetLogin())
                {
                    break;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Form Change : {1} -> {2}]", "BtnStatistics_Click", m_objView.GetCurrentForm().ToString(), CDefine.EFormView.FORM_VIEW_STATISTICS.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);

                m_objView.SetChangeForm(CDefine.EFormView.FORM_VIEW_STATISTICS);

            } while (false);
        }

        /// <summary>
        /// 티칭 메뉴 전환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTeach_Click(object sender, EventArgs e)
        {
            do
            {
                // 이전 폼이 PM이었다가 전환되는 경우 로그인 유무 체크
                if (false == GetLogin())
                {
                    break;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Form Change : {1} -> {2}]", "BtnTeach_Click", m_objView.GetCurrentForm().ToString(), CDefine.EFormView.FORM_VIEW_TEACH.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);

                m_objView.SetChangeForm(CDefine.EFormView.FORM_VIEW_TEACH);

            } while (false);
        }

        /// <summary>
        /// 셋업 메뉴 전환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetup_Click(object sender, EventArgs e)
        {
            do
            {
                // 이전 폼이 PM이었다가 전환되는 경우 로그인 유무 체크
                if (false == GetLogin())
                {
                    break;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Form Change : {1} -> {2}]", "BtnSetup_Click", m_objView.GetCurrentForm().ToString(), CDefine.EFormView.FORM_VIEW_SETUP.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);

                m_objView.SetChangeForm(CDefine.EFormView.FORM_VIEW_SETUP);

            } while (false);
        }

        /// <summary>
        /// 설정 메뉴 전환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConfig_Click(object sender, EventArgs e)
        {
            do
            {
                // 이전 폼이 PM이었다가 전환되는 경우 로그인 유무 체크
                if (false == GetLogin())
                {
                    break;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Form Change : {1} -> {2}]", "BtnConfig_Click", m_objView.GetCurrentForm().ToString(), CDefine.EFormView.FORM_VIEW_CONFIG.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);

                m_objView.SetChangeForm(CDefine.EFormView.FORM_VIEW_CONFIG);

            } while (false);
        }

        /// <summary>
        /// PM 메뉴 전환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPM_Click(object sender, EventArgs e)
        {
            // 설비 정지 or 셋업 상태가 아니면 도어 창 열 수 없음
            if (
                CDefine.ERunStatus.Stop != m_objDocument.GetRunStatus() &&
                CDefine.ERunStatus.Setup != m_objDocument.GetRunStatus()
                )
            {
                // Msg: 설비가 정지 상태가 아니면 도어창에 접근할 수 없습니다.
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.YOU_CAN_NOT_ACCESS_THE_DOOR_DIALOG_UNLESS_THE_EQUIPMENT_IS_STOPPED);
                return;
            }

            m_objDocument.GetMainFrame().SetDelegateSafetyDialogShow();

            // 버튼 로그 추가
            string strLog = string.Format("[{0}]", "BtnPM_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 언어 변환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLanguage_Click(object sender, EventArgs e)
        {
            CDialogSelectLanguage objDialogSelectLanguage = new CDialogSelectLanguage(m_objDocument);
            objDialogSelectLanguage.InitializeFormBaseButton(BtnLanguage);
            objDialogSelectLanguage.Show();
        }

        /// <summary>
        /// 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExit_Click(object sender, EventArgs e)
        {
            do
            {
                if (CDefine.ERunStatus.Stop != m_objDocument.GetRunStatus())
                {
                    break;
                }
                // MOVE STATE 변경.
                m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE] = CCIMDefine.EMoveState.MOVE_STATE_PAUSE;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}]", "BtnExit_Click");
                m_objDocument.SetUpdateButtonLog(this, strLog);
                m_objView.SetExit();

            } while (false);
        }
    }
}