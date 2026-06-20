namespace SVI_NFT_R
{
    partial class CDialogCellOutCode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDialogCellOutCode));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnCellOutRetest = new UiAsset.ImageButton();
            this.BtnCellOutManualTrackout = new UiAsset.ImageButton();
            this.BtnCellOutNG = new UiAsset.ImageButton();
            this.BtnCellOutGood = new UiAsset.ImageButton();
            this.BtnCancel = new UiAsset.ImageButton();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnCellOutRetest
            // 
            this.BtnCellOutRetest.BackColor = System.Drawing.Color.Transparent;
            this.BtnCellOutRetest.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutRetest.ButtonText = "RETEST";
            this.BtnCellOutRetest.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCellOutRetest.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutRetest.Location = new System.Drawing.Point(4, 5);
            this.BtnCellOutRetest.Name = "BtnCellOutRetest";
            this.BtnCellOutRetest.Size = new System.Drawing.Size(270, 57);
            this.BtnCellOutRetest.TabIndex = 4;
            this.BtnCellOutRetest.Text = "RETEST";
            this.BtnCellOutRetest.UseVisualStyleBackColor = false;
            this.BtnCellOutRetest.Click += new System.EventHandler(this.BtnCellOutRetest_Click);
            // 
            // BtnCellOutManualTrackout
            // 
            this.BtnCellOutManualTrackout.BackColor = System.Drawing.Color.Transparent;
            this.BtnCellOutManualTrackout.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutManualTrackout.ButtonText = "MANUAL TRACK OUT";
            this.BtnCellOutManualTrackout.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCellOutManualTrackout.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutManualTrackout.Location = new System.Drawing.Point(4, 66);
            this.BtnCellOutManualTrackout.Name = "BtnCellOutManualTrackout";
            this.BtnCellOutManualTrackout.Size = new System.Drawing.Size(270, 57);
            this.BtnCellOutManualTrackout.TabIndex = 4;
            this.BtnCellOutManualTrackout.Text = "MANUAL TRACK OUT";
            this.BtnCellOutManualTrackout.UseVisualStyleBackColor = false;
            this.BtnCellOutManualTrackout.Click += new System.EventHandler(this.BtnCellOutManualTrackout_Click);
            // 
            // BtnCellOutNG
            // 
            this.BtnCellOutNG.BackColor = System.Drawing.Color.Transparent;
            this.BtnCellOutNG.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutNG.ButtonText = "NG";
            this.BtnCellOutNG.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCellOutNG.Enabled = false;
            this.BtnCellOutNG.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutNG.Location = new System.Drawing.Point(4, 127);
            this.BtnCellOutNG.Name = "BtnCellOutNG";
            this.BtnCellOutNG.Size = new System.Drawing.Size(270, 57);
            this.BtnCellOutNG.TabIndex = 4;
            this.BtnCellOutNG.Text = "NG";
            this.BtnCellOutNG.UseVisualStyleBackColor = false;
            this.BtnCellOutNG.Click += new System.EventHandler(this.BtnCellOutNG_Click);
            // 
            // BtnCellOutGood
            // 
            this.BtnCellOutGood.BackColor = System.Drawing.Color.Transparent;
            this.BtnCellOutGood.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutGood.ButtonText = "GOOD";
            this.BtnCellOutGood.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCellOutGood.Enabled = false;
            this.BtnCellOutGood.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutGood.Location = new System.Drawing.Point(4, 188);
            this.BtnCellOutGood.Name = "BtnCellOutGood";
            this.BtnCellOutGood.Size = new System.Drawing.Size(270, 57);
            this.BtnCellOutGood.TabIndex = 4;
            this.BtnCellOutGood.Text = "GOOD";
            this.BtnCellOutGood.UseVisualStyleBackColor = false;
            this.BtnCellOutGood.Click += new System.EventHandler(this.BtnCellOutGood_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.BtnCancel.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCancel.ButtonText = "CANCEL";
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCancel.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCancel.Location = new System.Drawing.Point(4, 277);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(270, 57);
            this.BtnCancel.TabIndex = 4;
            this.BtnCancel.Text = "CANCEL";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // CDialogCellOutCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(278, 336);
            this.ControlBox = false;
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnCellOutGood);
            this.Controls.Add(this.BtnCellOutNG);
            this.Controls.Add(this.BtnCellOutManualTrackout);
            this.Controls.Add(this.BtnCellOutRetest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CDialogCellOutCode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "[ Cell Out Code ]";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogCellOutCode_FormClosed);
            this.Load += new System.EventHandler(this.CDialogCellOutCode_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private UiAsset.ImageButton BtnCellOutRetest;
        private UiAsset.ImageButton BtnCellOutManualTrackout;
        private UiAsset.ImageButton BtnCellOutNG;
        private UiAsset.ImageButton BtnCellOutGood;
        private UiAsset.ImageButton BtnCancel;
    }
}