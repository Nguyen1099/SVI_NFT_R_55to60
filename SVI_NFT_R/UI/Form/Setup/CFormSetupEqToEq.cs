using EqToEq;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormSetupEqToEq : CFormCommon, CFormInterface
    {
        private enum EDataColumn
        {
            Name = 0,
            Value
        }
        private const int PAGE_BIT_COUNT = 16;
        private const int PAGE_DATA_COUNT = 16;
        private readonly CDocument m_objDocument;
        private Control[] mNavigationButtons;
        private PagingHandler<EqToEqUiOneBitSet> mLeftBitsPagingHandler;
        private PagingHandler<EqToEqUiOneDataSet> mLeftDatasPagingHandler;
        private PagingHandler<EqToEqUiOneBitSet> mRightBitsPagingHandler;
        private PagingHandler<EqToEqUiOneDataSet> mRightDatasPagingHandler;
        private readonly ImageButton[] mSelectGroupButtons = new ImageButton[Enum.GetNames(typeof(EEqToEqUiGroup)).Length];
        private readonly SpeedButton[] mLeftBitsButtons = new SpeedButton[PAGE_BIT_COUNT];
        private readonly SpeedButton[] mRightBitsButtons = new SpeedButton[PAGE_BIT_COUNT];
        private readonly Color[] mReadOnlyBitColors;
        private readonly Color[] mBitColors;
        private bool mbOutputLock = false;
        private EEqToEqUiGroup mSelectGroupIndex = 0;

        public CFormSetupEqToEq(CDocument document)
        {
            mBitColors = new Color[]
            {
                m_colorOutputOff,
                m_colorOutputOn
            };
            mReadOnlyBitColors = new Color[]
            {
                m_colorInputOff,
                m_colorInputOn
            };
            m_objDocument = document;
            InitializeComponent();
        }

        public bool Initialize()
        {
            if (InitializeForm() == false)
            {
                return false;
            }
            return true;
        }

        public void DeInitialize()
        {
        }

        public bool SetChangeLanguage()
        {
            SetButtonText(BtnTitleLeft, m_objDocument.GetDatabaseUIText($"{nameof(BtnTitleLeft)}[{mSelectGroupIndex}]", Name));
            SetButtonText(BtnTitleRight, m_objDocument.GetDatabaseUIText($"{nameof(BtnTitleRight)}[{mSelectGroupIndex}]", Name));
            SetButtonChangeLanguage(BtnOutputLock);
            SetButtonChangeLanguage(BtnTitleSelectGroup);
            for (int i = 0; i < mSelectGroupButtons.Length; ++i)
            {
                EEqToEqUiGroup index = (EEqToEqUiGroup)mSelectGroupButtons[i].Tag;
                SetButtonText(mSelectGroupButtons[i], m_objDocument.GetDatabaseUIText($"{nameof(BtnSelectGroupBase)}[{index}]", Name));
            }
            return true;
        }

        public void SetTimer(bool bTimer)
        {
            Timer.Enabled = bTimer;
        }

        public void SetVisible(bool bVisible)
        {
            Visible = bVisible;

            if (bVisible == true)
            {
                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                m_objDocument.GetMainFrame().SetCurrentForm(this);
                // 화면 전환시 LOCK 상태로 변경
                SetOutputLock(true);
                // 화면 전환시 UI 강제 업데이트
                SetGroupIndex(mSelectGroupIndex, bForceUpdate: true);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Initialize();
            InitializeAutoScale();
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            DeInitialize();
        }

        private bool InitializeForm()
        {
            mNavigationButtons = new Control[]
            {
                BtnLeftBitsPageFirst,
                BtnLeftBitsPagePrevious,
                BtnLeftBitsPageNext,
                BtnLeftBitsPageLast,
                BtnLeftDatasPageFirst,
                BtnLeftDatasPagePrevious,
                BtnLeftDatasPageNext,
                BtnLeftDatasPageLast,
                BtnRightBitsPageFirst,
                BtnRightBitsPagePrevious,
                BtnRightBitsPageNext,
                BtnRightBitsPageLast,
                BtnRightDatasPageFirst,
                BtnRightDatasPagePrevious,
                BtnRightDatasPageNext,
                BtnRightDatasPageLast
            };

            // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
            m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);

            // 버튼 생성 및 초기화
            PnlLeftBits.SuspendLayout();
            PnlRightBits.SuspendLayout();
            PnlSelectGroup.SuspendLayout();
            for (int i = 0; i < PAGE_BIT_COUNT; ++i)
            {
                mLeftBitsButtons[i] = CreateAndAddNewSpeedButton(BtnLeftBitBase, PnlLeftBits, $"{nameof(BtnLeftBitBase)}{i}", bIsFlowBreak: true);
                mLeftBitsButtons[i].Click += BtnLeftBitBase_Click;
                mLeftBitsButtons[i].Tag = i;
                mRightBitsButtons[i] = CreateAndAddNewSpeedButton(BtnRightBitBase, PnlRightBits, $"{nameof(BtnRightBitBase)}{i}", bIsFlowBreak: true);
                mRightBitsButtons[i].Click += BtnRightBitBase_Click;
                mRightBitsButtons[i].Tag = i;
            }
            for (int i = 0; i < mSelectGroupButtons.Length; ++i)
            {
                mSelectGroupButtons[i] = CreateAndAddNewImageButton(BtnSelectGroupBase, PnlSelectGroup, $"{nameof(BtnSelectGroupBase)}{i}", bIsFlowBreak: true);
                mSelectGroupButtons[i].Click += BtnSelectGroupBase_Click;
                mSelectGroupButtons[i].Tag = i;
            }
            PnlLeftBits.ResumeLayout(false);
            PnlRightBits.ResumeLayout(false);
            PnlSelectGroup.ResumeLayout(false);

            // 데이터 영역 그리드 초기화
            InitializeGridView(DgvLeftDatas);
            InitializeGridView(DgvRightDatas);

            // 버튼 색상 정의
            SetButtonColor();

            // 첫 번째 그룹 선택
            SetGroupIndex(0, bForceUpdate: true);

            // 타이머 외부에서 제어
            Timer.Interval = 100;
            Timer.Enabled = false;

            return true;
        }

        private void SetButtonColor()
        {
            // 버튼 색 변경
            SetButtonBackColor(BtnTitleLeft, m_colorLabel);
            SetButtonBackColor(BtnTitleRight, m_colorLabel);
            SetButtonBackColor(BtnTitleSelectGroup, m_colorLabel);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.StartsWith(nameof(BtnLeftBitBase))
                    && false == btn.Name.StartsWith(nameof(BtnRightBitBase))
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
                SetControlButtonEnable(Controls, m_objDocument.IsMasterLogin == true && CanEditReadOnlySignals() == true);
                mbOutputLock = true;
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        SetControlButtonEnable(Controls, false);
                        mbOutputLock = true;
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        SetControlButtonEnable(Controls, false);
                        mbOutputLock = true;
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        SetControlButtonEnable(Controls, true);
                        break;
                    default:
                        break;
                }
            }

            foreach (var control in mNavigationButtons)
            {
                control.Enabled = true;
            }
            foreach (var control in mSelectGroupButtons)
            {
                control.Enabled = true;
            }
            SetOutputLock(mbOutputLock);
        }

        private void SetButtonChangeLanguage(Button objButton)
        {
            SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        private new bool InitializeGridView(DataGridView dgv)
        {
            // 그리드 뷰 기본 스타일 초기화
            if (base.InitializeGridView(dgv) == false)
            {
                return false;
            }
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            // 그리드 뷰 ReadOnly
            dgv.ReadOnly = true;
            // 그리드 뷰 다중 선택 x
            dgv.MultiSelect = false;
            // 그리드 뷰 선택 모드 (행 전체 선택)
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // 그리드 뷰 칼럼 추가
            foreach (EDataColumn index in Enum.GetValues(typeof(EDataColumn)))
            {
                dgv.Columns.Add($"{index}", $"{index}".ToUpper());
                // 칼럼 정렬 기능 x
                dgv.Columns[(int)index].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            // SVID, VALUE 사이즈 조정
            dgv.Columns[(int)EDataColumn.Name].Width = (int)(dgv.Width * 0.7);
            dgv.Columns[(int)EDataColumn.Value].Width = (int)(dgv.Width * 0.3);
            // 그리드 뷰 크기 변경
            dgv.Font = new Font("Consolas", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            return true;
        }

        private SpeedButton CreateAndAddNewSpeedButton(SpeedButton baseButton, FlowLayoutPanel container, string name, bool bIsFlowBreak)
        {
            SpeedButton addButton = new SpeedButton()
            {
                Name = name,
                Margin = baseButton.Margin,
                Size = baseButton.Size,
                Font = baseButton.Font,
                TextAlign = baseButton.TextAlign
            };
            container.Controls.Add(addButton);
            container.SetFlowBreak(addButton, bIsFlowBreak);
            return addButton;
        }

        private ImageButton CreateAndAddNewImageButton(ImageButton baseButton, FlowLayoutPanel container, string name, bool bIsFlowBreak)
        {
            ImageButton addButton = new ImageButton()
            {
                Name = name,
                Margin = baseButton.Margin,
                Size = baseButton.Size,
                Font = baseButton.Font,
                TextAlign = baseButton.TextAlign,
                CornerRadius = baseButton.CornerRadius,
                CornerRound = baseButton.CornerRound
            };
            container.Controls.Add(addButton);
            container.SetFlowBreak(addButton, bIsFlowBreak);
            return addButton;
        }

        private void ButtonLog(string message, [CallerMemberName] string callerMemberName = "")
        {
            m_objDocument.SetUpdateButtonLog(this, $"[{callerMemberName}] {message}");
        }

        private void SetGroupIndex(EEqToEqUiGroup index, bool bForceUpdate)
        {
            if (bForceUpdate == false
                && mSelectGroupIndex == index
                )
            {
                return;
            }
            mSelectGroupIndex = index;

            // 선택한 버튼 색상 업데이트
            int selectGroupIndex = (int)index;
            for (int i = 0; i < mSelectGroupButtons.Length; ++i)
            {
                SetButtonBackColor(mSelectGroupButtons[i], Equals(mSelectGroupButtons[i].Tag, selectGroupIndex) ? m_colorClick : m_colorNormal);
            }

            if (mLeftBitsPagingHandler != null
                || mLeftDatasPagingHandler != null
                || mRightBitsPagingHandler != null
                || mRightDatasPagingHandler != null
                )
            {
                mLeftBitsPagingHandler.OnPageIndexChanged -= LeftBitsPagingHandler_OnPageIndexChanged;
                mLeftDatasPagingHandler.OnPageIndexChanged -= LeftDatasPagingHandler_OnPageIndexChanged;
                mRightBitsPagingHandler.OnPageIndexChanged -= RightBitsPagingHandler_OnPageIndexChanged;
                mRightDatasPagingHandler.OnPageIndexChanged -= RightDatasPagingHandler_OnPageIndexChanged;
            }

            mLeftBitsPagingHandler = new PagingHandler<EqToEqUiOneBitSet>(EqToEqUiDataManager.Groups[mSelectGroupIndex].LeftBits, PAGE_BIT_COUNT);
            mLeftDatasPagingHandler = new PagingHandler<EqToEqUiOneDataSet>(EqToEqUiDataManager.Groups[mSelectGroupIndex].LeftDatas, PAGE_DATA_COUNT);
            mRightBitsPagingHandler = new PagingHandler<EqToEqUiOneBitSet>(EqToEqUiDataManager.Groups[mSelectGroupIndex].RightBits, PAGE_BIT_COUNT);
            mRightDatasPagingHandler = new PagingHandler<EqToEqUiOneDataSet>(EqToEqUiDataManager.Groups[mSelectGroupIndex].RightDatas, PAGE_DATA_COUNT);
            mLeftBitsPagingHandler.OnPageIndexChanged += LeftBitsPagingHandler_OnPageIndexChanged;
            mLeftDatasPagingHandler.OnPageIndexChanged += LeftDatasPagingHandler_OnPageIndexChanged;
            mRightBitsPagingHandler.OnPageIndexChanged += RightBitsPagingHandler_OnPageIndexChanged;
            mRightDatasPagingHandler.OnPageIndexChanged += RightDatasPagingHandler_OnPageIndexChanged;

            SetChangeLanguage();
            UpdateLeftBitsAddressInformations();
            UpdateRightBitsAddressInformations();
            UpdateLeftDatasAddressInformations();
            UpdateRightDatasAddressInformations();
            UpdateLeftBitsValues();
            UpdateRightBitsValues();
            UpdateLeftDatasValues();
            UpdateRightDatasValues();
        }

        private void SetOutputLock(bool bLock)
        {
            mbOutputLock = bLock;
            UpdateLeftBitsAddressInformations();
            UpdateRightBitsAddressInformations();
            UpdateLeftDatasAddressInformations();
            UpdateRightDatasAddressInformations();
        }

        private void UpdateLeftBitsAddressInformations()
        {
            UpdateBitsAddressInformations(mLeftBitsButtons, mLeftBitsPagingHandler);
        }

        private void UpdateRightBitsAddressInformations()
        {
            UpdateBitsAddressInformations(mRightBitsButtons, mRightBitsPagingHandler);
        }

        private void UpdateLeftDatasAddressInformations()
        {
            UpdateDatasAddressInformations(DgvLeftDatas, mLeftDatasPagingHandler);
        }

        private void UpdateRightDatasAddressInformations()
        {
            UpdateDatasAddressInformations(DgvRightDatas, mRightDatasPagingHandler);
        }

        private void UpdateLeftBitsValues()
        {
            UpdateBitsValues(mLeftBitsButtons, mLeftBitsPagingHandler);
        }

        private void UpdateRightBitsValues()
        {
            UpdateBitsValues(mRightBitsButtons, mRightBitsPagingHandler);
        }

        private void UpdateLeftDatasValues()
        {
            UpdateDatasValues(DgvLeftDatas, mLeftDatasPagingHandler);
        }

        private void UpdateRightDatasValues()
        {
            UpdateDatasValues(DgvRightDatas, mRightDatasPagingHandler);
        }

        private void UpdateBitsAddressInformations(SpeedButton[] buttons, PagingHandler<EqToEqUiOneBitSet> pagingHandler)
        {
            bool bCanEditReadOnlySignal = CanEditReadOnlySignals();
            for (int i = 0; i < buttons.Length; ++i)
            {
                buttons[i].Visible = i < pagingHandler.PageItems.Count;
                if (buttons[i].Visible == false)
                {
                    continue;
                }
                SetButtonText(buttons[i], $"[{pagingHandler.PageItems[i].Label}] {pagingHandler.PageItems[i].Name}");
                buttons[i].Enabled = mbOutputLock == false && (pagingHandler.PageItems[i].IsReadOnly == false || bCanEditReadOnlySignal == true);
            }
        }

        private void UpdateDatasAddressInformations(DataGridView dgvData, PagingHandler<EqToEqUiOneDataSet> pagingHandler)
        {
            bool bCanEditReadOnlySignal = CanEditReadOnlySignals();
            int rowHeight = (dgvData.Height - dgvData.ColumnHeadersHeight) / PAGE_DATA_COUNT;
            dgvData.Rows.Clear();
            for (int i = 0; i < pagingHandler.PageItems.Count; ++i)
            {
                dgvData.Rows.Add($"[{pagingHandler.PageItems[i].Label}] {pagingHandler.PageItems[i].Name}", "");
                dgvData.Rows[i].Height = rowHeight;
            }
            dgvData.Enabled = mbOutputLock == false && (pagingHandler.PageItems[0].IsReadOnly == false || bCanEditReadOnlySignal == true);
            if (dgvData.Enabled == false)
            {
                dgvData.ClearSelection();
            }
        }

        private bool CanEditReadOnlySignals()
        {
            switch (m_objDocument.GetRunMode())
            {
                case CDefine.ERunMode.RealRun:
                    if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON
                        || m_objDocument.m_objConfig.GetCCLinkIEParameter().bRunSimulationMode == true
                        )
                    {
                        // 설비 간 인터페이스 신호가 가상 영역으로 전환된 상태임으로 읽기 전용영역 편집 가능
                        return true;
                    }
                    break;

                case CDefine.ERunMode.UnlinkDryrun:
                    // 설비 간 인터페이스 신호가 가상 영역으로 전환된 상태임으로 읽기 전용영역 편집 가능
                    return true;

                default:
                    Debug.Assert(false);
                    break;
            }
            return false;
        }

        private void UpdateBitsValues(SpeedButton[] buttons, PagingHandler<EqToEqUiOneBitSet> pagingHandler)
        {
            for (int i = 0; i < buttons.Length; ++i)
            {
                if (buttons[i].Visible == false)
                {
                    continue;
                }
                Color[] backColors = pagingHandler.PageItems[i].IsReadOnly ? mReadOnlyBitColors : mBitColors;
                SetButtonBackColor(buttons[i], backColors[Convert.ToInt32(pagingHandler.PageItems[i].Value)]);
            }
        }

        private void UpdateDatasValues(DataGridView dgvData, PagingHandler<EqToEqUiOneDataSet> pagingHandler)
        {
            for (int i = 0; i < pagingHandler.PageItems.Count; ++i)
            {
                string readValue = pagingHandler.PageItems[i].Value;
                if (Equals(dgvData.Rows[i].Cells[(int)EDataColumn.Value].Value, readValue) == true)
                {
                    continue;
                }
                dgvData.Rows[i].Cells[(int)EDataColumn.Value].Value = readValue;
            }
        }

        private void LeftBitsPagingHandler_OnPageIndexChanged(object sender, EventArgs e)
        {
            UpdateLeftBitsAddressInformations();
            UpdateLeftBitsValues();
        }

        private void LeftDatasPagingHandler_OnPageIndexChanged(object sender, EventArgs e)
        {
            UpdateLeftDatasAddressInformations();
            UpdateLeftDatasValues();
        }

        private void RightBitsPagingHandler_OnPageIndexChanged(object sender, EventArgs e)
        {
            UpdateRightBitsAddressInformations();
            UpdateRightBitsValues();
        }

        private void RightDatasPagingHandler_OnPageIndexChanged(object sender, EventArgs e)
        {
            UpdateRightDatasAddressInformations();
            UpdateRightDatasValues();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 데이터 갱신
            {
                UpdateLeftBitsValues();
                UpdateLeftDatasValues();
                UpdateRightBitsValues();
                UpdateRightDatasValues();

                SetControlText(BtnLeftBitsPage, $"( {mLeftBitsPagingHandler} )");
                SetControlText(BtnLeftDatasPage, $"( {mLeftDatasPagingHandler} )");
                SetControlText(BtnRightBitsPage, $"( {mRightBitsPagingHandler} )");
                SetControlText(BtnRightDatasPage, $"( {mRightDatasPagingHandler} )");
            }

            // IO 잠금 버튼 업데이트
            {
                SetButtonBackColor(BtnOutputLock, (mbOutputLock == true) ? m_colorOn : m_colorNormal, (mbOutputLock == true));
            }
        }

        private void BtnOutputLock_Click(object sender, EventArgs e)
        {
            SetOutputLock(!mbOutputLock);
            ButtonLog($"[LockMode: {mbOutputLock}]");
        }

        private void BtnLeftBitsPageFirst_Click(object sender, EventArgs e)
        {
            mLeftBitsPagingHandler.SetFirstPage();
            ButtonLog($"PageIndex: {mLeftBitsPagingHandler.PageIndex}");
        }

        private void BtnLeftBitsPagePrevious_Click(object sender, EventArgs e)
        {
            mLeftBitsPagingHandler.SetPreviousPage();
            ButtonLog($"PageIndex: {mLeftBitsPagingHandler.PageIndex}");
        }

        private void BtnLeftBitsPageNext_Click(object sender, EventArgs e)
        {
            mLeftBitsPagingHandler.SetNextPage();
            ButtonLog($"PageIndex: {mLeftBitsPagingHandler.PageIndex}");
        }

        private void BtnLeftBitsPageLast_Click(object sender, EventArgs e)
        {
            mLeftBitsPagingHandler.SetLastPage();
            ButtonLog($"PageIndex: {mLeftBitsPagingHandler.PageIndex}");
        }

        private void BtnLeftDatasPageFirst_Click(object sender, EventArgs e)
        {
            mLeftDatasPagingHandler.SetFirstPage();
            ButtonLog($"PageIndex: {mLeftDatasPagingHandler.PageIndex}");
        }

        private void BtnLeftDatasPagePrevious_Click(object sender, EventArgs e)
        {
            mLeftDatasPagingHandler.SetPreviousPage();
            ButtonLog($"PageIndex: {mLeftDatasPagingHandler.PageIndex}");
        }

        private void BtnLeftDatasPageNext_Click(object sender, EventArgs e)
        {
            mLeftDatasPagingHandler.SetNextPage();
            ButtonLog($"PageIndex: {mLeftDatasPagingHandler.PageIndex}");
        }

        private void BtnLeftDatasPageLast_Click(object sender, EventArgs e)
        {
            mLeftDatasPagingHandler.SetLastPage();
            ButtonLog($"PageIndex: {mLeftDatasPagingHandler.PageIndex}");
        }

        private void BtnRightBitsPageFirst_Click(object sender, EventArgs e)
        {
            mRightBitsPagingHandler.SetFirstPage();
            ButtonLog($"PageIndex: {mRightBitsPagingHandler.PageIndex}");
        }

        private void BtnRightBitsPagePrevious_Click(object sender, EventArgs e)
        {
            mRightBitsPagingHandler.SetPreviousPage();
            ButtonLog($"PageIndex: {mRightBitsPagingHandler.PageIndex}");
        }

        private void BtnRightBitsPageNext_Click(object sender, EventArgs e)
        {
            mRightBitsPagingHandler.SetNextPage();
            ButtonLog($"PageIndex: {mRightBitsPagingHandler.PageIndex}");
        }

        private void BtnRightBitsPageLast_Click(object sender, EventArgs e)
        {
            mRightBitsPagingHandler.SetLastPage();
            ButtonLog($"PageIndex: {mRightBitsPagingHandler.PageIndex}");
        }

        private void BtnRightDatasPageFirst_Click(object sender, EventArgs e)
        {
            mRightDatasPagingHandler.SetFirstPage();
            ButtonLog($"PageIndex: {mRightDatasPagingHandler.PageIndex}");
        }

        private void BtnRightDatasPagePrevious_Click(object sender, EventArgs e)
        {
            mRightDatasPagingHandler.SetPreviousPage();
            ButtonLog($"PageIndex: {mRightDatasPagingHandler.PageIndex}");
        }

        private void BtnRightDatasPageNext_Click(object sender, EventArgs e)
        {
            mRightDatasPagingHandler.SetNextPage();
            ButtonLog($"PageIndex: {mRightDatasPagingHandler.PageIndex}");
        }

        private void BtnRightDatasPageLast_Click(object sender, EventArgs e)
        {
            mRightDatasPagingHandler.SetLastPage();
            ButtonLog($"PageIndex: {mRightDatasPagingHandler.PageIndex}");
        }

        private void BtnLeftBitBase_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(((Control)sender).Tag);
            bool setValue = mLeftBitsPagingHandler.PageItems[index].Toggle();
            ButtonLog($"{mLeftBitsPagingHandler.PageItems[index].Name}[{mLeftBitsPagingHandler.PageItems[index].Label}].Toggle({setValue})");
        }

        private void BtnRightBitBase_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(((Control)sender).Tag);
            bool setValue = mRightBitsPagingHandler.PageItems[index].Toggle();
            ButtonLog($"{mRightBitsPagingHandler.PageItems[index].Name}[{mRightBitsPagingHandler.PageItems[index].Label}].Toggle({setValue})");
        }

        private void BtnSelectGroupBase_Click(object sender, EventArgs e)
        {
            EEqToEqUiGroup index = (EEqToEqUiGroup)Convert.ToInt32(((Control)sender).Tag);
            SetGroupIndex(index, bForceUpdate: false);
            ButtonLog($"SelectedGroup: {index}");
        }

        private void DgvLeftDatas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;
            if (dgv.SelectedRows.Count == 0)
            {
                return;
            }
            int index = dgv.SelectedRows[0].Index;
            if (TryEditData(mLeftDatasPagingHandler, index) == false)
            {
                return;
            }
            ButtonLog($"");
        }

        private void DgvRightDatas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;
            if (dgv.SelectedRows.Count == 0)
            {
                return;
            }
            int index = dgv.SelectedRows[0].Index;
            if (TryEditData(mRightDatasPagingHandler, index) == false)
            {
                return;
            }
            ButtonLog($"");
        }

        private bool TryEditData(PagingHandler<EqToEqUiOneDataSet> pagingHandler, int index, [CallerMemberName] string callerMemberName = "")
        {
            string readValue = pagingHandler.PageItems[index].Value;

            if (EqToEqUiOneDataSet.IsStringType(pagingHandler.PageItems[index].HandlingData) == true)
            {
                using (var dialog = new FormKeyBoard(readValue))
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        ButtonLog($"StringEditDataCancel", callerMemberName);
                        return false;
                    }

                    pagingHandler.PageItems[index].Value = dialog.m_strReturnValue;
                    ButtonLog($"StringEditData: [{pagingHandler.PageItems[index].Label}]{pagingHandler.PageItems[index].Name}.Value = {dialog.m_strReturnValue}", callerMemberName);
                }
                return true;
            }

            if (EqToEqUiOneDataSet.IsIntegerType(pagingHandler.PageItems[index].HandlingData) == true
                || EqToEqUiOneDataSet.IsRealType(pagingHandler.PageItems[index].HandlingData) == true
                )
            {
                double.TryParse(readValue, out double originValue);
                using (var dialog = new FormKeyPad(originValue, pagingHandler.PageItems[index].DataRangeMin, pagingHandler.PageItems[index].DataRangeMax))
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        ButtonLog($"DecimalEditDataCancel", callerMemberName);
                        return false;
                    }

                    pagingHandler.PageItems[index].SetDoubleValue(dialog.m_dValue);
                    ButtonLog($"DecimalEditData: [{pagingHandler.PageItems[index].Label}]{pagingHandler.PageItems[index].Name}.Value = {dialog.m_dValue}", callerMemberName);
                }
                return true;
            }

            return false;
        }
    }
}