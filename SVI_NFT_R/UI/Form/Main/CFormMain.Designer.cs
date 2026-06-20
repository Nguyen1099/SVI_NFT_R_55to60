namespace SVI_NFT_R
{
    partial class CFormMain
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
            if(disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFormMain));
            this.panelForm = new System.Windows.Forms.Panel();
            this.panelFormView = new System.Windows.Forms.Panel();
            this.panelFormMenu = new System.Windows.Forms.Panel();
            this.BtnBase = new UiAsset.SpeedButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.timerMCRDlg = new System.Windows.Forms.Timer(this.components);
            this.panelForm.SuspendLayout();
            this.panelFormMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelForm
            // 
            this.panelForm.Controls.Add(this.panelFormView);
            this.panelForm.Controls.Add(this.panelFormMenu);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForm.Location = new System.Drawing.Point(0, 0);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(1276, 792);
            this.panelForm.TabIndex = 0;
            // 
            // panelFormView
            // 
            this.panelFormView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFormView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFormView.Location = new System.Drawing.Point(0, 46);
            this.panelFormView.Name = "panelFormView";
            this.panelFormView.Size = new System.Drawing.Size(1276, 746);
            this.panelFormView.TabIndex = 4;
            // 
            // panelFormMenu
            // 
            this.panelFormMenu.Controls.Add(this.BtnBase);
            this.panelFormMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFormMenu.Location = new System.Drawing.Point(0, 0);
            this.panelFormMenu.Name = "panelFormMenu";
            this.panelFormMenu.Size = new System.Drawing.Size(1276, 46);
            this.panelFormMenu.TabIndex = 6;
            // 
            // BtnBase
            // 
            this.BtnBase.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBase.Enabled = false;
            this.BtnBase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBase.Location = new System.Drawing.Point(0, 0);
            this.BtnBase.Name = "BtnBase";
            this.BtnBase.Size = new System.Drawing.Size(139, 41);
            this.BtnBase.TabIndex = 4;
            this.BtnBase.TabStop = false;
            this.BtnBase.Text = "BASE";
            this.BtnBase.UseVisualStyleBackColor = true;
            this.BtnBase.Visible = false;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // timerMCRDlg
            // 
            this.timerMCRDlg.Tick += new System.EventHandler(this.timerMCRDlg_Tick);
            // 
            // CFormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 792);
            this.ControlBox = false;
            this.Controls.Add(this.panelForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFormMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormMain";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormMain_FormClosed);
            this.Load += new System.EventHandler(this.CFormMain_Load);
            this.panelForm.ResumeLayout(false);
            this.panelFormMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelForm;
        private System.Windows.Forms.Panel panelFormView;
        private System.Windows.Forms.Panel panelFormMenu;
        private UiAsset.SpeedButton BtnBase;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timerMCRDlg;

    }
}