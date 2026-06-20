namespace SVI_NFT_R
{
    partial class CFormStatisticsEQPLoss
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
            this.BtnSaveToCsv = new UiAsset.ImageButton();
            this.BtnSelectAsc = new UiAsset.ImageButton();
            this.DateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.DateTimeTo = new System.Windows.Forms.DateTimePicker();
            this.GridViewEQPLossList = new System.Windows.Forms.DataGridView();
            this.BtnTitleEQPLossList = new UiAsset.SpeedButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnSelectDesc = new UiAsset.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewEQPLossList)).BeginInit();
            this.SuspendLayout();
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
            this.BtnSaveToCsv.TabIndex = 21;
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
            this.BtnSelectAsc.TabIndex = 22;
            this.BtnSelectAsc.Click += new System.EventHandler(this.BtnSelectAsc_Click);
            // 
            // DateTimeFrom
            // 
            this.DateTimeFrom.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeFrom.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeFrom.Location = new System.Drawing.Point(12, 64);
            this.DateTimeFrom.Name = "DateTimeFrom";
            this.DateTimeFrom.Size = new System.Drawing.Size(246, 46);
            this.DateTimeFrom.TabIndex = 19;
            this.DateTimeFrom.ValueChanged += new System.EventHandler(this.DateTimeFrom_ValueChanged);
            // 
            // DateTimeTo
            // 
            this.DateTimeTo.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeTo.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeTo.Location = new System.Drawing.Point(264, 64);
            this.DateTimeTo.Name = "DateTimeTo";
            this.DateTimeTo.Size = new System.Drawing.Size(246, 46);
            this.DateTimeTo.TabIndex = 20;
            this.DateTimeTo.ValueChanged += new System.EventHandler(this.DateTimeTo_ValueChanged);
            // 
            // GridViewEQPLossList
            // 
            this.GridViewEQPLossList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewEQPLossList.Location = new System.Drawing.Point(12, 116);
            this.GridViewEQPLossList.Name = "GridViewEQPLossList";
            this.GridViewEQPLossList.RowTemplate.Height = 23;
            this.GridViewEQPLossList.Size = new System.Drawing.Size(1252, 617);
            this.GridViewEQPLossList.TabIndex = 18;
            this.GridViewEQPLossList.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.GridViewEQPLossList_CellValueNeeded);
            // 
            // BtnTitleEQPLossList
            // 
            this.BtnTitleEQPLossList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleEQPLossList.Location = new System.Drawing.Point(12, 12);
            this.BtnTitleEQPLossList.Name = "BtnTitleEQPLossList";
            this.BtnTitleEQPLossList.Size = new System.Drawing.Size(1252, 46);
            this.BtnTitleEQPLossList.TabIndex = 17;
            this.BtnTitleEQPLossList.TabStop = false;
            this.BtnTitleEQPLossList.Text = "EQP LOSS LIST";
            this.BtnTitleEQPLossList.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
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
            this.BtnSelectDesc.TabIndex = 22;
            this.BtnSelectDesc.Click += new System.EventHandler(this.BtnSelectDesc_Click);
            // 
            // CFormStatisticsEQPLoss
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.Controls.Add(this.BtnSaveToCsv);
            this.Controls.Add(this.BtnSelectDesc);
            this.Controls.Add(this.BtnSelectAsc);
            this.Controls.Add(this.DateTimeFrom);
            this.Controls.Add(this.DateTimeTo);
            this.Controls.Add(this.GridViewEQPLossList);
            this.Controls.Add(this.BtnTitleEQPLossList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormStatisticsEQPLoss";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormStatisticsEQPLoss";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormStatisticsEQPLoss_FormClosed);
            this.Load += new System.EventHandler(this.CFormStatisticsEQPLoss_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewEQPLossList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UiAsset.ImageButton BtnSaveToCsv;
        private UiAsset.ImageButton BtnSelectAsc;
        private System.Windows.Forms.DateTimePicker DateTimeFrom;
        private System.Windows.Forms.DateTimePicker DateTimeTo;
        private System.Windows.Forms.DataGridView GridViewEQPLossList;
        private UiAsset.SpeedButton BtnTitleEQPLossList;
        private System.Windows.Forms.Timer timer;
        private UiAsset.ImageButton BtnSelectDesc;
    }
}