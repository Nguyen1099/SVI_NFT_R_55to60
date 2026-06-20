using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogIndividualAnalogCalibration : CFormCommon, CFormInterface
    {
        private CDocument mDocument;
        private int mChannelIndex;
        private CConfig.AnalogCalibrationParameter mSelectParameter;
        private CConfig.AnalogCalibrationParameter mCopyParameter;
        private double mMeasureValue = 0;
        private DataTable mCalData = new DataTable();
        private bool mUsingIndividualCannel = false;

        public CDialogIndividualAnalogCalibration(CDocument document)
        {
            InitializeComponent();

            mDocument = document;
        }

        public void SetTimer(bool bTimer)
        {
            timer.Enabled = bTimer;
        }

        public void SetVisible(bool bVisible)
        {
        }

        public bool SetChangeLanguage()
        {
            //SetButtonChangeLanguage();
            return true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Initialize();
        }

        protected override void OnClosed(EventArgs e)
        {
            DeInitialize();
            base.OnClosed(e);
        }

        private bool Initialize()
        {
            // 폼 초기화
            if (InitializeForm() == false)
            {
                return false;
            }
            return true;
        }

        private void DeInitialize()
        {
            SetTimer(false);
        }

        private bool InitializeForm()
        {
            ucTeachVacuum.Visible = false;
            var ioParams = mDocument.m_objConfig.GetIOParameter().objIOParameter;
            var query = ioParams.Values
                .Where(item => item.eIOType == CConfig.CIOInitializeParameter.EIOType.IO_TYPE_AI)
                .Select(item => item.strIOName);
            cbbCalAnalogIndex.Items.AddRange(query.Cast<object>().ToArray());
            // 디바이스 종류 초기화
            cbbSettingDeviceType.Items.Add("None");
            cbbSettingDeviceType.Items.Add("Type1: 압력계 [kPa]");
            cbbSettingDeviceType.Items.Add("Type2: 압력계 [MPa]");
            cbbSettingDeviceType.Items.Add("Type3: 회전수 [RPM]");
            // 캘리브레이션 테이블 초기화
            mCalData.Columns.Add("아날로그 값", typeof(double));
            mCalData.Columns.Add("실제 값", typeof(double));
            grdCalData.DataSource = mCalData;
            initializeGridView(grdCalData);

            cbbCalAnalogIndex.SelectedIndex = 0;

            displayMessage(string.Empty);

            // 폼 중앙에서 생성
            CenterToParent();
            // 버튼 색상 정의
            SetButtonColor();
            // 언어 변경
            SetChangeLanguage();
            // 타이머 제어
            timer.Interval = 100;
            timer.Enabled = true;
            return true;


        }

        private bool initializeGridView(DataGridView objGridView)
        {
            bool bReturn = false;

            do
            {
                // 그리드 뷰 기본 스타일 초기화
                if (false == InitializeGridView(objGridView))
                {
                    break;
                }
                // 그리드 뷰 ReadOnly
                objGridView.ReadOnly = true;
                // 그리드 뷰 다중 선택 x
                objGridView.MultiSelect = false;
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        private void SetButtonColor()
        {
            SetButtonBackColor(btnSettingSlope, m_colorLabelData);
            SetButtonBackColor(btnSettingYIntercept, m_colorLabelData);
            SetButtonBackColor(btnCalMeasure, m_colorLabelData);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(nameof(btnSettingSlope))
                    && false == btn.Name.Equals(nameof(btnSettingYIntercept))
                    && false == btn.Name.Equals(nameof(btnCalMeasure))
                    )
                {
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                    btn.BackColorChanged += NonClickableButton_BackColorChanged;
                    btn.Cursor = Cursors.Default;
                }
            }
        }

        private void SetButtonChangeLanguage(Button objButton)
        {
            SetButtonText(objButton, mDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            SetButtonText(objButton, mDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            // Vacuum UI 업데이트
            if (ucTeachVacuum.Visible == true)
            {
                ucTeachVacuum.UpdateUI();
            }
            // [확인용] 아날로그 값: 0.000 V
            string title = "[확인용] 아날로그 값:";
            string analogName = cbbCalAnalogIndex.Text;
            string value;
            string unit;
            if (string.IsNullOrWhiteSpace(analogName) == true)
            {
                value = $"-";
                unit = string.Empty;
            }
            else
            {
                value = $"{getAnalogValue(analogName):0.000}";
                unit = "V";
            }
            setControlText(lblCalAnalog, $"{title} {value} {unit}");

            // [확인용] 실제 값: 0.000 kPa
            title = "[확인용] 실제 값:";
            if (string.IsNullOrWhiteSpace(analogName) == true)
            {
                value = $"-";
                unit = string.Empty;
            }
            else
            {
                switch (mSelectParameter.Type)
                {
                    case CConfig.AnalogCalibrationParameter.EType.Type1:
                        value = $"{getMeasureValue(analogName):0.000}";
                        unit = "kPa";
                        break;
                    case CConfig.AnalogCalibrationParameter.EType.Type2:
                        value = $"{getMeasureValue(analogName):0.000}";
                        unit = "MPa";
                        break;
                    case CConfig.AnalogCalibrationParameter.EType.Type3:
                        value = $"{getMeasureValue(analogName):0.000}";
                        unit = "RPM";
                        break;
                    default:
                        value = "-";
                        unit = string.Empty;
                        break;
                }
            }
            setControlText(lblCalMeasure, $"{title} {value} {unit}");

            setControlEnabled(btnSettingPaste, mCopyParameter != null);

            setControlText(btnSettingSlope, $"{mSelectParameter.Slope:0.000}");
            setControlText(btnSettingYIntercept, $"{mSelectParameter.YIntercept:0.000}");
            setControlText(btnCalMeasure, $"{mMeasureValue:0.000} {unit}");

            // 채널 사용 버튼 텍스트 업데이트
            updateUsingChannelButtonText();
            SetButtonBackColor(btnUsingIndividualChannel, mSelectParameter.UsingIndividualChannelAirPressureCalibration ? m_colorOn : m_colorRed);


        }

        private void setControlText(Control control, string text)
        {
            if (control.Text != text)
            {
                control.Text = text;
            }
        }

        private void setControlEnabled(Control control, bool bEnabled)
        {
            if (control.Enabled != bEnabled)
            {
                control.Enabled = bEnabled;
            }
        }

        private void selectAnalogIndex(int index)
        {
            // 설정값 로드
            mSelectParameter = mDocument.m_objConfig.GetIndividualChannelAnalogCalibrationParameterAirPressure(mChannelIndex).DeepClone();
            // 셋팅 UI 업데이트
            updateSettingUI();
            // 캘리브레이션 UI 업데이트
            // updateCalibrationUI();
            // 캘리브레이션 데이터 초기화
            mCalData.Rows.Clear();
        }

        private void updateSettingUI()
        {
            cbbCalAnalogIndex.SelectedItem = cbbCalAnalogIndex.Items.Contains(mSelectParameter.ChannelName) ? mSelectParameter.ChannelName : null;
            cbbSettingDeviceType.SelectedIndex = (int)mSelectParameter.Type > cbbSettingDeviceType.Items.Count ? 0 : (int)mSelectParameter.Type;
        }

        private void updateCalibrationUI()
        {
            cbbCalAnalogIndex.Items.Clear();
            var ioParams = mDocument.m_objConfig.GetIOParameter().objIOParameter;
            var query = ioParams.Values
                .Where(item => item.eIOType == CConfig.CIOInitializeParameter.EIOType.IO_TYPE_AI)
                .Select(item => item.strIOName)
                .Cast<object>()
                .ToArray();
            cbbCalAnalogIndex.Items.AddRange(query);
        }

        private double getAnalogValue(string ioName)
        {
            double result = 0d;
            mDocument.m_objProcessMain.m_objIO.HLGetAnalog(ioName, ref result);
            return result;
        }

        private double getMeasureValue(string ioName)
        {
            double analogValue = getAnalogValue(ioName);
            return analogValue * mSelectParameter.Slope + mSelectParameter.YIntercept;
        }

        private void displayErrorMessage(string message)
        {
            lblErrorMessage.Text = message;
            lblMessage.Text = string.Empty;

            mDocument.SetUpdateButtonLog(this, $"Error: {message}");
        }

        private void displayMessage(string message)
        {
            lblErrorMessage.Text = string.Empty;
            lblMessage.Text = message;
        }

        private void btnCalAdd_Click(object sender, EventArgs e)
        {
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");

            if (string.IsNullOrWhiteSpace(cbbCalAnalogIndex.Text) == true)
            {
                displayErrorMessage($"아날로그 인덱스를 선택해주세요.");
                return;
            }

            double analogValue = getAnalogValue(cbbCalAnalogIndex.Text);

            mCalData.Rows.Add(analogValue, mMeasureValue);
            displayMessage($"캘리브레이션 데이터를 추가했습니다.");
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] Cal Item Add (Analog:{analogValue}, Measure:{mMeasureValue})");
        }

        private void btnCalDelete_Click(object sender, EventArgs e)
        {
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
            if (grdCalData.SelectedRows.Count == 0)
            {
                displayErrorMessage($"삭제할 데이터를 선택해주세요.");
                return;
            }

            int index = grdCalData.SelectedRows[0].Index;
            mCalData.Rows.RemoveAt(index);
            displayMessage($"캘리브레이션 데이터를 삭제했습니다.");
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] Cal Item Remove (Index:{index})");
        }

        private void btnCalClear_Click(object sender, EventArgs e)
        {
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
            mCalData.Rows.Clear();
            displayMessage($"캘리브레이션 데이터를 모두 삭제했습니다.");
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] Cal Item Clear");
        }

        private void btnCalCalc_Click(object sender, EventArgs e)
        {
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
            if (mCalData.Rows.Count < 3)
            {
                displayErrorMessage($"최소 2개 이상의 데이터가 필요합니다.");
                return;
            }

            double rSquared, yIntercept, slope;
            double[] xVals = new double[mCalData.Rows.Count];
            double[] yVals = new double[mCalData.Rows.Count];
            for (int i = 0; i < mCalData.Rows.Count; i++)
            {
                xVals[i] = (double)mCalData.Rows[i][0];
                yVals[i] = (double)mCalData.Rows[i][1];
            }
            linearRegression(xVals, yVals, out rSquared, out yIntercept, out slope);

            if (
                double.IsNaN(yIntercept)
                || double.IsNaN(slope)
                )
            {
                displayErrorMessage($"캘리브레이션에 실패 했습니다. 데이터가 올바른지 확인해주세요.");
                return;
            }

            mSelectParameter.YIntercept = yIntercept;
            mSelectParameter.Slope = slope;
            displayMessage($"캘리브레이션에 성공했습니다.");
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] Cal Success (YIntercept:{yIntercept}, Slope:{slope})");
        }

        /// <summary>
        /// Fits a line to a collection of (x,y) points.
        /// </summary>
        /// <param name="xVals">The x-axis values.</param>
        /// <param name="yVals">The y-axis values.</param>
        /// <param name="rSquared">The r^2 value of the line.</param>
        /// <param name="yIntercept">The y-intercept value of the line (i.e. y = ax + b, yIntercept is b).</param>
        /// <param name="slope">The slop of the line (i.e. y = ax + b, slope is a).</param>
        private void linearRegression(double[] xVals, double[] yVals, out double rSquared, out double yIntercept, out double slope)
        {
            if (xVals.Length != yVals.Length)
            {
                throw new Exception("Input values should be with the same length.");
            }

            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double sumCodeviates = 0;

            for (var i = 0; i < xVals.Length; i++)
            {
                var x = xVals[i];
                var y = yVals[i];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }

            var count = xVals.Length;
            var ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            var ssY = sumOfYSq - ((sumOfY * sumOfY) / count);

            var rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            var rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
            var sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            var meanX = sumOfX / count;
            var meanY = sumOfY / count;
            var dblR = rNumerator / Math.Sqrt(rDenom);

            rSquared = dblR * dblR;
            yIntercept = meanY - ((sCo / ssX) * meanX);
            slope = sCo / ssX;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
            checkParameterChanged();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
            checkParameterChanged();
            DialogResult = DialogResult.OK;
        }

        private void btnUsingIndividualChannel_Click(object sender, EventArgs e)
        {
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
            mUsingIndividualCannel = !mUsingIndividualCannel;
            updateUsingChannelButtonText();
            mSelectParameter.UsingIndividualChannelAirPressureCalibration = mUsingIndividualCannel;
        }

        private void updateUsingChannelButtonText()
        {
            if (mSelectParameter.UsingIndividualChannelAirPressureCalibration == false)
            {
                btnUsingIndividualChannel.Text = "NOT USE";
            }
            else
            {
                btnUsingIndividualChannel.Text = "USE";
            }
        }

        private void checkParameterChanged()
        {
            if (mSelectParameter != null)
            {
                var originalParam = mDocument.m_objConfig.GetIndividualChannelAnalogCalibrationParameterAirPressure(mChannelIndex);
                if (
                    originalParam.UsingIndividualChannelAirPressureCalibration != mSelectParameter.UsingIndividualChannelAirPressureCalibration
                    || originalParam.ChannelName != mSelectParameter.ChannelName
                    || originalParam.ChannelIndex != mSelectParameter.ChannelIndex
                    || originalParam.Slope != mSelectParameter.Slope
                    || originalParam.YIntercept != mSelectParameter.YIntercept
                    || originalParam.Type != mSelectParameter.Type
                    )
                {
                    if (MessageBox.Show("변경된 파라메터가 있습니다. 변경된 사항을 저장하시겠습니까?", "Analog Calibration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (originalParam.UsingIndividualChannelAirPressureCalibration != mSelectParameter.UsingIndividualChannelAirPressureCalibration) mDocument.SetUpdateButtonLog(this, $"[Save] UsingIndividualChannelAirPressureCalibration: {originalParam.UsingIndividualChannelAirPressureCalibration} => {mSelectParameter.UsingIndividualChannelAirPressureCalibration}");
                        if (originalParam.ChannelName != mSelectParameter.ChannelName) mDocument.SetUpdateButtonLog(this, $"[Save] ChannelName: {originalParam.ChannelName} => {mSelectParameter.ChannelName}");
                        if (originalParam.ChannelIndex != mSelectParameter.ChannelIndex) mDocument.SetUpdateButtonLog(this, $"[Save] ChannelIndex: {originalParam.ChannelIndex} => {mSelectParameter.ChannelIndex}");
                        if (originalParam.Type != mSelectParameter.Type) mDocument.SetUpdateButtonLog(this, $"[Save] Type: {originalParam.Type} => {mSelectParameter.Type}");
                        if (originalParam.Slope != mSelectParameter.Slope) mDocument.SetUpdateButtonLog(this, $"[Save] Slope: {originalParam.Slope} => {mSelectParameter.Slope}");
                        if (originalParam.YIntercept != mSelectParameter.YIntercept) mDocument.SetUpdateButtonLog(this, $"[Save] YIntercept: {originalParam.YIntercept} => {mSelectParameter.YIntercept}");

                        mDocument.m_objConfig.SaveIndividualAnalogCalibrationParameterAirPressure(mSelectParameter);
                    }
                }
            }
        }

        private void cbbSettingDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int deviceType = cbbSettingDeviceType.SelectedIndex;
            mSelectParameter.Type = (CConfig.AnalogCalibrationParameter.EType)deviceType;
        }

        private void btnSettingCopy_Click(object sender, EventArgs e)
        {
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
            mCopyParameter = mSelectParameter.DeepClone();
            displayMessage($"현재 설정 값을 저장했습니다.");
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] Copy Setting");
        }

        private void btnSettingPaste_Click(object sender, EventArgs e)
        {
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
            if (mCopyParameter == null)
            {
                displayErrorMessage($"복사된 파라메터가 없습니다.");
                return;
            }

            mSelectParameter = mCopyParameter.DeepClone();
            updateSettingUI();
            updateCalibrationUI();
            displayMessage($"설정 값을 붙여넣었습니다.");
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] Paste Setting");
        }

        private void btnSettingSlope_Click(object sender, EventArgs e)
        {
            using (var keyPad = new FormKeyPad(mSelectParameter.Slope))
            {
                if (keyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                mSelectParameter.Slope = keyPad.m_dResultValue;
            }
        }

        private void btnSettingYIntercept_Click(object sender, EventArgs e)
        {
            using (var keyPad = new FormKeyPad(mSelectParameter.YIntercept))
            {
                if (keyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                mSelectParameter.YIntercept = keyPad.m_dResultValue;
            }
        }

        private void btnCalMeasure_Click(object sender, EventArgs e)
        {
            using (var keyPad = new FormKeyPad(mMeasureValue))
            {
                if (keyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                mMeasureValue = keyPad.m_dResultValue;
            }
        }

        private void cbbCalAnalogIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            mChannelIndex = cbbCalAnalogIndex.SelectedIndex;
            selectAnalogIndex(mChannelIndex);

            string analogName = cbbCalAnalogIndex.Text;
            mSelectParameter.ChannelName = analogName;

            var searchVacuum = mDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum
                .Where(item => item.Value.GetInitializeParameter().strVacuumAnalogInputIO.Contains(analogName));
            if (searchVacuum.Count() == 0)
            {
                ucTeachVacuum.Visible = false;
                return;
            }
            ucTeachVacuum.Initialize(this, mDocument, searchVacuum.FirstOrDefault().Key, 0, () => false, () => true);
            ucTeachVacuum.Visible = true;
        }
    }
}
