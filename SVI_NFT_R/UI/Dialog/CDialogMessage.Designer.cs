namespace SVI_NFT_R
{
    partial class CDialogMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDialogMessage));
            this.RichTextBoxAlarmDescription = new System.Windows.Forms.RichTextBox();
            this.BtnNo = new UiAsset.ImageButton();
            this.BtnYes = new UiAsset.ImageButton();
            this.TextBoxAlarmTime = new System.Windows.Forms.TextBox();
            this.TextBoxAlarmCode = new System.Windows.Forms.TextBox();
            this.TextBoxAlarmObject = new System.Windows.Forms.TextBox();
            this.TextBoxAlarmPosition = new System.Windows.Forms.TextBox();
            this.BtnTitleAlarmTime = new UiAsset.SpeedButton();
            this.BtnTitleAlarmCode = new UiAsset.SpeedButton();
            this.BtnTitleAlarmObject = new UiAsset.SpeedButton();
            this.BtnTitleAlarmPosition = new UiAsset.SpeedButton();
            this.BtnTitleAlarmDescription = new UiAsset.SpeedButton();
            this.BtnLanguage = new UiAsset.ImageButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.pnlTitleAlarmTypeBase = new System.Windows.Forms.Panel();
            this.lblTitleAlarmType = new System.Windows.Forms.Label();
            this.pnlAlarmDescription = new System.Windows.Forms.Panel();
            this.pnlTitleAlarmTypeBase.SuspendLayout();
            this.pnlAlarmDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // RichTextBoxAlarmDescription
            // 
            this.RichTextBoxAlarmDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichTextBoxAlarmDescription.BackColor = System.Drawing.Color.White;
            this.RichTextBoxAlarmDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RichTextBoxAlarmDescription.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.RichTextBoxAlarmDescription.Location = new System.Drawing.Point(6, 6);
            this.RichTextBoxAlarmDescription.Name = "RichTextBoxAlarmDescription";
            this.RichTextBoxAlarmDescription.ReadOnly = true;
            this.RichTextBoxAlarmDescription.ShortcutsEnabled = false;
            this.RichTextBoxAlarmDescription.Size = new System.Drawing.Size(945, 163);
            this.RichTextBoxAlarmDescription.TabIndex = 6;
            this.RichTextBoxAlarmDescription.TabStop = false;
            this.RichTextBoxAlarmDescription.Text = "";
            // 
            // BtnNo
            // 
            this.BtnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnNo.BackColor = System.Drawing.Color.Transparent;
            this.BtnNo.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnNo.ButtonText = "NO";
            this.BtnNo.CornerRadius = 20;
            this.BtnNo.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnNo.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNo.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnNo.Location = new System.Drawing.Point(687, 499);
            this.BtnNo.Name = "BtnNo";
            this.BtnNo.Size = new System.Drawing.Size(284, 76);
            this.BtnNo.TabIndex = 7;
            this.BtnNo.Text = "NO";
            this.BtnNo.UseVisualStyleBackColor = false;
            this.BtnNo.Click += new System.EventHandler(this.BtnNo_Click);
            // 
            // BtnYes
            // 
            this.BtnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnYes.BackColor = System.Drawing.Color.Transparent;
            this.BtnYes.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnYes.ButtonText = "YES";
            this.BtnYes.CornerRadius = 20;
            this.BtnYes.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnYes.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnYes.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnYes.Location = new System.Drawing.Point(397, 499);
            this.BtnYes.Name = "BtnYes";
            this.BtnYes.Size = new System.Drawing.Size(284, 76);
            this.BtnYes.TabIndex = 7;
            this.BtnYes.Text = "YES";
            this.BtnYes.UseVisualStyleBackColor = false;
            this.BtnYes.Click += new System.EventHandler(this.BtnYes_Click);
            // 
            // TextBoxAlarmTime
            // 
            this.TextBoxAlarmTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxAlarmTime.BackColor = System.Drawing.Color.White;
            this.TextBoxAlarmTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxAlarmTime.Font = new System.Drawing.Font("Arial Narrow", 12F);
            this.TextBoxAlarmTime.Location = new System.Drawing.Point(247, 111);
            this.TextBoxAlarmTime.Name = "TextBoxAlarmTime";
            this.TextBoxAlarmTime.ReadOnly = true;
            this.TextBoxAlarmTime.Size = new System.Drawing.Size(724, 26);
            this.TextBoxAlarmTime.TabIndex = 9;
            this.TextBoxAlarmTime.TabStop = false;
            this.TextBoxAlarmTime.Text = "ALARM TIME";
            // 
            // TextBoxAlarmCode
            // 
            this.TextBoxAlarmCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxAlarmCode.BackColor = System.Drawing.Color.White;
            this.TextBoxAlarmCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxAlarmCode.Font = new System.Drawing.Font("Arial Narrow", 12F);
            this.TextBoxAlarmCode.Location = new System.Drawing.Point(247, 152);
            this.TextBoxAlarmCode.Name = "TextBoxAlarmCode";
            this.TextBoxAlarmCode.ReadOnly = true;
            this.TextBoxAlarmCode.Size = new System.Drawing.Size(724, 26);
            this.TextBoxAlarmCode.TabIndex = 9;
            this.TextBoxAlarmCode.TabStop = false;
            this.TextBoxAlarmCode.Text = "ALARM CODE";
            // 
            // TextBoxAlarmObject
            // 
            this.TextBoxAlarmObject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxAlarmObject.BackColor = System.Drawing.Color.White;
            this.TextBoxAlarmObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxAlarmObject.Font = new System.Drawing.Font("Arial Narrow", 12F);
            this.TextBoxAlarmObject.Location = new System.Drawing.Point(247, 193);
            this.TextBoxAlarmObject.Name = "TextBoxAlarmObject";
            this.TextBoxAlarmObject.ReadOnly = true;
            this.TextBoxAlarmObject.Size = new System.Drawing.Size(724, 26);
            this.TextBoxAlarmObject.TabIndex = 9;
            this.TextBoxAlarmObject.TabStop = false;
            this.TextBoxAlarmObject.Text = "ALARM OBJECT";
            // 
            // TextBoxAlarmPosition
            // 
            this.TextBoxAlarmPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxAlarmPosition.BackColor = System.Drawing.Color.White;
            this.TextBoxAlarmPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxAlarmPosition.Font = new System.Drawing.Font("Arial Narrow", 12F);
            this.TextBoxAlarmPosition.Location = new System.Drawing.Point(247, 234);
            this.TextBoxAlarmPosition.Name = "TextBoxAlarmPosition";
            this.TextBoxAlarmPosition.ReadOnly = true;
            this.TextBoxAlarmPosition.Size = new System.Drawing.Size(724, 26);
            this.TextBoxAlarmPosition.TabIndex = 9;
            this.TextBoxAlarmPosition.TabStop = false;
            this.TextBoxAlarmPosition.Text = "ALARM POSITION";
            // 
            // BtnTitleAlarmTime
            // 
            this.BtnTitleAlarmTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleAlarmTime.Font = new System.Drawing.Font("맑은 고딕", 14.25F);
            this.BtnTitleAlarmTime.Location = new System.Drawing.Point(12, 111);
            this.BtnTitleAlarmTime.Margin = new System.Windows.Forms.Padding(0);
            this.BtnTitleAlarmTime.Name = "BtnTitleAlarmTime";
            this.BtnTitleAlarmTime.Size = new System.Drawing.Size(229, 35);
            this.BtnTitleAlarmTime.TabIndex = 11;
            this.BtnTitleAlarmTime.TabStop = false;
            this.BtnTitleAlarmTime.Text = "TIME";
            this.BtnTitleAlarmTime.UseVisualStyleBackColor = true;
            // 
            // BtnTitleAlarmCode
            // 
            this.BtnTitleAlarmCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleAlarmCode.Font = new System.Drawing.Font("맑은 고딕", 14.25F);
            this.BtnTitleAlarmCode.Location = new System.Drawing.Point(12, 152);
            this.BtnTitleAlarmCode.Margin = new System.Windows.Forms.Padding(0);
            this.BtnTitleAlarmCode.Name = "BtnTitleAlarmCode";
            this.BtnTitleAlarmCode.Size = new System.Drawing.Size(229, 35);
            this.BtnTitleAlarmCode.TabIndex = 12;
            this.BtnTitleAlarmCode.TabStop = false;
            this.BtnTitleAlarmCode.Text = "CODE";
            this.BtnTitleAlarmCode.UseVisualStyleBackColor = true;
            // 
            // BtnTitleAlarmObject
            // 
            this.BtnTitleAlarmObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleAlarmObject.Font = new System.Drawing.Font("맑은 고딕", 14.25F);
            this.BtnTitleAlarmObject.Location = new System.Drawing.Point(12, 193);
            this.BtnTitleAlarmObject.Margin = new System.Windows.Forms.Padding(0);
            this.BtnTitleAlarmObject.Name = "BtnTitleAlarmObject";
            this.BtnTitleAlarmObject.Size = new System.Drawing.Size(229, 35);
            this.BtnTitleAlarmObject.TabIndex = 13;
            this.BtnTitleAlarmObject.TabStop = false;
            this.BtnTitleAlarmObject.Text = "OBJECT";
            this.BtnTitleAlarmObject.UseVisualStyleBackColor = true;
            // 
            // BtnTitleAlarmPosition
            // 
            this.BtnTitleAlarmPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleAlarmPosition.Font = new System.Drawing.Font("맑은 고딕", 14.25F);
            this.BtnTitleAlarmPosition.Location = new System.Drawing.Point(12, 234);
            this.BtnTitleAlarmPosition.Margin = new System.Windows.Forms.Padding(0);
            this.BtnTitleAlarmPosition.Name = "BtnTitleAlarmPosition";
            this.BtnTitleAlarmPosition.Size = new System.Drawing.Size(229, 35);
            this.BtnTitleAlarmPosition.TabIndex = 14;
            this.BtnTitleAlarmPosition.TabStop = false;
            this.BtnTitleAlarmPosition.Text = "POSITION";
            this.BtnTitleAlarmPosition.UseVisualStyleBackColor = true;
            // 
            // BtnTitleAlarmDescription
            // 
            this.BtnTitleAlarmDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnTitleAlarmDescription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleAlarmDescription.Font = new System.Drawing.Font("맑은 고딕", 14.25F);
            this.BtnTitleAlarmDescription.Location = new System.Drawing.Point(12, 275);
            this.BtnTitleAlarmDescription.Margin = new System.Windows.Forms.Padding(0);
            this.BtnTitleAlarmDescription.Name = "BtnTitleAlarmDescription";
            this.BtnTitleAlarmDescription.Size = new System.Drawing.Size(959, 35);
            this.BtnTitleAlarmDescription.TabIndex = 15;
            this.BtnTitleAlarmDescription.TabStop = false;
            this.BtnTitleAlarmDescription.Text = "DESCRIPTION";
            this.BtnTitleAlarmDescription.UseVisualStyleBackColor = true;
            // 
            // BtnLanguage
            // 
            this.BtnLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnLanguage.BackColor = System.Drawing.Color.Transparent;
            this.BtnLanguage.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnLanguage.ButtonText = "LANGUAGE";
            this.BtnLanguage.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnLanguage.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLanguage.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnLanguage.Location = new System.Drawing.Point(12, 499);
            this.BtnLanguage.Name = "BtnLanguage";
            this.BtnLanguage.Size = new System.Drawing.Size(91, 76);
            this.BtnLanguage.TabIndex = 16;
            this.BtnLanguage.TabStop = false;
            this.BtnLanguage.Text = "LANGUAGE";
            this.BtnLanguage.UseVisualStyleBackColor = false;
            this.BtnLanguage.Click += new System.EventHandler(this.BtnLanguage_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // pnlTitleAlarmTypeBase
            // 
            this.pnlTitleAlarmTypeBase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(105)))), ((int)(((byte)(220)))));
            this.pnlTitleAlarmTypeBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTitleAlarmTypeBase.Controls.Add(this.lblTitleAlarmType);
            this.pnlTitleAlarmTypeBase.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleAlarmTypeBase.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleAlarmTypeBase.Name = "pnlTitleAlarmTypeBase";
            this.pnlTitleAlarmTypeBase.Size = new System.Drawing.Size(984, 92);
            this.pnlTitleAlarmTypeBase.TabIndex = 17;
            // 
            // lblTitleAlarmType
            // 
            this.lblTitleAlarmType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTitleAlarmType.AutoSize = true;
            this.lblTitleAlarmType.Font = new System.Drawing.Font("맑은 고딕", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleAlarmType.ForeColor = System.Drawing.Color.White;
            this.lblTitleAlarmType.Location = new System.Drawing.Point(39, 20);
            this.lblTitleAlarmType.Name = "lblTitleAlarmType";
            this.lblTitleAlarmType.Size = new System.Drawing.Size(133, 65);
            this.lblTitleAlarmType.TabIndex = 1;
            this.lblTitleAlarmType.Text = "TYPE";
            // 
            // pnlAlarmDescription
            // 
            this.pnlAlarmDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAlarmDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAlarmDescription.Controls.Add(this.RichTextBoxAlarmDescription);
            this.pnlAlarmDescription.Location = new System.Drawing.Point(12, 316);
            this.pnlAlarmDescription.Name = "pnlAlarmDescription";
            this.pnlAlarmDescription.Size = new System.Drawing.Size(959, 177);
            this.pnlAlarmDescription.TabIndex = 18;
            // 
            // CDialogMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 586);
            this.ControlBox = false;
            this.Controls.Add(this.pnlAlarmDescription);
            this.Controls.Add(this.pnlTitleAlarmTypeBase);
            this.Controls.Add(this.BtnLanguage);
            this.Controls.Add(this.BtnTitleAlarmDescription);
            this.Controls.Add(this.BtnTitleAlarmPosition);
            this.Controls.Add(this.BtnTitleAlarmObject);
            this.Controls.Add(this.BtnTitleAlarmCode);
            this.Controls.Add(this.BtnTitleAlarmTime);
            this.Controls.Add(this.TextBoxAlarmPosition);
            this.Controls.Add(this.TextBoxAlarmObject);
            this.Controls.Add(this.TextBoxAlarmCode);
            this.Controls.Add(this.TextBoxAlarmTime);
            this.Controls.Add(this.BtnYes);
            this.Controls.Add(this.BtnNo);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(747, 521);
            this.Name = "CDialogMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[ Message ]";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogMessage_FormClosed);
            this.Load += new System.EventHandler(this.CDialogMessage_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CDialogMessage_KeyDown);
            this.pnlTitleAlarmTypeBase.ResumeLayout(false);
            this.pnlTitleAlarmTypeBase.PerformLayout();
            this.pnlAlarmDescription.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox RichTextBoxAlarmDescription;
        private UiAsset.ImageButton BtnNo;
        private UiAsset.ImageButton BtnYes;
        private System.Windows.Forms.TextBox TextBoxAlarmTime;
        private System.Windows.Forms.TextBox TextBoxAlarmCode;
        private System.Windows.Forms.TextBox TextBoxAlarmObject;
        private System.Windows.Forms.TextBox TextBoxAlarmPosition;
        private UiAsset.SpeedButton BtnTitleAlarmTime;
        private UiAsset.SpeedButton BtnTitleAlarmCode;
        private UiAsset.SpeedButton BtnTitleAlarmObject;
        private UiAsset.SpeedButton BtnTitleAlarmPosition;
        private UiAsset.SpeedButton BtnTitleAlarmDescription;
        private UiAsset.ImageButton BtnLanguage;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel pnlTitleAlarmTypeBase;
        private System.Windows.Forms.Label lblTitleAlarmType;
        private System.Windows.Forms.Panel pnlAlarmDescription;
    }
}