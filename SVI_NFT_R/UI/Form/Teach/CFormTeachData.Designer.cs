namespace SVI_NFT_R
{
    partial class CFormTeachData
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
            this.BtnCurrentPosition = new UiAsset.SpeedButton();
            this.BtnJogMoveMinus = new UiAsset.ImageButton();
            this.BtnSlowJogMode = new UiAsset.ImageButton();
            this.BtnJogMovePlus = new UiAsset.ImageButton();
            this.BtnFastJogMode = new UiAsset.ImageButton();
            this.BtnPositionMove = new UiAsset.ImageButton();
            this.BtnTitleJog = new UiAsset.SpeedButton();
            this.BtnSave = new UiAsset.ImageButton();
            this.BtnInposition = new UiAsset.SpeedButton();
            this.BtnAlarm = new UiAsset.SpeedButton();
            this.BtnTitleCurrentPosition = new UiAsset.SpeedButton();
            this.BtnTitleAxisName = new UiAsset.SpeedButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnMinusLimit = new UiAsset.SpeedButton();
            this.BtnHomeSensor = new UiAsset.SpeedButton();
            this.BtnPlusLimit = new UiAsset.SpeedButton();
            this.BtnTitleRelativeMoveValue = new UiAsset.SpeedButton();
            this.BtnRelativeMoveValue = new UiAsset.SpeedButton();
            this.BtnRelativeMoveMinus = new UiAsset.ImageButton();
            this.BtnRelativeMovePlus = new UiAsset.ImageButton();
            this.BtnStop = new UiAsset.ImageButton();
            this.BtnCurrentPositionStatus = new UiAsset.SpeedButton();
            this.BtnServo = new UiAsset.SpeedButton();
            this.BtnTargetPosition = new UiAsset.SpeedButton();
            this.BtnTargetPositionMove = new UiAsset.ImageButton();
            this.BtnHome = new UiAsset.SpeedButton();
            this.pnlServoLoadRatio = new System.Windows.Forms.Panel();
            this.lblTitleServoLoadRatio = new System.Windows.Forms.Label();
            this.lblServoLoadRatio = new System.Windows.Forms.Label();
            this.pnlCurrentVelocity = new System.Windows.Forms.Panel();
            this.lblTitleCurrentVelocity = new System.Windows.Forms.Label();
            this.lblCurrentVelocity = new System.Windows.Forms.Label();
            this.BtnPositionJog = new UiAsset.ImageButton();
            this.pnlServoLoadRatio.SuspendLayout();
            this.pnlCurrentVelocity.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnCurrentPosition
            // 
            this.BtnCurrentPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCurrentPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCurrentPosition.Location = new System.Drawing.Point(199, 116);
            this.BtnCurrentPosition.Name = "BtnCurrentPosition";
            this.BtnCurrentPosition.Size = new System.Drawing.Size(158, 46);
            this.BtnCurrentPosition.TabIndex = 10;
            this.BtnCurrentPosition.TabStop = false;
            this.BtnCurrentPosition.UseVisualStyleBackColor = true;
            this.BtnCurrentPosition.Click += new System.EventHandler(this.BtnCurrentPosition_Click);
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
            this.BtnJogMoveMinus.Location = new System.Drawing.Point(12, 584);
            this.BtnJogMoveMinus.Name = "BtnJogMoveMinus";
            this.BtnJogMoveMinus.Size = new System.Drawing.Size(181, 46);
            this.BtnJogMoveMinus.TabIndex = 10;
            this.BtnJogMoveMinus.Text = "JOG MOVE ( - )";
            this.BtnJogMoveMinus.UseVisualStyleBackColor = false;
            this.BtnJogMoveMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnJogMoveMinus_MouseDown);
            this.BtnJogMoveMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnJogMoveMinus_MouseUp);
            // 
            // BtnSlowJogMode
            // 
            this.BtnSlowJogMode.BackColor = System.Drawing.Color.Transparent;
            this.BtnSlowJogMode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSlowJogMode.ButtonText = "SLOW JOG MODE";
            this.BtnSlowJogMode.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnSlowJogMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSlowJogMode.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSlowJogMode.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSlowJogMode.Location = new System.Drawing.Point(199, 532);
            this.BtnSlowJogMode.Name = "BtnSlowJogMode";
            this.BtnSlowJogMode.Size = new System.Drawing.Size(181, 46);
            this.BtnSlowJogMode.TabIndex = 10;
            this.BtnSlowJogMode.Text = "SLOW JOG MODE";
            this.BtnSlowJogMode.UseVisualStyleBackColor = false;
            this.BtnSlowJogMode.Click += new System.EventHandler(this.BtnSlowJogMode_Click);
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
            this.BtnJogMovePlus.Location = new System.Drawing.Point(199, 584);
            this.BtnJogMovePlus.Name = "BtnJogMovePlus";
            this.BtnJogMovePlus.Size = new System.Drawing.Size(181, 46);
            this.BtnJogMovePlus.TabIndex = 10;
            this.BtnJogMovePlus.Text = "JOG MOVE ( + )";
            this.BtnJogMovePlus.UseVisualStyleBackColor = false;
            this.BtnJogMovePlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnJogMovePlus_MouseDown);
            this.BtnJogMovePlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnJogMovePlus_MouseUp);
            // 
            // BtnFastJogMode
            // 
            this.BtnFastJogMode.BackColor = System.Drawing.Color.Transparent;
            this.BtnFastJogMode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnFastJogMode.ButtonText = "FAST JOG MODE";
            this.BtnFastJogMode.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnFastJogMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnFastJogMode.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnFastJogMode.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnFastJogMode.Location = new System.Drawing.Point(12, 532);
            this.BtnFastJogMode.Name = "BtnFastJogMode";
            this.BtnFastJogMode.Size = new System.Drawing.Size(181, 46);
            this.BtnFastJogMode.TabIndex = 10;
            this.BtnFastJogMode.Text = "FAST JOG MODE";
            this.BtnFastJogMode.UseVisualStyleBackColor = false;
            this.BtnFastJogMode.Click += new System.EventHandler(this.BtnFastJogMode_Click);
            // 
            // BtnPositionMove
            // 
            this.BtnPositionMove.BackColor = System.Drawing.Color.Transparent;
            this.BtnPositionMove.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionMove.ButtonText = "POSITION MOVE";
            this.BtnPositionMove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPositionMove.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPositionMove.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionMove.Location = new System.Drawing.Point(12, 636);
            this.BtnPositionMove.Name = "BtnPositionMove";
            this.BtnPositionMove.Size = new System.Drawing.Size(181, 46);
            this.BtnPositionMove.TabIndex = 10;
            this.BtnPositionMove.Text = "POSITION MOVE";
            this.BtnPositionMove.UseVisualStyleBackColor = false;
            this.BtnPositionMove.Click += new System.EventHandler(this.BtnPositionMove_Click);
            // 
            // BtnTitleJog
            // 
            this.BtnTitleJog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleJog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleJog.Location = new System.Drawing.Point(12, 480);
            this.BtnTitleJog.Name = "BtnTitleJog";
            this.BtnTitleJog.Size = new System.Drawing.Size(368, 46);
            this.BtnTitleJog.TabIndex = 10;
            this.BtnTitleJog.TabStop = false;
            this.BtnTitleJog.Text = "JOG MOVE";
            this.BtnTitleJog.UseVisualStyleBackColor = true;
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.Transparent;
            this.BtnSave.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSave.ButtonText = "SAVE";
            this.BtnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSave.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSave.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSave.Location = new System.Drawing.Point(199, 687);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(181, 46);
            this.BtnSave.TabIndex = 10;
            this.BtnSave.Text = "SAVE";
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnInposition
            // 
            this.BtnInposition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnInposition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnInposition.Location = new System.Drawing.Point(199, 272);
            this.BtnInposition.Name = "BtnInposition";
            this.BtnInposition.Size = new System.Drawing.Size(181, 46);
            this.BtnInposition.TabIndex = 10;
            this.BtnInposition.TabStop = false;
            this.BtnInposition.Text = "INPOSITION";
            this.BtnInposition.UseVisualStyleBackColor = true;
            // 
            // BtnAlarm
            // 
            this.BtnAlarm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAlarm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAlarm.Location = new System.Drawing.Point(12, 272);
            this.BtnAlarm.Name = "BtnAlarm";
            this.BtnAlarm.Size = new System.Drawing.Size(181, 46);
            this.BtnAlarm.TabIndex = 10;
            this.BtnAlarm.TabStop = false;
            this.BtnAlarm.Text = "ALARM";
            this.BtnAlarm.UseVisualStyleBackColor = true;
            this.BtnAlarm.Click += new System.EventHandler(this.BtnAlarm_Click);
            // 
            // BtnTitleCurrentPosition
            // 
            this.BtnTitleCurrentPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleCurrentPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleCurrentPosition.Location = new System.Drawing.Point(12, 116);
            this.BtnTitleCurrentPosition.Name = "BtnTitleCurrentPosition";
            this.BtnTitleCurrentPosition.Size = new System.Drawing.Size(181, 46);
            this.BtnTitleCurrentPosition.TabIndex = 10;
            this.BtnTitleCurrentPosition.TabStop = false;
            this.BtnTitleCurrentPosition.Text = "CURRENT POSITION";
            this.BtnTitleCurrentPosition.UseVisualStyleBackColor = true;
            // 
            // BtnTitleAxisName
            // 
            this.BtnTitleAxisName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleAxisName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleAxisName.Location = new System.Drawing.Point(12, 12);
            this.BtnTitleAxisName.Name = "BtnTitleAxisName";
            this.BtnTitleAxisName.Size = new System.Drawing.Size(368, 46);
            this.BtnTitleAxisName.TabIndex = 11;
            this.BtnTitleAxisName.TabStop = false;
            this.BtnTitleAxisName.Text = "AXIS NAME";
            this.BtnTitleAxisName.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            this.timer.Interval = 10;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnMinusLimit
            // 
            this.BtnMinusLimit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnMinusLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMinusLimit.Location = new System.Drawing.Point(12, 64);
            this.BtnMinusLimit.Name = "BtnMinusLimit";
            this.BtnMinusLimit.Size = new System.Drawing.Size(118, 46);
            this.BtnMinusLimit.TabIndex = 10;
            this.BtnMinusLimit.TabStop = false;
            this.BtnMinusLimit.Text = "LIMIT ( - )";
            this.BtnMinusLimit.UseVisualStyleBackColor = true;
            // 
            // BtnHomeSensor
            // 
            this.BtnHomeSensor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnHomeSensor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnHomeSensor.Location = new System.Drawing.Point(138, 64);
            this.BtnHomeSensor.Name = "BtnHomeSensor";
            this.BtnHomeSensor.Size = new System.Drawing.Size(118, 46);
            this.BtnHomeSensor.TabIndex = 10;
            this.BtnHomeSensor.TabStop = false;
            this.BtnHomeSensor.Text = "HOME SENSOR";
            this.BtnHomeSensor.UseVisualStyleBackColor = true;
            // 
            // BtnPlusLimit
            // 
            this.BtnPlusLimit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPlusLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPlusLimit.Location = new System.Drawing.Point(262, 64);
            this.BtnPlusLimit.Name = "BtnPlusLimit";
            this.BtnPlusLimit.Size = new System.Drawing.Size(118, 46);
            this.BtnPlusLimit.TabIndex = 10;
            this.BtnPlusLimit.TabStop = false;
            this.BtnPlusLimit.Text = "LIMIT ( + )";
            this.BtnPlusLimit.UseVisualStyleBackColor = true;
            // 
            // BtnTitleRelativeMoveValue
            // 
            this.BtnTitleRelativeMoveValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTitleRelativeMoveValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleRelativeMoveValue.Location = new System.Drawing.Point(12, 376);
            this.BtnTitleRelativeMoveValue.Name = "BtnTitleRelativeMoveValue";
            this.BtnTitleRelativeMoveValue.Size = new System.Drawing.Size(181, 46);
            this.BtnTitleRelativeMoveValue.TabIndex = 10;
            this.BtnTitleRelativeMoveValue.TabStop = false;
            this.BtnTitleRelativeMoveValue.Text = "RELATIVE VALUE";
            this.BtnTitleRelativeMoveValue.UseVisualStyleBackColor = true;
            this.BtnTitleRelativeMoveValue.Click += new System.EventHandler(this.BtnServo_Click);
            // 
            // BtnRelativeMoveValue
            // 
            this.BtnRelativeMoveValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnRelativeMoveValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRelativeMoveValue.Location = new System.Drawing.Point(199, 376);
            this.BtnRelativeMoveValue.Name = "BtnRelativeMoveValue";
            this.BtnRelativeMoveValue.Size = new System.Drawing.Size(181, 46);
            this.BtnRelativeMoveValue.TabIndex = 10;
            this.BtnRelativeMoveValue.TabStop = false;
            this.BtnRelativeMoveValue.UseVisualStyleBackColor = true;
            this.BtnRelativeMoveValue.Click += new System.EventHandler(this.BtnRelativeMoveValue_Click);
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
            this.BtnRelativeMoveMinus.Location = new System.Drawing.Point(12, 428);
            this.BtnRelativeMoveMinus.Name = "BtnRelativeMoveMinus";
            this.BtnRelativeMoveMinus.Size = new System.Drawing.Size(181, 46);
            this.BtnRelativeMoveMinus.TabIndex = 10;
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
            this.BtnRelativeMovePlus.Location = new System.Drawing.Point(199, 428);
            this.BtnRelativeMovePlus.Name = "BtnRelativeMovePlus";
            this.BtnRelativeMovePlus.Size = new System.Drawing.Size(181, 46);
            this.BtnRelativeMovePlus.TabIndex = 10;
            this.BtnRelativeMovePlus.Text = "RELATIVE MOVE ( + )";
            this.BtnRelativeMovePlus.UseVisualStyleBackColor = false;
            this.BtnRelativeMovePlus.Click += new System.EventHandler(this.BtnRelativeMovePlus_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.BackColor = System.Drawing.Color.Transparent;
            this.BtnStop.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnStop.ButtonText = "STOP";
            this.BtnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnStop.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnStop.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnStop.Location = new System.Drawing.Point(199, 636);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(181, 46);
            this.BtnStop.TabIndex = 10;
            this.BtnStop.Text = "STOP";
            this.BtnStop.UseVisualStyleBackColor = false;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // BtnCurrentPositionStatus
            // 
            this.BtnCurrentPositionStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCurrentPositionStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCurrentPositionStatus.Location = new System.Drawing.Point(363, 116);
            this.BtnCurrentPositionStatus.Name = "BtnCurrentPositionStatus";
            this.BtnCurrentPositionStatus.Size = new System.Drawing.Size(17, 46);
            this.BtnCurrentPositionStatus.TabIndex = 14;
            this.BtnCurrentPositionStatus.TabStop = false;
            this.BtnCurrentPositionStatus.UseVisualStyleBackColor = true;
            // 
            // BtnServo
            // 
            this.BtnServo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnServo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnServo.Location = new System.Drawing.Point(12, 220);
            this.BtnServo.Name = "BtnServo";
            this.BtnServo.Size = new System.Drawing.Size(181, 46);
            this.BtnServo.TabIndex = 10;
            this.BtnServo.TabStop = false;
            this.BtnServo.Text = "SERVO";
            this.BtnServo.UseVisualStyleBackColor = true;
            this.BtnServo.Click += new System.EventHandler(this.BtnServo_Click);
            // 
            // BtnTargetPosition
            // 
            this.BtnTargetPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTargetPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTargetPosition.Location = new System.Drawing.Point(199, 168);
            this.BtnTargetPosition.Name = "BtnTargetPosition";
            this.BtnTargetPosition.Size = new System.Drawing.Size(181, 46);
            this.BtnTargetPosition.TabIndex = 15;
            this.BtnTargetPosition.TabStop = false;
            this.BtnTargetPosition.UseVisualStyleBackColor = true;
            this.BtnTargetPosition.Click += new System.EventHandler(this.BtnTargetPosition_Click);
            // 
            // BtnTargetPositionMove
            // 
            this.BtnTargetPositionMove.BackColor = System.Drawing.Color.Transparent;
            this.BtnTargetPositionMove.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnTargetPositionMove.ButtonText = "TAGET POSITION MOVE";
            this.BtnTargetPositionMove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTargetPositionMove.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnTargetPositionMove.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnTargetPositionMove.Location = new System.Drawing.Point(12, 168);
            this.BtnTargetPositionMove.Name = "BtnTargetPositionMove";
            this.BtnTargetPositionMove.Size = new System.Drawing.Size(181, 46);
            this.BtnTargetPositionMove.TabIndex = 16;
            this.BtnTargetPositionMove.Text = "TAGET POSITION MOVE";
            this.BtnTargetPositionMove.UseVisualStyleBackColor = false;
            this.BtnTargetPositionMove.Click += new System.EventHandler(this.BtnTargetPositionMove_Click);
            // 
            // BtnHome
            // 
            this.BtnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnHome.Location = new System.Drawing.Point(199, 220);
            this.BtnHome.Name = "BtnHome";
            this.BtnHome.Size = new System.Drawing.Size(181, 46);
            this.BtnHome.TabIndex = 17;
            this.BtnHome.TabStop = false;
            this.BtnHome.Text = "HOME";
            this.BtnHome.UseVisualStyleBackColor = true;
            this.BtnHome.Click += new System.EventHandler(this.BtnHome_Click);
            // 
            // pnlServoLoadRatio
            // 
            this.pnlServoLoadRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlServoLoadRatio.Controls.Add(this.lblTitleServoLoadRatio);
            this.pnlServoLoadRatio.Controls.Add(this.lblServoLoadRatio);
            this.pnlServoLoadRatio.Location = new System.Drawing.Point(199, 324);
            this.pnlServoLoadRatio.Name = "pnlServoLoadRatio";
            this.pnlServoLoadRatio.Size = new System.Drawing.Size(181, 46);
            this.pnlServoLoadRatio.TabIndex = 18;
            // 
            // lblTitleServoLoadRatio
            // 
            this.lblTitleServoLoadRatio.AutoSize = true;
            this.lblTitleServoLoadRatio.Location = new System.Drawing.Point(20, 7);
            this.lblTitleServoLoadRatio.Name = "lblTitleServoLoadRatio";
            this.lblTitleServoLoadRatio.Size = new System.Drawing.Size(122, 12);
            this.lblTitleServoLoadRatio.TabIndex = 1;
            this.lblTitleServoLoadRatio.Text = "SERVO LOAD RATIO";
            // 
            // lblServoLoadRatio
            // 
            this.lblServoLoadRatio.Location = new System.Drawing.Point(78, 23);
            this.lblServoLoadRatio.Name = "lblServoLoadRatio";
            this.lblServoLoadRatio.Size = new System.Drawing.Size(80, 18);
            this.lblServoLoadRatio.TabIndex = 0;
            this.lblServoLoadRatio.Text = "000.0 ( % )";
            this.lblServoLoadRatio.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pnlCurrentVelocity
            // 
            this.pnlCurrentVelocity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCurrentVelocity.Controls.Add(this.lblTitleCurrentVelocity);
            this.pnlCurrentVelocity.Controls.Add(this.lblCurrentVelocity);
            this.pnlCurrentVelocity.Location = new System.Drawing.Point(12, 324);
            this.pnlCurrentVelocity.Name = "pnlCurrentVelocity";
            this.pnlCurrentVelocity.Size = new System.Drawing.Size(181, 46);
            this.pnlCurrentVelocity.TabIndex = 19;
            // 
            // lblTitleCurrentVelocity
            // 
            this.lblTitleCurrentVelocity.AutoSize = true;
            this.lblTitleCurrentVelocity.Location = new System.Drawing.Point(7, 7);
            this.lblTitleCurrentVelocity.Name = "lblTitleCurrentVelocity";
            this.lblTitleCurrentVelocity.Size = new System.Drawing.Size(167, 12);
            this.lblTitleCurrentVelocity.TabIndex = 1;
            this.lblTitleCurrentVelocity.Text = "CURRENT MOVE VELOCITY";
            // 
            // lblCurrentVelocity
            // 
            this.lblCurrentVelocity.Location = new System.Drawing.Point(61, 23);
            this.lblCurrentVelocity.Name = "lblCurrentVelocity";
            this.lblCurrentVelocity.Size = new System.Drawing.Size(113, 18);
            this.lblCurrentVelocity.TabIndex = 0;
            this.lblCurrentVelocity.Text = "000.0 ( mm/sec )";
            this.lblCurrentVelocity.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BtnPositionJog
            // 
            this.BtnPositionJog.BackColor = System.Drawing.Color.Transparent;
            this.BtnPositionJog.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionJog.ButtonText = "POSITION JOG MOVE";
            this.BtnPositionJog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPositionJog.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnPositionJog.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnPositionJog.Location = new System.Drawing.Point(12, 687);
            this.BtnPositionJog.Name = "BtnPositionJog";
            this.BtnPositionJog.Size = new System.Drawing.Size(181, 46);
            this.BtnPositionJog.TabIndex = 20;
            this.BtnPositionJog.Text = "POSITION JOG MOVE";
            this.BtnPositionJog.UseVisualStyleBackColor = false;
            this.BtnPositionJog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnPositionJog_MouseDown);
            this.BtnPositionJog.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnPositionJog_MouseUp);
            // 
            // CFormTeachData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(392, 745);
            this.ControlBox = false;
            this.Controls.Add(this.BtnPositionJog);
            this.Controls.Add(this.pnlCurrentVelocity);
            this.Controls.Add(this.pnlServoLoadRatio);
            this.Controls.Add(this.BtnHome);
            this.Controls.Add(this.BtnTargetPositionMove);
            this.Controls.Add(this.BtnTargetPosition);
            this.Controls.Add(this.BtnCurrentPositionStatus);
            this.Controls.Add(this.BtnCurrentPosition);
            this.Controls.Add(this.BtnJogMoveMinus);
            this.Controls.Add(this.BtnSlowJogMode);
            this.Controls.Add(this.BtnJogMovePlus);
            this.Controls.Add(this.BtnFastJogMode);
            this.Controls.Add(this.BtnRelativeMoveValue);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnPositionMove);
            this.Controls.Add(this.BtnTitleJog);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnInposition);
            this.Controls.Add(this.BtnAlarm);
            this.Controls.Add(this.BtnRelativeMovePlus);
            this.Controls.Add(this.BtnRelativeMoveMinus);
            this.Controls.Add(this.BtnTitleRelativeMoveValue);
            this.Controls.Add(this.BtnPlusLimit);
            this.Controls.Add(this.BtnHomeSensor);
            this.Controls.Add(this.BtnMinusLimit);
            this.Controls.Add(this.BtnServo);
            this.Controls.Add(this.BtnTitleCurrentPosition);
            this.Controls.Add(this.BtnTitleAxisName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(100, 745);
            this.Name = "CFormTeachData";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormTeachData";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormTeachData_FormClosed);
            this.Load += new System.EventHandler(this.CFormTeachData_Load);
            this.pnlServoLoadRatio.ResumeLayout(false);
            this.pnlServoLoadRatio.PerformLayout();
            this.pnlCurrentVelocity.ResumeLayout(false);
            this.pnlCurrentVelocity.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UiAsset.SpeedButton BtnTitleCurrentPosition;
        private UiAsset.SpeedButton BtnTitleAxisName;
        private UiAsset.SpeedButton BtnCurrentPosition;
        private UiAsset.ImageButton BtnSave;
        private UiAsset.SpeedButton BtnTitleJog;
        private UiAsset.ImageButton BtnFastJogMode;
        private UiAsset.ImageButton BtnSlowJogMode;
        private UiAsset.ImageButton BtnJogMovePlus;
        private UiAsset.ImageButton BtnJogMoveMinus;
        private UiAsset.ImageButton BtnPositionMove;
        private UiAsset.SpeedButton BtnAlarm;
        private UiAsset.SpeedButton BtnInposition;
        private System.Windows.Forms.Timer timer;
        private UiAsset.SpeedButton BtnMinusLimit;
        private UiAsset.SpeedButton BtnHomeSensor;
        private UiAsset.SpeedButton BtnPlusLimit;
        private UiAsset.SpeedButton BtnTitleRelativeMoveValue;
        private UiAsset.SpeedButton BtnRelativeMoveValue;
        private UiAsset.ImageButton BtnRelativeMoveMinus;
        private UiAsset.ImageButton BtnRelativeMovePlus;
        private UiAsset.ImageButton BtnStop;
        private UiAsset.SpeedButton BtnCurrentPositionStatus;
        private UiAsset.SpeedButton BtnServo;
        private UiAsset.SpeedButton BtnTargetPosition;
        private UiAsset.ImageButton BtnTargetPositionMove;
        private UiAsset.SpeedButton BtnHome;
        private System.Windows.Forms.Panel pnlServoLoadRatio;
        private System.Windows.Forms.Label lblTitleServoLoadRatio;
        private System.Windows.Forms.Label lblServoLoadRatio;
        private System.Windows.Forms.Panel pnlCurrentVelocity;
        private System.Windows.Forms.Label lblTitleCurrentVelocity;
        private System.Windows.Forms.Label lblCurrentVelocity;
        private UiAsset.ImageButton BtnPositionJog;
    }
}