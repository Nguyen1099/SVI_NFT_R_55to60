namespace SVI_NFT_R
{
    partial class CDialogCIMDisconnected
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDialogCIMDisconnected));
            this.BtnComment = new UiAsset.SpeedButton();
            this.BtnConfirm = new UiAsset.SpeedButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // BtnComment
            // 
            this.BtnComment.BackColor = System.Drawing.Color.Black;
            this.BtnComment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnComment.Font = new System.Drawing.Font("굴림", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnComment.ForeColor = System.Drawing.Color.Red;
            this.BtnComment.Location = new System.Drawing.Point(4, 5);
            this.BtnComment.Name = "BtnComment";
            this.BtnComment.Size = new System.Drawing.Size(508, 196);
            this.BtnComment.TabIndex = 0;
            this.BtnComment.TabStop = false;
            this.BtnComment.Text = "CIM IS DISCONNECTED. HOST IS DISCONNECTED.";
            this.BtnComment.UseVisualStyleBackColor = false;
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnConfirm.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnConfirm.Location = new System.Drawing.Point(4, 207);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(508, 70);
            this.BtnConfirm.TabIndex = 1;
            this.BtnConfirm.TabStop = false;
            this.BtnConfirm.Text = "CONFIRM";
            this.BtnConfirm.UseVisualStyleBackColor = true;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // CDialogCIMDisconnected
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(516, 282);
            this.ControlBox = false;
            this.Controls.Add(this.BtnConfirm);
            this.Controls.Add(this.BtnComment);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CDialogCIMDisconnected";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[ CIM Disconnected ]";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CDialogCIMDisconnected_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogCIMDisconnected_FormClosed);
            this.Load += new System.EventHandler(this.CDialogCIMDisconnected_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UiAsset.SpeedButton BtnComment;
        private UiAsset.SpeedButton BtnConfirm;
        private System.Windows.Forms.Timer timer;
    }
}