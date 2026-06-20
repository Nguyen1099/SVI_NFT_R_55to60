using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormConfigOption : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 옵션 파라미터
        /// </summary>
        private CConfig.COptionParameter m_objOptionParameter;
        /// <summary>
        /// 크루셜 옵션 파라미터
        /// </summary>
        private CConfig.CCrucialOptionParameter m_objCrucialOptionParameter;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormConfigOption(CDocument objDocument)
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
        private void CFormConfigOption_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormConfigOption_FormClosed(object sender, FormClosedEventArgs e)
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
                m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);

                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                //////////////////////////////////////////////////////////////////////////
                // 버튼 색상 정의
                SetButtonColor();
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            SetButtonBackColor(BtnTitleOptionView, m_colorLabel);
            SetButtonBackColor(BtnTitleOperationOption, m_colorLabelSub);
            SetButtonBackColor(BtnTitleSystemConfig, m_colorLabelSub);
            SetButtonBackColor(BtnTitleRunMode, m_colorLabelSub);
            SetButtonBackColor(BtnRealRun, m_colorNormal);
            SetButtonBackColor(BtnUnlinkDryrun, m_colorNormal);
            SetButtonBackColor(BtnUseBuzzer, m_colorNormal);
            SetButtonBackColor(BtnFunctionList, m_colorNormal);
            SetButtonBackColor(BtnSave, m_colorNormal);
            SetButtonBackColor(BtnLoad, m_colorNormal);

            SetButtonBackColor(BtnTitleResultWaitAlarmCount, m_colorLabelSub);
            SetButtonBackColor(BtnTitleCimConfig, m_colorLabelSub);

            SetButtonBackColor(BtnTitleFanAlarmTime, m_colorLabelSub);
            SetButtonBackColor(BtnResultWaitAlarmCount, m_colorLabelData);
            SetButtonBackColor(BtnFanAlarmTime, m_colorLabelData);

            SetButtonBackColor(BtnTitlePasswordModeSelect, m_colorLabelSub);
            SetButtonBackColor(BtnPasswordModeSelectRm, m_colorNormal);
            SetButtonBackColor(BtnPasswordModeSelectEqp, m_colorClick);

            SetButtonBackColor(BtnTitleMaterialsManagement, m_colorLabelSub);
            SetButtonBackColor(BtnCellInputTimeClear, m_colorNormal);
            SetButtonBackColor(BtnMaterialsManagementTime, m_colorLabelData);

            SetButtonBackColor(BtnTitleLowEnergyMode, m_colorLabelSub);
            SetButtonBackColor(BtnSubTitleLowEnergyModeFfuNormalRpm, m_colorLabelSub);
            SetButtonBackColor(BtnSubTitleLowEnergyModeFfuSlowRpm, m_colorLabelSub);
            SetButtonBackColor(BtnLowEnergyModeFfuNormalRpm, m_colorLabelData);
            SetButtonBackColor(BtnLowEnergyModeFfuSlowRpm, m_colorLabelData);

            SetButtonBackColor(BtnTitleOutRobotTurnCylinderOption, m_colorLabelSub);
            SetButtonBackColor(BtnSubTitleOutRobotTurnCylinderOptionOk, m_colorLabelSub);
            SetButtonBackColor(BtnSubTitleOutRobotTurnCylinderOptionNg, m_colorLabelSub);

            SetButtonBackColor(BtnTitleConveyorOption, m_colorLabelSub);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnResultWaitAlarmCount.Name)
                    && false == btn.Name.Equals(BtnFanAlarmTime.Name)
                    && false == btn.Name.Equals(BtnMaterialsManagementTime.Name)
                    && false == btn.Name.Equals(BtnLowEnergyModeFfuNormalRpm.Name)
                    && false == btn.Name.Equals(BtnLowEnergyModeFfuSlowRpm.Name)
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
                SetControlButtonEnable(Controls, false);
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        SetControlButtonEnable(Controls, false);
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        SetControlButtonEnable(Controls, true);

                        // Run Mode는 Master 권한에서만 수정 가능함
                        BtnRealRun.Enabled = false;
                        BtnUnlinkDryrun.Enabled = false;
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        SetControlButtonEnable(Controls, true);
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
                SetButtonChangeLanguage(BtnTitleOptionView);
                SetButtonChangeLanguage(BtnTitleRunMode);
                SetButtonChangeLanguage(BtnTitleSystemConfig);
                SetButtonChangeLanguage(BtnTitleCimConfig);
                SetButtonChangeLanguage(BtnRealRun);
                SetButtonChangeLanguage(BtnUnlinkDryrun);
                SetButtonChangeLanguage(BtnUseBuzzer);
                SetButtonChangeLanguage(BtnUseCIM);
                SetButtonChangeLanguage(BtnUseBinPrime);
                SetButtonChangeLanguage(BtnFunctionList);

                SetButtonChangeLanguage(BtnTitleOperationOption);

                SetButtonChangeLanguage(BtnTitleResultWaitAlarmCount);

                SetButtonChangeLanguage(BtnTitleFanAlarmTime);

                SetButtonChangeLanguage(BtnTitlePasswordModeSelect);

                SetButtonChangeLanguage(BtnTitleMaterialsManagement);
                SetButtonChangeLanguage(BtnCellInputTimeClear);
                SetButtonChangeLanguage(BtnUseMaterialsManagement);

                SetButtonChangeLanguage(BtnTitleConveyorOption);
                SetButtonChangeLanguage(BtnUseOutConveyorInputCheck);
                SetButtonChangeLanguage(BtnUseOutConveyorOutputCheck);
                SetButtonChangeLanguage(BtnUseOutConveyorBlockedCheck);

                SetButtonChangeLanguage(BtnTitleLowEnergyMode);
                SetButtonChangeLanguage(BtnSubTitleLowEnergyModeFfuNormalRpm);
                SetButtonChangeLanguage(BtnSubTitleLowEnergyModeFfuSlowRpm);
                SetButtonChangeLanguage(BtnUseLowEnergyMode);

                SetButtonChangeLanguage(BtnTitleOutRobotTurnCylinderOption);
                SetButtonChangeLanguage(BtnSubTitleOutRobotTurnCylinderOptionOk);
                SetButtonChangeLanguage(BtnSubTitleOutRobotTurnCylinderOptionNg);
                SetButtonChangeLanguage(BtnOutRobotTurnCylinderOptionOkTurn);
                SetButtonChangeLanguage(BtnOutRobotTurnCylinderOptionOkReturn);
                SetButtonChangeLanguage(BtnOutRobotTurnCylinderOptionNgTurn);
                SetButtonChangeLanguage(BtnOutRobotTurnCylinderOptionNgReturn);

                SetButtonChangeLanguage(BtnSave);
                SetButtonChangeLanguage(BtnLoad);

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
            SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
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
            Visible = bVisible;

            if (true == bVisible)
            {
                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                m_objDocument.GetMainFrame().SetCurrentForm(this);
                // 파라미터 갱신
                m_objOptionParameter = m_objDocument.m_objConfig.GetOptionParameter().DeepClone();
                m_objCrucialOptionParameter = m_objDocument.m_objConfig.GetCrucialOptionParameter().DeepClone();
            }
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // 설비 운용 모드
            if (CDefine.ERunMode.RealRun == m_objDocument.GetRunMode())
            {
                SetButtonBackColor(BtnRealRun, m_colorClick);
                SetButtonBackColor(BtnUnlinkDryrun, m_colorNormal);
            }
            else if (CDefine.ERunMode.UnlinkDryrun == m_objDocument.GetRunMode())
            {
                SetButtonBackColor(BtnRealRun, m_colorNormal);
                SetButtonBackColor(BtnUnlinkDryrun, m_colorClick);
            }
            // 부저 사용
            SetUseColor(BtnUseBuzzer, m_objOptionParameter.bUseBuzzer);
            // CIM  사용
            SetUseColor(BtnUseCIM, m_objOptionParameter.bUseCIM);
            SetUseColor(BtnUseBinPrime, m_objCrucialOptionParameter.strBinPrimeMode == "ON");
            SetUseColor(BtnUseMaterialsManagement, m_objOptionParameter.bUseMaterialsManagement);
            // 데이터 표시
            // Recipe 에서 읽어올 것
            var machineMap = ENC.MemoryMap.Manager.CMMFManagerMachineStatus.Instance;
            // 결과 수신 연속 실패시 중알람 표시
            BtnResultWaitAlarmCount.Text = string.Format("{0:D}", m_objOptionParameter.iResultWaitAlarmCount);
            // Fan Alarm 무시 시간 설정
            BtnFanAlarmTime.Text = string.Format("{0:D} ( min )", m_objOptionParameter.iFanAlarmTime);
            // 자재관리 알람시간
            BtnMaterialsManagementTime.Text = string.Format("{0:D} ( min )", m_objOptionParameter.iMaterialsManagementTime);
            //저전력 모드 EFU
            SetUseColor(BtnUseLowEnergyMode, m_objOptionParameter.UseLowEnergyMode);
            SetControlText(BtnLowEnergyModeFfuNormalRpm, $"{m_objOptionParameter.LowEnergyModeFfuNormalRpm} ( RPM )");
            SetControlText(BtnLowEnergyModeFfuSlowRpm, $"{m_objOptionParameter.LowEnergyModeFfuSlowRpm} ( RPM )");

            // 컨베이어 옵션
            SetUseColor(this.BtnUseOutConveyorInputCheck, m_objOptionParameter.bUseOutConveyorInputCheck);
            SetUseColor(this.BtnUseOutConveyorOutputCheck, m_objOptionParameter.bUseOutConveyorOutputCheck);
            SetUseColor(this.BtnUseOutConveyorBlockedCheck, m_objOptionParameter.bUseOutConveyorBlockedCheck);
            // 배출 실린더 옵션
            SetUseColor(this.BtnOutRobotTurnCylinderOptionOkTurn, m_objOptionParameter.bUseOutRobotCylinderTurnOk);
            SetUseColor(this.BtnOutRobotTurnCylinderOptionOkReturn, !m_objOptionParameter.bUseOutRobotCylinderTurnOk);
            SetUseColor(this.BtnOutRobotTurnCylinderOptionNgTurn, m_objOptionParameter.bUseOutRobotCylinderTurnNg);
            SetUseColor(this.BtnOutRobotTurnCylinderOptionNgReturn, !m_objOptionParameter.bUseOutRobotCylinderTurnNg);
        }

        /// <summary>
        /// Use On Off
        /// </summary>
        /// <param name="objButton"></param>
        /// <param name="bUse"></param>
        private void SetUseColor(ImageButton objButton, bool bUse)
        {
            if (true == bUse)
            {
                SetButtonBackColor(objButton, m_colorClick);
            }
            else
            {
                SetButtonBackColor(objButton, m_colorNormal);
            }
        }

        /// <summary>
        /// REAL RUN 모드 전환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRealRun_Click(object sender, EventArgs e)
        {
            m_objDocument.SetRunMode(CDefine.ERunMode.RealRun);
            m_objDocument.m_objProcessMain.m_objProcessMotion.SetInitializeCommand(CProcessAbstract.EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_NONE);
            m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.AttachRealSignalDevice();
            m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.DryRunSignalClear();
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [RunModeType : {1}]", "BtnRealRun_Click", CDefine.ERunMode.RealRun.ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// DRY RUN 모드 전환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUnlinkDryrun_Click(object sender, EventArgs e)
        {
            // 설비 내부에 셀 정보가 있으면 옵션 변경 불가
            if (
                CCIMDefine.ERunState.RUN_STATE_RUN == m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE]
                && m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode != CDefine.ESimulationMode.SIMULATION_MODE_ON
                )
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.IF_A_CELL_EXISTS_INSIDE_THE_MACHINE_THE_OPTION_CANNOT_BE_CHANGED_REMOVE_THE_CELLS_INSIDE_THE_MACHINE_AND_TRY_AGAIN);
                return;
            }

            m_objDocument.SetRunMode(CDefine.ERunMode.UnlinkDryrun);
            m_objDocument.m_objProcessMain.m_objProcessMotion.SetInitializeCommand(CProcessAbstract.EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_NONE);
            m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.AttachVirtualSignalDevice();
            m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.DryRunSignalClear();
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [RunModeType : {1}]", "BtnUnlinkDryrun_Click", CDefine.ERunMode.UnlinkDryrun.ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 부저 사용 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUseBuzzer_Click(object sender, EventArgs e)
        {
            m_objOptionParameter.bUseBuzzer = !m_objOptionParameter.bUseBuzzer;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [bUseBuzzer : {1}]", "BtnUseBuzzer_Click", m_objOptionParameter.bUseBuzzer);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 씸 사용 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUseCIM_Click(object sender, EventArgs e)
        {
            // 설비 내부에 셀 정보가 있으면 옵션 변경 불가
            if (CCIMDefine.ERunState.RUN_STATE_RUN == m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.IF_A_CELL_EXISTS_INSIDE_THE_MACHINE_THE_OPTION_CANNOT_BE_CHANGED_REMOVE_THE_CELLS_INSIDE_THE_MACHINE_AND_TRY_AGAIN);
                return;
            }

            m_objOptionParameter.bUseCIM = !m_objOptionParameter.bUseCIM;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [bUseCIM : {1}]", "BtnUseCIM_Click", m_objOptionParameter.bUseCIM);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// BinPrime 사용 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUseBinPrime_Click(object sender, EventArgs e)
        {
            // 설비 내부에 셀 정보가 있으면 옵션 변경 불가
            if (CCIMDefine.ERunState.RUN_STATE_RUN == m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.IF_A_CELL_EXISTS_INSIDE_THE_MACHINE_THE_OPTION_CANNOT_BE_CHANGED_REMOVE_THE_CELLS_INSIDE_THE_MACHINE_AND_TRY_AGAIN);
                return;
            }

            m_objCrucialOptionParameter.strBinPrimeMode = m_objCrucialOptionParameter.strBinPrimeMode == "ON" ? "OFF" : "ON";
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [strBinPrimeMode : {1}]", "BtnUseBinPrime_Click", m_objCrucialOptionParameter.strBinPrimeMode);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// SDV 결과 수신 후 그랩 시작 까지 딜레이 시간.
        /// 설명 : SDV 결과 수신 후 바로 그랩 스타트 할 경우 자바스 프로그램이 죽는 현상이 발생 함. 딜레이 시간을 주고 동작.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnResultWaitAlarmCount_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnResultWaitAlarmCount_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (var objKeyPad = new FormKeyPad(Convert.ToDouble(m_objOptionParameter.iResultWaitAlarmCount)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    m_objOptionParameter.iResultWaitAlarmCount = (int)objKeyPad.m_dResultValue;
                    strLog = string.Format("[{0}] [iResultWaitAlarmCount : {1:D}] [{2}]", "BtnResultWaitAlarmCount_Click", m_objOptionParameter.iResultWaitAlarmCount, false);
                }
            }

            // 버튼 로그 추가
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnFanAlarmTime_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnFanAlarmTime_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (var objKeyPad = new FormKeyPad(Convert.ToDouble(m_objOptionParameter.iFanAlarmTime)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    m_objOptionParameter.iFanAlarmTime = (int)objKeyPad.m_dResultValue;
                    strLog = string.Format("[{0}] [iFanAlarmTime : {1:D}] [{2}]", "BtnFanAlarmTime_Click", m_objOptionParameter.iFanAlarmTime, false);
                }
            }
            // 버튼 로그 추가
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// EQP 기능 리스트 다이얼로그
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFunctionList_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnFunctionList_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (var objDailogEQPFunctionList = new CDialogEQPFunctionList(m_objDocument))
            {
                objDailogEQPFunctionList.ShowDialog();
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [{1}]", "BtnFunctionList_Click", false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE))
            {
                m_objOptionParameter = m_objDocument.m_objConfig.GetOptionParameter().DeepClone();
                m_objCrucialOptionParameter = m_objDocument.m_objConfig.GetCrucialOptionParameter().DeepClone();
                return;
            }

            m_objDocument.m_objConfig.SaveOptionParameter(m_objOptionParameter);
            m_objDocument.m_objConfig.SaveCrucialOptionParameter(m_objCrucialOptionParameter);

            // Msg: 저장이 완료되었습니다.
            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.SAVING_IS_COMPLETE);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}]", "BtnSave_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 불러오기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_LOAD))
                return;
            m_objDocument.m_objConfig.LoadOptionParameter();
            m_objDocument.m_objConfig.LoadCrucialOptionParameter();

            m_objOptionParameter = m_objDocument.m_objConfig.GetOptionParameter().DeepClone();
            m_objCrucialOptionParameter = m_objDocument.m_objConfig.GetCrucialOptionParameter().DeepClone();

            // Msg: 불러오기가 완료되었습니다.
            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.LOADING_IS_COMPLETE);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}]", "BtnLoad_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnUseMaterialsManagement_Click(object sender, EventArgs e)
        {
            m_objOptionParameter.bUseMaterialsManagement = !m_objOptionParameter.bUseMaterialsManagement;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [bUseMaterialsManagement : {1}]", "BtnUseMaterialsManagement_Click", m_objOptionParameter.bUseMaterialsManagement);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnCellInputTimeClear_Click(object sender, EventArgs e)
        {
            string strLog = string.Format("[{0}] [{1}]", "BtnCellInputTimeClear_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            string[] option = CellDataManager.Cells.Values.GetExistCellList().Select(i => i.ProcessIndex.ToString()).Concat(new string[] { "All" }).Distinct().ToArray();

            using (FormEnumSelect dialog = new FormEnumSelect(option))
            {
                dialog.BringToFront();
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                m_objDocument.SetUpdateButtonLog(this, $"Select: {dialog.ResultName}");
                if (dialog.ResultName == "All")
                {
                    foreach (var cell in CellDataManager.Cells.Values.GetExistCellList())
                    {
                        cell.Data.Cell.InputTime = DateTime.Now;
                        cell.IsChanged = true;
                    }
                }
                else
                {
                    var selectProcessIndex = (CellData.EProcess)Enum.Parse(typeof(CellData.EProcess), dialog.ResultName);
                    foreach (var cell in CellDataManager.ProcessCells[selectProcessIndex].GetExistCellList())
                    {
                        cell.Data.Cell.InputTime = DateTime.Now;
                        cell.IsChanged = true;
                    }
                }
            }

            strLog = string.Format("[{0}] [{1}]", "BtnCellInputTimeClear_Click", false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnMaterialsManagementTime_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnMaterialsManagementTime_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(Convert.ToDouble(m_objOptionParameter.iMaterialsManagementTime)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    m_objOptionParameter.iMaterialsManagementTime = (int)objKeyPad.m_dResultValue;
                    strLog = string.Format("[{0}] [iMaterialsManagementTime : {1:D}] [{2}]", "BtnMaterialsManagementTime_Click", m_objOptionParameter.iMaterialsManagementTime, false);
                }
            }
            // 버튼 로그 추가
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnUseLowEnergyMode_Click(object sender, EventArgs e)
        {
            m_objOptionParameter.UseLowEnergyMode = !m_objOptionParameter.UseLowEnergyMode;
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{nameof(m_objOptionParameter.UseLowEnergyMode)}: {m_objOptionParameter.UseLowEnergyMode}]");
        }

        private void BtnLowEnergyModeFfuNormalRpm_Click(object sender, EventArgs e)
        {
            using (var dialog = new FormKeyPad(m_objOptionParameter.LowEnergyModeFfuNormalRpm, 0, 1400))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                m_objOptionParameter.LowEnergyModeFfuNormalRpm = (int)dialog.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{nameof(m_objOptionParameter.LowEnergyModeFfuNormalRpm)}: {m_objOptionParameter.LowEnergyModeFfuNormalRpm}]");
            }
        }

        private void BtnLowEnergyModeFfuSlowRpm_Click(object sender, EventArgs e)
        {
            using (var dialog = new FormKeyPad(m_objOptionParameter.LowEnergyModeFfuSlowRpm, 0, 1400))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                m_objOptionParameter.LowEnergyModeFfuSlowRpm = (int)dialog.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{nameof(m_objOptionParameter.LowEnergyModeFfuSlowRpm)}: {m_objOptionParameter.LowEnergyModeFfuSlowRpm}]");
            }
        }

        private void BtnUseOutConveyorInputCheck_Click(object sender, EventArgs e)
        {
            m_objOptionParameter.bUseOutConveyorInputCheck = !m_objOptionParameter.bUseOutConveyorInputCheck;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [bUseOutConveyorInputCheck : {1}]", "BtnUseOutConveyorInputCheck_Click", m_objOptionParameter.bUseOutConveyorInputCheck);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnUseOutConveyorOutputCheck_Click(object sender, EventArgs e)
        {
            m_objOptionParameter.bUseOutConveyorOutputCheck = !m_objOptionParameter.bUseOutConveyorOutputCheck;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [bUseOutConveyorOutputCheck : {1}]", "BtnUseOutConveyorOutputCheck_Click", m_objOptionParameter.bUseOutConveyorOutputCheck);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnUseOutConveyorBlockedCheck_Click(object sender, EventArgs e)
        {
            m_objOptionParameter.bUseOutConveyorBlockedCheck = !m_objOptionParameter.bUseOutConveyorBlockedCheck;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [bUseOutConveyorBlockedCheck : {1}]", "BtnUseOutConveyorBlockedCheck_Click", m_objOptionParameter.bUseOutConveyorBlockedCheck);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnOutRobotTurnCylinderOptionOkReturn_Click(object sender, EventArgs e)
        {
            m_objOptionParameter.bUseOutRobotCylinderTurnOk = false;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [bUseOutRobotCylinderTurnOk : {1}]", "BtnOutRobotTurnCylinderOptionOkReturn_Click", m_objOptionParameter.bUseOutRobotCylinderTurnOk);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnOutRobotTurnCylinderOptionOkTurn_Click(object sender, EventArgs e)
        {
            m_objOptionParameter.bUseOutRobotCylinderTurnOk = true;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [bUseOutRobotCylinderTurnOk : {1}]", "BtnOutRobotTurnCylinderOptionOkTurn_Click", m_objOptionParameter.bUseOutRobotCylinderTurnOk);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnOutRobotTurnCylinderOptionNgReturn_Click(object sender, EventArgs e)
        {
            m_objOptionParameter.bUseOutRobotCylinderTurnNg = false;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [bUseOutRobotCylinderTurnNg : {1}]", "BtnOutRobotTurnCylinderOptionNgReturn_Click", m_objOptionParameter.bUseOutRobotCylinderTurnNg);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnOutRobotTurnCylinderOptionNgTurn_Click(object sender, EventArgs e)
        {
            m_objOptionParameter.bUseOutRobotCylinderTurnNg = true;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [bUseOutRobotCylinderTurnNg : {1}]", "BtnOutRobotTurnCylinderOptionNgTurn_Click", m_objOptionParameter.bUseOutRobotCylinderTurnNg);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }
    }
}