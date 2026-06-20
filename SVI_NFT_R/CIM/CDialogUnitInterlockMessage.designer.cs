namespace SVI_NFT_R
{
    partial class CDialogUnitInterlockMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDialogUnitInterlockMessage));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewInterlock = new System.Windows.Forms.DataGridView();
            this.labelInterlockMessage = new System.Windows.Forms.Label();
            this.BtnBuzzerOff = new UiAsset.ImageButton();
            this.BtnConfirm = new UiAsset.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInterlock)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // dataGridViewInterlock
            // 
            this.dataGridViewInterlock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInterlock.Location = new System.Drawing.Point(2, 50);
            this.dataGridViewInterlock.Name = "dataGridViewInterlock";
            this.dataGridViewInterlock.RowTemplate.Height = 23;
            this.dataGridViewInterlock.Size = new System.Drawing.Size(1135, 144);
            this.dataGridViewInterlock.TabIndex = 8;
            // 
            // labelInterlockMessage
            // 
            this.labelInterlockMessage.AutoSize = true;
            this.labelInterlockMessage.Font = new System.Drawing.Font("맑은 고딕", 23F, System.Drawing.FontStyle.Bold);
            this.labelInterlockMessage.Location = new System.Drawing.Point(3, 2);
            this.labelInterlockMessage.Name = "labelInterlockMessage";
            this.labelInterlockMessage.Size = new System.Drawing.Size(361, 42);
            this.labelInterlockMessage.TabIndex = 1;
            this.labelInterlockMessage.Text = "Unit Interlock Message";
            this.labelInterlockMessage.UseWaitCursor = true;
            // 
            // BtnBuzzerOff
            // 
            this.BtnBuzzerOff.BackColor = System.Drawing.Color.Transparent;
            this.BtnBuzzerOff.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBuzzerOff.ButtonText = "BUZZER OFF";
            this.BtnBuzzerOff.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnBuzzerOff.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnBuzzerOff.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBuzzerOff.Location = new System.Drawing.Point(1143, 98);
            this.BtnBuzzerOff.Name = "BtnBuzzerOff";
            this.BtnBuzzerOff.Size = new System.Drawing.Size(109, 93);
            this.BtnBuzzerOff.TabIndex = 10;
            this.BtnBuzzerOff.Click += new System.EventHandler(this.BtnBuzzerOff_Click);
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.BtnConfirm.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnConfirm.ButtonText = "CONFIRM";
            this.BtnConfirm.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnConfirm.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnConfirm.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnConfirm.Location = new System.Drawing.Point(1143, 2);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(109, 93);
            this.BtnConfirm.TabIndex = 11;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // CDialogUnitInterlockMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(1254, 196);
            this.ControlBox = false;
            this.Controls.Add(this.BtnBuzzerOff);
            this.Controls.Add(this.BtnConfirm);
            this.Controls.Add(this.dataGridViewInterlock);
            this.Controls.Add(this.labelInterlockMessage);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.Name = "CDialogUnitInterlockMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "                                              ";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CDialogUnitInterlockMessage_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogUnitInterlockMessage_FormClosed);
            this.Load += new System.EventHandler(this.CDialogUnitInterlockMessage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInterlock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Label labelInterlockMessage;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.DataGridView dataGridViewInterlock;
		private UiAsset.ImageButton BtnBuzzerOff;
		private UiAsset.ImageButton BtnConfirm;
    }
}