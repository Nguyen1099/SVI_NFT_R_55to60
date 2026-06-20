namespace SVI_NFT_R.UI.UserControls
{
    partial class UcUnitStatus
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
            this.PnlMoveBase = new System.Windows.Forms.TableLayoutPanel();
            this.LblMovePause = new System.Windows.Forms.Label();
            this.LblMoveRunning = new System.Windows.Forms.Label();
            this.LblTitle = new System.Windows.Forms.Label();
            this.PnlInterlockBase = new System.Windows.Forms.TableLayoutPanel();
            this.LblInterlockOn = new System.Windows.Forms.Label();
            this.LblInterlockOff = new System.Windows.Forms.Label();
            this.PnlTitleInterlockBase = new System.Windows.Forms.TableLayoutPanel();
            this.LblTitleInterlock = new System.Windows.Forms.Label();
            this.PnlTitleMoveBase = new System.Windows.Forms.TableLayoutPanel();
            this.LblTitleMove = new System.Windows.Forms.Label();
            this.LblUnitID = new System.Windows.Forms.Label();
            this.PnlTitleAvailabilityBase = new System.Windows.Forms.TableLayoutPanel();
            this.LblTitleAvailability = new System.Windows.Forms.Label();
            this.PnlAvailability = new System.Windows.Forms.TableLayoutPanel();
            this.LblAvailabilityDown = new System.Windows.Forms.Label();
            this.LblAvailabilityUp = new System.Windows.Forms.Label();
            this.PnlTitleRunBase = new System.Windows.Forms.TableLayoutPanel();
            this.LblTitleRun = new System.Windows.Forms.Label();
            this.PnlRunBase = new System.Windows.Forms.TableLayoutPanel();
            this.LblRunIdle = new System.Windows.Forms.Label();
            this.LblRunRun = new System.Windows.Forms.Label();
            this.BtnUnitUse = new UiAsset.ImageButton();
            this.BtnUnitPause = new UiAsset.ImageButton();
            this.PnlLayout.SuspendLayout();
            this.PnlMoveBase.SuspendLayout();
            this.PnlInterlockBase.SuspendLayout();
            this.PnlTitleInterlockBase.SuspendLayout();
            this.PnlTitleMoveBase.SuspendLayout();
            this.PnlTitleAvailabilityBase.SuspendLayout();
            this.PnlAvailability.SuspendLayout();
            this.PnlTitleRunBase.SuspendLayout();
            this.PnlRunBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlLayout
            // 
            this.PnlLayout.ColumnCount = 4;
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.41176F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.41176F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.58823F));
            this.PnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.58823F));
            this.PnlLayout.Controls.Add(this.BtnUnitUse, 2, 2);
            this.PnlLayout.Controls.Add(this.BtnUnitPause, 3, 2);
            this.PnlLayout.Controls.Add(this.LblTitle, 2, 0);
            this.PnlLayout.Controls.Add(this.LblUnitID, 2, 1);
            this.PnlLayout.Controls.Add(this.PnlTitleAvailabilityBase, 0, 0);
            this.PnlLayout.Controls.Add(this.PnlAvailability, 1, 0);
            this.PnlLayout.Controls.Add(this.PnlTitleInterlockBase, 0, 1);
            this.PnlLayout.Controls.Add(this.PnlInterlockBase, 1, 1);
            this.PnlLayout.Controls.Add(this.PnlTitleMoveBase, 0, 2);
            this.PnlLayout.Controls.Add(this.PnlMoveBase, 1, 2);
            this.PnlLayout.Controls.Add(this.PnlTitleRunBase, 0, 3);
            this.PnlLayout.Controls.Add(this.PnlRunBase, 1, 3);
            this.PnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlLayout.Location = new System.Drawing.Point(3, 3);
            this.PnlLayout.Name = "PnlLayout";
            this.PnlLayout.RowCount = 4;
            this.PnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.PnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.PnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.PnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.PnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.PnlLayout.Size = new System.Drawing.Size(440, 116);
            this.PnlLayout.TabIndex = 1;
            // 
            // PnlMoveBase
            // 
            this.PnlMoveBase.BackColor = System.Drawing.Color.Black;
            this.PnlMoveBase.ColumnCount = 2;
            this.PnlMoveBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PnlMoveBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PnlMoveBase.Controls.Add(this.LblMovePause, 1, 0);
            this.PnlMoveBase.Controls.Add(this.LblMoveRunning, 0, 0);
            this.PnlMoveBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlMoveBase.Location = new System.Drawing.Point(131, 60);
            this.PnlMoveBase.Margin = new System.Windows.Forms.Padding(2);
            this.PnlMoveBase.Name = "PnlMoveBase";
            this.PnlMoveBase.RowCount = 1;
            this.PnlMoveBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlMoveBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.PnlMoveBase.Size = new System.Drawing.Size(125, 25);
            this.PnlMoveBase.TabIndex = 4;
            // 
            // LblMovePause
            // 
            this.LblMovePause.AutoEllipsis = true;
            this.LblMovePause.BackColor = System.Drawing.Color.White;
            this.LblMovePause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblMovePause.Font = new System.Drawing.Font("맑은 고딕", 6.75F);
            this.LblMovePause.ForeColor = System.Drawing.Color.Black;
            this.LblMovePause.Location = new System.Drawing.Point(63, 1);
            this.LblMovePause.Margin = new System.Windows.Forms.Padding(1);
            this.LblMovePause.Name = "LblMovePause";
            this.LblMovePause.Size = new System.Drawing.Size(61, 23);
            this.LblMovePause.TabIndex = 1;
            this.LblMovePause.Text = "PAUSE";
            this.LblMovePause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblMoveRunning
            // 
            this.LblMoveRunning.AutoEllipsis = true;
            this.LblMoveRunning.BackColor = System.Drawing.Color.White;
            this.LblMoveRunning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblMoveRunning.Font = new System.Drawing.Font("맑은 고딕", 6.75F);
            this.LblMoveRunning.ForeColor = System.Drawing.Color.Black;
            this.LblMoveRunning.Location = new System.Drawing.Point(1, 1);
            this.LblMoveRunning.Margin = new System.Windows.Forms.Padding(1, 1, 0, 1);
            this.LblMoveRunning.Name = "LblMoveRunning";
            this.LblMoveRunning.Size = new System.Drawing.Size(61, 23);
            this.LblMoveRunning.TabIndex = 0;
            this.LblMoveRunning.Text = "RUNNING";
            this.LblMoveRunning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblTitle
            // 
            this.LblTitle.AutoEllipsis = true;
            this.PnlLayout.SetColumnSpan(this.LblTitle, 2);
            this.LblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblTitle.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LblTitle.Location = new System.Drawing.Point(260, 2);
            this.LblTitle.Margin = new System.Windows.Forms.Padding(2);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(178, 25);
            this.LblTitle.TabIndex = 2;
            this.LblTitle.Text = "Title";
            this.LblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PnlInterlockBase
            // 
            this.PnlInterlockBase.BackColor = System.Drawing.Color.Black;
            this.PnlInterlockBase.ColumnCount = 2;
            this.PnlInterlockBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PnlInterlockBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PnlInterlockBase.Controls.Add(this.LblInterlockOn, 1, 0);
            this.PnlInterlockBase.Controls.Add(this.LblInterlockOff, 0, 0);
            this.PnlInterlockBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlInterlockBase.Location = new System.Drawing.Point(131, 31);
            this.PnlInterlockBase.Margin = new System.Windows.Forms.Padding(2);
            this.PnlInterlockBase.Name = "PnlInterlockBase";
            this.PnlInterlockBase.RowCount = 1;
            this.PnlInterlockBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlInterlockBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.PnlInterlockBase.Size = new System.Drawing.Size(125, 25);
            this.PnlInterlockBase.TabIndex = 3;
            // 
            // LblInterlockOn
            // 
            this.LblInterlockOn.AutoEllipsis = true;
            this.LblInterlockOn.BackColor = System.Drawing.Color.White;
            this.LblInterlockOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblInterlockOn.Font = new System.Drawing.Font("맑은 고딕", 6.75F);
            this.LblInterlockOn.ForeColor = System.Drawing.Color.Black;
            this.LblInterlockOn.Location = new System.Drawing.Point(63, 1);
            this.LblInterlockOn.Margin = new System.Windows.Forms.Padding(1);
            this.LblInterlockOn.Name = "LblInterlockOn";
            this.LblInterlockOn.Size = new System.Drawing.Size(61, 23);
            this.LblInterlockOn.TabIndex = 1;
            this.LblInterlockOn.Text = "ON";
            this.LblInterlockOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblInterlockOff
            // 
            this.LblInterlockOff.AutoEllipsis = true;
            this.LblInterlockOff.BackColor = System.Drawing.Color.White;
            this.LblInterlockOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblInterlockOff.Font = new System.Drawing.Font("맑은 고딕", 6.75F);
            this.LblInterlockOff.ForeColor = System.Drawing.Color.Black;
            this.LblInterlockOff.Location = new System.Drawing.Point(1, 1);
            this.LblInterlockOff.Margin = new System.Windows.Forms.Padding(1, 1, 0, 1);
            this.LblInterlockOff.Name = "LblInterlockOff";
            this.LblInterlockOff.Size = new System.Drawing.Size(61, 23);
            this.LblInterlockOff.TabIndex = 0;
            this.LblInterlockOff.Text = "OFF";
            this.LblInterlockOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PnlTitleInterlockBase
            // 
            this.PnlTitleInterlockBase.BackColor = System.Drawing.Color.Black;
            this.PnlTitleInterlockBase.ColumnCount = 1;
            this.PnlTitleInterlockBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlTitleInterlockBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.PnlTitleInterlockBase.Controls.Add(this.LblTitleInterlock, 0, 0);
            this.PnlTitleInterlockBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlTitleInterlockBase.Location = new System.Drawing.Point(2, 31);
            this.PnlTitleInterlockBase.Margin = new System.Windows.Forms.Padding(2);
            this.PnlTitleInterlockBase.Name = "PnlTitleInterlockBase";
            this.PnlTitleInterlockBase.RowCount = 1;
            this.PnlTitleInterlockBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlTitleInterlockBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.PnlTitleInterlockBase.Size = new System.Drawing.Size(125, 25);
            this.PnlTitleInterlockBase.TabIndex = 5;
            // 
            // LblTitleInterlock
            // 
            this.LblTitleInterlock.AutoEllipsis = true;
            this.LblTitleInterlock.BackColor = System.Drawing.Color.White;
            this.LblTitleInterlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblTitleInterlock.Font = new System.Drawing.Font("맑은 고딕", 6.75F);
            this.LblTitleInterlock.Location = new System.Drawing.Point(1, 1);
            this.LblTitleInterlock.Margin = new System.Windows.Forms.Padding(1);
            this.LblTitleInterlock.Name = "LblTitleInterlock";
            this.LblTitleInterlock.Size = new System.Drawing.Size(123, 23);
            this.LblTitleInterlock.TabIndex = 0;
            this.LblTitleInterlock.Text = "INTERLOCK";
            this.LblTitleInterlock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PnlTitleMoveBase
            // 
            this.PnlTitleMoveBase.BackColor = System.Drawing.Color.Black;
            this.PnlTitleMoveBase.ColumnCount = 1;
            this.PnlTitleMoveBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlTitleMoveBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.PnlTitleMoveBase.Controls.Add(this.LblTitleMove, 0, 0);
            this.PnlTitleMoveBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlTitleMoveBase.Location = new System.Drawing.Point(2, 60);
            this.PnlTitleMoveBase.Margin = new System.Windows.Forms.Padding(2);
            this.PnlTitleMoveBase.Name = "PnlTitleMoveBase";
            this.PnlTitleMoveBase.RowCount = 1;
            this.PnlTitleMoveBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlTitleMoveBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.PnlTitleMoveBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.PnlTitleMoveBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.PnlTitleMoveBase.Size = new System.Drawing.Size(125, 25);
            this.PnlTitleMoveBase.TabIndex = 5;
            // 
            // LblTitleMove
            // 
            this.LblTitleMove.AutoEllipsis = true;
            this.LblTitleMove.BackColor = System.Drawing.Color.White;
            this.LblTitleMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblTitleMove.Font = new System.Drawing.Font("맑은 고딕", 6.75F);
            this.LblTitleMove.Location = new System.Drawing.Point(1, 1);
            this.LblTitleMove.Margin = new System.Windows.Forms.Padding(1);
            this.LblTitleMove.Name = "LblTitleMove";
            this.LblTitleMove.Size = new System.Drawing.Size(123, 23);
            this.LblTitleMove.TabIndex = 0;
            this.LblTitleMove.Text = "MOVE";
            this.LblTitleMove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblUnitID
            // 
            this.LblUnitID.AutoEllipsis = true;
            this.PnlLayout.SetColumnSpan(this.LblUnitID, 2);
            this.LblUnitID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblUnitID.Font = new System.Drawing.Font("맑은 고딕", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LblUnitID.Location = new System.Drawing.Point(260, 31);
            this.LblUnitID.Margin = new System.Windows.Forms.Padding(2);
            this.LblUnitID.Name = "LblUnitID";
            this.LblUnitID.Size = new System.Drawing.Size(178, 25);
            this.LblUnitID.TabIndex = 6;
            this.LblUnitID.Text = "UNIT ID :";
            this.LblUnitID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PnlTitleAvailabilityBase
            // 
            this.PnlTitleAvailabilityBase.BackColor = System.Drawing.Color.Black;
            this.PnlTitleAvailabilityBase.ColumnCount = 1;
            this.PnlTitleAvailabilityBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlTitleAvailabilityBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.PnlTitleAvailabilityBase.Controls.Add(this.LblTitleAvailability, 0, 0);
            this.PnlTitleAvailabilityBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlTitleAvailabilityBase.Location = new System.Drawing.Point(2, 2);
            this.PnlTitleAvailabilityBase.Margin = new System.Windows.Forms.Padding(2);
            this.PnlTitleAvailabilityBase.Name = "PnlTitleAvailabilityBase";
            this.PnlTitleAvailabilityBase.RowCount = 1;
            this.PnlTitleAvailabilityBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlTitleAvailabilityBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.PnlTitleAvailabilityBase.Size = new System.Drawing.Size(125, 25);
            this.PnlTitleAvailabilityBase.TabIndex = 8;
            // 
            // LblTitleAvailability
            // 
            this.LblTitleAvailability.AutoEllipsis = true;
            this.LblTitleAvailability.BackColor = System.Drawing.Color.White;
            this.LblTitleAvailability.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblTitleAvailability.Font = new System.Drawing.Font("맑은 고딕", 6.75F);
            this.LblTitleAvailability.Location = new System.Drawing.Point(1, 1);
            this.LblTitleAvailability.Margin = new System.Windows.Forms.Padding(1);
            this.LblTitleAvailability.Name = "LblTitleAvailability";
            this.LblTitleAvailability.Size = new System.Drawing.Size(123, 23);
            this.LblTitleAvailability.TabIndex = 0;
            this.LblTitleAvailability.Text = "AVAILABILITY";
            this.LblTitleAvailability.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PnlAvailability
            // 
            this.PnlAvailability.BackColor = System.Drawing.Color.Black;
            this.PnlAvailability.ColumnCount = 2;
            this.PnlAvailability.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PnlAvailability.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PnlAvailability.Controls.Add(this.LblAvailabilityDown, 1, 0);
            this.PnlAvailability.Controls.Add(this.LblAvailabilityUp, 0, 0);
            this.PnlAvailability.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlAvailability.Location = new System.Drawing.Point(131, 2);
            this.PnlAvailability.Margin = new System.Windows.Forms.Padding(2);
            this.PnlAvailability.Name = "PnlAvailability";
            this.PnlAvailability.RowCount = 1;
            this.PnlAvailability.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlAvailability.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.PnlAvailability.Size = new System.Drawing.Size(125, 25);
            this.PnlAvailability.TabIndex = 9;
            // 
            // LblAvailabilityDown
            // 
            this.LblAvailabilityDown.AutoEllipsis = true;
            this.LblAvailabilityDown.BackColor = System.Drawing.Color.White;
            this.LblAvailabilityDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblAvailabilityDown.Font = new System.Drawing.Font("맑은 고딕", 6.75F);
            this.LblAvailabilityDown.ForeColor = System.Drawing.Color.Black;
            this.LblAvailabilityDown.Location = new System.Drawing.Point(63, 1);
            this.LblAvailabilityDown.Margin = new System.Windows.Forms.Padding(1);
            this.LblAvailabilityDown.Name = "LblAvailabilityDown";
            this.LblAvailabilityDown.Size = new System.Drawing.Size(61, 23);
            this.LblAvailabilityDown.TabIndex = 1;
            this.LblAvailabilityDown.Text = "DOWN";
            this.LblAvailabilityDown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblAvailabilityUp
            // 
            this.LblAvailabilityUp.AutoEllipsis = true;
            this.LblAvailabilityUp.BackColor = System.Drawing.Color.White;
            this.LblAvailabilityUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblAvailabilityUp.Font = new System.Drawing.Font("맑은 고딕", 6.75F);
            this.LblAvailabilityUp.ForeColor = System.Drawing.Color.Black;
            this.LblAvailabilityUp.Location = new System.Drawing.Point(1, 1);
            this.LblAvailabilityUp.Margin = new System.Windows.Forms.Padding(1, 1, 0, 1);
            this.LblAvailabilityUp.Name = "LblAvailabilityUp";
            this.LblAvailabilityUp.Size = new System.Drawing.Size(61, 23);
            this.LblAvailabilityUp.TabIndex = 0;
            this.LblAvailabilityUp.Text = "UP";
            this.LblAvailabilityUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PnlTitleRunBase
            // 
            this.PnlTitleRunBase.BackColor = System.Drawing.Color.Black;
            this.PnlTitleRunBase.ColumnCount = 1;
            this.PnlTitleRunBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlTitleRunBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.PnlTitleRunBase.Controls.Add(this.LblTitleRun, 0, 0);
            this.PnlTitleRunBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlTitleRunBase.Location = new System.Drawing.Point(2, 89);
            this.PnlTitleRunBase.Margin = new System.Windows.Forms.Padding(2);
            this.PnlTitleRunBase.Name = "PnlTitleRunBase";
            this.PnlTitleRunBase.RowCount = 1;
            this.PnlTitleRunBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlTitleRunBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.PnlTitleRunBase.Size = new System.Drawing.Size(125, 25);
            this.PnlTitleRunBase.TabIndex = 10;
            // 
            // LblTitleRun
            // 
            this.LblTitleRun.AutoEllipsis = true;
            this.LblTitleRun.BackColor = System.Drawing.Color.White;
            this.LblTitleRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblTitleRun.Font = new System.Drawing.Font("맑은 고딕", 6.75F);
            this.LblTitleRun.Location = new System.Drawing.Point(1, 1);
            this.LblTitleRun.Margin = new System.Windows.Forms.Padding(1);
            this.LblTitleRun.Name = "LblTitleRun";
            this.LblTitleRun.Size = new System.Drawing.Size(123, 23);
            this.LblTitleRun.TabIndex = 0;
            this.LblTitleRun.Text = "RUN";
            this.LblTitleRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PnlRunBase
            // 
            this.PnlRunBase.BackColor = System.Drawing.Color.Black;
            this.PnlRunBase.ColumnCount = 2;
            this.PnlRunBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PnlRunBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PnlRunBase.Controls.Add(this.LblRunRun, 0, 0);
            this.PnlRunBase.Controls.Add(this.LblRunIdle, 1, 0);
            this.PnlRunBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlRunBase.Location = new System.Drawing.Point(131, 89);
            this.PnlRunBase.Margin = new System.Windows.Forms.Padding(2);
            this.PnlRunBase.Name = "PnlRunBase";
            this.PnlRunBase.RowCount = 1;
            this.PnlRunBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PnlRunBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.PnlRunBase.Size = new System.Drawing.Size(125, 25);
            this.PnlRunBase.TabIndex = 11;
            // 
            // LblRunIdle
            // 
            this.LblRunIdle.AutoEllipsis = true;
            this.LblRunIdle.BackColor = System.Drawing.Color.White;
            this.LblRunIdle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblRunIdle.Font = new System.Drawing.Font("맑은 고딕", 6.75F);
            this.LblRunIdle.ForeColor = System.Drawing.Color.Black;
            this.LblRunIdle.Location = new System.Drawing.Point(63, 1);
            this.LblRunIdle.Margin = new System.Windows.Forms.Padding(1);
            this.LblRunIdle.Name = "LblRunIdle";
            this.LblRunIdle.Size = new System.Drawing.Size(61, 23);
            this.LblRunIdle.TabIndex = 1;
            this.LblRunIdle.Text = "IDLE";
            this.LblRunIdle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblRunRun
            // 
            this.LblRunRun.AutoEllipsis = true;
            this.LblRunRun.BackColor = System.Drawing.Color.White;
            this.LblRunRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblRunRun.Font = new System.Drawing.Font("맑은 고딕", 6.75F);
            this.LblRunRun.ForeColor = System.Drawing.Color.Black;
            this.LblRunRun.Location = new System.Drawing.Point(1, 1);
            this.LblRunRun.Margin = new System.Windows.Forms.Padding(1, 1, 0, 1);
            this.LblRunRun.Name = "LblRunRun";
            this.LblRunRun.Size = new System.Drawing.Size(61, 23);
            this.LblRunRun.TabIndex = 0;
            this.LblRunRun.Text = "RUN";
            this.LblRunRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnUnitUse
            // 
            this.BtnUnitUse.BackColor = System.Drawing.Color.Transparent;
            this.BtnUnitUse.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnUnitUse.ButtonText = "USE";
            this.BtnUnitUse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnUnitUse.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnUnitUse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnUnitUse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnUnitUse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnUnitUse.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnUnitUse.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnUnitUse.Location = new System.Drawing.Point(260, 60);
            this.BtnUnitUse.Margin = new System.Windows.Forms.Padding(2);
            this.BtnUnitUse.Name = "BtnUnitUse";
            this.PnlLayout.SetRowSpan(this.BtnUnitUse, 2);
            this.BtnUnitUse.Size = new System.Drawing.Size(86, 54);
            this.BtnUnitUse.TabIndex = 0;
            this.BtnUnitUse.Text = "USE";
            this.BtnUnitUse.UseVisualStyleBackColor = false;
            // 
            // BtnUnitPause
            // 
            this.BtnUnitPause.BackColor = System.Drawing.Color.Transparent;
            this.BtnUnitPause.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnUnitPause.ButtonText = "PAUSE";
            this.BtnUnitPause.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnUnitPause.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnUnitPause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnUnitPause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnUnitPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnUnitPause.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnUnitPause.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnUnitPause.Location = new System.Drawing.Point(350, 60);
            this.BtnUnitPause.Margin = new System.Windows.Forms.Padding(2);
            this.BtnUnitPause.Name = "BtnUnitPause";
            this.PnlLayout.SetRowSpan(this.BtnUnitPause, 2);
            this.BtnUnitPause.Size = new System.Drawing.Size(88, 54);
            this.BtnUnitPause.TabIndex = 0;
            this.BtnUnitPause.Text = "PAUSE";
            this.BtnUnitPause.UseVisualStyleBackColor = false;
            // 
            // UcUnitStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.PnlLayout);
            this.Name = "UcUnitStatus";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(446, 122);
            this.PnlLayout.ResumeLayout(false);
            this.PnlMoveBase.ResumeLayout(false);
            this.PnlInterlockBase.ResumeLayout(false);
            this.PnlTitleInterlockBase.ResumeLayout(false);
            this.PnlTitleMoveBase.ResumeLayout(false);
            this.PnlTitleAvailabilityBase.ResumeLayout(false);
            this.PnlAvailability.ResumeLayout(false);
            this.PnlTitleRunBase.ResumeLayout(false);
            this.PnlRunBase.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.TableLayoutPanel PnlLayout;
        public UiAsset.ImageButton BtnUnitUse;
        public UiAsset.ImageButton BtnUnitPause;
        public System.Windows.Forms.Label LblTitle;
        public System.Windows.Forms.TableLayoutPanel PnlMoveBase;
        public System.Windows.Forms.Label LblMovePause;
        public System.Windows.Forms.Label LblMoveRunning;
        public System.Windows.Forms.TableLayoutPanel PnlInterlockBase;
        public System.Windows.Forms.Label LblInterlockOn;
        public System.Windows.Forms.Label LblInterlockOff;
        public System.Windows.Forms.TableLayoutPanel PnlTitleInterlockBase;
        public System.Windows.Forms.Label LblTitleInterlock;
        public System.Windows.Forms.TableLayoutPanel PnlTitleMoveBase;
        public System.Windows.Forms.Label LblTitleMove;
        public System.Windows.Forms.Label LblUnitID;
        public System.Windows.Forms.TableLayoutPanel PnlTitleAvailabilityBase;
        public System.Windows.Forms.Label LblTitleAvailability;
        public System.Windows.Forms.TableLayoutPanel PnlAvailability;
        public System.Windows.Forms.Label LblAvailabilityDown;
        public System.Windows.Forms.Label LblAvailabilityUp;
        public System.Windows.Forms.TableLayoutPanel PnlTitleRunBase;
        public System.Windows.Forms.Label LblTitleRun;
        public System.Windows.Forms.TableLayoutPanel PnlRunBase;
        public System.Windows.Forms.Label LblRunIdle;
        public System.Windows.Forms.Label LblRunRun;
    }
}
