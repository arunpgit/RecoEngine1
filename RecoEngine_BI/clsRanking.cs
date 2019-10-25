using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;
using RecoEngine_DataLayer;

namespace RecoEngine_BI
{
    public class clsRanking
    {

        public string fnOpportunitiesRnkng(int iProjectid)
        {
            DataTable dt;
            string strSql = "";
            strSql = "Select OPP_NAME from Opportunity  where  Project_id= " + iProjectid;
            if (Common.iDBType == (int)Enums.DBType.Oracle)
                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
            else
                dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

            //OraDBManager con = new OraDBManager(((DBManager)Common.dbMgr).GetConnectionString);
            //DataContext dc = new DataContext(((DBManager)Common.dbMgr).GetConnectionString);

            //DataTable dtTemp=from dt1 in dc.GetTable<DataTable>() 
            //where dt1.TableName.Equals("Opportunity")

            //DataTable dtTemp= 

            strSql = " Insert into CUSTOMER_PNTL (OPP_NAME,OPP_PNTL,Opp_Status)";
            foreach (DataRow dr in dt.Rows)
            {
                strSql += " Select  '" + dr[0].ToString() + "'  ," + dr[0].ToString() + "_Pntl , " + " CASE  When " + dr[0].ToString() + "_STATUS= 'NON_USER' Then 'X-SELL-" + dr[0].ToString() + "'";
                strSql += " When " + dr[0].ToString() + "_STATUS= 'DROPPER' Then 'MITIGATE-" +dr[0].ToString()+"'" ;
                strSql += " When " + dr[0].ToString() + "_STATUS= 'STOPPER' Then 'REVIVE-" + dr[0].ToString() + "'";
                strSql += " When " + dr[0].ToString() + "_STATUS= 'GROWER' Then 'NO ACTION-" + dr[0].ToString() + "'";
                strSql += " When " + dr[0].ToString() + "_STATUS= 'FLAT' Then 'UP-SELL-" + dr[0].ToString() + "'";
                strSql += " When " + dr[0].ToString() + "_STATUS= 'NEW_USER' Then 'NO ACTION' END   FROM TRE_OPPORTUNITY WHERE CUSTOMER=cust_rec.customer ";
                strSql += " Union";
            }
              return strSql = strSql.Remove(strSql.Length -5, 5) + ";";
        }

