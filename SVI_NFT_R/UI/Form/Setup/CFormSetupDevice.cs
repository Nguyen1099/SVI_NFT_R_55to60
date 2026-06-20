using HLDevice.Abstract;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormSetupDevice : CFormCommon, CFormInterface
    {
        private CDocument m_objDocument;
        private CDefine.ETemperatureController mSelectTemperatureIndex = 0;
        private CDefine.EMeasurement mSelectGapMeasurementIndex = 0;
        private CDefine.EMcu mSelectFfuIndex = 0;
        private CDefine.EGms mSelectGmsIndex = 0;
        private CDefine.EMcr mSelectMcrIndex = 0;
        private CDefine.EPowerMeter mSelectPowerMeterIndex = 0;
        private CDefine.EAngularSensor mSelectAngularSensorIndex = 0;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormSetupDevice(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
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
                if (false == InitializeForm()) break;

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
                // 버튼 색상 정의
                SetButtonColor();

                lstFfuChannel.Items.Clear();
                for (int i = 0; i < 32; i++)
                {
                    lstFfuChannel.Items.Add($"CH{i + 1}");
                }
                lstFfuChannel.SelectedIndex = 0;

                lstTemperatureChannel.Items.Clear();
                for (int i = 0; i < 32; i++)
                {
                    lstTemperatureChannel.Items.Add($"CH{i + 1}");
                }
                lstTemperatureChannel.SelectedIndex = 0;

                lstAngleSensorChannel.Items.Clear();
                for (int i = 0; i < 16; i++)
                {
                    lstAngleSensorChannel.Items.Add($"CH{i + 1}: ");
                }
                lstAngleSensorChannel.SelectedIndex = 0;

                lstGapMeasurementChannel.Items.Clear();
                for (int i = 0; i < 32; i++)
                {
                    lstGapMeasurementChannel.Items.Add($"SLAVE{i + 1}");
                }
                lstGapMeasurementChannel.SelectedIndex = 0;

                // 설비를 구성하고 있는 디바이스만 표시한다
                PnlFfuBase.Visible = Enum.GetNames(typeof(CDefine.EMcu)).Length > 0;
                PnlPowerMeterBase.Visible = Enum.GetNames(typeof(CDefine.EPowerMeter)).Length > 0;
                pnlTemperatureBase.Visible = Enum.GetNames(typeof(CDefine.ETemperatureController)).Length > 0;
                pnlAngleSensorBase.Visible = Enum.GetNames(typeof(CDefine.EAngularSensor)).Length > 0;
                pnlGapMeasurementBase.Visible = Enum.GetNames(typeof(CDefine.EMeasurement)).Length > 0;
                pnlMcrBase.Visible = Enum.GetNames(typeof(CDefine.EMcr)).Length > 0;
                pnlGmsBase.Visible = Enum.GetNames(typeof(CDefine.EGms)).Length > 0;

                string[] columnHeader = Data.SerialReader<double>.GetHeaderForCsvReport().Split(',');
                InitializeGridView(GridSerialCommStatistics, columnHeader.Skip(1).Take(columnHeader.Length - 1).ToArray());
                GridSerialCommStatistics.Rows.Add(CProcessMain.SerialReaders.Items.Count);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            // 버튼 색 변경
            SetButtonBackColor(BtnTitle, m_colorLabel);

            SetButtonBackColor(BtnTitleMCR, m_colorLabel);
            SetButtonBackColor(BtnTitleMcrGroupIndex, m_colorLabelSub);
            SetButtonBackColor(BtnTitleMcrIP, m_colorLabelSub);
            SetButtonBackColor(BtnMcrIP, m_colorLabelData);
            SetButtonBackColor(BtnTitleMcrPort, m_colorLabelSub);
            SetButtonBackColor(BtnMcrPort, m_colorLabelData);
            SetButtonBackColor(BtnMcrGroupIndex, m_colorLabelData);

            SetButtonBackColor(BtnTitleTemperature, m_colorLabel);
            SetButtonBackColor(BtnTitleTemperatureGroupIndex, m_colorLabelSub);
            SetButtonBackColor(BtnTemperatureGroupIndex, m_colorLabelData);
            SetButtonBackColor(BtnTitleTemperatureSerialPort, m_colorLabelSub);
            SetButtonBackColor(BtnTemperatureSerialPort, m_colorLabelData);
            SetButtonBackColor(BtnTitleTemperatureBaudrate, m_colorLabelSub);
            SetButtonBackColor(BtnTemperatureBaudrate, m_colorLabelData);

            SetButtonBackColor(BtnTitleGms, m_colorLabel);
            SetButtonBackColor(BtnTitleGmsGroupIndex, m_colorLabelSub);
            SetButtonBackColor(BtnTitleGmsBaudrate, m_colorLabelSub);
            SetButtonBackColor(BtnTitleGmsSerialPort, m_colorLabelSub);
            SetButtonBackColor(BtnGmsGroupIndex, m_colorLabelData);
            SetButtonBackColor(BtnGmsBaudrate, m_colorLabelData);
            SetButtonBackColor(BtnGmsSerialPort, m_colorLabelData);

            SetButtonBackColor(BtnTitleAngleSensor, m_colorLabel);
            SetButtonBackColor(BtnTitleAngleSensorGroupIndex, m_colorLabelSub);
            SetButtonBackColor(BtnTitleAngleSensorBaudrate, m_colorLabelSub);
            SetButtonBackColor(BtnTitleAngleSensorSerialPort, m_colorLabelSub);
            SetButtonBackColor(BtnAngleSensorGroupIndex, m_colorLabelData);
            SetButtonBackColor(BtnAngleSensorBaudrate, m_colorLabelData);
            SetButtonBackColor(BtnAngleSensorSerialPort, m_colorLabelData);

            SetButtonBackColor(BtnTitlePowerMeter, m_colorLabel);
            SetButtonBackColor(BtnTitlePowerMeterGroupIndex, m_colorLabelSub);
            SetButtonBackColor(BtnPowerMeterGroupIndex, m_colorLabelData);
            SetButtonBackColor(BtnTitlePowerMeterIP, m_colorLabelSub);
            SetButtonBackColor(BtnPowerMeterIP, m_colorLabelData);
            SetButtonBackColor(BtnTitlePowerMeterPort, m_colorLabelSub);
            SetButtonBackColor(BtnPowerMeterPort, m_colorLabelData);

            SetButtonBackColor(BtnTitleFFU, m_colorLabel);
            SetButtonBackColor(BtnTitleFFUGroupIndex, m_colorLabelSub);
            SetButtonBackColor(BtnTitleFFUSerialPort, m_colorLabelSub);
            SetButtonBackColor(BtnFFUSerialPort, m_colorLabelData);
            SetButtonBackColor(BtnTitleFFUBaudrate, m_colorLabelSub);
            SetButtonBackColor(BtnFFUGroupIndex, m_colorLabelData);
            SetButtonBackColor(BtnFFUBaudrate, m_colorLabelData);

            SetButtonBackColor(BtnTitleGapMeasurement, m_colorLabel);
            SetButtonBackColor(BtnTitleGapMeasurementGroupIndex, m_colorLabelSub);
            SetButtonBackColor(BtnTitleGapMeasurementSerialPort, m_colorLabelSub);
            SetButtonBackColor(BtnTitleGapMeasurementBaudrate, m_colorLabelSub);
            SetButtonBackColor(BtnGapMeasurementSerialPort, m_colorLabelData);
            SetButtonBackColor(BtnGapMeasurementBaudrate, m_colorLabelData);
            SetButtonBackColor(BtnGapMeasurementGourpIndex, m_colorLabelData);

            SetButtonBackColor(BtnTitleSerialCommStatistics, m_colorLabel);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnTemperatureGroupIndex.Name)
                    && false == btn.Name.Equals(BtnGapMeasurementGourpIndex.Name)
                    && false == btn.Name.Equals(BtnGmsGroupIndex.Name)
                    && false == btn.Name.Equals(BtnMcrGroupIndex.Name)
                    && false == btn.Name.Equals(BtnFFUGroupIndex.Name)
                    && false == btn.Name.Equals(BtnPowerMeterGroupIndex.Name)
                    && false == btn.Name.Equals(BtnAngleSensorGroupIndex.Name)
                    && false == btn.Name.Equals(btnReloadAnalogCalibrationFile.Name)
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
            // 권한 레벨에 따라 원하는 방향으로 사용
            switch (objUserInformation.m_eAuthorityLevel)
            {
                case CDefine.EUserAuthorityLevel.OPERATOR:
                    base.SetControlButtonEnable(this.Controls, false);
                    break;
                case CDefine.EUserAuthorityLevel.ENGINEER:
                    base.SetControlButtonEnable(this.Controls, false);
                    break;
                case CDefine.EUserAuthorityLevel.MASTER:
                    base.SetControlButtonEnable(this.Controls, true);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <returns></returns>
        public bool SetChangeLanguage()
        {
            return true;
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
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.SvidMonitoringLock = true;
            }
            else
            {
                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.SvidMonitoringLock = false;
            }
        }

        private bool InitializeGridView(DataGridView grid, string[] strColumnName)
        {
            if (InitializeGridView(grid) == false)
            {
                return false;
            }
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            grid.ReadOnly = true;
            for (int iLoopColumn = 0; iLoopColumn < strColumnName.Length; iLoopColumn++)
            {
                grid.Columns.Add(strColumnName[iLoopColumn], strColumnName[iLoopColumn]);
                grid.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                grid.Columns[iLoopColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                switch (iLoopColumn)
                {
                    // Name
                    case 0:
                        grid.Columns[iLoopColumn].Width = Convert.ToInt32((grid.Width) * 0.35d);
                        break;

                    // Alarm Duration
                    case 3:
                        grid.Columns[iLoopColumn].Width = Convert.ToInt32((grid.Width) * 0.2d);
                        break;

                    default:
                        grid.Columns[iLoopColumn].Width = Convert.ToInt32((grid.Width) * 0.135d);
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            CConfig objConfig = m_objDocument.m_objConfig;
            // MCR
            if (pnlMcrBase.Visible == true)
            {
                SetButtonText(BtnMcrGroupIndex, mSelectMcrIndex.ToString());
                SetButtonText(BtnMcrIP, objConfig.GetMcrParameter(mSelectMcrIndex).strIPAddress);
                SetButtonText(BtnMcrPort, objConfig.GetMcrParameter(mSelectMcrIndex).strPortNumber);
            }
            // Temperature
            if (pnlTemperatureBase.Visible == true)
            {
                SetButtonText(BtnTemperatureGroupIndex, mSelectTemperatureIndex.ToString());
                SetButtonText(BtnTemperatureSerialPort, objConfig.GetTemperatureParameter(mSelectTemperatureIndex).strSerialPortName);
                SetButtonText(BtnTemperatureBaudrate, string.Format("{0:D}", objConfig.GetTemperatureParameter(mSelectTemperatureIndex).iSerialPortBaudrate));
            }
            // GMS
            if (pnlGmsBase.Visible == true)
            {
                var gmsParameter = objConfig.GetElectrostaticParameter(mSelectGmsIndex);
                SetButtonText(BtnGmsGroupIndex, mSelectGmsIndex.ToString());
                SetButtonText(BtnGmsSerialPort, gmsParameter.strSerialPortName);
                SetButtonText(BtnGmsBaudrate, string.Format("{0:D}", gmsParameter.iSerialPortBaudrate));
            }
            // AngleSensor
            if (pnlAngleSensorBase.Visible == true)
            {
                var angleSensorParameter = objConfig.GetAngleSensorParameter(mSelectAngularSensorIndex);
                SetButtonText(BtnAngleSensorGroupIndex, mSelectAngularSensorIndex.ToString());
                SetButtonText(BtnAngleSensorSerialPort, angleSensorParameter.strSerialPortName);
                SetButtonText(BtnAngleSensorBaudrate, string.Format("{0:D}", angleSensorParameter.iSerialPortBaudrate));
            }
            // POWER METER
            if (PnlPowerMeterBase.Visible == true)
            {
                SetButtonText(BtnPowerMeterGroupIndex, mSelectPowerMeterIndex.ToString());
                SetButtonText(BtnPowerMeterIP, objConfig.GetPowerMeterParameter(mSelectPowerMeterIndex).strSocketIPAddress);
                SetButtonText(BtnPowerMeterPort, string.Format("{0:D}", objConfig.GetPowerMeterParameter(mSelectPowerMeterIndex).iSocketPortNumber));
            }
            // FFU
            if (PnlFfuBase.Visible == true)
            {
                SetButtonText(BtnFFUGroupIndex, mSelectFfuIndex.ToString());
                SetButtonText(BtnFFUSerialPort, objConfig.GetFfuParameter(mSelectFfuIndex).strSerialPortName);
                SetButtonText(BtnFFUBaudrate, string.Format("{0:D}", objConfig.GetFfuParameter(mSelectFfuIndex).iSerialPortBaudrate));
            }
            // GAP MEASUREMENT
            if (pnlGapMeasurementBase.Visible == true)
            {
                SetButtonText(BtnGapMeasurementGourpIndex, mSelectGapMeasurementIndex.ToString());
                SetButtonText(BtnGapMeasurementSerialPort, objConfig.GetMeasurementInitializeParameter(mSelectGapMeasurementIndex).strSerialPortName);
                SetButtonText(BtnGapMeasurementBaudrate, objConfig.GetMeasurementInitializeParameter(mSelectGapMeasurementIndex).iSerialPortBaudrate.ToString());
            }
            // SERIAL COMM STATISTICS
            if (PnlSerialCommStatistics.Visible == true)
            {
                SetButtonText(BtnTitleSerialCommStatistics, $"SERIAL COMM STATISTICS [ {DateTime.Now.Hour:00}H ~ {DateTime.Now.Hour + 1:00}H ]");
                for (int row = 0; row < CProcessMain.SerialReaders.Items.Count; row++)
                {
                    var serialReaderReportCsv = CProcessMain.SerialReaders.Items[row] as Data.ISerialReaderReportCsv;
                    if (serialReaderReportCsv == null)
                    {
                        continue;
                    }

                    string[] columns = serialReaderReportCsv.GetOneLineForCsvReport().Split(',');
                    for (int col = 0; col < GridSerialCommStatistics.ColumnCount; col++)
                    {
                        SetGridCellText(GridSerialCommStatistics, row, col, columns[col + 1]);
                        SetGridCellForeColor(GridSerialCommStatistics, row, col, CProcessMain.SerialReaders.Items[row].IsReadingFail ? Color.Yellow : Color.Empty);
                        SetGridCellBackColor(GridSerialCommStatistics, row, col, CProcessMain.SerialReaders.Items[row].IsReadingFail ? Color.Red : Color.Empty);
                    }
                }
            }
        }

        private void SetGridCellBackColor(DataGridView grid, int row, int col, Color color)
        {
            if (grid.Rows[row].Cells[col].Value == null
                || grid.Rows[row].Cells[col].Style.BackColor != color
                )
            {
                grid.Rows[row].Cells[col].Style.BackColor = color;
            }
        }

        private void SetGridCellForeColor(DataGridView grid, int row, int col, Color color)
        {
            if (grid.Rows[row].Cells[col].Value == null
                || grid.Rows[row].Cells[col].Style.ForeColor != color
                )
            {
                grid.Rows[row].Cells[col].Style.ForeColor = color;
            }
        }

        private void SetGridCellText(DataGridView grid, int row, int col, string text)
        {
            if (grid.Rows[row].Cells[col].Value == null
                || grid.Rows[row].Cells[col].Value.ToString() != text
                )
            {
                grid.Rows[row].Cells[col].Value = text;
            }
        }

        private void SetRichTextBoxText(RichTextBox richTextBox, string text, Color foreColor, Color backColor)
        {
            richTextBox.SelectionColor = foreColor;
            richTextBox.SelectionBackColor = backColor;
            richTextBox.AppendText(text);
        }

        /// <summary>
        /// MCR 리딩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMcrGroupIndex_Click(object sender, EventArgs e)
        {
            using (var dialog = new FormEnumSelect(Enum.GetNames(typeof(CDefine.EMcr))))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                CDefine.EMcr selectIndex;
                if (Enum.TryParse(dialog.ResultName, out selectIndex) == false)
                {
                    return;
                }

                mSelectMcrIndex = selectIndex;
            }
        }

        private async void BtnTitleMcrRead_Click(object sender, EventArgs e)
        {
            TxtMcrReadResult.ResetText();
            SetRichTextBoxText(TxtMcrReadResult, $"{mSelectMcrIndex}: {Environment.NewLine}", Color.Green, Color.Transparent);
            string readResult = string.Empty;
            await Task.Run(() =>
            {
                var mcr = m_objDocument.m_objProcessMain.m_objMcr[mSelectMcrIndex];
                mcr.HLTrigger();
                mcr.HLReadData(ref readResult);
            });
            if (string.IsNullOrWhiteSpace(readResult) == true)
            {
                SetRichTextBoxText(TxtMcrReadResult, $"Timeout", Color.Red, Color.Black);
                TxtMcrReadResult.SelectionStart = 0;
                return;
            }
            if (readResult.Contains("NO READ") == true)
            {
                SetRichTextBoxText(TxtMcrReadResult, $"NO READ", Color.Red, Color.Black);
                TxtMcrReadResult.SelectionStart = 0;
                return;
            }
            SetRichTextBoxText(TxtMcrReadResult, $"{readResult}", Color.Black, Color.Transparent);
            TxtMcrReadResult.SelectionStart = 0;
        }

        /// <summary>
        /// SVID MONITORING LOCK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSvidMonitoringLock_Click(object sender, EventArgs e)
        {
            m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.SvidMonitoringLock = true;
        }

        /// <summary>
        /// SVID MONITORING UNLOCK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSvidMonitoringUnlock_Click(object sender, EventArgs e)
        {
            m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.SvidMonitoringLock = false;
        }

        private void btnShowIndividualAnalogCalibrationDialog_Click(object sender, EventArgs e)
        {
            using (var calDialog = new CDialogIndividualAnalogCalibration(m_objDocument))
            {
                calDialog.ShowDialog();
            }
        }

        private void btnReloadAnalogCalibrationFile_Click(object sender, EventArgs e)
        {
            m_objDocument.m_objConfig.LoadAnalogCalibrationParameterAirPressure();
        }

        /// <summary>
        /// GMS 리딩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGmsGroupIndex_Click(object sender, EventArgs e)
        {
            using (var dialog = new FormEnumSelect(Enum.GetNames(typeof(CDefine.EGms))))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                CDefine.EGms selectIndex;
                if (Enum.TryParse(dialog.ResultName, out selectIndex) == false)
                {
                    return;
                }

                mSelectGmsIndex = selectIndex;
            }
        }

        private void btnGmsRead_Click(object sender, EventArgs e)
        {
            TxtGmsReadResult.ResetText();

            SetRichTextBoxText(TxtGmsReadResult, $"{mSelectGmsIndex}: {Environment.NewLine}", Color.Green, Color.Transparent);
            int address = 1;
            string nickname;
            if (m_objDocument.m_objProcessMain.m_objElectrostatic[mSelectGmsIndex].HLLoadNickName(address, out nickname) == false)
            {
                SetRichTextBoxText(TxtGmsReadResult, $"Nickname Read Failed", Color.Red, Color.Black);
                TxtGmsReadResult.SelectionStart = 0;
                return;
            }
            string[] results;
            if (m_objDocument.m_objProcessMain.m_objElectrostatic[mSelectGmsIndex].HLReadChannel(nickname, out results) == false)
            {
                SetRichTextBoxText(TxtGmsReadResult, $"Read Measure Value Failed", Color.Red, Color.Black);
                TxtGmsReadResult.SelectionStart = 0;
                return;
            }
            StringBuilder sb = new StringBuilder(1024);
            sb.AppendLine($"GroupIndex: {mSelectGmsIndex}");
            sb.AppendLine($"Address: {address}");
            sb.AppendLine($"Nickname: {nickname}");
            sb.AppendLine(string.Empty);
            for (int i = 0; i < results.Length; i++)
            {
                sb.AppendLine($"CH{i + 1}: {results[i]}");
            }
            SetRichTextBoxText(TxtGmsReadResult, sb.ToString(), Color.Black, Color.Transparent);
            TxtGmsReadResult.SelectionStart = 0;
        }

        /// <summary>
        /// FFU 현재 속도 값 리딩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFFUGroupIndex_Click(object sender, EventArgs e)
        {
            using (var dialog = new FormEnumSelect(Enum.GetNames(typeof(CDefine.EMcu))))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                CDefine.EMcu selectIndex;
                if (Enum.TryParse(dialog.ResultName, out selectIndex) == false)
                {
                    return;
                }

                mSelectFfuIndex = selectIndex;
            }
        }

        private void BtnFfuRead_Click(object sender, EventArgs e)
        {
            int selectIndex = lstFfuChannel.SelectedIndex;
            if (selectIndex == -1)
            {
                return;
            }
            TxtFfuReadResult.ResetText();

            SetRichTextBoxText(TxtFfuReadResult, $"{mSelectFfuIndex}→{lstFfuChannel.SelectedItem}: {Environment.NewLine}", Color.Green, Color.Transparent);
            int readValue;
            if (m_objDocument.m_objProcessMain.m_objFFU[mSelectFfuIndex].HLGetCurrentSpeed(1, selectIndex, out readValue) == false)
            {
                SetRichTextBoxText(TxtFfuReadResult, $"Read Failed", Color.Red, Color.Black);
                TxtFfuReadResult.SelectionStart = 0;
                return;
            }
            SetRichTextBoxText(TxtFfuReadResult, $"{readValue} RPM", Color.Black, Color.Transparent);
            TxtFfuReadResult.SelectionStart = 0;
        }

        private void BtnAngleSensorGroupIndex_Click(object sender, EventArgs e)
        {
            using (var dialog = new FormEnumSelect(Enum.GetNames(typeof(CDefine.EAngularSensor))))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                CDefine.EAngularSensor selectIndex;
                if (Enum.TryParse(dialog.ResultName, out selectIndex) == false)
                {
                    return;
                }

                mSelectAngularSensorIndex = selectIndex;
            }
        }

        private async void btnAngleSensorRead_Click(object sender, EventArgs e)
        {
            int selectIndex = lstAngleSensorChannel.SelectedIndex;
            if (selectIndex == -1)
            {
                return;
            }
            TxtAngleSensorReadResult.ResetText();

            SetRichTextBoxText(TxtAngleSensorReadResult, $"{mSelectAngularSensorIndex}→{lstAngleSensorChannel.SelectedItem}: {Environment.NewLine}", Color.Green, Color.Transparent);
            bool beforeSelect = m_objDocument.m_objProcessMain.m_objAngleSensor[mSelectAngularSensorIndex].GetIsReadingChannel(selectIndex + 1);
            m_objDocument.m_objProcessMain.m_objAngleSensor[mSelectAngularSensorIndex].SetIsReadingChannel(selectIndex + 1, true);
            await Task.Run(() => Thread.Sleep(500));
            double readVlaue = m_objDocument.m_objProcessMain.m_objAngleSensor[mSelectAngularSensorIndex].GetAngle(selectIndex + 1);
            SetRichTextBoxText(TxtAngleSensorReadResult, $"{readVlaue:0.0} º", Color.Black, Color.Transparent);
            m_objDocument.m_objProcessMain.m_objAngleSensor[mSelectAngularSensorIndex].SetIsReadingChannel(selectIndex + 1, beforeSelect);
        }

        private void BtnTemperatureGroupIndex_Click(object sender, EventArgs e)
        {
            using (var dialog = new FormEnumSelect(Enum.GetNames(typeof(CDefine.ETemperatureController))))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                CDefine.ETemperatureController selectIndex;
                if (Enum.TryParse(dialog.ResultName, out selectIndex) == false)
                {
                    return;
                }

                mSelectTemperatureIndex = selectIndex;
            }
        }

        private void BtnTitleTemperatureRead_Click(object sender, EventArgs e)
        {
            int selectIndex = lstTemperatureChannel.SelectedIndex;
            if (selectIndex == -1)
            {
                return;
            }
            TxtTemperatureReadResult.ResetText();

            SetRichTextBoxText(TxtTemperatureReadResult, $"{mSelectTemperatureIndex}→{lstTemperatureChannel.SelectedItem}: {Environment.NewLine}", Color.Green, Color.Transparent);
            double readValue;
            if (m_objDocument.m_objProcessMain.m_objTemperature[mSelectTemperatureIndex].HLReadTemp(Convert.ToByte(selectIndex), out readValue) == false)
            {
                SetRichTextBoxText(TxtTemperatureReadResult, $"Read Failed", Color.Red, Color.Black);
                TxtTemperatureReadResult.SelectionStart = 0;
                return;
            }
            SetRichTextBoxText(TxtTemperatureReadResult, $"{readValue:0.0} ºC", Color.Black, Color.Transparent);
            TxtTemperatureReadResult.SelectionStart = 0;
        }

        private void BtnGapMeasurementGourpIndex_Click(object sender, EventArgs e)
        {
            using (var dialog = new FormEnumSelect(Enum.GetNames(typeof(CDefine.EMeasurement))))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                CDefine.EMeasurement selectIndex;
                if (Enum.TryParse(dialog.ResultName, out selectIndex) == false)
                {
                    return;
                }

                mSelectGapMeasurementIndex = selectIndex;
            }
        }

        private void BtnTitleGapMeasurementRead_Click(object sender, EventArgs e)
        {
            int selectIndex = lstGapMeasurementChannel.SelectedIndex;
            if (selectIndex == -1)
            {
                return;
            }
            TxtGapMeasurementResult.ResetText();

            double[] readValues = null;
            if (m_objDocument.m_objProcessMain.m_objMeasurementInterfaces[mSelectGapMeasurementIndex].TryReadAllChannels(selectIndex, out readValues) == false)
            {
                SetRichTextBoxText(TxtGapMeasurementResult, $"{mSelectGapMeasurementIndex}→{lstGapMeasurementChannel.SelectedItem} TryReadAllChannels Failed", Color.Red, Color.Black);
                TxtGapMeasurementResult.SelectionStart = 0;
                return;
            }

            SetRichTextBoxText(TxtGapMeasurementResult, $"{mSelectGapMeasurementIndex}→{lstGapMeasurementChannel.SelectedItem}: {Environment.NewLine}", Color.Green, Color.Transparent);
            for (int i = 0; i < readValues.Length; i++)
            {
                SetRichTextBoxText(TxtGapMeasurementResult, $"CH{i + 1}: {readValues[i]:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            TxtGapMeasurementResult.SelectionStart = 0;
        }

        private void BtnPowerMeterGroupIndex_Click(object sender, EventArgs e)
        {
            using (var dialog = new FormEnumSelect(Enum.GetNames(typeof(CDefine.EPowerMeter))))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                CDefine.EPowerMeter selectIndex;
                if (Enum.TryParse(dialog.ResultName, out selectIndex) == false)
                {
                    return;
                }

                mSelectPowerMeterIndex = selectIndex;
            }
        }

        private void BtnTitlePowerMeterRead_Click(object sender, EventArgs e)
        {
            TxtPowerMeterResult.ResetText();

            // aggrigation 데이터 업데이트
            m_objDocument.m_objProcessMain.m_objPowerMeter[mSelectPowerMeterIndex].HLDataFetch();

            SetRichTextBoxText(TxtPowerMeterResult, $"{mSelectPowerMeterIndex}: {Environment.NewLine}", Color.Green, Color.Transparent);
            int unitID = m_objDocument.m_objConfig.GetPowerMeterParameter(mSelectPowerMeterIndex).iUnitID;
            switch (m_objDocument.m_objConfig.GetPowerMeterParameter(mSelectPowerMeterIndex).iUnitType)
            {
                case 0: // 단상
                    {
                        SetRichTextBoxText(TxtPowerMeterResult, $" → UnitType: P1{Environment.NewLine}", Color.Green, Color.Transparent);
                    }
                    break;

                case 1: // 3상
                    {
                        SetRichTextBoxText(TxtPowerMeterResult, $" → UnitType: P3{Environment.NewLine}", Color.Green, Color.Transparent);
                    }
                    break;

                default:
                    SetRichTextBoxText(TxtPowerMeterResult, $"Not Supported Unit Type: {m_objDocument.m_objConfig.GetPowerMeterParameter(mSelectPowerMeterIndex).iUnitType}{Environment.NewLine}", Color.Red, Color.Black);
                    break;
            }

            var readData = new FdcReportDataSet();
            if (m_objDocument.m_objProcessMain.m_objPowerMeter[mSelectPowerMeterIndex].TryReadFdcReportData(unitID, ref readData) == false)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"Data Read Failed{Environment.NewLine}", Color.Red, Color.Black);
                return;
            }

            if (readData.PHASE_POWER.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.PHASE_POWER)}: {readData.PHASE_POWER:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.INSTANTANEOUS_POWER.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.INSTANTANEOUS_POWER)}: {readData.INSTANTANEOUS_POWER:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.LEAKAGE_CURRENT.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.LEAKAGE_CURRENT)}: {readData.LEAKAGE_CURRENT:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.R_PHASE_VOLTAGE.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.R_PHASE_VOLTAGE)}: {readData.R_PHASE_VOLTAGE:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.R_PHASE_CURRENT.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.R_PHASE_CURRENT)}: {readData.R_PHASE_CURRENT:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.R_PHASE_VOLTAGE_VTHD.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.R_PHASE_VOLTAGE_VTHD)}: {readData.R_PHASE_VOLTAGE_VTHD:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.R_PHASE_CURRENT_ITDD.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.R_PHASE_CURRENT_ITDD)}: {readData.R_PHASE_CURRENT_ITDD:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.S_PHASE_VOLTAGE.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.S_PHASE_VOLTAGE)}: {readData.S_PHASE_VOLTAGE:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.S_PHASE_CURRENT.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.S_PHASE_CURRENT)}: {readData.S_PHASE_CURRENT:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.S_PHASE_VOLTAGE_VTHD.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.S_PHASE_VOLTAGE_VTHD)}: {readData.S_PHASE_VOLTAGE_VTHD:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.S_PHASE_CURRENT_ITDD.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.S_PHASE_CURRENT_ITDD)}: {readData.S_PHASE_CURRENT_ITDD:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.T_PHASE_VOLTAGE.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.T_PHASE_VOLTAGE)}: {readData.T_PHASE_VOLTAGE:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.T_PHASE_CURRENT.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.T_PHASE_CURRENT)}: {readData.T_PHASE_CURRENT:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.T_PHASE_VOLTAGE_VTHD.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.T_PHASE_VOLTAGE_VTHD)}: {readData.T_PHASE_VOLTAGE_VTHD:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
            if (readData.T_PHASE_CURRENT_ITDD.HasValue)
            {
                SetRichTextBoxText(TxtPowerMeterResult, $"{nameof(readData.T_PHASE_CURRENT_ITDD)}: {readData.T_PHASE_CURRENT_ITDD:0.000}{Environment.NewLine}", Color.Black, Color.Transparent);
            }
        }

        private void BtnSerialCommStatisticsClear_Click(object sender, EventArgs e)
        {
            string[] itemNames = new string[] { "All" };
            itemNames = itemNames.Concat(CProcessMain.SerialReaders.Items.Select(i => i.Name))
                .ToArray();
            using (var dialog = new FormEnumSelect(itemNames))
            {
                dialog.TitleText = "SELECT CLEAR ITEM";
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                switch (dialog.ResultIndex)
                {
                    case 0:
                        foreach (var item in CProcessMain.SerialReaders.Items)
                        {
                            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{item.Name}] [Clear] [{item.StatisticsOkCount},{item.StatisticsNgCount},{item.StatisticsAlarmDuration:hh\\:mm\\\"ss\\'f},{item.StatisticsAlarmCount}]");
                            item.Clear();
                        }
                        break;

                    default:
                        int index = dialog.ResultIndex - 1;
                        m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{CProcessMain.SerialReaders.Items[index].Name}] [Clear] [{CProcessMain.SerialReaders.Items[index].StatisticsOkCount},{CProcessMain.SerialReaders.Items[index].StatisticsNgCount},{CProcessMain.SerialReaders.Items[index].StatisticsAlarmDuration:hh\\:mm\\\"ss\\'f},{CProcessMain.SerialReaders.Items[index].StatisticsAlarmCount}]");
                        CProcessMain.SerialReaders.Items[index].Clear();
                        break;
                }
            }
        }

        private void BtnSerialCommStatisticsSetting_Click(object sender, EventArgs e)
        {
            string[] itemNames = CProcessMain.SerialReaders.Items.Cast<Data.ISerialReaderConfig>()
                .Select(i => $"{i.Name}: {i.ReadingFailIgnoreDuration.TotalSeconds:0.0} ( sec )")
                .ToArray();
            using (var dialog = new FormEnumSelect(itemNames))
            {
                dialog.TitleText = "SELECT ALARM SETTING ITEM";
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var serialReaderConfig = CProcessMain.SerialReaders.Items[dialog.ResultIndex] as Data.ISerialReaderConfig;
                using (var keyPad = new FormKeyPad(serialReaderConfig.ReadingFailIgnoreDuration.TotalSeconds, serialReaderConfig.ReadingFailIgnoreDurationMinValue.TotalSeconds, serialReaderConfig.ReadingFailIgnoreDurationMaxValue.TotalSeconds))
                {
                    if (keyPad.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    TimeSpan setValue = TimeSpan.FromSeconds(keyPad.m_dResultValue);
                    m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{serialReaderConfig.Name}] [ReadingFailIgnoreDuration: {serialReaderConfig.ReadingFailIgnoreDuration.TotalSeconds} -> {setValue.TotalSeconds}]");
                    serialReaderConfig.ReadingFailIgnoreDuration = setValue;
                }
            }
        }
    }
}