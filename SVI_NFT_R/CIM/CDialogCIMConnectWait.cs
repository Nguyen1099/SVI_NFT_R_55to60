using System;
using System.Drawing;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    public partial class CDialogCIMConnectWait : CFormCommon, CFormInterface
    {
        /// <summary>
        /// Waiting 중일때 움직이는 그림파일.
        /// </summary>
        private Bitmap bitMap;
        /// <summary>
        /// CIM 연결 여부.
        /// </summary>
        private bool bConnected;
        /// <summary>
        /// Cancel
        /// </summary>
        private bool m_bCancel;
        /// <summary>
        /// Document
        /// </summary>
        private CDocument m_objDocument;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CDialogCIMConnectWait(CDocument objDocument)
        {
            InitializeComponent();
            m_objDocument = objDocument as CDocument;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogCIMConnectWait_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogCIMConnectWait_FormClosed(object sender, FormClosedEventArgs e)
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
                // 비트맵 그리기.
                bitMap = new Bitmap(SVI_NFT_R.Properties.Resources.TitleRun);
                ImageAnimator.Animate(bitMap, new EventHandler(this.OnFrameChanged));

                // CIM 연결 상태 초기화
                bConnected = false;
                // Cancel 버튼을 눌렀는지 확인하는 버튼.
                m_bCancel = false;

                // 버튼 언어 변경.
                SetChangeLanguage();

                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                SetTimer(true);

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
        /// 이미지 그리기.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            ImageAnimator.UpdateFrames();
            Graphics g = pictureBoxWaiting.CreateGraphics();
            g.DrawImage(this.bitMap, new Point(0, 0));
            base.OnPaint(e);
        }

        /// <summary>
        /// 이미지 갱신.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFrameChanged(object sender, EventArgs e)
        {
            this.Invalidate();
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
        /// 타이머 이벤트 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 연결 상태 확인
        /// </summary>
        public void IsConnected()
        {
            if (bConnected)
            {
                m_bCancel = false;
                CDialogCIMConnectWait.ActiveForm.Close();
            }
        }

        /// <summary>
        /// 취소 상태 확인.
        /// </summary>
        /// <returns></returns>
        public bool IsCancel()
        {
            return m_bCancel;
        }

        /// <summary>
        /// 닫기 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            m_bCancel = true;
            this.Hide();
        }

        private void CDialogCIMConnectWait_FormClosing(object sender, FormClosingEventArgs e)
        {
            ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus objMachineStatus = new ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus();
            var objMMFManagerMachineStatus = ENC.MemoryMap.Manager.CMMFManagerMachineStatus.Instance;
            objMMFManagerMachineStatus[0].GetMachineStatus(out objMachineStatus);
            if (CDefine.EProgramExitStatus.PROGRAM_EXIT_STATUS_ON != objMachineStatus.m_eProgramExitStatus)
            {
                e.Cancel = true;
            }
        }

        private void CDialogCIMConnectWait_VisibleChanged(object sender, EventArgs e)
        {
            if (true == Visible)
            {
                m_bCancel = false;
            }
        }
    }
}
