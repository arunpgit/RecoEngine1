using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecoEngine_BI;
using RecoEngine_DataLayer;
using Telerik.WinControls;
using System.Configuration;
namespace RecoEngine
{
    public partial class cntrlDataConenction : UserControl
    {

        public delegate void EnableDataConenction();
        public event EnableDataConenction EnableDataSource;
       
        public cntrlDataConenction()
        {
            InitializeComponent();
        }

        private void cntrlDataConenction_Load(object sender, EventArgs e)
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

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
          
          
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
                        fnAddConnectionString(strConenctionstring);
                    }
                    catch
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "Invalid Connection", "connection", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        return false;
                    }
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



        }
        private void btnConnection_Click(object sender, EventArgs e)
        {
            string strConenctionstring = "";
            fnValidateConenctionString(ref strConenctionstring);
        }

    }
}
