namespace SVI_NFT_R
{
    partial class CFormSetupMotion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
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
            this.BtnSave = new UiAsset.ImageButton();
            this.BtnServoOff = new UiAsset.ImageButton();
            this.BtnPositionInformationNext = new UiAsset.ImageButton();
            this.BtnMotorInformationNext = new UiAsset.ImageButton();
            this.BtnPositionInformationPrevious = new UiAsset.ImageButton();
            this.BtnMotorInformationPrevious = new UiAsset.ImageButton();
            this.BtnServoOn = new UiAsset.ImageButton();
            this.BtnServoReset = new UiAsset.ImageButton();
            this.BtnOrigin = new UiAsset.ImageButton();
            this.BtnJogMoveMinus = new UiAsset.ImageButton();
            this.BtnJogMovePlus = new UiAsset.ImageButton();
            this.BtnRelativeMoveMinus = new UiAsset.ImageButton();
            this.BtnRelativeMovePlus = new UiAsset.ImageButton();
            this.BtnPositionMove = new UiAsset.ImageButton();
            this.BtnPositionSave = new UiAsset.ImageButton();
            this.BtnStop = new UiAsset.ImageButton();
            this.BtnCommandPosition = new UiAsset.SpeedButton();
            this.BtnCurrentPosition = new UiAsset.SpeedButton();
            this.BtnRelativePosition = new UiAsset.SpeedButton();
            this.BtnTitleRelativePosition = new UiAsset.SpeedButton();
            this.BtnTitleCommandPosition = new UiAsset.SpeedButton();
            this.BtnTitleCurrentPosition = new UiAsset.SpeedButton();
            this.BtnTitleUseHome = new UiAsset.SpeedButton();
            this.BtnUseHome = new UiAsset.SpeedButton();
            this.BtnTitleUseMotor = new UiAsset.SpeedButton();
            this.BtnUseMotor = new UiAsset.SpeedButton();
            this.BtnTitleInnerNo = new UiAsset.SpeedButton();
            this.BtnInnerNo = new UiAsset.SpeedButton();
            this.BtnTitleLimitPositionMinus = new UiAsset.SpeedButton();
            this.BtnLimitPositionMinus = new UiAsset.SpeedButton();
            this.BtnTitleLimitPositionPlus = new UiAsset.SpeedButton();
            this.BtnLimitPositionPlus = new UiAsset.SpeedButton();
            this.BtnTitleOriginSpeed = new UiAsset.SpeedButton();
            this.BtnOriginSpeed = new UiAsset.SpeedButton();
            this.BtnTitleStandardTimeOut = new UiAsset.SpeedButton();
            this.BtnTitleStandardTolerance = new UiAsset.SpeedButton();
            this.BtnTitleStandardDeceleration = new UiAsset.SpeedButton();
            this.BtnTitleManualSpeed = new UiAsset.SpeedButton();
            this.BtnTitleStandardAcceleration = new UiAsset.SpeedButton();
            this.BtnStandardTimeOut = new UiAsset.SpeedButton();
            this.BtnStandardTolerance = new UiAsset.SpeedButton();
            this.BtnTitleAutoSpeed = new UiAsset.SpeedButton();
            this.BtnStandardDeceleration = new UiAsset.SpeedButton();
            this.BtnStandardAcceleration = new UiAsset.SpeedButton();
            this.BtnManualSpeed = new UiAsset.SpeedButton();
            this.BtnAutoSpeed = new UiAsset.SpeedButton();
            this.BtnTitleJogSlow = new UiAsset.SpeedButton();
            this.BtnJogSlow = new UiAsset.SpeedButton();
            this.BtnPositionInformationPage = new UiAsset.SpeedButton();
            this.BtnMotorInformationPage = new UiAsset.SpeedButton();
            this.BtnTitleJogFast = new UiAsset.SpeedButton();
            this.BtnJogFast = new UiAsset.SpeedButton();
            this.GridViewPositionInformationList = new System.Windows.Forms.DataGridView();
            this.GridViewMotorInformationList = new System.Windows.Forms.DataGridView();
            this.BtnTitleMotorSetting = new UiAsset.SpeedButton();
            this.BtnTitlePositionInformation = new UiAsset.SpeedButton();
            this.BtnTitleMotorInformation = new UiAsset.SpeedButton();
            this.BtnTitleDelayAfterMoving = new UiAsset.SpeedButton();
            this.BtnDelayAfterMoving = new UiAsset.SpeedButton();
            this.BtnRepeatMove = new UiAsset.ImageButton();
            this.BtnOriginOffset = new UiAsset.SpeedButton();
            this.BtnTitleOriginOffset = new UiAsset.SpeedButton();
            this.BtnPositionJog = new UiAsset.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewPositionInformationList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewMotorInformationList)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.Transparent;
            this.BtnSave.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSave.ButtonText = "SAVE";
            this.BtnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSave.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSave.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSave.Location = new System.Drawing.Point(1156, 637);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(108, 97);
            this.BtnSave.TabIndex = 9;
            this.BtnSave.Text = "SAVE";
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnServoOff
            // 
            this.BtnServoOff.BackColor = System.Drawing.Color.Transparent;
            this.BtnServoOff.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnServoOff.ButtonText = "SERVO OFF";
            this.BtnServoOff.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnServoOff.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnServoOff.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnServoOff.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnServoOff.Location = new System.Drawing.Point(173, 637);
            this.BtnServoOff.Name = "BtnServoOff";
            this.BtnServoOff.Size = new System.Drawing.Size(155, 47);
            this.BtnServoOff.TabIndex = 9;
            this.BtnServoOff.Text = "SERVO OFF";
            this.BtnServoOff.UseVisualStyleBackColor = false;
            this.BtnServoOff.Click += new System.EventHandler(this.BtnServoOff_Click);
            // 
            // BtnPositionInformationNext
            // 
            this.BtnPositionInformationNext.BackColor = System.Drawing.Color.Transparent;
            this.BtnPositionInformationNext.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionInformationNext.ButtonText = "NEXT";
            this.BtnPositionInformationNext.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnPositionInformationNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPositionInformationNext.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPositionInformationNext.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionInformationNext.Location = new System.Drawing.Point(881, 582);
            this.BtnPositionInformationNext.Name = "BtnPositionInformationNext";
            this.BtnPositionInformationNext.Size = new System.Drawing.Size(155, 48);
            this.BtnPositionInformationNext.TabIndex = 9;
            this.BtnPositionInformationNext.Text = "NEXT";
            this.BtnPositionInformationNext.UseVisualStyleBackColor = false;
            this.BtnPositionInformationNext.Click += new System.EventHandler(this.BtnPositionInformationNext_Click);
            // 
            // BtnMotorInformationNext
            // 
            this.BtnMotorInformationNext.BackColor = System.Drawing.Color.Transparent;
            this.BtnMotorInformationNext.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnMotorInformationNext.ButtonText = "NEXT";
            this.BtnMotorInformationNext.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnMotorInformationNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnMotorInformationNext.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnMotorInformationNext.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnMotorInformationNext.Location = new System.Drawing.Point(398, 582);
            this.BtnMotorInformationNext.Name = "BtnMotorInformationNext";
            this.BtnMotorInformationNext.Size = new System.Drawing.Size(155, 48);
            this.BtnMotorInformationNext.TabIndex = 9;
            this.BtnMotorInformationNext.Text = "NEXT";
            this.BtnMotorInformationNext.UseVisualStyleBackColor = false;
            this.BtnMotorInformationNext.Click += new System.EventHandler(this.BtnMotorInformationNext_Click);
            // 
            // BtnPositionInformationPrevious
            // 
            this.BtnPositionInformationPrevious.BackColor = System.Drawing.Color.Transparent;
            this.BtnPositionInformationPrevious.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionInformationPrevious.ButtonText = "PREVIOUS";
            this.BtnPositionInformationPrevious.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnPositionInformationPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPositionInformationPrevious.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPositionInformationPrevious.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionInformationPrevious.Location = new System.Drawing.Point(559, 582);
            this.BtnPositionInformationPrevious.Name = "BtnPositionInformationPrevious";
            this.BtnPositionInformationPrevious.Size = new System.Drawing.Size(155, 48);
            this.BtnPositionInformationPrevious.TabIndex = 9;
            this.BtnPositionInformationPrevious.Text = "PREVIOUS";
            this.BtnPositionInformationPrevious.UseVisualStyleBackColor = false;
            this.BtnPositionInformationPrevious.Click += new System.EventHandler(this.BtnPositionInformationPrevious_Click);
            // 
            // BtnMotorInformationPrevious
            // 
            this.BtnMotorInformationPrevious.BackColor = System.Drawing.Color.Transparent;
            this.BtnMotorInformationPrevious.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnMotorInformationPrevious.ButtonText = "PREVIOUS";
            this.BtnMotorInformationPrevious.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnMotorInformationPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnMotorInformationPrevious.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnMotorInformationPrevious.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnMotorInformationPrevious.Location = new System.Drawing.Point(12, 582);
            this.BtnMotorInformationPrevious.Name = "BtnMotorInformationPrevious";
            this.BtnMotorInformationPrevious.Size = new System.Drawing.Size(155, 48);
            this.BtnMotorInformationPrevious.TabIndex = 9;
            this.BtnMotorInformationPrevious.Text = "PREVIOUS";
            this.BtnMotorInformationPrevious.UseVisualStyleBackColor = false;
            this.BtnMotorInformationPrevious.Click += new System.EventHandler(this.BtnMotorInformationPrevious_Click);
            // 
            // BtnServoOn
            // 
            this.BtnServoOn.BackColor = System.Drawing.Color.Transparent;
            this.BtnServoOn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnServoOn.ButtonText = "SERVO ON";
            this.BtnServoOn.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnServoOn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnServoOn.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnServoOn.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnServoOn.Location = new System.Drawing.Point(12, 637);
            this.BtnServoOn.Name = "BtnServoOn";
            this.BtnServoOn.Size = new System.Drawing.Size(155, 47);
            this.BtnServoOn.TabIndex = 9;
            this.BtnServoOn.Text = "SERVO ON";
            this.BtnServoOn.UseVisualStyleBackColor = false;
            this.BtnServoOn.Click += new System.EventHandler(this.BtnServoOn_Click);
            // 
            // BtnServoReset
            // 
            this.BtnServoReset.BackColor = System.Drawing.Color.Transparent;
            this.BtnServoReset.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnServoReset.ButtonText = "SERVO RESET";
            this.BtnServoReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnServoReset.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnServoReset.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnServoReset.Location = new System.Drawing.Point(12, 687);
            this.BtnServoReset.Name = "BtnServoReset";
            this.BtnServoReset.Size = new System.Drawing.Size(155, 47);
            this.BtnServoReset.TabIndex = 9;
            this.BtnServoReset.Text = "SERVO RESET";
            this.BtnServoReset.UseVisualStyleBackColor = false;
            this.BtnServoReset.Click += new System.EventHandler(this.BtnServoReset_Click);
            // 
            // BtnOrigin
            // 
            this.BtnOrigin.BackColor = System.Drawing.Color.Transparent;
            this.BtnOrigin.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOrigin.ButtonText = "ORIGIN";
            this.BtnOrigin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOrigin.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnOrigin.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnOrigin.Location = new System.Drawing.Point(173, 687);
            this.BtnOrigin.Name = "BtnOrigin";
            this.BtnOrigin.Size = new System.Drawing.Size(155, 47);
            this.BtnOrigin.TabIndex = 9;
            this.BtnOrigin.Text = "ORIGIN";
            this.BtnOrigin.UseVisualStyleBackColor = false;
            this.BtnOrigin.Click += new System.EventHandler(this.BtnOrigin_Click);
            // 
            // BtnJogMoveMinus
            // 
            this.BtnJogMoveMinus.BackColor = System.Drawing.Color.Transparent;
            this.BtnJogMoveMinus.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnJogMoveMinus.ButtonText = "JOG MOVE ( - )";
            this.BtnJogMoveMinus.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnJogMoveMinus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnJogMoveMinus.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnJogMoveMinus.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnJogMoveMinus.Location = new System.Drawing.Point(559, 687);
            this.BtnJogMoveMinus.Name = "BtnJogMoveMinus";
            this.BtnJogMoveMinus.Size = new System.Drawing.Size(155, 47);
            this.BtnJogMoveMinus.TabIndex = 9;
            this.BtnJogMoveMinus.Text = "JOG MOVE ( - )";
            this.BtnJogMoveMinus.UseVisualStyleBackColor = false;
            this.BtnJogMoveMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnJogMoveMinus_MouseDown);
            this.BtnJogMoveMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnJogMoveMinus_MouseUp);
            // 
            // BtnJogMovePlus
            // 
            this.BtnJogMovePlus.BackColor = System.Drawing.Color.Transparent;
            this.BtnJogMovePlus.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnJogMovePlus.ButtonText = "JOG MOVE ( + )";
            this.BtnJogMovePlus.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnJogMovePlus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnJogMovePlus.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnJogMovePlus.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnJogMovePlus.Location = new System.Drawing.Point(720, 687);
            this.BtnJogMovePlus.Name = "BtnJogMovePlus";
            this.BtnJogMovePlus.Size = new System.Drawing.Size(155, 47);
            this.BtnJogMovePlus.TabIndex = 9;
            this.BtnJogMovePlus.Text = "JOG MOVE ( + )";
            this.BtnJogMovePlus.UseVisualStyleBackColor = false;
            this.BtnJogMovePlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnJogMovePlus_MouseDown);
            this.BtnJogMovePlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnJogMovePlus_MouseUp);
            // 
            // BtnRelativeMoveMinus
            // 
            this.BtnRelativeMoveMinus.BackColor = System.Drawing.Color.Transparent;
            this.BtnRelativeMoveMinus.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnRelativeMoveMinus.ButtonText = "RELATIVE MOVE ( - )";
            this.BtnRelativeMoveMinus.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnRelativeMoveMinus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnRelativeMoveMinus.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnRelativeMoveMinus.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnRelativeMoveMinus.Location = new System.Drawing.Point(559, 637);
            this.BtnRelativeMoveMinus.Name = "BtnRelativeMoveMinus";
            this.BtnRelativeMoveMinus.Size = new System.Drawing.Size(155, 47);
            this.BtnRelativeMoveMinus.TabIndex = 9;
            this.BtnRelativeMoveMinus.Text = "RELATIVE MOVE ( - )";
            this.BtnRelativeMoveMinus.UseVisualStyleBackColor = false;
            this.BtnRelativeMoveMinus.Click += new System.EventHandler(this.BtnRelativeMoveMinus_Click);
            // 
            // BtnRelativeMovePlus
            // 
            this.BtnRelativeMovePlus.BackColor = System.Drawing.Color.Transparent;
            this.BtnRelativeMovePlus.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnRelativeMovePlus.ButtonText = "RELATIVE MOVE ( + )";
            this.BtnRelativeMovePlus.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnRelativeMovePlus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnRelativeMovePlus.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnRelativeMovePlus.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnRelativeMovePlus.Location = new System.Drawing.Point(720, 637);
            this.BtnRelativeMovePlus.Name = "BtnRelativeMovePlus";
            this.BtnRelativeMovePlus.Size = new System.Drawing.Size(155, 47);
            this.BtnRelativeMovePlus.TabIndex = 9;
            this.BtnRelativeMovePlus.Text = "RELATIVE MOVE ( + )";
            this.BtnRelativeMovePlus.UseVisualStyleBackColor = false;
            this.BtnRelativeMovePlus.Click += new System.EventHandler(this.BtnRelativeMovePlus_Click);
            // 
            // BtnPositionMove
            // 
            this.BtnPositionMove.BackColor = System.Drawing.Color.Transparent;
            this.BtnPositionMove.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionMove.ButtonText = "POSITION MOVE";
            this.BtnPositionMove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPositionMove.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPositionMove.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionMove.Location = new System.Drawing.Point(881, 637);
            this.BtnPositionMove.Name = "BtnPositionMove";
            this.BtnPositionMove.Size = new System.Drawing.Size(155, 47);
            this.BtnPositionMove.TabIndex = 9;
            this.BtnPositionMove.Text = "POSITION MOVE";
            this.BtnPositionMove.UseVisualStyleBackColor = false;
            this.BtnPositionMove.Click += new System.EventHandler(this.BtnPositionMove_Click);
            // 
            // BtnPositionSave
            // 
            this.BtnPositionSave.BackColor = System.Drawing.Color.Transparent;
            this.BtnPositionSave.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionSave.ButtonText = "POSITION SAVE";
            this.BtnPositionSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPositionSave.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPositionSave.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionSave.Location = new System.Drawing.Point(881, 687);
            this.BtnPositionSave.Name = "BtnPositionSave";
            this.BtnPositionSave.Size = new System.Drawing.Size(155, 47);
            this.BtnPositionSave.TabIndex = 9;
            this.BtnPositionSave.Text = "POSITION SAVE";
            this.BtnPositionSave.UseVisualStyleBackColor = false;
            this.BtnPositionSave.Click += new System.EventHandler(this.BtnPositionSave_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.BackColor = System.Drawing.Color.Transparent;
            this.BtnStop.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnStop.ButtonText = "STOP";
            this.BtnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnStop.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnStop.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnStop.Location = new System.Drawing.Point(1042, 687);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(108, 47);
            this.BtnStop.TabIndex = 9;
            this.BtnStop.Text = "STOP";
            this.BtnStop.UseVisualStyleBackColor = false;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // BtnCommandPosition
            // 
            this.BtnCommandPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCommandPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCommandPosition.Location = new System.Drawing.Point(447, 670);
            this.BtnCommandPosition.Name = "BtnCommandPosition";
            this.BtnCommandPosition.Size = new System.Drawing.Size(106, 29);
            this.BtnCommandPosition.TabIndex = 7;
            this.BtnCommandPosition.TabStop = false;
            this.BtnCommandPosition.Text = "0";
            this.BtnCommandPosition.UseVisualStyleBackColor = true;
            // 
            // BtnCurrentPosition
            // 
            this.BtnCurrentPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCurrentPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCurrentPosition.Location = new System.Drawing.Point(447, 636);
            this.BtnCurrentPosition.Name = "BtnCurrentPosition";
            this.BtnCurrentPosition.Size = new System.Drawing.Size(106, 29);
            this.BtnCurrentPosition.TabIndex = 7;
            this.BtnCurrentPosition.TabStop = false;
            this.BtnCurrentPosition.Text = "0";
            this.BtnCurrentPosition.UseVisualStyleBackColor = true;
            this.BtnCurrentPosition.Click += new System.EventHandler(this.BtnCurrentPosition_Click);
            // 
            // BtnRelativePosition
            // 
            this.BtnRelativePosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnRelativePosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRelativePosition.Location = new System.Drawing.Point(447, 704);
            this.BtnRelativePosition.Name = "BtnRelativePosition";
            this.BtnRelativePosition.Size = new System.Drawing.Size(106, 29);
            this.BtnRelativePosition.TabIndex = 7;
            this.BtnRelativePosition.TabStop = false;
            this.BtnRelativePosition.Text = "0";
            this.BtnRelativePosition.UseVisualStyleBackColor = true;
            this.BtnRelativePosition.Click += new System.EventHandler(this.BtnRelativePosition_Click);
            // 
            // BtnTitleRelativePosition
            // 
            this.BtnTitleRelativePosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleRelativePosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleRelativePosition.Location = new System.Drawing.Point(334, 704);
            this.BtnTitleRelativePosition.Name = "BtnTitleRelativePosition";
            this.BtnTitleRelativePosition.Size = new System.Drawing.Size(107, 29);
            this.BtnTitleRelativePosition.TabIndex = 7;
            this.BtnTitleRelativePosition.TabStop = false;
            this.BtnTitleRelativePosition.Text = "REL POS";
            this.BtnTitleRelativePosition.UseVisualStyleBackColor = true;
            // 
            // BtnTitleCommandPosition
            // 
            this.BtnTitleCommandPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleCommandPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleCommandPosition.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleCommandPosition.Location = new System.Drawing.Point(334, 670);
            this.BtnTitleCommandPosition.Name = "BtnTitleCommandPosition";
            this.BtnTitleCommandPosition.Size = new System.Drawing.Size(107, 29);
            this.BtnTitleCommandPosition.TabIndex = 7;
            this.BtnTitleCommandPosition.TabStop = false;
            this.BtnTitleCommandPosition.Text = "COM POS";
            this.BtnTitleCommandPosition.UseVisualStyleBackColor = true;
            // 
            // BtnTitleCurrentPosition
            // 
            this.BtnTitleCurrentPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleCurrentPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleCurrentPosition.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleCurrentPosition.Location = new System.Drawing.Point(334, 636);
            this.BtnTitleCurrentPosition.Name = "BtnTitleCurrentPosition";
            this.BtnTitleCurrentPosition.Size = new System.Drawing.Size(107, 29);
            this.BtnTitleCurrentPosition.TabIndex = 7;
            this.BtnTitleCurrentPosition.TabStop = false;
            this.BtnTitleCurrentPosition.Text = "CUR POS";
            this.BtnTitleCurrentPosition.UseVisualStyleBackColor = true;
            // 
            // BtnTitleUseHome
            // 
            this.BtnTitleUseHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleUseHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleUseHome.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleUseHome.Location = new System.Drawing.Point(1042, 558);
            this.BtnTitleUseHome.Name = "BtnTitleUseHome";
            this.BtnTitleUseHome.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleUseHome.TabIndex = 7;
            this.BtnTitleUseHome.TabStop = false;
            this.BtnTitleUseHome.Text = "HOME USE";
            this.BtnTitleUseHome.UseVisualStyleBackColor = true;
            // 
            // BtnUseHome
            // 
            this.BtnUseHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnUseHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnUseHome.Location = new System.Drawing.Point(1156, 558);
            this.BtnUseHome.Name = "BtnUseHome";
            this.BtnUseHome.Size = new System.Drawing.Size(108, 34);
            this.BtnUseHome.TabIndex = 8;
            this.BtnUseHome.TabStop = false;
            this.BtnUseHome.Text = "true";
            this.BtnUseHome.UseVisualStyleBackColor = true;
            this.BtnUseHome.Click += new System.EventHandler(this.BtnUseHome_Click);
            // 
            // BtnTitleUseMotor
            // 
            this.BtnTitleUseMotor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleUseMotor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleUseMotor.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleUseMotor.Location = new System.Drawing.Point(1042, 596);
            this.BtnTitleUseMotor.Name = "BtnTitleUseMotor";
            this.BtnTitleUseMotor.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleUseMotor.TabIndex = 7;
            this.BtnTitleUseMotor.TabStop = false;
            this.BtnTitleUseMotor.Text = "USE MOTOR";
            this.BtnTitleUseMotor.UseVisualStyleBackColor = true;
            // 
            // BtnUseMotor
            // 
            this.BtnUseMotor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnUseMotor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnUseMotor.Location = new System.Drawing.Point(1156, 596);
            this.BtnUseMotor.Name = "BtnUseMotor";
            this.BtnUseMotor.Size = new System.Drawing.Size(108, 34);
            this.BtnUseMotor.TabIndex = 8;
            this.BtnUseMotor.TabStop = false;
            this.BtnUseMotor.Text = "true";
            this.BtnUseMotor.UseVisualStyleBackColor = true;
            // 
            // BtnTitleInnerNo
            // 
            this.BtnTitleInnerNo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleInnerNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleInnerNo.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleInnerNo.Location = new System.Drawing.Point(1042, 520);
            this.BtnTitleInnerNo.Name = "BtnTitleInnerNo";
            this.BtnTitleInnerNo.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleInnerNo.TabIndex = 7;
            this.BtnTitleInnerNo.TabStop = false;
            this.BtnTitleInnerNo.Text = "INNER NO";
            this.BtnTitleInnerNo.UseVisualStyleBackColor = true;
            // 
            // BtnInnerNo
            // 
            this.BtnInnerNo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnInnerNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnInnerNo.Location = new System.Drawing.Point(1156, 520);
            this.BtnInnerNo.Name = "BtnInnerNo";
            this.BtnInnerNo.Size = new System.Drawing.Size(108, 34);
            this.BtnInnerNo.TabIndex = 8;
            this.BtnInnerNo.TabStop = false;
            this.BtnInnerNo.Text = "0";
            this.BtnInnerNo.UseVisualStyleBackColor = true;
            this.BtnInnerNo.Click += new System.EventHandler(this.BtnInnerNo_Click);
            // 
            // BtnTitleLimitPositionMinus
            // 
            this.BtnTitleLimitPositionMinus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleLimitPositionMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleLimitPositionMinus.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleLimitPositionMinus.Location = new System.Drawing.Point(1042, 444);
            this.BtnTitleLimitPositionMinus.Name = "BtnTitleLimitPositionMinus";
            this.BtnTitleLimitPositionMinus.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleLimitPositionMinus.TabIndex = 7;
            this.BtnTitleLimitPositionMinus.TabStop = false;
            this.BtnTitleLimitPositionMinus.Text = "LIMIT POSITION ( - )";
            this.BtnTitleLimitPositionMinus.UseVisualStyleBackColor = true;
            // 
            // BtnLimitPositionMinus
            // 
            this.BtnLimitPositionMinus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnLimitPositionMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLimitPositionMinus.Location = new System.Drawing.Point(1156, 444);
            this.BtnLimitPositionMinus.Name = "BtnLimitPositionMinus";
            this.BtnLimitPositionMinus.Size = new System.Drawing.Size(108, 34);
            this.BtnLimitPositionMinus.TabIndex = 8;
            this.BtnLimitPositionMinus.TabStop = false;
            this.BtnLimitPositionMinus.Text = "0";
            this.BtnLimitPositionMinus.UseVisualStyleBackColor = true;
            this.BtnLimitPositionMinus.Click += new System.EventHandler(this.BtnLimitPositionMinus_Click);
            // 
            // BtnTitleLimitPositionPlus
            // 
            this.BtnTitleLimitPositionPlus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleLimitPositionPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleLimitPositionPlus.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleLimitPositionPlus.Location = new System.Drawing.Point(1042, 406);
            this.BtnTitleLimitPositionPlus.Name = "BtnTitleLimitPositionPlus";
            this.BtnTitleLimitPositionPlus.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleLimitPositionPlus.TabIndex = 7;
            this.BtnTitleLimitPositionPlus.TabStop = false;
            this.BtnTitleLimitPositionPlus.Text = "LIMIT POSITION ( + )";
            this.BtnTitleLimitPositionPlus.UseVisualStyleBackColor = true;
            // 
            // BtnLimitPositionPlus
            // 
            this.BtnLimitPositionPlus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnLimitPositionPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLimitPositionPlus.Location = new System.Drawing.Point(1156, 406);
            this.BtnLimitPositionPlus.Name = "BtnLimitPositionPlus";
            this.BtnLimitPositionPlus.Size = new System.Drawing.Size(108, 34);
            this.BtnLimitPositionPlus.TabIndex = 8;
            this.BtnLimitPositionPlus.TabStop = false;
            this.BtnLimitPositionPlus.Text = "0";
            this.BtnLimitPositionPlus.UseVisualStyleBackColor = true;
            this.BtnLimitPositionPlus.Click += new System.EventHandler(this.BtnLimitPositionPlus_Click);
            // 
            // BtnTitleOriginSpeed
            // 
            this.BtnTitleOriginSpeed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleOriginSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleOriginSpeed.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleOriginSpeed.Location = new System.Drawing.Point(1042, 330);
            this.BtnTitleOriginSpeed.Name = "BtnTitleOriginSpeed";
            this.BtnTitleOriginSpeed.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleOriginSpeed.TabIndex = 7;
            this.BtnTitleOriginSpeed.TabStop = false;
            this.BtnTitleOriginSpeed.Text = "ORIGIN SPEED";
            this.BtnTitleOriginSpeed.UseVisualStyleBackColor = true;
            // 
            // BtnOriginSpeed
            // 
            this.BtnOriginSpeed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOriginSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOriginSpeed.Location = new System.Drawing.Point(1156, 330);
            this.BtnOriginSpeed.Name = "BtnOriginSpeed";
            this.BtnOriginSpeed.Size = new System.Drawing.Size(108, 34);
            this.BtnOriginSpeed.TabIndex = 8;
            this.BtnOriginSpeed.TabStop = false;
            this.BtnOriginSpeed.Text = "0";
            this.BtnOriginSpeed.UseVisualStyleBackColor = true;
            this.BtnOriginSpeed.Click += new System.EventHandler(this.BtnOriginSpeed_Click);
            // 
            // BtnTitleStandardTimeOut
            // 
            this.BtnTitleStandardTimeOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleStandardTimeOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleStandardTimeOut.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleStandardTimeOut.Location = new System.Drawing.Point(1042, 292);
            this.BtnTitleStandardTimeOut.Name = "BtnTitleStandardTimeOut";
            this.BtnTitleStandardTimeOut.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleStandardTimeOut.TabIndex = 7;
            this.BtnTitleStandardTimeOut.TabStop = false;
            this.BtnTitleStandardTimeOut.Text = "STANDARD TIME OUT";
            this.BtnTitleStandardTimeOut.UseVisualStyleBackColor = true;
            // 
            // BtnTitleStandardTolerance
            // 
            this.BtnTitleStandardTolerance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleStandardTolerance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleStandardTolerance.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleStandardTolerance.Location = new System.Drawing.Point(1042, 254);
            this.BtnTitleStandardTolerance.Name = "BtnTitleStandardTolerance";
            this.BtnTitleStandardTolerance.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleStandardTolerance.TabIndex = 7;
            this.BtnTitleStandardTolerance.TabStop = false;
            this.BtnTitleStandardTolerance.Text = "STANDARD TOLERANCE";
            this.BtnTitleStandardTolerance.UseVisualStyleBackColor = true;
            // 
            // BtnTitleStandardDeceleration
            // 
            this.BtnTitleStandardDeceleration.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleStandardDeceleration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleStandardDeceleration.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleStandardDeceleration.Location = new System.Drawing.Point(1042, 216);
            this.BtnTitleStandardDeceleration.Name = "BtnTitleStandardDeceleration";
            this.BtnTitleStandardDeceleration.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleStandardDeceleration.TabIndex = 7;
            this.BtnTitleStandardDeceleration.TabStop = false;
            this.BtnTitleStandardDeceleration.Text = "STANDARD DEC";
            this.BtnTitleStandardDeceleration.UseVisualStyleBackColor = true;
            // 
            // BtnTitleManualSpeed
            // 
            this.BtnTitleManualSpeed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleManualSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleManualSpeed.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleManualSpeed.Location = new System.Drawing.Point(1042, 140);
            this.BtnTitleManualSpeed.Name = "BtnTitleManualSpeed";
            this.BtnTitleManualSpeed.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleManualSpeed.TabIndex = 7;
            this.BtnTitleManualSpeed.TabStop = false;
            this.BtnTitleManualSpeed.Text = "MANUAL VELOCITY";
            this.BtnTitleManualSpeed.UseVisualStyleBackColor = true;
            // 
            // BtnTitleStandardAcceleration
            // 
            this.BtnTitleStandardAcceleration.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleStandardAcceleration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleStandardAcceleration.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleStandardAcceleration.Location = new System.Drawing.Point(1042, 178);
            this.BtnTitleStandardAcceleration.Name = "BtnTitleStandardAcceleration";
            this.BtnTitleStandardAcceleration.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleStandardAcceleration.TabIndex = 7;
            this.BtnTitleStandardAcceleration.TabStop = false;
            this.BtnTitleStandardAcceleration.Text = "STANDARD ACC";
            this.BtnTitleStandardAcceleration.UseVisualStyleBackColor = true;
            // 
            // BtnStandardTimeOut
            // 
            this.BtnStandardTimeOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnStandardTimeOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStandardTimeOut.Location = new System.Drawing.Point(1156, 292);
            this.BtnStandardTimeOut.Name = "BtnStandardTimeOut";
            this.BtnStandardTimeOut.Size = new System.Drawing.Size(108, 34);
            this.BtnStandardTimeOut.TabIndex = 8;
            this.BtnStandardTimeOut.TabStop = false;
            this.BtnStandardTimeOut.Text = "0";
            this.BtnStandardTimeOut.UseVisualStyleBackColor = true;
            this.BtnStandardTimeOut.Click += new System.EventHandler(this.BtnStandardTimeOut_Click);
            // 
            // BtnStandardTolerance
            // 
            this.BtnStandardTolerance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnStandardTolerance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStandardTolerance.Location = new System.Drawing.Point(1156, 254);
            this.BtnStandardTolerance.Name = "BtnStandardTolerance";
            this.BtnStandardTolerance.Size = new System.Drawing.Size(108, 34);
            this.BtnStandardTolerance.TabIndex = 8;
            this.BtnStandardTolerance.TabStop = false;
            this.BtnStandardTolerance.Text = "0";
            this.BtnStandardTolerance.UseVisualStyleBackColor = true;
            this.BtnStandardTolerance.Click += new System.EventHandler(this.BtnStandardTolerance_Click);
            // 
            // BtnTitleAutoSpeed
            // 
            this.BtnTitleAutoSpeed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleAutoSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleAutoSpeed.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleAutoSpeed.Location = new System.Drawing.Point(1042, 12);
            this.BtnTitleAutoSpeed.Name = "BtnTitleAutoSpeed";
            this.BtnTitleAutoSpeed.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleAutoSpeed.TabIndex = 7;
            this.BtnTitleAutoSpeed.TabStop = false;
            this.BtnTitleAutoSpeed.Text = "AUTO VELOCITY";
            this.BtnTitleAutoSpeed.UseVisualStyleBackColor = true;
            this.BtnTitleAutoSpeed.Visible = false;
            // 
            // BtnStandardDeceleration
            // 
            this.BtnStandardDeceleration.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnStandardDeceleration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStandardDeceleration.Location = new System.Drawing.Point(1156, 216);
            this.BtnStandardDeceleration.Name = "BtnStandardDeceleration";
            this.BtnStandardDeceleration.Size = new System.Drawing.Size(108, 34);
            this.BtnStandardDeceleration.TabIndex = 8;
            this.BtnStandardDeceleration.TabStop = false;
            this.BtnStandardDeceleration.Text = "0";
            this.BtnStandardDeceleration.UseVisualStyleBackColor = true;
            this.BtnStandardDeceleration.Click += new System.EventHandler(this.BtnStandardDeceleration_Click);
            // 
            // BtnStandardAcceleration
            // 
            this.BtnStandardAcceleration.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnStandardAcceleration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStandardAcceleration.Location = new System.Drawing.Point(1156, 178);
            this.BtnStandardAcceleration.Name = "BtnStandardAcceleration";
            this.BtnStandardAcceleration.Size = new System.Drawing.Size(108, 34);
            this.BtnStandardAcceleration.TabIndex = 8;
            this.BtnStandardAcceleration.TabStop = false;
            this.BtnStandardAcceleration.Text = "0";
            this.BtnStandardAcceleration.UseVisualStyleBackColor = true;
            this.BtnStandardAcceleration.Click += new System.EventHandler(this.BtnStandardAcceleration_Click);
            // 
            // BtnManualSpeed
            // 
            this.BtnManualSpeed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnManualSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnManualSpeed.Location = new System.Drawing.Point(1156, 140);
            this.BtnManualSpeed.Name = "BtnManualSpeed";
            this.BtnManualSpeed.Size = new System.Drawing.Size(108, 34);
            this.BtnManualSpeed.TabIndex = 8;
            this.BtnManualSpeed.TabStop = false;
            this.BtnManualSpeed.Text = "0";
            this.BtnManualSpeed.UseVisualStyleBackColor = true;
            this.BtnManualSpeed.Click += new System.EventHandler(this.BtnManualSpeed_Click);
            // 
            // BtnAutoSpeed
            // 
            this.BtnAutoSpeed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAutoSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAutoSpeed.Location = new System.Drawing.Point(1156, 12);
            this.BtnAutoSpeed.Name = "BtnAutoSpeed";
            this.BtnAutoSpeed.Size = new System.Drawing.Size(108, 34);
            this.BtnAutoSpeed.TabIndex = 8;
            this.BtnAutoSpeed.TabStop = false;
            this.BtnAutoSpeed.Text = "0";
            this.BtnAutoSpeed.UseVisualStyleBackColor = true;
            this.BtnAutoSpeed.Visible = false;
            this.BtnAutoSpeed.Click += new System.EventHandler(this.BtnAutoSpeed_Click);
            // 
            // BtnTitleJogSlow
            // 
            this.BtnTitleJogSlow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleJogSlow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleJogSlow.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleJogSlow.Location = new System.Drawing.Point(1042, 102);
            this.BtnTitleJogSlow.Name = "BtnTitleJogSlow";
            this.BtnTitleJogSlow.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleJogSlow.TabIndex = 7;
            this.BtnTitleJogSlow.TabStop = false;
            this.BtnTitleJogSlow.Text = "SLOW JOG";
            this.BtnTitleJogSlow.UseVisualStyleBackColor = true;
            this.BtnTitleJogSlow.Click += new System.EventHandler(this.BtnTitleJogSlow_Click);
            // 
            // BtnJogSlow
            // 
            this.BtnJogSlow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnJogSlow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnJogSlow.Location = new System.Drawing.Point(1156, 102);
            this.BtnJogSlow.Name = "BtnJogSlow";
            this.BtnJogSlow.Size = new System.Drawing.Size(108, 34);
            this.BtnJogSlow.TabIndex = 8;
            this.BtnJogSlow.TabStop = false;
            this.BtnJogSlow.Text = "0";
            this.BtnJogSlow.UseVisualStyleBackColor = true;
            this.BtnJogSlow.Click += new System.EventHandler(this.BtnJogSlow_Click);
            // 
            // BtnPositionInformationPage
            // 
            this.BtnPositionInformationPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPositionInformationPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPositionInformationPage.Location = new System.Drawing.Point(720, 582);
            this.BtnPositionInformationPage.Name = "BtnPositionInformationPage";
            this.BtnPositionInformationPage.Size = new System.Drawing.Size(155, 48);
            this.BtnPositionInformationPage.TabIndex = 7;
            this.BtnPositionInformationPage.TabStop = false;
            this.BtnPositionInformationPage.Text = "00";
            this.BtnPositionInformationPage.UseVisualStyleBackColor = true;
            // 
            // BtnMotorInformationPage
            // 
            this.BtnMotorInformationPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnMotorInformationPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMotorInformationPage.Location = new System.Drawing.Point(173, 582);
            this.BtnMotorInformationPage.Name = "BtnMotorInformationPage";
            this.BtnMotorInformationPage.Size = new System.Drawing.Size(219, 48);
            this.BtnMotorInformationPage.TabIndex = 7;
            this.BtnMotorInformationPage.TabStop = false;
            this.BtnMotorInformationPage.Text = "00";
            this.BtnMotorInformationPage.UseVisualStyleBackColor = true;
            // 
            // BtnTitleJogFast
            // 
            this.BtnTitleJogFast.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleJogFast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleJogFast.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleJogFast.Location = new System.Drawing.Point(1042, 64);
            this.BtnTitleJogFast.Name = "BtnTitleJogFast";
            this.BtnTitleJogFast.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleJogFast.TabIndex = 7;
            this.BtnTitleJogFast.TabStop = false;
            this.BtnTitleJogFast.Text = "FAST JOG";
            this.BtnTitleJogFast.UseVisualStyleBackColor = true;
            this.BtnTitleJogFast.Click += new System.EventHandler(this.BtnTitleJogFast_Click);
            // 
            // BtnJogFast
            // 
            this.BtnJogFast.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnJogFast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnJogFast.Location = new System.Drawing.Point(1156, 64);
            this.BtnJogFast.Name = "BtnJogFast";
            this.BtnJogFast.Size = new System.Drawing.Size(108, 34);
            this.BtnJogFast.TabIndex = 8;
            this.BtnJogFast.TabStop = false;
            this.BtnJogFast.Text = "0";
            this.BtnJogFast.UseVisualStyleBackColor = true;
            this.BtnJogFast.Click += new System.EventHandler(this.BtnJogFast_Click);
            // 
            // GridViewPositionInformationList
            // 
            this.GridViewPositionInformationList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.GridViewPositionInformationList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewPositionInformationList.Location = new System.Drawing.Point(559, 64);
            this.GridViewPositionInformationList.Name = "GridViewPositionInformationList";
            this.GridViewPositionInformationList.RowTemplate.Height = 23;
            this.GridViewPositionInformationList.Size = new System.Drawing.Size(477, 512);
            this.GridViewPositionInformationList.TabIndex = 6;
            this.GridViewPositionInformationList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewPositionInformationList_CellDoubleClick);
            this.GridViewPositionInformationList.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridViewPositionInformationList_ColumnHeaderMouseDoubleClick);
            // 
            // GridViewMotorInformationList
            // 
            this.GridViewMotorInformationList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.GridViewMotorInformationList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewMotorInformationList.Location = new System.Drawing.Point(12, 64);
            this.GridViewMotorInformationList.Name = "GridViewMotorInformationList";
            this.GridViewMotorInformationList.RowTemplate.Height = 23;
            this.GridViewMotorInformationList.Size = new System.Drawing.Size(541, 512);
            this.GridViewMotorInformationList.TabIndex = 6;
            this.GridViewMotorInformationList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewMotorInformationList_CellClick);
            // 
            // BtnTitleMotorSetting
            // 
            this.BtnTitleMotorSetting.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleMotorSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleMotorSetting.Location = new System.Drawing.Point(1042, 12);
            this.BtnTitleMotorSetting.Name = "BtnTitleMotorSetting";
            this.BtnTitleMotorSetting.Size = new System.Drawing.Size(222, 46);
            this.BtnTitleMotorSetting.TabIndex = 5;
            this.BtnTitleMotorSetting.TabStop = false;
            this.BtnTitleMotorSetting.Text = "MOTOR SETTING";
            this.BtnTitleMotorSetting.UseVisualStyleBackColor = true;
            // 
            // BtnTitlePositionInformation
            // 
            this.BtnTitlePositionInformation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitlePositionInformation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitlePositionInformation.Location = new System.Drawing.Point(559, 12);
            this.BtnTitlePositionInformation.Name = "BtnTitlePositionInformation";
            this.BtnTitlePositionInformation.Size = new System.Drawing.Size(477, 46);
            this.BtnTitlePositionInformation.TabIndex = 4;
            this.BtnTitlePositionInformation.TabStop = false;
            this.BtnTitlePositionInformation.Text = "POSITION INFORMATION";
            this.BtnTitlePositionInformation.UseVisualStyleBackColor = true;
            // 
            // BtnTitleMotorInformation
            // 
            this.BtnTitleMotorInformation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleMotorInformation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleMotorInformation.Location = new System.Drawing.Point(12, 12);
            this.BtnTitleMotorInformation.Name = "BtnTitleMotorInformation";
            this.BtnTitleMotorInformation.Size = new System.Drawing.Size(541, 46);
            this.BtnTitleMotorInformation.TabIndex = 4;
            this.BtnTitleMotorInformation.TabStop = false;
            this.BtnTitleMotorInformation.Text = "MOTOR INFORMATION";
            this.BtnTitleMotorInformation.UseVisualStyleBackColor = true;
            // 
            // BtnTitleDelayAfterMoving
            // 
            this.BtnTitleDelayAfterMoving.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleDelayAfterMoving.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleDelayAfterMoving.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleDelayAfterMoving.Location = new System.Drawing.Point(1042, 482);
            this.BtnTitleDelayAfterMoving.Name = "BtnTitleDelayAfterMoving";
            this.BtnTitleDelayAfterMoving.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleDelayAfterMoving.TabIndex = 10;
            this.BtnTitleDelayAfterMoving.TabStop = false;
            this.BtnTitleDelayAfterMoving.Text = "DELAY AFTER MOVING";
            this.BtnTitleDelayAfterMoving.UseVisualStyleBackColor = true;
            // 
            // BtnDelayAfterMoving
            // 
            this.BtnDelayAfterMoving.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnDelayAfterMoving.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDelayAfterMoving.Location = new System.Drawing.Point(1156, 482);
            this.BtnDelayAfterMoving.Name = "BtnDelayAfterMoving";
            this.BtnDelayAfterMoving.Size = new System.Drawing.Size(108, 34);
            this.BtnDelayAfterMoving.TabIndex = 11;
            this.BtnDelayAfterMoving.TabStop = false;
            this.BtnDelayAfterMoving.Text = "0";
            this.BtnDelayAfterMoving.UseVisualStyleBackColor = true;
            this.BtnDelayAfterMoving.Click += new System.EventHandler(this.BtnDelayAfterMoving_Click);
            // 
            // BtnRepeatMove
            // 
            this.BtnRepeatMove.BackColor = System.Drawing.Color.Transparent;
            this.BtnRepeatMove.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnRepeatMove.ButtonText = "REPEAT MOVE";
            this.BtnRepeatMove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnRepeatMove.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnRepeatMove.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnRepeatMove.Location = new System.Drawing.Point(398, 18);
            this.BtnRepeatMove.Name = "BtnRepeatMove";
            this.BtnRepeatMove.Size = new System.Drawing.Size(149, 35);
            this.BtnRepeatMove.TabIndex = 9;
            this.BtnRepeatMove.Text = "REPEAT MOVE";
            this.BtnRepeatMove.UseVisualStyleBackColor = false;
            this.BtnRepeatMove.Click += new System.EventHandler(this.BtnRepeatMove_Click);
            // 
            // BtnOriginOffset
            // 
            this.BtnOriginOffset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOriginOffset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOriginOffset.Location = new System.Drawing.Point(1156, 368);
            this.BtnOriginOffset.Name = "BtnOriginOffset";
            this.BtnOriginOffset.Size = new System.Drawing.Size(108, 34);
            this.BtnOriginOffset.TabIndex = 8;
            this.BtnOriginOffset.TabStop = false;
            this.BtnOriginOffset.Text = "0";
            this.BtnOriginOffset.UseVisualStyleBackColor = true;
            this.BtnOriginOffset.Click += new System.EventHandler(this.BtnOriginOffset_Click);
            // 
            // BtnTitleOriginOffset
            // 
            this.BtnTitleOriginOffset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleOriginOffset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleOriginOffset.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleOriginOffset.Location = new System.Drawing.Point(1042, 368);
            this.BtnTitleOriginOffset.Name = "BtnTitleOriginOffset";
            this.BtnTitleOriginOffset.Size = new System.Drawing.Size(108, 34);
            this.BtnTitleOriginOffset.TabIndex = 7;
            this.BtnTitleOriginOffset.TabStop = false;
            this.BtnTitleOriginOffset.Text = "ORIGIN OFFSET";
            this.BtnTitleOriginOffset.UseVisualStyleBackColor = true;
            // 
            // BtnPositionJog
            // 
            this.BtnPositionJog.BackColor = System.Drawing.Color.Transparent;
            this.BtnPositionJog.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionJog.ButtonText = "POSITION JOG";
            this.BtnPositionJog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPositionJog.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPositionJog.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionJog.Location = new System.Drawing.Point(1042, 637);
            this.BtnPositionJog.Name = "BtnPositionJog";
            this.BtnPositionJog.Size = new System.Drawing.Size(108, 47);
            this.BtnPositionJog.TabIndex = 12;
            this.BtnPositionJog.Text = "POSITION JOG";
            this.BtnPositionJog.UseVisualStyleBackColor = false;
            this.BtnPositionJog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnPositionJog_MouseDown);
            this.BtnPositionJog.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnPositionJog_MouseUp);
            // 
            // CFormSetupMotion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.Controls.Add(this.BtnPositionJog);
            this.Controls.Add(this.BtnDelayAfterMoving);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnServoOff);
            this.Controls.Add(this.BtnPositionInformationNext);
            this.Controls.Add(this.BtnMotorInformationNext);
            this.Controls.Add(this.BtnPositionInformationPrevious);
            this.Controls.Add(this.BtnMotorInformationPrevious);
            this.Controls.Add(this.BtnServoOn);
            this.Controls.Add(this.BtnServoReset);
            this.Controls.Add(this.BtnRepeatMove);
            this.Controls.Add(this.BtnOrigin);
            this.Controls.Add(this.BtnJogMoveMinus);
            this.Controls.Add(this.BtnJogMovePlus);
            this.Controls.Add(this.BtnRelativeMoveMinus);
            this.Controls.Add(this.BtnRelativeMovePlus);
            this.Controls.Add(this.BtnPositionMove);
            this.Controls.Add(this.BtnPositionSave);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnCommandPosition);
            this.Controls.Add(this.BtnCurrentPosition);
            this.Controls.Add(this.BtnRelativePosition);
            this.Controls.Add(this.BtnTitleRelativePosition);
            this.Controls.Add(this.BtnTitleCommandPosition);
            this.Controls.Add(this.BtnTitleCurrentPosition);
            this.Controls.Add(this.BtnUseHome);
            this.Controls.Add(this.BtnUseMotor);
            this.Controls.Add(this.BtnInnerNo);
            this.Controls.Add(this.BtnLimitPositionMinus);
            this.Controls.Add(this.BtnLimitPositionPlus);
            this.Controls.Add(this.BtnOriginOffset);
            this.Controls.Add(this.BtnOriginSpeed);
            this.Controls.Add(this.BtnStandardTimeOut);
            this.Controls.Add(this.BtnStandardTolerance);
            this.Controls.Add(this.BtnStandardDeceleration);
            this.Controls.Add(this.BtnStandardAcceleration);
            this.Controls.Add(this.BtnManualSpeed);
            this.Controls.Add(this.BtnJogSlow);
            this.Controls.Add(this.BtnPositionInformationPage);
            this.Controls.Add(this.BtnMotorInformationPage);
            this.Controls.Add(this.BtnJogFast);
            this.Controls.Add(this.GridViewPositionInformationList);
            this.Controls.Add(this.GridViewMotorInformationList);
            this.Controls.Add(this.BtnTitleMotorSetting);
            this.Controls.Add(this.BtnTitlePositionInformation);
            this.Controls.Add(this.BtnTitleMotorInformation);
            this.Controls.Add(this.BtnTitleDelayAfterMoving);
            this.Controls.Add(this.BtnTitleUseHome);
            this.Controls.Add(this.BtnTitleUseMotor);
            this.Controls.Add(this.BtnTitleInnerNo);
            this.Controls.Add(this.BtnTitleLimitPositionMinus);
            this.Controls.Add(this.BtnTitleLimitPositionPlus);
            this.Controls.Add(this.BtnTitleOriginOffset);
            this.Controls.Add(this.BtnTitleOriginSpeed);
            this.Controls.Add(this.BtnTitleStandardTimeOut);
            this.Controls.Add(this.BtnTitleStandardTolerance);
            this.Controls.Add(this.BtnTitleStandardDeceleration);
            this.Controls.Add(this.BtnTitleManualSpeed);
            this.Controls.Add(this.BtnTitleStandardAcceleration);
            this.Controls.Add(this.BtnTitleAutoSpeed);
            this.Controls.Add(this.BtnTitleJogSlow);
            this.Controls.Add(this.BtnTitleJogFast);
            this.Controls.Add(this.BtnAutoSpeed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormSetupMotion";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormSetupMotion";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormSetupMotion_FormClosed);
            this.Load += new System.EventHandler(this.CFormSetupMotion_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CFormSetupMotion_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewPositionInformationList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewMotorInformationList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UiAsset.SpeedButton BtnTitleMotorInformation;
        private UiAsset.SpeedButton BtnTitlePositionInformation;
        private UiAsset.SpeedButton BtnTitleMotorSetting;
        private System.Windows.Forms.DataGridView GridViewMotorInformationList;
        private System.Windows.Forms.DataGridView GridViewPositionInformationList;
        private UiAsset.SpeedButton BtnTitleJogFast;
        private UiAsset.SpeedButton BtnJogFast;
        private UiAsset.SpeedButton BtnJogSlow;
        private UiAsset.SpeedButton BtnTitleJogSlow;
        private UiAsset.SpeedButton BtnAutoSpeed;
        private UiAsset.SpeedButton BtnTitleAutoSpeed;
        private UiAsset.SpeedButton BtnOriginSpeed;
        private UiAsset.SpeedButton BtnTitleOriginSpeed;
        private UiAsset.SpeedButton BtnLimitPositionPlus;
        private UiAsset.SpeedButton BtnTitleLimitPositionPlus;
        private UiAsset.SpeedButton BtnLimitPositionMinus;
        private UiAsset.SpeedButton BtnTitleLimitPositionMinus;
        private UiAsset.SpeedButton BtnInnerNo;
        private UiAsset.SpeedButton BtnTitleInnerNo;
        private UiAsset.SpeedButton BtnUseMotor;
		private UiAsset.SpeedButton BtnTitleUseMotor;
        private UiAsset.SpeedButton BtnTitleCurrentPosition;
        private UiAsset.SpeedButton BtnCurrentPosition;
        private UiAsset.SpeedButton BtnTitleRelativePosition;
        private UiAsset.SpeedButton BtnRelativePosition;
		private System.Windows.Forms.Timer timer;
		private UiAsset.SpeedButton BtnMotorInformationPage;
        private UiAsset.SpeedButton BtnManualSpeed;
		private UiAsset.SpeedButton BtnTitleManualSpeed;
		private UiAsset.SpeedButton BtnPositionInformationPage;
        private UiAsset.SpeedButton BtnTitleCommandPosition;
        private UiAsset.SpeedButton BtnCommandPosition;
        private UiAsset.SpeedButton BtnStandardAcceleration;
        private UiAsset.SpeedButton BtnStandardDeceleration;
        private UiAsset.SpeedButton BtnTitleStandardAcceleration;
        private UiAsset.SpeedButton BtnTitleStandardDeceleration;
        private UiAsset.SpeedButton BtnStandardTolerance;
        private UiAsset.SpeedButton BtnTitleStandardTolerance;
        private UiAsset.SpeedButton BtnUseHome;
        private UiAsset.SpeedButton BtnTitleUseHome;
        private UiAsset.SpeedButton BtnTitleStandardTimeOut;
        private UiAsset.SpeedButton BtnStandardTimeOut;
		private UiAsset.ImageButton BtnStop;
		private UiAsset.ImageButton BtnSave;
		private UiAsset.ImageButton BtnPositionSave;
		private UiAsset.ImageButton BtnPositionMove;
		private UiAsset.ImageButton BtnRelativeMovePlus;
		private UiAsset.ImageButton BtnRelativeMoveMinus;
		private UiAsset.ImageButton BtnJogMovePlus;
		private UiAsset.ImageButton BtnJogMoveMinus;
		private UiAsset.ImageButton BtnOrigin;
		private UiAsset.ImageButton BtnServoReset;
		private UiAsset.ImageButton BtnServoOn;
		private UiAsset.ImageButton BtnServoOff;
		private UiAsset.ImageButton BtnMotorInformationPrevious;
		private UiAsset.ImageButton BtnMotorInformationNext;
		private UiAsset.ImageButton BtnPositionInformationPrevious;
		private UiAsset.ImageButton BtnPositionInformationNext;
        private UiAsset.SpeedButton BtnTitleDelayAfterMoving;
        private UiAsset.SpeedButton BtnDelayAfterMoving;
        private UiAsset.ImageButton BtnRepeatMove;
        private UiAsset.SpeedButton BtnOriginOffset;
        private UiAsset.SpeedButton BtnTitleOriginOffset;
        private UiAsset.ImageButton BtnPositionJog;
    }
}