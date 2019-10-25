using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecoEngine_BI;
using Telerik.WinControls.UI;
using Telerik.Charting;

namespace RecoEngine
{
    public partial class CntrlReports : UserControl
    {
        clsReports objReports = new clsReports();
      //  Label Opportunitylbl = new Label();
        public CntrlReports()
        {
            InitializeComponent();
         
            this.Load += new EventHandler(CntrlReports_Load);
       
        }
        void CntrlReports_Load(object sender, EventArgs e)
        {
            
            DisplayOppReports();
           

        }
        void DisplayOppReports()
        {
            try
            {
               
                DataSet ds = objReports.fnGetReports(Common.iProjectID);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            TableLayoutPanel pnlChart = new TableLayoutPanel();
                           
                           pnlChart.AutoSize = true;
                            pnlChart.ColumnCount = 2;
                            pnlChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50));
                            pnlChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50));
                            //  pnlChart.ColumnStyles.Clear();
                            pnlChart.RowCount = 4;
                            LinearAxis verticalAxis1 = new LinearAxis();
                            verticalAxis1.AxisType = AxisType.Second;
                            LinearAxis verticalAxis2 = new LinearAxis();
                            verticalAxis2.AxisType = AxisType.Second;
                            verticalAxis2.HorizontalLocation = AxisHorizontalLocation.Right;
                            BarSeries AccountsBar = new BarSeries();
                            
                            AccountsBar.BackColor = new Color();
                            AccountsBar.BackColor = Color.Green;
                            AccountsBar.VerticalAxis = verticalAxis1;
                            AccountsBar.ShowLabels = true;
                            LineSeries AvgDeltaLine = new LineSeries();
                            AvgDeltaLine.BorderColor = Color.Red;
                            AvgDeltaLine.ShowLabels = true;
                            AvgDeltaLine.Spline = true;
                            AvgDeltaLine.VerticalAxis = verticalAxis2;
                            BarSeries AccountsCountBar = new BarSeries();
                            AccountsCountBar.BackColor = Color.Green;
                            
                            
                            AccountsCountBar.ShowLabels = true;
                            BarSeries AvgPotentialBar = new BarSeries();
                            AvgPotentialBar.BackColor = Color.Brown;
                            AvgPotentialBar.ShowLabels = true;
                            foreach (DataRow dr in dt.Rows)
                            {
                                AccountsBar.DataPoints.Add(new CategoricalDataPoint(dr["AccountsPercentage"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["AccountsPercentage"]), dr["OppStatus"]));
                                AvgDeltaLine.DataPoints.Add(new CategoricalDataPoint(dr["AverageDelta"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["AverageDelta"]), dr["OppStatus"]));
                                AccountsCountBar.DataPoints.Add(new CategoricalDataPoint(dr["Accounts"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["Accounts"]), dr["OppStatus"]));
                                AvgPotentialBar.DataPoints.Add(new CategoricalDataPoint(dr["AvgAccountPotential"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["AvgAccountPotential"]), dr["OppStatus"]));
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                RadChartView chrtReports = new RadChartView();
                                chrtReports.ElementTree.EnableTheming = false;
                                chrtReports.ThemeName = "ControlDefault";
                               chrtReports.ShowToolTip = true;
                                if (i == 0)
                                {
                                   pnlChart.BackColor = Color.White;

                                   chrtReports.BackColor = Color.White; ;
                                    chrtReports.Series.Add(AccountsBar);
                                    chrtReports.Series.Add(AvgDeltaLine);
                                    chrtReports.Dock = DockStyle.Fill;
                                    pnlChart.Height = 400;
                                    TableLayoutPanel legendtbl = new TableLayoutPanel();
                                    legendtbl.ColumnCount = 2;
                                    legendtbl.RowCount = 1;
                                    legendtbl.Controls.Add(Legendisplay(AccountsBar.BackColor.Name.ToString(), "%Accounts", 0), 0, 0);
                                    legendtbl.Controls.Add(Legendisplay(AvgDeltaLine.BorderColor.Name.ToString(), "AvgDelta%", 1), 1, 0);
                                    legendtbl.Dock = DockStyle.Top;
                                    pnlChart.Controls.Add(ChartTitle(dt.Rows[0]["OppName"].ToString() + "-Opportunity"), 0, 0);
                                    pnlChart.Controls.Add(chrtReports, 0, 1);
                                    pnlChart.Controls.Add(legendtbl, 0, 2);
                                }
                                else
                                {
                                    chrtReports.Series.Add(AccountsCountBar);
                                    chrtReports.Series.Add(AvgPotentialBar);
                                    TableLayoutPanel legendtbl = new TableLayoutPanel();
                                    legendtbl.ColumnCount = 2;
                                    legendtbl.RowCount = 1;
                                    legendtbl.Controls.Add(Legendisplay(AccountsCountBar.BackColor.Name.ToString(), "Accounts", 0), 0, 0);
                                    legendtbl.Controls.Add(Legendisplay(AvgPotentialBar.BackColor.Name.ToString(), "AvgPotential Per Account", 1), 1, 0);
                                    legendtbl.Dock = DockStyle.Top;
                                    chrtReports.Dock = DockStyle.Fill;
                                    pnlChart.Controls.Add(ChartTitle(dt.Rows[0]["OppName"].ToString() + "-Potential"), 1, 0);
                                    pnlChart.Controls.Add(chrtReports, 1, 1);
                                    pnlChart.Controls.Add(legendtbl, 1, 2);
                                 }
                              }
                            pnlChart.Dock = DockStyle.Top;
                          
                            pnlMain.Controls.Add(pnlChart);
                            Label Opportunitylbl = new Label();
                            Opportunitylbl.Height = 40;
                            Opportunitylbl.Text = dt.Rows[0]["OppName"].ToString();
                            Opportunitylbl.Font = new Font(Opportunitylbl.Font.FontFamily, 10);
                            Opportunitylbl.Dock = DockStyle.Top;
                            Opportunitylbl.Padding = new Padding(0, 10, 0, 0);
                            Opportunitylbl.BorderStyle = BorderStyle.FixedSingle;
                            pnlMain.Controls.Add(Opportunitylbl);
                        }
                     }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

      
        Panel Legendisplay(string color,string lgndName,int i)
        {
            Label chkbx = new Label();
            Panel lgndPanel = new Panel();
            chkbx.Width = 15;
            chkbx.Height = 15;
            chkbx.BackColor = Color.FromName(color);
            chkbx.IsAccessible = false;
            Label lgndText = new Label();
            lgndText.Text = "     "+lgndName;
            lgndPanel.Controls.Add(chkbx);
            lgndPanel.Controls.Add(lgndText);
            if (i == 0)
            {
                lgndPanel.Margin = new Padding(120, 0, 0, 0);
                return lgndPanel;
            }
            else
            {
                lgndPanel.Margin = new Padding(0, 0, 0, 40);
                lgndPanel.Dock = DockStyle.Left;
                return lgndPanel;
            }
        }
        Label ChartTitle(string title)
        {
            Label lblChartTitle = new Label();
            lblChartTitle.Height = 30;
            lblChartTitle.Dock = DockStyle.Top;
            lblChartTitle.Text =  title;
            lblChartTitle.Font = new Font(lblChartTitle.Font.FontFamily, 10);
            lblChartTitle.Padding = new Padding(40, 10, 0, 0);
            return lblChartTitle;
        }
        void DisplayCampaignReports()
        {
            try
            {
                cbRank.SelectedIndex = 0;
                DataTable dt = objReports.fnGetRankingReports(Common.iProjectID, cbRank.SelectedIndex + 1);
                if (dt.Rows.Count > 0)
                {
                    var sumAccounts = Convert.ToInt32(dt.Compute("Sum(Accounts)", "")) - Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Accounts"]);
                    var sumpotential = dt.Compute("Sum(Rank1Potential)", "");
                    lblAccounts.Text = sumAccounts.ToString();
                    lblPotential.Text = sumpotential.ToString();
                    RankingGrid.DataSource = dt;
                }
                else 
                {
                    Telerik.WinControls.RadMessageBox.Show("No data available in Ranking table");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void reportsPage_SelectedPageChanged(object sender, EventArgs e)
        {
            try
            {
                 switch (reportsPage.SelectedPage.Tag.ToString().ToLower())
                {
                    case "reportsopportuntiy":
                     DisplayOppReports();
                        break;
                    case "reportscampaigns":
                        DisplayCampaignReports();
                        break;
                  

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = objReports.fnGetRankingReports(Common.iProjectID, cbRank.SelectedIndex + 1);
                //var sumAccounts = dt.Compute("Sum(Accounts)", "");
                if (dt.Rows.Count > 0)
                {
                    var sumAccounts = Convert.ToInt32(dt.Compute("Sum(Accounts)", "")) - Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Accounts"]);
                    int rank = cbRank.SelectedIndex + 1;
                    var sumpotential = dt.Compute("Sum(Rank" + rank + "Potential)", "");
                    lblAccounts.Text = sumAccounts.ToString();
                    lblPotential.Text = sumpotential.ToString();
                    lblAccountsprecent.Text = dt.Rows[dt.Rows.Count - 1][3].ToString();
                    RankingGrid.DataSource = null;
                    RankingGrid.DataSource = dt;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }

    }
}
