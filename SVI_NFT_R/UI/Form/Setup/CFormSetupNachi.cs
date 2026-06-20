using System;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormSetupNachi : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private readonly CDocument m_objDocument;
        /// <summary>
        /// IO Input & Output
        /// </summary>
        private enum EPageIndex
        {
            BIT_INPUT = 0,
            BIT_OUTPUT,
            WORD_INPUT,
            WORD_OUTPUT,
            INDEX_FINAL
        };
        /// <summary>
        /// 현재 IO 페이지
        /// </summary>
        private int[] m_iCurrentPage;
        /// <summary>
        /// 최대 IO 페이지
        /// </summary>
        private int[] m_iMaxPage;
        /// <summary>
        /// 페이지 당 표시할 IO 수
        /// </summary>
        private const int m_iDisplayPageIO = 16;

        // Bit X,Y 버튼 배열
        private readonly Button[] m_BtnInputBitIndex = new Button[m_iDisplayPageIO];
        private readonly Button[] m_BtnInputBitName = new Button[m_iDisplayPageIO];
        private readonly Button[] m_BtnOutputBitIndex = new Button[m_iDisplayPageIO];
        private readonly Button[] m_BtnOutputBitName = new Button[m_iDisplayPageIO];

        // Word WW, WR 버튼 배열
        private readonly Button[] m_BtnInputWordIndex = new Button[m_iDisplayPageIO];
        private readonly Button[] m_BtnInputWordName = new Button[m_iDisplayPageIO];
        private readonly Button[] m_BtnOutputWordIndex = new Button[m_iDisplayPageIO];
        private readonly Button[] m_BtnOutputWordName = new Button[m_iDisplayPageIO];

        /// <summary>
        /// CCLInk 정보 객체
        /// </summary>
        private CConfig.CInterfaceParameter m_objCCLinkParameter;

        /// <summary>
        /// CCLink 객체
        /// </summary>
        private HLDevice.CDeviceInterface m_objCCLink;
        private bool mbOutputLock = true;
        private bool mbUseOffsetNameView = true;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormSetupNachi(CDocument objDocument)
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
        private void CFormSetupNachi_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormSetupNachi_FormClosed(object sender, FormClosedEventArgs e)
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
                // 페이지 변수 초기화
                m_iCurrentPage = new int[(int)EPageIndex.INDEX_FINAL];
                m_iMaxPage = new int[(int)EPageIndex.INDEX_FINAL];

                // 버튼 매칭
                SetButtonControlInitialize();

                // CCLink 초기화 정보를 가져온다
                m_objCCLinkParameter = m_objDocument.m_objConfig.GetCCLinkVer2Parameter().DeepClone();


                for (int iLoopIO = 0; iLoopIO < (int)EPageIndex.INDEX_FINAL; iLoopIO++)
                {
                    // 현재 페이지 설정
                    m_iCurrentPage[iLoopIO] = 0;
                }

                int robotCount = Enum.GetValues(typeof(CProcessMotion.ERobot)).Length;
                m_iMaxPage[(int)EPageIndex.BIT_INPUT] = robotCount * 8 - 1;
                m_iMaxPage[(int)EPageIndex.BIT_OUTPUT] = robotCount * 8 - 1;
                m_iMaxPage[(int)EPageIndex.WORD_INPUT] = robotCount * 16 - 1;
                m_iMaxPage[(int)EPageIndex.WORD_OUTPUT] = robotCount * 16 - 1;

                BtnCurrentPageInputBit.Tag = EPageIndex.BIT_INPUT;
                BtnCurrentPageOutputBit.Tag = EPageIndex.BIT_OUTPUT;
                BtnCurrentPageInputWord.Tag = EPageIndex.WORD_INPUT;
                BtnCurrentPageOutputWord.Tag = EPageIndex.WORD_OUTPUT;

                // 버튼 색상 정의
                SetButtonColor();

                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                SetDisplayInputBit();
                SetDisplayOutputBit();
                SetDisplayInputWord();
                SetDisplayOutputWord();
                // CCLink 객체 연결
                m_objCCLink = m_objDocument.m_objProcessMain.m_objCCLinkVer2;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            // 버튼 색 변경
            SetButtonBackColor(BtnTitleNachiBit, m_colorLabel);
            SetButtonBackColor(BtnTitleNachiWord, m_colorLabel);

            SetButtonBackColor(BtnPreviousInputBit, m_colorNormal);
            SetButtonBackColor(BtnNextInputBit, m_colorNormal);
            SetButtonBackColor(BtnPreviousOutputBit, m_colorNormal);
            SetButtonBackColor(BtnNextOutputBit, m_colorNormal);
            SetButtonBackColor(BtnPreviousInputWord, m_colorNormal);
            SetButtonBackColor(BtnNextInputWord, m_colorNormal);
            SetButtonBackColor(BtnPreviousOutputWord, m_colorNormal);
            SetButtonBackColor(BtnNextOutputWord, m_colorNormal);
            SetButtonBackColor(BtnCurrentPageInputBit, m_colorLabelSub);
            SetButtonBackColor(BtnCurrentPageOutputBit, m_colorLabelSub);
            SetButtonBackColor(BtnCurrentPageInputWord, m_colorLabelSub);
            SetButtonBackColor(BtnCurrentPageOutputWord, m_colorLabelSub);

            SetButtonBackColor(BtnInputBit01, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit02, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit03, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit04, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit05, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit06, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit07, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit08, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit09, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit10, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit11, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit12, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit13, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit14, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit15, m_colorLabelSub);
            SetButtonBackColor(BtnInputBit16, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit01, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit02, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit03, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit04, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit05, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit06, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit07, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit08, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit09, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit10, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit11, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit12, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit13, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit14, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit15, m_colorLabelSub);
            SetButtonBackColor(BtnOutputBit16, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord01, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord02, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord03, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord04, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord05, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord06, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord07, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord08, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord09, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord10, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord11, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord12, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord13, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord14, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord15, m_colorLabelSub);
            SetButtonBackColor(BtnInputWord16, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord01, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord02, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord03, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord04, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord05, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord06, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord07, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord08, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord09, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord10, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord11, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord12, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord13, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord14, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord15, m_colorLabelSub);
            SetButtonBackColor(BtnOutputWord16, m_colorLabelSub);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnOutputBitName01.Name)
                    && false == btn.Name.Equals(BtnOutputBitName02.Name)
                    && false == btn.Name.Equals(BtnOutputBitName03.Name)
                    && false == btn.Name.Equals(BtnOutputBitName04.Name)
                    && false == btn.Name.Equals(BtnOutputBitName05.Name)
                    && false == btn.Name.Equals(BtnOutputBitName06.Name)
                    && false == btn.Name.Equals(BtnOutputBitName07.Name)
                    && false == btn.Name.Equals(BtnOutputBitName08.Name)
                    && false == btn.Name.Equals(BtnOutputBitName09.Name)
                    && false == btn.Name.Equals(BtnOutputBitName10.Name)
                    && false == btn.Name.Equals(BtnOutputBitName11.Name)
                    && false == btn.Name.Equals(BtnOutputBitName12.Name)
                    && false == btn.Name.Equals(BtnOutputBitName13.Name)
                    && false == btn.Name.Equals(BtnOutputBitName14.Name)
                    && false == btn.Name.Equals(BtnOutputBitName15.Name)
                    && false == btn.Name.Equals(BtnOutputBitName16.Name)
                    && false == btn.Name.Equals(BtnOutputWordName01.Name)
                    && false == btn.Name.Equals(BtnOutputWordName02.Name)
                    && false == btn.Name.Equals(BtnOutputWordName03.Name)
                    && false == btn.Name.Equals(BtnOutputWordName04.Name)
                    && false == btn.Name.Equals(BtnOutputWordName05.Name)
                    && false == btn.Name.Equals(BtnOutputWordName06.Name)
                    && false == btn.Name.Equals(BtnOutputWordName07.Name)
                    && false == btn.Name.Equals(BtnOutputWordName08.Name)
                    && false == btn.Name.Equals(BtnOutputWordName09.Name)
                    && false == btn.Name.Equals(BtnOutputWordName10.Name)
                    && false == btn.Name.Equals(BtnOutputWordName11.Name)
                    && false == btn.Name.Equals(BtnOutputWordName12.Name)
                    && false == btn.Name.Equals(BtnOutputWordName13.Name)
                    && false == btn.Name.Equals(BtnOutputWordName14.Name)
                    && false == btn.Name.Equals(BtnOutputWordName15.Name)
                    && false == btn.Name.Equals(BtnOutputWordName16.Name)
                    && false == btn.Name.Equals(BtnCurrentPageInputBit.Name)
                    && false == btn.Name.Equals(BtnCurrentPageOutputBit.Name)
                    && false == btn.Name.Equals(BtnCurrentPageInputWord.Name)
                    && false == btn.Name.Equals(BtnCurrentPageOutputWord.Name)
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
        /// 설비 상태 or 권한 레벨에 따라 자원 상태 변경
        /// </summary>
        private void SetResourceControl()
        {
            // 현재 유저 권한 레벨 받아옴
            CUserInformation objUserInformation = m_objDocument.GetUserInformation();

            // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                base.SetControlButtonEnable(Controls, false);
                base.SetButtonEnable(BtnPreviousInputBit, true);
                base.SetButtonEnable(BtnNextInputBit, true);
                base.SetButtonEnable(BtnPreviousOutputBit, true);
                base.SetButtonEnable(BtnNextOutputBit, true);
                base.SetButtonEnable(BtnPreviousInputWord, true);
                base.SetButtonEnable(BtnNextInputWord, true);
                base.SetButtonEnable(BtnPreviousOutputWord, true);
                base.SetButtonEnable(BtnNextOutputWord, true);
                base.SetButtonEnable(BtnCurrentPageInputBit, true);
                base.SetButtonEnable(BtnCurrentPageOutputBit, true);
                base.SetButtonEnable(BtnCurrentPageInputWord, true);
                base.SetButtonEnable(BtnCurrentPageOutputWord, true);
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        base.SetControlButtonEnable(Controls, false);
                        base.SetButtonEnable(BtnPreviousInputBit, true);
                        base.SetButtonEnable(BtnNextInputBit, true);
                        base.SetButtonEnable(BtnPreviousOutputBit, true);
                        base.SetButtonEnable(BtnNextOutputBit, true);
                        base.SetButtonEnable(BtnPreviousInputWord, true);
                        base.SetButtonEnable(BtnNextInputWord, true);
                        base.SetButtonEnable(BtnPreviousOutputWord, true);
                        base.SetButtonEnable(BtnNextOutputWord, true);
                        base.SetButtonEnable(BtnCurrentPageInputBit, true);
                        base.SetButtonEnable(BtnCurrentPageOutputBit, true);
                        base.SetButtonEnable(BtnCurrentPageInputWord, true);
                        base.SetButtonEnable(BtnCurrentPageOutputWord, true);
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        base.SetControlButtonEnable(Controls, false);
                        base.SetButtonEnable(BtnPreviousInputBit, true);
                        base.SetButtonEnable(BtnNextInputBit, true);
                        base.SetButtonEnable(BtnPreviousOutputBit, true);
                        base.SetButtonEnable(BtnNextOutputBit, true);
                        base.SetButtonEnable(BtnPreviousInputWord, true);
                        base.SetButtonEnable(BtnNextInputWord, true);
                        base.SetButtonEnable(BtnPreviousOutputWord, true);
                        base.SetButtonEnable(BtnNextOutputWord, true);
                        base.SetButtonEnable(BtnCurrentPageInputBit, true);
                        base.SetButtonEnable(BtnCurrentPageOutputBit, true);
                        base.SetButtonEnable(BtnCurrentPageInputWord, true);
                        base.SetButtonEnable(BtnCurrentPageOutputWord, true);
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        base.SetControlButtonEnable(Controls, true);
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
                SetButtonChangeLanguage(BtnTitleNachiBit);
                SetButtonChangeLanguage(BtnTitleNachiWord);
                SetButtonChangeLanguage(BtnPreviousInputBit);
                SetButtonChangeLanguage(BtnNextInputBit);
                SetButtonChangeLanguage(BtnPreviousOutputBit);
                SetButtonChangeLanguage(BtnNextOutputBit);
                SetButtonChangeLanguage(BtnPreviousInputWord);
                SetButtonChangeLanguage(BtnNextInputWord);
                SetButtonChangeLanguage(BtnPreviousOutputWord);
                SetButtonChangeLanguage(BtnNextOutputWord);
                SetButtonChangeLanguage(BtnOutputLock);

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
            Visible = bVisible;

            if (true == bVisible)
            {
                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                m_objDocument.GetMainFrame().SetCurrentForm(this);
                // 화면 전환시 LOCK 상태로 변경
                mbOutputLock = true;
            }
        }

        /// <summary>
        /// 타이머 동작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            bool bResult = false;
            // 페이지 표시
            var currentPageButtons = new[] { BtnCurrentPageInputBit, BtnCurrentPageOutputBit, BtnCurrentPageInputWord, BtnCurrentPageOutputWord };
            for (int i = 0; i < (int)EPageIndex.INDEX_FINAL; i++)
            {
                var currentIndex = m_iCurrentPage[i];
                int robotCount = Enum.GetNames(typeof(CProcessMotion.ERobot)).Length;
                int sectionPageCount = (m_iMaxPage[i] + 1) / robotCount;
                int currentSection = currentIndex / sectionPageCount;
                string robotName = ((CProcessMotion.ERobot)currentSection).ToString();
                SetButtonText(currentPageButtons[i], $"[ {robotName} ]{Environment.NewLine}( {m_iCurrentPage[i] + 1} / {m_iMaxPage[i] + 1} )");
            }

            // Bit 상태 표시
            for (int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++)
            {
                m_objCCLink.HLGetInterfaceBit(m_BtnInputBitName[iLoopCount].Text, out bResult);
                SetButtonBackColor(m_BtnInputBitName[iLoopCount], bResult ? m_colorInputOn : m_colorInputOff);
            }

            for (int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++)
            {
                m_objCCLink.HLGetInterfaceBit(m_BtnOutputBitName[iLoopCount].Text, out bResult);
                SetButtonBackColor(m_BtnOutputBitName[iLoopCount], bResult ? m_colorOutputOn : m_colorOutputOff);
            }

            // Word 상태 표시
            for (int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++)
            {
                m_objCCLink.HLGetInterfaceValue(m_BtnInputWordName[iLoopCount].Text, out bResult, iLoopCount);
                SetButtonBackColor(m_BtnInputWordName[iLoopCount], bResult ? m_colorInputOn : m_colorInputOff);
            }

            for (int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++)
            {
                m_objCCLink.HLGetInterfaceValue(m_BtnOutputWordName[iLoopCount].Text, out bResult, iLoopCount);
                SetButtonBackColor(m_BtnOutputWordName[iLoopCount], bResult ? m_colorOutputOn : m_colorOutputOff);
            }

            // IO 잠금 버튼 업데이트
            {
                SetButtonBackColor(BtnOutputLock, mbOutputLock ? m_colorOn : m_colorNormal, mbOutputLock);
            }
        }

        /// <summary>
        /// Input Bit 이전
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPreviousInput_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 >= m_iCurrentPage[(int)EPageIndex.BIT_INPUT])
                {
                    break;
                }
                m_iCurrentPage[(int)EPageIndex.BIT_INPUT]--;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Input Page : {1:D}]", "BtnPreviousInput_Click", m_iCurrentPage[(int)EPageIndex.BIT_INPUT]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                SetDisplayInputBit();
            } while (false);
        }

        /// <summary>
        /// Input Bit 다음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextInput_Click(object sender, EventArgs e)
        {
            do
            {
                if (m_iMaxPage[(int)EPageIndex.BIT_INPUT] <= m_iCurrentPage[(int)EPageIndex.BIT_INPUT])
                {
                    break;
                }
                m_iCurrentPage[(int)EPageIndex.BIT_INPUT]++;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Input Page : {1:D}]", "BtnNextInput_Click", m_iCurrentPage[(int)EPageIndex.BIT_INPUT]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                SetDisplayInputBit();
            } while (false);
        }

        /// <summary>
        /// Output Bit 이전
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPreviousOutput_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 >= m_iCurrentPage[(int)EPageIndex.BIT_OUTPUT])
                {
                    break;
                }
                m_iCurrentPage[(int)EPageIndex.BIT_OUTPUT]--;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Output Page : {1:D}]", "BtnPreviousOutput_Click", m_iCurrentPage[(int)EPageIndex.BIT_OUTPUT]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                SetDisplayOutputBit();
            } while (false);
        }

        /// <summary>
        /// Output Bit 다음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextOutput_Click(object sender, EventArgs e)
        {
            do
            {
                if (m_iMaxPage[(int)EPageIndex.BIT_OUTPUT] <= m_iCurrentPage[(int)EPageIndex.BIT_OUTPUT])
                {
                    break;
                }
                m_iCurrentPage[(int)EPageIndex.BIT_OUTPUT]++;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Output Page : {1:D}]", "BtnNextOutput_Click", m_iCurrentPage[(int)EPageIndex.BIT_OUTPUT]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                SetDisplayOutputBit();
            } while (false);
        }

        /// <summary>
        /// Input Word 이전
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPreviousInputWord_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 >= m_iCurrentPage[(int)EPageIndex.WORD_INPUT])
                {
                    break;
                }
                m_iCurrentPage[(int)EPageIndex.WORD_INPUT]--;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Input Word Page : {1:D}]", "BtnPreviousInputWord_Click", m_iCurrentPage[(int)EPageIndex.WORD_INPUT]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                SetDisplayInputWord();
            } while (false);
        }

        /// <summary>
        /// Input Word 다음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextInputWord_Click(object sender, EventArgs e)
        {
            do
            {
                if (m_iMaxPage[(int)EPageIndex.WORD_INPUT] <= m_iCurrentPage[(int)EPageIndex.WORD_INPUT])
                {
                    break;
                }
                m_iCurrentPage[(int)EPageIndex.WORD_INPUT]++;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Input Word Page : {1:D}]", "BtnNextInputWord_Click", m_iCurrentPage[(int)EPageIndex.WORD_INPUT]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                SetDisplayInputWord();
            } while (false);
        }

        /// <summary>
        /// Output Word 이전
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPreviousOutputWord_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 >= m_iCurrentPage[(int)EPageIndex.WORD_OUTPUT])
                {
                    break;
                }
                m_iCurrentPage[(int)EPageIndex.WORD_OUTPUT]--;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Output Word Page : {1:D}]", "BtnPreviousOutputWord_Click", m_iCurrentPage[(int)EPageIndex.WORD_OUTPUT]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                SetDisplayOutputWord();
            } while (false);
        }

        /// <summary>
        /// Output Word 다음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextOutputWord_Click(object sender, EventArgs e)
        {
            do
            {
                if (m_iMaxPage[(int)EPageIndex.WORD_OUTPUT] <= m_iCurrentPage[(int)EPageIndex.WORD_OUTPUT])
                {
                    break;
                }
                m_iCurrentPage[(int)EPageIndex.WORD_OUTPUT]++;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Output Word Page : {1:D}]", "BtnNextOutputWord_Click", m_iCurrentPage[(int)EPageIndex.WORD_OUTPUT]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                SetDisplayOutputWord();
            } while (false);
        }

        /// <summary>
        /// BIt X 입력 표시
        /// </summary>
        private void SetDisplayInputBit()
        {
            for (int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++)
            {
                string strNo = string.Format("X{0:D3}", 1 + iLoopCount + (m_iCurrentPage[(int)EPageIndex.BIT_INPUT] * m_iDisplayPageIO));
                if (false == m_objCCLinkParameter.objInterfaceParameterDigital.ContainsKey(strNo))
                {
                    continue;
                }
                if (false == mbUseOffsetNameView)
                {
                    m_BtnInputBitIndex[iLoopCount].Text = m_objCCLinkParameter.objInterfaceParameterDigital[strNo].strIndex;
                }
                else
                {
                    m_BtnInputBitIndex[iLoopCount].Text = m_objCCLinkParameter.objInterfaceParameterDigital[strNo].strOffsetName;
                }
                m_BtnInputBitName[iLoopCount].Text = m_objCCLinkParameter.objInterfaceParameterDigital[strNo].strDataName;
            }
        }

        /// <summary>
        /// BIt Y 출력 표시
        /// </summary>
        private void SetDisplayOutputBit()
        {
            for (int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++)
            {
                string strNo = string.Format("Y{0:D3}", 1 + iLoopCount + (m_iCurrentPage[(int)EPageIndex.BIT_OUTPUT] * m_iDisplayPageIO));
                if (false == m_objCCLinkParameter.objInterfaceParameterDigital.ContainsKey(strNo))
                {
                    continue;
                }
                if (false == mbUseOffsetNameView)
                {
                    m_BtnOutputBitIndex[iLoopCount].Text = m_objCCLinkParameter.objInterfaceParameterDigital[strNo].strIndex;
                }
                else
                {
                    m_BtnOutputBitIndex[iLoopCount].Text = m_objCCLinkParameter.objInterfaceParameterDigital[strNo].strOffsetName;
                }
                m_BtnOutputBitName[iLoopCount].Text = m_objCCLinkParameter.objInterfaceParameterDigital[strNo].strDataName;
            }
        }

        /// <summary>
        /// Word Input 입력 표시
        /// </summary>
        private void SetDisplayInputWord()
        {
            // INPUT WORD + PAGE가 1부터 시작하기 때문에 1를 더하고 그 값을 In/Out인 2로 나눔.
            for (int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++)
            {
                string strNo;
                strNo = $"WR{m_iCurrentPage[(int)EPageIndex.WORD_INPUT]:00}{iLoopCount:00}";
                if (false == m_objCCLinkParameter.objInterfaceParameterAnalog.ContainsKey(strNo))
                {
                    m_BtnInputWordIndex[iLoopCount].Text = string.Empty;
                    m_BtnInputWordName[iLoopCount].Text = string.Empty;
                    continue;
                }
                if (false == mbUseOffsetNameView)
                {
                    m_BtnInputWordIndex[iLoopCount].Text = m_objCCLinkParameter.objInterfaceParameterAnalog[strNo].strIndex;
                }
                else
                {
                    m_BtnInputWordIndex[iLoopCount].Text = m_objCCLinkParameter.objInterfaceParameterAnalog[strNo].strOffsetName;
                }
                m_BtnInputWordName[iLoopCount].Text = m_objCCLinkParameter.objInterfaceParameterAnalog[strNo].strDataName;
            }
        }

        /// <summary>
        /// Word Output 입력 표시
        /// </summary>
        private void SetDisplayOutputWord()
        {
            // OUTPUT WORD + PAGE가 1부터 시작하기 때문에 1를 더하고 그 값을 In/Out인 2로 나눔.
            for (int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++)
            {
                string strNo;
                strNo = $"WW{m_iCurrentPage[(int)EPageIndex.WORD_OUTPUT]:00}{iLoopCount:00}";
                if (false == m_objCCLinkParameter.objInterfaceParameterAnalog.ContainsKey(strNo))
                {
                    m_BtnOutputWordIndex[iLoopCount].Text = string.Empty;
                    m_BtnOutputWordName[iLoopCount].Text = string.Empty;
                    continue;
                }
                if (false == mbUseOffsetNameView)
                {
                    m_BtnOutputWordIndex[iLoopCount].Text = m_objCCLinkParameter.objInterfaceParameterAnalog[strNo].strIndex;
                }
                else
                {
                    m_BtnOutputWordIndex[iLoopCount].Text = m_objCCLinkParameter.objInterfaceParameterAnalog[strNo].strOffsetName;
                }
                m_BtnOutputWordName[iLoopCount].Text = m_objCCLinkParameter.objInterfaceParameterAnalog[strNo].strDataName;
            }
        }

        /// <summary>
        /// IO 버튼 매칭
        /// </summary>
        private void SetButtonControlInitialize()
        {
            for (int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++)
            {
                string strNo = string.Format("{0:D2}", iLoopCount + 1);
                // 입력 Bit 어드레스 버튼 매칭
                m_BtnInputBitIndex[iLoopCount] = (Controls.Find("BtnInputBit" + strNo, true))[0] as Button;
                // 입력 Bit  이름 버튼 매칭
                m_BtnInputBitName[iLoopCount] = (Controls.Find("BtnInputBitName" + strNo, true))[0] as Button;
                // 출력 Bit 어드레스 버튼 매칭
                m_BtnOutputBitIndex[iLoopCount] = (Controls.Find("BtnOutputBit" + strNo, true))[0] as Button;
                // 출력 Bit 이름 버튼 매칭
                m_BtnOutputBitName[iLoopCount] = (Controls.Find("BtnOutputBitName" + strNo, true))[0] as Button;

                // 입력 Word 어드레스 버튼 매칭
                m_BtnInputWordIndex[iLoopCount] = (Controls.Find("BtnInputWord" + strNo, true))[0] as Button;
                // 입력 Word 이름 매칭
                m_BtnInputWordName[iLoopCount] = (Controls.Find("BtnInputWordName" + strNo, true))[0] as Button;
                // 출력 Word 어드레스 버튼 매칭
                m_BtnOutputWordIndex[iLoopCount] = (Controls.Find("BtnOutputWord" + strNo, true))[0] as Button;
                // 출력 Word 이름 매칭
                m_BtnOutputWordName[iLoopCount] = (Controls.Find("BtnOutputWordName" + strNo, true))[0] as Button;
            }
        }

        /// <summary>
        /// Bit Output 출력 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOutputBitName_Click(object sender, EventArgs e)
        {
            if (false == mbOutputLock)
            {
                Button BtnSelected = (Button)sender;
                bool bResult = false;
                HLDevice.CDeviceInterface.EError eError;
                eError = m_objCCLink.HLGetInterfaceBit(BtnSelected.Text, out bResult);
                if (HLDevice.CDeviceInterface.EError.ERROR_NONE == eError)
                {
                    if (true == bResult)
                    {
                        m_objCCLink.HLSetInterfaceBit(BtnSelected.Text, false);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Output Bit Name : {1}] [Bit : {2}]", "BtnOutputBitName_Click", BtnSelected.Text, false);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                    else if (false == bResult)
                    {
                        m_objCCLink.HLSetInterfaceBit(BtnSelected.Text, true);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Output Bit Name : {1}] [Bit : {2}]", "BtnOutputBitName_Click", BtnSelected.Text, true);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
                else if (HLDevice.CDeviceInterface.EError.ERROR_LICENSE == eError)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.LISENCE_KEY_RECOGNITION_FAILED);
                }
            }
            else
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.OUTPUT_SIGNAL_CHANGE_LOCK_STATUS);
            }
        }

        /// <summary>
        /// Bit input 입력 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInputBitName_Click(object sender, EventArgs e)
        {
            // 시뮬레이션 모드에서만 동작
            if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                Button BtnSelected = (Button)sender;
                bool bResult = false;
                HLDevice.CDeviceInterface.EError eError;
                eError = m_objCCLink.HLGetInterfaceBit(BtnSelected.Text, out bResult);
                if (HLDevice.CDeviceInterface.EError.ERROR_NONE == eError)
                {
                    if (true == bResult)
                    {
                        m_objCCLink.HLSetInterfaceBit(BtnSelected.Text, false);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Input Bit Name : {1}] [Bit : {2}]", "BtnInputBitName_Click", BtnSelected.Text, false);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                    else if (false == bResult)
                    {
                        m_objCCLink.HLSetInterfaceBit(BtnSelected.Text, true);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Input Bit Name : {1}] [Bit : {2}]", "BtnInputBitName_Click", BtnSelected.Text, true);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
                else if (HLDevice.CDeviceInterface.EError.ERROR_LICENSE == eError)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.LISENCE_KEY_RECOGNITION_FAILED);
                }
            }
        }

        /// <summary>
        /// Word Output 출력 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOutputWordName_Click(object sender, EventArgs e)
        {
            if (false == mbOutputLock)
            {
                Button BtnSelected = (Button)sender;
                int iSelectedIndex = 0;
                bool bResult = false;

                for (int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++)
                {
                    if (m_BtnOutputWordName[iLoopCount] == BtnSelected)
                    {
                        iSelectedIndex = iLoopCount;
                        break;
                    }
                }

                HLDevice.CDeviceInterface.EError eError;
                eError = m_objCCLink.HLGetInterfaceValue(BtnSelected.Text, out bResult, iSelectedIndex);

                if (HLDevice.CDeviceInterface.EError.ERROR_NONE == eError)
                {
                    if (true == bResult)
                    {
                        m_objCCLink.HLSetInterfaceValue(BtnSelected.Text, false, iSelectedIndex);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Output Word Name : {1}] [Bit : {2}]", "BtnOutputWordName_Click", BtnSelected.Text, false);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                    else if (false == bResult)
                    {
                        m_objCCLink.HLSetInterfaceValue(BtnSelected.Text, true, iSelectedIndex);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Output Word Name : {1}] [Bit : {2}]", "BtnOutputWordName_Click", BtnSelected.Text, true);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
                else if (HLDevice.CDeviceInterface.EError.ERROR_LICENSE == eError)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.LISENCE_KEY_RECOGNITION_FAILED);
                }
            }
            else
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.OUTPUT_SIGNAL_CHANGE_LOCK_STATUS);
            }
        }

        /// <summary>
        /// Word Input 출력 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInputWordName_Click(object sender, EventArgs e)
        {
            // 시뮬레이션 모드에서만 동작
            if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                Button BtnSelected = (Button)sender;
                int iSelectedIndex = 0;
                bool bResult = false;

                for (int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++)
                {
                    if (m_BtnInputWordName[iLoopCount] == BtnSelected)
                    {
                        iSelectedIndex = iLoopCount;
                        break;
                    }
                }

                HLDevice.CDeviceInterface.EError eError;
                eError = m_objCCLink.HLGetInterfaceValue(BtnSelected.Text, out bResult, iSelectedIndex);

                if (HLDevice.CDeviceInterface.EError.ERROR_NONE == eError)
                {
                    if (true == bResult)
                    {
                        m_objCCLink.HLSetInterfaceValue(BtnSelected.Text, false, iSelectedIndex);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Input Word Name : {1}] [Bit : {2}]", "BtnOutputWordName_Click", BtnSelected.Text, false);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                    else if (false == bResult)
                    {
                        m_objCCLink.HLSetInterfaceValue(BtnSelected.Text, true, iSelectedIndex);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Input Word Name : {1}] [Bit : {2}]", "BtnOutputWordName_Click", BtnSelected.Text, true);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
                else if (HLDevice.CDeviceInterface.EError.ERROR_LICENSE == eError)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.LISENCE_KEY_RECOGNITION_FAILED);
                }
            }
        }

        private void BtnGotoMid_Click(object sender, EventArgs e)
        {
            do
            {
                var pageType = (EPageIndex)((Control)sender).Tag;
                var pageIndex = (int)((Control)sender).Tag;
                var currentIndex = m_iCurrentPage[pageIndex];
                int robotCount = Enum.GetNames(typeof(CProcessMotion.ERobot)).Length;
                int sectionPageCount = (m_iMaxPage[pageIndex] + 1) / robotCount;
                int currentSection = currentIndex / sectionPageCount;

                if ((currentSection + 1) == robotCount)
                {
                    m_iCurrentPage[pageIndex] = 0;
                }
                else
                {
                    m_iCurrentPage[pageIndex] = (currentSection + 1) * sectionPageCount;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1} : {2:D}]", "BtnGotoMid_Click", pageType, m_iCurrentPage[(int)pageType]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                switch (pageType)
                {
                    case EPageIndex.BIT_INPUT:
                        SetDisplayInputBit();
                        break;
                    case EPageIndex.BIT_OUTPUT:
                        SetDisplayOutputBit();
                        break;
                    case EPageIndex.WORD_INPUT:
                        SetDisplayInputWord();
                        break;
                    case EPageIndex.WORD_OUTPUT:
                        SetDisplayOutputWord();
                        break;
                }
            } while (false);
        }

        private void BtnOutputLock_Click(object sender, EventArgs e)
        {
            mbOutputLock = !mbOutputLock;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Lock Mode : {1}]", "BtnOutputLock_Click", mbOutputLock);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnLabelViewToggle_Click(object sender, EventArgs e)
        {
            mbUseOffsetNameView = !mbUseOffsetNameView;
            SetDisplayInputBit();
            SetDisplayOutputBit();
            SetDisplayInputWord();
            SetDisplayOutputWord();
        }

    }
}