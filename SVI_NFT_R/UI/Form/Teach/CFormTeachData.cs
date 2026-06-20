using HLDevice;
using HLDevice.Abstract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormTeachData : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 현재 포지션 값 대리자 생성
        /// </summary>
        public event UpdateCurrentPosition EventUpdatePosition;
        public delegate void UpdateCurrentPosition(double dPosition);
        /// <summary>
        /// 현재 모터 정보 업데이트 대리자 생성
        /// </summary>
        public event UpdateMotorInformation EventUpdateMotor;
        public delegate void UpdateMotorInformation(ref CDeviceMotor objMotor);
        /// <summary>
        /// 현재 모터 포지션 정보 업데이트
        /// </summary>
        public event UpdatePositionInformation EventUpdatePositionParameter;
        public delegate void UpdatePositionInformation(ref CDeviceMotorAbstract.CMotorPosition objPosition);
        /// <summary>
        /// 선택된 포지션 인덱스 정보 대리자 생성
        /// </summary>
        public event UpdateMoveIndex EventUpdateIndex;
        public delegate void UpdateMoveIndex(ref int iPositionIndex);
        /// <summary>
        /// 상대 이동 인터락 검사 대리자 생성
        /// </summary>
        public event Func<double, bool> EventCheckAbsolutePositionInterlock;
        /// <summary>
        /// 인터락 검사 대리자 생성
        /// </summary>
        public event DelegateCheckInterlock EventCheckInterlock;
        /// <summary>
        /// 원점 이동 인터락 검사 대리자 생성
        /// </summary>
        public event DelegateCheckInterlock EventCheckHomeInterlock;
        /// <summary>
        /// 조그 이동 인터락 검사 대리자 생성
        /// </summary>
        public event DelegateCheckJogInterlock EventCheckJogInterlock;
        public delegate void DelegateCheckInterlock(ref bool bStatus);
        public delegate void DelegateCheckJogInterlock(ref bool bStatus, bool bJogPositiveDirection, Action callbackInterlockPreActionOrNull);
        /// <summary>
        /// 현재 위치 선택 초기화
        /// </summary>
        public DelegatePositionDataReset m_objPositionReset = null;
        public delegate void DelegatePositionDataReset(EPositionReset ePositionReset);
        /// <summary>
        /// 티칭 정보 변경 로그 저장 대리자 생성 20180711
        /// </summary>
        public DelegateTeachInformation m_objTeachInformation = null;
        public delegate void DelegateTeachInformation(string strTeachData);
        public event EventHandler OriginReturnFinished;

        /// <summary>
        /// 조그 속도 모드
        /// </summary>
        private enum EJogVelocityMode
        {
            JOG_FAST_MODE = 0,
            JOG_SLOW_MODE
        }

        public enum EPositionReset
        {
            RESET_CURRENT_POSITION = 0,
            RESET_POSITION_LOG_DATA,
        }

        private readonly CDocument m_objDocument;
        private CDeviceMotor m_objMotor;
        private EJogVelocityMode m_eJogVelocityMode;
        private double m_dRelativeValue;
        private double m_dCurrentPosition;
        private readonly List<string> m_listTeachInformation = new List<string>();
        private bool mbJogMoving;
        private bool mbJogPositiveDirection = false;

        private double m_dTagetPositionValue;
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <param name="objMotor"></param>
        /// <returns></returns>
        public CFormTeachData(CDocument objDocument, CDeviceMotor objMotor)
        {
            m_objDocument = objDocument;
            // 모터 정보
            m_objMotor = objMotor;

            m_dRelativeValue = 0;
            m_dCurrentPosition = double.MinValue;
            m_listTeachInformation.Clear();
            m_dTagetPositionValue = 0;
            mbJogMoving = false;

            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormTeachData_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();

        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormTeachData_FormClosed(object sender, FormClosedEventArgs e)
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
                // 현재 포지션 저장 정보 리셋
                m_objPositionReset += new DelegatePositionDataReset(ResetPositionData);
                // 티칭 데이터 로그 저장 정보
                m_objTeachInformation += new DelegateTeachInformation(SaveTeachInformation);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            SetButtonBackColor(BtnTitleAxisName, m_colorLabel);
            SetButtonBackColor(BtnMinusLimit, m_colorNormal);
            SetButtonBackColor(BtnHomeSensor, m_colorNormal);
            SetButtonBackColor(BtnPlusLimit, m_colorNormal);
            SetButtonBackColor(BtnTitleCurrentPosition, m_colorLabelSub);
            SetButtonBackColor(BtnCurrentPosition, m_colorLabelData);
            SetButtonBackColor(BtnTargetPosition, m_colorLabelData);
            SetButtonBackColor(BtnServo, m_colorNormal);
            SetButtonBackColor(BtnAlarm, m_colorNormal);
            SetButtonBackColor(BtnInposition, m_colorNormal);
            SetButtonBackColor(BtnHome, m_colorNormal);
            SetButtonBackColor(BtnPositionMove, m_colorNormal);
            BtnPositionMove.PenColor = Color.Blue;
            BtnPositionMove.ForeColor = Color.Blue;
            SetButtonBackColor(BtnStop, m_colorNormal);
            SetButtonBackColor(BtnTitleRelativeMoveValue, m_colorLabelSub);
            SetButtonBackColor(BtnRelativeMoveValue, m_colorLabelData);
            SetButtonBackColor(BtnRelativeMoveMinus, m_colorNormal);
            SetButtonBackColor(BtnRelativeMovePlus, m_colorNormal);
            SetButtonBackColor(BtnTitleJog, m_colorLabelSub);
            SetButtonBackColor(BtnFastJogMode, m_colorNormal);
            SetButtonBackColor(BtnSlowJogMode, m_colorNormal);
            SetButtonBackColor(BtnJogMoveMinus, m_colorNormal);
            SetButtonBackColor(BtnJogMovePlus, m_colorNormal);
            SetButtonBackColor(BtnSave, m_colorNormal);
            SetButtonBackColor(BtnTargetPositionMove, m_colorNormal);
            SetButtonBackColor(BtnPositionJog, m_colorNormal);
            BtnPositionJog.PenColor = Color.Blue;
            BtnPositionJog.ForeColor = Color.Blue;

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnCurrentPosition.Name)
                    && false == btn.Name.Equals(BtnTargetPosition.Name)
                    && false == btn.Name.Equals(BtnServo.Name)
                    && false == btn.Name.Equals(BtnAlarm.Name)
                    && false == btn.Name.Equals(BtnHome.Name)
                    && false == btn.Name.Equals(BtnRelativeMoveValue.Name)
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
        /// 설명 : (상위 Teach Form에서 호출해야 함...)
        /// </summary>
        public void SetResourceControl()
        {
            // 현재 유저 권한 레벨 받아옴
            CUserInformation objUserInformation = m_objDocument.GetUserInformation();

            // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                base.SetControlButtonEnable(Controls, false);
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        base.SetControlButtonEnable(Controls, false);
                        BtnPositionMove.PenColor = Color.Black;
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        base.SetControlButtonEnable(Controls, true);
                        BtnPositionMove.PenColor = Color.Blue;
                        BtnPositionMove.ForeColor = Color.Blue;
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        base.SetControlButtonEnable(Controls, true);
                        BtnPositionMove.PenColor = Color.Blue;
                        BtnPositionMove.ForeColor = Color.Blue;
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
                SetButtonChangeLanguage(BtnTitleAxisName);
                SetButtonChangeLanguage(BtnServo);
                SetButtonChangeLanguage(BtnTitleCurrentPosition);
                SetButtonChangeLanguage(BtnAlarm);
                SetButtonChangeLanguage(BtnInposition);
                SetButtonChangeLanguage(BtnHome);
                SetButtonChangeLanguage(BtnPositionMove);
                SetButtonChangeLanguage(BtnTitleJog);
                SetButtonChangeLanguage(BtnFastJogMode);
                SetButtonChangeLanguage(BtnSlowJogMode);
                SetButtonChangeLanguage(BtnJogMovePlus);
                SetButtonChangeLanguage(BtnJogMoveMinus);
                SetButtonChangeLanguage(BtnTitleRelativeMoveValue);
                SetButtonChangeLanguage(BtnRelativeMoveMinus);
                SetButtonChangeLanguage(BtnRelativeMovePlus);
                SetButtonChangeLanguage(BtnStop);
                SetButtonChangeLanguage(BtnSave);
                SetButtonChangeLanguage(BtnMinusLimit);
                SetButtonChangeLanguage(BtnHomeSensor);
                SetButtonChangeLanguage(BtnPlusLimit);
                SetButtonChangeLanguage(BtnTargetPositionMove);
                lblTitleServoLoadRatio.Text = m_objDocument.GetDatabaseUIText(lblTitleServoLoadRatio.Name, Name);
                lblTitleCurrentVelocity.Text = m_objDocument.GetDatabaseUIText(lblTitleCurrentVelocity.Name, Name);
                SetButtonChangeLanguage(BtnPositionJog);

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
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        /// <summary>
        /// 타이머 유무
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            timer.Enabled = bTimer;
            // 폼 타이머 이벤트 마다 상대위치 값 초기화.
            m_dRelativeValue = 0;
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
            }
        }

        /// <summary>
        /// 타이머 동작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (true == mbJogMoving)
            {
                bool bInterlockState = false;
                EventCheckJogInterlock(ref bInterlockState, mbJogPositiveDirection, callbackInterlockPreAction);
                if (false == bInterlockState)
                {
                    // 인터락 발생시 콜백 함수에서 정지 및 플래그 처리함
                }
            }

            if (m_objMotor == null) return; // #Todo 임의 대응해놓음

            EventUpdateMotor(ref m_objMotor);
            if (null != m_objMotor)
            {
                var objMotorStatus = m_objMotor.HLGetMotorStatus();
                var objMotorOperationParameter = m_objMotor.HLGetMotorOperationParameter();
                // 모터 이름 표시
                BtnTitleAxisName.Text = m_objMotor.HLGetMotorInitializeParameter().strMotorName;
                bool bIsConveyor = BtnTitleAxisName.Text.Contains("_CONVEYOR_");
                // 현재 위치 표시
                BtnCurrentPosition.Text = string.Format("{0:F3}", objMotorStatus.dEncoderPosition);
                // 현재 위치 표시
                lblServoLoadRatio.Text = string.Format("{0,6:0.0} ( % )", objMotorStatus.dServoLoadRatio);
                lblCurrentVelocity.Text = string.Format("{0:F3} ( mm/sec )", objMotorStatus.dCurrentVelocity);
                // 알람 상태
                if (true == objMotorStatus.bAlarm)
                {
                    SetButtonBackColor(BtnAlarm, m_colorOff);
                }
                else
                {
                    SetButtonBackColor(BtnAlarm, m_colorNormal);
                }
                // 원점 복귀 상태 표시
                if (
                    true == objMotorOperationParameter.bUseHome
                    && false == objMotorStatus.bHome
                    )
                {
                    SetButtonBackColor(BtnHome, m_colorOff);
                }
                else
                {
                    SetButtonBackColor(BtnHome, m_colorOn);
                }
                // 서보 온오프 표시
                if (true == objMotorStatus.bServo)
                {
                    SetButtonBackColor(BtnServo, m_colorOn);
                }
                else
                {
                    SetButtonBackColor(BtnServo, m_colorOff);
                }
                // 인포지션 상태 표시
                if (true == objMotorStatus.bInposition)
                {
                    SetButtonBackColor(BtnInposition, m_colorOn);
                }
                else
                {
                    SetButtonBackColor(BtnInposition, m_colorNormal);
                }
                // 조그 속도 모드 표시
                if (EJogVelocityMode.JOG_FAST_MODE == m_eJogVelocityMode)
                {
                    SetButtonBackColor(BtnFastJogMode, m_colorOn, true);
                }
                else
                {
                    SetButtonBackColor(BtnFastJogMode, m_colorNormal);
                }
                if (EJogVelocityMode.JOG_SLOW_MODE == m_eJogVelocityMode)
                {
                    SetButtonBackColor(BtnSlowJogMode, m_colorOn, true);
                }
                else
                {
                    SetButtonBackColor(BtnSlowJogMode, m_colorNormal);
                }
                // 리미트 상태 표시
                if (
                    true == objMotorStatus.bMinusLimitSensor
                    && false == bIsConveyor
                    )
                {
                    SetButtonBackColor(BtnMinusLimit, m_colorOff);
                }
                else
                {
                    SetButtonBackColor(BtnMinusLimit, m_colorNormal);
                }
                if (
                    true == objMotorStatus.bPlusLimitSensor
                    && false == bIsConveyor
                    )
                {
                    SetButtonBackColor(BtnPlusLimit, m_colorOff);
                }
                else
                {
                    SetButtonBackColor(BtnPlusLimit, m_colorNormal);
                }
                if (
                    true == objMotorStatus.bHomeSensor
                    && false == bIsConveyor
                    )
                {
                    SetButtonBackColor(BtnHomeSensor, m_colorOn);
                }
                else
                {
                    SetButtonBackColor(BtnHomeSensor, m_colorNormal);
                }
                // 상대위치 값
                BtnRelativeMoveValue.Text = m_dRelativeValue.ToString();
                // 현재 위치 표시
                if (double.MinValue != m_dCurrentPosition)
                {
                    SetButtonBackColor(BtnCurrentPositionStatus, m_colorOn);
                }
                else
                {
                    SetButtonBackColor(BtnCurrentPositionStatus, m_colorNormal);
                }
            }
        }

        private void callbackInterlockPreAction()
        {
            m_objMotor.HLMoveEStop();
            mbJogMoving = false;
            m_objDocument.m_objProcessMain.IsJogMoving = false;
        }

        /// <summary>
        /// 서보 온/오프
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnServo_Click(object sender, EventArgs e)
        {
            if (true == m_objMotor.HLGetMotorStatus().bServo)
            {
                m_objMotor.HLServoOn(CDeviceMotorAbstract.EUse.DISABLE);
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Servo : {1}]", "BtnServo_Click", CDeviceMotorAbstract.EUse.DISABLE.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }
            else
            {
                m_objMotor.HLServoOn(CDeviceMotorAbstract.EUse.ENABLE);
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Servo : {1}]", "BtnServo_Click", CDeviceMotorAbstract.EUse.ENABLE.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }
        }

        /// <summary>
        /// 현재 포지션 값 복사
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCurrentPosition_Click(object sender, EventArgs e)
        {
            if (double.MinValue == m_dCurrentPosition)
            {
                m_dCurrentPosition = Convert.ToDouble(BtnCurrentPosition.Text);
                EventUpdatePosition(Convert.ToDouble(BtnCurrentPosition.Text));
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Position : {1}]", "BtnCurrentPosition_Click", BtnCurrentPosition.Text);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }
            else
            {
                m_dCurrentPosition = double.MinValue;
                EventUpdatePosition(double.MinValue);
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Reset]", "BtnCurrentPosition_Click", BtnCurrentPosition.Text);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }
        }

        /// <summary>
        /// 현재 포지션 및 포지션 로그 초기화 20180711
        /// </summary>
        /// <param name="ePositionReset"></param>
        private void ResetPositionData(EPositionReset ePositionReset)
        {
            if (EPositionReset.RESET_CURRENT_POSITION == ePositionReset)
            {
                m_dCurrentPosition = double.MinValue;
            }
            else if (EPositionReset.RESET_POSITION_LOG_DATA == ePositionReset)
            {
                m_listTeachInformation.Clear();
            }
        }

        /// <summary>
        /// 티칭 정보 로그 정보 저장 20180711
        /// </summary>
        /// <param name="strTeachData"></param>
        private void SaveTeachInformation(string strTeachData)
        {
            if (0 != strTeachData.Length)
            {
                m_listTeachInformation.Add(strTeachData);
            }
        }

        /// <summary>
        /// 알람 상태 초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlarm_Click(object sender, EventArgs e)
        {
            m_objMotor.HLServoOn(CDeviceMotorAbstract.EUse.DISABLE);
            Thread.Sleep(200);
            m_objMotor.HLAlarmReset();
            m_objMotor.HLServoOn(CDeviceMotorAbstract.EUse.ENABLE);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnAlarm_Click", "Alarm Reset");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 선택 위치 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPositionMove_Click(object sender, EventArgs e)
        {
            int iPositionIndex = -1;
            bool bResult = false;
            do
            {
                try
                {
                    if (m_objDocument.m_objProcessMain.CheckDoorLock() == false)
                    {
                        break;
                    }
                    if (m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto() == false)
                    {
                        // Msg: 세이프티 모드를 AUTO로 변경하시기 바랍니다.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                        break;
                    }

                    // 현재 선택된 포지션의 인터락 체크 결과를 얻어 온다.
                    EventCheckInterlock(ref bResult);
                    if (false == bResult)
                    {
                        break;
                    }

                    // 현재 선택된 포지션 인덱스를 얻어온다.
                    EventUpdateIndex(ref iPositionIndex);
                    if (0 <= iPositionIndex)
                    {
                        if (false == m_objMotor.HLMoveAbsoluteIndex(iPositionIndex, CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_MANUAL_RUN))
                        {
                            break;
                        }
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Position Index : {1:D}] [Velocity Mode : {2}]", "BtnPositionMove_Click", iPositionIndex, CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_MANUAL_RUN);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 정지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            m_objMotor.HLMoveStop();
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Stop : {1}]", "BtnStop_Click", m_objMotor.HLGetMotorInitializeParameter().strMotorName);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 빠른 조그 모드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFastJogMode_Click(object sender, EventArgs e)
        {
            m_eJogVelocityMode = EJogVelocityMode.JOG_FAST_MODE;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Velocity Mode : {1}]", "BtnFastJogMode_Click", m_eJogVelocityMode.ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 느린 조그 모드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSlowJogMode_Click(object sender, EventArgs e)
        {
            m_eJogVelocityMode = EJogVelocityMode.JOG_SLOW_MODE;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Velocity Mode : {1}]", "BtnSlowJogMode_Click", m_eJogVelocityMode.ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// ( + ) 조그 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnJogMovePlus_MouseUp(object sender, MouseEventArgs e)
        {
            m_objMotor.HLMoveStop();
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnJogMovePlus_MouseUp", "Move Stop");
            m_objDocument.SetUpdateButtonLog(this, strLog);

            mbJogMoving = false;
            m_objDocument.m_objProcessMain.IsJogMoving = false;
        }

        private void BtnJogMovePlus_MouseDown(object sender, MouseEventArgs e)
        {
            do
            {
                mbJogPositiveDirection = true;
                bool bInterlockState = false;
                EventCheckJogInterlock(ref bInterlockState, mbJogPositiveDirection, null);
                if (false == bInterlockState)
                {
                    break;
                }

                if (EJogVelocityMode.JOG_FAST_MODE == m_eJogVelocityMode)
                {
                    m_objMotor.HLJogFastMove(CDeviceMotorAbstract.EMoveDirection.DIRECTION_CW);
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Fast Move : {1}]", "BtnJogMovePlus_MouseDown", CDeviceMotorAbstract.EMoveDirection.DIRECTION_CW.ToString());
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
                else
                {
                    m_objMotor.HLJogSlowMove(CDeviceMotorAbstract.EMoveDirection.DIRECTION_CW);
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Slow Move : {1}]", "BtnJogMovePlus_MouseDown", CDeviceMotorAbstract.EMoveDirection.DIRECTION_CW.ToString());
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }

                mbJogMoving = true;
                m_objDocument.m_objProcessMain.IsJogMoving = true;
            } while (false);
        }

        /// <summary>
        /// ( - ) 조그 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnJogMoveMinus_MouseUp(object sender, MouseEventArgs e)
        {
            m_objMotor.HLMoveStop();
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnJogMoveMinus_MouseUp", "Move Stop");
            m_objDocument.SetUpdateButtonLog(this, strLog);

            mbJogMoving = false;
            m_objDocument.m_objProcessMain.IsJogMoving = false;
        }

        private void BtnJogMoveMinus_MouseDown(object sender, MouseEventArgs e)
        {
            do
            {
                mbJogPositiveDirection = false;
                bool bInterlockState = false;
                EventCheckJogInterlock(ref bInterlockState, mbJogPositiveDirection, null);
                if (false == bInterlockState)
                {
                    break;
                }

                if (EJogVelocityMode.JOG_FAST_MODE == m_eJogVelocityMode)
                {
                    m_objMotor.HLJogFastMove(CDeviceMotorAbstract.EMoveDirection.DIRECTION_CCW);
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Fast Move : {1}]", "BtnJogMoveMinus_MouseDown", CDeviceMotorAbstract.EMoveDirection.DIRECTION_CCW.ToString());
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
                else
                {
                    m_objMotor.HLJogSlowMove(CDeviceMotorAbstract.EMoveDirection.DIRECTION_CCW);
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Slow Move : {1}]", "BtnJogMoveMinus_MouseDown", CDeviceMotorAbstract.EMoveDirection.DIRECTION_CCW.ToString());
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }

                mbJogMoving = true;
                m_objDocument.m_objProcessMain.IsJogMoving = true;
            } while (false);
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE))
            {
                return;
            }

            try
            {
                CDeviceMotorAbstract.CMotorPosition objMotorPosition = new CDeviceMotorAbstract.CMotorPosition();
                EventUpdatePositionParameter(ref objMotorPosition);
                if (null != objMotorPosition)
                {
                    // Input Value Validation
                    if (CProcessMotion.EMotor.INSP_STAGE_Y.ToString() == m_objMotor.HLGetMotorInitializeParameter().strMotorName)
                    {
                        //if (false == CCIMDefine.RECIPE_PARAMS[CCIMDefine.EPpidParamList.INSPECTION_START_POSITION_TOP].CheckInRange(objMotorPosition.dPosition[(int)LongSideInspectionStageeMotor.EPosition.TriggerStart].ToString()))
                        //{
                        //    MessageBox.Show(string.Format("INSPECTION START POSITION IS RANGE OVER.\n({0})", CCIMDefine.RECIPE_PARAMS[CCIMDefine.EPpidParamList.INSPECTION_START_POSITION_TOP].GetRangeString()), "INPUT VALUE VALIDATION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    return;
                        //}
                        //if (false == CCIMDefine.RECIPE_PARAMS[CCIMDefine.EPpidParamList.INSPECTION_END_POSITION_TOP].CheckInRange(objMotorPosition.dPosition[(int)LongSideInspectionStageeMotor.EPosition.TriggerEnd].ToString()))
                        //{
                        //    MessageBox.Show(string.Format("INSPECTION END POSITION IS RANGE OVER.\n({0})", CCIMDefine.RECIPE_PARAMS[CCIMDefine.EPpidParamList.INSPECTION_END_POSITION_TOP].GetRangeString()), "INPUT VALUE VALIDATION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    return;
                        //}
                    }

                    if (System.Windows.Forms.DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE))
                    {
                        return;
                    }

                    m_objMotor.HLSaveMotorPositionParameter("", "", objMotorPosition);
                    //kjpark 20181130 PPID BODY 값 변경시 CEID 108 으로 보고됨  
                    m_objDocument.m_objRecipe.SetSaveRecipeAndReportCim(null);
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}]", "BtnSave_Click");
                    m_objDocument.SetUpdateButtonLog(this, strLog);

                    // 티칭 정보 로그 업데이트 20180711
                    for (int iLoopCount = 0; iLoopCount < m_listTeachInformation.Count; iLoopCount++)
                    {
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_TEACH_DATA, m_listTeachInformation[iLoopCount]);
                    }

                    m_listTeachInformation.Clear();
                    // Msg: 저장이 완료되었습니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.SAVING_IS_COMPLETE);
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 상대 위치 입력 값
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRelativeMoveValue_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnRelativeMoveValue_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            double dCurrentPos = Double.Parse(BtnCurrentPosition.Text);
            string strTitle = string.Format("Relative Move Value, Current Motor Position = {0}", dCurrentPos);
            CDeviceMotorAbstract.CMotorOperationParameter motorParam = new CDeviceMotorAbstract.CMotorOperationParameter();
            motorParam = m_objMotor.HLGetMotorOperationParameter();
            using (var objKeyPad = new FormKeyPad(0.0, motorParam.dLimitSoftwareMinus, motorParam.dLimitSoftwarePlus, strTitle))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    double dResultPos = objKeyPad.m_dResultValue + dCurrentPos;
                    if (dResultPos > motorParam.dLimitSoftwarePlus
                        || dResultPos < motorParam.dLimitSoftwareMinus)
                    {
                        MessageBox.Show(string.Format("{0} is Out OF Range MIN = {1}, MAX = {2}", dResultPos, motorParam.dLimitSoftwareMinus, motorParam.dLimitSoftwarePlus));
                    }
                    else
                    {
                        m_dRelativeValue = objKeyPad.m_dResultValue;
                    }
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [RelativeValue : {1:F3}] [{2}]", "BtnRelativeMoveValue_Click", m_dRelativeValue, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 상대위치 마이너스 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRelativeMoveMinus_Click(object sender, EventArgs e)
        {
            do
            {
                try
                {
                    if (m_objDocument.m_objProcessMain.CheckDoorLock() == false)
                    {
                        break;
                    }
                    if (m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto() == false)
                    {
                        // Msg: 세이프티 모드를 AUTO로 변경하시기 바랍니다.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                        break;
                    }

                    CDeviceMotorAbstract.CMotorStatus objMotorStatus = m_objMotor.HLGetMotorStatus();

                    double dValue = 0;
                    if (0 < m_dRelativeValue)
                    {
                        dValue = objMotorStatus.dEncoderPosition + m_dRelativeValue * (-1);
                    }
                    else
                    {
                        dValue = objMotorStatus.dEncoderPosition + m_dRelativeValue;
                    }

                    // 인터락 확인
                    if (EventCheckAbsolutePositionInterlock(dValue) == false)
                    {
                        break;
                    }

                    if (false == m_objMotor.HLMoveAbsolutePosition(dValue))
                    {
                        break;
                    }
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [ Value : {1:F3}] [Velocity Mode : {2}]", "BtnRelativeMoveMinus_Click", dValue, CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_MANUAL_RUN);
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상대위치 플러스 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRelativeMovePlus_Click(object sender, EventArgs e)
        {
            do
            {
                // 도어 잠김 확인
                if (m_objDocument.m_objProcessMain.CheckDoorLock() == false)
                {
                    break;
                }
                if (m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto() == false)
                {
                    // Msg: 세이프티 모드를 AUTO로 변경하시기 바랍니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                    break;
                }

                CDeviceMotorAbstract.CMotorStatus objMotorStatus = m_objMotor.HLGetMotorStatus();

                double dValue = 0;
                if (0 < m_dRelativeValue)
                {
                    dValue = objMotorStatus.dEncoderPosition + m_dRelativeValue;
                }
                else
                {
                    dValue = objMotorStatus.dEncoderPosition + m_dRelativeValue * (-1);
                }

                if (EventCheckAbsolutePositionInterlock(dValue) == false)
                {
                    break;
                }

                // 인터락 확인
                if (false == m_objMotor.HLMoveAbsolutePosition(dValue))
                {
                    break;
                }
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [ RelativeValue : {1:F3}] [Velocity Mode : {2}]", "BtnRelativeMovePlus_Click", m_dRelativeValue, CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_MANUAL_RUN);
                m_objDocument.SetUpdateButtonLog(this, strLog);

            } while (false);
        }

        private void BtnTargetPosition_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnTargetPosition_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            double dCurrentPos = Double.Parse(BtnCurrentPosition.Text);
            string strTitle = string.Format("Taget Move Value, Current Motor Position = {0}", dCurrentPos);
            CDeviceMotorAbstract.CMotorOperationParameter motorParam = new CDeviceMotorAbstract.CMotorOperationParameter();
            motorParam = m_objMotor.HLGetMotorOperationParameter();
            using (var objKeyPad = new FormKeyPad(0.0, motorParam.dLimitSoftwareMinus, motorParam.dLimitSoftwarePlus, strTitle))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    double dResultPos = objKeyPad.m_dResultValue;
                    if (dResultPos > motorParam.dLimitSoftwarePlus
                        || dResultPos < motorParam.dLimitSoftwareMinus)
                    {
                        MessageBox.Show(string.Format("{0} is Out OF Range MIN = {1}, MAX = {2}", dResultPos, motorParam.dLimitSoftwareMinus, motorParam.dLimitSoftwarePlus));
                    }
                    else
                    {
                        m_dTagetPositionValue = objKeyPad.m_dResultValue;
                    }

                    BtnTargetPosition.Text = string.Format("{0}", m_dTagetPositionValue);
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [TagetPositionValue : {1:F3}] [{2}]", "BtnTargetPosition_Click", m_dTagetPositionValue, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }


        private void BtnTargetPositionMove_Click(object sender, EventArgs e)
        {
            do
            {
                // 도어 잠김 확인
                if (m_objDocument.m_objProcessMain.CheckDoorLock() == false)
                {
                    break;
                }
                if (m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto() == false)
                {
                    // Msg: 세이프티 모드를 AUTO로 변경하시기 바랍니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                    break;
                }

                // 인터락 확인
                if (EventCheckAbsolutePositionInterlock(m_dTagetPositionValue) == false)
                {
                    break;
                }

                if (false == m_objMotor.HLMoveAbsolutePosition(m_dTagetPositionValue))
                {
                    break;
                }
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [ TagetPositionValue : {1:F3}] [Velocity Mode : {2}]", "BtnTargetPositionMove_Click", m_dTagetPositionValue, CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_MANUAL_RUN);
                m_objDocument.SetUpdateButtonLog(this, strLog);

            } while (false);
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            m_objDocument.SetUpdateButtonLog(this, string.Format("[{0}] [{1}]", "BtnHome_Click", true));
            do
            {
                // 원점 이동을 사용하지 않는 축의 경우
                if (false == m_objMotor.HLGetMotorOperationParameter().bUseHome)
                {
                    break;
                }

                // 도어 잠김 확인
                if (m_objDocument.m_objProcessMain.CheckDoorLock() == false)
                {
                    break;
                }
                if (m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto() == false)
                {
                    // Msg: 세이프티 모드를 AUTO로 변경하시기 바랍니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                    break;
                }

                // 원점 이동의 인터락 체크 결과를 얻어 온다.
                bool bResult = false;
                EventCheckHomeInterlock(ref bResult);
                if (false == bResult)
                {
                    break;
                }

                // 원점 이동 시작
                m_objDocument.GetMainFrame().ShowWaitMessage(true, "ORIGIN RETURN");
                m_objMotor.HLSetSoftwareLimit(CDeviceMotorAbstract.EUse.ENABLE);
                if (false == m_objMotor.HLSetHomeProcess())
                {
                    m_objDocument.GetMainFrame().ShowWaitMessage(false);
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_IS_HOME_PROCESS_FAIL, m_objMotor.HLGetMotorInitializeParameter().strMotorName);
                    break;
                }

                // 원점 이동 완료 대기
                int iHomeMainStepNo = 0;
                int iHomeStepNo = 0;
                bool bCurrentResult = m_objMotor.HLGetMotorStatus().bHome;
                m_objMotor.HLGetHomeProcessRate(ref iHomeMainStepNo, ref iHomeStepNo);
                int iTimeOut = m_objMotor.HLGetMotorOperationParameter().iStandardLimitTimeOut;
                while (100 != iHomeStepNo)
                {
                    if (iTimeOut < 0)
                    {
                        // Msg: 홈 프로세스 타임아웃 입니다.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_IS_HOME_PROCESS_TIMEOUT, m_objMotor.HLGetMotorInitializeParameter().strMotorName);
                        break;
                    }

                    Application.DoEvents();
                    Thread.Sleep(10);
                    iTimeOut -= 10;

                    bCurrentResult = m_objMotor.HLGetMotorStatus().bHome;
                    m_objMotor.HLGetHomeProcessRate(ref iHomeMainStepNo, ref iHomeStepNo);
                }

                // 초기화가 끝나면 이벤트 핸들러를 호출한다. (Inspection Stage 같은 경우 작업 완료 후 확장 축의 위치 정보를 0으로 설정해 줘야 하기 때문에 추가함)
                var eventHandlerCpature = OriginReturnFinished;
                if (null != eventHandlerCpature)
                {
                    eventHandlerCpature.Invoke(this, EventArgs.Empty);
                }
            } while (false);
            m_objDocument.GetMainFrame().ShowWaitMessage(false);
            // 버튼 로그 추가
            m_objDocument.SetUpdateButtonLog(this, string.Format("[{0}] [{1}]", "BtnHome_Click", false));
        }

        private void BtnPositionJog_MouseDown(object sender, MouseEventArgs e)
        {
            do
            {
                int iPositionIndex = -1;
                // 현재 선택된 포지션 인덱스를 얻어온다.
                EventUpdateIndex(ref iPositionIndex);
                if (iPositionIndex == -1)
                {
                    break;
                }

                mbJogPositiveDirection = m_objMotor.HLGetMotorStatus().dEncoderPosition < m_objMotor.HLGetMotorPosition().dPosition[iPositionIndex];
                bool bInterlockState = false;
                EventCheckJogInterlock(ref bInterlockState, mbJogPositiveDirection, null);
                if (false == bInterlockState)
                {
                    break;
                }

                if (EJogVelocityMode.JOG_FAST_MODE == m_eJogVelocityMode)
                {
                    m_objMotor.HLMoveAbsolutePosition(m_objMotor.HLGetMotorPosition().dPosition[iPositionIndex], m_objMotor.HLGetMotorOperationParameter().dVelocityJogFast);
                }
                else
                {
                    m_objMotor.HLMoveAbsolutePosition(m_objMotor.HLGetMotorPosition().dPosition[iPositionIndex], m_objMotor.HLGetMotorOperationParameter().dVelocityJogSlow);
                }

                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{m_eJogVelocityMode}] [Target: {m_objMotor.HLGetMotorPosition().dPosition[iPositionIndex]:0.000}]");
                mbJogMoving = true;
                m_objDocument.m_objProcessMain.IsJogMoving = true;
            } while (false);
        }

        private void BtnPositionJog_MouseUp(object sender, MouseEventArgs e)
        {
            m_objMotor.HLMoveStop();
            // 버튼 로그 추가
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");

            mbJogMoving = false;
            m_objDocument.m_objProcessMain.IsJogMoving = false;
        }
    }
}