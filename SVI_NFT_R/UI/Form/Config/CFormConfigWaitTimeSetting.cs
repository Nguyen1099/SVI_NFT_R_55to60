using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UiAsset;

namespace SVI_M_F
{
    public partial class CFormConfigWaitTimeSetting : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private readonly CDocument m_objDocument;
        /// <summary>
        /// 버튼 최대 수 (디자이너에서 만들어져 있는 최대 수)
        /// </summary>
        private const int m_iMaxButtonCount = 13;
        /// <summary>
        /// 실린더 대기 시간 버튼 객체
        /// </summary>
        private CWaitTimeButton[] m_objEqInterfaceButton;
        /// <summary>
        /// ETC 대기 시간 버튼 객체
        /// </summary>
        private CWaitTimeButton[] m_objEtcButton;
        /// <summary>
        /// Comm 타임아웃 시간 버튼 객체
        /// </summary>
        private CWaitTimeButton[] m_objCommTimeoutButton;
        /// <summary>
        /// 설비인터페이스 객체 리스트
        /// </summary>
        private List<CTimeData> m_objEQInterface;
        /// <summary>
        /// ETC 객체 리스트
        /// </summary>
        private List<CTimeData> m_objETC;
        /// <summary>
        /// ETC 객체 리스트
        /// </summary>
        private List<CTimeData> m_objCommTimeout;
        private readonly bool mbDisplayTimeUnitSec = true;

        /// <summary>
        /// 대기 시간 버튼 클래스 정의
        /// </summary>
        private class CWaitTimeButton
        {
            /// <summary>
            /// 이름 버튼
            /// </summary>
            public SpeedButton m_btnName;
            /// <summary>
            /// 시간 버튼
            /// </summary>
            public SpeedButton m_btnTime;

            public CWaitTimeButton()
            {
                m_btnName = new SpeedButton();
                m_btnTime = new SpeedButton();
            }
        }

        public class CTimeData
        {
            public string strBtnName;
            public int iTimeOut;
            public CTimeData(string strName, int iValue)
            {
                this.strBtnName = strName;
                this.iTimeOut = iValue;
            }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormConfigWaitTimeSetting(CDocument objDocument)
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
        private void CFormConfigWaitTimeSetting_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormConfigWaitTimeSetting_FormClosed(object sender, FormClosedEventArgs e)
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

