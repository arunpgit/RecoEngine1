using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RecoEngine_DataLayer;
using System.Data;

namespace RecoEngine_BI
{
    public class clsUsers
    {
        public bool fnCheckUserName(string strUserName, ref bool bIsWindowsPassword)
        {
            bool bIsAutologin = false;
            bool bIsActive = false;
            bool bIsWindowsPWd = bIsWindowsPassword;
            return fnCheckUserName(strUserName, ref bIsAutologin, ref bIsWindowsPassword, ref bIsActive);
        }
        public bool fnCheckUserName(string strUserName, ref bool bIsAutologin, ref bool bIsWindowsPassword, ref bool bIsActive)
        {
            try
            {

                DataSet ds = new DataSet();
                string strSql = "select * from users where username='" + strUserName + "'";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ds = ((OraDBManager)Common.dbMgr).ExecuteDataSet(CommandType.Text, strSql);
                else
                    ds = ((DBManager)Common.dbMgr).ExecuteDataSet(CommandType.Text, strSql);



                if (ds.Tables[0].Rows.Count > 0)
                {
                    if ((ds.Tables[0].Rows[0]["UseWindowsPassword"] != null) && (ds.Tables[0].Rows[0]["UseWindowsPassword"].ToString() != ""))
                        bIsWindowsPassword = Convert.ToBoolean(ds.Tables[0].Rows[0]["UseWindowsPassword"]);

                    if ((ds.Tables[0].Rows[0]["AutoLogin"] != null) && (ds.Tables[0].Rows[0]["AutoLogin"].ToString() != ""))
                        bIsAutologin = Convert.ToBoolean(ds.Tables[0].Rows[0]["AutoLogin"]);


                    if ((ds.Tables[0].Rows[0]["IsActive"] != null) && (ds.Tables[0].Rows[0]["IsActive"].ToString() != ""))
                        bIsActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsActive"]);


                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet fnCheckUser(string strUserName)
        {
            try
            {

                DataSet ds = new DataSet();
                string strSql = "select * from users where username='" + strUserName + "'";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ds = ((OraDBManager)Common.dbMgr).ExecuteDataSet(CommandType.Text, strSql);
                else
                    ds = ((DBManager)Common.dbMgr).ExecuteDataSet(CommandType.Text, strSql);

                return ds;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public DataTable fnGetUsersDetails()
        {
            try
            {

                DataTable dt = new DataTable();
                //string strSql = "select * from PROJECTS ";

                string strSql = "Select A.User_ID, A.FIRST_NAME,A.LAST_NAME,Case When A.USERTYPE_ID=" + (int)Enums.UserTypes.administrators + " Then 'Administrators'";
                strSql += " When A.USERTYPE_ID=" + (int)Enums.UserTypes.employees + " Then 'Employees' When A.USERTYPE_ID=" + (int)Enums.UserTypes.customers + " Then 'Customers' ";
                strSql += " When A.USERTYPE_ID=" + (int)Enums.UserTypes.managers + " Then 'Managers' When A.USERTYPE_ID=" + (int)Enums.UserTypes.operators + " Then 'Operators' Else 'Administrators' End as UserType";
                strSql += " ,A.USERNAME,A.EMAIL,Case When A.ISACTIVE=1 Then 'Yes' Else 'NO' End as IsActive,'' as Flag from Users A";


                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                {
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }

                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool fnDeleteUser(string strId)
        {
            try
            {
                string strSql = "Delete from Users where user_id=" + strId;


                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                else
                {
                    ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataTable fnGetUsersDetails(int iUserId)
        {
            try
            {

                DataTable dt = new DataTable();
                //string strSql = "select * from PROJECTS ";

                string strSql = "Select User_ID, FIRST_NAME,LAST_NAME,USERTYPE_ID,USERNAME,EMAIL,ISACTIVE from Users Where User_Id=" + iUserId;


                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                {
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }

                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void fnSaveUser(int iUserId, string strFName, string strLName, int iUserType, string strUserName, string strPassword, string strEmail, int iIsActive)
        {
            try
            {
                string strSql = "";
                if (iUserId == 0)
                {
                    strSql = "Insert into Users (FIRST_NAME,LAST_NAME,USERTYPE_ID,USERNAME,PASSWORD,EMAIL,ISACTIVE) values(";
                    strSql += "'" + strFName.Replace("'", "''") + "','" + strLName.Replace("'", "''") + "'," + iUserType + ",'" + strUserName.Replace("'", "''") + "',";
                    strSql += "'" + strPassword.Replace("'", "''") + "','" + strEmail.Replace("'", "''") + "'," + iIsActive + ")";
                }
                else
                {
                    strSql = "Update Users Set ";
                    strSql += "FIRST_NAME = '" + strFName.Replace("'", "''") + "',";
                    strSql += "LAST_NAME='" + strLName.Replace("'", "''") + "',";
                    strSql += "USERTYPE_ID=" + iUserType + ",";
                    // strSql += "PASSWORD='" + strPassword.Replace("'", "''") + "','";
                    strSql += "EMAIL='" + strEmail.Replace("'", "''") + "',";
                    strSql += "USERNAME='" + strUserName.Replace("'", "''") + "',";
                    strSql += "ISACTIVE=" + iIsActive + " Where USER_ID=" + iUserId;
                }


                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                else
                {
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public bool fnActiveUsers(string strUserId)
        {
            try
            {
                string[] str = strUserId.Split(';');
                if (str.Length > 0)
                {

                    string strSql = "UPDATE Users SET  ISACTIVE = " + str[1] + " WHERE  USER_ID = " + str[0];
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                   
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
    }
}
