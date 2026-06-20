using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogEQState : CFormCommon
    {
        private CDocument m_objDocument;
        // 시작 시간
        DateTime m_StartDateTime;
        int iTimer;

        public CDialogEQState(CDocument objDocument)
        {
            m_objDocument = objDocument;
            InitializeComponent();
        }

        /// <summary>
        /// 다이얼로그 로드.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogEQState_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        /// <summary>
        /// 다이얼로그 닫기.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogEQState_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        /// <summary>
        /// 버튼 언어
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool bResult = false;
            do
            {
                // 폼 초기화
                if (false == InitializeDialog())
                    break;

                BtnEquipmentSN.SetText(m_objDocument.m_objConfig.GetSystemParameter().strEquipmentSN);

                m_StartDateTime = DateTime.Now;
                iTimer = 60;

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 버튼 언어
        /// </summary>
        public void DeInitialize()
        {

        }

        /// <summary>
        /// 폼 초기화
        /// </summary>
        /// <returns></returns>
        public bool InitializeDialog()
        {
            bool bReturn = false;

            do
            {
                // 언어변환
                SetChangeLanguage();

                // 버튼 색 초기화
                SetButtonColor();

                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                SetTimer(true);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            SetButtonBackColor(BtnTitleAdress, m_colorLabelSub);
            SetButtonBackColor(BtnTitleContectUs, m_colorLabelSub);
            SetButtonBackColor(BtnAddress, m_colorLabelData);
            SetButtonBackColor(BtnTel, m_colorLabelData);
            SetButtonBackColor(BtnFax, m_colorLabelData);
            SetButtonBackColor(BtnEmail, m_colorLabelData);

            List<Control> imageButtons = Controls.GetChildControlListByType(typeof(ImageButton));
            foreach (ImageButton btn in imageButtons)
            {
                if (null != btn)
                {
                    SetButtonColor(btn, btn.ForeColor, m_colorNormal, true);
                }
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
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                    btn.BackColorChanged += NonClickableButton_BackColorChanged;
                    btn.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 설명 : 버튼 언어
        /// </summary>
        /// <returns></returns>
        public bool SetChangeLanguage()
        {
            bool bReturn = false;

            do
            {

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 설명 : 버튼 언어
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            if (true == bTimer)
            {
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled = false;
            }
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
            if (DateTime.Now - m_StartDateTime >= new TimeSpan(0, 0, 1))
            {
                this.Text = string.Format("[ About ] {0:D2} Sec", iTimer - (DateTime.Now - m_StartDateTime).Seconds);
            }

            if (DateTime.Now - m_StartDateTime >= new TimeSpan(0, 0, 60))
            {
                this.Hide();
            }
        }
    }
}
