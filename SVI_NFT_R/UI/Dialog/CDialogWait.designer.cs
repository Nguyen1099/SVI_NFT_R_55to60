namespace SVI_NFT_R
{
    partial class CDialogWait
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDialogWait));
            this.pictureBoxWaiting = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.TxtMessage = new System.Windows.Forms.TextBox();
            this.BtnCancel = new UiAsset.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWaiting)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxWaiting
            // 
            this.pictureBoxWaiting.Image = global::SVI_NFT_R.Properties.Resources.TitleRun;
            this.pictureBoxWaiting.Location = new System.Drawing.Point(456, 22);
            this.pictureBoxWaiting.Name = "pictureBoxWaiting";
            this.pictureBoxWaiting.Size = new System.Drawing.Size(65, 65);
            this.pictureBoxWaiting.TabIndex = 0;
            this.pictureBoxWaiting.TabStop = false;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // TxtMessage
            // 
            this.TxtMessage.BackColor = System.Drawing.Color.White;
            this.TxtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtMessage.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TxtMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.TxtMessage.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Bold);
            this.TxtMessage.Location = new System.Drawing.Point(31, 22);
            this.TxtMessage.Multiline = true;
            this.TxtMessage.Name = "TxtMessage";
            this.TxtMessage.ReadOnly = true;
            this.TxtMessage.Size = new System.Drawing.Size(419, 65);
            this.TxtMessage.TabIndex = 2;
            this.TxtMessage.TabStop = false;
            this.TxtMessage.Text = "WAITTING MESSAGE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.BtnCancel.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCancel.ButtonText = "CANCEL";
            this.BtnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnCancel.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCancel.Location = new System.Drawing.Point(256, 56);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(265, 39);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "CANCEL";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Visible = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // CDialogWait
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(551, 107);
            this.ControlBox = false;
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.TxtMessage);
            this.Controls.Add(this.pictureBoxWaiting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CDialogWait";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "[ Wait ]";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CDialogWait_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogWait_FormClosed);
            this.Load += new System.EventHandler(this.CDialogWait_Load);
            this.VisibleChanged += new System.EventHandler(this.CDialogWait_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWaiting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxWaiting;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TextBox TxtMessage;
        private UiAsset.ImageButton BtnCancel;
    }
}