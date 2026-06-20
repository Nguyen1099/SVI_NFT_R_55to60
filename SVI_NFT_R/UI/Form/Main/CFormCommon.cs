using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    /// <summary>
    /// 폼에서 공통으로 쓸 함수를 처리하기 위해 빼 놓음
    /// </summary>
    public partial class CFormCommon : Form
    {
        // 컬러 정의
        public static Color m_colorRed = Color.FromArgb(220, 120, 120);
        public static Color m_colorGreen = Color.FromArgb(120, 220, 120);
        public static Color m_colorYellow = Color.FromArgb(220, 220, 120);
        public static Color m_colorBlue = Color.FromArgb(197, 217, 241);
        public static Color m_colorOrange = Color.FromArgb(255, 192, 128);
        public static Color m_colorNormal = Color.FromArgb(242, 242, 242);
        public static Color m_colorClick = Color.FromArgb(166, 166, 166);
        public static Color m_colorHighlight = Color.Ivory;
        public static Color m_colorGlow = Color.FromArgb(166, 166, 166);
        public static Color m_colorLabelCategory = Color.FromArgb(230, 230, 230);
        public static Color m_colorLabel = Color.FromArgb(173, 160, 223);
        public static Color m_colorLabelSub = Color.FromArgb(226, 221, 244);
        public static Color m_colorLabelData = Color.FromArgb(254, 254, 254);
        public static Color m_colorOn = Color.FromArgb(120, 220, 120); // Color.FromArgb(192, 255, 192);
        public static Color m_colorOff = Color.FromArgb(220, 120, 120); //Color.FromArgb(255, 192, 192);
        public static Color m_colorInputOn = Color.FromArgb(94, 173, 70);
        public static Color m_colorOutputOn = Color.FromArgb(253, 132, 17);
        public static Color m_colorInputOff = Color.FromArgb(221, 244, 228);
        public static Color m_colorOutputOff = Color.FromArgb(244, 232, 221);
        public static Color m_colorIoNone = Color.Gainsboro;
        public static Color m_colorGridSelectionFore = Color.WhiteSmoke;
        public static Color m_colorGridSelectionBack = Color.FromArgb(20, 101, 179);
        public static Color m_colorGridOddRow = Color.FromArgb(240, 240, 240);
        /// <summary>
        /// 유저 권한 레벨 or 설비 상태에 따른 리소스 상태 변경
        /// </summary>
        public delegate void DelegateSetResourceControl();
        public DelegateSetResourceControl m_delegateSetResourceControl = null;
        protected readonly Utils.AutoScale mAutoScale = new Utils.AutoScale();
        protected bool mIsAutoScaleInitialized = false;
        protected bool mIsAutoScaleEnabled = false;
        protected Font mDefualtFont = new Font("맑은 고딕", 9f, FontStyle.Regular);

        public CFormCommon()
        {
            // UI 기본 폰트 설정
            Font = mDefualtFont;
        }

        protected override void OnLoad(EventArgs e)
        {
            MaximumSize = Size.Empty;
            MinimumSize = Size.Empty;

            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (mIsAutoScaleInitialized == false)
            {
                return;
            }
            mAutoScale.Resize(this);
        }

        protected void InitializeAutoScale()
        {
            if (mIsAutoScaleInitialized == true)
            {
                Debug.Assert(false);
                return;
            }

            mAutoScale.SetInitialSize(this);
            mIsAutoScaleInitialized = true;
        }

        protected void InitializeAutoScaleByControl(Control control)
        {
            if (mIsAutoScaleInitialized == true)
            {
                Debug.Assert(false);
                return;
            }

            mAutoScale.SetInitialSizeByControl(this, control);
            mIsAutoScaleInitialized = true;
        }

        /// <summary>
        /// 버튼에 색 입힘 (나중에 Button 클래스가 다른 클래스로 대체될 수 있음)
        /// 설명 : 같은 색인 경우 넘김
        /// </summary>
        /// <param name="objBtn"></param>
        /// <param name="objForeColor"></param>
        /// <param name="objBackColor"></param>
        public void SetButtonColor(Control objBtn, Color objForeColor, Color objBackColor)
        {
            if (objForeColor != objBtn.ForeColor)
            {
                objBtn.ForeColor = objForeColor;
            }
            if (objBackColor != objBtn.BackColor)
            {
                objBtn.BackColor = objBackColor;
            }
        }

        /// <summary>
        /// 버튼에 색 입힘 (나중에 Button 클래스가 다른 클래스로 대체될 수 있음)
        /// 설명 : 같은 색인 경우 넘김
        /// </summary>
        /// <param name="objBtn"></param>
        /// <param name="objForeColor"></param>
        /// <param name="objBackColor"></param>
        /// <param name="bFlatButton"></param>
        public static void SetButtonColor(ImageButton objBtn, Color objForeColor, Color objBackColor, bool bFlatButton = false)
        {
            objBtn.TabStop = false;
            if (objForeColor != objBtn.ForeColor)
            {
                objBtn.ForeColor = objForeColor;
            }
            if (true == bFlatButton)
            {
                if (
                    objBackColor != objBtn.BaseColor
                    || objBackColor != objBtn.GlowColor
                    || objBackColor != objBtn.HighlightColor
                    )
                {
                    objBtn.BaseColor = objBackColor;
                    objBtn.GlowColor = objBackColor;
                    objBtn.HighlightColor = objBackColor;
                }
            }
            else
            {
                // 플랫 버튼 타입이 아니면 그라데이션을 넣어준다.
                if (objBackColor != objBtn.BaseColor)
                {
                    objBtn.BaseColor = objBackColor;
                }
                if (
                    m_colorGlow != objBtn.GlowColor
                    || m_colorHighlight != objBtn.HighlightColor
                    )
                {
                    objBtn.GlowColor = m_colorGlow;
                    objBtn.HighlightColor = m_colorHighlight;
                }
            }
        }

        /// <summary>
        /// 버튼에 색 입힘 (나중에 Button 클래스가 다른 클래스로 대체될 수 있음)
        /// 설명 : 같은 색인 경우 넘김
        /// </summary>
        /// <param name="objBtn"></param>
        /// <param name="objBackColor"></param>
        public void SetButtonBackColor(Control objBtn, Color objBackColor)
        {
            SetButtonColor(objBtn, objBtn.ForeColor, objBackColor);
        }

        /// <summary>
        /// 버튼에 색 입힘 (나중에 Button 클래스가 다른 클래스로 대체될 수 있음)
        /// 설명 : 같은 색인 경우 넘김
        /// </summary>
        /// <param name="objBtn"></param>
        /// <param name="objBackColor"></param>
        internal void SetButtonBackColor(ImageButton objBtn, Color objBackColor, bool bFlatButton = false)
        {
            SetButtonColor(objBtn, objBtn.ForeColor, objBackColor, bFlatButton);
        }

        /// <summary>
        /// 버튼에 색 입힘 (나중에 Button 클래스가 다른 클래스로 대체될 수 있음)
        /// 설명 : 같은 색인 경우 넘김
        /// </summary>
        /// <param name="objBtn"></param>
        /// <param name="objForeColor"></param>
        public void SetButtonForeColor(Button objBtn, Color objForeColor)
        {
            SetButtonColor(objBtn, objForeColor, objBtn.BackColor);
        }

        /// <summary>
        /// 버튼에 색 입힘 (나중에 Button 클래스가 다른 클래스로 대체될 수 있음)
        /// 설명 : 같은 색인 경우 넘김
        /// </summary>
        /// <param name="objBtn"></param>
        /// <param name="objForeColor"></param>
        internal void SetButtonForeColor(ImageButton objBtn, Color objForeColor)
        {
            SetButtonColor(objBtn, objForeColor, objBtn.BackColor);
        }

        /// <summary>
        /// 버튼에 문자열 변경 (나중에 Button 클래스가 다른 클래스로 대체될 수 있음)
        /// 설명 : 같은 문자열인 경우 넘김
        /// </summary>
        /// <param name="objBtn"></param>
        /// <param name="strText"></param>
        public void SetButtonText(Button objBtn, string strText)
        {
            if (strText != objBtn.Text)
            {
                objBtn.Text = strText;
            }
        }

        /// <summary>
        /// 버튼에 문자열 변경 (나중에 Button 클래스가 다른 클래스로 대체될 수 있음)
        /// 설명 : 같은 문자열인 경우 넘김
        /// </summary>
        /// <param name="objBtn"></param>
        /// <param name="strText"></param>
        internal void SetButtonText(ImageButton objBtn, string strText)
        {
            if (strText != objBtn.ButtonText)
            {
                objBtn.ButtonText = strText;
            }
        }

        /// <summary>
        /// 컨트롤 문자열 변경
        /// 설명 : 같은 문자열인 경우 넘김
        /// </summary>
        /// <param name="objBtn"></param>
        /// <param name="strText"></param>
        internal void SetControlText(Control objControl, string strText)
        {
            if (strText != objControl.Text)
            {
                objControl.Text = strText;
            }
        }

        internal void SetControlVisible(Control control, bool bVisible)
        {
            if (control.Visible == bVisible)
            {
                return;
            }

            control.Visible = bVisible;
        }

        /// <summary>
        /// 그리드 뷰 폰트 변경
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="strFontName"></param>
        /// <param name="dSize"></param>
        /// <param name="objFontStyle"></param>
        public void SetGridViewFont(DataGridView objGridView, string strFontName, double dSize, FontStyle objFontStyle)
        {
            objGridView.Font = new Font(strFontName, (float)dSize, objFontStyle);
        }

        /// <summary>
        /// 그리드 뷰 폰트 크기 변경
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="dSize"></param>
        public void SetGridViewFont(DataGridView objGridView, double dSize)
        {
            SetGridViewFont(objGridView, objGridView.Font.Name, dSize, objGridView.Font.Style);
        }

        /// <summary>
        /// 그리드 뷰 폰트 이름 변경
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="strFontName"></param>
        public void SetGridViewFont(DataGridView objGridView, string strFontName)
        {
            SetGridViewFont(objGridView, strFontName, (double)objGridView.Font.Size, objGridView.Font.Style);
        }

        /// <summary>
        /// 그리드 뷰 폰트 스타일 변경
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="objFontStyle"></param>
        public void SetGridViewFont(DataGridView objGridView, FontStyle objFontStyle)
        {
            SetGridViewFont(objGridView, objGridView.Font.Name, (double)objGridView.Font.Size, objFontStyle);
        }

        /// <summary>
        /// 그리드 뷰 셀 데이터 변경
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="iColumnIndex"></param>
        /// <param name="iRowIndex"></param>
        /// <param name="iData"></param>
        public void SetGridViewCellData(DataGridView objGridView, int iColumnIndex, int iRowIndex, int iData)
        {
            try
            {
                if (iData != Convert.ToInt32(objGridView[iColumnIndex, iRowIndex].Value))
                {
                    objGridView[iColumnIndex, iRowIndex].Value = iData;
                }
                // null 값 object value값은 변환 시 0값으로 처리되기에 따로 처리되어야 함.
                else if (null == objGridView[iColumnIndex, iRowIndex].Value && 0 == iData)
                {
                    objGridView[iColumnIndex, iRowIndex].Value = iData;
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 그리드 뷰 셀 데이터 변경
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="iColumnIndex"></param>
        /// <param name="iRowIndex"></param>
        /// <param name="dData"></param>
        public void SetGridViewCellData(DataGridView objGridView, int iColumnIndex, int iRowIndex, double dData)
        {
            try
            {
                if (dData != Convert.ToDouble(objGridView[iColumnIndex, iRowIndex].Value))
                {
                    objGridView[iColumnIndex, iRowIndex].Value = dData;
                }
                // null 값 object value값은 변환 시 0값으로 처리되기에 따로 처리되어야 함.
                else if (null == objGridView[iColumnIndex, iRowIndex].Value && 0.0 == dData)
                {
                    objGridView[iColumnIndex, iRowIndex].Value = dData;
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 그리드 뷰 셀 데이터 변경
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="iColumnIndex"></param>
        /// <param name="iRowIndex"></param>
        /// <param name="strData"></param>
        public void SetGridViewCellData(DataGridView objGridView, int iColumnIndex, int iRowIndex, string strData)
        {
            try
            {
                if (null == objGridView[iColumnIndex, iRowIndex].Value)
                {
                    objGridView[iColumnIndex, iRowIndex].Value = strData;
                }
                else if (strData != objGridView[iColumnIndex, iRowIndex].Value.ToString())
                {
                    objGridView[iColumnIndex, iRowIndex].Value = strData;
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 그리드 뷰 배경색 변경
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="iColumnIndex"></param>
        /// <param name="iRowIndex"></param>
        /// <param name="objBackColor"></param>
        public void SetGridViewCellBackColor(DataGridView objGridView, int iColumnIndex, int iRowIndex, Color objBackColor)
        {
            try
            {
                objGridView[iColumnIndex, iRowIndex].Style.BackColor = objBackColor;
                objGridView[iColumnIndex, iRowIndex].Style.SelectionBackColor = objBackColor;
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 그리드 뷰 기본 스타일 초기화
        /// 설명 : 그리드 뷰 생성 시 기본 제약 조건으로 사용합니다. 해당 부분이 수정되면 다른 부분에도 영향이 갑니다.
        /// </summary>
        /// <param name="objGridView"></param>
        /// <returns></returns>
        public bool InitializeGridView(DataGridView objGridView)
        {
            bool bReturn = false;

            do
            {
                // 더블 버퍼링으로 속성 변경
                SetDoubleBuffered(objGridView, true);
                // 그리드 뷰 배경색
                objGridView.BackgroundColor = Color.White;
                // 그리드 뷰 칼럼 사이즈 조정
                objGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                // 그리드 뷰 행, 열 사이즈 유저 조정 막음
                objGridView.AllowUserToResizeRows = false;
                objGridView.AllowUserToResizeColumns = false;
                // 그리드 뷰 행 머리글 없앰
                objGridView.RowHeadersVisible = false;
                // 그리드 뷰 홀수행 색 변경
                objGridView.AlternatingRowsDefaultCellStyle.BackColor = m_colorGridOddRow;
                // 첫 행 포커스 해제
                objGridView.ClearSelection();
                // 마지막 행 제거
                objGridView.AllowUserToAddRows = false;
                // 그리드 선택 색 변경
                objGridView.DefaultCellStyle.SelectionForeColor = m_colorGridSelectionFore;
                objGridView.DefaultCellStyle.SelectionBackColor = m_colorGridSelectionBack;
                objGridView.UserDeletingRow += ObjGridView_UserDeletingRow;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        private void ObjGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// 그리드 뷰 더블 버퍼링 속성 변경
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="bSetting"></param>
        public void SetDoubleBuffered(DataGridView objGridView, bool bSetting)
        {
            Type objType = objGridView.GetType();
            PropertyInfo objPropertyInfo = objType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            objPropertyInfo.SetValue(objGridView, bSetting, null);
        }

        /// <summary>
        /// 리치 텍스트 박스 기본 스타일 초기화
        /// 설명 : 리치 텍스트 박스 생성 시 기본 제약 조건으로 사용합니다. 해당 부분이 수정되면 다른 부분에도 영향이 갑니다.
        /// </summary>
        /// <param name="objRich"></param>
        /// <returns></returns>
        public bool InitializeRichTextBox(RichTextBox objRich)
        {
            bool bReturn = false;

            do
            {
                // 읽기만 가능
                objRich.ReadOnly = true;
                // 복수 라인
                objRich.Multiline = true;
                // 스크롤바 세로
                objRich.ScrollBars = RichTextBoxScrollBars.Vertical;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 리치 텍스트 박스 폰트 변경
        /// 설명 : Control 단위로 빼줄 걸 그랬나.
        /// </summary>
        /// <param name="objRich"></param>
        /// <param name="strFontName"></param>
        /// <param name="dSize"></param>
        /// <param name="objFontStyle"></param>
        public void SetRichTextBoxFont(RichTextBox objRich, string strFontName, double dSize, FontStyle objFontStyle)
        {
            objRich.Font = new Font(strFontName, (float)dSize, objFontStyle);
            objRich.SelectionFont = new Font(strFontName, (float)dSize, objFontStyle);
        }

        /// <summary>
        /// 리치 텍스트 박스 폰트 크기 변경
        /// </summary>
        /// <param name="objRich"></param>
        /// <param name="dSize"></param>
        public void SetRichTextBoxFont(RichTextBox objRich, double dSize)
        {
            SetRichTextBoxFont(objRich, objRich.Font.Name, dSize, objRich.Font.Style);
        }

        /// <summary>
        /// 리치 텍스트 박스 폰트 이름 변경
        /// </summary>
        /// <param name="objRich"></param>
        /// <param name="strFontName"></param>
        public void SetRichTextBoxFont(RichTextBox objRich, string strFontName)
        {
            SetRichTextBoxFont(objRich, strFontName, (double)objRich.Font.Size, objRich.Font.Style);
        }

        /// <summary>
        /// 리치 텍스트 박스 폰트 스타일 변경
        /// </summary>
        /// <param name="objRich"></param>
        /// <param name="objFontStyle"></param>
        public void SetRichTextBoxFont(RichTextBox objRich, FontStyle objFontStyle)
        {
            SetRichTextBoxFont(objRich, objRich.Font.Name, (double)objRich.Font.Size, objFontStyle);
        }

        /// <summary>
        /// 리치 텍스트 박스 텍스트 삽입 ~ 가장 최신이 위로 갱신
        /// </summary>
        /// <param name="objRich"></param>
        /// <param name="queueText"></param>
        public void SetRichText(RichTextBox objRich, Queue<string> queueText)
        {
            // queue 쌓이는 Max 255로
            if (255 <= queueText.Count)
            {
                queueText.Dequeue();
            }

            string strText = "";
            string[] strTextArray = queueText.ToArray();
            for (int iLoopText = strTextArray.Length - 1; iLoopText >= 0; iLoopText--)
            {
                strText += strTextArray[iLoopText] + "\n";
            }
            objRich.Text = strText;
        }

        /// <summary>
        /// 메뉴를 갖는 폼들이 메뉴 버튼을 동적으로 생성하기 위한 함수
        /// </summary>
        /// <param name="objButton"></param>
        /// <param name="objParent"></param>
        /// <param name="strButtonName"></param>
        /// <param name="iButtonWidth"></param>
        /// <param name="iWhiteSpace"></param>
        /// <param name="eventButton"></param>
        public void SetDynamicMenuButton(Button[] objButton, Control objParent, string[] strButtonName, int iButtonWidth, int iWhiteSpace, EventHandler eventButton)
        {
            for (int iLoopButton = 0; iLoopButton < strButtonName.Length; iLoopButton++)
            {
                objButton[iLoopButton] = new SpeedButton();
                objButton[iLoopButton].Name = iLoopButton.ToString();
                objButton[iLoopButton].Text = strButtonName[iLoopButton];
                objButton[iLoopButton].Parent = objParent;
                objButton[iLoopButton].Size = new Size(iButtonWidth - iWhiteSpace, objParent.Height - 3);
                objButton[iLoopButton].BackColor = Color.White;
                objButton[iLoopButton].FlatStyle = FlatStyle.Flat;
                objButton[iLoopButton].Click += eventButton;
                // 버튼 오른쪽으로 열거
                objButton[iLoopButton].Location = new Point(iLoopButton * iButtonWidth + 3, 4);
                objButton[iLoopButton].Font = new Font(mDefualtFont.FontFamily, 8f);
            }
        }

        /// <summary>
        /// 메뉴를 갖는 폼들이 메뉴 버튼을 동적으로 생성하기 위한 함수
        /// </summary>
        /// <param name="objButton"></param>
        /// <param name="objParent"></param>
        /// <param name="strButtonName"></param>
        /// <param name="iButtonWidth"></param>
        /// <param name="iWhiteSpace"></param>
        /// <param name="eventButton"></param>
        internal void SetDynamicMenuButton(ImageButton[] objButton, Control objParent, string[] strButtonName, int iButtonWidth, int iWhiteSpace, EventHandler eventButton)
        {
            for (int iLoopButton = 0; iLoopButton < strButtonName.Length; iLoopButton++)
            {
                objButton[iLoopButton] = new ImageButton();
                objButton[iLoopButton].Name = iLoopButton.ToString();
                objButton[iLoopButton].Text = strButtonName[iLoopButton];
                objButton[iLoopButton].Parent = objParent;
                objButton[iLoopButton].Size = new Size(iButtonWidth - iWhiteSpace, objParent.Height);
                objButton[iLoopButton].BackColor = Color.White;
                objButton[iLoopButton].Click += eventButton;
                // 버튼 오른쪽으로 열거
                objButton[iLoopButton].Location = new Point(iLoopButton * iButtonWidth, 0);
            }
        }

        /// <summary>
        /// 컨트롤 내 버튼 활성화 상태 변경
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="bEnable"></param>
        public void SetControlButtonEnable(Control.ControlCollection collection, bool bEnable)
        {
            try
            {
                var buttons = collection.GetChildControlListByType(typeof(SpeedButton));
                foreach (SpeedButton btn in buttons)
                {
                    btn.Enabled = bEnable;
                }
                var imageButtons = collection.GetChildControlListByType(typeof(ImageButton));
                foreach (ImageButton btn in imageButtons)
                {
                    btn.Enabled = bEnable;
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 버튼 활성화 상태 변경
        /// </summary>
        /// <param name="objBtn"></param>
        /// <param name="bEnable"></param>
        public void SetButtonEnable(Button objBtn, bool bEnable)
        {
            objBtn.Enabled = bEnable;
        }

        /// <summary>
        /// 이미지 버튼 활성화 상태 변경
        /// </summary>
        /// <param name="objBtn"></param>
        /// <param name="bEnable"></param>
        internal void SetButtonEnable(ImageButton objBtn, bool bEnable)
        {
            objBtn.Enabled = bEnable;
        }

        internal void SetControlTextFitting(Control control, float minFontSize, float maxFontSize)
        {
            Size textSize = TextRenderer.MeasureText(control.Text, control.Font);
            if (Math.Abs(textSize.Width - control.Width) < 1d)
            {
                return;
            }
            float fontSize = control.Font.Size;
            float findDirection = control.Width < textSize.Width ? -1f : 1f;
            float factor = 0.2f;
            while (true)
            {
                fontSize += factor * findDirection;
                if (fontSize <= 0f)
                {
                    return;
                }

                using (Font font = new Font(control.Font.FontFamily, fontSize, control.Font.Style))
                {
                    textSize = TextRenderer.MeasureText(control.Text, font);
                }
                if (findDirection == -1f
                    && control.Width > textSize.Width
                    )
                {
                }
                else if (findDirection == 1f
                    && control.Width < textSize.Width
                    )
                {
                    // +방향으로 검색하는 경우 직전 스탭의 값으로 설정함
                    fontSize -= factor;
                }
                else
                {
                    continue;
                }

                Utils.InRange(ref fontSize, minFontSize, maxFontSize);
                control.Font = new Font(control.Font.FontFamily, fontSize, control.Font.Style);
                break;
            }
        }

        /// <summary>
        /// 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NonClickableButton_BackColorChanged(object sender, EventArgs e)
        {
            var speedButton = sender as SpeedButton;
            if (null != speedButton)
            {
                speedButton.FlatAppearance.MouseOverBackColor = speedButton.BackColor;
                speedButton.FlatAppearance.MouseDownBackColor = speedButton.BackColor;
            }
        }

        protected void bindingSelectionItem_Click(object sender, EventArgs e)
        {
            BindingSelection tagIndex = ((Control)sender).Tag as BindingSelection;
            tagIndex.Document.SetUpdateButtonLog(this, $"[{tagIndex.Sender.Name}] [{true}]");
            try
            {
                tagIndex.Select();
            }
            finally
            {
                tagIndex.Document.SetUpdateButtonLog(this, $"[{tagIndex.Sender.Name}] [{false}]");
            }
        }

        protected class BindingText
        {
            public CDocument Document { get; private set; } = null;
            public Control Sender { get; private set; } = null;
            public object Tag { get; private set; } = null;
            private readonly Func<BindingText, string> mGetTextOrNull;
            private readonly Action<string> mSetTextOrNull;

            public BindingText(CDocument document, Control sender, object tag, Func<BindingText, string> getTextOrNull = null, Action<string> setTextOrNull = null)
            {
                Document = document;
                Sender = sender;
                Tag = tag;
                mGetTextOrNull = getTextOrNull;
                mSetTextOrNull = setTextOrNull;
            }

            public void UpdateControlText()
            {
                string getText = GetText();
                if (getText != Sender.Text)
                {
                    Sender.Text = getText;
                }
            }

            public string GetText()
            {
                string result = mGetTextOrNull?.Invoke(this);
                return result ?? string.Empty;
            }

            public void SetText(string setValue)
            {
                mSetTextOrNull?.Invoke(setValue);
            }
        }

        protected class BindingSelection
        {
            public CDocument Document { get; private set; } = null;
            public Control Sender { get; private set; } = null;
            public object Tag { get; private set; } = null;
            public bool IsSelected => mGetIsSelected.Invoke(this);
            private readonly Func<BindingSelection, bool> mGetIsSelected;
            private readonly Action<BindingSelection> mSetSelect;

            public BindingSelection(CDocument Document, Control sender, object tag, Func<BindingSelection, bool> getIsSelected, Action<BindingSelection> setSelectOrNull = null)
            {
                Sender = sender;
                Tag = tag;
                mGetIsSelected = getIsSelected;
                mSetSelect = setSelectOrNull;
            }

            public void Select() => mSetSelect?.Invoke(this);
        }
    }
}