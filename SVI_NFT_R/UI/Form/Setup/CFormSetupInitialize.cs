using SVI_NFT_R.EHS;
using SVI_NFT_R.UI.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormSetupInitialize : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 해당 폼에서 사용할 컬러 define
        /// </summary>
        public class CInitializeColor
        {
            // 상황별 색깔 정의
            public Color colorError;
            public Color colorWaiting;
            public Color colorInitializing;
            public Color colorComplete;

            public CInitializeColor(Color complete, Color initializing, Color waiting, Color error)
            {
                colorError = error;
                colorWaiting = waiting;
                colorInitializing = initializing;
                colorComplete = complete;
            }
        };

        private class CheckGourpSet
        {
            public string Uiid { get; set; }
            public string Name { get; set; }
            public readonly List<IoIndicatorSet> CheckItems = new List<IoIndicatorSet>();
        }

        private class ProcessSet
        {
            public string Uiid { get; set; }
            public string Name { get; set; }
            public CProcessAbstract Process { get; set; }
        }

        private readonly List<CheckGourpSet> mCheckGroupItems = new List<CheckGourpSet>(16);
        private readonly List<ProcessSet> mProcessItems = new List<ProcessSet>(16);
        private int mCheckGroupIndex = 0;
        private int mCheckPageIndex = 0;
        private int mCheckPageCount = 1;
        private int mProcessPageIndex = 0;
        private int mProcessPageCount = 1;
        private UcCell[] mControlUcProcessItems;
        private UcCell[] mControlUcCheckGroupItems;
        private UcCell[] mControlUcCheckItems;
        private CDocument m_objDocument;

        private CInitializeColor m_objInitializeColor = new CInitializeColor(
            complete: m_colorOn,
            initializing: m_colorOrange,
            waiting: m_colorYellow,
            error: m_colorOff);

        private string mResourceProcessStateComplete = string.Empty;
        private string mResourceProcessStateInitializing = string.Empty;
        private string mResourceProcessStateWaiting = string.Empty;
        private string mResourceProcessStateError = string.Empty;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormSetupInitialize(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            InitializeComponent();

            // Display 영역에 맞게 Form 크기 피팅
            Size = PnlDisplayArea.Size;
            BackColor = PnlDisplayArea.BackColor;
        }

        /// <summary>
        /// 리치 에디터에 로그 메시지를 남긴다
        /// </summary>
        /// <param name="processLog">로그 메시지</param>
        public void SetInitializeProcessLog(string processLog)
        {
            if (RichInitializeLog.InvokeRequired)
            {
                Invoke(new Action<string>(SetInitializeProcessLog), new object[] { processLog });
            }
            else
            {
                var formatMessage = string.Format("{0: [ HH:mm:ss ]} : {1} {2}", DateTime.Now, processLog, Environment.NewLine);
                RichInitializeLog.SelectionColor = Color.Black;
                RichInitializeLog.SelectionBackColor = Color.White;
                RichInitializeLog.AppendText(formatMessage);
            }
        }

        /// <summary>
        /// 리치 에디터에 에러 로그 메시지를 남긴다
        /// </summary>
        /// <param name="errorLog">에러 메시지</param>
        public void SetInitializeErrorLog(string errorLog)
        {
            if (RichInitializeLog.InvokeRequired)
            {
                Invoke(new Action<string>(SetInitializeErrorLog), new object[] { errorLog });
            }
            else
            {
                var formatMessage = string.Format("{0: [ HH:mm:ss ]} : {1} {2}", DateTime.Now, errorLog, Environment.NewLine);
                RichInitializeLog.SelectionColor = Color.Red;
                RichInitializeLog.SelectionBackColor = Color.Black;
                RichInitializeLog.AppendText(formatMessage);
            }
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
        private void CFormSetupInitialize_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormSetupInitialize_FormClosed(object sender, FormClosedEventArgs e)
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
                m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
                // 컨트롤 리스트 초기화
                {
                    mControlUcProcessItems = PnlProcessItems.Controls.GetChildControlListByType(typeof(UcCell))
                        .Cast<UcCell>()
                        .ToArray();
                    mControlUcCheckGroupItems = PnlCheckGroupItems.Controls.GetChildControlListByType(typeof(UcCell))
                        .Cast<UcCell>()
                        .ToArray();
                    mControlUcCheckItems = PnlCheckItems.Controls.GetChildControlListByType(typeof(UcCell))
                        .Cast<UcCell>()
                        .ToArray();
                }
                // 설비 상태 점검 그룹 리스트 초기화
                SetCheckGroupItems();
                // 프로세스 리스트 초기화
                SetProcessItems();
                // 버튼 색상 정의
                SetButtonColor();
                // 버튼 태그 정의
                SetButtonTag();
                // 리치 텍스트 박스 초기화
                InitializeRichTextBox(RichInitializeLog);
                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 설비 상태 점검 그룹 리스트 초기화
        /// </summary>
        private void SetCheckGroupItems()
        {
            foreach (CDefine.ECheckGroup groupIndex in Enum.GetValues(typeof(CDefine.ECheckGroup)))
            {
                var groupItems = m_objDocument.GetMainFrame().GetCheckGroupItemsFromGroupIndex(groupIndex);
                if (groupItems.Count() == 0)
                {
                    continue;
                }
                CheckGourpSet checkGroupItem = new CheckGourpSet();
                checkGroupItem.Uiid = $"CheckGroup[{groupIndex}]";
                checkGroupItem.CheckItems.AddRange(groupItems);
                mCheckGroupItems.Add(checkGroupItem);
            }

            // 첫 번째 그룹 선택
            SetCheckGroupItemIndex(0);
        }

        /// <summary>
        /// 프로세스 리스트 초기화
        /// </summary>
        private void SetProcessItems()
        {
            CProcessAbstract[] processItems = CellDataManager.ProcessCells
                .Select(i => i.Value.First().OwnerManager)
                .ToArray();

            foreach (var item in CellDataManager.ProcessCells)
            {
                ProcessSet processSet = new ProcessSet();
                processSet.Uiid = item.Key.ToString();
                processSet.Process = item.Value.First().OwnerManager;
                mProcessItems.Add(processSet);
            }

            // 첫 번째 페이지 선택
            SetProcessPageIndex(0);
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            // 버튼 색 변경
            SetButtonBackColor(BtnTitleInitializeLog, m_colorLabel);
            SetButtonBackColor(BtnTitleCheck.BtnCellData, m_colorLabel);
            SetButtonBackColor(BtnTitleProcess.BtnCellData, m_colorLabel);

            SetButtonBackColor(BtnInitialize, m_colorNormal);
            SetButtonBackColor(BtnStop, m_colorNormal);

            SetButtonBackColor(BtnSubTitleCheckGroupName, m_colorLabel);

            PnlCheckBase.BackColor = m_colorLabelCategory;
            PnlCheckItems.BackColor = m_colorClick;
            PnlCheckItemPage.BackColor = m_colorClick;

            PnlProcessBase.BackColor = m_colorLabelCategory;

            var pagingItems = mControlUcProcessItems
                .Concat(mControlUcCheckItems);
            foreach (var item in pagingItems)
            {
                SetButtonBackColor(item.BtnCellData, m_colorLabelSub);
            }

            foreach (var item in mControlUcCheckGroupItems)
            {
                SetButtonBackColor(item.BtnCellData, m_colorNormal);
            }

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(nameof(UcCell.BtnCellData))
                    )
                {
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                    btn.BackColorChanged += NonClickableButton_BackColorChanged;
                    btn.Cursor = Cursors.Default;
                }
            }

            // !!! UserControl에 버튼 이름이 같아서 따로 처리해줌
            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var nonClickableButtons = pagingItems
                .Select(item => item.BtnCellData)
                .Concat(new SpeedButton[] { BtnTitleCheck.BtnCellData, BtnTitleProcess.BtnCellData });
            foreach (var item in nonClickableButtons)
            {
                item.FlatAppearance.MouseOverBackColor = item.BackColor;
                item.FlatAppearance.MouseDownBackColor = item.BackColor;
                item.BackColorChanged += NonClickableButton_BackColorChanged;
                item.Cursor = Cursors.Default;
            }
        }


        /// <summary>
        /// 버튼 이벤트에서 사용될 Tag 인스턴스 초기화
        /// </summary>
        private void SetButtonTag()
        {
            // 점검 그룹 아이템 인덱스 초기화
            {
                int index = 0;
                foreach (var item in mControlUcCheckGroupItems)
                {
                    item.Tag = index++;
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

                PnlCheckGroupItems.Enabled = true;
                SetControlButtonEnable(PnlCheckGroupItems.Controls, true);
                PnlCheckItemPage.Enabled = true;
                SetControlButtonEnable(PnlCheckItemPage.Controls, true);
                PnlProcessItemPage.Enabled = true;
                SetControlButtonEnable(PnlProcessItemPage.Controls, true);
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        SetControlButtonEnable(Controls, true);
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
                SetButtonChangeLanguage(BtnTitleInitializeLog);
                SetButtonText(BtnTitleCheck.BtnCellData, m_objDocument.GetDatabaseUIText(nameof(BtnTitleCheck), Name));
                SetButtonText(BtnTitleProcess.BtnCellData, m_objDocument.GetDatabaseUIText(nameof(BtnTitleProcess), Name));
                SetButtonChangeLanguage(BtnInitialize);
                SetButtonChangeLanguage(BtnStop);

                SetButtonChangeLanguage(BtnSubTitleCheckGroupName);

                foreach (var item in mProcessItems)
                {
                    item.Name = Resource.Get(item.Uiid).ToString();
                }

                foreach (var groupItem in mCheckGroupItems)
                {
                    groupItem.Name = m_objDocument.GetDatabaseUIText(groupItem.Uiid, "EquipmentInspection");
                    foreach (var item in groupItem.CheckItems)
                    {
                        item.SetChangeLanguage(m_objDocument.GetDatabaseUIText);
                    }
                }

                mResourceProcessStateComplete = m_objDocument.GetDatabaseUIText(nameof(mResourceProcessStateComplete), Name);
                mResourceProcessStateInitializing = m_objDocument.GetDatabaseUIText(nameof(mResourceProcessStateInitializing), Name);
                mResourceProcessStateWaiting = m_objDocument.GetDatabaseUIText(nameof(mResourceProcessStateWaiting), Name);
                mResourceProcessStateError = m_objDocument.GetDatabaseUIText(nameof(mResourceProcessStateError), Name);

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
        /// 리치 텍스트 박스 초기화
        /// </summary>
        /// <param name="objRich"></param>
        /// <returns></returns>
        public new bool InitializeRichTextBox(RichTextBox objRich)
        {
            bool bReturn = false;

            do
            {
                // 리치 텍스트 박스 기본 스타일 초기화
                if (false == base.InitializeRichTextBox(objRich))
                {
                    break;
                }
                // 리치 텍스트 박스 크기 변경
                SetRichTextBoxFont(objRich, 12.0);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 설비 초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInitialize_Click(object sender, EventArgs e)
        {
            do
            {
                // 런 중일 경우 초기화 버튼 안눌리게 한다.
                if (
                    CDefine.ERunStatus.Start == m_objDocument.GetRunStatus()
                    || CDefine.ERunStatus.Setup == m_objDocument.GetRunStatus()
                    || CDefine.ERunStatus.LoadingStop == m_objDocument.GetRunStatus()
                    || CDefine.ERunStatus.Initialize == m_objDocument.GetRunStatus()
                    || CDefine.ERunStatus.Stopping == m_objDocument.GetRunStatus()
                    || m_objDocument.m_objProcessMain.m_objProcessMotion.CheckAllSequenceManagerStateIsBatchMoving() == true
                    )
                {
                    break;
                }
                // 인터페이스 신호 초기화
                m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.DryRunSignalClear();
                //m_objDocument.m_objProcessMain.m_objProcessMotion.UnloadInterface.DryRunSignalClear();
                if (m_objDocument.m_objProcessMain.m_objProcessMotion.CheckAllSequenceManagerStateIsIdle() == false)
                {
                    break;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [RunMode : {1}] [Try]", "BtnInitialize_Click", CDefine.ERunStatus.Initialize.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);

                // 도어 잠김 확인
                if (false == m_objDocument.m_objProcessMain.CheckDoorLock())
                {
                    break;
                }

                // 오토 모드 확인
                if (false == m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto())
                {
                    // Msg: 세이프티 모드를 AUTO로 변경하시기 바랍니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                    break;
                }

                // 공유 메모리 내에 클리어 되지 않은 알람이 있는 경우 튕김
                if (true == m_objDocument.GetIsAlarmMessage())
                {
                    // Msg: 설비 알람 상태를 확인하여 주십시오.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.CHECK_THE_EQUIPMENT_ALARM_STATUS);
                    m_objDocument.SetChangeAlarmForm();
                    break;
                }

                // 인터페이스 인터락
                if (m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.CheckUpperEqInterfaceDoorOpened() == true)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_EQUIPMENT_DOOROPEN_SIGNAL_PARAM1, "UPPER");
                    break;
                }
                //if (m_objDocument.m_objProcessMain.m_objProcessMotion.UnloadInterface.CheckLowerEqInterfaceDoorOpened() == true)
                //{
                //    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_EQUIPMENT_DOOROPEN_SIGNAL_PARAM1, "LOWER");
                //    break;
                //}

                // 설비 상태 확인
                if (
                    CDefine.ESimulationMode.SIMULATION_MODE_OFF == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode
                    && false == m_objDocument.IsMasterLogin
                    && false == m_objDocument.m_objProcessMain.CheckEqpStatus()
                    )
                {
                    break;
                }

                // 이전 로그 클리어
                RichInitializeLog.Clear();

                // 가상 구성 사용중 경고
                if (m_objDocument.m_objConfig.UsingVirtualConfig == true)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_WARNING, CAlarmDefine.EMessageList.USING_VIRTUAL_CONFIG);
                    SetInitializeErrorLog($"## Some motors or robots are currently being used virtually in this equipment. ##");
                }

                // 초기화 시작
                m_objDocument.SetRunStatus(CDefine.ERunStatus.Initialize);

                // 버튼 로그 추가
                strLog = string.Format("[{0}] [RunMode : {1}] [Start]", "BtnInitialize_Click", CDefine.ERunStatus.Initialize.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        /// <summary>
        /// 설비 초기화 정지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // 런상태일 경우 버튼 클릭 안되게 한다.
            do
            {
                if (
                    CDefine.ERunStatus.Start == m_objDocument.GetRunStatus()
                    || CDefine.ERunStatus.Setup == m_objDocument.GetRunStatus()
                    || CDefine.ERunStatus.LoadingStop == m_objDocument.GetRunStatus()
                    )
                {
                    break;
                }

                if (CDefine.ERunStatus.Initialize == m_objDocument.GetRunStatus())
                {
                    m_objDocument.SetInitializeErrorLog("USER STOP");
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [RunMode : {1}]", "BtnCancel_Click", CDefine.ERunStatus.Stop.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);

                m_objDocument.SetRunStatus(CDefine.ERunStatus.Stop);
            } while (false);
        }

        /// <summary>
        /// 타이머 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // Update Check Group Items
            for (int i = 0; i < mControlUcCheckGroupItems.Length; i++)
            {
                bool bVisible = i < mCheckGroupItems.Count;
                if (mControlUcCheckGroupItems[i].Visible != bVisible)
                {
                    mControlUcCheckGroupItems[i].Visible = bVisible;
                }
                if (bVisible == false)
                {
                    continue;
                }

                SetButtonText(mControlUcCheckGroupItems[i].BtnCellData, mCheckGroupItems[i].Name);
                SetButtonBackColor(mControlUcCheckGroupItems[i].BtnCellData, mCheckGroupIndex == i ? m_colorClick : m_colorNormal);
                SetButtonBackColor(mControlUcCheckGroupItems[i].BtnIndicator, mCheckGroupItems[i].CheckItems.All(item => item.GetIo.Invoke() == true) == true ? m_colorOn : m_colorOff);
            }
            SetButtonBackColor(BtnTitleCheck.BtnIndicator, mControlUcCheckGroupItems.Where(i => i.Visible).All(i => i.BtnIndicator.BackColor == m_colorOn) ? m_colorOn : m_colorOff);

            // Update Check Items
            for (int i = 0; i < mControlUcCheckItems.Length; i++)
            {
                int index = i + (mControlUcCheckItems.Length * mCheckPageIndex);
                bool bVisible = index < mCheckGroupItems[mCheckGroupIndex].CheckItems.Count && mCheckGroupItems[i].CheckItems.Count > 0;
                if (mControlUcCheckItems[i].Visible != bVisible)
                {
                    mControlUcCheckItems[i].Visible = bVisible;
                }
                if (bVisible == false)
                {
                    continue;
                }

                mCheckGroupItems[mCheckGroupIndex].CheckItems[index].Update(mControlUcCheckItems[i].BtnCellData, mControlUcCheckItems[i].BtnIndicator);
            }
            SetControlText(LblNaviCheckItemPage, $"( {mCheckPageIndex + 1} / {mCheckPageCount} )");

            // Update Process Items
            Color processIndicatorColor = m_objInitializeColor.colorComplete;
            for (int i = 0; i < mControlUcProcessItems.Length; i++)
            {
                int index = i + (mControlUcProcessItems.Length * mProcessPageIndex);
                bool bVisible = index < mProcessItems.Count;
                if (mControlUcProcessItems[i].Visible != bVisible)
                {
                    mControlUcProcessItems[i].Visible = bVisible;
                }
                if (bVisible == false)
                {
                    continue;
                }

                SetButtonText(mControlUcProcessItems[i].BtnCellData, mProcessItems[index].Name);
                switch (mProcessItems[index].Process.GetInitializeStatus())
                {
                    case CProcessAbstract.EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_NONE:
                        SetButtonBackColor(mControlUcProcessItems[i].BtnIndicator, m_objInitializeColor.colorWaiting);
                        SetButtonText(mControlUcProcessItems[i].BtnIndicator, mResourceProcessStateWaiting);
                        processIndicatorColor = m_objInitializeColor.colorWaiting;
                        break;

                    case CProcessAbstract.EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_START:
                        SetButtonBackColor(mControlUcProcessItems[i].BtnIndicator, m_objInitializeColor.colorInitializing);
                        SetButtonText(mControlUcProcessItems[i].BtnIndicator, mResourceProcessStateInitializing);
                        processIndicatorColor = m_objInitializeColor.colorWaiting;
                        break;

                    case CProcessAbstract.EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_DONE:
                        SetButtonBackColor(mControlUcProcessItems[i].BtnIndicator, m_objInitializeColor.colorComplete);
                        SetButtonText(mControlUcProcessItems[i].BtnIndicator, mResourceProcessStateComplete);
                        break;

                    case CProcessAbstract.EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_ERROR:
                        SetButtonBackColor(mControlUcProcessItems[i].BtnIndicator, m_objInitializeColor.colorError);
                        SetButtonText(mControlUcProcessItems[i].BtnIndicator, mResourceProcessStateError);
                        processIndicatorColor = m_objInitializeColor.colorWaiting;
                        break;
                }
            }
            SetControlText(LblNaviProcessPage, $"( {mProcessPageIndex + 1} / {mProcessPageCount} )");
            SetButtonBackColor(BtnTitleProcess.BtnIndicator, processIndicatorColor);
        }

        /// <summary>
        /// 점검 그룹 아이템 선택
        /// </summary>
        /// <param name="itemIndex">그룹 아이템 인덱스</param>
        private void SetCheckGroupItemIndex(int itemIndex)
        {
            Utils.InRange(ref itemIndex, 0, mCheckGroupItems.Count - 1);
            mCheckGroupIndex = itemIndex;

            SetCheckPageIndex(0);
        }

        /// <summary>
        /// 점검 아이템 페이지 선택
        /// </summary>
        /// <param name="pageIndex">페이지 인덱스</param>
        private void SetCheckPageIndex(int pageIndex)
        {
            int pageDisplayItemCount = mControlUcCheckItems.Length;
            int itemCount = mCheckGroupItems[mCheckGroupIndex].CheckItems.Count;
            mCheckPageCount = itemCount / pageDisplayItemCount;
            if (itemCount % pageDisplayItemCount != 0)
            {
                mCheckPageCount++;
            }

            Utils.InRange(ref pageIndex, 0, mCheckPageCount - 1);

            mCheckPageIndex = pageIndex;
        }

        /// <summary>
        /// 프로세스 아이템 페이지 선택
        /// </summary>
        /// <param name="pageIndex">페이지 인덱스</param>
        private void SetProcessPageIndex(int pageIndex)
        {
            int pageDisplayItemCount = mControlUcProcessItems.Length;
            int itemCount = mProcessItems.Count;
            mProcessPageCount = itemCount / pageDisplayItemCount;
            if (itemCount % pageDisplayItemCount != 0)
            {
                mProcessPageCount++;
            }

            Utils.InRange(ref pageIndex, 0, mProcessPageCount - 1);

            mProcessPageIndex = pageIndex;
        }

        private void BtnNaviCheckItemFirst_Click(object sender, EventArgs e)
        {
            SetCheckPageIndex(0);
        }

        private void BtnNaviCheckItemPrivious_Click(object sender, EventArgs e)
        {
            int setIndex = mCheckPageIndex - 1;
            SetCheckPageIndex(setIndex);
        }

        private void BtnNaviCheckItemNext_Click(object sender, EventArgs e)
        {
            int setIndex = mCheckPageIndex + 1;
            SetCheckPageIndex(setIndex);
        }

        private void BtnNaviCheckItemLast_Click(object sender, EventArgs e)
        {
            SetCheckPageIndex(mCheckPageCount);
        }

        private void BtnNaviProcessFirst_Click(object sender, EventArgs e)
        {
            SetProcessPageIndex(0);
        }

        private void BtnNaviProcessPrivious_Click(object sender, EventArgs e)
        {
            int setIndex = mProcessPageIndex - 1;
            SetProcessPageIndex(setIndex);
        }

        private void BtnNaviProcessNext_Click(object sender, EventArgs e)
        {
            int setIndex = mProcessPageIndex + 1;
            SetProcessPageIndex(setIndex);
        }

        private void BtnNaviProcessLast_Click(object sender, EventArgs e)
        {
            SetProcessPageIndex(mProcessPageCount);
        }

        private void ucCellCheckGroupItem_Click(object sender, EventArgs e)
        {
            SetCheckGroupItemIndex(Convert.ToInt32(((Control)sender).Tag));
        }
    }
}