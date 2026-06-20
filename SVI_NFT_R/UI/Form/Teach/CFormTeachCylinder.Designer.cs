namespace SVI_NFT_R
{
    partial class CFormTeachCylinder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.BtnSecondMove = new UiAsset.ImageButton();
            this.BtnTitleSecondMoveTime = new UiAsset.SpeedButton();
            this.BtnSecondDelayTime = new UiAsset.SpeedButton();
            this.BtnSecondMoveTime = new UiAsset.SpeedButton();
            this.BtnTitleSecondDelayTime = new UiAsset.SpeedButton();
            this.BtnCycleMoveTime = new UiAsset.SpeedButton();
            this.BtnTitleCycleMoveTime = new UiAsset.SpeedButton();
            this.BtnRepeatCount = new UiAsset.SpeedButton();
            this.BtnRepeatMove = new UiAsset.ImageButton();
            this.BtnTitleRepeatCount = new UiAsset.SpeedButton();
            this.BtnFirstDelayTime = new UiAsset.SpeedButton();
            this.BtnFirstMoveTime = new UiAsset.SpeedButton();
            this.BtnTitleFirstDelayTime = new UiAsset.SpeedButton();
            this.BtnFirstMove = new UiAsset.ImageButton();
            this.BtnTitleFirstMoveTime = new UiAsset.SpeedButton();
            this.GridViewCylinderTimeList = new System.Windows.Forms.DataGridView();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GridViewCylinderList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnCylinderRepeatMove = new UiAsset.SpeedButton();
            this.BtnTitleCylinderDelayTime = new UiAsset.SpeedButton();
            this.BtnTitleCylinderList = new UiAsset.SpeedButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnGridViewClear = new UiAsset.ImageButton();
            this.BtnSave = new UiAsset.ImageButton();
            this.BtnRepeatStop = new UiAsset.ImageButton();
            this.backgroundWorkerRepeat = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewCylinderTimeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewCylinderList)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnSecondMove
            // 
            this.BtnSecondMove.BackColor = System.Drawing.Color.Transparent;
            this.BtnSecondMove.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSecondMove.ButtonText = "SECOND MOVE";
            this.BtnSecondMove.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnSecondMove.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSecondMove.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSecondMove.Location = new System.Drawing.Point(651, 168);
            this.BtnSecondMove.Name = "BtnSecondMove";
            this.BtnSecondMove.Size = new System.Drawing.Size(200, 46);
            this.BtnSecondMove.TabIndex = 64;
            this.BtnSecondMove.Text = "SECOND MOVE";
            this.BtnSecondMove.UseVisualStyleBackColor = false;
            this.BtnSecondMove.Click += new System.EventHandler(this.BtnSecondMove_Click);
            // 
            // BtnTitleSecondMoveTime
            // 
            this.BtnTitleSecondMoveTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleSecondMoveTime.Location = new System.Drawing.Point(856, 220);
            this.BtnTitleSecondMoveTime.Name = "BtnTitleSecondMoveTime";
            this.BtnTitleSecondMoveTime.Size = new System.Drawing.Size(200, 46);
            this.BtnTitleSecondMoveTime.TabIndex = 64;
            this.BtnTitleSecondMoveTime.TabStop = false;
            this.BtnTitleSecondMoveTime.Text = "SECOND MOVE TIME";
            this.BtnTitleSecondMoveTime.UseVisualStyleBackColor = true;
            // 
            // BtnSecondDelayTime
            // 
            this.BtnSecondDelayTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSecondDelayTime.Location = new System.Drawing.Point(1064, 64);
            this.BtnSecondDelayTime.Name = "BtnSecondDelayTime";
            this.BtnSecondDelayTime.Size = new System.Drawing.Size(200, 46);
            this.BtnSecondDelayTime.TabIndex = 65;
            this.BtnSecondDelayTime.TabStop = false;
            this.BtnSecondDelayTime.UseVisualStyleBackColor = true;
            this.BtnSecondDelayTime.Click += new System.EventHandler(this.BtnSecondDelayTime_Click);
            // 
            // BtnSecondMoveTime
            // 
            this.BtnSecondMoveTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSecondMoveTime.Location = new System.Drawing.Point(1063, 220);
            this.BtnSecondMoveTime.Name = "BtnSecondMoveTime";
            this.BtnSecondMoveTime.Size = new System.Drawing.Size(200, 46);
            this.BtnSecondMoveTime.TabIndex = 65;
            this.BtnSecondMoveTime.TabStop = false;
            this.BtnSecondMoveTime.UseVisualStyleBackColor = true;
            // 
            // BtnTitleSecondDelayTime
            // 
            this.BtnTitleSecondDelayTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleSecondDelayTime.Location = new System.Drawing.Point(857, 64);
            this.BtnTitleSecondDelayTime.Name = "BtnTitleSecondDelayTime";
            this.BtnTitleSecondDelayTime.Size = new System.Drawing.Size(200, 46);
            this.BtnTitleSecondDelayTime.TabIndex = 65;
            this.BtnTitleSecondDelayTime.TabStop = false;
            this.BtnTitleSecondDelayTime.Text = "SECOND MOVE DELAY TIME";
            this.BtnTitleSecondDelayTime.UseVisualStyleBackColor = true;
            // 
            // BtnCycleMoveTime
            // 
            this.BtnCycleMoveTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCycleMoveTime.Location = new System.Drawing.Point(651, 220);
            this.BtnCycleMoveTime.Name = "BtnCycleMoveTime";
            this.BtnCycleMoveTime.Size = new System.Drawing.Size(200, 46);
            this.BtnCycleMoveTime.TabIndex = 65;
            this.BtnCycleMoveTime.TabStop = false;
            this.BtnCycleMoveTime.UseVisualStyleBackColor = true;
            // 
            // BtnTitleCycleMoveTime
            // 
            this.BtnTitleCycleMoveTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleCycleMoveTime.Location = new System.Drawing.Point(445, 220);
            this.BtnTitleCycleMoveTime.Name = "BtnTitleCycleMoveTime";
            this.BtnTitleCycleMoveTime.Size = new System.Drawing.Size(200, 46);
            this.BtnTitleCycleMoveTime.TabIndex = 65;
            this.BtnTitleCycleMoveTime.TabStop = false;
            this.BtnTitleCycleMoveTime.Text = "CYCLE TIME";
            this.BtnTitleCycleMoveTime.UseVisualStyleBackColor = true;
            // 
            // BtnRepeatCount
            // 
            this.BtnRepeatCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRepeatCount.Location = new System.Drawing.Point(651, 272);
            this.BtnRepeatCount.Name = "BtnRepeatCount";
            this.BtnRepeatCount.Size = new System.Drawing.Size(200, 46);
            this.BtnRepeatCount.TabIndex = 65;
            this.BtnRepeatCount.TabStop = false;
            this.BtnRepeatCount.Text = "0";
            this.BtnRepeatCount.UseVisualStyleBackColor = true;
            this.BtnRepeatCount.Click += new System.EventHandler(this.BtnRepeatCount_Click);
            // 
            // BtnRepeatMove
            // 
            this.BtnRepeatMove.BackColor = System.Drawing.Color.Transparent;
            this.BtnRepeatMove.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnRepeatMove.ButtonText = "REPEAT MOVE";
            this.BtnRepeatMove.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnRepeatMove.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnRepeatMove.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnRepeatMove.Location = new System.Drawing.Point(857, 272);
            this.BtnRepeatMove.Name = "BtnRepeatMove";
            this.BtnRepeatMove.Size = new System.Drawing.Size(200, 46);
            this.BtnRepeatMove.TabIndex = 65;
            this.BtnRepeatMove.Text = "REPEAT MOVE";
            this.BtnRepeatMove.UseVisualStyleBackColor = false;
            this.BtnRepeatMove.Click += new System.EventHandler(this.BtnRepeatMove_Click);
            // 
            // BtnTitleRepeatCount
            // 
            this.BtnTitleRepeatCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleRepeatCount.Location = new System.Drawing.Point(445, 272);
            this.BtnTitleRepeatCount.Name = "BtnTitleRepeatCount";
            this.BtnTitleRepeatCount.Size = new System.Drawing.Size(200, 46);
            this.BtnTitleRepeatCount.TabIndex = 65;
            this.BtnTitleRepeatCount.TabStop = false;
            this.BtnTitleRepeatCount.Text = "REPEAT COUNT";
            this.BtnTitleRepeatCount.UseVisualStyleBackColor = true;
            // 
            // BtnFirstDelayTime
            // 
            this.BtnFirstDelayTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFirstDelayTime.Location = new System.Drawing.Point(651, 64);
            this.BtnFirstDelayTime.Name = "BtnFirstDelayTime";
            this.BtnFirstDelayTime.Size = new System.Drawing.Size(200, 46);
            this.BtnFirstDelayTime.TabIndex = 65;
            this.BtnFirstDelayTime.TabStop = false;
            this.BtnFirstDelayTime.UseVisualStyleBackColor = true;
            this.BtnFirstDelayTime.Click += new System.EventHandler(this.BtnFirstDelayTime_Click);
            // 
            // BtnFirstMoveTime
            // 
            this.BtnFirstMoveTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFirstMoveTime.Location = new System.Drawing.Point(1063, 168);
            this.BtnFirstMoveTime.Name = "BtnFirstMoveTime";
            this.BtnFirstMoveTime.Size = new System.Drawing.Size(200, 46);
            this.BtnFirstMoveTime.TabIndex = 65;
            this.BtnFirstMoveTime.TabStop = false;
            this.BtnFirstMoveTime.UseVisualStyleBackColor = true;
            // 
            // BtnTitleFirstDelayTime
            // 
            this.BtnTitleFirstDelayTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleFirstDelayTime.Location = new System.Drawing.Point(445, 64);
            this.BtnTitleFirstDelayTime.Name = "BtnTitleFirstDelayTime";
            this.BtnTitleFirstDelayTime.Size = new System.Drawing.Size(200, 46);
            this.BtnTitleFirstDelayTime.TabIndex = 65;
            this.BtnTitleFirstDelayTime.TabStop = false;
            this.BtnTitleFirstDelayTime.Text = "FIRST MOVE DELAY TIME";
            this.BtnTitleFirstDelayTime.UseVisualStyleBackColor = true;
            // 
            // BtnFirstMove
            // 
            this.BtnFirstMove.BackColor = System.Drawing.Color.Transparent;
            this.BtnFirstMove.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnFirstMove.ButtonText = "FIRST MOVE";
            this.BtnFirstMove.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.LeftTop | UiAsset.ImageButton.ImageButtonRoundCorner.LeftBottom)));
            this.BtnFirstMove.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnFirstMove.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnFirstMove.Location = new System.Drawing.Point(445, 168);
            this.BtnFirstMove.Name = "BtnFirstMove";
            this.BtnFirstMove.Size = new System.Drawing.Size(200, 46);
            this.BtnFirstMove.TabIndex = 65;
            this.BtnFirstMove.Text = "FIRST MOVE";
            this.BtnFirstMove.UseVisualStyleBackColor = false;
            this.BtnFirstMove.Click += new System.EventHandler(this.BtnFirstMove_Click);
            // 
            // BtnTitleFirstMoveTime
            // 
            this.BtnTitleFirstMoveTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleFirstMoveTime.Location = new System.Drawing.Point(857, 168);
            this.BtnTitleFirstMoveTime.Name = "BtnTitleFirstMoveTime";
            this.BtnTitleFirstMoveTime.Size = new System.Drawing.Size(200, 46);
            this.BtnTitleFirstMoveTime.TabIndex = 65;
            this.BtnTitleFirstMoveTime.TabStop = false;
            this.BtnTitleFirstMoveTime.Text = "FIRST MOVE TIME";
            this.BtnTitleFirstMoveTime.UseVisualStyleBackColor = true;
            // 
            // GridViewCylinderTimeList
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridViewCylinderTimeList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.GridViewCylinderTimeList.ColumnHeadersHeight = 25;
            this.GridViewCylinderTimeList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7});
            this.GridViewCylinderTimeList.Location = new System.Drawing.Point(445, 376);
            this.GridViewCylinderTimeList.MultiSelect = false;
            this.GridViewCylinderTimeList.Name = "GridViewCylinderTimeList";
            this.GridViewCylinderTimeList.ReadOnly = true;
            this.GridViewCylinderTimeList.RowTemplate.Height = 23;
            this.GridViewCylinderTimeList.Size = new System.Drawing.Size(819, 357);
            this.GridViewCylinderTimeList.TabIndex = 41;
            // 
            // Column2
            // 
            this.Column2.FillWeight = 50F;
            this.Column2.HeaderText = "NO";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 50;
            // 
            // Column3
            // 
            this.Column3.FillWeight = 270F;
            this.Column3.HeaderText = "CYLINDER NAME";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 270;
            // 
            // Column4
            // 
            this.Column4.FillWeight = 130F;
            this.Column4.HeaderText = "UP TIME";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 130;
            // 
            // Column5
            // 
            this.Column5.FillWeight = 130F;
            this.Column5.HeaderText = "DOWN TIME";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 130;
            // 
            // Column6
            // 
            this.Column6.FillWeight = 130F;
            this.Column6.HeaderText = "CYCLE TIME";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 130;
            // 
            // Column7
            // 
            this.Column7.FillWeight = 130F;
            this.Column7.HeaderText = "MAX-MIN DELTA";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 130;
            // 
            // GridViewCylinderList
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridViewCylinderList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.GridViewCylinderList.ColumnHeadersHeight = 30;
            this.GridViewCylinderList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.GridViewCylinderList.Location = new System.Drawing.Point(12, 64);
            this.GridViewCylinderList.MultiSelect = false;
            this.GridViewCylinderList.Name = "GridViewCylinderList";
            this.GridViewCylinderList.ReadOnly = true;
            this.GridViewCylinderList.RowTemplate.Height = 50;
            this.GridViewCylinderList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridViewCylinderList.Size = new System.Drawing.Size(427, 669);
            this.GridViewCylinderList.TabIndex = 41;
            this.GridViewCylinderList.SelectionChanged += new System.EventHandler(this.GridViewCylinderList_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column1.HeaderText = "CYLINDER NAME";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // BtnCylinderRepeatMove
            // 
            this.BtnCylinderRepeatMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCylinderRepeatMove.Location = new System.Drawing.Point(445, 116);
            this.BtnCylinderRepeatMove.Name = "BtnCylinderRepeatMove";
            this.BtnCylinderRepeatMove.Size = new System.Drawing.Size(819, 46);
            this.BtnCylinderRepeatMove.TabIndex = 40;
            this.BtnCylinderRepeatMove.TabStop = false;
            this.BtnCylinderRepeatMove.Text = "CYLINDER REPEAT MOVE";
            this.BtnCylinderRepeatMove.UseVisualStyleBackColor = true;
            // 
            // BtnTitleCylinderDelayTime
            // 
            this.BtnTitleCylinderDelayTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleCylinderDelayTime.Location = new System.Drawing.Point(445, 12);
            this.BtnTitleCylinderDelayTime.Name = "BtnTitleCylinderDelayTime";
            this.BtnTitleCylinderDelayTime.Size = new System.Drawing.Size(819, 46);
            this.BtnTitleCylinderDelayTime.TabIndex = 40;
            this.BtnTitleCylinderDelayTime.TabStop = false;
            this.BtnTitleCylinderDelayTime.Text = "CYLINDER DELAY TIME";
            this.BtnTitleCylinderDelayTime.UseVisualStyleBackColor = true;
            // 
            // BtnTitleCylinderList
            // 
            this.BtnTitleCylinderList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleCylinderList.Location = new System.Drawing.Point(12, 12);
            this.BtnTitleCylinderList.Name = "BtnTitleCylinderList";
            this.BtnTitleCylinderList.Size = new System.Drawing.Size(427, 46);
            this.BtnTitleCylinderList.TabIndex = 40;
            this.BtnTitleCylinderList.TabStop = false;
            this.BtnTitleCylinderList.Text = "CYLINDER LIST";
            this.BtnTitleCylinderList.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnGridViewClear
            // 
            this.BtnGridViewClear.BackColor = System.Drawing.Color.Transparent;
            this.BtnGridViewClear.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnGridViewClear.ButtonText = "VIEW CLEAR";
            this.BtnGridViewClear.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnGridViewClear.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnGridViewClear.Location = new System.Drawing.Point(445, 324);
            this.BtnGridViewClear.Name = "BtnGridViewClear";
            this.BtnGridViewClear.Size = new System.Drawing.Size(406, 46);
            this.BtnGridViewClear.TabIndex = 65;
            this.BtnGridViewClear.Text = "VIEW CLEAR";
            this.BtnGridViewClear.UseVisualStyleBackColor = false;
            this.BtnGridViewClear.Click += new System.EventHandler(this.BtnGridViewClear_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.Transparent;
            this.BtnSave.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSave.ButtonText = "SAVE";
            this.BtnSave.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSave.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSave.Location = new System.Drawing.Point(857, 324);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(407, 46);
            this.BtnSave.TabIndex = 65;
            this.BtnSave.Text = "SAVE";
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnRepeatStop
            // 
            this.BtnRepeatStop.BackColor = System.Drawing.Color.Transparent;
            this.BtnRepeatStop.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnRepeatStop.ButtonText = "REPEAT STOP";
            this.BtnRepeatStop.CornerRound = ((UiAsset.ImageButton.ImageButtonRoundCorner)((UiAsset.ImageButton.ImageButtonRoundCorner.RightTop | UiAsset.ImageButton.ImageButtonRoundCorner.RightBottom)));
            this.BtnRepeatStop.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnRepeatStop.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnRepeatStop.Location = new System.Drawing.Point(1063, 272);
            this.BtnRepeatStop.Name = "BtnRepeatStop";
            this.BtnRepeatStop.Size = new System.Drawing.Size(200, 46);
            this.BtnRepeatStop.TabIndex = 66;
            this.BtnRepeatStop.Text = "REPEAT STOP";
            this.BtnRepeatStop.UseVisualStyleBackColor = false;
            this.BtnRepeatStop.Click += new System.EventHandler(this.BtnRepeatStop_Click);
            // 
            // backgroundWorkerRepeat
            // 
            this.backgroundWorkerRepeat.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerRepeat_DoWork);
            // 
            // CFormTeachCylinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.Controls.Add(this.BtnRepeatStop);
            this.Controls.Add(this.BtnSecondMove);
            this.Controls.Add(this.BtnTitleSecondMoveTime);
            this.Controls.Add(this.BtnSecondDelayTime);
            this.Controls.Add(this.BtnSecondMoveTime);
            this.Controls.Add(this.BtnTitleSecondDelayTime);
            this.Controls.Add(this.BtnCycleMoveTime);
            this.Controls.Add(this.BtnTitleCycleMoveTime);
            this.Controls.Add(this.BtnRepeatCount);
            this.Controls.Add(this.BtnGridViewClear);
            this.Controls.Add(this.BtnRepeatMove);
            this.Controls.Add(this.BtnTitleRepeatCount);
            this.Controls.Add(this.BtnFirstDelayTime);
            this.Controls.Add(this.BtnFirstMoveTime);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnTitleFirstDelayTime);
            this.Controls.Add(this.BtnFirstMove);
            this.Controls.Add(this.BtnTitleFirstMoveTime);
            this.Controls.Add(this.GridViewCylinderTimeList);
            this.Controls.Add(this.GridViewCylinderList);
            this.Controls.Add(this.BtnCylinderRepeatMove);
            this.Controls.Add(this.BtnTitleCylinderDelayTime);
            this.Controls.Add(this.BtnTitleCylinderList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormTeachCylinder";
            this.Text = "CFormTeachCylinder";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormTeachCylinder_FormClosed);
            this.Load += new System.EventHandler(this.CFormTeachCylinder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewCylinderTimeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewCylinderList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UiAsset.SpeedButton BtnTitleCylinderList;
        private UiAsset.SpeedButton BtnTitleCylinderDelayTime;
        private UiAsset.SpeedButton BtnCylinderRepeatMove;
        private System.Windows.Forms.DataGridView GridViewCylinderList;
        private UiAsset.SpeedButton BtnTitleSecondMoveTime;
        private UiAsset.SpeedButton BtnTitleFirstMoveTime;
        private UiAsset.SpeedButton BtnTitleFirstDelayTime;
        private UiAsset.SpeedButton BtnTitleSecondDelayTime;
        private UiAsset.SpeedButton BtnTitleRepeatCount;
        private UiAsset.SpeedButton BtnRepeatCount;
        private System.Windows.Forms.DataGridView GridViewCylinderTimeList;
        private UiAsset.SpeedButton BtnFirstDelayTime;
        private UiAsset.SpeedButton BtnSecondDelayTime;
        private UiAsset.SpeedButton BtnSecondMoveTime;
        private UiAsset.SpeedButton BtnFirstMoveTime;
        private UiAsset.ImageButton BtnFirstMove;
        private UiAsset.ImageButton BtnSecondMove;
        private UiAsset.SpeedButton BtnTitleCycleMoveTime;
        private UiAsset.SpeedButton BtnCycleMoveTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private UiAsset.ImageButton BtnRepeatMove;
        private System.Windows.Forms.Timer timer;
        private UiAsset.ImageButton BtnGridViewClear;
        private UiAsset.ImageButton BtnSave;
        private UiAsset.ImageButton BtnRepeatStop;
        private System.ComponentModel.BackgroundWorker backgroundWorkerRepeat;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
    }
}