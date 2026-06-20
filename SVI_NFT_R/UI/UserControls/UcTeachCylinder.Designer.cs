namespace SVI_NFT_R.UI.UserControls
{
    partial class UcTeachCylinder
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
            this.pnlLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnDeviceName = new UiAsset.SpeedButton();
            this.btnAction1 = new UiAsset.ImageButton();
            this.btnAction2 = new UiAsset.ImageButton();
            this.pnlLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLayout
            // 
            this.pnlLayout.ColumnCount = 3;
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlLayout.Controls.Add(this.btnDeviceName, 0, 0);
            this.pnlLayout.Controls.Add(this.btnAction1, 1, 0);
            this.pnlLayout.Controls.Add(this.btnAction2, 2, 0);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlLayout.Name = "pnlLayout";
            this.pnlLayout.RowCount = 1;
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.Size = new System.Drawing.Size(427, 46);
            this.pnlLayout.TabIndex = 0;
            // 
            // btnDeviceName
            // 
            this.btnDeviceName.BackColor = System.Drawing.Color.White;
            this.btnDeviceName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeviceName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeviceName.Location = new System.Drawing.Point(0, 0);
            this.btnDeviceName.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.btnDeviceName.Name = "btnDeviceName";
            this.btnDeviceName.Size = new System.Drawing.Size(126, 46);
            this.btnDeviceName.TabIndex = 0;
            this.btnDeviceName.TabStop = false;
            this.btnDeviceName.Text = "DEVICE NAME";
            this.btnDeviceName.UseVisualStyleBackColor = false;
            // 
            // btnAction1
            // 
            this.btnAction1.BackColor = System.Drawing.Color.Transparent;
            this.btnAction1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnAction1.ButtonText = "ACTION1";
            this.btnAction1.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.btnAction1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnAction1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAction1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAction1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAction1.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnAction1.Location = new System.Drawing.Point(128, 0);
            this.btnAction1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnAction1.Name = "btnAction1";
            this.btnAction1.Size = new System.Drawing.Size(148, 46);
            this.btnAction1.TabIndex = 1;
            this.btnAction1.Text = "ACTION1";
            this.btnAction1.UseVisualStyleBackColor = false;
            this.btnAction1.Click += new System.EventHandler(this.btnAction1_Click);
            // 
            // btnAction2
            // 
            this.btnAction2.BackColor = System.Drawing.Color.Transparent;
            this.btnAction2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnAction2.ButtonText = "ACTION2";
            this.btnAction2.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.btnAction2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnAction2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAction2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAction2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAction2.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnAction2.Location = new System.Drawing.Point(278, 0);
            this.btnAction2.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.btnAction2.Name = "btnAction2";
            this.btnAction2.Size = new System.Drawing.Size(149, 46);
            this.btnAction2.TabIndex = 1;
            this.btnAction2.Text = "ACTION2";
            this.btnAction2.UseVisualStyleBackColor = false;
            this.btnAction2.Click += new System.EventHandler(this.btnAction2_Click);
            // 
            // UcTeachCylinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlLayout);
            this.Name = "UcTeachCylinder";
            this.Size = new System.Drawing.Size(427, 46);
            this.pnlLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlLayout;
        private UiAsset.SpeedButton btnDeviceName;
        private UiAsset.ImageButton btnAction1;
        private UiAsset.ImageButton btnAction2;
    }
}
