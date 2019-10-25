using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace RecoEngine_DataLayer
{
    public class OraDBManager
    {
         # region "Variable & Declaration"

        private OracleConnection _cn;
        private string sConnectionString = "";
        public string strProductName = "";

        private bool bDebugOn = false;

        public void SetDebug(bool bValue)
        {
            bDebugOn = bValue;
        }

        private OracleTransaction _Transaction;
        public bool bInTransaction = false;
        #endregion "Variable & Declaration"

        # region "Constructor"

        public OraDBManager(string sConStr)
        {
            sConnectionString = sConStr;
        }

        #endregion "Constructor"

        # region "Begin Commit Rollback"
        public bool BeginTrans()
        {
            try
            {
                OpenConnection();
                _Transaction = _cn.BeginTransaction();
                bInTransaction = true;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CommitTrans()
        {
            try
            {
                _Transaction.Commit();
                CloseConnection();
                bInTransaction = false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RollbackTrans()
        {
            try
            {
                _Transaction.Rollback();
                CloseConnection();
                bInTransaction = false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        # endregion "Begin Commit Rollback"

        private OracleConnection Con
        {
            get { return _cn; }
            set
            {
                _cn = value;
                _cn.Open();
            }
        }

        public string GetConnectionString
        {

            get { return sConnectionString; }

        }

        public void GetConnectionAttributes(ref string sDataSource, ref string sDatabase, ref string sUserId, ref string sPassword)
        {
            string[] sAttrib = sConnectionString.Split(';');
            string[] sArrTemp = null;

            for (int i = 0; i < sAttrib.Length; i++)
            {
                sArrTemp = sAttrib[i].Split('=');
                if (sArrTemp.Length == 2)
                {
                    if (sArrTemp[0] == "Data Source")
                        sDataSource = sArrTemp[1].Trim();
                    else if (sArrTemp[0] == "Initial Catalog")
                        sDatabase = sArrTemp[1].Trim();
                    else if (sArrTemp[0] == "User Id")
                        sUserId = sArrTemp[1].Trim();
                    else if (sArrTemp[0] == "Password")
                        sPassword = sArrTemp[1].Trim();
                }
            }
        }

        private void CloseConnection()
        {
            if ((_cn != null))
            {
                _cn.Close();
                _cn.Dispose();
                _cn = null;
            }
        }

        private void OpenConnection()
        {
            _cn = new OracleConnection(sConnectionString);
            if ((_cn != null))
            {
                _cn.Open();
            }
        }


        private void AttachParameters(OracleCommand Cmd, OracleParameter[] Param)
        {

            if (Cmd == null)
                throw new ArgumentNullException("Cmd");

            if (Param == null)
            {
                foreach (OracleParameter p in Param)
                {
                    if ((p != null))
                    {
                        if ((p.Direction == ParameterDirection.InputOutput | p.Direction == ParameterDirection.Input) & p.Value.ToString() == "")
                        {
                            p.Value = DBNull.Value;
                        }
                    }
                    Cmd.Parameters.Add(p);
                }
            }
            else
            {
                foreach (OracleParameter p in Param)
                {
                    Cmd.Parameters.Add(p);

                }
            }
        }

        private void PrepareCommand(ref OracleCommand Cmd, CommandType CmdType, string CmdText, OracleParameter[] Params)
        {

            if (Cmd == null)
            {
                throw new ArgumentNullException("Cmd");
            }

            if (CmdText == "" | CmdText.Length == 0)
            {
                throw new ArgumentNullException("CmdText");
            }

            if (!bInTransaction)
            {
                if (!(Con.State == ConnectionState.Open))
                {
                    OpenConnection();
                }
            }

            Cmd.Connection = Con;

            if (bInTransaction)
                Cmd.Transaction = _Transaction;

            Cmd.CommandText = CmdText;
            Cmd.CommandType = CmdType;
            Cmd.CommandTimeout = 60;
            if ((Params != null))
            {
                AttachParameters(Cmd, Params);
            }
        }

        public int ExecuteNonQuery(CommandType CmdType, string CmdText)
        {
            return ExecuteNonQuery(CmdType, CmdText, (OracleParameter[])null);
        }

        public int ExecuteNonQuery(OracleCommand cmd)
        {
            if (!bInTransaction)
                OpenConnection();

            cmd.Connection = Con;
            if (bInTransaction)
                cmd.Transaction = _Transaction;

            int iResult = cmd.ExecuteNonQuery();
            cmd.CommandTimeout = 60;

            if (!bInTransaction)
                CloseConnection();

            return iResult;

        }

        public int ExecuteNonQuery(CommandType CmdType, string CmdText, OracleParameter[] Params)
        {

            if (bDebugOn == true)
                System.Windows.Forms.MessageBox.Show(CmdText, strProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            try
            {
                OracleCommand cmd = new OracleCommand();
                int retVal;

                if (!bInTransaction)
                    OpenConnection();

                PrepareCommand(ref cmd, CmdType, CmdText, Params);

                if (bInTransaction)
                    cmd.Transaction = _Transaction;

                retVal = cmd.ExecuteNonQuery();

                if (!bInTransaction)
                    CloseConnection();

                cmd.Parameters.Clear();
                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteNonQuery(CommandType CmdType, string CmdText, OracleParameter[] Params,bool bIsOutParms)
        {

            if (bDebugOn == true)
                System.Windows.Forms.MessageBox.Show(CmdText, strProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            try
            {
                OracleCommand cmd = new OracleCommand();
                int retVal;

                if (!bInTransaction)
                    OpenConnection();

                PrepareCommand(ref cmd, CmdType, CmdText, Params);

                if (bInTransaction)
                    cmd.Transaction = _Transaction;

                retVal = cmd.ExecuteNonQuery();
                if(bIsOutParms)
                    retVal=Convert.ToInt32(cmd.Parameters["RETURN_VALUE"].Value);
                if (!bInTransaction)
                    CloseConnection();

                cmd.Parameters.Clear();
                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet ExecuteDataSet(CommandType CmdType, string CmdText)
        {
            return ExecuteDataSet(CmdType, CmdText, (OracleParameter[])null);
        }

        public DataSet ExecuteDataSet(CommandType CmdType, string CmdText, OracleParameter[] Params)
        {
            try
            {
                if (bDebugOn == true)
                    System.Windows.Forms.MessageBox.Show(CmdText, strProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                OracleCommand cmd = new OracleCommand();

                if (!bInTransaction)
                    OpenConnection();

                PrepareCommand(ref cmd, CmdType, CmdText, Params);

                if (bInTransaction)
                    cmd.Transaction = _Transaction;

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (!bInTransaction)
                    CloseConnection();

                cmd.Parameters.Clear();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ExecuteDataTable(CommandType CmdType, string CmdText)
        {
            return ExecuteDataTable(CmdType, CmdText, (OracleParameter[])null);
        }

        public DataTable ExecuteDataTable(CommandType CmdType, string CmdText, OracleParameter[] Params)
        {

            if (bDebugOn == true)
                System.Windows.Forms.MessageBox.Show(CmdText, strProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            OracleCommand cmd = new OracleCommand();
            if (!bInTransaction)
                OpenConnection();

            PrepareCommand(ref cmd, CmdType, CmdText, Params);

            if (bInTransaction)
                cmd.Transaction = _Transaction;

            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (!bInTransaction)
                CloseConnection();

            cmd.Parameters.Clear();
            return dt;
        }
        public OracleDataReader ExecuteReader(CommandType CmdType, string CmdText, OracleParameter[] Params)
        {

            if (bDebugOn == true)
                System.Windows.Forms.MessageBox.Show(CmdText, strProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            OracleCommand cmd = new OracleCommand();
            OracleDataReader dr;

            if (!bInTransaction)
                OpenConnection();

            PrepareCommand(ref cmd, CmdType, CmdText, Params);

            if (bInTransaction)
                cmd.Transaction = _Transaction;

            dr = cmd.ExecuteReader();

            if (!bInTransaction)
                CloseConnection();

            cmd.Parameters.Clear();
            return dr;
        }


        public string ExecuteScalar(CommandType CmdType, string CmdText)
        {
            return ExecuteScalar(CmdType, CmdText, (OracleParameter[])null);
        }

        public string ExecuteScalar(CommandType CmdType, string CmdText, OracleParameter[] Params)
        {
            Object rValue;
            if (bDebugOn == true)
                System.Windows.Forms.MessageBox.Show(CmdText, strProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            OracleCommand cmd = new OracleCommand();

            if (!bInTransaction)
                OpenConnection();

            PrepareCommand(ref cmd, CmdType, CmdText, Params);

            if (bInTransaction)
                cmd.Transaction = _Transaction;

            rValue = cmd.ExecuteScalar();

            if (!bInTransaction)
                CloseConnection();

            cmd.Parameters.Clear();

            if (rValue == null)
                return "";
            else
                return rValue.ToString();
        }

        //public int GetInsertedMasterProject(int P_EPM_PROJECT_ID, int P_VENDOR_ID, string P_PROJECT_DESC, DateTime P_PROJECT_START_DATE, int P_CURRENCY_ID, int P_EXCHANGE_RATE, int P_DEPT_UNIT_ID, string P_CONTRACT_NO)
        //{
        //    OracleCommand cmd = new OracleCommand();
        //    OpenConnection();
        //    cmd.Connection = _cn;
        //    cmd.CommandText = "TESTEPM.INS_PROJECT_MASTER";
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    OracleParameter par;

        //    par = new OracleParameter("P_EPM_PROJECT_ID", OracleType.Number);
        //    par.Value = P_EPM_PROJECT_ID;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("P_VENDOR_ID", OracleType.Number);
        //    par.Value = P_VENDOR_ID;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("P_PROJECT_DESC", OracleType.VarChar);
        //    par.Value = P_PROJECT_DESC;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("P_PROJECT_START_DATE", OracleType.DateTime);
        //    par.Value =P_PROJECT_START_DATE;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("P_CURRENCY_ID", OracleType.Number);
        //    par.Value = P_CURRENCY_ID;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("P_EXCHANGE_RATE", OracleType.Number);
        //    par.Value = P_EXCHANGE_RATE;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("P_DEPT_UNIT_ID", OracleType.Number);
        //    par.Value = P_DEPT_UNIT_ID;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("P_CONTRACT_NO", OracleType.VarChar);
        //    par.Value = P_CONTRACT_NO;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("RETURN_VALUE", OracleType.Number);
        //    par.Direction = ParameterDirection.ReturnValue;
        //    cmd.Parameters.Add(par);

        //    cmd.ExecuteNonQuery();
        //    CloseConnection();
        //    return Convert.ToInt32(cmd.Parameters["RETURN_VALUE"].Value);
        //}

        //public int GetInsertedProjectMilestone(int p_project_id, int p_epm_milestone_id, string p_milestone_code, string p_milestone_desc, int p_milestone_amount, int p_currency_id, int p_exchange_rate)
        //{
        //    OracleCommand cmd = new OracleCommand();
        //    OpenConnection();
        //    cmd.Connection = _cn;
        //    cmd.CommandText = "<sd$epm.insert_project_milestone>";
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    OracleParameter par =
        //    new OracleParameter("RETURN_VALUE", OracleType.Number);
        //    par.Direction = ParameterDirection.ReturnValue;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_project_id", OracleType.Number);
        //    par.Value = p_project_id;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_epm_milestone_id", OracleType.Number);
        //    par.Value = p_epm_milestone_id;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_milestone_code", OracleType.VarChar);
        //    par.Value = p_milestone_code;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_milestone_desc", OracleType.VarChar);
        //    par.Value = p_milestone_desc;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_milestone_amount", OracleType.Number);
        //    par.Value = p_milestone_amount;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_currency_id", OracleType.Number);
        //    par.Value = p_currency_id;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_exchange_rate", OracleType.Number);
        //    par.Value = p_exchange_rate;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    cmd.ExecuteNonQuery();
        //    CloseConnection();
        //    return Convert.ToInt32(cmd.Parameters["RETURN_VALUE"].Value);
        //}

        //public int GetInsertedMilestoneInvoice(int p_milestone_id, int p_epm_invoice_id, int p_vendor_id, string p_desc_n, DateTime p_invoice_date, string p_invoice_no, int p_invoice_amount, int p_penalty_amount, string p_penalty_desc, int p_currency_id, int p_exchange_rate, string p_file_name)
        //{
        //    OracleCommand cmd = new OracleCommand();
        //    OpenConnection();
        //    cmd.Connection = _cn;
        //    cmd.CommandText = "<sd$epm.insert_milestone_invoice>";
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    OracleParameter par =
        //    new OracleParameter("RETURN_VALUE", OracleType.Number);
        //    par.Direction = ParameterDirection.ReturnValue;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_milestone_id", OracleType.Number);
        //    par.Value = p_milestone_id;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_epm_invoice_id", OracleType.Number);
        //    par.Value = p_epm_invoice_id;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_vendor_id", OracleType.Number);
        //    par.Value = p_vendor_id;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_desc_n", OracleType.VarChar);
        //    par.Value = p_desc_n;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_invoice_date", OracleType.DateTime);
        //    par.Value = p_invoice_date;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_invoice_no", OracleType.VarChar);
        //    par.Value = p_invoice_no;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_invoice_amount", OracleType.Number);
        //    par.Value = p_invoice_amount;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_penalty_amount", OracleType.Number);
        //    par.Value = p_penalty_amount;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_penalty_desc", OracleType.VarChar);
        //    par.Value = p_penalty_desc;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_currency_id", OracleType.Number);
        //    par.Value = p_currency_id;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_exchange_rate", OracleType.Number);
        //    par.Value = p_exchange_rate;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    par = new OracleParameter("p_file_name", OracleType.VarChar);
        //    par.Value = p_file_name;
        //    par.Direction = ParameterDirection.Input;
        //    cmd.Parameters.Add(par);

        //    cmd.ExecuteNonQuery();
        //    CloseConnection();
        //    return Convert.ToInt32(cmd.Parameters["RETURN_VALUE"].Value);
        //}

    }
}
