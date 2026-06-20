namespace SVI_NFT_R
{
    partial class CDialogOperatorCallMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDialogOperatorCallMessage));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewOpCall = new System.Windows.Forms.DataGridView();
            this.labelOperatorCallMessage = new System.Windows.Forms.Label();
            this.BtnConfirm = new UiAsset.ImageButton();
            this.BtnBuzzerOff = new UiAsset.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOpCall)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // dataGridViewOpCall
            // 
            this.dataGridViewOpCall.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOpCall.Location = new System.Drawing.Point(3, 49);
            this.dataGridViewOpCall.Name = "dataGridViewOpCall";
            this.dataGridViewOpCall.RowTemplate.Height = 23;
            this.dataGridViewOpCall.Size = new System.Drawing.Size(1134, 144);
            this.dataGridViewOpCall.TabIndex = 7;
            // 
            // labelOperatorCallMessage
            // 
            this.labelOperatorCallMessage.AutoSize = true;
            this.labelOperatorCallMessage.Font = new System.Drawing.Font("맑은 고딕", 23F, System.Drawing.FontStyle.Bold);
            this.labelOperatorCallMessage.Location = new System.Drawing.Point(3, 4);
            this.labelOperatorCallMessage.Name = "labelOperatorCallMessage";
            this.labelOperatorCallMessage.Size = new System.Drawing.Size(355, 42);
            this.labelOperatorCallMessage.TabIndex = 4;
            this.labelOperatorCallMessage.Text = "Operator Call Message";
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.BtnConfirm.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnConfirm.ButtonText = "CONFIRM";
            this.BtnConfirm.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnConfirm.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnConfirm.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnConfirm.Location = new System.Drawing.Point(1143, 4);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(109, 93);
            this.BtnConfirm.TabIndex = 8;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // BtnBuzzerOff
            // 
            this.BtnBuzzerOff.BackColor = System.Drawing.Color.Transparent;
            this.BtnBuzzerOff.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBuzzerOff.ButtonText = "BUZZER OFF";
            this.BtnBuzzerOff.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnBuzzerOff.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnBuzzerOff.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBuzzerOff.Location = new System.Drawing.Point(1143, 100);
            this.BtnBuzzerOff.Name = "BtnBuzzerOff";
            this.BtnBuzzerOff.Size = new System.Drawing.Size(109, 93);
            this.BtnBuzzerOff.TabIndex = 8;
            this.BtnBuzzerOff.Click += new System.EventHandler(this.BtnBuzzerOff_Click);
            // 
            // CDialogOperatorCallMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Yellow;
            this.ClientSize = new System.Drawing.Size(1254, 196);
            this.ControlBox = false;
            this.Controls.Add(this.BtnBuzzerOff);
            this.Controls.Add(this.BtnConfirm);
            this.Controls.Add(this.dataGridViewOpCall);
            this.Controls.Add(this.labelOperatorCallMessage);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.Name = "CDialogOperatorCallMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[ Operator Call Message ]";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CDialogOperatorCallMessage_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogOperatorCallMessage_FormClosed);
            this.Load += new System.EventHandler(this.CDialogOperatorCallMessage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOpCall)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Label labelOperatorCallMessage;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.DataGridView dataGridViewOpCall;
		private UiAsset.ImageButton BtnConfirm;
		private UiAsset.ImageButton BtnBuzzerOff;

    }
}