namespace RecoEngine
{
    partial class cntrlUsers
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbMain = new Telerik.WinControls.UI.RadGroupBox();
            this.grdUsers = new Telerik.WinControls.UI.RadGridView();
            this.pnTop = new Telerik.WinControls.UI.RadPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.btnDelete = new Telerik.WinControls.UI.RadButton();
            this.btnNew = new Telerik.WinControls.UI.RadButton();
            this.btnInactive = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.gbMain)).BeginInit();
            this.gbMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdUsers.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnTop)).BeginInit();
            this.pnTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInactive)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.gbMain.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.gbMain.Controls.Add(this.grdUsers);
            this.gbMain.Controls.Add(this.pnTop);
            this.gbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMain.FooterImageIndex = -1;
            this.gbMain.FooterImageKey = "";
            this.gbMain.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office;
            this.gbMain.HeaderImageIndex = -1;
            this.gbMain.HeaderImageKey = "";
            this.gbMain.HeaderMargin = new System.Windows.Forms.Padding(0);
            this.gbMain.HeaderText = "";
            this.gbMain.Location = new System.Drawing.Point(0, 0);
            this.gbMain.Margin = new System.Windows.Forms.Padding(0);
            this.gbMain.Name = "gbMain";
            this.gbMain.Padding = new System.Windows.Forms.Padding(2, 18, 2, 2);
            this.gbMain.Size = new System.Drawing.Size(767, 527);
            this.gbMain.TabIndex = 0;
            // 
            // grdUsers
            // 
            this.grdUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdUsers.Location = new System.Drawing.Point(2, 54);
            this.grdUsers.Name = "grdUsers";
            this.grdUsers.Size = new System.Drawing.Size(763, 471);
            this.grdUsers.TabIndex = 8;
            this.grdUsers.CellValueChanged += new Telerik.WinControls.UI.GridViewCellEventHandler(this.grdUsers_CellValueChanged);
            this.grdUsers.DoubleClick += new System.EventHandler(this.grdUsers_DoubleClick);
            // 
            // pnTop
            // 
            this.pnTop.Controls.Add(this.btnInactive);
            this.pnTop.Controls.Add(this.label5);
            this.pnTop.Controls.Add(this.btnDelete);
            this.pnTop.Controls.Add(this.btnNew);
            this.pnTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTop.Location = new System.Drawing.Point(2, 18);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(763, 36);
            this.pnTop.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(563, 36);
            this.label5.TabIndex = 4;
            this.label5.Text = "User Management";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(621, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(62, 24);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(698, 6);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(62, 24);
            this.btnNew.TabIndex = 3;
            this.btnNew.Text = "New";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnInactive
            // 
            this.btnInactive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInactive.Location = new System.Drawing.Point(544, 6);
            this.btnInactive.Name = "btnInactive";
            this.btnInactive.Size = new System.Drawing.Size(62, 24);
            this.btnInactive.TabIndex = 5;
            this.btnInactive.Text = "InActive";
            this.btnInactive.Click += new System.EventHandler(this.btnUserInActive_Click);
            // 
            // cntrlUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbMain);
            this.Name = "cntrlUsers";
            this.Size = new System.Drawing.Size(767, 527);
            this.Load += new System.EventHandler(this.cntrlUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gbMain)).EndInit();
            this.gbMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdUsers.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnTop)).EndInit();
            this.pnTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInactive)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox gbMain;
        private Telerik.WinControls.UI.RadPanel pnTop;
        private Telerik.WinControls.UI.RadButton btnDelete;
        private Telerik.WinControls.UI.RadButton btnNew;
        private Telerik.WinControls.UI.RadGridView grdUsers;
        private System.Windows.Forms.Label label5;
        private Telerik.WinControls.UI.RadButton btnInactive;
    }
}
