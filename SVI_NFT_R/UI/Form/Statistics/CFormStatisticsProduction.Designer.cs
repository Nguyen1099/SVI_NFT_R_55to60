namespace SVI_NFT_R
{
    partial class CFormStatisticsProduction
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.BtnTitleProductionList = new UiAsset.SpeedButton();
            this.GridViewWeeklyGroup = new System.Windows.Forms.DataGridView();
            this.GridViewNightGroup = new System.Windows.Forms.DataGridView();
            this.BtnTotalInputWeekly = new UiAsset.SpeedButton();
            this.BtnTitleTotalInputWeekly = new UiAsset.SpeedButton();
            this.BtnTitleTotalOutputWeekly = new UiAsset.SpeedButton();
            this.BtnTotalOutputWeekly = new UiAsset.SpeedButton();
            this.BtnTotalInputNight = new UiAsset.SpeedButton();
            this.BtnTitleTotalInputNight = new UiAsset.SpeedButton();
            this.BtnTitleTotalOutputNight = new UiAsset.SpeedButton();
            this.BtnTotalOutputNight = new UiAsset.SpeedButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.DateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.backgroundWorkerUpdate = new System.ComponentModel.BackgroundWorker();
            this.BtnPrevious = new UiAsset.ImageButton();
            this.BtnNext = new UiAsset.ImageButton();
            this.BtnToday = new UiAsset.ImageButton();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewWeeklyGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewNightGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnTitleProductionList
            // 
            this.BtnTitleProductionList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleProductionList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleProductionList.Location = new System.Drawing.Point(12, 12);
            this.BtnTitleProductionList.Name = "BtnTitleProductionList";
            this.BtnTitleProductionList.Size = new System.Drawing.Size(1252, 46);
            this.BtnTitleProductionList.TabIndex = 2;
            this.BtnTitleProductionList.TabStop = false;
            this.BtnTitleProductionList.Text = "PRODUCTION LIST";
            this.BtnTitleProductionList.UseVisualStyleBackColor = true;
            // 
            // GridViewWeeklyGroup
            // 
            this.GridViewWeeklyGroup.AllowUserToAddRows = false;
            this.GridViewWeeklyGroup.AllowUserToDeleteRows = false;
            this.GridViewWeeklyGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewWeeklyGroup.Enabled = false;
            this.GridViewWeeklyGroup.Location = new System.Drawing.Point(12, 116);
            this.GridViewWeeklyGroup.Name = "GridViewWeeklyGroup";
            this.GridViewWeeklyGroup.ReadOnly = true;
            this.GridViewWeeklyGroup.RowTemplate.Height = 23;
            this.GridViewWeeklyGroup.Size = new System.Drawing.Size(623, 513);
            this.GridViewWeeklyGroup.TabIndex = 3;
            // 
            // GridViewNightGroup
            // 
            this.GridViewNightGroup.AllowUserToAddRows = false;
            this.GridViewNightGroup.AllowUserToDeleteRows = false;
            this.GridViewNightGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewNightGroup.Enabled = false;
            this.GridViewNightGroup.Location = new System.Drawing.Point(641, 116);
            this.GridViewNightGroup.Name = "GridViewNightGroup";
            this.GridViewNightGroup.ReadOnly = true;
            this.GridViewNightGroup.RowTemplate.Height = 23;
            this.GridViewNightGroup.Size = new System.Drawing.Size(623, 513);
            this.GridViewNightGroup.TabIndex = 3;
            // 
            // BtnTotalInputWeekly
            // 
            this.BtnTotalInputWeekly.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTotalInputWeekly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTotalInputWeekly.Location = new System.Drawing.Point(12, 687);
            this.BtnTotalInputWeekly.Name = "BtnTotalInputWeekly";
            this.BtnTotalInputWeekly.Size = new System.Drawing.Size(308, 46);
            this.BtnTotalInputWeekly.TabIndex = 2;
            this.BtnTotalInputWeekly.TabStop = false;
            this.BtnTotalInputWeekly.Text = "0";
            this.BtnTotalInputWeekly.UseVisualStyleBackColor = true;
            // 
            // BtnTitleTotalInputWeekly
            // 
            this.BtnTitleTotalInputWeekly.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleTotalInputWeekly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleTotalInputWeekly.Location = new System.Drawing.Point(12, 635);
            this.BtnTitleTotalInputWeekly.Name = "BtnTitleTotalInputWeekly";
            this.BtnTitleTotalInputWeekly.Size = new System.Drawing.Size(308, 46);
            this.BtnTitleTotalInputWeekly.TabIndex = 2;
            this.BtnTitleTotalInputWeekly.TabStop = false;
            this.BtnTitleTotalInputWeekly.Text = "TOTAL INPUT";
            this.BtnTitleTotalInputWeekly.UseVisualStyleBackColor = true;
            // 
            // BtnTitleTotalOutputWeekly
            // 
            this.BtnTitleTotalOutputWeekly.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleTotalOutputWeekly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleTotalOutputWeekly.Location = new System.Drawing.Point(327, 635);
            this.BtnTitleTotalOutputWeekly.Name = "BtnTitleTotalOutputWeekly";
            this.BtnTitleTotalOutputWeekly.Size = new System.Drawing.Size(308, 46);
            this.BtnTitleTotalOutputWeekly.TabIndex = 2;
            this.BtnTitleTotalOutputWeekly.TabStop = false;
            this.BtnTitleTotalOutputWeekly.Text = "TOTAL OUTPUT";
            this.BtnTitleTotalOutputWeekly.UseVisualStyleBackColor = true;
            // 
            // BtnTotalOutputWeekly
            // 
            this.BtnTotalOutputWeekly.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTotalOutputWeekly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTotalOutputWeekly.Location = new System.Drawing.Point(327, 687);
            this.BtnTotalOutputWeekly.Name = "BtnTotalOutputWeekly";
            this.BtnTotalOutputWeekly.Size = new System.Drawing.Size(308, 46);
            this.BtnTotalOutputWeekly.TabIndex = 2;
            this.BtnTotalOutputWeekly.TabStop = false;
            this.BtnTotalOutputWeekly.Text = "0";
            this.BtnTotalOutputWeekly.UseVisualStyleBackColor = true;
            // 
            // BtnTotalInputNight
            // 
            this.BtnTotalInputNight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTotalInputNight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTotalInputNight.Location = new System.Drawing.Point(641, 687);
            this.BtnTotalInputNight.Name = "BtnTotalInputNight";
            this.BtnTotalInputNight.Size = new System.Drawing.Size(308, 46);
            this.BtnTotalInputNight.TabIndex = 2;
            this.BtnTotalInputNight.TabStop = false;
            this.BtnTotalInputNight.Text = "0";
            this.BtnTotalInputNight.UseVisualStyleBackColor = true;
            // 
            // BtnTitleTotalInputNight
            // 
            this.BtnTitleTotalInputNight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleTotalInputNight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleTotalInputNight.Location = new System.Drawing.Point(641, 635);
            this.BtnTitleTotalInputNight.Name = "BtnTitleTotalInputNight";
            this.BtnTitleTotalInputNight.Size = new System.Drawing.Size(308, 46);
            this.BtnTitleTotalInputNight.TabIndex = 2;
            this.BtnTitleTotalInputNight.TabStop = false;
            this.BtnTitleTotalInputNight.Text = "TOTAL INPUT";
            this.BtnTitleTotalInputNight.UseVisualStyleBackColor = true;
            // 
            // BtnTitleTotalOutputNight
            // 
            this.BtnTitleTotalOutputNight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleTotalOutputNight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleTotalOutputNight.Location = new System.Drawing.Point(956, 635);
            this.BtnTitleTotalOutputNight.Name = "BtnTitleTotalOutputNight";
            this.BtnTitleTotalOutputNight.Size = new System.Drawing.Size(308, 46);
            this.BtnTitleTotalOutputNight.TabIndex = 2;
            this.BtnTitleTotalOutputNight.TabStop = false;
            this.BtnTitleTotalOutputNight.Text = "TOTAL OUTPUT";
            this.BtnTitleTotalOutputNight.UseVisualStyleBackColor = true;
            // 
            // BtnTotalOutputNight
            // 
            this.BtnTotalOutputNight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTotalOutputNight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTotalOutputNight.Location = new System.Drawing.Point(956, 687);
            this.BtnTotalOutputNight.Name = "BtnTotalOutputNight";
            this.BtnTotalOutputNight.Size = new System.Drawing.Size(308, 46);
            this.BtnTotalOutputNight.TabIndex = 2;
            this.BtnTotalOutputNight.TabStop = false;
            this.BtnTotalOutputNight.Text = "0";
            this.BtnTotalOutputNight.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // DateTimeFrom
            // 
            this.DateTimeFrom.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeFrom.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeFrom.Location = new System.Drawing.Point(641, 64);
            this.DateTimeFrom.Name = "DateTimeFrom";
            this.DateTimeFrom.Size = new System.Drawing.Size(623, 46);
            this.DateTimeFrom.TabIndex = 17;
            this.DateTimeFrom.TabStop = false;
            this.DateTimeFrom.ValueChanged += new System.EventHandler(this.DateTimeFrom_ValueChanged);
            // 
            // backgroundWorkerUpdate
            // 
            this.backgroundWorkerUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerUpdate_DoWork);
            // 
            // BtnPrevious
            // 
            this.BtnPrevious.BackColor = System.Drawing.Color.Transparent;
            this.BtnPrevious.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPrevious.ButtonText = "PREVIOUS";
            this.BtnPrevious.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPrevious.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPrevious.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPrevious.Location = new System.Drawing.Point(12, 64);
            this.BtnPrevious.Name = "BtnPrevious";
            this.BtnPrevious.Size = new System.Drawing.Size(203, 46);
            this.BtnPrevious.TabIndex = 20;
            this.BtnPrevious.Text = "PREVIOUS";
            this.BtnPrevious.UseVisualStyleBackColor = false;
            this.BtnPrevious.Click += new System.EventHandler(this.BtnPrevious_Click);
            // 
            // BtnNext
            // 
            this.BtnNext.BackColor = System.Drawing.Color.Transparent;
            this.BtnNext.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnNext.ButtonText = "NEXT";
            this.BtnNext.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnNext.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnNext.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnNext.Location = new System.Drawing.Point(430, 64);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(203, 46);
            this.BtnNext.TabIndex = 21;
            this.BtnNext.Text = "NEXT";
            this.BtnNext.UseVisualStyleBackColor = false;
            this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // BtnToday
            // 
            this.BtnToday.BackColor = System.Drawing.Color.Transparent;
            this.BtnToday.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnToday.ButtonText = "TODAY";
            this.BtnToday.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnToday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnToday.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnToday.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnToday.Location = new System.Drawing.Point(221, 64);
            this.BtnToday.Name = "BtnToday";
            this.BtnToday.Size = new System.Drawing.Size(203, 46);
            this.BtnToday.TabIndex = 22;
            this.BtnToday.Text = "TODAY";
            this.BtnToday.UseVisualStyleBackColor = false;
            this.BtnToday.Click += new System.EventHandler(this.BtnToday_Click);
            // 
            // timerUpdate
            // 
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // CFormStatisticsProduction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.Controls.Add(this.BtnNext);
            this.Controls.Add(this.BtnToday);
            this.Controls.Add(this.BtnPrevious);
            this.Controls.Add(this.DateTimeFrom);
            this.Controls.Add(this.GridViewNightGroup);
            this.Controls.Add(this.GridViewWeeklyGroup);
            this.Controls.Add(this.BtnTotalOutputNight);
            this.Controls.Add(this.BtnTotalOutputWeekly);
            this.Controls.Add(this.BtnTitleTotalOutputNight);
            this.Controls.Add(this.BtnTitleTotalInputNight);
            this.Controls.Add(this.BtnTitleTotalOutputWeekly);
            this.Controls.Add(this.BtnTotalInputNight);
            this.Controls.Add(this.BtnTitleTotalInputWeekly);
            this.Controls.Add(this.BtnTotalInputWeekly);
            this.Controls.Add(this.BtnTitleProductionList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormStatisticsProduction";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormStatisticsProduction";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormStatisticsProduction_FormClosed);
            this.Load += new System.EventHandler(this.CFormStatisticsProduction_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewWeeklyGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewNightGroup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UiAsset.SpeedButton BtnTitleProductionList;
        private System.Windows.Forms.DataGridView GridViewWeeklyGroup;
        private System.Windows.Forms.DataGridView GridViewNightGroup;
        private UiAsset.SpeedButton BtnTotalInputWeekly;
        private UiAsset.SpeedButton BtnTitleTotalInputWeekly;
        private UiAsset.SpeedButton BtnTitleTotalOutputWeekly;
        private UiAsset.SpeedButton BtnTotalOutputWeekly;
        private UiAsset.SpeedButton BtnTotalInputNight;
        private UiAsset.SpeedButton BtnTitleTotalInputNight;
        private UiAsset.SpeedButton BtnTitleTotalOutputNight;
        private UiAsset.SpeedButton BtnTotalOutputNight;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.DateTimePicker DateTimeFrom;
        private System.ComponentModel.BackgroundWorker backgroundWorkerUpdate;
        private UiAsset.ImageButton BtnPrevious;
        private UiAsset.ImageButton BtnNext;
        private UiAsset.ImageButton BtnToday;
        private System.Windows.Forms.Timer timerUpdate;
    }
}