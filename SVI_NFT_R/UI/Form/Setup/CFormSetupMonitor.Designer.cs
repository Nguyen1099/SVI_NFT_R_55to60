namespace SVI_NFT_R
{
    partial class CFormSetupMonitor
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
            if(disposing && (components != null))
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
            this.PnlCellInfoBase = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnTest = new System.Windows.Forms.Button();
            this.flowLayoutPanelWatchFlagsBase = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnTitleGlobalFlags = new UiAsset.SpeedButton();
            this.flowLayoutPanelWatchFlagsSubLeft = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnFlagDeveloperMode = new UiAsset.SpeedButton();
            this.flowLayoutPanelWatchFlagsSubRight = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnFlagDeveloperModeLoginReady = new UiAsset.SpeedButton();
            this.BtnFlagUnuseAutoLogout = new UiAsset.SpeedButton();
            this.btnVacuumOffAll = new System.Windows.Forms.Button();
            this.btnCellOutAll = new System.Windows.Forms.Button();
            this.btnLowerInterfaceSignalAllClear = new System.Windows.Forms.Button();
            this.btnUpperInterfaceSignalAllClear = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblOverrideSpeedRate = new System.Windows.Forms.Label();
            this.btnOverrideSpeedRateEdit = new System.Windows.Forms.Button();
            this.lblTitleOverrideSpeedRate = new System.Windows.Forms.Label();
            this.lblTodayInput = new System.Windows.Forms.Label();
            this.lblTodayOutput = new System.Windows.Forms.Label();
            this.lblTotalOutput = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblSolenoidDelay = new System.Windows.Forms.Label();
            this.btnSolenoidDelayEdit = new System.Windows.Forms.Button();
            this.lblTitleSolenoidDelay = new System.Windows.Forms.Label();
            this.LblMtbiTime = new System.Windows.Forms.Label();
            this.BtnMtbiFinished = new System.Windows.Forms.Button();
            this.BtnMtbiStart = new System.Windows.Forms.Button();
            this.flowLayoutPanelWatchFlagsBase.SuspendLayout();
            this.flowLayoutPanelWatchFlagsSubLeft.SuspendLayout();
            this.flowLayoutPanelWatchFlagsSubRight.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnTitle
            // 
            this.BtnTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitle.Location = new System.Drawing.Point(12, 12);
            this.BtnTitle.Name = "BtnTitle";
            this.BtnTitle.Size = new System.Drawing.Size(1252, 46);
            this.BtnTitle.TabIndex = 6;
            this.BtnTitle.TabStop = false;
            this.BtnTitle.Text = "MONITOR";
            this.BtnTitle.UseVisualStyleBackColor = true;
            // 
            // PnlCellInfoBase
            // 
            this.PnlCellInfoBase.AutoScroll = true;
            this.PnlCellInfoBase.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.PnlCellInfoBase.Location = new System.Drawing.Point(634, 64);
            this.PnlCellInfoBase.Name = "PnlCellInfoBase";
            this.PnlCellInfoBase.Size = new System.Drawing.Size(630, 460);
            this.PnlCellInfoBase.TabIndex = 8;
            // 
            // BtnTest
            // 
            this.BtnTest.Location = new System.Drawing.Point(653, 686);
            this.BtnTest.Name = "BtnTest";
            this.BtnTest.Size = new System.Drawing.Size(90, 46);
            this.BtnTest.TabIndex = 9;
            this.BtnTest.Text = "TEST";
            this.BtnTest.UseVisualStyleBackColor = true;
            this.BtnTest.Visible = false;
            this.BtnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // flowLayoutPanelWatchFlagsBase
            // 
            this.flowLayoutPanelWatchFlagsBase.Controls.Add(this.BtnTitleGlobalFlags);
            this.flowLayoutPanelWatchFlagsBase.Controls.Add(this.flowLayoutPanelWatchFlagsSubLeft);
            this.flowLayoutPanelWatchFlagsBase.Controls.Add(this.flowLayoutPanelWatchFlagsSubRight);
            this.flowLayoutPanelWatchFlagsBase.Location = new System.Drawing.Point(12, 478);
            this.flowLayoutPanelWatchFlagsBase.Name = "flowLayoutPanelWatchFlagsBase";
            this.flowLayoutPanelWatchFlagsBase.Size = new System.Drawing.Size(616, 255);
            this.flowLayoutPanelWatchFlagsBase.TabIndex = 10;
            // 
            // BtnTitleGlobalFlags
            // 
            this.BtnTitleGlobalFlags.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleGlobalFlags.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleGlobalFlags.Location = new System.Drawing.Point(0, 0);
            this.BtnTitleGlobalFlags.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.BtnTitleGlobalFlags.Name = "BtnTitleGlobalFlags";
            this.BtnTitleGlobalFlags.Size = new System.Drawing.Size(616, 46);
            this.BtnTitleGlobalFlags.TabIndex = 0;
            this.BtnTitleGlobalFlags.TabStop = false;
            this.BtnTitleGlobalFlags.Text = "WATCH SOFTWARE FLAGS";
            this.BtnTitleGlobalFlags.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelWatchFlagsSubLeft
            // 
            this.flowLayoutPanelWatchFlagsSubLeft.Controls.Add(this.BtnFlagDeveloperMode);
            this.flowLayoutPanelWatchFlagsSubLeft.Location = new System.Drawing.Point(0, 52);
            this.flowLayoutPanelWatchFlagsSubLeft.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.flowLayoutPanelWatchFlagsSubLeft.Name = "flowLayoutPanelWatchFlagsSubLeft";
            this.flowLayoutPanelWatchFlagsSubLeft.Size = new System.Drawing.Size(305, 203);
            this.flowLayoutPanelWatchFlagsSubLeft.TabIndex = 1;
            // 
            // BtnFlagDeveloperMode
            // 
            this.BtnFlagDeveloperMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnFlagDeveloperMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFlagDeveloperMode.Location = new System.Drawing.Point(0, 0);
            this.BtnFlagDeveloperMode.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.BtnFlagDeveloperMode.Name = "BtnFlagDeveloperMode";
            this.BtnFlagDeveloperMode.Size = new System.Drawing.Size(305, 46);
            this.BtnFlagDeveloperMode.TabIndex = 0;
            this.BtnFlagDeveloperMode.TabStop = false;
            this.BtnFlagDeveloperMode.Text = "DEVELOPER MODE";
            this.BtnFlagDeveloperMode.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelWatchFlagsSubRight
            // 
            this.flowLayoutPanelWatchFlagsSubRight.Controls.Add(this.BtnFlagDeveloperModeLoginReady);
            this.flowLayoutPanelWatchFlagsSubRight.Controls.Add(this.BtnFlagUnuseAutoLogout);
            this.flowLayoutPanelWatchFlagsSubRight.Location = new System.Drawing.Point(311, 52);
            this.flowLayoutPanelWatchFlagsSubRight.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanelWatchFlagsSubRight.Name = "flowLayoutPanelWatchFlagsSubRight";
            this.flowLayoutPanelWatchFlagsSubRight.Size = new System.Drawing.Size(305, 203);
            this.flowLayoutPanelWatchFlagsSubRight.TabIndex = 2;
            // 
            // BtnFlagDeveloperModeLoginReady
            // 
            this.BtnFlagDeveloperModeLoginReady.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnFlagDeveloperModeLoginReady.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFlagDeveloperModeLoginReady.Location = new System.Drawing.Point(0, 0);
            this.BtnFlagDeveloperModeLoginReady.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.BtnFlagDeveloperModeLoginReady.Name = "BtnFlagDeveloperModeLoginReady";
            this.BtnFlagDeveloperModeLoginReady.Size = new System.Drawing.Size(305, 46);
            this.BtnFlagDeveloperModeLoginReady.TabIndex = 1;
            this.BtnFlagDeveloperModeLoginReady.TabStop = false;
            this.BtnFlagDeveloperModeLoginReady.Text = "DEVELOPER MODE LOGIN READY";
            this.BtnFlagDeveloperModeLoginReady.UseVisualStyleBackColor = true;
            // 
            // BtnFlagUnuseAutoLogout
            // 
            this.BtnFlagUnuseAutoLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnFlagUnuseAutoLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFlagUnuseAutoLogout.Location = new System.Drawing.Point(0, 52);
            this.BtnFlagUnuseAutoLogout.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.BtnFlagUnuseAutoLogout.Name = "BtnFlagUnuseAutoLogout";
            this.BtnFlagUnuseAutoLogout.Size = new System.Drawing.Size(305, 46);
            this.BtnFlagUnuseAutoLogout.TabIndex = 2;
            this.BtnFlagUnuseAutoLogout.TabStop = false;
            this.BtnFlagUnuseAutoLogout.Text = "UNUSE AUTO LOGOUT";
            this.BtnFlagUnuseAutoLogout.UseVisualStyleBackColor = true;
            // 
            // btnVacuumOffAll
            // 
            this.btnVacuumOffAll.Location = new System.Drawing.Point(1097, 633);
            this.btnVacuumOffAll.Name = "btnVacuumOffAll";
            this.btnVacuumOffAll.Size = new System.Drawing.Size(167, 47);
            this.btnVacuumOffAll.TabIndex = 12;
            this.btnVacuumOffAll.Text = "모든 베큠 끄기";
            this.btnVacuumOffAll.UseVisualStyleBackColor = true;
            this.btnVacuumOffAll.Click += new System.EventHandler(this.btnVacuumOffAll_Click);
            // 
            // btnCellOutAll
            // 
            this.btnCellOutAll.Location = new System.Drawing.Point(1097, 686);
            this.btnCellOutAll.Name = "btnCellOutAll";
            this.btnCellOutAll.Size = new System.Drawing.Size(167, 47);
            this.btnCellOutAll.TabIndex = 13;
            this.btnCellOutAll.Text = "모든 셀 정보 삭제";
            this.btnCellOutAll.UseVisualStyleBackColor = true;
            this.btnCellOutAll.Click += new System.EventHandler(this.btnCellOutAll_Click);
            // 
            // btnLowerInterfaceSignalAllClear
            // 
            this.btnLowerInterfaceSignalAllClear.Location = new System.Drawing.Point(924, 686);
            this.btnLowerInterfaceSignalAllClear.Name = "btnLowerInterfaceSignalAllClear";
            this.btnLowerInterfaceSignalAllClear.Size = new System.Drawing.Size(167, 47);
            this.btnLowerInterfaceSignalAllClear.TabIndex = 14;
            this.btnLowerInterfaceSignalAllClear.Text = "하류 인터페이스 신호 강제 클리어";
            this.btnLowerInterfaceSignalAllClear.UseVisualStyleBackColor = true;
            this.btnLowerInterfaceSignalAllClear.Click += new System.EventHandler(this.btnLowerInterfaceSignalAllClear_Click);
            // 
            // btnUpperInterfaceSignalAllClear
            // 
            this.btnUpperInterfaceSignalAllClear.Location = new System.Drawing.Point(924, 633);
            this.btnUpperInterfaceSignalAllClear.Name = "btnUpperInterfaceSignalAllClear";
            this.btnUpperInterfaceSignalAllClear.Size = new System.Drawing.Size(167, 47);
            this.btnUpperInterfaceSignalAllClear.TabIndex = 15;
            this.btnUpperInterfaceSignalAllClear.Text = "상류 인터페이스 신호 강제 클리어";
            this.btnUpperInterfaceSignalAllClear.UseVisualStyleBackColor = true;
            this.btnUpperInterfaceSignalAllClear.Click += new System.EventHandler(this.btnUpperInterfaceSignalAllClear_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblOverrideSpeedRate);
            this.panel1.Controls.Add(this.btnOverrideSpeedRateEdit);
            this.panel1.Controls.Add(this.lblTitleOverrideSpeedRate);
            this.panel1.Location = new System.Drawing.Point(1097, 582);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(167, 47);
            this.panel1.TabIndex = 19;
            // 
            // lblOverrideSpeedRate
            // 
            this.lblOverrideSpeedRate.AutoSize = true;
            this.lblOverrideSpeedRate.Location = new System.Drawing.Point(18, 24);
            this.lblOverrideSpeedRate.Name = "lblOverrideSpeedRate";
            this.lblOverrideSpeedRate.Size = new System.Drawing.Size(58, 15);
            this.lblOverrideSpeedRate.TabIndex = 2;
            this.lblOverrideSpeedRate.Text = "100 ( % )";
            // 
            // btnOverrideSpeedRateEdit
            // 
            this.btnOverrideSpeedRateEdit.Location = new System.Drawing.Point(91, 19);
            this.btnOverrideSpeedRateEdit.Name = "btnOverrideSpeedRateEdit";
            this.btnOverrideSpeedRateEdit.Size = new System.Drawing.Size(74, 26);
            this.btnOverrideSpeedRateEdit.TabIndex = 1;
            this.btnOverrideSpeedRateEdit.Text = "수정";
            this.btnOverrideSpeedRateEdit.UseVisualStyleBackColor = true;
            this.btnOverrideSpeedRateEdit.Click += new System.EventHandler(this.btnOverrideSpeedRateEdit_Click);
            // 
            // lblTitleOverrideSpeedRate
            // 
            this.lblTitleOverrideSpeedRate.AutoSize = true;
            this.lblTitleOverrideSpeedRate.Location = new System.Drawing.Point(3, 2);
            this.lblTitleOverrideSpeedRate.Name = "lblTitleOverrideSpeedRate";
            this.lblTitleOverrideSpeedRate.Size = new System.Drawing.Size(151, 15);
            this.lblTitleOverrideSpeedRate.TabIndex = 0;
            this.lblTitleOverrideSpeedRate.Text = "모터 속도 오버라이드 비율";
            // 
            // lblTodayInput
            // 
            this.lblTodayInput.AutoSize = true;
            this.lblTodayInput.Font = new System.Drawing.Font("굴림", 15.75F);
            this.lblTodayInput.Location = new System.Drawing.Point(12, 64);
            this.lblTodayInput.Name = "lblTodayInput";
            this.lblTodayInput.Size = new System.Drawing.Size(58, 21);
            this.lblTodayInput.TabIndex = 24;
            this.lblTodayInput.Text = "label1";
            this.lblTodayInput.Click += new System.EventHandler(this.lblTodayInput_Click);
            // 
            // lblTodayOutput
            // 
            this.lblTodayOutput.AutoSize = true;
            this.lblTodayOutput.Font = new System.Drawing.Font("굴림", 15.75F);
            this.lblTodayOutput.Location = new System.Drawing.Point(12, 105);
            this.lblTodayOutput.Name = "lblTodayOutput";
            this.lblTodayOutput.Size = new System.Drawing.Size(58, 21);
            this.lblTodayOutput.TabIndex = 25;
            this.lblTodayOutput.Text = "label2";
            this.lblTodayOutput.Click += new System.EventHandler(this.lblTodayOutput_Click);
            // 
            // lblTotalOutput
            // 
            this.lblTotalOutput.AutoSize = true;
            this.lblTotalOutput.Font = new System.Drawing.Font("굴림", 15.75F);
            this.lblTotalOutput.Location = new System.Drawing.Point(12, 146);
            this.lblTotalOutput.Name = "lblTotalOutput";
            this.lblTotalOutput.Size = new System.Drawing.Size(58, 21);
            this.lblTotalOutput.TabIndex = 26;
            this.lblTotalOutput.Text = "label3";
            this.lblTotalOutput.Click += new System.EventHandler(this.lblTotalOutput_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblSolenoidDelay);
            this.panel2.Controls.Add(this.btnSolenoidDelayEdit);
            this.panel2.Controls.Add(this.lblTitleSolenoidDelay);
            this.panel2.Location = new System.Drawing.Point(924, 582);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(167, 47);
            this.panel2.TabIndex = 19;
            // 
            // lblSolenoidDelay
            // 
            this.lblSolenoidDelay.AutoSize = true;
            this.lblSolenoidDelay.Location = new System.Drawing.Point(18, 24);
            this.lblSolenoidDelay.Name = "lblSolenoidDelay";
            this.lblSolenoidDelay.Size = new System.Drawing.Size(57, 15);
            this.lblSolenoidDelay.TabIndex = 2;
            this.lblSolenoidDelay.Text = "50 ( ms )";
            // 
            // btnSolenoidDelayEdit
            // 
            this.btnSolenoidDelayEdit.Location = new System.Drawing.Point(91, 19);
            this.btnSolenoidDelayEdit.Name = "btnSolenoidDelayEdit";
            this.btnSolenoidDelayEdit.Size = new System.Drawing.Size(74, 26);
            this.btnSolenoidDelayEdit.TabIndex = 1;
            this.btnSolenoidDelayEdit.Text = "수정";
            this.btnSolenoidDelayEdit.UseVisualStyleBackColor = true;
            this.btnSolenoidDelayEdit.Click += new System.EventHandler(this.btnSolenoidDelayEdit_Click);
            // 
            // lblTitleSolenoidDelay
            // 
            this.lblTitleSolenoidDelay.AutoSize = true;
            this.lblTitleSolenoidDelay.Location = new System.Drawing.Point(3, 2);
            this.lblTitleSolenoidDelay.Name = "lblTitleSolenoidDelay";
            this.lblTitleSolenoidDelay.Size = new System.Drawing.Size(135, 15);
            this.lblTitleSolenoidDelay.TabIndex = 0;
            this.lblTitleSolenoidDelay.Text = "진공 솔레노이드 딜레이";
            // 
            // LblMtbiTime
            // 
            this.LblMtbiTime.AutoSize = true;
            this.LblMtbiTime.Location = new System.Drawing.Point(747, 547);
            this.LblMtbiTime.Name = "LblMtbiTime";
            this.LblMtbiTime.Size = new System.Drawing.Size(12, 15);
            this.LblMtbiTime.TabIndex = 33;
            this.LblMtbiTime.Text = "-";
            // 
            // BtnMtbiFinished
            // 
            this.BtnMtbiFinished.Location = new System.Drawing.Point(749, 633);
            this.BtnMtbiFinished.Name = "BtnMtbiFinished";
            this.BtnMtbiFinished.Size = new System.Drawing.Size(167, 46);
            this.BtnMtbiFinished.TabIndex = 31;
            this.BtnMtbiFinished.Text = "MTBi 완료\r\n(데이터 저장)";
            this.BtnMtbiFinished.UseVisualStyleBackColor = true;
            this.BtnMtbiFinished.Click += new System.EventHandler(this.BtnMtbiFinished_Click);
            // 
            // BtnMtbiStart
            // 
            this.BtnMtbiStart.Location = new System.Drawing.Point(749, 581);
            this.BtnMtbiStart.Name = "BtnMtbiStart";
            this.BtnMtbiStart.Size = new System.Drawing.Size(167, 46);
            this.BtnMtbiStart.TabIndex = 32;
            this.BtnMtbiStart.Text = "MTBi 준비\r\n(데이터 수집 시작)";
            this.BtnMtbiStart.UseVisualStyleBackColor = true;
            this.BtnMtbiStart.Click += new System.EventHandler(this.BtnMtbiStart_Click);
            // 
            // CFormSetupMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.Controls.Add(this.LblMtbiTime);
            this.Controls.Add(this.BtnMtbiFinished);
            this.Controls.Add(this.BtnMtbiStart);
            this.Controls.Add(this.lblTotalOutput);
            this.Controls.Add(this.lblTodayOutput);
            this.Controls.Add(this.lblTodayInput);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnUpperInterfaceSignalAllClear);
            this.Controls.Add(this.btnLowerInterfaceSignalAllClear);
            this.Controls.Add(this.btnCellOutAll);
            this.Controls.Add(this.btnVacuumOffAll);
            this.Controls.Add(this.flowLayoutPanelWatchFlagsBase);
            this.Controls.Add(this.PnlCellInfoBase);
            this.Controls.Add(this.BtnTitle);
            this.Controls.Add(this.BtnTest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormSetupMonitor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormSetupMonitor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormSetupMonitor_FormClosed);
            this.Load += new System.EventHandler(this.CFormSetupMonitor_Load);
            this.flowLayoutPanelWatchFlagsBase.ResumeLayout(false);
            this.flowLayoutPanelWatchFlagsSubLeft.ResumeLayout(false);
            this.flowLayoutPanelWatchFlagsSubRight.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private UiAsset.SpeedButton BtnTitle;
        private System.Windows.Forms.FlowLayoutPanel PnlCellInfoBase;
        private System.Windows.Forms.Button BtnTest;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelWatchFlagsBase;
        private UiAsset.SpeedButton BtnTitleGlobalFlags;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelWatchFlagsSubLeft;
        private UiAsset.SpeedButton BtnFlagDeveloperMode;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelWatchFlagsSubRight;
        private UiAsset.SpeedButton BtnFlagDeveloperModeLoginReady;
        private UiAsset.SpeedButton BtnFlagUnuseAutoLogout;
        private System.Windows.Forms.Button btnVacuumOffAll;
        private System.Windows.Forms.Button btnCellOutAll;
        private System.Windows.Forms.Button btnLowerInterfaceSignalAllClear;
        private System.Windows.Forms.Button btnUpperInterfaceSignalAllClear;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblOverrideSpeedRate;
        private System.Windows.Forms.Button btnOverrideSpeedRateEdit;
        private System.Windows.Forms.Label lblTitleOverrideSpeedRate;
        private System.Windows.Forms.Label lblTodayInput;
        private System.Windows.Forms.Label lblTodayOutput;
        private System.Windows.Forms.Label lblTotalOutput;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblSolenoidDelay;
        private System.Windows.Forms.Button btnSolenoidDelayEdit;
        private System.Windows.Forms.Label lblTitleSolenoidDelay;
        private System.Windows.Forms.Label LblMtbiTime;
        private System.Windows.Forms.Button BtnMtbiFinished;
        private System.Windows.Forms.Button BtnMtbiStart;
    }
}