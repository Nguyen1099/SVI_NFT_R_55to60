namespace SVI_NFT_R
{
    partial class CFormSetupDB
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
            this.GridViewDB = new System.Windows.Forms.DataGridView();
            this.GridViewEditDB = new System.Windows.Forms.DataGridView();
            this.panelFormMenu = new System.Windows.Forms.Panel();
            this.BtnBase = new UiAsset.ImageButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnInsert = new UiAsset.ImageButton();
            this.BtnUpdate = new UiAsset.ImageButton();
            this.BtnDelete = new UiAsset.ImageButton();
            this.BtnSaveToTxt = new UiAsset.ImageButton();
            this.BtnCommit = new UiAsset.ImageButton();
            this.BtnRollback = new UiAsset.ImageButton();
            this.BtnInputPosition = new UiAsset.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewEditDB)).BeginInit();
            this.panelFormMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // GridViewDB
            // 
            this.GridViewDB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewDB.Location = new System.Drawing.Point(12, 64);
            this.GridViewDB.Name = "GridViewDB";
            this.GridViewDB.RowTemplate.Height = 23;
            this.GridViewDB.Size = new System.Drawing.Size(841, 669);
            this.GridViewDB.TabIndex = 0;
            this.GridViewDB.CurrentCellChanged += new System.EventHandler(this.GridViewDB_CurrentCellChanged);
            // 
            // GridViewEditDB
            // 
            this.GridViewEditDB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewEditDB.Location = new System.Drawing.Point(859, 64);
            this.GridViewEditDB.Name = "GridViewEditDB";
            this.GridViewEditDB.RowTemplate.Height = 23;
            this.GridViewEditDB.Size = new System.Drawing.Size(405, 357);
            this.GridViewEditDB.TabIndex = 7;
            this.GridViewEditDB.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewEditDB_CellClick);
            // 
            // panelFormMenu
            // 
            this.panelFormMenu.Controls.Add(this.BtnBase);
            this.panelFormMenu.Location = new System.Drawing.Point(12, 12);
            this.panelFormMenu.Name = "panelFormMenu";
            this.panelFormMenu.Size = new System.Drawing.Size(1252, 46);
            this.panelFormMenu.TabIndex = 8;
            // 
            // BtnBase
            // 
            this.BtnBase.BackColor = System.Drawing.Color.Transparent;
            this.BtnBase.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBase.ButtonText = "BASE";
            this.BtnBase.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnBase.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnBase.Location = new System.Drawing.Point(0, 0);
            this.BtnBase.Name = "BtnBase";
            this.BtnBase.Size = new System.Drawing.Size(142, 46);
            this.BtnBase.TabIndex = 0;
            this.BtnBase.Visible = false;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnInsert
            // 
            this.BtnInsert.BackColor = System.Drawing.Color.Transparent;
            this.BtnInsert.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnInsert.ButtonText = "INSERT";
            this.BtnInsert.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnInsert.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnInsert.Location = new System.Drawing.Point(859, 479);
            this.BtnInsert.Name = "BtnInsert";
            this.BtnInsert.Size = new System.Drawing.Size(405, 46);
            this.BtnInsert.TabIndex = 0;
            this.BtnInsert.Click += new System.EventHandler(this.BtnInsert_Click);
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.BackColor = System.Drawing.Color.Transparent;
            this.BtnUpdate.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnUpdate.ButtonText = "UPDATE";
            this.BtnUpdate.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnUpdate.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnUpdate.Location = new System.Drawing.Point(859, 531);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(405, 46);
            this.BtnUpdate.TabIndex = 0;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.BackColor = System.Drawing.Color.Transparent;
            this.BtnDelete.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnDelete.ButtonText = "DELETE";
            this.BtnDelete.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnDelete.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnDelete.Location = new System.Drawing.Point(859, 583);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(405, 46);
            this.BtnDelete.TabIndex = 0;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnSaveToTxt
            // 
            this.BtnSaveToTxt.BackColor = System.Drawing.Color.Transparent;
            this.BtnSaveToTxt.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSaveToTxt.ButtonText = "SAVE TO TXT";
            this.BtnSaveToTxt.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnSaveToTxt.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnSaveToTxt.Location = new System.Drawing.Point(859, 635);
            this.BtnSaveToTxt.Name = "BtnSaveToTxt";
            this.BtnSaveToTxt.Size = new System.Drawing.Size(405, 46);
            this.BtnSaveToTxt.TabIndex = 0;
            this.BtnSaveToTxt.Click += new System.EventHandler(this.BtnSaveToTxt_Click);
            // 
            // BtnCommit
            // 
            this.BtnCommit.BackColor = System.Drawing.Color.Transparent;
            this.BtnCommit.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCommit.ButtonText = "COMMIT";
            this.BtnCommit.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnCommit.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnCommit.Location = new System.Drawing.Point(859, 687);
            this.BtnCommit.Name = "BtnCommit";
            this.BtnCommit.Size = new System.Drawing.Size(200, 46);
            this.BtnCommit.TabIndex = 0;
            this.BtnCommit.Click += new System.EventHandler(this.BtnCommit_Click);
            // 
            // BtnRollback
            // 
            this.BtnRollback.BackColor = System.Drawing.Color.Transparent;
            this.BtnRollback.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnRollback.ButtonText = "ROLLBACK";
            this.BtnRollback.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnRollback.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnRollback.Location = new System.Drawing.Point(1064, 687);
            this.BtnRollback.Name = "BtnRollback";
            this.BtnRollback.Size = new System.Drawing.Size(200, 46);
            this.BtnRollback.TabIndex = 0;
            this.BtnRollback.Click += new System.EventHandler(this.BtnRollback_Click);
            // 
            // btnInputPosition
            // 
            this.BtnInputPosition.BackColor = System.Drawing.Color.Transparent;
            this.BtnInputPosition.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnInputPosition.ButtonText = "INPUT POSITION";
            this.BtnInputPosition.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BtnInputPosition.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BtnInputPosition.Location = new System.Drawing.Point(859, 427);
            this.BtnInputPosition.Name = "btnInputPosition";
            this.BtnInputPosition.Size = new System.Drawing.Size(405, 46);
            this.BtnInputPosition.TabIndex = 9;
            this.BtnInputPosition.Click += new System.EventHandler(this.btnInputPosition_Click);
            // 
            // CFormSetupDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1276, 745);
            this.Controls.Add(this.BtnInputPosition);
            this.Controls.Add(this.BtnRollback);
            this.Controls.Add(this.BtnCommit);
            this.Controls.Add(this.BtnSaveToTxt);
            this.Controls.Add(this.BtnDelete);
            this.Controls.Add(this.BtnUpdate);
            this.Controls.Add(this.BtnInsert);
            this.Controls.Add(this.panelFormMenu);
            this.Controls.Add(this.GridViewEditDB);
            this.Controls.Add(this.GridViewDB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1276, 745);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1276, 745);
            this.Name = "CFormSetupDB";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormSetupDB";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormSetupDB_FormClosed);
            this.Load += new System.EventHandler(this.CFormSetupDB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewEditDB)).EndInit();
            this.panelFormMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView GridViewDB;
        private System.Windows.Forms.DataGridView GridViewEditDB;
        private System.Windows.Forms.Panel panelFormMenu;
        private System.Windows.Forms.Timer timer;
        private UiAsset.ImageButton BtnBase;
        private UiAsset.ImageButton BtnInsert;
        private UiAsset.ImageButton BtnUpdate;
        private UiAsset.ImageButton BtnDelete;
        private UiAsset.ImageButton BtnSaveToTxt;
        private UiAsset.ImageButton BtnCommit;
        private UiAsset.ImageButton BtnRollback;
        private UiAsset.ImageButton BtnInputPosition;
    }
}