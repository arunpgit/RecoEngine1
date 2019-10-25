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
        string strMainFilter = "";
        AlertForm alert;
        void fnShowOpportunitiesDetails()
        {
            try
            {
                DataTable dt = new DataTable();
               
                dt = clstreDetails.fnGetOpportunityDetails(Common.iProjectID);
                grdOpportunities.DataSource = dt;
                grdOpportunities.MasterTemplate.Refresh(null); 
                grdOpportunities.AllowAddNewRow = false;
                grdOpportunities.ShowRowHeaderColumn = false;
                grdOpportunities.EnableGrouping = false;
                grdOpportunities.ShowFilteringRow = false;
                grdOpportunities.AutoSizeRows = false;
                grdOpportunities.AllowEditRow = false;
                grdOpportunities.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                grdOpportunities.BestFitColumns();

                for (int i = 1; i < dt.Columns.Count - 1; i++)
                {
                    grdOpportunities.MasterTemplate.Columns[i].Width = 100;
                }

                grdOpportunities.AutoScroll = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult ds = RadMessageBox.Show(this, "This will apply the rules on the entire dataset, are you sure you want to continue", "Confirmation", MessageBoxButtons.YesNo, RadMessageIcon.Info);
                if (ds.ToString() == "Yes")
                {

                    if (objRanking.fnRankingcriteria(Common.iProjectID).Rows.Count > 0)
                    {
                        alert = new AlertForm();
                        alert.SetMessage("Loading data. Please wait..."); // "Loading data. Please wait..."
                        alert.TopMost = true;
                        alert.StartPosition = FormStartPosition.CenterScreen;
                        alert.Show();
                        alert.Refresh();
                        fnCreateView();
                        string strT1String = clstreDetails.fnBuildTimePeriod(Common.timePeriods.strtp1);
                        string strT2String = clstreDetails.fnBuildTimePeriod(Common.timePeriods.strtp2);
                       // clstreDetails.fnDeleteTreOppfrmExport();
                        //if (ClsObj.fnRunOPoortunities(Common.iProjectID, Common.strTableName, strT1String, strT2String, strMainFilter))
                        //{
                        //    objRanking.fnMainRankingfrmExport(Common.iProjectID);
                        //    fnShowOpportunitiesDetails();

                        //    clstreDetails.fnDropTableTab(Common.strTableName);
                        //}

                        if (ClsObj.fnRunOPoortunitiesfrmProcedure(Common.iProjectID, Common.strTableName, strT1String, strT2String))
                        {
                            objRanking.fnMainRankingfrmExport(Common.iProjectID);
                            fnShowOpportunitiesDetails();
                            //clstreDetails.fnDropTableTab(Common.strTableName);
                        }

                        alert.Close();
                    }
                    else
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "Select the  Ranking Criteria ,Inorder to run Opportunities ", "Information", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                alert.Close();
                MessageBox.Show(ex.Message);
            }       
        }
        void fnCreateView()
        {
            string strColumns = "";
            clsDataSource objDatsource = new clsDataSource();
             strMainFilter = objDatsource.fnselectFilterCondition(Common.iProjectID);
             
            DataTable dtcol = objDatsource.fnGetTreDetailsSchema(Common.strTableName);
            foreach (DataRow dr in dtcol.Rows)
            {
                strColumns += dr[0].ToString();
                strColumns += ",";
            }
            dtcol = objDatsource.fnGetCalaculatedColMappingData(Common.iProjectID,Common.strTableName);
             foreach (DataRow dr in dtcol.Rows)
            {
                strColumns += dr["COMBINE_COLUMNS"].ToString() + " " + dr["COLNAME"].ToString();
                strColumns += ",";
            }
            if (strColumns.Length > 0)
                strColumns = strColumns.Remove(strColumns.Length - 1, 1);
            clstreDetails.fnCreateTableTab(Common.strTableName, strColumns, strMainFilter);
        }
    }
}
