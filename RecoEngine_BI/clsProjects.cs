using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RecoEngine_DataLayer;

namespace RecoEngine_BI
{
    public class clsProjects
    {
        public int fnGetOpportunityCount(int iProjectID,int iUserID, ref int iOfferCount)

        {
            try
            {
                string strSql = "Select count(1) from  OPPORTUNITY WHERE PROJECT_ID =" + iProjectID + " and CREATEDBY=" + iUserID;


                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iOfferCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select count(1) from  OFFERS WHERE ISACTIVE=1 AND PROJECT_ID =" + iProjectID));
                    return int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                else
                {
                    //iOfferCount = int.Parse(((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select count(1) from  OFFERS WHERE PROJECT_ID =" + iProjectID));
                    return int.Parse(((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public DataTable fnGetProjectDetails(int iUserId,int iProjectId = 0)
        {
            try
            {

                DataTable dt = new DataTable();
                //string strSql = "select * from PROJECTS ";

                string strSql = "Select P.Project_Id,P.Name,P.Description, P.CREATEDON ";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    strSql += " ,U.First_name || ' ' || U.last_name as UName ";
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    strSql += " ,U.First_name + ' ' + U.last_name as UName ";
                strSql += " From Projects P Left join Users U on U.USER_ID=P.CREATEDBY ";
                if (iUserId != 0)
                    strSql += " Where CREATEDBY=" + iUserId;

                if (iProjectId != 0)
                    strSql += " AND Project_Id=" + iProjectId ;
               
                    

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

        public void fnDeleteProject(int iProjectId)
        {
            try
            {
                string strSql = "";
                        
       //deleet recommendations of projects

                strSql += "Delete from Projects where Project_Id=" + iProjectId;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                else
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
              strSql = "DELETE FROM TRE_MAPPING WHERE PROJECTID=" + iProjectId;
              if (Common.iDBType == (int)Enums.DBType.Oracle)
                  ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
              else
                  ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);

              strSql = "DELETE FROM  TRE_Calculated_columns WHERE PROJECT_ID=" + iProjectId;
              if (Common.iDBType == (int)Enums.DBType.Oracle)
                  ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
              else
                  ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
              strSql = "DELETE FROM  Filter_Main WHERE PROJECT_ID=" + iProjectId;
              if (Common.iDBType == (int)Enums.DBType.Oracle)
                  ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
              else
                  ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
              strSql = "DELETE FROM OPPORTUNITY WHERE PROJECT_ID=" + iProjectId;
              if (Common.iDBType == (int)Enums.DBType.Oracle)
                  ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
              else
                  ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
              strSql = "DELETE FROM offers where project_id=" + iProjectId;
              if (Common.iDBType == (int)Enums.DBType.Oracle)
                  ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
              else
                  ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
              strSql = "DELETE FROM campaigns where project_id=" + iProjectId;
              if (Common.iDBType == (int)Enums.DBType.Oracle)
                  ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
              else
                  ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
              strSql = "DELETE FROM Export_Settings where project_id=" + iProjectId;

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                else
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void fnInsertProjectdtls(int iProjectId, string strPName, string strDescription, int iLoginUserID)
        {
            try
            {
                string strSql = "";
                string dateformat = "yyyy/mm/dd hh24:mi:ss";


                if (iProjectId == 0)
                {
                    strSql = "INSERT INTO PROJECTS (NAME, DESCRIPTION,CREATEDON,CREATEDBY) VALUES(";
                    strSql += "'" + strPName.Replace("'", "''") + "','" + strDescription + "',";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        strSql += " sysdate," + iLoginUserID + ")";
                    else
                        strSql += " Getdate()," + iLoginUserID + ")";
                }
                else
                {
                    strSql = "Update PROJECTS set name='" + strPName.Replace("'", "''") + "',";
                    strSql += " Description = '" + strDescription.Replace("'", "''") + "'";
                    strSql += " where Project_Id=" + iProjectId;
                }
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                else
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable fnGetProjectNames(int iUserId)
        {
            try
            {

                DataTable dt = new DataTable();
                //string strSql = "select * from PROJECTS ";

                string strSql = "Select P.Project_Id,P.Name ";

                strSql += " From Projects P Left join Users U on U.USER_ID=P.CREATEDBY ";
                if (iUserId != 0)
                    strSql += " Where CREATEDBY=" + iUserId;


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
    
    }
}
