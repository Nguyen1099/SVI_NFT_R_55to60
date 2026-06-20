using System;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    public partial class CFormView : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 현재 폼
        /// </summary>
        private CDefine.EFormView m_eCurrentForm = CDefine.EFormView.FORM_VIEW_FINAL;
        /// <summary>
        /// 폼 (화면이 전환되는 폼)
        /// </summary>
        private struct structureForm
        {
            private CFormCommon _objForm;
            public CFormCommon m_objForm
            {
                get { return _objForm; }
            }
            private CFormInterface _IForm;
            public CFormInterface m_IForm
            {
                get { return _IForm; }
            }

            public structureForm(CFormInterface form)
            {
                this._IForm = form;
                this._objForm = this._IForm as CFormCommon;
            }
        }

        private structureForm[] m_stForm;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormView(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormView_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormView_FormClosed(object sender, FormClosedEventArgs e)
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
                // 폼 생성
                m_stForm = new structureForm[(int)CDefine.EFormView.FORM_VIEW_FINAL];
                // 화면 전환
                SetChangeForm(CDefine.EFormView.FORM_VIEW_MAIN);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 폼 변경
        /// </summary>
        /// <param name="eForm"></param>
        public void SetChangeForm(CDefine.EFormView eForm)
        {
            do
            {
                // 현재 폼이 이전 폼과 같으면 건너뜀
                if (m_eCurrentForm == eForm)
                {
                    break;
                }
                // 현재 폼 Invisible
                if (CDefine.EFormView.FORM_VIEW_FINAL != m_eCurrentForm)
                {
                    m_stForm[(int)m_eCurrentForm].m_IForm.SetVisible(false);
                    m_stForm[(int)m_eCurrentForm].m_IForm.SetTimer(false);
                }

                // 통계 UI에서 쓸대없이 메모리를 많이 가지고 있음으로 창을 빠져나올때 강제로 가비지 컬랙터를 실행한다.
                if (CDefine.EFormView.FORM_VIEW_STATISTICS == m_eCurrentForm)
                {
                    GC.Collect();
                }

                // 생성이 되어 있지 않으면 생성
                if (null == m_stForm[(int)eForm].m_IForm)
                {
                    switch (eForm)
                    {
                        case CDefine.EFormView.FORM_VIEW_MAIN:
                            m_stForm[(int)eForm] = new structureForm(new CFormMain(m_objDocument) as CFormInterface);
                            break;
                        case CDefine.EFormView.FORM_VIEW_ALARM:
                            m_stForm[(int)eForm] = new structureForm(new CFormAlarm(m_objDocument) as CFormInterface);
                            break;
                        case CDefine.EFormView.FORM_VIEW_STATISTICS:
                            m_stForm[(int)eForm] = new structureForm(new CFormStatistics(m_objDocument) as CFormInterface);
                            break;
                        case CDefine.EFormView.FORM_VIEW_TEACH:
                            m_stForm[(int)eForm] = new structureForm(new CFormTeach(m_objDocument) as CFormInterface);
                            break;
                        case CDefine.EFormView.FORM_VIEW_SETUP:
                            m_stForm[(int)eForm] = new structureForm(new CFormSetup(m_objDocument) as CFormInterface);
                            break;
                        case CDefine.EFormView.FORM_VIEW_CONFIG:
                            m_stForm[(int)eForm] = new structureForm(new CFormConfig(m_objDocument) as CFormInterface);
                            break;
                        default:
                            break;
                    }
                    // 패널에 생성된 화면 붙임
                    SetFormDockStyle(m_stForm[(int)eForm].m_objForm, this.panelView);
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

        public void SetChangeInitializeForm()
        {
            SetChangeForm(CDefine.EFormView.FORM_VIEW_SETUP);
            var objSetupForm = m_stForm[(int)m_eCurrentForm].m_objForm as CFormSetup;
            if (null != objSetupForm)
            {
                objSetupForm.SetChangeForm(CDefine.EFormViewSetup.FORM_VIEW_SETUP_INITIALIZE);
            }
        }

        public void SetChangeMainFlowForm()
        {
            SetChangeForm(CDefine.EFormView.FORM_VIEW_MAIN);
            var objMainForm = m_stForm[(int)m_eCurrentForm].m_objForm as CFormMain;
            if (null != objMainForm)
            {
                objMainForm.SetChangeForm(CDefine.EFormViewMain.FORM_VIEW_MAIN_FLOW);
            }
        }

        public void SetChangeStatisticsTactTimeForm()
        {
            SetChangeForm(CDefine.EFormView.FORM_VIEW_STATISTICS);
            var objSetupForm = m_stForm[(int)m_eCurrentForm].m_objForm as CFormStatistics;
            if (null != objSetupForm)
            {
                objSetupForm.SetChangeForm(CDefine.EFormViewStatistics.FORM_VIEW_STATISTICS_TACT_TIME);
            }
        }

        public void SetChangeStatisticsProductionForm()
        {
            SetChangeForm(CDefine.EFormView.FORM_VIEW_STATISTICS);
            var objSetupForm = m_stForm[(int)m_eCurrentForm].m_objForm as CFormStatistics;
            if (null != objSetupForm)
            {
                objSetupForm.SetChangeForm(CDefine.EFormViewStatistics.FORM_VIEW_STATISTICS_PRODUCTION);
            }
        }

        public void SetChangeStatisticsAlarmForm()
        {
            SetChangeForm(CDefine.EFormView.FORM_VIEW_STATISTICS);
            var objSetupForm = m_stForm[(int)m_eCurrentForm].m_objForm as CFormStatistics;
            if (null != objSetupForm)
            {
                objSetupForm.SetChangeForm(CDefine.EFormViewStatistics.FORM_VIEW_STATISTICS_ALARM);
            }
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
                m_stForm[(int)m_eCurrentForm].m_IForm.SetChangeLanguage();

                bReturn = true;
            } while (false);

            return bReturn;
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
        /// 현재 Form View를 리턴
        /// </summary>
        /// <returns></returns>
        public CDefine.EFormView GetCurrentForm()
        {
            return m_eCurrentForm;
        }

        /// <summary>
        /// 종료
        /// </summary>
        public void SetExit()
        {
            // Msg: 프로그램을 종료하시겠습니까?
            if (DialogResult.Yes == m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_EXIT_THE_PROGRAM))
            {
                // 종료 시작시 모든 버튼 비활성화
                m_objDocument.GetMainFrame().Enabled = false;

                // 종료 대기 창 표시
                string targetPath = @".\Utility\WaitForAppTerminate.exe";
                string args = "/target=" + System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                System.Diagnostics.Process.Start(targetPath, args);

                // 공유메모리에서 프로그램 종료 상태 쓰기.
                ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus objMachineStatus = new ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus();
                var objMMFManagerMachineStatus = ENC.MemoryMap.Manager.CMMFManagerMachineStatus.Instance;
                objMMFManagerMachineStatus[0].GetMachineStatus(out objMachineStatus);
                objMachineStatus.m_eProgramExitStatus = CDefine.EProgramExitStatus.PROGRAM_EXIT_STATUS_ON;
                objMMFManagerMachineStatus[0].SetMachineStatus(objMachineStatus);

                // 폼 메인 폼을 닫으면서 관련 부분 해제
                Application.Exit();
                Application.ExitThread();
            }
        }
    }
}