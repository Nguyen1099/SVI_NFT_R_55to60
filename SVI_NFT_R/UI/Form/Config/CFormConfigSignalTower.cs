using System;
using System.Drawing;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormConfigSignalTower : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 해당 폼에서 사용할 컬러 define
        /// </summary>
        public class CSignalTowerColor
        {
            // 상황별 색깔 정의
            public Color objOn = m_colorOn;
            public Color objOff = m_colorOff;
            public Color objBlink = m_colorYellow;
            public Color objDisabled = m_colorClick;
        }
        private CSignalTowerColor m_objSignalTowerColor;
        /// <summary>
        /// 시그널 타워 파라미터
        /// </summary>
        private CConfig.CSignalTowerParameter m_objSignalTowerParameter;
        // 버튼 배열
        private ImageButton[,] m_btnLampValue;
        private ImageButton[] m_btnBuzzerValue;
        private SpeedButton[] m_btnEnableScenarioValue;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormConfigSignalTower(CDocument objDocument)
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
        private void CFormConfigSignalTower_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormConfigSignalTower_FormClosed(object sender, FormClosedEventArgs e)
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
                // 버튼 색상 정의
                SetButtonColor();
                // 버튼 배열로 묶어줌
                m_btnLampValue = new ImageButton[(int)CDefine.ELampSituation.LAMP_SITUATION_FINAL, (int)CDefine.ELampColor.LAMP_COLOR_FINAL];
                m_btnBuzzerValue = new ImageButton[(int)CDefine.ELampSituation.LAMP_SITUATION_FINAL];
                m_btnEnableScenarioValue = new SpeedButton[(int)CDefine.ELampSituation.LAMP_SITUATION_FINAL];

                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY, (int)CDefine.ELampColor.LAMP_COLOR_RED] = this.BtnInitializeRed;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW] = this.BtnInitializeYellow;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY, (int)CDefine.ELampColor.LAMP_COLOR_GREEN] = this.BtnInitializeGreen;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING, (int)CDefine.ELampColor.LAMP_COLOR_RED] = this.BtnRunningRed;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW] = this.BtnRunningYellow;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING, (int)CDefine.ELampColor.LAMP_COLOR_GREEN] = this.BtnRunningGreen;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_IDLE, (int)CDefine.ELampColor.LAMP_COLOR_RED] = this.BtnIdleRed;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_IDLE, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW] = this.BtnIdleYellow;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_IDLE, (int)CDefine.ELampColor.LAMP_COLOR_GREEN] = this.BtnIdleGreen;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STOP, (int)CDefine.ELampColor.LAMP_COLOR_RED] = this.BtnStopRed;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STOP, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW] = this.BtnStopYellow;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STOP, (int)CDefine.ELampColor.LAMP_COLOR_GREEN] = this.BtnStopGreen;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY, (int)CDefine.ELampColor.LAMP_COLOR_RED] = this.BtnRunDelayRed;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW] = this.BtnRunDelayYellow;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY, (int)CDefine.ELampColor.LAMP_COLOR_GREEN] = this.BtnRunDelayGreen;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM, (int)CDefine.ELampColor.LAMP_COLOR_RED] = this.BtnLightAlarmRed;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW] = this.BtnLightAlarmYellow;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM, (int)CDefine.ELampColor.LAMP_COLOR_GREEN] = this.BtnLightAlarmGreen;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_FAULT, (int)CDefine.ELampColor.LAMP_COLOR_RED] = this.BtnFaultRed;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_FAULT, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW] = this.BtnFaultYellow;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_FAULT, (int)CDefine.ELampColor.LAMP_COLOR_GREEN] = this.BtnFaultGreen;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_PM, (int)CDefine.ELampColor.LAMP_COLOR_RED] = this.BtnPMRed;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_PM, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW] = this.BtnPMYellow;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_PM, (int)CDefine.ELampColor.LAMP_COLOR_GREEN] = this.BtnPMGreen;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL, (int)CDefine.ELampColor.LAMP_COLOR_RED] = this.BtnOpCallRed;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW] = this.BtnOpCallYellow;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL, (int)CDefine.ELampColor.LAMP_COLOR_GREEN] = this.BtnOpCallGreen;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK, (int)CDefine.ELampColor.LAMP_COLOR_RED] = this.BtnInterlockRed;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW] = this.BtnInterlockYellow;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK, (int)CDefine.ELampColor.LAMP_COLOR_GREEN] = this.BtnInterlockGreen;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN, (int)CDefine.ELampColor.LAMP_COLOR_RED] = this.BtnShutDownRed;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW] = this.BtnShutDownYellow;
                m_btnLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN, (int)CDefine.ELampColor.LAMP_COLOR_GREEN] = this.BtnShutDownGreen;

                m_btnBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY] = this.BtnInitializeBuzzer;
                m_btnBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING] = this.BtnRunningBuzzer;
                m_btnBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_IDLE] = this.BtnIdleBuzzer;
                m_btnBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STOP] = this.BtnStopBuzzer;
                m_btnBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY] = this.BtnRunDelayBuzzer;
                m_btnBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM] = this.BtnLightAlarmBuzzer;
                m_btnBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_FAULT] = this.BtnFaultBuzzer;
                m_btnBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_PM] = this.BtnPMBuzzer;
                m_btnBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL] = this.BtnOpCallBuzzer;
                m_btnBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK] = this.BtnInterlockBuzzer;
                m_btnBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN] = this.BtnShutDownBuzzer;

                m_btnEnableScenarioValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY] = this.BtnTitleInitialize;
                m_btnEnableScenarioValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING] = this.BtnTitleRunning;
                m_btnEnableScenarioValue[(int)CDefine.ELampSituation.LAMP_SITUATION_IDLE] = this.BtnTitleIdle;
                m_btnEnableScenarioValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STOP] = this.BtnTitleStop;
                m_btnEnableScenarioValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY] = this.BtnTitleRunDelay;
                m_btnEnableScenarioValue[(int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM] = this.BtnTitleLightAlarm;
                m_btnEnableScenarioValue[(int)CDefine.ELampSituation.LAMP_SITUATION_FAULT] = this.BtnTitleFault;
                m_btnEnableScenarioValue[(int)CDefine.ELampSituation.LAMP_SITUATION_PM] = this.BtnTitlePM;
                m_btnEnableScenarioValue[(int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL] = this.BtnTitleOpCall;
                m_btnEnableScenarioValue[(int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK] = this.BtnTitleInterlock;
                m_btnEnableScenarioValue[(int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN] = this.BtnTitleShutDown;

                m_objSignalTowerColor = new CSignalTowerColor();
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
            base.SetButtonBackColor(this.BtnTitleLampSetting, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitle, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleRed, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleYellow, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleGreen, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleBuzzer, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleInitialize, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleRunning, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleIdle, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleStop, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleRunDelay, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleLightAlarm, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleFault, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitlePM, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleOpCall, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleInterlock, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleShutDown, m_colorLabelSub);

            var imageButtons = Controls.GetChildControlListByType(typeof(ImageButton));
            foreach (ImageButton btn in imageButtons)
            {
                base.SetButtonBackColor(btn, m_colorNormal);
            }

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnTitleInitialize.Name)
                    && false == btn.Name.Equals(BtnTitleLightAlarm.Name)
                    && false == btn.Name.Equals(BtnTitleFault.Name)
                    && false == btn.Name.Equals(BtnTitlePM.Name)
                    && false == btn.Name.Equals(BtnTitleOpCall.Name)
                    && false == btn.Name.Equals(BtnTitleInterlock.Name)
                    && false == btn.Name.Equals(BtnTitleRunDelay.Name)
                    && false == btn.Name.Equals(BtnTitleShutDown.Name)
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
                SetButtonChangeLanguage(this.BtnTitleLampSetting);
                SetButtonChangeLanguage(this.BtnTitleInitialize);
                SetButtonChangeLanguage(this.BtnTitleRunning);
                SetButtonChangeLanguage(this.BtnTitleIdle);
                SetButtonChangeLanguage(this.BtnTitleStop);
                SetButtonChangeLanguage(this.BtnTitleRunDelay);
                SetButtonChangeLanguage(this.BtnTitleLightAlarm);
                SetButtonChangeLanguage(this.BtnTitleFault);
                SetButtonChangeLanguage(this.BtnTitlePM);
                SetButtonChangeLanguage(this.BtnTitleOpCall);
                SetButtonChangeLanguage(this.BtnTitleInterlock);
                SetButtonChangeLanguage(this.BtnTitleShutDown);
                SetButtonChangeLanguage(this.BtnTitleRed);
                SetButtonChangeLanguage(this.BtnTitleYellow);
                SetButtonChangeLanguage(this.BtnTitleGreen);
                SetButtonChangeLanguage(this.BtnTitleBuzzer);
                SetButtonChangeLanguage(this.BtnSave);
                SetButtonChangeLanguage(this.BtnLoad);

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
                // 시그널 타워 파라미터 갱신
                m_objSignalTowerParameter = m_objDocument.m_objConfig.GetSignalTowerParameter();
            }
        }

        /// <summary>
        /// 램프 값 봐서 알맞는 색상으로 변경
        /// </summary>
        /// <param name="iLoopSituation"></param>
        /// <param name="iLoopColor"></param>
        /// <param name="eValue"></param>
        private void SetLampValueColor(int iLoopSituation, int iLoopColor, CDefine.ELampValue eValue, bool bEnabled)
        {
            if (CDefine.ELampValue.LAMP_VALUE_ON == eValue)
            {
                base.SetButtonBackColor(m_btnLampValue[iLoopSituation, iLoopColor], bEnabled ? m_objSignalTowerColor.objOn : m_objSignalTowerColor.objDisabled);
                base.SetButtonText(m_btnLampValue[iLoopSituation, iLoopColor], "ON");
            }
            else if (CDefine.ELampValue.LAMP_VALUE_OFF == eValue)
            {
                base.SetButtonBackColor(m_btnLampValue[iLoopSituation, iLoopColor], bEnabled ? m_objSignalTowerColor.objOff : m_objSignalTowerColor.objDisabled);
                base.SetButtonText(m_btnLampValue[iLoopSituation, iLoopColor], "OFF");
            }
            else if (CDefine.ELampValue.LAMP_VALUE_BLINK == eValue)
            {
                base.SetButtonBackColor(m_btnLampValue[iLoopSituation, iLoopColor], bEnabled ? m_objSignalTowerColor.objBlink : m_objSignalTowerColor.objDisabled);
                base.SetButtonText(m_btnLampValue[iLoopSituation, iLoopColor], "BLINK");
            }
        }

        /// <summary>
        /// 램프 값 봐서 알맞는 색상으로 변경
        /// </summary>
        private void SetLampValueColor()
        {
            for (int iLoopSituation = 0; iLoopSituation < (int)CDefine.ELampSituation.LAMP_SITUATION_FINAL; iLoopSituation++)
            {
                for (int iLoopColor = 0; iLoopColor < (int)CDefine.ELampColor.LAMP_COLOR_FINAL; iLoopColor++)
                {
                    SetLampValueColor(iLoopSituation, iLoopColor, m_objSignalTowerParameter.eLampValue[iLoopSituation, iLoopColor], m_objSignalTowerParameter.bEnableScenario[iLoopSituation]);
                }
            }
        }

        /// <summary>
        /// 부저 값 봐서 알맞는 색상으로 변경
        /// </summary>
        /// <param name="iLoopSituation"></param>
        /// <param name="eValue"></param>
        private void SetBuzzerValueColor(int iLoopSituation, CDefine.EBuzzerValue eValue)
        {
            if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == eValue)
            {
                base.SetButtonBackColor(m_btnBuzzerValue[iLoopSituation], m_objSignalTowerColor.objOn);
                base.SetButtonText(m_btnBuzzerValue[iLoopSituation], "ON");
            }
            else if (CDefine.EBuzzerValue.BUZZER_VALUE_OFF == eValue)
            {
                base.SetButtonBackColor(m_btnBuzzerValue[iLoopSituation], m_objSignalTowerColor.objOff);
                base.SetButtonText(m_btnBuzzerValue[iLoopSituation], "OFF");
            }
        }

        /// <summary>
        /// 부저 값 봐서 알맞는 색상으로 변경
        /// </summary>
        private void SetBuzzerValueColor()
        {
            for (int iLoopSituation = 0; iLoopSituation < (int)CDefine.ELampSituation.LAMP_SITUATION_FINAL; iLoopSituation++)
            {
                SetBuzzerValueColor(iLoopSituation, m_objSignalTowerParameter.eBuzzerValue[iLoopSituation]);
            }
        }

        /// <summary>
        /// 시나리오 사용 값 봐서 알맞는 색상으로 변경
        /// </summary>
        /// <param name="iLoopSituation"></param>
        /// <param name="eValue"></param>
        private void SetEnableScenarioValueColor(int iLoopSituation, bool value)
        {
            if (true == value)
            {
                base.SetButtonBackColor(m_btnEnableScenarioValue[iLoopSituation], m_colorLabelSub);
            }
            else
            {
                base.SetButtonBackColor(m_btnEnableScenarioValue[iLoopSituation], m_colorClick);
            }
        }

        /// <summary>
        /// 시나리오 사용 값 봐서 알맞는 색상으로 변경
        /// </summary>
        private void SetEnableScenarioValueColor()
        {
            for (int iLoopSituation = 0; iLoopSituation < (int)CDefine.ELampSituation.LAMP_SITUATION_FINAL; iLoopSituation++)
            {
                SetEnableScenarioValueColor(iLoopSituation, m_objSignalTowerParameter.bEnableScenario[iLoopSituation]);
            }
        }

        /// <summary>
        /// 클릭 시 값 토글
        /// </summary>
        /// <param name="iLampSituation"></param>
        /// <param name="iLampColor"></param>
        private void SetLampValueToggle(int iLampSituation, int iLampColor)
        {
            int iValue = (int)m_objSignalTowerParameter.eLampValue[iLampSituation, iLampColor];
            // ON, OFF, BLINK 루프
            iValue = (iValue + 1) % (int)CDefine.ELampValue.LAMP_VALUE_FINAL;
            m_objSignalTowerParameter.eLampValue[iLampSituation, iLampColor] = (CDefine.ELampValue)iValue;
        }

        /// <summary>
        /// 클릭 시 값 토글
        /// </summary>
        /// <param name="iBuzzerSituation"></param>
        private void SetBuzzerValueToggle(int iBuzzerSituation)
        {
            int iValue = (int)m_objSignalTowerParameter.eBuzzerValue[iBuzzerSituation];
            // ON, OFF 루프
            iValue = (iValue + 1) % (int)CDefine.EBuzzerValue.BUZZER_VALUE_FINAL;
            m_objSignalTowerParameter.eBuzzerValue[iBuzzerSituation] = (CDefine.EBuzzerValue)iValue;
        }

        /// <summary>
        /// 클릭 시 값 토글
        /// </summary>
        /// <param name="value"></param>
        private void SetEnableScenarioValueToggle(int iSituation)
        {
            m_objSignalTowerParameter.bEnableScenario[iSituation] = !m_objSignalTowerParameter.bEnableScenario[iSituation];
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // 시나리오 사용 값 봐서 알맞는 색상으로 변경
            SetEnableScenarioValueColor();
            // 램프 값 봐서 알맞는 색상으로 변경
            SetLampValueColor();
            // 부저 값 봐서 알맞는 색상으로 변경
            SetBuzzerValueColor();
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE))
                return;
            // 해당 UI에서 만든 클래스를 도큐먼트 데이터 클래스에 집어넣어줌
            // 파일 세이브
            m_objDocument.m_objConfig.SaveSignalTowerParameter(m_objSignalTowerParameter);
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
            // 파일 로드
            // 도큐먼트 데이터 클래스에 있는 걸 끌어옴
            m_objSignalTowerParameter = m_objDocument.m_objConfig.GetSignalTowerParameter();
            // Msg: 불러오기가 완료되었습니다.
            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.LOADING_IS_COMPLETE);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}]", "BtnLoad_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 초기화 레드 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInitializeRed_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY, (int)CDefine.ELampColor.LAMP_COLOR_RED);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnInitializeRed_Click", CDefine.ELampSituation.LAMP_SITUATION_STAND_BY.ToString(), CDefine.ELampColor.LAMP_COLOR_RED.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY, (int)CDefine.ELampColor.LAMP_COLOR_RED].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 초기화 엘로우 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInitializeYellow_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnInitializeYellow_Click", CDefine.ELampSituation.LAMP_SITUATION_STAND_BY.ToString(), CDefine.ELampColor.LAMP_COLOR_YELLOW.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 초기화 그린 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInitializeGreen_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY, (int)CDefine.ELampColor.LAMP_COLOR_GREEN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnInitializeGreen_Click", CDefine.ELampSituation.LAMP_SITUATION_STAND_BY.ToString(), CDefine.ELampColor.LAMP_COLOR_GREEN.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY, (int)CDefine.ELampColor.LAMP_COLOR_GREEN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 부저 초기화 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInitializeBuzzer_Click(object sender, EventArgs e)
        {
            SetBuzzerValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnInitializeBuzzer_Click", CDefine.ELampSituation.LAMP_SITUATION_STAND_BY.ToString(), m_objSignalTowerParameter.eBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 런 레드 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunningRed_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING, (int)CDefine.ELampColor.LAMP_COLOR_RED);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnRunningRed_Click", CDefine.ELampSituation.LAMP_SITUATION_RUNNING.ToString(), CDefine.ELampColor.LAMP_COLOR_RED.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING, (int)CDefine.ELampColor.LAMP_COLOR_RED].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 런 엘로우 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunningYellow_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnRunningYellow_Click", CDefine.ELampSituation.LAMP_SITUATION_RUNNING.ToString(), CDefine.ELampColor.LAMP_COLOR_YELLOW.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 런 그린 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunningGreen_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING, (int)CDefine.ELampColor.LAMP_COLOR_GREEN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnRunningGreen_Click", CDefine.ELampSituation.LAMP_SITUATION_RUNNING.ToString(), CDefine.ELampColor.LAMP_COLOR_GREEN.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING, (int)CDefine.ELampColor.LAMP_COLOR_GREEN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 부저 런 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunningBuzzer_Click(object sender, EventArgs e)
        {
            SetBuzzerValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnRunningBuzzer_Click", CDefine.ELampSituation.LAMP_SITUATION_RUNNING.ToString(), m_objSignalTowerParameter.eBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 IDLE 레드 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnIdleRed_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_IDLE, (int)CDefine.ELampColor.LAMP_COLOR_RED);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnIdleRed_Click", CDefine.ELampSituation.LAMP_SITUATION_IDLE.ToString(), CDefine.ELampColor.LAMP_COLOR_RED.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_IDLE, (int)CDefine.ELampColor.LAMP_COLOR_RED].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 IDLE 엘로우 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnIdleYellow_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_IDLE, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnIdleYellow_Click", CDefine.ELampSituation.LAMP_SITUATION_IDLE.ToString(), CDefine.ELampColor.LAMP_COLOR_YELLOW.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_IDLE, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 IDLE 그린 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnIdleGreen_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_IDLE, (int)CDefine.ELampColor.LAMP_COLOR_GREEN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnIdleGreen_Click", CDefine.ELampSituation.LAMP_SITUATION_IDLE.ToString(), CDefine.ELampColor.LAMP_COLOR_GREEN.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_IDLE, (int)CDefine.ELampColor.LAMP_COLOR_GREEN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 부저 IDLE ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnIdleBuzzer_Click(object sender, EventArgs e)
        {
            SetBuzzerValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_IDLE);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnIdleBuzzer_Click", CDefine.ELampSituation.LAMP_SITUATION_IDLE.ToString(), m_objSignalTowerParameter.eBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_IDLE].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 정지 레드 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStopRed_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_STOP, (int)CDefine.ELampColor.LAMP_COLOR_RED);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnStopRed_Click", CDefine.ELampSituation.LAMP_SITUATION_STOP.ToString(), CDefine.ELampColor.LAMP_COLOR_RED.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STOP, (int)CDefine.ELampColor.LAMP_COLOR_RED].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 정지 엘로우 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStopYellow_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_STOP, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnStopYellow_Click", CDefine.ELampSituation.LAMP_SITUATION_STOP.ToString(), CDefine.ELampColor.LAMP_COLOR_YELLOW.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STOP, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 정지 그린 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStopGreen_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_STOP, (int)CDefine.ELampColor.LAMP_COLOR_GREEN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnStopGreen_Click", CDefine.ELampSituation.LAMP_SITUATION_STOP.ToString(), CDefine.ELampColor.LAMP_COLOR_GREEN.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STOP, (int)CDefine.ELampColor.LAMP_COLOR_GREEN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 부저 정지 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStopBuzzer_Click(object sender, EventArgs e)
        {
            SetBuzzerValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_STOP);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnStopBuzzer_Click", CDefine.ELampSituation.LAMP_SITUATION_STOP.ToString(), m_objSignalTowerParameter.eBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_STOP].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 운전 딜레이 레드 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunDelayRed_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY, (int)CDefine.ELampColor.LAMP_COLOR_RED);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnRunDelayRed_Click", CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY.ToString(), CDefine.ELampColor.LAMP_COLOR_RED.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY, (int)CDefine.ELampColor.LAMP_COLOR_RED].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 운전 딜레이 엘로우 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunDelayYellow_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnRunDelayYellow_Click", CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY.ToString(), CDefine.ELampColor.LAMP_COLOR_YELLOW.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 운전 딜레이 그린 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunDelayGreen_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY, (int)CDefine.ELampColor.LAMP_COLOR_GREEN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnRunDelayGreen_Click", CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY.ToString(), CDefine.ELampColor.LAMP_COLOR_GREEN.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY, (int)CDefine.ELampColor.LAMP_COLOR_GREEN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 부저 운전 딜레이 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunDelayBuzzer_Click(object sender, EventArgs e)
        {
            SetBuzzerValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnRunDelayBuzzer_Click", CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY.ToString(), m_objSignalTowerParameter.eBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 FAULT 레드 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFaultRed_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_FAULT, (int)CDefine.ELampColor.LAMP_COLOR_RED);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnFaultRed_Click", CDefine.ELampSituation.LAMP_SITUATION_FAULT.ToString(), CDefine.ELampColor.LAMP_COLOR_RED.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_FAULT, (int)CDefine.ELampColor.LAMP_COLOR_RED].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 FAULT 엘로우 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFaultYellow_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_FAULT, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnFaultYellow_Click", CDefine.ELampSituation.LAMP_SITUATION_FAULT.ToString(), CDefine.ELampColor.LAMP_COLOR_YELLOW.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_FAULT, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 FAULT 그린 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFaultGreen_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_FAULT, (int)CDefine.ELampColor.LAMP_COLOR_GREEN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnFaultGreen_Click", CDefine.ELampSituation.LAMP_SITUATION_FAULT.ToString(), CDefine.ELampColor.LAMP_COLOR_GREEN.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_FAULT, (int)CDefine.ELampColor.LAMP_COLOR_GREEN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 부저 FAULT ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFaultBuzzer_Click(object sender, EventArgs e)
        {
            SetBuzzerValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_FAULT);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnFaultBuzzer_Click", CDefine.ELampSituation.LAMP_SITUATION_FAULT.ToString(), m_objSignalTowerParameter.eBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_FAULT].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 PM 레드 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPMRed_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_PM, (int)CDefine.ELampColor.LAMP_COLOR_RED);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnPMRed_Click", CDefine.ELampSituation.LAMP_SITUATION_PM.ToString(), CDefine.ELampColor.LAMP_COLOR_RED.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_PM, (int)CDefine.ELampColor.LAMP_COLOR_RED].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 PM 엘로우 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPMYellow_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_PM, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnPMYellow_Click", CDefine.ELampSituation.LAMP_SITUATION_PM.ToString(), CDefine.ELampColor.LAMP_COLOR_YELLOW.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_PM, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 PM 그린 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPMGreen_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_PM, (int)CDefine.ELampColor.LAMP_COLOR_GREEN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnPMGreen_Click", CDefine.ELampSituation.LAMP_SITUATION_PM.ToString(), CDefine.ELampColor.LAMP_COLOR_GREEN.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_PM, (int)CDefine.ELampColor.LAMP_COLOR_GREEN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 부저 PM ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPMBuzzer_Click(object sender, EventArgs e)
        {
            SetBuzzerValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_PM);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnPMBuzzer_Click", CDefine.ELampSituation.LAMP_SITUATION_PM.ToString(), m_objSignalTowerParameter.eBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_PM].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 OP CALL 레드 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpCallRed_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL, (int)CDefine.ELampColor.LAMP_COLOR_RED);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnOpCallRed_Click", CDefine.ELampSituation.LAMP_SITUATION_OP_CALL.ToString(), CDefine.ELampColor.LAMP_COLOR_RED.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL, (int)CDefine.ELampColor.LAMP_COLOR_RED].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 OP CALL 엘로우 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpCallYellow_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnOpCallYellow_Click", CDefine.ELampSituation.LAMP_SITUATION_OP_CALL.ToString(), CDefine.ELampColor.LAMP_COLOR_YELLOW.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 OP CALL 그린 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpCallGreen_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL, (int)CDefine.ELampColor.LAMP_COLOR_GREEN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnOpCallGreen_Click", CDefine.ELampSituation.LAMP_SITUATION_OP_CALL.ToString(), CDefine.ELampColor.LAMP_COLOR_GREEN.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL, (int)CDefine.ELampColor.LAMP_COLOR_GREEN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 부저 OP CALL ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpCallBuzzer_Click(object sender, EventArgs e)
        {
            SetBuzzerValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnOpCallBuzzer_Click", CDefine.ELampSituation.LAMP_SITUATION_OP_CALL.ToString(), m_objSignalTowerParameter.eBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 INTERLOCK CALL 레드 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInterlockRed_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK, (int)CDefine.ELampColor.LAMP_COLOR_RED);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnInterlockRed_Click", CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK.ToString(), CDefine.ELampColor.LAMP_COLOR_RED.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK, (int)CDefine.ELampColor.LAMP_COLOR_RED].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 INTERLOCK CALL 엘로우 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInterlockYellow_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnInterlockYellow_Click", CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK.ToString(), CDefine.ELampColor.LAMP_COLOR_YELLOW.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 INTERLOCK CALL 그린 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInterlockGreen_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK, (int)CDefine.ELampColor.LAMP_COLOR_GREEN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnInterlockGreen_Click", CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK.ToString(), CDefine.ELampColor.LAMP_COLOR_GREEN.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK, (int)CDefine.ELampColor.LAMP_COLOR_GREEN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 부저 INTERLOCK CALL ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInterlockBuzzer_Click(object sender, EventArgs e)
        {
            SetBuzzerValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnInterlockBuzzer_Click", CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK.ToString(), m_objSignalTowerParameter.eBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 SHUT DOWN 레드 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShutDownRed_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN, (int)CDefine.ELampColor.LAMP_COLOR_RED);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnShutDownRed_Click", CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN.ToString(), CDefine.ELampColor.LAMP_COLOR_RED.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN, (int)CDefine.ELampColor.LAMP_COLOR_RED].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 SHUT DOWN 엘로우 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShutDownYellow_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnShutDownYellow_Click", CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN.ToString(), CDefine.ELampColor.LAMP_COLOR_YELLOW.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 램프 SHUT DOWN 그린 ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShutDownGreen_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN, (int)CDefine.ELampColor.LAMP_COLOR_GREEN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnShutDownGreen_Click", CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN.ToString(), CDefine.ELampColor.LAMP_COLOR_GREEN.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN, (int)CDefine.ELampColor.LAMP_COLOR_GREEN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 부저 SHUT DOWN ON OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShutDownBuzzer_Click(object sender, EventArgs e)
        {
            SetBuzzerValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnShutDownBuzzer_Click", CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN.ToString(), m_objSignalTowerParameter.eBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnLightAlarmRed_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM, (int)CDefine.ELampColor.LAMP_COLOR_RED);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnLightAlarmRed_Click", CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM.ToString(), CDefine.ELampColor.LAMP_COLOR_RED.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM, (int)CDefine.ELampColor.LAMP_COLOR_RED].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnLightAlarmYellow_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnLightAlarmYellow_Click", CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM.ToString(), CDefine.ELampColor.LAMP_COLOR_YELLOW.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM, (int)CDefine.ELampColor.LAMP_COLOR_YELLOW].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnLightAlarmGreen_Click(object sender, EventArgs e)
        {
            SetLampValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM, (int)CDefine.ELampColor.LAMP_COLOR_GREEN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}] [{3}]", "BtnLightAlarmGreen_Click", CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM.ToString(), CDefine.ELampColor.LAMP_COLOR_GREEN.ToString(), m_objSignalTowerParameter.eLampValue[(int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM, (int)CDefine.ELampColor.LAMP_COLOR_GREEN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnLightAlarmBuzzer_Click(object sender, EventArgs e)
        {
            SetBuzzerValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnLightAlarmBuzzer_Click", CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM.ToString(), m_objSignalTowerParameter.eBuzzerValue[(int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnTitleInitialize_Click(object sender, EventArgs e)
        {
            SetEnableScenarioValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnTitleInitialize_Click", CDefine.ELampSituation.LAMP_SITUATION_STAND_BY.ToString(), m_objSignalTowerParameter.bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_STAND_BY].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnTitleRunning_Click(object sender, EventArgs e)
        {
            //SetEnableScenarioValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING);
            //// 버튼 로그 추가
            //string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnTitleRunning_Click", CDefine.ELampSituation.LAMP_SITUATION_RUNNING.ToString(), m_objSignalTowerParameter.bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_RUNNING].ToString());
            //m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnTitleIdle_Click(object sender, EventArgs e)
        {
            //SetEnableScenarioValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_IDLE);
            //// 버튼 로그 추가
            //string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnTitleIdle_Click", CDefine.ELampSituation.LAMP_SITUATION_IDLE.ToString(), m_objSignalTowerParameter.bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_IDLE].ToString());
            //m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnTitleStop_Click(object sender, EventArgs e)
        {
            //SetEnableScenarioValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_STOP);
            //// 버튼 로그 추가
            //string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnTitleStop_Click", CDefine.ELampSituation.LAMP_SITUATION_STOP.ToString(), m_objSignalTowerParameter.bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_STOP].ToString());
            //m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnTitleLightAlarm_Click(object sender, EventArgs e)
        {
            SetEnableScenarioValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnTitleLightAlarm_Click", CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM.ToString(), m_objSignalTowerParameter.bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_LIGHT_ALARM].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnTitleFault_Click(object sender, EventArgs e)
        {
            SetEnableScenarioValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_FAULT);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnTitleFault_Click", CDefine.ELampSituation.LAMP_SITUATION_FAULT.ToString(), m_objSignalTowerParameter.bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_FAULT].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnTitlePM_Click(object sender, EventArgs e)
        {
            SetEnableScenarioValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_PM);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnTitlePM_Click", CDefine.ELampSituation.LAMP_SITUATION_PM.ToString(), m_objSignalTowerParameter.bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_PM].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnTitleOpCall_Click(object sender, EventArgs e)
        {
            SetEnableScenarioValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnTitleOpCall_Click", CDefine.ELampSituation.LAMP_SITUATION_OP_CALL.ToString(), m_objSignalTowerParameter.bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_OP_CALL].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnTitleInterlock_Click(object sender, EventArgs e)
        {
            SetEnableScenarioValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnTitleInterlock_Click", CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK.ToString(), m_objSignalTowerParameter.bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_INTERLOCK].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnTitleRunDelay_Click(object sender, EventArgs e)
        {
            SetEnableScenarioValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnTitleRunDelay_Click", CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY.ToString(), m_objSignalTowerParameter.bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_RUN_DELAY].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnTitleShutDown_Click(object sender, EventArgs e)
        {
            SetEnableScenarioValueToggle((int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}] [{2}]", "BtnTitleShutDown_Click", CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN.ToString(), m_objSignalTowerParameter.bEnableScenario[(int)CDefine.ELampSituation.LAMP_SITUATION_SHUT_DOWN].ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }
    }
}