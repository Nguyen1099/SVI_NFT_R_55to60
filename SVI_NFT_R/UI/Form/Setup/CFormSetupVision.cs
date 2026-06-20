using HLDevice.Abstract;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormSetupVision : CFormCommon, CFormInterface
    {
        /// <summary>도큐먼트</summary>
        private CDocument m_objDocument;
        private CConfig.CInspOptionParameter[] m_objInspOptionParameter = new CConfig.CInspOptionParameter[Enum.GetNames(typeof(CDefine.EInspectType)).Length];
        private CDeviceMotorAbstract.CMotorPosition mInspStageMotorPosition;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormSetupVision(CDocument objDocument)
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
        private void CFormSetupVision_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormSetupVision_FormClosed(object sender, FormClosedEventArgs e)
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
                if (null == m_objDocument)
                {
                    break;
                }
                // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
                base.m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
                // 데이터 업데이트
                UpdateData();
                // 버튼 색상 정의
                SetButtonColor();

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 데이터 업데이트
        /// </summary>
        public void UpdateData()
        {
            // 설정 파라미터 로드
            m_objInspOptionParameter[(int)CDefine.EInspectType.Main] = m_objDocument.m_objConfig.GetInspOptionParameter(CDefine.EInspectType.Main).DeepClone();
            // 모터 파라미터 로드
            mInspStageMotorPosition = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle.MotorStageX.Axis.HLGetMotorPosition().DeepClone();
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            // 버튼 색 변경
            SetButtonBackColor(BtnTitle, m_colorLabel);
            SetButtonBackColor(BtnTitleVision, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVisionParameter, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVisionVisionMode, m_colorLabelSub);

            SetButtonBackColor(BtnSave, m_colorNormal);

            SetButtonBackColor(BtnTitlePulseWidthTime, m_colorLabelSub);
            SetButtonBackColor(BtnTitlePulsePeriodLength, m_colorLabelSub);
            SetButtonBackColor(BtnTitleInspectionVelocity, m_colorLabelSub);
            SetButtonBackColor(BtnTitleTriggerStartPosition, m_colorLabelSub);
            SetButtonBackColor(BtnTitleTriggerEndPosition, m_colorLabelSub);
            SetButtonBackColor(BtnPulseWidthTime, m_colorLabelData);
            SetButtonBackColor(BtnPulsePeriodLength, m_colorLabelData);
            SetButtonBackColor(BtnInspectionVelocity, m_colorLabelData);
            SetButtonBackColor(BtnTriggerStartPosition, m_colorLabelData);
            SetButtonBackColor(BtnTriggerEndPosition, m_colorLabelData);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnPulseWidthTime.Name)
                    && false == btn.Name.Equals(BtnPulsePeriodLength.Name)
                    && false == btn.Name.Equals(BtnInspectionVelocity.Name)
                    && false == btn.Name.Equals(BtnTriggerStartPosition.Name)
                    && false == btn.Name.Equals(BtnTriggerEndPosition.Name)
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
            bool bEnabled = false;
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                bEnabled = false;
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        bEnabled = false;
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        bEnabled = true;
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        bEnabled = true;
                        break;
                    default:
                        break;
                }
            }
            base.SetControlButtonEnable(this.Controls, bEnabled);
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
                SetButtonChangeLanguage(BtnTitle);
                SetButtonChangeLanguage(BtnTitleVision);
                SetButtonChangeLanguage(BtnTitleVisionParameter);
                SetButtonChangeLanguage(BtnTitlePulseWidthTime);
                SetButtonChangeLanguage(BtnTitlePulsePeriodLength);
                SetButtonChangeLanguage(BtnTitleInspectionVelocity);
                SetButtonChangeLanguage(BtnTitleTriggerStartPosition);
                SetButtonChangeLanguage(BtnTitleTriggerEndPosition);
                SetButtonChangeLanguage(BtnTitleVisionVisionMode);
                SetButtonChangeLanguage(BtnVisionVisionNormalMode);
                SetButtonChangeLanguage(BtnVisionVisionBypassMode);
                SetButtonChangeLanguage(BtnVisionVisionErrorSkipMode);
                SetButtonChangeLanguage(BtnVisionVisionVirtualMode);

                SetButtonChangeLanguage(BtnSave);
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
                // 업데이트 데이터
                UpdateData();
            }
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (true == PnlBaseAoiVision.Visible)
            {
                SetButtonText(BtnPulseWidthTime, $"{m_objInspOptionParameter[(int)CDefine.EInspectType.Main].dPulseWidth:0} ( ㎲ )");
                SetButtonText(BtnPulsePeriodLength, $"{m_objInspOptionParameter[(int)CDefine.EInspectType.Main].dPeriod:0} ( ㎛ )");
                SetButtonText(BtnInspectionVelocity, $"{mInspStageMotorPosition.dVelocity[(int)InShuttleMotorX.EPosition.ScanTriggerEnd]:0.000} ( {CDefine.UNIT_MILLIMETER}/s )");
                SetButtonText(BtnTriggerStartPosition, $"{mInspStageMotorPosition.dPosition[(int)InShuttleMotorX.EPosition.ScanTriggerStart]:0.000} ( {CDefine.UNIT_MILLIMETER} )");
                SetButtonText(BtnTriggerEndPosition, $"{mInspStageMotorPosition.dPosition[(int)InShuttleMotorX.EPosition.ScanTriggerEnd]:0.000} ( {CDefine.UNIT_MILLIMETER} )");

                // 검사 모드 표시
                SetButtonBackColor(BtnVisionVisionNormalMode, m_objInspOptionParameter[(int)CDefine.EInspectType.Main].eInspectionMode == CConfig.CInspOptionParameter.EInspectionMode.INSPECT_USE ? m_colorClick : m_colorNormal);
                SetButtonBackColor(BtnVisionVisionVirtualMode, m_objInspOptionParameter[(int)CDefine.EInspectType.Main].eInspectionMode == CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE ? m_colorClick : m_colorNormal);
                SetButtonBackColor(BtnVisionVisionBypassMode, m_objInspOptionParameter[(int)CDefine.EInspectType.Main].eInspectionMode == CConfig.CInspOptionParameter.EInspectionMode.INSPECT_BY_PASS ? m_colorClick : m_colorNormal);
                SetButtonBackColor(BtnVisionVisionErrorSkipMode, m_objInspOptionParameter[(int)CDefine.EInspectType.Main].eInspectionMode == CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP ? m_colorClick : m_colorNormal);
            }
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE))
                return;
            m_objDocument.m_objConfig.SaveInspOptionParameter(m_objInspOptionParameter[(int)CDefine.EInspectType.Main], CDefine.EInspectType.Main);
            m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle.MotorStageX.Axis.HLSaveMotorPositionParameter("", "", mInspStageMotorPosition);
            UpdateData();
            // Msg: 저장이 완료되었습니다.
            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.SAVING_IS_COMPLETE);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}]", "BtnSave_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }


        /// <summary>
        /// 검사기 검사 모드 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnVisionMode_Click(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            string logMessage;
            if (buttonName.Contains("NormalMode") == true)
            {
                m_objInspOptionParameter[(int)CDefine.EInspectType.Main].eInspectionMode = CConfig.CInspOptionParameter.EInspectionMode.INSPECT_USE;
            }
            else if (buttonName.Contains("VirtualMode") == true)
            {
                m_objInspOptionParameter[(int)CDefine.EInspectType.Main].eInspectionMode = CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE;
            }
            else if (buttonName.Contains("BypassMode") == true)
            {
                m_objInspOptionParameter[(int)CDefine.EInspectType.Main].eInspectionMode = CConfig.CInspOptionParameter.EInspectionMode.INSPECT_BY_PASS;
            }
            else if (buttonName.Contains("ErrorSkipMode") == true)
            {
                m_objInspOptionParameter[(int)CDefine.EInspectType.Main].eInspectionMode = CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP;
            }
            else
            {
                Debug.Assert(false);
                return;
            }
            logMessage = $"{buttonName} [eInspectionMode : {m_objInspOptionParameter[(int)CDefine.EInspectType.Main].eInspectionMode}]";
            m_objDocument.SetUpdateButtonLog(this, logMessage);
        }

        private void BtnPulseWidthTime_Click(object sender, EventArgs e)
        {
            var parameter = m_objInspOptionParameter[(int)CDefine.EInspectType.Main];
            double originValue = parameter.dPulseWidth;
            using (var keypad = new FormKeyPad(originValue))
            {
                if (keypad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                parameter.dPulseWidth = keypad.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{nameof(parameter.dPulseWidth)}: {originValue} -> {parameter.dPulseWidth}]");
            }
        }

        private void BtnPulsePeriodLength_Click(object sender, EventArgs e)
        {
            var parameter = m_objInspOptionParameter[(int)CDefine.EInspectType.Main];
            double originValue = parameter.dPeriod;
            using (var keypad = new FormKeyPad(originValue))
            {
                if (keypad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                parameter.dPeriod = keypad.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{nameof(parameter.dPeriod)}: {originValue} -> {parameter.dPeriod}]");
            }
        }

        private void BtnInspectionVelocity_Click(object sender, EventArgs e)
        {
            int positionIndex = (int)InShuttleMotorX.EPosition.ScanTriggerEnd;
            double originValue = mInspStageMotorPosition.dVelocity[positionIndex];
            using (var keypad = new FormKeyPad(originValue))
            {
                if (keypad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                mInspStageMotorPosition.dVelocity[positionIndex] = keypad.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{nameof(mInspStageMotorPosition.dVelocity)}({positionIndex}): {originValue} -> {mInspStageMotorPosition.dVelocity[positionIndex]}]");
            }
        }

        private void BtnTriggerStartPosition_Click(object sender, EventArgs e)
        {
            int positionIndex = (int)InShuttleMotorX.EPosition.ScanTriggerStart;
            double originValue = mInspStageMotorPosition.dPosition[positionIndex];
            using (var keypad = new FormKeyPad(originValue))
            {
                if (keypad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                mInspStageMotorPosition.dPosition[positionIndex] = keypad.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{nameof(mInspStageMotorPosition.dPosition)}({positionIndex}): {originValue} -> {mInspStageMotorPosition.dPosition[positionIndex]}]");
            }
        }

        private void BtnTriggerEndPosition_Click(object sender, EventArgs e)
        {
            int positionIndex = (int)InShuttleMotorX.EPosition.ScanTriggerEnd;
            double originValue = mInspStageMotorPosition.dPosition[positionIndex];
            using (var keypad = new FormKeyPad(originValue))
            {
                if (keypad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                mInspStageMotorPosition.dPosition[positionIndex] = keypad.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{nameof(mInspStageMotorPosition.dPosition)}({positionIndex}): {originValue} -> {mInspStageMotorPosition.dPosition[positionIndex]}]");
            }
        }
    }
}