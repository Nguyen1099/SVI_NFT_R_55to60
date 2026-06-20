using SVI_NFT_R.DEVICE.Nachi;
using SVI_NFT_R.DEVICE.Nachi.SignalNames;
using SVI_NFT_R.UI.UserControls;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormTeachOutRobot : CFormCommon, CFormInterface
    {
        private readonly string ROBOT_NAME = "OUT ROBOT";
        private readonly CDocument m_objDocument;
        private OutRobot mOutRobotManager;
        private BindingSelection[] mBindingSelectionControls;
        private BindingText[] mBindingTextControls;
        private UcTeachVacuum[] mTeachVacuums;
        private UcTeachCylinder[] mTeachCylinders;
        private bool mbUseBlowAutoOff = true;
        private bool mbAnalogDataView = false;
        private double mJcmOffsetX = 0d;
        private double mJcmOffsetY = 0d;
        private double mJcmOffsetT = 0d;
        private ERobotProcess mSelectProcess = ERobotProcess.P1;
        private Button[] mBatchButtons;
        private Button[] mJcmToolSelectButtons;
        private Button[] mJcmToolSelectStatusButtons;
        private bool mbCanResourceControl = false;

        // TEST NACHI(TEST) ///////////////////////////////////////////////////////////////////////////////////
        private Thread mThreadRobotDryRun;
        public bool m_bContinue = true;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        // 배치 동작 정의
        private const int BATCH_PAGE_ITME_MAX_COUNT = 5;
        private int mBatchMovePageCount;
        private int mBatchMovePageIndex;
        private BatchMoveInformation[] mBatchMovePage;
        private readonly BatchMoveInformation[] mBatchMovePageP1 = new BatchMoveInformation[]
        {
            new BatchMoveInformation(OutRobot.EBatch.LoadGet),
            new BatchMoveInformation(OutRobot.EBatch.LoadPut),
            null,
            null,
            null,
        };
        private readonly BatchMoveInformation[] mBatchMovePageP4 = new BatchMoveInformation[]
        {
            new BatchMoveInformation(OutRobot.EBatch.UnloadGet),
            new BatchMoveInformation(OutRobot.EBatch.UnloadPut),
            null,
            null,
            null,
        };

        // JCM 커맨드 정의
        private const int JCM_COMMAND_PAGE_ITME_MAX_COUNT = 4;
        private int mJcmCommandPageCount;
        private int mJcmCommandPageIndex;
        private JcmCommandInformation[] mJcmCommandPage;

        // INSP STAGE
        private readonly JcmCommandInformation[] mJcmCommandPageP1 = new JcmCommandInformation[]
        {
            new JcmCommandInformation(ERobotProcess.P1, EJcmCommand.T1),
            new JcmCommandInformation(ERobotProcess.P1, EJcmCommand.T2),
        };
        // OUTFLIP
        private readonly JcmCommandInformation[] mJcmCommandPageP4 = new JcmCommandInformation[]
        {
            new JcmCommandInformation(ERobotProcess.P4, EJcmCommand.T1),
            new JcmCommandInformation(ERobotProcess.P4, EJcmCommand.T2),
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
            public OutRobot.EBatch BatchCmd { get; private set; }

            public BatchMoveInformation(OutRobot.EBatch batchCmd)
            {
                BatchCmd = batchCmd;
            }
        }

        private class JcmCommandInformation
        {
            /// <summary>
            /// 다국어 처리를 위한 컴포넌트 아이디 (네이밍 규칙: $"BtnJcmCmd{JcmCommand}")
            /// </summary>
            public string Uiid => $"BtnJcmCmd{JcmCommand}";
            /// <summary>
            /// JCM 커맨드의 이름
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// JCM 커맨드
            /// </summary>
            public EJcmCommand JcmCommand { get; private set; }
            /// <summary>
            /// 로봇 프로세스
            /// </summary>
            public ERobotProcess RobotProcess { get; private set; }

            public JcmCommandInformation(ERobotProcess robotProcess, EJcmCommand jcmCmd)
            {
                RobotProcess = robotProcess;
                JcmCommand = jcmCmd;
            }
        }

        public CFormTeachOutRobot(CDocument objDocument)
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

        private void CFormTeachOutputNachi_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        private void CFormTeachOutputNachi_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 해제
            DeInitialize();
        }

        public bool Initialize()
        {
            bool bReturn = false;

            do
            {
                mOutRobotManager = m_objDocument.m_objProcessMain.m_objProcessMotion.OutRobot;

                // 폼 초기화
                if (false == InitializeForm())
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public void DeInitialize()
        {
        }

        public bool InitializeForm()
        {
            bool bReturn = false;

            do
            {
                // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
                m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
                // 버튼 색상 정의
                SetButtonColor();
                SetButtonTag();

                // 수동 이동 페이지 선택
                mBatchButtons = new Button[]
                {
                    BtnBatchIndex1,
                    BtnBatchIndex2,
                    BtnBatchIndex3,
                    BtnBatchIndex4,
                    BtnBatchIndex5
                };
                mJcmToolSelectButtons = new Button[]
                {
                    BtnJcmModeCheckTeachingTool1,
                    BtnJcmModeCheckTeachingTool2,
                    BtnJcmModeCheckTeachingTool3,
                    BtnJcmModeCheckTeachingTool4
                };
                mJcmToolSelectStatusButtons = new Button[]
                {
                    BtnJcmModeCheckTeachingToolStatus1,
                    BtnJcmModeCheckTeachingToolStatus2,
                    BtnJcmModeCheckTeachingToolStatus3,
                    BtnJcmModeCheckTeachingToolStatus4
                };

                // 베큠
                mTeachVacuums = new UcTeachVacuum[]
                {
                    UcTeachVacuum1,
                    UcTeachVacuum2,
                };
                UcTeachVacuum1.Initialize(this, m_objDocument, mOutRobotManager.Pickers[0].VacuumIndex, CDeviceIODefine.EDigitalInput.X_OUT_ROBOT_P1_CELL_DETECT_SENSOR, GetIsBlowAutoOff, GetIsAnalogDataView);
                UcTeachVacuum2.Initialize(this, m_objDocument, mOutRobotManager.Pickers[1].VacuumIndex, CDeviceIODefine.EDigitalInput.X_OUT_ROBOT_P2_CELL_DETECT_SENSOR, GetIsBlowAutoOff, GetIsAnalogDataView);
                mTeachCylinders = new UcTeachCylinder[]
                {
                    UcTeachCylinder1,
                    UcTeachCylinder2,
                };
                UcTeachCylinder1.Initialize(this, m_objDocument, mOutRobotManager.TurnCylinders[0].CylinderIndex, mOutRobotManager.TurnCylinders[0].m_objInterlock.CheckCylinderInterlock);
                UcTeachCylinder2.Initialize(this, m_objDocument, mOutRobotManager.TurnCylinders[1].CylinderIndex, mOutRobotManager.TurnCylinders[1].m_objInterlock.CheckCylinderInterlock);

                // 배치 이동 페이지 선택
                BtnMotorNachiP1.PerformClick();

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

        private void SetButtonTag()
        {
            mBindingTextControls = Controls
                .GetChildControlListByType(typeof(Label))
                .Concat(Controls.GetChildControlListByType(typeof(Button)))
                .Concat(Controls.GetChildControlListByType(typeof(SpeedButton)))
                .Where(item => item.Tag is BindingText)
                .Select(item => item.Tag as BindingText)
                .ToArray();

            mBindingSelectionControls = Controls
                .GetChildControlListByType(typeof(ImageButton))
                .Where(item => item.Tag is BindingSelection)
                .Select(item => item.Tag as BindingSelection)
                .ToArray();
        }

        private void SetButtonColor()
        {
            var imageButtons = Controls.GetChildControlListByType(typeof(ImageButton));
            foreach (ImageButton imageButton in imageButtons)
            {
                SetButtonBackColor(imageButton, m_colorNormal);
            }

            SetButtonBackColor(BtnTitle, m_colorLabel);
            SetButtonBackColor(BtnTitleBatchMove, m_colorLabelCategory);
            SetButtonBackColor(BtnTitleOperationStatus, m_colorLabel);
            SetButtonBackColor(BtnTitleAction, m_colorLabelSub);

            SetButtonBackColor(BtnTitleActuator, m_colorLabelSub);

            SetButtonBackColor(BtnStop, m_colorNormal);
            SetButtonBackColor(BtnPause, m_colorNormal);
            SetButtonBackColor(BtnMotorInitialize, m_colorNormal);

            SetButtonBackColor(BtnPrevBatch, m_colorNormal);
            SetButtonBackColor(BtnNextBatch, m_colorNormal);
            SetButtonBackColor(BtnTitleBatchPage, m_colorLabelData);

            SetButtonBackColor(BtnSubTitleJcmModeCheckTeaching, m_colorLabelSub);
            SetButtonBackColor(BtnSubTitleJcmModeOffset, m_colorLabelSub);
            SetButtonBackColor(BtnPrevJcm, m_colorNormal);
            SetButtonBackColor(BtnNextJcm, m_colorNormal);
            SetButtonBackColor(BtnTitleJcmPage, m_colorLabelData);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnBatchIndex1.Name)
                    && false == btn.Name.Equals(BtnBatchIndex2.Name)
                    && false == btn.Name.Equals(BtnBatchIndex3.Name)
                    && false == btn.Name.Equals(BtnBatchIndex4.Name)
                    && false == btn.Name.Equals(BtnBatchIndex5.Name)
                    && false == btn.Name.Equals(BtnJcmModeOffsetX.Name)
                    && false == btn.Name.Equals(BtnJcmModeOffsetY.Name)
                    && false == btn.Name.Equals(BtnJcmModeOffsetT.Name)
                    && false == btn.Name.Equals(BtnJcmModeCheckTeachingTool1.Name)
                    && false == btn.Name.Equals(BtnJcmModeCheckTeachingTool2.Name)
                    && false == btn.Name.Equals(BtnJcmModeCheckTeachingTool3.Name)
                    && false == btn.Name.Equals(BtnJcmModeCheckTeachingTool4.Name)
                    && false == btn.Name.Equals(BtnJcmModeSave.Name)
                    && false == btn.Name.Equals(BtnJcmModeNoSave.Name)
                    )
                {
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                    btn.BackColorChanged += NonClickableButton_BackColorChanged;
                    btn.Cursor = Cursors.Default;
                }
            }
        }

        private void SetResourceControl()
        {
            // 현재 유저 권한 레벨 받아옴
            CUserInformation objUserInformation = m_objDocument.GetUserInformation();

            // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                SetControlButtonEnable(Controls, false);
                mbCanResourceControl = false;
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        SetControlButtonEnable(Controls, false);
                        mbCanResourceControl = false;
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        SetControlButtonEnable(Controls, true);
                        mbCanResourceControl = true;
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        SetControlButtonEnable(Controls, true);
                        mbCanResourceControl = true;
                        break;
                    default:
                        break;
                }
            }

            // 비활성화 상태에서도 항목 조회는 가능하도록 관련 항목 활성화
            SetButtonEnable(BtnPrevBatch, true);
            SetButtonEnable(BtnNextBatch, true);
            SetButtonEnable(BtnPrevJcm, true);
            SetButtonEnable(BtnNextJcm, true);
            SetButtonEnable(BtnMotorNachiP1, true);
            SetButtonEnable(BtnMotorNachiP4, true);
        }

        public bool SetChangeLanguage()
        {
            bool bReturn = false;

            do
            {
                // 데이터 테이블에서 일치하는 ID에 해당하는 TEXT를 불러옴
                SetButtonChangeLanguage(BtnTitle);
                SetButtonChangeLanguage(BtnMotorNachiP1);
                SetButtonChangeLanguage(BtnMotorNachiP4);

                SetButtonChangeLanguage(BtnTitleBatchMove);

                SetButtonChangeLanguage(BtnTitleActuator);

                SetButtonChangeLanguage(BtnPrevBatch);
                SetButtonChangeLanguage(BtnNextBatch);

                SetButtonChangeLanguage(BtnTitleOperationStatus);
                SetButtonChangeLanguage(BtnTeachMode);
                SetButtonChangeLanguage(BtnPlayBackMode);
                SetButtonChangeLanguage(BtnRunningU1);
                SetButtonChangeLanguage(BtnUnderStopping);
                SetButtonChangeLanguage(BtnServoReady);
                SetButtonChangeLanguage(BtnHomePosition);
                SetButtonChangeLanguage(BtnAlarm);
                SetButtonChangeLanguage(BtnInposition);
                SetButtonChangeLanguage(BtnBatteryWarning);
                SetButtonChangeLanguage(BtnTitleAction);
                SetButtonChangeLanguage(BtnStop);
                SetButtonChangeLanguage(BtnPause);
                SetButtonChangeLanguage(BtnMotorInitialize);

                SetLabelChangeLanguage(LblTitleJcmMode);
                SetButtonChangeLanguage(BtnSubTitleJcmModeCheckTeaching);
                SetButtonChangeLanguage(BtnSubTitleJcmModeOffset);
                SetButtonChangeLanguage(BtnJcmModeSave);
                SetButtonChangeLanguage(BtnJcmModeNoSave);
                SetButtonChangeLanguage(BtnJcmMode);
                SetButtonChangeLanguage(BtnPrevJcm);
                SetButtonChangeLanguage(BtnNextJcm);

                foreach (var vacuum in mTeachVacuums)
                {
                    vacuum.SetChangeLanguage();
                }

                foreach (var cylinder in mTeachCylinders)
                {
                    cylinder.SetChangeLanguage();
                }

                SetButtonChangeLanguage(BtnUseBlowAutoOff);

                var batchMovePages = mBatchMovePageP1
                    .Concat(mBatchMovePageP4);
                foreach (var item in batchMovePages)
                {
                    if (null == item)
                    {
                        continue;
                    }
                    item.Name = m_objDocument.GetDatabaseUIText(item.Uiid, Name);
                }

                var jcmCommandPages = mJcmCommandPageP1
                    .Concat(mJcmCommandPageP4);
                foreach (var item in jcmCommandPages)
                {
                    if (null == item)
                    {
                        continue;
                    }
                    item.Name = m_objDocument.GetDatabaseUIText(item.Uiid, Name);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        private void SetButtonChangeLanguage(Button objButton)
        {
            SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

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

        public void SetTimer(bool bTimer)
        {
            timer.Enabled = bTimer;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 나찌 상태 표시
            // 서보 기동 준비 신호
            // 티치 모드 전환 ( 교시 모드 )
            var robotStatus = mOutRobotManager.Nachi.Robot.Status;
            if (true == robotStatus.IsTeachModeEntry)
            {
                SetButtonBackColor(BtnTeachMode, m_colorOn);
            }
            else
            {
                SetButtonBackColor(BtnTeachMode, m_colorNormal);
            }
            // 재생 모드 ( Unit Ready 를 보자 )
            if (true == robotStatus.IsRunningU1 && true == robotStatus.IsPlayBackModeEntry)
            {
                SetButtonBackColor(BtnPlayBackMode, m_colorOn);
            }
            else
            {
                SetButtonBackColor(BtnPlayBackMode, m_colorNormal);
            }
            // 서보 준비 완료 신호
            if (true == robotStatus.IsMotorEnergized)
            {
                SetButtonBackColor(BtnServoReady, m_colorOn);
            }
            else
            {
                SetButtonBackColor(BtnServoReady, m_colorNormal);
            }
            // 홈 위치 신호
            if (true == robotStatus.IsHomePositionU11)
            {
                SetButtonBackColor(BtnHomePosition, m_colorOn);
            }
            else
            {
                SetButtonBackColor(BtnHomePosition, m_colorNormal);
            }
            // 기동중
            if (true == robotStatus.IsRunningU1)
            {
                SetButtonBackColor(BtnRunningU1, m_colorOn);
            }
            else
            {
                SetButtonBackColor(BtnRunningU1, m_colorNormal);
            }
            // 정지중
            if (true == robotStatus.IsUnderStopping)
            {
                SetButtonBackColor(BtnUnderStopping, m_colorOff);
            }
            else
            {
                SetButtonBackColor(BtnUnderStopping, m_colorNormal);
            }
            // 알람 
            if (true == robotStatus.IsFailure)
            {
                SetButtonBackColor(BtnAlarm, m_colorOff);
            }
            else
            {
                SetButtonBackColor(BtnAlarm, m_colorNormal);
            }
            // 인포지션
            if (false == robotStatus.IsBusy)
            {
                SetButtonBackColor(BtnInposition, m_colorOn);
            }
            else
            {
                SetButtonBackColor(BtnInposition, m_colorNormal);
            }
            // 배터리 경고
            if (true == robotStatus.IsBatteryWarningM1)
            {
                SetButtonBackColor(BtnBatteryWarning, m_colorOff);
            }
            else
            {
                SetButtonBackColor(BtnBatteryWarning, m_colorNormal);
            }
            // 일시 정지
            if (false == mOutRobotManager.Nachi.Robot.Signals.Y.BB[Bit.EOutput.PAUSE].Value)
            {
                SetButtonBackColor(BtnPause, m_colorOn);
            }
            else
            {
                SetButtonBackColor(BtnPause, m_colorNormal);
            }
            // 에러 메시지
            var errorCode = mOutRobotManager.Nachi.Robot.Status.CurrentErrorCode;
            SetButtonBackColor(BtnErrorMessage, errorCode == 0 ? m_colorNormal : m_colorOff);
            SetButtonText(BtnErrorMessage, errorCode == 0 ? string.Empty : errorCode.ToString());

            // 프로그램 선택 버튼 정보 표시
            {
                SetButtonBackColor(BtnMotorNachiP1, (mBatchMovePage == mBatchMovePageP1) ? m_colorClick : m_colorNormal);
                SetButtonBackColor(BtnMotorNachiP4, (mBatchMovePage == mBatchMovePageP4) ? m_colorClick : m_colorNormal);

                int currentPositionIndex = mOutRobotManager.Nachi.Robot.Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].Value;
                SetButtonBackColor(BtnCurrentPosition1, currentPositionIndex == 1 ? m_colorOn : m_colorNormal);
                SetButtonBackColor(BtnCurrentPosition4, currentPositionIndex == 4 ? m_colorOn : m_colorNormal);
            }

            // 배치 동작 정보 표시
            {
                bool bIsJcmMode = mOutRobotManager.Nachi.Robot.Status.IsJcmMode;
                SetButtonBackColor(BtnTitleBatchMove, !bIsJcmMode ? m_colorLabelCategory : m_colorLabelData);
                int startIndex = mBatchMovePageIndex * BATCH_PAGE_ITME_MAX_COUNT;
                for (int i = 0; i < BATCH_PAGE_ITME_MAX_COUNT; i++)
                {
                    if ((startIndex + i) >= mBatchMovePage.Length
                        || null == mBatchMovePage[startIndex + i]
                        )
                    {
                        SetButtonText(mBatchButtons[i], string.Empty);
                        SetControlVisible(mBatchButtons[i], false);
                        continue;
                    }
                    SetButtonText(mBatchButtons[i], mBatchMovePage[startIndex + i].Name);
                    SetControlVisible(mBatchButtons[i], true);
                    SetButtonBackColor(mBatchButtons[i], mOutRobotManager.BatchCommand == mBatchMovePage[startIndex + i].BatchCmd ? m_colorClick : m_colorNormal);
                    SetControlEnabled(mBatchButtons[i], !bIsJcmMode && mbCanResourceControl);
                }
                SetButtonText(BtnTitleBatchPage, string.Format("PAGE ( {0} / {1} )", mBatchMovePageIndex + 1, mBatchMovePageCount));
            }

            // 로봇 진동 테스트 실행중 표시
            {
                SetButtonBackColor(BtnVibrationTestStart, (null != mThreadRobotDryRun) ? m_colorOn : m_colorNormal);
            }

            foreach (var item in mBindingSelectionControls)
            {
                ImageButton button = item.Sender as ImageButton;
                if (button != null)
                {
                    if (button.Name.Contains("Tool") == true)
                    {
                        SetButtonBackColor(button, (item.IsSelected) ? m_colorOutputOn : m_colorOutputOff, true);
                    }
                    else
                    {
                        SetButtonBackColor(button, (item.IsSelected) ? m_colorInputOn : m_colorInputOff, true);
                    }
                }
            }

            foreach (var item in mBindingTextControls)
            {
                item.UpdateControlText();
            }

            // 베큠
            {
                foreach (var vacuum in mTeachVacuums)
                {
                    if (vacuum.Visible == false)
                    {
                        continue;
                    }
                    vacuum.UpdateUI();
                }
                SetButtonBackColor(BtnUseBlowAutoOff, mbUseBlowAutoOff ? m_colorOn : m_colorLabelSub, true);
            }
            // 실린더
            {
                foreach (var cylinder in mTeachCylinders)
                {
                    cylinder.UpdateUI();
                }
            }

            // JCM Mode
            {
                bool bJcmMode = mOutRobotManager.Nachi.Robot.Status.IsJcmMode == true;
                Color backColor = bJcmMode ? m_colorLabelCategory : m_colorLabelData;
                if (PnlJcmModeBase.BackColor != backColor)
                {
                    PnlJcmModeBase.BackColor = backColor;
                }
                SetButtonBackColor(BtnJcmMode, bJcmMode ? m_colorClick : m_colorNormal);

                bool bCanJcmCheckTeaching = (
                    bJcmMode == true
                    && mOutRobotManager.Nachi.Robot.CanJcmEndCheckTeaching() == false
                    );
                SetButtonBackColor(BtnJcmModeOffsetX, bCanJcmCheckTeaching ? m_colorNormal : m_colorClick);
                SetButtonBackColor(BtnJcmModeOffsetY, bCanJcmCheckTeaching ? m_colorNormal : m_colorClick);
                SetButtonBackColor(BtnJcmModeOffsetT, bCanJcmCheckTeaching ? m_colorNormal : m_colorClick);
                SetControlEnabled(BtnJcmModeOffsetX, bCanJcmCheckTeaching && mbCanResourceControl);
                SetControlEnabled(BtnJcmModeOffsetY, bCanJcmCheckTeaching && mbCanResourceControl);
                SetControlEnabled(BtnJcmModeOffsetT, bCanJcmCheckTeaching && mbCanResourceControl);

                int startIndex = mJcmCommandPageIndex * JCM_COMMAND_PAGE_ITME_MAX_COUNT;
                for (int i = 0; i < JCM_COMMAND_PAGE_ITME_MAX_COUNT; i++)
                {
                    if ((startIndex + i) >= mJcmCommandPage.Length
                        || null == mJcmCommandPage[startIndex + i]
                        )
                    {
                        SetButtonText(mJcmToolSelectButtons[i], string.Empty);
                        SetControlVisible(mJcmToolSelectButtons[i], false);
                        SetControlVisible(mJcmToolSelectStatusButtons[i], false);
                        continue;
                    }

                    SetButtonText(mJcmToolSelectButtons[i], mJcmCommandPage[startIndex + i].Name);
                    SetControlVisible(mJcmToolSelectButtons[i], true);
                    SetControlVisible(mJcmToolSelectStatusButtons[i], true);

                    SetButtonBackColor(mJcmToolSelectButtons[i], bCanJcmCheckTeaching ? m_colorNormal : m_colorClick);
                    bool bIsArrival = mOutRobotManager.Nachi.Robot.Status.IsJcmCheckTeachingTool == mJcmCommandPage[startIndex + i].JcmCommand
                        && mOutRobotManager.Nachi.Robot.Status.CurrentPosition == (int)mJcmCommandPage[startIndex + i].RobotProcess
                        ;
                    SetButtonBackColor(mJcmToolSelectStatusButtons[i], bIsArrival ? m_colorGreen : m_colorNormal);
                    SetControlEnabled(mJcmToolSelectButtons[i], bCanJcmCheckTeaching && mbCanResourceControl);
                    SetControlEnabled(mJcmToolSelectStatusButtons[i], bCanJcmCheckTeaching && mbCanResourceControl);
                }

                bool bCanJcmEndCheckTeaching = mOutRobotManager.Nachi.Robot.CanJcmEndCheckTeaching();
                SetButtonBackColor(BtnJcmModeSave, bCanJcmEndCheckTeaching ? m_colorNormal : m_colorClick);
                SetButtonBackColor(BtnJcmModeNoSave, bCanJcmEndCheckTeaching ? m_colorNormal : m_colorClick);
                SetControlEnabled(BtnJcmModeSave, bCanJcmEndCheckTeaching && mbCanResourceControl);
                SetControlEnabled(BtnJcmModeNoSave, bCanJcmEndCheckTeaching && mbCanResourceControl);

                SetButtonText(BtnJcmModeOffsetX, $"「 X 」:{Environment.NewLine}{Environment.NewLine}{mJcmOffsetX,10:0.000} {CDefine.UNIT_MILLIMETER}");
                SetButtonText(BtnJcmModeOffsetY, $"「 Y 」:{Environment.NewLine}{Environment.NewLine}{mJcmOffsetY,10:0.000} {CDefine.UNIT_MILLIMETER}");
                SetButtonText(BtnJcmModeOffsetT, $"「 T 」:{Environment.NewLine}{Environment.NewLine}{mJcmOffsetT,10:0.000} {CDefine.UNIT_ANGULAR}");

                SetButtonText(BtnTitleJcmPage, string.Format("PAGE ( {0} / {1} )", mJcmCommandPageIndex + 1, mJcmCommandPageCount));
            }
        }

        public void SetControlEnabled(Control control, bool bEnable)
        {
            if (control.Enabled != bEnable)
            {
                control.Enabled = bEnable;
            }
        }

        public new void SetControlVisible(Control control, bool bVisible)
        {
            if (control.Visible != bVisible)
            {
                control.Visible = bVisible;
            }
        }

        public void SetVisible(bool bVisible)
        {
            Visible = bVisible;

            if (true == bVisible)
            {
                mOutRobotManager = m_objDocument.m_objProcessMain.m_objProcessMotion.OutRobot;
                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                m_objDocument.GetMainFrame().SetCurrentForm(this);
                // 로봇 진동 테스트 버튼 표시
                BtnVibrationTestStart.Visible = (
                    CDefine.ERunMode.UnlinkDryrun == m_objDocument.GetRunMode()
                    || null != mThreadRobotDryRun
                    );
            }
            else
            {
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}]", "BtnStop_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            // 일시 정지
            if (true == mOutRobotManager.Nachi.Robot.Signals.Y.BB[Bit.EOutput.PAUSE].Value)
            {
                mOutRobotManager.Nachi.Robot.SetPause();
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Pause : PAUSE]", "BtnPause_Click");
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }
            else
            {
                mOutRobotManager.Nachi.Robot.SetResume();
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Pause : RESUME]", "BtnPause_Click");
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }
        }

        private async void BtnMotorInitialize_Click(object sender, EventArgs e)
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

                if (m_objDocument.m_objProcessMain.m_objProcessMotion.CheckAllSequenceManagerStateIsBatchMoving() == true)
                {
                    break;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnMotorInitialize_Click", true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                m_objDocument.GetMainFrame().ShowWaitMessage(true, $"{ROBOT_NAME} INITIALIZE");
                if (false == mOutRobotManager.Nachi.m_objInterlock.CheckMotionClassInterlock("", (int)OutRobotNachi.ECommand.Initialize))
                {
                    break;
                }
                mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.Initialize);
                bool bFunctionResult = await Task.Factory.StartNew(() => mOutRobotManager.Nachi.WaitForEndProcess());
                if (false == bFunctionResult)
                {
                    break;
                }
                mOutRobotManager.SetInitializeCommand(CProcessAbstract.EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_DONE);

                // 버튼 로그 추가
                strLog = string.Format("[{0}] [{1}]", "BtnMotorInitialize_Click", false);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);

            m_objDocument.GetMainFrame().ShowWaitMessage(false);
        }

        private void BtnMotorNachiP1_Click(object sender, EventArgs e)
        {
            do
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnMotorNachiP1_Click", true);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                mSelectProcess = ERobotProcess.P1;

                SetJcmCommandPage(mJcmCommandPageP1);
                SetBatchMovePage(mBatchMovePageP1);

                // 버튼 로그 추가
                strLog = string.Format("[{0}] [{1}]", "BtnMotorNachiP1_Click", false);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        private void BtnMotorNachiP4_Click(object sender, EventArgs e)
        {
            do
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnMotorNachiP4_Click", true);
                m_objDocument.SetUpdateButtonLog(this, strLog);
                mSelectProcess = ERobotProcess.P4;

                SetJcmCommandPage(mJcmCommandPageP4);
                SetBatchMovePage(mBatchMovePageP4);

                // 버튼 로그 추가
                strLog = string.Format("[{0}] [{1}]", "BtnMotorNachiP4_Click", false);
                m_objDocument.SetUpdateButtonLog(this, strLog);
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

        private void BtnBatchIndexExecute_Click(object sender, EventArgs e)
        {
            do
            {
                int index = Convert.ToInt32(((Control)sender).Tag);
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnBatchIndexExecute_Click(index:{2})", true, index);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                SetBatchMoveExecuteItem(index);

                // 버튼 로그 추가
                strLog = string.Format("[{0}] [{1}]", "BtnBatchIndexExecute_Click(index:{2})", false, index);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        private void BtnPrevJcm_Click(object sender, EventArgs e)
        {
            do
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", MethodBase.GetCurrentMethod().Name, true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                SetJcmCommandPageDecrease();

                // 버튼 로그 추가
                strLog = string.Format("[{0}] [{1}]", MethodBase.GetCurrentMethod().Name, false);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        private void BtnNextJcm_Click(object sender, EventArgs e)
        {
            do
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", MethodBase.GetCurrentMethod().Name, true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                SetJcmCommandPageIncrease();

                // 버튼 로그 추가
                strLog = string.Format("[{0}] [{1}]", MethodBase.GetCurrentMethod().Name, false);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        private async void BtnJcmToolIndexExecute_Click(object sender, EventArgs e)
        {
            do
            {
                int index = Convert.ToInt32(((Control)sender).Tag);
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}(index:{index})] [{true}]");
                await Task.Run(() => SetJcmCommandExecuteItem(index));
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}(index:{index})] [{false}]");
            } while (false);
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            BindingSelection tagIndex = ((Control)sender).Tag as BindingSelection;
            m_objDocument.SetUpdateButtonLog(this, $"[{tagIndex.Sender.Name}] [{true}]");
            try
            {
                tagIndex.Select();
            }
            finally
            {
                m_objDocument.SetUpdateButtonLog(this, $"[{tagIndex.Sender.Name}] [{false}]");
            }
        }

        private void BtnMotorNachi_Click(object sender, EventArgs e)
        {
            if (m_objDocument.GetRunStatus() != CDefine.ERunStatus.Stop)
            {
                return;
            }

            if (mThreadRobotDryRun == null)
            {
                m_bContinue = false;
                mThreadRobotDryRun = new Thread(ThreadRobotDryRunProcess);
                mThreadRobotDryRun.Start();
            }
            else
            {
                m_objDocument.GetMainFrame().ShowWaitMessage(true, $"{ROBOT_NAME} 진동 테스트 프로세스 정지중");
                m_bContinue = true;
                var waitJobFinished = new Thread(() => mThreadRobotDryRun.Join());
                waitJobFinished.Start();
                while (null != mThreadRobotDryRun)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
                m_objDocument.GetMainFrame().ShowWaitMessage(false);
            }
        }

        private void ThreadRobotDryRunProcess()
        {
            int step = 0;

            while (false == m_bContinue)
            {
                bool bErrorOccured = true;
                switch (step)
                {
                    // Initialize
                    case 0x00:
                        if (mOutRobotManager.Nachi.m_objInterlock.CheckMotionClassInterlock(string.Empty, (int)OutRobotNachi.ECommand.Initialize) == false) break;
                        mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.Initialize);
                        if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        bErrorOccured = false;
                        break;

                    // Loop
                    case 0x01:
                        ////if (mOutRobotManager.Nachi.m_objInterlock.CheckMotionClassInterlock(string.Empty, (int)OutRobotNachi.ECommand.LoadPermit) == false) break;
                        //mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.LoadProcessBegin);
                        //if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        //mOutRobotManager.Nachi.ProcessPreOrder.Method = EMethod.Get;
                        //mOutRobotManager.Nachi.ProcessPreOrder.StageIndex = EStage.Stage2;
                        //mOutRobotManager.Nachi.ProcessPreOrder.ToolIndex = ETool.Tool2;
                        //mOutRobotManager.Nachi.ProcessMethodIndex = EMethod.Get;
                        //mOutRobotManager.Nachi.ProcessStageIndex = EStage.Stage1;
                        //mOutRobotManager.Nachi.ProcessToolIndex = ETool.Tool1;
                        //mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.LoadPermit);
                        //if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        //mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.LoadCycleEnd);
                        //if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        //mOutRobotManager.Nachi.ProcessStageIndex = EStage.Stage2;
                        //mOutRobotManager.Nachi.ProcessToolIndex = ETool.Tool2;
                        //mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.LoadPermit);
                        //if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        //mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.LoadCycleEnd);
                        //if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        //mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.LoadProcessExit);
                        //if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        bErrorOccured = false;
                        break;

                    case 0x02:
                        ////if (mOutRobotManager.Nachi.m_objInterlock.CheckMotionClassInterlock(string.Empty, (int)OutRobotNachi.ECommand.UnloadPermit) == false) break;
                        //mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.UnloadProcessBegin);
                        //if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        //mOutRobotManager.Nachi.ProcessPreOrder.Method = EMethod.Put;
                        //mOutRobotManager.Nachi.ProcessPreOrder.StageIndex = EStage.Stage2;
                        //mOutRobotManager.Nachi.ProcessPreOrder.ToolIndex = ETool.Tool2;
                        //mOutRobotManager.Nachi.ProcessMethodIndex = EMethod.Put;
                        //mOutRobotManager.Nachi.ProcessStageIndex = EStage.Stage1;
                        //mOutRobotManager.Nachi.ProcessToolIndex = ETool.Tool1;
                        //mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.UnloadPermit);
                        //if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        //mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.UnloadCycleEnd);
                        //if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        //mOutRobotManager.Nachi.ProcessStageIndex = EStage.Stage2;
                        //mOutRobotManager.Nachi.ProcessToolIndex = ETool.Tool2;
                        //mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.UnloadPermit);
                        //if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        //mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.UnloadCycleEnd);
                        //if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        //mOutRobotManager.Nachi.SetCommand(OutRobotNachi.ECommand.UnloadProcessExit);
                        //if (mOutRobotManager.Nachi.WaitForEndProcess() == false) break;
                        bErrorOccured = false;
                        break;

                    default:
                        step = 0x00;
                        bErrorOccured = false;
                        break;
                }
                if (bErrorOccured == true)
                {
                    break;
                }

                step++;
            }

            mThreadRobotDryRun = null;
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

                bool bJcmMode = mOutRobotManager.Nachi.Robot.Status.IsJcmMode == true;
                if (bJcmMode == true)
                {
                    break;
                }

                int targetIndex = (mBatchMovePageIndex * BATCH_PAGE_ITME_MAX_COUNT) + index;
                if (mBatchMovePage.Length < (targetIndex + 1))
                {
                    break;
                }

                if (OutRobot.EBatch.None != mOutRobotManager.BatchCommand)
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
                mOutRobotManager.BatchCommand = mBatchMovePage[targetIndex].BatchCmd;

                // Batch Move End Wait
                while (OutRobot.EBatch.None != mOutRobotManager.BatchCommand)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }

                // Wait Dialog Close
                m_objDocument.GetMainFrame().ShowWaitMessage(false);

            } while (false);
        }

        private void SetJcmCommandPage(JcmCommandInformation[] setPage)
        {
            do
            {
                if (mJcmCommandPage == setPage)
                {
                    break;
                }

                mJcmCommandPage = setPage;
                mJcmCommandPageCount = ((mJcmCommandPage.Length - 1) / JCM_COMMAND_PAGE_ITME_MAX_COUNT) + 1;
                mJcmCommandPageIndex = 0;

            } while (false);
        }

        private void SetJcmCommandPageIncrease()
        {
            do
            {
                if ((mJcmCommandPageIndex + 1) >= mJcmCommandPageCount)
                {
                    break;
                }

                mJcmCommandPageIndex++;
            } while (false);
        }

        private void SetJcmCommandPageDecrease()
        {
            do
            {
                if ((mJcmCommandPageIndex - 1) < 0)
                {
                    break;
                }

                mJcmCommandPageIndex--;
            } while (false);
        }

        private void SetJcmCommandExecuteItem(int index)
        {
            if (
                mOutRobotManager.Nachi.Robot.CanJcmExitMode() == false
                || mOutRobotManager.Nachi.Robot.CanJcmEndCheckTeaching() == true
                )
            {
                return;
            }
            if (m_objDocument.m_objProcessMain.CheckDoorLock() == false)
            {
                return;
            }
            if (m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto() == false)
            {
                // Msg: 세이프티 모드를 AUTO로 변경하시기 바랍니다.
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                return;
            }

            int targetIndex = (mJcmCommandPageIndex * JCM_COMMAND_PAGE_ITME_MAX_COUNT) + index;
            if (mJcmCommandPage.Length < (targetIndex + 1))
            {
                return;
            }

            if (mOutRobotManager.BatchCommand != OutRobot.EBatch.None)
            {
                return;
            }

            if (mJcmCommandPage[targetIndex] == null)
            {
                return;
            }

            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{true}]");
            m_objDocument.GetMainFrame().ShowWaitMessage(true, $"{ROBOT_NAME} CHECK TEACHING: {mJcmCommandPage[targetIndex].Name}");
            try
            {
                if (mOutRobotManager.Nachi.Robot.JcmSelectProcess(mSelectProcess) == false)
                {
                    return;
                }
                NachiRobotAlignData alignData = new NachiRobotAlignData();
                alignData.AlignTool1.X = alignData.AlignTool2.X = mJcmOffsetX;
                alignData.AlignTool1.Y = alignData.AlignTool2.Y = mJcmOffsetY;
                alignData.AlignTool1.T = alignData.AlignTool2.T = mJcmOffsetT;
                if (mOutRobotManager.Nachi.Robot.SendAlignData(alignData) == false)
                {
                    return;
                }
                if (mOutRobotManager.Nachi.Robot.JcmCheckTeaching(mJcmCommandPage[targetIndex].JcmCommand) == false)
                {
                    return;
                }
            }
            finally
            {
                m_objDocument.GetMainFrame().ShowWaitMessage(false);
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{false}]");
            }
        }

        private void BtnUseBlowAutoOff_Click(object sender, EventArgs e)
        {
            mbUseBlowAutoOff = !mbUseBlowAutoOff;
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [Change : {mbUseBlowAutoOff}]");
        }

        private async void BtnJcmMode_Click(object sender, EventArgs e)
        {
            if (m_objDocument.m_objProcessMain.CheckDoorLock() == false)
            {
                return;
            }
            if (m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto() == false)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                return;
            }
            if (mOutRobotManager.BatchCommand != OutRobot.EBatch.None)
            {
                return;
            }

            do
            {
                if (mOutRobotManager.Nachi.Robot.Status.IsJcmMode == true)
                {
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [JCM Mode : {1}] [{2}]", "BtnJcmMode_Click", false, true);
                    m_objDocument.SetUpdateButtonLog(this, strLog);

                    // JCM 모드 리셋
                    {
                        m_objDocument.GetMainFrame().ShowWaitMessage(true, $"{ROBOT_NAME} JCM MODE RESET");
                        if (await Task.Factory.StartNew(() => mOutRobotManager.Nachi.Robot.JcmExitMode()) == false)
                        {
                            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_IS_FAILED_TO_EXIT_MANUAL_MODE_PARAM1,
                                CProcessMotion.ERobot.IN_ROBOT);
                        }
                        // : 종료시 초기화 위치로 이동함
                    }

                    // 버튼 로그 추가
                    strLog = string.Format("[{0}] [JCM Mode : {1}] [{2}]", "BtnJcmMode_Click", false, false);
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
                else
                {
                    if (mOutRobotManager.Nachi.Robot.CanJcmEnterMode() == false)
                    {
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.THE_ROBOT_POSITION_IS_NOT_THE_ORIGIN_PLEASE_INITIALIZE_THE_ROBOT_AND_TRY_AGAIN);
                        break;
                    }

                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [JCM Mode : {1}] [{2}]", "BtnJcmMode_Click", true, true);
                    m_objDocument.SetUpdateButtonLog(this, strLog);

                    // JCM 모드 설정
                    {
                        m_objDocument.GetMainFrame().ShowWaitMessage(true, $"{ROBOT_NAME} JCM MODE SET");
                        if (await Task.Factory.StartNew(() => mOutRobotManager.Nachi.Robot.JcmEnterMode()) == false)
                        {
                            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_IS_FAILED_TO_ENTRY_MANUAL_MODE_PARAM1,
                                CProcessMotion.ERobot.IN_ROBOT);
                        }
                    }

                    // 버튼 로그 추가
                    strLog = string.Format("[{0}] [JCM Mode : {1}] [{2}]", "BtnJcmMode_Click", true, false);
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
            } while (false);
            m_objDocument.GetMainFrame().ShowWaitMessage(false);
        }

        private void BtnJcmModeOffsetX_Click(object sender, EventArgs e)
        {
            if (
                mOutRobotManager.Nachi.Robot.CanJcmExitMode() == false
                || mOutRobotManager.Nachi.Robot.CanJcmEndCheckTeaching() == true
                )
            {
                return;
            }

            double originalValue = mJcmOffsetX;
            using (FormKeyPad objKeyPad = new FormKeyPad(originalValue))
            {
                if (objKeyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [Change : {originalValue:0.000} -> {objKeyPad.m_dResultValue:0.000}]");
                mJcmOffsetX = objKeyPad.m_dResultValue;
            }
        }

        private void BtnJcmModeOffsetY_Click(object sender, EventArgs e)
        {
            if (
                mOutRobotManager.Nachi.Robot.CanJcmExitMode() == false
                || mOutRobotManager.Nachi.Robot.CanJcmEndCheckTeaching() == true
                )
            {
                return;
            }

            double originalValue = mJcmOffsetY;
            using (FormKeyPad objKeyPad = new FormKeyPad(originalValue))
            {
                if (objKeyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [Change : {originalValue:0.000} -> {objKeyPad.m_dResultValue:0.000}]");
                mJcmOffsetY = objKeyPad.m_dResultValue;
            }
        }

        private void BtnJcmModeOffsetT_Click(object sender, EventArgs e)
        {
            if (
                mOutRobotManager.Nachi.Robot.CanJcmExitMode() == false
                || mOutRobotManager.Nachi.Robot.CanJcmEndCheckTeaching() == true
                )
            {
                return;
            }

            double originalValue = mJcmOffsetT;
            using (FormKeyPad objKeyPad = new FormKeyPad(originalValue))
            {
                if (objKeyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [Change : {originalValue:0.000} -> {objKeyPad.m_dResultValue:0.000}]");
                mJcmOffsetT = objKeyPad.m_dResultValue;
            }
        }

        private async void BtnJcmModeSave_Click(object sender, EventArgs e)
        {
            if (mOutRobotManager.Nachi.Robot.CanJcmEndCheckTeaching() == false)
            {
                return;
            }
            if (m_objDocument.m_objProcessMain.CheckDoorLock() == false)
            {
                return;
            }
            if (m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto() == false)
            {
                // Msg: 세이프티 모드를 AUTO로 변경하시기 바랍니다.
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                return;
            }

            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{true}]");
            m_objDocument.GetMainFrame().ShowWaitMessage(true, $"{ROBOT_NAME} CHECK TEACHING SAVE");
            try
            {
                if (await Task.Factory.StartNew(() => mOutRobotManager.Nachi.Robot.JcmCheckTeachingSave()) == false)
                {
                    return;
                }
            }
            finally
            {
                m_objDocument.GetMainFrame().ShowWaitMessage(false);
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{false}]");
            }
        }

        private async void BtnJcmModeNoSave_Click(object sender, EventArgs e)
        {
            if (mOutRobotManager.Nachi.Robot.CanJcmEndCheckTeaching() == false)
            {
                return;
            }
            if (m_objDocument.m_objProcessMain.CheckDoorLock() == false)
            {
                return;
            }
            if (m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto() == false)
            {
                // Msg: 세이프티 모드를 AUTO로 변경하시기 바랍니다.
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                return;
            }

            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{true}]");
            m_objDocument.GetMainFrame().ShowWaitMessage(true, $"{ROBOT_NAME} CHECK TEACHING NO SAVE");
            try
            {
                if (await Task.Factory.StartNew(() => mOutRobotManager.Nachi.Robot.JcmCheckTeachingNoSave()) == false)
                {
                    return;
                }
            }
            finally
            {
                m_objDocument.GetMainFrame().ShowWaitMessage(false);
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{false}]");
            }
        }
    }
}