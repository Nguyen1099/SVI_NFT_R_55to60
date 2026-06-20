namespace SVI_NFT_R
{
    partial class CFormStatisticsEquipmentOperationMonitoring
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
            this.BtnTitleEquipmentOperationMonitoring = new UiAsset.SpeedButton();
            this.DateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.DateTimeTo = new System.Windows.Forms.DateTimePicker();
            this.BtnTitleProductionStatus = new UiAsset.SpeedButton();
            this.GridViewProductionStatus = new System.Windows.Forms.DataGridView();
            this.BtnTitleOperationStatus = new UiAsset.SpeedButton();
            this.GridViewOperationStatus = new System.Windows.Forms.DataGridView();
            this.BtnTitleNonOperationStatus = new UiAsset.SpeedButton();
            this.GridViewNonOperationStatus = new System.Windows.Forms.DataGridView();
            this.BtnTitleMomentaryStop = new UiAsset.SpeedButton();
            this.GridViewMomentaryStopStatus = new System.Windows.Forms.DataGridView();
            this.BtnTitleTimeToRefair = new UiAsset.SpeedButton();
            this.GridViewTimeToRefair = new System.Windows.Forms.DataGridView();
            this.BtnTitleMCRStatus = new UiAsset.SpeedButton();
            this.GridViewMCRStatus = new System.Windows.Forms.DataGridView();
            this.BtnSelect = new UiAsset.ImageButton();
            this.backgroundWorkerUpdate = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewProductionStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewOperationStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewNonOperationStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewMomentaryStopStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewTimeToRefair)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewMCRStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnTitleEquipmentOperationMonitoring
            // 
            this.BtnTitleEquipmentOperationMonitoring.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleEquipmentOperationMonitoring.Location = new System.Drawing.Point(12, 12);
            this.BtnTitleEquipmentOperationMonitoring.Name = "BtnTitleEquipmentOperationMonitoring";
            this.BtnTitleEquipmentOperationMonitoring.Size = new System.Drawing.Size(1252, 46);
            this.BtnTitleEquipmentOperationMonitoring.TabIndex = 3;
            this.BtnTitleEquipmentOperationMonitoring.TabStop = false;
            this.BtnTitleEquipmentOperationMonitoring.Text = "EQUIPMENT OPERATION MONITORING";
            this.BtnTitleEquipmentOperationMonitoring.UseVisualStyleBackColor = true;
            // 
            // DateTimeFrom
            // 
            this.DateTimeFrom.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeFrom.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeFrom.Location = new System.Drawing.Point(12, 64);
            this.DateTimeFrom.Name = "DateTimeFrom";
            this.DateTimeFrom.Size = new System.Drawing.Size(413, 46);
            this.DateTimeFrom.TabIndex = 13;
            this.DateTimeFrom.ValueChanged += new System.EventHandler(this.DateTimeFrom_ValueChanged);
            // 
            // DateTimeTo
            // 
            this.DateTimeTo.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeTo.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeTo.Location = new System.Drawing.Point(432, 64);
            this.DateTimeTo.Name = "DateTimeTo";
            this.DateTimeTo.Size = new System.Drawing.Size(413, 46);
            this.DateTimeTo.TabIndex = 12;
            this.DateTimeTo.ValueChanged += new System.EventHandler(this.DateTimeTo_ValueChanged);
            // 
            // BtnTitleProductionStatus
            // 
            this.BtnTitleProductionStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleProductionStatus.Location = new System.Drawing.Point(12, 116);
            this.BtnTitleProductionStatus.Name = "BtnTitleProductionStatus";
            this.BtnTitleProductionStatus.Size = new System.Drawing.Size(413, 46);
            this.BtnTitleProductionStatus.TabIndex = 3;
            this.BtnTitleProductionStatus.TabStop = false;
            this.BtnTitleProductionStatus.Text = "PRODUCTION STATUS";
            this.BtnTitleProductionStatus.UseVisualStyleBackColor = true;
            // 
            // GridViewProductionStatus
            // 
            this.GridViewProductionStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewProductionStatus.Location = new System.Drawing.Point(12, 168);
            this.GridViewProductionStatus.Name = "GridViewProductionStatus";
            this.GridViewProductionStatus.RowTemplate.Height = 23;
            this.GridViewProductionStatus.Size = new System.Drawing.Size(413, 254);
            this.GridViewProductionStatus.TabIndex = 14;
            // 
            // BtnTitleOperationStatus
            // 
            this.BtnTitleOperationStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleOperationStatus.Location = new System.Drawing.Point(432, 116);
            this.BtnTitleOperationStatus.Name = "BtnTitleOperationStatus";
            this.BtnTitleOperationStatus.Size = new System.Drawing.Size(413, 46);
            this.BtnTitleOperationStatus.TabIndex = 3;
            this.BtnTitleOperationStatus.TabStop = false;
            this.BtnTitleOperationStatus.Text = "OPERATION STATUS";
            this.BtnTitleOperationStatus.UseVisualStyleBackColor = true;
            // 
            // GridViewOperationStatus
            // 
            this.GridViewOperationStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewOperationStatus.Location = new System.Drawing.Point(432, 168);
            this.GridViewOperationStatus.Name = "GridViewOperationStatus";
            this.GridViewOperationStatus.RowTemplate.Height = 23;
            this.GridViewOperationStatus.Size = new System.Drawing.Size(413, 254);
            this.GridViewOperationStatus.TabIndex = 14;
            // 
            // BtnTitleNonOperationStatus
            // 
            this.BtnTitleNonOperationStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleNonOperationStatus.Location = new System.Drawing.Point(851, 116);
            this.BtnTitleNonOperationStatus.Name = "BtnTitleNonOperationStatus";
            this.BtnTitleNonOperationStatus.Size = new System.Drawing.Size(413, 46);
            this.BtnTitleNonOperationStatus.TabIndex = 3;
            this.BtnTitleNonOperationStatus.TabStop = false;
            this.BtnTitleNonOperationStatus.Text = "NON OPERATION STATUS";
            this.BtnTitleNonOperationStatus.UseVisualStyleBackColor = true;
            // 
            // GridViewNonOperationStatus
            // 
            this.GridViewNonOperationStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewNonOperationStatus.Location = new System.Drawing.Point(851, 168);
            this.GridViewNonOperationStatus.Name = "GridViewNonOperationStatus";
            this.GridViewNonOperationStatus.RowTemplate.Height = 23;
            this.GridViewNonOperationStatus.Size = new System.Drawing.Size(413, 254);
            this.GridViewNonOperationStatus.TabIndex = 14;
            // 
            // BtnTitleMomentaryStop
            // 
            this.BtnTitleMomentaryStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleMomentaryStop.Location = new System.Drawing.Point(12, 428);
            this.BtnTitleMomentaryStop.Name = "BtnTitleMomentaryStop";
            this.BtnTitleMomentaryStop.Size = new System.Drawing.Size(413, 46);
            this.BtnTitleMomentaryStop.TabIndex = 3;
            this.BtnTitleMomentaryStop.TabStop = false;
            this.BtnTitleMomentaryStop.Text = "MOMENTARY STOP STATUS";
            this.BtnTitleMomentaryStop.UseVisualStyleBackColor = true;
            // 
            // GridViewMomentaryStopStatus
            // 
            this.GridViewMomentaryStopStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewMomentaryStopStatus.Location = new System.Drawing.Point(12, 480);
            this.GridViewMomentaryStopStatus.Name = "GridViewMomentaryStopStatus";
            this.GridViewMomentaryStopStatus.RowTemplate.Height = 23;
            this.GridViewMomentaryStopStatus.Size = new System.Drawing.Size(413, 254);
            this.GridViewMomentaryStopStatus.TabIndex = 14;
            // 
            // BtnTitleTimeToRefair
            // 
            this.BtnTitleTimeToRefair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleTimeToRefair.Location = new System.Drawing.Point(432, 428);
            this.BtnTitleTimeToRefair.Name = "BtnTitleTimeToRefair";
            this.BtnTitleTimeToRefair.Size = new System.Drawing.Size(413, 46);
            this.BtnTitleTimeToRefair.TabIndex = 3;
            this.BtnTitleTimeToRefair.TabStop = false;
            this.BtnTitleTimeToRefair.Text = "TIME TO REFAIR";
            this.BtnTitleTimeToRefair.UseVisualStyleBackColor = true;
            // 
            // GridViewTimeToRefair
            // 
            this.GridViewTimeToRefair.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewTimeToRefair.Location = new System.Drawing.Point(432, 480);
            this.GridViewTimeToRefair.Name = "GridViewTimeToRefair";
            this.GridViewTimeToRefair.RowTemplate.Height = 23;
            this.GridViewTimeToRefair.Size = new System.Drawing.Size(413, 254);
            this.GridViewTimeToRefair.TabIndex = 14;
            // 
            // BtnTitleMCRStatus
            // 
            this.BtnTitleMCRStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleMCRStatus.Location = new System.Drawing.Point(851, 428);
            this.BtnTitleMCRStatus.Name = "BtnTitleMCRStatus";
            this.BtnTitleMCRStatus.Size = new System.Drawing.Size(413, 46);
            this.BtnTitleMCRStatus.TabIndex = 3;
            this.BtnTitleMCRStatus.TabStop = false;
            this.BtnTitleMCRStatus.Text = "MCR STATUS";
            this.BtnTitleMCRStatus.UseVisualStyleBackColor = true;
            // 
            // GridViewMCRStatus
            // 
            this.GridViewMCRStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewMCRStatus.Location = new System.Drawing.Point(851, 480);
            this.GridViewMCRStatus.Name = "GridViewMCRStatus";
            this.GridViewMCRStatus.RowTemplate.Height = 23;
            this.GridViewMCRStatus.Size = new System.Drawing.Size(413, 254);
            this.GridViewMCRStatus.TabIndex = 14;
            // 
            // BtnSelect
            // 
            this.BtnSelect.BackColor = System.Drawing.Color.Transparent;
            this.BtnSelect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSelect.ButtonText = "SELECT";
            this.BtnSelect.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSelect.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSelect.Location = new System.Drawing.Point(851, 64);
            this.BtnSelect.Name = "BtnSelect";
            this.BtnSelect.Size = new System.Drawing.Size(413, 46);
            this.BtnSelect.TabIndex = 38;
            this.BtnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorkerUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerUpdate_DoWork);
            // 
            // CFormStatisticsEquipmentOperationMonitoring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.Controls.Add(this.BtnSelect);
            this.Controls.Add(this.GridViewMCRStatus);
            this.Controls.Add(this.GridViewTimeToRefair);
            this.Controls.Add(this.GridViewMomentaryStopStatus);
            this.Controls.Add(this.GridViewNonOperationStatus);
            this.Controls.Add(this.GridViewOperationStatus);
            this.Controls.Add(this.GridViewProductionStatus);
            this.Controls.Add(this.DateTimeFrom);
            this.Controls.Add(this.DateTimeTo);
            this.Controls.Add(this.BtnTitleMCRStatus);
            this.Controls.Add(this.BtnTitleTimeToRefair);
            this.Controls.Add(this.BtnTitleMomentaryStop);
            this.Controls.Add(this.BtnTitleNonOperationStatus);
            this.Controls.Add(this.BtnTitleOperationStatus);
            this.Controls.Add(this.BtnTitleProductionStatus);
            this.Controls.Add(this.BtnTitleEquipmentOperationMonitoring);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormStatisticsEquipmentOperationMonitoring";
            this.Text = "CFormStatisticsEquipmentOperationMonitoring";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormStatisticsEquipmentOperationMonitoring_FormClosed);
            this.Load += new System.EventHandler(this.CFormStatisticsEquipmentOperationMonitoring_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewProductionStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewOperationStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewNonOperationStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewMomentaryStopStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewTimeToRefair)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewMCRStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private UiAsset.SpeedButton BtnTitleEquipmentOperationMonitoring;
        private System.Windows.Forms.DateTimePicker DateTimeFrom;
		private System.Windows.Forms.DateTimePicker DateTimeTo;
        private UiAsset.SpeedButton BtnTitleProductionStatus;
        private System.Windows.Forms.DataGridView GridViewProductionStatus;
        private UiAsset.SpeedButton BtnTitleOperationStatus;
        private System.Windows.Forms.DataGridView GridViewOperationStatus;
        private UiAsset.SpeedButton BtnTitleNonOperationStatus;
        private System.Windows.Forms.DataGridView GridViewNonOperationStatus;
        private UiAsset.SpeedButton BtnTitleMomentaryStop;
        private System.Windows.Forms.DataGridView GridViewMomentaryStopStatus;
        private UiAsset.SpeedButton BtnTitleTimeToRefair;
        private System.Windows.Forms.DataGridView GridViewTimeToRefair;
        private UiAsset.SpeedButton BtnTitleMCRStatus;
        private System.Windows.Forms.DataGridView GridViewMCRStatus;
		private UiAsset.ImageButton BtnSelect;
        private System.ComponentModel.BackgroundWorker backgroundWorkerUpdate;
    }
}