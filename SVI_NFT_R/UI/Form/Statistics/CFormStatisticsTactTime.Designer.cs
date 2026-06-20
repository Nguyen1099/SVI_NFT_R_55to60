using UiAsset;

namespace SVI_NFT_R
{
    partial class CFormStatisticsTactTime
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
            this.GridViewTactTimeOutput = new System.Windows.Forms.DataGridView();
            this.GridViewTactTimeUnit = new System.Windows.Forms.DataGridView();
            this.BtnTitleTactTimeOutput = new UiAsset.SpeedButton();
            this.BtnTitleTactTimeUnit = new UiAsset.SpeedButton();
            this.BtnTitleSlowUnit = new UiAsset.SpeedButton();
            this.BtnSlowUnit = new UiAsset.SpeedButton();
            this.BtnTitleFastUnit = new UiAsset.SpeedButton();
            this.BtnFastUnit = new UiAsset.SpeedButton();
            this.BtnTitleAverage = new UiAsset.SpeedButton();
            this.BtnAverage = new UiAsset.SpeedButton();
            this.BtnTitleMaxLength = new UiAsset.SpeedButton();
            this.BtnMaxLength = new UiAsset.SpeedButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnClearUnit = new UiAsset.ImageButton();
            this.BtnClearOutput = new UiAsset.ImageButton();
            this.BtnTitleTactTimeUnitDetail = new UiAsset.SpeedButton();
            this.GridViewTactTimeUnitDetail = new System.Windows.Forms.DataGridView();
            this.DgvSimpleOutTactTime = new System.Windows.Forms.DataGridView();
            this.BtnOutTactSimpleView = new UiAsset.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewTactTimeOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewTactTimeUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewTactTimeUnitDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSimpleOutTactTime)).BeginInit();
            this.SuspendLayout();
            // 
            // GridViewTactTimeOutput
            // 
            this.GridViewTactTimeOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewTactTimeOutput.Location = new System.Drawing.Point(641, 64);
            this.GridViewTactTimeOutput.Name = "GridViewTactTimeOutput";
            this.GridViewTactTimeOutput.RowTemplate.Height = 23;
            this.GridViewTactTimeOutput.Size = new System.Drawing.Size(623, 565);
            this.GridViewTactTimeOutput.TabIndex = 6;
            this.GridViewTactTimeOutput.VirtualMode = true;
            this.GridViewTactTimeOutput.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.GridViewTactTimeOutput_CellValueNeeded);
            // 
            // GridViewTactTimeUnit
            // 
            this.GridViewTactTimeUnit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewTactTimeUnit.Location = new System.Drawing.Point(12, 64);
            this.GridViewTactTimeUnit.Name = "GridViewTactTimeUnit";
            this.GridViewTactTimeUnit.RowTemplate.Height = 23;
            this.GridViewTactTimeUnit.Size = new System.Drawing.Size(623, 280);
            this.GridViewTactTimeUnit.TabIndex = 7;
            this.GridViewTactTimeUnit.SelectionChanged += new System.EventHandler(this.GridViewTactTimeUnit_SelectionChanged);
            // 
            // BtnTitleTactTimeOutput
            // 
            this.BtnTitleTactTimeOutput.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleTactTimeOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleTactTimeOutput.Location = new System.Drawing.Point(641, 12);
            this.BtnTitleTactTimeOutput.Name = "BtnTitleTactTimeOutput";
            this.BtnTitleTactTimeOutput.Size = new System.Drawing.Size(623, 46);
            this.BtnTitleTactTimeOutput.TabIndex = 4;
            this.BtnTitleTactTimeOutput.TabStop = false;
            this.BtnTitleTactTimeOutput.Text = "OUTPUT TACT TIME";
            this.BtnTitleTactTimeOutput.UseVisualStyleBackColor = true;
            // 
            // BtnTitleTactTimeUnit
            // 
            this.BtnTitleTactTimeUnit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleTactTimeUnit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleTactTimeUnit.Location = new System.Drawing.Point(12, 12);
            this.BtnTitleTactTimeUnit.Name = "BtnTitleTactTimeUnit";
            this.BtnTitleTactTimeUnit.Size = new System.Drawing.Size(623, 46);
            this.BtnTitleTactTimeUnit.TabIndex = 5;
            this.BtnTitleTactTimeUnit.TabStop = false;
            this.BtnTitleTactTimeUnit.Text = "UNIT TACT TIME";
            this.BtnTitleTactTimeUnit.UseVisualStyleBackColor = true;
            // 
            // BtnTitleSlowUnit
            // 
            this.BtnTitleSlowUnit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleSlowUnit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleSlowUnit.Location = new System.Drawing.Point(12, 635);
            this.BtnTitleSlowUnit.Name = "BtnTitleSlowUnit";
            this.BtnTitleSlowUnit.Size = new System.Drawing.Size(203, 46);
            this.BtnTitleSlowUnit.TabIndex = 8;
            this.BtnTitleSlowUnit.TabStop = false;
            this.BtnTitleSlowUnit.Text = "SLOW UNIT";
            this.BtnTitleSlowUnit.UseVisualStyleBackColor = true;
            // 
            // BtnSlowUnit
            // 
            this.BtnSlowUnit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSlowUnit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSlowUnit.Location = new System.Drawing.Point(12, 687);
            this.BtnSlowUnit.Name = "BtnSlowUnit";
            this.BtnSlowUnit.Size = new System.Drawing.Size(203, 46);
            this.BtnSlowUnit.TabIndex = 8;
            this.BtnSlowUnit.TabStop = false;
            this.BtnSlowUnit.UseVisualStyleBackColor = true;
            // 
            // BtnTitleFastUnit
            // 
            this.BtnTitleFastUnit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleFastUnit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleFastUnit.Location = new System.Drawing.Point(221, 635);
            this.BtnTitleFastUnit.Name = "BtnTitleFastUnit";
            this.BtnTitleFastUnit.Size = new System.Drawing.Size(203, 46);
            this.BtnTitleFastUnit.TabIndex = 8;
            this.BtnTitleFastUnit.TabStop = false;
            this.BtnTitleFastUnit.Text = "FAST UNIT";
            this.BtnTitleFastUnit.UseVisualStyleBackColor = true;
            // 
            // BtnFastUnit
            // 
            this.BtnFastUnit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnFastUnit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFastUnit.Location = new System.Drawing.Point(221, 687);
            this.BtnFastUnit.Name = "BtnFastUnit";
            this.BtnFastUnit.Size = new System.Drawing.Size(203, 46);
            this.BtnFastUnit.TabIndex = 8;
            this.BtnFastUnit.TabStop = false;
            this.BtnFastUnit.UseVisualStyleBackColor = true;
            // 
            // BtnTitleAverage
            // 
            this.BtnTitleAverage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleAverage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleAverage.Location = new System.Drawing.Point(641, 635);
            this.BtnTitleAverage.Name = "BtnTitleAverage";
            this.BtnTitleAverage.Size = new System.Drawing.Size(203, 46);
            this.BtnTitleAverage.TabIndex = 8;
            this.BtnTitleAverage.TabStop = false;
            this.BtnTitleAverage.Text = "AVERAGE (sec)";
            this.BtnTitleAverage.UseVisualStyleBackColor = true;
            // 
            // BtnAverage
            // 
            this.BtnAverage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAverage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAverage.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnAverage.Location = new System.Drawing.Point(641, 687);
            this.BtnAverage.Name = "BtnAverage";
            this.BtnAverage.Size = new System.Drawing.Size(203, 46);
            this.BtnAverage.TabIndex = 8;
            this.BtnAverage.TabStop = false;
            this.BtnAverage.UseVisualStyleBackColor = true;
            // 
            // BtnTitleMaxLength
            // 
            this.BtnTitleMaxLength.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleMaxLength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleMaxLength.Location = new System.Drawing.Point(850, 635);
            this.BtnTitleMaxLength.Name = "BtnTitleMaxLength";
            this.BtnTitleMaxLength.Size = new System.Drawing.Size(203, 46);
            this.BtnTitleMaxLength.TabIndex = 8;
            this.BtnTitleMaxLength.TabStop = false;
            this.BtnTitleMaxLength.Text = "MAX LENGTH";
            this.BtnTitleMaxLength.UseVisualStyleBackColor = true;
            // 
            // BtnMaxLength
            // 
            this.BtnMaxLength.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnMaxLength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMaxLength.Location = new System.Drawing.Point(850, 687);
            this.BtnMaxLength.Name = "BtnMaxLength";
            this.BtnMaxLength.Size = new System.Drawing.Size(203, 46);
            this.BtnMaxLength.TabIndex = 8;
            this.BtnMaxLength.TabStop = false;
            this.BtnMaxLength.UseVisualStyleBackColor = true;
            this.BtnMaxLength.Click += new System.EventHandler(this.BtnMaxLength_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnClearUnit
            // 
            this.BtnClearUnit.BackColor = System.Drawing.Color.Transparent;
            this.BtnClearUnit.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnClearUnit.ButtonText = "CLEAR";
            this.BtnClearUnit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnClearUnit.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnClearUnit.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnClearUnit.Location = new System.Drawing.Point(432, 635);
            this.BtnClearUnit.Name = "BtnClearUnit";
            this.BtnClearUnit.Size = new System.Drawing.Size(203, 98);
            this.BtnClearUnit.TabIndex = 9;
            this.BtnClearUnit.Text = "CLEAR";
            this.BtnClearUnit.UseVisualStyleBackColor = false;
            this.BtnClearUnit.Click += new System.EventHandler(this.BtnClearUnit_Click);
            // 
            // BtnClearOutput
            // 
            this.BtnClearOutput.BackColor = System.Drawing.Color.Transparent;
            this.BtnClearOutput.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnClearOutput.ButtonText = "CLEAR";
            this.BtnClearOutput.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnClearOutput.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnClearOutput.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnClearOutput.Location = new System.Drawing.Point(1061, 635);
            this.BtnClearOutput.Name = "BtnClearOutput";
            this.BtnClearOutput.Size = new System.Drawing.Size(203, 98);
            this.BtnClearOutput.TabIndex = 9;
            this.BtnClearOutput.Text = "CLEAR";
            this.BtnClearOutput.UseVisualStyleBackColor = false;
            this.BtnClearOutput.Click += new System.EventHandler(this.BtnClearOutput_Click);
            // 
            // BtnTitleTactTimeUnitDetail
            // 
            this.BtnTitleTactTimeUnitDetail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleTactTimeUnitDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleTactTimeUnitDetail.Location = new System.Drawing.Point(12, 350);
            this.BtnTitleTactTimeUnitDetail.Name = "BtnTitleTactTimeUnitDetail";
            this.BtnTitleTactTimeUnitDetail.Size = new System.Drawing.Size(623, 46);
            this.BtnTitleTactTimeUnitDetail.TabIndex = 10;
            this.BtnTitleTactTimeUnitDetail.TabStop = false;
            this.BtnTitleTactTimeUnitDetail.Text = "UNIT TACT TIME DETAIL";
            this.BtnTitleTactTimeUnitDetail.UseVisualStyleBackColor = true;
            // 
            // GridViewTactTimeUnitDetail
            // 
            this.GridViewTactTimeUnitDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewTactTimeUnitDetail.Location = new System.Drawing.Point(12, 402);
            this.GridViewTactTimeUnitDetail.Name = "GridViewTactTimeUnitDetail";
            this.GridViewTactTimeUnitDetail.RowTemplate.Height = 23;
            this.GridViewTactTimeUnitDetail.Size = new System.Drawing.Size(623, 227);
            this.GridViewTactTimeUnitDetail.TabIndex = 11;
            // 
            // DgvSimpleOutTactTime
            // 
            this.DgvSimpleOutTactTime.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvSimpleOutTactTime.Location = new System.Drawing.Point(641, 64);
            this.DgvSimpleOutTactTime.Name = "DgvSimpleOutTactTime";
            this.DgvSimpleOutTactTime.RowTemplate.Height = 23;
            this.DgvSimpleOutTactTime.Size = new System.Drawing.Size(623, 565);
            this.DgvSimpleOutTactTime.TabIndex = 12;
            this.DgvSimpleOutTactTime.Visible = false;
            // 
            // BtnOutTactSimpleView
            // 
            this.BtnOutTactSimpleView.BackColor = System.Drawing.Color.Transparent;
            this.BtnOutTactSimpleView.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOutTactSimpleView.ButtonText = "SIMPLE VIEW";
            this.BtnOutTactSimpleView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOutTactSimpleView.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnOutTactSimpleView.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnOutTactSimpleView.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnOutTactSimpleView.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOutTactSimpleView.Location = new System.Drawing.Point(1094, 19);
            this.BtnOutTactSimpleView.Name = "BtnOutTactSimpleView";
            this.BtnOutTactSimpleView.Size = new System.Drawing.Size(161, 32);
            this.BtnOutTactSimpleView.TabIndex = 13;
            this.BtnOutTactSimpleView.Text = "SIMPLE VIEW";
            this.BtnOutTactSimpleView.UseVisualStyleBackColor = false;
            this.BtnOutTactSimpleView.Click += new System.EventHandler(this.BtnOutTactSimpleView_Click);
            // 
            // CFormStatisticsTactTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.Controls.Add(this.BtnOutTactSimpleView);
            this.Controls.Add(this.DgvSimpleOutTactTime);
            this.Controls.Add(this.GridViewTactTimeUnitDetail);
            this.Controls.Add(this.BtnTitleTactTimeUnitDetail);
            this.Controls.Add(this.BtnClearOutput);
            this.Controls.Add(this.BtnClearUnit);
            this.Controls.Add(this.BtnMaxLength);
            this.Controls.Add(this.BtnAverage);
            this.Controls.Add(this.BtnFastUnit);
            this.Controls.Add(this.BtnSlowUnit);
            this.Controls.Add(this.BtnTitleMaxLength);
            this.Controls.Add(this.BtnTitleAverage);
            this.Controls.Add(this.BtnTitleFastUnit);
            this.Controls.Add(this.BtnTitleSlowUnit);
            this.Controls.Add(this.GridViewTactTimeOutput);
            this.Controls.Add(this.GridViewTactTimeUnit);
            this.Controls.Add(this.BtnTitleTactTimeOutput);
            this.Controls.Add(this.BtnTitleTactTimeUnit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormStatisticsTactTime";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormStatisticsTactTime";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormStatisticsTactTime_FormClosed);
            this.Load += new System.EventHandler(this.CFormStatisticsTactTime_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewTactTimeOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewTactTimeUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewTactTimeUnitDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSimpleOutTactTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView GridViewTactTimeOutput;
        private System.Windows.Forms.DataGridView GridViewTactTimeUnit;
        private SpeedButton BtnTitleTactTimeOutput;
        private SpeedButton BtnTitleTactTimeUnit;
        private SpeedButton BtnTitleSlowUnit;
        private SpeedButton BtnSlowUnit;
        private SpeedButton BtnTitleFastUnit;
        private SpeedButton BtnFastUnit;
        private SpeedButton BtnTitleAverage;
        private SpeedButton BtnAverage;
        private SpeedButton BtnTitleMaxLength;
        private SpeedButton BtnMaxLength;
        private System.Windows.Forms.Timer timer;
        private ImageButton BtnClearUnit;
        private ImageButton BtnClearOutput;
        private SpeedButton BtnTitleTactTimeUnitDetail;
        private System.Windows.Forms.DataGridView GridViewTactTimeUnitDetail;
        private System.Windows.Forms.DataGridView DgvSimpleOutTactTime;
        private ImageButton BtnOutTactSimpleView;
    }
}