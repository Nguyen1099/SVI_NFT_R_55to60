namespace SVI_NFT_R
{
    partial class CDialogTerminalMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDialogTerminalMessage));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewTerminal = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnConfirm = new UiAsset.ImageButton();
            this.BtnBuzzerOff = new UiAsset.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTerminal)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // dataGridViewTerminal
            // 
            this.dataGridViewTerminal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTerminal.Location = new System.Drawing.Point(4, 49);
            this.dataGridViewTerminal.Name = "dataGridViewTerminal";
            this.dataGridViewTerminal.RowTemplate.Height = 23;
            this.dataGridViewTerminal.Size = new System.Drawing.Size(1132, 144);
            this.dataGridViewTerminal.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 23F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 42);
            this.label1.TabIndex = 4;
            this.label1.Text = "Terminal Message";
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.BtnConfirm.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnConfirm.ButtonText = "CONFIRM";
            this.BtnConfirm.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnConfirm.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnConfirm.Location = new System.Drawing.Point(1142, 4);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(109, 93);
            this.BtnConfirm.TabIndex = 9;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // BtnBuzzerOff
            // 
            this.BtnBuzzerOff.BackColor = System.Drawing.Color.Transparent;
            this.BtnBuzzerOff.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBuzzerOff.ButtonText = "BUZZER OFF";
            this.BtnBuzzerOff.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnBuzzerOff.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBuzzerOff.Location = new System.Drawing.Point(1142, 100);
            this.BtnBuzzerOff.Name = "BtnBuzzerOff";
            this.BtnBuzzerOff.Size = new System.Drawing.Size(109, 93);
            this.BtnBuzzerOff.TabIndex = 9;
            this.BtnBuzzerOff.Click += new System.EventHandler(this.BtnBuzzerOff_Click);
            // 
            // CDialogTerminalMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1254, 196);
            this.ControlBox = false;
            this.Controls.Add(this.BtnBuzzerOff);
            this.Controls.Add(this.BtnConfirm);
            this.Controls.Add(this.dataGridViewTerminal);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.Name = "CDialogTerminalMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[ Terminal Message ]";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CDialogTerminalMessage_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogTerminalMessage_FormClosed);
            this.Load += new System.EventHandler(this.CDialogTerminalMessage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTerminal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewTerminal;
        private System.Windows.Forms.Timer timer;
        private UiAsset.ImageButton BtnConfirm;
        private UiAsset.ImageButton BtnBuzzerOff;

    }
}