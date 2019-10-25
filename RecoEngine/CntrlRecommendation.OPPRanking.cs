
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


namespace RecoEngine
{
    partial class CntrlRecommendation
    {
        DataTable dt = new DataTable();
        bool bDonotFireSelectionChange = false;
        int iCurrentRankID = 0;
        void fnBindRanking()
        {
            try
            {

               // DataTable dt = ClsObj.fnGetOppRanking(Common.iProjectID);
                if(ddRankingCriteria.DataSource ==null)
                ddRankingCriteria.DataSource = Enum.GetValues(typeof(RecoEngine_BI.Enums.Rank_Criteria));
                bIsRankingLoaded = true;
                if (!bIsRankingLoaded)
                    fnShowOpportunityRanking();
                bIsRankingLoaded = true;
               // fnShowOpportunityRanking();
                //if (dt.Rows.Count != 0)
                //{
                //    dt.Columns.Add(new DataColumn("Select", typeof(bool)));
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        dt.Rows[i]["Select"] = false;
                //    }
                //    grdRanking.DataSource = dt;
                //    grdRanking.AllowAddNewRow = false;
                //    grdRanking.ShowRowHeaderColumn = false;
                //    grdRanking.EnableGrouping = false;
                //    grdRanking.ShowFilteringRow = false;
                //   // grdRanking.MasterTemplate.AutoSizeRows = false;
                //    grdRanking.Columns["ID"].IsVisible = false;
                //    for (int i = 0; i < dt.Columns.Count - 1; i++)
                //    {
                //        grdRanking.MasterTemplate.Columns[i].ReadOnly = true;
                //    }
                //    grdRanking.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                 
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void fnShowOpportunityRanking()
        {
            try
            {

                clsOpportunities clsObj = new clsOpportunities();
                bool bIsOpplist = false;
                dt = clsObj.fnGetOpportunity(Common.iProjectID,Common.iUserID, bIsOpplist);
               
                ddlOppRank4.Enabled = false;
                ddlOppRank3.Enabled = false;
                ddlOppRank2.Enabled = false;
                
                bDonotFireSelectionChange = true;
                ddlOppRank1.DataSource = dt;
                ddlOppRank1.SelectedValue = "";
                ddlOppRank1.SelectedText = "";
                ddlOppRank1.ValueMember = "OPPORTUNITY_ID";
                ddlOppRank1.DisplayMember = "OPP_NAME";


                ddlOppRank2.BindingContext = new BindingContext();
                ddlOppRank2.DataSource = dt;
                ddlOppRank2.SelectedValue = "";
                ddlOppRank2.SelectedText = "";
                ddlOppRank2.ValueMember = "OPPORTUNITY_ID";
                ddlOppRank2.DisplayMember = "OPP_NAME";


                ddlOppRank3.BindingContext = new BindingContext();
                ddlOppRank3.DataSource = dt;
                ddlOppRank3.SelectedValue = "";
                ddlOppRank3.SelectedText = "";
                ddlOppRank3.ValueMember = "OPPORTUNITY_ID";
                ddlOppRank3.DisplayMember = "OPP_NAME";


                ddlOppRank4.BindingContext = new BindingContext();
                ddlOppRank4.DataSource = dt;
                ddlOppRank4.SelectedValue = "";
                ddlOppRank4.SelectedText = "";
                ddlOppRank4.ValueMember = "OPPORTUNITY_ID";
                ddlOppRank4.DisplayMember = "OPP_NAME";

                bDonotFireSelectionChange = false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ddlOppRank1_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {



            if (bIsRankingLoaded && ddlOppRank1.SelectedIndex != -1 && ddlOppRank1.SelectedValue.ToString() != "")
            {

                if (dt.Rows.Count > 1)
                    ddlOppRank2.Enabled = true;


                if (ddlOppRank2.SelectedValue != null && int.Parse(ddlOppRank2.SelectedValue.ToString()) > 0)
                {
                    if (ddlOppRank2.SelectedValue.ToString() == ddlOppRank1.SelectedValue.ToString())
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "This Opportunity hase been selected for another rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        ddlOppRank1.SelectedIndex = -1;
                        ddlOppRank1.Focus();
                        return;
                    }

                }
                if (ddlOppRank3.SelectedValue != null && int.Parse(ddlOppRank3.SelectedValue.ToString()) > 0)
                {
                    if (ddlOppRank3.SelectedValue.ToString() == ddlOppRank1.SelectedValue.ToString())
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "This Opportunity hase been selected for another rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        ddlOppRank1.SelectedIndex = -1;
                        ddlOppRank1.Focus();
                        return;
                    }

                }
                if (ddlOppRank4.SelectedValue != null && int.Parse(ddlOppRank4.SelectedValue.ToString()) > 0)
                {
                    if (ddlOppRank4.SelectedValue.ToString() == ddlOppRank1.SelectedValue.ToString())
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "This Opportunity hase been selected for another rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        ddlOppRank1.SelectedIndex = -1;
                        ddlOppRank1.Focus();
                        return;
                    }

                }
            }

        }
        private void ddlOppRank2_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

            //ddlOppRank3.Enabled = false;
            //ddlOppRank4.Enabled = false;
            if (ddlOppRank2.SelectedIndex != -1)
            {
                if (dt.Rows.Count > 2)
                    ddlOppRank3.Enabled = true;

                if (ddlOppRank1.SelectedIndex != -1 && ddlOppRank1.SelectedValue != null && int.Parse(ddlOppRank1.SelectedValue.ToString()) > 0)
                {
                    if (ddlOppRank2.SelectedValue.ToString() == ddlOppRank1.SelectedValue.ToString())
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "This Opportunity hase been selected for another rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        ddlOppRank2.SelectedIndex = -1;
                        ddlOppRank2.Focus();
                        return;
                    }

                }

                if (ddlOppRank3.SelectedIndex != -1 && ddlOppRank3.SelectedValue != null && int.Parse(ddlOppRank3.SelectedValue.ToString()) > 0)
                {
                    if (ddlOppRank3.SelectedValue.ToString() == ddlOppRank2.SelectedValue.ToString())
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "This Opportunity hase been selected for another rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        ddlOppRank2.SelectedIndex = -1;
                        ddlOppRank2.Focus();
                        return;
                    }
                }
                if (ddlOppRank4.SelectedIndex != -1 && ddlOppRank4.SelectedValue != null && int.Parse(ddlOppRank4.SelectedValue.ToString()) > 0)
                {
                    if (ddlOppRank4.SelectedValue.ToString() == ddlOppRank2.SelectedValue.ToString())
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "This Opportunity hase been selected for another rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        ddlOppRank2.SelectedIndex = -1;
                        ddlOppRank2.Focus();
                        return;
                    }

                }
            }


        }
        private void ddlOppRank3_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

            //ddlOppRank4.Enabled = false;

            if (ddlOppRank3.SelectedIndex != -1)
            {
                if (dt.Rows.Count > 3)
                    ddlOppRank4.Enabled = true;
                if (ddlOppRank1.SelectedIndex != -1 && ddlOppRank1.SelectedValue != null && int.Parse(ddlOppRank1.SelectedValue.ToString()) > 0)
                {
                    if (ddlOppRank3.SelectedValue.ToString() == ddlOppRank1.SelectedValue.ToString())
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "This Opportunity hase been selected for another rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        ddlOppRank3.SelectedIndex = -1;
                        ddlOppRank3.Focus();
                        return;
                    }

                }



                if (ddlOppRank2.SelectedIndex != -1 && ddlOppRank2.SelectedValue != null && int.Parse(ddlOppRank2.SelectedValue.ToString()) > 0)
                {
                    if (ddlOppRank2.SelectedValue.ToString() == ddlOppRank3.SelectedValue.ToString())
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "This Opportunity hase been selected for another rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        ddlOppRank3.SelectedIndex = -1;
                        ddlOppRank3.Focus();
                        return;
                    }

                }

                if (ddlOppRank4.SelectedIndex != -1 && ddlOppRank4.SelectedValue != null && int.Parse(ddlOppRank4.SelectedValue.ToString()) > 0)
                {
                    if (ddlOppRank4.SelectedValue.ToString() == ddlOppRank3.SelectedValue.ToString())
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "This Opportunity hase been selected for another rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        ddlOppRank3.SelectedIndex = -1;
                        ddlOppRank3.Focus();
                        return;
                    }

                }


            }
        }
        private void ddlOppRank4_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

            if (ddlOppRank4.SelectedIndex != -1)
            {
                bDonotFireSelectionChange = true;
                if (ddlOppRank1.SelectedIndex != -1 && ddlOppRank1.SelectedValue != null && int.Parse(ddlOppRank1.SelectedValue.ToString()) > 0)
                {
                    if (ddlOppRank4.SelectedValue.ToString() == ddlOppRank1.SelectedValue.ToString())
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "This Opportunity hase been selected for another rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        ddlOppRank4.SelectedIndex = -1;
                        ddlOppRank4.Focus();
                        return;
                    }

                }

                if (ddlOppRank2.SelectedIndex != -1 && ddlOppRank2.SelectedValue != null && int.Parse(ddlOppRank2.SelectedValue.ToString()) > 0)
                {
                    if (ddlOppRank2.SelectedValue.ToString() == ddlOppRank4.SelectedValue.ToString())
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "This Opportunity hase been selected for another rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        ddlOppRank4.SelectedIndex = -1;
                        ddlOppRank4.Focus();
                        return;
                    }
                }


                if (ddlOppRank3.SelectedIndex != -1 && ddlOppRank3.SelectedValue != null && int.Parse(ddlOppRank3.SelectedValue.ToString()) > 0)
                {
                    if (ddlOppRank3.SelectedValue.ToString() == ddlOppRank4.SelectedValue.ToString())
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "This Opportunity hase been selected for another rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        ddlOppRank4.SelectedIndex = -1;
                        ddlOppRank4.Focus();
                        return;
                    }
                }


            }
        }
        private void ddRankingCriteria_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (ddRankingCriteria.SelectedItem.ToString() == RecoEngine_BI.Enums.Rank_Criteria.Potential.ToString())
            {
                ddlOppRank1.Enabled = false;
                ddlOppRank2.Enabled = false;
                ddlOppRank3.Enabled = false;
                ddlOppRank4.Enabled = false;
            }
            else
            {
                ddlOppRank1.Enabled = true;
                ddlOppRank2.Enabled = true;
                ddlOppRank3.Enabled = true;
                ddlOppRank4.Enabled = true;
                fnShowOpportunityRanking();
            
            }
        }
        private void btnRnkngSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (objRanking.fnCheckOpportunityExists(Common.iProjectID))
                {

                    if (ddRankingCriteria.SelectedItem.ToString() == RecoEngine_BI.Enums.Rank_Criteria.Custom.ToString())
                    {

                        if ((ddlOppRank1.SelectedIndex == -1) && (ddlOppRank2.SelectedIndex == -1) && (ddlOppRank3.SelectedIndex == -1) && (ddlOppRank4.SelectedIndex == -1))
                        {
                            Telerik.WinControls.RadMessageBox.Show(this, "Please select the opportunity.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                            ddlOppRank1.Focus();
                            return;
                        }
                        else
                        {
                            string strRank2 = "";
                            string strRank3 = "";
                            string strRank4 = "";
                            if (ddlOppRank1.SelectedValue == null)
                            {
                                Telerik.WinControls.RadMessageBox.Show(this, "Please select opportunity 1.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                                ddlOppRank1.Focus();
                                return;
                            }


                            if (ddlOppRank2.SelectedIndex != -1)
                            {
                                strRank2 = ddlOppRank2.SelectedItem.ToString();
                                if (strRank2 == ddlOppRank1.SelectedValue.ToString())
                                {
                                    Telerik.WinControls.RadMessageBox.Show(this, "An Opportunity is Selected for More then One Rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                                    ddlOppRank2.Focus();
                                    return;
                                }
                            }
                            else if (ddlOppRank2.SelectedIndex == -1 && ddlOppRank2.Enabled == true)
                            {
                                Telerik.WinControls.RadMessageBox.Show(this, "Please Select the Opportunity for Rank 2.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                                ddlOppRank4.Focus();
                                return;
                            }

                            if (ddlOppRank3.SelectedIndex != -1)
                            {
                                strRank3 = ddlOppRank3.SelectedItem.ToString();
                                if (strRank3 == strRank2 || strRank3 == ddlOppRank1.SelectedValue.ToString())
                                {
                                    Telerik.WinControls.RadMessageBox.Show(this, "An Opportunity is Selected for More then One Rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                                    ddlOppRank3.Focus();
                                    return;
                                }

                            }
                            else if (ddlOppRank3.SelectedIndex == -1 && ddlOppRank3.Enabled == true)
                            {
                                Telerik.WinControls.RadMessageBox.Show(this, "Please Select the Opportunity for Rank 3.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                                ddlOppRank4.Focus();
                                return;
                            }


                            if (ddlOppRank4.SelectedIndex != -1)
                            {
                                strRank4 = ddlOppRank4.SelectedItem.ToString();
                                if (strRank4 == strRank3 || strRank4 == strRank2 || strRank4 == ddlOppRank1.SelectedValue.ToString())
                                {
                                    Telerik.WinControls.RadMessageBox.Show(this, "An Opportunity is Selected for More then One Rank.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                                    ddlOppRank4.Focus();
                                    return;
                                }

                            }

                            else if (ddlOppRank4.SelectedIndex == -1 && ddlOppRank4.Enabled == true)
                            {
                                Telerik.WinControls.RadMessageBox.Show(this, "Please Select the Opportunity for Rank 4.", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                                ddlOppRank4.Focus();
                                return;
                            }


                            ClsObj.fnSaveOPPRanking("Custom", Common.iProjectID, ddlOppRank1.SelectedItem.ToString(), strRank2, strRank3, strRank4);
                            objRanking.fnCustomRanking(Common.iProjectID, ddlOppRank1.SelectedItem.ToString(), strRank2, strRank3, strRank4);
                            fnBindRanking();

                        }
                    }
                    else
                    {
                        string strRank1 = "NULL";
                        string strRank2 = "NULL";
                        string strRank3 = "NULL";
                        string strRank4 = "NULL";

                        ClsObj.fnSaveOPPRanking("Potential", Common.iProjectID, strRank1, strRank2, strRank3, strRank4);
                        objRanking.fnPotentialRanking(Common.iProjectID);
                        fnBindRanking();
                    }
                }
                else 
                {
                    Telerik.WinControls.RadMessageBox.Show(this, "There are no Opportunities to do ranking ", "Opportunity Ranking", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                            
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
             
        }
         //private void grdRanking_SelectionChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (bIsRankingLoaded && grdRanking.CurrentRow != null)
        //        {
        //            bDonotFireSelectionChange = true;
        //            GridViewRowInfo grdRow = grdRanking.CurrentRow;
        //            //iCurrentRankID = int.Parse(grdRow.Cells["ID"].Value.ToString());
      
        //            if (grdRow.Cells["RANK1"].Value != null)
        //            {
        //                ddlOppRank1.SelectedValue = grdRow.Cells["RANK1"].Value;
        //            }
        //            else
        //                ddlOppRank1.SelectedIndex = -1;

        //            if (grdRow.Cells["RANK2"].Value != null)
        //            {
        //                ddlOppRank2.SelectedValue = grdRow.Cells["RANK2"].Value;
        //            }
        //            else
        //                ddlOppRank2.SelectedIndex = -1;
        //            if (grdRow.Cells["RANK3"].Value != null)
        //            {
        //                ddlOppRank3.SelectedValue = grdRow.Cells["RANK3"].Value;
        //            }
        //            else
        //                ddlOppRank3.SelectedIndex = -1;
        //            if (grdRow.Cells["RANK4"].Value != null)
        //            {
        //                ddlOppRank4.SelectedValue = grdRow.Cells["RANK4"].Value;
        //            }
        //            else
        //                ddlOppRank4.SelectedIndex = -1;

        //            ddRankingCriteria.SelectedIndex=Convert.ToInt16(grdRow.Cells["TYPE"].Value)-1;
        //        // int i=   (int)(RecoEngine_BI.Enums.Rank_Criteria.Potential);
        //            btnDelete.Visible = true;
        //            btnRnkngSave.Text = "Update";
        //            bDonotFireSelectionChange = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.Message);
        //        Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
        //    }
        //}

        //private void btnRankingDelete_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataTable dt = ((DataTable)grdRanking.DataSource);
        //        DataRow[] drRow = dt.Select("Select=1");
        //        if (drRow.Length == 0)
        //        {
        //            Telerik.WinControls.RadMessageBox.Show(this, "Select at least one.", "Information", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
        //            return;
        //        }
        //        else
        //        {

        //            DialogResult ds = Telerik.WinControls.RadMessageBox.Show(this, "Do you wish to delete selected record(s)?", "Confirmation", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
        //            if (ds != DialogResult.Yes)
        //            {
        //                return;
        //            }
        //            ArrayList recForDelete = new ArrayList();
        //            string strId = "";
        //            for (int i = 0; i < drRow.Length; i++)
        //            {
        //                strId = drRow[i]["ID"].ToString();

        //                recForDelete.Add(new ValueItemPair(strId, ""));
        //            }

        //            if (recForDelete.Count > 0)
        //            {
        //                string curRecId = "";

        //                for (int i = 0; i < recForDelete.Count; i++)
        //                {

        //                    curRecId = ((ValueItemPair)recForDelete[i]).Value.ToString();
        //                    if (!ClsObj.fnDeleteRankingOpportunity(curRecId))
        //                    {
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        // dt.Select("User_ID=" + curRecId.ToString())[0].Delete();
        //                    }
        //                }
        //            }
        //            fnBindRanking();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
        //    }
        //}
        
        //private void btnNewRanking_Click(object sender, EventArgs e)
        //{
           
        //    ddlOppRank1.SelectedIndex = -1;
        //    ddlOppRank2.SelectedIndex = -1;
        //    ddlOppRank3.SelectedIndex = -1;
        //    ddlOppRank4.SelectedIndex = -1;
        //    btnRnkngSave.Text = "Save";
        //}
    }
}
