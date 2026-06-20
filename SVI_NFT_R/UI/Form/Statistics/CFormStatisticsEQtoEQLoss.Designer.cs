namespace SVI_NFT_R
{
    partial class CFormStatisticsEQtoEQLoss
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
            this.BtnSaveToCsv = new UiAsset.ImageButton();
            this.BtnSelectAsc = new UiAsset.ImageButton();
            this.DateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.DateTimeTo = new System.Windows.Forms.DateTimePicker();
            this.GridViewEQtoEQLossList = new System.Windows.Forms.DataGridView();
            this.BtnTitleEQtoEQLossList = new UiAsset.SpeedButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.BtnSelectDesc = new UiAsset.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewEQtoEQLossList)).BeginInit();
            this.SuspendLayout();
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
            this.BtnSaveToCsv.TabIndex = 27;
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
            this.BtnSelectAsc.TabIndex = 28;
            this.BtnSelectAsc.Click += new System.EventHandler(this.BtnSelectAsc_Click);
            // 
            // DateTimeFrom
            // 
            this.DateTimeFrom.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeFrom.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeFrom.Location = new System.Drawing.Point(12, 64);
            this.DateTimeFrom.Name = "DateTimeFrom";
            this.DateTimeFrom.Size = new System.Drawing.Size(246, 46);
            this.DateTimeFrom.TabIndex = 25;
            this.DateTimeFrom.ValueChanged += new System.EventHandler(this.DateTimeFrom_ValueChanged);
            // 
            // DateTimeTo
            // 
            this.DateTimeTo.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeTo.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeTo.Location = new System.Drawing.Point(264, 64);
            this.DateTimeTo.Name = "DateTimeTo";
            this.DateTimeTo.Size = new System.Drawing.Size(246, 46);
            this.DateTimeTo.TabIndex = 26;
            this.DateTimeTo.ValueChanged += new System.EventHandler(this.DateTimeTo_ValueChanged);
            // 
            // GridViewEQtoEQLossList
            // 
            this.GridViewEQtoEQLossList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewEQtoEQLossList.Location = new System.Drawing.Point(12, 116);
            this.GridViewEQtoEQLossList.Name = "GridViewEQtoEQLossList";
            this.GridViewEQtoEQLossList.RowTemplate.Height = 23;
            this.GridViewEQtoEQLossList.Size = new System.Drawing.Size(1252, 617);
            this.GridViewEQtoEQLossList.TabIndex = 24;
            this.GridViewEQtoEQLossList.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.GridViewEQtoEQLossList_CellValueNeeded);
            // 
            // BtnTitleEQtoEQLossList
            // 
            this.BtnTitleEQtoEQLossList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleEQtoEQLossList.Location = new System.Drawing.Point(12, 12);
            this.BtnTitleEQtoEQLossList.Name = "BtnTitleEQtoEQLossList";
            this.BtnTitleEQtoEQLossList.Size = new System.Drawing.Size(1252, 46);
            this.BtnTitleEQtoEQLossList.TabIndex = 23;
            this.BtnTitleEQtoEQLossList.TabStop = false;
            this.BtnTitleEQtoEQLossList.Text = "EQ TO EQ LOSS LIST";
            this.BtnTitleEQtoEQLossList.UseVisualStyleBackColor = true;
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
            this.BtnSelectDesc.TabIndex = 28;
            this.BtnSelectDesc.Click += new System.EventHandler(this.BtnSelectDesc_Click);
            // 
            // CFormStatisticsEQtoEQLoss
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
            this.Controls.Add(this.GridViewEQtoEQLossList);
            this.Controls.Add(this.BtnTitleEQtoEQLossList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormStatisticsEQtoEQLoss";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormStatisticsEQtoEQLoss";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormStatisticsEQtoEQLoss_FormClosed);
            this.Load += new System.EventHandler(this.CFormStatisticsEQtoEQLoss_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewEQtoEQLossList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private UiAsset.ImageButton BtnSaveToCsv;
        private UiAsset.ImageButton BtnSelectAsc;
        private System.Windows.Forms.DateTimePicker DateTimeFrom;
        private System.Windows.Forms.DateTimePicker DateTimeTo;
        private System.Windows.Forms.DataGridView GridViewEQtoEQLossList;
        private UiAsset.SpeedButton BtnTitleEQtoEQLossList;
        private System.Windows.Forms.Timer timer1;
        private UiAsset.ImageButton BtnSelectDesc;
    }
}