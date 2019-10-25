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
    public partial class CntrlRecommendation : UserControl
    {

        int iOpportunityId = 0;
        string strExpression = "";
        string strPntlExpression = "";
        clsOpportunities ClsObj = new clsOpportunities();
        clsRanking objRanking = new clsRanking();
        //string strOpportunityName = "";
        bool bMWindowLoaded = false;
        bool bOpportunityLoaded = false;
        bool bIsRankingLoaded = false;

        bool bGroupingLoaded = false;
        bool bIsShowOPPList = true;
        clsTre_Details clstreDetails = new clsTre_Details();
        int iTabIndex = 1;
        string[] strCt;
        string[] strCount;
        string[] strAvg;
        string strCurrentSegmentColumn = "";
        int iIsActive = 0;
        public CntrlRecommendation(int iTabIndex)
        {
            InitializeComponent();
            this.iTabIndex = iTabIndex;
        }


        private void CntrlRecommendation_Load(object sender, EventArgs e)
        {
            try
            {
                mvmntwindowMainpanel.Padding = new Padding(0, 0, 0, 0);
                gbGMain.Padding = new Padding(0, 0, 0, 0);
                fnShowTimePeriod();
                fnShowGrouping();
                //}
                switch (iTabIndex)
                {
                    case 1:
                        pgVRecommendation.SelectedPage = pgRcmndSettings;
                        break;
                    case 2:
                        pgVRecommendation.SelectedPage = pgOpportunityMapping;
                        fnShowOpporunityList(bIsShowOPPList);
                        break;
                    case 3:
                        pgVRecommendation.SelectedPage = pgRanking;
                        fnBindRanking();
                        break;
                    case 5:
                        // pgVRecommendation.SelectedPage = pgOppurtunityPotential;
                        break;
                    case 6:
                        pgVRecommendation.SelectedPage = pgOppDetails;
                        break;

                }

                btnNew.Focus();
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void pgVRecommendation_SelectedPageChanged(object sender, EventArgs e)
        {
            try
            {
                //if (Common.sOpportunityName == "")
                //{
                //    setCurrentOpportunityName();
                //}
                switch (pgVRecommendation.SelectedPage.Tag.ToString().ToLower())
                {
                    case "recommendationsettings":
                        //  fnShowTimePeriod();
                        break;
                    case "opportunitymapping":
                        fnShowOpporunityList(bIsShowOPPList);
                        break;
                    //case "thresholds":
                    //    fnShowThreshold();
                    //    break;
                    case "opportunities":
                        fnShowOpportunitiesDetails();
                        break;
                    case "ranking":
                        fnBindRanking();
                        break;

                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

      

       
       

      
    }

}
