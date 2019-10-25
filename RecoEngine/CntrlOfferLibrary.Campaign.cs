using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using RecoEngine_BI;
using Telerik.WinControls;
using System.Collections;
using Telerik.WinControls.UI;

namespace RecoEngine
{
    partial class CntrlOfferLibrary 
    {
        clsDataSource clsDSOBJ = new clsDataSource();
        clsCampaign clsObjcampaign = new clsCampaign();
        int iCampaignID = 0;
        DataTable dtOpportunity = new DataTable();
        bool bIsCampaignLoaded = false;
        string strEligibility = "";
        string[] Status = { "Non-User", "Dropper", "Stopper", "Grower", "Flat", "New-User" };
       // string[] SegmentType = { "RECOMMEND", "REPLICATE", "REACTIVATE", "RETAIN", "SATISFY", "X-SELL", "MITIGATE", "REVIVE", "NO ACTION", "UP-SELL", "NO-ACTION" };     
        string[] SegmentType = { "X-SELL", "MITIGATE", "REVIVE", "NO ACTION", "UP-SELL","NO-ACTION"};     
        void fnShowCampaignList(bool bShow)
        {
            try
            {
                if (bShow)
                {
                    clsObjcampaign.fnDelteCampaignRankingsfrmExport(Common.iProjectID, Common.strTableName);
                    bIsShowOPPList = true;
                    gbCampaignlist.Visible = true;
                    gbNewcampaign.Visible = false;
                    gbNewcampaign.Dock = DockStyle.None;
                    gbCampaignlist.Dock = DockStyle.Fill;
                    fnLoadCampaigns();
                 }
                else
                {
                
                    bIsShowOPPList = false;
                    gbNewcampaign.Visible = true;
                    gbNewcampaign.Dock = DockStyle.Fill;
                    gbCampaignlist.Visible = false;
                    gbCampaignlist.Dock = DockStyle.None;
                }
                if (!bIsCampaignLoaded)
                    fnFilldds();
                    bIsCampaignLoaded = true;
                
            }
            catch (Exception ex)
            {

                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);

            }

        }
        void fnFilldds()
        {
            try
            {
                DataSet dsLevels = clsObjcampaign.fnGetSgmntOfferChnl(Common.iProjectID);
                dtOpportunity = dsLevels.Tables[0];
                ddlSegment.DataSource = dsLevels.Tables[0];
                
                ddlSegment.ValueMember = "Opportunity_id";
                ddlSegment.DisplayMember = "OPP_NAME";
                ddlSegment.SelectedIndex = 0;
                ddlOffer.DataSource = dsLevels.Tables[1];
                ddlOffer.ValueMember = "OFFER_ID";
                ddlOffer.DisplayMember = "CODE";
                ddlOffer.SelectedIndex = 0;
                ddOpt.DataSource = dsLevels.Tables[2];
                ddOpt.ValueMember = "CODE";
                ddOpt.DisplayMember = "DESCRIPTION";
                ddOpt.SelectedIndex = 0;
                ddlAction.DataSource = dsLevels.Tables[3];
                ddlAction.ValueMember = "CODE";
                ddlAction.DisplayMember = "DESCRIPTION";
                ddlAction.SelectedIndex = 0;
                ddlChannel.DataSource = dsLevels.Tables[4];
                ddlChannel.ValueMember = "CODE";
                ddlChannel.DisplayMember = "DESCRIPTION";
                ddlChannel.SelectedIndex = 0;
                ddPrioritise.DataSource = Enum.GetValues(typeof(RecoEngine_BI.Enums.Offer_Priority));
                //ddlSegmentType.DataSource = SegmentType;
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }

        }
        void fnLoadCampaigns()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {


                DataTable dt = clsObjcampaign.fnGetCampaignMapping(Common.iProjectID);
                dt.Columns.Add(new DataColumn("Select", typeof(bool)));
                dt.Columns["Select"].SetOrdinal(0);
                dt.Columns.Add(new DataColumn("Active", typeof(bool)));
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Select"] = false;
                    if (dt.Rows[i]["ISACTIVEID"].ToString() == "1")
                        dt.Rows[i]["Active"] = true;
                    else
                        dt.Rows[i]["Active"] = false;

                }
             //  dt.Columns["Average"].SetOrdinal(4);
                grdCampaign.DataSource = dt;
                grdCampaign.Columns["CAMPAIGN_ID"].IsVisible = false;
                grdCampaign.Columns["Average1"].IsVisible = false;
                grdCampaign.Columns["ISACTIVEID"].IsVisible = false;
                grdCampaign.Columns["Flag"].IsVisible = false;
                grdCampaign.Columns["Project_ID"].IsVisible = false;
                grdCampaign.Columns["OPPORTUNITY_ID"].IsVisible = false;
                grdCampaign.Columns["OFFER_ID"].IsVisible = false;
                grdCampaign.Columns["CHANNEL"].IsVisible = false;
                grdCampaign.Columns["ELIGIBILITY"].IsVisible = false;
                grdCampaign.Columns["SEGMENT_DESCRIPTION"].IsVisible = false;
                grdCampaign.Columns["ISACTIVEID"].IsVisible = false;
                grdCampaign.Columns["CREATEDDATE"].IsVisible = false;
                grdCampaign.Columns["DESCRIPTION"].Width = 200;
                for (int i = 1; i < dt.Columns.Count - 1; i++)
                {
                    if (dt.Columns[i].ColumnName != "Select")
                        grdCampaign.MasterTemplate.Columns[i].ReadOnly = true;
                }

            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void ddlSegment_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {
                if (!bIsCampaignLoaded)
                    return;
                                txtCampaignCode.Text = ddlSegment.SelectedItem.ToString() + "_" + ddlOffer.SelectedItem.ToString() + "_" + ddOpt.SelectedItem.ToString() + "_" + ddlAction.SelectedItem.ToString() + "_" + ddlChannel.SelectedItem.ToString();
                txtSegmentType.Text = dtOpportunity.Rows[ddlSegment.SelectedIndex]["OPP_ACTION"].ToString();
                if (txtSegmentType.Text == "STIMULATION")
                {
                    chkSegmentDrpdn.Visible = true;
                 }
                else
                    chkSegmentDrpdn.Visible = false;
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void btnNewCmpgn_Click(object sender, EventArgs e)
        {
            try
            {
            
                fnShowCampaignList(false);
                iOfferID = 0;
                txtCampaignName.Text = "";
                txtCampaignCode.Text = "";
                txtRate.Text = "";
                if (ddlOffer.SelectedItem != null && ddlSegment.SelectedItem != null && ddlChannel.SelectedItem != null)
                txtCampaignCode.Text = ddlSegment.SelectedItem.ToString() + "_" + ddlOffer.SelectedItem.ToString() + "_" + ddlChannel.SelectedItem.ToString();
                ddlSegment.SelectedIndex = 0;
                ddlOffer.SelectedIndex = 0;
                ddlChannel.SelectedIndex = 0;
                strEligibility = "";
                chkIsActive.Checked = true;
                txtCampaigndesc.Text = "";
                bndgcntrlchkDropDowntp1(chkSegmentDrpdn);
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
          
         
        }
        private void btnCInActive_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ((DataTable)grdCampaign.DataSource);
                DataRow[] drRow = dt.Select("Flag='Y'");
                if (drRow.Length == 0)
                {
                    Telerik.WinControls.RadMessageBox.Show(this, "Active/Inactive at least one Campaign.", "Information", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
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
                        strId = drRow[i]["CAMPAIGN_ID"].ToString();
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
                            if (!clsObjcampaign.fnActiveCampaigns(recForInactive[i].ToString()))
                            {
                                return;
                            }
                        }
                    }

                    fnLoadCampaigns();
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void grdCampaign_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (grdCampaign.MasterView.CurrentRow != null)
                {

                    iCampaignID = int.Parse(grdCampaign.MasterView.CurrentRow.Cells["CAMPAIGN_ID"].Value.ToString());
                    txtCampaignName.Text = grdCampaign.MasterView.CurrentRow.Cells["Name"].Value.ToString();
                    txtCampaigndesc.Text = grdCampaign.MasterView.CurrentRow.Cells["Description"].Value.ToString();
                    txtCampaignCode.Text = grdCampaign.MasterView.CurrentRow.Cells["CODE"].Value.ToString();
                    txtRate.Text = grdCampaign.MasterView.CurrentRow.Cells["TAKE_UP_RATE"].Value.ToString();
                    var k = (from p in ddlSegment.Items where p.Value.ToString() == grdCampaign.MasterView.CurrentRow.Cells["OPPORTUNITY_ID"].Value.ToString() select p.RowIndex).ToList()[0];
                       ddlSegment.SelectedIndex = Convert.ToInt16(k);
                       strEligibility = grdCampaign.MasterView.CurrentRow.Cells["ELIGIBILITY"].Value.ToString();
                       string Segmentvalues = grdCampaign.MasterView.CurrentRow.Cells["SEGMENT_DESCRIPTION"].Value.ToString();
                       if (grdCampaign.MasterView.CurrentRow.Cells["OPPORTUNITY_ID"].Value.ToString() == "STIMULATE")
                       {
                           List<int> SelectedItems = new List<int>();
                           
                           foreach (string str in Segmentvalues.Split(new string[] { "','" }, StringSplitOptions.None))
                           {
                               SelectedItems.Add(Array.IndexOf(SegmentType, str.Remove(str.IndexOf("-" + ddlSegment.SelectedItem.ToString()), ddlSegment.SelectedItem.ToString().Length + 1)));
                           }
                           bndgcntrlchkDropDowntp1(chkSegmentDrpdn, SelectedItems);
                       
                       }
                      if (grdCampaign.MasterView.CurrentRow.Cells["ISACTIVEID"].Value.ToString() == "1")
                        chkCIsActive.Checked = true;
                    else
                        chkCIsActive.Checked = false;
                    fnShowCampaignList(false);

                    //ClsObj
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        void bindingExpressionEditor(int iExpressionType)
        {
            try
            {
               // DataTable dt = clsDSOBJ.fnGetTreDetails("ETS_TRE_BASE3");
               string tblQuery;
               DataTable dt = clsObjcampaign.fnGetEffectiveTable(Common.iProjectID, out  tblQuery,Common.strTableName);
                DataTableReader dr = new DataTableReader(dt);
                DataTable dtSchema = dr.GetSchemaTable();
               // string strExpression = "";
                using (var frm = new frmExpressEditor(iExpressionType, "(" + tblQuery + ")", strEligibility))
                {
                    frm._fieldDict = Common.GetDict(dt);
                    frm.AvailableFields = frm._fieldDict.ToList<KeyValuePair<string, Type>>();
                    frm.dtSource = dtSchema;
                    var res = frm.ShowDialog();

                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        Common.strfiltertxt = frm.strExpression;

                    }
                }
            }
            catch (Exception ex)
            {

                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);

            }
        }
        private void btnFunction_Click(object sender, EventArgs e)
        {
            try
            {
                //strEligibility = "";

                bindingExpressionEditor((int)Enums.ExpressionType.Filter);
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }

        }
        private void btnCSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCampaignName.Text.Trim().Length == 0)
                {
                    Telerik.WinControls.RadMessageBox.Show("Campaign Name Should not be blank", "Information", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                    txtCampaignName.Focus();
                    return;
                }
            
               if( ddlOffer.SelectedItem==null ||  ddlSegment.SelectedItem==null )
                {
                    
                   Telerik.WinControls.RadMessageBox.Show("Opportunity and Offers must be there to create Campaign", "Information", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                      return;
                
                }
                int iIsActive = 0;
                if (chkCIsActive.Checked)
                    iIsActive = 1;
                //if(strEligibility=="")
                  strEligibility=Common.strfiltertxt;
                 string strOppRankstatus ="";

                 if (txtSegmentType.Text == "STIMULATION")
                 {

                     if (chkSegmentDrpdn.m_TextBox.Text!="")
                     {
                      foreach (string str in chkSegmentDrpdn.m_TextBox.Text.Split(new string[] { ";" },StringSplitOptions.None))
                         {
                             strOppRankstatus += SegmentType[Array.IndexOf(Status, str)] + "-" + ddlSegment.SelectedItem.ToString() + "','";
                         }


                         strOppRankstatus = strOppRankstatus.Trim().Substring(0, strOppRankstatus.Length - 3);
                     }

                     else
                     {
                         Telerik.WinControls.RadMessageBox.Show("Please Select Segment Type", "Information", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                         return;
                     }
                 }
                   else
                 {
                     strOppRankstatus =   txtSegmentType.Text + "-" + ddlSegment.SelectedItem.ToString()  ;
                     //strOppRankstatus += " OR RANK2_ACTION = '" + txtSegmentType.Text + "-" + ddlSegment.SelectedItem.ToString() + "'";
                     //strOppRankstatus += " OR RANK3_ACTION = '" + txtSegmentType.Text + "-" + ddlSegment.SelectedItem.ToString() + "'";
                     //strOppRankstatus += " OR RANK4_ACTION = '" + txtSegmentType.Text + "-" + ddlSegment.SelectedItem.ToString() + "')";
                 }
                 iCampaignID = clsObjcampaign.fnSaveCampaign(iCampaignID, txtCampaignName.Text, txtCampaignCode.Text, txtCampaigndesc.Text, ddlSegment.SelectedItem.ToString(), ddlOffer.SelectedItem.Value.ToString(), ddlSegment.SelectedItem.Value.ToString(), ddlChannel.SelectedItem.Value.ToString(), txtRate.Text, strEligibility, txtSegmentType.Text, strOppRankstatus, Common.iUserID, Common.iProjectID, iIsActive, Common.strTableName);
                   Telerik.WinControls.RadMessageBox.Show("Successfully saved.", "Information", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                Common.strfiltertxt = "";
                btnNewCmpgn_Click(null, null);       
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }

        }
        private void btnCpDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ((DataTable)grdCampaign.DataSource);
                DataRow[] drRow = dt.Select("Select=1");
                if (drRow.Length == 0)
                {
                    Telerik.WinControls.RadMessageBox.Show(this, "Select at least one Campaign.", "Information", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                {

                    DialogResult ds = Telerik.WinControls.RadMessageBox.Show(this, "Do you wish to delete selected Campaign(s)?", "Confirmation", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                    if (ds != DialogResult.Yes)
                    {
                        return;
                    }
                    ArrayList recForDelete = new ArrayList();
                    int iCmpId ;
                    for (int i = 0; i < drRow.Length; i++)
                    {
                          iCmpId = Convert.ToInt32(drRow[i]["CAMPAIGN_ID"]);
                          recForDelete.Add(iCmpId);
                    }
                        

                    if (recForDelete.Count > 0)
                    {
                        for (int i = 0; i < recForDelete.Count; i++)
                        {
                            if (!clsObjcampaign.fnDeleteCampaign(Convert.ToInt32(recForDelete[i]),Common.strTableName))
                            {
                                return;
                            }
                        }
                    }

                    fnLoadCampaigns();
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {

            try
            {
                iCampaignID = 0;
                fnShowCampaignList(true);
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        public string getPriortiseVal()
        {
            string strVal = "";

            strVal = ddPrioritise.SelectedItem.ToString();
            return strVal;
        }

        private void rbtnPrioritise_Click(object sender, EventArgs e)
        {
            try
            {
                clsExport objexport=new clsExport();
                DataTable table = objexport.fnGetCampaigns(Common.iProjectID);

                if (ddPrioritise.SelectedItem.ToString() == RecoEngine_BI.Enums.Offer_Priority.Avg_Ptnl.ToString())
                {
                    DataTable dt =clsObjcampaign.fnGetCampaignMapping(Common.iProjectID); 
                    //clsObjcampaign.fnPriorotiseCampaigns(dr, Common.strTableName);
                    foreach (DataRow dr in table.Rows)
                    {
                        clsObjcampaign.fnSaveCampaignRankings(dr["ELIGIBILITY"].ToString(), Convert.ToInt16(dr["PROJECT_ID"]), dr["SEGMENT_DESCRIPTION"].ToString(), Convert.ToInt16(dr["CAMPAIGN_ID"]), Common.strTableName);
                    }
                    clsObjcampaign.fnPrioritizeRankings(Common.iProjectID, "Average");
                 //   clsObjcampaign.fnPrioritizeRankings(Common.iProjectID, "Average");
                    grdCampaign.DataSource = null;
                    fnLoadCampaigns();
                }
                else
                {
                   
                   foreach (DataRow dr in table.Rows)
                   {
                       clsObjcampaign.fnSaveCampaignRankings(dr["ELIGIBILITY"].ToString(), Convert.ToInt16(dr["PROJECT_ID"]), dr["SEGMENT_DESCRIPTION"].ToString(), Convert.ToInt16(dr["CAMPAIGN_ID"]), Common.strTableName);
                   }
                   clsObjcampaign.fnPrioritizeRankings(Common.iProjectID, "Take_UP_RATE");
                   grdCampaign.DataSource = null;
                    fnLoadCampaigns();
                  }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
           
           
            //grdCampaign.Refresh();

//  Insert into PRIORITIZED_TEMP(Customer)
//SELECT A.Customer FROM 
//tre_opportunity  A 
//wHERE  A.Customer IN (SELECT CUSTOMER FROM TRE_DETAILS_NEW 
//WHERE HANDSET_TYPE='2G' 
//and WEEK= (Select Max(week) from TRE_DETAILS_NEW where year=to_char(sysdate, 'YYYY')))  
//and  A.ARPU_STATUS!='NA' and CUSTOMER NOT IN ( SELECT CUSTOMER FROM PRIORITIZED_TEMP)
       
        
        
        
        
        
        
        }
        private void grdCampaign_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (grdCampaign.CurrentRow != null)
                {
                    if (e.Column.Name.ToUpper() == "ACTIVE")
                    {
                        GridViewRowInfo drRow = grdCampaign.CurrentRow;
                        drRow.Cells["Flag"].Value = "Y";
                    }
                }
            }
            catch (Exception ex)
            {

                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);

            }
           
        }
        void bndgcntrlchkDropDowntp1(CntrlchkDropDown ctrl, List<int> SelectedItems = null)
        {
            try
            {
               DataTable dt =new DataTable();
                DataSet ds = new DataSet();
                string Selected = "Selected";
                dt.Columns.Add(new DataColumn("Selected", typeof(bool)));
                dt.Columns.Add(new DataColumn("Status", typeof(string)));
                dt.Columns.Add(new DataColumn("Empty", typeof(string)));
                dt.Columns.Add(new DataColumn("Empty2", typeof(string)));
                for (int i = 0; i < Status.Count(); i++)
                {
                    DataRow _newRow = dt.NewRow();
                    _newRow["Status"] = Status[i];
                    _newRow["Selected"] = false;
                    dt.Rows.Add(_newRow);
                }
                dt.Columns["Selected"].SetOrdinal(0);
                if (SelectedItems != null)
                {
                    foreach (int i in SelectedItems)
                    {
                        dt.Rows[i]["Selected"] = true;
                    }
                }
                //dt.Columns["timeperiod"].SetOrdinal(0);

                ds.Tables.Add(dt);
                ctrl.ConfigureDropDown(ds, Selected);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }

}