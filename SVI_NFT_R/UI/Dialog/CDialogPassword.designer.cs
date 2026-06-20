namespace SVI_NFT_R
{
    partial class CDialogPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDialogPassword));
            this.PnlBase = new System.Windows.Forms.Panel();
            this.PnlKeyPadSize = new System.Windows.Forms.Panel();
            this.BtnTouch = new UiAsset.SpeedButton();
            this.BtnPasswordFocus = new UiAsset.SpeedButton();
            this.BtnTitleConfirm = new UiAsset.ImageButton();
            this.BtnTitlePassword = new UiAsset.SpeedButton();
            this.TextPassword = new System.Windows.Forms.TextBox();
            this.BtnOk = new UiAsset.ImageButton();
            this.BtnCancel = new UiAsset.ImageButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.PnlBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlBase
            // 
            this.PnlBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlBase.Controls.Add(this.PnlKeyPadSize);
            this.PnlBase.Controls.Add(this.BtnTouch);
            this.PnlBase.Controls.Add(this.BtnPasswordFocus);
            this.PnlBase.Controls.Add(this.BtnTitleConfirm);
            this.PnlBase.Controls.Add(this.BtnTitlePassword);
            this.PnlBase.Controls.Add(this.TextPassword);
            this.PnlBase.Controls.Add(this.BtnOk);
            this.PnlBase.Controls.Add(this.BtnCancel);
            this.PnlBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlBase.Location = new System.Drawing.Point(0, 0);
            this.PnlBase.Margin = new System.Windows.Forms.Padding(0);
            this.PnlBase.Name = "PnlBase";
            this.PnlBase.Size = new System.Drawing.Size(542, 321);
            this.PnlBase.TabIndex = 8;
            // 
            // PnlKeyPadSize
            // 
            this.PnlKeyPadSize.BackColor = System.Drawing.Color.Magenta;
            this.PnlKeyPadSize.Location = new System.Drawing.Point(155, 177);
            this.PnlKeyPadSize.Name = "PnlKeyPadSize";
            this.PnlKeyPadSize.Size = new System.Drawing.Size(252, 3);
            this.PnlKeyPadSize.TabIndex = 44;
            this.PnlKeyPadSize.Visible = false;
            // 
            // BtnTouch
            // 
            this.BtnTouch.BackColor = System.Drawing.Color.White;
            this.BtnTouch.BackgroundImage = global::SVI_NFT_R.Properties.Resources.touchScreen;
            this.BtnTouch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnTouch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTouch.FlatAppearance.BorderSize = 0;
            this.BtnTouch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTouch.Font = new System.Drawing.Font("Gulim", 9.75F);
            this.BtnTouch.Location = new System.Drawing.Point(431, 134);
            this.BtnTouch.Name = "BtnTouch";
            this.BtnTouch.Size = new System.Drawing.Size(33, 40);
            this.BtnTouch.TabIndex = 43;
            this.BtnTouch.TabStop = false;
            this.BtnTouch.UseVisualStyleBackColor = false;
            this.BtnTouch.Click += new System.EventHandler(this.BtnTouch_Click);
            // 
            // BtnPasswordFocus
            // 
            this.BtnPasswordFocus.BackColor = System.Drawing.Color.LightGray;
            this.BtnPasswordFocus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPasswordFocus.FlatAppearance.BorderSize = 0;
            this.BtnPasswordFocus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPasswordFocus.ForeColor = System.Drawing.Color.Transparent;
            this.BtnPasswordFocus.Location = new System.Drawing.Point(64, 174);
            this.BtnPasswordFocus.Name = "BtnPasswordFocus";
            this.BtnPasswordFocus.Size = new System.Drawing.Size(400, 2);
            this.BtnPasswordFocus.TabIndex = 9;
            this.BtnPasswordFocus.TabStop = false;
            this.BtnPasswordFocus.UseVisualStyleBackColor = false;
            // 
            // BtnTitleConfirm
            // 
            this.BtnTitleConfirm.BackColor = System.Drawing.Color.Transparent;
            this.BtnTitleConfirm.BaseColor = System.Drawing.Color.White;
            this.BtnTitleConfirm.ButtonText = "CONFIRM";
            this.BtnTitleConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleConfirm.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnTitleConfirm.Font = new System.Drawing.Font("Malgun Gothic", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleConfirm.GlowColor = System.Drawing.Color.White;
            this.BtnTitleConfirm.Location = new System.Drawing.Point(17, 24);
            this.BtnTitleConfirm.Name = "BtnTitleConfirm";
            this.BtnTitleConfirm.PenColor = System.Drawing.Color.White;
            this.BtnTitleConfirm.Size = new System.Drawing.Size(341, 76);
            this.BtnTitleConfirm.TabIndex = 7;
            this.BtnTitleConfirm.TabStop = false;
            this.BtnTitleConfirm.Text = "CONFIRM";
            this.BtnTitleConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnTitleConfirm.UseVisualStyleBackColor = false;
            // 
            // BtnTitlePassword
            // 
            this.BtnTitlePassword.AutoSize = true;
            this.BtnTitlePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitlePassword.FlatAppearance.BorderSize = 0;
            this.BtnTitlePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitlePassword.Font = new System.Drawing.Font("Gulim", 9.75F);
            this.BtnTitlePassword.Location = new System.Drawing.Point(57, 145);
            this.BtnTitlePassword.Name = "BtnTitlePassword";
            this.BtnTitlePassword.Size = new System.Drawing.Size(92, 27);
            this.BtnTitlePassword.TabIndex = 7;
            this.BtnTitlePassword.TabStop = false;
            this.BtnTitlePassword.Text = "PASSWORD";
            this.BtnTitlePassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnTitlePassword.UseVisualStyleBackColor = true;
            // 
            // TextPassword
            // 
            this.TextPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextPassword.Font = new System.Drawing.Font("Gulim", 9.75F);
            this.TextPassword.Location = new System.Drawing.Point(161, 151);
            this.TextPassword.Name = "TextPassword";
            this.TextPassword.Size = new System.Drawing.Size(245, 15);
            this.TextPassword.TabIndex = 1;
            this.TextPassword.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TextPassword_MouseClick);
            this.TextPassword.Enter += new System.EventHandler(this.TextPassword_Enter);
            this.TextPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextPassword_KeyDown);
            this.TextPassword.Leave += new System.EventHandler(this.TextID_Leave);
            // 
            // BtnOk
            // 
            this.BtnOk.BackColor = System.Drawing.Color.Transparent;
            this.BtnOk.BaseColor = System.Drawing.Color.White;
            this.BtnOk.ButtonText = "OK";
            this.BtnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOk.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnOk.Font = new System.Drawing.Font("Gulim", 9.75F);
            this.BtnOk.GlowColor = System.Drawing.Color.White;
            this.BtnOk.Location = new System.Drawing.Point(155, 236);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(168, 45);
            this.BtnOk.TabIndex = 2;
            this.BtnOk.TabStop = false;
            this.BtnOk.Text = "OK";
            this.BtnOk.UseVisualStyleBackColor = false;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.BtnCancel.BaseColor = System.Drawing.Color.White;
            this.BtnCancel.ButtonText = "CANCEL";
            this.BtnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCancel.Font = new System.Drawing.Font("Gulim", 9.75F);
            this.BtnCancel.GlowColor = System.Drawing.Color.White;
            this.BtnCancel.Location = new System.Drawing.Point(329, 236);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(168, 45);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.TabStop = false;
            this.BtnCancel.Text = "CANCEL";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // CDialogPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(542, 321);
            this.ControlBox = false;
            this.Controls.Add(this.PnlBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CDialogPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CDialogLogin";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CDialogLogin_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogLogin_FormClosed);
            this.Load += new System.EventHandler(this.CDialogLogin_Load);
            this.PnlBase.ResumeLayout(false);
            this.PnlBase.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox TextPassword;
        private UiAsset.ImageButton BtnOk;
        private UiAsset.SpeedButton BtnTitlePassword;
        private UiAsset.ImageButton BtnTitleConfirm;
        private UiAsset.ImageButton BtnCancel;
        private System.Windows.Forms.Panel PnlBase;
        private System.Windows.Forms.Timer timer;
        private UiAsset.SpeedButton BtnPasswordFocus;
        private UiAsset.SpeedButton BtnTouch;
        private System.Windows.Forms.Panel PnlKeyPadSize;
    }
}