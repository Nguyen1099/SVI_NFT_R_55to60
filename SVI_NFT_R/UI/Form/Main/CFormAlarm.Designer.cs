namespace SVI_NFT_R
{
    partial class CFormAlarm
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
            this.GridViewAlarmList = new System.Windows.Forms.DataGridView();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnTitleAlarmTime = new UiAsset.SpeedButton();
            this.BtnTitleAlarmPart = new UiAsset.SpeedButton();
            this.BtnTitleAlarmText = new UiAsset.SpeedButton();
            this.BtnTitleAlarmDescription = new UiAsset.SpeedButton();
            this.BtnAlarmReset = new UiAsset.ImageButton();
            this.BtnAlarmAllReset = new UiAsset.ImageButton();
            this.BtnBuzzerOff = new UiAsset.ImageButton();
            this.BtnAlarmTime = new UiAsset.SpeedButton();
            this.BtnAlarmPart = new UiAsset.SpeedButton();
            this.BtnAlarmText = new UiAsset.SpeedButton();
            this.pictureBoxEquipmentImage = new System.Windows.Forms.PictureBox();
            this.pnlAlarmDescription = new System.Windows.Forms.Panel();
            this.txtAlarmDescription = new System.Windows.Forms.RichTextBox();
            this.BtnAlarmSop = new UiAsset.ImageButton();
            this.BtnAlarmImage = new UiAsset.ImageButton();
            this.pdfViewer1 = new PdfiumViewer.PdfViewer();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewAlarmList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEquipmentImage)).BeginInit();
            this.pnlAlarmDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // GridViewAlarmList
            // 
            this.GridViewAlarmList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewAlarmList.Location = new System.Drawing.Point(641, 12);
            this.GridViewAlarmList.Name = "GridViewAlarmList";
            this.GridViewAlarmList.RowTemplate.Height = 23;
            this.GridViewAlarmList.Size = new System.Drawing.Size(623, 380);
            this.GridViewAlarmList.TabIndex = 1;
            this.GridViewAlarmList.CurrentCellChanged += new System.EventHandler(this.GridViewAlarmList_CurrentCellChanged);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnTitleAlarmTime
            // 
            this.BtnTitleAlarmTime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleAlarmTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleAlarmTime.Location = new System.Drawing.Point(641, 398);
            this.BtnTitleAlarmTime.Name = "BtnTitleAlarmTime";
            this.BtnTitleAlarmTime.Size = new System.Drawing.Size(139, 46);
            this.BtnTitleAlarmTime.TabIndex = 3;
            this.BtnTitleAlarmTime.TabStop = false;
            this.BtnTitleAlarmTime.Text = "OCCURRED TIME";
            this.BtnTitleAlarmTime.UseVisualStyleBackColor = true;
            // 
            // BtnTitleAlarmPart
            // 
            this.BtnTitleAlarmPart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleAlarmPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleAlarmPart.Location = new System.Drawing.Point(641, 450);
            this.BtnTitleAlarmPart.Name = "BtnTitleAlarmPart";
            this.BtnTitleAlarmPart.Size = new System.Drawing.Size(139, 46);
            this.BtnTitleAlarmPart.TabIndex = 3;
            this.BtnTitleAlarmPart.TabStop = false;
            this.BtnTitleAlarmPart.Text = "PART";
            this.BtnTitleAlarmPart.UseVisualStyleBackColor = true;
            // 
            // BtnTitleAlarmText
            // 
            this.BtnTitleAlarmText.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleAlarmText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleAlarmText.Location = new System.Drawing.Point(641, 502);
            this.BtnTitleAlarmText.Name = "BtnTitleAlarmText";
            this.BtnTitleAlarmText.Size = new System.Drawing.Size(139, 46);
            this.BtnTitleAlarmText.TabIndex = 3;
            this.BtnTitleAlarmText.TabStop = false;
            this.BtnTitleAlarmText.Text = "OCCURRED REASON";
            this.BtnTitleAlarmText.UseVisualStyleBackColor = true;
            // 
            // BtnTitleAlarmDescription
            // 
            this.BtnTitleAlarmDescription.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleAlarmDescription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleAlarmDescription.Location = new System.Drawing.Point(641, 554);
            this.BtnTitleAlarmDescription.Name = "BtnTitleAlarmDescription";
            this.BtnTitleAlarmDescription.Size = new System.Drawing.Size(139, 98);
            this.BtnTitleAlarmDescription.TabIndex = 3;
            this.BtnTitleAlarmDescription.TabStop = false;
            this.BtnTitleAlarmDescription.Text = "SOLUTION";
            this.BtnTitleAlarmDescription.UseVisualStyleBackColor = true;
            // 
            // BtnAlarmReset
            // 
            this.BtnAlarmReset.BackColor = System.Drawing.Color.Transparent;
            this.BtnAlarmReset.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnAlarmReset.ButtonText = "RESET";
            this.BtnAlarmReset.CornerRadius = 20;
            this.BtnAlarmReset.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnAlarmReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAlarmReset.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnAlarmReset.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnAlarmReset.Location = new System.Drawing.Point(641, 658);
            this.BtnAlarmReset.Name = "BtnAlarmReset";
            this.BtnAlarmReset.Size = new System.Drawing.Size(204, 122);
            this.BtnAlarmReset.TabIndex = 3;
            this.BtnAlarmReset.Text = "RESET";
            this.BtnAlarmReset.UseVisualStyleBackColor = false;
            this.BtnAlarmReset.Click += new System.EventHandler(this.BtnAlarmReset_Click);
            // 
            // BtnAlarmAllReset
            // 
            this.BtnAlarmAllReset.BackColor = System.Drawing.Color.Transparent;
            this.BtnAlarmAllReset.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnAlarmAllReset.ButtonText = "ALL RESET";
            this.BtnAlarmAllReset.CornerRadius = 20;
            this.BtnAlarmAllReset.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnAlarmAllReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAlarmAllReset.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnAlarmAllReset.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnAlarmAllReset.Location = new System.Drawing.Point(851, 658);
            this.BtnAlarmAllReset.Name = "BtnAlarmAllReset";
            this.BtnAlarmAllReset.Size = new System.Drawing.Size(204, 122);
            this.BtnAlarmAllReset.TabIndex = 3;
            this.BtnAlarmAllReset.Text = "ALL RESET";
            this.BtnAlarmAllReset.UseVisualStyleBackColor = false;
            this.BtnAlarmAllReset.Click += new System.EventHandler(this.BtnAlarmAllReset_Click);
            // 
            // BtnBuzzerOff
            // 
            this.BtnBuzzerOff.BackColor = System.Drawing.Color.Transparent;
            this.BtnBuzzerOff.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBuzzerOff.ButtonText = "BUZZER OFF";
            this.BtnBuzzerOff.CornerRadius = 20;
            this.BtnBuzzerOff.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBuzzerOff.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnBuzzerOff.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBuzzerOff.Location = new System.Drawing.Point(1060, 658);
            this.BtnBuzzerOff.Name = "BtnBuzzerOff";
            this.BtnBuzzerOff.Size = new System.Drawing.Size(204, 122);
            this.BtnBuzzerOff.TabIndex = 3;
            this.BtnBuzzerOff.Text = "BUZZER OFF";
            this.BtnBuzzerOff.UseVisualStyleBackColor = false;
            this.BtnBuzzerOff.Click += new System.EventHandler(this.BtnBuzzerOff_Click);
            // 
            // BtnAlarmTime
            // 
            this.BtnAlarmTime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAlarmTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAlarmTime.Location = new System.Drawing.Point(786, 398);
            this.BtnAlarmTime.Name = "BtnAlarmTime";
            this.BtnAlarmTime.Size = new System.Drawing.Size(478, 46);
            this.BtnAlarmTime.TabIndex = 3;
            this.BtnAlarmTime.TabStop = false;
            this.BtnAlarmTime.UseVisualStyleBackColor = true;
            // 
            // BtnAlarmPart
            // 
            this.BtnAlarmPart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAlarmPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAlarmPart.Location = new System.Drawing.Point(786, 450);
            this.BtnAlarmPart.Name = "BtnAlarmPart";
            this.BtnAlarmPart.Size = new System.Drawing.Size(478, 46);
            this.BtnAlarmPart.TabIndex = 3;
            this.BtnAlarmPart.TabStop = false;
            this.BtnAlarmPart.UseVisualStyleBackColor = true;
            // 
            // BtnAlarmText
            // 
            this.BtnAlarmText.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAlarmText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAlarmText.Location = new System.Drawing.Point(786, 502);
            this.BtnAlarmText.Name = "BtnAlarmText";
            this.BtnAlarmText.Size = new System.Drawing.Size(478, 46);
            this.BtnAlarmText.TabIndex = 3;
            this.BtnAlarmText.TabStop = false;
            this.BtnAlarmText.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.BtnAlarmText.UseVisualStyleBackColor = true;
            // 
            // pictureBoxEquipmentImage
            // 
            this.pictureBoxEquipmentImage.BackgroundImage = global::SVI_NFT_R.Properties.Resources.TOP_VIEW_FLOW;
            this.pictureBoxEquipmentImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxEquipmentImage.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxEquipmentImage.Name = "pictureBoxEquipmentImage";
            this.pictureBoxEquipmentImage.Size = new System.Drawing.Size(623, 768);
            this.pictureBoxEquipmentImage.TabIndex = 4;
            this.pictureBoxEquipmentImage.TabStop = false;
            this.pictureBoxEquipmentImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxEquipmentImage_MouseDown);
            this.pictureBoxEquipmentImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxEquipmentImage_MouseUp);
            // 
            // pnlAlarmDescription
            // 
            this.pnlAlarmDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAlarmDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAlarmDescription.Controls.Add(this.txtAlarmDescription);
            this.pnlAlarmDescription.Location = new System.Drawing.Point(786, 554);
            this.pnlAlarmDescription.Name = "pnlAlarmDescription";
            this.pnlAlarmDescription.Size = new System.Drawing.Size(478, 98);
            this.pnlAlarmDescription.TabIndex = 19;
            // 
            // txtAlarmDescription
            // 
            this.txtAlarmDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAlarmDescription.Location = new System.Drawing.Point(6, 6);
            this.txtAlarmDescription.Name = "txtAlarmDescription";
            this.txtAlarmDescription.ReadOnly = true;
            this.txtAlarmDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtAlarmDescription.ShortcutsEnabled = false;
            this.txtAlarmDescription.Size = new System.Drawing.Size(464, 84);
            this.txtAlarmDescription.TabIndex = 6;
            this.txtAlarmDescription.TabStop = false;
            this.txtAlarmDescription.Text = "";
            // 
            // BtnAlarmSop
            // 
            this.BtnAlarmSop.BackColor = System.Drawing.Color.Transparent;
            this.BtnAlarmSop.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnAlarmSop.ButtonText = "SOP";
            this.BtnAlarmSop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAlarmSop.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnAlarmSop.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnAlarmSop.Location = new System.Drawing.Point(138, 12);
            this.BtnAlarmSop.Name = "BtnAlarmSop";
            this.BtnAlarmSop.Size = new System.Drawing.Size(120, 40);
            this.BtnAlarmSop.TabIndex = 25;
            this.BtnAlarmSop.Text = "SOP";
            this.BtnAlarmSop.UseVisualStyleBackColor = false;
            this.BtnAlarmSop.Click += new System.EventHandler(this.BtnAlarmSop_Click_1);
            // 
            // BtnAlarmImage
            // 
            this.BtnAlarmImage.BackColor = System.Drawing.Color.Transparent;
            this.BtnAlarmImage.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnAlarmImage.ButtonText = "IMAGE";
            this.BtnAlarmImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAlarmImage.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnAlarmImage.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnAlarmImage.Location = new System.Drawing.Point(12, 12);
            this.BtnAlarmImage.Name = "BtnAlarmImage";
            this.BtnAlarmImage.Size = new System.Drawing.Size(120, 40);
            this.BtnAlarmImage.TabIndex = 24;
            this.BtnAlarmImage.Text = "IMAGE";
            this.BtnAlarmImage.UseVisualStyleBackColor = false;
            this.BtnAlarmImage.Click += new System.EventHandler(this.BtnAlarmImage_Click_1);
            // 
            // pdfViewer1
            // 
            this.pdfViewer1.Location = new System.Drawing.Point(13, 58);
            this.pdfViewer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pdfViewer1.Name = "pdfViewer1";
            this.pdfViewer1.Size = new System.Drawing.Size(622, 722);
            this.pdfViewer1.TabIndex = 26;
            this.pdfViewer1.Visible = false;
            // 
            // CFormAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 792);
            this.Controls.Add(this.pdfViewer1);
            this.Controls.Add(this.BtnAlarmSop);
            this.Controls.Add(this.BtnAlarmImage);
            this.Controls.Add(this.pnlAlarmDescription);
            this.Controls.Add(this.pictureBoxEquipmentImage);
            this.Controls.Add(this.BtnBuzzerOff);
            this.Controls.Add(this.BtnAlarmAllReset);
            this.Controls.Add(this.BtnAlarmReset);
            this.Controls.Add(this.BtnTitleAlarmDescription);
            this.Controls.Add(this.BtnTitleAlarmText);
            this.Controls.Add(this.BtnTitleAlarmPart);
            this.Controls.Add(this.BtnAlarmText);
            this.Controls.Add(this.BtnAlarmPart);
            this.Controls.Add(this.BtnAlarmTime);
            this.Controls.Add(this.BtnTitleAlarmTime);
            this.Controls.Add(this.GridViewAlarmList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 792);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1276, 792);
            this.Name = "CFormAlarm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormAlarm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormAlarm_FormClosed);
            this.Load += new System.EventHandler(this.CFormAlarm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewAlarmList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEquipmentImage)).EndInit();
            this.pnlAlarmDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView GridViewAlarmList;
        private System.Windows.Forms.Timer timer;
        private UiAsset.SpeedButton BtnTitleAlarmTime;
        private UiAsset.SpeedButton BtnTitleAlarmPart;
        private UiAsset.SpeedButton BtnTitleAlarmText;
        private UiAsset.SpeedButton BtnTitleAlarmDescription;
        private UiAsset.ImageButton BtnAlarmReset;
        private UiAsset.ImageButton BtnAlarmAllReset;
        private UiAsset.ImageButton BtnBuzzerOff;
        private UiAsset.SpeedButton BtnAlarmTime;
        private UiAsset.SpeedButton BtnAlarmPart;
        private UiAsset.SpeedButton BtnAlarmText;
        private System.Windows.Forms.PictureBox pictureBoxEquipmentImage;
        private System.Windows.Forms.Panel pnlAlarmDescription;
        private System.Windows.Forms.RichTextBox txtAlarmDescription;
        private UiAsset.ImageButton BtnAlarmSop;
        private UiAsset.ImageButton BtnAlarmImage;
        private PdfiumViewer.PdfViewer pdfViewer1;
    }
}