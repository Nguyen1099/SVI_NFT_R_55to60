using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormTeachCylinder : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 실린더 동작 정보 정의
        /// </summary>
        private enum ECylinderInformation
        {
            NO = 0,
            CYLINDFER_NAME,
            UP_TIME,
            DOWN_TIME,
            CYCLE_TIME,
            MIN_MAX_DELTA_TIME,
            CYLINDER_INFORMATION_LIST_COLUMN_FINAL
        };

        /// <summary>
        /// 도큐먼트
        /// </summary>
        private readonly CDocument m_objDocument;
        /// <summary>
        /// 실린더 리스트
        /// </summary>
        private List<CCylinder> m_listCylinder;
        /// <summary>
        /// 반복 동작 카운트
        /// </summary>
        private int m_iRepeatCount = 0;
        /// <summary>
        /// 반복 동작 누적 횟수
        /// </summary>
        private int m_iRepeatIndex = 0;
        /// <summary>
        /// 대표 실린더 객체
        /// </summary>
        private CCylinder m_objCylinder;
        /// <summary>
        /// 실린더 데이터
        /// </summary>
        private CCylinderAbstract.CCylinderDataParameter m_objCylinderDataParameter;
        /// <summary>
        /// 선택된 실린더 리스트
        /// </summary>
        private readonly List<CCylinder> mSelectionCylinders = new List<CCylinder>();
        private bool m_bRepeatStop = false;
        private string mResourceTitleDelayTimeUp;
        private string mResourceTitleDelayTimeDown;
        private string mResourceTitleMoveTimeUp;
        private string mResourceTitleMoveTimeDown;
        private string mResourceMoveUp;
        private string mResourceMoveDown;
        private string mResourceTitleDelayTimeLeft;
        private string mResourceTitleDelayTimeRight;
        private string mResourceTitleMoveTimeLeft;
        private string mResourceTitleMoveTimeRight;
        private string mResourceMoveLeft;
        private string mResourceMoveRight;
        private string mResourceTitleDelayTimeForward;
        private string mResourceTitleDelayTimeBackward;
        private string mResourceTitleMoveTimeForward;
        private string mResourceTitleMoveTimeBackward;
        private string mResourceMoveForward;
        private string mResourceMoveBackward;
        private string mResourceTitleDelayTimeCw;
        private string mResourceTitleDelayTimeCcw;
        private string mResourceTitleMoveTimeCw;
        private string mResourceTitleMoveTimeCcw;
        private string mResourceMoveCw;
        private string mResourceMoveCcw;
        private string mResourceTitleDelayTimeReturn;
        private string mResourceTitleDelayTimeTurn;
        private string mResourceTitleMoveTimeReturn;
        private string mResourceTitleMoveTimeTurn;
        private string mResourceMoveReturn;
        private string mResourceMoveTurn;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormTeachCylinder(CDocument objDocument)
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
        private void CFormTeachCylinder_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormTeachCylinder_FormClosed(object sender, FormClosedEventArgs e)
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
                // 버튼 색상 정의
                SetButtonColor();
                // 실린더 객체 정보 추가
                m_listCylinder = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objCylinder.Values.ToList();

                // 그리드 뷰 초기화
                InitializeGridView(GridViewCylinderList);
                InitializeGridView(GridViewCylinderTimeList);
                // 항목 표시
                SetMotorInformationGridView(m_listCylinder);

                // 실린더 리스트 다중 선택 설정
                GridViewCylinderList.MultiSelect = true;
                // 실린더 처음 항목 데이터 로드
                GridViewCylinderList.Rows[0].Selected = true;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            base.SetButtonBackColor(BtnTitleCylinderList, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleCylinderDelayTime, m_colorLabelSub);
            base.SetButtonBackColor(BtnTitleFirstDelayTime, m_colorLabelSub);
            base.SetButtonBackColor(BtnFirstDelayTime, m_colorLabelData);
            base.SetButtonBackColor(BtnTitleSecondDelayTime, m_colorLabelSub);
            base.SetButtonBackColor(BtnSecondDelayTime, m_colorLabelData);
            base.SetButtonBackColor(BtnCylinderRepeatMove, m_colorLabelSub);
            base.SetButtonBackColor(BtnFirstMove, m_colorNormal);
            base.SetButtonBackColor(BtnSecondMove, m_colorNormal);
            base.SetButtonBackColor(BtnTitleCycleMoveTime, m_colorLabelSub);
            base.SetButtonBackColor(BtnCycleMoveTime, m_colorLabelData);
            base.SetButtonBackColor(BtnTitleRepeatCount, m_colorLabelSub);
            base.SetButtonBackColor(BtnRepeatCount, m_colorLabelData);
            base.SetButtonBackColor(BtnTitleFirstMoveTime, m_colorLabelSub);
            base.SetButtonBackColor(BtnFirstMoveTime, m_colorLabelData);
            base.SetButtonBackColor(BtnTitleSecondMoveTime, m_colorLabelSub);
            base.SetButtonBackColor(BtnSecondMoveTime, m_colorLabelData);
            base.SetButtonBackColor(BtnRepeatMove, m_colorNormal);
            base.SetButtonBackColor(BtnRepeatStop, m_colorNormal);
            base.SetButtonBackColor(BtnGridViewClear, m_colorNormal);
            base.SetButtonBackColor(BtnSave, m_colorNormal);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnFirstDelayTime.Name)
                    && false == btn.Name.Equals(BtnSecondDelayTime.Name)
                    && false == btn.Name.Equals(BtnRepeatCount.Name)
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

                GridViewCylinderList.Enabled = true;

                if (CDefine.EUserAuthorityLevel.MASTER == objUserInformation.m_eAuthorityLevel)
                {
                    BtnFirstDelayTime.Enabled = true;
                    BtnSecondDelayTime.Enabled = true;
                    BtnSave.Enabled = true;
                }
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        base.SetControlButtonEnable(Controls, false);
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        base.SetControlButtonEnable(Controls, true);
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
        /// 그리드 뷰 초기화
        /// </summary>
        /// <param name="objGridView"></param>
        /// <returns></returns>
        private new bool InitializeGridView(DataGridView objGridView)
        {
            bool bReturn = false;

            do
            {

                // 그리드 뷰 기본 스타일 초기화
                if (false == base.InitializeGridView(objGridView))
                {
                    break;
                }
                objGridView.RowHeadersVisible = false;
                // 행 조정 x
                objGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                // 그리드 뷰 ReadOnly
                objGridView.ReadOnly = true;
                // 그리드 뷰 다중 선택 x
                objGridView.MultiSelect = false;
                // 그리드 뷰 스크롤바 x
                //objGridView.ScrollBars = ScrollBars.None;
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                // 칼럼 정렬 기능 x
                objGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, "신명조", 10.0, FontStyle.Regular);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 실린더 정보 그리드 뷰에 고정 틀로 뿌려줌
        /// </summary>
        /// <param name="objMotorInformationList"></param>
        private void SetMotorInformationGridView(List<CCylinder> objMotorInformationList)
        {
            DataGridView objGridView = GridViewCylinderList;
            // 행 초기화
            objGridView.Rows.Clear();

            for (int iLoopCount = 0; iLoopCount < objMotorInformationList.Count; iLoopCount++)
            {
                string strRowData = objMotorInformationList[iLoopCount].GetCylinderName();
                objGridView.Rows.Add(strRowData);
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
                SetButtonChangeLanguage(BtnTitleCylinderList);
                SetButtonChangeLanguage(BtnTitleCylinderDelayTime);
                SetButtonChangeLanguage(BtnCylinderRepeatMove);
                SetButtonChangeLanguage(BtnTitleCycleMoveTime);
                SetButtonChangeLanguage(BtnTitleRepeatCount);
                SetButtonChangeLanguage(BtnRepeatMove);
                SetButtonChangeLanguage(BtnRepeatStop);
                SetButtonChangeLanguage(BtnGridViewClear);
                SetButtonChangeLanguage(BtnSave);

                mResourceTitleDelayTimeUp = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleDelayTimeUp), Name);
                mResourceTitleDelayTimeDown = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleDelayTimeDown), Name);
                mResourceTitleMoveTimeUp = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleMoveTimeUp), Name);
                mResourceTitleMoveTimeDown = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleMoveTimeDown), Name);
                mResourceMoveUp = m_objDocument.GetDatabaseUIText(nameof(mResourceMoveUp), Name);
                mResourceMoveDown = m_objDocument.GetDatabaseUIText(nameof(mResourceMoveDown), Name);
                mResourceTitleDelayTimeLeft = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleDelayTimeLeft), Name);
                mResourceTitleDelayTimeRight = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleDelayTimeRight), Name);
                mResourceTitleMoveTimeLeft = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleMoveTimeLeft), Name);
                mResourceTitleMoveTimeRight = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleMoveTimeRight), Name);
                mResourceMoveLeft = m_objDocument.GetDatabaseUIText(nameof(mResourceMoveLeft), Name);
                mResourceMoveRight = m_objDocument.GetDatabaseUIText(nameof(mResourceMoveRight), Name);
                mResourceTitleDelayTimeForward = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleDelayTimeForward), Name);
                mResourceTitleDelayTimeBackward = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleDelayTimeBackward), Name);
                mResourceTitleMoveTimeForward = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleMoveTimeForward), Name);
                mResourceTitleMoveTimeBackward = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleMoveTimeBackward), Name);
                mResourceMoveForward = m_objDocument.GetDatabaseUIText(nameof(mResourceMoveForward), Name);
                mResourceMoveBackward = m_objDocument.GetDatabaseUIText(nameof(mResourceMoveBackward), Name);
                mResourceTitleDelayTimeCw = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleDelayTimeCw), Name);
                mResourceTitleDelayTimeCcw = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleDelayTimeCcw), Name);
                mResourceTitleMoveTimeCw = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleMoveTimeCw), Name);
                mResourceTitleMoveTimeCcw = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleMoveTimeCcw), Name);
                mResourceMoveCw = m_objDocument.GetDatabaseUIText(nameof(mResourceMoveCw), Name);
                mResourceMoveCcw = m_objDocument.GetDatabaseUIText(nameof(mResourceMoveCcw), Name);
                mResourceTitleDelayTimeReturn = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleDelayTimeReturn), Name);
                mResourceTitleDelayTimeTurn = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleDelayTimeTurn), Name);
                mResourceTitleMoveTimeReturn = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleMoveTimeReturn), Name);
                mResourceTitleMoveTimeTurn = m_objDocument.GetDatabaseUIText(nameof(mResourceTitleMoveTimeTurn), Name);
                mResourceMoveReturn = m_objDocument.GetDatabaseUIText(nameof(mResourceMoveReturn), Name);
                mResourceMoveTurn = m_objDocument.GetDatabaseUIText(nameof(mResourceMoveTurn), Name);


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
            }
        }

        /// <summary>
        /// 타이머 동작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            string strMilisecond = " ( ㎳ )";
            if (null != m_objCylinder)
            {
                // 첫 동작 딜레이 시간 표시
                BtnFirstDelayTime.Text = m_objCylinderDataParameter.iFirstMoveAfterDelayTime.ToString() + strMilisecond;
                // 두번째 동작 딜레이 시간 표시
                BtnSecondDelayTime.Text = m_objCylinderDataParameter.iSecondMoveAfterDelayTime.ToString() + strMilisecond;
                // 첫번째 동작 시간 표시
                BtnFirstMoveTime.Text = m_objCylinder.m_dFirstMoveTime.ToString() + strMilisecond;
                // 두번째 동작 시간 표시
                BtnSecondMoveTime.Text = m_objCylinder.m_dSecondMoveTime.ToString() + strMilisecond;
                // 사이클 동작 시간 표시
                BtnCycleMoveTime.Text = (m_objCylinder.m_dFirstMoveTime + m_objCylinder.m_dSecondMoveTime).ToString() + strMilisecond;

                var getStatus = m_objCylinder.Status;
                var getCommand = m_objCylinder.Command;
                bool bHasSensor = m_objCylinder.HasSensor();
                switch (m_objCylinder.GetCylinderType())
                {
                    default:
                    case CCylinderAbstract.ECylinderType.UpDown:
                        SetButtonText(BtnTitleFirstDelayTime, mResourceTitleDelayTimeUp);
                        SetButtonText(BtnTitleSecondDelayTime, mResourceTitleDelayTimeDown);
                        SetButtonText(BtnTitleFirstMoveTime, mResourceTitleMoveTimeUp);
                        SetButtonText(BtnTitleSecondMoveTime, mResourceTitleMoveTimeDown);
                        SetButtonText(BtnFirstMove, mResourceMoveUp);
                        SetButtonText(BtnSecondMove, mResourceMoveDown);
                        if (bHasSensor == true)
                        {
                            SetButtonBackColor(BtnFirstMove, (CCylinderAbstract.ECylinderStatus.STS_UP == getStatus) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_UP == getStatus);
                            SetButtonBackColor(BtnSecondMove, (CCylinderAbstract.ECylinderStatus.STS_DOWN == getStatus) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_DOWN == getStatus);
                        }
                        else
                        {
                            SetButtonBackColor(BtnFirstMove, (CCylinderAbstract.ECylinderCommand.CMD_UP == getCommand) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_UP == getCommand);
                            SetButtonBackColor(BtnSecondMove, (CCylinderAbstract.ECylinderCommand.CMD_DOWN == getCommand) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_DOWN == getCommand);
                        }
                        break;

                    case CCylinderAbstract.ECylinderType.LeftRigth:
                        SetButtonText(BtnTitleFirstDelayTime, mResourceTitleDelayTimeLeft);
                        SetButtonText(BtnTitleSecondDelayTime, mResourceTitleDelayTimeRight);
                        SetButtonText(BtnTitleFirstMoveTime, mResourceTitleMoveTimeLeft);
                        SetButtonText(BtnTitleSecondMoveTime, mResourceTitleMoveTimeRight);
                        SetButtonText(BtnFirstMove, mResourceMoveLeft);
                        SetButtonText(BtnSecondMove, mResourceMoveRight);
                        if (bHasSensor == true)
                        {
                            SetButtonBackColor(BtnFirstMove, (CCylinderAbstract.ECylinderStatus.STS_LEFT == getStatus) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_LEFT == getStatus);
                            SetButtonBackColor(BtnSecondMove, (CCylinderAbstract.ECylinderStatus.STS_RIGHT == getStatus) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_RIGHT == getStatus);
                        }
                        else
                        {
                            SetButtonBackColor(BtnFirstMove, (CCylinderAbstract.ECylinderCommand.CMD_LEFT == getCommand) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_LEFT == getCommand);
                            SetButtonBackColor(BtnSecondMove, (CCylinderAbstract.ECylinderCommand.CMD_RIGHT == getCommand) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_RIGHT == getCommand);
                        }
                        break;

                    case CCylinderAbstract.ECylinderType.ForwardBackward:
                        SetButtonText(BtnTitleFirstDelayTime, mResourceTitleDelayTimeForward);
                        SetButtonText(BtnTitleSecondDelayTime, mResourceTitleDelayTimeBackward);
                        SetButtonText(BtnTitleFirstMoveTime, mResourceTitleMoveTimeForward);
                        SetButtonText(BtnTitleSecondMoveTime, mResourceTitleMoveTimeBackward);
                        SetButtonText(BtnFirstMove, mResourceMoveForward);
                        SetButtonText(BtnSecondMove, mResourceMoveBackward);
                        if (bHasSensor == true)
                        {
                            SetButtonBackColor(BtnFirstMove, (CCylinderAbstract.ECylinderStatus.STS_FORWARD == getStatus) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_FORWARD == getStatus);
                            SetButtonBackColor(BtnSecondMove, (CCylinderAbstract.ECylinderStatus.STS_BACKWARD == getStatus) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_BACKWARD == getStatus);
                        }
                        else
                        {
                            SetButtonBackColor(BtnFirstMove, (CCylinderAbstract.ECylinderCommand.CMD_FORWARD == getCommand) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_FORWARD == getCommand);
                            SetButtonBackColor(BtnSecondMove, (CCylinderAbstract.ECylinderCommand.CMD_BACKWARD == getCommand) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_BACKWARD == getCommand);
                        }
                        break;

                    case CCylinderAbstract.ECylinderType.CwCcw:
                        SetButtonText(BtnTitleFirstDelayTime, mResourceTitleDelayTimeCw);
                        SetButtonText(BtnTitleSecondDelayTime, mResourceTitleDelayTimeCcw);
                        SetButtonText(BtnTitleFirstMoveTime, mResourceTitleMoveTimeCw);
                        SetButtonText(BtnTitleSecondMoveTime, mResourceTitleMoveTimeCcw);
                        SetButtonText(BtnFirstMove, mResourceMoveCw);
                        SetButtonText(BtnSecondMove, mResourceMoveCcw);
                        if (bHasSensor == true)
                        {
                            SetButtonBackColor(BtnFirstMove, (CCylinderAbstract.ECylinderStatus.STS_CW == getStatus) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_CW == getStatus);
                            SetButtonBackColor(BtnSecondMove, (CCylinderAbstract.ECylinderStatus.STS_CCW == getStatus) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_CCW == getStatus);
                        }
                        else
                        {
                            SetButtonBackColor(BtnFirstMove, (CCylinderAbstract.ECylinderCommand.CMD_CW == getCommand) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_CW == getCommand);
                            SetButtonBackColor(BtnSecondMove, (CCylinderAbstract.ECylinderCommand.CMD_CCW == getCommand) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_CCW == getCommand);
                        }
                        break;

                    case CCylinderAbstract.ECylinderType.ReturnTurn:
                        SetButtonText(BtnTitleFirstDelayTime, mResourceTitleDelayTimeReturn);
                        SetButtonText(BtnTitleSecondDelayTime, mResourceTitleDelayTimeTurn);
                        SetButtonText(BtnTitleFirstMoveTime, mResourceTitleMoveTimeReturn);
                        SetButtonText(BtnTitleSecondMoveTime, mResourceTitleMoveTimeTurn);
                        SetButtonText(BtnFirstMove, mResourceMoveReturn);
                        SetButtonText(BtnSecondMove, mResourceMoveTurn);
                        if (bHasSensor == true)
                        {
                            SetButtonBackColor(BtnFirstMove, (CCylinderAbstract.ECylinderStatus.STS_RETURN == getStatus) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_RETURN == getStatus);
                            SetButtonBackColor(BtnSecondMove, (CCylinderAbstract.ECylinderStatus.STS_TURN == getStatus) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_TURN == getStatus);
                        }
                        else
                        {
                            SetButtonBackColor(BtnFirstMove, (CCylinderAbstract.ECylinderCommand.CMD_RETURN == getCommand) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_RETURN == getCommand);
                            SetButtonBackColor(BtnSecondMove, (CCylinderAbstract.ECylinderCommand.CMD_TURN == getCommand) ? m_colorOn : m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_TURN == getCommand);
                        }
                        break;
                }
            }

            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                return;
            }

            // 반복 실행중 상태 표시
            {
                SetButtonBackColor(BtnRepeatMove, (true == backgroundWorkerRepeat.IsBusy) ? m_colorOn : m_colorNormal, true == backgroundWorkerRepeat.IsBusy);
                SetButtonBackColor(BtnRepeatStop, (false == backgroundWorkerRepeat.IsBusy) ? m_colorOn : m_colorNormal, false == backgroundWorkerRepeat.IsBusy);

                GridViewCylinderList.Enabled = !backgroundWorkerRepeat.IsBusy;
                BtnFirstDelayTime.Enabled = !backgroundWorkerRepeat.IsBusy;
                BtnSecondDelayTime.Enabled = !backgroundWorkerRepeat.IsBusy;
                BtnFirstMove.Enabled = !backgroundWorkerRepeat.IsBusy;
                BtnSecondMove.Enabled = !backgroundWorkerRepeat.IsBusy;
                BtnRepeatMove.Enabled = !backgroundWorkerRepeat.IsBusy;
                BtnRepeatCount.Enabled = !backgroundWorkerRepeat.IsBusy;
                BtnSave.Enabled = !backgroundWorkerRepeat.IsBusy;
            }
        }

        /// <summary>
        /// 반복 동작 카운트 설정.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRepeatCount_Click(object sender, EventArgs e)
        {
            if (true == backgroundWorkerRepeat.IsBusy)
            {
                return;
            }

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnRepeatCount_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (var objKeyPad = new FormKeyPad(Convert.ToDouble(0.0)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    m_iRepeatCount = ((int)objKeyPad.m_dResultValue > 0) ? (int)objKeyPad.m_dResultValue : 0;
                    BtnRepeatCount.Text = m_iRepeatCount.ToString();
                }
            }

            // 버튼 로그 추가
            strLog = string.Format("[{0}] [RepeatCount : {1:D}] [{2}]", "BtnRepeatCount_Click", m_iRepeatCount, true);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 실린더 반복 동작 실행.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRepeatMove_Click(object sender, EventArgs e)
        {
            do
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnRepeatMove_Click", true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                if (true == backgroundWorkerRepeat.IsBusy)
                {
                    break;
                }

                backgroundWorkerRepeat.RunWorkerAsync();
            } while (false);
        }

        /// <summary>
        /// 실린더 동작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFirstMove_Click(object sender, EventArgs e)
        {
            if (true == backgroundWorkerRepeat.IsBusy)
            {
                return;
            }

            Parallel.ForEach(mSelectionCylinders, cylinder =>
            {
                var setCmd = default(CCylinderAbstract.ECylinderCommand);
                switch (cylinder.GetCylinderType())
                {
                    default:
                    case CCylinderAbstract.ECylinderType.UpDown:
                        setCmd = CCylinderAbstract.ECylinderCommand.CMD_UP;
                        break;
                    case CCylinderAbstract.ECylinderType.LeftRigth:
                        setCmd = CCylinderAbstract.ECylinderCommand.CMD_LEFT;
                        break;
                    case CCylinderAbstract.ECylinderType.ForwardBackward:
                        setCmd = CCylinderAbstract.ECylinderCommand.CMD_FORWARD;
                        break;
                    case CCylinderAbstract.ECylinderType.CwCcw:
                        setCmd = CCylinderAbstract.ECylinderCommand.CMD_CW;
                        break;
                    case CCylinderAbstract.ECylinderType.ReturnTurn:
                        setCmd = CCylinderAbstract.ECylinderCommand.CMD_RETURN;
                        break;
                }
                cylinder.SetCylinderCommand(setCmd);
            });

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Cylinder Command : {1}]", "BtnFirstMove_Click", "");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 실린더 동작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSecondMove_Click(object sender, EventArgs e)
        {
            if (true == backgroundWorkerRepeat.IsBusy)
            {
                return;
            }

            Parallel.ForEach(mSelectionCylinders, cylinder =>
            {
                var setCmd = default(CCylinderAbstract.ECylinderCommand);
                switch (cylinder.GetCylinderType())
                {
                    default:
                    case CCylinderAbstract.ECylinderType.UpDown:
                        setCmd = CCylinderAbstract.ECylinderCommand.CMD_DOWN;
                        break;
                    case CCylinderAbstract.ECylinderType.LeftRigth:
                        setCmd = CCylinderAbstract.ECylinderCommand.CMD_RIGHT;
                        break;
                    case CCylinderAbstract.ECylinderType.ForwardBackward:
                        setCmd = CCylinderAbstract.ECylinderCommand.CMD_BACKWARD;
                        break;
                    case CCylinderAbstract.ECylinderType.CwCcw:
                        setCmd = CCylinderAbstract.ECylinderCommand.CMD_CCW;
                        break;
                    case CCylinderAbstract.ECylinderType.ReturnTurn:
                        setCmd = CCylinderAbstract.ECylinderCommand.CMD_TURN;
                        break;
                }
                cylinder.SetCylinderCommand(setCmd);
            });

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Cylinder Command : {1}]", "BtnSecondMove_Click", "");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 항목 클리어
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGridViewClear_Click(object sender, EventArgs e)
        {
            GridViewCylinderTimeList.Rows.Clear();
            m_iRepeatIndex = 0;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [RepeatIndex : {1:D}]", "BtnGridViewClear_Click", m_iRepeatIndex);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 첫 동작 딜레이 시간 조절
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFirstDelayTime_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnFirstDelayTime_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (var objKeyPad = new FormKeyPad(Convert.ToDouble(0.0)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    m_objCylinderDataParameter.iFirstMoveAfterDelayTime = ((int)objKeyPad.m_dResultValue > 0) ? (int)objKeyPad.m_dResultValue : 0;
                }
            }

            // 버튼 로그 추가
            strLog = string.Format("[{0}] [FirstMoveAfterDelayTime : {1:D}] [{2}]", "BtnFirstDelayTime_Click", m_objCylinderDataParameter.iFirstMoveAfterDelayTime, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 두번째 동작 딜레이 시간 조절
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSecondDelayTime_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnSecondDelayTime_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (var objKeyPad = new FormKeyPad(Convert.ToDouble(0.0)))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    m_objCylinderDataParameter.iSecondMoveAfterDelayTime = ((int)objKeyPad.m_dResultValue > 0) ? (int)objKeyPad.m_dResultValue : 0;
                }
            }

            // 버튼 로그 추가
            strLog = string.Format("[{0}] [SecondMoveAfterDelayTime : {1:D}] [{2}]", "BtnSecondDelayTime_Click", m_objCylinderDataParameter.iSecondMoveAfterDelayTime, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 항목 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE))
                return;

            m_objCylinder.SaveCylinderData("", "", m_objCylinderDataParameter);
            // Msg: 저장이 완료되었습니다.
            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.SAVING_IS_COMPLETE);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}]", "BtnSave_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnRepeatStop_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnRepeatStop_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            m_bRepeatStop = true;

            // 버튼 로그 추가
            strLog = string.Format("[{0}] [{1}]", "BtnRepeatStop_Click", false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void backgroundWorkerRepeat_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            do
            {
                // 반복 동작 횟수가 없으면 동작 하지 않음
                if (0 >= m_iRepeatCount)
                {
                    return;
                }
                if (m_objDocument.GetRunStatus() != CDefine.ERunStatus.Stop)
                {
                    return;
                }

                string strMilisecond = " ( ㎳ )";
                string[] strData = new string[(int)ECylinderInformation.CYLINDER_INFORMATION_LIST_COLUMN_FINAL];
                m_bRepeatStop = false;
                for (int iLoopCount = 0; iLoopCount < m_iRepeatCount; iLoopCount++)
                {
                    if (true == m_bRepeatStop)
                    {
                        return;
                    }
                    if (m_objDocument.GetRunStatus() != CDefine.ERunStatus.Stop)
                    {
                        return;
                    }

                    for (int cycleCount = 0; cycleCount < 2; cycleCount++)
                    {
                        var result = Parallel.ForEach(mSelectionCylinders, cylinder =>
                        {
                            CCylinderAbstract.ECylinderCommand firstCmd;
                            CCylinderAbstract.ECylinderCommand secondCmd;
                            switch (cylinder.GetCylinderType())
                            {
                                default:
                                case CCylinderAbstract.ECylinderType.UpDown:
                                    firstCmd = CCylinderAbstract.ECylinderCommand.CMD_UP;
                                    secondCmd = CCylinderAbstract.ECylinderCommand.CMD_DOWN;
                                    break;
                                case CCylinderAbstract.ECylinderType.LeftRigth:
                                    firstCmd = CCylinderAbstract.ECylinderCommand.CMD_LEFT;
                                    secondCmd = CCylinderAbstract.ECylinderCommand.CMD_RIGHT;
                                    break;
                                case CCylinderAbstract.ECylinderType.ForwardBackward:
                                    firstCmd = CCylinderAbstract.ECylinderCommand.CMD_FORWARD;
                                    secondCmd = CCylinderAbstract.ECylinderCommand.CMD_BACKWARD;
                                    break;
                                case CCylinderAbstract.ECylinderType.CwCcw:
                                    firstCmd = CCylinderAbstract.ECylinderCommand.CMD_CW;
                                    secondCmd = CCylinderAbstract.ECylinderCommand.CMD_CCW;
                                    break;
                                case CCylinderAbstract.ECylinderType.ReturnTurn:
                                    firstCmd = CCylinderAbstract.ECylinderCommand.CMD_RETURN;
                                    secondCmd = CCylinderAbstract.ECylinderCommand.CMD_TURN;
                                    break;
                            }

                            CCylinderAbstract.ECylinderCommand executeCmd;
                            if (0 == cycleCount)
                            {
                                executeCmd = firstCmd;
                            }
                            else
                            {
                                executeCmd = secondCmd;
                            }
                            if (false == cylinder.SetCylinderCommand(executeCmd))
                            {
                                return;
                            }
                        });

                        if (false == result.IsCompleted)
                        {
                            return;
                        }
                    }

                    m_iRepeatIndex++;
                    // 그리드 기록
                    strData[(int)ECylinderInformation.NO] = m_iRepeatIndex.ToString();
                    strData[(int)ECylinderInformation.CYLINDFER_NAME] = m_objCylinder.GetCylinderName();
                    strData[(int)ECylinderInformation.UP_TIME] = m_objCylinder.m_dFirstMoveTime.ToString() + strMilisecond;
                    strData[(int)ECylinderInformation.DOWN_TIME] = m_objCylinder.m_dSecondMoveTime.ToString() + strMilisecond;
                    strData[(int)ECylinderInformation.CYCLE_TIME] = (m_objCylinder.m_dFirstMoveTime + m_objCylinder.m_dSecondMoveTime).ToString() + strMilisecond;
                    var query = from cylinder in mSelectionCylinders select cylinder.m_dFirstMoveTime + cylinder.m_dSecondMoveTime;
                    strData[(int)ECylinderInformation.MIN_MAX_DELTA_TIME] = string.Format("{0:0}{1}", query.Max() - query.Min(), strMilisecond);

                    setAddDataGridView(strData);
                }
            } while (false);

            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [RepeatCount : {1}] [{2}]", "BtnRepeatMove_Click", m_iRepeatCount, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private delegate void StringArgReturningVoidDelegate(string[] addData);
        private void setAddDataGridView(string[] addData)
        {
            if (true == GridViewCylinderTimeList.InvokeRequired)
            {
                var d = new StringArgReturningVoidDelegate(setAddDataGridView);
                Invoke(d, (object)addData);
            }
            else
            {
                GridViewCylinderTimeList.Rows.Add(addData);
                GridViewCylinderTimeList.CurrentCell = GridViewCylinderTimeList.Rows[GridViewCylinderTimeList.Rows.Count - 1].Cells[0];
                GridViewCylinderTimeList.Update();
            }
        }

        private void GridViewCylinderList_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView objGridView = sender as DataGridView;
            // 셀을 클릭하면 데이터 갱신하는 역활
            do
            {
                try
                {
                    mSelectionCylinders.Clear();

                    foreach (DataGridViewCell row in objGridView.SelectedCells)
                    {
                        mSelectionCylinders.Add(m_listCylinder[row.RowIndex]);
                    }

                    if (0 == mSelectionCylinders.Count)
                    {
                        // 모든 셀의 선택을 해제 했을 때 첫번째 셀을 다시 선택하도록함
                        GridViewCylinderList.Rows[0].Selected = true;
                    }
                    else
                    {
                        m_objCylinder = mSelectionCylinders[0];
                        m_objCylinderDataParameter = m_objCylinder.GetCylinderParameter();
                    }
                }
                catch (System.ArgumentOutOfRangeException ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }
    }
}