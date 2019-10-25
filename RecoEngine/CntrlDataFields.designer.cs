namespace RecoEngine
{
    partial class CntrlDataFields
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
            this.dataschemaGrid = new Telerik.WinControls.UI.RadGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNext = new Telerik.WinControls.UI.RadButton();
            this.btnFilter = new Telerik.WinControls.UI.RadButton();
            this.btnSave = new Telerik.WinControls.UI.RadButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAddColumn = new Telerik.WinControls.UI.RadButton();
            this.lblTabName = new System.Windows.Forms.Label();
            this.ddlTableName = new Telerik.WinControls.UI.RadDropDownList();
            this.gbMain = new Telerik.WinControls.UI.RadGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataschemaGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataschemaGrid.MasterTemplate)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlTableName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbMain)).BeginInit();
            this.gbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataschemaGrid
            // 
            this.dataschemaGrid.AutoScroll = true;
            this.dataschemaGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataschemaGrid.Location = new System.Drawing.Point(2, 54);
            // 
            // dataschemaGrid
            // 
            this.dataschemaGrid.MasterTemplate.AllowAddNewRow = false;
            this.dataschemaGrid.MasterTemplate.AllowCellContextMenu = false;
            this.dataschemaGrid.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.dataschemaGrid.MasterTemplate.AllowDeleteRow = false;
            this.dataschemaGrid.MasterTemplate.AllowDragToGroup = false;
            this.dataschemaGrid.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.dataschemaGrid.Name = "dataschemaGrid";
            this.dataschemaGrid.Size = new System.Drawing.Size(500, 330);
            this.dataschemaGrid.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnFilter);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 384);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 36);
            this.panel1.TabIndex = 1;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(428, 6);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(65, 24);
            this.btnNext.TabIndex = 10;
            this.btnNext.Text = "Next";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter.Location = new System.Drawing.Point(286, 6);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(65, 24);
            this.btnFilter.TabIndex = 9;
            this.btnFilter.Text = "Filter";
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(357, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 24);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.btnAddColumn);
            this.panel2.Controls.Add(this.lblTabName);
            this.panel2.Controls.Add(this.ddlTableName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(2, 18);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(500, 36);
            this.panel2.TabIndex = 2;
            // 
            // btnAddColumn
            // 
            this.btnAddColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddColumn.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddColumn.Location = new System.Drawing.Point(401, 6);
            this.btnAddColumn.Name = "btnAddColumn";
            this.btnAddColumn.Size = new System.Drawing.Size(96, 24);
            this.btnAddColumn.TabIndex = 10;
            this.btnAddColumn.Text = "Create Column";
            this.btnAddColumn.Click += new System.EventHandler(this.btnAddColumn_Click);
            // 
            // lblTabName
            // 
            this.lblTabName.AutoSize = true;
            this.lblTabName.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTabName.Location = new System.Drawing.Point(31, 12);
            this.lblTabName.Name = "lblTabName";
            this.lblTabName.Size = new System.Drawing.Size(65, 14);
            this.lblTabName.TabIndex = 9;
            this.lblTabName.Text = "Select Table";
            // 
            // ddlTableName
            // 
            this.ddlTableName.DropDownAnimationEnabled = true;
            this.ddlTableName.Font = new System.Drawing.Font("Arial", 8.25F);
            this.ddlTableName.Location = new System.Drawing.Point(148, 8);
            this.ddlTableName.MaxDropDownItems = 0;
            this.ddlTableName.Name = "ddlTableName";
            this.ddlTableName.ShowImageInEditorArea = true;
            this.ddlTableName.Size = new System.Drawing.Size(186, 18);
            this.ddlTableName.TabIndex = 8;
            this.ddlTableName.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.ddlTableName_SelectedIndexChanged);
            // 
            // gbMain
            // 
            this.gbMain.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.gbMain.Controls.Add(this.dataschemaGrid);
            this.gbMain.Controls.Add(this.panel1);
            this.gbMain.Controls.Add(this.panel2);
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
            this.gbMain.Size = new System.Drawing.Size(504, 422);
            this.gbMain.TabIndex = 3;
            // 
            // CntrlDataFields
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Controls.Add(this.gbMain);
            this.Name = "CntrlDataFields";
            this.Size = new System.Drawing.Size(504, 422);
            this.Load += new System.EventHandler(this.CntrlDataFields_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataschemaGrid.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataschemaGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlTableName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbMain)).EndInit();
            this.gbMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView dataschemaGrid;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RadButton btnSave;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTabName;
        private Telerik.WinControls.UI.RadDropDownList ddlTableName;
        private Telerik.WinControls.UI.RadGroupBox gbMain;
        private Telerik.WinControls.UI.RadButton btnAddColumn;
        private Telerik.WinControls.UI.RadButton btnFilter;
        private Telerik.WinControls.UI.RadButton btnNext;
    }
}
