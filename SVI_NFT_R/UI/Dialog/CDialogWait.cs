using System;
using System.Drawing;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    public partial class CDialogWait : CFormCommon, CFormInterface
    {
        public bool CanCancel
        {
            get
            {
                return mbCanCancel;
            }
            set
            {
                if (mbCanCancel != value)
                {
                    mbCanCancel = value;
                    Invoke(new Action(() =>
                    {
                        BtnCancel.Visible = mbCanCancel;
                        Size = mbCanCancel ? new Size(567, 195) : new Size(567, 146);
                    }));
                }
            }
        }
        public event EventHandler CancelClick;
        private bool mbCanCancel = true;
        /// <summary>
        /// Waiting 중일때 움직이는 그림파일.
        /// </summary>
        private Bitmap bitMap;
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CDialogWait(CDocument objDocument)
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
        private void CDialogWait_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void CDialogWait_FormClosed(object sender, FormClosedEventArgs e)
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
                if (false == InitializeForm())
                    break;

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

                // 버튼 언어 변경.
                SetChangeLanguage();

                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                SetTimer(true);

                TxtMessage.Enabled = false;
                TxtMessage.ForeColor = Color.Black;

                CanCancel = false;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제.
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
        /// 프레임 변경시 마다 화면을 새로 그려준다.
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
            return true;
        }

        /// <summary>
        /// 타이머 시작
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
        /// 대기 메시지 설정
        /// </summary>
        /// <param name="strData">대기 메시지</param>
        public void SetText(string strData)
        {
            TxtMessage.Text = strData;
        }

        /// <summary>
        /// 현재 대기 메시지를 반환함
        /// </summary>
        /// <returns>현재 대기 메시지</returns>
        public string GetText() => TxtMessage.Text;

        /// <summary>
        /// 타이머 이벤트 함수.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 창을 띄우면 창을 가운데로 이동함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogWait_VisibleChanged(object sender, EventArgs e)
        {
            if (true == Visible)
            {
                CenterToScreen();
            }
        }

        private void CDialogWait_FormClosing(object sender, FormClosingEventArgs e)
        {
            ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus objMachineStatus = new ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus();
            var objMMFManagerMachineStatus = ENC.MemoryMap.Manager.CMMFManagerMachineStatus.Instance;
            objMMFManagerMachineStatus[0].GetMachineStatus(out objMachineStatus);
            if (CDefine.EProgramExitStatus.PROGRAM_EXIT_STATUS_ON != objMachineStatus.m_eProgramExitStatus)
            {
                e.Cancel = true;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            CancelClick?.Invoke(sender, e);
        }
    }
}
