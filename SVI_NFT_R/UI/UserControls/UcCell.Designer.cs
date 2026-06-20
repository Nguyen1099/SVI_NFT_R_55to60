namespace SVI_NFT_R.UI.UserControls
{
    partial class UcCell
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
            this.BtnIndicator = new UiAsset.SpeedButton();
            this.pnlCellData = new System.Windows.Forms.Panel();
            this.BtnCellData = new UiAsset.SpeedButton();
            this.pnlLayout.SuspendLayout();
            this.pnlCellData.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLayout
            // 
            this.pnlLayout.ColumnCount = 5;
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.pnlLayout.Controls.Add(this.BtnIndicator, 0, 0);
            this.pnlLayout.Controls.Add(this.pnlCellData, 2, 0);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlLayout.Margin = new System.Windows.Forms.Padding(6);
            this.pnlLayout.Name = "pnlLayout";
            this.pnlLayout.RowCount = 5;
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.pnlLayout.Size = new System.Drawing.Size(119, 60);
            this.pnlLayout.TabIndex = 0;
            // 
            // BtnIndicator
            // 
            this.BtnIndicator.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BtnIndicator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnIndicator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnIndicator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnIndicator.Location = new System.Drawing.Point(0, 0);
            this.BtnIndicator.Margin = new System.Windows.Forms.Padding(0);
            this.BtnIndicator.Name = "BtnIndicator";
            this.pnlLayout.SetRowSpan(this.BtnIndicator, 5);
            this.BtnIndicator.Size = new System.Drawing.Size(10, 60);
            this.BtnIndicator.TabIndex = 0;
            this.BtnIndicator.TabStop = false;
            this.BtnIndicator.UseVisualStyleBackColor = false;
            // 
            // pnlCellData
            // 
            this.pnlCellData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLayout.SetColumnSpan(this.pnlCellData, 3);
            this.pnlCellData.Controls.Add(this.BtnCellData);
            this.pnlCellData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCellData.Location = new System.Drawing.Point(15, 0);
            this.pnlCellData.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCellData.Name = "pnlCellData";
            this.pnlLayout.SetRowSpan(this.pnlCellData, 5);
            this.pnlCellData.Size = new System.Drawing.Size(104, 60);
            this.pnlCellData.TabIndex = 1;
            // 
            // BtnCellData
            // 
            this.BtnCellData.BackColor = System.Drawing.Color.White;
            this.BtnCellData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCellData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnCellData.FlatAppearance.BorderSize = 0;
            this.BtnCellData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCellData.Location = new System.Drawing.Point(0, 0);
            this.BtnCellData.Margin = new System.Windows.Forms.Padding(0);
            this.BtnCellData.Name = "BtnCellData";
            this.BtnCellData.Size = new System.Drawing.Size(102, 58);
            this.BtnCellData.TabIndex = 0;
            this.BtnCellData.TabStop = false;
            this.BtnCellData.Text = "1";
            this.BtnCellData.UseVisualStyleBackColor = false;
            this.BtnCellData.Paint += new System.Windows.Forms.PaintEventHandler(this.BtnCellData_Paint);
            // 
            // UcCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlLayout);
            this.Name = "UcCell";
            this.Size = new System.Drawing.Size(119, 60);
            this.pnlLayout.ResumeLayout(false);
            this.pnlCellData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlLayout;
        private System.Windows.Forms.Panel pnlCellData;
        public UiAsset.SpeedButton BtnCellData;
        public UiAsset.SpeedButton BtnIndicator;
    }
}
