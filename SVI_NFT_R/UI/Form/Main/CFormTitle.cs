using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormTitle : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        private int mBlinkTick = 0;
        private CDatabaseDefine.ESvidList[] mFfuItems = null;
        private readonly Color mNormalBackColor = Color.White;
        private readonly Color mVirtualBackColor = Color.Yellow;
        private readonly Color mDryRunBackColor = Color.Red;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormTitle(CDocument objDocument)
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
        private void CFormTitle_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormTitle_FormClosed(object sender, FormClosedEventArgs e)
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

        public static T GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
        {
            // Get attributes of this type.
            object[] attributes =
                assembly.GetCustomAttributes(typeof(T), true);

            // If we didn't get anything, return null.
            if ((attributes == null) || (attributes.Length == 0))
                return null;

            // Convert the first attribute value into
            // the desired type and return it.
            return (T)attributes[0];
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
                // 타이머 시작
                timer.Interval = 100;
                timer.Enabled = true;

                Assembly asm = Assembly.GetExecutingAssembly();
                //DateTime buildDateTime = asm.GetBuildDateTime();
                // !빌드 날짜를 사용하면 현장에서 디버깅시 날짜가 수시로 바뀔 수 있어서 Resource에 고정 값을 넣고 배포시 Resource 값을 업데이트 해야함.
                DateTime buildDateTime = DateTime.Parse(Properties.Resources.BuildDate);
                BtnVersionInfo.Text = $"VER{string.Join(".", asm.GetFileVersion())}_{buildDateTime:yyMMdd}";

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
            SetButtonBackColor(BtnUserBase, m_colorLabelCategory);
            SetButtonBackColor(BtnCimBase, m_colorLabelCategory);
            SetButtonBackColor(BtnConnectBase, m_colorLabelCategory);
            SetButtonBackColor(BtnStateBase, m_colorLabelCategory);
            SetButtonBackColor(BtnTitleVersionInfo, m_colorLabelCategory);
            SetButtonBackColor(BtnTimer, m_colorLabelCategory);

            PnlTimeBase.BackColor = m_colorLabelCategory;

            SetButtonBackColor(BtnLogout, m_colorBlue);

            SetButtonBackColor(BtnLoginLevel, m_colorLabelData);
            SetButtonBackColor(BtnUserID, m_colorLabelData);
            SetButtonBackColor(BtnUserName, m_colorLabelData);
            SetButtonBackColor(BtnTitleCellTracking, m_colorLabelData);
            SetButtonBackColor(BtnTitleTrackingControl, m_colorLabelData);
            SetButtonBackColor(BtnTitleInterlockControl, m_colorLabelData);
            SetButtonBackColor(BtnTitleControl, m_colorLabelData);
            SetButtonBackColor(BtnTitleAvailability, m_colorLabelData);
            SetButtonBackColor(BtnTitleInterlock, m_colorLabelData);
            SetButtonBackColor(BtnTitleMove, m_colorLabelData);
            SetButtonBackColor(BtnTitleRun, m_colorLabelData);
            SetButtonBackColor(BtnVersionInfo, m_colorLabelData);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnTitleDoor.Name)
                    && false == btn.Name.Equals(BtnCellTracking.Name)
                    && false == btn.Name.Equals(BtnTrackingControl.Name)
                    && false == btn.Name.Equals(BtnInterlockControl.Name)
                    && false == btn.Name.Equals(BtnVersionInfo.Name)
                    && false == btn.Name.Equals(BtnLogout.Name)
                    && false == btn.Name.Equals(BtnMinimizeWindow.Name)
                    && false == btn.Name.Equals(BtnTowerLampBuzzer.Name)
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
        /// 언어 변경
        /// </summary>
        /// <returns></returns>
        public bool SetChangeLanguage()
        {
            bool bReturn = false;

            do
            {
                // 데이터 테이블에서 일치하는 ID에 해당하는 TEXT를 불러옴            
                SetButtonChangeLanguage(BtnTitleVersionInfo);
                SetButtonChangeLanguage(BtnUserBase);
                SetButtonChangeLanguage(BtnCimBase);
                SetButtonChangeLanguage(BtnConnectBase);
                SetButtonChangeLanguage(BtnStateBase);

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
        /// 로그인 정보 갱신
        /// </summary>
        private void SetUpdateLogin()
        {
            CUserInformation objUserInformation = m_objDocument.GetUserInformation();
            Color bgColor = m_colorLabelData;
            // 로그인 정보
            string strAuthorityLevel = "";
            if (CDefine.EUserAuthorityLevel.OPERATOR == objUserInformation.m_eAuthorityLevel)
            {
                strAuthorityLevel = "OPERATOR";
                bgColor = m_colorLabelData;
            }
            else if (CDefine.EUserAuthorityLevel.ENGINEER == objUserInformation.m_eAuthorityLevel)
            {
                strAuthorityLevel = "ENGINEER";
                bgColor = m_colorYellow;
            }
            else if (CDefine.EUserAuthorityLevel.MASTER == objUserInformation.m_eAuthorityLevel)
            {
                strAuthorityLevel = "MASTER";
                bgColor = m_colorRed;

                if (true == m_objDocument.IsMasterLogin)
                {
                    if (mBlinkTick > 10)
                    {
                        bgColor = m_colorBlue;
                    }
                    else
                    {
                        bgColor = m_colorRed;
                    }
                }
            }
            else
            {
                strAuthorityLevel = "NONE";
                bgColor = m_colorClick;
            }
            base.SetButtonText(BtnLoginLevel, strAuthorityLevel);
            base.SetButtonText(BtnUserID, objUserInformation.m_strID);
            base.SetButtonText(BtnUserName, objUserInformation.m_strName);
            SetButtonText(BtnLogout, string.Format("LOGOUT [{0}]", m_objDocument.m_objProcessMain.m_objProcessLogin.GetAutoLogoutLesstimeString()));
            SetButtonBackColor(BtnLoginLevel, bgColor);
            SetButtonBackColor(BtnUserID, bgColor);
            SetButtonBackColor(BtnUserName, bgColor);
        }

        /// <summary>
        /// 시계 정보 갱신
        /// </summary>
        private void SetUpdateTime()
        {
            // 시계
            DateTime dt = DateTime.Now;
            DateTime koreaDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dt, "Korea Standard Time");
            lblLocalDate.Text = $"{koreaDateTime:yyyy-MM-dd}";
            lblLocalTime.Text = $"{koreaDateTime:HH:mm:ss}";

            // 베트남 시간
            // TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dt, "SE Asia Standard Time") 
        }

        private void SetTowerLampStatus()
        {
            SetButtonBackColor(BtnTowerLampGreen, (true == m_objDocument.GetIOBit(CDeviceIODefine.EDigitalOutput.Y_TOWER_LAMP_GREEN)) ? Color.LawnGreen : Color.Snow);
            SetButtonBackColor(BtnTowerLampYellow, (true == m_objDocument.GetIOBit(CDeviceIODefine.EDigitalOutput.Y_TOWER_LAMP_YELLOW)) ? Color.Orange : Color.Snow);
            SetButtonBackColor(BtnTowerLampRed, (true == m_objDocument.GetIOBit(CDeviceIODefine.EDigitalOutput.Y_TOWER_LAMP_RED)) ? Color.Crimson : Color.Snow);
            SetButtonBackColor(BtnTowerLampBuzzer, (CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus()) ? Color.DarkRed : Color.Snow);
        }

        /// <summary>
        /// Cell Tracking 정보 갱신
        /// </summary>
        private void SetUpdateCellTracking()
        {
            base.SetButtonText(BtnCellTracking, m_objDocument.m_objConfig.GetCimParameter().strEFValue[(int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_TRACKING]);
            if (CDialogEQPFunctionList.EFSTOnOff.ON.ToString() == m_objDocument.m_objConfig.GetCimParameter().strEFValue[(int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_TRACKING])
            {
                SetButtonBackColor(BtnCellTracking, m_colorOn);
            }
            else
            {
                SetButtonBackColor(BtnCellTracking, Color.LightGray);
            }
        }

        /// <summary>
        /// Tracking Control 정보 갱신
        /// </summary>
        private void SetUpdateTrackingControl()
        {
            base.SetButtonText(BtnTrackingControl, m_objDocument.m_objConfig.GetCimParameter().strEFValue[(int)CDialogEQPFunctionList.EFName.EF_NAME_TRACKING_CONTROL]);
            if (CDialogEQPFunctionList.EFSTTrackingControl.NOTHING.ToString() != m_objDocument.m_objConfig.GetCimParameter().strEFValue[(int)CDialogEQPFunctionList.EFName.EF_NAME_TRACKING_CONTROL])
            {
                SetButtonBackColor(BtnTrackingControl, m_colorOn);
            }
            else
            {
                SetButtonBackColor(BtnTrackingControl, Color.LightGray);
            }
        }

        /// <summary>
        /// Interlock Control 정보 갱신
        /// </summary>
        private void SetUpdateInterlockControl()
        {
            base.SetButtonText(BtnInterlockControl, m_objDocument.m_objConfig.GetCimParameter().strEFValue[(int)CDialogEQPFunctionList.EFName.EF_NAME_INTERLOCK_CONTROL]);
            SetButtonBackColor(BtnInterlockControl, m_colorOn);
        }

        /// <summary>
        /// Control 정보 갱신
        /// </summary>
        private void SetUpdateControl()
        {
            switch (m_objDocument.m_eControlState)
            {
                case CCIMDefine.EControlState.CRST_OFFLINE:
                    base.SetButtonText(BtnControl, "OFFLINE");
                    SetButtonBackColor(BtnControl, m_colorOff);
                    break;
                case CCIMDefine.EControlState.CRST_ONLINE_REMOTE:
                    base.SetButtonText(BtnControl, "REMOTE");
                    SetButtonBackColor(BtnControl, m_colorOn);
                    break;
                case CCIMDefine.EControlState.CRST_ONLINE_LOCAL:
                    base.SetButtonText(BtnControl, "LOCAL");
                    SetButtonBackColor(BtnControl, m_colorOn);
                    break;
                default:
                    base.SetButtonText(BtnControl, "DOWN");
                    SetButtonBackColor(BtnControl, m_colorOff);
                    break;
            }
        }

        /// <summary>
        /// 기타 on & off 상태 정보 갱신
        /// </summary>
        private void SetUpdateStatus()
        {
            // CIM 상태 체크 
            if (true == m_objDocument.m_objConfig.GetOptionParameter().bUseCIM)
            {
                if (true == m_objDocument.m_bCIMConnected)
                {
                    base.SetButtonBackColor(this.BtnTitleCim, m_colorOn);
                }
                else
                {
                    base.SetButtonBackColor(this.BtnTitleCim, m_colorOff);
                }
            }
            else
            {
                base.SetButtonBackColor(this.BtnTitleCim, Color.LightGray);
            }
            // Availability 상태 체크
            if (m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE] == CCIMDefine.EAvailabilityState.AVAILABILITY_STATE_UP)
            {
                SetButtonBackColor(BtnTitleAvailabilityUp, m_colorOn);
                SetButtonBackColor(BtnTitleAvailabilityDown, m_colorNormal);
            }
            else
            {
                SetButtonBackColor(BtnTitleAvailabilityUp, m_colorNormal);
                SetButtonBackColor(BtnTitleAvailabilityDown, m_colorOff);
            }
            // PP / SPL 상태 체크
            if (m_objDocument.m_ePP_SPLState == CCIMDefine.EPP_SPLState.PP_SPL_STATE_NORMAL)
            {
                base.SetButtonBackColor(this.BtnTitleSample, m_colorOn);
            }
            else
            {
                base.SetButtonBackColor(this.BtnTitleSample, m_colorOff);
            }
            // Interlock 상태 체크
            if (m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE] == CCIMDefine.EInterlockState.INTERLOCK_STATE_ON)
            {
                SetButtonBackColor(BtnTitleInterlockOn, m_colorOff);
                SetButtonBackColor(BtnTitleInterlockOff, m_colorNormal);
            }
            else
            {
                SetButtonBackColor(BtnTitleInterlockOn, m_colorNormal);
                SetButtonBackColor(BtnTitleInterlockOff, m_colorOn);
            }
            // Move 상태 체크
            if (m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE] == CCIMDefine.EMoveState.MOVE_STATE_RUNNING)
            {
                SetButtonBackColor(BtnTitleMoveRunning, m_colorOn);
                SetButtonBackColor(BtnTitleMovePause, m_colorNormal);
            }
            else
            {
                SetButtonBackColor(BtnTitleMoveRunning, m_colorNormal);
                SetButtonBackColor(BtnTitleMovePause, m_colorOff);
            }
            // RUN 상태 체크
            if (m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE] == CCIMDefine.ERunState.RUN_STATE_RUN)
            {
                SetButtonBackColor(BtnTitleRunRun, m_colorOn);
                SetButtonBackColor(BtnTitleRunIdle, m_colorNormal);
            }
            else
            {
                SetButtonBackColor(BtnTitleRunRun, m_colorNormal);
                SetButtonBackColor(BtnTitleRunIdle, m_colorOff);
            }

            // 도어 상태 확인
            bool bDoorClose = DoorManager.Doors.Values.All(item => item.IsOpened == false);
            bool bDoorLock = DoorManager.Doors.Values.All(item => item.IsLocked == true);
            bool bPlcDoorClose = m_objDocument.m_objProcessMain.GetSafetyIO().IsSafetyAllDoorClosed();
            if (false == bDoorClose || false == bPlcDoorClose)
            {
                // Door Open
                SetButtonBackColor(BtnTitleDoor, m_colorOff);
            }
            else if (false == bDoorLock)
            {
                // Door Unlock
                SetButtonBackColor(BtnTitleDoor, m_colorYellow);
            }
            else
            {
                // Door Close
                SetButtonBackColor(BtnTitleDoor, m_colorOn);
            }

            bool bEnableSwitch = m_objDocument.m_objProcessMain.GetSafetyIO().IsAnyEnableSwitchGripped();
            if (true == bEnableSwitch)
            {
                // Enable Switch Grip
                SetButtonBackColor(BtnEnableSwitchGrip, m_colorOn);
            }
            else
            {
                SetButtonBackColor(BtnEnableSwitchGrip, m_colorOff);
            }
        }

        /// <summary>
        /// 디바이스 연결 상태 갱신
        /// </summary>
        private void SetUpdateDeviceConnected()
        {
            // Align
            SetControlBackColor(BtnTitleConnectPreAlign, GetAlignStatusColor(CDefine.EAlign.PreAlign));
            // Temperature
            SetConnectedBackColor(this.BtnTitleConnectTemp1, m_objDocument.m_objProcessMain.m_objTemperature[CDefine.ETemperatureController.MainGruop].HLIsConnected() && CProcessMain.SerialReaders.Temperature[CDefine.ETemperature.MainElectronicBox].IsReadingFail == false);
            SetConnectedBackColor(this.BtnTitleConnectTemp2, m_objDocument.m_objProcessMain.m_objTemperature[CDefine.ETemperatureController.MainGruop].HLIsConnected() && CProcessMain.SerialReaders.Temperature[CDefine.ETemperature.PcRack].IsReadingFail == false);
            // Vision
            SetControlBackColor(BtnTitleConnectVision1, GetVisionStatusColor(CDefine.EInspectType.Main, CDefine.EInspInterface.PC1));
            // FFU
            {
                bool bFfuIsConnected = m_objDocument.m_objProcessMain.m_objFFU[CDefine.EMcu.MainGruop].HLIsConnected() && CProcessMain.SerialReaders.Mcu[CDefine.EMcu.MainGruop].IsReadingFail == false;
                if (bFfuIsConnected == true)
                {
                    if (mFfuItems == null)
                    {
                        mFfuItems = Enum.GetNames(typeof(CDatabaseDefine.ESvidList))
                            .Where(item => item.StartsWith("EFU_RPM"))
                            .Select(item => (CDatabaseDefine.ESvidList)Enum.Parse(typeof(CDatabaseDefine.ESvidList), item))
                            .ToArray();
                    }
                    // 포트가 열렸으면 회전수를 확인하여 색을 변경한다.
                    List<object> objSVIDList = m_objDocument.GetSVIDList();
                    if (objSVIDList.Count == 0
                        || mFfuItems.Any(item => (double)objSVIDList[(int)item] < m_objDocument.mEfuRunningRpm) == true
                        )
                    {
                        SetConnectedBackColor(BtnTitleFfu, false);
                    }
                    else
                    {
                        SetConnectedBackColor(BtnTitleFfu, true);
                    }
                }
                else
                {
                    SetConnectedBackColor(BtnTitleFfu, false);
                }
            }
            // ACCURA
            SetConnectedBackColor(this.BtnTitleAccuraGps, m_objDocument.m_objProcessMain.m_objPowerMeter[CDefine.EPowerMeter.POWER_METER_GPS].HLIsConnected());
            SetConnectedBackColor(this.BtnTitleAccuraUps, m_objDocument.m_objProcessMain.m_objPowerMeter[CDefine.EPowerMeter.POWER_METER_UPS].HLIsConnected());
            // MCR
            SetConnectedBackColor(BtnTitleConnectMcrP1, m_objDocument.m_objProcessMain.m_objMcr[CDefine.EMcr.P1].HLIsConnected());
            SetConnectedBackColor(BtnTitleConnectMcrP2, m_objDocument.m_objProcessMain.m_objMcr[CDefine.EMcr.P2].HLIsConnected());
        }

        private Color GetVisionStatusColor(CDefine.EInspectType type, CDefine.EInspInterface index)
        {
            if (m_objDocument.m_objConfig.GetInspOptionParameter(type).eInspectionMode == CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE)
            {
                return Color.LightGray;
            }
            if (m_objDocument.m_objProcessMain.m_objInspInterfaces[index].HLIsConnected() == false)
            {
                return m_colorOff;
            }
            return m_colorOn;
        }

        private Color GetAlignStatusColor(CDefine.EAlign alignIndex)
        {
            if (m_objDocument.m_objConfig.GetAlignOptionParameter(alignIndex).bUseVision == false)
            {
                return Color.LightGray;
            }

            if (m_objDocument.m_objProcessMain.m_objAlignInterfaces[alignIndex].HLIsConnected() == false)
            {
                return m_colorOff;
            }

            return m_colorOn;
        }

        private void SetControlBackColor(Control control, Color color)
        {
            if (control.BackColor == color)
            {
                return;
            }
            control.BackColor = color;
        }

        /// <summary>
        /// 디바이스 연결 상태 체크
        /// </summary>
        /// <param name="objButton"></param>
        /// <param name="bConnected"></param>
        private void SetConnectedBackColor(Button objButton, bool bConnected)
        {
            if (true == bConnected)
            {
                SetButtonBackColor(objButton, m_colorOn);
            }
            else
            {
                SetButtonBackColor(objButton, m_colorOff);
            }
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // 로그인 정보 갱신
            SetUpdateLogin();
            // 시계 정보 갱신
            SetUpdateTime();
            // Cell Tracking 정보 갱신
            SetUpdateCellTracking();
            // Tracking Control 정보 갱신
            SetUpdateTrackingControl();
            // Interlock Control 정보 갱신
            SetUpdateInterlockControl();
            // Control 정보 갱신
            SetUpdateControl();
            // 기타 on & off 상태 정보 갱신
            SetUpdateStatus();
            // 디바이스 연결 상태 갱신
            SetUpdateDeviceConnected();
            // 타워램프 상태 갱신
            SetTowerLampStatus();

            // 런모드 타입에 따라서 배경색을 변경함
            Color normalBackColor = m_objDocument.m_objConfig.UsingVirtualConfig ? mVirtualBackColor : mNormalBackColor;
            if (
                m_objDocument.GetRunMode() == CDefine.ERunMode.RealRun
                || mBlinkTick > 10
                )
            {
                if (normalBackColor != BackColor)
                {
                    BackColor = normalBackColor;
                }
                mBlinkTick++;
                if (mBlinkTick > 20)
                {
                    mBlinkTick = 0;
                }
            }
            else
            {
                if (mDryRunBackColor != BackColor)
                {
                    BackColor = mDryRunBackColor;
                }
                mBlinkTick++;
            }
        }

        /// <summary>
        /// 로고 클릭 시 로그인 다이얼로그 띄워줌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SamsungLogo_Click(object sender, EventArgs e)
        {
            m_objDocument.m_objProcessMain.IsSamsungLogoClick = true;
        }

        /// <summary>
        /// EQP 기능 리스트 다이얼로그 띄워줌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFunctionList_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnFunctionList_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (CDialogEQPFunctionList objDialogEQPFunctionList = new CDialogEQPFunctionList(m_objDocument))
            {
                objDialogEQPFunctionList.ShowDialog();
            }

            // 버튼 로그 추가
            strLog = string.Format("[{0}] [{1}]", "BtnFunctionList_Click", false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 버전정보 클릭시 로그인 다이얼로그 띄워줌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnVersionInfo_Click(object sender, EventArgs e)
        {
            do
            {
                // 설비 시작 상태면 튕김
                if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                {
                    break;
                }
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "SamsungLogo_Click", true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                // 현재 유저 정보 & 설비 상태를 넘겨줌
                m_objDocument.GetMainFrame().m_objDialogLogin.IsCheckOnlyMode = true;
                m_objDocument.GetMainFrame().m_objDialogLogin.ShowDialog();
                if (true == m_objDocument.GetMainFrame().m_objDialogLogin.IsCanceled)
                {
                    break;
                }

                // 장비 현황 판
                CDialogEQState objEQState = new CDialogEQState(m_objDocument);
                objEQState.Show();

                // 버튼 로그 추가
                strLog = string.Format("[{0}] [{1}]", "SamsungLogo_Click", false);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
            m_objDocument.GetMainFrame().m_objDialogLogin.IsCheckOnlyMode = false;
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnLogout_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            m_objDocument.SetLogout();

            // 버튼 로그 추가
            strLog = string.Format("[{0}] [{1}]", "BtnLogout_Click", false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnEnableSwitchGrip_Click(object sender, EventArgs e)
        {
            if (CDefine.ESimulationMode.SIMULATION_MODE_OFF == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                return;
            }

            bool currentSignal = m_objDocument.m_objProcessMain.GetSafetyIO().IsAnyEnableSwitchGripped();
            foreach (var index in m_objDocument.m_objProcessMain.GetSafetyIO().EnableSwitchGrips)
            {
                m_objDocument.SetIOBit(index, !currentSignal);
            }
        }

        private void btnMinimizeWindow_Click(object sender, EventArgs e)
        {
            var mainFrame = m_objDocument.GetMainFrame();
            if (mainFrame == null)
            {
                return;
            }
            mainFrame.WindowState = FormWindowState.Minimized;
        }

        private void BtnTowerLampBuzzer_Click(object sender, EventArgs e)
        {
            do
            {
                // 알람 없는 경우
                if (false == m_objDocument.GetIsAlarmMessage())
                {
                    m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.IsForceDoorBuzzerOffRequest = !m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.IsForceDoorBuzzerOffRequest;
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
                    break;
                }

                if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus())
                {
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
                    m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.IsForceDoorBuzzerOffRequest = true;
                }
                else
                {
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
                    m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.IsForceDoorBuzzerOffRequest = false;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0} : {1}]", "BtnBuzzerOff_Click", m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus().ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }
    }
}