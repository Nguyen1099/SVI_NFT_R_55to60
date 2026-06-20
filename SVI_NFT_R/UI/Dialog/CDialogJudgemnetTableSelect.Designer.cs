namespace SVI_NFT_R
{
    partial class CDialogJudgemnetTableSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDialogJudgemnetTableSelect));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnCellOutRetest = new UiAsset.ImageButton();
            this.BtnCellOutManualTrackout = new UiAsset.ImageButton();
            this.BtnCellOutNG = new UiAsset.ImageButton();
            this.BtnCellOutGood = new UiAsset.ImageButton();
            this.BtnCellOutBinPrime = new UiAsset.ImageButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnCancel = new UiAsset.ImageButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.BtnCellOutRetest);
            this.flowLayoutPanel1.Controls.Add(this.BtnCellOutManualTrackout);
            this.flowLayoutPanel1.Controls.Add(this.BtnCellOutNG);
            this.flowLayoutPanel1.Controls.Add(this.BtnCellOutGood);
            this.flowLayoutPanel1.Controls.Add(this.BtnCellOutBinPrime);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(277, 392);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // BtnCellOutRetest
            // 
            this.BtnCellOutRetest.BackColor = System.Drawing.Color.Transparent;
            this.BtnCellOutRetest.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutRetest.ButtonText = "  [R] RETEST";
            this.BtnCellOutRetest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCellOutRetest.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCellOutRetest.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCellOutRetest.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutRetest.Location = new System.Drawing.Point(3, 3);
            this.BtnCellOutRetest.Name = "BtnCellOutRetest";
            this.BtnCellOutRetest.Size = new System.Drawing.Size(270, 57);
            this.BtnCellOutRetest.TabIndex = 9;
            this.BtnCellOutRetest.Text = "  [R] RETEST";
            this.BtnCellOutRetest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCellOutRetest.UseVisualStyleBackColor = false;
            this.BtnCellOutRetest.Click += new System.EventHandler(this.BtnCellOutRetest_Click);
            // 
            // BtnCellOutManualTrackout
            // 
            this.BtnCellOutManualTrackout.BackColor = System.Drawing.Color.Transparent;
            this.BtnCellOutManualTrackout.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutManualTrackout.ButtonText = "  [O] OUT";
            this.BtnCellOutManualTrackout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCellOutManualTrackout.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCellOutManualTrackout.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCellOutManualTrackout.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutManualTrackout.Location = new System.Drawing.Point(3, 66);
            this.BtnCellOutManualTrackout.Name = "BtnCellOutManualTrackout";
            this.BtnCellOutManualTrackout.Size = new System.Drawing.Size(270, 57);
            this.BtnCellOutManualTrackout.TabIndex = 8;
            this.BtnCellOutManualTrackout.Text = "  [O] OUT";
            this.BtnCellOutManualTrackout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCellOutManualTrackout.UseVisualStyleBackColor = false;
            this.BtnCellOutManualTrackout.Click += new System.EventHandler(this.BtnCellOutManualTrackout_Click);
            // 
            // BtnCellOutNG
            // 
            this.BtnCellOutNG.BackColor = System.Drawing.Color.Transparent;
            this.BtnCellOutNG.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutNG.ButtonText = "  [L] LOSS";
            this.BtnCellOutNG.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCellOutNG.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCellOutNG.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCellOutNG.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutNG.Location = new System.Drawing.Point(3, 129);
            this.BtnCellOutNG.Name = "BtnCellOutNG";
            this.BtnCellOutNG.Size = new System.Drawing.Size(270, 57);
            this.BtnCellOutNG.TabIndex = 7;
            this.BtnCellOutNG.Text = "  [L] LOSS";
            this.BtnCellOutNG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCellOutNG.UseVisualStyleBackColor = false;
            this.BtnCellOutNG.Click += new System.EventHandler(this.BtnCellOutNG_Click);
            // 
            // BtnCellOutGood
            // 
            this.BtnCellOutGood.BackColor = System.Drawing.Color.Transparent;
            this.BtnCellOutGood.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutGood.ButtonText = "  [G] GOOD";
            this.BtnCellOutGood.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCellOutGood.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCellOutGood.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCellOutGood.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutGood.Location = new System.Drawing.Point(3, 192);
            this.BtnCellOutGood.Name = "BtnCellOutGood";
            this.BtnCellOutGood.Size = new System.Drawing.Size(270, 57);
            this.BtnCellOutGood.TabIndex = 6;
            this.BtnCellOutGood.Text = "  [G] GOOD";
            this.BtnCellOutGood.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCellOutGood.UseVisualStyleBackColor = false;
            this.BtnCellOutGood.Click += new System.EventHandler(this.BtnCellOutGood_Click);
            // 
            // BtnCellOutBinPrime
            // 
            this.BtnCellOutBinPrime.BackColor = System.Drawing.Color.Transparent;
            this.BtnCellOutBinPrime.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutBinPrime.ButtonText = "  [T] BIN PRIME";
            this.BtnCellOutBinPrime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCellOutBinPrime.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCellOutBinPrime.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCellOutBinPrime.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCellOutBinPrime.Location = new System.Drawing.Point(3, 255);
            this.BtnCellOutBinPrime.Name = "BtnCellOutBinPrime";
            this.BtnCellOutBinPrime.Size = new System.Drawing.Size(270, 57);
            this.BtnCellOutBinPrime.TabIndex = 10;
            this.BtnCellOutBinPrime.Text = "  [T] BIN PRIME";
            this.BtnCellOutBinPrime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCellOutBinPrime.UseVisualStyleBackColor = false;
            this.BtnCellOutBinPrime.Click += new System.EventHandler(this.BtnCellOutBinPrime_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 329);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(277, 63);
            this.panel1.TabIndex = 11;
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.BtnCancel.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCancel.ButtonText = "CANCEL";
            this.BtnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCancel.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCancel.Location = new System.Drawing.Point(3, 3);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(270, 57);
            this.BtnCancel.TabIndex = 5;
            this.BtnCancel.Text = "CANCEL";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // CDialogJudgemnetTableSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(277, 392);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CDialogJudgemnetTableSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "[ Judgement Select ]";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogCellOutCode_FormClosed);
            this.Load += new System.EventHandler(this.CDialogCellOutCode_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CDialogJudgemnetTableSelect_KeyDown);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private UiAsset.ImageButton BtnCellOutRetest;
        private UiAsset.ImageButton BtnCellOutManualTrackout;
        private UiAsset.ImageButton BtnCellOutNG;
        private UiAsset.ImageButton BtnCellOutGood;
        private UiAsset.ImageButton BtnCellOutBinPrime;
        private System.Windows.Forms.Panel panel1;
        private UiAsset.ImageButton BtnCancel;
        private System.Windows.Forms.Timer timer;
    }
}