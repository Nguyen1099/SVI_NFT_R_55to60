namespace SVI_NFT_R
{
    partial class CFormTeachVacuum
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.BtnVacuumOffDelayTime = new UiAsset.SpeedButton();
            this.BtnTitleVacuumOffDelayTime = new UiAsset.SpeedButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnVacuumTimeout = new UiAsset.SpeedButton();
            this.BtnTitleVacuumTimeout = new UiAsset.SpeedButton();
            this.GridViewVacuumList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnTitleVacuumDelayTime = new UiAsset.SpeedButton();
            this.BtnTitleVacuumList = new UiAsset.SpeedButton();
            this.BtnTitleBlowOutput = new UiAsset.SpeedButton();
            this.BtnTitleVacuumOutput = new UiAsset.SpeedButton();
            this.BtnVacuum = new UiAsset.ImageButton();
            this.BtnBlow = new UiAsset.ImageButton();
            this.BtnOutputLock = new UiAsset.ImageButton();
            this.BtnTitleVacuumOutput04 = new UiAsset.SpeedButton();
            this.BtnTitleVacuumOutput03 = new UiAsset.SpeedButton();
            this.BtnTitleVacuumOutput02 = new UiAsset.SpeedButton();
            this.BtnTitleVacuumOutput01 = new UiAsset.SpeedButton();
            this.BtnTitleBlowOutput04 = new UiAsset.SpeedButton();
            this.BtnTitleBlowOutput03 = new UiAsset.SpeedButton();
            this.BtnTitleBlowOutput02 = new UiAsset.SpeedButton();
            this.BtnTitleBlowOutput01 = new UiAsset.SpeedButton();
            this.BtnVacuumOutput04 = new UiAsset.SpeedButton();
            this.BtnVacuumOutput03 = new UiAsset.SpeedButton();
            this.BtnVacuumOutput02 = new UiAsset.SpeedButton();
            this.BtnVacuumOutput01 = new UiAsset.SpeedButton();
            this.BtnBlowOutput04 = new UiAsset.SpeedButton();
            this.BtnBlowOutput03 = new UiAsset.SpeedButton();
            this.BtnBlowOutput02 = new UiAsset.SpeedButton();
            this.BtnBlowOutput01 = new UiAsset.SpeedButton();
            this.BtnVacuumInput04 = new UiAsset.SpeedButton();
            this.BtnVacuumInput03 = new UiAsset.SpeedButton();
            this.BtnVacuumInput02 = new UiAsset.SpeedButton();
            this.BtnVacuumInput01 = new UiAsset.SpeedButton();
            this.BtnVacuumAnalogInput04 = new UiAsset.SpeedButton();
            this.BtnVacuumAnalogInput03 = new UiAsset.SpeedButton();
            this.BtnVacuumAnalogInput02 = new UiAsset.SpeedButton();
            this.BtnVacuumAnalogInput01 = new UiAsset.SpeedButton();
            this.BtnTitleVacuumInput04 = new UiAsset.SpeedButton();
            this.BtnTitleVacuumInput03 = new UiAsset.SpeedButton();
            this.BtnTitleVacuumInput02 = new UiAsset.SpeedButton();
            this.BtnTitleVacuumInput01 = new UiAsset.SpeedButton();
            this.BtnTitleVacuumAnalogInput = new UiAsset.SpeedButton();
            this.BtnTitleVacuumInput = new UiAsset.SpeedButton();
            this.LblSensorOffTime = new System.Windows.Forms.Label();
            this.LblBlowSettingTime = new System.Windows.Forms.Label();
            this.LblSensorOnTime = new System.Windows.Forms.Label();
            this.LblVaccumSettingTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewVacuumList)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnVacuumOffDelayTime
            // 
            this.BtnVacuumOffDelayTime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumOffDelayTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumOffDelayTime.Location = new System.Drawing.Point(1064, 64);
            this.BtnVacuumOffDelayTime.Name = "BtnVacuumOffDelayTime";
            this.BtnVacuumOffDelayTime.Size = new System.Drawing.Size(200, 46);
            this.BtnVacuumOffDelayTime.TabIndex = 85;
            this.BtnVacuumOffDelayTime.TabStop = false;
            this.BtnVacuumOffDelayTime.UseVisualStyleBackColor = true;
            this.BtnVacuumOffDelayTime.Click += new System.EventHandler(this.BtnVacuumOffDelayTime_Click);
            // 
            // BtnTitleVacuumOffDelayTime
            // 
            this.BtnTitleVacuumOffDelayTime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumOffDelayTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumOffDelayTime.Location = new System.Drawing.Point(857, 64);
            this.BtnTitleVacuumOffDelayTime.Name = "BtnTitleVacuumOffDelayTime";
            this.BtnTitleVacuumOffDelayTime.Size = new System.Drawing.Size(200, 46);
            this.BtnTitleVacuumOffDelayTime.TabIndex = 77;
            this.BtnTitleVacuumOffDelayTime.TabStop = false;
            this.BtnTitleVacuumOffDelayTime.Text = "BLOW AFTER DELAY TIME";
            this.BtnTitleVacuumOffDelayTime.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnVacuumTimeout
            // 
            this.BtnVacuumTimeout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumTimeout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumTimeout.Location = new System.Drawing.Point(651, 64);
            this.BtnVacuumTimeout.Name = "BtnVacuumTimeout";
            this.BtnVacuumTimeout.Size = new System.Drawing.Size(200, 46);
            this.BtnVacuumTimeout.TabIndex = 78;
            this.BtnVacuumTimeout.TabStop = false;
            this.BtnVacuumTimeout.UseVisualStyleBackColor = true;
            this.BtnVacuumTimeout.Click += new System.EventHandler(this.BtnVacuumTimeout_Click);
            // 
            // BtnTitleVacuumTimeout
            // 
            this.BtnTitleVacuumTimeout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumTimeout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumTimeout.Location = new System.Drawing.Point(445, 64);
            this.BtnTitleVacuumTimeout.Name = "BtnTitleVacuumTimeout";
            this.BtnTitleVacuumTimeout.Size = new System.Drawing.Size(200, 46);
            this.BtnTitleVacuumTimeout.TabIndex = 81;
            this.BtnTitleVacuumTimeout.TabStop = false;
            this.BtnTitleVacuumTimeout.Text = "VACUUM TIMEOUT";
            this.BtnTitleVacuumTimeout.UseVisualStyleBackColor = true;
            // 
            // GridViewVacuumList
            // 
            this.GridViewVacuumList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridViewVacuumList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.GridViewVacuumList.ColumnHeadersHeight = 30;
            this.GridViewVacuumList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.GridViewVacuumList.Location = new System.Drawing.Point(12, 64);
            this.GridViewVacuumList.MultiSelect = false;
            this.GridViewVacuumList.Name = "GridViewVacuumList";
            this.GridViewVacuumList.ReadOnly = true;
            this.GridViewVacuumList.RowTemplate.Height = 35;
            this.GridViewVacuumList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridViewVacuumList.Size = new System.Drawing.Size(427, 618);
            this.GridViewVacuumList.TabIndex = 69;
            this.GridViewVacuumList.SelectionChanged += new System.EventHandler(this.GridViewVacuumList_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column1.HeaderText = "VACUUM NAME";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // BtnTitleVacuumDelayTime
            // 
            this.BtnTitleVacuumDelayTime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumDelayTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumDelayTime.Location = new System.Drawing.Point(445, 12);
            this.BtnTitleVacuumDelayTime.Name = "BtnTitleVacuumDelayTime";
            this.BtnTitleVacuumDelayTime.Size = new System.Drawing.Size(819, 46);
            this.BtnTitleVacuumDelayTime.TabIndex = 68;
            this.BtnTitleVacuumDelayTime.TabStop = false;
            this.BtnTitleVacuumDelayTime.Text = "VACUUM DELAY TIME";
            this.BtnTitleVacuumDelayTime.UseVisualStyleBackColor = true;
            // 
            // BtnTitleVacuumList
            // 
            this.BtnTitleVacuumList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumList.Location = new System.Drawing.Point(12, 12);
            this.BtnTitleVacuumList.Name = "BtnTitleVacuumList";
            this.BtnTitleVacuumList.Size = new System.Drawing.Size(427, 46);
            this.BtnTitleVacuumList.TabIndex = 66;
            this.BtnTitleVacuumList.TabStop = false;
            this.BtnTitleVacuumList.Text = "VACUUM LIST";
            this.BtnTitleVacuumList.UseVisualStyleBackColor = true;
            // 
            // BtnTitleBlowOutput
            // 
            this.BtnTitleBlowOutput.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleBlowOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleBlowOutput.Location = new System.Drawing.Point(857, 220);
            this.BtnTitleBlowOutput.Name = "BtnTitleBlowOutput";
            this.BtnTitleBlowOutput.Size = new System.Drawing.Size(407, 46);
            this.BtnTitleBlowOutput.TabIndex = 91;
            this.BtnTitleBlowOutput.TabStop = false;
            this.BtnTitleBlowOutput.Text = "BLOW OUTPUT";
            this.BtnTitleBlowOutput.UseVisualStyleBackColor = true;
            // 
            // BtnTitleVacuumOutput
            // 
            this.BtnTitleVacuumOutput.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumOutput.Location = new System.Drawing.Point(445, 220);
            this.BtnTitleVacuumOutput.Name = "BtnTitleVacuumOutput";
            this.BtnTitleVacuumOutput.Size = new System.Drawing.Size(406, 46);
            this.BtnTitleVacuumOutput.TabIndex = 88;
            this.BtnTitleVacuumOutput.TabStop = false;
            this.BtnTitleVacuumOutput.Text = "VACUUM OUTPUT";
            this.BtnTitleVacuumOutput.UseVisualStyleBackColor = true;
            // 
            // BtnVacuum
            // 
            this.BtnVacuum.BackColor = System.Drawing.Color.Transparent;
            this.BtnVacuum.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnVacuum.ButtonText = "VACUUM";
            this.BtnVacuum.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnVacuum.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuum.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnVacuum.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnVacuum.Location = new System.Drawing.Point(445, 168);
            this.BtnVacuum.Name = "BtnVacuum";
            this.BtnVacuum.Size = new System.Drawing.Size(200, 46);
            this.BtnVacuum.TabIndex = 92;
            this.BtnVacuum.Text = "VACUUM";
            this.BtnVacuum.UseVisualStyleBackColor = false;
            this.BtnVacuum.Click += new System.EventHandler(this.BtnVacuum_Click);
            // 
            // BtnBlow
            // 
            this.BtnBlow.BackColor = System.Drawing.Color.Transparent;
            this.BtnBlow.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBlow.ButtonText = "BLOW";
            this.BtnBlow.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnBlow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBlow.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnBlow.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBlow.Location = new System.Drawing.Point(651, 168);
            this.BtnBlow.Name = "BtnBlow";
            this.BtnBlow.Size = new System.Drawing.Size(200, 46);
            this.BtnBlow.TabIndex = 93;
            this.BtnBlow.Text = "BLOW";
            this.BtnBlow.UseVisualStyleBackColor = false;
            this.BtnBlow.Click += new System.EventHandler(this.BtnBlow_Click);
            // 
            // BtnOutputLock
            // 
            this.BtnOutputLock.BackColor = System.Drawing.Color.Transparent;
            this.BtnOutputLock.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOutputLock.ButtonText = "OUTPUT LOCK";
            this.BtnOutputLock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOutputLock.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnOutputLock.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOutputLock.Location = new System.Drawing.Point(857, 168);
            this.BtnOutputLock.Name = "BtnOutputLock";
            this.BtnOutputLock.Size = new System.Drawing.Size(407, 46);
            this.BtnOutputLock.TabIndex = 94;
            this.BtnOutputLock.Text = "OUTPUT LOCK";
            this.BtnOutputLock.UseVisualStyleBackColor = false;
            this.BtnOutputLock.Click += new System.EventHandler(this.BtnOutputLock_Click);
            // 
            // BtnTitleVacuumOutput04
            // 
            this.BtnTitleVacuumOutput04.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumOutput04.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumOutput04.Location = new System.Drawing.Point(445, 428);
            this.BtnTitleVacuumOutput04.Name = "BtnTitleVacuumOutput04";
            this.BtnTitleVacuumOutput04.Size = new System.Drawing.Size(64, 46);
            this.BtnTitleVacuumOutput04.TabIndex = 102;
            this.BtnTitleVacuumOutput04.TabStop = false;
            this.BtnTitleVacuumOutput04.Text = "Y000";
            this.BtnTitleVacuumOutput04.UseVisualStyleBackColor = true;
            this.BtnTitleVacuumOutput04.Visible = false;
            // 
            // BtnTitleVacuumOutput03
            // 
            this.BtnTitleVacuumOutput03.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumOutput03.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumOutput03.Location = new System.Drawing.Point(445, 376);
            this.BtnTitleVacuumOutput03.Name = "BtnTitleVacuumOutput03";
            this.BtnTitleVacuumOutput03.Size = new System.Drawing.Size(64, 46);
            this.BtnTitleVacuumOutput03.TabIndex = 101;
            this.BtnTitleVacuumOutput03.TabStop = false;
            this.BtnTitleVacuumOutput03.Text = "Y000";
            this.BtnTitleVacuumOutput03.UseVisualStyleBackColor = true;
            this.BtnTitleVacuumOutput03.Visible = false;
            // 
            // BtnTitleVacuumOutput02
            // 
            this.BtnTitleVacuumOutput02.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumOutput02.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumOutput02.Location = new System.Drawing.Point(445, 324);
            this.BtnTitleVacuumOutput02.Name = "BtnTitleVacuumOutput02";
            this.BtnTitleVacuumOutput02.Size = new System.Drawing.Size(64, 46);
            this.BtnTitleVacuumOutput02.TabIndex = 100;
            this.BtnTitleVacuumOutput02.TabStop = false;
            this.BtnTitleVacuumOutput02.Text = "Y000";
            this.BtnTitleVacuumOutput02.UseVisualStyleBackColor = true;
            this.BtnTitleVacuumOutput02.Visible = false;
            // 
            // BtnTitleVacuumOutput01
            // 
            this.BtnTitleVacuumOutput01.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumOutput01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumOutput01.Location = new System.Drawing.Point(445, 272);
            this.BtnTitleVacuumOutput01.Name = "BtnTitleVacuumOutput01";
            this.BtnTitleVacuumOutput01.Size = new System.Drawing.Size(64, 46);
            this.BtnTitleVacuumOutput01.TabIndex = 99;
            this.BtnTitleVacuumOutput01.TabStop = false;
            this.BtnTitleVacuumOutput01.Text = "Y000";
            this.BtnTitleVacuumOutput01.UseVisualStyleBackColor = true;
            this.BtnTitleVacuumOutput01.Visible = false;
            // 
            // BtnTitleBlowOutput04
            // 
            this.BtnTitleBlowOutput04.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleBlowOutput04.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleBlowOutput04.Location = new System.Drawing.Point(857, 428);
            this.BtnTitleBlowOutput04.Name = "BtnTitleBlowOutput04";
            this.BtnTitleBlowOutput04.Size = new System.Drawing.Size(64, 46);
            this.BtnTitleBlowOutput04.TabIndex = 110;
            this.BtnTitleBlowOutput04.TabStop = false;
            this.BtnTitleBlowOutput04.Text = "Y000";
            this.BtnTitleBlowOutput04.UseVisualStyleBackColor = true;
            this.BtnTitleBlowOutput04.Visible = false;
            // 
            // BtnTitleBlowOutput03
            // 
            this.BtnTitleBlowOutput03.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleBlowOutput03.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleBlowOutput03.Location = new System.Drawing.Point(857, 376);
            this.BtnTitleBlowOutput03.Name = "BtnTitleBlowOutput03";
            this.BtnTitleBlowOutput03.Size = new System.Drawing.Size(64, 46);
            this.BtnTitleBlowOutput03.TabIndex = 109;
            this.BtnTitleBlowOutput03.TabStop = false;
            this.BtnTitleBlowOutput03.Text = "Y000";
            this.BtnTitleBlowOutput03.UseVisualStyleBackColor = true;
            this.BtnTitleBlowOutput03.Visible = false;
            // 
            // BtnTitleBlowOutput02
            // 
            this.BtnTitleBlowOutput02.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleBlowOutput02.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleBlowOutput02.Location = new System.Drawing.Point(857, 324);
            this.BtnTitleBlowOutput02.Name = "BtnTitleBlowOutput02";
            this.BtnTitleBlowOutput02.Size = new System.Drawing.Size(64, 46);
            this.BtnTitleBlowOutput02.TabIndex = 108;
            this.BtnTitleBlowOutput02.TabStop = false;
            this.BtnTitleBlowOutput02.Text = "Y000";
            this.BtnTitleBlowOutput02.UseVisualStyleBackColor = true;
            this.BtnTitleBlowOutput02.Visible = false;
            // 
            // BtnTitleBlowOutput01
            // 
            this.BtnTitleBlowOutput01.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleBlowOutput01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleBlowOutput01.Location = new System.Drawing.Point(857, 272);
            this.BtnTitleBlowOutput01.Name = "BtnTitleBlowOutput01";
            this.BtnTitleBlowOutput01.Size = new System.Drawing.Size(64, 46);
            this.BtnTitleBlowOutput01.TabIndex = 107;
            this.BtnTitleBlowOutput01.TabStop = false;
            this.BtnTitleBlowOutput01.Text = "Y000";
            this.BtnTitleBlowOutput01.UseVisualStyleBackColor = true;
            this.BtnTitleBlowOutput01.Visible = false;
            // 
            // BtnVacuumOutput04
            // 
            this.BtnVacuumOutput04.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumOutput04.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumOutput04.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.BtnVacuumOutput04.Location = new System.Drawing.Point(515, 428);
            this.BtnVacuumOutput04.Name = "BtnVacuumOutput04";
            this.BtnVacuumOutput04.Size = new System.Drawing.Size(336, 46);
            this.BtnVacuumOutput04.TabIndex = 118;
            this.BtnVacuumOutput04.TabStop = false;
            this.BtnVacuumOutput04.Tag = "3";
            this.BtnVacuumOutput04.UseVisualStyleBackColor = true;
            this.BtnVacuumOutput04.Visible = false;
            this.BtnVacuumOutput04.Click += new System.EventHandler(this.BtnVacuumOutput_Click);
            // 
            // BtnVacuumOutput03
            // 
            this.BtnVacuumOutput03.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumOutput03.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumOutput03.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.BtnVacuumOutput03.Location = new System.Drawing.Point(515, 376);
            this.BtnVacuumOutput03.Name = "BtnVacuumOutput03";
            this.BtnVacuumOutput03.Size = new System.Drawing.Size(336, 46);
            this.BtnVacuumOutput03.TabIndex = 117;
            this.BtnVacuumOutput03.TabStop = false;
            this.BtnVacuumOutput03.Tag = "2";
            this.BtnVacuumOutput03.UseVisualStyleBackColor = true;
            this.BtnVacuumOutput03.Visible = false;
            this.BtnVacuumOutput03.Click += new System.EventHandler(this.BtnVacuumOutput_Click);
            // 
            // BtnVacuumOutput02
            // 
            this.BtnVacuumOutput02.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumOutput02.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumOutput02.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.BtnVacuumOutput02.Location = new System.Drawing.Point(515, 324);
            this.BtnVacuumOutput02.Name = "BtnVacuumOutput02";
            this.BtnVacuumOutput02.Size = new System.Drawing.Size(336, 46);
            this.BtnVacuumOutput02.TabIndex = 116;
            this.BtnVacuumOutput02.TabStop = false;
            this.BtnVacuumOutput02.Tag = "1";
            this.BtnVacuumOutput02.UseVisualStyleBackColor = true;
            this.BtnVacuumOutput02.Visible = false;
            this.BtnVacuumOutput02.Click += new System.EventHandler(this.BtnVacuumOutput_Click);
            // 
            // BtnVacuumOutput01
            // 
            this.BtnVacuumOutput01.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumOutput01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumOutput01.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.BtnVacuumOutput01.Location = new System.Drawing.Point(515, 272);
            this.BtnVacuumOutput01.Name = "BtnVacuumOutput01";
            this.BtnVacuumOutput01.Size = new System.Drawing.Size(336, 46);
            this.BtnVacuumOutput01.TabIndex = 115;
            this.BtnVacuumOutput01.TabStop = false;
            this.BtnVacuumOutput01.Tag = "0";
            this.BtnVacuumOutput01.UseVisualStyleBackColor = true;
            this.BtnVacuumOutput01.Visible = false;
            this.BtnVacuumOutput01.Click += new System.EventHandler(this.BtnVacuumOutput_Click);
            // 
            // BtnBlowOutput04
            // 
            this.BtnBlowOutput04.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBlowOutput04.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBlowOutput04.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.BtnBlowOutput04.Location = new System.Drawing.Point(927, 428);
            this.BtnBlowOutput04.Name = "BtnBlowOutput04";
            this.BtnBlowOutput04.Size = new System.Drawing.Size(337, 46);
            this.BtnBlowOutput04.TabIndex = 122;
            this.BtnBlowOutput04.TabStop = false;
            this.BtnBlowOutput04.Tag = "3";
            this.BtnBlowOutput04.UseVisualStyleBackColor = true;
            this.BtnBlowOutput04.Visible = false;
            this.BtnBlowOutput04.Click += new System.EventHandler(this.BtnVacuumOutput_Click);
            // 
            // BtnBlowOutput03
            // 
            this.BtnBlowOutput03.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBlowOutput03.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBlowOutput03.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.BtnBlowOutput03.Location = new System.Drawing.Point(927, 376);
            this.BtnBlowOutput03.Name = "BtnBlowOutput03";
            this.BtnBlowOutput03.Size = new System.Drawing.Size(337, 46);
            this.BtnBlowOutput03.TabIndex = 121;
            this.BtnBlowOutput03.TabStop = false;
            this.BtnBlowOutput03.Tag = "2";
            this.BtnBlowOutput03.UseVisualStyleBackColor = true;
            this.BtnBlowOutput03.Visible = false;
            this.BtnBlowOutput03.Click += new System.EventHandler(this.BtnVacuumOutput_Click);
            // 
            // BtnBlowOutput02
            // 
            this.BtnBlowOutput02.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBlowOutput02.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBlowOutput02.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.BtnBlowOutput02.Location = new System.Drawing.Point(927, 324);
            this.BtnBlowOutput02.Name = "BtnBlowOutput02";
            this.BtnBlowOutput02.Size = new System.Drawing.Size(337, 46);
            this.BtnBlowOutput02.TabIndex = 120;
            this.BtnBlowOutput02.TabStop = false;
            this.BtnBlowOutput02.Tag = "1";
            this.BtnBlowOutput02.UseVisualStyleBackColor = true;
            this.BtnBlowOutput02.Visible = false;
            this.BtnBlowOutput02.Click += new System.EventHandler(this.BtnVacuumOutput_Click);
            // 
            // BtnBlowOutput01
            // 
            this.BtnBlowOutput01.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBlowOutput01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBlowOutput01.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.BtnBlowOutput01.Location = new System.Drawing.Point(927, 272);
            this.BtnBlowOutput01.Name = "BtnBlowOutput01";
            this.BtnBlowOutput01.Size = new System.Drawing.Size(337, 46);
            this.BtnBlowOutput01.TabIndex = 119;
            this.BtnBlowOutput01.TabStop = false;
            this.BtnBlowOutput01.Tag = "0";
            this.BtnBlowOutput01.UseVisualStyleBackColor = true;
            this.BtnBlowOutput01.Visible = false;
            this.BtnBlowOutput01.Click += new System.EventHandler(this.BtnVacuumOutput_Click);
            // 
            // BtnVacuumInput04
            // 
            this.BtnVacuumInput04.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumInput04.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumInput04.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.BtnVacuumInput04.Location = new System.Drawing.Point(515, 688);
            this.BtnVacuumInput04.Name = "BtnVacuumInput04";
            this.BtnVacuumInput04.Size = new System.Drawing.Size(336, 46);
            this.BtnVacuumInput04.TabIndex = 136;
            this.BtnVacuumInput04.TabStop = false;
            this.BtnVacuumInput04.Tag = "3";
            this.BtnVacuumInput04.UseVisualStyleBackColor = true;
            this.BtnVacuumInput04.Visible = false;
            // 
            // BtnVacuumInput03
            // 
            this.BtnVacuumInput03.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumInput03.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumInput03.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.BtnVacuumInput03.Location = new System.Drawing.Point(515, 636);
            this.BtnVacuumInput03.Name = "BtnVacuumInput03";
            this.BtnVacuumInput03.Size = new System.Drawing.Size(336, 46);
            this.BtnVacuumInput03.TabIndex = 135;
            this.BtnVacuumInput03.TabStop = false;
            this.BtnVacuumInput03.Tag = "2";
            this.BtnVacuumInput03.UseVisualStyleBackColor = true;
            this.BtnVacuumInput03.Visible = false;
            // 
            // BtnVacuumInput02
            // 
            this.BtnVacuumInput02.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumInput02.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumInput02.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.BtnVacuumInput02.Location = new System.Drawing.Point(515, 584);
            this.BtnVacuumInput02.Name = "BtnVacuumInput02";
            this.BtnVacuumInput02.Size = new System.Drawing.Size(336, 46);
            this.BtnVacuumInput02.TabIndex = 134;
            this.BtnVacuumInput02.TabStop = false;
            this.BtnVacuumInput02.Tag = "1";
            this.BtnVacuumInput02.UseVisualStyleBackColor = true;
            this.BtnVacuumInput02.Visible = false;
            // 
            // BtnVacuumInput01
            // 
            this.BtnVacuumInput01.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumInput01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumInput01.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.BtnVacuumInput01.Location = new System.Drawing.Point(515, 532);
            this.BtnVacuumInput01.Name = "BtnVacuumInput01";
            this.BtnVacuumInput01.Size = new System.Drawing.Size(336, 46);
            this.BtnVacuumInput01.TabIndex = 133;
            this.BtnVacuumInput01.TabStop = false;
            this.BtnVacuumInput01.Tag = "0";
            this.BtnVacuumInput01.UseVisualStyleBackColor = true;
            this.BtnVacuumInput01.Visible = false;
            // 
            // BtnVacuumAnalogInput04
            // 
            this.BtnVacuumAnalogInput04.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumAnalogInput04.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumAnalogInput04.Location = new System.Drawing.Point(857, 688);
            this.BtnVacuumAnalogInput04.Name = "BtnVacuumAnalogInput04";
            this.BtnVacuumAnalogInput04.Size = new System.Drawing.Size(407, 46);
            this.BtnVacuumAnalogInput04.TabIndex = 132;
            this.BtnVacuumAnalogInput04.TabStop = false;
            this.BtnVacuumAnalogInput04.Tag = "3";
            this.BtnVacuumAnalogInput04.Text = "0.0 kPa";
            this.BtnVacuumAnalogInput04.UseVisualStyleBackColor = true;
            this.BtnVacuumAnalogInput04.Visible = false;
            this.BtnVacuumAnalogInput04.Click += new System.EventHandler(this.BtnVacuumAnalogInput_Click);
            // 
            // BtnVacuumAnalogInput03
            // 
            this.BtnVacuumAnalogInput03.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumAnalogInput03.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumAnalogInput03.Location = new System.Drawing.Point(857, 636);
            this.BtnVacuumAnalogInput03.Name = "BtnVacuumAnalogInput03";
            this.BtnVacuumAnalogInput03.Size = new System.Drawing.Size(407, 46);
            this.BtnVacuumAnalogInput03.TabIndex = 131;
            this.BtnVacuumAnalogInput03.TabStop = false;
            this.BtnVacuumAnalogInput03.Tag = "2";
            this.BtnVacuumAnalogInput03.Text = "0.0 kPa";
            this.BtnVacuumAnalogInput03.UseVisualStyleBackColor = true;
            this.BtnVacuumAnalogInput03.Visible = false;
            this.BtnVacuumAnalogInput03.Click += new System.EventHandler(this.BtnVacuumAnalogInput_Click);
            // 
            // BtnVacuumAnalogInput02
            // 
            this.BtnVacuumAnalogInput02.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumAnalogInput02.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumAnalogInput02.Location = new System.Drawing.Point(857, 584);
            this.BtnVacuumAnalogInput02.Name = "BtnVacuumAnalogInput02";
            this.BtnVacuumAnalogInput02.Size = new System.Drawing.Size(407, 46);
            this.BtnVacuumAnalogInput02.TabIndex = 130;
            this.BtnVacuumAnalogInput02.TabStop = false;
            this.BtnVacuumAnalogInput02.Tag = "1";
            this.BtnVacuumAnalogInput02.Text = "0.0 kPa";
            this.BtnVacuumAnalogInput02.UseVisualStyleBackColor = true;
            this.BtnVacuumAnalogInput02.Visible = false;
            this.BtnVacuumAnalogInput02.Click += new System.EventHandler(this.BtnVacuumAnalogInput_Click);
            // 
            // BtnVacuumAnalogInput01
            // 
            this.BtnVacuumAnalogInput01.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuumAnalogInput01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVacuumAnalogInput01.Location = new System.Drawing.Point(857, 532);
            this.BtnVacuumAnalogInput01.Name = "BtnVacuumAnalogInput01";
            this.BtnVacuumAnalogInput01.Size = new System.Drawing.Size(407, 46);
            this.BtnVacuumAnalogInput01.TabIndex = 129;
            this.BtnVacuumAnalogInput01.TabStop = false;
            this.BtnVacuumAnalogInput01.Tag = "0";
            this.BtnVacuumAnalogInput01.Text = "0.0 kPa";
            this.BtnVacuumAnalogInput01.UseVisualStyleBackColor = true;
            this.BtnVacuumAnalogInput01.Visible = false;
            this.BtnVacuumAnalogInput01.Click += new System.EventHandler(this.BtnVacuumAnalogInput_Click);
            // 
            // BtnTitleVacuumInput04
            // 
            this.BtnTitleVacuumInput04.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumInput04.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumInput04.Location = new System.Drawing.Point(445, 688);
            this.BtnTitleVacuumInput04.Name = "BtnTitleVacuumInput04";
            this.BtnTitleVacuumInput04.Size = new System.Drawing.Size(64, 46);
            this.BtnTitleVacuumInput04.TabIndex = 128;
            this.BtnTitleVacuumInput04.TabStop = false;
            this.BtnTitleVacuumInput04.Text = "X000";
            this.BtnTitleVacuumInput04.UseVisualStyleBackColor = true;
            this.BtnTitleVacuumInput04.Visible = false;
            // 
            // BtnTitleVacuumInput03
            // 
            this.BtnTitleVacuumInput03.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumInput03.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumInput03.Location = new System.Drawing.Point(445, 636);
            this.BtnTitleVacuumInput03.Name = "BtnTitleVacuumInput03";
            this.BtnTitleVacuumInput03.Size = new System.Drawing.Size(64, 46);
            this.BtnTitleVacuumInput03.TabIndex = 127;
            this.BtnTitleVacuumInput03.TabStop = false;
            this.BtnTitleVacuumInput03.Text = "X000";
            this.BtnTitleVacuumInput03.UseVisualStyleBackColor = true;
            this.BtnTitleVacuumInput03.Visible = false;
            // 
            // BtnTitleVacuumInput02
            // 
            this.BtnTitleVacuumInput02.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumInput02.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumInput02.Location = new System.Drawing.Point(445, 584);
            this.BtnTitleVacuumInput02.Name = "BtnTitleVacuumInput02";
            this.BtnTitleVacuumInput02.Size = new System.Drawing.Size(64, 46);
            this.BtnTitleVacuumInput02.TabIndex = 126;
            this.BtnTitleVacuumInput02.TabStop = false;
            this.BtnTitleVacuumInput02.Text = "X000";
            this.BtnTitleVacuumInput02.UseVisualStyleBackColor = true;
            this.BtnTitleVacuumInput02.Visible = false;
            // 
            // BtnTitleVacuumInput01
            // 
            this.BtnTitleVacuumInput01.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumInput01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumInput01.Location = new System.Drawing.Point(445, 532);
            this.BtnTitleVacuumInput01.Name = "BtnTitleVacuumInput01";
            this.BtnTitleVacuumInput01.Size = new System.Drawing.Size(64, 46);
            this.BtnTitleVacuumInput01.TabIndex = 125;
            this.BtnTitleVacuumInput01.TabStop = false;
            this.BtnTitleVacuumInput01.Text = "X000";
            this.BtnTitleVacuumInput01.UseVisualStyleBackColor = true;
            this.BtnTitleVacuumInput01.Visible = false;
            // 
            // BtnTitleVacuumAnalogInput
            // 
            this.BtnTitleVacuumAnalogInput.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumAnalogInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumAnalogInput.Location = new System.Drawing.Point(857, 480);
            this.BtnTitleVacuumAnalogInput.Name = "BtnTitleVacuumAnalogInput";
            this.BtnTitleVacuumAnalogInput.Size = new System.Drawing.Size(407, 46);
            this.BtnTitleVacuumAnalogInput.TabIndex = 123;
            this.BtnTitleVacuumAnalogInput.TabStop = false;
            this.BtnTitleVacuumAnalogInput.Text = "VACUUM ANALOG INPUT";
            this.BtnTitleVacuumAnalogInput.UseVisualStyleBackColor = true;
            this.BtnTitleVacuumAnalogInput.Click += new System.EventHandler(this.BtnTitleVacuumAnalogInput_Click);
            // 
            // BtnTitleVacuumInput
            // 
            this.BtnTitleVacuumInput.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVacuumInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVacuumInput.Location = new System.Drawing.Point(445, 480);
            this.BtnTitleVacuumInput.Name = "BtnTitleVacuumInput";
            this.BtnTitleVacuumInput.Size = new System.Drawing.Size(406, 46);
            this.BtnTitleVacuumInput.TabIndex = 124;
            this.BtnTitleVacuumInput.TabStop = false;
            this.BtnTitleVacuumInput.Text = "VACUUM INPUT";
            this.BtnTitleVacuumInput.UseVisualStyleBackColor = true;
            // 
            // LblSensorOffTime
            // 
            this.LblSensorOffTime.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.LblSensorOffTime.Location = new System.Drawing.Point(235, 716);
            this.LblSensorOffTime.Name = "LblSensorOffTime";
            this.LblSensorOffTime.Size = new System.Drawing.Size(204, 24);
            this.LblSensorOffTime.TabIndex = 140;
            this.LblSensorOffTime.Text = "-";
            this.LblSensorOffTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblBlowSettingTime
            // 
            this.LblBlowSettingTime.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.LblBlowSettingTime.Location = new System.Drawing.Point(12, 716);
            this.LblBlowSettingTime.Name = "LblBlowSettingTime";
            this.LblBlowSettingTime.Size = new System.Drawing.Size(214, 24);
            this.LblBlowSettingTime.TabIndex = 141;
            this.LblBlowSettingTime.Text = "-";
            this.LblBlowSettingTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblSensorOnTime
            // 
            this.LblSensorOnTime.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.LblSensorOnTime.Location = new System.Drawing.Point(235, 688);
            this.LblSensorOnTime.Name = "LblSensorOnTime";
            this.LblSensorOnTime.Size = new System.Drawing.Size(204, 24);
            this.LblSensorOnTime.TabIndex = 142;
            this.LblSensorOnTime.Text = "-";
            this.LblSensorOnTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblVaccumSettingTime
            // 
            this.LblVaccumSettingTime.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblVaccumSettingTime.Location = new System.Drawing.Point(12, 688);
            this.LblVaccumSettingTime.Name = "LblVaccumSettingTime";
            this.LblVaccumSettingTime.Size = new System.Drawing.Size(214, 24);
            this.LblVaccumSettingTime.TabIndex = 143;
            this.LblVaccumSettingTime.Text = "-";
            this.LblVaccumSettingTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CFormTeachVacuum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.Controls.Add(this.LblSensorOffTime);
            this.Controls.Add(this.LblBlowSettingTime);
            this.Controls.Add(this.LblSensorOnTime);
            this.Controls.Add(this.LblVaccumSettingTime);
            this.Controls.Add(this.BtnVacuumInput04);
            this.Controls.Add(this.BtnVacuumInput03);
            this.Controls.Add(this.BtnVacuumInput02);
            this.Controls.Add(this.BtnVacuumInput01);
            this.Controls.Add(this.BtnVacuumAnalogInput04);
            this.Controls.Add(this.BtnVacuumAnalogInput03);
            this.Controls.Add(this.BtnVacuumAnalogInput02);
            this.Controls.Add(this.BtnVacuumAnalogInput01);
            this.Controls.Add(this.BtnTitleVacuumInput04);
            this.Controls.Add(this.BtnTitleVacuumInput03);
            this.Controls.Add(this.BtnTitleVacuumInput02);
            this.Controls.Add(this.BtnTitleVacuumInput01);
            this.Controls.Add(this.BtnTitleVacuumAnalogInput);
            this.Controls.Add(this.BtnTitleVacuumInput);
            this.Controls.Add(this.BtnBlowOutput04);
            this.Controls.Add(this.BtnBlowOutput03);
            this.Controls.Add(this.BtnBlowOutput02);
            this.Controls.Add(this.BtnBlowOutput01);
            this.Controls.Add(this.BtnVacuumOutput04);
            this.Controls.Add(this.BtnVacuumOutput03);
            this.Controls.Add(this.BtnVacuumOutput02);
            this.Controls.Add(this.BtnVacuumOutput01);
            this.Controls.Add(this.BtnTitleBlowOutput04);
            this.Controls.Add(this.BtnTitleBlowOutput03);
            this.Controls.Add(this.BtnTitleBlowOutput02);
            this.Controls.Add(this.BtnTitleBlowOutput01);
            this.Controls.Add(this.BtnTitleVacuumOutput04);
            this.Controls.Add(this.BtnTitleVacuumOutput03);
            this.Controls.Add(this.BtnTitleVacuumOutput02);
            this.Controls.Add(this.BtnTitleVacuumOutput01);
            this.Controls.Add(this.BtnOutputLock);
            this.Controls.Add(this.BtnBlow);
            this.Controls.Add(this.BtnVacuum);
            this.Controls.Add(this.BtnTitleBlowOutput);
            this.Controls.Add(this.BtnTitleVacuumOutput);
            this.Controls.Add(this.BtnVacuumOffDelayTime);
            this.Controls.Add(this.BtnTitleVacuumOffDelayTime);
            this.Controls.Add(this.BtnVacuumTimeout);
            this.Controls.Add(this.BtnTitleVacuumTimeout);
            this.Controls.Add(this.GridViewVacuumList);
            this.Controls.Add(this.BtnTitleVacuumDelayTime);
            this.Controls.Add(this.BtnTitleVacuumList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormTeachVacuum";
            this.Text = "CFormTeachVacuum";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormTeachVacuum_FormClosed);
            this.Load += new System.EventHandler(this.CFormTeachVacuum_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CFormTeachVacuum_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewVacuumList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UiAsset.SpeedButton BtnVacuumOffDelayTime;
        private UiAsset.SpeedButton BtnTitleVacuumOffDelayTime;
        private System.Windows.Forms.Timer timer;
        private UiAsset.SpeedButton BtnVacuumTimeout;
        private UiAsset.SpeedButton BtnTitleVacuumTimeout;
        private System.Windows.Forms.DataGridView GridViewVacuumList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private UiAsset.SpeedButton BtnTitleVacuumDelayTime;
        private UiAsset.SpeedButton BtnTitleVacuumList;
        private UiAsset.SpeedButton BtnTitleBlowOutput;
        private UiAsset.SpeedButton BtnTitleVacuumOutput;
        private UiAsset.ImageButton BtnVacuum;
        private UiAsset.ImageButton BtnBlow;
        private UiAsset.ImageButton BtnOutputLock;
        private UiAsset.SpeedButton BtnTitleVacuumOutput04;
        private UiAsset.SpeedButton BtnTitleVacuumOutput03;
        private UiAsset.SpeedButton BtnTitleVacuumOutput02;
        private UiAsset.SpeedButton BtnTitleVacuumOutput01;
        private UiAsset.SpeedButton BtnTitleBlowOutput04;
        private UiAsset.SpeedButton BtnTitleBlowOutput03;
        private UiAsset.SpeedButton BtnTitleBlowOutput02;
        private UiAsset.SpeedButton BtnTitleBlowOutput01;
        private UiAsset.SpeedButton BtnVacuumOutput04;
        private UiAsset.SpeedButton BtnVacuumOutput03;
        private UiAsset.SpeedButton BtnVacuumOutput02;
        private UiAsset.SpeedButton BtnVacuumOutput01;
        private UiAsset.SpeedButton BtnBlowOutput04;
        private UiAsset.SpeedButton BtnBlowOutput03;
        private UiAsset.SpeedButton BtnBlowOutput02;
        private UiAsset.SpeedButton BtnBlowOutput01;
        private UiAsset.SpeedButton BtnVacuumInput04;
        private UiAsset.SpeedButton BtnVacuumInput03;
        private UiAsset.SpeedButton BtnVacuumInput02;
        private UiAsset.SpeedButton BtnVacuumInput01;
        private UiAsset.SpeedButton BtnVacuumAnalogInput04;
        private UiAsset.SpeedButton BtnVacuumAnalogInput03;
        private UiAsset.SpeedButton BtnVacuumAnalogInput02;
        private UiAsset.SpeedButton BtnVacuumAnalogInput01;
        private UiAsset.SpeedButton BtnTitleVacuumInput04;
        private UiAsset.SpeedButton BtnTitleVacuumInput03;
        private UiAsset.SpeedButton BtnTitleVacuumInput02;
        private UiAsset.SpeedButton BtnTitleVacuumInput01;
        private UiAsset.SpeedButton BtnTitleVacuumAnalogInput;
        private UiAsset.SpeedButton BtnTitleVacuumInput;
        private System.Windows.Forms.Label LblSensorOffTime;
        private System.Windows.Forms.Label LblBlowSettingTime;
        private System.Windows.Forms.Label LblSensorOnTime;
        private System.Windows.Forms.Label LblVaccumSettingTime;
    }
}