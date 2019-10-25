using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using RecoEngine_BI;
using RecoEngine_DataLayer;
using System.Configuration;
using Telerik.WinControls.UI;
namespace RecoEngine
{
    public partial class CntrlDataSource : UserControl
    {
        bool bIsloaded = false;
        bool bDataConnectionLoaded = false;
        bool bDataFeildsLoaded = false;
        bool bPreviewLoaded = false;
        public event EventHandler PreviewNxtBtnClick;
        public delegate void EnableDataConenction();
        public event EnableDataConenction EnableDataSource;
        clsDataSource clsDSObj = new clsDataSource();
        int iTabIndex = 1;
        public CntrlDataSource(int iTabIndex)
        {
            this.iTabIndex = iTabIndex;
            InitializeComponent();

        }
        public void fnSetFocus()
        {
            txtServerName.Focus();
        }
        private void CntrlDataSource_Load(object sender, EventArgs e)
        {
            if (Common.bIsConnectionStringEstablish)
            {
               // pgDataConenction.Enabled = false;
                btnConnection.Enabled = false;
                 if (iTabIndex == 3)
                {
                    pgDataSource.SelectedPage = pgPreview;
                    fnLoadPreview();
                }
                else
                {
                    pgDataSource.SelectedPage = pgDataFields;
                    fnLoadDataFeilds();
                }
            }
            else
            {
                pgDataSource.SelectedPage = pgDataConenction;
                fnLoadDataConnection();

            }
        }
        private void fnLoadPreview()
        {
            try
            {
                if (Common.bIsConnectionStringEstablish)
                {
                    lblError.Visible = false;
                    if (!bPreviewLoaded)
                    {
                        //DataTable dt = clsDSObj.fnGetTreDetails(Common.strTableName);
                        DataTable dt = clsDSObj.fnGetTreDetails("Tre_Random");
                        grdPreview.DataSource = dt;
                        this.grdPreview.MasterTemplate.Refresh(null); 
                        grdPreview.Refresh();
                        grdPreview.AllowAddNewRow = false;
                        grdPreview.ShowRowHeaderColumn = false;
                        grdPreview.EnableGrouping = false;
                        grdPreview.ShowFilteringRow = false;
                        grdPreview.AutoSizeRows = false;
                        grdPreview.AllowEditRow = false;
                        grdPreview.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                        grdPreview.MasterTemplate.BestFitColumns();
                        grdPreview.AutoScroll = true;
                    }
                }
                else
                {
                    grdPreview.Visible = false;
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void fnLoadDataFeilds()
        {
            try
            {
                if (Common.bIsConnectionStringEstablish)
                {
                    lblError.Visible = false;
                    if (!bDataFeildsLoaded)
                    {
                        CntrlDataFields ctlDataSource = new CntrlDataFields();
                        ctlDataSource.Dock = DockStyle.Fill;

                        Telerik.WinControls.UI.RadGroupBox gbDummy = Common.GetfrmDummy();
                        pgDataFields.Controls.Add(gbDummy);
                        ctlDataSource.DataFieldsNxtBtnClick += new EventHandler(ctlDataSource_DataFieldsNxtBtnClick);
                        pgDataFields.Controls.Add(ctlDataSource);
                        pgDataFields.Controls.Remove(gbDummy);
                        bDataFeildsLoaded = true;
                    }
                }
                else
                {
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        void ctlDataSource_DataFieldsNxtBtnClick(object sender, EventArgs e)
        {
            bPreviewLoaded = false;
            pgDataSource.SelectedPage = pgPreview;
        
           //fnLoadPreview();
        }
        private void fnLoadDataConnection()
        {
            try
            {
                fnLoadSource();
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        void fnLoadSource()
        {
            try
            {
                DataTable dtTypes = new DataTable();
                dtTypes.Columns.Add(new DataColumn("Id", typeof(int)));
                dtTypes.Columns.Add(new DataColumn("Type", typeof(string)));
                dtTypes.Rows.Add(((int)Enums.DBType.Oracle).ToString(), "Oracle");
                //  dtTypes.Rows.Add(((int)Enums.DBType.SQl).ToString(), "SQL Server");


                ddlSource.DataSource = dtTypes;
                ddlSource.ValueMember = "Id";
                ddlSource.DisplayMember = "Type";

                ddlSource.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool fnValidateConenctionString(ref string strConenctionstring)
        {
            try
            {
                if (txtServerName.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Server Name should not be blank.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtServerName.Focus();
                    return false;
                }
                if (txtDBName.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Database Name should not be blank.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtDBName.Focus();
                    return false;
                }
                if (txtUName.Text.Trim().Length == 0)
                {
                    MessageBox.Show("User Name should not be blank.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUName.Focus();
                    return false;
                }
                if (txtPassword.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Password should not be blank.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPassword.Focus();
                    return false;
                }

                if (ddlSource.SelectedValue.ToString() == ((int)Enums.DBType.Oracle).ToString())
                {
                    //Data Source=localhost:1521/XE;Persist Security Info=True;Password=reco123;User ID=recousr;Unicode=True
                    strConenctionstring = "Data Source=" + txtServerName.Text.Trim() + "/" + txtDBName.Text.Trim() + ";Persist Security Info=True;Unicode=True;";
                    strConenctionstring += "User ID=" + txtUName.Text.Trim() + ";password=" + txtPassword.Text.Trim();

                    RecoEngine_BI.Common.iDBType = 1;
                    RecoEngine_BI.Common.SetConnectionString(strConenctionstring);
                    try
                    {
                        ((OraDBManager)RecoEngine_BI.Common.dbMgr).ExecuteNonQuery(CommandType.Text, "select sysdate from dual");
                        Common.bIsConnectionStringEstablish = true;
                    }
                    catch
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "Invalid Connection", "connection", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                    fnAddConnectionString(strConenctionstring);
                }

                //DialogResult ds; 
                //string themeName = this.radComboThemes.SelectedItem.Text; 
                //Telerik.WinControls.RadMessageBox.SetThemeName(themeName); ds = Telerik.WinControls.RadMessageBox.Show(this, this.radTxtMessage.Text, this.radTxtCaption.Text, buttons, icon, MessageBoxDefaultButton.Button1, rtl); 


                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void fnAddConnectionString(string strConnectionstring)
        {
           
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); // Add an Application Setting.
            config.AppSettings.Settings.Add("ConnectionString",
                   strConnectionstring + " ");

            // Save the changes in App.config file.

            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");

            pgDataSource.SelectedPage = pgDataFields;
            fnLoadDataFeilds();
            
        }
        private void btnConnection_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string strConenctionstring = "";
                fnValidateConenctionString(ref strConenctionstring);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        void ctlDataConnection_EnableDataSource()
        {
            if (EnableDataSource != null)
                EnableDataSource();
        }
        private void pgDataSource_SelectedPageChanged(object sender, EventArgs e)
        {
            //datafetch();
            try
            {
                switch (pgDataSource.SelectedPage.Tag.ToString().ToLower())
                {
                    case "dataconnection":
                        fnLoadDataConnection();
                        break;
                    case "datafields":
                        fnLoadDataFeilds();
                        break;
                    case "preview":
                        fnLoadPreview();
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pgDataSource.SelectedPage = pgDataFields;
            fnLoadDataFeilds();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            if (PreviewNxtBtnClick != null)
            {
                PreviewNxtBtnClick(this, e);

            }
        }
      

       
       
    }
}

