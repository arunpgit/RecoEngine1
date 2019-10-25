using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using RecoEngine_BI;
using Telerik.WinControls.UI;
using System.Collections;
namespace RecoEngine
{
    public partial class cntrlUsers : UserControl
    {
        clsUsers clsObj = new clsUsers();
        DataTable dt = null;
        public cntrlUsers()
        {
            InitializeComponent();
        }
        private void fnLoadGrid()
        {
            try
            {
                dt = clsObj.fnGetUsersDetails();
                dt.Columns.Add(new DataColumn("Select", typeof(bool)));
                dt.Columns["Select"].SetOrdinal(0);
                dt.Columns.Add(new DataColumn("Active", typeof(bool)));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Select"] = false;
                    if (dt.Rows[i]["ISACTIVE"].ToString() == "Yes")
                        dt.Rows[i]["Active"] = true;
                    else
                        dt.Rows[i]["Active"] = false;

                }
                grdUsers.DataSource = dt;
                grdUsers.AllowAddNewRow = false;
                grdUsers.ShowRowHeaderColumn = false;
                grdUsers.EnableGrouping = false;
                grdUsers.ShowFilteringRow = false;
                grdUsers.AutoSizeRows = false;
                grdUsers.Columns["User_Id"].IsVisible = false;
                grdUsers.Columns["Flag"].IsVisible = false;


                for (int i = 1; i < dt.Columns.Count - 1; i++)
                {
                    grdUsers.MasterTemplate.Columns[i].ReadOnly = true;
                }
                grdUsers.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void cntrlUsers_Load(object sender, EventArgs e)
        {
            try
            {
                fnLoadGrid();
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                fnShowUsersForm(0);
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void grdUsers_DoubleClick(object sender, EventArgs e)
        {
            if (grdUsers.MasterView.CurrentRow != null)
            {

                int iUserID = int.Parse(grdUsers.MasterView.CurrentRow.Cells["User_Id"].Value.ToString());
                fnShowUsersForm(iUserID);
            }

        }
        private void fnShowUsersForm(int iUserID)
        {
            try
            {
                frmUsers frm = new frmUsers(iUserID);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    fnLoadGrid();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ((DataTable)grdUsers.DataSource);
                DataRow[] drRow = dt.Select("Select=1");
                if (drRow.Length == 0)
                {
                    Telerik.WinControls.RadMessageBox.Show(this, "Select at least one user.", "Information", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                {

                    DialogResult ds = Telerik.WinControls.RadMessageBox.Show(this, "Do you wish to delete selected user(s)?", "Confirmation", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                    if (ds != DialogResult.Yes)
                    {
                        return;
                    }
                    ArrayList recForDelete = new ArrayList();
                    string strId = "";
                    for (int i = 0; i < drRow.Length; i++)
                    {
                        strId = drRow[i]["User_Id"].ToString();

                        if (strId == Common.iUserID.ToString())
                        {
                            Telerik.WinControls.RadMessageBox.Show(this, "You canot delete current log in user.", "Information", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                            break;
                        }
                        recForDelete.Add(new ValueItemPair(strId, ""));
                    }

                    if (recForDelete.Count > 0)
                    {
                        string curRecId = "";

                        for (int i = 0; i < recForDelete.Count; i++)
                        {

                            curRecId = ((ValueItemPair)recForDelete[i]).Value.ToString();
                            if (!clsObj.fnDeleteUser(curRecId))
                            {
                                return;
                            }
                            else
                            {
                               // dt.Select("User_ID=" + curRecId.ToString())[0].Delete();
                            }
                        }
                    }
                    fnLoadGrid();
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void btnUserInActive_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ((DataTable)grdUsers.DataSource);
                DataRow[] drRow = dt.Select("Flag='Y'");
                if (drRow.Length == 0)
                {
                    Telerik.WinControls.RadMessageBox.Show(this, "Active/Inactive at least one User.", "Information", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                {

                    DialogResult ds = Telerik.WinControls.RadMessageBox.Show(this, "Do you wish to Active/Inactive selected Campaign(s)?", "Confirmation", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                    if (ds != DialogResult.Yes)
                    {
                        return;
                    }
                    ArrayList recForInactive = new ArrayList();
                    string strId = "";
                    for (int i = 0; i < drRow.Length; i++)
                    {
                        strId = drRow[i]["User_ID"].ToString();
                        if ((bool)drRow[i]["Active"])
                        {
                            strId += ";1";
                        }
                        else
                            strId += ";0";
                        recForInactive.Add(strId);
                    }

                    if (recForInactive.Count > 0)
                    {
                        for (int i = 0; i < recForInactive.Count; i++)
                        {
                            if (!clsObj.fnActiveUsers(recForInactive[i].ToString()))
                            {
                                return;
                            }
                        }
                    }

                    fnLoadGrid();
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void grdUsers_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (grdUsers.CurrentRow != null)
                {
                    if (e.Column.Name.ToUpper() == "ACTIVE")
                    {
                        GridViewRowInfo drRow = grdUsers.CurrentRow;
                        drRow.Cells["Flag"].Value = "Y";
                    }
                }
            }
            catch (Exception ex)
            {

                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);

            }
        }
    }
}
public class ValueItemPair
{
    object _Value = null;
    object _Key = null;

    public ValueItemPair(object Value, object Key)
    {
        _Value = Value;
        _Key = Key;
    }
    public object Value
    {
        get { return _Value; }
    }

    public object Key
    {
        get { return _Key; }
    }
}