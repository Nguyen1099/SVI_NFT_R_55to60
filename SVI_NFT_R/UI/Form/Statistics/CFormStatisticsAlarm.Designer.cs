namespace SVI_NFT_R
{
    partial class CFormStatisticsAlarm
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
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnSelectAsc = new UiAsset.ImageButton();
            this.DateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.DateTimeTo = new System.Windows.Forms.DateTimePicker();
            this.GridViewAlarmList = new System.Windows.Forms.DataGridView();
            this.BtnSaveToCsv = new UiAsset.ImageButton();
            this.BtnSelectDesc = new UiAsset.ImageButton();
            this.panelFormMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnOptionHeavyAlarm = new UiAsset.ImageButton();
            this.BtnOptionLightAlarm = new UiAsset.ImageButton();
            this.BtnOptionAllAlarm = new UiAsset.ImageButton();
            this.BtnOptionViewRanking = new UiAsset.ImageButton();
            this.BtnOptionViewGrouping = new UiAsset.ImageButton();
            this.BtnOptionSilenceAlarm = new UiAsset.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewAlarmList)).BeginInit();
            this.panelFormMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnSelectAsc
            // 
            this.BtnSelectAsc.BackColor = System.Drawing.Color.Transparent;
            this.BtnSelectAsc.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSelectAsc.ButtonText = "SELECT (ASC)";
            this.BtnSelectAsc.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnSelectAsc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSelectAsc.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSelectAsc.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSelectAsc.Location = new System.Drawing.Point(768, 64);
            this.BtnSelectAsc.Name = "BtnSelectAsc";
            this.BtnSelectAsc.Size = new System.Drawing.Size(245, 46);
            this.BtnSelectAsc.TabIndex = 11;
            this.BtnSelectAsc.Text = "SELECT (ASC)";
            this.BtnSelectAsc.UseVisualStyleBackColor = false;
            this.BtnSelectAsc.Click += new System.EventHandler(this.BtnSelectAsc_Click);
            // 
            // DateTimeFrom
            // 
            this.DateTimeFrom.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeFrom.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeFrom.Location = new System.Drawing.Point(12, 64);
            this.DateTimeFrom.Name = "DateTimeFrom";
            this.DateTimeFrom.Size = new System.Drawing.Size(246, 46);
            this.DateTimeFrom.TabIndex = 10;
            this.DateTimeFrom.ValueChanged += new System.EventHandler(this.DateTimeFrom_ValueChanged);
            // 
            // DateTimeTo
            // 
            this.DateTimeTo.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeTo.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeTo.Location = new System.Drawing.Point(264, 64);
            this.DateTimeTo.Name = "DateTimeTo";
            this.DateTimeTo.Size = new System.Drawing.Size(246, 46);
            this.DateTimeTo.TabIndex = 10;
            this.DateTimeTo.ValueChanged += new System.EventHandler(this.DateTimeTo_ValueChanged);
            // 
            // GridViewAlarmList
            // 
            this.GridViewAlarmList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewAlarmList.Location = new System.Drawing.Point(12, 116);
            this.GridViewAlarmList.Name = "GridViewAlarmList";
            this.GridViewAlarmList.RowTemplate.Height = 23;
            this.GridViewAlarmList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.GridViewAlarmList.Size = new System.Drawing.Size(1252, 617);
            this.GridViewAlarmList.TabIndex = 4;
            this.GridViewAlarmList.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridViewAlarmList_CellMouseUp);
            this.GridViewAlarmList.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.GridViewAlarmList_CellValueNeeded);
            // 
            // BtnSaveToCsv
            // 
            this.BtnSaveToCsv.BackColor = System.Drawing.Color.Transparent;
            this.BtnSaveToCsv.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSaveToCsv.ButtonText = "SAVE TO CSV";
            this.BtnSaveToCsv.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSaveToCsv.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSaveToCsv.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSaveToCsv.Location = new System.Drawing.Point(517, 64);
            this.BtnSaveToCsv.Name = "BtnSaveToCsv";
            this.BtnSaveToCsv.Size = new System.Drawing.Size(245, 46);
            this.BtnSaveToCsv.TabIndex = 11;
            this.BtnSaveToCsv.Text = "SAVE TO CSV";
            this.BtnSaveToCsv.UseVisualStyleBackColor = false;
            this.BtnSaveToCsv.Click += new System.EventHandler(this.BtnSaveToCsv_Click);
            // 
            // BtnSelectDesc
            // 
            this.BtnSelectDesc.BackColor = System.Drawing.Color.Transparent;
            this.BtnSelectDesc.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSelectDesc.ButtonText = "SELECT (DESC)";
            this.BtnSelectDesc.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnSelectDesc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSelectDesc.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSelectDesc.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSelectDesc.Location = new System.Drawing.Point(1019, 64);
            this.BtnSelectDesc.Name = "BtnSelectDesc";
            this.BtnSelectDesc.Size = new System.Drawing.Size(245, 46);
            this.BtnSelectDesc.TabIndex = 11;
            this.BtnSelectDesc.Text = "SELECT (DESC)";
            this.BtnSelectDesc.UseVisualStyleBackColor = false;
            this.BtnSelectDesc.Click += new System.EventHandler(this.BtnSelectDesc_Click);
            // 
            // panelFormMenu
            // 
            this.panelFormMenu.Controls.Add(this.BtnOptionHeavyAlarm);
            this.panelFormMenu.Controls.Add(this.BtnOptionLightAlarm);
            this.panelFormMenu.Controls.Add(this.BtnOptionSilenceAlarm);
            this.panelFormMenu.Controls.Add(this.BtnOptionAllAlarm);
            this.panelFormMenu.Location = new System.Drawing.Point(12, 12);
            this.panelFormMenu.Name = "panelFormMenu";
            this.panelFormMenu.Size = new System.Drawing.Size(664, 46);
            this.panelFormMenu.TabIndex = 14;
            // 
            // BtnOptionHeavyAlarm
            // 
            this.BtnOptionHeavyAlarm.BackColor = System.Drawing.Color.Transparent;
            this.BtnOptionHeavyAlarm.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOptionHeavyAlarm.ButtonText = "HEAVY ALARM";
            this.BtnOptionHeavyAlarm.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnOptionHeavyAlarm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOptionHeavyAlarm.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnOptionHeavyAlarm.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOptionHeavyAlarm.Location = new System.Drawing.Point(0, 0);
            this.BtnOptionHeavyAlarm.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.BtnOptionHeavyAlarm.Name = "BtnOptionHeavyAlarm";
            this.BtnOptionHeavyAlarm.Size = new System.Drawing.Size(164, 46);
            this.BtnOptionHeavyAlarm.TabIndex = 6;
            this.BtnOptionHeavyAlarm.Text = "HEAVY ALARM";
            this.BtnOptionHeavyAlarm.UseVisualStyleBackColor = false;
            this.BtnOptionHeavyAlarm.Click += new System.EventHandler(this.BtnOptionHeavyAlarm_Click);
            // 
            // BtnOptionLightAlarm
            // 
            this.BtnOptionLightAlarm.BackColor = System.Drawing.Color.Transparent;
            this.BtnOptionLightAlarm.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOptionLightAlarm.ButtonText = "LIGHT ALARM";
            this.BtnOptionLightAlarm.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnOptionLightAlarm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOptionLightAlarm.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnOptionLightAlarm.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOptionLightAlarm.Location = new System.Drawing.Point(166, 0);
            this.BtnOptionLightAlarm.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.BtnOptionLightAlarm.Name = "BtnOptionLightAlarm";
            this.BtnOptionLightAlarm.Size = new System.Drawing.Size(164, 46);
            this.BtnOptionLightAlarm.TabIndex = 7;
            this.BtnOptionLightAlarm.Text = "LIGHT ALARM";
            this.BtnOptionLightAlarm.UseVisualStyleBackColor = false;
            this.BtnOptionLightAlarm.Click += new System.EventHandler(this.BtnOptionLightAlarm_Click);
            // 
            // BtnOptionAllAlarm
            // 
            this.BtnOptionAllAlarm.BackColor = System.Drawing.Color.Transparent;
            this.BtnOptionAllAlarm.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOptionAllAlarm.ButtonText = "ALL ALARM";
            this.BtnOptionAllAlarm.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnOptionAllAlarm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOptionAllAlarm.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnOptionAllAlarm.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOptionAllAlarm.Location = new System.Drawing.Point(498, 0);
            this.BtnOptionAllAlarm.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.BtnOptionAllAlarm.Name = "BtnOptionAllAlarm";
            this.BtnOptionAllAlarm.Size = new System.Drawing.Size(164, 46);
            this.BtnOptionAllAlarm.TabIndex = 8;
            this.BtnOptionAllAlarm.Text = "ALL ALARM";
            this.BtnOptionAllAlarm.UseVisualStyleBackColor = false;
            this.BtnOptionAllAlarm.Click += new System.EventHandler(this.BtnOptionAllAlarm_Click);
            // 
            // BtnOptionViewRanking
            // 
            this.BtnOptionViewRanking.BackColor = System.Drawing.Color.Transparent;
            this.BtnOptionViewRanking.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOptionViewRanking.ButtonText = "VIEW ALARM RANKING";
            this.BtnOptionViewRanking.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnOptionViewRanking.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOptionViewRanking.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnOptionViewRanking.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOptionViewRanking.Location = new System.Drawing.Point(768, 12);
            this.BtnOptionViewRanking.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.BtnOptionViewRanking.Name = "BtnOptionViewRanking";
            this.BtnOptionViewRanking.Size = new System.Drawing.Size(245, 46);
            this.BtnOptionViewRanking.TabIndex = 15;
            this.BtnOptionViewRanking.Text = "VIEW ALARM RANKING";
            this.BtnOptionViewRanking.UseVisualStyleBackColor = false;
            this.BtnOptionViewRanking.Click += new System.EventHandler(this.BtnAlarmRanking_Click);
            // 
            // BtnOptionViewGrouping
            // 
            this.BtnOptionViewGrouping.BackColor = System.Drawing.Color.Transparent;
            this.BtnOptionViewGrouping.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOptionViewGrouping.ButtonText = "VIEW ALARM GROUPING";
            this.BtnOptionViewGrouping.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnOptionViewGrouping.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOptionViewGrouping.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnOptionViewGrouping.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOptionViewGrouping.Location = new System.Drawing.Point(1019, 12);
            this.BtnOptionViewGrouping.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.BtnOptionViewGrouping.Name = "BtnOptionViewGrouping";
            this.BtnOptionViewGrouping.Size = new System.Drawing.Size(245, 46);
            this.BtnOptionViewGrouping.TabIndex = 16;
            this.BtnOptionViewGrouping.Text = "VIEW ALARM GROUPING";
            this.BtnOptionViewGrouping.UseVisualStyleBackColor = false;
            this.BtnOptionViewGrouping.Click += new System.EventHandler(this.BtnViewAlarmGrouping_Click);
            // 
            // BtnOptionSilenceAlarm
            // 
            this.BtnOptionSilenceAlarm.BackColor = System.Drawing.Color.Transparent;
            this.BtnOptionSilenceAlarm.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOptionSilenceAlarm.ButtonText = "SILENCE ALARM";
            this.BtnOptionSilenceAlarm.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnOptionSilenceAlarm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOptionSilenceAlarm.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnOptionSilenceAlarm.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOptionSilenceAlarm.Location = new System.Drawing.Point(332, 0);
            this.BtnOptionSilenceAlarm.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.BtnOptionSilenceAlarm.Name = "BtnOptionSilenceAlarm";
            this.BtnOptionSilenceAlarm.Size = new System.Drawing.Size(164, 46);
            this.BtnOptionSilenceAlarm.TabIndex = 9;
            this.BtnOptionSilenceAlarm.Text = "SILENCE ALARM";
            this.BtnOptionSilenceAlarm.UseVisualStyleBackColor = false;
            this.BtnOptionSilenceAlarm.Click += new System.EventHandler(this.BtnOptionSilenceAlarm_Click);
            // 
            // CFormStatisticsAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.Controls.Add(this.BtnOptionViewGrouping);
            this.Controls.Add(this.BtnOptionViewRanking);
            this.Controls.Add(this.panelFormMenu);
            this.Controls.Add(this.BtnSaveToCsv);
            this.Controls.Add(this.BtnSelectDesc);
            this.Controls.Add(this.BtnSelectAsc);
            this.Controls.Add(this.DateTimeFrom);
            this.Controls.Add(this.DateTimeTo);
            this.Controls.Add(this.GridViewAlarmList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormStatisticsAlarm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormStatisticsAlarm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormStatisticsAlarm_FormClosed);
            this.Load += new System.EventHandler(this.CFormStatisticsAlarm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewAlarmList)).EndInit();
            this.panelFormMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
		private System.Windows.Forms.DataGridView GridViewAlarmList;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.DateTimePicker DateTimeTo;
        private System.Windows.Forms.DateTimePicker DateTimeFrom;
		private UiAsset.ImageButton BtnSelectAsc;
        private UiAsset.ImageButton BtnSaveToCsv;
        private UiAsset.ImageButton BtnSelectDesc;
        private System.Windows.Forms.FlowLayoutPanel panelFormMenu;
        private UiAsset.ImageButton BtnOptionHeavyAlarm;
        private UiAsset.ImageButton BtnOptionLightAlarm;
        private UiAsset.ImageButton BtnOptionAllAlarm;
        private UiAsset.ImageButton BtnOptionViewRanking;
        private UiAsset.ImageButton BtnOptionViewGrouping;
        private UiAsset.ImageButton BtnOptionSilenceAlarm;
    }
}