        public void fnCustomRanking(int iProjectid,string strRank1,string strRank2,string strRank3,string strRank4)
        {
            try
            {
                string strSql = "";
                strSql = "Delete from TRE_RANKING ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                strSql = " DECLARE ";
                strSql += " Rank1 Customer_Pntl.Opp_Pntl%TYPE;";
                strSql += " Rank2 Customer_Pntl.Opp_Pntl%TYPE;";
                strSql += " Rank3 Customer_Pntl.Opp_Pntl%TYPE;";
                strSql += " Rank4 Customer_Pntl.Opp_Pntl%TYPE;";
                strSql += " Rank1_Name Customer_Pntl.Opp_Name%TYPE ;";
                strSql += " Rank2_Name Customer_Pntl.Opp_Name%TYPE ;";
                strSql += " Rank3_Name Customer_Pntl.Opp_Name%TYPE ;";
                strSql += " Rank4_Name Customer_Pntl.Opp_Name%TYPE ;";
                strSql += " Rank1_Action Customer_Pntl.Opp_Status%TYPE;";
                strSql += " Rank2_Action Customer_Pntl.Opp_Status%TYPE;";
                strSql += " Rank3_Action Customer_Pntl.Opp_Status%TYPE;";
                strSql += " Rank4_Action Customer_Pntl.Opp_Status%TYPE;";
                strSql += " customer  Tre_Opportunity.CUSTOMER%TYPE;";
                strSql += " Counter integer :=0;";
                strSql += " CURSOR ttl_customer IS ";
                strSql += " SELECT CUSTOMER FROM Tre_Opportunity; ";
                strSql += " BEGIN ";
                strSql += " FOR cust_rec in ttl_customer";
                strSql += " LOOP";
                strSql += fnOpportunitiesRnkng(iProjectid);
                strSql += " Rank1_Name  :='" + strRank1 + "';";
                strSql += " Rank2_Name  :='" + strRank2 + "';";
                strSql += " Rank3_Name  :='" + strRank3 + "';";
                strSql += " Rank4_Name  :='" + strRank4 + "';";
                strSql += " Customrank_Selection(cust_rec.customer,Rank1,Rank2,Rank3,Rank4,Counter,Rank1_Name,Rank2_Name,Rank3_Name,Rank4_Name,Rank1_Action,Rank2_Action,Rank3_Action,Rank4_Action);";
                strSql += " Delete  from CUSTOMER_PNTL; ";
                strSql += " end LOOP;";
                strSql += " END;";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void fnPotentialRanking(int iProjectid)
        {
            try
            {
                string strSql = "";
                strSql = "Delete from TRE_RANKING ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                strSql  = " DECLARE ";
                strSql += " Rank1 Customer_Pntl.Opp_Pntl%TYPE;";
                strSql += " Rank2 Customer_Pntl.Opp_Pntl%TYPE;";
                strSql += " Rank3 Customer_Pntl.Opp_Pntl%TYPE;";
                strSql += " Rank4 Customer_Pntl.Opp_Pntl%TYPE;";
                strSql += " Rank1_Name Customer_Pntl.Opp_Name%TYPE ;";
                strSql += " Rank2_Name Customer_Pntl.Opp_Name%TYPE ;";
                strSql += " Rank3_Name Customer_Pntl.Opp_Name%TYPE ;";
                strSql += " Rank4_Name Customer_Pntl.Opp_Name%TYPE ;";
                strSql += " Rank1_Action Customer_Pntl.Opp_Status%TYPE;";
                strSql += " Rank2_Action Customer_Pntl.Opp_Status%TYPE;";
                strSql += " Rank3_Action Customer_Pntl.Opp_Status%TYPE;";
                strSql += " Rank4_Action Customer_Pntl.Opp_Status%TYPE;";
                strSql += " customer  Tre_Opportunity.CUSTOMER%TYPE;";
                strSql += " CURSOR ttl_customer IS ";
                strSql += " SELECT CUSTOMER FROM Tre_Opportunity; ";
                strSql += " BEGIN ";
                strSql += " FOR cust_rec in ttl_customer";
                strSql += " LOOP ";
                strSql += fnOpportunitiesRnkng(iProjectid);
                strSql += " Rank_Selection(cust_rec.customer,Rank1,Rank2,Rank3,Rank4,Rank1_Name,Rank2_Name,Rank3_Name,Rank4_Name,Rank1_Action,Rank2_Action,Rank3_Action,Rank4_Action);";
                strSql += " Delete  from CUSTOMER_PNTL; ";
                strSql += " end LOOP;";
                strSql += " END;";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

        public void fnCustomRankingFrmExport(int iProjectid, string strRank1, string strRank2, string strRank3, string strRank4)
        {
            try
            {
                string strSql = "";
                DataTable dtTemp1 = null;
                DataTable dtTemp2 = null;
                string strColPntl = "";
                string strColStatus = "";
                string strColumns_PNTL = "";
                string strColumns_STATUS = "";
                string strCustomCols = "";
                string str_CustomCols = "";

                strSql = "Select OPP_NAME from OPPORTUNITY";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dtTemp1 = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    dtTemp1 = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                foreach (DataRow dr in dtTemp1.Rows)
                {
                    strColPntl = strColPntl + dr.ItemArray[0].ToString() + "_PNTL as '" + dr.ItemArray[0].ToString() + "' , ";
                    strColStatus = strColStatus + dr.ItemArray[0].ToString() + "_STATUS as '" + dr.ItemArray[0].ToString() + "' , ";
                }

                strSql = "Select RANK1, RANK2, RANK3, RANK4 from OPPORTUNITY_RANKING";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dtTemp2 = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    dtTemp2 = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                foreach (DataRow dr in dtTemp2.Rows)
                {
                    strCustomCols = "'" + dr.ItemArray[0].ToString() + "'" + " , " + "'" + dr.ItemArray[1].ToString() + "'" + " , " + "'" + dr.ItemArray[2].ToString() + "'" + " ," + "'" +
                                                                                   dr.ItemArray[3].ToString() + "'";
                }

                strColumns_PNTL = strColPntl.Replace("'", "''");
                strColumns_STATUS = strColStatus.Replace("'", "''");
                str_CustomCols = strCustomCols.Replace("'", "''");

                strColumns_PNTL = strColumns_PNTL.Remove((strColumns_PNTL.Length) - 2);
                strColumns_STATUS = strColumns_STATUS.Remove((strColumns_STATUS.Length) - 2);

                strSql = " DECLARE ";
                strSql += " strModColumnsPNTL string(20000);";
                strSql += " strModColumnsSTATUS string(20000);";
                strSql += " strCustomCol string(20000);";
                strSql += " BEGIN";
                strSql += " strModColumnsPNTL := '" + strColumns_PNTL + "';";
                strSql += " strModColumnsSTATUS := '" + strColumns_STATUS + "';";
                strSql += " strCustomCol := '" + str_CustomCols + "';";
                strSql += " TRE_CUSTOM_RANKING(strModColumnsPNTL, strModColumnsSTATUS, strCustomCol);";
                strSql += " END;";

                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                //((OraDBManager)Common.dbMgr).CommitTrans();
                //((OraDBManager)Common.dbMgr).BeginTrans();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void fnPotentialRankingFrmExport(int iProjectid)
        {
            try
            {
                string strSql = "";
                string strColPntl = "";
                string strColStatus = "";
                string strColumns_PNTL = "";
                string strColumns_STATUS = "";
                DataTable dtTemp=null;
                //strSql = "Delete from TRE_RANKING ";
                //if (Common.iDBType == (int)Enums.DBType.Oracle)
                //    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                //else if (Common.iDBType == (int)Enums.DBType.SQl)
                //    ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                strSql = "Select OPP_NAME from OPPORTUNITY";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                  dtTemp = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                  dtTemp =  ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                foreach (DataRow dr in dtTemp.Rows)
                {
                    strColPntl = strColPntl + dr.ItemArray[0].ToString() + "_PNTL as '" + dr.ItemArray[0].ToString() + "' , ";
                    strColStatus = strColStatus + dr.ItemArray[0].ToString() + "_STATUS as '" + dr.ItemArray[0].ToString() + "' , ";
                }

                strColumns_PNTL = strColPntl.Replace("'","''");
                strColumns_STATUS = strColStatus.Replace("'", "''");

                strColumns_PNTL = strColumns_PNTL.Remove((strColumns_PNTL.Length) - 2);
                strColumns_STATUS = strColumns_STATUS.Remove((strColumns_STATUS.Length) - 2);

                strSql = " DECLARE ";
                strSql += " strModColumnsPNTL string(20000);";
                strSql += " strModColumnsSTATUS string(20000);";
                strSql += " BEGIN";
                strSql += " strModColumnsPNTL := '" + strColumns_PNTL + "';";
                strSql += " strModColumnsSTATUS := '" + strColumns_STATUS + "';";
                strSql += " TRE_POTENTIAL_RANKING(strModColumnsPNTL, strModColumnsSTATUS);";
                strSql += " END;";

                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                //strSql = " DECLARE ";
                //strSql += " Rank1 Customer_Pntl.Opp_Pntl%TYPE;";
                //strSql += " Rank2 Customer_Pntl.Opp_Pntl%TYPE;";
                //strSql += " Rank3 Customer_Pntl.Opp_Pntl%TYPE;";
                //strSql += " Rank4 Customer_Pntl.Opp_Pntl%TYPE;";
                //strSql += " Rank1_Name Customer_Pntl.Opp_Name%TYPE ;";
                //strSql += " Rank2_Name Customer_Pntl.Opp_Name%TYPE ;";
                //strSql += " Rank3_Name Customer_Pntl.Opp_Name%TYPE ;";
                //strSql += " Rank4_Name Customer_Pntl.Opp_Name%TYPE ;";
                //strSql += " Rank1_Action Customer_Pntl.Opp_Status%TYPE;";
                //strSql += " Rank2_Action Customer_Pntl.Opp_Status%TYPE;";
                //strSql += " Rank3_Action Customer_Pntl.Opp_Status%TYPE;";
                //strSql += " Rank4_Action Customer_Pntl.Opp_Status%TYPE;";
                //strSql += " customer  Tre_Opportunity.CUSTOMER%TYPE;";
                //strSql += " CURSOR ttl_customer IS ";
                //strSql += " SELECT CUSTOMER FROM Tre_OpportunityExport; ";
                //strSql += " BEGIN ";
                //strSql += " FOR cust_rec in ttl_customer";
                //strSql += " LOOP ";
                //strSql += fnOpportunitiesRnkngfrmExport(iProjectid);
                //strSql += " Rank_Selection(cust_rec.customer,Rank1,Rank2,Rank3,Rank4,Rank1_Name,Rank2_Name,Rank3_Name,Rank4_Name,Rank1_Action,Rank2_Action,Rank3_Action,Rank4_Action);";
                //strSql += " Delete  from CUSTOMER_PNTL; ";
                //strSql += " end LOOP;";
                //strSql += " END;";
                //if (Common.iDBType == (int)Enums.DBType.Oracle)
                //    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                //((OraDBManager)Common.dbMgr).CommitTrans();
                //((OraDBManager)Common.dbMgr).BeginTrans();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string fnOpportunitiesRnkngfrmExport(int iProjectid)
        {
            DataTable dt;
            string strSql = "";
            // The below is commented bcz as running on procedure we are not checking isonmain
            //strSql = "Select OPP_NAME from Opportunity where ISONMAIN=1 AND Project_id= " + iProjectid;
            strSql = "Select OPP_NAME from Opportunity where Project_id= " + iProjectid;
            if (Common.iDBType == (int)Enums.DBType.Oracle)
                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
            else
                dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
            strSql = " Insert into CUSTOMER_PNTL (OPP_NAME,OPP_PNTL,Opp_Status)";
            foreach (DataRow dr in dt.Rows)
            {
                strSql += " Select  '" + dr[0].ToString() + "'  ," + dr[0].ToString() + "_Pntl , " + " CASE  When " + dr[0].ToString() + "_STATUS= 'NON_USER' Then 'X-SELL-" + dr[0].ToString() + "'";
                strSql += " When " + dr[0].ToString() + "_STATUS= 'DROPPER' Then 'MITIGATE-" + dr[0].ToString() + "'";
                strSql += " When " + dr[0].ToString() + "_STATUS= 'STOPPER' Then 'REVIVE-" + dr[0].ToString() + "'";
                strSql += " When " + dr[0].ToString() + "_STATUS= 'GROWER' Then 'NO ACTION-" + dr[0].ToString() + "'";
                strSql += " When " + dr[0].ToString() + "_STATUS= 'FLAT' Then 'UP-SELL-" + dr[0].ToString() + "'";
                strSql += " When " + dr[0].ToString() + "_STATUS= 'NEW_USER' Then 'NO ACTION' END   FROM TRE_OPPORTUNITYEXPORT WHERE CUSTOMER=cust_rec.customer ";
                strSql += " Union";
            }
            return strSql = strSql.Remove(strSql.Length - 5, 5) + ";";

        }
        public DataTable  fnRankingcriteria(int iProjectId )
        {
        string strSql="";  
            DataTable dt;
         strSql+="Select TYPE,RANK1,RANK2,RANK3,RANK4 from OPPORTUNITY_RANKING where PROJECT_ID=" + iProjectId;
         if (Common.iDBType == (int)Enums.DBType.Oracle)
             dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
         else
             dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
         return dt;
        }

        public void fnMainRanking(int iProjectId)
        {

            DataTable dt = fnRankingcriteria(iProjectId);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Type"].ToString() == "Custom")
                {

                    fnCustomRanking(iProjectId, dt.Rows[0]["Rank1"].ToString(), dt.Rows[0]["Rank2"].ToString(), dt.Rows[0]["Rank3"].ToString(), dt.Rows[0]["Rank4"].ToString());
                }
                else 
                {
                    fnPotentialRanking(iProjectId);
                }
            }
        
        }
        public void fnMainRankingfrmExport(int iProjectId)
        {

            DataTable dt = fnRankingcriteria(iProjectId);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Type"].ToString() == "Custom")
                {
                    fnCustomRankingFrmExport(iProjectId, dt.Rows[0]["Rank1"].ToString(), dt.Rows[0]["Rank2"].ToString(), dt.Rows[0]["Rank3"].ToString(), dt.Rows[0]["Rank4"].ToString());
                    //fnCustomRanking(iProjectId, dt.Rows[0]["Rank1"].ToString(), dt.Rows[0]["Rank2"].ToString(), dt.Rows[0]["Rank3"].ToString(), dt.Rows[0]["Rank4"].ToString());
                }
                else
                {
                    fnPotentialRankingFrmExport (iProjectId);
                }
            }

        }
        public bool fnCheckOpportunityExists(int iProjectId)
        {
            DataTable dt;
            string strSql = "";
            strSql = "Select OPP_NAME from Opportunity  where ISONMAIN=0 and Project_id= " + iProjectId;
            if (Common.iDBType == (int)Enums.DBType.Oracle)
                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
            else
                dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
            if (dt.Rows.Count > 0)
                return true;
            else
              return false;
        
        }

    }
}



