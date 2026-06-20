using System;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogCellOutCode : CFormCommon, CFormInterface
    {
        private string _CellOutCode;
        public string m_CellOutCode
        {
            get { return _CellOutCode; }
            set { _CellOutCode = value; }
        }
        private readonly CDocument m_objDocument;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public CDialogCellOutCode(CDocument document)
        {
            InitializeComponent();
            m_objDocument = document;
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
                SetButtonChangeLanguage(BtnCellOutRetest);
                SetButtonChangeLanguage(BtnCellOutManualTrackout);
                SetButtonChangeLanguage(BtnCellOutNG);
                SetButtonChangeLanguage(BtnCellOutGood);
                SetButtonChangeLanguage(BtnCancel);

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
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            base.SetButtonBackColor(BtnCellOutRetest, m_colorNormal);
            base.SetButtonBackColor(BtnCellOutManualTrackout, m_colorNormal);
            base.SetButtonBackColor(BtnCellOutNG, m_colorNormal);
            base.SetButtonBackColor(BtnCellOutGood, m_colorNormal);
            base.SetButtonBackColor(BtnCancel, m_colorNormal);
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
            DialogResult = DialogResult.OK;
            ActiveForm.Close();
        }

        /// <summary>
        /// Manual Track Out 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellOutManualTrackout_Click(object sender, EventArgs e)
        {
            m_CellOutCode = CDefine.CIM_JUDGE_OUT;
            DialogResult = DialogResult.OK;
            ActiveForm.Close();

        }

        /// <summary>
        /// NG 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellOutNG_Click(object sender, EventArgs e)
        {
            m_CellOutCode = CDefine.CIM_JUDGE_OUT;
            DialogResult = DialogResult.OK;
            ActiveForm.Close();
        }

        /// <summary>
        /// Good 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellOutGood_Click(object sender, EventArgs e)
        {
            m_CellOutCode = CDefine.CIM_JUDGE_GOOD;
            DialogResult = DialogResult.OK;
            ActiveForm.Close();
        }

        /// <summary>
        /// 폼 종료 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            ActiveForm.Close();
        }
    }
}
