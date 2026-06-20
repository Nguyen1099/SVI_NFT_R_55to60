using System;
using System.Drawing;
using System.Windows.Forms;

namespace UiAsset
{
    /// <summary>
    /// 두 가지 상태를 나타내는 UI 정보
    /// </summary>
    [Serializable]
    public class IoIndicatorSet
    {
        /// <summary>
        /// 현재 상태를 읽어오는 함수
        /// </summary>
        public Func<bool> GetIo { get; private set; } = DoNothing;
        /// <summary>
        /// State를 꾸며줄 텍스트
        /// </summary>
        public string StateSubText { get; set; } = string.Empty;
        /// <summary>
        /// Title에 다국어를 불러 올 때 Database에 FORM_NAME을 의미함
        /// </summary>
        public string TitleUiGroup { get; private set; } = string.Empty;
        /// <summary>
        /// Title에 다국어를 불러 올 때 Database에 ID을 의미함
        /// </summary>
        public string TitleUiid { get; private set; } = string.Empty;
        /// <summary>
        /// True 상태에 다국어를 불러 올 때 Database에 FORM_NAME을 의미함
        /// </summary>
        public string StateTrueUiGroup { get; set; } = string.Empty;
        /// <summary>
        /// True 상태에 다국어를 불러 올 때 Database에 ID을 의미함
        /// </summary>
        public string StateTrueUiid { get; set; } = string.Empty;
        /// <summary>
        /// True 상태에 글자색
        /// </summary>
        public Color StateTrueForeColor { get; set; } = Color.Black;
        /// <summary>
        /// True 상태에 배경색
        /// </summary>
        public Color StateTrueBackColor { get; set; } = Color.Green;
        /// <summary>
        /// False 상태에 다국어를 불러 올 때 Database에 FORM_NAME을 의미함
        /// </summary>
        public string StateFalseUiGroup { get; set; } = string.Empty;
        /// <summary>
        /// False 상태에 다국어를 불러 올 때 Database에 ID을 의미함
        /// </summary>
        public string StateFalseUiid { get; set; } = string.Empty;
        /// <summary>
        /// False 상태에 글자색
        /// </summary>
        public Color StateFalseForeColor { get; set; } = Color.Black;
        /// <summary>
        /// False 상태에 배경색
        /// </summary>
        public Color StateFalseBackColor { get; set; } = Color.Red;
        private string mResourceTitle = string.Empty;
        private string mResourceStateTrue = string.Empty;
        private string mResourceStateFalse = string.Empty;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="uiGroup">Title에 다국어를 불러 올 때 Database에 FORM_NAME (Title, StateTrue, StateFalse에 동일하게 설정함)</param>
        /// <param name="titleUiid">Title에 다국어를 불러 올 때 Database에 ID</param>
        /// <param name="getIo">현재 상태를 읽어오는 함수</param>
        public void Initialize(string uiGroup, string titleUiid, Func<bool> getIo)
        {
            TitleUiGroup = uiGroup;
            StateTrueUiGroup = uiGroup;
            StateFalseUiGroup = uiGroup;
            TitleUiid = titleUiid;
            GetIo = getIo;
        }

        /// <summary>
        /// True 상태에서 텍스트와 배경색 설정
        /// </summary>
        /// <param name="uiid">True 상태에 다국어를 불러 올 때 Database에 ID</param>
        /// <param name="backColor">True 상태에 배경색</param>
        public void InitializeStateTrue(string uiid, Color backColor)
        {
            StateTrueUiid = uiid;
            StateTrueBackColor = backColor;
        }

        /// <summary>
        /// False 상태에서 텍스트와 배경색 설정
        /// </summary>
        /// <param name="uiid">False 상태에 다국어를 불러 올 때 Database에 ID</param>
        /// <param name="backColor">False 상태에 배경색</param>
        public void InitializeStateFalse(string uiid, Color backColor)
        {
            StateFalseUiid = uiid;
            StateFalseBackColor = backColor;
        }

        /// <summary>
        /// 다국어 업데이트
        /// </summary>
        /// <param name="getDatabaseUiText">데이터 베이스에서 다국어를 불러오는 함수 string (string id, string formName)</param>
        public void SetChangeLanguage(Func<string, string, string> getDatabaseUiText)
        {
            mResourceTitle = getDatabaseUiText.Invoke(TitleUiid, TitleUiGroup);
            if (string.IsNullOrWhiteSpace(StateTrueUiid) == false
                && string.IsNullOrWhiteSpace(StateTrueUiGroup) == false
                )
            {
                mResourceStateTrue = getDatabaseUiText.Invoke(StateTrueUiid, StateTrueUiGroup);
            }
            if (string.IsNullOrWhiteSpace(StateFalseUiid) == false
                && string.IsNullOrWhiteSpace(StateFalseUiGroup) == false
                )
            {
                mResourceStateFalse = getDatabaseUiText.Invoke(StateFalseUiid, StateFalseUiGroup);
            }
        }

        /// <summary>
        /// 현재 상태를 읽어와서 UI를 업데이트함
        /// </summary>
        public void Update(Control title, Control indicator)
        {
            SetControlText(title, mResourceTitle);

            bool bState = GetIo.Invoke();
            SetControlText(indicator, bState == true ? $"{mResourceStateTrue}{StateSubText}" : $"{mResourceStateFalse}{StateSubText}");
            SetControlBackColor(indicator, bState == true ? StateTrueBackColor : StateFalseBackColor);
            SetControlForeColor(indicator, bState == true ? StateTrueForeColor : StateFalseForeColor);
        }

        private void SetControlText(Control control, string text)
        {
            if (control.Text != text)
            {
                control.Text = text;
            }
        }

        private void SetControlBackColor(Control control, Color color)
        {
            if (control.BackColor != color)
            {
                control.BackColor = color;
            }
        }

        private void SetControlForeColor(Control control, Color color)
        {
            if (control.ForeColor != color)
            {
                control.ForeColor = color;
            }
        }

        private static bool DoNothing()
        {
            return false;
        }
    }
}
