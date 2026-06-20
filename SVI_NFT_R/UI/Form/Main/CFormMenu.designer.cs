namespace SVI_NFT_R
{
    partial class CFormMenu
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
            this.BtnMain = new UiAsset.ImageButton();
            this.BtnAlarm = new UiAsset.ImageButton();
            this.BtnStatistics = new UiAsset.ImageButton();
            this.BtnTeach = new UiAsset.ImageButton();
            this.BtnSetup = new UiAsset.ImageButton();
            this.BtnConfig = new UiAsset.ImageButton();
            this.BtnPM = new UiAsset.ImageButton();
            this.BtnLanguage = new UiAsset.ImageButton();
            this.BtnExit = new UiAsset.ImageButton();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnMain
            // 
            this.BtnMain.BackColor = System.Drawing.Color.Transparent;
            this.BtnMain.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnMain.ButtonText = "MAIN";
            this.BtnMain.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnMain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnMain.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnMain.Enabled = false;
            this.BtnMain.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnMain.Location = new System.Drawing.Point(12, 12);
            this.BtnMain.Name = "BtnMain";
            this.BtnMain.Size = new System.Drawing.Size(146, 76);
            this.BtnMain.TabIndex = 3;
            this.BtnMain.TabStop = false;
            this.BtnMain.Text = "MAIN";
            this.BtnMain.UseVisualStyleBackColor = false;
            this.BtnMain.Click += new System.EventHandler(this.BtnMain_Click);
            // 
            // BtnAlarm
            // 
            this.BtnAlarm.BackColor = System.Drawing.Color.Transparent;
            this.BtnAlarm.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnAlarm.ButtonText = "ALARM";
            this.BtnAlarm.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnAlarm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAlarm.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnAlarm.Enabled = false;
            this.BtnAlarm.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnAlarm.Location = new System.Drawing.Point(164, 12);
            this.BtnAlarm.Name = "BtnAlarm";
            this.BtnAlarm.Size = new System.Drawing.Size(146, 76);
            this.BtnAlarm.TabIndex = 3;
            this.BtnAlarm.TabStop = false;
            this.BtnAlarm.Text = "ALARM";
            this.BtnAlarm.UseVisualStyleBackColor = false;
            this.BtnAlarm.Click += new System.EventHandler(this.BtnAlarm_Click);
            // 
            // BtnStatistics
            // 
            this.BtnStatistics.BackColor = System.Drawing.Color.Transparent;
            this.BtnStatistics.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnStatistics.ButtonText = "STATISTICS";
            this.BtnStatistics.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnStatistics.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnStatistics.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnStatistics.Enabled = false;
            this.BtnStatistics.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnStatistics.Location = new System.Drawing.Point(316, 12);
            this.BtnStatistics.Name = "BtnStatistics";
            this.BtnStatistics.Size = new System.Drawing.Size(146, 76);
            this.BtnStatistics.TabIndex = 3;
            this.BtnStatistics.TabStop = false;
            this.BtnStatistics.Text = "STATISTICS";
            this.BtnStatistics.UseVisualStyleBackColor = false;
            this.BtnStatistics.Click += new System.EventHandler(this.BtnStatistics_Click);
            // 
            // BtnTeach
            // 
            this.BtnTeach.BackColor = System.Drawing.Color.Transparent;
            this.BtnTeach.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnTeach.ButtonText = "TEACH";
            this.BtnTeach.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnTeach.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTeach.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnTeach.Enabled = false;
            this.BtnTeach.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnTeach.Location = new System.Drawing.Point(468, 12);
            this.BtnTeach.Name = "BtnTeach";
            this.BtnTeach.Size = new System.Drawing.Size(146, 76);
            this.BtnTeach.TabIndex = 3;
            this.BtnTeach.TabStop = false;
            this.BtnTeach.Text = "TEACH";
            this.BtnTeach.UseVisualStyleBackColor = false;
            this.BtnTeach.Click += new System.EventHandler(this.BtnTeach_Click);
            // 
            // BtnSetup
            // 
            this.BtnSetup.BackColor = System.Drawing.Color.Transparent;
            this.BtnSetup.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSetup.ButtonText = "SETUP";
            this.BtnSetup.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnSetup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSetup.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSetup.Enabled = false;
            this.BtnSetup.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSetup.Location = new System.Drawing.Point(620, 12);
            this.BtnSetup.Name = "BtnSetup";
            this.BtnSetup.Size = new System.Drawing.Size(146, 76);
            this.BtnSetup.TabIndex = 3;
            this.BtnSetup.TabStop = false;
            this.BtnSetup.Text = "SETUP";
            this.BtnSetup.UseVisualStyleBackColor = false;
            this.BtnSetup.Click += new System.EventHandler(this.BtnSetup_Click);
            // 
            // BtnConfig
            // 
            this.BtnConfig.BackColor = System.Drawing.Color.Transparent;
            this.BtnConfig.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnConfig.ButtonText = "CONFIG";
            this.BtnConfig.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnConfig.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnConfig.Enabled = false;
            this.BtnConfig.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnConfig.Location = new System.Drawing.Point(772, 12);
            this.BtnConfig.Name = "BtnConfig";
            this.BtnConfig.Size = new System.Drawing.Size(146, 76);
            this.BtnConfig.TabIndex = 3;
            this.BtnConfig.TabStop = false;
            this.BtnConfig.Text = "CONFIG";
            this.BtnConfig.UseVisualStyleBackColor = false;
            this.BtnConfig.Click += new System.EventHandler(this.BtnConfig_Click);
            // 
            // BtnPM
            // 
            this.BtnPM.BackColor = System.Drawing.Color.Transparent;
            this.BtnPM.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPM.ButtonText = "PM";
            this.BtnPM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPM.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPM.Enabled = false;
            this.BtnPM.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPM.Location = new System.Drawing.Point(924, 12);
            this.BtnPM.Name = "BtnPM";
            this.BtnPM.Size = new System.Drawing.Size(91, 76);
            this.BtnPM.TabIndex = 3;
            this.BtnPM.TabStop = false;
            this.BtnPM.Text = "PM";
            this.BtnPM.UseVisualStyleBackColor = false;
            this.BtnPM.Click += new System.EventHandler(this.BtnPM_Click);
            // 
            // BtnLanguage
            // 
            this.BtnLanguage.BackColor = System.Drawing.Color.Transparent;
            this.BtnLanguage.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnLanguage.ButtonText = "LANGUAGE";
            this.BtnLanguage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnLanguage.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnLanguage.Enabled = false;
            this.BtnLanguage.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnLanguage.Location = new System.Drawing.Point(1021, 12);
            this.BtnLanguage.Name = "BtnLanguage";
            this.BtnLanguage.Size = new System.Drawing.Size(91, 76);
            this.BtnLanguage.TabIndex = 3;
            this.BtnLanguage.TabStop = false;
            this.BtnLanguage.Text = "LANGUAGE";
            this.BtnLanguage.UseVisualStyleBackColor = false;
            this.BtnLanguage.Click += new System.EventHandler(this.BtnLanguage_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.BackColor = System.Drawing.Color.Transparent;
            this.BtnExit.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnExit.ButtonText = "EXIT";
            this.BtnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnExit.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnExit.Enabled = false;
            this.BtnExit.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnExit.Location = new System.Drawing.Point(1118, 12);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(146, 76);
            this.BtnExit.TabIndex = 3;
            this.BtnExit.TabStop = false;
            this.BtnExit.Text = "EXIT";
            this.BtnExit.UseVisualStyleBackColor = false;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // CFormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 100);
            this.ControlBox = false;
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnLanguage);
            this.Controls.Add(this.BtnPM);
            this.Controls.Add(this.BtnConfig);
            this.Controls.Add(this.BtnSetup);
            this.Controls.Add(this.BtnTeach);
            this.Controls.Add(this.BtnStatistics);
            this.Controls.Add(this.BtnAlarm);
            this.Controls.Add(this.BtnMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 100);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1276, 100);
            this.Name = "CFormMenu";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormMenu";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormMenu_FormClosed);
            this.Load += new System.EventHandler(this.CFormMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private UiAsset.ImageButton BtnMain;
        private UiAsset.ImageButton BtnAlarm;
        private UiAsset.ImageButton BtnStatistics;
        private UiAsset.ImageButton BtnTeach;
        private UiAsset.ImageButton BtnSetup;
        private UiAsset.ImageButton BtnConfig;
        private UiAsset.ImageButton BtnPM;
        private UiAsset.ImageButton BtnLanguage;
        private UiAsset.ImageButton BtnExit;

    }
}