                // 버튼 생성
                {
                    // INTERFACE TIME
                    m_objEQInterface = new List<CTimeData>();
                    CTimeData objEqInterfaceData = new CTimeData("LOAD INTERFACE CHECK LOSS DELAY TIME", m_objDocument.m_objConfig.GetEqTimeParameter().iLoadInterfaceDelayTime);
                    m_objEQInterface.Add(objEqInterfaceData);
                    objEqInterfaceData = new CTimeData("UNLOAD INTERFACE CHECK LOSS DELAY TIME", m_objDocument.m_objConfig.GetEqTimeParameter().iUnloadInterfaceDelayTime);
                    m_objEQInterface.Add(objEqInterfaceData);
                    // ETC TIME
                    m_objETC = new List<CTimeData>();
                    // COMM TIMEOUT
                    m_objCommTimeout = new List<CTimeData>();
                    CTimeData
                    objCommTimeoutData = new CTimeData("SVI REPLY TIMEOUT", m_objDocument.m_objConfig.GetCommTimeoutParameter().iSviReplyTimeout);
                    m_objCommTimeout.Add(objCommTimeoutData);
                    objCommTimeoutData = new CTimeData("SVI GRAB END REPLY TIMEOUT", m_objDocument.m_objConfig.GetCommTimeoutParameter().iSviGrabEndReplyTimeout);
                    m_objCommTimeout.Add(objCommTimeoutData);
                    objCommTimeoutData = new CTimeData("SVI TOTAL RESULT TIMEOUT", m_objDocument.m_objConfig.GetCommTimeoutParameter().iSviTotalResultTimeout);
                    m_objCommTimeout.Add(objCommTimeoutData);
                    objCommTimeoutData = new CTimeData("ROBOT TIMEOUT", m_objDocument.m_objConfig.GetCommTimeoutParameter().iRobotTimeout);
                    m_objCommTimeout.Add(objCommTimeoutData);
                    objCommTimeoutData = new CTimeData("ALIGN VISION TIMEOUT", m_objDocument.m_objConfig.GetCommTimeoutParameter().iAlignVisionTimeout);
                    m_objCommTimeout.Add(objCommTimeoutData);
                    objCommTimeoutData = new CTimeData("CIM TRACKING TIMEOUT", m_objDocument.m_objConfig.GetCommTimeoutParameter().iCimTrackingTimeout);
                    m_objCommTimeout.Add(objCommTimeoutData);
                    objCommTimeoutData = new CTimeData("CIM LOGIN TIMEOUT", m_objDocument.m_objConfig.GetCommTimeoutParameter().iCimLoginTimeout);
                    m_objCommTimeout.Add(objCommTimeoutData);
                    // MOTOR TIME (NOT USED)
                    // 버튼 여백 & 넓이 설정
                    int iWhiteSpace = 6;
                    m_objEqInterfaceButton = new CWaitTimeButton[m_objEQInterface.Count];
                    m_objEtcButton = new CWaitTimeButton[m_objETC.Count];
                    m_objCommTimeoutButton = new CWaitTimeButton[m_objCommTimeout.Count];
                    // 버튼 동적 생성
                    SetDynamicButton(m_objEqInterfaceButton, BtnEQInterfaceName, BtnEQInterfaceTime, iWhiteSpace, new EventHandler(ButtonEQInterfaceTime_Click), "EqInterface");
                    SetDynamicButton(m_objEtcButton, BtnEtcName, BtnEtcTime, iWhiteSpace, new EventHandler(ButtonEtcTime_Click), "Etc");
                    SetDynamicButton(m_objCommTimeoutButton, BtnCommTimeoutName, BtnCommTimeoutTime, iWhiteSpace, new EventHandler(ButtonCommTimeout_Click), "CommTimeout");
                }

                // Panel 설정
                {
                    // EQ INTERFACE
                    var nameControlsQuary = m_objEqInterfaceButton.Select(item => item.m_btnName);
                    var timeControlsQuary = m_objEqInterfaceButton.Select(item => item.m_btnTime);
                    PnlEqInterfaceTime.Controls.AddRange(nameControlsQuary.ToArray());
                    PnlEqInterfaceTime.Controls.AddRange(timeControlsQuary.ToArray());
                    // ETC TIME
                    nameControlsQuary = m_objEtcButton.Select(item => item.m_btnName);
                    timeControlsQuary = m_objEtcButton.Select(item => item.m_btnTime);
                    PnlEtcTime.Controls.AddRange(nameControlsQuary.ToArray());
                    PnlEtcTime.Controls.AddRange(timeControlsQuary.ToArray());
                    // COMM TIMEOUT
                    nameControlsQuary = m_objCommTimeoutButton.Select(item => item.m_btnName);
                    timeControlsQuary = m_objCommTimeoutButton.Select(item => item.m_btnTime);
                    PnlCommTimeout.Controls.AddRange(nameControlsQuary.ToArray());
                    PnlCommTimeout.Controls.AddRange(timeControlsQuary.ToArray());
                    // MOTOR TIME (NOT USED)
                    PnlMotorTime.Visible = false;
                }
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
            base.SetButtonBackColor(this.BtnTitleEQInterfaceTime, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleCommTimeout, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleMotorTime, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleEtcTime, m_colorLabel);

