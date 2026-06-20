namespace SVI_NFT_R
{
    partial class CFormStatisticsDBMessage
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
            this.GridViewList = new System.Windows.Forms.DataGridView();
            this.panelFormMenu = new System.Windows.Forms.Panel();
            this.BtnBase = new UiAsset.ImageButton();
            this.DateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.DateTimeTo = new System.Windows.Forms.DateTimePicker();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnSaveToCsv = new UiAsset.ImageButton();
            this.BtnSelectAsc = new UiAsset.ImageButton();
            this.BtnSelectDesc = new UiAsset.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewList)).BeginInit();
            this.panelFormMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // GridViewList
            // 
            this.GridViewList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewList.Location = new System.Drawing.Point(12, 116);
            this.GridViewList.Name = "GridViewList";
            this.GridViewList.RowTemplate.Height = 23;
            this.GridViewList.Size = new System.Drawing.Size(1252, 617);
            this.GridViewList.TabIndex = 22;
            this.GridViewList.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.GridViewList_CellValueNeeded);
            // 
            // panelFormMenu
            // 
            this.panelFormMenu.Controls.Add(this.BtnBase);
            this.panelFormMenu.Location = new System.Drawing.Point(12, 12);
            this.panelFormMenu.Name = "panelFormMenu";
            this.panelFormMenu.Size = new System.Drawing.Size(1252, 46);
            this.panelFormMenu.TabIndex = 18;
            // 
            // BtnBase
            // 
            this.BtnBase.BackColor = System.Drawing.Color.Transparent;
            this.BtnBase.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBase.ButtonText = "BASE";
            this.BtnBase.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnBase.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBase.Location = new System.Drawing.Point(0, 0);
            this.BtnBase.Name = "BtnBase";
            this.BtnBase.Size = new System.Drawing.Size(139, 46);
            this.BtnBase.TabIndex = 31;
            this.BtnBase.Visible = false;
            // 
            // DateTimeFrom
            // 
            this.DateTimeFrom.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeFrom.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeFrom.Location = new System.Drawing.Point(12, 64);
            this.DateTimeFrom.Name = "DateTimeFrom";
            this.DateTimeFrom.Size = new System.Drawing.Size(246, 46);
            this.DateTimeFrom.TabIndex = 30;
            this.DateTimeFrom.ValueChanged += new System.EventHandler(this.DateTimeFrom_ValueChanged);
            // 
            // DateTimeTo
            // 
            this.DateTimeTo.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeTo.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeTo.Location = new System.Drawing.Point(264, 64);
            this.DateTimeTo.Name = "DateTimeTo";
            this.DateTimeTo.Size = new System.Drawing.Size(246, 46);
            this.DateTimeTo.TabIndex = 29;
            this.DateTimeTo.ValueChanged += new System.EventHandler(this.DateTimeTo_ValueChanged);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnSaveToCsv
            // 
            this.BtnSaveToCsv.BackColor = System.Drawing.Color.Transparent;
            this.BtnSaveToCsv.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSaveToCsv.ButtonText = "SAVE TO CSV";
            this.BtnSaveToCsv.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSaveToCsv.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSaveToCsv.Location = new System.Drawing.Point(517, 64);
            this.BtnSaveToCsv.Name = "BtnSaveToCsv";
            this.BtnSaveToCsv.Size = new System.Drawing.Size(245, 46);
            this.BtnSaveToCsv.TabIndex = 32;
            this.BtnSaveToCsv.Click += new System.EventHandler(this.BtnSaveToCsv_Click);
            // 
            // BtnSelectAsc
            // 
            this.BtnSelectAsc.BackColor = System.Drawing.Color.Transparent;
            this.BtnSelectAsc.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSelectAsc.ButtonText = "SELECT (ASC)";
            this.BtnSelectAsc.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnSelectAsc.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSelectAsc.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSelectAsc.Location = new System.Drawing.Point(768, 64);
            this.BtnSelectAsc.Name = "BtnSelectAsc";
            this.BtnSelectAsc.Size = new System.Drawing.Size(245, 46);
            this.BtnSelectAsc.TabIndex = 33;
            this.BtnSelectAsc.Click += new System.EventHandler(this.BtnSelectAsc_Click);
            // 
            // BtnSelectDesc
            // 
            this.BtnSelectDesc.BackColor = System.Drawing.Color.Transparent;
            this.BtnSelectDesc.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSelectDesc.ButtonText = "SELECT (DESC)";
            this.BtnSelectDesc.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnSelectDesc.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSelectDesc.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSelectDesc.Location = new System.Drawing.Point(1019, 64);
            this.BtnSelectDesc.Name = "BtnSelectDesc";
            this.BtnSelectDesc.Size = new System.Drawing.Size(245, 46);
            this.BtnSelectDesc.TabIndex = 33;
            this.BtnSelectDesc.Click += new System.EventHandler(this.BtnSelectDesc_Click);
            // 
            // CFormStatisticsDBMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.ControlBox = false;
            this.Controls.Add(this.BtnSaveToCsv);
            this.Controls.Add(this.BtnSelectDesc);
            this.Controls.Add(this.BtnSelectAsc);
            this.Controls.Add(this.DateTimeFrom);
            this.Controls.Add(this.DateTimeTo);
            this.Controls.Add(this.GridViewList);
            this.Controls.Add(this.panelFormMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormStatisticsDBMessage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormStatisticsDBMessage";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormStatisticsDBMessage_FormClosed);
            this.Load += new System.EventHandler(this.CFormStatisticsDBMessage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewList)).EndInit();
            this.panelFormMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView GridViewList;
		private System.Windows.Forms.Panel panelFormMenu;
        private System.Windows.Forms.DateTimePicker DateTimeFrom;
		private System.Windows.Forms.DateTimePicker DateTimeTo;
        private System.Windows.Forms.Timer timer;
		private UiAsset.ImageButton BtnBase;
		private UiAsset.ImageButton BtnSaveToCsv;
		private UiAsset.ImageButton BtnSelectAsc;
        private UiAsset.ImageButton BtnSelectDesc;
    }
}