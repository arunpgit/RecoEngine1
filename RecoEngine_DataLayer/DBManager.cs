using System;
using System.Data;
using System.Data.SqlClient;

namespace RecoEngine_DataLayer
{
    public class DBManager
    {
        //

        # region "Variable & Declaration"

        private SqlConnection _cn;
        private string sConnectionString = "";
        public string strProductName = "";
        private bool bDebugOn = false;

        public void SetDebug(bool bValue)
        {
            bDebugOn = bValue;
        }

        private SqlTransaction _Transaction;
        public bool bInTransaction = false;
        #endregion "Variable & Declaration"

        # region "Constructor"

        public DBManager(string sConStr)
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

        public bool RoolbackTrans()
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

        private SqlConnection Con
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
            _cn = new SqlConnection(sConnectionString);
            if ((_cn != null))
            {
                _cn.Open();
            }
        }


        private void AttachParameters(SqlCommand Cmd, SqlParameter[] Param)
        {

            if (Cmd == null)
                throw new ArgumentNullException("Cmd");

            if (Param == null)
            {
                foreach (SqlParameter p in Param)
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
                foreach (SqlParameter p in Param)
                {
                    Cmd.Parameters.Add(p);

                }
            }
        }

        private void PrepareCommand(ref SqlCommand Cmd, CommandType CmdType, string CmdText, SqlParameter[] Params)
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
            return ExecuteNonQuery(CmdType, CmdText, (SqlParameter[])null);
        }

        public int ExecuteNonQuery(SqlCommand cmd)
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

        public int ExecuteNonQuery(CommandType CmdType, string CmdText, SqlParameter[] Params)
        {

            if (bDebugOn == true)
                System.Windows.Forms.MessageBox.Show(CmdText, strProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            try
            {
                SqlCommand cmd = new SqlCommand();
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

        public DataSet ExecuteDataSet(CommandType CmdType, string CmdText)
        {
            return ExecuteDataSet(CmdType, CmdText, (SqlParameter[])null);
        }

        public DataSet ExecuteDataSet(CommandType CmdType, string CmdText, SqlParameter[] Params)
        {
            try
            {
                if (bDebugOn == true)
                    System.Windows.Forms.MessageBox.Show(CmdText, strProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                SqlCommand cmd = new SqlCommand();

                if (!bInTransaction)
                    OpenConnection();

                PrepareCommand(ref cmd, CmdType, CmdText, Params);

                if (bInTransaction)
                    cmd.Transaction = _Transaction;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
            return ExecuteDataTable(CmdType, CmdText, (SqlParameter[])null);
        }

        public DataTable ExecuteDataTable(CommandType CmdType, string CmdText, SqlParameter[] Params)
        {

            if (bDebugOn == true)
                System.Windows.Forms.MessageBox.Show(CmdText, strProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            SqlCommand cmd = new SqlCommand();
            if (!bInTransaction)
                OpenConnection();

            PrepareCommand(ref cmd, CmdType, CmdText, Params);

            if (bInTransaction)
                cmd.Transaction = _Transaction;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (!bInTransaction)
                CloseConnection();

            cmd.Parameters.Clear();
            return dt;
        }
        public SqlDataReader ExecuteReader(CommandType CmdType, string CmdText, SqlParameter[] Params)
        {

            if (bDebugOn == true)
                System.Windows.Forms.MessageBox.Show(CmdText, strProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

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
            return ExecuteScalar(CmdType, CmdText, (SqlParameter[])null);
        }

        public string ExecuteScalar(CommandType CmdType, string CmdText, SqlParameter[] Params)
        {
            Object rValue;
            if (bDebugOn == true)
                System.Windows.Forms.MessageBox.Show(CmdText, strProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            SqlCommand cmd = new SqlCommand();

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
        public bool GetFileFromDB(string sFileId, string sVersionNumber, string sChoosenPath, ref string tempFilePath, string sSpecialFileName, bool bIsScanFile, bool bDonotShowFileIdAndVersion)
        {

            OpenConnection();
            try
            {
                System.IO.FileStream fs;
                System.IO.BinaryWriter bw;
                long startIndex = 0;
                int bufferSize = 100;
                byte[] outbyte = new byte[bufferSize];
                long retval;

                string strSql = "";

                if (bIsScanFile)
                {
                    strSql = "Select FName,FileContent from ScanFiles Where FileId=" + sFileId;
                }
                else
                {
                    strSql = "Select A.FName, B.FileContent from FileVersions B Inner Join Files A On A.FileId=B.FileId Where B.FileId=" + sFileId + " and B.VersionNumber=" + sVersionNumber;
                }

                SqlCommand cmd = new SqlCommand(strSql, _cn);

                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                while (myReader.Read())
                {
                    if (sChoosenPath == "")
                    {
                        if (sSpecialFileName == "")
                        {
                            if (bDonotShowFileIdAndVersion)
                                tempFilePath = GetMosaiqueTempPath() + myReader.GetString(0);
                            else
                                tempFilePath = GetMosaiqueTempPath() + sFileId + "_" + sVersionNumber + "_" + myReader.GetString(0);
                        }
                        else
                        {
                            if (bDonotShowFileIdAndVersion)
                                tempFilePath = GetMosaiqueTempPath() + sSpecialFileName + "_" + myReader.GetString(0);
                            else
                                tempFilePath = GetMosaiqueTempPath() + sVersionNumber + "_" + sSpecialFileName + "_" + myReader.GetString(0);
                        }

                        if (System.IO.File.Exists(tempFilePath))
                        {
                            if (TryFileDelete(tempFilePath) == false)
                            {
                                return false;
                            }
                        }
                        fs = new System.IO.FileStream(tempFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                    }
                    else
                        fs = new System.IO.FileStream(sChoosenPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);

                    bw = new System.IO.BinaryWriter(fs);
                    startIndex = 0;

                    retval = myReader.GetBytes(1, startIndex, outbyte, 0, bufferSize);
                    while (retval == bufferSize)
                    {
                        bw.Write(outbyte);
                        bw.Flush();
                        startIndex += bufferSize;
                        retval = myReader.GetBytes(1, startIndex, outbyte, 0, bufferSize);
                    }

                    // Write the remaining buffer.
                    bw.Write(outbyte, 0, (int)retval);
                    bw.Flush();

                    // Close the output file.
                    bw.Close();
                    fs.Close();
                    bw = null;
                    fs = null;
                }

                myReader.Close();

                if (tempFilePath != "")
                {
                    if (System.IO.File.Exists(tempFilePath))
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(tempFilePath);
                        fi.CreationTime = DateTime.Now;
                        fi.LastWriteTime = fi.CreationTime;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;
            }
            finally
            {

                CloseConnection();
            }
        }

        public bool GetFileFromDB_FinBase(string sFileId, string sVersionNumber, string sChoosenPath, ref string tempFilePath, string sSpecialFileName, bool bIsScanFile, bool bDonotShowFileIdAndVersion)
        {
            if (!bInTransaction)
                OpenConnection();
            try
            {
                System.IO.FileStream fs;
                System.IO.BinaryWriter bw;
                long startIndex = 0;
                int bufferSize = 100;
                byte[] outbyte = new byte[bufferSize];
                long retval;

                string strSql = "";

                if (bIsScanFile)
                {
                    strSql = "Select FName,FileContent from ScanFiles Where FileId=" + sFileId;
                }
                else
                {
                    strSql = "Select A.FName, B.FileContent from FileVersions B Inner Join Files A On A.FileId=B.FileId Where B.FileId=" + sFileId + " and B.VersionNumber=" + sVersionNumber;
                }

                SqlCommand cmd = new SqlCommand(strSql, _cn);

                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                while (myReader.Read())
                {
                    if (sChoosenPath == "")
                    {
                        if (sSpecialFileName == "")
                        {
                            if (bDonotShowFileIdAndVersion)
                                tempFilePath = GetMosaiqueTempPath() + myReader.GetString(0);
                            else
                                tempFilePath = GetMosaiqueTempPath() + sFileId + "_" + sVersionNumber + "_" + myReader.GetString(0);
                        }
                        else
                        {
                            if (bDonotShowFileIdAndVersion)
                                tempFilePath = GetMosaiqueTempPath() + sSpecialFileName + "_" + myReader.GetString(0);
                            else
                                tempFilePath = GetMosaiqueTempPath() + sVersionNumber + "_" + sSpecialFileName + "_" + myReader.GetString(0);
                        }

                        if (System.IO.File.Exists(tempFilePath))
                        {
                            if (TryFileDelete(tempFilePath) == false)
                            {
                                return false;
                            }
                        }
                        fs = new System.IO.FileStream(tempFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                    }
                    else
                        fs = new System.IO.FileStream(sChoosenPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);

                    bw = new System.IO.BinaryWriter(fs);
                    startIndex = 0;

                    retval = myReader.GetBytes(1, startIndex, outbyte, 0, bufferSize);
                    while (retval == bufferSize)
                    {
                        bw.Write(outbyte);
                        bw.Flush();
                        startIndex += bufferSize;
                        retval = myReader.GetBytes(1, startIndex, outbyte, 0, bufferSize);
                    }

                    // Write the remaining buffer.
                    bw.Write(outbyte, 0, (int)retval);
                    bw.Flush();

                    // Close the output file.
                    bw.Close();
                    fs.Close();
                    bw = null;
                    fs = null;
                }

                myReader.Close();

                if (tempFilePath != "")
                {
                    if (System.IO.File.Exists(tempFilePath))
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(tempFilePath);
                        fi.CreationTime = DateTime.Now;
                        fi.LastWriteTime = fi.CreationTime;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;
            }
            finally
            {
                if (!bInTransaction)
                    CloseConnection();
            }
        }

        public bool GetFileFromDB(string sFileId, string sVersionNumber, string sChoosenPath, ref string tempFilePath, string sSpecialFileName, bool bIsScanFile)
        {
            return GetFileFromDB(sFileId, sVersionNumber, sChoosenPath, ref tempFilePath, sSpecialFileName, bIsScanFile, false);
        }

        private bool TryFileDelete(string sFilePath)
        {
            bool bIsReadOnly = false;
            System.IO.FileInfo fi = new System.IO.FileInfo(sFilePath);
            try
            {
                if ((fi.Attributes & System.IO.FileAttributes.ReadOnly) > 0)
                {
                    fi.Attributes -= System.IO.FileAttributes.ReadOnly;
                    bIsReadOnly = true;
                }
                fi.Delete();
                return true;
            }
            catch (Exception ex)
            {
                ex = null;
                if (bIsReadOnly)
                {
                    //file was marked as read-only before trying delete.
                    fi.Attributes = System.IO.FileAttributes.ReadOnly;
                }
                return false;
            }
            finally
            {
                fi = null;
            }
        }

        public static string GetMosaiqueTempPath()
        {
            string sMosTempPath = System.IO.Path.GetTempPath();
            if (sMosTempPath.EndsWith(@"\") == false)
            { sMosTempPath += @"\"; }
            sMosTempPath += @"EPM\";
            if (System.IO.Directory.Exists(sMosTempPath) == false)
            {
                System.IO.Directory.CreateDirectory(sMosTempPath);
            }
            return sMosTempPath;
        }
    }
}
