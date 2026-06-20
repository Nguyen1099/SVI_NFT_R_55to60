using System;

namespace SVI_NFT_R
{
    partial class CFormSetupAlignVision
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
            this.BtnTitle = new UiAsset.SpeedButton();
            this.BtnSave = new UiAsset.ImageButton();
            this.ucSettingAlign1 = new SVI_NFT_R.UI.UserControls.UcSettingAlign();
            this.pnlLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.ucSettingAlign2 = new SVI_NFT_R.UI.UserControls.UcSettingAlign();
            this.ucSettingAlign3 = new SVI_NFT_R.UI.UserControls.UcSettingAlign();
            this.ucSettingAlign4 = new SVI_NFT_R.UI.UserControls.UcSettingAlign();
            this.BtnPagingFirst = new UiAsset.ImageButton();
            this.BtnPaging = new UiAsset.SpeedButton();
            this.BtnPagingPrevious = new UiAsset.ImageButton();
            this.PnlPaging = new System.Windows.Forms.TableLayoutPanel();
            this.BtnPagingNext = new UiAsset.ImageButton();
            this.BtnPagingLast = new UiAsset.ImageButton();
            this.pnlLayout.SuspendLayout();
            this.PnlPaging.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // BtnTitle
            // 
            this.BtnTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitle.Location = new System.Drawing.Point(12, 12);
            this.BtnTitle.Name = "BtnTitle";
            this.BtnTitle.Size = new System.Drawing.Size(1252, 46);
            this.BtnTitle.TabIndex = 8;
            this.BtnTitle.TabStop = false;
            this.BtnTitle.Text = "ALIGN VISION";
            this.BtnTitle.UseVisualStyleBackColor = true;
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.Transparent;
            this.BtnSave.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSave.ButtonText = "SAVE";
            this.BtnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSave.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSave.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSave.Location = new System.Drawing.Point(1054, 686);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(210, 46);
            this.BtnSave.TabIndex = 57;
            this.BtnSave.Text = "SAVE";
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // ucSettingAlign1
            // 
            this.ucSettingAlign1.Location = new System.Drawing.Point(0, 0);
            this.ucSettingAlign1.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.ucSettingAlign1.Name = "ucSettingAlign1";
            this.ucSettingAlign1.Size = new System.Drawing.Size(308, 534);
            this.ucSettingAlign1.TabIndex = 60;
            // 
            // pnlLayout
            // 
            this.pnlLayout.Controls.Add(this.ucSettingAlign1);
            this.pnlLayout.Controls.Add(this.ucSettingAlign2);
            this.pnlLayout.Controls.Add(this.ucSettingAlign3);
            this.pnlLayout.Controls.Add(this.ucSettingAlign4);
            this.pnlLayout.Location = new System.Drawing.Point(12, 64);
            this.pnlLayout.Name = "pnlLayout";
            this.pnlLayout.Size = new System.Drawing.Size(1252, 616);
            this.pnlLayout.TabIndex = 61;
            // 
            // ucSettingAlign2
            // 
            this.ucSettingAlign2.Location = new System.Drawing.Point(314, 0);
            this.ucSettingAlign2.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.ucSettingAlign2.Name = "ucSettingAlign2";
            this.ucSettingAlign2.Size = new System.Drawing.Size(308, 534);
            this.ucSettingAlign2.TabIndex = 61;
            // 
            // ucSettingAlign3
            // 
            this.ucSettingAlign3.Location = new System.Drawing.Point(628, 0);
            this.ucSettingAlign3.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.ucSettingAlign3.Name = "ucSettingAlign3";
            this.ucSettingAlign3.Size = new System.Drawing.Size(308, 534);
            this.ucSettingAlign3.TabIndex = 62;
            // 
            // ucSettingAlign4
            // 
            this.ucSettingAlign4.Location = new System.Drawing.Point(942, 0);
            this.ucSettingAlign4.Margin = new System.Windows.Forms.Padding(0);
            this.ucSettingAlign4.Name = "ucSettingAlign4";
            this.ucSettingAlign4.Size = new System.Drawing.Size(308, 534);
            this.ucSettingAlign4.TabIndex = 63;
            // 
            // BtnPagingFirst
            // 
            this.BtnPagingFirst.BackColor = System.Drawing.Color.Transparent;
            this.BtnPagingFirst.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPagingFirst.ButtonText = "│◁";
            this.BtnPagingFirst.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnPagingFirst.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPagingFirst.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPagingFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnPagingFirst.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnPagingFirst.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnPagingFirst.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPagingFirst.Location = new System.Drawing.Point(0, 0);
            this.BtnPagingFirst.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPagingFirst.Name = "BtnPagingFirst";
            this.BtnPagingFirst.Size = new System.Drawing.Size(60, 46);
            this.BtnPagingFirst.TabIndex = 62;
            this.BtnPagingFirst.Text = "│◁";
            this.BtnPagingFirst.UseVisualStyleBackColor = false;
            this.BtnPagingFirst.Click += new System.EventHandler(this.BtnPagingFirst_Click);
            // 
            // BtnPaging
            // 
            this.BtnPaging.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPaging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnPaging.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPaging.Location = new System.Drawing.Point(120, 0);
            this.BtnPaging.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPaging.Name = "BtnPaging";
            this.BtnPaging.Size = new System.Drawing.Size(123, 46);
            this.BtnPaging.TabIndex = 63;
            this.BtnPaging.TabStop = false;
            this.BtnPaging.Text = "( 1 / 1 )";
            this.BtnPaging.UseVisualStyleBackColor = true;
            // 
            // BtnPagingPrevious
            // 
            this.BtnPagingPrevious.BackColor = System.Drawing.Color.Transparent;
            this.BtnPagingPrevious.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPagingPrevious.ButtonText = "◁";
            this.BtnPagingPrevious.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnPagingPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPagingPrevious.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPagingPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnPagingPrevious.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnPagingPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnPagingPrevious.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPagingPrevious.Location = new System.Drawing.Point(60, 0);
            this.BtnPagingPrevious.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPagingPrevious.Name = "BtnPagingPrevious";
            this.BtnPagingPrevious.Size = new System.Drawing.Size(60, 46);
            this.BtnPagingPrevious.TabIndex = 62;
            this.BtnPagingPrevious.Text = "◁";
            this.BtnPagingPrevious.UseVisualStyleBackColor = false;
            this.BtnPagingPrevious.Click += new System.EventHandler(this.BtnPagingPrevious_Click);
            // 
            // PnlPaging
            // 
            this.PnlPaging.ColumnCount = 5;
            this.PnlPaging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.PnlPaging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.PnlPaging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlPaging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.PnlPaging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.PnlPaging.Controls.Add(this.BtnPagingFirst, 0, 0);
            this.PnlPaging.Controls.Add(this.BtnPaging, 2, 0);
            this.PnlPaging.Controls.Add(this.BtnPagingPrevious, 1, 0);
            this.PnlPaging.Controls.Add(this.BtnPagingNext, 3, 0);
            this.PnlPaging.Controls.Add(this.BtnPagingLast, 4, 0);
            this.PnlPaging.Location = new System.Drawing.Point(12, 687);
            this.PnlPaging.Name = "PnlPaging";
            this.PnlPaging.RowCount = 1;
            this.PnlPaging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlPaging.Size = new System.Drawing.Size(363, 46);
            this.PnlPaging.TabIndex = 64;
            // 
            // BtnPagingNext
            // 
            this.BtnPagingNext.BackColor = System.Drawing.Color.Transparent;
            this.BtnPagingNext.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPagingNext.ButtonText = "▷";
            this.BtnPagingNext.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnPagingNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPagingNext.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPagingNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnPagingNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnPagingNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnPagingNext.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPagingNext.Location = new System.Drawing.Point(243, 0);
            this.BtnPagingNext.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPagingNext.Name = "BtnPagingNext";
            this.BtnPagingNext.Size = new System.Drawing.Size(60, 46);
            this.BtnPagingNext.TabIndex = 62;
            this.BtnPagingNext.Text = "▷";
            this.BtnPagingNext.UseVisualStyleBackColor = false;
            this.BtnPagingNext.Click += new System.EventHandler(this.BtnPagingNext_Click);
            // 
            // BtnPagingLast
            // 
            this.BtnPagingLast.BackColor = System.Drawing.Color.Transparent;
            this.BtnPagingLast.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPagingLast.ButtonText = "▷│";
            this.BtnPagingLast.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnPagingLast.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPagingLast.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPagingLast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnPagingLast.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnPagingLast.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnPagingLast.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPagingLast.Location = new System.Drawing.Point(303, 0);
            this.BtnPagingLast.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPagingLast.Name = "BtnPagingLast";
            this.BtnPagingLast.Size = new System.Drawing.Size(60, 46);
            this.BtnPagingLast.TabIndex = 62;
            this.BtnPagingLast.Text = "▷│";
            this.BtnPagingLast.UseVisualStyleBackColor = false;
            this.BtnPagingLast.Click += new System.EventHandler(this.BtnPagingLast_Click);
            // 
            // CFormSetupAlignVision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.Controls.Add(this.PnlPaging);
            this.Controls.Add(this.pnlLayout);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormSetupAlignVision";
            this.Text = "CFormSetupDevice";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormSetupDevice_FormClosed);
            this.Load += new System.EventHandler(this.CFormSetupDevice_Load);
            this.pnlLayout.ResumeLayout(false);
            this.PnlPaging.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Timer timer;
        private UiAsset.SpeedButton BtnTitle;
        private UiAsset.ImageButton BtnSave;
        private UI.UserControls.UcSettingAlign ucSettingAlign1;
        private System.Windows.Forms.FlowLayoutPanel pnlLayout;
        private UI.UserControls.UcSettingAlign ucSettingAlign2;
        private UI.UserControls.UcSettingAlign ucSettingAlign3;
        private UiAsset.ImageButton BtnPagingFirst;
        private UiAsset.SpeedButton BtnPaging;
        private UiAsset.ImageButton BtnPagingPrevious;
        private System.Windows.Forms.TableLayoutPanel PnlPaging;
        private UiAsset.ImageButton BtnPagingNext;
        private UiAsset.ImageButton BtnPagingLast;
        private UI.UserControls.UcSettingAlign ucSettingAlign4;
    }
}