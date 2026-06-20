using HLDevice;
using HLDevice.Abstract;
using SVI_NFT_R.UI.UserControls;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormTeachInShuttle : CFormCommon, CFormInterface
    {
        private enum EMotor
        {
            STAGE_X = 0,
            MOTOR_FINAL
        };

        private enum EMotorPositionSet
        {
            MOTION_SET_POSITION_1 = 0,
            MOTION_SET_POSITION_2,
            MOTION_SET_POSITION_3,
            MOTION_SET_POSITION_4,
            MOTION_SET_POSITION_5,
            MOTION_SET_POSITION_FINAL
        };

        enum EPageIndex
        {
            PAGE_1 = 0,
            PAGE_2,
            PAGE_3,
            PAGE_4,
            PAGE_5,
            PAGE_FINAL
        };

        private readonly CDocument m_objDocument;
        private CDeviceMotor m_objMotor;
        private CProcessInterlock mInterlock;
        private EPageIndex m_ePageIndex;
        private int m_iMaxPage = 0;
        private double m_dCurrentPosition = double.MinValue;
        private int m_iPositionIndex = -1;
        private string m_strSelectedMotor;
        private CDeviceMotorAbstract.CMotorPosition m_objMotorPosition;
        private ImageButton[] m_BtnMotor = new ImageButton[(int)EMotor.MOTOR_FINAL];
        private readonly Button[] m_BtnPositionName = new Button[(int)EMotorPositionSet.MOTION_SET_POSITION_FINAL];
        private readonly Button[] m_BtnPositionValue = new Button[(int)EMotorPositionSet.MOTION_SET_POSITION_FINAL];
        private readonly Button[] m_BtnPositionCheck = new Button[(int)EMotorPositionSet.MOTION_SET_POSITION_FINAL];
        private CFormTeachData m_objFormTeachData;
        private InShuttle mInShuttleManager;
        private UcTeachVacuum[] mTeachVacuums;
        private bool mbUseBlowAutoOff = true;
        private bool mbAnalogDataView = false;
        private string mUnit = CDefine.UNIT_MILLIMETER;

        // 배치 동작 정의
        private const int BATCH_PAGE_ITME_MAX_COUNT = 5;
        private int mBatchMovePageCount;
        private int mBatchMovePageIndex;
        private BatchMoveInformation[] mBatchMovePage;

        private readonly BatchMoveInformation[] mBatchMovePageInShuttle = new BatchMoveInformation[]
        {
            new BatchMoveInformation(InShuttle.EBatch.ScanProcess),
            new BatchMoveInformation(InShuttle.EBatch.AlignProcess),
            null,
            null,
            null,

            new BatchMoveInformation(InShuttle.EBatch.MoveLoadPosition),
            new BatchMoveInformation(InShuttle.EBatch.MoveScanStartWaitPosition),
            new BatchMoveInformation(InShuttle.EBatch.MoveScanTriggerStartPosition),
            new BatchMoveInformation(InShuttle.EBatch.MoveScanTriggerEndPosition),
            new BatchMoveInformation(InShuttle.EBatch.MoveAlignPosition),

            new BatchMoveInformation(InShuttle.EBatch.MoveUnloadPosition),
            null,
            null,
            null,
            null,
        };

        private class BatchMoveInformation
        {
            /// <summary>
            /// 다국어 처리를 위한 컴포넌트 아이디 (네이밍 규칙: $"BtnBatchCmd{BatchCmd}")
            /// </summary>
            public string Uiid => $"BtnBatchCmd{BatchCmd}";
            /// <summary>
            /// 배치 동작의 이름
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 배치 동작 커맨드
            /// </summary>
            public InShuttle.EBatch BatchCmd { get; private set; }

            public BatchMoveInformation(InShuttle.EBatch batchCmd)
            {
                BatchCmd = batchCmd;
            }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormTeachInShuttle(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            InitializeComponent();

            // Display 영역에 맞게 Form 크기 피팅
            Size = PnlDisplayArea.Size;
            BackColor = PnlDisplayArea.BackColor;
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
        private void CFormTeachInShuttle_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormTeachInShuttle_FormClosed(object sender, FormClosedEventArgs e)
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
                mInShuttleManager = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle;

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
                // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
                m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
                // 버튼 색상 정의
                SetButtonColor();

                PnlDataArea.Dock = DockStyle.Right;

                // 모터 버튼 객체 연결
                m_BtnMotor[(int)EMotor.STAGE_X] = BtnMotorStageX;

                // 포지션 이름 버튼 객체 연결
                m_BtnPositionName[(int)EMotorPositionSet.MOTION_SET_POSITION_1] = BtnTitlePositionName1;
                m_BtnPositionName[(int)EMotorPositionSet.MOTION_SET_POSITION_2] = BtnTitlePositionName2;
                m_BtnPositionName[(int)EMotorPositionSet.MOTION_SET_POSITION_3] = BtnTitlePositionName3;
                m_BtnPositionName[(int)EMotorPositionSet.MOTION_SET_POSITION_4] = BtnTitlePositionName4;
                m_BtnPositionName[(int)EMotorPositionSet.MOTION_SET_POSITION_5] = BtnTitlePositionName5;

                // 포지션 값 버튼 객체 연결
                m_BtnPositionValue[(int)EMotorPositionSet.MOTION_SET_POSITION_1] = BtnPositionValue1;
                m_BtnPositionValue[(int)EMotorPositionSet.MOTION_SET_POSITION_2] = BtnPositionValue2;
                m_BtnPositionValue[(int)EMotorPositionSet.MOTION_SET_POSITION_3] = BtnPositionValue3;
                m_BtnPositionValue[(int)EMotorPositionSet.MOTION_SET_POSITION_4] = BtnPositionValue4;
                m_BtnPositionValue[(int)EMotorPositionSet.MOTION_SET_POSITION_5] = BtnPositionValue5;

                // 포지션 위치 체크 버튼 객체 연결
                m_BtnPositionCheck[(int)EMotorPositionSet.MOTION_SET_POSITION_1] = BtnPositionCheck1;
                m_BtnPositionCheck[(int)EMotorPositionSet.MOTION_SET_POSITION_2] = BtnPositionCheck2;
                m_BtnPositionCheck[(int)EMotorPositionSet.MOTION_SET_POSITION_3] = BtnPositionCheck3;
                m_BtnPositionCheck[(int)EMotorPositionSet.MOTION_SET_POSITION_4] = BtnPositionCheck4;
                m_BtnPositionCheck[(int)EMotorPositionSet.MOTION_SET_POSITION_5] = BtnPositionCheck5;

                // 포지션 페이지 인덱스
                m_ePageIndex = EPageIndex.PAGE_1;

                // 초기 모터 객체 설정
                m_objMotor = mInShuttleManager.MotorStageX.Axis;
                mInterlock = mInShuttleManager.MotorStageX.m_objInterlock;

                // 저장 폼 생성
                m_objFormTeachData = new CFormTeachData(m_objDocument, m_objMotor);
                m_objFormTeachData.TopLevel = false;
                m_objFormTeachData.Dock = DockStyle.Fill;
                PnlDataArea.Controls.Add(m_objFormTeachData);
                m_objFormTeachData.Show();

                // 현재 포지션 대리자 등록
                m_objFormTeachData.EventUpdatePosition += new CFormTeachData.UpdateCurrentPosition(GetCurrentPosition);
                // 모터 정보 대리자 등록
                m_objFormTeachData.EventUpdateMotor += new CFormTeachData.UpdateMotorInformation(SetMotorInformation);
                // 모터 포지션 인덱스 대리자 등록
                m_objFormTeachData.EventUpdateIndex += new CFormTeachData.UpdateMoveIndex(GetPositionIndex);
                // 모터 포지션 업데이트
                m_objFormTeachData.EventUpdatePositionParameter += new CFormTeachData.UpdatePositionInformation(GetCurrentPositionParameter);
                // 인터락 이벤트 
                m_objFormTeachData.EventCheckAbsolutePositionInterlock += CheckRelMoveInterlock;
                m_objFormTeachData.EventCheckInterlock += new CFormTeachData.DelegateCheckInterlock(CheckInterlock);
                m_objFormTeachData.EventCheckHomeInterlock += new CFormTeachData.DelegateCheckInterlock(CheckHomeInterlock);
                m_objFormTeachData.EventCheckJogInterlock += new CFormTeachData.DelegateCheckJogInterlock(CheckJogInterlock);

                // 베큠
                mTeachVacuums = new UcTeachVacuum[]
                {
                    UcTeachVacuum1,
                    UcTeachVacuum2,
                };
                UcTeachVacuum1.Initialize(this, m_objDocument, mInShuttleManager.Pickers[0].VacuumIndex, CDeviceIODefine.EDigitalInput.X_IN_SHUTTLE_P1_CELL_DETECT_SENSOR, GetIsBlowAutoOff, GetIsAnalogDataView);
                UcTeachVacuum2.Initialize(this, m_objDocument, mInShuttleManager.Pickers[1].VacuumIndex, CDeviceIODefine.EDigitalInput.X_IN_SHUTTLE_P2_CELL_DETECT_SENSOR, GetIsBlowAutoOff, GetIsAnalogDataView);
                // 배치 이동 페이지 선택
                SetBatchMovePage(mBatchMovePageInShuttle);

                // 첫 번째 모터 선택
                BtnMotorStageX.PerformClick();

                bReturn = true;
            } while (false);

            return bReturn;
        }

        private bool GetIsBlowAutoOff()
        {
            return mbUseBlowAutoOff;
        }

        private bool GetIsAnalogDataView()
        {
            return mbAnalogDataView;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            SetButtonBackColor(BtnTitle, m_colorLabel);
            SetButtonBackColor(BtnTitlePosition, m_colorLabelSub);
            SetButtonBackColor(BtnTitlePositionName1, m_colorLabelSub);
            SetButtonBackColor(BtnPositionValue1, m_colorLabelData);
            SetButtonBackColor(BtnTitlePositionName2, m_colorLabelSub);
            SetButtonBackColor(BtnPositionValue2, m_colorLabelData);
            SetButtonBackColor(BtnTitlePositionName3, m_colorLabelSub);
            SetButtonBackColor(BtnPositionValue3, m_colorLabelData);
            SetButtonBackColor(BtnTitlePositionName4, m_colorLabelSub);
            SetButtonBackColor(BtnPositionValue4, m_colorLabelData);
            SetButtonBackColor(BtnTitlePositionName5, m_colorLabelSub);
            SetButtonBackColor(BtnPositionValue5, m_colorLabelData);
            SetButtonBackColor(BtnPrevPosition, m_colorNormal);
            SetButtonBackColor(BtnNextPosition, m_colorNormal);
            SetButtonBackColor(BtnTitleActuator, m_colorLabelSub);

            SetButtonBackColor(BtnPrevBatch, m_colorNormal);
            SetButtonBackColor(BtnNextBatch, m_colorNormal);
            SetButtonBackColor(BtnTitleBatchMove, m_colorLabelCategory);

            PnlAlignDataLayout.BackColor = m_colorLabelCategory;
            SetButtonBackColor(BtnSubTitleAlignDataP1, m_colorLabelData);
            SetButtonBackColor(BtnAlignDataP1, m_colorLabelData);
            SetButtonBackColor(BtnSubTitleAlignDataP2, m_colorLabelData);
            SetButtonBackColor(BtnAlignDataP2, m_colorLabelData);

            var imageButtons = Controls.GetChildControlListByType(typeof(ImageButton));
            foreach (ImageButton imageButton in imageButtons)
            {
                base.SetButtonBackColor(imageButton, m_colorNormal);
            }

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnTitlePositionName1.Name)
                    && false == btn.Name.Equals(BtnTitlePositionName2.Name)
                    && false == btn.Name.Equals(BtnTitlePositionName3.Name)
                    && false == btn.Name.Equals(BtnTitlePositionName4.Name)
                    && false == btn.Name.Equals(BtnTitlePositionName5.Name)
                    && false == btn.Name.Equals(BtnPositionValue1.Name)
                    && false == btn.Name.Equals(BtnPositionValue2.Name)
                    && false == btn.Name.Equals(BtnPositionValue3.Name)
                    && false == btn.Name.Equals(BtnPositionValue4.Name)
                    && false == btn.Name.Equals(BtnPositionValue5.Name)
                    && false == btn.Name.Equals(BtnBatchIndex1.Name)
                    && false == btn.Name.Equals(BtnBatchIndex2.Name)
                    && false == btn.Name.Equals(BtnBatchIndex3.Name)
                    && false == btn.Name.Equals(BtnBatchIndex4.Name)
                    && false == btn.Name.Equals(BtnBatchIndex5.Name)
                    && false == btn.Name.Equals(BtnAlignDataP1.Name)
                    && false == btn.Name.Equals(BtnAlignDataP2.Name)
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
                SetControlButtonEnable(Controls, false);
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        SetControlButtonEnable(Controls, false);
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        SetControlButtonEnable(Controls, true);
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        SetControlButtonEnable(Controls, true);
                        break;
                    default:
                        break;
                }
            }
            // 오른쪽 조작 패널 상태 변경
            m_objFormTeachData.SetResourceControl();

            // 비활성화 상태에서도 항목 조회는 가능하도록 관련 항목 활성화
            SetButtonEnable(BtnPrevPosition, true);
            SetButtonEnable(BtnNextPosition, true);
            SetButtonEnable(BtnPrevBatch, true);
            SetButtonEnable(BtnNextBatch, true);
            SetButtonEnable(BtnMotorStageX, true);
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
                SetButtonChangeLanguage(BtnTitle);
                SetButtonChangeLanguage(BtnMotorStageX);
                SetButtonChangeLanguage(BtnTitlePosition);
                SetButtonChangeLanguage(BtnPrevPosition);
                SetButtonChangeLanguage(BtnNextPosition);
                SetButtonChangeLanguage(BtnTitleActuator);

                SetButtonChangeLanguage(BtnTitleBatchMove);
                SetButtonChangeLanguage(BtnPrevBatch);
                SetButtonChangeLanguage(BtnNextBatch);

                SetLabelChangeLanguage(LblTitleAlignData);

                var allBatchMovePages = mBatchMovePageInShuttle;
                foreach (var item in allBatchMovePages)
                {
                    if (null == item)
                    {
                        continue;
                    }
                    item.Name = m_objDocument.GetDatabaseUIText(item.Uiid, Name);
                }

                // 티칭 데이터 폼도 변경
                m_objFormTeachData.SetChangeLanguage();

                foreach (var vacuum in mTeachVacuums)
                {
                    vacuum.SetChangeLanguage();
                }

                SetButtonChangeLanguage(BtnUseBlowAutoOff);

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
            SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        private void SetLabelChangeLanguage(Label objLabel)
        {
            string uiText = m_objDocument.GetDatabaseUIText(objLabel.Name, Name);
            if (uiText != objLabel.Text)
            {
                objLabel.Text = uiText;
            }
        }

        /// <summary>
        /// 타이머 유무
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            timer.Enabled = bTimer;
            m_objFormTeachData.SetTimer(bTimer);
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
                mInShuttleManager = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle;
                UpdateData();
                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                m_objDocument.GetMainFrame().SetCurrentForm(this);
            }
            else
            {
                // 포지션 로그 정보 리셋
                m_objFormTeachData.m_objPositionReset(CFormTeachData.EPositionReset.RESET_POSITION_LOG_DATA);
            }
        }

        /// <summary>
        /// 타이머 동작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            // 베큠
            {
                foreach (var vacuum in mTeachVacuums)
                {
                    vacuum.UpdateUI();
                }
                SetButtonBackColor(BtnUseBlowAutoOff, mbUseBlowAutoOff ? m_colorOn : m_colorLabelSub, true);
            }

            // Align Data 업데이트
            {
                const int TITLE_MAX_LENGTH = 5;
                var alignData = mInShuttleManager.AlignVision.AlignResults;
                SetButtonBackColor(BtnAlignDataP1, alignData[0].CanUse ? m_colorOn : m_colorOff);
                SetButtonBackColor(BtnAlignDataP2, alignData[1].CanUse ? m_colorOn : m_colorOff);
                SetButtonText(BtnAlignDataP1, $"SCORE: {alignData[0].Score,4:0.0}\n{"X",TITLE_MAX_LENGTH}: {alignData[0].X,6:0.000} ( {CDefine.UNIT_MILLIMETER} )\n{"Y",TITLE_MAX_LENGTH}: {alignData[0].Y,6:0.000} ( {CDefine.UNIT_MILLIMETER} )\n{"T",TITLE_MAX_LENGTH}: {alignData[0].T,6:0.000} ( {CDefine.UNIT_ANGULAR} )");
                SetButtonText(BtnAlignDataP2, $"SCORE: {alignData[1].Score,4:0.0}\n{"X",TITLE_MAX_LENGTH}: {alignData[1].X,6:0.000} ( {CDefine.UNIT_MILLIMETER} )\n{"Y",TITLE_MAX_LENGTH}: {alignData[1].Y,6:0.000} ( {CDefine.UNIT_MILLIMETER} )\n{"T",TITLE_MAX_LENGTH}: {alignData[1].T,6:0.000} ( {CDefine.UNIT_ANGULAR} )");
            }

            // 배치 동작 정보 표시
            {
                Button[] batchButtons = new Button[]
                {
                    BtnBatchIndex1,
                    BtnBatchIndex2,
                    BtnBatchIndex3,
                    BtnBatchIndex4,
                    BtnBatchIndex5
                };
                int startIndex = mBatchMovePageIndex * BATCH_PAGE_ITME_MAX_COUNT;
                for (int i = 0; i < BATCH_PAGE_ITME_MAX_COUNT; i++)
                {
                    if (
                        (startIndex + i) >= mBatchMovePage.Length
                        || null == mBatchMovePage[startIndex + i]
                        )
                    {
                        SetButtonText(batchButtons[i], string.Empty);
                        batchButtons[i].Visible = false;
                        continue;
                    }

                    SetButtonText(batchButtons[i], mBatchMovePage[startIndex + i].Name);
                    batchButtons[i].Visible = true;

                    if (mInShuttleManager.BatchCommand == mBatchMovePage[startIndex + i].BatchCmd)
                    {
                        SetButtonBackColor(batchButtons[i], m_colorClick);
                    }
                    else
                    {
                        SetButtonBackColor(batchButtons[i], m_colorNormal);
                    }
                }
                SetButtonText(BtnTitleBatchPage, string.Format("PAGE ( {0} / {1} )", mBatchMovePageIndex + 1, mBatchMovePageCount));
                SetButtonBackColor(BtnTitleBatchMove, m_colorLabelCategory/* : m_colorLabelData*/);
            }

            // 포지션 이름 및 포지션 값 표시
            {
                for (int iLoopCount = 0; iLoopCount < (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL; iLoopCount++)
                {
                    int targetIndex = iLoopCount + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL);
                    base.SetButtonBackColor(m_BtnPositionName[iLoopCount], (m_iPositionIndex != targetIndex) ? m_colorNormal : m_colorClick);

                    // 포지션 이름
                    m_BtnPositionName[iLoopCount].Text = m_objMotorPosition.strPositionName[iLoopCount + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL)];

                    bool bVisible = !string.IsNullOrWhiteSpace(m_BtnPositionName[iLoopCount].Text);
                    if (m_BtnPositionName[iLoopCount].Visible != bVisible)
                    {
                        m_BtnPositionName[iLoopCount].Visible = bVisible;
                        m_BtnPositionValue[iLoopCount].Visible = bVisible;
                        m_BtnPositionCheck[iLoopCount].Visible = bVisible;
                    }

                    if (bVisible == false)
                    {
                        continue;
                    }

                    // 포지션 값
                    m_BtnPositionValue[iLoopCount].Text = string.Format("{0:F3} ( {1} )", m_objMotorPosition.dPosition[iLoopCount + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL)], mUnit);

                    // 포지션 위치 값 비교해서 표시
                    string strCurrentPosition = string.Format("{0:F3} ( {1} )", m_objMotor.HLGetMotorStatus().dEncoderPosition, mUnit);
                    if (m_BtnPositionValue[iLoopCount].Text == strCurrentPosition)
                    {
                        base.SetButtonBackColor(m_BtnPositionCheck[iLoopCount], m_colorOn);
                    }
                    else
                    {
                        base.SetButtonBackColor(m_BtnPositionCheck[iLoopCount], m_colorNormal);
                    }
                }
            }
        }

        /// <summary>
        /// 현재 모터 정보 갱신
        /// </summary>
        /// <param name="objMotor"></param>
        private void SetMotorInformation(ref CDeviceMotor objMotor)
        {
            if (null != m_objMotor)
            {
                objMotor = m_objMotor;
            }
        }

        /// <summary>
        /// 현재 포지션 값 로딩
        /// </summary>
        /// <param name="dPosition"></param>
        private void GetCurrentPosition(double dPosition)
        {
            m_dCurrentPosition = dPosition;
        }

        /// <summary>
        /// 현재 선택된 모터 포지션 클래스
        /// </summary>
        /// <param name="objMotorPosition"></param>
        private void GetCurrentPositionParameter(ref CDeviceMotorAbstract.CMotorPosition objMotorPosition)
        {
            objMotorPosition = m_objMotorPosition;
        }

        /// <summary>
        /// 현재 선택된 포지션 인덱스
        /// </summary>
        /// <param name="iPositionIndex"></param>
        private void GetPositionIndex(ref int iPositionIndex)
        {
            iPositionIndex = m_iPositionIndex;
        }

        /// <summary>
        /// 모터 포지션 업데이트
        /// </summary>
        private void UpdateData()
        {
            int iNameCount = 0;
            m_objMotorPosition = m_objMotor.HLGetMotorPosition();
            // 포지션 맥스 값 현재 50 후에 가변 적으로 변경
            for (int iLoopCount = 0; iLoopCount < 50; iLoopCount++)
            {
                if (0 != m_objMotorPosition.strPositionName[iLoopCount].Length)
                {
                    iNameCount++;
                }
            }

            // 선택된 포지션 초기화
            for (int iLoopCount = 0; iLoopCount < (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL; iLoopCount++)
            {
                base.SetButtonBackColor(m_BtnPositionName[iLoopCount], m_colorNormal);
            }
            m_iPositionIndex = -1;

            // 최대 페이지 설정
            m_iMaxPage = iNameCount / (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL;

            //kjpark 20190125 Teach Name 5개로 덜어질 경우 Next 버튼 눌러도 페이지 이동 안되게 수정
            if (iNameCount % (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL == 0)
            {
                m_iMaxPage = m_iMaxPage - 1;
            }
        }

        /// <summary>
        /// 상대 이동 인터락 체크
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CheckRelMoveInterlock(double arg)
        {
            return mInterlock.CheckMotionClassInterlock(m_strSelectedMotor, arg);
        }

        /// <summary>
        /// 인터락 체크
        /// </summary>
        /// <param name="bData"></param>
        private void CheckInterlock(ref bool bData)
        {
            if (false == mInterlock.CheckMotionClassInterlock(m_strSelectedMotor, m_objMotor.HLGetMotorPosition().dPosition[m_iPositionIndex]))
            {
                bData = false;
            }
            else
            {
                bData = true;
            }
        }

        /// <summary>
        /// 원점 이동 인터락 체크
        /// </summary>
        /// <param name="bData"></param>
        private void CheckHomeInterlock(ref bool bData)
        {
            if (false == mInterlock.CheckMotionClassInterlock(m_strSelectedMotor, CDefine.MANUAL_ORIGIN_POSITION_NO))
            {
                bData = false;
            }
            else
            {
                bData = true;
            }
        }

        /// <summary>
        /// 조그 이동 인터락 체크
        /// </summary>
        /// <param name="bData"></param>
        private void CheckJogInterlock(ref bool bData, bool bJogPositiveDirection, Action callbackInterlockPreActionOrNull)
        {
            if (false == mInterlock.CheckForcedInterlock(m_strSelectedMotor, bJogPositiveDirection, callbackInterlockPreActionOrNull))
            {
                bData = false;
            }
            else
            {
                bData = true;
            }
        }

        /// <summary>
        /// 이전 화면
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPrevPosition_Click(object sender, EventArgs e)
        {
            switch (m_ePageIndex)
            {
                case EPageIndex.PAGE_1:
                    break;
                case EPageIndex.PAGE_2:
                    m_ePageIndex = EPageIndex.PAGE_1;
                    break;
                case EPageIndex.PAGE_3:
                    m_ePageIndex = EPageIndex.PAGE_2;
                    break;
                case EPageIndex.PAGE_4:
                    m_ePageIndex = EPageIndex.PAGE_3;
                    break;
                case EPageIndex.PAGE_5:
                    m_ePageIndex = EPageIndex.PAGE_4;
                    break;
            }

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Page Index : {1}]", "BtnPrevPosition_Click", m_ePageIndex.ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 다음 화면
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextPosition_Click(object sender, EventArgs e)
        {
            if (m_iMaxPage <= (int)m_ePageIndex)
                return;

            switch (m_ePageIndex)
            {
                case EPageIndex.PAGE_1:
                    m_ePageIndex = EPageIndex.PAGE_2;
                    break;
                case EPageIndex.PAGE_2:
                    m_ePageIndex = EPageIndex.PAGE_3;
                    break;
                case EPageIndex.PAGE_3:
                    m_ePageIndex = EPageIndex.PAGE_4;
                    break;
                case EPageIndex.PAGE_4:
                    m_ePageIndex = EPageIndex.PAGE_5;
                    break;
                case EPageIndex.PAGE_5:
                    break;
            }

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Page Index : {1}]", "BtnNextPosition_Click", m_ePageIndex.ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 포지션 값 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPositionValue_Click(object sender, EventArgs e)
        {
            Button BtnSelected = (Button)sender;
            int iSelected = 0;
            if (m_BtnPositionValue[(int)EMotorPositionSet.MOTION_SET_POSITION_1] == BtnSelected)
            {
                iSelected = (int)EMotorPositionSet.MOTION_SET_POSITION_1;
            }
            else if (m_BtnPositionValue[(int)EMotorPositionSet.MOTION_SET_POSITION_2] == BtnSelected)
            {
                iSelected = (int)EMotorPositionSet.MOTION_SET_POSITION_2;
            }
            else if (m_BtnPositionValue[(int)EMotorPositionSet.MOTION_SET_POSITION_3] == BtnSelected)
            {
                iSelected = (int)EMotorPositionSet.MOTION_SET_POSITION_3;
            }
            else if (m_BtnPositionValue[(int)EMotorPositionSet.MOTION_SET_POSITION_4] == BtnSelected)
            {
                iSelected = (int)EMotorPositionSet.MOTION_SET_POSITION_4;
            }
            else if (m_BtnPositionValue[(int)EMotorPositionSet.MOTION_SET_POSITION_5] == BtnSelected)
            {
                iSelected = (int)EMotorPositionSet.MOTION_SET_POSITION_5;
            }
            if (double.MinValue != m_dCurrentPosition)
            {
                // 포지션 설정값 인터락 확인
                if (mInterlock.CheckMotorSettingPositionInterlock(m_objMotor.HLGetMotorInitializeParameter().strMotorName, iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL), m_dCurrentPosition) == false)
                {
                    m_dCurrentPosition = double.MinValue;
                    m_objFormTeachData.m_objPositionReset(CFormTeachData.EPositionReset.RESET_CURRENT_POSITION);
                    return;
                }

                // 포지션 저장 로그 정보
                string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ POSITION ] : {1}, [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotor.HLGetMotorInitializeParameter().strMotorName
                    , m_objMotorPosition.strPositionName[iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL)]
                    , m_objMotorPosition.dPosition[iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL)]
                    , m_dCurrentPosition);
                m_objFormTeachData.m_objTeachInformation(strTeachInformation);

                m_objMotorPosition.dPosition[iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL)] = m_dCurrentPosition;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Position Index : {1:D}] [Position : {2:F3}]", "BtnPositionValue_Click", iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL), m_dCurrentPosition);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                m_dCurrentPosition = double.MinValue;
                // 포지션 리셋 데이터
                m_objFormTeachData.m_objPositionReset(CFormTeachData.EPositionReset.RESET_CURRENT_POSITION);
            }
            else
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnPositionValue_Click", true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                CDeviceMotorAbstract.CMotorOperationParameter motorParam = new CDeviceMotorAbstract.CMotorOperationParameter();
                motorParam = m_objMotor.HLGetMotorOperationParameter();
                using (FormKeyPad objKeyPad = new FormKeyPad(m_objMotorPosition.dPosition[iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL)], motorParam.dLimitSoftwareMinus, motorParam.dLimitSoftwarePlus, m_BtnPositionName[iSelected].Text))
                {
                    if (DialogResult.OK == objKeyPad.ShowDialog())
                    {
                        // 포지션 설정값 인터락 확인
                        if (mInterlock.CheckMotorSettingPositionInterlock(m_objMotor.HLGetMotorInitializeParameter().strMotorName, iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL), objKeyPad.m_dResultValue) == false)
                        {
                            return;
                        }

                        // 포지션 저장 로그 정보
                        string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ POSITION ] : {1}, [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                            , m_objMotor.HLGetMotorInitializeParameter().strMotorName
                            , m_objMotorPosition.strPositionName[iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL)]
                            , m_objMotorPosition.dPosition[iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL)]
                            , objKeyPad.m_dResultValue);
                        m_objFormTeachData.m_objTeachInformation(strTeachInformation);

                        m_objMotorPosition.dPosition[iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL)] = objKeyPad.m_dResultValue;
                    }
                }

                // 버튼 로그 추가
                strLog = string.Format("[{0}] [Position Index : {1:D}] [Position : {2:F3}] [{3}]", "BtnPositionValue_Click", iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL), m_objMotorPosition.dPosition[iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL)], false);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }
        }

        /// <summary>
        /// 포지션 이름 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTitlePositionName_Click(object sender, EventArgs e)
        {
            Button BtnSelected = (Button)sender;
            int iSelected = 0;
            if (m_BtnPositionName[(int)EMotorPositionSet.MOTION_SET_POSITION_1] == BtnSelected)
            {
                iSelected = (int)EMotorPositionSet.MOTION_SET_POSITION_1;
            }
            else if (m_BtnPositionName[(int)EMotorPositionSet.MOTION_SET_POSITION_2] == BtnSelected)
            {
                iSelected = (int)EMotorPositionSet.MOTION_SET_POSITION_2;
            }
            else if (m_BtnPositionName[(int)EMotorPositionSet.MOTION_SET_POSITION_3] == BtnSelected)
            {
                iSelected = (int)EMotorPositionSet.MOTION_SET_POSITION_3;
            }
            else if (m_BtnPositionName[(int)EMotorPositionSet.MOTION_SET_POSITION_4] == BtnSelected)
            {
                iSelected = (int)EMotorPositionSet.MOTION_SET_POSITION_4;
            }
            else if (m_BtnPositionName[(int)EMotorPositionSet.MOTION_SET_POSITION_5] == BtnSelected)
            {
                iSelected = (int)EMotorPositionSet.MOTION_SET_POSITION_5;
            }
            int targetIndex = iSelected + ((int)m_ePageIndex * (int)EMotorPositionSet.MOTION_SET_POSITION_FINAL);
            // 선택된 포지션 이름이 없을 경우 break
            if (0 != m_objMotorPosition.strPositionName[targetIndex].Length)
            {
                if (m_iPositionIndex != targetIndex)
                {
                    // 선택된 포지션 인덱스 설정
                    m_iPositionIndex = targetIndex;
                }
                else
                {
                    // 선택된 포지션 재선택시 해당 위치로 이동
                    BtnTitlePositionName_DoubleClick(sender, e);
                    m_iPositionIndex = -1;
                }
            }
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Position Index : {1:D}]", "BtnTitlePositionName_Click", m_iPositionIndex);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnTitlePositionName_DoubleClick(object sender, EventArgs e)
        {
            int iPositionIndex = -1;
            bool bResult = false;
            do
            {
                try
                {
                    if (m_objDocument.m_objProcessMain.CheckDoorLock() == false)
                    {
                        break;
                    }
                    if (m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto() == false)
                    {
                        // Msg: 세이프티 모드를 AUTO로 변경하시기 바랍니다.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                        break;
                    }

                    // 현재 선택된 포지션의 인터락 체크 결과를 얻어 온다.
                    CheckInterlock(ref bResult);
                    if (false == bResult)
                    {
                        break;
                    }

                    // 현재 선택된 포지션 인덱스를 얻어온다.
                    GetPositionIndex(ref iPositionIndex);
                    if (0 <= iPositionIndex)
                    {
                        if (false == m_objMotor.HLMoveAbsoluteIndex(iPositionIndex, CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_MANUAL_RUN))
                        {
                            break;
                        }
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Position Index : {1:D}] [Velocity Mode : {2}]", "BtnPositionMove_Click", iPositionIndex, CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_MANUAL_RUN);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        private void BtnPrevBatch_Click(object sender, EventArgs e)
        {
            do
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnPrevBatch_Click", true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                SetBatchMovePageDecrease();

                // 버튼 로그 추가
                strLog = string.Format("[{0}] [{1}]", "BtnPrevBatch_Click", false);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        private void BtnNextBatch_Click(object sender, EventArgs e)
        {
            do
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnNextBatch_Click", true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                SetBatchMovePageIncrease();

                // 버튼 로그 추가
                strLog = string.Format("[{0}] [{1}]", "BtnNextBatch_Click", false);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        private async void BtnBatchIndexExecute_Click(object sender, EventArgs e)
        {
            do
            {
                int index = Convert.ToInt32(((Control)sender).Tag);
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnBatchIndexExecute_Click(index:{2})", true, index);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                await Task.Run(() => SetBatchMoveExecuteItem(index));

                // 버튼 로그 추가
                strLog = string.Format("[{0}] [{1}]", "BtnBatchIndexExecute_Click(index:{2})", false, index);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        private void SetBatchMovePage(BatchMoveInformation[] setPage)
        {
            do
            {
                if (mBatchMovePage == setPage)
                {
                    break;
                }

                mBatchMovePage = setPage;
                mBatchMovePageCount = ((mBatchMovePage.Length - 1) / BATCH_PAGE_ITME_MAX_COUNT) + 1;
                mBatchMovePageIndex = 0;

            } while (false);
        }

        private void SetBatchMovePageIncrease()
        {
            do
            {
                if ((mBatchMovePageIndex + 1) >= mBatchMovePageCount)
                {
                    break;
                }

                mBatchMovePageIndex++;
            } while (false);
        }

        private void SetBatchMovePageDecrease()
        {
            do
            {
                if ((mBatchMovePageIndex - 1) < 0)
                {
                    break;
                }

                mBatchMovePageIndex--;
            } while (false);
        }

        private void SetBatchMoveExecuteItem(int index)
        {
            do
            {
                if (m_objDocument.m_objProcessMain.CheckDoorLock() == false)
                {
                    break;
                }

                if (m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto() == false)
                {
                    // Msg: 세이프티 모드를 AUTO로 변경하시기 바랍니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                    break;
                }

                int targetIndex = (mBatchMovePageIndex * BATCH_PAGE_ITME_MAX_COUNT) + index;
                if (mBatchMovePage.Length < (targetIndex + 1))
                {
                    break;
                }

                if (m_objDocument.m_objProcessMain.m_objProcessMotion.CheckAllSequenceManagerStateIsBatchMoving() == true)
                {
                    break;
                }

                if (null == mBatchMovePage[targetIndex])
                {
                    break;
                }

                // Wait Dialog Show
                m_objDocument.GetMainFrame().ShowWaitMessage(true, mBatchMovePage[targetIndex].Name);

                // Batch Move Execute
                mInShuttleManager.BatchCommand = mBatchMovePage[targetIndex].BatchCmd;

                // Batch Move End Wait
                while (InShuttle.EBatch.None != mInShuttleManager.BatchCommand)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }

                // Wait Dialog Close
                m_objDocument.GetMainFrame().ShowWaitMessage(false);

            } while (false);
        }

        private void BtnPickerVacuumOnStatusToggle_Click(object sender, EventArgs e)
        {
            mbAnalogDataView = !mbAnalogDataView;
        }

        private void BtnUseBlowAutoOff_Click(object sender, EventArgs e)
        {
            mbUseBlowAutoOff = !mbUseBlowAutoOff;
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [Change : {mbUseBlowAutoOff}]");
        }

        /// <summary>
        /// 모터 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMotorSelect_Click(object sender, EventArgs e)
        {
            int selectedButtonIndex;
            ShowMotorVisible((ImageButton)sender, out selectedButtonIndex);
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [Selected Motor : {((EMotor)selectedButtonIndex)}]");
        }

        /// <summary>
        ///  모터 선택 UI 업데이트
        /// </summary>
        private void ShowMotorVisible(ImageButton selectedButton, out int selectedButtonIndex)
        {
            selectedButtonIndex = 0;
            // Sub Button을 가진 축을 거르기 위함.
            bool isMotorSelected = false;
            if (m_BtnMotor[(int)EMotor.STAGE_X] == selectedButton)
            {
                selectedButtonIndex = (int)EMotor.STAGE_X;
                PnlContent.Visible = true;
                isMotorSelected = true;
                SetBatchMovePage(mBatchMovePageInShuttle);
                mUnit = CDefine.UNIT_MILLIMETER;
                m_objMotor = mInShuttleManager.MotorStageX.Axis;
                m_strSelectedMotor = mInShuttleManager.MotorStageX.MotorIndex.ToString();
            }
            foreach (var item in m_BtnMotor)
            {
                SetButtonBackColor(item, m_colorNormal);
            }
            SetButtonBackColor(m_BtnMotor[selectedButtonIndex], m_colorClick);

            if (isMotorSelected)
            {
                ShowMotorPosition();
            }
        }

        /// <summary>
        /// 모터 포지션 정보 UI 업데이트
        /// </summary>
        private void ShowMotorPosition()
        {
            // 포지션 로그 정보 리셋
            m_objFormTeachData.m_objPositionReset(CFormTeachData.EPositionReset.RESET_POSITION_LOG_DATA);
            // 첫페이지 전환
            m_ePageIndex = EPageIndex.PAGE_1;
            UpdateData();
        }

        private void BtnAlignDataP1_Click(object sender, EventArgs e)
        {
            if (InShuttle.EBatch.None != mInShuttleManager.BatchCommand)
            {
                return;
            }

            // Wait Dialog Show
            m_objDocument.GetMainFrame().ShowWaitMessage(true, "ALIGN GRAB 1");

            // Batch Move Execute
            mInShuttleManager.BatchCommand = InShuttle.EBatch.AlignGrabP1;

            // Batch Move End Wait
            while (InShuttle.EBatch.None != mInShuttleManager.BatchCommand)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }

            // Wait Dialog Close
            m_objDocument.GetMainFrame().ShowWaitMessage(false);
        }

        private void BtnAlignDataP2_Click(object sender, EventArgs e)
        {
            if (InShuttle.EBatch.None != mInShuttleManager.BatchCommand)
            {
                return;
            }

            // Wait Dialog Show
            m_objDocument.GetMainFrame().ShowWaitMessage(true, "ALIGN GRAB 2");

            // Batch Move Execute
            mInShuttleManager.BatchCommand = InShuttle.EBatch.AlignGrabP2;

            // Batch Move End Wait
            while (InShuttle.EBatch.None != mInShuttleManager.BatchCommand)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }

            // Wait Dialog Close
            m_objDocument.GetMainFrame().ShowWaitMessage(false);
        }
    }
}