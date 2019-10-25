using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using System.Collections;
using RecoEngine_BI;
using System.Threading;

namespace RecoEngine
{
    public partial class CntrlOfferLibrary : UserControl
    {
        bool bIsShowOPPList = false;
      
        clsOffers clsObj = new clsOffers();
        int iTabIndex = 0;
        int iOfferID = 0;
        bool bIsOffersLoaded = false;
          public CntrlOfferLibrary(int iTabIndex)
        {
            this.iTabIndex = iTabIndex;
            InitializeComponent();
        }
        private void CntrlOfferLibrary_Load(object sender, EventArgs e)
        {
            try
            {

                switch (iTabIndex)
                {
                    case 1:
                        pgOfferLibrary.SelectedPage = pgOffers;
                        fnShowOffersList(true);
                       
                        break;
                    case 2:
                        pgOfferLibrary.SelectedPage = pgCampaign;
                        fnShowCampaignList(true);
                        break;

                }


            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        void fnShowOffersList(bool bShow)
        {
            try
            {
                if (bShow)
                {
                    bIsShowOPPList = true;
                    gbOffersList.Visible = true;
                    gbMain.Visible = false;
                    gbMain.Dock = DockStyle.None;
                    gbOffersList.Dock = DockStyle.Fill;
                    fnLoadOffers();

                }
                else
                {
                    bIsShowOPPList = false;
                    gbMain.Visible = true;
                    gbMain.Dock = DockStyle.Fill;
                    gbOffersList.Visible = false;
                    gbOffersList.Dock = DockStyle.None;
                }
                //it should get populated in get levels
                if (!bIsOffersLoaded)
                    fnFillLevels();
                bIsOffersLoaded = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void fnLoadOffers()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {


                DataTable dt = clsObj.fnGetOffers(Common.iProjectID);
                dt.Columns.Add(new DataColumn("Select", typeof(bool)));
                dt.Columns.Add(new DataColumn("Active", typeof(bool)));
                dt.Columns["Select"].SetOrdinal(0);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Select"] = false;
                    if (dt.Rows[i]["ISACTIVEID"].ToString() == "1")
                        dt.Rows[i]["Active"] = true;
                    else
                        dt.Rows[i]["Active"] = false;

                }
                grdOffers.DataSource = dt;
                grdOffers.Columns["OFFER_ID"].IsVisible = false;
                grdOffers.Columns["ISACTIVEID"].IsVisible = false;
                grdOffers.Columns["Flag"].IsVisible = false;
                grdOffers.Columns["Project_ID"].IsVisible = false;
                grdOffers.Columns["LEVEL1"].IsVisible = false;
                grdOffers.Columns["LEVEL2"].IsVisible = false;
                grdOffers.Columns["LEVEL3"].IsVisible = false;
                grdOffers.Columns["LEVEL4"].IsVisible = false;
                grdOffers.Columns["DESCRIPTION"].Width = 200;
                for (int i = 1; i < dt.Columns.Count - 1; i++)
                {
                    if (dt.Columns[i].ColumnName != "Select")
                        grdOffers.MasterTemplate.Columns[i].ReadOnly = true;
                }
                grdOffers.CellValueChanged += new GridViewCellEventHandler(grdOffers_CellValueChanged);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        void grdOffers_CellValueChanged(object sender, GridViewCellEventArgs e)
        {

            if (grdOffers.CurrentRow != null)
            {
                if (e.Column.Name.ToUpper() == "ACTIVE")
                {
                    GridViewRowInfo drRow = grdOffers.CurrentRow;
                    drRow.Cells["Flag"].Value = "Y";
                }
            }
        }
        void fnFillLevels()
        {
            try
            {
                DataSet dsLevels = clsObj.fnGetLevels();


                ddlLevel1.DataSource = dsLevels.Tables[0];
                ddlLevel1.ValueMember = "CODE";
                ddlLevel1.DisplayMember = "DESCRIPTION";
                ddlLevel1.SelectedIndex = 0;

                ddlLevel2.DataSource = dsLevels.Tables[1];
                ddlLevel2.ValueMember = "CODE";
                ddlLevel2.DisplayMember = "DESCRIPTION";
                ddlLevel2.SelectedIndex = 0;

                ddlLevel3.DataSource = dsLevels.Tables[2];
                ddlLevel3.ValueMember = "CODE";
                ddlLevel3.DisplayMember = "DESCRIPTION";
                ddlLevel3.SelectedIndex = 0;

                ddlLevel4.DataSource = dsLevels.Tables[3];
                ddlLevel4.ValueMember = "CODE";
                ddlLevel4.DisplayMember = "DESCRIPTION";
                ddlLevel4.SelectedIndex = 0;

                ddlLevel5.DataSource = dsLevels.Tables[4];
                ddlLevel5.ValueMember = "CODE";
                ddlLevel5.DisplayMember = "DESCRIPTION";
                ddlLevel5.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void grdOffers_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (grdOffers.MasterView.CurrentRow != null)
                {

                    iOfferID = int.Parse(grdOffers.MasterView.CurrentRow.Cells["OFFER_ID"].Value.ToString());
                    txtName.Text = grdOffers.MasterView.CurrentRow.Cells["Name"].Value.ToString();
                    txtDesc.Text = grdOffers.MasterView.CurrentRow.Cells["Description"].Value.ToString();
                    txtCode.Text = grdOffers.MasterView.CurrentRow.Cells["CODE"].Value.ToString();

                    ddlLevel2.SelectedValue = grdOffers.MasterView.CurrentRow.Cells["LEVEL1"].Value.ToString();
                    ddlLevel3.SelectedValue = grdOffers.MasterView.CurrentRow.Cells["LEVEL2"].Value.ToString();
                    ddlLevel4.SelectedValue = grdOffers.MasterView.CurrentRow.Cells["LEVEL3"].Value.ToString();
                    ddlLevel5.SelectedValue = grdOffers.MasterView.CurrentRow.Cells["LEVEL4"].Value.ToString();

                    if (grdOffers.MasterView.CurrentRow.Cells["ISACTIVEID"].Value.ToString() == "1")
                        chkIsActive.Checked = true;
                    else
                        chkIsActive.Checked = false;
                    fnShowOffersList(false);

                    //ClsObj
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void radNew_Click(object sender, EventArgs e)
        {
            try
            {
                fnShowOffersList(false);
                iOfferID = 0;
                txtName.Text = "";
                txtCode.Text = "";
                ddlLevel2.SelectedIndex = 0;
                ddlLevel3.SelectedIndex = 0;
                ddlLevel4.SelectedIndex = 0;
                ddlLevel5.SelectedIndex = 0;
                chkIsActive.Checked = true;
                txtDesc.Text = "";
                txtCode.Text = ddlLevel2.SelectedValue.ToString() + "_" + ddlLevel3.SelectedValue.ToString() + "_" + ddlLevel4.SelectedValue.ToString() + "_" + ddlLevel5.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void ddlLevel1_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {
                if (!bIsOffersLoaded)
                    return;
                txtCode.Text = ddlLevel1.SelectedValue.ToString() + "_" + ddlLevel2.SelectedValue.ToString() + "_" + ddlLevel3.SelectedValue.ToString() + "_" + ddlLevel4.SelectedValue.ToString() + "_" + ddlLevel5.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void pgOfferLibrary_SelectedPageChanged(object sender, EventArgs e)
        {
            try
            {
                switch (pgOfferLibrary.SelectedPage.Tag.ToString().ToLower())
                {
                    case "offers":
                        fnShowOffersList(true);
                        bIsCampaignLoaded = false;
                        break;
                    case "campaign":
                        fnShowCampaignList(true);
                        break;

                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text.Trim().Length == 0)
                {
                    Telerik.WinControls.RadMessageBox.Show("Offer Name Should not be blank", "Validation", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                    txtName.Focus();
                    return;
                }


                int iIsActive = 0;
                if (chkIsActive.Checked)
                    iIsActive = 1;

                iOfferID = clsObj.fnSaveOffers(iOfferID, txtName.Text.ToString(), txtCode.Text, txtDesc.Text.ToString(), ddlLevel2.SelectedValue.ToString(), ddlLevel3.SelectedValue.ToString(), ddlLevel4.SelectedValue.ToString(), ddlLevel5.SelectedValue.ToString(), Common.iUserID, Common.iProjectID, iIsActive);

                Telerik.WinControls.RadMessageBox.Show("Successfully saved.", "Validation", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                radNew_Click(null, null);
                frmOriginal frmorgin = (frmOriginal)Common.TopMostParent(this);
                frmorgin.fnOffersOpprortunityCount();
                //frmorgin.fnShowOffers(1);
               
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
    
        private void btnONext_Click(object sender, EventArgs e)
        {
            try
            {
                fnShowOffersList(true);
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ((DataTable)grdOffers.DataSource);
                DataRow[] drRow = dt.Select("Select=1");
                if (drRow.Length == 0)
                {
                    Telerik.WinControls.RadMessageBox.Show(this, "Select at least one Offer.", "Information", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                {

                    DialogResult ds = Telerik.WinControls.RadMessageBox.Show(this, "Do you wish to delete selected Offer(s)?", "Confirmation", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                    if (ds != DialogResult.Yes)
                    {
                        return;
                    }
                    ArrayList recForDelete = new ArrayList();
                    string strId = "";
                    for (int i = 0; i < drRow.Length; i++)
                    {
                        strId = drRow[i]["OFFER_ID"].ToString();

                        if (clsObj.fnCheckOffersDependencies(strId))
                        {
                            ds = Telerik.WinControls.RadMessageBox.Show(this, "This offer has Campaigns?,do you want to delete the campaign details also r\n to delete please click yes else click no", "Confirmation", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                            if (ds != DialogResult.Yes)
                            {
                                break;
                            }
                            else
                                recForDelete.Add(strId);
                        }
                        else
                            recForDelete.Add(strId);


                        //recForDelete.Add(new ValueItemPair(strId, drRow[i]["OPP_NAME"].ToString()));
                    }

                    if (recForDelete.Count > 0)
                    {
                        for (int i = 0; i < recForDelete.Count; i++)
                        {
                            if (!clsObj.fnDeleteOffers(recForDelete[i].ToString()))
                            {
                                return;
                            }
                        }
                    }

                    fnLoadOffers();
                    frmOriginal frmorgin = (frmOriginal)Common.TopMostParent(this);
                    frmorgin.fnOffersOpprortunityCount();
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
       private void btnOInActive_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ((DataTable)grdOffers.DataSource);
                DataRow[] drRow = dt.Select("Flag='Y'");
                if (drRow.Length == 0)
                {
                    Telerik.WinControls.RadMessageBox.Show(this, "Active/Inactive at least one Offer.", "Information", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                {

                    DialogResult ds = Telerik.WinControls.RadMessageBox.Show(this, "Do you wish to Active/Inactive selected Offer(s)?", "Confirmation", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                    if (ds != DialogResult.Yes)
                    {
                        return;
                    }
                    ArrayList recForInactive = new ArrayList();
                    string strId = "";
                    for (int i = 0; i < drRow.Length; i++)
                    {
                        strId = drRow[i]["OFFER_ID"].ToString();
                        if ((bool)drRow[i]["Active"])
                        {
                            strId += ";1";
                        }
                        else
                            strId += ";0";

                        recForInactive.Add(strId);


                        //recForDelete.Add(new ValueItemPair(strId, drRow[i]["OPP_NAME"].ToString()));
                    }

                    if (recForInactive.Count > 0)
                    {
                        for (int i = 0; i < recForInactive.Count; i++)
                        {
                            if (!clsObj.fnActiveOffers(recForInactive[i].ToString()))
                            {
                                return;
                            }
                        }
                    }

                    fnLoadOffers();
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

     
       private void pgCampaign_Paint(object sender, PaintEventArgs e)
       {

       }

   

       

     

           
    }
}
