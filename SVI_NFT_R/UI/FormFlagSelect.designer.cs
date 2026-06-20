namespace UiAsset
{
    partial class FormFlagSelect
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
            this.pnlBoundary = new System.Windows.Forms.Panel();
            this.pnlCancel = new System.Windows.Forms.Panel();
            this.pnlBottomButtonLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new UiAsset.ImageButton();
            this.btnSelect = new UiAsset.ImageButton();
            this.pnlTitleUnderline = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlBottomSpace = new System.Windows.Forms.Panel();
            this.pnlItemSideBar = new System.Windows.Forms.Panel();
            this.btnFirstItem = new UiAsset.ImageButton();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.pnlBoundary.SuspendLayout();
            this.pnlCancel.SuspendLayout();
            this.pnlBottomButtonLayout.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBoundary
            // 
            this.pnlBoundary.Controls.Add(this.pnlCancel);
            this.pnlBoundary.Controls.Add(this.pnlTitleUnderline);
            this.pnlBoundary.Controls.Add(this.pnlContent);
            this.pnlBoundary.Controls.Add(this.pnlTitle);
            this.pnlBoundary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBoundary.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlBoundary.Location = new System.Drawing.Point(0, 0);
            this.pnlBoundary.Name = "pnlBoundary";
            this.pnlBoundary.Size = new System.Drawing.Size(393, 202);
            this.pnlBoundary.TabIndex = 0;
            // 
            // pnlCancel
            // 
            this.pnlCancel.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlCancel.Controls.Add(this.pnlBottomButtonLayout);
            this.pnlCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCancel.Location = new System.Drawing.Point(0, 140);
            this.pnlCancel.Name = "pnlCancel";
            this.pnlCancel.Size = new System.Drawing.Size(393, 62);
            this.pnlCancel.TabIndex = 7;
            // 
            // pnlBottomButtonLayout
            // 
            this.pnlBottomButtonLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBottomButtonLayout.ColumnCount = 3;
            this.pnlBottomButtonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlBottomButtonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.pnlBottomButtonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlBottomButtonLayout.Controls.Add(this.btnCancel, 2, 0);
            this.pnlBottomButtonLayout.Controls.Add(this.btnSelect, 0, 0);
            this.pnlBottomButtonLayout.Location = new System.Drawing.Point(9, 13);
            this.pnlBottomButtonLayout.Name = "pnlBottomButtonLayout";
            this.pnlBottomButtonLayout.RowCount = 1;
            this.pnlBottomButtonLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlBottomButtonLayout.Size = new System.Drawing.Size(375, 44);
            this.pnlBottomButtonLayout.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnCancel.ButtonText = "CANCEL";
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnCancel.Location = new System.Drawing.Point(190, 0);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(185, 44);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.Transparent;
            this.btnSelect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnSelect.ButtonText = "SELECT";
            this.btnSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSelect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSelect.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnSelect.Location = new System.Drawing.Point(0, 0);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(0);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(184, 44);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "SELECT";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // pnlTitleUnderline
            // 
            this.pnlTitleUnderline.BackColor = System.Drawing.Color.Thistle;
            this.pnlTitleUnderline.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleUnderline.Location = new System.Drawing.Point(0, 46);
            this.pnlTitleUnderline.Name = "pnlTitleUnderline";
            this.pnlTitleUnderline.Size = new System.Drawing.Size(393, 1);
            this.pnlTitleUnderline.TabIndex = 6;
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContent.AutoScroll = true;
            this.pnlContent.Controls.Add(this.pnlBottomSpace);
            this.pnlContent.Controls.Add(this.pnlItemSideBar);
            this.pnlContent.Controls.Add(this.btnFirstItem);
            this.pnlContent.Location = new System.Drawing.Point(0, 46);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(393, 95);
            this.pnlContent.TabIndex = 5;
            // 
            // pnlBottomSpace
            // 
            this.pnlBottomSpace.Location = new System.Drawing.Point(0, 0);
            this.pnlBottomSpace.Name = "pnlBottomSpace";
            this.pnlBottomSpace.Size = new System.Drawing.Size(14, 25);
            this.pnlBottomSpace.TabIndex = 3;
            // 
            // pnlItemSideBar
            // 
            this.pnlItemSideBar.BackColor = System.Drawing.Color.Thistle;
            this.pnlItemSideBar.Location = new System.Drawing.Point(9, 25);
            this.pnlItemSideBar.Name = "pnlItemSideBar";
            this.pnlItemSideBar.Size = new System.Drawing.Size(1, 44);
            this.pnlItemSideBar.TabIndex = 2;
            // 
            // btnFirstItem
            // 
            this.btnFirstItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFirstItem.BackColor = System.Drawing.Color.Transparent;
            this.btnFirstItem.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnFirstItem.ButtonText = "FIRST ITEM";
            this.btnFirstItem.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.RightTop;
            this.btnFirstItem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFirstItem.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnFirstItem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFirstItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFirstItem.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnFirstItem.Location = new System.Drawing.Point(13, 25);
            this.btnFirstItem.Name = "btnFirstItem";
            this.btnFirstItem.Size = new System.Drawing.Size(371, 44);
            this.btnFirstItem.TabIndex = 1;
            this.btnFirstItem.Text = "FIRST ITEM";
            this.btnFirstItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFirstItem.UseVisualStyleBackColor = false;
            // 
            // pnlTitle
            // 
            this.pnlTitle.BackColor = System.Drawing.Color.MediumPurple;
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(393, 46);
            this.pnlTitle.TabIndex = 4;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(223, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "SELECT FLAG OPTION";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // FormFlagSelect
            // 
            this.AcceptButton = this.btnSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(393, 202);
            this.ControlBox = false;
            this.Controls.Add(this.pnlBoundary);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(1280, 1000);
            this.Name = "FormFlagSelect";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormFlagSelect_KeyUp);
            this.pnlBoundary.ResumeLayout(false);
            this.pnlCancel.ResumeLayout(false);
            this.pnlBottomButtonLayout.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBoundary;
        private System.Windows.Forms.Panel pnlCancel;
        private System.Windows.Forms.Panel pnlTitleUnderline;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlTitle;
        private UiAsset.ImageButton btnCancel;
        private System.Windows.Forms.Label lblTitle;
        private UiAsset.ImageButton btnFirstItem;
        private System.Windows.Forms.Panel pnlItemSideBar;
        private System.Windows.Forms.Panel pnlBottomSpace;
        private System.Windows.Forms.TableLayoutPanel pnlBottomButtonLayout;
        private ImageButton btnSelect;
        private System.Windows.Forms.Timer timer;
    }
}