namespace RecoEngine
{
    partial class CntrlOpportunityList
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
            this.gbOpportunityList = new Telerik.WinControls.UI.RadGroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdOppList = new Telerik.WinControls.UI.RadGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNew = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.gbOpportunityList)).BeginInit();
            this.gbOpportunityList.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOppList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOppList.MasterTemplate)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).BeginInit();
            this.SuspendLayout();
            // 
            // gbOpportunityList
            // 
            this.gbOpportunityList.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.gbOpportunityList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.gbOpportunityList.Controls.Add(this.panel1);
            this.gbOpportunityList.Controls.Add(this.panel2);
            this.gbOpportunityList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOpportunityList.FooterImageIndex = -1;
            this.gbOpportunityList.FooterImageKey = "";
            this.gbOpportunityList.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office;
            this.gbOpportunityList.HeaderImageIndex = -1;
            this.gbOpportunityList.HeaderImageKey = "";
            this.gbOpportunityList.HeaderMargin = new System.Windows.Forms.Padding(0);
            this.gbOpportunityList.HeaderText = "";
            this.gbOpportunityList.Location = new System.Drawing.Point(0, 0);
            this.gbOpportunityList.Name = "gbOpportunityList";
            this.gbOpportunityList.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            // 
            // 
            // 
            this.gbOpportunityList.RootElement.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.gbOpportunityList.Size = new System.Drawing.Size(712, 524);
            this.gbOpportunityList.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grdOppList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(708, 487);
            this.panel1.TabIndex = 4;
            // 
            // grdOppList
            // 
            this.grdOppList.AutoScroll = true;
            this.grdOppList.BackColor = System.Drawing.Color.Transparent;
            this.grdOppList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdOppList.Location = new System.Drawing.Point(0, 0);
            // 
            // grdOppList
            // 
            this.grdOppList.MasterTemplate.AllowAddNewRow = false;
            this.grdOppList.MasterTemplate.AllowCellContextMenu = false;
            this.grdOppList.MasterTemplate.AllowColumnChooser = false;
            this.grdOppList.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.grdOppList.MasterTemplate.AllowColumnReorder = false;
            this.grdOppList.MasterTemplate.AllowDeleteRow = false;
            this.grdOppList.MasterTemplate.AllowEditRow = false;
            this.grdOppList.MasterTemplate.EnableGrouping = false;
            this.grdOppList.Name = "grdOppList";
            this.grdOppList.Size = new System.Drawing.Size(708, 487);
            this.grdOppList.TabIndex = 0;
            this.grdOppList.Text = "radGridView1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnNew);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(2, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(708, 37);
            this.panel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(-3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(516, 34);
            this.label1.TabIndex = 2;
            this.label1.Text = "Opportunity Creation";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(639, 5);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(66, 29);
            this.btnNew.TabIndex = 14;
            this.btnNew.Text = "Create New";
            //this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // CntrlOpportunityList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOpportunityList);
            this.Name = "CntrlOpportunityList";
            this.Size = new System.Drawing.Size(712, 524);
            this.Load += new System.EventHandler(this.CntrlOpportunityList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gbOpportunityList)).EndInit();
            this.gbOpportunityList.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdOppList.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOppList)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox gbOpportunityList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RadGridView grdOppList;
        private Telerik.WinControls.UI.RadButton btnNew;
    }
}
