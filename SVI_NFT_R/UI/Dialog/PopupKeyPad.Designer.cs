namespace SVI_NFT_R
{
    partial class PopupKeyPad
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
            this.PnlBase = new System.Windows.Forms.TableLayoutPanel();
            this.BtnOK = new UiAsset.ImageButton();
            this.BtnBackSpace = new UiAsset.ImageButton();
            this.BtnKey7 = new UiAsset.ImageButton();
            this.BtnKey8 = new UiAsset.ImageButton();
            this.BtnKey9 = new UiAsset.ImageButton();
            this.BtnKey4 = new UiAsset.ImageButton();
            this.BtnKey5 = new UiAsset.ImageButton();
            this.BtnKey6 = new UiAsset.ImageButton();
            this.BtnKey1 = new UiAsset.ImageButton();
            this.BtnKey2 = new UiAsset.ImageButton();
            this.BtnKey3 = new UiAsset.ImageButton();
            this.BtnKey0 = new UiAsset.ImageButton();
            this.BtnClear = new UiAsset.ImageButton();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.PnlBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlBase
            // 
            this.PnlBase.ColumnCount = 3;
            this.PnlBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.PnlBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.PnlBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.PnlBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.PnlBase.Controls.Add(this.BtnOK, 1, 4);
            this.PnlBase.Controls.Add(this.BtnBackSpace, 1, 0);
            this.PnlBase.Controls.Add(this.BtnKey7, 0, 1);
            this.PnlBase.Controls.Add(this.BtnKey8, 1, 1);
            this.PnlBase.Controls.Add(this.BtnKey9, 2, 1);
            this.PnlBase.Controls.Add(this.BtnKey4, 0, 2);
            this.PnlBase.Controls.Add(this.BtnKey5, 1, 2);
            this.PnlBase.Controls.Add(this.BtnKey6, 2, 2);
            this.PnlBase.Controls.Add(this.BtnKey1, 0, 3);
            this.PnlBase.Controls.Add(this.BtnKey2, 1, 3);
            this.PnlBase.Controls.Add(this.BtnKey3, 2, 3);
            this.PnlBase.Controls.Add(this.BtnKey0, 0, 4);
            this.PnlBase.Controls.Add(this.BtnClear, 0, 0);
            this.PnlBase.Location = new System.Drawing.Point(0, 0);
            this.PnlBase.Name = "PnlBase";
            this.PnlBase.RowCount = 5;
            this.PnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.PnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.PnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.PnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.PnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.PnlBase.Size = new System.Drawing.Size(237, 395);
            this.PnlBase.TabIndex = 0;
            // 
            // BtnOK
            // 
            this.BtnOK.BackColor = System.Drawing.Color.Transparent;
            this.BtnOK.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOK.ButtonText = "OK";
            this.PnlBase.SetColumnSpan(this.BtnOK, 2);
            this.BtnOK.CornerRadius = 16;
            this.BtnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnOK.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOK.Location = new System.Drawing.Point(82, 319);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(152, 73);
            this.BtnOK.TabIndex = 2;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = false;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnBackSpace
            // 
            this.BtnBackSpace.BackColor = System.Drawing.Color.Transparent;
            this.BtnBackSpace.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBackSpace.ButtonText = "←";
            this.BtnBackSpace.CornerRadius = 16;
            this.BtnBackSpace.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBackSpace.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnBackSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnBackSpace.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnBackSpace.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnBackSpace.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnBackSpace.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBackSpace.Location = new System.Drawing.Point(82, 3);
            this.BtnBackSpace.Name = "BtnBackSpace";
            this.BtnBackSpace.Size = new System.Drawing.Size(73, 73);
            this.BtnBackSpace.TabIndex = 1;
            this.BtnBackSpace.Text = "←";
            this.BtnBackSpace.UseVisualStyleBackColor = false;
            this.BtnBackSpace.Click += new System.EventHandler(this.BtnBackSpace_Click);
            // 
            // BtnKey7
            // 
            this.BtnKey7.BackColor = System.Drawing.Color.Transparent;
            this.BtnKey7.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey7.ButtonText = "7";
            this.BtnKey7.CornerRadius = 16;
            this.BtnKey7.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop;
            this.BtnKey7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnKey7.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnKey7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnKey7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnKey7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnKey7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnKey7.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey7.Location = new System.Drawing.Point(3, 82);
            this.BtnKey7.Name = "BtnKey7";
            this.BtnKey7.Size = new System.Drawing.Size(73, 73);
            this.BtnKey7.TabIndex = 1;
            this.BtnKey7.Text = "7";
            this.BtnKey7.UseVisualStyleBackColor = false;
            this.BtnKey7.Click += new System.EventHandler(this.BtnKey0_Click);
            // 
            // BtnKey8
            // 
            this.BtnKey8.BackColor = System.Drawing.Color.Transparent;
            this.BtnKey8.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey8.ButtonText = "8";
            this.BtnKey8.CornerRadius = 16;
            this.BtnKey8.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnKey8.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnKey8.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnKey8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnKey8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnKey8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnKey8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnKey8.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey8.Location = new System.Drawing.Point(82, 82);
            this.BtnKey8.Name = "BtnKey8";
            this.BtnKey8.Size = new System.Drawing.Size(73, 73);
            this.BtnKey8.TabIndex = 1;
            this.BtnKey8.Text = "8";
            this.BtnKey8.UseVisualStyleBackColor = false;
            this.BtnKey8.Click += new System.EventHandler(this.BtnKey0_Click);
            // 
            // BtnKey9
            // 
            this.BtnKey9.BackColor = System.Drawing.Color.Transparent;
            this.BtnKey9.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey9.ButtonText = "9";
            this.BtnKey9.CornerRadius = 16;
            this.BtnKey9.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.RightTop;
            this.BtnKey9.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnKey9.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnKey9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnKey9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnKey9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnKey9.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnKey9.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey9.Location = new System.Drawing.Point(161, 82);
            this.BtnKey9.Name = "BtnKey9";
            this.BtnKey9.Size = new System.Drawing.Size(73, 73);
            this.BtnKey9.TabIndex = 1;
            this.BtnKey9.Text = "9";
            this.BtnKey9.UseVisualStyleBackColor = false;
            this.BtnKey9.Click += new System.EventHandler(this.BtnKey0_Click);
            // 
            // BtnKey4
            // 
            this.BtnKey4.BackColor = System.Drawing.Color.Transparent;
            this.BtnKey4.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey4.ButtonText = "4";
            this.BtnKey4.CornerRadius = 16;
            this.BtnKey4.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnKey4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnKey4.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnKey4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnKey4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnKey4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnKey4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnKey4.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey4.Location = new System.Drawing.Point(3, 161);
            this.BtnKey4.Name = "BtnKey4";
            this.BtnKey4.Size = new System.Drawing.Size(73, 73);
            this.BtnKey4.TabIndex = 1;
            this.BtnKey4.Text = "4";
            this.BtnKey4.UseVisualStyleBackColor = false;
            this.BtnKey4.Click += new System.EventHandler(this.BtnKey0_Click);
            // 
            // BtnKey5
            // 
            this.BtnKey5.BackColor = System.Drawing.Color.Transparent;
            this.BtnKey5.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey5.ButtonText = "5";
            this.BtnKey5.CornerRadius = 16;
            this.BtnKey5.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnKey5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnKey5.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnKey5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnKey5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnKey5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnKey5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnKey5.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey5.Location = new System.Drawing.Point(82, 161);
            this.BtnKey5.Name = "BtnKey5";
            this.BtnKey5.Size = new System.Drawing.Size(73, 73);
            this.BtnKey5.TabIndex = 1;
            this.BtnKey5.Text = "5";
            this.BtnKey5.UseVisualStyleBackColor = false;
            this.BtnKey5.Click += new System.EventHandler(this.BtnKey0_Click);
            // 
            // BtnKey6
            // 
            this.BtnKey6.BackColor = System.Drawing.Color.Transparent;
            this.BtnKey6.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey6.ButtonText = "6";
            this.BtnKey6.CornerRadius = 16;
            this.BtnKey6.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnKey6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnKey6.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnKey6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnKey6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnKey6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnKey6.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnKey6.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey6.Location = new System.Drawing.Point(161, 161);
            this.BtnKey6.Name = "BtnKey6";
            this.BtnKey6.Size = new System.Drawing.Size(73, 73);
            this.BtnKey6.TabIndex = 1;
            this.BtnKey6.Text = "6";
            this.BtnKey6.UseVisualStyleBackColor = false;
            this.BtnKey6.Click += new System.EventHandler(this.BtnKey0_Click);
            // 
            // BtnKey1
            // 
            this.BtnKey1.BackColor = System.Drawing.Color.Transparent;
            this.BtnKey1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey1.ButtonText = "1";
            this.BtnKey1.CornerRadius = 16;
            this.BtnKey1.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnKey1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnKey1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnKey1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnKey1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnKey1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnKey1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnKey1.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey1.Location = new System.Drawing.Point(3, 240);
            this.BtnKey1.Name = "BtnKey1";
            this.BtnKey1.Size = new System.Drawing.Size(73, 73);
            this.BtnKey1.TabIndex = 1;
            this.BtnKey1.Text = "1";
            this.BtnKey1.UseVisualStyleBackColor = false;
            this.BtnKey1.Click += new System.EventHandler(this.BtnKey0_Click);
            // 
            // BtnKey2
            // 
            this.BtnKey2.BackColor = System.Drawing.Color.Transparent;
            this.BtnKey2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey2.ButtonText = "2";
            this.BtnKey2.CornerRadius = 16;
            this.BtnKey2.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnKey2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnKey2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnKey2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnKey2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnKey2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnKey2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnKey2.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey2.Location = new System.Drawing.Point(82, 240);
            this.BtnKey2.Name = "BtnKey2";
            this.BtnKey2.Size = new System.Drawing.Size(73, 73);
            this.BtnKey2.TabIndex = 1;
            this.BtnKey2.Text = "2";
            this.BtnKey2.UseVisualStyleBackColor = false;
            this.BtnKey2.Click += new System.EventHandler(this.BtnKey0_Click);
            // 
            // BtnKey3
            // 
            this.BtnKey3.BackColor = System.Drawing.Color.Transparent;
            this.BtnKey3.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey3.ButtonText = "3";
            this.BtnKey3.CornerRadius = 16;
            this.BtnKey3.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom;
            this.BtnKey3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnKey3.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnKey3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnKey3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnKey3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnKey3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnKey3.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey3.Location = new System.Drawing.Point(161, 240);
            this.BtnKey3.Name = "BtnKey3";
            this.BtnKey3.Size = new System.Drawing.Size(73, 73);
            this.BtnKey3.TabIndex = 1;
            this.BtnKey3.Text = "3";
            this.BtnKey3.UseVisualStyleBackColor = false;
            this.BtnKey3.Click += new System.EventHandler(this.BtnKey0_Click);
            // 
            // BtnKey0
            // 
            this.BtnKey0.BackColor = System.Drawing.Color.Transparent;
            this.BtnKey0.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey0.ButtonText = "0";
            this.BtnKey0.CornerRadius = 16;
            this.BtnKey0.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnKey0.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnKey0.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnKey0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnKey0.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnKey0.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnKey0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnKey0.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnKey0.Location = new System.Drawing.Point(3, 319);
            this.BtnKey0.Name = "BtnKey0";
            this.BtnKey0.Size = new System.Drawing.Size(73, 73);
            this.BtnKey0.TabIndex = 1;
            this.BtnKey0.Text = "0";
            this.BtnKey0.UseVisualStyleBackColor = false;
            this.BtnKey0.Click += new System.EventHandler(this.BtnKey0_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.BackColor = System.Drawing.Color.Transparent;
            this.BtnClear.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnClear.ButtonText = "CLR";
            this.BtnClear.CornerRadius = 16;
            this.BtnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnClear.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnClear.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnClear.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnClear.Location = new System.Drawing.Point(3, 3);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(73, 73);
            this.BtnClear.TabIndex = 1;
            this.BtnClear.Text = "CLR";
            this.BtnClear.UseVisualStyleBackColor = false;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // Timer
            // 
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // PopupKeyPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PnlBase);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "PopupKeyPad";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PopupKeyPad";
            this.TopMost = true;
            this.PnlBase.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel PnlBase;
        private System.Windows.Forms.Timer Timer;
        private UiAsset.ImageButton BtnKey7;
        private UiAsset.ImageButton BtnBackSpace;
        private UiAsset.ImageButton BtnKey8;
        private UiAsset.ImageButton BtnKey9;
        private UiAsset.ImageButton BtnKey4;
        private UiAsset.ImageButton BtnKey5;
        private UiAsset.ImageButton BtnKey6;
        private UiAsset.ImageButton BtnKey1;
        private UiAsset.ImageButton BtnKey2;
        private UiAsset.ImageButton BtnKey3;
        private UiAsset.ImageButton BtnKey0;
        private UiAsset.ImageButton BtnClear;
        private UiAsset.ImageButton BtnOK;
    }
}