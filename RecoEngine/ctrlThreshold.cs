using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecoEngine_BI;
using System.Windows.Forms.DataVisualization.Charting;
using Telerik.WinControls;
using Telerik.WinControls.UI;
namespace RecoEngine
{
    public partial class ctrlThreshold : RadForm
    {
        clsTre_Details objTredetails = new clsTre_Details();
        VerticalLineAnnotation VAStopper = new VerticalLineAnnotation();
        RectangleAnnotation RAStopper = new RectangleAnnotation();
        VerticalLineAnnotation VADropper = new VerticalLineAnnotation();
        RectangleAnnotation RADropper = new RectangleAnnotation();
        VerticalLineAnnotation VAGrower = new VerticalLineAnnotation();
        RectangleAnnotation RAGrower = new RectangleAnnotation();
        DataTable tblchart = new DataTable();
        double Ymax = 0;

        int iOpportunityId = 0;
        string strFormula = "";
        string strPtnlFilter = "";
        System.Windows.Forms.DataVisualization.Charting.ChartArea CA;

        public string strCutOff = "";
        public string strCount = "";
        public string strAvgDelta = "";
        bool bIsTreSholdLoaded = false;
        string[] strT1 = Common.timePeriods.strtp1;
        string[] strT2 = Common.timePeriods.strtp2;

