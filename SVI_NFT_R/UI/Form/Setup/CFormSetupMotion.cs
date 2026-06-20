using HLDevice;
using HLDevice.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormSetupMotion : CFormCommon, CFormInterface
    {
        // {342165FB-7315-468F-A424-1A2E1A2234F1}
        public readonly Guid GUID = new Guid(0x342165fb, 0x7315, 0x468f, 0xa4, 0x24, 0x1a, 0x2e, 0x1a, 0x22, 0x34, 0xf1);
        /// <summary>
        /// 모터 정보 칼럼 정의
        /// </summary>
        private enum EMotorInformationListColumn
        {
            NO = 0,
            NAME,
            READY,
            SERVO,
            ALARM,
            ORIGIN,
            MINUS_LIMIT,
            HOME,
            PLUS_LIMIT,
            MOTOR_INFORMATION_LIST_COLUMN_FINAL
        };
        /// <summary>
        /// 위치 정보 칼럼 정의
        /// </summary>
        private enum EPositionInformationListColumn
        {
            NAME = 0,
            POSITION,
            SPEED,
            ACC,
            DEC,
            POSITION_INFORMATION_LIST_COLUMN_FINAL
        };
        /// <summary>
        /// 페이지 인덱스
        /// </summary>
        private enum EPageIndex
        {
            PAGE_1 = 0,
            PAGE_2 = 1,
            PAGE_3 = 2,
            PAGE_4 = 3
        }
        /// <summary>
        /// 조그 속도 모드
        /// </summary>
        private enum EJogVelocityMode
        {
            JOG_FAST_MODE = 0,
            JOG_SLOW_MODE
        }

        /// <summary>
        /// 도큐먼트
        /// </summary>
        private readonly CDocument m_objDocument;
        /// <summary>
        /// 선택된 모터 정보 리스트 행
        /// </summary>
        private int m_iSelectedRow = 0;
        /// <summary>
        /// 모터 항목 현재 페이지
        /// </summary>
        private int m_iMotorCurrentPage = 0;
        /// <summary>
        /// 모터 항목 최대 페이지
        /// </summary>
        private int m_iMotorMaxPage = 0;
        /// <summary>
        /// 포지션 항목 현재 페이지
        /// </summary>
        private int m_iPositionCurrentPage = 0;
        /// <summary>
        /// 포지션 항목 최대 페이지
        /// </summary>
        private int m_iPositionMaxPage = 0;
        /// <summary>
        /// 한 페이지에 들어갈 셀 수량
        /// </summary>
        private readonly int m_iPageCount = 8;
        /// <summary>
        /// 상대 위치 입력 값
        /// </summary>
        private double m_dRelativeMoveValue = 0;
        /// <summary>
        /// 임의 모터 정보 데이터 리스트
        /// </summary>
        private List<CDeviceMotor> m_listMotorInformation;
        /// <summary>
        /// 인터락 데이터 리스트
        /// </summary>
        private List<CProcessInterlock> m_listInterlock;
        /// <summary>
        /// 조그 속도 모드 설정
        /// </summary>
        private EJogVelocityMode m_eJogVelocityMode;
        /// <summary>
        /// 티칭 정보 로그 저장 리스트
        /// </summary>
        private List<string> m_listTeachInformation;
        /// <summary>
        /// 티칭 옵션 리스트
        /// </summary>
        private List<string> m_listTeachOptionInformation;
        private bool mbJogMoving = false;
        private bool mbJogPositiveDirection = false;
        private bool mbBlinkSignal = false;
        private TimeSpan mBlinkChangeTime = TimeSpan.FromSeconds(1);
        private readonly Stopwatch mBlinkTimer = new Stopwatch();

        /// <summary>
        /// 모터 포지션 객체
        /// </summary>
        private CDeviceMotorAbstract.CMotorPosition m_objMotorPosition = new CDeviceMotorAbstract.CMotorPosition();

        /// <summary>
        /// 모터 운용 파라미터 객체
        /// </summary>
        private CDeviceMotorAbstract.CMotorOperationParameter m_objMotorOperationParameter = new CDeviceMotorAbstract.CMotorOperationParameter();

        /// <summary>
        /// 모터 초기화 파라미터 객체
        /// </summary>
        private CDeviceMotorAbstract.CMotorInitializeParameter m_objMotorInitializeParameter = new CDeviceMotorAbstract.CMotorInitializeParameter();

        /// <summary>
        /// 모터 상태 정보 객체
        /// </summary>
        private CDeviceMotorAbstract.CMotorStatus m_objMotorStatus = new CDeviceMotorAbstract.CMotorStatus();
        private Task[] mTaskOriginEndWait;
        private int[] mHomeSteps;
        private int[] mHomeMainSteps;
        private int[][] mRepeatPositions;
        private int[] mRepeatCounts;
        private string mResourceTitleRepeatPositionCount;
        private string mResourceTitleRepeatCount;
        private string mResourceTitleRepeatPosition;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormSetupMotion(CDocument objDocument)
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
        private void CFormSetupMotion_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormSetupMotion_FormClosed(object sender, FormClosedEventArgs e)
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
                base.m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
                // 모터 정보 리스트 그리드 뷰 초기화
                string[] strMotorInformationList =
                {
                    EMotorInformationListColumn.NO.ToString(),
                    EMotorInformationListColumn.NAME.ToString(),
                    EMotorInformationListColumn.READY.ToString(),
                    EMotorInformationListColumn.SERVO.ToString(),
                    EMotorInformationListColumn.ALARM.ToString(),
                    EMotorInformationListColumn.ORIGIN.ToString(),
                    "- LIMIT",
                    EMotorInformationListColumn.HOME.ToString(),
                    "+ LIMIT",
                };
                if (false == InitializeGridView(GridViewMotorInformationList, strMotorInformationList))
                    break;
                // 위치 정보 리스트 그리드 뷰 초기화
                string[] strPositionInformationList =
                {
                    EPositionInformationListColumn.NAME.ToString(),
                    EPositionInformationListColumn.POSITION.ToString(),
                    EPositionInformationListColumn.SPEED.ToString(),
                    EPositionInformationListColumn.ACC.ToString(),
                    EPositionInformationListColumn.DEC.ToString(),
                };
                if (false == InitializeGridView(GridViewPositionInformationList, strPositionInformationList))
                {
                    break;
                }

                var orderItems = m_objDocument.m_objProcessMain.m_objProcessMotion.MotorInterlocks.ToList()
                    .OrderBy(i => m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[i.Key].HLGetMotorInitializeParameter().iAxisNo);
                m_listMotorInformation = orderItems.Select(i => m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[i.Key])
                    .ToList();
                m_listInterlock = orderItems.Select(i => i.Value)
                    .ToList();

                mTaskOriginEndWait = new Task[m_listMotorInformation.Count];
                mHomeSteps = new int[m_listMotorInformation.Count];
                mHomeMainSteps = new int[m_listMotorInformation.Count];

                mRepeatCounts = Enumerable.Repeat(10, m_listMotorInformation.Count).ToArray();
                mRepeatPositions = new int[m_listMotorInformation.Count][];

                // 최대 포지션 인덱스 설정
                if (m_listMotorInformation.Count % m_iPageCount == 0)
                {
                    m_iMotorMaxPage = (m_listMotorInformation.Count / m_iPageCount) - 1;
                }
                else
                {
                    m_iMotorMaxPage = (m_listMotorInformation.Count / m_iPageCount);
                }
                m_iPositionMaxPage = (int)EPageIndex.PAGE_2;
                SetMotorInformationGridView(m_listMotorInformation, m_iMotorCurrentPage);
                GridViewMotorInformationList.Rows[0].Selected = true;

                // 초기 조그 속도 모드 설정
                m_eJogVelocityMode = EJogVelocityMode.JOG_SLOW_MODE;

                // 버튼 색상 정의
                SetButtonColor();

                // 티칭 로그 정보 저장 리스트
                m_listTeachInformation = new List<string>();
                m_listTeachInformation.Clear();

                m_listTeachOptionInformation = new List<string>();
                m_listTeachOptionInformation.Clear();

                // 타이머 외부에서 제어
                timer.Interval = 10;
                timer.Enabled = false;

                mBlinkTimer.Start();

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
            //base.SetButtonBackColor(this.BtnTitle, m_colorLabel);
            base.SetButtonBackColor(BtnTitleMotorInformation, m_colorLabel);
            base.SetButtonBackColor(BtnTitlePositionInformation, m_colorLabel);
            base.SetButtonBackColor(BtnTitleMotorSetting, m_colorLabel);
            base.SetButtonBackColor(BtnTitleJogFast, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleJogSlow, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleAutoSpeed, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleManualSpeed, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleStandardAcceleration, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleStandardDeceleration, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleStandardTimeOut, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleStandardTolerance, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleOriginSpeed, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleOriginOffset, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleLimitPositionPlus, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleLimitPositionMinus, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleDelayAfterMoving, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleInnerNo, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleUseMotor, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleUseHome, m_colorLabelSub);
            base.SetButtonBackColor(BtnMotorInformationPrevious, m_colorNormal);
            base.SetButtonBackColor(BtnMotorInformationNext, m_colorNormal);
            base.SetButtonBackColor(BtnMotorInformationPage, m_colorLabelSub);
            base.SetButtonBackColor(BtnPositionInformationPrevious, m_colorNormal);
            base.SetButtonBackColor(BtnPositionInformationNext, m_colorNormal);
            base.SetButtonBackColor(BtnPositionInformationPage, m_colorLabelSub);
            base.SetButtonBackColor(BtnServoOn, m_colorNormal);
            base.SetButtonBackColor(BtnServoOff, m_colorNormal);
            base.SetButtonBackColor(BtnServoReset, m_colorNormal);
            base.SetButtonBackColor(BtnOrigin, m_colorNormal);
            base.SetButtonBackColor(BtnTitleCurrentPosition, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleCommandPosition, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleRelativePosition, m_colorLabelSub);
            base.SetButtonBackColor(BtnRelativeMovePlus, m_colorNormal);
            base.SetButtonBackColor(BtnRelativeMoveMinus, m_colorNormal);
            base.SetButtonBackColor(BtnPositionMove, m_colorNormal);
            BtnPositionMove.ForeColor = Color.Blue;
            BtnPositionMove.PenColor = Color.Blue;
            base.SetButtonBackColor(BtnJogMovePlus, m_colorNormal);
            base.SetButtonBackColor(BtnJogMoveMinus, m_colorNormal);
            base.SetButtonBackColor(BtnPositionSave, m_colorNormal);
            base.SetButtonBackColor(BtnStop, m_colorNormal);
            base.SetButtonBackColor(BtnSave, m_colorNormal);
            base.SetButtonBackColor(BtnPositionJog, m_colorNormal);
            BtnPositionJog.ForeColor = Color.Blue;
            BtnPositionJog.PenColor = Color.Blue;

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnRelativePosition.Name)
                    && false == btn.Name.Equals(BtnTitleJogFast.Name)
                    && false == btn.Name.Equals(BtnTitleJogSlow.Name)
                    && false == btn.Name.Equals(BtnJogFast.Name)
                    && false == btn.Name.Equals(BtnJogSlow.Name)
                    && false == btn.Name.Equals(BtnAutoSpeed.Name)
                    && false == btn.Name.Equals(BtnManualSpeed.Name)
                    && false == btn.Name.Equals(BtnStandardAcceleration.Name)
                    && false == btn.Name.Equals(BtnStandardDeceleration.Name)
                    && false == btn.Name.Equals(BtnStandardTolerance.Name)
                    && false == btn.Name.Equals(BtnStandardTimeOut.Name)
                    && false == btn.Name.Equals(BtnOriginSpeed.Name)
                    && false == btn.Name.Equals(BtnOriginOffset.Name)
                    && false == btn.Name.Equals(BtnLimitPositionPlus.Name)
                    && false == btn.Name.Equals(BtnLimitPositionMinus.Name)
                    && false == btn.Name.Equals(BtnDelayAfterMoving.Name)
                    && false == btn.Name.Equals(BtnUseHome.Name)
                    && false == btn.Name.Equals(BtnCurrentPosition.Name)
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
            BtnRepeatMove.Visible = m_objDocument.IsMasterLogin;

            // 현재 유저 권한 레벨 받아옴
            CUserInformation objUserInformation = m_objDocument.GetUserInformation();

            // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                base.SetControlButtonEnable(Controls, false);
                base.SetButtonEnable(BtnMotorInformationPrevious, true);
                base.SetButtonEnable(BtnMotorInformationNext, true);
                base.SetButtonEnable(BtnPositionInformationPrevious, true);
                base.SetButtonEnable(BtnPositionInformationNext, true);
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        base.SetControlButtonEnable(Controls, false);
                        base.SetButtonEnable(BtnMotorInformationPrevious, true);
                        base.SetButtonEnable(BtnMotorInformationNext, true);
                        base.SetButtonEnable(BtnPositionInformationPrevious, true);
                        base.SetButtonEnable(BtnPositionInformationNext, true);
                        BtnPositionMove.ForeColor = Color.Black;
                        BtnPositionMove.PenColor = Color.Black;
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        base.SetControlButtonEnable(Controls, false);
                        base.SetButtonEnable(BtnMotorInformationPrevious, true);
                        base.SetButtonEnable(BtnMotorInformationNext, true);
                        base.SetButtonEnable(BtnPositionInformationPrevious, true);
                        base.SetButtonEnable(BtnPositionInformationNext, true);
                        base.SetButtonEnable(BtnTitleJogFast, true);
                        base.SetButtonEnable(BtnTitleJogSlow, true);
                        base.SetButtonEnable(BtnServoOn, true);
                        base.SetButtonEnable(BtnServoOff, true);
                        base.SetButtonEnable(BtnServoReset, true);
                        base.SetButtonEnable(BtnOrigin, true);
                        base.SetButtonEnable(BtnRelativePosition, true);
                        base.SetButtonEnable(BtnRelativeMoveMinus, true);
                        base.SetButtonEnable(BtnRelativeMovePlus, true);
                        base.SetButtonEnable(BtnJogMoveMinus, true);
                        base.SetButtonEnable(BtnJogMovePlus, true);
                        base.SetButtonEnable(BtnPositionMove, true);
                        base.SetButtonEnable(BtnPositionSave, true);
                        base.SetButtonEnable(BtnStop, true);
                        BtnPositionMove.ForeColor = Color.Blue;
                        BtnPositionMove.PenColor = Color.Blue;
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        base.SetControlButtonEnable(Controls, true);
                        BtnPositionMove.ForeColor = Color.Blue;
                        BtnPositionMove.PenColor = Color.Blue;
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
            bool bReturn;

            do
            {
                // 데이터 테이블에서 일치하는 ID에 해당하는 TEXT를 불러옴
                SetButtonChangeLanguage(BtnTitleMotorInformation);
                SetButtonChangeLanguage(BtnTitlePositionInformation);
                SetButtonChangeLanguage(BtnTitleMotorSetting);
                SetButtonChangeLanguage(BtnTitleJogFast);
                SetButtonChangeLanguage(BtnTitleJogSlow);
                SetButtonChangeLanguage(BtnTitleAutoSpeed);
                SetButtonChangeLanguage(BtnTitleManualSpeed);
                SetButtonChangeLanguage(BtnTitleStandardAcceleration);
                SetButtonChangeLanguage(BtnTitleStandardDeceleration);
                SetButtonChangeLanguage(BtnTitleStandardTolerance);
                SetButtonChangeLanguage(BtnTitleOriginSpeed);
                SetButtonChangeLanguage(BtnTitleOriginOffset);
                SetButtonChangeLanguage(BtnTitleLimitPositionPlus);
                SetButtonChangeLanguage(BtnTitleLimitPositionMinus);
                SetButtonChangeLanguage(BtnTitleDelayAfterMoving);
                SetButtonChangeLanguage(BtnTitleInnerNo);
                SetButtonChangeLanguage(BtnTitleUseHome);
                SetButtonChangeLanguage(BtnTitleUseMotor);
                SetButtonChangeLanguage(BtnTitleStandardTimeOut);
                SetButtonChangeLanguage(BtnMotorInformationPrevious);
                SetButtonChangeLanguage(BtnMotorInformationNext);
                SetButtonChangeLanguage(BtnPositionInformationPrevious);
                SetButtonChangeLanguage(BtnPositionInformationNext);
                SetButtonChangeLanguage(BtnServoOn);
                SetButtonChangeLanguage(BtnServoOff);
                SetButtonChangeLanguage(BtnServoReset);
                SetButtonChangeLanguage(BtnOrigin);
                SetButtonChangeLanguage(BtnTitleCurrentPosition);
                SetButtonChangeLanguage(BtnTitleCommandPosition);
                SetButtonChangeLanguage(BtnTitleRelativePosition);
                SetButtonChangeLanguage(BtnRelativeMovePlus);
                SetButtonChangeLanguage(BtnRelativeMoveMinus);
                SetButtonChangeLanguage(BtnPositionMove);
                SetButtonChangeLanguage(BtnJogMovePlus);
                SetButtonChangeLanguage(BtnJogMoveMinus);
                SetButtonChangeLanguage(BtnPositionSave);
                SetButtonChangeLanguage(BtnStop);
                SetButtonChangeLanguage(BtnSave);
                SetButtonChangeLanguage(BtnPositionJog);

                SetButtonChangeLanguage(BtnRepeatMove);
                mResourceTitleRepeatCount = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleRepeatCount), Name);
                mResourceTitleRepeatPosition = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleRepeatPosition), Name);
                mResourceTitleRepeatPositionCount = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleRepeatPositionCount), Name);

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
                SetDataReLoad();
            }
        }

        /// <summary>
        /// 그리드 뷰 초기화
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="strColumnName"></param>
        /// <returns></returns>
        private bool InitializeGridView(DataGridView objGridView, string[] strColumnName)
        {
            bool bReturn = false;

            do
            {
                // 그리드 뷰 기본 스타일 초기화
                if (false == base.InitializeGridView(objGridView))
                {
                    break;
                }
                objGridView.RowHeadersVisible = true;
                objGridView.RowHeadersWidth = (int)(objGridView.Width * 0.06);

                objGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                // 한 화면에 페이지 수량만큼 나오도록 조정
                objGridView.RowTemplate.Height = ((objGridView.Height - objGridView.ColumnHeadersHeight) / m_iPageCount);
                // 그리드 뷰 ReadOnly
                objGridView.ReadOnly = true;
                // 그리드 뷰 다중 선택 x
                objGridView.MultiSelect = false;
                // 그리드 뷰 스크롤바 x
                objGridView.ScrollBars = ScrollBars.None;
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                objGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                // 그리드 뷰 칼럼 추가
                for (int iLoopColumn = 0; iLoopColumn < strColumnName.Length; iLoopColumn++)
                {
                    objGridView.Columns.Add(string.Format("{0}", iLoopColumn), strColumnName[iLoopColumn]);
                    // 칼럼 정렬 기능 x
                    objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                    // 칼럼 글자 가운데 정렬
                    objGridView.Columns[iLoopColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                // 그리드 뷰마다 사이즈 조절 필요..
                if ((int)EMotorInformationListColumn.MOTOR_INFORMATION_LIST_COLUMN_FINAL == strColumnName.Length)
                {
                    objGridView.Columns[(int)EMotorInformationListColumn.NO].Width = (int)(objGridView.Width * 0.05);
                    objGridView.Columns[(int)EMotorInformationListColumn.NAME].Width = (int)(objGridView.Width * 0.25);
                }
                else if ((int)EPositionInformationListColumn.POSITION_INFORMATION_LIST_COLUMN_FINAL == strColumnName.Length)
                {
                    objGridView.Columns[(int)EPositionInformationListColumn.POSITION].Width = (int)(objGridView.Width * 0.08);
                    objGridView.Columns[(int)EPositionInformationListColumn.SPEED].Width = (int)(objGridView.Width * 0.08);
                    objGridView.Columns[(int)EPositionInformationListColumn.ACC].Width = (int)(objGridView.Width * 0.08);
                    objGridView.Columns[(int)EPositionInformationListColumn.DEC].Width = (int)(objGridView.Width * 0.15);
                }
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, "신명조", 7.0, FontStyle.Regular);
                // Row 해더를 숨김
                objGridView.RowHeadersVisible = false;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 정보 그리드 뷰에 고정 틀로 뿌려줌
        /// </summary>
        /// <param name="objMotorInformationList"></param>
        /// <param name="iCurrentPage"></param>
        private void SetMotorInformationGridView(List<CDeviceMotor> objMotorInformationList, int iCurrentPage)
        {
            DataGridView objGridView = GridViewMotorInformationList;
            // 행 초기화
            objGridView.Rows.Clear();

            for (int iLoopMotorInformationList = 0; iLoopMotorInformationList < m_iPageCount; iLoopMotorInformationList++)
            {
                string[] strRowData = new string[(int)EMotorInformationListColumn.MOTOR_INFORMATION_LIST_COLUMN_FINAL];
                if (iLoopMotorInformationList + (m_iPageCount * iCurrentPage) >= objMotorInformationList.Count)
                    break;
                strRowData[(int)EMotorInformationListColumn.NO] = string.Format("{0}", iLoopMotorInformationList + (m_iPageCount * iCurrentPage));
                strRowData[(int)EMotorInformationListColumn.NAME] = objMotorInformationList[iLoopMotorInformationList + (m_iPageCount * iCurrentPage)].HLGetMotorInitializeParameter().strMotorName;
                objGridView.Rows.Add(strRowData);
            }
        }

        /// <summary>
        /// 모터 정보 그리드 뷰에 고정 틀로 뿌려줌
        /// </summary>
        /// <param name="objMotorInformationList"></param>
        /// <param name="iCurrentPage"></param>
        private void SetMotorInformationStatusCheck(List<CDeviceMotor> objMotorInformationList, int iCurrentPage)
        {
            DataGridView objGridView = GridViewMotorInformationList;
            for (int iLoopMotorInformationList = 0; iLoopMotorInformationList < m_iPageCount; iLoopMotorInformationList++)
            {
                if (iLoopMotorInformationList + (m_iPageCount * iCurrentPage) >= objMotorInformationList.Count)
                    break;
                CDeviceMotorAbstract.CMotorStatus objMotorStatus = objMotorInformationList[iLoopMotorInformationList + (m_iPageCount * iCurrentPage)].HLGetMotorStatus();
                // 모터 인포지션 상태 확인
                if (true == objMotorStatus.bInposition)
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.READY, iLoopMotorInformationList, m_colorOn);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.READY, iLoopMotorInformationList, "SET");
                }
                else
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.READY, iLoopMotorInformationList, m_colorOff);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.READY, iLoopMotorInformationList, string.Empty);
                }
                // 모터 서보 ON 상태 확인
                if (true == objMotorStatus.bServo)
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.SERVO, iLoopMotorInformationList, m_colorOn);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.SERVO, iLoopMotorInformationList, "ON");
                }
                else
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.SERVO, iLoopMotorInformationList, m_colorOff);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.SERVO, iLoopMotorInformationList, "OFF");
                }
                // 모터 알람 상태 확인
                if (true == objMotorStatus.bAlarm)
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.ALARM, iLoopMotorInformationList, m_colorOff);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.ALARM, iLoopMotorInformationList, "ALARM");
                }
                else
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.ALARM, iLoopMotorInformationList, Color.Empty);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.ALARM, iLoopMotorInformationList, string.Empty);
                }
                // 모터 오리진 상태 확인
                if (true == objMotorStatus.bHome)
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.ORIGIN, iLoopMotorInformationList, m_colorOn);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.ORIGIN, iLoopMotorInformationList, "ORIGIN");
                }
                else
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.ORIGIN, iLoopMotorInformationList, Color.Empty);
                    string displayText;
                    if (mTaskOriginEndWait[iLoopMotorInformationList + (m_iPageCount * iCurrentPage)] != null) displayText = $"{mHomeSteps[iLoopMotorInformationList + (m_iPageCount * iCurrentPage)],3} %";
                    else displayText = string.Empty;
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.ORIGIN, iLoopMotorInformationList, displayText);
                }
                // 모터 - 리미트 감지 확인
                if (false == objMotorStatus.bMinusLimitSensor)
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.MINUS_LIMIT, iLoopMotorInformationList, Color.Empty);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.MINUS_LIMIT, iLoopMotorInformationList, string.Empty);
                }
                else
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.MINUS_LIMIT, iLoopMotorInformationList, m_colorOff);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.MINUS_LIMIT, iLoopMotorInformationList, "NOT");
                }
                // 모터 홈 센서 감지 확인
                if (true == objMotorStatus.bHomeSensor)
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.HOME, iLoopMotorInformationList, m_colorOn);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.HOME, iLoopMotorInformationList, "HOME");
                }
                else
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.HOME, iLoopMotorInformationList, Color.Empty);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.HOME, iLoopMotorInformationList, string.Empty);
                }
                // 모터 + 리미트 감지 확인
                if (false == objMotorStatus.bPlusLimitSensor)
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.PLUS_LIMIT, iLoopMotorInformationList, Color.Empty);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.PLUS_LIMIT, iLoopMotorInformationList, string.Empty);
                }
                else
                {
                    base.SetGridViewCellBackColor(objGridView, (int)EMotorInformationListColumn.PLUS_LIMIT, iLoopMotorInformationList, m_colorOff);
                    base.SetGridViewCellData(objGridView, (int)EMotorInformationListColumn.PLUS_LIMIT, iLoopMotorInformationList, "POT");
                }
            }
        }

        /// <summary>
        /// 선택된 셀에 모터 설정값 불러옴
        /// </summary>
        private void SetMotorSettingData()
        {
            // 그리드 뷰 셀이 선택되면 선택된 모터 정보 리스트에 데이터를 버튼에 뿌려줌
            BtnJogFast.Text = string.Format("{0:F3}", m_objMotorOperationParameter.dVelocityJogFast);
            BtnJogSlow.Text = string.Format("{0:F3}", m_objMotorOperationParameter.dVelocityJogSlow);
            BtnAutoSpeed.Text = string.Format("{0:F3}", m_objMotorOperationParameter.dStandardAutoVelocity);
            BtnManualSpeed.Text = string.Format("{0:F3}", m_objMotorOperationParameter.dStandardManualVelocity);
            BtnOriginSpeed.Text = string.Format("{0:F3}", m_objMotorOperationParameter.dVelocityHomeFirst);
            BtnOriginOffset.Text = string.Format("{0:F3}", m_objMotorOperationParameter.dHomeOffset);
            BtnLimitPositionPlus.Text = string.Format("{0:F3}", m_objMotorOperationParameter.dLimitSoftwarePlus);
            BtnLimitPositionMinus.Text = string.Format("{0:F3}", m_objMotorOperationParameter.dLimitSoftwareMinus);
            BtnDelayAfterMoving.Text = string.Format("{0} ( ㎳ )", m_objMotorOperationParameter.iDelayAfterMoving);
            BtnInnerNo.Text = string.Format("{0}", m_objMotorInitializeParameter.iAxisNo);
            // 모터 사용 여부?
            BtnUseMotor.Text = "true";
            m_objMotorStatus = m_listMotorInformation[m_iSelectedRow].HLGetMotorStatus();
            // 현재 엔코더 위치 표시
            BtnCurrentPosition.Text = string.Format("{0:F3}", m_objMotorStatus.dEncoderPosition);
            // 현재 커맨드 위치 표시
            BtnCommandPosition.Text = string.Format("{0:F3}", m_objMotorStatus.dCommandPosition);
            // 기준 가속도 설정 값 표시
            BtnStandardAcceleration.Text = m_objMotorOperationParameter.dStandardAcceleration.ToString();
            // 기준 감속도 설정 값 표시
            BtnStandardDeceleration.Text = m_objMotorOperationParameter.dStandardDeceleration.ToString();
            // Tolerace 설정
            BtnStandardTolerance.Text = m_objMotorOperationParameter.dStandardTolerence.ToString();
            // HomeUse 설정
            BtnUseHome.Text = (true == m_objMotorOperationParameter.bUseHome) ? "true" : "false";
            // 타임아웃 설정
            BtnStandardTimeOut.Text = m_objMotorOperationParameter.iStandardLimitTimeOut.ToString() + "( ㎳ )";
        }

        /// <summary>
        /// 선택된 셀에 위치 정보 불러옴
        /// </summary>
        private void SetPositionInformationList(bool bSelectionToTop = true)
        {
            DataGridView objPositionGridView = GridViewPositionInformationList;
            int captureSelectRowIndex = 0;
            if (bSelectionToTop == false)
            {
                captureSelectRowIndex = objPositionGridView.SelectedRows[0].Index;
            }
            // 기존에 있는 위치 정보 클리어
            objPositionGridView.Rows.Clear();
            for (int iLoopPageCount = 0; iLoopPageCount < m_iPageCount; iLoopPageCount++)
            {
                string[] strRowData = new string[(int)EPositionInformationListColumn.POSITION_INFORMATION_LIST_COLUMN_FINAL];
                strRowData[(int)EPositionInformationListColumn.NAME] = m_objMotorPosition.strPositionName[iLoopPageCount + (m_iPageCount * m_iPositionCurrentPage)];
                strRowData[(int)EPositionInformationListColumn.POSITION] = string.Format("{0:F3}", m_objMotorPosition.dPosition[iLoopPageCount + (m_iPageCount * m_iPositionCurrentPage)]);
                strRowData[(int)EPositionInformationListColumn.SPEED] = string.Format("{0:F3}", m_objMotorPosition.dVelocity[iLoopPageCount + (m_iPageCount * m_iPositionCurrentPage)]);
                strRowData[(int)EPositionInformationListColumn.ACC] = string.Format("{0:F3}", m_objMotorPosition.dAcceleration[iLoopPageCount + (m_iPageCount * m_iPositionCurrentPage)]);
                strRowData[(int)EPositionInformationListColumn.DEC] = string.Format("{0:F3}", m_objMotorPosition.dDeceleration[iLoopPageCount + (m_iPageCount * m_iPositionCurrentPage)]);
                objPositionGridView.Rows.Add(strRowData);
            }
            // 포커스 선택
            objPositionGridView.Rows[captureSelectRowIndex].Selected = true;
        }

        /// <summary>
        /// 타이머 동작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (mBlinkTimer.Elapsed > mBlinkChangeTime)
            {
                mBlinkTimer.Reset();
                mBlinkTimer.Start();
                mbBlinkSignal = !mbBlinkSignal;
            }

            // 현재 페이지 표시
            base.SetButtonText(BtnMotorInformationPage, string.Format("PAGE ( {0} / {1} )", m_iMotorCurrentPage + 1, m_iMotorMaxPage + 1));
            base.SetButtonText(BtnPositionInformationPage, string.Format("PAGE ( {0} / {1} )", m_iPositionCurrentPage + 1, m_iPositionMaxPage + 1));
            // 선택된 모터가 없을 경우 표시하지 않음
            if (0 <= m_iSelectedRow)
            {
                if (
                    true == mbJogMoving
                    && m_listInterlock.Count != 0
                    && false == m_listInterlock[m_iSelectedRow].CheckForcedInterlock(m_listMotorInformation[m_iSelectedRow].HLGetMotorInitializeParameter().strMotorName, mbJogPositiveDirection, callbackInterlockPreAction)
                    )
                {
                    // 인터락 발생시 콜백 함수에서 정지 및 플래그 처리함
                }

                m_objMotorStatus = m_listMotorInformation[m_iSelectedRow].HLGetMotorStatus();
                // Need for Servo On
                if (false == m_objMotorStatus.bServo)
                {
                    SetButtonBackColor(BtnServoOn, (true == mbBlinkSignal) ? m_colorClick : m_colorNormal);
                }
                else
                {
                    SetButtonBackColor(BtnServoOn, m_colorNormal);
                }
                // Need for Alarm Reset
                if (true == m_objMotorStatus.bAlarm)
                {
                    SetButtonBackColor(BtnServoReset, (true == mbBlinkSignal) ? m_colorClick : m_colorNormal);
                }
                else
                {
                    SetButtonBackColor(BtnServoReset, m_colorNormal);
                }
                // Need for Origin
                if (
                    true == m_objMotorOperationParameter.bUseHome
                    && false == m_objMotorStatus.bHome
                    )
                {
                    SetButtonBackColor(BtnOrigin, (true == mbBlinkSignal) ? m_colorClick : m_colorNormal);
                }
                else
                {
                    SetButtonBackColor(BtnOrigin, m_colorNormal);
                }
                // 모터 운용 파라미터 정보 표시
                SetMotorSettingData();
            }

            // 모터 정보 그리드 뷰 상태 체크
            SetMotorInformationStatusCheck(m_listMotorInformation, m_iMotorCurrentPage);
            // 조그 속도 모드 표시
            if (EJogVelocityMode.JOG_FAST_MODE == m_eJogVelocityMode)
            {
                BtnTitleJogFast.BackColor = m_colorOn;
                BtnTitleJogSlow.BackColor = m_colorLabelSub;
            }
            else
            {
                BtnTitleJogFast.BackColor = m_colorLabelSub;
                BtnTitleJogSlow.BackColor = m_colorOn;
            }

            if (BtnRepeatMove.Visible == true)
            {
                SetButtonBackColor(BtnRepeatMove, m_objDocument.m_objProcessMain.m_objProcessMotion.IsRunningMotorRepeatMove ? m_colorClick : m_colorNormal);
            }
        }

        private void callbackInterlockPreAction()
        {
            m_listMotorInformation[m_iSelectedRow].HLMoveEStop();
            mbJogMoving = false;
            m_objDocument.m_objProcessMain.IsJogMoving = false;
        }

        /// <summary>
        /// 모터 셀 항목 클릭.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewMotorInformationList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView objGridView = sender as DataGridView;
            // 모터 셀을 클릭하면 데이터 갱신하는 역활
            do
            {
                try
                {
                    int iSelectedRow = objGridView.CurrentCell.RowIndex;
                    // 현재 행 갱신
                    m_iSelectedRow = iSelectedRow + (m_iPageCount * m_iMotorCurrentPage);
                    // 데이터 리로드
                    SetDataReLoad();
                    // 포지션 저장 로그 정보 20180711
                    m_listTeachInformation.Clear();
                    m_listTeachOptionInformation.Clear();
                }
                catch (System.ArgumentOutOfRangeException ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        /// <summary>
        /// 데이터 리로드.
        /// </summary>
        private void SetDataReLoad()
        {
            do
            {
                try
                {
                    if (0 <= m_iSelectedRow)
                    {
                        if (null == m_objMotorPosition
                            || null == m_objMotorOperationParameter
                            || null == m_objMotorInitializeParameter
                            || null == m_objMotorStatus
                            || null == m_listMotorInformation)
                        {
                            break;
                        }
                        // 선택된 모터 포지션 객체 연결
                        m_objMotorPosition = m_listMotorInformation[m_iSelectedRow].HLGetMotorPosition();
                        // 선택된 모터 운용 파라미터 객체 연결
                        m_objMotorOperationParameter = m_listMotorInformation[m_iSelectedRow].HLGetMotorOperationParameter();
                        // 선택된 모터 초기화 파라미터 객체 연결
                        m_objMotorInitializeParameter = m_listMotorInformation[m_iSelectedRow].HLGetMotorInitializeParameter();
                        // 선택된 모터 상태 정보 객체 연결
                        m_objMotorStatus = m_listMotorInformation[m_iSelectedRow].HLGetMotorStatus();
                        // 새로운 축 선택시 포지션 인덱스 0
                        m_iPositionCurrentPage = 0;
                        // 포지션 정보 갱신.
                        SetPositionInformationList();
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        /// <summary>
        /// 모터 포지션 그리드 더블클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewPositionInformationList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView objGridView = sender as DataGridView;
            // 위치 셀을 더블 클릭하면 키보드, 키패드 띄워서 데이터 받아서 다시 해당 셀에 집어넣음
            string strSelectedOption = "";
            double dPreviousValue = 0;
            double dChangeValue = 0;

            do
            {
                try
                {
                    // 설비 자동 동작중이면 스킵
                    if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                    {
                        return;
                    }
                    // Row 인덱스가 -1이면 헤더 클릭
                    if (-1 == e.RowIndex)
                    {
                        break;
                    }

                    int iRow = e.RowIndex;
                    int iColumn = e.ColumnIndex;
                    // 선택한 칼럼이 이름이면 키보드
                    if ((int)EPositionInformationListColumn.NAME == iColumn)
                    {
                        using (FormKeyBoard objKeyboard = new FormKeyBoard(m_objMotorPosition.strPositionName[iRow + (m_iPageCount * m_iPositionCurrentPage)]))
                        {
                            if (DialogResult.OK == objKeyboard.ShowDialog())
                            {
                                m_objMotorPosition.strPositionName[iRow + (m_iPageCount * m_iPositionCurrentPage)] = objKeyboard.m_strReturnValue;
                                objGridView[iColumn, iRow].Value = string.Format("{0}", objKeyboard.m_strReturnValue);
                            }
                        }
                    }
                    // 그 외 칼럼은 키패드
                    else
                    {
                        if ((int)EPositionInformationListColumn.POSITION == iColumn)
                        {
                            CDeviceMotorAbstract.CMotorOperationParameter motorParam = new CDeviceMotorAbstract.CMotorOperationParameter();
                            motorParam = m_listMotorInformation[m_iSelectedRow].HLGetMotorOperationParameter();
                            using (var objKeyPad = new FormKeyPad(double.Parse(objGridView[iColumn, iRow].Value.ToString()), motorParam.dLimitSoftwareMinus, motorParam.dLimitSoftwarePlus, m_objMotorPosition.strPositionName[iRow + (m_iPageCount * m_iPositionCurrentPage)]))
                            {
                                if (DialogResult.OK == objKeyPad.ShowDialog())
                                {
                                    if ((int)EPositionInformationListColumn.POSITION == iColumn)
                                    {
                                        strSelectedOption = EPositionInformationListColumn.POSITION.ToString();
                                        dPreviousValue = m_objMotorPosition.dPosition[iRow + (m_iPageCount * m_iPositionCurrentPage)];
                                        dChangeValue = objKeyPad.m_dResultValue;
                                        // 포지션 설정값 인터락 확인
                                        if (m_listInterlock[m_iSelectedRow].CheckMotorSettingPositionInterlock(m_listMotorInformation[m_iSelectedRow].HLGetMotorInitializeParameter().strMotorName, iRow + (m_iPageCount * m_iPositionCurrentPage), dChangeValue) == false)
                                        {
                                            break;
                                        }
                                        m_objMotorPosition.dPosition[iRow + (m_iPageCount * m_iPositionCurrentPage)] = objKeyPad.m_dResultValue;
                                    }
                                    else if ((int)EPositionInformationListColumn.SPEED == iColumn)
                                    {
                                        strSelectedOption = EPositionInformationListColumn.SPEED.ToString();
                                        dPreviousValue = m_objMotorPosition.dVelocity[iRow + (m_iPageCount * m_iPositionCurrentPage)];
                                        dChangeValue = objKeyPad.m_dResultValue;

                                        m_objMotorPosition.dVelocity[iRow + (m_iPageCount * m_iPositionCurrentPage)] = objKeyPad.m_dResultValue;
                                    }
                                    else if ((int)EPositionInformationListColumn.ACC == iColumn)
                                    {
                                        strSelectedOption = EPositionInformationListColumn.ACC.ToString();
                                        dPreviousValue = m_objMotorPosition.dAcceleration[iRow + (m_iPageCount * m_iPositionCurrentPage)];
                                        dChangeValue = objKeyPad.m_dResultValue;

                                        m_objMotorPosition.dAcceleration[iRow + (m_iPageCount * m_iPositionCurrentPage)] = objKeyPad.m_dResultValue;
                                    }
                                    else if ((int)EPositionInformationListColumn.DEC == iColumn)
                                    {
                                        strSelectedOption = EPositionInformationListColumn.DEC.ToString();
                                        dPreviousValue = m_objMotorPosition.dDeceleration[iRow + (m_iPageCount * m_iPositionCurrentPage)];
                                        dChangeValue = objKeyPad.m_dResultValue;

                                        m_objMotorPosition.dDeceleration[iRow + (m_iPageCount * m_iPositionCurrentPage)] = objKeyPad.m_dResultValue;
                                    }

                                    // 포지션 저장 로그 정보 20180711
                                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ] : {2}, [ PREV VALUE ] : {3:F3}, [ CHANGE VALUE ] : {4:F3}"
                                        , m_objMotorInitializeParameter.strMotorName
                                        , strSelectedOption
                                        , m_objMotorPosition.strPositionName[iRow + (m_iPageCount * m_iPositionCurrentPage)]
                                        , dPreviousValue
                                        , dChangeValue);
                                    m_listTeachInformation.Add(strTeachInformation);

                                    objGridView[iColumn, iRow].Value = string.Format("{0:F3}", objKeyPad.m_dResultValue);
                                }
                            }
                        }
                        else
                        {
                            using (var objKeyPad = new FormKeyPad(double.Parse(objGridView[iColumn, iRow].Value.ToString()), m_objMotorPosition.strPositionName[iRow + (m_iPageCount * m_iPositionCurrentPage)]))
                            {
                                if (DialogResult.OK == objKeyPad.ShowDialog())
                                {
                                    if ((int)EPositionInformationListColumn.POSITION == iColumn)
                                    {
                                        strSelectedOption = EPositionInformationListColumn.POSITION.ToString();
                                        dPreviousValue = m_objMotorPosition.dPosition[iRow + (m_iPageCount * m_iPositionCurrentPage)];
                                        dChangeValue = objKeyPad.m_dResultValue;
                                        // 포지션 설정값 인터락 확인
                                        if (m_listInterlock[m_iSelectedRow].CheckMotorSettingPositionInterlock(m_listMotorInformation[m_iSelectedRow].HLGetMotorInitializeParameter().strMotorName, iRow + (m_iPageCount * m_iPositionCurrentPage), dChangeValue) == false)
                                        {
                                            break;
                                        }
                                        m_objMotorPosition.dPosition[iRow + (m_iPageCount * m_iPositionCurrentPage)] = objKeyPad.m_dResultValue;
                                    }
                                    else if ((int)EPositionInformationListColumn.SPEED == iColumn)
                                    {
                                        strSelectedOption = EPositionInformationListColumn.SPEED.ToString();
                                        dPreviousValue = m_objMotorPosition.dVelocity[iRow + (m_iPageCount * m_iPositionCurrentPage)];
                                        dChangeValue = objKeyPad.m_dResultValue;

                                        m_objMotorPosition.dVelocity[iRow + (m_iPageCount * m_iPositionCurrentPage)] = objKeyPad.m_dResultValue;
                                    }
                                    else if ((int)EPositionInformationListColumn.ACC == iColumn)
                                    {
                                        strSelectedOption = EPositionInformationListColumn.ACC.ToString();
                                        dPreviousValue = m_objMotorPosition.dAcceleration[iRow + (m_iPageCount * m_iPositionCurrentPage)];
                                        dChangeValue = objKeyPad.m_dResultValue;

                                        m_objMotorPosition.dAcceleration[iRow + (m_iPageCount * m_iPositionCurrentPage)] = objKeyPad.m_dResultValue;
                                    }
                                    else if ((int)EPositionInformationListColumn.DEC == iColumn)
                                    {
                                        strSelectedOption = EPositionInformationListColumn.DEC.ToString();
                                        dPreviousValue = m_objMotorPosition.dDeceleration[iRow + (m_iPageCount * m_iPositionCurrentPage)];
                                        dChangeValue = objKeyPad.m_dResultValue;

                                        m_objMotorPosition.dDeceleration[iRow + (m_iPageCount * m_iPositionCurrentPage)] = objKeyPad.m_dResultValue;
                                    }

                                    // 포지션 저장 로그 정보 20180711
                                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ] : {2}, [ PREV VALUE ] : {3:F3}, [ CHANGE VALUE ] : {4:F3}"
                                        , m_objMotorInitializeParameter.strMotorName
                                        , strSelectedOption
                                        , m_objMotorPosition.strPositionName[iRow + (m_iPageCount * m_iPositionCurrentPage)]
                                        , dPreviousValue
                                        , dChangeValue);
                                    m_listTeachInformation.Add(strTeachInformation);

                                    objGridView[iColumn, iRow].Value = string.Format("{0:F3}", objKeyPad.m_dResultValue);
                                }
                            }
                        }
                    }
                }
                catch (System.ArgumentOutOfRangeException ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        /// <summary>
        /// 모터 항목 이전 화면 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMotorInformationPrevious_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 >= m_iMotorCurrentPage)
                {
                    break;
                }
                // 모터 정보 그리드 뷰에 페이지에 맞게 뿌려줌
                SetMotorInformationGridView(m_listMotorInformation, --m_iMotorCurrentPage);

                int iSelectedRow = 0;
                // 현재 행 갱신
                m_iSelectedRow = iSelectedRow + (m_iPageCount * m_iMotorCurrentPage);
                // 데이터 리로드
                SetDataReLoad();
                // 포지션 저장 로그 정보 20180711
                m_listTeachInformation.Clear();
                m_listTeachOptionInformation.Clear();

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Motor Current Page : {1:D}]", "BtnMotorInformationPrevious_Click", m_iMotorCurrentPage);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        /// <summary>
        /// 모터 항목 다음 화면 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMotorInformationNext_Click(object sender, EventArgs e)
        {
            do
            {
                if (m_listMotorInformation.Count <= (m_iMotorCurrentPage + 1) * m_iPageCount)
                {
                    break;
                }
                // 모터 정보 그리드 뷰에 페이지에 맞게 뿌려줌
                SetMotorInformationGridView(m_listMotorInformation, ++m_iMotorCurrentPage);

                int iSelectedRow = 0;
                // 현재 행 갱신
                m_iSelectedRow = iSelectedRow + (m_iPageCount * m_iMotorCurrentPage);
                // 데이터 리로드
                SetDataReLoad();
                // 포지션 저장 로그 정보 20180711
                m_listTeachInformation.Clear();
                m_listTeachOptionInformation.Clear();

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Motor Current Page : {1:D}]", "BtnMotorInformationNext_Click", m_iMotorCurrentPage);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        /// <summary>
        /// 모터 포지션 항목 이전 화면 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPositionInformationPrevious_Click(object sender, EventArgs e)
        {
            do
            {
                // 모터 포지션 페이지 인덱스 감소
                if (0 >= m_iPositionCurrentPage)
                {
                    break;
                }
                m_iPositionCurrentPage--;
                SetPositionInformationList();
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Position Current Page : {1:D}]", "BtnPositionInformationPrevious_Click", m_iPositionCurrentPage);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        /// <summary>
        /// 모터 포지션 다음 화면 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPositionInformationNext_Click(object sender, EventArgs e)
        {
            do
            {
                // 모터 포지션 페이지 인덱스 증가
                if (m_iPositionMaxPage <= m_iPositionCurrentPage)
                {
                    break;
                }
                m_iPositionCurrentPage++;
                SetPositionInformationList();
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Position Current Page : {1:D}]", "BtnPositionInformationNext_Click", m_iPositionCurrentPage);
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        /// <summary>
        /// 모터 서보 ON
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnServoOn_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 > m_iSelectedRow)
                {
                    break;
                }
                m_listMotorInformation[m_iSelectedRow].HLServoOn(CDeviceMotorAbstract.EUse.ENABLE);
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Servo : {1}]", "BtnServoOn_Click", CDeviceMotorAbstract.EUse.ENABLE.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        /// <summary>
        /// 모터 서보 OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnServoOff_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 > m_iSelectedRow)
                {
                    break;
                }
                m_listMotorInformation[m_iSelectedRow].HLServoOn(CDeviceMotorAbstract.EUse.DISABLE);
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Servo : {1}]", "BtnServoOff_Click", CDeviceMotorAbstract.EUse.DISABLE.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        /// <summary>
        /// 모터 서보 알람 리셋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnServoReset_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 > m_iSelectedRow)
                {
                    break;
                }
                m_listMotorInformation[m_iSelectedRow].HLAlarmReset();
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnServoReset_Click", "Servo Alarm Reset");
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        /// <summary>
        /// 모터 원점 동작 수행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOrigin_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 > m_iSelectedRow)
                {
                    break;
                }

                // 도어 잠김 확인
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

                if (0 > m_iSelectedRow)
                {
                    break;
                }

                int selectItem = m_iSelectedRow;
                // 인터락 체크
                if (m_listInterlock.Count == 0)
                    break;

                if (false == m_listInterlock[selectItem].CheckMotionClassInterlock(m_listMotorInformation[selectItem].HLGetMotorInitializeParameter().strMotorName, CDefine.MANUAL_ORIGIN_POSITION_NO))
                {
                    break;
                }

                if (
                    true == m_objMotorOperationParameter.bUseHome
                    && mTaskOriginEndWait[selectItem] == null
                    )
                {
                    m_listMotorInformation[selectItem].HLSetSoftwareLimit(CDeviceMotorAbstract.EUse.ENABLE);
                    if (m_listMotorInformation[selectItem].HLSetHomeProcess() == false)
                    {
                        // Msg: 홈 프로세스 실패 입니다.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_IS_HOME_PROCESS_FAIL, m_listMotorInformation[selectItem].HLGetMotorInitializeParameter().strMotorName);
                        break;
                    }

                    mTaskOriginEndWait[selectItem] = new Task(() =>
                    {
                        mHomeMainSteps[selectItem] = 0;
                        mHomeSteps[selectItem] = 0;
                        bool bCurrentResult = m_listMotorInformation[selectItem].HLGetMotorStatus().bHome;
                        m_listMotorInformation[selectItem].HLGetHomeProcessRate(ref mHomeMainSteps[selectItem], ref mHomeSteps[selectItem]);
                        int iTimeOut = m_listMotorInformation[selectItem].HLGetMotorOperationParameter().iStandardLimitTimeOut;
                        while (100 != mHomeSteps[selectItem])
                        {
                            if (iTimeOut < 0)
                            {
                                // Msg: 홈 프로세스 타임아웃 입니다.
                                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_IS_HOME_PROCESS_TIMEOUT, m_listMotorInformation[selectItem].HLGetMotorInitializeParameter().strMotorName);
                                break;
                            }
                            Thread.Sleep(10);
                            iTimeOut -= 10;
                            bCurrentResult = m_listMotorInformation[selectItem].HLGetMotorStatus().bHome;
                            m_listMotorInformation[selectItem].HLGetHomeProcessRate(ref mHomeMainSteps[selectItem], ref mHomeSteps[selectItem]);
                        }
                        mTaskOriginEndWait[selectItem] = null;
                    });

                    // 시작
                    mTaskOriginEndWait[selectItem].Start();

                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [{1}]", "BtnOrigin_Click", "Origin Process");
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
            } while (false);
        }

        /// <summary>
        /// 상대 위치 입력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRelativePosition_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnRelativePosition_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);
            m_objMotorStatus = m_listMotorInformation[m_iSelectedRow].HLGetMotorStatus();
            string strTitle = string.Format("Relative Move Value, Current Motor Position = {0}", m_objMotorStatus.dEncoderPosition);
            using (FormKeyPad objKeyPad = new FormKeyPad(Convert.ToDouble(BtnRelativePosition.Text), m_objMotorOperationParameter.dLimitSoftwareMinus, m_objMotorOperationParameter.dLimitSoftwarePlus, strTitle))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    double dResultPos = objKeyPad.m_dResultValue + m_objMotorStatus.dEncoderPosition;
                    if (dResultPos > m_objMotorOperationParameter.dLimitSoftwarePlus
                        || dResultPos < m_objMotorOperationParameter.dLimitSoftwareMinus)
                    {
                        MessageBox.Show(string.Format("{0} is Out OF Range MIN = {1}, MAX = {2}", dResultPos, m_objMotorOperationParameter.dLimitSoftwareMinus, m_objMotorOperationParameter.dLimitSoftwarePlus));
                    }
                    else
                    {
                        m_dRelativeMoveValue = objKeyPad.m_dResultValue;
                        BtnRelativePosition.Text = m_dRelativeMoveValue.ToString();
                    }
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [RelativeMoveValue : {1:F3}] [{2}]", "BtnRelativePosition_Click", m_dRelativeMoveValue, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 상대 위치 이동 ( + 방향 )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRelativeMovePlus_Click(object sender, EventArgs e)
        {
            do
            {
                // 도어 잠김 확인
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

                if (0 > m_iSelectedRow)
                {
                    break;
                }

                // 인터락 체크
                if (
                    0 == m_listInterlock.Count
                    || false == m_listInterlock[m_iSelectedRow].CheckMotionClassInterlock(m_listMotorInformation[m_iSelectedRow].HLGetMotorInitializeParameter().strMotorName, m_objMotorStatus.dEncoderPosition + m_dRelativeMoveValue)
                    )
                {
                    break;
                }

                //if (false == m_listMotorInformation[m_iSelectedRow].HLMoveAbsolutePosition(m_objMotorStatus.dEncoderPosition + m_dRelativeMoveValue))
                if (false == m_listMotorInformation[m_iSelectedRow].HLMoveRelativePosition(m_dRelativeMoveValue, CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_MANUAL_RUN))
                {
                    // 알람
                    m_listMotorInformation[m_iSelectedRow].HLGetMotorError();
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [{1}]", "BtnRelativeMovePlus_Click", "Motor Error");
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                    break;
                }
                else
                {
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Move Relative Position : {1:F3}]", "BtnRelativeMovePlus_Click", m_dRelativeMoveValue);
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }

            } while (false);
        }

        /// <summary>
        /// 상대 위치 이동 ( - 방향 )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRelativeMoveMinus_Click(object sender, EventArgs e)
        {
            do
            {
                // 도어 잠김 확인
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

                if (0 > m_iSelectedRow)
                {
                    break;
                }

                // 인터락 체크
                if (
                    0 == m_listInterlock.Count
                    || false == m_listInterlock[m_iSelectedRow].CheckMotionClassInterlock(m_listMotorInformation[m_iSelectedRow].HLGetMotorInitializeParameter().strMotorName, m_objMotorStatus.dEncoderPosition - m_dRelativeMoveValue)
                    )
                {
                    break;
                }

                //if (false == m_listMotorInformation[m_iSelectedRow].HLMoveAbsolutePosition(m_objMotorStatus.dEncoderPosition - m_dRelativeMoveValue))
                if (false == m_listMotorInformation[m_iSelectedRow].HLMoveRelativePosition(m_dRelativeMoveValue * -1, CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_MANUAL_RUN))
                {
                    // 알람
                    m_listMotorInformation[m_iSelectedRow].HLGetMotorError();
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [{1}]", "BtnRelativeMoveMinus_Click", "Motor Error");
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                    break;
                }
                else
                {
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Move Relative Position : {1:F3}]", "BtnRelativeMoveMinus_Click", m_dRelativeMoveValue);
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
            } while (false);
        }

        /// <summary>
        /// 빠른 조그 속도 모드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTitleJogFast_Click(object sender, EventArgs e)
        {
            m_eJogVelocityMode = EJogVelocityMode.JOG_FAST_MODE;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [JogVelocityMode : {1}]", "BtnTitleJogFast_Click", m_eJogVelocityMode.ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 느린 조그 속도 모드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTitleJogSlow_Click(object sender, EventArgs e)
        {
            m_eJogVelocityMode = EJogVelocityMode.JOG_SLOW_MODE;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [JogVelocityMode : {1}]", "BtnTitleJogSlow_Click", m_eJogVelocityMode.ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 빠른 조그 속도 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnJogFast_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnJogFast_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(Convert.ToDouble(BtnJogFast.Text), 1, 100))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleJogFast.Text
                    , m_objMotorOperationParameter.dVelocityJogFast
                    , objKeyPad.m_dResultValue);
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.dVelocityJogFast = objKeyPad.m_dResultValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [VelocityJogFast : {1:F3}] [{2}]", "BtnJogFast_Click", m_objMotorOperationParameter.dVelocityJogFast, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 느린 조그 속도 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnJogSlow_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnJogSlow_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(Convert.ToDouble(BtnJogSlow.Text), 1, 100))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleJogSlow.Text
                    , m_objMotorOperationParameter.dVelocityJogSlow
                    , objKeyPad.m_dResultValue);
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.dVelocityJogSlow = objKeyPad.m_dResultValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [VelocityJogSlow : {1:F3}] [{2}]", "BtnJogSlow_Click", m_objMotorOperationParameter.dVelocityJogSlow, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 조그 이동 ( + 방향 )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnJogMovePlus_MouseDown(object sender, MouseEventArgs e)
        {
            do
            {
                if (0 > m_iSelectedRow)
                {
                    break;
                }

                if (m_listInterlock.Count == 0)
                {
                    break;
                }

                mbJogPositiveDirection = true;

                // Jog로 움직일 때 Fast Mode의 경우 Forced Interlock 체크(움직이려는 모터축과 관계된 모든 방해물 제거)
                if (false == m_listInterlock[m_iSelectedRow].CheckForcedInterlock(m_listMotorInformation[m_iSelectedRow].HLGetMotorInitializeParameter().strMotorName, mbJogPositiveDirection, null))
                {
                    break;
                }

                if (EJogVelocityMode.JOG_FAST_MODE == m_eJogVelocityMode)
                {

                    m_listMotorInformation[m_iSelectedRow].HLJogFastMove(CDeviceMotorAbstract.EMoveDirection.DIRECTION_CW);
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Jog Fast Move : {1}]", "BtnJogMovePlus_MouseDown", CDeviceMotorAbstract.EMoveDirection.DIRECTION_CW.ToString());
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
                else
                {
                    m_listMotorInformation[m_iSelectedRow].HLJogSlowMove(CDeviceMotorAbstract.EMoveDirection.DIRECTION_CW);
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Jog Slow Move : {1}]", "BtnJogMovePlus_MouseDown", CDeviceMotorAbstract.EMoveDirection.DIRECTION_CW.ToString());
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }

                mbJogMoving = true;
                m_objDocument.m_objProcessMain.IsJogMoving = true;
            } while (false);
        }

        /// <summary>
        /// 조그 이동 정지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnJogMovePlus_MouseUp(object sender, MouseEventArgs e)
        {
            do
            {
                if (0 > m_iSelectedRow)
                {
                    break;
                }
                m_listMotorInformation[m_iSelectedRow].HLMoveStop();
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnJogMovePlus_MouseUp", "Move Stop");
                m_objDocument.SetUpdateButtonLog(this, strLog);

                mbJogMoving = false;
                m_objDocument.m_objProcessMain.IsJogMoving = false;
            } while (false);
        }

        /// <summary>
        /// 조그 이동 ( - 방향 )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnJogMoveMinus_MouseDown(object sender, MouseEventArgs e)
        {
            do
            {
                if (0 > m_iSelectedRow)
                {
                    break;
                }

                if (m_listInterlock.Count == 0)
                {
                    break;
                }

                mbJogPositiveDirection = false;

                // Jog로 움직일 때 Fast Mode의 경우 Forced Interlock 체크(움직이려는 모터축과 관계된 모든 방해물 제거)
                if (false == m_listInterlock[m_iSelectedRow].CheckForcedInterlock(m_listMotorInformation[m_iSelectedRow].HLGetMotorInitializeParameter().strMotorName, mbJogPositiveDirection, null))
                {
                    break;
                }

                if (EJogVelocityMode.JOG_FAST_MODE == m_eJogVelocityMode)
                {

                    m_listMotorInformation[m_iSelectedRow].HLJogFastMove(CDeviceMotorAbstract.EMoveDirection.DIRECTION_CCW);
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Jog Fast Move : {1}]", "BtnJogMoveMinus_MouseDown", CDeviceMotorAbstract.EMoveDirection.DIRECTION_CCW.ToString());
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
                else
                {
                    m_listMotorInformation[m_iSelectedRow].HLJogSlowMove(CDeviceMotorAbstract.EMoveDirection.DIRECTION_CCW);
                    // 버튼 로그 추가
                    string strLog = string.Format("[{0}] [Jog Slow Move : {1}]", "BtnJogMoveMinus_MouseDown", CDeviceMotorAbstract.EMoveDirection.DIRECTION_CCW.ToString());
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }

                mbJogMoving = true;
                m_objDocument.m_objProcessMain.IsJogMoving = true;
            } while (false);
        }

        /// <summary>
        /// 조그 이동 정지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnJogMoveMinus_MouseUp(object sender, MouseEventArgs e)
        {
            do
            {
                if (0 > m_iSelectedRow)
                {
                    break;
                }
                m_listMotorInformation[m_iSelectedRow].HLMoveStop();
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnJogMoveMinus_MouseUp", "Move Stop");
                m_objDocument.SetUpdateButtonLog(this, strLog);

                mbJogMoving = false;
                m_objDocument.m_objProcessMain.IsJogMoving = false;
            } while (false);
        }

        /// <summary>
        /// 기준 자동 모드 속도
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAutoSpeed_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnAutoSpeed_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(Convert.ToDouble(BtnAutoSpeed.Text)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleAutoSpeed.Text
                    , m_objMotorOperationParameter.dStandardAutoVelocity
                    , objKeyPad.m_dResultValue);
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.dStandardAutoVelocity = objKeyPad.m_dResultValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [StandardAutoVelocity : {1:F3}] [{2}]", "BtnAutoSpeed_Click", m_objMotorOperationParameter.dStandardAutoVelocity, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 기준 메뉴얼 모드 속도
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnManualSpeed_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnManualSpeed_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(Convert.ToDouble(BtnManualSpeed.Text)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleManualSpeed.Text
                    , m_objMotorOperationParameter.dStandardManualVelocity
                    , objKeyPad.m_dResultValue);
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.dStandardManualVelocity = objKeyPad.m_dResultValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [StandardManualVelocity : {1:F3}] [{2}]", "BtnManualSpeed_Click", m_objMotorOperationParameter.dStandardManualVelocity, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 기준 가속도 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStandardAcceleration_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnStandardAcceleration_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(Convert.ToDouble(BtnStandardAcceleration.Text)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleStandardAcceleration.Text
                    , m_objMotorOperationParameter.dStandardAcceleration
                    , objKeyPad.m_dResultValue);
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.dStandardAcceleration = objKeyPad.m_dResultValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [StandardAcceleration : {1:F3}] [{2}]", "BtnStandardAcceleration_Click", m_objMotorOperationParameter.dStandardAcceleration, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 기준 감속도 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStandardDeceleration_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnStandardDeceleration_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(Convert.ToDouble(BtnStandardDeceleration.Text)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleStandardDeceleration.Text
                    , m_objMotorOperationParameter.dStandardDeceleration
                    , objKeyPad.m_dResultValue);
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.dStandardDeceleration = objKeyPad.m_dResultValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [StandardDeceleration : {1:F3}] [{2}]", "BtnStandardDeceleration_Click", m_objMotorOperationParameter.dStandardDeceleration, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 모터 이동 표준 허용편차 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStandardTolerance_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnStandardTolerance_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(Convert.ToDouble(BtnStandardTolerance.Text)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleStandardTolerance.Text
                    , m_objMotorOperationParameter.dStandardTolerence
                    , objKeyPad.m_dResultValue);
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.dStandardTolerence = objKeyPad.m_dResultValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [StandardTolerence : {1:F3}] [{2}]", "BtnStandardTolerance_Click", m_objMotorOperationParameter.dStandardTolerence, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 모터 이동 표준 타임아웃 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStandardTimeOut_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnStandardTimeOut_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(0))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleStandardTimeOut.Text
                    , (double)m_objMotorOperationParameter.iStandardLimitTimeOut
                    , objKeyPad.m_dResultValue);
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.iStandardLimitTimeOut = (int)objKeyPad.m_dResultValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [StandardTimeOut : {1:D}] [{2}]", "BtnStandardTimeOut_Click", m_objMotorOperationParameter.iStandardLimitTimeOut, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 1차 홈 속도 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOriginSpeed_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnOriginSpeed_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(0))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleOriginSpeed.Text
                    , m_objMotorOperationParameter.dVelocityHomeFirst
                    , objKeyPad.m_dResultValue);
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.dVelocityHomeFirst = (int)objKeyPad.m_dResultValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [OriginSpeed : {1:F3}] [{2}]", "BtnOriginSpeed_Click", m_objMotorOperationParameter.dVelocityHomeFirst, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnOriginOffset_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnOriginOffset_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(0))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleOriginOffset.Text
                    , m_objMotorOperationParameter.dHomeOffset
                    , objKeyPad.m_dResultValue);
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.dHomeOffset = objKeyPad.m_dResultValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [OriginSpeed : {1:F3}] [{2}]", "BtnOriginOffset_Click", m_objMotorOperationParameter.dHomeOffset, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// + 소프트 웨어 리미트 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLimitPositionPlus_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnLimitPositionPlus_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(Convert.ToDouble(BtnLimitPositionPlus.Text)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleLimitPositionPlus.Text
                    , m_objMotorOperationParameter.dLimitSoftwarePlus
                    , objKeyPad.m_dResultValue);
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.dLimitSoftwarePlus = objKeyPad.m_dResultValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [LimitSoftwarePlus : {1:F3}] [{2}]", "BtnLimitPositionPlus_Click", m_objMotorOperationParameter.dLimitSoftwarePlus, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// - 소프트 웨어 리미트 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLimitPositionMinus_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnLimitPositionMinus_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(Convert.ToDouble(BtnLimitPositionMinus.Text)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2:F3}, [ CHANGE VALUE ] : {3:F3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleLimitPositionMinus.Text
                    , m_objMotorOperationParameter.dLimitSoftwareMinus
                    , objKeyPad.m_dResultValue);
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.dLimitSoftwareMinus = objKeyPad.m_dResultValue;
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [LimitSoftwareMinus : {1:F3}] [{2}]", "BtnLimitPositionMinus_Click", m_objMotorOperationParameter.dLimitSoftwareMinus, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 이동 완료 후 안정화 시간
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelayAfterMoving_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnDelayAfterMoving_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(m_objMotorOperationParameter.iDelayAfterMoving))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    // 포지션 저장 로그 정보 20180711
                    string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ], [ PREV VALUE ] : {2}, [ CHANGE VALUE ] : {3}"
                    , m_objMotorInitializeParameter.strMotorName
                    , BtnTitleDelayAfterMoving.Text
                    , m_objMotorOperationParameter.iDelayAfterMoving
                    , Convert.ToInt32(objKeyPad.m_dResultValue));
                    m_listTeachOptionInformation.Add(strTeachInformation);

                    m_objMotorOperationParameter.iDelayAfterMoving = Convert.ToInt32(objKeyPad.m_dResultValue);
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [LimitSoftwareMinus : {1:F3}] [{2}]", "BtnDelayAfterMoving_Click", m_objMotorOperationParameter.dLimitSoftwareMinus, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 홈 동작 사용 유무 ( ABS 타입에서 는 홈 사용 하지 않음 )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUseHome_Click(object sender, EventArgs e)
        {
            m_objMotorOperationParameter.bUseHome = !m_objMotorOperationParameter.bUseHome;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [bUseHome : {1}]", "BtnUseHome_Click", m_objMotorOperationParameter.bUseHome);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 모터 포지션 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPositionSave_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 > m_iSelectedRow)
                {
                    break;
                }

                // Input Value Validation
                //if ((int)CProcessMotion.EMotor.INSP_STAGE_Y == m_iSelectedRow)
                //{
                //    if (false == CCIMDefine.RECIPE_PARAMS[CCIMDefine.EPpidParamList.INSPECTION_START_POSITION_1ST].CheckInRange(m_objMotorPosition.dPosition[(int)InspStageMotorY.EPosition.InspTriggerStart1st].ToString()))
                //    {
                //        MessageBox.Show(string.Format("INSPECTION START POSITION IS RANGE OVER.\n({0})", CCIMDefine.RECIPE_PARAMS[CCIMDefine.EPpidParamList.INSPECTION_START_POSITION_1ST].GetRangeString()), "INPUT VALUE VALIDATION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }
                //    if (false == CCIMDefine.RECIPE_PARAMS[CCIMDefine.EPpidParamList.INSPECTION_END_POSITION_1ST].CheckInRange(m_objMotorPosition.dPosition[(int)InspStageMotorY.EPosition.InspTriggerEnd1st].ToString()))
                //    {
                //        MessageBox.Show(string.Format("INSPECTION END POSITION IS RANGE OVER.\n({0})", CCIMDefine.RECIPE_PARAMS[CCIMDefine.EPpidParamList.INSPECTION_END_POSITION_1ST].GetRangeString()), "INPUT VALUE VALIDATION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }

                //    if (false == CCIMDefine.RECIPE_PARAMS[CCIMDefine.EPpidParamList.INSPECTION_START_POSITION_2ND].CheckInRange(m_objMotorPosition.dPosition[(int)InspStageMotorY.EPosition.InspTriggerStart2nd].ToString()))
                //    {
                //        MessageBox.Show(string.Format("INSPECTION START POSITION IS RANGE OVER.\n({0})", CCIMDefine.RECIPE_PARAMS[CCIMDefine.EPpidParamList.INSPECTION_START_POSITION_2ND].GetRangeString()), "INPUT VALUE VALIDATION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }
                //    if (false == CCIMDefine.RECIPE_PARAMS[CCIMDefine.EPpidParamList.INSPECTION_END_POSITION_2ND].CheckInRange(m_objMotorPosition.dPosition[(int)InspStageMotorY.EPosition.InspTriggerEnd2nd].ToString()))
                //    {
                //        MessageBox.Show(string.Format("INSPECTION END POSITION IS RANGE OVER.\n({0})", CCIMDefine.RECIPE_PARAMS[CCIMDefine.EPpidParamList.INSPECTION_END_POSITION_2ND].GetRangeString()), "INPUT VALUE VALIDATION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }
                //}

                if (System.Windows.Forms.DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE))
                    break;

                m_listMotorInformation[m_iSelectedRow].HLSaveMotorPositionParameter("", "", m_objMotorPosition);
                //kjpark 20181130 PPID BODY 값 변경시 CEID 108 으로 보고됨                              
                m_objDocument.m_objRecipe.SetSaveRecipeAndReportCim(null);
                // 버튼 로그 추가
                string strLog = string.Format("[{0}]", "BtnPositionSave_Click");
                m_objDocument.SetUpdateButtonLog(this, strLog);

                // 티칭 정보 로그 업데이트 20180711
                for (int iLoopCount = 0; iLoopCount < m_listTeachInformation.Count; iLoopCount++)
                {
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_TEACH_DATA, m_listTeachInformation[iLoopCount]);
                }
                m_listTeachInformation.Clear();

                // Msg: 저장이 완료되었습니다.
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.SAVING_IS_COMPLETE);
            } while (false);
        }

        /// <summary>
        /// 모터 운용 파라미터 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE))
                return;

            do
            {
                if (0 > m_iSelectedRow)
                {
                    break;
                }
                m_objDocument.m_objConfig.SaveBackupDir();

                m_listMotorInformation[m_iSelectedRow].HLSaveMotorOperationParameter("", "", m_objMotorOperationParameter);

                // 소프트웨어 리미트 설정 하지 않는다.
                m_objDocument.m_objProcessMain.m_objProcessMotion.SetSoftwareLimit();

                // 버튼 로그 추가
                string strLog = string.Format("[{0}]", "BtnSave_Click");
                m_objDocument.SetUpdateButtonLog(this, strLog);

                // 티칭 정보 로그 업데이트 20180711
                for (int iLoopCount = 0; iLoopCount < m_listTeachOptionInformation.Count; iLoopCount++)
                {
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_TEACH_DATA, m_listTeachOptionInformation[iLoopCount]);
                }

                m_listTeachOptionInformation.Clear();

                // Msg: 저장이 완료되었습니다.
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.SAVING_IS_COMPLETE);
            } while (false);
        }

        /// <summary>
        /// 모터 정지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            do
            {
                if (0 > m_iSelectedRow)
                {
                    break;
                }
                m_listMotorInformation[m_iSelectedRow].HLMoveStop();
                // 버튼 로그 추가
                string strLog = string.Format("[{0}]", "BtnStop_Click");
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        /// <summary>
        /// 포지션 이동.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPositionMove_Click(object sender, EventArgs e)
        {
            DataGridView objGridView = GridViewPositionInformationList;
            do
            {
                try
                {
                    // 도어 잠김 확인
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

                    if (null == objGridView.CurrentCell)
                    {
                        break;
                    }
                    int iRow = objGridView.CurrentCell.RowIndex;
                    // 선택한 셀이 없을 경우 
                    if (0 > iRow || 0 > m_iSelectedRow)
                    {
                        break;
                    }

                    if (m_listInterlock.Count == 0)
                        break;

                    double targetPos = m_listMotorInformation[m_iSelectedRow].HLGetMotorPosition().dPosition[iRow + (m_iPageCount * m_iPositionCurrentPage)];
                    if (false == m_listInterlock[m_iSelectedRow].CheckMotionClassInterlock(m_listMotorInformation[m_iSelectedRow].HLGetMotorInitializeParameter().strMotorName, targetPos))
                    {
                        break;
                    }
                    if (false == m_listMotorInformation[m_iSelectedRow].HLMoveAbsoluteIndex(iRow + (m_iPageCount * m_iPositionCurrentPage), CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_MANUAL_RUN))
                    {
                        m_listMotorInformation[m_iSelectedRow].HLGetMotorError();
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [{1}]", "BtnPositionMove_Click", "Motor Error");
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                        break;
                    }
                }
                catch (System.ArgumentOutOfRangeException ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        /// <summary>
        /// 포지션 일괄 적용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewPositionInformationList_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView objGridView = sender as DataGridView;
            // 위치 셀을 더블 클릭하면 키보드, 키패드 띄워서 데이터 받아서 다시 해당 셀에 집어넣음
            string strSelectedOption = "";
            double dPreviousValue = 0;
            double dChangeValue = 0;

            do
            {
                try
                {
                    // 설비 자동 동작중이면 스킵
                    if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                    {
                        return;
                    }
                    // Row 인덱스가 -1이 아니면 셀 선택
                    if (-1 != e.RowIndex)
                    {
                        break;
                    }

                    int iColumn = e.ColumnIndex;
                    // 선택한 칼럼이 이름이면 키보드
                    if ((int)EPositionInformationListColumn.NAME == iColumn)
                    {
                    }
                    // 그 외 칼럼은 키패드
                    else
                    {
                        using (FormKeyPad objKeyPad = new FormKeyPad(0.0))
                        {
                            if (DialogResult.OK == objKeyPad.ShowDialog())
                            {
                                if ((int)EPositionInformationListColumn.POSITION == iColumn)
                                {
                                    strSelectedOption = EPositionInformationListColumn.POSITION.ToString();
                                    for (int iLoopPosition = 0; iLoopPosition < m_objMotorPosition.dPosition.Length; iLoopPosition++)
                                    {
                                        // 포지션 이름 있는 거만 일괄 적용
                                        if ("" != m_objMotorPosition.strPositionName[iLoopPosition])
                                        {
                                            dPreviousValue = m_objMotorPosition.dPosition[iLoopPosition];
                                        }
                                    }

                                    dChangeValue = objKeyPad.m_dResultValue;

                                    // 포지션 설정값 인터락 확인
                                    for (int iLoopPosition = 0; iLoopPosition < m_objMotorPosition.dPosition.Length; iLoopPosition++)
                                    {
                                        if (m_listInterlock[m_iSelectedRow].CheckMotorSettingPositionInterlock(m_listMotorInformation[m_iSelectedRow].HLGetMotorInitializeParameter().strMotorName, iLoopPosition, dChangeValue) == false)
                                        {
                                            // 실패한 위치로 포커스 이동
                                            GridViewPositionInformationList.Rows[iLoopPosition].Selected = true;
                                            return;
                                        }
                                    }

                                    for (int iLoopPosition = 0; iLoopPosition < m_objMotorPosition.dPosition.Length; iLoopPosition++)
                                    {
                                        // 포지션 이름 있는 거만 일괄 적용
                                        if ("" != m_objMotorPosition.strPositionName[iLoopPosition])
                                        {
                                            m_objMotorPosition.dPosition[iLoopPosition] = objKeyPad.m_dResultValue;
                                        }
                                    }
                                }
                                else if ((int)EPositionInformationListColumn.SPEED == iColumn)
                                {
                                    strSelectedOption = EPositionInformationListColumn.SPEED.ToString();
                                    for (int iLoopPosition = 0; iLoopPosition < m_objMotorPosition.dPosition.Length; iLoopPosition++)
                                    {
                                        // 포지션 이름 있는 거만 일괄 적용
                                        if ("" != m_objMotorPosition.strPositionName[iLoopPosition])
                                        {
                                            dPreviousValue = m_objMotorPosition.dVelocity[iLoopPosition];
                                        }
                                    }
                                    dChangeValue = objKeyPad.m_dResultValue;

                                    for (int iLoopPosition = 0; iLoopPosition < m_objMotorPosition.dPosition.Length; iLoopPosition++)
                                    {
                                        // 포지션 이름 있는 거만 일괄 적용
                                        if ("" != m_objMotorPosition.strPositionName[iLoopPosition])
                                        {
                                            m_objMotorPosition.dVelocity[iLoopPosition] = objKeyPad.m_dResultValue;
                                        }
                                    }
                                }
                                else if ((int)EPositionInformationListColumn.ACC == iColumn)
                                {
                                    strSelectedOption = EPositionInformationListColumn.ACC.ToString();
                                    for (int iLoopPosition = 0; iLoopPosition < m_objMotorPosition.dPosition.Length; iLoopPosition++)
                                    {
                                        // 포지션 이름 있는 거만 일괄 적용
                                        if ("" != m_objMotorPosition.strPositionName[iLoopPosition])
                                        {
                                            dPreviousValue = m_objMotorPosition.dAcceleration[iLoopPosition];
                                        }
                                    }
                                    dChangeValue = objKeyPad.m_dResultValue;

                                    for (int iLoopPosition = 0; iLoopPosition < m_objMotorPosition.dPosition.Length; iLoopPosition++)
                                    {
                                        // 포지션 이름 있는 거만 일괄 적용
                                        if ("" != m_objMotorPosition.strPositionName[iLoopPosition])
                                        {
                                            m_objMotorPosition.dAcceleration[iLoopPosition] = objKeyPad.m_dResultValue;
                                        }
                                    }
                                }
                                else if ((int)EPositionInformationListColumn.DEC == iColumn)
                                {
                                    strSelectedOption = EPositionInformationListColumn.DEC.ToString();
                                    for (int iLoopPosition = 0; iLoopPosition < m_objMotorPosition.dPosition.Length; iLoopPosition++)
                                    {
                                        // 포지션 이름 있는 거만 일괄 적용
                                        if ("" != m_objMotorPosition.strPositionName[iLoopPosition])
                                        {
                                            dPreviousValue = m_objMotorPosition.dDeceleration[iLoopPosition];
                                        }
                                    }
                                    dChangeValue = objKeyPad.m_dResultValue;

                                    for (int iLoopPosition = 0; iLoopPosition < m_objMotorPosition.dPosition.Length; iLoopPosition++)
                                    {
                                        // 포지션 이름 있는 거만 일괄 적용
                                        if ("" != m_objMotorPosition.strPositionName[iLoopPosition])
                                        {
                                            m_objMotorPosition.dDeceleration[iLoopPosition] = objKeyPad.m_dResultValue;
                                        }
                                    }
                                }

                                for (int iLoopPosition = 0; iLoopPosition < m_objMotorPosition.dPosition.Length; iLoopPosition++)
                                {
                                    // 포지션 이름 있는 거만 일괄 적용
                                    if ("" != m_objMotorPosition.strPositionName[iLoopPosition])
                                    {
                                        // 포지션 저장 로그 정보 20180711
                                        string strTeachInformation = string.Format("[ MOTOR ]: {0}, [ {1} ] : {2}, [ PREV VALUE ] : {3:F3}, [ CHANGE VALUE ] : {4:F3}"
                                            , m_objMotorInitializeParameter.strMotorName
                                            , strSelectedOption
                                            , m_objMotorPosition.strPositionName[iLoopPosition]
                                            , dPreviousValue
                                            , dChangeValue);
                                        m_listTeachInformation.Add(strTeachInformation);

                                        // 현재 디스플레이 중인 페이지만 업데이트
                                        int minIndex = m_iPageCount * m_iPositionCurrentPage;
                                        if (iLoopPosition >= minIndex
                                            && iLoopPosition < (minIndex + objGridView.Rows.Count)
                                            )
                                        {
                                            objGridView[iColumn, iLoopPosition - minIndex].Value = string.Format("{0:F3}", objKeyPad.m_dResultValue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.ArgumentOutOfRangeException ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        private void BtnInnerNo_Click(object sender, EventArgs e)
        {
            DataGridView objGridView = GridViewPositionInformationList;
            // 설비 자동 동작중이면 스킵
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                return;
            }

            int iRow = objGridView.CurrentCell.RowIndex;
            // 선택한 셀이 없을 경우 
            if (0 > iRow || 0 > m_iSelectedRow)
            {
                return;
            }

            uint warningCode = 0u;
            if (m_listMotorInformation[m_iSelectedRow].HLGetStatusA5nWarningCode(ref warningCode) == true)
            {
                MessageBox.Show($"WarningCode : {warningCode}");
            }
        }

        private void CFormSetupMotion_KeyUp(object sender, KeyEventArgs e)
        {
            //
        }

        private void CFormSetupMotion_KeyDown(object sender, KeyEventArgs e)
        {
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                return;
            }
            int selectItemIndex = m_iPageCount * m_iPositionCurrentPage + GridViewPositionInformationList.SelectedRows[0].Index;
            if (
                e.Control == true
                && e.KeyCode == Keys.C
                )
            {
                using (var memoryStream = new MemoryStream())
                using (var writer = new BinaryWriter(memoryStream))
                {
                    writer.Write(GUID.ToByteArray());
                    writer.Write(m_objMotorPosition.strPositionName[selectItemIndex]);
                    writer.Write(m_objMotorPosition.dPosition[selectItemIndex]);
                    writer.Write(m_objMotorPosition.dPrevPosition[selectItemIndex]);
                    writer.Write(m_objMotorPosition.dVelocity[selectItemIndex]);
                    writer.Write(m_objMotorPosition.dAcceleration[selectItemIndex]);
                    writer.Write(m_objMotorPosition.dDeceleration[selectItemIndex]);
                    writer.Write(m_objMotorPosition.dTolerence[selectItemIndex]);
                    Clipboard.SetDataObject(memoryStream.ToArray());
                    m_objDocument.SetUpdateButtonLog(this, $"Copy to clipboard.\t[strPositionName:{m_objMotorPosition.strPositionName[selectItemIndex]}]");
                }
            }
            else if (
               e.Control == true
               && e.KeyCode == Keys.X
               )
            {
                using (var memoryStream = new MemoryStream())
                using (var writer = new BinaryWriter(memoryStream))
                {
                    writer.Write(GUID.ToByteArray());
                    writer.Write(m_objMotorPosition.strPositionName[selectItemIndex]);
                    writer.Write(m_objMotorPosition.dPosition[selectItemIndex]);
                    writer.Write(m_objMotorPosition.dPrevPosition[selectItemIndex]);
                    writer.Write(m_objMotorPosition.dVelocity[selectItemIndex]);
                    writer.Write(m_objMotorPosition.dAcceleration[selectItemIndex]);
                    writer.Write(m_objMotorPosition.dDeceleration[selectItemIndex]);
                    writer.Write(m_objMotorPosition.dTolerence[selectItemIndex]);
                    Clipboard.SetDataObject(memoryStream.ToArray());
                    m_objDocument.SetUpdateButtonLog(this, $"Cut to clipboard.\t[strPositionName:{m_objMotorPosition.strPositionName[selectItemIndex]}]");

                    m_objMotorPosition.strPositionName[selectItemIndex] = string.Empty;
                    m_objMotorPosition.dPosition[selectItemIndex] = 0d;
                    m_objMotorPosition.dPrevPosition[selectItemIndex] = 0d;
                    m_objMotorPosition.dVelocity[selectItemIndex] = 10d;
                    m_objMotorPosition.dAcceleration[selectItemIndex] = 200d;
                    m_objMotorPosition.dDeceleration[selectItemIndex] = 200d;
                    m_objMotorPosition.dTolerence[selectItemIndex] = 0d;
                    SetPositionInformationList(false);
                }
            }
            else if (
                e.Control == true
                && e.KeyCode == Keys.V
                )
            {
                var clipboardValue = Clipboard.GetDataObject().GetData(typeof(byte[])) as byte[];
                if (clipboardValue != null)
                {
                    using (var memoryStream = new MemoryStream(clipboardValue))
                    using (var reader = new BinaryReader(memoryStream))
                    {
                        Guid readGuid = new Guid(reader.ReadBytes(16));
                        if (GUID == readGuid)
                        {
                            m_objMotorPosition.strPositionName[selectItemIndex] = reader.ReadString();
                            m_objMotorPosition.dPosition[selectItemIndex] = reader.ReadDouble();
                            m_objMotorPosition.dPrevPosition[selectItemIndex] = reader.ReadDouble();
                            m_objMotorPosition.dVelocity[selectItemIndex] = reader.ReadDouble();
                            m_objMotorPosition.dAcceleration[selectItemIndex] = reader.ReadDouble();
                            m_objMotorPosition.dDeceleration[selectItemIndex] = reader.ReadDouble();
                            m_objMotorPosition.dTolerence[selectItemIndex] = reader.ReadDouble();
                            m_objDocument.SetUpdateButtonLog(this, $"Paste from clipboard.\t[strPositionName:{m_objMotorPosition.strPositionName[selectItemIndex]}]");
                            SetPositionInformationList(false);
                        }
                    }
                }
            }
        }

        private void BtnCurrentPosition_Click(object sender, EventArgs e)
        {
            // 클릭시 선택한 모터의 현재 위치 값 클립 보드로 복사함
            Clipboard.SetText(BtnCurrentPosition.Text);
        }

        private async void BtnRepeatMove_Click(object sender, EventArgs e)
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

            if (m_objDocument.m_objProcessMain.m_objProcessMotion.CheckAllSequenceManagerStateIsBatchMoving() == true)
            {
                return;
            }

            CProcessMotion.EMotor motorIndex = (CProcessMotion.EMotor)Enum.Parse(typeof(CProcessMotion.EMotor), m_listMotorInformation[m_iSelectedRow].HLGetMotorInitializeParameter().strMotorName);

            int nRepeatPositionCount = 2;
            using (FormKeyPad dialog = new FormKeyPad(nRepeatPositionCount, 2, 10, $"[{motorIndex}]\n{mResourceTitleRepeatPositionCount}"))
            {
                dialog.BringToFront();
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                nRepeatPositionCount = Convert.ToInt32(dialog.m_dResultValue);
            }
            using (FormKeyPad dialog = new FormKeyPad(mRepeatCounts[m_iSelectedRow], 1, 9999, $"[{motorIndex}]\n{mResourceTitleRepeatCount}"))
            {
                dialog.BringToFront();
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                mRepeatCounts[m_iSelectedRow] = Convert.ToInt32(dialog.m_dResultValue);
            }

            mRepeatPositions[m_iSelectedRow] = new int[nRepeatPositionCount];

            Regex regex = new Regex(@"\[(\d{2})\]");
            string[] selectItems = Enumerable.Range(0, m_listMotorInformation[m_iSelectedRow].HLGetMotorPosition().strPositionName.Length)
                .Where(i => string.IsNullOrWhiteSpace(m_listMotorInformation[m_iSelectedRow].HLGetMotorPosition().strPositionName[i]) == false)
                .Select(i => $"[{i:00}] {m_listMotorInformation[m_iSelectedRow].HLGetMotorPosition().strPositionName[i]}")
                .ToArray();
            for (int iLoop = 0; iLoop < nRepeatPositionCount; iLoop++)
            {
                using (FormEnumSelect dialog = new FormEnumSelect(selectItems, $"[{mRepeatPositions[m_iSelectedRow][iLoop]:00}] {m_listMotorInformation[m_iSelectedRow].HLGetMotorPosition().strPositionName[mRepeatPositions[m_iSelectedRow][iLoop]]}"))
                {
                    dialog.TitleText = $"{iLoop + 1}{mResourceTitleRepeatPosition}";
                    dialog.BringToFront();
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    var match = regex.Match(dialog.ResultName);
                    if (match.Success == false)
                    {
                        return;
                    }
                    mRepeatPositions[m_iSelectedRow][iLoop] = Convert.ToInt32(match.Result("$1"));
                }
            }

            await m_objDocument.m_objProcessMain.m_objProcessMotion.MotorRepeatMoveRun(m_listInterlock[m_iSelectedRow], motorIndex, mRepeatCounts[m_iSelectedRow], mRepeatPositions[m_iSelectedRow]);
        }

        private void BtnPositionJog_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView objGridView = GridViewPositionInformationList;

            do
            {
                int iRow = objGridView.CurrentCell.RowIndex;
                // 선택한 셀이 없을 경우 
                if (0 > iRow || 0 > m_iSelectedRow)
                {
                    break;
                }

                if (m_listInterlock.Count == 0)
                {
                    break;
                }

                int iPositionIndex = iRow + (m_iPageCount * m_iPositionCurrentPage);
                double targetPos = m_listMotorInformation[m_iSelectedRow].HLGetMotorPosition().dPosition[iPositionIndex];
                mbJogPositiveDirection = m_listMotorInformation[m_iSelectedRow].HLGetMotorStatus().dEncoderPosition < targetPos;

                // Jog로 움직일 때 Fast Mode의 경우 Forced Interlock 체크(움직이려는 모터축과 관계된 모든 방해물 제거)
                if (false == m_listInterlock[m_iSelectedRow].CheckForcedInterlock(m_listMotorInformation[m_iSelectedRow].HLGetMotorInitializeParameter().strMotorName, mbJogPositiveDirection, null))
                {
                    break;
                }

                if (EJogVelocityMode.JOG_FAST_MODE == m_eJogVelocityMode)
                {
                    m_listMotorInformation[m_iSelectedRow].HLMoveAbsolutePosition(m_listMotorInformation[m_iSelectedRow].HLGetMotorPosition().dPosition[iPositionIndex], m_listMotorInformation[m_iSelectedRow].HLGetMotorOperationParameter().dVelocityJogFast);
                }
                else
                {
                    m_listMotorInformation[m_iSelectedRow].HLMoveAbsolutePosition(m_listMotorInformation[m_iSelectedRow].HLGetMotorPosition().dPosition[iPositionIndex], m_listMotorInformation[m_iSelectedRow].HLGetMotorOperationParameter().dVelocityJogFast);
                }

                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{m_eJogVelocityMode}] [Target: {m_listMotorInformation[m_iSelectedRow].HLGetMotorPosition().dPosition[iPositionIndex]:0.000}]");
                mbJogMoving = true;
                m_objDocument.m_objProcessMain.IsJogMoving = true;
            } while (false);
        }

        private void BtnPositionJog_MouseUp(object sender, MouseEventArgs e)
        {
            do
            {
                if (0 > m_iSelectedRow)
                {
                    break;
                }
                m_listMotorInformation[m_iSelectedRow].HLMoveStop();
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");

                mbJogMoving = false;
                m_objDocument.m_objProcessMain.IsJogMoving = false;
            } while (false);
        }
    }
}