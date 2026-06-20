using System;
using System.Linq;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormSetupIO : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private readonly CDocument m_objDocument;
        /// <summary>
        /// IO Input & Output
        /// </summary>
        private enum EIO
        {
            INPUT = 0,
            OUTPUT,
            IO_FINAL
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
        private const int m_iDisplayPageIO = 32;
        // Input IO 버튼 배열
        private readonly Button[] m_BtnInputAddress = new Button[m_iDisplayPageIO];
        private readonly Button[] m_BtnInputName = new Button[m_iDisplayPageIO];

        // Output IO 버튼 배열
        private readonly Button[] m_BtnOutputAddress = new Button[m_iDisplayPageIO];
        private readonly Button[] m_BtnOutputName = new Button[m_iDisplayPageIO];

        private string[] mDiDisplayItems;
        private string[] mDoDisplayItems;

        /// <summary>
        /// IO 정보 객체
        /// </summary>
        private CConfig.CIOInitializeParameter m_objIOParameter;

        /// <summary>
        /// IO 객체
        /// </summary>
        private HLDevice.CDeviceIO m_objIO;
        private bool mbOutputLock = true;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormSetupIO(CDocument objDocument)
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
        private void CFormSetupIO_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormSetupIO_FormClosed(object sender, FormClosedEventArgs e)
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
                m_iCurrentPage = new int[(int)EIO.IO_FINAL];
                m_iMaxPage = new int[(int)EIO.IO_FINAL];

                // 버튼 매칭
                SetButtonControlInitialize();
                // IO 초기화 정보를 가져온다
                m_objIOParameter = m_objDocument.m_objConfig.GetIOParameter();

                // 해당 부분은 IO 디바이스에서 전체 수를 구해와서 나눠서 구해야 함
                int iInputIOCount = m_objIOParameter.objIOParameter.Values.Where(i => i.eIOType == CConfig.CIOInitializeParameter.EIOType.IO_TYPE_DI).Count();
                int iOutputIOCount = m_objIOParameter.objIOParameter.Values.Where(i => i.eIOType == CConfig.CIOInitializeParameter.EIOType.IO_TYPE_DO).Count();
                int[] iIOCount = { iInputIOCount, iOutputIOCount };

                for (int iLoopIO = 0; iLoopIO < (int)EIO.IO_FINAL; iLoopIO++)
                {
                    // 현재 페이지 설정
                    m_iCurrentPage[iLoopIO] = 0;
                    // 최대 페이지 설정
                    if (iIOCount[iLoopIO] % m_iDisplayPageIO == 0)
                    {
                        m_iMaxPage[iLoopIO] = (iIOCount[iLoopIO] / m_iDisplayPageIO) - 1;
                    }
                    else
                    {
                        m_iMaxPage[iLoopIO] = (iIOCount[iLoopIO] / m_iDisplayPageIO);
                    }
                }
                // 버튼 색상 정의
                SetButtonColor();
                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                SetDisplayInput();
                SetDisplayOutput();
                // IO 객체 
                m_objIO = m_objDocument.m_objProcessMain.m_objIO;
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
            base.SetButtonBackColor(BtnTitleInputList, m_colorLabel);
            base.SetButtonBackColor(BtnTitleOutputList, m_colorLabel);

            base.SetButtonBackColor(BtnPreviousInput, m_colorNormal);
            base.SetButtonBackColor(BtnNextInput, m_colorNormal);
            base.SetButtonBackColor(BtnPreviousOutput, m_colorNormal);
            base.SetButtonBackColor(BtnNextOutput, m_colorNormal);
            base.SetButtonBackColor(BtnTitleCurrentPageInput, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleCurrentPageOutput, m_colorLabelSub);

            base.SetButtonBackColor(BtnInputAddress01, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress02, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress03, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress04, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress05, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress06, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress07, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress08, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress09, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress10, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress11, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress12, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress13, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress14, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress15, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress16, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress17, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress18, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress19, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress20, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress21, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress22, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress23, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress24, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress25, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress26, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress27, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress28, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress29, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress30, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress31, m_colorLabelSub);
            base.SetButtonBackColor(BtnInputAddress32, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress01, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress02, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress03, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress04, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress05, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress06, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress07, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress08, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress09, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress10, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress11, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress12, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress13, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress14, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress15, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress16, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress17, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress18, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress19, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress20, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress21, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress22, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress23, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress24, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress25, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress26, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress27, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress28, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress29, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress30, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress31, m_colorLabelSub);
            base.SetButtonBackColor(BtnOutputAddress32, m_colorLabelSub);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnOutputName01.Name)
                    && false == btn.Name.Equals(BtnOutputName02.Name)
                    && false == btn.Name.Equals(BtnOutputName03.Name)
                    && false == btn.Name.Equals(BtnOutputName04.Name)
                    && false == btn.Name.Equals(BtnOutputName05.Name)
                    && false == btn.Name.Equals(BtnOutputName06.Name)
                    && false == btn.Name.Equals(BtnOutputName07.Name)
                    && false == btn.Name.Equals(BtnOutputName08.Name)
                    && false == btn.Name.Equals(BtnOutputName09.Name)
                    && false == btn.Name.Equals(BtnOutputName10.Name)
                    && false == btn.Name.Equals(BtnOutputName11.Name)
                    && false == btn.Name.Equals(BtnOutputName12.Name)
                    && false == btn.Name.Equals(BtnOutputName13.Name)
                    && false == btn.Name.Equals(BtnOutputName14.Name)
                    && false == btn.Name.Equals(BtnOutputName15.Name)
                    && false == btn.Name.Equals(BtnOutputName16.Name)
                    && false == btn.Name.Equals(BtnOutputName17.Name)
                    && false == btn.Name.Equals(BtnOutputName18.Name)
                    && false == btn.Name.Equals(BtnOutputName19.Name)
                    && false == btn.Name.Equals(BtnOutputName20.Name)
                    && false == btn.Name.Equals(BtnOutputName21.Name)
                    && false == btn.Name.Equals(BtnOutputName22.Name)
                    && false == btn.Name.Equals(BtnOutputName23.Name)
                    && false == btn.Name.Equals(BtnOutputName24.Name)
                    && false == btn.Name.Equals(BtnOutputName25.Name)
                    && false == btn.Name.Equals(BtnOutputName26.Name)
                    && false == btn.Name.Equals(BtnOutputName27.Name)
                    && false == btn.Name.Equals(BtnOutputName28.Name)
                    && false == btn.Name.Equals(BtnOutputName29.Name)
                    && false == btn.Name.Equals(BtnOutputName30.Name)
                    && false == btn.Name.Equals(BtnOutputName31.Name)
                    && false == btn.Name.Equals(BtnOutputName32.Name)
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
                base.SetButtonEnable(BtnPreviousInput, true);
                base.SetButtonEnable(BtnNextInput, true);
                base.SetButtonEnable(BtnPreviousOutput, true);
                base.SetButtonEnable(BtnNextOutput, true);
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        base.SetControlButtonEnable(Controls, false);
                        base.SetButtonEnable(BtnPreviousInput, true);
                        base.SetButtonEnable(BtnNextInput, true);
                        base.SetButtonEnable(BtnPreviousOutput, true);
                        base.SetButtonEnable(BtnNextOutput, true);
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        base.SetControlButtonEnable(Controls, false);
                        base.SetButtonEnable(BtnPreviousInput, true);
                        base.SetButtonEnable(BtnNextInput, true);
                        base.SetButtonEnable(BtnPreviousOutput, true);
                        base.SetButtonEnable(BtnNextOutput, true);
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
                SetButtonChangeLanguage(BtnTitleInputList);
                SetButtonChangeLanguage(BtnTitleOutputList);
                SetButtonChangeLanguage(BtnPreviousInput);
                SetButtonChangeLanguage(BtnNextInput);
                SetButtonChangeLanguage(BtnPreviousOutput);
                SetButtonChangeLanguage(BtnNextOutput);
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
            SetButtonText(BtnTitleCurrentPageInput, string.Format("PAGE ( {0} / {1} )", m_iCurrentPage[(int)EIO.INPUT] + 1, m_iMaxPage[(int)EIO.INPUT] + 1));
            SetButtonText(BtnTitleCurrentPageOutput, string.Format("PAGE ( {0} / {1} )", m_iCurrentPage[(int)EIO.OUTPUT] + 1, m_iMaxPage[(int)EIO.OUTPUT] + 1));

            // IO 상태 표시
            for (int i = 0; i < m_iDisplayPageIO; i++)
            {
                if (mDiDisplayItems.Length < i + 1)
                {
                    SetButtonBackColor(m_BtnInputName[i], m_colorInputOff);
                    continue;
                }

                string key = mDiDisplayItems[i];
                m_objIO.HLGetDigitalBit(m_objIOParameter.objIOParameter[key].strIOName, ref bResult);
                SetButtonBackColor(m_BtnInputName[i], bResult ? m_colorInputOn : m_colorInputOff);
                SetButtonEnable(m_BtnInputAddress[i], CDefine.OFF);
                SetButtonEnable(m_BtnInputName[i], m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON ? CDefine.ON : CDefine.OFF);
            }

            for (int i = 0; i < m_iDisplayPageIO; i++)
            {
                if (mDoDisplayItems.Length < i + 1)
                {
                    SetButtonBackColor(m_BtnOutputName[i], m_colorOutputOff);
                    continue;
                }

                string key = mDoDisplayItems[i];
                m_objIO.HLGetDigitalBit(m_objIOParameter.objIOParameter[key].strIOName, ref bResult);
                SetButtonBackColor(m_BtnOutputName[i], bResult ? m_colorOutputOn : m_colorOutputOff);
                SetButtonEnable(m_BtnOutputAddress[i], CDefine.OFF);
            }
            SetButtonEnable(BtnTitleInputList, CDefine.OFF);
            SetButtonEnable(BtnTitleOutputList, CDefine.OFF);
            SetButtonEnable(BtnTitleCurrentPageInput, CDefine.OFF);
            SetButtonEnable(BtnTitleCurrentPageOutput, CDefine.OFF);

            // IO 잠금 버튼 업데이트
            {
                SetButtonBackColor(BtnOutputLock, (true == mbOutputLock) ? m_colorOn : m_colorNormal, (true == mbOutputLock));
            }
        }

        /// <summary>
        /// 입력 이전 페이지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPreviousInput_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 >= m_iCurrentPage[(int)EIO.INPUT])
                {
                    break;
                }
                m_iCurrentPage[(int)EIO.INPUT]--;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Input Page : {1:D}]", "BtnPreviousInput_Click", m_iCurrentPage[(int)EIO.INPUT]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                // IO 관련 페이지 Repaint
                SetDisplayInput();
            } while (false);
        }

        /// <summary>
        /// 입력 다음 페이지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextInput_Click(object sender, EventArgs e)
        {
            do
            {
                if (m_iMaxPage[(int)EIO.INPUT] <= m_iCurrentPage[(int)EIO.INPUT])
                {
                    break;
                }
                m_iCurrentPage[(int)EIO.INPUT]++;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Input Page : {1:D}]", "BtnNextInput_Click", m_iCurrentPage[(int)EIO.INPUT]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                // IO 관련 페이지 Repaint
                SetDisplayInput();
            } while (false);
        }

        /// <summary>
        /// 출력 이전 페이지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPreviousOutput_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 >= m_iCurrentPage[(int)EIO.OUTPUT])
                {
                    break;
                }
                m_iCurrentPage[(int)EIO.OUTPUT]--;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Output Page : {1:D}]", "BtnPreviousOutput_Click", m_iCurrentPage[(int)EIO.OUTPUT]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                // IO 관련 페이지 Repaint
                SetDisplayOutput();
            } while (false);
        }

        /// <summary>
        /// 출력 다음 페이지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextOutput_Click(object sender, EventArgs e)
        {
            do
            {
                if (m_iMaxPage[(int)EIO.OUTPUT] <= m_iCurrentPage[(int)EIO.OUTPUT])
                {
                    break;
                }
                m_iCurrentPage[(int)EIO.OUTPUT]++;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Current Output Page : {1:D}]", "BtnNextOutput_Click", m_iCurrentPage[(int)EIO.OUTPUT]);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                // IO 관련 페이지 Repaint
                SetDisplayOutput();
            } while (false);
        }

        /// <summary>
        /// 입력 IO 표시
        /// </summary>
        private void SetDisplayInput()
        {
            mDiDisplayItems = m_objIOParameter.objIOParameter
                .Where(i => i.Value.eIOType == CConfig.CIOInitializeParameter.EIOType.IO_TYPE_DI)
                .Skip(m_iCurrentPage[(int)EIO.INPUT] * m_iDisplayPageIO)
                .Take(m_iDisplayPageIO)
                .Select(i => i.Value.strIndex)
                .ToArray();

            for (int i = 0; i < m_iDisplayPageIO; i++)
            {
                if (mDiDisplayItems.Length < i + 1)
                {
                    SetButtonText(m_BtnInputAddress[i], string.Empty);
                    SetButtonText(m_BtnInputName[i], string.Empty);
                    continue;
                }

                string key = mDiDisplayItems[i];
                SetButtonText(m_BtnInputAddress[i], m_objIOParameter.objIOParameter[key].strIndex);
                SetButtonText(m_BtnInputName[i], m_objIOParameter.objIOParameter[key].strIOName);
            }
        }

        /// <summary>
        /// 출력 IO 표시
        /// </summary>
        private void SetDisplayOutput()
        {
            mDoDisplayItems = m_objIOParameter.objIOParameter
                .Where(i => i.Value.eIOType == CConfig.CIOInitializeParameter.EIOType.IO_TYPE_DO)
                .Skip(m_iCurrentPage[(int)EIO.OUTPUT] * m_iDisplayPageIO)
                .Take(m_iDisplayPageIO)
                .Select(i => i.Value.strIndex)
                .ToArray();

            for (int i = 0; i < m_iDisplayPageIO; i++)
            {
                if (mDoDisplayItems.Length < i + 1)
                {
                    SetButtonText(m_BtnOutputAddress[i], string.Empty);
                    SetButtonText(m_BtnOutputName[i], string.Empty);
                    continue;
                }

                string key = mDoDisplayItems[i];
                SetButtonText(m_BtnOutputAddress[i], m_objIOParameter.objIOParameter[key].strIndex);
                SetButtonText(m_BtnOutputName[i], m_objIOParameter.objIOParameter[key].strIOName);
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
                // 입력 어드레스 버튼 매칭
                m_BtnInputAddress[iLoopCount] = (Controls.Find("BtnInputAddress" + strNo, true))[0] as Button;
                // 입력 IO 이름 버튼 매칭
                m_BtnInputName[iLoopCount] = (Controls.Find("BtnInputName" + strNo, true))[0] as Button;
                // 출력 어드레스 버튼 매칭
                m_BtnOutputAddress[iLoopCount] = (Controls.Find("BtnOutputAddress" + strNo, true))[0] as Button;
                // 출력 IO 이름 버튼 매칭
                m_BtnOutputName[iLoopCount] = (Controls.Find("BtnOutputName" + strNo, true))[0] as Button;
            }
        }

        /// <summary>
        /// IO 버튼 클릭 ( 출력 )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOutputName_Click(object sender, EventArgs e)
        {
            if (false == mbOutputLock)
            {
                Button BtnSelected = (Button)sender;
                bool bResult = false;

                if (true == m_objIO.HLGetDigitalBit(BtnSelected.Text, ref bResult))
                {
                    if (true == bResult)
                    {
                        m_objIO.HLSetDigitalBit(BtnSelected.Text, false);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Output Name : {1}] [Bit : {2}]", "BtnOutputName_Click", BtnSelected.Text, false);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                    else if (false == bResult)
                    {
                        m_objIO.HLSetDigitalBit(BtnSelected.Text, true);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Output Name : {1}] [Bit : {2}]", "BtnOutputName_Click", BtnSelected.Text, true);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
            }
            else
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.OUTPUT_SIGNAL_CHANGE_LOCK_STATUS);
            }
        }

        /// <summary>
        /// IO 버튼 클릭 ( 입력 )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInputName_Click(object sender, EventArgs e)
        {
            // 시뮬레이션 모드에서만 동작
            if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                Button BtnSelected = (Button)sender;
                bool bResult = false;

                if (true == m_objIO.HLGetDigitalBit(BtnSelected.Text, ref bResult))
                {
                    if (true == bResult)
                    {
                        m_objIO.HLSetDigitalBit(BtnSelected.Text, false);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Input Name : {1}] [Bit : {2}]", "BtnInputName_Click", BtnSelected.Text, false);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                    else if (false == bResult)
                    {
                        m_objIO.HLSetDigitalBit(BtnSelected.Text, true);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Input Name : {1}] [Bit : {2}]", "BtnInputName_Click", BtnSelected.Text, true);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
            }
        }

        private void BtnOutputLock_Click(object sender, EventArgs e)
        {
            mbOutputLock = !mbOutputLock;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Lock Mode : {1}]", "BtnOutputLock_Click", mbOutputLock);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }
    }
}