namespace SVI_NFT_R.UI.UserControls
{
    partial class UcDoorControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcDoorControl));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnTitle = new UiAsset.SpeedButton();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.BtnDoorOpen = new UiAsset.ImageButton();
            this.BtnDoorLock = new UiAsset.ImageButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.BtnTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.BtnDoorOpen, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.BtnDoorLock, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(289, 105);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // BtnTitle
            // 
            this.BtnTitle.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.BtnTitle, 2);
            this.BtnTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnTitle.ImageIndex = 0;
            this.BtnTitle.ImageList = this.imageList;
            this.BtnTitle.Location = new System.Drawing.Point(2, 2);
            this.BtnTitle.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTitle.Name = "BtnTitle";
            this.BtnTitle.Size = new System.Drawing.Size(285, 41);
            this.BtnTitle.TabIndex = 0;
            this.BtnTitle.TabStop = false;
            this.BtnTitle.Text = "TITLE";
            this.BtnTitle.UseVisualStyleBackColor = false;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Key.png");
            // 
            // BtnDoorOpen
            // 
            this.BtnDoorOpen.BackColor = System.Drawing.Color.Transparent;
            this.BtnDoorOpen.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnDoorOpen.ButtonText = "";
            this.BtnDoorOpen.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnDoorOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnDoorOpen.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnDoorOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnDoorOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnDoorOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnDoorOpen.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnDoorOpen.Image = ((System.Drawing.Image)(resources.GetObject("BtnDoorOpen.Image")));
            this.BtnDoorOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnDoorOpen.Location = new System.Drawing.Point(2, 47);
            this.BtnDoorOpen.Margin = new System.Windows.Forms.Padding(2);
            this.BtnDoorOpen.Name = "BtnDoorOpen";
            this.BtnDoorOpen.Size = new System.Drawing.Size(140, 56);
            this.BtnDoorOpen.TabIndex = 1;
            this.BtnDoorOpen.UseVisualStyleBackColor = false;
            // 
            // BtnDoorLock
            // 
            this.BtnDoorLock.BackColor = System.Drawing.Color.Transparent;
            this.BtnDoorLock.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnDoorLock.ButtonText = "";
            this.BtnDoorLock.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnDoorLock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnDoorLock.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnDoorLock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnDoorLock.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnDoorLock.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnDoorLock.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnDoorLock.Image = ((System.Drawing.Image)(resources.GetObject("BtnDoorLock.Image")));
            this.BtnDoorLock.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnDoorLock.Location = new System.Drawing.Point(146, 47);
            this.BtnDoorLock.Margin = new System.Windows.Forms.Padding(2);
            this.BtnDoorLock.Name = "BtnDoorLock";
            this.BtnDoorLock.Size = new System.Drawing.Size(141, 56);
            this.BtnDoorLock.TabIndex = 2;
            this.BtnDoorLock.UseVisualStyleBackColor = false;
            // 
            // UcDoorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UcDoorControl";
            this.Size = new System.Drawing.Size(289, 105);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal UiAsset.SpeedButton BtnTitle;
        internal UiAsset.ImageButton BtnDoorOpen;
        internal UiAsset.ImageButton BtnDoorLock;
        private System.Windows.Forms.ImageList imageList;
    }
}
