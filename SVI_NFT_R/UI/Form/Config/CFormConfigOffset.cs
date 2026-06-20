using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormConfigOffset : CFormCommon, CFormInterface
    {
        private class InternalModelOffsetSet
        {
            public int Index { get; set; } = 1;
            public bool IsRegistered { get; set; } = false;
            public CConfig.CModelOffsetParameter Data { get; set; } = new CConfig.CModelOffsetParameter();
        }

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
        private List<InternalModelOffsetSet> m_objModelParameterList;
        /// <summary>
        /// 모델 리스트 칼럼 정의
        /// </summary>
        private enum EModelListColumn
        {
            IDX = 0,
            REGISTERED
        };
        private readonly Type mModelOffsetType;
        private readonly FieldInfo[] mModelOffsetFields;
        private CConfig.CModelOffsetParameter mSelectModel;
        private int mCurrentPage = 0;
        private readonly int mMaxPage;
        private const int PAGE_DISPLAY_ITEM_COUNT = 10;
        private int mCurrentPpidNumber => m_objDocument.m_objModel.GetModelParameter(m_objDocument.m_objConfig.GetSystemParameter().strPPID).Index;
        private const int MIN_PPID_NUMBER = CModel.RANGE_PPID_START_NUMBER;
        private const int MAX_PPID_NUMBER = CModel.RANGE_PPID_END_NUMBER;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormConfigOffset(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            InitializeComponent();

            mModelOffsetType = typeof(CConfig.CModelOffsetParameter);
            mModelOffsetFields = mModelOffsetType.GetFields();

            mMaxPage = mModelOffsetFields.Length / PAGE_DISPLAY_ITEM_COUNT + 1;

            m_objModelParameterList = new List<InternalModelOffsetSet>(MAX_PPID_NUMBER);
            for (int i = MIN_PPID_NUMBER; i <= MAX_PPID_NUMBER; i++)
            {
                m_objModelParameterList.Add(new InternalModelOffsetSet() { Index = i });
            }
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
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeAutoScale();
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
                string[] strModelList = Enum.GetNames(typeof(EModelListColumn));
                if (false == InitializeGridView(this.GridViewOffsetList, strModelList))
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
            base.SetButtonBackColor(this.BtnTitleOffsetList, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleOffsetInfo, m_colorLabel);
            base.SetButtonBackColor(this.BtnTitleSelectedPpidNumber, m_colorLabelSub);
            base.SetButtonBackColor(this.BtnSelectedPpidNumber, m_colorLabelData);
            base.SetButtonBackColor(this.BtnSave, m_colorNormal);
            base.SetButtonBackColor(this.BtnLoad, m_colorNormal);
            base.SetButtonBackColor(this.BtnDelete, m_colorNormal);
            base.SetButtonBackColor(this.BtnCopy, m_colorNormal);
            base.SetButtonBackColor(this.BtnTitlePPIDNumber, m_colorLabelSub);
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
            base.SetButtonBackColor(this.BtnPpidNumber, m_colorLabelData);
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
                    && false == btn.Name.Equals(BtnSelectedPpidNumber.Name)
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
                SetButtonChangeLanguage(this.BtnTitleOffsetList);
                SetButtonChangeLanguage(this.BtnTitleOffsetInfo);
                SetButtonChangeLanguage(this.BtnTitleSelectedPpidNumber);
                SetButtonChangeLanguage(this.BtnSave);
                SetButtonChangeLanguage(this.BtnLoad);
                SetButtonChangeLanguage(this.BtnDelete);
                SetButtonChangeLanguage(this.BtnCopy);
                SetButtonChangeLanguage(this.BtnTitlePPIDNumber);
                SetButtonChangeLanguage(this.BtnFirstPage);
                SetButtonChangeLanguage(this.BtnPriviousPage);
                SetButtonChangeLanguage(this.BtnNextPage);
                SetButtonChangeLanguage(this.BtnLastPage);

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
                var modelOffsetParameters = m_objDocument.m_objConfig.GetModelOffsetParameters();
                for (int i = 0; i < m_objModelParameterList.Count; i++)
                {
                    int offsetIndex = m_objModelParameterList[i].Index;
                    if (modelOffsetParameters.ContainsKey(offsetIndex) == true)
                    {
                        m_objModelParameterList[i].Data = modelOffsetParameters[offsetIndex];
                        m_objModelParameterList[i].IsRegistered = true;
                    }
                    else
                    {
                        m_objModelParameterList[i].Data = new CConfig.CModelOffsetParameter();
                        m_objModelParameterList[i].IsRegistered = false;
                    }
                }
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
        private void SetModelListGridView(List<InternalModelOffsetSet> objModelList, bool bForceUpdate)
        {
            bool bCompare = true;
            DataGridView objGridView = GridViewOffsetList;

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
                            if (objModelList[iLoopModelList].IsRegistered != (objGridView[(int)EModelListColumn.REGISTERED, iLoopModelList].Value.ToString() == "O"))
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
                    int lastSelection = m_iSelectedRow;
                    objGridView.Rows.Clear();

                    int columnCount = Enum.GetNames(typeof(EModelListColumn)).Length;
                    for (int iLoopModelList = 0; iLoopModelList < objModelList.Count; iLoopModelList++)
                    {
                        string[] strRowData = new string[columnCount];
                        strRowData[(int)EModelListColumn.IDX] = objModelList[iLoopModelList].Index.ToString();
                        strRowData[(int)EModelListColumn.REGISTERED] = objModelList[iLoopModelList].IsRegistered ? "O" : string.Empty;
                        objGridView.Rows.Add(strRowData);
                        //// 갱신할 경우 현재 적용된 PPID를 리스트에서 선택되도록 한다.
                        //if (objModelList[iLoopModelList].Index == mCurrentPpidNumber)
                        //{
                        //    m_iSelectedRow = iLoopModelList;
                        //}
                    }
                    try
                    {
                        m_iSelectedRow = lastSelection;
                        if (m_iSelectedRow >= objModelList.Count)
                        {
                            m_iSelectedRow = 0;
                        }
                        objGridView[0, m_iSelectedRow].Selected = true;
                        mSelectModel = m_objModelParameterList[m_iSelectedRow].Data.DeepClone();
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
                        && (int)CDefine.EUserAuthorityLevel.ENGINEER <= (int)objUserInformation.m_eAuthorityLevel
                        )
                    {
                        // 현재 PPID와 선택된 PPID가 일치하는 경우에만 버튼 활성화
                        if (m_objModelParameterList[iRow].Index == mCurrentPpidNumber)
                        {
                            // 수정 버튼 활성화
                            SetEditButtonEnable(true);
                            // 삭제 버튼 비활성화
                            base.SetButtonEnable(this.BtnDelete, true);
                            base.SetButtonEnable(this.BtnSave, true);
                            base.SetButtonEnable(this.BtnLoad, true);
                            base.SetButtonEnable(this.BtnCopy, false);
                            BtnDelete.ForeColor = Color.Blue;
                            BtnSave.ForeColor = Color.Blue;
                            BtnLoad.ForeColor = Color.Blue;
                        }
                        else
                        {
                            // 수정 버튼 비활성화
                            SetEditButtonEnable(false);
                            base.SetButtonEnable(this.BtnDelete, true);
                            base.SetButtonEnable(this.BtnSave, false);
                            base.SetButtonEnable(this.BtnLoad, false);
                            base.SetButtonEnable(this.BtnCopy, true);
                            BtnDelete.ForeColor = Color.Blue;
                            BtnCopy.ForeColor = Color.Blue;
                        }
                    }
                    else
                    {
                        // 수정 버튼 비활성화
                        SetEditButtonEnable(false);
                        base.SetButtonEnable(this.BtnSave, false);
                        base.SetButtonEnable(this.BtnLoad, false);
                        base.SetButtonEnable(this.BtnDelete, false);
                        base.SetButtonEnable(this.BtnCopy, false);
                    }

                    base.SetButtonText(this.BtnPpidNumber, m_objModelParameterList[m_iSelectedRow].Index.ToString());

                    mSelectModel = m_objModelParameterList[m_iSelectedRow].Data.DeepClone();
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
            this.BtnPpidNumber.Enabled = bEnable;
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
                for (int i = 0; i < PAGE_DISPLAY_ITEM_COUNT; i++)
                {
                    int itemIndex = mCurrentPage * PAGE_DISPLAY_ITEM_COUNT + i;
                    if (mModelOffsetFields.Length > itemIndex)
                    {
                        noButtons[i].Visible = true;
                        titleButtons[i].Visible = true;
                        valueButtons[i].Visible = true;
                        SetButtonText(noButtons[i], string.Format("{0}", itemIndex + 1));
                        var attribute = mModelOffsetFields[itemIndex].GetCustomAttribute<CConfig.DisplayInfoAttribute>();
                        SetButtonText(titleButtons[i], string.Format("{0}", attribute.Name));
                        SetButtonText(valueButtons[i], string.Format("{0:0.000}{1}", mModelOffsetFields[itemIndex].GetValue(selectModel), attribute.Unit));

                        SetButtonBackColor(valueButtons[i], valueButtons[i].Enabled ? m_colorLabelData : m_colorLabelCategory);
                    }
                    else
                    {
                        noButtons[i].Visible = false;
                        titleButtons[i].Visible = false;
                        valueButtons[i].Visible = false;
                    }
                }
                SetButtonText(BtnCurrentPage, string.Format("PAGE ( {0,2} / {1,2} )", mCurrentPage + 1, mMaxPage));
            }

            // 현재 PPID 갱신
            base.SetButtonText(this.BtnSelectedPpidNumber, mCurrentPpidNumber.ToString());
            // Recipe가 추가 되었는지 감시
            bool bForceUpdate = false;
            if (true == m_objDocument.m_objModel.IsOffsetChanged)
            {
                // 모델 파라미터 리스트 갱신
                // 변경 후 다른 탭으로 이동 후 복귀시 업데이트 안되는 버그 수정
                var modelOffsetParameters = m_objDocument.m_objConfig.GetModelOffsetParameters();
                for (int i = 0; i < m_objModelParameterList.Count; i++)
                {
                    int offsetIndex = m_objModelParameterList[i].Index;
                    if (modelOffsetParameters.ContainsKey(offsetIndex) == true)
                    {
                        m_objModelParameterList[i].Data = modelOffsetParameters[offsetIndex];
                        m_objModelParameterList[i].IsRegistered = true;
                    }
                    else
                    {
                        m_objModelParameterList[i].Data = new CConfig.CModelOffsetParameter();
                        m_objModelParameterList[i].IsRegistered = false;
                    }
                }
                m_objDocument.m_objModel.IsOffsetChanged = false;
                bForceUpdate = true;
            }
            // 모델 리스트 갱신
            SetModelListGridView(m_objModelParameterList, bForceUpdate);
        }

        /// <summary>
        /// 레시피 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE_THE_OFFSET_DATA))
            {
                return;
            }

            var selectModel = mSelectModel;
            m_objDocument.m_objConfig.SaveModelOffsetParameter(mSelectModel, m_objModelParameterList[m_iSelectedRow].Index);
            m_objDocument.m_objConfig.LoadModelOffsetParameter();

            var modelOffsetParameters = m_objDocument.m_objConfig.GetModelOffsetParameters();
            for (int i = 0; i < m_objModelParameterList.Count; i++)
            {
                int offsetIndex = m_objModelParameterList[i].Index;
                if (modelOffsetParameters.ContainsKey(offsetIndex) == true)
                {
                    m_objModelParameterList[i].Data = modelOffsetParameters[offsetIndex];
                    m_objModelParameterList[i].IsRegistered = true;
                }
                else
                {
                    m_objModelParameterList[i].Data = new CConfig.CModelOffsetParameter();
                    m_objModelParameterList[i].IsRegistered = false;
                }
            }
            SetResourceControl();

            // 버튼 로그 추가
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
        }

        /// <summary>
        /// 레시피 불러오기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_LOAD_THE_OFFSET_DATA))
            {
                return;
            }

            m_objDocument.m_objConfig.LoadModelOffsetParameter();

            SetModelListData(m_iSelectedRow);

            // 버튼 로그 추가
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
        }

        /// <summary>
        /// 레시피 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_DELETE_THE_OFFSET_DATA))
            {
                return;
            }

            m_objDocument.m_objConfig.ResetModelOffsetParameter(m_objModelParameterList[m_iSelectedRow].Index);
            m_objDocument.m_objConfig.LoadModelOffsetParameter();

            var modelOffsetParameters = m_objDocument.m_objConfig.GetModelOffsetParameters();
            for (int i = 0; i < m_objModelParameterList.Count; i++)
            {
                int offsetIndex = m_objModelParameterList[i].Index;
                if (modelOffsetParameters.ContainsKey(offsetIndex) == true)
                {
                    m_objModelParameterList[i].Data = modelOffsetParameters[offsetIndex];
                    m_objModelParameterList[i].IsRegistered = true;
                }
                else
                {
                    m_objModelParameterList[i].Data = new CConfig.CModelOffsetParameter();
                    m_objModelParameterList[i].IsRegistered = false;
                }
            }
            SetResourceControl();

            // 버튼 로그 추가
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
        }

        /// <summary>
        /// 레시피 생성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_COPY_THE_OFFSET_DATA))
            {
                return;
            }

            var selectModel = mSelectModel;
            m_objDocument.m_objConfig.SaveModelOffsetParameter(mSelectModel);

            var modelOffsetParameters = m_objDocument.m_objConfig.GetModelOffsetParameters();
            for (int i = 0; i < m_objModelParameterList.Count; i++)
            {
                int offsetIndex = m_objModelParameterList[i].Index;
                if (modelOffsetParameters.ContainsKey(offsetIndex) == true)
                {
                    m_objModelParameterList[i].Data = modelOffsetParameters[offsetIndex];
                    m_objModelParameterList[i].IsRegistered = true;
                }
                else
                {
                    m_objModelParameterList[i].Data = new CConfig.CModelOffsetParameter();
                    m_objModelParameterList[i].IsRegistered = false;
                }
            }
            SetResourceControl();

            // 버튼 로그 추가
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
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
                if (null == mSelectModel)
                {
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
                using (FormKeyPad keyPad = new FormKeyPad(Convert.ToDouble(mModelOffsetFields[itemIndex].GetValue(mSelectModel))))
                {
                    if (DialogResult.OK != keyPad.ShowDialog())
                    {
                        break;
                    }
                    mModelOffsetFields[itemIndex].SetValue(mSelectModel, keyPad.m_dResultValue);
                }

            } while (false);
        }

        private void BtnSelectedPPID_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in GridViewOffsetList.Rows)
            {
                if (row.Cells[(int)EModelListColumn.IDX].Value.ToString() == BtnSelectedPpidNumber.Text.ToString())
                {
                    row.Selected = true;
                    if (row.Displayed == false)
                    {
                        GridViewOffsetList.FirstDisplayedScrollingRowIndex = row.Index;
                    }
                    break;
                }
            }
        }
    }
}