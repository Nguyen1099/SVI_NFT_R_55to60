using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormConfigRecipe : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 선택된 알람 리스트 행
        /// </summary>
        private int m_iSelectedRow = 0;
        /// <summary>
        /// 모델 파라미터 리스트
        /// </summary>
        private List<CConfig.CModelParameter> m_objModelParameterList;
        /// <summary>
        /// 모델 리스트 칼럼 정의
        /// </summary>
        private enum EModelListColumn
        {
            IDX = 0,
            PPID,
            MODEL_LIST_COLUMN_FINAL
        };
        private CConfig.CModelParameter mSelectModel;
        private readonly List<ParameterItem> mParameterItems;
        private int mCurrentPage = 0;
        private readonly int mMaxPage;
        private const int PAGE_DISPLAY_ITEM_COUNT = 10;
        private const bool LOCK_JAVAS_PARAMS = false;
        private string mResourceCreateOffsetOptionTitle = "";
        private string mResourceCreateOffsetOption1 = "";
        private string mResourceCreateOffsetOption2 = "";
        private string mResourceDeleteOffsetOptionTitle = "";
        private string mResourceDeleteOffsetOption1 = "";
        private string mResourceDeleteOffsetOption2 = "";
        private string mResourceCommonOffsetOptionCancel = "";
        private int mCreatePpidNumber = CModel.RANGE_PPID_SHARE_START_NUMBER;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormConfigRecipe(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            InitializeComponent();

            mParameterItems = new List<ParameterItem>()
            {
                new ParameterItem(this, CCIMDefine.EPpidParamList.CELL_WIDTH, typeof(double), new Action<Form, string>(m_objDocument.SetUpdateButtonLog)),
                new ParameterItem(this, CCIMDefine.EPpidParamList.CELL_HEIGHT, typeof(double), new Action<Form, string>(m_objDocument.SetUpdateButtonLog)),
            };
            mMaxPage = mParameterItems.Count / PAGE_DISPLAY_ITEM_COUNT + 1;
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
        private void CFormConfigRecipe_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormConfigRecipe_FormClosed(object sender, FormClosedEventArgs e)
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
                // 모델 리스트 그리드 뷰 초기화
                //string[] strModelList = { enumModelListColumn.PPID.ToString(), enumModelListColumn.NAME.ToString() };
                string[] strModelList = { EModelListColumn.IDX.ToString(), EModelListColumn.PPID.ToString() };
                if (false == InitializeGridView(this.GridViewRecipeList, strModelList))
                {
                    break;
                }
                // 버튼 색상 정의
                SetButtonColor();
                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            base.SetButtonBackColor(this.BtnTitleRecipeList, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleRecipeInfo, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleSelectedPPID, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnSelectedPPID, m_colorLabelData);
            base.SetButtonBackColor(this.BtnSave, m_colorNormal);
            base.SetButtonBackColor(this.BtnLoad, m_colorNormal);
            base.SetButtonBackColor(this.BtnDelete, m_colorNormal);
            base.SetButtonBackColor(this.BtnCreate, m_colorNormal);
            base.SetButtonBackColor(this.BtnTitleCreatePPID, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleCreatePpidNumber, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitlePPID, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleParamName01, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleParamName02, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleParamName03, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleParamName04, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleParamName05, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleParamName06, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleParamName07, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleParamName08, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleParamName09, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleParamName10, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnCreatePPID, m_colorLabelData);
            base.SetButtonBackColor(this.BtnCreatePpidNumber, m_colorLabelData);
            base.SetButtonBackColor(this.BtnPPID, m_colorLabelData);
            base.SetButtonBackColor(this.BtnParamValue01, m_colorLabelData);
            base.SetButtonBackColor(this.BtnParamValue02, m_colorLabelData);
            base.SetButtonBackColor(this.BtnParamValue03, m_colorLabelData);
            base.SetButtonBackColor(this.BtnParamValue04, m_colorLabelData);
            base.SetButtonBackColor(this.BtnParamValue05, m_colorLabelData);
            base.SetButtonBackColor(this.BtnParamValue06, m_colorLabelData);
            base.SetButtonBackColor(this.BtnParamValue07, m_colorLabelData);
            base.SetButtonBackColor(this.BtnParamValue08, m_colorLabelData);
            base.SetButtonBackColor(this.BtnParamValue09, m_colorLabelData);
            base.SetButtonBackColor(this.BtnParamValue10, m_colorLabelData);
            base.SetButtonBackColor(this.BtnTitleName, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleValue, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNo, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNo01, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNo02, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNo03, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNo04, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNo05, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNo06, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNo07, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNo08, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNo09, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnTitleNo10, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnCurrentPage, m_colorLabelData);

            base.SetButtonBackColor(this.BtnFirstPage, m_colorNormal);
            base.SetButtonBackColor(this.BtnPriviousPage, m_colorNormal);
            base.SetButtonBackColor(this.BtnNextPage, m_colorNormal);
            base.SetButtonBackColor(this.BtnLastPage, m_colorNormal);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnCreatePpidNumber.Name)
                    && false == btn.Name.Equals(BtnCreatePPID.Name)
                    && false == btn.Name.Equals(BtnSelectedPPID.Name)
                    && false == btn.Name.Equals(BtnParamValue01.Name)
                    && false == btn.Name.Equals(BtnParamValue02.Name)
                    && false == btn.Name.Equals(BtnParamValue03.Name)
                    && false == btn.Name.Equals(BtnParamValue04.Name)
                    && false == btn.Name.Equals(BtnParamValue05.Name)
                    && false == btn.Name.Equals(BtnParamValue06.Name)
                    && false == btn.Name.Equals(BtnParamValue07.Name)
                    && false == btn.Name.Equals(BtnParamValue08.Name)
                    && false == btn.Name.Equals(BtnParamValue09.Name)
                    && false == btn.Name.Equals(BtnParamValue10.Name)
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
            // 이 부분은 해당 데이터에서 처리하도록 변경
            if (m_iSelectedRow >= m_objModelParameterList.Count)
            {
                m_iSelectedRow = m_objModelParameterList.Count - 1;
            }
            // 일부분 버튼은 살리고 막아야 하기 때문..
            SetModelListData(m_iSelectedRow);
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
                SetButtonChangeLanguage(this.BtnTitleRecipeList);
                SetButtonChangeLanguage(this.BtnTitleRecipeInfo);
                SetButtonChangeLanguage(this.BtnTitleSelectedPPID);
                SetButtonChangeLanguage(this.BtnSave);
                SetButtonChangeLanguage(this.BtnLoad);
                SetButtonChangeLanguage(this.BtnDelete);
                SetButtonChangeLanguage(this.BtnCreate);
                SetButtonChangeLanguage(this.BtnTitleCreatePPID);
                SetButtonChangeLanguage(this.BtnTitleCreatePpidNumber);
                SetButtonChangeLanguage(this.BtnTitlePPID);
                SetButtonChangeLanguage(this.BtnFirstPage);
                SetButtonChangeLanguage(this.BtnPriviousPage);
                SetButtonChangeLanguage(this.BtnNextPage);
                SetButtonChangeLanguage(this.BtnLastPage);

                mResourceCreateOffsetOptionTitle = m_objDocument.GetDatabaseUIText(nameof(mResourceCreateOffsetOptionTitle), Name);
                mResourceCreateOffsetOption1 = m_objDocument.GetDatabaseUIText(nameof(mResourceCreateOffsetOption1), Name);
                mResourceCreateOffsetOption2 = m_objDocument.GetDatabaseUIText(nameof(mResourceCreateOffsetOption2), Name);
                mResourceDeleteOffsetOptionTitle = m_objDocument.GetDatabaseUIText(nameof(mResourceDeleteOffsetOptionTitle), Name);
                mResourceDeleteOffsetOption1 = m_objDocument.GetDatabaseUIText(nameof(mResourceDeleteOffsetOption1), Name);
                mResourceDeleteOffsetOption2 = m_objDocument.GetDatabaseUIText(nameof(mResourceDeleteOffsetOption2), Name);
                mResourceCommonOffsetOptionCancel = m_objDocument.GetDatabaseUIText(nameof(mResourceCommonOffsetOptionCancel), Name);

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
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, this.Name));
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
            this.Visible = bVisible;

            if (true == bVisible)
            {
                // 모델 파라미터 리스트 갱신
                //변경 후 다른 탭으로 이동 후 복귀시 업데이트 안되는 버그 수정
                m_objModelParameterList = m_objDocument.m_objModel.GetModelParameterList();
                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                m_objDocument.GetMainFrame().SetCurrentForm(this);

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
                    break;
                // 그리드 뷰 ReadOnly
                objGridView.ReadOnly = true;
                // 그리드 뷰 다중 선택 x
                objGridView.MultiSelect = false;
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                objGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                // 그리드 뷰 칼럼 추가
                for (int iLoopColumn = 0; iLoopColumn < strColumnName.Length; iLoopColumn++)
                {
                    objGridView.Columns.Add(string.Format("{0}", iLoopColumn), strColumnName[iLoopColumn]);
                    // 칼럼 정렬 기능 x
                    objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                    objGridView.Columns[iLoopColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, 12.0);

                objGridView.Columns[(int)EModelListColumn.IDX].Width = 50;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모델 목록 갱신
        /// </summary>
        /// <param name="objModelList"></param>
        private void SetModelListGridView(List<CConfig.CModelParameter> objModelList, bool bForceUpdate)
        {
            bool bCompare = true;
            DataGridView objGridView = this.GridViewRecipeList;

            do
            {
                // 현재 그리드 뷰에 레시피 리스트와 비교 (길이 다르면 바로 갱신)
                try
                {
                    if (
                        objModelList.Count == objGridView.RowCount
                        && false == bForceUpdate
                        )
                    {
                        for (int iLoopModelList = 0; iLoopModelList < objModelList.Count; iLoopModelList++)
                        {
                            if (
                                objModelList[iLoopModelList].strPPID != objGridView[(int)EModelListColumn.PPID, iLoopModelList].Value.ToString()
                                || objModelList[iLoopModelList].Index.ToString() != objGridView[(int)EModelListColumn.IDX, iLoopModelList].Value.ToString()
                                )
                            {
                                bCompare = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        bCompare = false;
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

                // 데이터 같으면 다시 쓸 필요 없음
                if (true == bCompare)
                    break;

                // 갱신하는 부분
                lock (objGridView)
                {
                    objGridView.Rows.Clear();

                    for (int iLoopModelList = 0; iLoopModelList < objModelList.Count; iLoopModelList++)
                    {
                        string[] strRowData = new string[(int)EModelListColumn.MODEL_LIST_COLUMN_FINAL];
                        strRowData[(int)EModelListColumn.IDX] = objModelList[iLoopModelList].Index.ToString();
                        strRowData[(int)EModelListColumn.PPID] = objModelList[iLoopModelList].strPPID;
                        objGridView.Rows.Add(strRowData);
                        // 갱신할 경우 현재 적용된 PPID를 리스트에서 선택되도록 한다.
                        if (objModelList[iLoopModelList].strPPID == m_objDocument.m_objConfig.GetSystemParameter().strPPID)
                            m_iSelectedRow = iLoopModelList;
                    }
                    try
                    {
                        if (m_iSelectedRow >= objModelList.Count)
                        {
                            m_iSelectedRow = 0;
                        }
                        objGridView[0, m_iSelectedRow].Selected = true;
                        mSelectModel = (CConfig.CModelParameter)m_objModelParameterList[m_iSelectedRow].DeepClone();
                    }
                    catch (Exception ex)
                    {
                        LogWrite.Exception(ex);
                        mSelectModel = null;
                    }
                }

            } while (false);
        }

        /// <summary>
        /// 모델 데이터 갱신
        /// </summary>
        /// <param name="iRow"></param>
        private void SetModelListData(int iRow)
        {
            do
            {
                try
                {
                    // 현재 유저 권한 레벨 받아옴
                    CUserInformation objUserInformation = m_objDocument.GetUserInformation();
                    // 설비 상태 정지 & 유저 권한 마스터 이상인 경우에만
                    // 다음 버튼 기능 활성화 유무 가능
                    if (
                        CCIMDefine.EMoveState.MOVE_STATE_PAUSE == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]
                        && (int)CDefine.EUserAuthorityLevel.MASTER <= (int)objUserInformation.m_eAuthorityLevel
                        )
                    {
                        // 현재 PPID와 선택된 PPID가 일치하는 경우에만 버튼 활성화
                        if (m_objModelParameterList[iRow].strPPID == m_objDocument.m_objConfig.GetSystemParameter().strPPID)
                        {
                            // 수정 버튼 활성화
                            SetEditButtonEnable(true);
                            // 삭제 버튼 비활성화
                            base.SetButtonEnable(this.BtnDelete, false);
                            base.SetButtonEnable(this.BtnSave, true);
                            base.SetButtonEnable(this.BtnLoad, true);
                            base.SetButtonEnable(this.BtnCreate, true);
                            BtnSave.ForeColor = Color.Blue;
                            BtnLoad.ForeColor = Color.Blue;
                            BtnCreate.ForeColor = Color.Blue;
                        }
                        else
                        {
                            // 수정 버튼 비활성화
                            SetEditButtonEnable(false);
                            base.SetButtonEnable(this.BtnDelete, true);
                            base.SetButtonEnable(this.BtnSave, false);
                            base.SetButtonEnable(this.BtnLoad, true);
                            base.SetButtonEnable(this.BtnCreate, true);
                            BtnDelete.ForeColor = Color.Blue;
                            BtnLoad.ForeColor = Color.Blue;
                            BtnCreate.ForeColor = Color.Blue;
                        }
                    }
                    else
                    {
                        // 수정 버튼 비활성화
                        SetEditButtonEnable(false);
                        base.SetButtonEnable(this.BtnSave, false);
                        base.SetButtonEnable(this.BtnLoad, false);
                        base.SetButtonEnable(this.BtnDelete, false);
                        base.SetButtonEnable(this.BtnCreate, false);
                    }

                    base.SetButtonText(this.BtnPPID, m_objModelParameterList[iRow].strPPID);

                    mSelectModel = (CConfig.CModelParameter)m_objModelParameterList[iRow].DeepClone();
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                    mSelectModel = null;
                }
            } while (false);
        }

        /// <summary>
        /// 수정 버튼 활성화 & 비활성화
        /// </summary>
        /// <param name="bEnable"></param>
        private void SetEditButtonEnable(bool bEnable)
        {
            this.BtnPPID.Enabled = bEnable;
            this.BtnParamValue01.Enabled = bEnable;
            this.BtnParamValue02.Enabled = bEnable;
            this.BtnParamValue03.Enabled = bEnable;
            this.BtnParamValue04.Enabled = bEnable;
            this.BtnParamValue05.Enabled = bEnable;
            this.BtnParamValue06.Enabled = bEnable;
            this.BtnParamValue07.Enabled = bEnable;
            this.BtnParamValue08.Enabled = bEnable;
            this.BtnParamValue09.Enabled = bEnable;
            this.BtnParamValue10.Enabled = bEnable;
        }


        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            var selectModel = mSelectModel;
            if (null != selectModel)
            {
                SpeedButton[] noButtons = { BtnTitleNo01, BtnTitleNo02, BtnTitleNo03, BtnTitleNo04, BtnTitleNo05, BtnTitleNo06, BtnTitleNo07, BtnTitleNo08, BtnTitleNo09, BtnTitleNo10 };
                SpeedButton[] titleButtons = { BtnTitleParamName01, BtnTitleParamName02, BtnTitleParamName03, BtnTitleParamName04, BtnTitleParamName05, BtnTitleParamName06, BtnTitleParamName07, BtnTitleParamName08, BtnTitleParamName09, BtnTitleParamName10 };
                SpeedButton[] valueButtons = { BtnParamValue01, BtnParamValue02, BtnParamValue03, BtnParamValue04, BtnParamValue05, BtnParamValue06, BtnParamValue07, BtnParamValue08, BtnParamValue09, BtnParamValue10 };
                SpeedButton[] rangeButtons = { BtnParamRange01, BtnParamRange02, BtnParamRange03, BtnParamRange04, BtnParamRange05, BtnParamRange06, BtnParamRange07, BtnParamRange08, BtnParamRange09, BtnParamRange10 };
                for (int i = 0; i < PAGE_DISPLAY_ITEM_COUNT; i++)
                {
                    int itemIndex = mCurrentPage * PAGE_DISPLAY_ITEM_COUNT + i;
                    if (mParameterItems.Count > itemIndex)
                    {
                        noButtons[i].Visible = true;
                        titleButtons[i].Visible = true;
                        valueButtons[i].Visible = true;
                        rangeButtons[i].Visible = true;
                        SetButtonText(noButtons[i], string.Format("{0}", itemIndex + 1));
                        SetButtonText(titleButtons[i], string.Format("{0}", mParameterItems[itemIndex].Title));
                        SetButtonText(valueButtons[i], string.Format("{0}{1}", selectModel[mParameterItems[itemIndex].Index], mParameterItems[itemIndex].Unit));
                        SetButtonText(rangeButtons[i], string.Format("{0}", mParameterItems[itemIndex].RangeInformation));

                        SetButtonBackColor(valueButtons[i], valueButtons[i].Enabled ? m_colorLabelData : m_colorLabelCategory);
                    }
                    else
                    {
                        noButtons[i].Visible = false;
                        titleButtons[i].Visible = false;
                        valueButtons[i].Visible = false;
                        rangeButtons[i].Visible = false;
                    }
                }
                SetButtonText(BtnCurrentPage, string.Format("PAGE ( {0,2} / {1,2} )", mCurrentPage + 1, mMaxPage));
            }

            // 현재 PPID 갱신
            base.SetButtonText(this.BtnSelectedPPID, m_objDocument.m_objConfig.GetSystemParameter().strPPID);
            // Recipe가 추가 되었는지 감시
            bool bForceUpdate = false;
            if (true == m_objDocument.m_objModel.IsRecipeChanged)
            {
                // 모델 파라미터 리스트 갱신
                // 변경 후 다른 탭으로 이동 후 복귀시 업데이트 안되는 버그 수정
                m_objModelParameterList = m_objDocument.m_objModel.GetModelParameterList();
                m_objDocument.m_objModel.IsRecipeChanged = false;
                bForceUpdate = true;
            }
            // 모델 리스트 갱신
            SetModelListGridView(m_objModelParameterList, bForceUpdate);

            SetButtonText(BtnCreatePpidNumber, mCreatePpidNumber.ToString());
        }

        /// <summary>
        /// 레시피 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            do
            {
                if (m_objDocument.m_objModel.IsSelectJobChangeTpmLossCode() == false)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.RECIPE_EDIT_FAILED_SELECT_TPM_LOSS_CODE_JC);
                    break;
                }

                if (m_objDocument.m_objModel.InRangeRmsOnly(mSelectModel.Index) == true)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.RECIPE_EDIT_FAILED_RMS_ONLY);
                    break;
                }

                CConfig.CModelParameter objModelParameter = mSelectModel;
                CConfig.CModelParameter m_objModelParameter = m_objDocument.m_objRecipe.m_objModelParameter;
                bool bChanged = false;
                foreach (CCIMDefine.EPpidParamList item in Enum.GetValues(typeof(CCIMDefine.EPpidParamList)))
                {
                    if (objModelParameter[item] != m_objModelParameter[item])
                    {
                        bChanged = true;
                        break;
                    }
                }
                // 변경사항이 있을때에만 CIM에 보고함
                if (false == bChanged)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THERE_ARE_NO_CHANGES_TO_THE_RECIPE);
                    break;
                }

                // SSH - 메시지 추가. - 레시피 저장
                // Msg: 레시피를 저장하시겠습니까?
                if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE_THE_RECIPE))
                {
                    break;
                }

                //kjpark 20181130 PPID BODY 값 변경시 CEID 108 으로 보고됨
                m_objDocument.m_objRecipe.SetSaveRecipeAndReportCim(objModelParameter);

                // 모델 파라미터 리스트 갱신
                m_objModelParameterList = m_objDocument.m_objModel.GetModelParameterList();
                SetResourceControl();
            } while (false);

            // 버튼 로그 추가
            string strLog = string.Format("[{0}]", "BtnSave_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 레시피 불러오기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            do
            {
                if (m_objDocument.m_objModel.IsSelectJobChangeTpmLossCode() == false)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.RECIPE_EDIT_FAILED_SELECT_TPM_LOSS_CODE_JC);
                    break;
                }

                // SSH - 메시지 추가. - 레시피 불러오기
                // Msg: 레시피를 불러오시겠습니까?
                if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_LOAD_THE_RECIPE))
                {
                    break;
                }

                // 시스템 파라미터 현재 PPID 변경
                CConfig.CSystemParameter objSystemParameter = m_objDocument.m_objConfig.GetSystemParameter();

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [PPID : {1} -> {2}]", "BtnLoad_Click", objSystemParameter.strPPID, m_objModelParameterList[m_iSelectedRow].strPPID);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                bool bCIMReply = false;
                if (objSystemParameter.strPPID != m_objModelParameterList[m_iSelectedRow].strPPID)
                {
                    bCIMReply = true;
                }

                // 현재 적용된 PPID 변경. 
                objSystemParameter.strPPID = m_objModelParameterList[m_iSelectedRow].strPPID;

                m_objDocument.m_objRecipe.SetLoadRecipeAndReportCim(objSystemParameter, bCIMReply);

                // 모델 파라미터 리스트 갱신
                m_objModelParameterList = m_objDocument.m_objModel.GetModelParameterList();

                SetResourceControl();
                // 삭제 버튼 비활성화
                base.SetButtonEnable(this.BtnDelete, false);
                base.SetButtonEnable(this.BtnSave, true);

            } while (false);
        }

        /// <summary>
        /// 레시피 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            do
            {
                if (m_objModelParameterList[m_iSelectedRow].strPPID.Length == 0)
                    break;

                if (m_objDocument.m_objModel.IsSelectJobChangeTpmLossCode() == false)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.RECIPE_EDIT_FAILED_SELECT_TPM_LOSS_CODE_JC);
                    break;
                }

                if (m_objDocument.m_objModel.InRangeRmsOnly(mSelectModel.Index) == true)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.RECIPE_EDIT_FAILED_RMS_ONLY);
                    break;
                }

                // SSH - 메시지 추가. -  레시피 삭제
                // Msg: 레시피를 삭제하시겠습니까?
                if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_DELETE_THE_RECIPE))
                {
                    break;
                }

                // D: 값 초기화 후 삭제 
                // G: 값 유지 후 삭제
                string[] ppidSubscripts = new string[]
                {
                    "D",
                    "G"
                };
                string[] options = new string[]
                {
                    mResourceDeleteOffsetOption1,
                    mResourceDeleteOffsetOption2
                };
                int optionIndex = -1;
                using (var dialogEnum = new FormEnumSelect(options))
                {
                    dialogEnum.TitleText = mResourceDeleteOffsetOptionTitle;
                    dialogEnum.CancelText = mResourceCommonOffsetOptionCancel;
                    if (dialogEnum.ShowDialog() != DialogResult.OK)
                    {
                        break;
                    }

                    optionIndex = dialogEnum.ResultIndex;
                }

                // 레시피 삭제
                string strPPID = m_objModelParameterList[m_iSelectedRow].strPPID;
                string strPath = string.Format(@"{0}\{1}", m_objDocument.m_objConfig.GetModelPath(), strPPID);

                // 데이터 삭제 전에 CIM 보고하자.
                m_objDocument.m_objRecipe.SendPpidChangeEvent(CCIMDefine.EPpidChangeMode.PPID_CHANGE_MODE_DELETE, m_objModelParameterList[m_iSelectedRow], ppidSubscripts[optionIndex]);

                // 모델 옵셋 파라미터 리셋
                if (ppidSubscripts[optionIndex] == "D")
                {
                    m_objDocument.m_objConfig.ResetModelOffsetParameter(m_objModelParameterList[m_iSelectedRow].Index);
                }

                m_objDocument.m_objModel.SetDirectoryDelete(strPath);
                // 모델 파라미터 리스트 갱신
                m_objModelParameterList = m_objDocument.m_objModel.GetModelParameterList();
                SetResourceControl();
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Delete PPID : {1}]", "BtnDelete_Click", strPPID);
                m_objDocument.SetUpdateButtonLog(this, strLog);

            } while (false);
        }

        /// <summary>
        /// 레시피 생성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            CModel objModel = m_objDocument.m_objModel;
            CConfig objConfig = m_objDocument.m_objConfig;
            string strPPID = this.BtnCreatePPID.Text;
            var currentOffsetParameter = m_objDocument.m_objConfig.GetModelOffsetParameter();

            // 레시피 생성
            do
            {
                if (m_objDocument.m_objModel.IsSelectJobChangeTpmLossCode() == false)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.RECIPE_EDIT_FAILED_SELECT_TPM_LOSS_CODE_JC);
                    break;
                }

                // PPID가 존재하지 않는 경우
                if (true == string.IsNullOrWhiteSpace(strPPID))
                {
                    // Msg: PPID가 없습니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THERE_IS_NOT_PPID);
                    break;
                }

                // PPID가 중복되는 경우
                if (true == m_objDocument.m_objModel.GetPPIDOverlap(strPPID))
                {
                    // Msg: PPID가 중복됩니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THE_PPID_IS_DUPLICATED);
                    break;
                }

                if (m_objDocument.m_objModel.IsDuiplicateIndex(mCreatePpidNumber) == true)
                {
                    // Msg: PPID 번호가 중복됩니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THE_PPID_NUMBER_IS_DUPLICATED);
                    break;
                }

                // PPID이름이 'TT_'로 시작하는지 않는 경우
                if (false == strPPID.StartsWith("TT_"))
                {
                    // Msg: 레시피를 생성 할 수 없습니다. 설비에서 생성하는 PPID는 'TT_'로 시작해야 합니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.RECIPE_CREATE_FAILED_PPID_WRONG_FORMAT);
                    break;
                }

                // SSH - 메시지 추가. -  레시피 생성
                // Msg: 레시피를 생성하시겠습니까?
                if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_CREATE_THE_RECIPE))
                {
                    break;
                }

                // N: 기존 값 참조 생성(기존 값 없으면 현재 값 복사 후 생성)
                // O: 기존 값 참조 생성
                string[] ppidSubscripts = new string[]
                {
                    "N",
                    "O"
                };
                string[] options = new string[]
                {
                    mResourceCreateOffsetOption1,
                    mResourceCreateOffsetOption2
                };
                int optionIndex = -1;
                using (var dialogEnum = new FormEnumSelect(options))
                {
                    dialogEnum.TitleText = mResourceCreateOffsetOptionTitle;
                    dialogEnum.CancelText = mResourceCommonOffsetOptionCancel;
                    if (dialogEnum.ShowDialog() != DialogResult.OK)
                    {
                        break;
                    }

                    optionIndex = dialogEnum.ResultIndex;
                }

                string strExistFilePath = string.Format(@"{0}\{1}", objConfig.GetModelPath(), objConfig.GetSystemParameter().strPPID);
                string strNewFilePath = string.Format(@"{0}\{1}", objConfig.GetModelPath(), strPPID);
                // 폴더 복사
                Constants.CopyFolderRecursive(strExistFilePath, strNewFilePath);
                objModel.SetPPIDMatch(strPPID, mCreatePpidNumber);

                // 모델 옵셋 파라미터 저장
                if (ppidSubscripts[optionIndex] == "N")
                {
                    m_objDocument.m_objConfig.ResetModelOffsetParameter(mCreatePpidNumber);
                }

                // 모델 파라미터 리스트 갱신
                m_objModelParameterList = m_objDocument.m_objModel.GetModelParameterList();
                SetResourceControl();

                BtnCreatePPID.Text = "";

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [Create PPID : {1}]", "BtnCreate_Click", strPPID);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                // CIM 보고하자.
                CConfig.CModelParameter objModelParameter = objModel.GetModelParameter(strPPID);
                m_objDocument.m_objRecipe.SendPpidChangeEvent(CCIMDefine.EPpidChangeMode.PPID_CHANGE_MODE_CREATE, objModelParameter, ppidSubscripts[optionIndex]);

            } while (false);
        }

        /// <summary>
        /// 생성 PPID 입력 받음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreatePPID_Click(object sender, EventArgs e)
        {
            // 설비 자동 동작중이면 스킵
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                return;
            }

            Button objButton = sender as Button;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnCreatePPID_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            m_objDocument.SetUpdateButtonLog(this, strLog);
            using (FormKeyBoard objKeyboard = new FormKeyBoard("TT_"))
            {
                if (DialogResult.OK == objKeyboard.ShowDialog())
                {
                    string strPPID = objKeyboard.m_strReturnValue.ToUpper();
                    bool bCheck = false;
                    foreach (char c in strPPID)
                    {
                        if (
                            (c >= 0x30 && c <= 0x39)
                            || (c >= 0x41 && c <= 0x5A)
                            || (c == '-')
                            || (c == '_')
                            )
                        {
                            bCheck = true;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (bCheck == false)
                    {
                        if (strPPID.Length > 0)
                        {
                            MessageBox.Show("공백이나 특수문자를 빼고 입력해주세요.");
                        }
                        SetButtonText(BtnCreatePPID, string.Empty);
                    }
                    else
                    {
                        SetButtonText(BtnCreatePPID, objKeyboard.m_strReturnValue.ToUpper());
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [Create PPID : {1}] [{2}]", "BtnCreatePPID_Click", BtnCreatePPID.Text, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnCreatePpidNumber_Click(object sender, EventArgs e)
        {
            int originalValue = mCreatePpidNumber;
            using (var dialog = new FormKeyPad(mCreatePpidNumber, CModel.RANGE_PPID_SHARE_START_NUMBER, CModel.RANGE_PPID_SHARE_END_NUMBER))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                mCreatePpidNumber = Convert.ToInt32(dialog.m_dResultValue);
            }
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] {originalValue} -> {mCreatePpidNumber}");
        }

        private void GridViewRecipeList_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView objGridView = sender as DataGridView;

            try
            {
                if (objGridView.SelectedRows.Count == 0)
                {
                    return;
                }

                m_iSelectedRow = objGridView.SelectedRows[0].Index;
                // 모델 리스트 데이터 갱신
                SetModelListData(m_iSelectedRow);
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        private void BtnFirstPage_Click(object sender, EventArgs e)
        {
            mCurrentPage = 0;
        }

        private void BtnPrivious_Click(object sender, EventArgs e)
        {
            if (mCurrentPage > 0)
            {
                mCurrentPage--;
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (mCurrentPage < mMaxPage - 1)
            {
                mCurrentPage++;
            }
        }

        private void BtnLastPage_Click(object sender, EventArgs e)
        {
            mCurrentPage = mMaxPage - 1;
        }

        private void BtnParamValue00_Click(object sender, EventArgs e)
        {
            do
            {
                if (m_objDocument.m_objModel.IsSelectJobChangeTpmLossCode() == false)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.RECIPE_EDIT_FAILED_SELECT_TPM_LOSS_CODE_JC);
                    break;
                }

                if (null == mSelectModel)
                {
                    return;
                }

                if (m_objDocument.m_objModel.InRangeRmsOnly(mSelectModel.Index) == true)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.RECIPE_EDIT_FAILED_RMS_ONLY);
                    break;
                }

                if (CCIMDefine.EMoveState.MOVE_STATE_PAUSE != m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                {
                    break;
                }

                Button btn = sender as Button;
                if (null == btn)
                {
                    break;
                }
                int buttonIndex = Convert.ToInt32(btn.Tag);

                int itemIndex = mCurrentPage * PAGE_DISPLAY_ITEM_COUNT + buttonIndex;
                if (mParameterItems.Count > itemIndex)
                {
                    mParameterItems[itemIndex].RaiseParameterEditEvent(mSelectModel);
                }

            } while (false);
        }

        private void BtnSelectedPPID_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in GridViewRecipeList.Rows)
            {
                if (row.Cells[(int)EModelListColumn.PPID].Value.ToString() == BtnSelectedPPID.Text.ToString())
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        private class ParameterItem
        {
            public CCIMDefine.EPpidParamList Index { get; private set; }
            public bool IsReadonly { get; private set; }
            public string RangeInformation { get; private set; }
            public string Unit { get; private set; }
            public string Title { get; private set; }
            public Type ParameterType { get; private set; }
            private Form mOwner;
            private Action<Form, string> mButtonLogHandler;

            public ParameterItem(Form owner, CCIMDefine.EPpidParamList index, Type parameterType, Action<Form, string> buttonLogHandler, bool bReadonly = false)
            {
                mOwner = owner;
                mButtonLogHandler = buttonLogHandler;
                Index = index;
                ParameterType = parameterType;
                IsReadonly = bReadonly;
                Title = Index.ToString();
                RangeInformation = CCIMDefine.RECIPE_PARAMS[Index].GetRangeString();
                Unit = string.IsNullOrWhiteSpace(CCIMDefine.RECIPE_PARAMS[Index].Unit) ? "" : string.Format(" ( {0} )", CCIMDefine.RECIPE_PARAMS[Index].Unit);
            }

            public void RaiseParameterEditEvent(CConfig.CModelParameter changeModel)
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}_Click] [{1}]", Index.ToString(), true);
                mButtonLogHandler(mOwner, strLog);
                do
                {
                    if (true == IsReadonly)
                    {
                        break;
                    }

                    if (ParameterType == typeof(string))
                    {
                        // Keyboard
                        using (FormKeyBoard keyBoard = new FormKeyBoard())
                        {
                            if (DialogResult.OK == keyBoard.ShowDialog())
                            {
                                changeModel[Index] = keyBoard.m_strReturnValue;
                            }
                        }
                    }
                    else if (
                        ParameterType == typeof(double)
                        || ParameterType == typeof(float)
                        || ParameterType == typeof(long)
                        || ParameterType == typeof(ulong)
                        || ParameterType == typeof(int)
                        || ParameterType == typeof(uint)
                        || ParameterType == typeof(short)
                        || ParameterType == typeof(ushort)
                        || ParameterType == typeof(byte)
                        || ParameterType == typeof(sbyte)
                        )
                    {
                        // Keypad
                        using (FormKeyPad keyPad = new FormKeyPad(Convert.ToDouble(changeModel[Index])))
                        {
                            keyPad.m_dMaxValue = CCIMDefine.RECIPE_PARAMS[Index].MaxValue;
                            keyPad.m_dMinValue = CCIMDefine.RECIPE_PARAMS[Index].MinValue;
                            if (DialogResult.OK == keyPad.ShowDialog())
                            {
                                changeModel[Index] = keyPad.m_dResultValue.ToString();
                            }
                        }
                    }
                    else if (ParameterType.IsEnum == true)
                    {
                        bool bHasFlagsAttribute = ParameterType.GetCustomAttributes(true).Any(i => typeof(FlagsAttribute).Equals(i.GetType()));
                        if (bHasFlagsAttribute == true)
                        {
                            // Flags Selector
                            const string FLAGS_SEPARATOR = ",";
                            string[] initialItemNames = changeModel[Index].Split(FLAGS_SEPARATOR.ToCharArray())
                                .Select(i => i.Trim())
                                .ToArray();
                            using (FormFlagSelect flagSelector = new FormFlagSelect(Enum.GetNames(ParameterType), initialItemNames))
                            {
                                if (DialogResult.OK == flagSelector.ShowDialog())
                                {
                                    changeModel[Index] = string.Join(FLAGS_SEPARATOR, flagSelector.ResultNames);
                                }
                            }
                        }
                        else
                        {
                            // Enum Selector
                            using (FormEnumSelect enumSelector = new FormEnumSelect(Enum.GetNames(ParameterType), changeModel[Index]))
                            {
                                if (DialogResult.OK == enumSelector.ShowDialog())
                                {
                                    changeModel[Index] = enumSelector.ResultName;
                                }
                            }
                        }
                    }
                } while (false);

                // 버튼 로그 추가
                strLog = string.Format("[{0}_Click] [{1}] [{2}]", Index.ToString(), changeModel[Index], false);
                mButtonLogHandler(mOwner, strLog);
            }
        }
    }
}