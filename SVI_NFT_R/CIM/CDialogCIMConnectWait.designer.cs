namespace SVI_NFT_R
{
    partial class CDialogCIMConnectWait
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDialogCIMConnectWait));
            this.BtnCancel = new UiAsset.SpeedButton();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxWaiting = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWaiting)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.Color.White;
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancel.Location = new System.Drawing.Point(27, 66);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(222, 37);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.TabStop = false;
            this.BtnCancel.Text = "CANCEL";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(22, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(414, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "Waiting for CIM Connection...";
            // 
            // pictureBoxWaiting
            // 
            this.pictureBoxWaiting.Image = global::SVI_NFT_R.Properties.Resources.TitleRun;
            this.pictureBoxWaiting.Location = new System.Drawing.Point(461, 18);
            this.pictureBoxWaiting.Name = "pictureBoxWaiting";
            this.pictureBoxWaiting.Size = new System.Drawing.Size(65, 65);
            this.pictureBoxWaiting.TabIndex = 0;
            this.pictureBoxWaiting.TabStop = false;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // CDialogCIMConnectWait
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(551, 107);
            this.ControlBox = false;
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBoxWaiting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CDialogCIMConnectWait";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "[ CIM Connect Wait ]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CDialogCIMConnectWait_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogCIMConnectWait_FormClosed);
            this.Load += new System.EventHandler(this.CDialogCIMConnectWait_Load);
            this.VisibleChanged += new System.EventHandler(this.CDialogCIMConnectWait_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWaiting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxWaiting;
        private System.Windows.Forms.Label label1;
        private UiAsset.SpeedButton BtnCancel;
        private System.Windows.Forms.Timer timer;
    }
}