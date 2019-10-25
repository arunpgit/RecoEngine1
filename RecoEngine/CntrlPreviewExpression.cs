using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecoEngine_BI;

namespace RecoEngine
{
    public partial class CntrlPreviewExpression : Form
    {
        clsTre_Details objTredetails = new clsTre_Details();
        public CntrlPreviewExpression(string strTableName)
        {
            InitializeComponent();
            BindGrid(strTableName);

        }

        void BindGrid(string strTableName)
        {
            try
            {

                grdPreviewExpression.DataSource = objTredetails.fnPreviewExpressionEditor(strTableName);
                grdPreviewExpression.BestFitColumns();
            }
            catch (Exception ex)
            {
              MessageBox.Show(ex.Message) ;
            }
        }

        private void grdPreviewExpression_Click(object sender, EventArgs e)
        {

        }
    }
}
