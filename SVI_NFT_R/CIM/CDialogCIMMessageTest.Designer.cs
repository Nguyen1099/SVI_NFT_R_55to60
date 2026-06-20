namespace SVI_NFT_R
{
    partial class CDialogCIMMessageTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDialogCIMMessageTest));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewMessage = new System.Windows.Forms.DataGridView();
            this.BtnExit = new UiAsset.SpeedButton();
            this.BtnSendMessage = new UiAsset.SpeedButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMessage)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // dataGridViewMessage
            // 
            this.dataGridViewMessage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMessage.Location = new System.Drawing.Point(4, 4);
            this.dataGridViewMessage.Name = "dataGridViewMessage";
            this.dataGridViewMessage.RowTemplate.Height = 23;
            this.dataGridViewMessage.Size = new System.Drawing.Size(529, 562);
            this.dataGridViewMessage.TabIndex = 9;
            this.dataGridViewMessage.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMessage_CellDoubleClick);
            // 
            // BtnExit
            // 
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExit.Location = new System.Drawing.Point(272, 572);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(261, 54);
            this.BtnExit.TabIndex = 1;
            this.BtnExit.TabStop = false;
            this.BtnExit.Text = "Exit";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnSendMessage
            // 
            this.BtnSendMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSendMessage.Location = new System.Drawing.Point(4, 572);
            this.BtnSendMessage.Name = "BtnSendMessage";
            this.BtnSendMessage.Size = new System.Drawing.Size(261, 54);
            this.BtnSendMessage.TabIndex = 1;
            this.BtnSendMessage.TabStop = false;
            this.BtnSendMessage.Text = "Send Message";
            this.BtnSendMessage.UseVisualStyleBackColor = true;
            this.BtnSendMessage.Click += new System.EventHandler(this.BtnSendMessage_Click);
            // 
            // CDialogCIMMessageTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(537, 631);
            this.ControlBox = false;
            this.Controls.Add(this.dataGridViewMessage);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnSendMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CDialogCIMMessageTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "[ CIM Message Test ]";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogCIMMessageTest_FormClosed);
            this.Load += new System.EventHandler(this.CDialogCIMMessageTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMessage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UiAsset.SpeedButton BtnSendMessage;
        private UiAsset.SpeedButton BtnExit;
        private System.Windows.Forms.DataGridView dataGridViewMessage;
        private System.Windows.Forms.Timer timer;
    }
}