namespace SVI_NFT_R.UI.UserControls
{
    partial class UcDoorStatus
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlMessage = new System.Windows.Forms.Panel();
            this.btnMessage = new UiAsset.SpeedButton();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.btnTitle = new UiAsset.SpeedButton();
            this.pnlMessage.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMessage
            // 
            this.pnlMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMessage.Controls.Add(this.btnMessage);
            this.pnlMessage.Location = new System.Drawing.Point(3, 47);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Size = new System.Drawing.Size(136, 75);
            this.pnlMessage.TabIndex = 1;
            // 
            // btnMessage
            // 
            this.btnMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMessage.FlatAppearance.BorderSize = 0;
            this.btnMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMessage.Location = new System.Drawing.Point(0, 0);
            this.btnMessage.Margin = new System.Windows.Forms.Padding(2);
            this.btnMessage.Name = "btnMessage";
            this.btnMessage.Size = new System.Drawing.Size(134, 73);
            this.btnMessage.TabIndex = 2;
            this.btnMessage.TabStop = false;
            this.btnMessage.Text = "MESSAGE";
            this.btnMessage.UseVisualStyleBackColor = true;
            // 
            // pnlTitle
            // 
            this.pnlTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTitle.Controls.Add(this.btnTitle);
            this.pnlTitle.Location = new System.Drawing.Point(3, 3);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(136, 41);
            this.pnlTitle.TabIndex = 2;
            // 
            // btnTitle
            // 
            this.btnTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTitle.FlatAppearance.BorderSize = 0;
            this.btnTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTitle.Location = new System.Drawing.Point(0, 0);
            this.btnTitle.Margin = new System.Windows.Forms.Padding(2);
            this.btnTitle.Name = "btnTitle";
            this.btnTitle.Size = new System.Drawing.Size(134, 39);
            this.btnTitle.TabIndex = 1;
            this.btnTitle.TabStop = false;
            this.btnTitle.Text = "TITLE";
            this.btnTitle.UseVisualStyleBackColor = true;
            // 
            // UcDoorStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.pnlTitle);
            this.Controls.Add(this.pnlMessage);
            this.Name = "UcDoorStatus";
            this.Size = new System.Drawing.Size(142, 125);
            this.pnlMessage.ResumeLayout(false);
            this.pnlTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlMessage;
        private UiAsset.SpeedButton btnMessage;
        private System.Windows.Forms.Panel pnlTitle;
        private UiAsset.SpeedButton btnTitle;
    }
}
