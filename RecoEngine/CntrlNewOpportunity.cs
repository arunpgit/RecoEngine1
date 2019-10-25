using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Xml;
using System.IO;
using RecoEngine_BI;


namespace RecoEngine
{
    public partial class CntrlNewOpportunity : UserControl
    {
        int iOpportunityId = 0;
        string strExpression = "";
        clsOpportunities ClsObj = new clsOpportunities();
        public CntrlNewOpportunity()
        {
            InitializeComponent();
        }


        private void pctFormula_Click(object sender, EventArgs e)
        {
            try
            {
                clsDataSource clsObj = new clsDataSource();
                // frmExpressEditor frm = new frmExpressEditor();
                DataTable dtSource = clsObj.fnGetColMappingData ();

                DataTable dt = new DataTable();

                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    if ((dtSource.Rows[i]["TYPE"].ToString() == ((int)Enums.ColType.Key).ToString()) ||
                        (dtSource.Rows[i]["TYPE"].ToString() == ((int)Enums.ColType.Input).ToString() && dtSource.Rows[i]["ISREQUIRED"].ToString() == "1"))
                    {
                        if (dtSource.Rows[i]["COLDATATYPE"].ToString().ToUpper() == "System.Decimal".ToUpper())
                        {
                            dt.Columns.Add(new DataColumn(dtSource.Rows[i]["COLNAME"].ToString(), typeof(decimal)));

                        }
                        else if (dtSource.Rows[i]["COLDATATYPE"].ToString().ToUpper() == "System.DateTime".ToUpper())
                            dt.Columns.Add(new DataColumn(dtSource.Rows[i]["COLNAME"].ToString(), typeof(DateTime)));
                        else if (dtSource.Rows[i]["COLDATATYPE"].ToString().ToUpper() == "System.String".ToUpper())
                        {
                            dt.Columns.Add(new DataColumn(dtSource.Rows[i]["COLNAME"].ToString(), typeof(string)));
                        }
                        else if (dtSource.Rows[i]["COLDATATYPE"].ToString().ToUpper() == "System.int".ToUpper() ||
                            dtSource.Rows[i]["COLDATATYPE"].ToString().ToUpper() == "System.Int16".ToUpper() ||
                            dtSource.Rows[i]["COLDATATYPE"].ToString().ToUpper() == "System.Int32".ToUpper() ||
                            dtSource.Rows[i]["COLDATATYPE"].ToString().ToUpper() == "System.Int64".ToUpper())
                        {
                            dt.Columns.Add(new DataColumn(dtSource.Rows[i]["COLNAME"].ToString(), typeof(int)));
                        }
                    }
                }


                dt.Columns.Add(new DataColumn("OPPValue", typeof(decimal)));


                using (var frm = new frmExpressEditor((int)Enums.ExpressionType.Opp_ptnl, Common.strTableName, strExpression))
                {
                    frm._fieldDict = GetDict(dt);
                    frm.AvailableFields = frm._fieldDict.ToList<KeyValuePair<string, Type>>();
                    frm.dtSource = dtSource;
                    var res = frm.ShowDialog();
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        strExpression = frm.strExpression;                        
                    }
                }

            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }

        }
        internal Dictionary<string, Type> GetDict(DataTable dt)
        {
            try
            {
                Dictionary<string, Type> de = new Dictionary<string, Type>();

                foreach (DataColumn dc in dt.Columns)
                {
                    de.Add("Field." + dc.ColumnName.ToString(), dc.DataType);
                    if (dc.ColumnName.IndexOf("_") > -1)
                        de.Add("Field." + dc.ColumnName.ToString().Replace("_", ""), dc.DataType);
                }

                return de;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim().Length == 0)
            {
                Telerik.WinControls.RadMessageBox.Show("Opportunity Name Should not be blank", "Validation", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                txtName.Focus();
                return;
            }
            if (strExpression == "")
            {
                Telerik.WinControls.RadMessageBox.Show("Please Add formula.", "Validation", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                pctFormula.Focus();
                return;
            }

            string strEx = strExpression.Replace("FIELD!", "");
            if (strEx.StartsWith("="))
                strEx = strEx.Substring(1);

            Common.strFormula = strEx;
            Common.sOpportunityName = txtName.Text;

            iOpportunityId = ClsObj.fnSaveOpportunity(iOpportunityId, txtName.Text.ToString(), txtDesc.Text.ToString(), strEx, Common.iUserID, Common.iProjectID,Common.strTableName,Common.strKeyName,RecoEngine.Common.timePeriods.strtp1,RecoEngine.Common.timePeriods.strtp2);
    
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtName.Text.Length >0)
            {
                string strFc = txtName.Text.Substring(0, 1);

                if (!Char.IsLetter(txtName.Text, 0))
                {
                    Telerik.WinControls.RadMessageBox.Show("Opportunity should start with Alphabet");                    
                }
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {           


            base.OnKeyPress(e);

            if (txtName.Text.Length == 0 && e.KeyChar != (char)0x08)
            {
                 if (!(char.IsLetter(e.KeyChar)))
                 {
                     e.Handled = true;
                 }
            }
            if (!(char.IsLetter(e.KeyChar)) && !(char.IsDigit(e.KeyChar)) && (e.KeyChar != '_') && e.KeyChar != (char)0x08)
            {
                e.Handled = true;
                // char is neither letter or digit.
                // there are more methods you can use to determine the
                // type of char, e.g. char.IsSymbol
            }
        }

    }

}
