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

        void BindingTimeperiods()
        {
            bndgcntrlchkDropDowntp1(chkddlTP1, true);
            bndgcntrlchkDropDowntp1(cntrlchkDropDowntp2, false);
        }

        void bndgcntrlchkDropDowntp1(CntrlchkDropDown ctrl, bool bIsT1)
        {
            try
            {
                DataTable dt = clstreDetails.fnTREtimeperiods(Common.strTableName);
                DataSet ds = new DataSet();
                string Selected = "Selected";
                dt.Columns.Add(new DataColumn("Selected", typeof(bool)));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((bIsT1 && (i == 1 || i == 2 || i == 3)) || (!bIsT1 && (i == 0)))
                    {
                        dt.Rows[i]["Selected"] = true;
                    }
                    else
                    {
                        dt.Rows[i]["Selected"] = false;
                    }
                }
                dt.Columns["Selected"].SetOrdinal(0);
                //dt.Columns["timeperiod"].SetOrdinal(0);

                ds.Tables.Add(dt);
                ctrl.ConfigureDropDown(ds, Selected);
          
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkddlTP1.m_TextBox.Text.ToString() != cntrlchkDropDowntp2.m_TextBox.Text.ToString())
                {
                    if (chkddlTP1.m_TextBox.Text.ToString() != "" && cntrlchkDropDowntp2.m_TextBox.Text.ToString() != "")
                    {
                        fnBindMWData();
                    }
                }
                else
                {
                    MessageBox.Show("A time period can only be chosen once");
                }
            }
            catch (Exception ex)
            {

                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        void fnFillSegments()
        {
            try
            {
                DataTable dtTbl = new DataTable();

                dtTbl = clstreDetails.fnFillSegment(Common.strTableName);
                ddlSegment.DataSource = dtTbl;
                ddlSegment.ValueMember = "COLNAME";
                ddlSegment.DisplayMember = "COLNAME";

                ddlSegment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void fnShowGrouping()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (!bGroupingLoaded)
                {
                    if (!bMWindowLoaded)
                    {
                        Telerik.WinControls.RadMessageBox.Show(this, "Please select Time period.", "Information", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                        pgVRecommendation.SelectedPage = pgRcmndSettings;
                    }

                    fnFillSegments();


                    bGroupingLoaded = true;
                }
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
        public void fnShowTimePeriod()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (!bMWindowLoaded)
                {
                    BindingTimeperiods();
                    fnBindMWData();
                    btnView.Focus();
                    bMWindowLoaded = true;
                }
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

        void fnBindGData()
        {
            try
            {

                DataTable dt = new DataTable();
                dt = clstreDetails.fnGetSegmentData(Common.strTableName, Common.timePeriods.strtp1, Common.timePeriods.strtp2, ddlSegment.SelectedItem.Value.ToString(),Common.iProjectID, chkActive.Checked);
                strCurrentSegmentColumn = ddlSegment.SelectedItem.Value.ToString();
                iIsActive = chkActive.Checked ? 1 : 0;
                grdGrouping.DataSource = dt;
                grdGrouping.AllowAddNewRow = false;
                grdGrouping.ShowRowHeaderColumn = false;
                grdGrouping.EnableGrouping = false;
                grdGrouping.ShowFilteringRow = false;
                grdGrouping.AutoSizeRows = false;
                grdGrouping.AllowEditRow = false;
                grdGrouping.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                grdGrouping.MasterTemplate.BestFitColumns(); 
                grdGrouping.AutoScroll = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void fnBindMWData()
        {
            try
            {
          
                Common.timePeriods.strtp1 = chkddlTP1.m_TextBox.Text.ToString().Split(';').ToArray();
                Common.timePeriods.strtp2 = cntrlchkDropDowntp2.m_TextBox.Text.ToString().Split(';').ToArray();
                DataTable dt = new DataTable();
                dt = clstreDetails.fnTREtimePeriodData(Common.timePeriods.strtp1, Common.timePeriods.strtp2, Common.strTableName,Common.iProjectID);
                gridtpdetails.DataSource = dt;
                gridtpdetails.AllowAddNewRow = false;
                gridtpdetails.ShowRowHeaderColumn = false;
                gridtpdetails.EnableGrouping = false;
                gridtpdetails.ShowFilteringRow = false;
                gridtpdetails.AutoSizeRows = false;
                gridtpdetails.AllowEditRow = false;
                gridtpdetails.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                gridtpdetails.MasterTemplate.BestFitColumns(); 
                gridtpdetails.AutoScroll = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnGView_Click(object sender, EventArgs e)
        {
            try
            {
                if (Common.timePeriods.strtp2.Length > 0 && ddlSegment.SelectedItem.Value.ToString() != "")
                {
                    fnBindGData();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void rbtSegment_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbtSegment.Checked == true)
                {
                    chkActive.Visible = true;

                }
                else
                {
                    chkActive.Visible = false;
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

                pgVRecommendation.SelectedPage = pgOpportunityMapping;
                fnShowOpporunityList(true);
            }
            catch (Exception ex)
            {

                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            } 

        }

    }
}
