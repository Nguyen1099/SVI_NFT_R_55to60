using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormConfigMCCLog : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 옵션 파라미터
        /// </summary>
        private CConfig.CMccOptionParameter m_objMccOptionParameter;
        private CConfig.CMccInitializeParameter m_objMccInitializeParameter;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormConfigMCCLog(CDocument objDocument)
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
        private void CFormConfigMCCLog_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormConfigMCCLog_FormClosed(object sender, FormClosedEventArgs e)
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
            base.SetButtonBackColor(this.BtnTitleMCCLogView, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleFileServerIP, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnFileServerIP, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTitleFileServerPort, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnFileServerPort, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTitleBasicPath, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnBasicPath, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTitleFileServerLoginID, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnFileServerLoginID, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTitleFileServerLoginPassword, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnFileServerLoginPassword, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTitleLoggingTime, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnLoggingTime, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTitleLogDeletePeriod, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnLogDeletePeriod, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTitleLogUploadTime, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnLogUploadTime, m_colorLabelData);
            base.SetButtonBackColor(this.BtnUseMCC, m_colorNormal);
            base.SetButtonBackColor(this.BtnSave, m_colorNormal);
            base.SetButtonBackColor(this.BtnLoad, m_colorNormal);
            base.SetButtonBackColor(this.BtnTitleUploadStatus, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNextScheduleLessTime, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnNextScheduleLessTime, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTitleUploadInformation, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnUploadInformation, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTitleUploadCompleted, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleLastUploadSuccessed, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnManualUpload, m_colorNormal);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnFileServerIP.Name)
                    && false == btn.Name.Equals(BtnFileServerPort.Name)
                    && false == btn.Name.Equals(BtnBasicPath.Name)
                    && false == btn.Name.Equals(BtnFileServerLoginID.Name)
                    && false == btn.Name.Equals(BtnFileServerLoginPassword.Name)
                    && false == btn.Name.Equals(BtnLoggingTime.Name)
                    && false == btn.Name.Equals(BtnLogDeletePeriod.Name)
                    && false == btn.Name.Equals(BtnLogUploadTime.Name)
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
                SetButtonChangeLanguage(this.BtnTitleMCCLogView);
                SetButtonChangeLanguage(this.BtnTitleFileServerIP);
                SetButtonChangeLanguage(this.BtnTitleFileServerPort);
                SetButtonChangeLanguage(this.BtnTitleBasicPath);
                SetButtonChangeLanguage(this.BtnTitleFileServerLoginID);
                SetButtonChangeLanguage(this.BtnTitleFileServerLoginPassword);
                SetButtonChangeLanguage(this.BtnTitleLoggingTime);
                SetButtonChangeLanguage(this.BtnTitleLogDeletePeriod);
                SetButtonChangeLanguage(this.BtnTitleLogUploadTime);
                SetButtonChangeLanguage(this.BtnUseMCC);
                SetButtonChangeLanguage(this.BtnSave);
                SetButtonChangeLanguage(this.BtnLoad);
                SetButtonChangeLanguage(this.BtnTitleUploadStatus);
                SetButtonChangeLanguage(this.BtnTitleNextScheduleLessTime);
                SetButtonChangeLanguage(this.BtnTitleUploadInformation);
                SetButtonChangeLanguage(this.BtnTitleUploadCompleted);
                SetButtonChangeLanguage(this.BtnTitleLastUploadSuccessed);
                SetButtonChangeLanguage(this.BtnManualUpload);

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
                // MCC 로그 파라미터 갱신
                m_objMccOptionParameter = m_objDocument.m_objConfig.GetMccOptionParameter().DeepClone();
                m_objMccInitializeParameter = m_objDocument.m_objConfig.GetMccInitializeParameter().DeepClone();
            }
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // 파일 서버 IP
            base.SetButtonText(this.BtnFileServerIP, m_objMccInitializeParameter.objConnectInformation.strHostName);
            // 파일 서버 PORT
            base.SetButtonText(this.BtnFileServerPort, string.Format("{0}", m_objMccInitializeParameter.objConnectInformation.strHostPort));
            // 기본 경로
            base.SetButtonText(this.BtnBasicPath, m_objMccOptionParameter.strBasicPath);
            // 로그인 ID
            base.SetButtonText(this.BtnFileServerLoginID, m_objMccInitializeParameter.objConnectInformation.strUserName);
            // 로그인 암호
            base.SetButtonText(this.BtnFileServerLoginPassword, m_objMccInitializeParameter.objConnectInformation.strUserPassword);
            // 로깅 타임
            base.SetButtonText(this.BtnLoggingTime, string.Format("{0:D} ( min )", m_objMccOptionParameter.iLoggingTime));
            // 로그 보관 기간
            base.SetButtonText(this.BtnLogDeletePeriod, string.Format("{0:D} ( Days )", m_objMccOptionParameter.iLogDeletePeriod));
            // 로그 업로드 간격
            base.SetButtonText(this.BtnLogUploadTime, string.Format("{0:D} ( min )", m_objMccOptionParameter.iLogUploadTime));
            // MCC 로그 사용
            SetUseColor(this.BtnUseMCC, m_objMccOptionParameter.bMccEnabled);

            // UPLOAD INFORMATION
            {
                var fileInterface = m_objDocument.m_objProcessMain.m_objProcessMccLogTransfer;
                prgUploadProgress.Value = fileInterface.UploadProgress;
                SetControlBackColor(pnlUploadCompleted, fileInterface.IsUploadCompleted ? m_colorOn : m_colorOff);
                SetControlBackColor(pnlLastUploadSuccessed, fileInterface.IsLastUploadSuccessed ? m_colorOn : m_colorOff);
                SetControlText(BtnNextScheduleLessTime, $"{fileInterface.UploadLessMinutes.ToString(@"hh\:mm\:ss")}");
                string infomation =
                    $"{fileInterface.UploadFilesPath}\n"
                    + (fileInterface.UploadFilesTotalCount == 0 ? $"( 0 / 0 )" : $"( {fileInterface.UploadFilesCount} / {fileInterface.UploadFilesTotalCount} )");
                SetControlText(BtnUploadInformation, infomation);
            }
        }

        private void SetControlBackColor(Control control, Color color)
        {
            if (control.BackColor != color)
            {
                control.BackColor = color;
            }
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
                base.SetButtonBackColor(objButton, m_colorClick);
            }
            else
            {
                base.SetButtonBackColor(objButton, m_colorNormal);
            }
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            m_objDocument.m_objConfig.SaveMccOptionParameter(m_objMccOptionParameter);
            m_objDocument.m_objConfig.SaveMccInitializeParameter(m_objMccInitializeParameter);
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
            m_objMccOptionParameter = m_objDocument.m_objConfig.GetMccOptionParameter().DeepClone();
            m_objMccInitializeParameter = m_objDocument.m_objConfig.GetMccInitializeParameter().DeepClone();
            // Msg: 불러오기가 완료되었습니다.
            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.LOADING_IS_COMPLETE);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}]", "BtnLoad_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 파일 서버 IP 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFileServerIP_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnFileServerIP_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyBoard objKeyboard = new FormKeyBoard(m_objMccInitializeParameter.objConnectInformation.strHostName))
            {
                if (DialogResult.OK == objKeyboard.ShowDialog())
                {
                    m_objMccInitializeParameter.objConnectInformation.strHostName = objKeyboard.m_strReturnValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [FileServerIP : {1}] [{2}]", "BtnFileServerIP_Click", m_objMccInitializeParameter.objConnectInformation.strHostName, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 파일 서버 PORT 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFileServerPort_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnFileServerPort_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeypad = new FormKeyPad(m_objMccInitializeParameter.objConnectInformation.strHostPort, 0, ushort.MaxValue))
            {
                if (DialogResult.OK != objKeypad.ShowDialog())
                {
                    return;
                }
                m_objMccInitializeParameter.objConnectInformation.strHostPort = (int)objKeypad.m_dResultValue;
            }

            // 버튼 로그 추가
            strLog = string.Format("[{0}] [FileServerIP : {1}] [{2}]", "BtnFileServerPort_Click", m_objMccInitializeParameter.objConnectInformation.strHostPort, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 기본 경로 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBasicPath_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnBasicPath_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyBoard objKeyboard = new FormKeyBoard(m_objMccOptionParameter.strBasicPath))
            {
                if (DialogResult.OK == objKeyboard.ShowDialog())
                {
                    var splitText = objKeyboard.m_strReturnValue.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                    if (
                        splitText.Length == 0
                        || string.IsNullOrWhiteSpace(objKeyboard.m_strReturnValue) == true
                        || splitText.Where(item => string.IsNullOrWhiteSpace(item)).Any() == true
                        )
                    {
                        return;
                    }
                    m_objMccOptionParameter.strBasicPath = string.Join("\\", splitText) + "\\";
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [BasicPath : {1}] [{2}]", "BtnBasicPath_Click", m_objMccOptionParameter.strBasicPath, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 로그인 아이디 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFileServerLoginID_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnFileServerLoginID_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyBoard objKeyboard = new FormKeyBoard(m_objMccInitializeParameter.objConnectInformation.strUserName))
            {
                if (DialogResult.OK == objKeyboard.ShowDialog())
                {
                    m_objMccInitializeParameter.objConnectInformation.strUserName = objKeyboard.m_strReturnValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [LoginID : {1}] [{2}]", "BtnFileServerLoginID_Click", m_objMccInitializeParameter.objConnectInformation.strUserName, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 로그인 암호 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFileServerLoginPassword_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnFileServerLoginPassword_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyBoard objKeyboard = new FormKeyBoard(m_objMccInitializeParameter.objConnectInformation.strUserPassword))
            {
                if (DialogResult.OK == objKeyboard.ShowDialog())
                {
                    m_objMccInitializeParameter.objConnectInformation.strUserPassword = objKeyboard.m_strReturnValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [LoginPassword : {1}] [{2}]", "BtnFileServerLoginPassword_Click", m_objMccInitializeParameter.objConnectInformation.strUserPassword, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// Log량이 적은 설비 경우 Default 60분, Log량이 많은 설비 경우 Default 10분으로 설정
        /// 설명 : 사용자 01 ~ 99분 사이로만 Setting 가능하도록 구성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoggingTime_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnLoggingTime_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeypad = new FormKeyPad(m_objMccOptionParameter.iLoggingTime))
            {
                if (DialogResult.OK == objKeypad.ShowDialog())
                {
                    if (0 >= (int)objKeypad.m_dResultValue || 99 < (int)objKeypad.m_dResultValue)
                    {
                        // Msg: 로깅 시간은 01 ~ 99분 사이로 설정해야 합니다.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.LOADING_TIME_01_99_RANGE_SET);
                    }
                    else
                    {
                        m_objMccOptionParameter.iLoggingTime = (int)objKeypad.m_dResultValue;
                    }
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [LoggingTime : {1:D}] [{2}]", "BtnLoggingTime_Click", m_objMccOptionParameter.iLoggingTime, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 사용자 30 ~ 365일 사이로만 Setting 가능하도록 구성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogDeletePeriod_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnLogDeletePeriod_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeypad = new FormKeyPad(m_objMccOptionParameter.iLogDeletePeriod))
            {
                if (DialogResult.OK == objKeypad.ShowDialog())
                {
                    if (30 > (int)objKeypad.m_dResultValue || 365 < (int)objKeypad.m_dResultValue)
                    {
                        // Msg: 로그 보관 기간은 30 ~ 365일 사이로 설정해야 합니다.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.LOG_DELETE_PERIOD_30_365_RANGE_SET);
                    }
                    else
                    {
                        m_objMccOptionParameter.iLogDeletePeriod = (int)objKeypad.m_dResultValue;
                    }
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [LoggingTime : {1:D}] [{2}]", "BtnLoggingRetentionPeriodDays_Click", m_objMccOptionParameter.iLogDeletePeriod, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 사용자 01 ~ 99분 사이로만 Setting 가능하도록 구성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogUploadTime_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnLogUploadTime_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeypad = new FormKeyPad(m_objMccOptionParameter.iLogUploadTime))
            {
                if (DialogResult.OK == objKeypad.ShowDialog())
                {
                    if (0 >= (int)objKeypad.m_dResultValue || 99 < (int)objKeypad.m_dResultValue)
                    {
                        // Msg: 로그 업로드 시간 설정은 01 ~ 99분 사이로 설정해야 합니다.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.LOADING_TIME_01_99_RANGE_SET);
                    }
                    else
                    {
                        m_objMccOptionParameter.iLogUploadTime = (int)objKeypad.m_dResultValue;
                    }
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [LoggingTime : {1:D}] [{2}]", "BtnLogUploadTime_Click", m_objMccOptionParameter.iLogUploadTime, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// Use MCC On / Off
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUseMCC_Click(object sender, EventArgs e)
        {
            m_objMccOptionParameter.bMccEnabled = !m_objMccOptionParameter.bMccEnabled;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [bMCCOn : {1}]", "BtnUseMCC_Click", m_objMccOptionParameter.bMccEnabled.ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private async void BtnManualUpload_Click(object sender, EventArgs e)
        {
            await m_objDocument.m_objProcessMain.m_objProcessMccLogTransfer.RunUploadNowAsync();
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [ManualUploadTriggerOn]", "BtnManualUpload_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }
    }
}