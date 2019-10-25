using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RecoEngine
{
    public partial class frmDatsource : Telerik.WinControls.UI.RadForm
    {
        public frmDatsource()
        {
            InitializeComponent();
        }

        private void frmDatsource_Load(object sender, EventArgs e)
        {
           
            fnLoadDataSource();
        }
        void fnLoadDataSource()
        {
            pnMain.Controls.Clear();
            CntrlDataSource ctl = new CntrlDataSource(1);
            ctl.Dock = DockStyle.Fill;

            Telerik.WinControls.UI.RadGroupBox gbDummy = Common.GetfrmDummy();
            pnMain.Controls.Add(gbDummy);

            pnMain.Controls.Add(ctl);
            pnMain.Controls.Remove(gbDummy);
            ctl.fnSetFocus();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Common.bIsConnectionStringEstablish || Common.strTableName.Trim() == "")
            {
                Telerik.WinControls.RadMessageBox.Show(this, "Please Establish Conenction  / Map the feilds. ", "DataConnection", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                frmLogin frm = new frmLogin();
                frm.Show();
                this.Hide();
            }
        }

        private void frmDatsource_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
