namespace SVI_NFT_R
{
    partial class CDialogIndividualAnalogCalibration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlCalibration = new System.Windows.Forms.GroupBox();
            this.pnlDescription = new System.Windows.Forms.Panel();
            this.lblCalAnalog = new System.Windows.Forms.Label();
            this.txbCalDescription = new System.Windows.Forms.RichTextBox();
            this.lblCalMeasure = new System.Windows.Forms.Label();
            this.pnlLayoutCalibration = new System.Windows.Forms.TableLayoutPanel();
            this.btnCalAdd = new UiAsset.ImageButton();
            this.btnCalCalc = new UiAsset.ImageButton();
            this.btnCalClear = new UiAsset.ImageButton();
            this.btnCalDelete = new UiAsset.ImageButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ucTeachVacuum = new SVI_NFT_R.UI.UserControls.UcTeachVacuum();
            this.btnCalMeasure = new UiAsset.SpeedButton();
            this.lblTitleCalMeasure = new System.Windows.Forms.Label();
            this.lblTitleCalAnalogIndex = new System.Windows.Forms.Label();
            this.cbbCalAnalogIndex = new System.Windows.Forms.ComboBox();
            this.grdCalData = new System.Windows.Forms.DataGridView();
            this.pnlSetting = new System.Windows.Forms.GroupBox();
            this.btnUsingIndividualChannel = new System.Windows.Forms.Button();
            this.btnSettingYIntercept = new UiAsset.SpeedButton();
            this.btnSettingSlope = new UiAsset.SpeedButton();
            this.btnSettingPaste = new UiAsset.ImageButton();
            this.btnSettingCopy = new UiAsset.ImageButton();
            this.lblTitleSettingDeviceType = new System.Windows.Forms.Label();
            this.lblTitleSettingYIntercept = new System.Windows.Forms.Label();
            this.lblTitleSettingSlope = new System.Windows.Forms.Label();
            this.lblTitleSettingUsingChannel = new System.Windows.Forms.Label();
            this.cbbSettingDeviceType = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSave = new UiAsset.ImageButton();
            this.btnClose = new UiAsset.ImageButton();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblErrorMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel2.SuspendLayout();
            this.pnlCalibration.SuspendLayout();
            this.pnlDescription.SuspendLayout();
            this.pnlLayoutCalibration.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCalData)).BeginInit();
            this.pnlSetting.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pnlCalibration);
            this.panel2.Controls.Add(this.pnlSetting);
            this.panel2.Controls.Add(this.flowLayoutPanel1);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(919, 949);
            this.panel2.TabIndex = 3;
            // 
            // pnlCalibration
            // 
            this.pnlCalibration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCalibration.Controls.Add(this.pnlDescription);
            this.pnlCalibration.Controls.Add(this.pnlLayoutCalibration);
            this.pnlCalibration.Location = new System.Drawing.Point(15, 221);
            this.pnlCalibration.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCalibration.Name = "pnlCalibration";
            this.pnlCalibration.Padding = new System.Windows.Forms.Padding(4);
            this.pnlCalibration.Size = new System.Drawing.Size(889, 619);
            this.pnlCalibration.TabIndex = 7;
            this.pnlCalibration.TabStop = false;
            this.pnlCalibration.Text = "[2] 캘리브레이션";
            // 
            // pnlDescription
            // 
            this.pnlDescription.Controls.Add(this.lblCalAnalog);
            this.pnlDescription.Controls.Add(this.txbCalDescription);
            this.pnlDescription.Controls.Add(this.lblCalMeasure);
            this.pnlDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDescription.Location = new System.Drawing.Point(528, 26);
            this.pnlDescription.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDescription.Name = "pnlDescription";
            this.pnlDescription.Size = new System.Drawing.Size(357, 589);
            this.pnlDescription.TabIndex = 0;
            // 
            // lblCalAnalog
            // 
            this.lblCalAnalog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCalAnalog.AutoSize = true;
            this.lblCalAnalog.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCalAnalog.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblCalAnalog.Location = new System.Drawing.Point(10, 521);
            this.lblCalAnalog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCalAnalog.Name = "lblCalAnalog";
            this.lblCalAnalog.Size = new System.Drawing.Size(277, 28);
            this.lblCalAnalog.TabIndex = 5;
            this.lblCalAnalog.Text = "[확인용] 아날로그 값: 0.000 V";
            // 
            // txbCalDescription
            // 
            this.txbCalDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbCalDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbCalDescription.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txbCalDescription.Location = new System.Drawing.Point(10, 9);
            this.txbCalDescription.Margin = new System.Windows.Forms.Padding(4);
            this.txbCalDescription.Name = "txbCalDescription";
            this.txbCalDescription.ReadOnly = true;
            this.txbCalDescription.Size = new System.Drawing.Size(338, 485);
            this.txbCalDescription.TabIndex = 0;
            this.txbCalDescription.Text = "1. 설정 값 선택\n2. 캘리브레이션\n    2-1. 아날로그 채널 선택\n    2-2. 실제 값 입력\n    2-3. 데이터 추가\n    (2-" +
    "2~3 반복 [베큠 Off, 베큠 On 미감지, 베큠 On 감지])\n    2-4. 캘리브레이션\n3. 저장";
            // 
            // lblCalMeasure
            // 
            this.lblCalMeasure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCalMeasure.AutoSize = true;
            this.lblCalMeasure.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCalMeasure.ForeColor = System.Drawing.Color.Magenta;
            this.lblCalMeasure.Location = new System.Drawing.Point(10, 556);
            this.lblCalMeasure.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCalMeasure.Name = "lblCalMeasure";
            this.lblCalMeasure.Size = new System.Drawing.Size(255, 28);
            this.lblCalMeasure.TabIndex = 5;
            this.lblCalMeasure.Text = "[확인용] 실제 값: 0.000 kPa";
            // 
            // pnlLayoutCalibration
            // 
            this.pnlLayoutCalibration.ColumnCount = 3;
            this.pnlLayoutCalibration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.pnlLayoutCalibration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.pnlLayoutCalibration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.pnlLayoutCalibration.Controls.Add(this.btnCalAdd, 0, 1);
            this.pnlLayoutCalibration.Controls.Add(this.btnCalCalc, 0, 3);
            this.pnlLayoutCalibration.Controls.Add(this.btnCalClear, 2, 1);
            this.pnlLayoutCalibration.Controls.Add(this.btnCalDelete, 1, 1);
            this.pnlLayoutCalibration.Controls.Add(this.panel1, 0, 0);
            this.pnlLayoutCalibration.Controls.Add(this.grdCalData, 0, 2);
            this.pnlLayoutCalibration.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLayoutCalibration.Location = new System.Drawing.Point(4, 26);
            this.pnlLayoutCalibration.Margin = new System.Windows.Forms.Padding(4);
            this.pnlLayoutCalibration.Name = "pnlLayoutCalibration";
            this.pnlLayoutCalibration.RowCount = 4;
            this.pnlLayoutCalibration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 225F));
            this.pnlLayoutCalibration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.pnlLayoutCalibration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayoutCalibration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.pnlLayoutCalibration.Size = new System.Drawing.Size(524, 589);
            this.pnlLayoutCalibration.TabIndex = 4;
            // 
            // btnCalAdd
            // 
            this.btnCalAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnCalAdd.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnCalAdd.ButtonText = "[2-3] 데이터 추가";
            this.btnCalAdd.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnCalAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCalAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCalAdd.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnCalAdd.Location = new System.Drawing.Point(4, 229);
            this.btnCalAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnCalAdd.Name = "btnCalAdd";
            this.btnCalAdd.Size = new System.Drawing.Size(166, 58);
            this.btnCalAdd.TabIndex = 3;
            this.btnCalAdd.Text = "[2-3] 데이터 추가";
            this.btnCalAdd.UseVisualStyleBackColor = false;
            this.btnCalAdd.Click += new System.EventHandler(this.btnCalAdd_Click);
            // 
            // btnCalCalc
            // 
            this.btnCalCalc.BackColor = System.Drawing.Color.Transparent;
            this.btnCalCalc.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnCalCalc.ButtonText = "[2-4] 캘리브레이션";
            this.pnlLayoutCalibration.SetColumnSpan(this.btnCalCalc, 3);
            this.btnCalCalc.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnCalCalc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalCalc.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCalCalc.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCalCalc.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnCalCalc.Location = new System.Drawing.Point(4, 527);
            this.btnCalCalc.Margin = new System.Windows.Forms.Padding(4);
            this.btnCalCalc.Name = "btnCalCalc";
            this.btnCalCalc.Size = new System.Drawing.Size(516, 58);
            this.btnCalCalc.TabIndex = 1;
            this.btnCalCalc.Text = "[2-4] 캘리브레이션";
            this.btnCalCalc.UseVisualStyleBackColor = false;
            this.btnCalCalc.Click += new System.EventHandler(this.btnCalCalc_Click);
            // 
            // btnCalClear
            // 
            this.btnCalClear.BackColor = System.Drawing.Color.Transparent;
            this.btnCalClear.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnCalClear.ButtonText = "클리어";
            this.btnCalClear.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnCalClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCalClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCalClear.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnCalClear.Location = new System.Drawing.Point(352, 229);
            this.btnCalClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnCalClear.Name = "btnCalClear";
            this.btnCalClear.Size = new System.Drawing.Size(168, 58);
            this.btnCalClear.TabIndex = 3;
            this.btnCalClear.Text = "클리어";
            this.btnCalClear.UseVisualStyleBackColor = false;
            this.btnCalClear.Click += new System.EventHandler(this.btnCalClear_Click);
            // 
            // btnCalDelete
            // 
            this.btnCalDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnCalDelete.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnCalDelete.ButtonText = "데이터 삭제";
            this.btnCalDelete.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnCalDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCalDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCalDelete.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnCalDelete.Location = new System.Drawing.Point(178, 229);
            this.btnCalDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnCalDelete.Name = "btnCalDelete";
            this.btnCalDelete.Size = new System.Drawing.Size(166, 58);
            this.btnCalDelete.TabIndex = 3;
            this.btnCalDelete.Text = "데이터 삭제";
            this.btnCalDelete.UseVisualStyleBackColor = false;
            this.btnCalDelete.Click += new System.EventHandler(this.btnCalDelete_Click);
            // 
            // panel1
            // 
            this.pnlLayoutCalibration.SetColumnSpan(this.panel1, 3);
            this.panel1.Controls.Add(this.ucTeachVacuum);
            this.panel1.Controls.Add(this.btnCalMeasure);
            this.panel1.Controls.Add(this.lblTitleCalMeasure);
            this.panel1.Controls.Add(this.lblTitleCalAnalogIndex);
            this.panel1.Controls.Add(this.cbbCalAnalogIndex);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(518, 219);
            this.panel1.TabIndex = 4;
            // 
            // ucTeachVacuum
            // 
            this.ucTeachVacuum.BackColor = System.Drawing.Color.Transparent;
            this.ucTeachVacuum.DeviceNameBackColor = System.Drawing.Color.White;
            this.ucTeachVacuum.DeviceNameText = "DEVICE NAME";
            this.ucTeachVacuum.IsHideDeviceName = true;
            this.ucTeachVacuum.Location = new System.Drawing.Point(45, 143);
            this.ucTeachVacuum.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ucTeachVacuum.Name = "ucTeachVacuum";
            this.ucTeachVacuum.Size = new System.Drawing.Size(446, 62);
            this.ucTeachVacuum.TabIndex = 17;
            // 
            // btnCalMeasure
            // 
            this.btnCalMeasure.BackColor = System.Drawing.Color.White;
            this.btnCalMeasure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalMeasure.Location = new System.Drawing.Point(45, 101);
            this.btnCalMeasure.Margin = new System.Windows.Forms.Padding(4);
            this.btnCalMeasure.Name = "btnCalMeasure";
            this.btnCalMeasure.Size = new System.Drawing.Size(446, 31);
            this.btnCalMeasure.TabIndex = 16;
            this.btnCalMeasure.TabStop = false;
            this.btnCalMeasure.Text = "0";
            this.btnCalMeasure.UseVisualStyleBackColor = false;
            this.btnCalMeasure.Click += new System.EventHandler(this.btnCalMeasure_Click);
            // 
            // lblTitleCalMeasure
            // 
            this.lblTitleCalMeasure.AutoSize = true;
            this.lblTitleCalMeasure.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTitleCalMeasure.Location = new System.Drawing.Point(26, 72);
            this.lblTitleCalMeasure.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitleCalMeasure.Name = "lblTitleCalMeasure";
            this.lblTitleCalMeasure.Size = new System.Drawing.Size(148, 23);
            this.lblTitleCalMeasure.TabIndex = 5;
            this.lblTitleCalMeasure.Text = "[2-2] 실제 값 입력";
            // 
            // lblTitleCalAnalogIndex
            // 
            this.lblTitleCalAnalogIndex.AutoSize = true;
            this.lblTitleCalAnalogIndex.Location = new System.Drawing.Point(26, 9);
            this.lblTitleCalAnalogIndex.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitleCalAnalogIndex.Name = "lblTitleCalAnalogIndex";
            this.lblTitleCalAnalogIndex.Size = new System.Drawing.Size(199, 23);
            this.lblTitleCalAnalogIndex.TabIndex = 5;
            this.lblTitleCalAnalogIndex.Text = "[2-1] 아날로그 채널 선택";
            // 
            // cbbCalAnalogIndex
            // 
            this.cbbCalAnalogIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbCalAnalogIndex.FormattingEnabled = true;
            this.cbbCalAnalogIndex.Location = new System.Drawing.Point(45, 34);
            this.cbbCalAnalogIndex.Margin = new System.Windows.Forms.Padding(4);
            this.cbbCalAnalogIndex.Name = "cbbCalAnalogIndex";
            this.cbbCalAnalogIndex.Size = new System.Drawing.Size(445, 29);
            this.cbbCalAnalogIndex.TabIndex = 0;
            this.cbbCalAnalogIndex.SelectedIndexChanged += new System.EventHandler(this.cbbCalAnalogIndex_SelectedIndexChanged);
            // 
            // grdCalData
            // 
            this.grdCalData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pnlLayoutCalibration.SetColumnSpan(this.grdCalData, 3);
            this.grdCalData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCalData.Location = new System.Drawing.Point(4, 295);
            this.grdCalData.Margin = new System.Windows.Forms.Padding(4);
            this.grdCalData.Name = "grdCalData";
            this.grdCalData.RowTemplate.Height = 23;
            this.grdCalData.Size = new System.Drawing.Size(516, 224);
            this.grdCalData.TabIndex = 5;
            // 
            // pnlSetting
            // 
            this.pnlSetting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSetting.Controls.Add(this.btnUsingIndividualChannel);
            this.pnlSetting.Controls.Add(this.btnSettingYIntercept);
            this.pnlSetting.Controls.Add(this.btnSettingSlope);
            this.pnlSetting.Controls.Add(this.btnSettingPaste);
            this.pnlSetting.Controls.Add(this.btnSettingCopy);
            this.pnlSetting.Controls.Add(this.lblTitleSettingDeviceType);
            this.pnlSetting.Controls.Add(this.lblTitleSettingYIntercept);
            this.pnlSetting.Controls.Add(this.lblTitleSettingSlope);
            this.pnlSetting.Controls.Add(this.lblTitleSettingUsingChannel);
            this.pnlSetting.Controls.Add(this.cbbSettingDeviceType);
            this.pnlSetting.Location = new System.Drawing.Point(15, 72);
            this.pnlSetting.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSetting.Name = "pnlSetting";
            this.pnlSetting.Padding = new System.Windows.Forms.Padding(4);
            this.pnlSetting.Size = new System.Drawing.Size(889, 141);
            this.pnlSetting.TabIndex = 6;
            this.pnlSetting.TabStop = false;
            this.pnlSetting.Text = "[1] 설정 값";
            // 
            // btnUsingIndividualChannel
            // 
            this.btnUsingIndividualChannel.Location = new System.Drawing.Point(130, 43);
            this.btnUsingIndividualChannel.Name = "btnUsingIndividualChannel";
            this.btnUsingIndividualChannel.Size = new System.Drawing.Size(210, 35);
            this.btnUsingIndividualChannel.TabIndex = 8;
            this.btnUsingIndividualChannel.UseVisualStyleBackColor = true;
            this.btnUsingIndividualChannel.Click += new System.EventHandler(this.btnUsingIndividualChannel_Click);
            // 
            // btnSettingYIntercept
            // 
            this.btnSettingYIntercept.BackColor = System.Drawing.Color.White;
            this.btnSettingYIntercept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettingYIntercept.Location = new System.Drawing.Point(504, 91);
            this.btnSettingYIntercept.Margin = new System.Windows.Forms.Padding(4);
            this.btnSettingYIntercept.Name = "btnSettingYIntercept";
            this.btnSettingYIntercept.Size = new System.Drawing.Size(210, 31);
            this.btnSettingYIntercept.TabIndex = 16;
            this.btnSettingYIntercept.TabStop = false;
            this.btnSettingYIntercept.Text = "0";
            this.btnSettingYIntercept.UseVisualStyleBackColor = false;
            this.btnSettingYIntercept.Click += new System.EventHandler(this.btnSettingYIntercept_Click);
            // 
            // btnSettingSlope
            // 
            this.btnSettingSlope.BackColor = System.Drawing.Color.White;
            this.btnSettingSlope.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettingSlope.Location = new System.Drawing.Point(130, 92);
            this.btnSettingSlope.Margin = new System.Windows.Forms.Padding(4);
            this.btnSettingSlope.Name = "btnSettingSlope";
            this.btnSettingSlope.Size = new System.Drawing.Size(210, 31);
            this.btnSettingSlope.TabIndex = 16;
            this.btnSettingSlope.TabStop = false;
            this.btnSettingSlope.Text = "0";
            this.btnSettingSlope.UseVisualStyleBackColor = false;
            this.btnSettingSlope.Click += new System.EventHandler(this.btnSettingSlope_Click);
            // 
            // btnSettingPaste
            // 
            this.btnSettingPaste.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettingPaste.BackColor = System.Drawing.Color.Transparent;
            this.btnSettingPaste.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnSettingPaste.ButtonText = "붙여넣기";
            this.btnSettingPaste.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnSettingPaste.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSettingPaste.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSettingPaste.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnSettingPaste.Location = new System.Drawing.Point(744, 86);
            this.btnSettingPaste.Margin = new System.Windows.Forms.Padding(4);
            this.btnSettingPaste.Name = "btnSettingPaste";
            this.btnSettingPaste.Size = new System.Drawing.Size(131, 40);
            this.btnSettingPaste.TabIndex = 15;
            this.btnSettingPaste.Text = "붙여넣기";
            this.btnSettingPaste.UseVisualStyleBackColor = false;
            this.btnSettingPaste.Click += new System.EventHandler(this.btnSettingPaste_Click);
            // 
            // btnSettingCopy
            // 
            this.btnSettingCopy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettingCopy.BackColor = System.Drawing.Color.Transparent;
            this.btnSettingCopy.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnSettingCopy.ButtonText = "복사";
            this.btnSettingCopy.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnSettingCopy.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSettingCopy.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSettingCopy.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnSettingCopy.Location = new System.Drawing.Point(744, 38);
            this.btnSettingCopy.Margin = new System.Windows.Forms.Padding(4);
            this.btnSettingCopy.Name = "btnSettingCopy";
            this.btnSettingCopy.Size = new System.Drawing.Size(131, 40);
            this.btnSettingCopy.TabIndex = 14;
            this.btnSettingCopy.Text = "복사";
            this.btnSettingCopy.UseVisualStyleBackColor = false;
            this.btnSettingCopy.Click += new System.EventHandler(this.btnSettingCopy_Click);
            // 
            // lblTitleSettingDeviceType
            // 
            this.lblTitleSettingDeviceType.AutoSize = true;
            this.lblTitleSettingDeviceType.Location = new System.Drawing.Point(370, 48);
            this.lblTitleSettingDeviceType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitleSettingDeviceType.Name = "lblTitleSettingDeviceType";
            this.lblTitleSettingDeviceType.Size = new System.Drawing.Size(118, 23);
            this.lblTitleSettingDeviceType.TabIndex = 10;
            this.lblTitleSettingDeviceType.Text = "디바이스 종류";
            // 
            // lblTitleSettingYIntercept
            // 
            this.lblTitleSettingYIntercept.AutoSize = true;
            this.lblTitleSettingYIntercept.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblTitleSettingYIntercept.Location = new System.Drawing.Point(370, 96);
            this.lblTitleSettingYIntercept.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitleSettingYIntercept.Name = "lblTitleSettingYIntercept";
            this.lblTitleSettingYIntercept.Size = new System.Drawing.Size(61, 23);
            this.lblTitleSettingYIntercept.TabIndex = 11;
            this.lblTitleSettingYIntercept.Text = "Y-절편";
            // 
            // lblTitleSettingSlope
            // 
            this.lblTitleSettingSlope.AutoSize = true;
            this.lblTitleSettingSlope.ForeColor = System.Drawing.Color.MediumBlue;
            this.lblTitleSettingSlope.Location = new System.Drawing.Point(30, 96);
            this.lblTitleSettingSlope.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitleSettingSlope.Name = "lblTitleSettingSlope";
            this.lblTitleSettingSlope.Size = new System.Drawing.Size(61, 23);
            this.lblTitleSettingSlope.TabIndex = 12;
            this.lblTitleSettingSlope.Text = "기울기";
            // 
            // lblTitleSettingUsingChannel
            // 
            this.lblTitleSettingUsingChannel.AutoSize = true;
            this.lblTitleSettingUsingChannel.Location = new System.Drawing.Point(30, 48);
            this.lblTitleSettingUsingChannel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitleSettingUsingChannel.Name = "lblTitleSettingUsingChannel";
            this.lblTitleSettingUsingChannel.Size = new System.Drawing.Size(84, 23);
            this.lblTitleSettingUsingChannel.TabIndex = 13;
            this.lblTitleSettingUsingChannel.Text = "사용 여부";
            // 
            // cbbSettingDeviceType
            // 
            this.cbbSettingDeviceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbSettingDeviceType.FormattingEnabled = true;
            this.cbbSettingDeviceType.Location = new System.Drawing.Point(504, 44);
            this.cbbSettingDeviceType.Margin = new System.Windows.Forms.Padding(4);
            this.cbbSettingDeviceType.Name = "cbbSettingDeviceType";
            this.cbbSettingDeviceType.Size = new System.Drawing.Size(209, 29);
            this.cbbSettingDeviceType.TabIndex = 7;
            this.cbbSettingDeviceType.SelectedIndexChanged += new System.EventHandler(this.cbbSettingDeviceType_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(1, 848);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(916, 74);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnSave.ButtonText = "[3] 저장";
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnSave.Location = new System.Drawing.Point(8, 8);
            this.btnSave.Margin = new System.Windows.Forms.Padding(8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(442, 59);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "[3] 저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnClose.ButtonText = "닫기";
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnClose.Location = new System.Drawing.Point(466, 8);
            this.btnClose.Margin = new System.Windows.Forms.Padding(8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(442, 59);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(798, 22);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(26, 28);
            this.label14.TabIndex = 5;
            this.label14.Text = "+";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.ForeColor = System.Drawing.Color.OrangeRed;
            this.label13.Location = new System.Drawing.Point(572, 25);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(126, 25);
            this.label13.TabIndex = 5;
            this.label13.Text = "{아날로그 값}";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(696, 29);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 28);
            this.label12.TabIndex = 5;
            this.label12.Text = "*";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.Color.MediumBlue;
            this.label11.Location = new System.Drawing.Point(718, 25);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 25);
            this.label11.TabIndex = 5;
            this.label11.Text = "{기울기}";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.Color.ForestGreen;
            this.label15.Location = new System.Drawing.Point(825, 25);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(81, 25);
            this.label15.TabIndex = 5;
            this.label15.Text = "{Y-절편}";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(545, 25);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 28);
            this.label10.TabIndex = 5;
            this.label10.Text = "=";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.Magenta;
            this.label9.Location = new System.Drawing.Point(459, 25);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 25);
            this.label9.TabIndex = 5;
            this.label9.Text = "{실제 값}";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblErrorMessage,
            this.lblMessage});
            this.statusStrip.Location = new System.Drawing.Point(0, 919);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 18, 0);
            this.statusStrip.Size = new System.Drawing.Size(919, 30);
            this.statusStrip.TabIndex = 7;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblErrorMessage.ForeColor = System.Drawing.Color.Crimson;
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(20, 25);
            this.lblErrorMessage.Text = "-";
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMessage.ForeColor = System.Drawing.Color.Blue;
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(20, 25);
            this.lblMessage.Text = "-";
            // 
            // CDialogIndividualAnalogCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(919, 949);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(934, 852);
            this.Name = "CDialogIndividualAnalogCalibration";
            this.Text = "Analog Calibration";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlCalibration.ResumeLayout(false);
            this.pnlDescription.ResumeLayout(false);
            this.pnlDescription.PerformLayout();
            this.pnlLayoutCalibration.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCalData)).EndInit();
            this.pnlSetting.ResumeLayout(false);
            this.pnlSetting.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private UiAsset.ImageButton btnClose;
        private UiAsset.ImageButton btnCalAdd;
        private UiAsset.ImageButton btnSave;
        private System.Windows.Forms.TableLayoutPanel pnlLayoutCalibration;
        private UiAsset.ImageButton btnCalCalc;
        private UiAsset.ImageButton btnCalClear;
        private UiAsset.ImageButton btnCalDelete;
        private System.Windows.Forms.ComboBox cbbCalAnalogIndex;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblCalAnalog;
        private System.Windows.Forms.Label lblTitleCalMeasure;
        private System.Windows.Forms.Label lblTitleCalAnalogIndex;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.GroupBox pnlSetting;
        private System.Windows.Forms.Label lblTitleSettingDeviceType;
        private System.Windows.Forms.Label lblTitleSettingYIntercept;
        private System.Windows.Forms.Label lblTitleSettingSlope;
        private System.Windows.Forms.Label lblTitleSettingUsingChannel;
        private System.Windows.Forms.ComboBox cbbSettingDeviceType;
        private System.Windows.Forms.GroupBox pnlCalibration;
        private System.Windows.Forms.Panel pnlDescription;
        private System.Windows.Forms.RichTextBox txbCalDescription;
        private System.Windows.Forms.ToolStripStatusLabel lblErrorMessage;
        private System.Windows.Forms.Label lblCalMeasure;
        private System.Windows.Forms.ToolStripStatusLabel lblMessage;
        private System.Windows.Forms.DataGridView grdCalData;
        private UiAsset.ImageButton btnSettingPaste;
        private UiAsset.ImageButton btnSettingCopy;
        private UiAsset.SpeedButton btnCalMeasure;
        private UiAsset.SpeedButton btnSettingYIntercept;
        private UiAsset.SpeedButton btnSettingSlope;
        private UI.UserControls.UcTeachVacuum ucTeachVacuum;
        private System.Windows.Forms.Button btnUsingIndividualChannel;
    }
}