using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecoEngine_BI;

namespace RecoEngine
{
    public partial class CntrlOpportunityList : UserControl
    {
        clsOpportunities clsObj = new clsOpportunities();

        public CntrlOpportunityList()
        {
            InitializeComponent();
        }

        private void CntrlOpportunityList_Load(object sender, EventArgs e)
        {
            fnLoadData();
        }
        private void fnLoadData()
        {
            
            DataTable dt = clsObj.fnGetOpportunity(Common.iProjectID,Common.iUserID,true);
            grdOppList.DataSource = dt;
            grdOppList.Columns["OPPORTUNITY_ID"].IsVisible = false;
            grdOppList.Columns["Flag"].IsVisible = false;
            grdOppList.Columns["FORMULA"].IsVisible = false;


        }

     
    }
}
