using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RecoEngine_DataLayer;
using System.Collections;

namespace RecoEngine_BI
{
    public class clsCampaign
    {
        public void fnDelteCampaignRankings(int iProjectId, string strTabname)
        {
            try
            {
                string strSql = "";
                int iCount = 0;
                strSql = "Select Count(1) from Campaign_Ranking where ProjectId=" + iProjectId;
                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                if (iCount != 0)
                {

                    strSql = "Delete from CAMPAIGN_RANKING ";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }
                    else
                    {
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }
                }

                strSql = "INSERT INTO CAMPAIGN_RANKING(CUSTOMER,CAMPAIGN_RANKING1,CAMPAIGN_RANKING2,CAMPAIGN_RANKING3,CAMPAIGN_RANKING4,PROJECTID)  SELECT CUSTOMER,'No Campaign','No Campaign','No Campaign','No Campaign'," + iProjectId + " FROM TRE_RANKING";
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    strSql = "Select Eligibility,O.Opp_Name as Segment,Segment_Type,CAMPAIGN_ID from Campaigns C";
                    strSql += " Left Join Opportunity  O on   O.Opportunity_Id=C.Opportunity_Id Where C.Project_Id=" + iProjectId;
                    DataTable dt = new DataTable();
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                    else
                        dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {

                            fnSaveCampaignRankings(dr["Eligibility"].ToString(), iProjectId, dr["Segment_Type"].ToString() + "-" + dr["Segment"].ToString(), Convert.ToInt32(dr["CAMPAIGN_ID"]), strTabname);
                        }

                    

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public DataTable fnGetCampaignMapping(int iProjectId)
        {
            try
            {
                DataTable dt;
                string strSql = "";
                strSql = "  Select C.CAMPAIGN_ID,C.PROJECT_ID,C.NAME,C.CODE,C.OPPORTUNITY_ID,C.OFFER_ID,C.CHANNEL,C.DESCRIPTION,C.OPP_RANK,C.TAKE_UP_RATE,C.ELIGIBILITY,C.Segment_Type,C.SEGMENT_DESCRIPTION, ";
                strSql += " C.ACCOUNTS,C.TOTAL_POTENTIAL,  Case When C.ACCOUNTS=0 then 0 Else Round(C.TOTAL_POTENTIAL/C.ACCOUNTS) END AS Average,Case When C.ACCOUNTS=0 then 0 Else Round(C.TOTAL_POTENTIAL/C.ACCOUNTS) END AS Average1, C.CREATEDDATE,C.ISACTIVE as ISACTIVEID,";
                strSql += " CASE WHEN C.ISACTIVE=1 THEN 'YES' ELSE 'NO' END as ISACTIVE, ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    strSql += " U.First_name || ' ' || U.last_name as UName ";
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    strSql += " ,U.First_name + ' ' + U.last_name as UName ";
                strSql += " ,'' as Flag From CAMPAIGNS C Left join Users U on U.USER_ID=C.CREATEDBY ";
                if (iProjectId != 0)
                    strSql += " Where C.Project_Id=" + iProjectId;
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
        public DataSet fnGetSgmntOfferChnl(int iProjectId)
        {
            try
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable("Segment");
                string strSql = "";

                strSql = "Select * from Opportunity where ISACTIVE=1 and PROJECT_ID=" + iProjectId;

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                ds.Tables.Add(dt);

                dt = new DataTable("Offer");

                strSql = "Select * from Offers where ISACTIVE=1 and PROJECT_ID=" + iProjectId;

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                ds.Tables.Add(dt);

                dt = new DataTable("OPT_LOOKUP");
                strSql = "";

                strSql = "Select * from OPT_LOOKUP ";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                ds.Tables.Add(dt);

                dt = new DataTable("ACTION");
                strSql = "";

                strSql = "Select * from ACTION_LOOKUP ";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                ds.Tables.Add(dt);
                dt = new DataTable("Channel");

                strSql = "Select * from CHANNEL_LOOKUP ";

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
        public DataTable fnGetCampaignPtnl(string strOppName, string strEligibility, string strOppRankstatus, string strTabName)
        {
            try
            {


                DataTable dt;
                string strSql = "";
                strSql = "SELECT  ROUND(Sum(A." + strOppName + "_pntl),2) as TotalPotential  , Count(A.Customer)AS ACCOUNTS ";
                strSql += " FROM  TRE_OPPORTUNITYEXPORT A ";
                strSql += " LEFT JOIN  " + strTabName + "  D ON A.CUSTOMER=D.CUSTOMER AND A.WEEK=D.WEEK ";
                strSql += " LEFT JOIN  Tre_Ranking  R";
                strSql += " ON A.CUSTOMER=R.CUSTOMER";

                strSql += " WHERE ";

                if (strEligibility != "")
                strSql += strEligibility + " AND";
                strSql += " (RANK1_ACTION  IN ('" + strOppRankstatus + "')";
                strSql += " OR RANK2_ACTION IN ('" + strOppRankstatus + "')";
                strSql += " OR RANK3_ACTION IN ('" + strOppRankstatus + "')";
                strSql += " OR RANK4_ACTION IN ('" + strOppRankstatus + "'))";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int fnSaveCampaign(int iCampaignId, string strName, string strCode, string strDesc, string strOppName, string strOfferid, string strOppid, string strChnl, string strtakeUp, string strEligibilty, string strSegmentType, string strOppRankstatus, int iLoginUserId, int iProjectId, int iIsActive, string strTabName)
        {
            try
            {
                string strSql = "";

                if (iCampaignId == 0)
                {
                    strSql = "INSERT INTO CAMPAIGNS(PROJECT_ID,NAME,CODE,DESCRIPTION,OPPORTUNITY_ID,OFFER_ID,CHANNEL,SEGMENT_TYPE,TAKE_UP_RATE,ELIGIBILITY,ACCOUNTS,TOTAL_POTENTIAL,CREATEDBY,CREATEDDATE,ISACTIVE,SEGMENT_DESCRIPTION) ";
                    strSql += "SELECT " + iProjectId + ",'" + strName.Replace("'", "''") + "','" + strCode.Replace("'", "''") + "','" + strDesc.Replace("'", "''") + "',";
                    strSql += strOppid + "," + strOfferid + ",'" + strChnl.Replace("'", "''") + "','" + strSegmentType.Replace("'", "''") + "'," + strtakeUp + ",'" + strEligibilty.Replace("'", "''") + "',";
                    strSql += "Count(A.Customer),Sum(A." + strOppName + "_pntl)," + iLoginUserId.ToString() + ", ";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        strSql += " sysdate ," + iIsActive;
                       
                        strSql += ",'"+ strOppRankstatus.Replace("'", "''");
                        strSql += "'  FROM  TRE_OPPORTUNITYEXPORT A ";
                        strSql += " LEFT JOIN  " + strTabName + " D ON A.CUSTOMER=D.CUSTOMER AND A.WEEK=D.WEEK ";
                        strSql += " LEFT JOIN  Tre_Ranking  R";
                        strSql += " ON A.CUSTOMER=R.CUSTOMER";
                        strSql += " WHERE ";
                        if (strEligibilty != "")
                            strSql += strEligibilty + " AND";
                        strSql += " (RANK1_ACTION  IN ('" + strOppRankstatus + "')";
                        strSql += " OR RANK2_ACTION IN ('" + strOppRankstatus + "')";
                        strSql += " OR RANK3_ACTION IN ('" + strOppRankstatus + "')";
                        strSql += " OR RANK4_ACTION IN ('" + strOppRankstatus + "'))";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                iCampaignId = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select max(CAMPAIGN_ID) from CAMPAIGNS"));
                      //this is commented because as we need to do it from priortize
                        // fnSaveCampaignRankings(strEligibilty, iProjectId, strOppRankstatus, iCampaignId, strTabName);

                    }
                    else
                    {
                        strSql += "getdate() ," + iIsActive;
                        strSql += " FROM tre_opportunity  A  LEFT JOIN ( SELECT CUSTOMER FROM " + strTabName + "  WHERE " + strEligibilty + " and Week= (Select Max(week) from " + strTabName + " where year=year(getdate()))) ";
                        strSql += " B ON B.customer = A.customer";
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        iCampaignId = int.Parse(((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select max(CAMPAIGN_ID) from CAMPAIGNS"));

                    }
                }
                else
                {
                    DataTable dt = new DataTable();


                    strSql = " Select Eligibility,O.Opp_Name as Segment,Segment_Type from Campaigns C";
                    strSql += " Left Join Opportunity  O on   O.Opportunity_Id=C.Opportunity_Id Where Campaign_Id=" + iCampaignId;

                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                    else
                        dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                    if (dt.Rows.Count > 0)
                        fnDeleteCampaignRankings(iCampaignId, dt.Rows[0]["Eligibility"].ToString(), dt.Rows[0]["Segment_Type"].ToString() + "-" + dt.Rows[0]["Segment"].ToString(), strTabName);


                    dt = fnGetCampaignPtnl(strOppName, strEligibilty, strOppRankstatus, strTabName);

                    strSql = "UPDATE  CAMPAIGNS SET NAME='" + strName.Replace("'", "''") + "',CODE='" + strCode.Replace("'", "''") + "',DESCRIPTION='" + strDesc.Replace("'", "''") + "',";
                    strSql += " OPPORTUNITY_ID = '" + strOppid + "',OFFER_ID ='" + strOfferid + "',CHANNEL ='" + strChnl.Replace("'", "''") + "',";
                    strSql += "TAKE_UP_RATE ='" + strtakeUp + "',SEGMENT_TYPE='" + strSegmentType + "',ELIGIBILITY ='" + strEligibilty.Replace("'", "''") + "',ACCOUNTS ='" + dt.Rows[0]["ACCOUNTS"].ToString() + "',TOTAL_POTENTIAL ='" + dt.Rows[0]["TotalPotential"].ToString() + "',";
                    strSql += "CREATEDBY='" + iLoginUserId.ToString() + "',";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        strSql += "CREATEDDATE = sysdate" + ",ISACTIVE ='" + iIsActive +"',SEGMENT_DESCRIPTION='"+strOppRankstatus.Replace("'","''") + "' WHERE CAMPAIGN_ID = " + iCampaignId; 
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                      //  fnSaveCampaignRankings(strEligibilty, iProjectId, strOppRankstatus, iCampaignId, strTabName);
                        //iCampaignId = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select max(CAMPAIGN_ID) from CAMPAIGNS"));
                    }
                    else
                    {
                        strSql += "CREATEDDATE ='" + " getdate() ," + "',ISACTIVE ='" + iIsActive;
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        iCampaignId = int.Parse(((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select max(CAMPAIGN_ID) from CAMPAIGNS"));
                    }

                }

                return iCampaignId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool fnDeleteCampaign(int iCampaignId, string strTabName)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSql = "";

                strSql = " Select Eligibility,O.Opp_Name as Segment,Segment_Type from Campaigns C";
                strSql += " Left Join Opportunity  O on   O.Opportunity_Id=C.Opportunity_Id Where Campaign_Id=" + iCampaignId;

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                if (dt.Rows.Count > 0)
                    fnDeleteCampaignRankings(iCampaignId, dt.Rows[0]["Eligibility"].ToString(), dt.Rows[0]["Segment"].ToString() + "-" + dt.Rows[0]["Segment_Type"].ToString(), strTabName);


                strSql = "DELETE from CAMPAIGNS WHERE  CAMPAIGN_ID =" + iCampaignId;
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable fnRankOpportunityName(string strRank)
        {

            try
            {
                DataTable dt = new DataTable();
                string strSql = "";
                strSql += "select OPP_NAME from Opportunity where OPP_NAME =(select NVL(Rank" + strRank + ",0) from opportunity_ranking)";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                if (dt.Rows.Count < 1)
                {
                    strSql = "select OPP_NAME from Opportunity where OPPORTUNITY_ID= ";
                    strSql += "(select OPPORTUNITY_ID from (";
                    strSql += " select OPPORTUNITY_ID, rownum as rn ";
                    strSql += " from (select OPPORTUNITY_ID from Opportunity_Values order by OPP_PTNL_SUM desc";
                    strSql += "  )  ) where rn=" + strRank + ")";
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
                //else
                //    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public void fnPriorotiseCampaigns(DataRow[] drows, string strTabName)
        {
            try
            {
                DataTable dt;

                string strSql = "";
                strSql = "Delete from PRIORITIZED_TEMP";
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                foreach (DataRow dr in drows)
                {
                    DataTable dtRank;
                    dtRank = fnRankOpportunityName(dr["OPP_RANK"].ToString());
                    strSql = "SELECT OPP_NAME from OPPORTUNITY WHERE OPPORTUNITY_ID=" + dr["OPPORTUNITY_ID"];

                    DataTable dtopp;
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        dtopp = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                    else
                        dtopp = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                    strSql = " Insert into PRIORITIZED_TEMP(CampaignId,Customer)";
                    strSql += " SELECT " + dr["CAMPAIGN_ID"] + ",  A.Customer FROM tre_opportunity  A ";
                    strSql += " LEFT JOIN (SELECT  CUSTOMER , WEEK FROM  " + strTabName + " ) D ON D.CUSTOMER=A.CUSTOMER ";
                    strSql += " wHERE  A.Customer IN (SELECT CUSTOMER FROM ETS_TRE_BASE3  WHERE ";
                    if (dr["ELIGIBILITY"].ToString() != "")
                        strSql += dr["ELIGIBILITY"] + " and ";
                    strSql += " WEEK= (Select Max(week) from " + strTabName + " where year=to_char(sysdate, 'YYYY'))) ";
                    strSql += " and  A." + dtopp.Rows[0]["OPP_NAME"] + "_STATUS!='NA' ";
                    if (dtRank.Rows.Count > 0)
                        strSql += " and  A." + dtRank.Rows[0]["OPP_NAME"] + "_STATUS!='NA' ";
                    //strSql += " and  A." + dtRank.Rows[0]["OPP_NAME"] + "_STATUS!='NA' ";
                    strSql += "and A.CUSTOMER NOT IN ( SELECT CUSTOMER FROM PRIORITIZED_TEMP)";

                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }
                    else
                    {
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                    }
                    strSql = "Update Campaigns C SET ACCOUNTS= (Select Count(P.Customer) from PRIORITIZED_TEMP P  where C.CAMPAIGN_ID=P.CAMPAIGNID and P.CAMPAIGNID=" + dr["CAMPAIGN_ID"] + ")";
                    // strSql += "where Exists (select 1 from Campaigns O , PRIORITIZED_TEMP T where O.CAMPAIGN_ID=T.CAMPAIGNID AND O.CAMPAIGN_ID=C.CAMPAIGN_ID)";
                    strSql += "  where C.CAMPAIGN_ID=" + dr["CAMPAIGN_ID"];
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {

                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                    }
                    else
                    {

                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                    }
                }




                //strSql = "select Count(Customer),CAMPAIGNID  from PRIORITIZED_TEMP Group by CAMPAIGNID";
                //if (Common.iDBType == (int)Enums.DBType.Oracle)
                // dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                //   else

                // dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                //return dt;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public bool fnActiveCampaigns(string strOfferId)
        {
            try
            {
                //string[] str = strOfferId.Split(';');
                //if (str.Length > 0)
                //{

                //    string strSql = "UPDATE CAMPAIGNS SET  ISACTIVE = " + str[1] + " WHERE  CAMPAIGN_ID = " + str[0];
                // ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                // if (str[1] == "0")
                //     {
                //         strSql = " DELETE FROM PRIORITIZED_TEMP  WHERE CAMPAIGNID=" + str[0];
                //         //strSql += "SELECT  nvl(Sum(CAMPAIGN_ID),0) FROM CAMPAIGNS WHERE CAMPAIGN_ID=" + str[0];
                //         ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                //     }
                //}
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnSaveCampaignRankings(string strEligibility, int IprojectId, string Oppsegment, int CampaignId, string strTabName)
        {
            string strSql = "";
            strSql += " DECLARE";
            strSql += "  customer  Tre_Opportunity.CUSTOMER%TYPE; ";
            strSql += " Campaignid Campaigns.Campaign_Id%TYPE:=" + CampaignId + ";";
            strSql += "  ProjectId INT(4):=" + IprojectId + ";";
            strSql += " CurrentSegment string(5000):='''" + Oppsegment.Replace("'","''") + "''';";
            strSql += " CurrentCmpgnRank1 Campaign_Ranking.Campaign_Ranking1%TYPE;";
            strSql += " CurrentCmpgnRank2 Campaign_Ranking.Campaign_Ranking1%TYPE;";
            strSql += " CurrentCmpgnRank3 Campaign_Ranking.Campaign_Ranking1%TYPE;";
            strSql += " CurrentCmpgnRank4 Campaign_Ranking.Campaign_Ranking1%TYPE;";
            strSql += " Rank1_Action Tre_Ranking.Rank1_Action%TYPE;";
            strSql += " Rank2_Action Tre_Ranking.Rank2_Action%TYPE;";
            strSql += " Rank3_Action Tre_Ranking.Rank3_Action%TYPE;";
            strSql += " Rank4_Action Tre_Ranking.Rank4_Action%TYPE;";
            strSql += " CampaignRank1    string(100);";
            strSql += " CampaignRank2    string(100);";
            strSql += " CampaignRank3    string(100);";
            strSql += " CampaignRank4    string(100);";
            strSql += " CURSOR ttl_customer IS ";
            strSql += " SELECT A.CUSTOMER,RANK1_ACTION,RANK2_ACTION,RANK3_ACTION,RANK4_ACTION    FROM  TRE_OPPORTUNITYEXPORT A ";
            strSql += " LEFT JOIN (SELECT * FROM " + strTabName + "_V ) D ON A.CUSTOMER=D.CUSTOMER AND A.WEEK=D.WEEK ";
            strSql += " LEFT JOIN (SELECT CUSTOMER,RANK1_ACTION,RANK2_ACTION,RANK3_ACTION,RANK4_ACTION  FROM Tre_Ranking ) R";
            strSql += " ON A.CUSTOMER=R.CUSTOMER ";
            strSql += " WHERE ";
            if (strEligibility != "")
                strSql += strEligibility + " AND ";
            //strSql += Oppsegment+";";
            strSql += " (RANK1_ACTION IN ( CurrentSegment )";
            strSql += " OR RANK2_ACTION IN ( CurrentSegment )";
            strSql += " OR RANK3_ACTION IN ( CurrentSegment )";
            strSql += " OR RANK4_ACTION IN ( CurrentSegment ));";
            strSql += " BEGIN  FOR cust_rec in ttl_customer LOOP";
            strSql += " SELECT Campaign_Ranking1,Campaign_Ranking2,Campaign_Ranking3,Campaign_Ranking4 INTO CurrentCmpgnRank1,CurrentCmpgnRank2";
            strSql += " ,CurrentCmpgnRank3,CurrentCmpgnRank4 from Campaign_Ranking  WHERE  Campaign_Ranking.Customer=cust_rec.CUSTOMER;";
            strSql += " Delete from Campaign_Ranking  WHERE  Campaign_Ranking.Customer=cust_rec.CUSTOMER;";
            strSql += " SP_CampaignRanking(cust_rec.customer,Campaignid,ProjectId,CurrentSegment,CurrentCmpgnRank1,CurrentCmpgnRank2,CurrentCmpgnRank3,CurrentCmpgnRank4,cust_rec.RANK1_ACTION,cust_rec.RANK2_ACTION,cust_rec.RANK3_ACTION,cust_rec.RANK4_ACTION,CampaignRank1,CampaignRank2,CampaignRank3,CampaignRank4);";
            strSql += " end LOOP; ";
            strSql += "  END; ";
            if (Common.iDBType == (int)Enums.DBType.Oracle)
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);



        }
        public void fnPrioritizeRankings(int iprojectId, string strPrioritize)
        {
            string strSql = "";
            strSql += " DECLARE ";
            strSql += " Customer  TRE_OPPORTUNITYEXPORT.CUSTOMER%TYPE;";
            strSql += " Project_id  int(3):=" + iprojectId + ";";
            strSql += " strPrioritise  String(50):='" + strPrioritize + "';";
            strSql += " CampaignRank1    Campaign_Ranking.Campaign_Ranking1%TYPE;";
            strSql += " CampaignRank2    Campaign_Ranking.Campaign_Ranking2%TYPE;";
            strSql += " CampaignRank3    Campaign_Ranking.Campaign_Ranking3%TYPE;";
            strSql += " CampaignRank4    Campaign_Ranking.Campaign_Ranking4%TYPE;  ";
            strSql += " PriorCampaignRank1    Campaigns.Campaign_Id%TYPE;";
            strSql += " PriorCampaignRank2    Campaigns.Campaign_Id%TYPE;";
            strSql += " PriorCampaignRank3    Campaigns.Campaign_Id%TYPE;";
            strSql += " PriorCampaignRank4    Campaigns.Campaign_Id%TYPE; ";
            strSql += " CURSOR ttl_customer IS ";
            strSql += " SELECT CUSTOMER FROM CAMPAIGN_RANKING;";
            strSql += " BEGIN  FOR cust_rec in ttl_customer LOOP";
            strSql += " CampaignRank1:='';";
            strSql += " CampaignRank2:='';";
            strSql += " CampaignRank3:='';";
            strSql += " CampaignRank4:='';";
            strSql += " Select Campaign_Ranking1,Campaign_Ranking2,Campaign_Ranking3,Campaign_Ranking4 INTO CampaignRank1,CampaignRank2,CampaignRank3,CampaignRank4  from Campaign_Ranking C where C.CUSTOMER=cust_rec.Customer;";
            strSql += " DELETE FROM PRIORITIZED_TEMP WHERE Customer=cust_rec.Customer;";
            strSql += " SP_PrioritizeCampaignRanking(cust_rec.customer,Project_id,CampaignRank1,CampaignRank2,CampaignRank3,CampaignRank4,strPrioritise,PriorCampaignRank1,PriorCampaignRank2,PriorCampaignRank3,PriorCampaignRank4);";
            strSql += " END LOOP; ";
            strSql += " END; ";
            if (Common.iDBType == (int)Enums.DBType.Oracle)
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
            //if (Common.iDBType == (int)Enums.DBType.Oracle)
            //    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

        }
        public void fnDeleteCampaignRankings(int Campaignid, string strEligibilty, string strOppRankstatus, string strTabName)
        {
            string strSql = "";
            strSql += " DECLARE  ";
            strSql += " Customer  TRE_OPPORTUNITYEXPORT.CUSTOMER%TYPE;";
            strSql += " Campaignid Varchar2(3):=" + Campaignid + ";";
            strSql += " CurrentSegment string(500):='" + strOppRankstatus + "';";
            strSql += " CurrentCmpgnRank1 Campaign_Ranking.Campaign_Ranking1%TYPE;";
            strSql += " CurrentCmpgnRank2 Campaign_Ranking.Campaign_Ranking1%TYPE;";
            strSql += " CurrentCmpgnRank3 Campaign_Ranking.Campaign_Ranking1%TYPE;";
            strSql += " CurrentCmpgnRank4 Campaign_Ranking.Campaign_Ranking1%TYPE;";
            strSql += " CampaignRank1  string(100);";
            strSql += " CampaignRank2  string(100);";
            strSql += " CampaignRank3  string(100);";
            strSql += " CampaignRank4  string(100);";
            strSql += " CURSOR ttl_customer IS ";
            strSql += " SELECT A.CUSTOMER    FROM  TRE_OPPORTUNITYEXPORT A ";
            strSql += " LEFT JOIN (SELECT * FROM " + strTabName + " ) D ON A.CUSTOMER=D.CUSTOMER AND D.WEEK=A.WEEK ";
            strSql += " LEFT JOIN (SELECT CUSTOMER,RANK1_ACTION,RANK2_ACTION,RANK3_ACTION,RANK4_ACTION  FROM Tre_Ranking ) R";
            strSql += " ON A.CUSTOMER=R.CUSTOMER WHERE  ";
            if (strEligibilty != "")
                strSql += strEligibilty + " AND";
            strSql += " (RANK1_ACTION =  CurrentSegment";
            strSql += " OR RANK2_ACTION = CurrentSegment";
            strSql += " OR RANK3_ACTION = CurrentSegment";
            strSql += " OR RANK4_ACTION =CurrentSegment);";
            strSql += " BEGIN  FOR cust_rec in ttl_customer LOOP";
            strSql += " SELECT Campaign_Ranking1,Campaign_Ranking2,Campaign_Ranking3,Campaign_Ranking4 INTO CurrentCmpgnRank1,CurrentCmpgnRank2";
            strSql += " ,CurrentCmpgnRank3,CurrentCmpgnRank4 from Campaign_Ranking  WHERE  Campaign_Ranking.Customer=cust_rec.CUSTOMER;";
            strSql += " Delete from Campaign_Ranking  WHERE  Campaign_Ranking.Customer=cust_rec.CUSTOMER; ";
            strSql += " SP_DeleteCampaignRanking(cust_rec.customer,Campaignid,CurrentCmpgnRank1,CurrentCmpgnRank2,CurrentCmpgnRank3,CurrentCmpgnRank4,CampaignRank1,CampaignRank2,CampaignRank3,CampaignRank4);";
            strSql += " END LOOP; ";
            strSql += " END; ";
            if (Common.iDBType == (int)Enums.DBType.Oracle)
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);



        }
        public DataTable fnGetEffectiveTable(int iProjectId, out string tablequery, string strTabName)
        {

            try
            {
                DataTable dt = new DataTable();
                tablequery = "";
                string Tre_Columns = "";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    Tre_Columns = fnTre_oppColumns(iProjectId);
                    if (Tre_Columns != "")
                        tablequery = " SELECT C.*," + Tre_Columns + " FROM " + strTabName + "  C LEFT JOIN TRE_OPPORTUNITYEXPORT T  ON T.CUSTOMER=C.CUSTOMER AND T.WEEK=C.WEEK WHERE ROWNUM <= 2";
                    else
                        tablequery = " SELECT C.* FROM " + strTabName + " C LEFT JOIN TRE_OPPORTUNITYEXPORT T  ON T.CUSTOMER=C.CUSTOMER AND T.WEEK=C.WEEK WHERE ROWNUM <= 2";

                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, tablequery);
                }
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public string fnTre_oppColumns(int iProjectId)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSql = "";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    strSql = "SELECT OPP_NAME  from OPPORTUNITY where project_id=" + iProjectId +" And ISONMAIN=1";
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                    strSql = "";
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            strSql += dr["OPP_NAME"].ToString() + "_" + "DELTA,";
                            strSql += dr["OPP_NAME"].ToString() + "_" + "STATUS,";
                            strSql += dr["OPP_NAME"].ToString() + "_" + "PNTL,";

                        }
                    }

                }
                if (strSql.Length > 1)
                    strSql = strSql.Remove(strSql.Length - 1);
                return strSql;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void fnSaveCampaignRankingsfrmExport(int iPriorValue, int IprojectId, string strTabName, string strMainFilter)
        {
           
            string strSql = "";
            string strDBFiltr = "";

            if (strMainFilter == "")
            {
                strDBFiltr = " 1=1 ";
            }
              

            strSql += " DECLARE";
            strSql += " ProjectId INT(4):=" + IprojectId + ";";
            strSql += " strDBFiltr string(100):= '" + strDBFiltr + "';";
            strSql += " iPriorValue INT:=" + iPriorValue + ";";
            strSql += " BEGIN";
            strSql += " TRE_CAMPAIGN_RANKING(ProjectId, strDBFiltr, iPriorValue);";
            strSql += " END; ";

            //strSql += " DECLARE";
            //strSql += "  customer  TRE_OPPORTUNITYEXPORT.CUSTOMER%TYPE; ";
            //strSql += " Campaignid Campaigns.Campaign_Id%TYPE:=" + CampaignId + ";";
            //strSql += "  ProjectId INT(4):=" + IprojectId + ";";
            //strSql += " CurrentSegment string(5000):='" + Oppsegment.Replace("'", "") + "';";
            //strSql += " CurrentCmpgnRank1 Campaign_Ranking.Campaign_Ranking1%TYPE;";
            //strSql += " CurrentCmpgnRank2 Campaign_Ranking.Campaign_Ranking1%TYPE;";
            //strSql += " CurrentCmpgnRank3 Campaign_Ranking.Campaign_Ranking1%TYPE;";
            //strSql += " CurrentCmpgnRank4 Campaign_Ranking.Campaign_Ranking1%TYPE;";
            //strSql += " Rank1_Action Tre_Ranking.Rank1_Action%TYPE;";
            //strSql += " Rank2_Action Tre_Ranking.Rank2_Action%TYPE;";
            //strSql += " Rank3_Action Tre_Ranking.Rank3_Action%TYPE;";
            //strSql += " Rank4_Action Tre_Ranking.Rank4_Action%TYPE;";
            //strSql += " CampaignRank1    string(100);";
            //strSql += " CampaignRank2    string(100);";
            //strSql += " CampaignRank3    string(100);";
            //strSql += " CampaignRank4    string(100);";
            //strSql += " CURSOR ttl_customer IS ";
            //strSql += " SELECT A.CUSTOMER,RANK1_ACTION,RANK2_ACTION,RANK3_ACTION,RANK4_ACTION    FROM  TRE_OPPORTUNITYEXPORT A ";
            //strSql += " LEFT JOIN (SELECT * FROM " + strTabName + "_V ) D ON A.CUSTOMER=D.CUSTOMER AND A.WEEK=D.WEEK ";
            //strSql += " LEFT JOIN (SELECT CUSTOMER,RANK1_ACTION,RANK2_ACTION,RANK3_ACTION,RANK4_ACTION  FROM Tre_Ranking ) R";
            //strSql += " ON A.CUSTOMER=R.CUSTOMER ";
            //strSql += " WHERE ";
            //if (strEligibility != "")
            //    strSql += strEligibility + " And ";
            //if (strMainFilter != "" && strMainFilter != strEligibility)
            //{

            //    strSql += strMainFilter + " And";
            //}
            //strSql += " (RANK1_ACTION IN ('" + Oppsegment + "')";
            //strSql += " OR RANK2_ACTION IN ('" + Oppsegment + "')";
            //strSql += " OR RANK3_ACTION IN ('" + Oppsegment + "')";
            //strSql += " OR RANK4_ACTION IN ('" + Oppsegment + "'));";
            //strSql += " BEGIN  FOR cust_rec in ttl_customer LOOP";
            //strSql += " SELECT Campaign_Ranking1,Campaign_Ranking2,Campaign_Ranking3,Campaign_Ranking4 INTO CurrentCmpgnRank1,CurrentCmpgnRank2";
            //strSql += " ,CurrentCmpgnRank3,CurrentCmpgnRank4 from Campaign_Ranking  WHERE  Campaign_Ranking.Customer=cust_rec.CUSTOMER;";
            //strSql += " Delete from Campaign_Ranking  WHERE  Campaign_Ranking.Customer=cust_rec.CUSTOMER;";
            //strSql += " SP_CampaignRanking(cust_rec.customer,Campaignid,ProjectId,CurrentSegment,CurrentCmpgnRank1,CurrentCmpgnRank2,CurrentCmpgnRank3,CurrentCmpgnRank4,cust_rec.RANK1_ACTION,cust_rec.RANK2_ACTION,cust_rec.RANK3_ACTION,cust_rec.RANK4_ACTION,CampaignRank1,CampaignRank2,CampaignRank3,CampaignRank4);";
            //strSql += " end LOOP; ";
            //strSql += "  END; ";
            if (Common.iDBType == (int)Enums.DBType.Oracle)
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
        }
        public void fnPrioritizeRankingsfrmExport(int iprojectId, string strPrioritize)
        {
            try
            {
                string strSql = "";
                strSql += " DECLARE ";
                strSql += " Customer  TRE_OPPORTUNITYEXPORT.CUSTOMER%TYPE;";
                strSql += " Project_id  int(3):=" + iprojectId + ";";
                strSql += " strPrioritise  String(50):='" + strPrioritize + "';";
                strSql += " CampaignRank1    Campaign_Ranking.Campaign_Ranking1%TYPE;";
                strSql += " CampaignRank2    Campaign_Ranking.Campaign_Ranking2%TYPE;";
                strSql += " CampaignRank3    Campaign_Ranking.Campaign_Ranking3%TYPE;";
                strSql += " CampaignRank4    Campaign_Ranking.Campaign_Ranking4%TYPE;  ";
                strSql += " PriorCampaignRank1    Campaigns.Campaign_Id%TYPE;";
                strSql += " PriorCampaignRank2    Campaigns.Campaign_Id%TYPE;";
                strSql += " PriorCampaignRank3    Campaigns.Campaign_Id%TYPE;";
                strSql += " PriorCampaignRank4    Campaigns.Campaign_Id%TYPE; ";
                strSql += " CURSOR ttl_customer IS ";
                strSql += " SELECT CUSTOMER FROM CAMPAIGN_RANKING;";
                strSql += " BEGIN  FOR cust_rec in ttl_customer LOOP";
                strSql += " CampaignRank1:='';";
                strSql += " CampaignRank2:='';";
                strSql += " CampaignRank3:='';";
                strSql += " CampaignRank4:='';";
                strSql += " Select Campaign_Ranking1,Campaign_Ranking2,Campaign_Ranking3,Campaign_Ranking4 INTO CampaignRank1,CampaignRank2,CampaignRank3,CampaignRank4  from Campaign_Ranking C where C.CUSTOMER=cust_rec.Customer;";
                strSql += " DELETE FROM PRIORITIZED_TEMP WHERE Customer=cust_rec.Customer;";
                strSql += " SP_PrioritizeCampaignRanking(cust_rec.customer,Project_id,CampaignRank1,CampaignRank2,CampaignRank3,CampaignRank4,strPrioritise,PriorCampaignRank1,PriorCampaignRank2,PriorCampaignRank3,PriorCampaignRank4);";
                strSql += " END LOOP; ";
                strSql += " END; ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                //if (Common.iDBType == (int)Enums.DBType.Oracle)
                //    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void fnDelteCampaignRankingsfrmExport(int iProjectId, string strTabname)
        {
            
                string strSql = "";
                int iCount = 0;
                strSql = "Select Count(1) from Campaign_Ranking where ProjectId=" + iProjectId;
                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                if (iCount != 0)
                {

                    strSql = "Delete from CAMPAIGN_RANKING ";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }
                      
                  strSql = "Delete from PRIORITIZED_TEMP";

                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    //else
                    //{
                    //    ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    //}
                }
                    strSql = "INSERT INTO CAMPAIGN_RANKING(CUSTOMER,CAMPAIGN_RANKING1,CAMPAIGN_RANKING2,CAMPAIGN_RANKING3,CAMPAIGN_RANKING4,PROJECTID)  SELECT CUSTOMER,'No Campaign','No Campaign','No Campaign','No Campaign'," + iProjectId + " FROM TRE_RANKING";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
            }
        }
    }