            for (int iLoopEQInterface = 0; iLoopEQInterface < m_objEQInterface.Count; iLoopEQInterface++)
            {
                base.SetButtonBackColor(this.m_objEqInterfaceButton[iLoopEQInterface].m_btnName, m_colorLabelSub);
                base.SetButtonBackColor(this.m_objEqInterfaceButton[iLoopEQInterface].m_btnTime, m_colorLabelData);
            }
            for (int iLoopETC = 0; iLoopETC < m_objETC.Count; iLoopETC++)
            {
                base.SetButtonBackColor(this.m_objEtcButton[iLoopETC].m_btnName, m_colorLabelSub);
                base.SetButtonBackColor(this.m_objEtcButton[iLoopETC].m_btnTime, m_colorLabelData);
            }
            for (int iLoopCommTimeout = 0; iLoopCommTimeout < m_objCommTimeout.Count; iLoopCommTimeout++)
            {
                SetButtonBackColor(m_objCommTimeoutButton[iLoopCommTimeout].m_btnName, m_colorLabelSub);
                SetButtonBackColor(m_objCommTimeoutButton[iLoopCommTimeout].m_btnTime, m_colorLabelData);
            }

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    //&& false == btn.Name.Equals("")
                    )
                {
                    bool bContinue = false;
                    for (int iLoopEQInterface = 0; iLoopEQInterface < m_objEQInterface.Count; iLoopEQInterface++)
                    {
                        if (true == btn.Name.Equals(m_objEqInterfaceButton[iLoopEQInterface].m_btnTime.Name))
                        {
                            bContinue = true;
                            break;
                        }
                    }
                    for (int iLoopETC = 0; iLoopETC < m_objETC.Count; iLoopETC++)
                    {
                        if (true == btn.Name.Equals(m_objEtcButton[iLoopETC].m_btnTime.Name))
                        {
                            bContinue = true;
                            break;
                        }
                    }
                    for (int iLoopCommTimeout = 0; iLoopCommTimeout < m_objCommTimeout.Count; iLoopCommTimeout++)
                    {
                        if (true == btn.Name.Equals(m_objCommTimeoutButton[iLoopCommTimeout].m_btnTime.Name))
                        {
                            bContinue = true;
                            break;
                        }
                    }
                    if (true == bContinue)
                    {
                        continue;
                    }

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

            // 런타임 중에도 변경 가능함
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        base.SetControlButtonEnable(this.Controls, false);
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                        {
                            base.SetControlButtonEnable(this.Controls, false);
                        }
                        else
                        {
                            base.SetControlButtonEnable(this.Controls, true);
                        }
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
                SetButtonChangeLanguage(this.BtnTitleEQInterfaceTime);
                SetButtonChangeLanguage(this.BtnTitleCommTimeout);
                SetButtonChangeLanguage(this.BtnTitleMotorTime);
                SetButtonChangeLanguage(this.BtnTitleEtcTime);

                foreach (var item in m_objEqInterfaceButton)
                {
                    SetButtonChangeLanguage(item.m_btnName);
                }
                foreach (var item in m_objEtcButton)
                {
                    SetButtonChangeLanguage(item.m_btnName);
                }
                foreach (var item in m_objCommTimeoutButton)
                {
                    SetButtonChangeLanguage(item.m_btnName);
                }

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
            }
        }

        /// <summary>
        /// 버튼 동적 생성
        /// 설명 : Target 버튼을 기준으로 버튼이 이래 방향으로 생성
        /// </summary>
        /// <param name="objWaitTime"></param>
        /// <param name="objTargetNameButton"></param>
        /// <param name="objTargetTimeButton"></param>
        /// <param name="iWhiteSpace"></param>
        /// <param name="eEvent"></param>
        private void SetDynamicButton(CWaitTimeButton[] objWaitTime, Button objTargetNameButton, Button objTargetTimeButton, int iWhiteSpace, EventHandler eEvent, string categoryIndex)
        {
            for (int iLoopWaitTime = 0; iLoopWaitTime < objWaitTime.Length; iLoopWaitTime++)
            {
                if (null == objWaitTime[iLoopWaitTime])
                {
                    objWaitTime[iLoopWaitTime] = new CWaitTimeButton();
                }
                // 이름 버튼
                objWaitTime[iLoopWaitTime].m_btnName.Name = string.Format("BtnTitleWaitTime{0}{1}", categoryIndex, iLoopWaitTime);
                objWaitTime[iLoopWaitTime].m_btnName.Text = iLoopWaitTime.ToString();
                objWaitTime[iLoopWaitTime].m_btnName.Parent = this;
                objWaitTime[iLoopWaitTime].m_btnName.Size = objTargetNameButton.Size;
                objWaitTime[iLoopWaitTime].m_btnName.BackColor = Color.White;
                objWaitTime[iLoopWaitTime].m_btnName.FlatStyle = FlatStyle.Flat;
                objWaitTime[iLoopWaitTime].m_btnName.Tag = iLoopWaitTime;
                objWaitTime[iLoopWaitTime].m_btnName.Location = new Point(objTargetNameButton.Location.X, (iLoopWaitTime * (objTargetNameButton.Height + iWhiteSpace)) + objTargetNameButton.Location.Y);
                // 시간 버튼
                objWaitTime[iLoopWaitTime].m_btnTime.Name = string.Format("BtnValueWaitTime{0}{1}", categoryIndex, iLoopWaitTime);
                objWaitTime[iLoopWaitTime].m_btnTime.Text = iLoopWaitTime.ToString();
                objWaitTime[iLoopWaitTime].m_btnTime.Parent = this;
                objWaitTime[iLoopWaitTime].m_btnTime.Size = objTargetTimeButton.Size;
                objWaitTime[iLoopWaitTime].m_btnTime.BackColor = Color.White;
                objWaitTime[iLoopWaitTime].m_btnTime.FlatStyle = FlatStyle.Flat;
                objWaitTime[iLoopWaitTime].m_btnTime.Location = new Point(objTargetTimeButton.Location.X, (iLoopWaitTime * (objTargetTimeButton.Height + iWhiteSpace)) + objTargetTimeButton.Location.Y);
                objWaitTime[iLoopWaitTime].m_btnTime.Tag = iLoopWaitTime;
                objWaitTime[iLoopWaitTime].m_btnTime.Click += eEvent;
            }

            // 컨트롤 리스트 등록
            Controls.AddRange(objWaitTime.Select(item => item.m_btnName).ToArray());
            Controls.AddRange(objWaitTime.Select(item => item.m_btnTime).ToArray());
        }

        /// <summary>
        /// Eq 인터페이스 버튼 클릭 이벤트 정의
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonEQInterfaceTime_Click(object sender, EventArgs e)
        {
            Button objButton = sender as Button;
            try
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}] [{2}]", "ButtonEQInterfaceTime_Click", m_objEQInterface[Convert.ToInt32(objButton.Tag)].strBtnName, true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                string replaceUnit = mbDisplayTimeUnitSec ? "( sec )" : "( ㎳ )";
                FormKeyPad objKeyPad = new FormKeyPad(double.Parse(objButton.Text.Replace(replaceUnit, string.Empty)));
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 버튼 내 지정 인덱스로 접근해서 데이터를 적어줌
                    lock (m_objEQInterface[Convert.ToInt32(objButton.Tag)])
                    {
                        if (objKeyPad.m_dResultValue < 0)
                        {
                            objKeyPad.m_dResultValue = 0;
                        }

                        CConfig.CEqTimeParameter objEQTimeParamter = m_objDocument.m_objConfig.GetEqTimeParameter().DeepClone();
                        // 버튼 로그 추가
                        strLog = string.Format("[{0}] [{1}] [TIME : {2:D} → {3:D}] [{4}]", "ButtonEQInterfaceTime_Click", m_objEQInterface[Convert.ToInt32(objButton.Tag)].strBtnName, m_objEQInterface[Convert.ToInt32(objButton.Tag)].iTimeOut, (int)objKeyPad.m_dResultValue, false);
                        // 데이터 변경 후 저장.
                        m_objEQInterface[Convert.ToInt32(objButton.Tag)].iTimeOut = convertTimeUnit(objKeyPad.m_dResultValue);
                        switch (Convert.ToInt32(objButton.Tag))
                        {
                            case 0:
                                objEQTimeParamter.iLoadInterfaceDelayTime = convertTimeUnit(objKeyPad.m_dResultValue);
                                break;
                            case 1:
                                objEQTimeParamter.iUnloadInterfaceDelayTime = convertTimeUnit(objKeyPad.m_dResultValue);
                                break;
                        }
                        m_objDocument.m_objConfig.SaveEqTimeParameter(objEQTimeParamter);

                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// etc 시간 버튼 클릭 이벤트 정의
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonEtcTime_Click(object sender, EventArgs e)
        {
            Button objButton = sender as Button;
            try
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}] [{2}]", "ButtonEtcTime_Click", m_objETC[Convert.ToInt32(objButton.Tag)].strBtnName, true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                string replaceUnit = mbDisplayTimeUnitSec ? "( sec )" : "( ㎳ )";
                FormKeyPad objKeyPad = new FormKeyPad(double.Parse(objButton.Text.Replace(replaceUnit, string.Empty)));
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 버튼 내 지정 인덱스로 접근해서 데이터를 적어줌
                    lock (m_objETC[Convert.ToInt32(objButton.Tag)])
                    {
                        if (objKeyPad.m_dResultValue < 0)
                        {
                            objKeyPad.m_dResultValue = 0;
                        }

                        CConfig.CEtcTimeParameter objETCTimeParamter = m_objDocument.m_objConfig.GetEtcTimeParameter().DeepClone();
                        // 버튼 로그 추가
                        strLog = string.Format("[{0}] [{1}] [TIME : {2:D} → {3:D}] [{4}]", "ButtonEtcTime_Click", m_objETC[Convert.ToInt32(objButton.Tag)].strBtnName, m_objETC[Convert.ToInt32(objButton.Tag)].iTimeOut, (int)objKeyPad.m_dResultValue, false);
                        // 데이터 변경 후 저장.
                        m_objETC[Convert.ToInt32(objButton.Tag)].iTimeOut = convertTimeUnit(objKeyPad.m_dResultValue);
                        switch (Convert.ToInt32(objButton.Tag))
                        {
                            case 0:
                                //commTimeParameter.iSviReplyTimeout = convertTimeUnit(objKeyPad.m_dResultValue);
                                break;
                        }
                        m_objDocument.m_objConfig.SaveEtcTimeParameter(objETCTimeParamter);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// etc 시간 버튼 클릭 이벤트 정의
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonCommTimeout_Click(object sender, EventArgs e)
        {
            Button objButton = sender as Button;
            try
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}] [{2}]", "ButtonCommTimeout_Click", m_objCommTimeout[Convert.ToInt32(objButton.Tag)].strBtnName, true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                string replaceUnit = mbDisplayTimeUnitSec ? "( sec )" : "( ㎳ )";
                FormKeyPad objKeyPad = new FormKeyPad(double.Parse(objButton.Text.Replace(replaceUnit, string.Empty)));
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 버튼 내 지정 인덱스로 접근해서 데이터를 적어줌
                    lock (m_objCommTimeout[Convert.ToInt32(objButton.Tag)])
                    {
                        if (objKeyPad.m_dResultValue < 0)
                        {
                            objKeyPad.m_dResultValue = 0;
                        }

                        CConfig.CCommTimeoutParameter objCommTimeoutParamter = m_objDocument.m_objConfig.GetCommTimeoutParameter().DeepClone();
                        // 버튼 로그 추가
                        strLog = string.Format("[{0}] [{1}] [TIME : {2:D} → {3:D}] [{4}]", "ButtonCommTimeout_Click", m_objCommTimeout[Convert.ToInt32(objButton.Tag)].strBtnName, m_objCommTimeout[Convert.ToInt32(objButton.Tag)].iTimeOut, (int)objKeyPad.m_dResultValue, false);
                        // 데이터 변경 후 저장.
                        m_objCommTimeout[Convert.ToInt32(objButton.Tag)].iTimeOut = convertTimeUnit(objKeyPad.m_dResultValue);
                        switch (Convert.ToInt32(objButton.Tag))
                        {
                            case 0:
                                objCommTimeoutParamter.iSviReplyTimeout = convertTimeUnit(objKeyPad.m_dResultValue);
                                break;
                            case 1:
                                objCommTimeoutParamter.iSviGrabEndReplyTimeout = convertTimeUnit(objKeyPad.m_dResultValue);
                                break;
                            case 2:
                                objCommTimeoutParamter.iSviTotalResultTimeout = convertTimeUnit(objKeyPad.m_dResultValue);
                                break;
                            case 3:
                                objCommTimeoutParamter.iRobotTimeout = convertTimeUnit(objKeyPad.m_dResultValue);
                                break;
                            case 4:
                                objCommTimeoutParamter.iAlignVisionTimeout = convertTimeUnit(objKeyPad.m_dResultValue);
                                break;
                            case 5:
                                objCommTimeoutParamter.iCimTrackingTimeout = convertTimeUnit(objKeyPad.m_dResultValue);
                                break;
                            case 6:
                                objCommTimeoutParamter.iCimLoginTimeout = convertTimeUnit(objKeyPad.m_dResultValue);
                                break;
                        }
                        m_objDocument.m_objConfig.SaveCommTimeoutParameter(objCommTimeoutParamter);

                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        private int convertTimeUnit(double input)
        {
            if (mbDisplayTimeUnitSec == true)
            {
                return Convert.ToInt32(input * 1000d);
            }
            else
            {
                return Convert.ToInt32(input);
            }
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            for (int iLoopEqInterface = 0; iLoopEqInterface < m_objEQInterface.Count; iLoopEqInterface++)
            {
                if (mbDisplayTimeUnitSec == true)
                {
                    SetButtonText(m_objEqInterfaceButton[iLoopEqInterface].m_btnTime, string.Format("{0:0.000} ( sec )", m_objEQInterface[iLoopEqInterface].iTimeOut / 1000d));
                }
                else
                {
                    SetButtonText(m_objEqInterfaceButton[iLoopEqInterface].m_btnTime, string.Format("{0} ( ㎳ )", m_objEQInterface[iLoopEqInterface].iTimeOut));
                }
            }
            for (int iLoopETC = 0; iLoopETC < m_objETC.Count; iLoopETC++)
            {
                if (mbDisplayTimeUnitSec == true)
                {
                    SetButtonText(m_objEtcButton[iLoopETC].m_btnTime, string.Format("{0:0.000} ( sec )", m_objETC[iLoopETC].iTimeOut / 1000d));
                }
                else
                {
                    SetButtonText(m_objEtcButton[iLoopETC].m_btnTime, string.Format("{0} ( ㎳ )", m_objETC[iLoopETC].iTimeOut));
                }
            }
            for (int iLoopCommTimeout = 0; iLoopCommTimeout < m_objCommTimeout.Count; iLoopCommTimeout++)
            {
                if (mbDisplayTimeUnitSec == true)
                {
                    SetButtonText(m_objCommTimeoutButton[iLoopCommTimeout].m_btnTime, string.Format("{0:0.000} ( sec )", m_objCommTimeout[iLoopCommTimeout].iTimeOut / 1000d));
                }
                else
                {
                    SetButtonText(m_objCommTimeoutButton[iLoopCommTimeout].m_btnTime, string.Format("{0} ( ㎳ )", m_objCommTimeout[iLoopCommTimeout].iTimeOut));
                }
            }
        }
    }
}