        public ctrlThreshold(int iOpportunityID, string strFormula, string strPtnlFilter,string[] strT1, string[] strT2)
        {
            this.strFormula = strFormula;
            this.strPtnlFilter = strPtnlFilter;
            this.strT1 = strT1;
            this.strT2 = strT2;
            iOpportunityId = iOpportunityID;
            InitializeComponent();
            this.Load += new EventHandler(ctrlThreshold_Load);
        }
        void ctrlThreshold_Load(object sender, EventArgs e)
        {
            try
            {
                fnMappingChart();
                fnStprAnnotation();
                fnDrprAnnotation();
                fnGrwrAnnotation();
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                bIsTreSholdLoaded = true;
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        void fnMappingChart()
        {
            try
            {
                lblTop.Text += Common.sOpportunityName;
                chartThreshold.ChartAreas.Clear();
                chartThreshold.Annotations.Clear();
                chartThreshold.Series.Clear();
                if (!tblchart.Columns.Contains("X"))
                    tblchart.Columns.Add("X", typeof(decimal));
                if (!tblchart.Columns.Contains("Y"))
                    tblchart.Columns.Add("Y", typeof(double));
                double Y = 0;
                DataTable dt = new DataTable();
               
                if (iOpportunityId > 0)
                {
                    dt = objTredetails.fnGetOppBreakDowndetails(iOpportunityId);
                }
                string strFlat = "";
                bool bIsNew = false;
                if (iOpportunityId > 0 && dt.Rows.Count > 0)
                {




                    if (dt.Rows[0]["T1"] != null && dt.Rows[0]["T1"].ToString() != "")
                    {
                        strT1 = dt.Rows[0]["T1"].ToString().Split(',');
                    }
                    if (dt.Rows[0]["T2"] != null && dt.Rows[0]["T2"].ToString() != "")
                    {
                        strT2 = dt.Rows[0]["T2"].ToString().Split(',');
                    }
                    if (dt.Rows[0]["DROPPERS_CUTOFF"] != null && dt.Rows[0]["DROPPERS_CUTOFF"].ToString() != "")
                    {
                        txtCtDropper.Text = dt.Rows[0]["DROPPERS_CUTOFF"].ToString();

                        strFlat = txtCtDropper.Text;
                    }
                    if (dt.Rows[0]["STOPPERS_CUTOFF"] != null && dt.Rows[0]["STOPPERS_CUTOFF"].ToString() != "")
                    {
                        txtCtStopper.Text = dt.Rows[0]["STOPPERS_CUTOFF"].ToString();

                    }
                    if (dt.Rows[0]["GROWERS_CUTOFF"] != null && dt.Rows[0]["GROWERS_CUTOFF"].ToString() != "")
                    {
                        txtCtGrower.Text = dt.Rows[0]["GROWERS_CUTOFF"].ToString();
                        strFlat += " - " + txtCtGrower.Text;
                    }
                    if (strFlat != "")
                        txtCtFlat.Text = strFlat;

                }
                else
                {

                    txtCtDropper.Text = Common.strDropper;
                    txtCtGrower.Text = Common.strGrower;
                    txtCtStopper.Text = Common.strStopper;
                    txtCtFlat.Text = Common.strDropper + " - " + Common.strGrower;
                    bIsNew = true;
                }
                txtCtNewUser.Text = "0";
                txtCtNonUser.Text = "0";
                VAStopper.X = Convert.ToDouble(txtCtStopper.Text);
                VADropper.X = Convert.ToDouble(txtCtDropper.Text);
                VAGrower.X = Convert.ToDouble(txtCtGrower.Text);
               // trkBarvalues();
                // fnAvgDeltaCount();
                

             //   dt = objTredetails.fnGetTREThreShold(strT1, strT2, strFormula,Common.strPtnlFilter, txtCtDropper.Text, txtCtGrower.Text, txtCtStopper.Text, iOpportunityId, Common.strTableName);


                dt = objTredetails.fnGetTREThreShold(strT1, strT2, strFormula, Common.strPtnlFilter, txtCtDropper.Text, txtCtGrower.Text, txtCtStopper.Text, iOpportunityId, "Tre_Random");

               fnAvgDeltaCount();

                var test = (from obj in dt.AsEnumerable()
                            select new
                            {
                                onnet = obj["DELTA"].ToString()
                            }).ToList();

                for (decimal i = -1; i <= 1; i += 0.1M)
                {

                    if (i == 1)
                    {

                        Y = (from p in test where Convert.ToDecimal(p.onnet) >= i select p.onnet).ToList().Count;
                        Y = (Convert.ToDouble(Y) / Convert.ToDouble(test.Count())) * 100;
                    }
                    else
                    {

                        Y = (from p in test where Convert.ToDecimal(p.onnet) >= i && Convert.ToDecimal(p.onnet) < i + Convert.ToDecimal(0.1) select p.onnet).ToList().Count;
                        Y = (Convert.ToDouble(Y) / Convert.ToDouble(test.Count())) * 100;
                    }
                    tblchart.Rows.Add(i, Y);

                }

                System.Windows.Forms.DataVisualization.Charting.ChartArea chartarea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
                chartThreshold.ChartAreas.Add(chartarea);
                Series series = new Series();
                series.ChartType = SeriesChartType.Spline;
                series.XValueMember = "X";
                series.YValueMembers = "Y";
                chartThreshold.Series.Add(series);
                chartThreshold.DataSource = tblchart;
                chartThreshold.ChartAreas[0].AxisX.Interval = 0.10;
                chartThreshold.DataBind();
                chartThreshold.Update();
                chartThreshold.ChartAreas[0].RecalculateAxesScale();
                Ymax = chartThreshold.ChartAreas[0].AxisY.Maximum;
                chartThreshold.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                chartThreshold.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void fnAvgDeltaCount()
        {
            try
            {
                DataTable dt = new DataTable();
                string[] strT1 = RecoEngine.Common.timePeriods.strtp1;
                string[] strT2 = RecoEngine.Common.timePeriods.strtp2;
                dt = objTredetails.fnGetTREThreShold(strT1, strT2, strFormula, Common.strPtnlFilter, txtCtDropper.Text, txtCtGrower.Text, txtCtStopper.Text, iOpportunityId, "Tre_Random");

                DataTable dtAVG = dt.Clone();
                DataRow[] drRows = dt.Select("STATUS ='STOPPER'");
                txtCStopper.Text = drRows.Length.ToString();
                if (drRows.Length > 0)
                {
                    dtAVG = drRows.CopyToDataTable();
                    txtAvgStopper.Text = Decimal.Round((decimal)dtAVG.Compute("AVG(DELTA)", ""), 2).ToString();
                }
                else
                {
                    txtAvgStopper.Text = "0";
                }
                drRows = dt.Select("STATUS ='DROPPER'");
                txtCDropper.Text = drRows.Length.ToString();
                if (drRows.Length > 0)
                {
                    dtAVG = drRows.CopyToDataTable();
                    txtAvgDropper.Text = Decimal.Round((decimal)dtAVG.Compute("AVG(DELTA)", ""), 2).ToString();
                }
                else
                {
                    txtAvgDropper.Text = "0";
                }
                drRows = dt.Select("STATUS ='GROWER'");
                txtCGrower.Text = drRows.Length.ToString();
                if (drRows.Length > 0)
                {
                    dtAVG = drRows.CopyToDataTable();
                    txtAvgGrower.Text = Decimal.Round((decimal)dtAVG.Compute("AVG(DELTA)", ""), 2).ToString();
                }
                else
                {
                    txtAvgGrower.Text = "0";
                }

                drRows = dt.Select("STATUS ='FLAT'");
                txtCFlat.Text = drRows.Length.ToString();
                if (drRows.Length > 0)
                {
                    dtAVG = drRows.CopyToDataTable();
                    txtAvgFlat.Text = Decimal.Round((decimal)dtAVG.Compute("AVG(DELTA)", ""), 2).ToString();
                }
                else
                {
                    txtAvgFlat.Text = "0";
                }
                drRows = dt.Select("STATUS ='NON_USER'");
                txtCNonUser.Text = drRows.Length.ToString();
                if (drRows.Length > 0)
                {
                    dtAVG = drRows.CopyToDataTable();
                    txtAvgNonUser.Text = Decimal.Round((decimal)dtAVG.Compute("AVG(DELTA)", ""), 2).ToString();
                }
                else
                {
                    txtAvgNonUser.Text = "0";
                }
                drRows = dt.Select("STATUS ='NEW_USER'");
                txtCNewUser.Text = drRows.Length.ToString();
                if (drRows.Length > 0)
                {
                    dtAVG = drRows.CopyToDataTable();
                    txtAvgNewUser.Text = Decimal.Round((decimal)dtAVG.Compute("AVG(DELTA)", ""), 2).ToString();
                }
                else
                {
                    txtAvgNewUser.Text = "0";
                }
                if (dt.Rows.Count > 0)
                {
                    txtCPDropper.Text   = String.Format("{0:0.00}", ((Convert.ToDecimal(txtCDropper.Text) / (dt.Rows.Count)) * 100)) + "%";
                    txtCPStopper.Text   = String.Format("{0:0.00}", ((Convert.ToDecimal(txtCStopper.Text) / (dt.Rows.Count)) * 100)) + "%";
                    txtCPGrower.Text    = String.Format("{0:0.00}", ((Convert.ToDecimal(txtCGrower.Text) / (dt.Rows.Count)) * 100)) + "%";
                    txtCPNonUser.Text   = String.Format("{0:0.00}", ((Convert.ToDecimal(txtCNonUser.Text) / (dt.Rows.Count)) * 100)) + "%";
                    txtCPNewUser.Text   = String.Format("{0:0.00}", ((Convert.ToDecimal(txtCNewUser.Text) / (dt.Rows.Count)) * 100)) + "%";
                    txtCPFlat.Text      = String.Format("{0:0.00}", ((Convert.ToDecimal(txtCFlat.Text) / (dt.Rows.Count)) * 100)) + "%";
                }
                else
                {
                     txtCPDropper.Text   = "0";
                     txtCPStopper.Text = "0";
                     txtCPGrower.Text = "0";
                     txtCPNonUser.Text = "0";
                     txtCPNewUser.Text = "0";
                     txtCPFlat.Text = "0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void fnStprAnnotation()
        {
            try
            {
                CA = chartThreshold.ChartAreas[0];
                VAStopper.AxisX = CA.AxisX;
                VAStopper.AllowMoving = true;
                VAStopper.IsInfinitive = true;
                VAStopper.ClipToChartArea = CA.Name;
                VAStopper.Name = "stopper";
                VAStopper.LineColor = Color.Red;
                VAStopper.LineWidth = 1;
                RAStopper.AxisX = chartThreshold.ChartAreas[0].AxisX;
                RAStopper.IsSizeAlwaysRelative = false;
                RAStopper.Width = 0.1;
                RAStopper.Height = Ymax / 10;
                RAStopper.LineColor = Color.Navy;
                RAStopper.BackColor = Color.Red;
                RAStopper.AxisY = chartThreshold.ChartAreas[0].AxisY;
                RAStopper.Y = -RAStopper.Height;
                RAStopper.X = VAStopper.X - RAStopper.Width / 2;
                RAStopper.Text = Math.Round(VAStopper.X,2).ToString();
                RAStopper.ForeColor = Color.White;
                RAStopper.Font = new System.Drawing.Font("Arial", 8f);
                chartThreshold.AnnotationPositionChanged += new EventHandler(chartThreshold_AnnotationPositionChanged);
                chartThreshold.Annotations.Add(VAStopper);
                chartThreshold.Annotations.Add(RAStopper);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void fnDrprAnnotation()
        {
            try
            {
                CA = chartThreshold.ChartAreas[0];
                VADropper.AxisX = CA.AxisX;
                VADropper.AllowMoving = true;
                VADropper.IsInfinitive = true;
                VADropper.ClipToChartArea = CA.Name;
                VADropper.Name = "Dropper";
                VADropper.LineColor = Color.Green;
                VADropper.LineWidth = 1;
                //  VADropper.X = 0.1;
                RADropper.AxisX = chartThreshold.ChartAreas[0].AxisX;
                RADropper.IsSizeAlwaysRelative = false;
                RADropper.Width = 0.1;
                RADropper.Height = Ymax / 10;
                RADropper.LineColor = Color.White;
                RADropper.BackColor = Color.Green;
                RADropper.AxisY = chartThreshold.ChartAreas[0].AxisY;
                RADropper.Y = -RADropper.Height;
                RADropper.X = Math.Round(VADropper.X - RADropper.Width / 2,2);
                RADropper.Text = Math.Round(VADropper.X,2).ToString();
                RADropper.ForeColor = Color.White;
                RADropper.Font = new System.Drawing.Font("Arial", 8f);
                chartThreshold.AnnotationPositionChanged += new EventHandler(chartThreshold_AnnotationPositionChanged);
                chartThreshold.Annotations.Add(VADropper);
                chartThreshold.Annotations.Add(RADropper);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void fnGrwrAnnotation()
        {
            try
            {
                CA = chartThreshold.ChartAreas[0];
                VAGrower.AxisX = CA.AxisX;
                VAGrower.AllowMoving = true;
                VAGrower.IsInfinitive = true;
                VAGrower.ClipToChartArea = CA.Name;
                VAGrower.Name = "Grower";
                VAGrower.LineColor = Color.Black;
                VAGrower.LineWidth = 1;
                RAGrower.AxisX = chartThreshold.ChartAreas[0].AxisX;
                RAGrower.IsSizeAlwaysRelative = false;
                RAGrower.Width = 0.1;
                RAGrower.Height = Ymax / 10;
                RAGrower.LineColor = Color.White;
                RAGrower.BackColor = Color.Black;
                RAGrower.AxisY = chartThreshold.ChartAreas[0].AxisY;
                RAGrower.Y = -RAGrower.Height;
                RAGrower.X = VAGrower.X - RAGrower.Width / 2;
                RAGrower.Text = Math.Round(VAGrower.X,2).ToString();
                RAGrower.ForeColor = Color.White;
                RAGrower.Font = new System.Drawing.Font("Arial", 8f);
                chartThreshold.AnnotationPositionChanged += new EventHandler(chartThreshold_AnnotationPositionChanged);
                chartThreshold.Annotations.Add(VAGrower);
                chartThreshold.Annotations.Add(RAGrower);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void chartThreshold_AnnotationPositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (VAStopper.X >= VADropper.X)
                {
                    VAStopper.X = Convert.ToDouble(txtCtStopper.Text);
                    VADropper.X = Convert.ToDouble(txtCtDropper.Text);
                    MessageBox.Show("Stopper Value Should not exceed Dropper Value");
                }
                else if (VADropper.X >= VAGrower.X)
                {
                    VADropper.X = Convert.ToDouble(txtCtDropper.Text);
                    VAGrower.X = Convert.ToDouble(txtCtGrower.Text);
                    MessageBox.Show("Dropper Value Should not exceed Grower Value");
                }
                else
                {
                    RAStopper.X = VAStopper.X - RAStopper.Width / 2;
                    RADropper.X = VADropper.X - RADropper.Width / 2;
                    RAGrower.X = VAGrower.X - RAGrower.Width / 2;
                    trkBarvalues();
                    txtCtStopper.Text = Convert.ToString(VAStopper.X);
                    txtCtDropper.Text = Convert.ToString(VADropper.X);
                    txtCtGrower.Text = Convert.ToString(VAGrower.X);
                    RADropper.Text = Math.Round(VADropper.X, 2).ToString();
                    RAGrower.Text = Math.Round(VAGrower.X, 2).ToString();
                    RAStopper.Text = Math.Round(VAStopper.X, 2).ToString();
                    txtCtFlat.Text = txtCtDropper.Text + " - " + txtCtGrower.Text;
                    chartThreshold.UpdateAnnotations();
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        void trkBarvalues()
        {
            grwrTrackBar.Value =Convert.ToInt32(( Math.Round(VAGrower.X + 1.0,2) )* 100);  // int.Parse(Math.Round((VAGrower.X + 1.0) *50,0).ToString());
            drprTrackBar.Value = Convert.ToInt32((Math.Round(VADropper.X + 1.0, 2)) * 100);//Convert.ToInt16(((Math.Round(VADropper.X, 0) + 1.0) * 10) * 5);
            stprTrackBar.Value = Convert.ToInt32((Math.Round(VAStopper.X + 1.0, 2)) * 100); //Convert.ToInt16(((Math.Round(VAStopper.X, 2) + 1.0) * 10) * 5);
        }
        private void stprTrackBar_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                VAStopper.X = (stprTrackBar.Value / 10.0) * 0.1 - 1;
                if (VAStopper.X >= VADropper.X)
                {
                    VAStopper.X = Convert.ToDouble(txtCtStopper.Text);
                    VADropper.X = Convert.ToDouble(txtCtDropper.Text);
                    MessageBox.Show("Stopper Value Should not exceed Dropper Value");
                }
                else
                {
                    RAStopper.X = VAStopper.X - RAStopper.Width / 2;
                    txtCtStopper.Text = Convert.ToString(VAStopper.X);
                    fnAvgDeltaCount();
                    //  chartThreshold.UpdateAnnotations();
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void grwrTrackBar_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                VAGrower.X = (grwrTrackBar.Value / 10.0) * 0.1 - 1;
                if (VADropper.X >= VAGrower.X)
                {
                    VADropper.X = Convert.ToDouble(txtCtDropper.Text);
                    VAGrower.X = Convert.ToDouble(txtCtGrower.Text);
                    MessageBox.Show("Dropper Value Should not exceed Grower Value");
                }
                else
                {

                    RAGrower.X = VAGrower.X - RAGrower.Width / 2;
                    chartThreshold.UpdateAnnotations();
                    txtCtGrower.Text = Convert.ToString(VAGrower.X);
                    fnAvgDeltaCount();
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }

        }
        private void drprTrackBar_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                VADropper.X = (drprTrackBar.Value / 10.0) * 0.1 - 1;
                if (VADropper.X >= VAGrower.X)
                {
                    VADropper.X = Convert.ToDouble(txtCtDropper.Text);
                    VAGrower.X = Convert.ToDouble(txtCtGrower.Text);
                    MessageBox.Show("Dropper Value Should not exceed Grower Value");
                }
                else
                {

                    RADropper.X = VADropper.X - RADropper.Width / 2;
                    chartThreshold.UpdateAnnotations();
                    txtCtDropper.Text = Convert.ToString(VADropper.X);
                    fnAvgDeltaCount();
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
                clsTre_Details objTreDetails = new clsTre_Details();
                string[] strT1 = Common.timePeriods.strtp1;
                string[] strT2 = Common.timePeriods.strtp2;

                strCutOff = txtCtDropper.Text + ";" + txtCtGrower.Text + ";" + txtCtStopper.Text;
                strCount = txtCFlat.Text + ";" + txtCDropper.Text + ";" + txtCGrower.Text + ";" + txtCStopper.Text + ";" + txtCNonUser.Text + ";" + txtCNewUser.Text;
                strAvgDelta = txtAvgFlat.Text + ";" + txtAvgDropper.Text + ";" + txtAvgGrower.Text + ";" + txtAvgStopper.Text + ";" + txtAvgNonUser.Text + ";" + txtAvgNewUser.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                this.Close();

                //if (objTredetails.fnSaveTREThreShold(strT1, strT2, Common.sOpportunityName, txtCtDropper.Text, txtCtGrower.Text, txtCtStopper.Text, iOpportunityId, Common.strTableName))
                //{
                //    objTredetails.fnGetBaseData(Common.strTableName, txtCtGrower.Text);
                //    objTreDetails.fnSaveOPPBreakDownStatus(iOpportunityId, 0, Convert.ToDecimal(txtCtDropper.Text), Convert.ToDecimal(txtCtStopper.Text), Convert.ToDecimal(txtCtGrower.Text),
                //    Convert.ToDecimal(txtCtNonUser.Text), Convert.ToDecimal(txtCtNewUser.Text), Convert.ToDecimal(txtCFlat.Text), Convert.ToDecimal(txtCDropper.Text), Convert.ToDecimal(txtCStopper.Text),
                //    Convert.ToDecimal(txtCGrower.Text), Convert.ToDecimal(txtCNonUser.Text), Convert.ToDecimal(txtCNewUser.Text), Convert.ToDecimal(txtAvgFlat.Text), Convert.ToDecimal(txtAvgDropper.Text),
                //    Convert.ToDecimal(txtAvgStopper.Text), Convert.ToDecimal(txtAvgGrower.Text), Convert.ToDecimal(txtAvgNonUser.Text), Convert.ToDecimal(txtAvgNewUser.Text), timePeriods.strtp1, timePeriods.strtp2, bIsInsert);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void txtCtDropper_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && (sender as Telerik.WinControls.UI.RadTextBox).Text.IndexOf('.') > -1 && ((sender as Telerik.WinControls.UI.RadTextBox).Text.IndexOf('-') > -1))
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char)13)
            {

                if (!bIsTreSholdLoaded)
                    return;
         
                if ((txtCtDropper.Text.ToString().Trim() != "" && txtCtDropper.Text.ToString().Trim() != "-"))
                {
                    if (Convert.ToDouble(txtCtDropper.Text) > Convert.ToDouble(txtCtGrower.Text) || Convert.ToDouble(txtCtDropper.Text) < Convert.ToDouble(txtCtStopper.Text))
                    {
                        MessageBox.Show("Dropper value cannot be higher than grower or less than stopper");
                        return;
                    }
                   
                    else
                    {

                       // VAStopper.X = Convert.ToDouble(txtCtStopper.Text);
                        VADropper.X = Convert.ToDouble(txtCtDropper.Text);
                        //VAGrower.X = Convert.ToDouble(txtCtGrower.Text);
                        //RAStopper.X = VAStopper.X - RAStopper.Width / 2;
                        RADropper.X = VADropper.X - RADropper.Width / 2;
                        RADropper.Text=Math.Round(VADropper.X,2).ToString();
                        RAGrower.X = VAGrower.X - RAGrower.Width / 2;
                        txtCtFlat.Text = txtCtDropper.Text + " -  +" + txtCtGrower.Text;
                        trkBarvalues();


                    }
                }
                // Then Enter key was pressed

            }


        }
        private void txtCtStopper_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {

                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if ((e.KeyChar == '.') && (sender as Telerik.WinControls.UI.RadTextBox).Text.IndexOf('.') > -1 && ((sender as Telerik.WinControls.UI.RadTextBox).Text.IndexOf('-') > -1))
                {
                    e.Handled = true;
                }
                if (e.KeyChar == (char)13)
                {

                    if (!bIsTreSholdLoaded)
                        return;

                    if ((txtCtStopper.Text.ToString().Trim() != "" && txtCtStopper.Text.ToString().Trim() != "-"))
                    {
                        if (Convert.ToDouble(txtCtStopper.Text) > Convert.ToDouble(txtCtDropper.Text))
                        {
                            MessageBox.Show("Stopper Value must be less than Dropper Value");
                            txtCtStopper.Text = "";
                            return;
                        }
                      
                        else
                        {

                            VAStopper.X = Convert.ToDouble(txtCtStopper.Text);
                            RAStopper.X = VAStopper.X - RAStopper.Width / 2;
                            RAStopper.Text = Math.Round(VAStopper.X, 2).ToString();
                            txtCtFlat.Text = txtCtDropper.Text + " -  +" + txtCtGrower.Text;
                            trkBarvalues();


                        }
                    }
                    // Then Enter key was pressed

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void txtCtGrower_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {

                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if ((e.KeyChar == '.') && (sender as Telerik.WinControls.UI.RadTextBox).Text.IndexOf('.') > -1 && ((sender as Telerik.WinControls.UI.RadTextBox).Text.IndexOf('-') > -1))
                {
                    e.Handled = true;
                }
                if (e.KeyChar == (char)13)
                {

                    if (!bIsTreSholdLoaded)
                        return;

                    if ( (txtCtGrower.Text.ToString().Trim() != "" && txtCtGrower.Text.ToString().Trim() != "-"))
                    {
                        if (Convert.ToDouble(txtCtStopper.Text) > Convert.ToDouble(txtCtDropper.Text))
                        {
                            MessageBox.Show("Grower Value cannot be less than Dropper Value");
                            return;
                        }
                        
                        else
                        {

                             VAGrower.X = Convert.ToDouble(txtCtGrower.Text);
                             RAGrower.X = VAGrower.X - RAStopper.Width / 2;
                             RAGrower.Text = Math.Round(VAGrower.X, 2).ToString();
                            txtCtFlat.Text = txtCtDropper.Text + " -  +" + txtCtGrower.Text;
                            trkBarvalues();


                        }
                    }
                    // Then Enter key was pressed

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

       


    }
}
