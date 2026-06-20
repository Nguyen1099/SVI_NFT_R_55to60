using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogSelectLanguage : CFormCommon
    {
        private CDocument m_objDocument;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        public CDialogSelectLanguage(CDocument objDocument)
        {
            m_objDocument = objDocument;
            InitializeComponent();
        }

        public void InitializeFormBaseButton(Button button)
        {
            pnlBase.Size = new Size(button.Width, (button.Height * 5) + (6 * 4));
            Rectangle rectButton = new Rectangle(button.Location, button.Size);
            Location = button.Parent.PointToScreen(new Point(rectButton.Left, rectButton.Bottom - pnlBase.Height));
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Size = pnlBase.Size;
        }

        /// <summary>
        /// 폼 실행 시점
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogSelectLanguage_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료 시점
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogSelectLanguage_FormClosed(object sender, FormClosedEventArgs e)
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
            timer.Enabled = false;
            m_objDocument.GetMainFrame().SetChangeLanguage();
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
                // 버튼 색상 정의
                SetButtonColor();

                timer.Interval = 100;
                timer.Enabled = true;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 초기 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            base.SetButtonBackColor(this.BtnCancel, m_colorNormal);
            CDefine.ELanguage eLanguage = m_objDocument.m_objConfig.GetOptionParameter().eLanguage;
            switch (eLanguage)
            {
                case CDefine.ELanguage.LANGUAGE_KOREA:
                    base.SetButtonBackColor(this.BtnSelectChina, m_colorNormal);
                    base.SetButtonBackColor(this.BtnSelectEnglish, m_colorNormal);
                    base.SetButtonBackColor(this.BtnSelectKorea, m_colorClick);
                    base.SetButtonBackColor(this.BtnSelectVietnam, m_colorNormal);
                    break;
                case CDefine.ELanguage.LANGUAGE_CHINA:
                    base.SetButtonBackColor(this.BtnSelectChina, m_colorClick);
                    base.SetButtonBackColor(this.BtnSelectEnglish, m_colorNormal);
                    base.SetButtonBackColor(this.BtnSelectKorea, m_colorNormal);
                    base.SetButtonBackColor(this.BtnSelectVietnam, m_colorNormal);
                    break;
                case CDefine.ELanguage.LANGUAGE_ENGLISH:
                    base.SetButtonBackColor(this.BtnSelectChina, m_colorNormal);
                    base.SetButtonBackColor(this.BtnSelectEnglish, m_colorClick);
                    base.SetButtonBackColor(this.BtnSelectKorea, m_colorNormal);
                    base.SetButtonBackColor(this.BtnSelectVietnam, m_colorNormal);
                    break;
                case CDefine.ELanguage.LANGUAGE_VIETNAM:
                    base.SetButtonBackColor(this.BtnSelectChina, m_colorNormal);
                    base.SetButtonBackColor(this.BtnSelectEnglish, m_colorNormal);
                    base.SetButtonBackColor(this.BtnSelectKorea, m_colorNormal);
                    base.SetButtonBackColor(this.BtnSelectVietnam, m_colorClick);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// 타이머 동작 구현부
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 폼에 포커스가 사라지면 자동으로 닫는다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogSelectLanguage_Deactivate(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 언어를 선택 했을 때 구현부 (언어를 변경하고 창을 닫는다)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLanguageSelect_Click(object sender, EventArgs e)
        {
            ImageButton btnSelect = sender as ImageButton;
            CConfig.COptionParameter objOptionParameter = m_objDocument.m_objConfig.GetOptionParameter();
            CDefine.ELanguage beforeLanguage = objOptionParameter.eLanguage;

            if (null != btnSelect)
            {
                if ("KOREA" == btnSelect.ButtonText)
                {
                    objOptionParameter.eLanguage = CDefine.ELanguage.LANGUAGE_KOREA;
                }
                else if ("ENGLISH" == btnSelect.ButtonText)
                {
                    objOptionParameter.eLanguage = CDefine.ELanguage.LANGUAGE_ENGLISH;
                }
                else if ("VIETNAM" == btnSelect.ButtonText)
                {
                    objOptionParameter.eLanguage = CDefine.ELanguage.LANGUAGE_VIETNAM;
                }
                else if ("CHINA" == btnSelect.ButtonText)
                {
                    objOptionParameter.eLanguage = CDefine.ELanguage.LANGUAGE_CHINA;
                }
            }
            m_objDocument.m_objConfig.SaveOptionParameter(objOptionParameter);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Language : {1} -> {2}]", "BtnLanguageSelect_Click", beforeLanguage.ToString(), m_objDocument.m_objConfig.GetOptionParameter().eLanguage.ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
            Close();
        }

        /// <summary>
        /// 취소 버튼 구현부
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
