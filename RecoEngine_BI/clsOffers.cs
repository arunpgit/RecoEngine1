using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RecoEngine_DataLayer;
using System.Collections;

namespace RecoEngine_BI
{
    public class clsOffers 
    {
        public DataTable fnGetOffers(int iProjectId)
        {
            try
            {

                DataTable dt;
                string strSql = "";

                strSql = "Select O.OFFER_ID,O.PROJECT_ID,O.NAME,O.CODE,O.DESCRIPTION,O.LEVEL1,O.LEVEL2,O.LEVEL3,O.LEVEL4,O.CREATEDDATE,O.ISACTIVE as ISACTIVEID,";
                strSql += "CASE WHEN O.ISACTIVE=1 THEN 'YES' ELSE 'NO' END as ISACTIVE, ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    strSql += " U.First_name || ' ' || U.last_name as UName ";
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    strSql += " ,U.First_name + ' ' + U.last_name as UName ";
                strSql += " ,'' as Flag From OFFERS O Left join Users U on U.USER_ID=O.CREATEDBY ";
                if (iProjectId != 0)
                    strSql += " Where O.Project_Id=" + iProjectId;

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet fnGetLevels()
        {
            try
            {

                DataSet ds = new DataSet();

                DataTable dt = new DataTable("Level1");
                string strSql = "";

                strSql = "Select * from PRODUCT_LOOKUP ";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                ds.Tables.Add(dt);

                dt = new DataTable("Level2");
                 strSql = "";

                strSql = "Select * from REVENUE_LOOKUP ";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                ds.Tables.Add(dt);


                dt = new DataTable("Level3");


                strSql = "Select * from BUSINESS_AXE_LOOKUP ";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                ds.Tables.Add(dt);

                dt = new DataTable("Level4");

                strSql = "Select * from CATEGORY_LOOKUP ";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                ds.Tables.Add(dt);

                dt = new DataTable("Level5");

                strSql = "Select * from TYPE_LOOKUP ";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                ds.Tables.Add(dt);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool fnDeleteOffers(string strOfferId)
        {
            try
            {

                string strSql = "DELETE from CAMPAIGNS WHERE  OFFER_ID =" + strOfferId;
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                strSql = "DELETE FROM OFFERS WHERE  OFFER_ID = " + strOfferId;
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool fnActiveOffers(string strOfferId)
        {
            try
            {
                string[] str = strOfferId.Split(';');
                if (str.Length > 0)
                {

                    string strSql = "UPDATE OFFERS SET  ISACTIVE = " + str[1] + " WHERE  OFFER_ID = " + str[0];
                     ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    if (str[1] == "0")
                    {  
                        strSql = "UPDATE CAMPAIGNS SET  ISACTIVE = " + str[1] + " WHERE  OFFER_ID = " + str[0];
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                         strSql = " DELETE FROM PRIORITIZED_TEMP  WHERE CAMPAIGNID=";
                         strSql += "(SELECT  nvl(Sum(CAMPAIGN_ID),0) FROM CAMPAIGNS WHERE  OFFER_ID = " + str[0]+")";
                         ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool fnCheckOffersDependencies(string strOfferId)
        {
            try
            {
                if (int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select nvl(count(1),0) from CAMPAIGNS WHERE  OFFER_ID = " + strOfferId)) > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int fnSaveOffers(int iOfferId, string strName, string strCode, string strDesc, string strLevel1, string strLevel2, string strLevel3, string strLevel4, int strLoginUserId, int iProjectId, int iIsActive)
        {
            try
            {
                string strSql = "";
                if (iOfferId == 0)
                {
                    strSql = "INSERT INTO OFFERS(PROJECT_ID,NAME,CODE,DESCRIPTION,LEVEL1,LEVEL2,LEVEL3,LEVEL4,CREATEDBY,CREATEDDATE,ISACTIVE) Values (";
                    strSql += iProjectId + ",'" + strName.Replace("'", "''") + "','" + strCode.Replace("'", "''") + "','" + strDesc.Replace("'", "''") + "','" + strLevel1.Replace("'", "''") + "',";
                    strSql += "'" + strLevel2.Replace("'", "''") + "','" + strLevel3.Replace("'", "''") + "','" + strLevel4.Replace("'", "''") + "'," + strLoginUserId + ",";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {

                        strSql += " sysdate ," + iIsActive + ")";
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                        iOfferId = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select max(OFFER_ID) from OFFERS"));

                    }
                    else
                    {
                        strSql += "getdate() ," + iIsActive + ")";
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        iOfferId = int.Parse(((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select max(OFFER_ID) from OFFERS"));

                    }
                }
                else
                {
                    strSql = "UPDATE   OFFERS SET NAME='" + strName.Replace("'", "''") + "',CODE='" + strCode.Replace("'", "''") + "',DESCRIPTION='" + strDesc.Replace("'", "''") + "',";
                    strSql += " LEVEL1 = '" + strLevel1.Replace("'", "''") + "',LEVEL2 ='" + strLevel2.Replace("'", "''") + "',LEVEL3 ='" + strLevel3.Replace("'", "''") + "',";
                    strSql += "LEVEL4 ='" + strLevel4.Replace("'", "''") + "',ISACTIVE=" + iIsActive + " WHERE OFFER_ID = " + iOfferId;
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }
                    else
                    {
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }
                }

                return iOfferId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
