using SVI_NFT_R.UI.UserControls;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormSetupAlignVision : CFormCommon, CFormInterface
    {
        private const int PAGE_ITEM_COUNT = 4;
        private readonly CConfig.CAlignOptionParameter[] m_objAlignOptionParameter;
        private readonly CDocument m_objDocument;
        private readonly UcSettingAlign[] mSettingAligns;
        private readonly CDefine.EAlign[] mAlignEnums;
        private readonly PagingHandler<CDefine.EAlign> mPagingHandler;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormSetupAlignVision(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            InitializeComponent();

            mSettingAligns = Controls.GetChildControlListByType(typeof(UcSettingAlign))
                .Cast<UcSettingAlign>()
                .ToArray();

            mAlignEnums = Enum.GetValues(typeof(CDefine.EAlign))
                .Cast<CDefine.EAlign>()
                .ToArray();

            m_objAlignOptionParameter = new CConfig.CAlignOptionParameter[mAlignEnums.Length];

            for (int i = 0; i < mSettingAligns.Length; i++)
            {
                var ctrl = mSettingAligns[i];
                ctrl.Tag = i;
                ctrl.BtnSelectModeUseVision.Click += BtnSelectModeUseVision_Click;
                ctrl.BtnInterlockX.Click += BtnInterlockX_Click;
                ctrl.BtnInterlockY.Click += BtnInterlockY_Click;
                ctrl.BtnInterlockT.Click += BtnInterlockT_Click;
                ctrl.BtnInterlockScore.Click += BtnInterlockScore_Click;
                ctrl.BtnRetryCount.Click += BtnRetryCount_Click;
                ctrl.BtnToleranceX.Click += BtnToleranceX_Click;
                ctrl.BtnToleranceY.Click += BtnToleranceY_Click;
                ctrl.BtnToleranceT.Click += BtnToleranceT_Click;
            }

            mPagingHandler = new PagingHandler<CDefine.EAlign>(mAlignEnums, PAGE_ITEM_COUNT);
            mPagingHandler.OnPageIndexChanged += PagingHandle_OnPageIndexChanged;
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
        private void CFormSetupDevice_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormSetupDevice_FormClosed(object sender, FormClosedEventArgs e)
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
                if (null == m_objDocument)
                {
                    break;
                }
                // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
                base.m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);

                UpdateData();
                // 버튼 색상 정의
                SetButtonColor();

                mPagingHandler.SetFirstPage(bForcedUpdate: true);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 데이터 업데이트
        /// </summary>
        public void UpdateData()
        {
            // 비전 설정 파라미터 로드
            for (int i = 0; i < mAlignEnums.Length; i++)
            {
                m_objAlignOptionParameter[i] = m_objDocument.m_objConfig.GetAlignOptionParameter(mAlignEnums[i]).DeepClone();
            }
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            // 버튼 색 변경
            base.SetButtonBackColor(this.BtnTitle, m_colorLabel);
            foreach (var ctrl in mSettingAligns)
            {
                base.SetButtonBackColor(ctrl.BtnTitle, m_colorLabelSub);
                base.SetButtonBackColor(ctrl.BtnSubTitleInterlockX, m_colorLabelSub);
                base.SetButtonBackColor(ctrl.BtnSubTitleInterlockY, m_colorLabelSub);
                base.SetButtonBackColor(ctrl.BtnSubTitleInterlockT, m_colorLabelSub);
                base.SetButtonBackColor(ctrl.BtnSubTitleInterlockScore, m_colorLabelSub);
                base.SetButtonBackColor(ctrl.BtnSubTitleRetryCount, m_colorLabelSub);
                base.SetButtonBackColor(ctrl.BtnSubTitleToleranceX, m_colorLabelSub);
                base.SetButtonBackColor(ctrl.BtnSubTitleToleranceY, m_colorLabelSub);
                base.SetButtonBackColor(ctrl.BtnSubTitleToleranceT, m_colorLabelSub);
                base.SetButtonBackColor(ctrl.BtnInterlockX, m_colorLabelData);
                base.SetButtonBackColor(ctrl.BtnInterlockY, m_colorLabelData);
                base.SetButtonBackColor(ctrl.BtnInterlockT, m_colorLabelData);
                base.SetButtonBackColor(ctrl.BtnInterlockScore, m_colorLabelData);
                base.SetButtonBackColor(ctrl.BtnRetryCount, m_colorLabelData);
                base.SetButtonBackColor(ctrl.BtnToleranceX, m_colorLabelData);
                base.SetButtonBackColor(ctrl.BtnToleranceY, m_colorLabelData);
                base.SetButtonBackColor(ctrl.BtnToleranceT, m_colorLabelData);
                base.SetButtonBackColor(ctrl.BtnSelectModeUseVision, m_colorNormal);
            }
            base.SetButtonBackColor(this.BtnSave, m_colorNormal);
            base.SetButtonBackColor(BtnPaging, m_colorLabelData);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(nameof(UcSettingAlign.BtnInterlockX))
                    && false == btn.Name.Equals(nameof(UcSettingAlign.BtnInterlockY))
                    && false == btn.Name.Equals(nameof(UcSettingAlign.BtnInterlockT))
                    && false == btn.Name.Equals(nameof(UcSettingAlign.BtnInterlockScore))
                    && false == btn.Name.Equals(nameof(UcSettingAlign.BtnRetryCount))
                    && false == btn.Name.Equals(nameof(UcSettingAlign.BtnToleranceX))
                    && false == btn.Name.Equals(nameof(UcSettingAlign.BtnToleranceY))
                    && false == btn.Name.Equals(nameof(UcSettingAlign.BtnToleranceT))
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
                base.SetControlButtonEnable(this.Controls, false);
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        base.SetControlButtonEnable(this.Controls, false);
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        base.SetControlButtonEnable(this.Controls, true);
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        base.SetControlButtonEnable(this.Controls, true);
                        break;
                    default:
                        break;
                }
            }

            BtnPagingFirst.Enabled = true;
            BtnPagingPrevious.Enabled = true;
            BtnPagingNext.Enabled = true;
            BtnPagingLast.Enabled = true;
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
                SetButtonChangeLanguage(this.BtnTitle);
                for (int i = 0; i < mSettingAligns.Length; ++i)
                {
                    var ctrl = mSettingAligns[i];
                    if (ctrl.Visible == false)
                    {
                        continue;
                    }
                    int index = (int)mPagingHandler.PageItems[i];
                    string title = m_objDocument.GetDatabaseUIText($"{nameof(BtnTitle)}[{mAlignEnums[index]}]", Name);
                    ctrl.Text = title;
                    SetButtonChangeLanguage(ctrl.BtnSelectModeUseVision);
                    SetButtonChangeLanguage(ctrl.BtnSubTitleInterlockX);
                    SetButtonChangeLanguage(ctrl.BtnSubTitleInterlockY);
                    SetButtonChangeLanguage(ctrl.BtnSubTitleInterlockT);
                    SetButtonChangeLanguage(ctrl.BtnSubTitleInterlockScore);
                    SetButtonChangeLanguage(ctrl.BtnSubTitleRetryCount);
                    SetButtonChangeLanguage(ctrl.BtnSubTitleToleranceX);
                    SetButtonChangeLanguage(ctrl.BtnSubTitleToleranceY);
                    SetButtonChangeLanguage(ctrl.BtnSubTitleToleranceT);
                }
                SetButtonChangeLanguage(this.BtnSave);
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
                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                m_objDocument.GetMainFrame().SetCurrentForm(this);
                UpdateData();
            }
        }

        private void PagingHandle_OnPageIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < mSettingAligns.Length; i++)
            {
                var ctrl = mSettingAligns[i];
                if (ctrl.Visible != i < mPagingHandler.PageItems.Count)
                {
                    ctrl.Visible = i < mPagingHandler.PageItems.Count;
                }
            }
            SetChangeLanguage();
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < mSettingAligns.Length; i++)
            {
                var ctrl = mSettingAligns[i];
                if (ctrl.Visible == false)
                {
                    continue;
                }

                int index = (int)mPagingHandler.PageItems[i];
                var param = m_objAlignOptionParameter[index];
                SetButtonBackColor(ctrl.BtnSelectModeUseVision, param.bUseVision ? m_colorClick : m_colorNormal);
                SetButtonText(ctrl.BtnInterlockX, $"{param.dAlignInterlockX:0.000} ( {CDefine.UNIT_MILLIMETER} )");
                SetButtonText(ctrl.BtnInterlockY, $"{param.dAlignInterlockY:0.000} ( {CDefine.UNIT_MILLIMETER} )");
                SetButtonText(ctrl.BtnInterlockT, $"{param.dAlignInterlockT:0.000} ( {CDefine.UNIT_ANGULAR} )");
                SetButtonText(ctrl.BtnInterlockScore, $"{param.iVisionScore:0.000} ( % )");
                SetButtonText(ctrl.BtnRetryCount, $"{param.RetryCount:0}");
                SetButtonText(ctrl.BtnToleranceX, $"{param.ToleranceX:0.000} ( {CDefine.UNIT_MILLIMETER} )");
                SetButtonText(ctrl.BtnToleranceY, $"{param.ToleranceY:0.000} ( {CDefine.UNIT_MILLIMETER} )");
                SetButtonText(ctrl.BtnToleranceT, $"{param.ToleranceT:0.000} ( {CDefine.UNIT_ANGULAR} )");

                SetButtonBackColor(ctrl.BtnSubTitleToleranceX, param.RetryCount == 0 ? m_colorClick : m_colorLabelSub);
                SetButtonBackColor(ctrl.BtnSubTitleToleranceY, param.RetryCount == 0 ? m_colorClick : m_colorLabelSub);
                SetButtonBackColor(ctrl.BtnSubTitleToleranceT, param.RetryCount == 0 ? m_colorClick : m_colorLabelSub);
                SetButtonBackColor(ctrl.BtnToleranceX, param.RetryCount == 0 ? m_colorClick : m_colorLabelData);
                SetButtonBackColor(ctrl.BtnToleranceY, param.RetryCount == 0 ? m_colorClick : m_colorLabelData);
                SetButtonBackColor(ctrl.BtnToleranceT, param.RetryCount == 0 ? m_colorClick : m_colorLabelData);
            }

            // 페이지 정보 업데이트
            {
                SetButtonText(BtnPaging, $"( {mPagingHandler} )");
            }
        }

        private void BtnSelectModeUseVision_Click(object sender, EventArgs e)
        {
            int index = (int)mPagingHandler.PageItems[(int)((Control)sender).Tag];
            m_objAlignOptionParameter[index].bUseVision = !m_objAlignOptionParameter[index].bUseVision;
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] SetValue({index}): {m_objAlignOptionParameter[index].bUseVision}");
        }

        private void BtnInterlockX_Click(object sender, EventArgs e)
        {
            int index = (int)mPagingHandler.PageItems[(int)((Control)sender).Tag];
            using (var keyPad = new FormKeyPad(m_objAlignOptionParameter[index].dAlignInterlockX, 0d, 9999d))
            {
                if (keyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                m_objAlignOptionParameter[index].dAlignInterlockX = keyPad.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] SetValue({index}): {m_objAlignOptionParameter[index].dAlignInterlockX}");
            }
        }

        private void BtnInterlockY_Click(object sender, EventArgs e)
        {
            int index = (int)mPagingHandler.PageItems[(int)((Control)sender).Tag];
            using (var keyPad = new FormKeyPad(m_objAlignOptionParameter[index].dAlignInterlockY, 0d, 9999d))
            {
                if (keyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                m_objAlignOptionParameter[index].dAlignInterlockY = keyPad.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] SetValue({index}): {m_objAlignOptionParameter[index].dAlignInterlockY}");
            }
        }

        private void BtnInterlockT_Click(object sender, EventArgs e)
        {
            int index = (int)mPagingHandler.PageItems[(int)((Control)sender).Tag];
            using (var keyPad = new FormKeyPad(m_objAlignOptionParameter[index].dAlignInterlockT, 0d, 9999d))
            {
                if (keyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                m_objAlignOptionParameter[index].dAlignInterlockT = keyPad.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] SetValue({index}): {m_objAlignOptionParameter[index].dAlignInterlockT}");
            }
        }

        private void BtnInterlockScore_Click(object sender, EventArgs e)
        {
            int index = (int)mPagingHandler.PageItems[(int)((Control)sender).Tag];
            using (var keyPad = new FormKeyPad(m_objAlignOptionParameter[index].iVisionScore, 0d, 100d))
            {
                if (keyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                m_objAlignOptionParameter[index].iVisionScore = keyPad.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] SetValue({index}): {m_objAlignOptionParameter[index].iVisionScore}");
            }
        }

        private void BtnRetryCount_Click(object sender, EventArgs e)
        {
            int index = (int)mPagingHandler.PageItems[(int)((Control)sender).Tag];

            if (m_objAlignOptionParameter[index].CanUsingRetry == false)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.CAN_NOT_DO_THE_ACTION_PARAM2, "EDIT", "NOT SURPPORT");
                return;
            }

            using (var keyPad = new FormKeyPad(m_objAlignOptionParameter[index].RetryCount, 0d, 5d))
            {
                if (keyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                m_objAlignOptionParameter[index].RetryCount = Convert.ToInt32(keyPad.m_dResultValue);
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] SetValue({index}): {m_objAlignOptionParameter[index].RetryCount}");
            }
        }

        private void BtnToleranceX_Click(object sender, EventArgs e)
        {
            int index = (int)mPagingHandler.PageItems[(int)((Control)sender).Tag];
            using (var keyPad = new FormKeyPad(m_objAlignOptionParameter[index].ToleranceX, 0.001d, 5d))
            {
                if (keyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                m_objAlignOptionParameter[index].ToleranceX = keyPad.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] SetValue({index}): {m_objAlignOptionParameter[index].ToleranceX}");
            }
        }

        private void BtnToleranceY_Click(object sender, EventArgs e)
        {
            int index = (int)mPagingHandler.PageItems[(int)((Control)sender).Tag];
            using (var keyPad = new FormKeyPad(m_objAlignOptionParameter[index].ToleranceY, 0.001d, 5d))
            {
                if (keyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                m_objAlignOptionParameter[index].ToleranceY = keyPad.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] SetValue({index}): {m_objAlignOptionParameter[index].ToleranceY}");
            }
        }

        private void BtnToleranceT_Click(object sender, EventArgs e)
        {
            int index = (int)mPagingHandler.PageItems[(int)((Control)sender).Tag];
            using (var keyPad = new FormKeyPad(m_objAlignOptionParameter[index].ToleranceT, 0.001d, 5d))
            {
                if (keyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                m_objAlignOptionParameter[index].ToleranceT = keyPad.m_dResultValue;
                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] SetValue({index}): {m_objAlignOptionParameter[index].ToleranceT}");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE))
            {
                return;
            }

            for (int i = 0; i < mAlignEnums.Length; i++)
            {
                m_objDocument.m_objConfig.SaveAlignOptionParameter(m_objAlignOptionParameter[i], mAlignEnums[i]);
            }
            UpdateData();
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");

            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.SAVING_IS_COMPLETE);
        }

        private void BtnPagingFirst_Click(object sender, EventArgs e)
        {
            mPagingHandler.SetFirstPage();
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] PageIndex: {mPagingHandler.PageIndex}");
        }

        private void BtnPagingPrevious_Click(object sender, EventArgs e)
        {
            mPagingHandler.SetPreviousPage();
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] PageIndex: {mPagingHandler.PageIndex}");
        }

        private void BtnPagingNext_Click(object sender, EventArgs e)
        {
            mPagingHandler.SetNextPage();
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] PageIndex: {mPagingHandler.PageIndex}");
        }

        private void BtnPagingLast_Click(object sender, EventArgs e)
        {
            mPagingHandler.SetLastPage();
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] PageIndex: {mPagingHandler.PageIndex}");
        }
    }
}