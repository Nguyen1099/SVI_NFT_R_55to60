using System;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogJudgemnetTableSelect : CFormCommon, CFormInterface
    {
        private string _CellOutCode;
        public string m_CellOutCode
        {
            get { return _CellOutCode; }
            set { _CellOutCode = value; }
        }
        private CDocument m_objDocument;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CDialogJudgemnetTableSelect(CDocument objDocument)
        {
            InitializeComponent();
            m_objDocument = objDocument;
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogCellOutCode_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogCellOutCode_FormClosed(object sender, FormClosedEventArgs e)
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
                if (false == InitializeForm()) break;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            SetTimer(false);
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
                // 언어변환
                SetChangeLanguage();

                SetButtonColor();

                // 타이머 시작
                timer.Interval = 100;
                timer.Enabled = true;

                SetTimer(true);

                bReturn = true;
            } while (false);

            return bReturn;
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
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            base.SetButtonBackColor(this.BtnCellOutRetest, m_colorNormal);
            base.SetButtonBackColor(this.BtnCellOutManualTrackout, m_colorNormal);
            base.SetButtonBackColor(this.BtnCellOutNG, m_colorNormal);
            base.SetButtonBackColor(this.BtnCellOutGood, m_colorNormal);
            base.SetButtonBackColor(this.BtnCellOutBinPrime, m_colorNormal);
            base.SetButtonBackColor(this.BtnCancel, m_colorNormal);
        }

        /// <summary>
        /// 타이머 사용 유무
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            if (bTimer)
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
        /// 타이머 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Retest 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellOutRetest_Click(object sender, EventArgs e)
        {
            m_CellOutCode = CDefine.CIM_JUDGE_RETEST;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Manual Track Out 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellOutManualTrackout_Click(object sender, EventArgs e)
        {
            m_CellOutCode = CDefine.CIM_JUDGE_OUT;
            this.DialogResult = DialogResult.OK;
            Close();

        }

        /// <summary>
        /// NG 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellOutNG_Click(object sender, EventArgs e)
        {
            m_CellOutCode = CDefine.CIM_JUDGE_LOTLOSS;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Good 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellOutGood_Click(object sender, EventArgs e)
        {
            m_CellOutCode = CDefine.CIM_JUDGE_GOOD;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Bin Prime 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellOutBinPrime_Click(object sender, EventArgs e)
        {
            m_CellOutCode = CDefine.CIM_JUDGE_T;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// 폼 종료 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CDialogJudgemnetTableSelect_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.R:
                    BtnCellOutRetest_Click(sender, e);
                    break;
                case Keys.O:
                    BtnCellOutManualTrackout_Click(sender, e);
                    break;
                case Keys.L:
                    BtnCellOutNG_Click(sender, e);
                    break;
                case Keys.G:
                    BtnCellOutGood_Click(sender, e);
                    break;
                case Keys.T:
                    BtnCellOutBinPrime_Click(sender, e);
                    break;
                case Keys.Escape:
                    BtnCancel_Click(sender, e);
                    break;
            }
        }
    }
}
