namespace SVI_NFT_R.UI.Dialog
{
    partial class CDialogNotification
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
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.PnlDisplayArea = new System.Windows.Forms.Panel();
            this.PnlLayout = new System.Windows.Forms.TableLayoutPanel();
            this.ImgDisplay = new Cyotek.Windows.Forms.ImageBox();
            this.LblContent = new System.Windows.Forms.Label();
            this.BtnVisibleElapsed = new UiAsset.SpeedButton();
            this.BtnTitle = new UiAsset.SpeedButton();
            this.PnlDisplayArea.SuspendLayout();
            this.PnlLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // PnlDisplayArea
            // 
            this.PnlDisplayArea.BackColor = System.Drawing.Color.White;
            this.PnlDisplayArea.Controls.Add(this.PnlLayout);
            this.PnlDisplayArea.Controls.Add(this.BtnVisibleElapsed);
            this.PnlDisplayArea.Controls.Add(this.BtnTitle);
            this.PnlDisplayArea.Location = new System.Drawing.Point(36, 28);
            this.PnlDisplayArea.Name = "PnlDisplayArea";
            this.PnlDisplayArea.Size = new System.Drawing.Size(396, 172);
            this.PnlDisplayArea.TabIndex = 31;
            // 
            // PnlLayout
            // 
            this.PnlLayout.ColumnCount = 5;
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.330333F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.00751F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.330333F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.0015F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.330333F));
            this.PnlLayout.Controls.Add(this.ImgDisplay, 3, 1);
            this.PnlLayout.Controls.Add(this.LblContent, 1, 1);
            this.PnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlLayout.Location = new System.Drawing.Point(0, 28);
            this.PnlLayout.Name = "PnlLayout";
            this.PnlLayout.RowCount = 3;
            this.PnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.PnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.PnlLayout.Size = new System.Drawing.Size(396, 116);
            this.PnlLayout.TabIndex = 25;
            // 
            // ImgDisplay
            // 
            this.ImgDisplay.AllowFreePan = false;
            this.ImgDisplay.AllowUnfocusedMouseWheel = true;
            this.ImgDisplay.AllowZoom = false;
            this.ImgDisplay.PanMode = Cyotek.Windows.Forms.ImageBoxPanMode.None;
            this.ImgDisplay.AutoScroll = false;
            this.ImgDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ImgDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImgDisplay.GridDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.ImgDisplay.GridScale = Cyotek.Windows.Forms.ImageBoxGridScale.None;
            this.ImgDisplay.Image = global::SVI_NFT_R.Properties.Resources.TitleRun;
            this.ImgDisplay.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.ImgDisplay.LimitSelectionToImage = false;
            this.ImgDisplay.Location = new System.Drawing.Point(326, 15);
            this.ImgDisplay.Name = "ImgDisplay";
            this.ImgDisplay.PanMode = Cyotek.Windows.Forms.ImageBoxPanMode.None;
            this.ImgDisplay.ShortcutsEnabled = false;
            this.ImgDisplay.Size = new System.Drawing.Size(53, 86);
            this.ImgDisplay.TabIndex = 24;
            // 
            // LblContent
            // 
            this.LblContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblContent.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LblContent.Location = new System.Drawing.Point(16, 12);
            this.LblContent.Name = "LblContent";
            this.LblContent.Padding = new System.Windows.Forms.Padding(3);
            this.LblContent.Size = new System.Drawing.Size(291, 92);
            this.LblContent.TabIndex = 25;
            this.LblContent.Text = "CONTENT";
            this.LblContent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnVisibleElapsed
            // 
            this.BtnVisibleElapsed.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BtnVisibleElapsed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVisibleElapsed.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BtnVisibleElapsed.FlatAppearance.BorderSize = 0;
            this.BtnVisibleElapsed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVisibleElapsed.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnVisibleElapsed.Location = new System.Drawing.Point(0, 144);
            this.BtnVisibleElapsed.Name = "BtnVisibleElapsed";
            this.BtnVisibleElapsed.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.BtnVisibleElapsed.Size = new System.Drawing.Size(396, 28);
            this.BtnVisibleElapsed.TabIndex = 26;
            this.BtnVisibleElapsed.TabStop = false;
            this.BtnVisibleElapsed.Text = "000.0 sec";
            this.BtnVisibleElapsed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnVisibleElapsed.UseVisualStyleBackColor = false;
            this.BtnVisibleElapsed.Visible = false;
            // 
            // BtnTitle
            // 
            this.BtnTitle.BackColor = System.Drawing.Color.Green;
            this.BtnTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnTitle.FlatAppearance.BorderSize = 0;
            this.BtnTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitle.Location = new System.Drawing.Point(0, 0);
            this.BtnTitle.Name = "BtnTitle";
            this.BtnTitle.Size = new System.Drawing.Size(396, 28);
            this.BtnTitle.TabIndex = 19;
            this.BtnTitle.TabStop = false;
            this.BtnTitle.Text = "Notification";
            this.BtnTitle.UseVisualStyleBackColor = false;
            this.BtnTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnTitle_MouseDown);
            this.BtnTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BtnTitle_MouseMove);
            this.BtnTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnTitle_MouseUp);
            // 
            // CDialogNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1316, 556);
            this.ControlBox = false;
            this.Controls.Add(this.PnlDisplayArea);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CDialogNotification";
            this.Text = "Notification";
            this.PnlDisplayArea.ResumeLayout(false);
            this.PnlLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel PnlDisplayArea;
        public UiAsset.SpeedButton BtnTitle;
        private Cyotek.Windows.Forms.ImageBox ImgDisplay;
        private System.Windows.Forms.TableLayoutPanel PnlLayout;
        private System.Windows.Forms.Label LblContent;
        public UiAsset.SpeedButton BtnVisibleElapsed;
    }
}