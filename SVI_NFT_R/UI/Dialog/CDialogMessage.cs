using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogMessage : CFormCommon, CFormInterface
    {
        /// <summary>도큐먼트</summary>
        private readonly CDocument m_objDocument;
        /// <summary>
        /// 알람 구조체 정보
        /// </summary>
        private CDefine.structureAlarmInformation m_objAlarmInformation;
        // 폰트 크기 설정
        private const double m_dFontSizeTitleAlarmType = 48d;
        private const double m_dFontSizeTitleAlarmEtc = 14.25d;
        private const double m_dFontSizeAlarmDescription = 15d;
        /// <summary>
        /// 알람 메시지 여부
        /// </summary>
        private readonly bool m_bIsAlarmMessage;
        private CDefine.ELanguage mLastLanguage = CDefine.ELanguage.LANGUAGE_FINAL;
        private object[] mMessageArgs;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <param name="objAlarmInformation"></param>
        /// <param name="bIsAlarmMessage"></param>
        /// <returns></returns>
        public CDialogMessage(CDocument objDocument, CDefine.structureAlarmInformation objAlarmInformation, bool bIsAlarmMessage, params object[] args)
        {
            // 도큐먼트 받음
            m_objDocument = objDocument;
            // 알람 구조체 정보 받음
            m_objAlarmInformation = objAlarmInformation;
            // 알람 메시지 여부
            m_bIsAlarmMessage = bIsAlarmMessage;
            mMessageArgs = args;

            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogMessage_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogMessage_FormClosed(object sender, FormClosedEventArgs e)
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
                    break;

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
                // 화면 중앙에서 생성
                CenterToScreen();
                // 텍스트 박스's 초기화
                if (false == InitializeTextBox(TextBoxAlarmTime, Font.Name, m_dFontSizeTitleAlarmEtc))
                {
                    break;
                }
                if (false == InitializeTextBox(TextBoxAlarmCode, Font.Name, m_dFontSizeTitleAlarmEtc))
                {
                    break;
                }
                if (false == InitializeTextBox(TextBoxAlarmObject, Font.Name, m_dFontSizeTitleAlarmEtc))
                {
                    break;
                }
                if (false == InitializeTextBox(TextBoxAlarmPosition, Font.Name, m_dFontSizeTitleAlarmEtc))
                {
                    break;
                }
                // 텍스트 박스 설정 초기화
                if (false == InitializeRichTextBox(RichTextBoxAlarmDescription))
                {
                    break;
                }

                // 알람 시간
                TextBoxAlarmTime.Text = DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT);
                // 알람 코드
                TextBoxAlarmCode.Text = string.Format("{0}", m_objAlarmInformation.iAlarmCode);
                // 알람 오브젝트
                TextBoxAlarmObject.Text = m_objAlarmInformation.strAlarmObject;
                // 알람 위치
                TextBoxAlarmPosition.Text = m_objAlarmInformation.strAlarmFunction;

                // 버튼 색상 정의
                SetButtonColor();
                // 버튼 언어 변경
                SetChangeLanguage();
                // 로그
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, string.Format("[Message_{0}] [Code:{1}] [Msg:{2}]", lblTitleAlarmType.Text, TextBoxAlarmCode.Text, RichTextBoxAlarmDescription.Text));

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
            SetButtonBackColor(BtnTitleAlarmTime, m_colorLabelSub);
            SetButtonBackColor(BtnTitleAlarmCode, m_colorLabelSub);
            SetButtonBackColor(BtnTitleAlarmObject, m_colorLabelSub);
            SetButtonBackColor(BtnTitleAlarmPosition, m_colorLabelSub);
            SetButtonBackColor(BtnTitleAlarmDescription, m_colorLabelSub);

            SetButtonBackColor(BtnYes, m_colorNormal);
            SetButtonBackColor(BtnNo, m_colorNormal);
            SetButtonBackColor(BtnLanguage, m_colorNormal);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (UiAsset.SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    //&& false == btn.Name.Equals("")
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
                // 알람 타입
                switch (m_objAlarmInformation.eAlarmLevel)
                {
                    case CDefine.EAlarmType.ALARM_INFORMATION:
                        lblTitleAlarmType.Text = m_objDocument.GetDatabaseUIText("TextBoxTitleAlarmTypeInformation", Name);
                        break;
                    case CDefine.EAlarmType.ALARM_WARNING:
                        lblTitleAlarmType.Text = m_objDocument.GetDatabaseUIText("TextBoxTitleAlarmTypeWarning", Name);
                        break;
                    case CDefine.EAlarmType.ALARM_ALARM:
                        lblTitleAlarmType.Text = m_objDocument.GetDatabaseUIText("TextBoxTitleAlarmTypeAlarm", Name);
                        break;
                    case CDefine.EAlarmType.ALARM_INTERLOCK:
                        lblTitleAlarmType.Text = m_objDocument.GetDatabaseUIText("TextBoxTitleAlarmTypeInterlock", Name);
                        break;
                    case CDefine.EAlarmType.ALARM_QUESTION:
                        lblTitleAlarmType.Text = m_objDocument.GetDatabaseUIText("TextBoxTitleAlarmTypeQuestion", Name);
                        break;
                    default:
                        break;
                }
                // 알람 시간
                SetButtonChangeLanguage(BtnTitleAlarmTime, "TextBoxTitleAlarmTime");
                // 알람 코드
                SetButtonChangeLanguage(BtnTitleAlarmCode, "TextBoxTitleAlarmCode");
                // 알람 객체
                SetButtonChangeLanguage(BtnTitleAlarmObject, "TextBoxTitleAlarmObject");
                // 알람 위치
                SetButtonChangeLanguage(BtnTitleAlarmPosition, "TextBoxTitleAlarmPosition");
                SetButtonChangeLanguage(BtnTitleAlarmDescription, BtnTitleAlarmDescription.Name);
                // 알람 타입에 따라 다른 언어를 불러옴
                // 데이터 테이블에서 일치하는 ID에 해당하는 TEXT를 불러옴
                if (CDefine.EAlarmType.ALARM_QUESTION != m_objAlarmInformation.eAlarmLevel)
                {
                    BtnYes.Visible = false;
                    SetButtonChangeLanguage(BtnNo, "BtnOK");
                }
                else
                {
                    SetButtonChangeLanguage(BtnYes, "BtnYes");
                    SetButtonChangeLanguage(BtnNo, "BtnNo");
                }

                try
                {
                    // 유저 메세지 데이터 테이블에서 언어 Row값 뽑아옴
                    CManagerTable objManagerAlarmTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationAlarm;
                    int index = (int)m_objDocument.m_objConfig.GetOptionParameter().eLanguage;
                    var languageIndex = new[]
                    {
                        new { Alarm = (int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_KOREA, AlarmAction = (int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_KOREA },
                        new { Alarm = (int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_CHINA, AlarmAction = (int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_CHINA },
                        new { Alarm = (int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_ENGLISH, AlarmAction = (int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_ENGLISH },
                        new { Alarm = (int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_VIETNAM, AlarmAction = (int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_VIETNAM },
                    };
                    DataRow[] objDataRow;
                    DataTable objDataTable;
                    if (true == m_bIsAlarmMessage)
                    {
                        objDataTable = objManagerAlarmTable.HLGetDataTable();
                        objDataRow = objDataTable.Select(string.Format("{0} = '{1}'", objManagerAlarmTable.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationAlarm.ID], m_objAlarmInformation.iAlarmCode));
                        RichTextBoxAlarmDescription.Text = $"{objDataRow[0].ItemArray[languageIndex[index].Alarm]}.\n{objDataRow[0].ItemArray[languageIndex[index].AlarmAction]}";
                        RichTextBoxAlarmDescription.SelectAll();
                        SetRichTextBoxFont(RichTextBoxAlarmDescription, Font.Name, m_dFontSizeAlarmDescription, RichTextBoxAlarmDescription.Font.Style);
                    }
                    else
                    {
                        RichTextBoxAlarmDescription.Text = m_objDocument.GetUserMessage((CAlarmDefine.EMessageList)m_objAlarmInformation.iAlarmCode, mMessageArgs);
                        RichTextBoxAlarmDescription.SelectAll();
                        SetRichTextBoxFont(RichTextBoxAlarmDescription, Font.Name, m_dFontSizeAlarmDescription, RichTextBoxAlarmDescription.Font.Style);
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        /// <param name="strKey"></param>
        private void SetButtonChangeLanguage(Button objButton, string strKey)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(strKey, Name));
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        /// <param name="strKey"></param>
        private void SetButtonChangeLanguage(ImageButton objButton, string strKey)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(strKey, Name));
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objTextBox"></param>
        /// <param name="strKey"></param>
        private void SetTextBoxChangeLanguage(TextBox objTextBox, string strKey)
        {
            objTextBox.Text = m_objDocument.GetDatabaseUIText(strKey, Name);
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
        /// 텍스트 박스's 초기화
        /// </summary>
        /// <param name="objTextBox"></param>
        /// <param name="strFontName"></param>
        /// <param name="dSize"></param>
        /// <returns></returns>
        private bool InitializeTextBox(TextBox objTextBox, string strFontName, double dSize)
        {
            bool bReturn = false;

            do
            {
                // 읽기만
                objTextBox.ReadOnly = true;
                // 탭 포커스 이동 x
                objTextBox.TabStop = false;
                // 폰트 변경
                objTextBox.Font = new Font(strFontName, (float)dSize, objTextBox.Font.Style);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 리치 텍스트 박스 초기화
        /// </summary>
        /// <param name="objRich"></param>
        /// <returns></returns>
        private new bool InitializeRichTextBox(RichTextBox objRich)
        {
            bool bReturn = false;

            do
            {
                // 리치 텍스트 박스 기본 스타일 초기화
                if (false == base.InitializeRichTextBox(objRich))
                    break;
                // 리치 텍스트 박스 크기 변경
                base.SetRichTextBoxFont(objRich, Font.Name, m_dFontSizeAlarmDescription, objRich.Font.Style);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// YES or OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnYes_Click(object sender, EventArgs e)
        {
            if (CDefine.EAlarmType.ALARM_QUESTION == m_objAlarmInformation.eAlarmLevel)
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnYes_Click", DialogResult.Yes.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);

                CDialogMessage.ActiveForm.DialogResult = DialogResult.Yes;
                CDialogMessage.ActiveForm.Close();
            }
        }

        /// <summary>
        /// NO or CANCEL or OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNo_Click(object sender, EventArgs e)
        {
            if (CDefine.EAlarmType.ALARM_QUESTION == m_objAlarmInformation.eAlarmLevel)
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnNo_Click", DialogResult.No.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);

                CDialogMessage.ActiveForm.DialogResult = DialogResult.No;
                CDialogMessage.ActiveForm.Close();
            }
            else
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnNo_Click", DialogResult.OK.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);

                CDialogMessage.ActiveForm.DialogResult = DialogResult.OK;
                CDialogMessage.ActiveForm.Close();
            }
        }

        private void CDialogMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (CDefine.EAlarmType.ALARM_QUESTION != m_objAlarmInformation.eAlarmLevel)
            {
                switch (e.KeyCode)
                {
                    case Keys.Space:
                    case Keys.Enter:
                    case Keys.Escape:
                        BtnNo_Click(sender, EventArgs.Empty);
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Space:
                    case Keys.Enter:
                        BtnYes_Click(sender, EventArgs.Empty);
                        break;
                    case Keys.Escape:
                        BtnNo_Click(sender, EventArgs.Empty);
                        break;
                }
            }
        }

        private void BtnLanguage_Click(object sender, EventArgs e)
        {
            CDialogSelectLanguage objDialogSelectLanguage = new CDialogSelectLanguage(m_objDocument);
            objDialogSelectLanguage.InitializeFormBaseButton(BtnLanguage);
            objDialogSelectLanguage.Show();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (CDefine.ELanguage.LANGUAGE_KOREA == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
            {
                base.SetButtonText(BtnLanguage, "KOREA");
            }
            else if (CDefine.ELanguage.LANGUAGE_CHINA == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
            {
                base.SetButtonText(BtnLanguage, "CHINA");
            }
            else if (CDefine.ELanguage.LANGUAGE_ENGLISH == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
            {
                base.SetButtonText(BtnLanguage, "ENGLISH");
            }
            else if (CDefine.ELanguage.LANGUAGE_VIETNAM == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
            {
                base.SetButtonText(BtnLanguage, "VIETNAM");
            }
            if (mLastLanguage != m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
            {
                SetChangeLanguage();
            }
            mLastLanguage = m_objDocument.m_objConfig.GetOptionParameter().eLanguage;
        }
    }
}