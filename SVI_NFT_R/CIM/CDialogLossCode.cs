using System;
using System.Drawing;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogLossCode : CFormCommon, CFormInterface
    {
        public enum ButtonIndex
        {
            BTN_BM = 0,
            BTN_REGULAR_PM,
            BTN_IRREGULAR_PM_EQP,
            BTN_IRREGULAR_PM_QTY,
            BTN_CM_EE,
            BTN_CM_EI,
            BTN_CM_NEW_PRODUCT,
            BTN_CHANGE_SAME_MODEL,
            BTN_CHANGE_DIFFERENT_MODEL,
            BTN_MATERIAL_CHANGE,
            BTN_MATERIAL_DOWN,
            BTN_FINAL
        }
        public enum CodeNumber
        {
            CODE_BM = 3000,
            CODE_REGULAR_PM = 12000,
            CODE_IRREGULAR_PM_EQP = 15100,
            CODE_IRREGULAR_PM_QTY = 15200,
            CODE_CM_EE = 17200,
            CODE_CM_EI = 17300,
            CODE_CM_NEW_PRODUCT = 14000,
            CODE_CHANGE_SAME_MODEL = 41100,
            CODE_CHANGE_DIFFERENT_MODEL = 41200,
            CODE_MATERIAL_CHANGE = 35000,
            CODE_MATERIAL_DOWN = 51000,
            CODE_FINAL
        }

        /// <summary>
        /// 버튼
        /// </summary>
        private ImageButton[] BtnCode;
        /// <summary>
        /// 현재 선택된 버튼
        /// </summary>
        private ButtonIndex m_eButtonSelectedIndex;
        /// <summary>
        /// 현재 선택된 코드번호
        /// </summary>
        private CodeNumber _eCodeNumber;
        public CodeNumber m_eCodeNumber
        {
            get { return _eCodeNumber; }
            set { _eCodeNumber = value; }
        }
        /// <summary>
        /// 비고
        /// </summary>
        private string _strDescription;
        public string m_strDescription
        {
            get { return _strDescription; }
            set { _strDescription = value; }
        }


        private CDocument m_objDocument;
        private bool mbConfirmButton = false;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CDialogLossCode(CDocument objDocument)
        {
            InitializeComponent();
            m_objDocument = objDocument as CDocument;
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogLossCode_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogLossCode_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeInitialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool bResult = false;
            do
            {
                // 다이얼로그 초기화
                if (false == InitializeForm())
                    break;

                bResult = true;
            } while (false);
            return bResult;
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
                // 버튼 매핑.
                BtnCode = new ImageButton[(int)ButtonIndex.BTN_FINAL];
                BtnCode[(int)ButtonIndex.BTN_BM] = BtnCodeBM;
                BtnCode[(int)ButtonIndex.BTN_REGULAR_PM] = BtnCodeRegularPM;
                BtnCode[(int)ButtonIndex.BTN_IRREGULAR_PM_EQP] = BtnCodeIrregularPMEquipment;
                BtnCode[(int)ButtonIndex.BTN_IRREGULAR_PM_QTY] = BtnCodeIrregularPMQuality;
                BtnCode[(int)ButtonIndex.BTN_CM_EE] = BtnCodeCMEE;
                BtnCode[(int)ButtonIndex.BTN_CM_EI] = BtnCodeCMEI;
                BtnCode[(int)ButtonIndex.BTN_CM_NEW_PRODUCT] = BtnCodeCMNewProduct;
                BtnCode[(int)ButtonIndex.BTN_CHANGE_SAME_MODEL] = BtnCodeChangeSameModel;
                BtnCode[(int)ButtonIndex.BTN_CHANGE_DIFFERENT_MODEL] = BtnCodeChangeDifferentModel;
                BtnCode[(int)ButtonIndex.BTN_MATERIAL_CHANGE] = BtnCodeMaterialChange;
                BtnCode[(int)ButtonIndex.BTN_MATERIAL_DOWN] = BtnCodeMaterialDown;

                // 초기 선택은 없음.
                m_eButtonSelectedIndex = ButtonIndex.BTN_FINAL;
                m_eCodeNumber = CodeNumber.CODE_FINAL;
                m_strDescription = "";

                // 버튼 이벤트 등록
                for (int iLoopCount = 0; iLoopCount < (int)ButtonIndex.BTN_FINAL; iLoopCount++)
                {
                    BtnCode[iLoopCount].Click += Button_Click;
                }

                SetChangeLanguage();

                SetButtonColor();

                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                SetTimer(true);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            SetTimer(false);
        }


        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Button_Click(object sender, EventArgs e)
        {
            switch (((ImageButton)sender).Name)
            {
                case "BtnCodeBM":
                    if (m_eButtonSelectedIndex == ButtonIndex.BTN_BM)
                    {
                        break;
                    }
                    using (var confirmDialog = new CDialogPassword(m_objDocument))
                    {
                        confirmDialog.ShowDialog();
                        if (confirmDialog.DialogResult != DialogResult.OK)
                        {
                            break;
                        }
                    }
                    m_eButtonSelectedIndex = ButtonIndex.BTN_BM;
                    m_eCodeNumber = CodeNumber.CODE_BM;
                    m_strDescription = "BREAKDOWN_MANUAL";
                    break;
                case "BtnCodeRegularPM":
                    if (m_eButtonSelectedIndex == ButtonIndex.BTN_REGULAR_PM)
                    {
                        break;
                    }
                    using (var confirmDialog = new CDialogPassword(m_objDocument))
                    {
                        confirmDialog.ShowDialog();
                        if (confirmDialog.DialogResult != DialogResult.OK)
                        {
                            break;
                        }
                    }
                    m_eButtonSelectedIndex = ButtonIndex.BTN_REGULAR_PM;
                    m_eCodeNumber = CodeNumber.CODE_REGULAR_PM;
                    m_strDescription = "REGULAR_PM";
                    break;
                case "BtnCodeIrregularPMEquipment":
                    m_eButtonSelectedIndex = ButtonIndex.BTN_IRREGULAR_PM_EQP;
                    m_eCodeNumber = CodeNumber.CODE_IRREGULAR_PM_EQP;
                    m_strDescription = "CHECK_EQUIPMENT";
                    break;
                case "BtnCodeIrregularPMQuality":
                    m_eButtonSelectedIndex = ButtonIndex.BTN_IRREGULAR_PM_QTY;
                    m_eCodeNumber = CodeNumber.CODE_IRREGULAR_PM_QTY;
                    m_strDescription = "CHECK_QUALITY";
                    break;
                case "BtnCodeCMEE":
                    if (m_eButtonSelectedIndex == ButtonIndex.BTN_CM_EE)
                    {
                        break;
                    }
                    using (var confirmDialog = new CDialogPassword(m_objDocument))
                    {
                        confirmDialog.ShowDialog();
                        if (confirmDialog.DialogResult != DialogResult.OK)
                        {
                            break;
                        }
                    }
                    m_eButtonSelectedIndex = ButtonIndex.BTN_CM_EE;
                    m_eCodeNumber = CodeNumber.CODE_CM_EE;
                    m_strDescription = "IMPROVE_EQUIPMENT_EE";
                    break;
                case "BtnCodeCMEI":
                    if (m_eButtonSelectedIndex == ButtonIndex.BTN_CM_EI)
                    {
                        break;
                    }
                    using (var confirmDialog = new CDialogPassword(m_objDocument))
                    {
                        confirmDialog.ShowDialog();
                        if (confirmDialog.DialogResult != DialogResult.OK)
                        {
                            break;
                        }
                    }
                    m_eButtonSelectedIndex = ButtonIndex.BTN_CM_EI;
                    m_eCodeNumber = CodeNumber.CODE_CM_EI;
                    m_strDescription = "IMPROVE_EQUIPMENT_EI";
                    break;
                case "BtnCodeCMNewProduct":
                    if (m_eButtonSelectedIndex == ButtonIndex.BTN_CM_NEW_PRODUCT)
                    {
                        break;
                    }
                    using (var confirmDialog = new CDialogPassword(m_objDocument))
                    {
                        confirmDialog.ShowDialog();
                        if (confirmDialog.DialogResult != DialogResult.OK)
                        {
                            break;
                        }
                    }
                    m_eButtonSelectedIndex = ButtonIndex.BTN_CM_NEW_PRODUCT;
                    m_eCodeNumber = CodeNumber.CODE_CM_NEW_PRODUCT;
                    m_strDescription = "SETUP_NEW_PRODUCT";
                    break;
                case "BtnCodeChangeSameModel":
                    if (m_eButtonSelectedIndex == ButtonIndex.BTN_CHANGE_SAME_MODEL)
                    {
                        break;
                    }
                    using (var confirmDialog = new CDialogPassword(m_objDocument))
                    {
                        confirmDialog.ShowDialog();
                        if (confirmDialog.DialogResult != DialogResult.OK)
                        {
                            break;
                        }
                    }
                    m_eButtonSelectedIndex = ButtonIndex.BTN_CHANGE_SAME_MODEL;
                    m_eCodeNumber = CodeNumber.CODE_CHANGE_SAME_MODEL;
                    m_strDescription = "CHANGE_SAME_MODEL";
                    break;
                case "BtnCodeChangeDifferentModel":
                    if (m_eButtonSelectedIndex == ButtonIndex.BTN_CHANGE_DIFFERENT_MODEL)
                    {
                        break;
                    }
                    using (var confirmDialog = new CDialogPassword(m_objDocument))
                    {
                        confirmDialog.ShowDialog();
                        if (confirmDialog.DialogResult != DialogResult.OK)
                        {
                            break;
                        }
                    }
                    m_eButtonSelectedIndex = ButtonIndex.BTN_CHANGE_DIFFERENT_MODEL;
                    m_eCodeNumber = CodeNumber.CODE_CHANGE_DIFFERENT_MODEL;
                    m_strDescription = "CHANGE_DIFFERENT_MODEL";
                    break;
                case "BtnCodeMaterialChange":
                    if (m_eButtonSelectedIndex == ButtonIndex.BTN_MATERIAL_CHANGE)
                    {
                        break;
                    }
                    using (var confirmDialog = new CDialogPassword(m_objDocument))
                    {
                        confirmDialog.ShowDialog();
                        if (confirmDialog.DialogResult != DialogResult.OK)
                        {
                            break;
                        }
                    }
                    m_eButtonSelectedIndex = ButtonIndex.BTN_MATERIAL_CHANGE;
                    m_eCodeNumber = CodeNumber.CODE_MATERIAL_CHANGE;
                    m_strDescription = "CHANGE_MATERIAL";
                    break;
                case "BtnCodeMaterialDown":
                    if (m_eButtonSelectedIndex == ButtonIndex.BTN_MATERIAL_DOWN)
                    {
                        break;
                    }
                    using (var confirmDialog = new CDialogPassword(m_objDocument))
                    {
                        confirmDialog.ShowDialog();
                        if (confirmDialog.DialogResult != DialogResult.OK)
                        {
                            break;
                        }
                    }
                    m_eButtonSelectedIndex = ButtonIndex.BTN_MATERIAL_DOWN;
                    m_eCodeNumber = CodeNumber.CODE_MATERIAL_DOWN;
                    m_strDescription = "DOWN_MATERIAL";
                    break;
                default:
                    m_eButtonSelectedIndex = ButtonIndex.BTN_FINAL;
                    m_eCodeNumber = CodeNumber.CODE_FINAL;
                    m_strDescription = "";
                    break;
            }
        }

        /// <summary>
        /// 언어 변환
        /// </summary>
        /// <returns></returns>
        public bool SetChangeLanguage()
        {
            bool bReturn = false;

            do
            {
                SetButtonChangeLanguage(this.BtnTitleEqpState);
                SetButtonChangeLanguage(this.BtnTitleTPIndex);
                SetButtonChangeLanguage(this.BtnTitleDescription);
                SetButtonChangeLanguage(this.BtnTitleCode);
                SetButtonChangeLanguage(this.BtnTPIndexBM);
                SetButtonChangeLanguage(this.BtnTPIndexPM);
                SetButtonChangeLanguage(this.BtnTPIndexCM);
                SetButtonChangeLanguage(this.BtnTPIndexModelChange);
                SetButtonChangeLanguage(this.BtnTPIndexMaterial);
                SetButtonChangeLanguage(this.BtnDescriptionBreakDownManual);
                SetButtonChangeLanguage(this.BtnDescriptionRegularPM);
                SetButtonChangeLanguage(this.BtnDescriptionIrregularPM);
                SetButtonChangeLanguage(this.BtnDescriptionIrregularPMCheckEquipment);
                SetButtonChangeLanguage(this.BtnDescriptionIrregularPMCheckQuality);
                SetButtonChangeLanguage(this.BtnDescriptionImmproveEquipmentEE);
                SetButtonChangeLanguage(this.BtnDescriptionImproveProcessingEI);
                SetButtonChangeLanguage(this.BtnDescriptionSetupNewProduct);
                SetButtonChangeLanguage(this.BtnDescriptionChangeSameModel);
                SetButtonChangeLanguage(this.BtnDescriptionChangeDifferentMode);
                SetButtonChangeLanguage(this.BtnDescriptionChangeMaterial);
                SetButtonChangeLanguage(this.BtnDescriptionDownMaterial);
                SetButtonChangeLanguage(this.BtnEqpState);
                SetButtonChangeLanguage(this.BtnConfirm);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 언어 변경 적용
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(Button objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, this.Name));
        }

        /// <summary>
        /// 언어 변경 적용
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, this.Name));
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            base.SetButtonBackColor(this.BtnConfirm, m_colorNormal);
            base.SetButtonBackColor(this.BtnCodeBM, m_colorNormal);
            base.SetButtonBackColor(this.BtnCodeRegularPM, m_colorNormal);
            base.SetButtonBackColor(this.BtnCodeIrregularPMEquipment, m_colorNormal);
            base.SetButtonBackColor(this.BtnCodeIrregularPMQuality, m_colorNormal);
            base.SetButtonBackColor(this.BtnCodeCMEE, m_colorNormal);
            base.SetButtonBackColor(this.BtnCodeCMEI, m_colorNormal);
            base.SetButtonBackColor(this.BtnCodeCMNewProduct, m_colorNormal);
            base.SetButtonBackColor(this.BtnCodeChangeSameModel, m_colorNormal);
            base.SetButtonBackColor(this.BtnCodeChangeDifferentModel, m_colorNormal);
            base.SetButtonBackColor(this.BtnCodeMaterialChange, m_colorNormal);
            base.SetButtonBackColor(this.BtnCodeMaterialDown, m_colorNormal);
            base.SetButtonBackColor(this.BtnLanguage, m_colorNormal);

            base.SetButtonBackColor(BtnTitleTPIndex, m_colorLabel);
            base.SetButtonBackColor(BtnTitleCode, m_colorLabel);
            base.SetButtonBackColor(BtnTitleDescription, m_colorLabel);
            base.SetButtonBackColor(BtnTitleEqpState, m_colorLabel);
            base.SetButtonBackColor(BtnTPIndexCM, m_colorLabelSub);
            base.SetButtonBackColor(BtnTPIndexMaterial, m_colorLabelSub);
            base.SetButtonBackColor(BtnDescriptionDownMaterial, m_colorLabelSub);
            base.SetButtonBackColor(BtnDescriptionChangeDifferentMode, m_colorLabelSub);
            base.SetButtonBackColor(BtnDescriptionChangeMaterial, m_colorLabelSub);
            base.SetButtonBackColor(BtnDescriptionChangeSameModel, m_colorLabelSub);
            base.SetButtonBackColor(BtnTPIndexModelChange, m_colorLabelSub);
            base.SetButtonBackColor(BtnTPIndexPM, m_colorLabelSub);
            base.SetButtonBackColor(BtnDescriptionSetupNewProduct, m_colorLabelSub);
            base.SetButtonBackColor(BtnDescriptionIrregularPMCheckQuality, m_colorLabelSub);
            base.SetButtonBackColor(BtnDescriptionImproveProcessingEI, m_colorLabelSub);
            base.SetButtonBackColor(BtnDescriptionIrregularPMCheckEquipment, m_colorLabelSub);
            base.SetButtonBackColor(BtnDescriptionIrregularPM, m_colorLabelSub);
            base.SetButtonBackColor(BtnDescriptionImmproveEquipmentEE, m_colorLabelSub);
            base.SetButtonBackColor(BtnDescriptionRegularPM, m_colorLabelSub);
            base.SetButtonBackColor(BtnDescriptionBreakDownManual, m_colorLabelSub);
            base.SetButtonBackColor(BtnEqpState, m_colorLabelSub);
            base.SetButtonBackColor(BtnTPIndexBM, m_colorLabelSub);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    //&& false == btn.Name.Equals("")
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
        /// 타이머 시작
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            if (true == bTimer)
            {
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled = false;
            }
        }

        /// <summary>
        /// Visible 유무
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
        }

        /// <summary>
        /// 타이머 이벤트 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // 버튼 색 초기화.
            for (int iLoopCount = 0; iLoopCount < (int)ButtonIndex.BTN_FINAL; iLoopCount++)
            {
                SetButtonColor(BtnCode[iLoopCount], Color.Black, m_colorNormal);
            }

            switch (m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
            {
                case CDefine.ELanguage.LANGUAGE_KOREA:
                    base.SetButtonText(this.BtnLanguage, "KOREA");
                    break;
                case CDefine.ELanguage.LANGUAGE_CHINA:
                    base.SetButtonText(this.BtnLanguage, "CHINA");
                    break;
                case CDefine.ELanguage.LANGUAGE_ENGLISH:
                    base.SetButtonText(this.BtnLanguage, "ENGLISH");
                    break;
                case CDefine.ELanguage.LANGUAGE_VIETNAM:
                    base.SetButtonText(this.BtnLanguage, "VIETNAM");
                    break;
            }

            // 선택된 버튼만 색 변환.
            if (ButtonIndex.BTN_FINAL != m_eButtonSelectedIndex)
                SetButtonColor(BtnCode[(int)m_eButtonSelectedIndex], Color.Black, m_colorClick);
        }

        /// <summary>
        /// Confirm 버튼 이벤트.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            do
            {
                // 코드 번호 또는 Description이 공백이면 Confirm 안되게 한다.
                if ("" == m_strDescription || CodeNumber.CODE_FINAL == m_eCodeNumber)
                    break;

                mbConfirmButton = true;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                CDialogLossCode.ActiveForm.Close();
            } while (false);

        }

        /// <summary>
        /// 언어 변환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLanguage_Click(object sender, EventArgs e)
        {
            // 언어 변경
            CConfig.COptionParameter objOptionParameter = m_objDocument.m_objConfig.GetOptionParameter();
            objOptionParameter.eLanguage = (CDefine.ELanguage)(((int)objOptionParameter.eLanguage + 1) % (int)CDefine.ELanguage.LANGUAGE_FINAL);
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Language : {1} -> {2}]", "BtnLanguage_Click", m_objDocument.m_objConfig.GetOptionParameter().eLanguage.ToString(), objOptionParameter.eLanguage.ToString());
            m_objDocument.SetUpdateButtonLog(this, strLog);

            m_objDocument.m_objConfig.SaveOptionParameter(objOptionParameter);

            do
            {
                CMainFrame objMain = m_objDocument.GetMainFrame();
                if (null == objMain)
                {
                    break;
                }
                // 타이틀 변경
                CFormInterface objInterface = objMain.GetFormTitle() as CFormInterface;
                if (null == objInterface)
                {
                    break;
                }
                objInterface.SetChangeLanguage();

                // 메뉴 변경
                objInterface = objMain.GetFormMenu() as CFormInterface;
                objInterface.SetChangeLanguage();

                // 현재폼 변경.
                objInterface = objMain.GetCurrentForm() as CFormInterface;
                objInterface.SetChangeLanguage();

                // 폼 변경
                objInterface = objMain.GetFormView() as CFormInterface;
                objInterface.SetChangeLanguage();

                // 현 다이얼 로그 
                SetChangeLanguage();
            } while (false);
        }

        private void CDialogLossCode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (false == mbConfirmButton)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 폼 키 업 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogLossCode_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // 엔터나 스페이스키로 컨펌 버튼을 누른다.
                case Keys.Enter:
                case Keys.Space:
                    BtnConfirm_Click(sender, EventArgs.Empty);
                    break;
                // Alt + F4로 닫기 기능을 막는다.
                case Keys.F4:
                    if (true == e.Alt)
                    {
                        e.Handled = true;
                    }
                    break;
            }
        }
    }
}
