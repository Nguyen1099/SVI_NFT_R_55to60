namespace SVI_NFT_R
{
    partial class CFormSetupVision
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
            this.BtnSave = new UiAsset.ImageButton();
            this.BtnTitle = new UiAsset.SpeedButton();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.PnlBaseAoiVision = new System.Windows.Forms.Panel();
            this.BtnVisionVisionVirtualMode = new UiAsset.ImageButton();
            this.BtnVisionVisionErrorSkipMode = new UiAsset.ImageButton();
            this.BtnVisionVisionBypassMode = new UiAsset.ImageButton();
            this.BtnVisionVisionNormalMode = new UiAsset.ImageButton();
            this.BtnTitleVision = new UiAsset.SpeedButton();
            this.BtnTitleVisionParameter = new UiAsset.SpeedButton();
            this.BtnTitleVisionVisionMode = new UiAsset.SpeedButton();
            this.BtnTriggerEndPosition = new UiAsset.SpeedButton();
            this.BtnTriggerStartPosition = new UiAsset.SpeedButton();
            this.BtnInspectionVelocity = new UiAsset.SpeedButton();
            this.BtnPulsePeriodLength = new UiAsset.SpeedButton();
            this.BtnPulseWidthTime = new UiAsset.SpeedButton();
            this.BtnTitleTriggerEndPosition = new UiAsset.SpeedButton();
            this.BtnTitleTriggerStartPosition = new UiAsset.SpeedButton();
            this.BtnTitleInspectionVelocity = new UiAsset.SpeedButton();
            this.BtnTitlePulsePeriodLength = new UiAsset.SpeedButton();
            this.BtnTitlePulseWidthTime = new UiAsset.SpeedButton();
            this.flowLayoutPanel.SuspendLayout();
            this.PnlBaseAoiVision.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.Transparent;
            this.BtnSave.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSave.ButtonText = "SAVE";
            this.BtnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSave.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSave.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSave.Location = new System.Drawing.Point(1054, 687);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(210, 46);
            this.BtnSave.TabIndex = 32;
            this.BtnSave.Text = "SAVE";
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnTitle
            // 
            this.BtnTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitle.Location = new System.Drawing.Point(12, 12);
            this.BtnTitle.Name = "BtnTitle";
            this.BtnTitle.Size = new System.Drawing.Size(1252, 46);
            this.BtnTitle.TabIndex = 7;
            this.BtnTitle.TabStop = false;
            this.BtnTitle.Text = "SDV VISION";
            this.BtnTitle.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Controls.Add(this.PnlBaseAoiVision);
            this.flowLayoutPanel.Location = new System.Drawing.Point(12, 64);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(1252, 621);
            this.flowLayoutPanel.TabIndex = 33;
            // 
            // PnlBaseAoiVision
            // 
            this.PnlBaseAoiVision.Controls.Add(this.BtnTriggerEndPosition);
            this.PnlBaseAoiVision.Controls.Add(this.BtnTriggerStartPosition);
            this.PnlBaseAoiVision.Controls.Add(this.BtnInspectionVelocity);
            this.PnlBaseAoiVision.Controls.Add(this.BtnPulsePeriodLength);
            this.PnlBaseAoiVision.Controls.Add(this.BtnPulseWidthTime);
            this.PnlBaseAoiVision.Controls.Add(this.BtnTitleTriggerEndPosition);
            this.PnlBaseAoiVision.Controls.Add(this.BtnTitleTriggerStartPosition);
            this.PnlBaseAoiVision.Controls.Add(this.BtnTitleInspectionVelocity);
            this.PnlBaseAoiVision.Controls.Add(this.BtnTitlePulsePeriodLength);
            this.PnlBaseAoiVision.Controls.Add(this.BtnTitlePulseWidthTime);
            this.PnlBaseAoiVision.Controls.Add(this.BtnVisionVisionVirtualMode);
            this.PnlBaseAoiVision.Controls.Add(this.BtnVisionVisionErrorSkipMode);
            this.PnlBaseAoiVision.Controls.Add(this.BtnVisionVisionBypassMode);
            this.PnlBaseAoiVision.Controls.Add(this.BtnVisionVisionNormalMode);
            this.PnlBaseAoiVision.Controls.Add(this.BtnTitleVision);
            this.PnlBaseAoiVision.Controls.Add(this.BtnTitleVisionParameter);
            this.PnlBaseAoiVision.Controls.Add(this.BtnTitleVisionVisionMode);
            this.PnlBaseAoiVision.Location = new System.Drawing.Point(0, 0);
            this.PnlBaseAoiVision.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.PnlBaseAoiVision.Name = "PnlBaseAoiVision";
            this.PnlBaseAoiVision.Size = new System.Drawing.Size(407, 621);
            this.PnlBaseAoiVision.TabIndex = 1;
            // 
            // BtnVisionVisionVirtualMode
            // 
            this.BtnVisionVisionVirtualMode.BackColor = System.Drawing.Color.Transparent;
            this.BtnVisionVisionVirtualMode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnVisionVisionVirtualMode.ButtonText = "NOT USE MODE";
            this.BtnVisionVisionVirtualMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVisionVisionVirtualMode.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnVisionVisionVirtualMode.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnVisionVisionVirtualMode.Location = new System.Drawing.Point(2, 468);
            this.BtnVisionVisionVirtualMode.Name = "BtnVisionVisionVirtualMode";
            this.BtnVisionVisionVirtualMode.Size = new System.Drawing.Size(405, 46);
            this.BtnVisionVisionVirtualMode.TabIndex = 52;
            this.BtnVisionVisionVirtualMode.Text = "NOT USE MODE";
            this.BtnVisionVisionVirtualMode.UseVisualStyleBackColor = false;
            this.BtnVisionVisionVirtualMode.Click += new System.EventHandler(this.BtnVisionMode_Click);
            // 
            // BtnVisionVisionErrorSkipMode
            // 
            this.BtnVisionVisionErrorSkipMode.BackColor = System.Drawing.Color.Transparent;
            this.BtnVisionVisionErrorSkipMode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnVisionVisionErrorSkipMode.ButtonText = "ERROR SKIP MODE";
            this.BtnVisionVisionErrorSkipMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVisionVisionErrorSkipMode.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnVisionVisionErrorSkipMode.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnVisionVisionErrorSkipMode.Location = new System.Drawing.Point(2, 572);
            this.BtnVisionVisionErrorSkipMode.Name = "BtnVisionVisionErrorSkipMode";
            this.BtnVisionVisionErrorSkipMode.Size = new System.Drawing.Size(405, 46);
            this.BtnVisionVisionErrorSkipMode.TabIndex = 51;
            this.BtnVisionVisionErrorSkipMode.Text = "ERROR SKIP MODE";
            this.BtnVisionVisionErrorSkipMode.UseVisualStyleBackColor = false;
            this.BtnVisionVisionErrorSkipMode.Visible = false;
            this.BtnVisionVisionErrorSkipMode.Click += new System.EventHandler(this.BtnVisionMode_Click);
            // 
            // BtnVisionVisionBypassMode
            // 
            this.BtnVisionVisionBypassMode.BackColor = System.Drawing.Color.Transparent;
            this.BtnVisionVisionBypassMode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnVisionVisionBypassMode.ButtonText = "BYPASS MODE";
            this.BtnVisionVisionBypassMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVisionVisionBypassMode.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnVisionVisionBypassMode.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnVisionVisionBypassMode.Location = new System.Drawing.Point(2, 520);
            this.BtnVisionVisionBypassMode.Name = "BtnVisionVisionBypassMode";
            this.BtnVisionVisionBypassMode.Size = new System.Drawing.Size(405, 46);
            this.BtnVisionVisionBypassMode.TabIndex = 50;
            this.BtnVisionVisionBypassMode.Text = "BYPASS MODE";
            this.BtnVisionVisionBypassMode.UseVisualStyleBackColor = false;
            this.BtnVisionVisionBypassMode.Visible = false;
            this.BtnVisionVisionBypassMode.Click += new System.EventHandler(this.BtnVisionMode_Click);
            // 
            // BtnVisionVisionNormalMode
            // 
            this.BtnVisionVisionNormalMode.BackColor = System.Drawing.Color.Transparent;
            this.BtnVisionVisionNormalMode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnVisionVisionNormalMode.ButtonText = "USE MODE";
            this.BtnVisionVisionNormalMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVisionVisionNormalMode.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnVisionVisionNormalMode.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnVisionVisionNormalMode.Location = new System.Drawing.Point(2, 416);
            this.BtnVisionVisionNormalMode.Name = "BtnVisionVisionNormalMode";
            this.BtnVisionVisionNormalMode.Size = new System.Drawing.Size(405, 46);
            this.BtnVisionVisionNormalMode.TabIndex = 49;
            this.BtnVisionVisionNormalMode.Text = "USE MODE";
            this.BtnVisionVisionNormalMode.UseVisualStyleBackColor = false;
            this.BtnVisionVisionNormalMode.Click += new System.EventHandler(this.BtnVisionMode_Click);
            // 
            // BtnTitleVision
            // 
            this.BtnTitleVision.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVision.Location = new System.Drawing.Point(2, 0);
            this.BtnTitleVision.Name = "BtnTitleVision";
            this.BtnTitleVision.Size = new System.Drawing.Size(405, 46);
            this.BtnTitleVision.TabIndex = 36;
            this.BtnTitleVision.TabStop = false;
            this.BtnTitleVision.Text = "TOP AUTOMATED OPTICAL INSPECT";
            this.BtnTitleVision.UseVisualStyleBackColor = true;
            // 
            // BtnTitleVisionParameter
            // 
            this.BtnTitleVisionParameter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVisionParameter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVisionParameter.Location = new System.Drawing.Point(2, 52);
            this.BtnTitleVisionParameter.Name = "BtnTitleVisionParameter";
            this.BtnTitleVisionParameter.Size = new System.Drawing.Size(405, 46);
            this.BtnTitleVisionParameter.TabIndex = 35;
            this.BtnTitleVisionParameter.TabStop = false;
            this.BtnTitleVisionParameter.Text = "VISION PARAMETER";
            this.BtnTitleVisionParameter.UseVisualStyleBackColor = true;
            // 
            // BtnTitleVisionVisionMode
            // 
            this.BtnTitleVisionVisionMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleVisionVisionMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleVisionVisionMode.Location = new System.Drawing.Point(2, 364);
            this.BtnTitleVisionVisionMode.Name = "BtnTitleVisionVisionMode";
            this.BtnTitleVisionVisionMode.Size = new System.Drawing.Size(405, 46);
            this.BtnTitleVisionVisionMode.TabIndex = 35;
            this.BtnTitleVisionVisionMode.TabStop = false;
            this.BtnTitleVisionVisionMode.Text = "VISION MODE";
            this.BtnTitleVisionVisionMode.UseVisualStyleBackColor = true;
            // 
            // BtnTriggerEndPosition
            // 
            this.BtnTriggerEndPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTriggerEndPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTriggerEndPosition.Location = new System.Drawing.Point(207, 312);
            this.BtnTriggerEndPosition.Name = "BtnTriggerEndPosition";
            this.BtnTriggerEndPosition.Size = new System.Drawing.Size(200, 46);
            this.BtnTriggerEndPosition.TabIndex = 72;
            this.BtnTriggerEndPosition.TabStop = false;
            this.BtnTriggerEndPosition.UseVisualStyleBackColor = true;
            this.BtnTriggerEndPosition.Click += new System.EventHandler(this.BtnTriggerEndPosition_Click);
            // 
            // BtnTriggerStartPosition
            // 
            this.BtnTriggerStartPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTriggerStartPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTriggerStartPosition.Location = new System.Drawing.Point(207, 260);
            this.BtnTriggerStartPosition.Name = "BtnTriggerStartPosition";
            this.BtnTriggerStartPosition.Size = new System.Drawing.Size(200, 46);
            this.BtnTriggerStartPosition.TabIndex = 71;
            this.BtnTriggerStartPosition.TabStop = false;
            this.BtnTriggerStartPosition.UseVisualStyleBackColor = true;
            this.BtnTriggerStartPosition.Click += new System.EventHandler(this.BtnTriggerStartPosition_Click);
            // 
            // BtnInspectionVelocity
            // 
            this.BtnInspectionVelocity.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnInspectionVelocity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnInspectionVelocity.Location = new System.Drawing.Point(207, 208);
            this.BtnInspectionVelocity.Name = "BtnInspectionVelocity";
            this.BtnInspectionVelocity.Size = new System.Drawing.Size(200, 46);
            this.BtnInspectionVelocity.TabIndex = 73;
            this.BtnInspectionVelocity.TabStop = false;
            this.BtnInspectionVelocity.UseVisualStyleBackColor = true;
            this.BtnInspectionVelocity.Click += new System.EventHandler(this.BtnInspectionVelocity_Click);
            // 
            // BtnPulsePeriodLength
            // 
            this.BtnPulsePeriodLength.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPulsePeriodLength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPulsePeriodLength.Location = new System.Drawing.Point(207, 156);
            this.BtnPulsePeriodLength.Name = "BtnPulsePeriodLength";
            this.BtnPulsePeriodLength.Size = new System.Drawing.Size(200, 46);
            this.BtnPulsePeriodLength.TabIndex = 75;
            this.BtnPulsePeriodLength.TabStop = false;
            this.BtnPulsePeriodLength.UseVisualStyleBackColor = true;
            this.BtnPulsePeriodLength.Click += new System.EventHandler(this.BtnPulsePeriodLength_Click);
            // 
            // BtnPulseWidthTime
            // 
            this.BtnPulseWidthTime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPulseWidthTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPulseWidthTime.Location = new System.Drawing.Point(207, 104);
            this.BtnPulseWidthTime.Name = "BtnPulseWidthTime";
            this.BtnPulseWidthTime.Size = new System.Drawing.Size(200, 46);
            this.BtnPulseWidthTime.TabIndex = 74;
            this.BtnPulseWidthTime.TabStop = false;
            this.BtnPulseWidthTime.UseVisualStyleBackColor = true;
            this.BtnPulseWidthTime.Click += new System.EventHandler(this.BtnPulseWidthTime_Click);
            // 
            // BtnTitleTriggerEndPosition
            // 
            this.BtnTitleTriggerEndPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleTriggerEndPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleTriggerEndPosition.Location = new System.Drawing.Point(2, 312);
            this.BtnTitleTriggerEndPosition.Name = "BtnTitleTriggerEndPosition";
            this.BtnTitleTriggerEndPosition.Size = new System.Drawing.Size(200, 46);
            this.BtnTitleTriggerEndPosition.TabIndex = 67;
            this.BtnTitleTriggerEndPosition.TabStop = false;
            this.BtnTitleTriggerEndPosition.Text = "TRIGGER END POSITION";
            this.BtnTitleTriggerEndPosition.UseVisualStyleBackColor = true;
            // 
            // BtnTitleTriggerStartPosition
            // 
            this.BtnTitleTriggerStartPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleTriggerStartPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleTriggerStartPosition.Location = new System.Drawing.Point(2, 260);
            this.BtnTitleTriggerStartPosition.Name = "BtnTitleTriggerStartPosition";
            this.BtnTitleTriggerStartPosition.Size = new System.Drawing.Size(200, 46);
            this.BtnTitleTriggerStartPosition.TabIndex = 66;
            this.BtnTitleTriggerStartPosition.TabStop = false;
            this.BtnTitleTriggerStartPosition.Text = "TRIGGER START POSITION";
            this.BtnTitleTriggerStartPosition.UseVisualStyleBackColor = true;
            // 
            // BtnTitleInspectionVelocity
            // 
            this.BtnTitleInspectionVelocity.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleInspectionVelocity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleInspectionVelocity.Location = new System.Drawing.Point(2, 208);
            this.BtnTitleInspectionVelocity.Name = "BtnTitleInspectionVelocity";
            this.BtnTitleInspectionVelocity.Size = new System.Drawing.Size(200, 46);
            this.BtnTitleInspectionVelocity.TabIndex = 68;
            this.BtnTitleInspectionVelocity.TabStop = false;
            this.BtnTitleInspectionVelocity.Text = "INSPECTION VELOCITY";
            this.BtnTitleInspectionVelocity.UseVisualStyleBackColor = true;
            // 
            // BtnTitlePulsePeriodLength
            // 
            this.BtnTitlePulsePeriodLength.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitlePulsePeriodLength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitlePulsePeriodLength.Location = new System.Drawing.Point(2, 156);
            this.BtnTitlePulsePeriodLength.Name = "BtnTitlePulsePeriodLength";
            this.BtnTitlePulsePeriodLength.Size = new System.Drawing.Size(200, 46);
            this.BtnTitlePulsePeriodLength.TabIndex = 70;
            this.BtnTitlePulsePeriodLength.TabStop = false;
            this.BtnTitlePulsePeriodLength.Text = "PULSE PERIOD LENGTH";
            this.BtnTitlePulsePeriodLength.UseVisualStyleBackColor = true;
            // 
            // BtnTitlePulseWidthTime
            // 
            this.BtnTitlePulseWidthTime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitlePulseWidthTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitlePulseWidthTime.Location = new System.Drawing.Point(2, 104);
            this.BtnTitlePulseWidthTime.Name = "BtnTitlePulseWidthTime";
            this.BtnTitlePulseWidthTime.Size = new System.Drawing.Size(200, 46);
            this.BtnTitlePulseWidthTime.TabIndex = 69;
            this.BtnTitlePulseWidthTime.TabStop = false;
            this.BtnTitlePulseWidthTime.Text = "PULSE WIDTH TIME";
            this.BtnTitlePulseWidthTime.UseVisualStyleBackColor = true;
            // 
            // CFormSetupVision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormSetupVision";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormSetupVision";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormSetupVision_FormClosed);
            this.Load += new System.EventHandler(this.CFormSetupVision_Load);
            this.flowLayoutPanel.ResumeLayout(false);
            this.PnlBaseAoiVision.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private UiAsset.SpeedButton BtnTitle;
        private UiAsset.ImageButton BtnSave;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Panel PnlBaseAoiVision;
        private UiAsset.ImageButton BtnVisionVisionVirtualMode;
        private UiAsset.ImageButton BtnVisionVisionErrorSkipMode;
        private UiAsset.ImageButton BtnVisionVisionBypassMode;
        private UiAsset.ImageButton BtnVisionVisionNormalMode;
        private UiAsset.SpeedButton BtnTitleVision;
        private UiAsset.SpeedButton BtnTitleVisionParameter;
        private UiAsset.SpeedButton BtnTitleVisionVisionMode;
        private UiAsset.SpeedButton BtnTriggerEndPosition;
        private UiAsset.SpeedButton BtnTriggerStartPosition;
        private UiAsset.SpeedButton BtnInspectionVelocity;
        private UiAsset.SpeedButton BtnPulsePeriodLength;
        private UiAsset.SpeedButton BtnPulseWidthTime;
        private UiAsset.SpeedButton BtnTitleTriggerEndPosition;
        private UiAsset.SpeedButton BtnTitleTriggerStartPosition;
        private UiAsset.SpeedButton BtnTitleInspectionVelocity;
        private UiAsset.SpeedButton BtnTitlePulsePeriodLength;
        private UiAsset.SpeedButton BtnTitlePulseWidthTime;
    }
}