using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RecoEngine_DataLayer;
using System.Collections;
namespace RecoEngine_BI
{
    public class clsExport
    {
       //public bool fnInsertExport(bool bIsControlGroup, string strBaseCustomers, bool bIsFixedCustomers, bool bIsInsertintoDB, int iprojectId, string Rank, ref DataTable dtfile)
       // {
       //     try
       //     {

       //         if (bIsInsertintoDB)
       //         {
       //             string strSql = "";
       //             strSql = "Delete from Export";
       //             if (Common.iDBType == (int)Enums.DBType.Oracle)
       //             {
       //                 ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
       //             }
       //             else
       //             {
       //                 ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
       //             }
       //             DataTable dt = new DataTable();

       //             strSql = " Insert Into  Export(PROJECT_ID,CAMPAIGN_ID,CUSTOMER,CG) ";
       //             strSql += " Select C.PROJECT_ID,P." + Rank + ",P.CUSTOMER,'N' from PRIORITIZED_TEMP P Left Join";
       //             strSql += " CAMPAIGNS  C ON C.CAMPAIGN_ID=P." + Rank + "  Where C.PROJECT_ID= " + iprojectId;
       //             if (Common.iDBType == (int)Enums.DBType.Oracle)
       //             {
       //                 ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
       //             }
       //             else
       //             {
       //                 ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
       //             }

       //             strSql = " Select count(1) from Export";
       //             int iCount = 0;
       //             if (Common.iDBType == (int)Enums.DBType.Oracle)
       //             {
       //                 iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
       //             }
       //             if (iCount > 0)
       //             {
       //                 if (bIsControlGroup)
       //                 {

       //                     strSql = "  Select " + Rank + " as CampaignID,Count(Customer) from PRIORITIZED_TEMP Group by " + Rank;
       //                     if (Common.iDBType == (int)Enums.DBType.Oracle)
       //                     {
       //                         dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
       //                     }
       //                     else
       //                     {
       //                         dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
       //                     }

       //                     foreach (DataRow dr in dt.Rows)
       //                     {
       //                         strSql = " Update Export Set CG ='Y' where customer  in  ";
       //                         strSql += " (SELECT CUSTOMER FROM (SELECT  CUSTOMER FROM PRIORITIZED_TEMP P";
       //                         strSql += " WHERE " + Rank + "=" + dr["CampaignID"] + " ORDER BY DBMS_RANDOM.RANDOM)";
       //                         if (!bIsFixedCustomers)
       //                             strSql += " WHERE ROWNUM<=" + Convert.ToDouble(strBaseCustomers) * Convert.ToInt16(dr["Count(Customer)"]) / 100 + ")";
       //                         else
       //                             strSql += " WHERE ROWNUM<=" + strBaseCustomers + ")";

       //                         if (Common.iDBType == (int)Enums.DBType.Oracle)
       //                         {
       //                             ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
       //                         }
       //                         else
       //                         {
       //                             ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
       //                         }

       //                     }
       //                 }

       //             }
       //             else
       //             {
       //                 return false;
       //             }
       //         }


       //     }
       //     catch (Exception ex)
       //     {
       //         throw ex;
       //     }

