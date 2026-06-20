using System;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogCIMDisconnected : CFormCommon, CFormInterface
    {
        private CDocument m_objDocument;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CDialogCIMDisconnected(CDocument objDocument)
        {
            InitializeComponent();
            m_objDocument = objDocument as CDocument;
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogCIMDisconnected_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogCIMDisconnected_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeInitialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool bResult = false;
            do
            {
                // 다이얼로그 초기화
                if (false == InitializeForm()) break;

                bResult = true;
            } while (false);
            return bResult;
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
                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                SetTimer(true);

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
        /// 해제.
        /// </summary>
        public void DeInitialize()
        {
            SetTimer(false);
        }

        /// <summary>
        /// 버튼 언어 변경.
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
        /// 타이머 시작 유무
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
        /// 디스플레이 문자열 설정
        /// </summary>
        /// <param name="strComment"></param>
        public void SetComment(string strComment)
        {
            BtnComment.Text = strComment;
        }

        /// <summary>
        /// Visible 유무
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
        }

        /// <summary>
        /// 타이머 이벤트 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 컴펌 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void CDialogCIMDisconnected_FormClosing(object sender, FormClosingEventArgs e)
        {
            ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus objMachineStatus = new ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus();
            var objMMFManagerMachineStatus = ENC.MemoryMap.Manager.CMMFManagerMachineStatus.Instance;
            objMMFManagerMachineStatus[0].GetMachineStatus(out objMachineStatus);
            if (CDefine.EProgramExitStatus.PROGRAM_EXIT_STATUS_ON != objMachineStatus.m_eProgramExitStatus)
            {
                e.Cancel = true;
            }
        }
    }
}
