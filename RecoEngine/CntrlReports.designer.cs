namespace RecoEngine
{
    partial class CntrlReports
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
            this.reportsPage = new Telerik.WinControls.UI.RadPageView();
            this.reportsOppurtunity = new Telerik.WinControls.UI.RadPageViewPage();
            this.reportsCampaigns = new Telerik.WinControls.UI.RadPageViewPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.RankingGrid = new Telerik.WinControls.UI.RadGridView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.cbRank = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblAccountsprecent = new System.Windows.Forms.Label();
            this.lblAccounts = new System.Windows.Forms.Label();
            this.lblPotential = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.reportsPage)).BeginInit();
            this.reportsPage.SuspendLayout();
            this.reportsOppurtunity.SuspendLayout();
            this.reportsCampaigns.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RankingGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RankingGrid.MasterTemplate)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // reportsPage
            // 
            this.reportsPage.Controls.Add(this.reportsOppurtunity);
            this.reportsPage.Controls.Add(this.reportsCampaigns);
            this.reportsPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportsPage.Location = new System.Drawing.Point(0, 0);
            this.reportsPage.Name = "reportsPage";
            this.reportsPage.SelectedPage = this.reportsOppurtunity;
            this.reportsPage.Size = new System.Drawing.Size(711, 426);
            this.reportsPage.TabIndex = 0;
            this.reportsPage.ThemeName = "ControlDefault";
            this.reportsPage.SelectedPageChanged += new System.EventHandler(this.reportsPage_SelectedPageChanged);
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.reportsPage.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
            // 
            // reportsOppurtunity
            // 
            this.reportsOppurtunity.Controls.Add(this.pnlMain);
            this.reportsOppurtunity.Location = new System.Drawing.Point(10, 37);
            this.reportsOppurtunity.Name = "reportsOppurtunity";
            this.reportsOppurtunity.Size = new System.Drawing.Size(690, 378);
            this.reportsOppurtunity.Tag = "reportsOpportuntiy";
            this.reportsOppurtunity.Text = "Oppurtunity";
            // 
            // reportsCampaigns
            // 
            this.reportsCampaigns.Controls.Add(this.panel1);
            this.reportsCampaigns.Location = new System.Drawing.Point(10, 37);
            this.reportsCampaigns.Name = "reportsCampaigns";
            this.reportsCampaigns.Size = new System.Drawing.Size(690, 378);
            this.reportsCampaigns.Tag = "reportsCampaigns";
            this.reportsCampaigns.Text = "Campaigns";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlBottom);
            this.panel1.Controls.Add(this.pnlTop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(690, 378);
            this.panel1.TabIndex = 0;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.RankingGrid);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(0, 89);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(690, 289);
            this.pnlBottom.TabIndex = 1;
            // 
            // RankingGrid
            // 
            this.RankingGrid.AutoScroll = true;
            this.RankingGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RankingGrid.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.RankingGrid.MasterTemplate.AllowAddNewRow = false;
            this.RankingGrid.MasterTemplate.AllowDeleteRow = false;
            this.RankingGrid.MasterTemplate.AllowEditRow = false;
            this.RankingGrid.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.RankingGrid.Name = "RankingGrid";
            this.RankingGrid.Size = new System.Drawing.Size(690, 289);
            this.RankingGrid.TabIndex = 0;
            this.RankingGrid.Text = "radGridView1";
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.cbRank);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Controls.Add(this.lblAccountsprecent);
            this.pnlTop.Controls.Add(this.lblAccounts);
            this.pnlTop.Controls.Add(this.lblPotential);
            this.pnlTop.Controls.Add(this.label3);
            this.pnlTop.Controls.Add(this.label2);
            this.pnlTop.Controls.Add(this.lbl);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(690, 89);
            this.pnlTop.TabIndex = 0;
            // 
            // cbRank
            // 
            this.cbRank.Items.AddRange(new object[] {
            "Rank1",
            "Rank2",
            "Rank3",
            "Rank4"});
            this.cbRank.Location = new System.Drawing.Point(105, 33);
            this.cbRank.Name = "cbRank";
            this.cbRank.Size = new System.Drawing.Size(121, 21);
            this.cbRank.TabIndex = 15;
            this.cbRank.SelectedIndexChanged += new System.EventHandler(this.cbRank_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "Ranking :";
            // 
            // lblAccountsprecent
            // 
            this.lblAccountsprecent.AutoSize = true;
            this.lblAccountsprecent.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccountsprecent.Location = new System.Drawing.Point(564, 61);
            this.lblAccountsprecent.Name = "lblAccountsprecent";
            this.lblAccountsprecent.Size = new System.Drawing.Size(0, 17);
            this.lblAccountsprecent.TabIndex = 13;
            // 
            // lblAccounts
            // 
            this.lblAccounts.AutoSize = true;
            this.lblAccounts.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccounts.Location = new System.Drawing.Point(564, 39);
            this.lblAccounts.Name = "lblAccounts";
            this.lblAccounts.Size = new System.Drawing.Size(0, 17);
            this.lblAccounts.TabIndex = 12;
            // 
            // lblPotential
            // 
            this.lblPotential.AutoSize = true;
            this.lblPotential.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPotential.Location = new System.Drawing.Point(564, 16);
            this.lblPotential.Name = "lblPotential";
            this.lblPotential.Size = new System.Drawing.Size(0, 17);
            this.lblPotential.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(349, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(202, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Accounts % without Opportunity :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(349, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Total Accounts :";
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl.Location = new System.Drawing.Point(349, 16);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(98, 17);
            this.lbl.TabIndex = 8;
            this.lbl.Text = "Total Potential :";
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(690, 378);
            this.pnlMain.TabIndex = 0;
            // 
            // CntrlReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.reportsPage);
            this.Name = "CntrlReports";
            this.Size = new System.Drawing.Size(711, 426);
            ((System.ComponentModel.ISupportInitialize)(this.reportsPage)).EndInit();
            this.reportsPage.ResumeLayout(false);
            this.reportsOppurtunity.ResumeLayout(false);
            this.reportsCampaigns.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RankingGrid.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RankingGrid)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPageView reportsPage;
        private Telerik.WinControls.UI.RadPageViewPage reportsOppurtunity;
        private Telerik.WinControls.UI.RadPageViewPage reportsCampaigns;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlBottom;
        private Telerik.WinControls.UI.RadGridView RankingGrid;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.ComboBox cbRank;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAccountsprecent;
        private System.Windows.Forms.Label lblAccounts;
        private System.Windows.Forms.Label lblPotential;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.Panel pnlMain;
    }
}
