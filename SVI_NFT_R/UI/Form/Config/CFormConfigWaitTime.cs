using SVI_NFT_R.UI.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormConfigWaitTime : CFormCommon, CFormInterface
    {
        private class CategorySet
        {
            public string Uiid { get; set; }
            public string Name { get; set; }
            public readonly List<WaitTimeSet> WaitTimes = new List<WaitTimeSet>();
        }

        private class WaitTimeSet
        {
            public string Uiid { get; set; }
            public string Name { get; set; }
            public IWaitTimeSetting Setting { get; set; }
        }

        private readonly List<CategorySet> mCategoryItems = new List<CategorySet>(16);
        private int mCategoryIndex = 0;
        private int mWaitTimesPageIndex = 0;
        private int mWaitTimesPageCount = 1;
        private ImageButton[] mControlBtnCategoryItems;
        private UcCell[] mControlUcWaitTimeItems;
        private readonly CDocument m_objDocument;

        public CFormConfigWaitTime(CDocument objDocument)
        {
            m_objDocument = objDocument;
            InitializeComponent();

            // Display 영역에 맞게 Form 크기 피팅
            Size = pnlDisplayArea.Size;
            BackColor = pnlDisplayArea.BackColor;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeAutoScale();
            Initialize();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            DeInitialize();
            base.OnFormClosed(e);
        }

        private bool Initialize()
        {
            if (InitializeForm() == false)
            {
                return false;
            }

            return true;
        }

        private void DeInitialize()
        {
        }

        private bool InitializeForm()
        {
            // 폼 사이즈 피팅
            pnlDisplayArea.Location = new Point(0, 0);
            Size = pnlDisplayArea.Size;

            // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
            m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
            // 컨트롤 리스트 초기화
            {
                mControlBtnCategoryItems = PnlCategory.Controls.GetChildControlListByType(typeof(ImageButton))
                    .Cast<ImageButton>()
                    .ToArray();
                mControlUcWaitTimeItems = PnlWaitTimes.Controls.GetChildControlListByType(typeof(UcCell))
                    .Cast<UcCell>()
                    .ToArray();
            }
            // 설비 상태 점검 그룹 리스트 초기화
            SetCategoryItems();
            // 버튼 색상 정의
            SetButtonColor();
            // 버튼 태그 정의
            SetButtonTag();
            // 타이머 외부에서 제어
            timer.Interval = 100;
            timer.Enabled = false;

            return true;
        }

        private void SetCategoryItems()
        {
            foreach (var category in Config.WaitTime.Settings.Values.GroupBy(i => i.Category))
            {
                CategorySet categoryItem = new CategorySet();
                categoryItem.Uiid = $"Category>{category.Key}";
                categoryItem.WaitTimes.AddRange(category.Select(i => new WaitTimeSet() { Uiid = $"Item>{i.Category}>{i.Name}", Setting = i }));
                mCategoryItems.Add(categoryItem);
            }

            // 첫 번째 그룹 선택
            SetCategoryItemIndex(0);
        }

        private void SetButtonTag()
        {
            // 카테고리 아이템 인덱스 초기화
            {
                int index = 0;
                foreach (var item in mControlBtnCategoryItems)
                {
                    item.Tag = index++;
                }
            }

            // 아이템 인덱스 초기화
            {
                int index = 0;
                foreach (var item in mControlUcWaitTimeItems)
                {
                    item.Tag = index++;
                }
            }
        }

        private void SetButtonColor()
        {
            SetButtonBackColor(BtnTitle, m_colorLabel);
            SetButtonBackColor(BtnTitleCategory, m_colorLabelSub);
            SetButtonBackColor(BtnTitleWaitTimes, m_colorLabelSub);

            SetButtonBackColor(BtnNaviWaitTimesFirst, m_colorNormal);
            SetButtonBackColor(BtnNaviWaitTimesPrivious, m_colorNormal);
            SetButtonBackColor(BtnNaviWaitTimesNext, m_colorNormal);
            SetButtonBackColor(BtnNaviWaitTimesLast, m_colorNormal);

            foreach (var item in mControlUcWaitTimeItems)
            {
                SetButtonBackColor(item.BtnIndicator, m_colorLabelSub);
                SetButtonBackColor(item.BtnCellData, m_colorLabelData);
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
        }

        private void SetResourceControl()
        {
            // 현재 유저 권한 레벨 받아옴
            CUserInformation objUserInformation = m_objDocument.GetUserInformation();

            // 런타임 중에도 변경 가능함
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        SetControlButtonEnable(Controls, false);
                        BtnNaviWaitTimesFirst.Enabled = true;
                        BtnNaviWaitTimesPrivious.Enabled = true;
                        BtnNaviWaitTimesNext.Enabled = true;
                        BtnNaviWaitTimesLast.Enabled = true;
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                        {
                            SetControlButtonEnable(Controls, false);
                            BtnNaviWaitTimesFirst.Enabled = true;
                            BtnNaviWaitTimesPrivious.Enabled = true;
                            BtnNaviWaitTimesNext.Enabled = true;
                            BtnNaviWaitTimesLast.Enabled = true;
                        }
                        else
                        {
                            SetControlButtonEnable(Controls, true);
                        }
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        SetControlButtonEnable(Controls, true);
                        break;
                    default:
                        break;
                }
            }
        }

        public bool SetChangeLanguage()
        {
            bool bReturn = false;

            do
            {
                // 데이터 테이블에서 일치하는 ID에 해당하는 TEXT를 불러옴
                SetButtonChangeLanguage(BtnTitle);
                SetButtonChangeLanguage(BtnTitleCategory);
                SetButtonChangeLanguage(BtnTitleWaitTimes);

                SetButtonChangeLanguage(BtnNaviWaitTimesFirst);
                SetButtonChangeLanguage(BtnNaviWaitTimesPrivious);
                SetButtonChangeLanguage(BtnNaviWaitTimesNext);
                SetButtonChangeLanguage(BtnNaviWaitTimesLast);

                foreach (var categoryItem in mCategoryItems)
                {
                    categoryItem.Name = m_objDocument.GetDatabaseUIText(categoryItem.Uiid, Name);
                    foreach (var item in categoryItem.WaitTimes)
                    {
                        item.Name = m_objDocument.GetDatabaseUIText(item.Uiid, Name);
                    }
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

        public void SetTimer(bool bTimer)
        {
            timer.Enabled = bTimer;
        }

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

        private void timer_Tick(object sender, EventArgs e)
        {
            // Update Category Items
            for (int i = 0; i < mControlBtnCategoryItems.Length; i++)
            {
                bool bVisible = i < mCategoryItems.Count;
                if (mControlBtnCategoryItems[i].Visible != bVisible)
                {
                    mControlBtnCategoryItems[i].Visible = bVisible;
                }
                if (bVisible == false)
                {
                    continue;
                }

                SetButtonText(mControlBtnCategoryItems[i], mCategoryItems[i].Name);
                SetButtonBackColor(mControlBtnCategoryItems[i], mCategoryIndex == i ? m_colorClick : m_colorNormal);
            }

            // Update Wait Time Items
            for (int i = 0; i < mControlUcWaitTimeItems.Length; i++)
            {
                var selectCategoryItems = mCategoryItems[mCategoryIndex].WaitTimes;
                int index = i + (mControlUcWaitTimeItems.Length * mWaitTimesPageIndex);
                bool bVisible = index < selectCategoryItems.Count;
                if (mControlUcWaitTimeItems[i].Visible != bVisible)
                {
                    mControlUcWaitTimeItems[i].Visible = bVisible;
                }
                if (bVisible == false)
                {
                    continue;
                }

                SetButtonText(mControlUcWaitTimeItems[i].BtnIndicator, selectCategoryItems[index].Name);
                SetButtonText(mControlUcWaitTimeItems[i].BtnCellData, selectCategoryItems[index].Setting.ToString());
            }
            SetControlText(LblNaviWaitTimesPage, $"PAGE ( {mWaitTimesPageIndex + 1} / {mWaitTimesPageCount} )");
        }

        private void SetCategoryItemIndex(int itemIndex)
        {
            int oldCategoryIndex = mCategoryIndex;
            Utils.InRange(ref itemIndex, 0, mCategoryItems.Count - 1);
            mCategoryIndex = itemIndex;

            WriteLog($"{mCategoryItems[oldCategoryIndex].Uiid} -> {mCategoryItems[mCategoryIndex].Uiid}");

            SetWaitTimesPageIndex(0);
        }

        private void SetWaitTimesPageIndex(int pageIndex)
        {
            int pageDisplayItemCount = mControlUcWaitTimeItems.Length;
            int itemCount = mCategoryItems[mCategoryIndex].WaitTimes.Count;
            mWaitTimesPageCount = itemCount / pageDisplayItemCount;
            if (itemCount % pageDisplayItemCount != 0)
            {
                mWaitTimesPageCount++;
            }

            Utils.InRange(ref pageIndex, 0, mWaitTimesPageCount - 1);

            mWaitTimesPageIndex = pageIndex;

            // 페이지 변경시 즉시 UI 업데이트
            PnlWaitTimes.SuspendLayout();
            timer_Tick(timer, EventArgs.Empty);
            PnlWaitTimes.ResumeLayout();

            WriteLog($"PAGE({mWaitTimesPageIndex + 1}/{mWaitTimesPageCount})");
        }

        private void BtnNaviWaitTimesFirst_Click(object sender, EventArgs e)
        {
            SetWaitTimesPageIndex(0);
        }

        private void BtnNaviWaitTimesPrivious_Click(object sender, EventArgs e)
        {
            int setIndex = mWaitTimesPageIndex - 1;
            SetWaitTimesPageIndex(setIndex);
        }

        private void BtnNaviWaitTimesNext_Click(object sender, EventArgs e)
        {
            int setIndex = mWaitTimesPageIndex + 1;
            SetWaitTimesPageIndex(setIndex);
        }

        private void BtnNaviWaitTimesLast_Click(object sender, EventArgs e)
        {
            SetWaitTimesPageIndex(mWaitTimesPageCount);
        }

        private void BtnCategory_Click(object sender, EventArgs e)
        {
            SetCategoryItemIndex(Convert.ToInt32(((Control)sender).Tag));
        }

        private void ucCellWaitTimeEdit_Click(object sender, EventArgs e)
        {
            int itemIndex = Convert.ToInt32(((Control)sender).Tag) + mWaitTimesPageIndex * mControlUcWaitTimeItems.Length;
            IWaitTimeSetting setting = mCategoryItems[mCategoryIndex].WaitTimes[itemIndex].Setting;
            string initialValue = setting.ToString();
            try
            {
                using (var dialog = new FormKeyPad(setting.SettingFromUnit, mCategoryItems[mCategoryIndex].WaitTimes[itemIndex].Name))
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    setting.SettingFromUnit = dialog.m_dResultValue;
                    setting.Save();
                }
            }
            finally
            {
                WriteLog($"{mCategoryItems[mCategoryIndex].WaitTimes[itemIndex].Uiid}: {initialValue} -> {setting}");
            }
        }

        private void WriteLog(string message, [CallerMemberName] string callerName = "")
        {
            m_objDocument.SetUpdateButtonLog(this, $"[{callerName}] {message}");
        }
    }
}