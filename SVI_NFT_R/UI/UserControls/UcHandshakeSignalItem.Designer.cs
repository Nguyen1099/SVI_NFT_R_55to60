namespace SVI_NFT_R.UI.UserControls
{
    partial class UcHandshakeSignalItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PnlLayout = new System.Windows.Forms.TableLayoutPanel();
            this.LblTitle = new System.Windows.Forms.Label();
            this.BtnIndicator1 = new UiAsset.SpeedButton();
            this.BtnIndicator2 = new UiAsset.SpeedButton();
            this.PnlLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlLayout
            // 
            this.PnlLayout.ColumnCount = 3;
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.PnlLayout.Controls.Add(this.LblTitle, 1, 0);
            this.PnlLayout.Controls.Add(this.BtnIndicator1, 0, 0);
            this.PnlLayout.Controls.Add(this.BtnIndicator2, 2, 0);
            this.PnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlLayout.Location = new System.Drawing.Point(0, 0);
            this.PnlLayout.Margin = new System.Windows.Forms.Padding(0);
            this.PnlLayout.Name = "PnlLayout";
            this.PnlLayout.RowCount = 1;
            this.PnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.PnlLayout.Size = new System.Drawing.Size(142, 24);
            this.PnlLayout.TabIndex = 0;
            // 
            // LblTitle
            // 
            this.LblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblTitle.Location = new System.Drawing.Point(20, 0);
            this.LblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(102, 24);
            this.LblTitle.TabIndex = 0;
            this.LblTitle.Text = "label1";
            this.LblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnIndicator1
            // 
            this.BtnIndicator1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnIndicator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnIndicator1.Enabled = false;
            this.BtnIndicator1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnIndicator1.ForeColor = System.Drawing.Color.LavenderBlush;
            this.BtnIndicator1.Location = new System.Drawing.Point(0, 0);
            this.BtnIndicator1.Margin = new System.Windows.Forms.Padding(0);
            this.BtnIndicator1.Name = "BtnIndicator1";
            this.BtnIndicator1.Size = new System.Drawing.Size(20, 24);
            this.BtnIndicator1.TabIndex = 1;
            this.BtnIndicator1.TabStop = false;
            this.BtnIndicator1.UseVisualStyleBackColor = false;
            // 
            // BtnIndicator2
            // 
            this.BtnIndicator2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnIndicator2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnIndicator2.Enabled = false;
            this.BtnIndicator2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnIndicator2.ForeColor = System.Drawing.Color.LavenderBlush;
            this.BtnIndicator2.Location = new System.Drawing.Point(122, 0);
            this.BtnIndicator2.Margin = new System.Windows.Forms.Padding(0);
            this.BtnIndicator2.Name = "BtnIndicator2";
            this.BtnIndicator2.Size = new System.Drawing.Size(20, 24);
            this.BtnIndicator2.TabIndex = 1;
            this.BtnIndicator2.TabStop = false;
            this.BtnIndicator2.UseVisualStyleBackColor = false;
            // 
            // UcHandshakeSignalItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.PnlLayout);
            this.Name = "UcHandshakeSignalItem";
            this.Size = new System.Drawing.Size(142, 24);
            this.PnlLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TableLayoutPanel PnlLayout;
        public System.Windows.Forms.Label LblTitle;
        public UiAsset.SpeedButton BtnIndicator1;
        public UiAsset.SpeedButton BtnIndicator2;
    }
}