       // }
        public DataTable fnGetCountValues(int iProjectID, string strTabName)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSql = "";
                strSql += " Select T.Customers,S.Segment,O.Offers,C.Campaigns from";
                strSql += " (SELECT Count(Distinct Customer)as Customers from " + strTabName + " )T,";
                strSql += " (SELECT Count(1) as Segment from OPPORTUNITY where PROJECT_ID=" + iProjectID + " AND ISACTIVE=1)S,";
                strSql += " (SELECT Count(1)as Offers  from offers where PROJECT_ID=" + iProjectID + " AND ISACTIVE=1)O,";
                strSql += " (SELECT Count(1) as Campaigns from Campaigns where PROJECT_ID=" + iProjectID + " AND ISACTIVE=1)C ";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
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
        public DataTable fnGetExport()
        {

            try
            {

                DataTable dt = new DataTable();
                string strSql = "";
                strSql += "select * from Export";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
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
        public DataTable fnGetCampaigns(int iProjectId)
        {
            string strSql = "Select ELIGIBILITY,PROJECT_ID,SEGMENT_TYPE,SEGMENT_DESCRIPTION,CAMPAIGN_ID FROM CAMPAIGNS WHERE  ISACTIVE=1 AND PROJECT_ID=" + iProjectId;
            DataTable dt = new DataTable();
            if (Common.iDBType == (int)Enums.DBType.Oracle)
            {

                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
            }
            else
            {
                dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
            }
            return dt;
        }
        public bool fnInsertExportSettings(char bIsControlGroup, string strBaseCustomers, char bIsFixedCustomers, char bIsInsertintoDB, int iprojectId, string Rank,string MinimumCount, string MaximumCount)
        {
            string strSql = "";
            strSql = "Select Count(1) from EXPORT_SETTINGS WHERE PROJECT_ID=" + iprojectId;

            int iCount = 0;
            if (Common.iDBType == (int)Enums.DBType.Oracle)
            {
                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
            }
            if (iCount > 0)
            {
                strSql = "DELETE from EXPORT_SETTINGS WHERE PROJECT_ID=" + iprojectId;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                }

            }


            strSql = "INSERT INTO EXPORT_SETTINGS (PROJECT_ID,RANKING,EXPORT_FILE,ISFIXEDCUSTOMER,BASECUSTOMERS,ISCONTROLGROUP,MINLIMIT,MAXLIMIT) VALUES (" + iprojectId + ",'" + Rank + "',";
            strSql += "'" + bIsInsertintoDB + "','" + bIsFixedCustomers + "','" + strBaseCustomers + "','" + bIsControlGroup + "','"+MinimumCount+"','"+MaximumCount+"')";
            if (Common.iDBType == (int)Enums.DBType.Oracle)
            {
                ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
            }



            return true;
        }
        public DataTable fnSelectExportSettings(int iProjectId)
        {
            string strSql = "Select * from Export_Settings where Project_Id=" + iProjectId;
            DataTable dt = new DataTable();
            if (Common.iDBType == (int)Enums.DBType.Oracle)
            {
                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
            }
            return dt;

        }
        public DataTable fnExportToFile(bool bIsControlGroup, bool bIsFixedCustomers, string strBaseCustomer, string MaxLimit, string MinLimit, int iprojectId, int iCampaignId,string strRank, ref DataTable dtRandom)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtrndm = new DataTable();
                bool Ismax = false;
                string strSql = "";

                strSql = " Select PROJECTID,Customer,source as Ranking ,Val as CampaignId,'Y' AS CG From ";
                strSql += "(select PROJECTID,Customer,source,VAL from  (SELECT CUSTOMER,PROJECTID, ";
                strSql += "CASE WHEN TRIM(PR_RANK1)='NO CAMPAIGN' THEN '0' ELSE PR_RANK1 END as PR_RANK1, ";
                strSql += "CASE WHEN TRIM(PR_RANK2)='NO CAMPAIGN' THEN '0' ELSE PR_RANK2 END as PR_RANK2,";
                strSql += "CASE WHEN TRIM(PR_RANK3)='NO CAMPAIGN' THEN '0' ELSE PR_RANK3 END as PR_RANK3,";
                strSql += "CASE WHEN TRIM(PR_RANK4)='NO CAMPAIGN' THEN '0' ELSE PR_RANK4 END as PR_RANK4 ";
                strSql += "FROM PRIORITIZED_TEMP WHERE PROJECTID=" + iprojectId + " And " + strRank + ") UNPIVOT ";
                strSql += " ( VAL FOR( SOURCE ) IN (PR_RANK1 AS 'RANKING1',PR_RANK2 AS 'RANKING2', PR_RANK3 AS 'RANKING3', PR_RANK4 AS 'RANKING4')))  where Val=" + iCampaignId;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
                else
                {
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }


                if (MinLimit != "")

                    if (dt.Rows.Count < Convert.ToDouble(MinLimit))
                    {
                        dt = new DataTable();
                    }
                if (MaxLimit != "")
                {
                    if (dt.Rows.Count > Convert.ToDouble(MaxLimit))
                    {
                        strSql = "Select PROJECTID,Customer,Ranking ,CampaignId,'Y' as CG From (Select PROJECTID,Customer,source as Ranking ,Val as CampaignId From ";
                        strSql += "(select PROJECTID,Customer, source,VAL from  (SELECT * FROM PRIORITIZED_TEMP WHERE PROJECTID=" + iprojectId + " And " + strRank + ") UNPIVOT ";
                        strSql += " ( VAL FOR( SOURCE ) IN (PR_RANK1 AS 'RANKING1',PR_RANK2 AS 'RANKING2', PR_RANK3 AS 'RANKING3', PR_RANK4 AS 'RANKING4')))  where Val=" + iCampaignId;
                        strSql += " ORDER BY DBMS_RANDOM.RANDOM) WHERE rownum <=" + Convert.ToDouble(MaxLimit);

                        if (Common.iDBType == (int)Enums.DBType.Oracle)
                        {
                            dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                            Ismax = true;
                        }
                        else
                        {
                            dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                        }
                    }
                }
                if (bIsControlGroup && dt.Rows.Count>0)
                {
                    strSql = "";
                    if (Ismax)
                        strSql += " Select Customer From (";
                    strSql += " Select Customer From ( Select PROJECTID,Customer,source as Ranking ,Val as CampaignId From ";
                    strSql += "(select PROJECTID,Customer,source,VAL from  (SELECT * FROM PRIORITIZED_TEMP WHERE PROJECTID=" + iprojectId + " And " + strRank + ") UNPIVOT ";
                    strSql += " ( VAL FOR( SOURCE ) IN (PR_RANK1 AS 'RANKING1',PR_RANK2 AS 'RANKING2', PR_RANK3 AS 'RANKING3', PR_RANK4 AS 'RANKING4')))  where Val=" + iCampaignId;
                    if (Ismax)
                        strSql += " ORDER BY DBMS_RANDOM.RANDOM) WHERE rownum <=" + Convert.ToDouble(MaxLimit) ;
                    if (bIsFixedCustomers)
                    {
                        strSql += " ORDER BY DBMS_RANDOM.RANDOM) WHERE rownum <=" + Convert.ToDouble(strBaseCustomer);
                    }
                    else
                    {
                        if (Ismax)
                            strSql += " ORDER BY DBMS_RANDOM.RANDOM) WHERE rownum <=" + Convert.ToDouble(strBaseCustomer) * dt.Rows.Count / 100;
                        else
                            
                            strSql += " ORDER BY DBMS_RANDOM.RANDOM) WHERE rownum <=" + Convert.ToDouble(strBaseCustomer) * dt.Rows.Count / 100;
                    }
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        dtrndm = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                    }
                    else
                    {
                        dtrndm = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                    }



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