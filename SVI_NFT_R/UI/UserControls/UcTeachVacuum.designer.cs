namespace SVI_NFT_R.UI.UserControls
{
    partial class UcTeachVacuum
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
            this.PnlLayout = new System.Windows.Forms.TableLayoutPanel();
            this.BtnDeviceName = new UiAsset.SpeedButton();
            this.BtnVacuum = new UiAsset.ImageButton();
            this.BtnBlow = new UiAsset.ImageButton();
            this.PnlDisplayAnalogBase = new System.Windows.Forms.Panel();
            this.LblDisplayAnalog1 = new System.Windows.Forms.Label();
            this.LblDisplayAnalog4 = new System.Windows.Forms.Label();
            this.LblDisplayAnalog3 = new System.Windows.Forms.Label();
            this.LblDisplayAnalog2 = new System.Windows.Forms.Label();
            this.BtnDisplayAnalog = new UiAsset.ImageButton();
            this.BtnSensor = new UiAsset.SpeedButton();
            this.PnlLayout.SuspendLayout();
            this.PnlDisplayAnalogBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlLayout
            // 
            this.PnlLayout.ColumnCount = 10;
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.95392F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.60522F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.60522F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.95252F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.88312F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.PnlLayout.Controls.Add(this.BtnDeviceName, 0, 0);
            this.PnlLayout.Controls.Add(this.BtnVacuum, 2, 0);
            this.PnlLayout.Controls.Add(this.BtnBlow, 4, 0);
            this.PnlLayout.Controls.Add(this.PnlDisplayAnalogBase, 8, 0);
            this.PnlLayout.Controls.Add(this.BtnSensor, 6, 0);
            this.PnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlLayout.Location = new System.Drawing.Point(0, 0);
            this.PnlLayout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PnlLayout.Name = "PnlLayout";
            this.PnlLayout.RowCount = 1;
            this.PnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlLayout.Size = new System.Drawing.Size(625, 58);
            this.PnlLayout.TabIndex = 0;
            // 
            // BtnDeviceName
            // 
            this.BtnDeviceName.BackColor = System.Drawing.Color.White;
            this.BtnDeviceName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnDeviceName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnDeviceName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDeviceName.Location = new System.Drawing.Point(0, 0);
            this.BtnDeviceName.Margin = new System.Windows.Forms.Padding(0);
            this.BtnDeviceName.Name = "BtnDeviceName";
            this.BtnDeviceName.Size = new System.Drawing.Size(84, 58);
            this.BtnDeviceName.TabIndex = 0;
            this.BtnDeviceName.TabStop = false;
            this.BtnDeviceName.Text = "DEVICE NAME";
            this.BtnDeviceName.UseVisualStyleBackColor = false;
            // 
            // BtnVacuum
            // 
            this.BtnVacuum.BackColor = System.Drawing.Color.Transparent;
            this.BtnVacuum.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnVacuum.ButtonText = "VACUUM";
            this.BtnVacuum.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnVacuum.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnVacuum.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnVacuum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnVacuum.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnVacuum.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnVacuum.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnVacuum.Location = new System.Drawing.Point(88, 0);
            this.BtnVacuum.Margin = new System.Windows.Forms.Padding(0);
            this.BtnVacuum.Name = "BtnVacuum";
            this.BtnVacuum.Size = new System.Drawing.Size(112, 58);
            this.BtnVacuum.TabIndex = 1;
            this.BtnVacuum.Text = "VACUUM";
            this.BtnVacuum.UseVisualStyleBackColor = false;
            this.BtnVacuum.Click += new System.EventHandler(this.BtnVacuum_Click);
            // 
            // BtnBlow
            // 
            this.BtnBlow.BackColor = System.Drawing.Color.Transparent;
            this.BtnBlow.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBlow.ButtonText = "BLOW";
            this.BtnBlow.CornerRound = UiAsset.ImageButton.ImageButtonRoundCorner.None;
            this.BtnBlow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBlow.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnBlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnBlow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnBlow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnBlow.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBlow.Location = new System.Drawing.Point(204, 0);
            this.BtnBlow.Margin = new System.Windows.Forms.Padding(0);
            this.BtnBlow.Name = "BtnBlow";
            this.BtnBlow.Size = new System.Drawing.Size(112, 58);
            this.BtnBlow.TabIndex = 1;
            this.BtnBlow.Text = "BLOW";
            this.BtnBlow.UseVisualStyleBackColor = false;
            this.BtnBlow.Click += new System.EventHandler(this.BtnBlow_Click);
            // 
            // PnlDisplayAnalogBase
            // 
            this.PnlDisplayAnalogBase.Controls.Add(this.LblDisplayAnalog1);
            this.PnlDisplayAnalogBase.Controls.Add(this.LblDisplayAnalog4);
            this.PnlDisplayAnalogBase.Controls.Add(this.LblDisplayAnalog3);
            this.PnlDisplayAnalogBase.Controls.Add(this.LblDisplayAnalog2);
            this.PnlDisplayAnalogBase.Controls.Add(this.BtnDisplayAnalog);
            this.PnlDisplayAnalogBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlDisplayAnalogBase.Location = new System.Drawing.Point(408, 0);
            this.PnlDisplayAnalogBase.Margin = new System.Windows.Forms.Padding(0);
            this.PnlDisplayAnalogBase.Name = "PnlDisplayAnalogBase";
            this.PnlDisplayAnalogBase.Size = new System.Drawing.Size(210, 58);
            this.PnlDisplayAnalogBase.TabIndex = 6;
            // 
            // LblDisplayAnalog1
            // 
            this.LblDisplayAnalog1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.LblDisplayAnalog1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblDisplayAnalog1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblDisplayAnalog1.Location = new System.Drawing.Point(6, 4);
            this.LblDisplayAnalog1.Margin = new System.Windows.Forms.Padding(0);
            this.LblDisplayAnalog1.Name = "LblDisplayAnalog1";
            this.LblDisplayAnalog1.Size = new System.Drawing.Size(85, 24);
            this.LblDisplayAnalog1.TabIndex = 2;
            this.LblDisplayAnalog1.Text = "-00.0kPa";
            this.LblDisplayAnalog1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblDisplayAnalog4
            // 
            this.LblDisplayAnalog4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.LblDisplayAnalog4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblDisplayAnalog4.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblDisplayAnalog4.Location = new System.Drawing.Point(94, 29);
            this.LblDisplayAnalog4.Margin = new System.Windows.Forms.Padding(0);
            this.LblDisplayAnalog4.Name = "LblDisplayAnalog4";
            this.LblDisplayAnalog4.Size = new System.Drawing.Size(85, 24);
            this.LblDisplayAnalog4.TabIndex = 5;
            this.LblDisplayAnalog4.Text = "-00.0kPa";
            this.LblDisplayAnalog4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblDisplayAnalog3
            // 
            this.LblDisplayAnalog3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.LblDisplayAnalog3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblDisplayAnalog3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblDisplayAnalog3.Location = new System.Drawing.Point(6, 29);
            this.LblDisplayAnalog3.Margin = new System.Windows.Forms.Padding(0);
            this.LblDisplayAnalog3.Name = "LblDisplayAnalog3";
            this.LblDisplayAnalog3.Size = new System.Drawing.Size(85, 24);
            this.LblDisplayAnalog3.TabIndex = 4;
            this.LblDisplayAnalog3.Text = "-00.0kPa";
            this.LblDisplayAnalog3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblDisplayAnalog2
            // 
            this.LblDisplayAnalog2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.LblDisplayAnalog2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblDisplayAnalog2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblDisplayAnalog2.Location = new System.Drawing.Point(94, 4);
            this.LblDisplayAnalog2.Margin = new System.Windows.Forms.Padding(0);
            this.LblDisplayAnalog2.Name = "LblDisplayAnalog2";
            this.LblDisplayAnalog2.Size = new System.Drawing.Size(85, 24);
            this.LblDisplayAnalog2.TabIndex = 3;
            this.LblDisplayAnalog2.Text = "-00.0kPa";
            this.LblDisplayAnalog2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BtnDisplayAnalog
            // 
            this.BtnDisplayAnalog.BackColor = System.Drawing.Color.Transparent;
            this.BtnDisplayAnalog.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnDisplayAnalog.ButtonText = "";
            this.BtnDisplayAnalog.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnDisplayAnalog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnDisplayAnalog.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnDisplayAnalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnDisplayAnalog.Enabled = false;
            this.BtnDisplayAnalog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnDisplayAnalog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnDisplayAnalog.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnDisplayAnalog.Location = new System.Drawing.Point(0, 0);
            this.BtnDisplayAnalog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnDisplayAnalog.Name = "BtnDisplayAnalog";
            this.BtnDisplayAnalog.Size = new System.Drawing.Size(210, 58);
            this.BtnDisplayAnalog.TabIndex = 6;
            this.BtnDisplayAnalog.UseVisualStyleBackColor = false;
            // 
            // BtnSensor
            // 
            this.BtnSensor.BackColor = System.Drawing.Color.White;
            this.BtnSensor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSensor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnSensor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSensor.Location = new System.Drawing.Point(320, 0);
            this.BtnSensor.Margin = new System.Windows.Forms.Padding(0);
            this.BtnSensor.Name = "BtnSensor";
            this.BtnSensor.Size = new System.Drawing.Size(84, 58);
            this.BtnSensor.TabIndex = 7;
            this.BtnSensor.TabStop = false;
            this.BtnSensor.Text = "SENSOR";
            this.BtnSensor.UseVisualStyleBackColor = false;
            // 
            // UcTeachVacuum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.PnlLayout);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UcTeachVacuum";
            this.Size = new System.Drawing.Size(625, 58);
            this.PnlLayout.ResumeLayout(false);
            this.PnlDisplayAnalogBase.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel PnlLayout;
        private UiAsset.SpeedButton BtnDeviceName;
        private UiAsset.ImageButton BtnVacuum;
        private UiAsset.ImageButton BtnBlow;
        private System.Windows.Forms.Label LblDisplayAnalog1;
        private System.Windows.Forms.Label LblDisplayAnalog2;
        private System.Windows.Forms.Label LblDisplayAnalog3;
        private System.Windows.Forms.Label LblDisplayAnalog4;
        private System.Windows.Forms.Panel PnlDisplayAnalogBase;
        private UiAsset.ImageButton BtnDisplayAnalog;
        private UiAsset.SpeedButton BtnSensor;
    }
}
