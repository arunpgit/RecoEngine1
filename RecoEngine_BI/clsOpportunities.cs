using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RecoEngine_DataLayer;
using System.Collections;

namespace RecoEngine_BI
{
    public class clsOpportunities
    {
        clsTre_Details objclsTreDetails = new clsTre_Details();

        //public DataTable fnGetOpportunityDetails(int iOpportunityID)
        //{
        //    try
        //    {

        //        DataTable dt;
        //        string strSql = "select A.*, B.OPP_Name,B.Description,B.Formula,B.CreatedDate,B.CreatedBy from ";
        //       strSql+= " OPPORTUNITY B Inner join Status_BreakDown A ON A.Opportunity_ID=B.Opportunity_ID ";
        //       strSql+=" and A.Opportunity_ID= " + iOpportunityID;                                

        //        if (Common.iDBType == (int)Enums.DBType.Oracle)
        //            dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
        //        else
        //            dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable fnGetOpportunity(int iProjectId,int iUserId,bool bIsOppList)
        {
            try
            {

                DataTable dt;
                string strSql = "";
                strSql = "Select A.OPPORTUNITY_ID,A.OPP_NAME,A.DESCRIPTION,A.FORMULA,A.ELGBL_FORMULA,A.OPP_ACTION,A.CREATEDDATE,A.CREATEDBY,A.PROJECT_ID,A.PTNL_FORMULA,A.ISONMAIN,A.ISACTIVE as ISACTIVEID,";
                strSql += "CASE WHEN A.ISACTIVE=1 THEN 'YES' ELSE 'NO' END as ISACTIVE, ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    strSql += " U.First_name || ' ' || U.last_name as UName ";
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    strSql += " ,U.First_name + ' ' + U.last_name as UName ";
                strSql += " ,'' as Flag,'' as Threshold,B.T1,B.T2 From OPPORTUNITY A Left join Users U on U.USER_ID=A.CREATEDBY ";
                strSql += " left join Status_BreakDown B ON A.Opportunity_ID=B.Opportunity_ID Where ";
                if (iProjectId != 0)
                    strSql += " A.Project_Id=" + iProjectId + " and  A.CREATEDBY=" + iUserId;
                if (!bIsOppList)
                strSql += " And A.IsActive=1";


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
        public void fnSaveOPPRanking(string strRName, int iProjectId, string strRank1, string strRank2, string strRank3, string strRank4)

        {
            try
            {
                string strSql = "";
                strSql = "delete from OPPORTUNITY_RANKING where PROJECT_ID=" + iProjectId;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
               ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                strSql = " Insert into OPPORTUNITY_RANKING(TYPE,PROJECT_ID,RANK1,RANK2,RANK3,RANK4) values( ";
                strSql += "'" + strRName + "'," + iProjectId + ",";
                if (strRank1 != "")
                    strSql += "'"+strRank1 + "'";
                else
                    strSql += "," + "NULL";

                if (strRank2 != "")
                    strSql += ", '" + strRank2 + "'";
                else
                    strSql += ", " + "NULL";

                if (strRank3 != "")
                    strSql += ", '" + strRank3 + "'";
                else
                    strSql += ", " + "NULL";

                if (strRank4 != "")
                    strSql += ", '" + strRank4 +"'";
                else
                    strSql += "," + "NULL";

                strSql += ")";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnInsertOpportunity()
        {

            string strSql = "Select OPP_NAME from opportunity";
            DataTable dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

            foreach (DataRow sr in dt.Rows)
            {
                strSql =
                strSql = sr.ToString();
                strSql += ",";
            }

        }
        public bool fnDeleteRankingOpportunity(string strID)
        {
            try
            {
                string strSql = " Delete From OPPORTUNITY_RANKING where ID =" + strID;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                else
                    ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable fnGetOppRanking(int iProjectId)
        {
            try
            {

                DataTable dt;
                string strSql = "";


                strSql = "Select ORR.Project_ID,ORR.TYPE,ORR.RANK1,ORR.RANK2,ORR.RANK3,ORR.RANK4,O.OPP_NAME as Opportunity1, ";
                strSql += "O1.OPP_NAME as Opportunity2,O2.OPP_NAME as Opportunity3,O3.OPP_NAME as Opportunity4 ";
                strSql += " from OPPORTUNITY_RANKING ORR ";
                strSql += " Left Join  OPPORTUNITY O On O.Opportunity_ID=ORR.RANK1 ";
                strSql += " Left Join  OPPORTUNITY O1 On O1.Opportunity_ID=ORR.RANK2 ";
                strSql += " Left Join  OPPORTUNITY O2 On O2.Opportunity_ID=ORR.RANK3 ";
                strSql += " Left Join  OPPORTUNITY O3 On O3.Opportunity_ID=ORR.RANK4 ";
                strSql += "  where O.PROJECT_ID =" + iProjectId;

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
        public int fnSaveOpportunity_old(int iOpportunityId, string strOppName, string strDesc, string strFormula, int strLoginUserId, int iProjectId, string strTableName, string strKeyName, string[] strT1, string[] strT2)
        {
            try
            {

                string strSql = "";
                DataTable dt = new DataTable();

                strSql = "Select OPPORTUNITY_ID from OPPORTUNITY  where OPP_NAME='" + strOppName + "' and PROJECT_ID=" + iProjectId;

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                if (dt.Rows.Count > 0)
                {
                    iOpportunityId = int.Parse(dt.Rows[dt.Rows.Count - 1][0].ToString());

                    strSql = "Update OPPORTUNITY  Set OPP_NAME= '" + strOppName + "',DESCRIPTION='" + strDesc.Replace("'", "''") + "',FORMULA='" + strFormula.Replace("'", "''") + "',";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        strSql += "CREATEDDATE=sysdate ,CREATEDBY=" + strLoginUserId + ",PROJECT_ID=" + iProjectId + " where OPPORTUNITY_ID=" + iOpportunityId;

                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }
                    else
                    {
                        strSql += "CREATEDDATE=getdate() ,CREATEDBY=" + strLoginUserId + ",PROJECT_ID" + iProjectId + " where OPPORTUNITY_ID=" + iOpportunityId;
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }

                }
                else
                {


                    strSql = " Insert into OPPORTUNITY (OPP_NAME,DESCRIPTION,FORMULA,CREATEDDATE,CREATEDBY,PROJECT_ID) Values ( ";
                    strSql += " '" + strOppName + "','" + strDesc.Replace("'", "''") + "','" + strFormula.Replace("'", "''") + "',";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {

                        strSql += " sysdate ," + strLoginUserId + "," + iProjectId + ")";
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                        iOpportunityId = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select max(OPPORTUNITY_ID) from OPPORTUNITY"));

                    }
                    else
                    {
                        strSql += "getdate() ," + strLoginUserId + "," + iProjectId + ")";
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        iOpportunityId = int.Parse(((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select max(OPPORTUNITY_ID) from OPPORTUNITY"));

                    }
                }
                int iCount = 0;

                strSql = " Select count(1) from user_tab_columns where table_name = 'TRE_OPPORTUNITY' and upper(column_name) = '" + strOppName.ToUpper() + "'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                else
                {
                    iCount = int.Parse(((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }

                if (iCount == 0)
                {

                    strSql = "alter table TRE_OPPORTUNITY add " + strOppName.ToUpper() + " NUMBER(18,2) ";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    else
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                    strSql = "alter table TRE_OPPORTUNITY add " + strOppName.ToUpper() + "_DELTA NUMBER(18,2) ";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    else
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                    strSql = "alter table TRE_OPPORTUNITY add " + strOppName.ToUpper() + "_STATUS varchar2(50) ";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    else
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                    strSql = "alter table TRE_OPPORTUNITY add " + strOppName.ToUpper() + "_PNTL NUMBER(18,2) ";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    else
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }


                DataTable dtSource = new DataTable();
                strSql = "Select * from TRE_MAPPING where TYPE=" + ((int)Enums.ColType.Key).ToString() + " OR TYPE=" + ((int)Enums.ColType.Time).ToString();
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dtSource = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dtSource = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                if (dtSource.Rows.Count > 0)
                {

                    string strColName = "";
                    string strUpdateString = "";
                    string strUpdateMString = "";
                    string strBT1 = Common.fnBuildQuery(strT1);
                    string strBT2 = Common.fnBuildQuery(strT2);

                    for (int i = 0; i < dtSource.Rows.Count; i++)
                    {
                        if (strColName != "")
                        {
                            strColName += ",";
                            strUpdateString += " And ";
                            strUpdateMString += " And ";
                        }
                        strColName += dtSource.Rows[i]["COLNAME"].ToString();
                        strUpdateString += "A." + dtSource.Rows[i]["COLNAME"].ToString() + "=B." + dtSource.Rows[i]["COLNAME"].ToString();

                        strUpdateMString += "OPT." + dtSource.Rows[i]["COLNAME"].ToString() + "=TDN." + dtSource.Rows[i]["COLNAME"].ToString();
                        strUpdateMString += " AND OPT." + dtSource.Rows[i]["COLNAME"].ToString() + "=A." + dtSource.Rows[i]["COLNAME"].ToString();
                    }


                    strSql = "Select count(1) from  TRE_OPPORTUNITY ";
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                    if (iCount == 0)
                    {
                        //strSql = " INSERT INTO TRE_OPPORTUNITY(" + strColName + "," + strOppName + ")";
                        //strSql += " select " + strColName + ", Case When T1=0 then 0 Else Round(T2/T1-1,2) From ";
                        //strSql += "(Select " + strColName + ", round(avg(" + strFormula.ToUpper() + "),2) T1 from " + strTableName;
                        //strSql += " where " + strBT1 + "  group by " + strColName + ") A ";
                        //strSql += " Left join (Select " + strColName + ", round(avg(" + strFormula.ToUpper() + "),2) T2 from " + strTableName + " where  " + strBT2 + " group by " + strColName + ") B ";
                        //strSql += " On " + strUpdateString ; // A.CUSTOMER=B.CUSTOMER";


                        strSql = " INSERT INTO TRE_OPPORTUNITY(" + strColName + "," + strOppName + ")";
                        strSql += " select " + strColName + "," + strOppName + " From ";
                        strSql += "    from " + strTableName;

                        if (Common.iDBType == (int)Enums.DBType.Oracle)
                            ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                        else
                            ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }
                    else
                    {
                        if (Common.iDBType == (int)Enums.DBType.Oracle)
                        {
                            strSql = " Update TRE_OPPORTUNITY A Set " + strOppName + "=";
                            strSql += " (Select " + strFormula.Replace("'", "''") + " from " + strTableName + " B where " + strUpdateString + ")";
                            strSql += " where Exists (select 1 from TRE_OPPORTUNITY OPT , " + strTableName + " TDN where " + strUpdateMString + ")";
                            ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);



                            strSql = "Update TRE_OPPORTUNITY A Set " + strOppName + "_Delta=";
                            strSql += " (select Case When T1=0 then 0 Else Round(T2/T1-1,2) End From ";
                            strSql += "(Select CUSTOMER, round(avg(" + strOppName.ToUpper() + "),2) T1 from TRE_OPPORTUNITY  ";
                            strSql += " where " + strBT1 + "  group by CUSTOMER) A ";
                            strSql += " Left join (Select CUSTOMER, round(avg(" + strOppName.ToUpper() + "),2) T2 from TRE_OPPORTUNITY where  " + strBT2 + " group by CUSTOMER) B ";
                            strSql += " On A.CUSTOMER=B.CUSTOMER)";
                            ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                            //strSql = " Update TRE_OPPORTUNITY A Set " + strOppName + "_Delta=";
                            //strSql += " (select Case When T1=0 then 0 Else Round(T2/T1-1,2) End From ";
                            //strSql += "(Select " + strColName + ", round(avg(" + strFormula.ToUpper() + "),2) T1 from " + strTableName;
                            //strSql += " where " + strBT1 + "  group by " + strColName + ") A ";
                            //strSql += " Left join (Select " + strColName + ", round(avg(" + strFormula.ToUpper() + "),2) T2 from " + strTableName + " where  " + strBT2 + " group by " + strColName + ") B ";
                            //strSql += " On " + strUpdateString + ")";
                            //strSql += " where Exists (select 1 from TRE_OPPORTUNITY OPT , " + strTableName + " TDN where " + strUpdateMString + ")";

                        }
                        else
                        {
                            //strSql = " Update TRE_OPPORTUNITY Set (" + strOppName + ")=";
                            //strSql += " (Select " + strFormula.Replace("'", "''") + " from " + strTableName + " where " + strUpdateString + ")";
                            //((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                        }
                    }

                    //updating the other column of opportunity



                    //strSql = " Update TRE_OPPORTUNITY A Set " + strOppName + "=";
                    //strSql += " (Select " + strFormula.Replace("'", "''") + " from " + strTableName + " B where " + strUpdateString + ")";
                    //strSql += " where Exists (select 1 from TRE_OPPORTUNITY OPT , " + strTableName + " TDN where " + strUpdateMString + ")";


                    //string strSql = "Select NVL(A.CUSTOMER,B.CUSTOMER) CUSTOMER, A.T1, B.T2,Case When T1=0 then 0 Else Round(T2/T1-1,2) End DELTA,";
                    //strSql += " Case When A.T1+B.T2 =0 Then 'NON_USER' ";
                    //strSql += " When A.T1=0 And B.T2>0 Then 'NEW_USER' ";
                    //strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End < " + Convert.ToDecimal(strStopper) + " Then 'STOPPER' ";
                    //strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End < " + Convert.ToDecimal(strDropper) + " Then 'DROPPER' ";
                    //strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End >  " + Convert.ToDecimal(strGrower) + " Then 'GROWER' ";
                    //strSql += " ELSE 'FLAT' END as Status From";
                    //strSql += "(Select CUSTOMER, round(avg(" + strOppName.ToUpper() + "),2) T1 from TRE_OPPORTUNITY  ";
                    //strSql += " where " + fnBuildQuery(strT1) + "  group by CUSTOMER) A ";
                    //strSql += " Left join (Select CUSTOMER, round(avg(" + strOppName.ToUpper() + "),2) T2 from TRE_OPPORTUNITY where  " + fnBuildQuery(strT2) + " group by CUSTOMER) B ";
                    //strSql += " On A.CUSTOMER=B.CUSTOMER";

                    ////from " + strTableName;

                }
                return iOpportunityId;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int fnSaveOpportunity(int iOpportunityId, string strOppName, string strDesc, string strFormula, string strElgblFormula, int strLoginUserId, int iProjectId, string strTableName, string strKeyName, string[] strT1, string[] strT2, int iIsActive,string OppType)
        {
            try
            {

                string strSql = "";
                DataTable dt = new DataTable();

                //strSql = "CREATE INDEX OPPO_IX ON OPPORTUNITY(OPP_NAME)";
                //((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);

                strSql = "Select OPPORTUNITY_ID from OPPORTUNITY  where OPPORTUNITY_ID='" + iOpportunityId + "' and PROJECT_ID=" + iProjectId;

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                if (dt.Rows.Count > 0)
                {
                    iOpportunityId = int.Parse(dt.Rows[dt.Rows.Count - 1][0].ToString());

                    strSql = "Update OPPORTUNITY  Set OPP_NAME= '" + strOppName + "',DESCRIPTION='" + strDesc.Replace("'", "''") + "',FORMULA='" + strFormula.Replace("'", "''") + "',";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        strSql += "CREATEDDATE=sysdate ,CREATEDBY=" + strLoginUserId + ",PROJECT_ID=" + iProjectId + ",ISACTIVE = " + iIsActive + ",ELGBL_FORMULA = '" + strElgblFormula.Replace("'", "''") + "',OPP_ACTION = '" + OppType + "'  where OPPORTUNITY_ID=" + iOpportunityId;

                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }
                    else
                    {
                        strSql += "CREATEDDATE=getdate() ,CREATEDBY=" + strLoginUserId + ",PROJECT_ID" + iProjectId + ",ISACTIVE = " + iIsActive + ",OPP_ACTION = '" + OppType + "'  where OPPORTUNITY_ID=" + iOpportunityId;
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }
                }
                else
                {
                    strSql = " Insert into OPPORTUNITY (OPP_NAME,DESCRIPTION,FORMULA,ELGBL_FORMULA,OPP_ACTION,CREATEDDATE,CREATEDBY,PROJECT_ID,ISACTIVE) Values ( ";
                    strSql += " '" + strOppName + "','" + strDesc.Replace("'", "''") + "','" + strFormula.Replace("'", "''") + "', '" + strElgblFormula.Replace("'", "''") + "', '" + OppType + "',";

                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {

                        strSql += " sysdate ," + strLoginUserId + "," + iProjectId + "," + iIsActive + ")";
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                        iOpportunityId = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select max(OPPORTUNITY_ID) from OPPORTUNITY"));

                    }
                    else
                    {
                        strSql += "getdate() ," + strLoginUserId + "," + iProjectId + "," + iIsActive + ")";
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                        iOpportunityId = int.Parse(((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, "Select max(OPPORTUNITY_ID) from OPPORTUNITY"));
                    }
                }
                int iCount = 0;

                strSql = " Select count(1) from user_tab_columns where table_name = 'TRE_OPPORTUNITY' and upper(column_name) = '" + strOppName.ToUpper() + "_DELTA'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                else
                {
                    iCount = int.Parse(((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }

                if (iCount == 0)
                {
                    strSql = "alter table TRE_OPPORTUNITY add " + strOppName.ToUpper() + "_DELTA NUMBER(18,2) ";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    else
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    strSql = "alter table TRE_OPPORTUNITY add " + strOppName.ToUpper() + "_STATUS varchar2(50) ";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    else
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                    strSql = "alter table TRE_OPPORTUNITY add " + strOppName.ToUpper() + "_PNTL NUMBER(18,2) ";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    else
                        ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }

                return iOpportunityId;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool fnCheckOPPHasInRanking(string strOppId,int iProjectId)
        {
            try
            {
                string strSql = "Select nvl(count(1),0) from OPPORTUNITY_RANKING where project_id= " + iProjectId + " AND ( RANK1 =" + strOppId + " OR RANK2 = " + strOppId + " OR RANK3 = " + strOppId + " OR RANK4 =" + strOppId + ")";
                if (int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text,strSql )) > 0)
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
        public bool fnDeleteOpportunity(ArrayList recForDelete)
        {
            try
            {
                string curRecId = "";
                string strName = "";
                //((OraDBManager)Common.dbMgr).BeginTrans();
                for (int i = 0; i < recForDelete.Count; i++)
                {
                    string[] str = recForDelete[i].ToString().Split(';');
                    curRecId = str[0];
                    strName = str[1];

                    string strSql = "Delete from STATUS_BREAKDOWN where OPPORTUNITY_ID=" + curRecId;
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                    int iCount = 0;
                    strSql = " Select count(1) from user_tab_columns where table_name = 'TRE_OPPORTUNITY' and upper(column_name) = '" + strName.ToUpper() + "_DELTA'";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                    }
                    if (iCount > 0)
                    {
                        strSql = "Alter  TABLE TRE_OPPORTUNITY drop Column " + strName.ToUpper() + "_DELTA";
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                        strSql = "Alter  TABLE TRE_OPPORTUNITY drop Column " + strName.ToUpper() + "_STATUS";
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                        strSql = "Alter  TABLE TRE_OPPORTUNITY drop Column " + strName.ToUpper() + "_PNTL";
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                    }

                     strSql = " DELETE from OPPORTUNITY_VALUES WHERE OPPORTUNITY_ID=" + curRecId;
                     ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                     //strSql = " DELETE from PRIORITIZED_TEMP WHERE CAMPAIGNID=";
                     //strSql += "(SELECT nvl(SUM(CAMPAIGN_ID),0) FROM CAMPAIGNS WHERE OPPORTUNITY_ID="+curRecId+")";
                     //((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                     strSql = "Delete from CAMPAIGNS where OPPORTUNITY_ID=" + curRecId;
                     ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                     strSql = "Delete from OPPORTUNITY where OPPORTUNITY_ID=" + curRecId;
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                 }
                //((OraDBManager)Common.dbMgr).CommitTrans();
                return true;

            }
            catch (Exception ex)
            {
                ((OraDBManager)Common.dbMgr).RollbackTrans();
                throw ex;
            }
        }
        public DataTable fnGetBaseColumns(ref DataTable dtSchema)
        {
            try
            {


                DataTable dt = new DataTable();
                string strSql = "";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    strSql = "select * from ETS_TRE_BASE2 where ROWNUM <= 2";
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
                else
                {
                    strSql = "select top 1 * from ETS_TRE_BASE2";
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
                DataTableReader dr = new DataTableReader(dt);
                dtSchema = dr.GetSchemaTable();
                dr.Close();
                dr = null;

                return dt;

                //ets_tre_base
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool fnRunOPoortunities(int iProjectId, string strTabName)
        {
            try
            {
                //((OraDBManager)Common.dbMgr).BeginTrans();

                DataTable dt = new DataTable();

                string strSql = "SELECT O.OPPORTUNITY_ID,O.OPP_NAME,O.FORMULA,O.ELGBL_FORMULA,O.PTNL_FORMULA,S.DROPPERS_CUTOFF ,S.STOPPERS_CUTOFF,S.GROWERS_CUTOFF,";
                strSql += " S.T1,S.T2,S.CURRENTSEGMENT,S.SEGMENTISACTIVE,TT.TIMEPERIOD_ID ";
                strSql += " FROM OPPORTUNITY O INNER JOIN STATUS_BREAKDOWN S ON O.OPPORTUNITY_ID=S.OPPORTUNITY_ID ";
                strSql += " LEFT JOIN TRE_TIMEPERIOD TT ON TT.T1=S.T1 AND TT.T2=S.T2";
                strSql += " WHERE O.PROJECT_ID= " + iProjectId + " AND O.ISONMAIN=0";

                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!fnSaveOppRecomendationSettings(dt.Rows[i], strTabName, iProjectId))
                    {
                        ((OraDBManager)Common.dbMgr).RollbackTrans();
                        return false;
                    }
                    if (!fnSaveThreshold(dt.Rows[i], strTabName,iProjectId))
                    {
                        ((OraDBManager)Common.dbMgr).RollbackTrans();
                        return false;
                    }

                    strSql = "Update OPPORTUNITY A Set ISONMAIN=1 Where OPPORTUNITY_ID=" + dt.Rows[i]["OPPORTUNITY_ID"];
                    DataTable dtupdate = new DataTable();
                    dtupdate = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }

                //((OraDBManager)Common.dbMgr).CommitTrans();
                return true;
            }
            catch (Exception ex)
            {
                ((OraDBManager)Common.dbMgr).RollbackTrans();
                throw ex;
            }
        }
        clsTre_Details clsTObj = new clsTre_Details();
        private bool fnSaveOppRecomendationSettings(DataRow dr, string strTabName,int iProjectId)
        {

            try
            {
                int iMaxId = 0;
                string strT1Feilds = "";
                string strT2Feilds = "";

                clsTObj.fnGetTimePeriodData(strTabName, dr["T1"].ToString().Split(',').ToArray(), dr["T2"].ToString().Split(',').ToArray(), int.Parse(dr["TIMEPERIOD_ID"].ToString()), iProjectId, ref iMaxId, ref strT1Feilds, ref strT2Feilds, true);

                clsTObj.fnGetSegmentData(strTabName, int.Parse(dr["TIMEPERIOD_ID"].ToString()), dr["CURRENTSEGMENT"].ToString(), dr["T2"].ToString(), dr["T2"].ToString().Split(';'),iProjectId, dr["SEGMENTISACTIVE"].ToString() == "1" ? true : false, ref iMaxId, ref strT1Feilds);

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private bool fnSaveThreshold(DataRow dr, string strTabName, int iProjectid)
        {
            try
            {
                string strPtnlFilter = "";
                clsTObj.fnSaveTREThreShold(dr["T1"].ToString().Split(','), dr["T2"].ToString().Split(','), dr["OPP_NAME"].ToString(), dr["DROPPERS_CUTOFF"].ToString(), dr["GROWERS_CUTOFF"].ToString(), dr["STOPPERS_CUTOFF"].ToString(), int.Parse(dr["OPPORTUNITY_ID"].ToString()), strTabName,iProjectid, dr["ELGBL_FORMULA"].ToString(),true);
                clsTObj.fnGetBaseData(strTabName, dr["GROWERS_CUTOFF"].ToString(),iProjectid);
                if (dr["PTNL_FORMULA"].ToString() != "")
                    clsTObj.fnSaveOPPPotential(dr["OPP_NAME"].ToString(), int.Parse(dr["OPPORTUNITY_ID"].ToString()), strTabName, dr["PTNL_FORMULA"].ToString());
               clsTObj.fnInsertOppValues(int.Parse(dr["OPPORTUNITY_ID"].ToString()));
                return true;
            }
            catch (Exception ex)
            {
                ((OraDBManager)Common.dbMgr).RollbackTrans();
                throw ex;
            }
        }
        private bool fnSaveThresholdfrmExport(DataRow dr, string strTabName, int iProjectid,string strMainFilter)
        {
            try
            {
                string strPtnlFilter = "";
                clsTObj.fnSaveTREThreSholdfrmExport(dr["T1"].ToString().Split(','), dr["T2"].ToString().Split(','), dr["OPP_NAME"].ToString(), dr["DROPPERS_CUTOFF"].ToString(), dr["GROWERS_CUTOFF"].ToString(), dr["STOPPERS_CUTOFF"].ToString(), int.Parse(dr["OPPORTUNITY_ID"].ToString()), strTabName, dr["ELGBL_FORMULA"].ToString(), iProjectid,strMainFilter, true);
                clsTObj.fnGetBaseData(strTabName, dr["GROWERS_CUTOFF"].ToString(), iProjectid);
                if (dr["PTNL_FORMULA"].ToString() != "")
                    clsTObj.fnSaveOPPPotentialfrmExport(dr["OPP_NAME"].ToString(), int.Parse(dr["OPPORTUNITY_ID"].ToString()), strTabName, dr["PTNL_FORMULA"].ToString());
                  //clsTObj.fnInsertOppValues(int.Parse(dr["OPPORTUNITY_ID"].ToString()));
                return true;
            }
            catch (Exception ex)
            {
                ((OraDBManager)Common.dbMgr).RollbackTrans();
                throw ex;
            }
        }
        public bool fnActiveOpportunities(string strOfferId)
        {
            try
            {
                string[] str = strOfferId.Split(';');
                if (str.Length > 0)
                {

                    string strSql = "UPDATE Opportunity SET  ISACTIVE = " + str[1] + " WHERE  OPPORTUNITY_ID = " + str[0];
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    if (str[1] == "0")
                    {
                        strSql = "UPDATE CAMPAIGNS SET  ISACTIVE = " + str[1] + " WHERE  OPPORTUNITY_ID = " + str[0];
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                        strSql = " DELETE FROM PRIORITIZED_TEMP  WHERE CAMPAIGNID=";
                        strSql += "(SELECT  nvl(Sum(CAMPAIGN_ID),0) FROM CAMPAIGNS WHERE OPPORTUNITY_ID=" + str[0]+")";
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
        public bool fnRunOPoortunitiesfrmExport(int iProjectId, string strTabName,string Timeperiod1, string Timeperiod2,string strMainFilter)
        {
            try
            {
                //((OraDBManager)Common.dbMgr).BeginTrans();

                DataTable dt = new DataTable();

                string strSql = "SELECT O.OPPORTUNITY_ID,O.OPP_NAME,O.FORMULA,O.ELGBL_FORMULA,O.PTNL_FORMULA,S.DROPPERS_CUTOFF ,S.STOPPERS_CUTOFF,S.GROWERS_CUTOFF,";
                strSql += " '" + Timeperiod1 + "' as T1,'" + Timeperiod2 + "' as T2,S.CURRENTSEGMENT,S.SEGMENTISACTIVE,TT.TIMEPERIOD_ID ";
                strSql += " FROM OPPORTUNITY O INNER JOIN STATUS_BREAKDOWN S ON O.OPPORTUNITY_ID=S.OPPORTUNITY_ID ";
                strSql += " LEFT JOIN TRE_TIMEPERIOD TT ON TT.T1='"+Timeperiod1 +"' AND TT.T2='"+Timeperiod2+"'";
                strSql += " WHERE O.PROJECT_ID= " + iProjectId;
                //strSql += " AND O.ISONMAIN=1";

                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                #region Comments by sravanthi
                /* Commented by Sravanthi

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!fnSaveOppRecomendationSettings(dt.Rows[i], strTabName, iProjectId))
                    {
                        ((OraDBManager)Common.dbMgr).RollbackTrans();
                        return false;
                    }
                    if (!fnSaveThresholdfrmExport(dt.Rows[i], strTabName, iProjectId,strMainFilter))
                    {
                        ((OraDBManager)Common.dbMgr).RollbackTrans();
                        return false;
                    }
                    //commented by arun
                    //strSql = "Update OPPORTUNITY A Set ISONMAIN=1 Where OPPORTUNITY_ID=" + dt.Rows[i]["OPPORTUNITY_ID"];
                    //dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                } */
                 #endregion

                if (!fnSaveOppRecomendationSettings(dt.Rows[0], strTabName, iProjectId))
                {
                    ((OraDBManager)Common.dbMgr).RollbackTrans();
                    return false;
                }

                strSql = " Declare";
                strSql += " BaseTable string(200);";
                strSql += " BEGIN";
                strSql += " BaseTable := 'ets_tre_base';";
                strSql += " TRE_GET_DELTASTATUS(BaseTable);";
                strSql += " END;";

                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                //((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                strSql = " Declare";
                strSql += " MainTableName string(200);";
                strSql += " Week int;";
                strSql += " BEGIN";
                strSql += " MainTableName := '" + strTabName + "_V'" + ";";
                strSql += " Week :=" + objclsTreDetails.fnMaxWeek(strTabName) + ";";
                strSql += " TRE_GET_PTNL(MainTableName, Week);";
                strSql += " END;";

                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
               // //((OraDBManager)Common.dbMgr).CommitTrans();
                return true;
            }
            catch (Exception ex)
            {
                ((OraDBManager)Common.dbMgr).RollbackTrans();
                throw ex;
            }
        }
        public bool fnRunOPoortunities(int iProjectId, string strTabName, string Timeperiod1, string Timeperiod2, string strMainFilter)
        {
            try
            {
                //((OraDBManager)Common.dbMgr).BeginTrans();

                DataTable dt = new DataTable();

                string strSql = "SELECT O.OPPORTUNITY_ID,O.OPP_NAME,O.FORMULA,O.ELGBL_FORMULA,O.PTNL_FORMULA,S.DROPPERS_CUTOFF ,S.STOPPERS_CUTOFF,S.GROWERS_CUTOFF,";
                strSql += " '" + Timeperiod1 + "' as T1,'" + Timeperiod2 + "' as T2,S.CURRENTSEGMENT,S.SEGMENTISACTIVE,TT.TIMEPERIOD_ID ";
                strSql += " FROM OPPORTUNITY O INNER JOIN STATUS_BREAKDOWN S ON O.OPPORTUNITY_ID=S.OPPORTUNITY_ID ";
                strSql += " LEFT JOIN TRE_TIMEPERIOD TT ON TT.T1='" + Timeperiod1 + "' AND TT.T2='" + Timeperiod2 + "'";
                strSql += " WHERE O.PROJECT_ID= " + iProjectId ;
               //Below is Commented bcz tre_ranknig checks on opportunities onmain=1 and tre_exportunityexport will have data of only onmain =0
                //strSql += " AND O.ISONMAIN=0";
               
                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                if (!fnSaveOppRecomendationSettings(dt.Rows[0], strTabName, iProjectId))
                {
                    ((OraDBManager)Common.dbMgr).RollbackTrans();
                    return false;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {//the below is commented and the code is added above to maintain ADM_WEEKLY_A,B and BASE tables instead of recreation
                    //if (!fnSaveOppRecomendationSettings(dt.Rows[i], strTabName, iProjectId))
                    //{
                    //    ((OraDBManager)Common.dbMgr).RollbackTrans();
                    //    return false;
                    //}
                    if (!fnSaveThresholdfrmExport(dt.Rows[i], strTabName, iProjectId, strMainFilter))
                    {
                        ((OraDBManager)Common.dbMgr).RollbackTrans();
                        return false;
                    }

                    strSql = "Update OPPORTUNITY A Set ISONMAIN=1 Where OPPORTUNITY_ID=" + dt.Rows[i]["OPPORTUNITY_ID"];
                    DataTable dtupdate = new DataTable();
                    dtupdate = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }

                // //((OraDBManager)Common.dbMgr).CommitTrans();
                return true;
            }
            catch (Exception ex)
            {
                ((OraDBManager)Common.dbMgr).RollbackTrans();
                throw ex;
            }
        }

        public bool fnRunOPoortunitiesfrmProcedure(int iProjectId, string strTabName, string Timeperiod1, string Timeperiod2)
        {
            try
            {
                //((OraDBManager)Common.dbMgr).BeginTrans();

                DataTable dt = new DataTable();

                string strSql = "SELECT O.OPPORTUNITY_ID,O.OPP_NAME,O.FORMULA,O.ELGBL_FORMULA,O.PTNL_FORMULA,S.DROPPERS_CUTOFF ,S.STOPPERS_CUTOFF,S.GROWERS_CUTOFF,";
                strSql += " '" + Timeperiod1 + "' as T1,'" + Timeperiod2 + "' as T2,S.CURRENTSEGMENT,S.SEGMENTISACTIVE,TT.TIMEPERIOD_ID ";
                strSql += " FROM OPPORTUNITY O INNER JOIN STATUS_BREAKDOWN S ON O.OPPORTUNITY_ID=S.OPPORTUNITY_ID ";
                strSql += " LEFT JOIN TRE_TIMEPERIOD TT ON TT.T1='" + Timeperiod1 + "' AND TT.T2='" + Timeperiod2 + "'";
                strSql += " WHERE O.PROJECT_ID= " + iProjectId;
                //Below is Commented bcz tre_ranknig checks on opportunities onmain=1 and tre_exportunityexport will have data of only onmain =0
                //strSql += " AND O.ISONMAIN=0";

                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                if (!fnSaveOppRecomendationSettings(dt.Rows[0], strTabName, iProjectId))
                {
                    ((OraDBManager)Common.dbMgr).RollbackTrans();
                    return false;
                }
                strSql = " Declare";
                strSql += " BaseTable string(200);";
                strSql += " BEGIN";
                strSql += " BaseTable := 'ets_tre_base';";
                strSql += " TRE_GET_DELTASTATUS(BaseTable);";
                strSql += " END;";

                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
            
                //((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                strSql= " Declare";
                strSql += " MainTableName string(200);";
                strSql += " Week int;";
                strSql += " BEGIN";
                strSql += " MainTableName := '" + strTabName+"_V'" + ";";
                strSql += " Week :=" + objclsTreDetails.fnMaxWeek(strTabName) +";";
                strSql += " TRE_GET_PTNL(MainTableName, Week);";
                strSql += " END;";

                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                // //((OraDBManager)Common.dbMgr).CommitTrans();
                return true;
       
            }
            catch (Exception ex)
            {
                ((OraDBManager)Common.dbMgr).RollbackTrans();
                throw ex;
            }
        }
    }
}
