using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RecoEngine_DataLayer;

namespace RecoEngine_BI
{
    public class clsTre_Details
    {
        public DataTable fnTREtimeperiods(string strTableName)
        {
            try
            {
                DataTable dt;
                string strSQl = "select distinct to_char(YEAR) || '-' || to_char(WEEK) as timeperiod,year,week  from " + strTableName + "  where YEAR is not null  and WEEK is not null  order by year desc, week desc";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSQl);
                else
                {
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSQl);
                }

                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable fnFillSegment(string strTabName)
        {
            try
            {
                DataTable dt;
                string strSql = "Select COLNAME from TRE_MAPPING where TABLENAME='" + strTabName + "' and type=" + ((int)Enums.ColType.Segment).ToString();
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
        private string fnBuildQuery(string[] str)
        {
            try
            {
                string strQuery = "";

                for (int i = 0; i < str.Length; i++)
                {
                    if (strQuery != "")
                        strQuery += " OR ";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        strQuery += " to_char(year) || '-' || to_char(week) = ";
                    else if (Common.iDBType == (int)Enums.DBType.SQl)
                        strQuery += " to_char(year) + '-' + to_char(week) = ";
                    strQuery += "'";
                    strQuery += str[i].Trim();
                    strQuery += "'";
                }

                return strQuery;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string fnBuildTimePeriod(string[] str)
        {
            try
            {
                string strQuery = "";

                for (int i = 0; i < str.Length; i++)
                {
                    if (strQuery != "")
                        strQuery += " , ";

                    strQuery += str[i].Trim();
                }

                return strQuery;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void fnGetBaseData(string strTabName, string strGrower, int iProjectId)
        {
            try
            {
                string strSql = "";
                string strSegmentString = "";
                string strKeyString = "";
                string strSegmentVString = "";
                string strKeyVString = "";
                string strKeyCString = "";
                string strETS_TRE_BASEString = "";
                string strETS_TRE_BASEVString = "";
                string strETS_TRE_BASE2String = "";
                string strETS_TRE_BASE2VString = "";
                string strETS_TRE_BASE3String = "";
                string strETS_TRE_BASE3VString = "";
                string str1 = "";
                string strV1 = "";

                DataTable dtTab = new DataTable();

                // strTabName = "TRE_DETAILS_NEW";
                strSql = "Select * from TRE_MAPPING where TABLENAME='" + strTabName + "' and PROJECTID=" + iProjectId;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dtTab = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dtTab = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                foreach (DataRow dr in dtTab.Rows)
                {

                    if (dr["type"].ToString() == ((int)Enums.ColType.Input).ToString())
                    {
                         if (dr["coldataType"].ToString() != "System.String" && dr["coldataType"].ToString() != "System.DateTime")
                        {

                            str1 = "";
                            strV1 = "";

                            if (strETS_TRE_BASEString != "")
                                strETS_TRE_BASEString += ",";

                            if (strETS_TRE_BASE2String != "")
                                strETS_TRE_BASE2String += ",";

                            if (strETS_TRE_BASE3String != "")
                                strETS_TRE_BASE3String += ",";

                            if (strETS_TRE_BASEVString != "")
                                strETS_TRE_BASEVString += ",";

                            if (strETS_TRE_BASE2VString != "")
                                strETS_TRE_BASE2VString += ",";

                            if (strETS_TRE_BASE3VString != "")
                                strETS_TRE_BASE3VString += ",";

                            str1 = "A_" + dr[0].ToString() + ",B_" + dr[0].ToString() + ",X_" + dr[0].ToString() + ",D_" + dr[0].ToString();



                            strV1 = "A_" + dr[0].ToString() + ",B_" + dr[0].ToString() + ",X_" + dr[0].ToString() + ",";
                            strV1 += " Case When B_" + dr[0].ToString() + " > 0 Then ROUND((A_" + dr[0].ToString() + "/B_" + dr[0].ToString() + ")-1,3) Else 0 End ";


                            strETS_TRE_BASEString += str1;
                            strETS_TRE_BASEVString += strV1;

                            str1 += ",S_" + dr[0].ToString();
                            // need to pass 0.25 value dynamically
                            strV1 = " ,CASE WHEN B_" + dr[0].ToString() + " = 0  AND A_" + dr[0].ToString() + " = 0 Then 'NON USER' ";
                            strV1 += "  WHEN B_" + dr[0].ToString() + " = 0  AND A_" + dr[0].ToString() + " > 0 Then 'NEW USER' ";
                            strV1 += "  WHEN B_" + dr[0].ToString() + " > 0  AND D_" + dr[0].ToString() + " = -1 Then 'STOPPER' ";
                            strV1 += "  WHEN D_" + dr[0].ToString() + " <= " + Convert.ToDecimal(strGrower) + " Then 'DROPPER' ";
                            strV1 += "  WHEN D_" + dr[0].ToString() + " >= " + Convert.ToDecimal(strGrower) + "  Then 'GROWER' ";
                            strV1 += "  ELSE 'FLAT' End ";

                            strETS_TRE_BASE2String += str1;
                            strETS_TRE_BASE2VString += "A_" + dr[0].ToString() + ",B_" + dr[0].ToString() + ",X_" + dr[0].ToString() + ",D_" + dr[0].ToString() + strV1;

                            str1 += ",P_" + dr[0].ToString();

                            strV1 = ", CASE WHEN S_" + dr[0].ToString() + " = 'NON USER' Then X_" + dr[0].ToString();
                            strV1 += "  WHEN S_" + dr[0].ToString() + " = 'DROPPER' then abs(D_" + dr[0].ToString() + ")";
                            strV1 += "  WHEN S_" + dr[0].ToString() + "= 'STOPPER' then B_" + dr[0].ToString() + "*" + Convert.ToDecimal(strGrower);
                            strV1 += "  WHEN S_" + dr[0].ToString() + "='FLAT' then a_" + dr[0].ToString() + "*" + Convert.ToDecimal(strGrower);
                            strV1 += "  ELSE 0 End ";

                            strETS_TRE_BASE3String += str1;
                            strETS_TRE_BASE3VString += "A_" + dr[0].ToString() + ",B_" + dr[0].ToString() + ",X_" + dr[0].ToString() + ",D_" + dr[0].ToString() + ",S_" + dr[0].ToString() + strV1;


                        }
                        }
                        else if (dr["type"].ToString() == ((int)Enums.ColType.Segment).ToString())
                        {
                            if (strSegmentString != "")
                                strSegmentString += ",";

                            if (strSegmentVString != "")
                                strSegmentVString += ",";

                            strSegmentVString += "A." + dr[0].ToString();
                            strSegmentString += dr[0].ToString();
                        }
                        else if (dr["type"].ToString() == ((int)Enums.ColType.Key).ToString())
                        {
                            if (strKeyString != "")
                                strKeyString += ",";

                            if (strKeyVString != "")
                                strKeyVString += ",";

                            if (strKeyCString != "")
                                strKeyCString += ",";

                            strKeyVString += "A." + dr[0].ToString();
                            strKeyString += dr[0].ToString();

                            strKeyCString += "A." + dr[0].ToString() + "=" + "B." + dr[0].ToString();
                        }
                    }
                

                strSql = "Delete from ETS_TRE_BASE2";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                }
                else
                {
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                }
                strSql = "Delete from ETS_TRE_BASE3";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                }
                else
                {
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                }

                string strInsertString = "Insert into ETS_TRE_BASE2(";

                if (strKeyString != "")
                    strInsertString += strKeyString + ",";

                if (strSegmentString != "")
                    strInsertString += strSegmentString + ",";

                if (strETS_TRE_BASE2String != "")
                    strInsertString += strETS_TRE_BASE2String;

                strInsertString += ") Select ";
                if (strKeyString != "")
                    strInsertString += strKeyString + ",";

                if (strSegmentString != "")
                    strInsertString += strSegmentString + ",";

                strInsertString += strETS_TRE_BASE2VString + " From ETS_TRE_BASE ";


                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertString);
                }
                else
                {
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertString);
                }


                strInsertString = "Insert into ETS_TRE_BASE3(";

                if (strKeyString != "")
                    strInsertString += strKeyString + ",";

                if (strSegmentString != "")
                    strInsertString += strSegmentString + ",";

                if (strETS_TRE_BASE3String != "")
                    strInsertString += strETS_TRE_BASE3String;

                strInsertString += ") Select ";
                if (strKeyString != "")
                    strInsertString += strKeyString + ",";

                if (strSegmentString != "")
                    strInsertString += strSegmentString + ",";

                strInsertString += strETS_TRE_BASE3VString + " From ETS_TRE_BASE2 ";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertString);
                }
                else
                {
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertString);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnGetSegmentData(string strTabName, int iTIMEPERIOD, string strSegmentColumn, string strT2String, string[] strT2, int iProjectId, bool isActiveChecked, ref int iCount, ref string strSegmentDataFeilds)
        {
            try
            {
                string strSql = "";
                string strInsertString = "Insert into ETS_TRE_X_SELL_PNTL(TIMEPERIOD,SegmentColName,CURRENTSEGMENT, ";
                string strInsertSString = "";
                string strInsertSValues = "";
                string strInsertValues = "";
                string strSegmentString = "";
                string strKeyString = "";
                string strSegmentVString = "";
                string strKeyVString = "";
                string strKeyCString = "";
                string strETS_TRE_BASEString = "";
                string strETS_TRE_BASEVString = "";
                string str1 = "";
                string strV1 = "";

                DataTable dtTab = new DataTable();

                // strTabName = "TRE_DETAILS_NEW";
                strSql = "Select * from TRE_MAPPING where TABLENAME='" + strTabName + "'" + " and  PROJECTID=" + iProjectId;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dtTab = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dtTab = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                foreach (DataRow dr in dtTab.Rows)
                {

                    if (dr["type"].ToString() == ((int)Enums.ColType.Input).ToString())
                    {
                         if (dr["coldataType"].ToString() != "System.String" && dr["coldataType"].ToString() != "System.DateTime")
                        {
                            str1 = "";
                            strV1 = "";

                            if (strInsertSString != "")
                                strInsertSString += ",";

                            if (strInsertSValues != "")
                                strInsertSValues += ",";

                            if (strSegmentDataFeilds != "")
                                strSegmentDataFeilds += ",";

                            strInsertSString += "X_" + dr[0].ToString();

                            if (isActiveChecked)
                                strInsertSValues += "CASE WHEN ROUND(SUM(REPLACE(A_" + dr[0].ToString() + ",',','.')),2) > 0 THEN ROUND(SUM(REPLACE(A_" + dr[0].ToString() + ",',','.'))/SUM(CASE WHEN REPLACE(A_" + dr[0].ToString() + ",',','.') > 0 THEN 1 ELSE 0 END),2) ELSE 0 END ";
                            else
                                strInsertSValues += " ROUND(Avg(A_" + dr[0].ToString() + "),2)";


                            //strInsertSUserString += "X_" + dr[0].ToString() + "_USR," + "X_" + dr[0].ToString() ;

                            //strInsertSUserValues += " SUM(CASE WHEN REPLACE(A_" + dr[0].ToString() + ",',','.') > 0 THEN 1 ELSE 0 END),";
                            //strInsertSUserValues += " SUM(REPLACE(A_" + dr[0].ToString() + ",',','.')) ";

                            strSegmentDataFeilds += "X_" + dr[0].ToString() + " as " + dr[0].ToString();


                            if (strETS_TRE_BASEString != "")
                                strETS_TRE_BASEString += ",";


                            if (strETS_TRE_BASEVString != "")
                                strETS_TRE_BASEVString += ",";



                            str1 = "A_" + dr[0].ToString() + ",B_" + dr[0].ToString() + ",X_" + dr[0].ToString() + ",D_" + dr[0].ToString();



                            strV1 = "A_" + dr[0].ToString() + ",B_" + dr[0].ToString() + ",X_" + dr[0].ToString() + ",";
                            strV1 += " Case When B_" + dr[0].ToString() + " > 0 Then ROUND((A_" + dr[0].ToString() + "/B_" + dr[0].ToString() + ")-1,3) Else 0 End ";


                            strETS_TRE_BASEString += str1;
                            strETS_TRE_BASEVString += strV1;
                        }

                    }
                    else if (dr["type"].ToString() == ((int)Enums.ColType.Segment).ToString())
                    {
                        if (strSegmentString != "")
                            strSegmentString += ",";

                        if (strSegmentVString != "")
                            strSegmentVString += ",";

                        strSegmentVString += "A." + dr[0].ToString();
                        strSegmentString += dr[0].ToString();
                    }
                    else if (dr["type"].ToString() == ((int)Enums.ColType.Key).ToString())
                    {
                        if (strKeyString != "")
                            strKeyString += ",";

                        if (strKeyVString != "")
                            strKeyVString += ",";

                        if (strKeyCString != "")
                            strKeyCString += ",";

                        strKeyVString += "A." + dr[0].ToString();
                        strKeyString += dr[0].ToString();

                        strKeyCString += "A." + dr[0].ToString() + "=" + "B." + dr[0].ToString();
                    }
                }


                //if (iCount == 0)
                //{

                strSql = "truncate Table ETS_TRE_X_SELL_PNTL";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                }
                else
                {
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                }

                strInsertValues = " Select '" + iTIMEPERIOD + "','" + strSegmentColumn + "'," + strSegmentColumn;

                if (strInsertSString != "")
                    strInsertString += strInsertSString;


                strInsertString += ")" + strInsertValues + "," + strInsertSValues + " From  ETS_ADM_WEEKLY_A  where TIMEPERIOD_ID=" + iTIMEPERIOD + " Group By " + strSegmentColumn;


                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertString);
                     
                    
                        //((OraDBManager)Common.dbMgr).CommitTrans();
                        //((OraDBManager)Common.dbMgr).BeginTrans();
                    
                }
                else
                {
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertString);
                }



                strSql = "Truncate Table ETS_TRE_BASE";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                }
                else
                {
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                }



                strInsertString = "Insert into ETS_TRE_BASE(";
                if (strKeyString != "")
                    strInsertString += strKeyString + ",";

                if (strSegmentString != "")
                    strInsertString += strSegmentString + ",";

                if (strETS_TRE_BASEString != "")
                    strInsertString += strETS_TRE_BASEString;

                strInsertString += ") Select ";
                if (strKeyVString != "")
                    strInsertString += strKeyVString + ",";

                if (strSegmentVString != "")
                    strInsertString += strSegmentVString + ",";

                strInsertString += strETS_TRE_BASEVString + " From ETS_ADM_WEEKLY_A A, ETS_ADM_WEEKLY_B B,ETS_TRE_X_SELL_PNTL C WHERE ";
                strInsertString += strKeyCString + " (+) " + " And A." + strSegmentColumn + " = C.CURRENTSEGMENT ";
                //where a.msisdn = b.msisdn (+) and nvl(b.A_decile,'Not Tagged') = c.a_current_segment;


                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertString);
                    //((OraDBManager)Common.dbMgr).CommitTrans();
                    //((OraDBManager)Common.dbMgr).BeginTrans();
                }
                else
                {
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertString);
                }



                //  }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void fnGetSegmentData_old(string strTabName, int iTIMEPERIOD, string strSegmentColumn, string strT2String, string[] strT2, ref int iCount, ref string strSegmentDataFeilds)
        {
            try
            {
                string strSql = "";

                string strInsertString = "Insert into ETS_TRE_X_SELL_PNTL(TIMEPERIOD,SegmentColName,CURRENTSEGMENT, ";
                string strInsertSString = "";
                string strInsertSValues = "";

                string strInsertValues = "";


                string strETS_TRE_BASEString = "";
                string strETS_TRE_BASEVString = "";

                DataTable dtTab = new DataTable();

                // strTabName = "TRE_DETAILS_NEW";
                strSql = "Select * from TRE_MAPPING where TABLENAME='" + strTabName + "' and type  =  " + ((int)Enums.ColType.Input).ToString();
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dtTab = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dtTab = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                foreach (DataRow dr in dtTab.Rows)
                {

                    if (dr["type"].ToString() == ((int)Enums.ColType.Input).ToString())
                    {
                        if (strInsertSString != "")
                            strInsertSString += ",";

                        if (strInsertSValues != "")
                            strInsertSValues += ",";

                        if (strSegmentDataFeilds != "")
                            strSegmentDataFeilds += ",";

                        strInsertSString += "X_" + dr[0].ToString();
                        strInsertSValues += " Round(Avg(A_" + dr[0].ToString() + "),2)";

                        strSegmentDataFeilds += "X_" + dr[0].ToString() + " as " + dr[0].ToString();
                    }
                }


                if (iCount == 0)
                {
                    strInsertValues = " Select '" + iTIMEPERIOD + "','" + strSegmentColumn + "'," + strSegmentColumn;

                    if (strInsertSString != "")
                        strInsertString += strInsertSString;


                    strInsertString += ")" + strInsertValues + "," + strInsertSValues + " From  ETS_ADM_WEEKLY_A  where TIMEPERIOD_ID=" + iTIMEPERIOD + " Group By " + strSegmentColumn;


                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertString);
                    }
                    else
                    {
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertString);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnGetTimePeriodData_old(string strTabName, string[] strT1, string[] strT2, int iTimePeriodID, ref int iMaxId, ref string strT1Feilds, ref string strT2Feilds)
        {
            try
            {
                string strSql = "";

                string strInsertTString = "Insert into ETS_ADM_WEEKLY(Id,TIMEPERIOD_ID ";
                string strInsertT1String = "";
                string strUpdateTString = "Update ETS_ADM_WEEKLY Set  ";
                string strUpdateT2String = "";
                string strUpdateT2Values = "";
                string strKeyString = "";
                string strSegmentString = "";


                string strTimeString = "";
                string strInsertT1Values = "";

                string strInsertTValues = "";

                DataTable dtTab = new DataTable();

                // strTabName = "TRE_DETAILS_NEW";
                strSql = "Select * from TRE_MAPPING where TABLENAME='" + strTabName + "'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dtTab = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dtTab = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                foreach (DataRow dr in dtTab.Rows)
                {

                    if (dr["type"].ToString() != ((int)Enums.ColType.None).ToString())
                    {
                        if (dr["type"].ToString() == ((int)Enums.ColType.Key).ToString())
                        {
                            if (strKeyString != "")
                                strKeyString += ",";
                            strKeyString += dr[0].ToString();
                        }
                        else if (dr["type"].ToString() == ((int)Enums.ColType.Time).ToString())
                        {
                            if (strTimeString != "")
                                strTimeString += ",";
                            strTimeString += dr[0].ToString();
                        }
                        else if (dr["type"].ToString() == ((int)Enums.ColType.Segment).ToString())
                        {
                            if (strSegmentString != "")
                                strSegmentString += ",";

                            strSegmentString += dr[0].ToString();
                        }
                        else if (dr["type"].ToString() == ((int)Enums.ColType.Input).ToString())
                        {
                            if (strInsertT1String != "")
                                strInsertT1String += ",";

                            if (strUpdateT2String != "")
                                strUpdateT2String += ",";

                            if (strInsertT1Values != "")
                                strInsertT1Values += ",";

                            if (strUpdateT2Values != "")
                                strUpdateT2Values += ",";
                            if (strT1Feilds != "")
                                strT1Feilds += ",";

                            if (strT2Feilds != "")
                                strT2Feilds += ",";

                            strInsertT1String += "T1_" + dr[0].ToString();
                            strInsertT1Values += " Round(Avg(" + dr[0].ToString() + "),2)";


                            strT1Feilds += "T1_" + dr[0].ToString() + " as " + dr[0].ToString();
                            strT2Feilds += "T2_" + dr[0].ToString() + " as " + dr[0].ToString();

                            strUpdateT2String += "T2_" + dr[0].ToString();
                            strUpdateT2Values += " Round(Avg(" + dr[0].ToString() + "),2)";

                        }
                    }
                }


                if (iMaxId == 0)
                {

                    strSql = "Select NVL(MAX(ID), 0)+1 AS MAX_VAL from ETS_ADM_WEEKLY";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        iMaxId = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                    else
                        iMaxId = int.Parse(((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));

                    strInsertTValues = " Select " + iMaxId + "," + iTimePeriodID;


                    if (strInsertT1String != "")
                        strInsertTString += "," + strInsertT1String;


                    strInsertTString += ")" + strInsertTValues + "," + strInsertT1Values + " From  " + strTabName + " where " + fnBuildQuery(strT1);




                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertTString);
                    }
                    else
                    {
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertTString);
                    }
                    strUpdateTString = "Update ETS_ADM_WEEKLY Set  ( ";
                    strUpdateTString += strUpdateT2String + " ) =( Select  " + strUpdateT2Values + " From " + strTabName + " where " + fnBuildQuery(strT2) + ") Where Id=" + iMaxId;
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strUpdateTString);
                    }
                    else
                    {
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strUpdateTString);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnGetTimePeriodData(string strTabName, string[] strT1, string[] strT2, int iTimePeriodID, int iProjectId, ref int iMaxId, ref string strT1Feilds, ref string strT2Feilds, bool bIsONMain = false)
        {
            try
            {
                string strSql = "";

                string strKeyString = "";
                string strKeyVString = "";
                string strSegmentString = "";

                string strInsertAString = "";
                string strInsertBString = "";
                string strUpdateAVString = "";
                string strInsertAVString = "";
                string strInsertBVString = "";

                string strTimeString = "";


                string strInsertValues = "";


                DataTable dtTab = new DataTable();

                // strTabName = "TRE_DETAILS_NEW";
                strSql = "Select * from TRE_MAPPING where TABLENAME='" + strTabName + "' and PROJECTID=" + iProjectId;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dtTab = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dtTab = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                string strMainfilter = "";
                if (!bIsONMain)
                    strTabName = "TRE_RANDOM";
                if (strTabName != "TRE_RANDOM")
                {
                    strTabName = strTabName + "_V";
                    DataTable dt = new DataTable();
                    strSql = "Select FILTER FROM FILTER_MAIN WHERE PROJECT_ID=" + iProjectId;
                    dt = (((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql));
                    if (dt.Rows.Count > 0)
                    {
                        strMainfilter = dt.Rows[0]["FILTER"].ToString();
                    }


                }
                foreach (DataRow dr in dtTab.Rows)
                {

                    if (dr["type"].ToString() != ((int)Enums.ColType.None).ToString())
                    {
                        if (dr["type"].ToString() == ((int)Enums.ColType.Key).ToString())
                        {
                            if (strKeyString != "")
                                strKeyString += ",";
                            strKeyString += dr[0].ToString();
                            strKeyVString = "A." + dr[0].ToString() + "= B." + dr[0].ToString();
                        }
                        else if (dr["type"].ToString() == ((int)Enums.ColType.Time).ToString())
                        {
                            if (strTimeString != "")
                                strTimeString += ",";
                            strTimeString += dr[0].ToString();
                        }
                        else if (dr["type"].ToString() == ((int)Enums.ColType.Segment).ToString())
                        {
                            if (strSegmentString != "")
                                strSegmentString += ",";

                            strSegmentString += dr[0].ToString();
                        }
                        else if (dr["type"].ToString() == ((int)Enums.ColType.Input).ToString())
                        {
                             if (dr["coldataType"].ToString() != "System.String" && dr["coldataType"].ToString() != "System.DateTime")
                            {
                                if (strT1Feilds != "")
                                    strT1Feilds += ",";

                                if (strT2Feilds != "")
                                    strT2Feilds += ",";

                                if (strInsertAString != "")
                                    strInsertAString += ",";

                                if (strInsertAVString != "")
                                    strInsertAVString += ",";


                                if (strInsertBString != "")
                                    strInsertBString += ",";

                                if (strInsertBVString != "")
                                    strInsertBVString += ",";

                                if (strUpdateAVString != "")
                                    strUpdateAVString += ",";

                                strInsertAString += "A_" + dr[0].ToString();
                                strInsertAVString += " Round(Avg(" + dr[0].ToString() + "),2)";

                                strInsertBString += "B_" + dr[0].ToString();
                                strInsertBVString += " Round(Avg(" + dr[0].ToString() + "),2)";

                                strT1Feilds += "Round(Avg(A_" + dr[0].ToString() + "),2) as " + dr[0].ToString();
                                strT2Feilds += "Round(Avg(B_" + dr[0].ToString() + "),2) as " + dr[0].ToString();

                            }
                        }
                    }
                }


                if (iMaxId == 0)
                {

                    strInsertValues = " Select  " + iTimePeriodID;
                    string strKeySegmentString = "";
                    if (strKeyString != "")
                    {
                        strKeySegmentString += strKeyString;
                        //strInsertValues += "," + strKeyString;
                        //strInsertATable += "," + strKeyString;
                        //strInsertBTable += "," + strKeyString;
                    }

                    if (strSegmentString != "")
                    {
                        if (strKeySegmentString != "")
                            strKeySegmentString += "," + strSegmentString;
                    }
                    if (strKeySegmentString != "")
                        strInsertValues += "," + strKeySegmentString;


                    strSql = "truncate table  ETS_ADM_WEEKLY_A";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                    }
                    else
                    {
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                    }

                    strSql = "truncate table  ETS_ADM_WEEKLY_B";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                    }
                    else
                    {
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                    }
                    string[] strlastT2 = { strT2.First() };
                    //string strInsertATable = "Insert into ETS_ADM_WEEKLY_A(TIMEPERIOD_ID ";

                    //strInsertATable += "," + strKeySegmentString + "," + strInsertAString + ")" + strInsertValues + "," + strInsertAVString + " From  " + strTabName;
                    //strInsertATable += " where " + fnBuildQuery(strT2) + " Group by " + strKeySegmentString +")";



                    string strInsertATable = "Insert into ETS_ADM_WEEKLY_A(TIMEPERIOD_ID ";
                    strInsertATable += "," + strInsertAString + "," + strKeySegmentString + ")";
                    strInsertATable += " Select " + iTimePeriodID + ",G.*," + strSegmentString + " FROM";
                    strInsertATable += " (Select " + strInsertAVString + "," + strKeyString + " as CUSTOMER From ";
                    strInsertATable += strTabName + " where" + fnBuildQuery(strT2) + " Group by " + strKeyString + ")G ,";
                    strInsertATable += " (Select " + strKeySegmentString + " From " + strTabName + " where" + fnBuildQuery(strlastT2);
                    if (strMainfilter != "")
                        strInsertATable += " and " + strMainfilter;
                    strInsertATable += " Group by " + strKeySegmentString + ")K where K.CUSTOMER=G.CUSTOMER";



                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertATable);
                        if (bIsONMain)
                        {
                            //((OraDBManager)Common.dbMgr).CommitTrans();
                            //((OraDBManager)Common.dbMgr).BeginTrans();
                        }
                    }
                    else
                    {
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertATable);
                    }

                    string strInsertBTable = "Insert into ETS_ADM_WEEKLY_B(TIMEPERIOD_ID ";

                    strInsertValues = " Select  " + iTimePeriodID;
                    if (strKeyString != "")
                    {
                        strInsertValues += "," + strKeyString;
                        strInsertBTable += "," + strKeyString;
                    }

                    strInsertBTable += "," + strInsertBString + ")" + strInsertValues + "," + strInsertBVString + " From " + strTabName;
                    strInsertBTable += " where " + fnBuildQuery(strT1);
                    if (strMainfilter != "")
                    {
                        strInsertBTable += " and " + strMainfilter;
                    }

                     strInsertBTable +=" Group by " + strKeyString;

                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertBTable);
                        if (bIsONMain)
                        {
                            //((OraDBManager)Common.dbMgr).CommitTrans();
                            //((OraDBManager)Common.dbMgr).BeginTrans();
                        }
                    }
                    else
                    {
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strInsertBTable);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable fnGetOpportunityDetails(int iProjectId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtOpp = new DataTable();
                string strSql = "Select * from OPPORTUNITY WHERE PROJECT_ID=" + iProjectId + " AND ISONMAIN=0 ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                if (dt.Rows.Count > 0)
                {
                    string strCol = "T.Customer,";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strCol != "")
                            //strCol += ",";

                            strCol += dt.Rows[i]["OPP_NAME"].ToString() + "_DELTA," + dt.Rows[i]["OPP_NAME"].ToString() + "_STATUS," + dt.Rows[i]["OPP_NAME"].ToString() + "_PNTL,";
                    }

                    strCol = strCol.Remove(strCol.Length - 1);
                    strSql = "Select WEEK, " + strCol + ",Rank1,Rank1_Action,Rank2,Rank2_Action,Rank3,Rank3_Action,Rank4,Rank4_Action from  TRE_OPPORTUNITY T  Left Join Tre_Ranking R  ON R.CUSTOMER=T.CUSTOMER";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        dtOpp = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                    else if (Common.iDBType == (int)Enums.DBType.SQl)
                        dtOpp = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                }



                return dtOpp;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable fnGetSegmentData(string strTabName, string[] strT1, string[] strT2, string strSegmentColName, int iProjectId, bool isActiveChecked)
        {
            try
            {
                DataTable dt = new DataTable();
                int iCount = 0;
                string strT2String = fnBuildTimePeriod(strT2);
                string strT1String = fnBuildTimePeriod(strT1);
                string strSegmentDataFeilds = "";
                int iTimePeriodID = 0;


                string strSql = "Select TIMEPERIOD_ID from TRE_TIMEPERIOD WHERE T1='" + strT1String + "' AND  T2='" + strT2String + "'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);


                if (dt.Rows.Count > 0)
                {
                    iTimePeriodID = int.Parse(dt.Rows[dt.Rows.Count - 1][0].ToString());
                }


                strSql = "Select count(1) as x from ETS_TRE_X_SELL_PNTL WHERE TIMEPERIOD='" + iTimePeriodID + "' AND SEGMENTCOLNAME ='" + strSegmentColName + "'";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));


                // fnGetSegmentData(string strTabName, string strSegmentColumn,string strT2String, string[] strT2, ref int iMaxId, ref string strSegmentDataFeilds)
                fnGetSegmentData(strTabName, iTimePeriodID, strSegmentColName, strT2String, strT2, iProjectId, isActiveChecked, ref iCount, ref strSegmentDataFeilds);

                 strSql = " Select CURRENTSEGMENT ," + strSegmentDataFeilds + " from ETS_TRE_X_SELL_PNTL where  TIMEPERIOD = '" + iTimePeriodID + "' AND SEGMENTCOLNAME ='" + strSegmentColName + "' order by CURRENTSEGMENT";


                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);


                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void fnInsertBaseData(string strTabName, int iTimePeriodID, string strSegmentColName)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable fnTREtimePeriodData(string[] strT1, string[] strT2, string strTableName, int iProjectId)
        {
            try
            {
                DataTable dt = new DataTable();
                int iTimePeriodID = 0;

                string strT1Feilds = "";
                string strT2Feilds = "";
                string strT1String = fnBuildTimePeriod(strT1);
                string strT2String = fnBuildTimePeriod(strT2);

                string strSql = "Select TIMEPERIOD_ID from TRE_TIMEPERIOD WHERE T1='" + strT1String + "' And T2='" + strT2String + "'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);


                if (dt.Rows.Count > 0)
                {
                    iTimePeriodID = int.Parse(dt.Rows[dt.Rows.Count - 1][0].ToString());
                }
                else
                {
                    strSql = "insert into TRE_TIMEPERIOD (T1,T2) Values ('" + strT1String + "','" + strT2String + "')";

                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                    else
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);

                    strSql = "Select NVL(MAX(TIMEPERIOD_ID), 0) AS TIMEPERIOD_ID from TRE_TIMEPERIOD";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        iTimePeriodID = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                    else
                        iTimePeriodID = int.Parse(((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }


                strSql = "Select * from ETS_ADM_WEEKLY_A where TIMEPERIOD_ID=" + iTimePeriodID;

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                int iMaxId = 0;
                if (dt.Rows.Count > 0)
                {
                    iMaxId = int.Parse(dt.Rows[0]["TIMEPERIOD_ID"].ToString());
                }

                fnGetTimePeriodData(strTableName, strT1, strT2, iTimePeriodID, iProjectId, ref iMaxId, ref strT1Feilds, ref strT2Feilds);


                strSql = " Select 'T2' as Period , " + strT1Feilds + " from ETS_ADM_WEEKLY_A where  TIMEPERIOD_ID = " + iTimePeriodID;
                strSql += " Union ";
                strSql += " Select 'T1' as Period , " + strT2Feilds + " from ETS_ADM_WEEKLY_B where TIMEPERIOD_ID = " + iTimePeriodID;


                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);


                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnSaveOPPBreakDownStatus(int iOPPId, decimal dCtDropper, decimal dCtGrower, decimal dCtStoppeer, string[] strT1, string[] strT2, string strCurrentSegmentColumn, int iISActive)
        {
            string sT1 = fnBuildTimePeriod(strT1);
            string sT2 = fnBuildTimePeriod(strT2);
            try
            {
                string strSql = "Delete from STATUS_BREAKDOWN where OPPORTUNITY_ID= " + iOPPId;

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                strSql = "Insert into STATUS_BREAKDOWN (OPPORTUNITY_ID,DROPPERS_CUTOFF,STOPPERS_CUTOFF,GROWERS_CUTOFF,";
                strSql += " T1,T2,CURRENTSEGMENT,SEGMENTISACTIVE) values (" + iOPPId + "," + dCtDropper + "," + dCtStoppeer + "," + dCtGrower + ",";
                strSql += "'" + sT1 + "','" + sT2 + "','" + strCurrentSegmentColumn + "'," + iISActive + ")";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnSaveOPPBreakDownStatus(int iOPPId, decimal dCtFlat, decimal dCtDropper, decimal dCtGrower, decimal dCtStoppeer, decimal dCtNonUser, decimal dCtNewUser,
        decimal dCFlat, decimal dCDropper, decimal dCStoppeer, decimal dCGrower, decimal dCNonUser, decimal dCNewUser,
        decimal dAVGFlat, decimal dAVGDropper, decimal dAVGStoppeer, decimal dAVGGrower, decimal dAVGNonUser, decimal dAVGNewUser, string[] strT1, string[] strT2, string strCurrentSegmentColumn, int iISActive)
        {
            string sT1 = fnBuildTimePeriod(strT1);
            string sT2 = fnBuildTimePeriod(strT2);
            try
            {
                string strSql = "Delete from STATUS_BREAKDOWN where OPPORTUNITY_ID= " + iOPPId;

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                strSql = "Insert into STATUS_BREAKDOWN (OPPORTUNITY_ID,FLAT_CUTOFF,DROPPERS_CUTOFF,STOPPERS_CUTOFF,GROWERS_CUTOFF,NONUSERS_CUTOFF,NEWUSERS_CUTOFF,";
                strSql += " FLAT_COUNT,DROPPERS_COUNT,STOPPERS_COUNT,GROWERS_COUNT,NONUSERS_COUNT,NEWUSERS_COUNT,FLAT_AVG,DROPPERS_AVG,STOPPERS_AVG,";
                strSql += " GROWERS_AVG,NONUSERS_AVG,NEWUSERS_AVG,T1,T2,CURRENTSEGMENT,SEGMENTISACTIVE) values (" + iOPPId + ",0 ," + dCtDropper + "," + dCtStoppeer + "," + dCtGrower + ",";
                strSql += dCtNonUser + "," + dCtNewUser + "," + dCFlat + "," + dCDropper + "," + dCStoppeer + "," + dCGrower + "," + dCNonUser + "," + dCNewUser + ",";
                strSql += dAVGFlat + "," + dAVGDropper + "," + dAVGStoppeer + "," + dAVGGrower + "," + dAVGNonUser + "," + dAVGNewUser + ",'" + sT1 + "','" + sT2 + "','" + strCurrentSegmentColumn + "'," + iISActive + ")";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable fnGetTREThreShold(string[] strT1, string[] strT2, string strFormula, string strPtnlFilter, string strDropper, string strGrower, string strStopper, int iOpportunityId, string strTabName)
        {
            try
            {
                DataTable dt = new DataTable();


                //string strSql = " Update TRE_OPPORTUNITY A Set " + strOppName.ToUpper() + "_STATUS=";
                //strSql += " (Select " + strOppName.ToUpper() + "_DELTA from TRE_OPPORTUNITY B where A.CUSTOMER=B.CUSTOMER)";
                //strSql += " where Exists (select 1 from TRE_OPPORTUNITY O , TRE_OPPORTUNITY T where O.CUSTOMER=T.CUSTOMER AND O.CUSTOMER=A.CUSTOMER )";
                //((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                //string strSql = "Select * from OPPORTUNITY where OPPORTUNITY_ID = " + iOpportunityId;
                //if (Common.iDBType == (int)Enums.DBType.Oracle)
                //    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                //else
                //    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                //string strFormula = "";

                //if (dt.Rows.Count > 0)
                //{
                //    strFormula = dt.Rows[0]["FORMULA"].ToString();
                //}
                string strSql = "select S.Customer,S.T1,S.T2,S.Delta,S.Status from (";
                strSql += " Select NVL(A.CUSTOMER,B.CUSTOMER) CUSTOMER, A.T1, B.T2,Case When T1=0 then 0 Else Round(T2/T1-1,2) End DELTA,";
                strSql += " Case When A.T1+B.T2 =0 Then 'NON_USER' ";
                strSql += " When A.T1=0 And B.T2>0 Then 'NEW_USER' ";
                strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End <= " + Convert.ToDecimal(strStopper) + " Then 'STOPPER' ";
                strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End <= " + Convert.ToDecimal(strDropper) + " Then 'DROPPER' ";
                strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End >  " + Convert.ToDecimal(strGrower) + " Then 'GROWER' ";
                strSql += " ELSE 'FLAT' END as Status From";
                strSql += "(Select CUSTOMER, round(avg(" + strFormula.ToUpper() + "),2) T1 from " + strTabName;
                strSql += " where " + fnBuildQuery(strT1) + "  group by CUSTOMER) A ";
                strSql += " Left join (Select CUSTOMER, round(avg(" + strFormula.ToUpper() + "),2) T2 from  " + strTabName + " where  " + fnBuildQuery(strT2) + " group by CUSTOMER) B ";
                strSql += " On A.CUSTOMER=B.CUSTOMER )S inner join";
                strSql += "(Select Distinct CUSTOMER from  " + strTabName;
                if (strPtnlFilter != "")
                    strSql += " where  " + strPtnlFilter;
                strSql += ")G on S.CUSTOMER=G.CUSTOMER";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {

                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
                else if (Common.iDBType == (int)Enums.DBType.SQl)
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
        public DataTable fnGetTREThreSholdForNewOPP(string[] strT1, string[] strT2, string strFormula, string strDropper, string strGrower, string strStopper, int iOpportunityId, string strTabName)
        {
            try
            {
                DataTable dt = new DataTable();

                string strSql = "Select NVL(A.CUSTOMER,B.CUSTOMER) CUSTOMER, A.T1, B.T2,Case When T1=0 then 0 Else Round(T2/T1-1,2) End DELTA,";
                strSql += " Case When A.T1+B.T2 =0 Then 'NON_USER' ";
                strSql += " When A.T1=0 And B.T2>0 Then 'NEW_USER' ";
                strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End < " + Convert.ToDecimal(strStopper) + " Then 'STOPPER' ";
                strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End < " + Convert.ToDecimal(strDropper) + " Then 'DROPPER' ";
                strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End >  " + Convert.ToDecimal(strGrower) + " Then 'GROWER' ";
                strSql += " ELSE 'FLAT' END as Status From";
                strSql += "(Select CUSTOMER, round(avg(" + strFormula.ToUpper() + "),2) T1 from " + strTabName;
                strSql += " where " + fnBuildQuery(strT1) + "  group by CUSTOMER) A ";
                strSql += " Left join (Select CUSTOMER, round(avg(" + strFormula.ToUpper() + "),2) T2 from  " + strTabName + " where  " + fnBuildQuery(strT2) + " group by CUSTOMER) B ";
                strSql += " On A.CUSTOMER=B.CUSTOMER";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {

                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
                else if (Common.iDBType == (int)Enums.DBType.SQl)
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
        public bool fnSaveTREThreShold(string[] strT1, string[] strT2, string strOppName, string strDropper, string strGrower, string strStopper, int iOpportunityId, string strTableName,int iProjectid, string strPtnlFilter,  bool bIsONMain = false)
        {
            try
            {
                DataTable dtSource = new DataTable();
                string strSql = "Select * from OPPORTUNITY where OPPORTUNITY_ID = " + iOpportunityId;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dtSource = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dtSource = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                string strFormula = "";
                if (!bIsONMain)
                    strTableName = "TRE_RANDOM";
                if (dtSource.Rows.Count > 0)
                {
                    strFormula = dtSource.Rows[0]["FORMULA"].ToString();
                   int iCount = 0;
                    strSql = "Select * from TRE_MAPPING where PROJECTID= " + iProjectid + " AND  TYPE=" + ((int)Enums.ColType.Key).ToString() + " OR TYPE=" + ((int)Enums.ColType.Time).ToString();
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
                        int MaxWeek = fnMaxWeek(strTableName);
                        string strKeyString = "";
                        string strKColName = "";
                        string strKACol = "";

                        for (int i = 0; i < dtSource.Rows.Count; i++)
                        {
                            if (strColName != "")
                            {
                                strColName += ",";
                                strUpdateString += " And ";
                                strUpdateMString += " And ";
                            }

                            if (dtSource.Rows[i]["TYPE"].ToString() == ((int)Enums.ColType.Key).ToString())
                            {
                                if (strKeyString != "")
                                    strKeyString += " AND ";

                                if (strKColName != "")
                                    strKColName += ",";

                                if (strKACol != "")
                                    strKACol += ",";

                                strKeyString += "A." + dtSource.Rows[i]["COLNAME"].ToString() + "=B." + dtSource.Rows[i]["COLNAME"].ToString();

                                strKACol += "A." + dtSource.Rows[i]["COLNAME"].ToString();
                                strKColName += dtSource.Rows[i]["COLNAME"].ToString();

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
                            strSql = " INSERT INTO TRE_OPPORTUNITY(CUSTOMER,WEEK," + strOppName.ToUpper() + "_DELTA," + strOppName.ToUpper() + "_STATUS)";
                            strSql += " select A.CUSTOMER,A.WEEK, Case When T1=0 then 0 Else Round(T2/T1-1,2) END, ";
                            strSql += " Case When A.T1+B.T2 =0 Then 'NON_USER' ";
                            strSql += " When A.T1=0 And B.T2>0 Then 'NEW_USER' ";
                            strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End <= " + Convert.ToDecimal(strStopper) + " Then 'STOPPER' ";
                            strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End <= " + Convert.ToDecimal(strDropper) + " Then 'DROPPER' ";
                            strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End >=  " + Convert.ToDecimal(strGrower) + " Then 'GROWER' ";
                            strSql += " ELSE 'FLAT' END as Status From  (Select CUSTOMER," + MaxWeek + " as Week ,round(avg(" + strFormula.ToUpper() + "),2) T1 from " + strTableName;
                            strSql += " where " + strBT1 + "  group by CUSTOMER) A ";
                            strSql += " Left join (Select CUSTOMER, round(avg(" + strFormula.ToUpper() + "),2) T2 from " + strTableName + " where  " + strBT2 + " group by CUSTOMER) B ";
                            strSql += " On " + strKeyString;


                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                             //   //((OraDBManager)Common.dbMgr).CommitTrans();
                            }
                              
                            else
                                ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                        }
                        else
                        {
                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {

                                strSql = " Delete From TRE_OPP_TEMP";
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                strSql = " INSERT INTO TRE_OPP_TEMP(CUSTOMER,WEEK,DELTA,STATUS) select S.Customer,S.Week,S.DELTA,S.STATUS from (";
                                strSql += " select A.CUSTOMER,A.Week, Case When T1=0 then 0 Else Round(T2/T1-1,2) END as DELTA, ";
                                strSql += " Case When A.T1+B.T2 =0 Then 'NON_USER' ";
                                strSql += " When A.T1=0 And B.T2>0 Then 'NEW_USER' ";
                                strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End < " + Convert.ToDecimal(strStopper) + " Then 'STOPPER' ";
                                strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End < " + Convert.ToDecimal(strDropper) + " Then 'DROPPER' ";
                                strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End >  " + Convert.ToDecimal(strGrower) + " Then 'GROWER' ";
                                strSql += " ELSE 'FLAT' END as Status From  (Select CUSTOMER," + MaxWeek + " as Week,round(avg(" + strFormula.ToUpper() + "),2) T1 from " + strTableName;
                                strSql += " where " + strBT1 + "  group by CUSTOMER) A ";
                                strSql += " Left join (Select CUSTOMER, round(avg(" + strFormula.ToUpper() + "),2) T2 from " + strTableName + " where  " + strBT2 + " group by CUSTOMER) B ";
                                strSql += " On " + strKeyString + " )S inner join";

                                strSql += "(Select Distinct CUSTOMER from " + strTableName;
                                if (strPtnlFilter != "")
                                    strSql += " where " + strPtnlFilter;
                                strSql += ")G on S.CUSTOMER=G.CUSTOMER";

                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = "   INSERT INTO TRE_OPP_TEMP (CUSTOMER,WEEK, DELTA,STATUS) select Customer,"+MaxWeek+" ,null, 'NA'  from " + strTableName;
                                strSql += " A Where A.Customer not in (Select Customer from TRE_OPP_TEMP)  Group By Customer";
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                //strSql = " Update TRE_OPPORTUNITY A Set (" + strOppName.ToUpper() + "_DELTA," + strOppName.ToUpper() + "_STATUS," + "WEEK)=";
                                //strSql += " (Select DELTA,STATUS,WEEK from TRE_OPP_TEMP B where A.CUSTOMER=B.CUSTOMER)";
                                //strSql += " where Exists (select 1 from TRE_OPPORTUNITY O , TRE_OPP_TEMP T where O.CUSTOMER=T.CUSTOMER AND O.CUSTOMER=A.CUSTOMER )";
                                //((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                //as every customer is not updated commented previous one
                                strSql = " Update TRE_OPPORTUNITY A Set (" + strOppName.ToUpper() + "_DELTA," + strOppName.ToUpper() + "_STATUS," + "WEEK)=";
                                strSql += " (Select DELTA,STATUS,WEEK from TRE_OPP_TEMP B where A.CUSTOMER=B.CUSTOMER)";
                                strSql += " where Exists (select 1 from TRE_OPPORTUNITY O , TRE_OPP_TEMP T where O.CUSTOMER=T.CUSTOMER  )";
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                               // //((OraDBManager)Common.dbMgr).CommitTrans();
                                if (bIsONMain)
                                {
                                    strSql = " INSERT INTO TRE_OPPORTUNITY(CUSTOMER,WEEK," + strOppName.ToUpper() + "_DELTA," + strOppName.ToUpper() + "_STATUS)";
                                    strSql += " Select Z.Customer,Z.WEEK,Z.DELTA,Z.STATUS FROM (select S.Customer,S.WEEK,S.DELTA,S.STATUS from ";
                                    strSql += " (select A.CUSTOMER,B.WEEK, Case When T1=0 then 0 Else Round(T2/T1-1,2) END as DELTA, ";
                                    strSql += " Case When A.T1+B.T2 =0 Then 'NON_USER' ";
                                    strSql += " When A.T1=0 And B.T2>0 Then 'NEW_USER' ";
                                    strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End < " + Convert.ToDecimal(strStopper) + " Then 'STOPPER' ";
                                    strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End < " + Convert.ToDecimal(strDropper) + " Then 'DROPPER' ";
                                    strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End > " + Convert.ToDecimal(strGrower) + " Then 'GROWER' ";
                                    strSql += " ELSE 'FLAT' END as Status From  (Select CUSTOMER, round(avg(" + strFormula.ToUpper() + "),2) T1 from " + strTableName;
                                    strSql += " where " + strBT1 + "  group by CUSTOMER) A ";
                                    strSql += " Left join (Select CUSTOMER," + MaxWeek + " as Week, round(avg(" + strFormula.ToUpper() + "),2) T2 from " + strTableName + " where  " + strBT2 + " group by CUSTOMER) B ";
                                    strSql += " On " + strKeyString + ")S inner join";
                                    strSql += "(Select Distinct CUSTOMER from  " + strTableName;
                                    if (strPtnlFilter != "")
                                        strSql += " where " + strPtnlFilter;
                                    strSql += " )G on S.CUSTOMER=G.CUSTOMER)Z";
                                    strSql += " Where Z.Customer not in (Select Customer from TRE_OPPORTUNITY)";

                                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                    strSql = "INSERT INTO TRE_OPPORTUNITY (CUSTOMER,WEEK," + strOppName.ToUpper() + "_DELTA," + strOppName.ToUpper() + "_STATUS) ";
                                    strSql += "select Customer,"+MaxWeek+",null, 'NA'  from " + strTableName + " A ";
                                    strSql += "Where A.Customer not in (Select Customer from TRE_OPPORTUNITY)  Group By Customer";

                                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                   // //((OraDBManager)Common.dbMgr).CommitTrans();
                                }

                            }
                            else
                            {
                                //strSql = " Update TRE_OPPORTUNITY Set (" + strOppName + ")=";
                                //strSql += " (Select " + strFormula.Replace("'", "''") + " from " + strTableName + " where " + strUpdateString + ")";
                                //((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                            }
                        }



                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int fnMaxWeek(string strTableName)
        {
            try
            {
                string strSql = "";
                strSql = " SELECT Max(WEEK) FROM  " + strTableName;
                return int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql)); ;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool fnSaveOPPPotential(string strOppName, int iOpportunityId, string strTableName, string strFormula)
        {
            try
            {
                DataTable dtSource = new DataTable();
                string strSql = "";
                //strFormula=strFormula.Replace("'","");
                strSql = "Update OPPORTUNITY Set PTNL_FORMULA='" + strFormula.ToUpper().Replace("'", "''") + "' where OPPORTUNITY_ID = " + iOpportunityId;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                else
                    ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {

                    strSql = " Update TRE_OPPORTUNITY A Set " + strOppName.ToUpper() + "_PNTL=0";
                    strSql += " WHERE " + strOppName.ToUpper() + "_STATUS='NA' ";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    strSql = " Update TRE_OPPORTUNITY A Set " + strOppName.ToUpper() + "_PNTL=";
                    strSql += " (Select " + strFormula + " from ETS_TRE_BASE2 B where A.CUSTOMER=B.CUSTOMER)";
                    strSql += " WHERE EXISTS (SELECT 1 from TRE_OPPORTUNITY O , ETS_TRE_BASE2 T   where  O.CUSTOMER=T.CUSTOMER AND O.CUSTOMER=A.CUSTOMER AND O." + strOppName.ToUpper() + "_STATUS!='NA' )";
                    //strSql = " Update TRE_OPPORTUNITY A Set " + strOppName.ToUpper() + "_PNTL=";
                    //strSql += " (Select " + strFormula + " from ETS_TRE_BASE2 B where A.CUSTOMER=B.CUSTOMER)";
                    //strSql += " where Exists (select 1 from TRE_OPPORTUNITY O , ETS_TRE_BASE2 T where O.CUSTOMER=T.CUSTOMER AND O.CUSTOMER=A.CUSTOMER)";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }
                else
                {
                    //strSql = " Update TRE_OPPORTUNITY Set (" + strOppName + ")=";
                    //strSql += " (Select " + strFormula.Replace("'", "''") + " from " + strTableName + " where " + strUpdateString + ")";
                    //((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool fnInsertOppValues(int oppid)
        {
            DataTable dt = new DataTable();
            string strSQL = "Delete From Opportunity_values where opportunity_id =" + oppid;
            ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSQL);
            strSQL = "Select  OPP_NAME From Opportunity where opportunity_id =" + oppid;
            dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSQL);
            string Oppname = dt.Rows[0]["OPP_NAME"].ToString();
            strSQL = "Insert into Opportunity_values Select " + oppid + " , sum(" + Oppname + "_delta),sum(" + Oppname + "_pntl) from tre_opportunity ";
            if (Common.iDBType == (int)Enums.DBType.Oracle)
            {
                ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSQL);
            }
            return true;
        }
        public DataTable fnGetOppBreakDowndetails(int iOpportunityId)
        {
            try
            {
                DataTable dt = new DataTable();

                string strSQL = "Select * from STATUS_BREAKDOWN where OPPORTUNITY_ID=" + iOpportunityId;

                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSQL);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int fnInsertTREtimePeriodfrmExport(string[] strT1, string[] strT2, string strTableName, int iProjectId)
        {
            try
            {
                DataTable dt = new DataTable();
                int iTimePeriodID = 0;

                string strT1Feilds = "";
                string strT2Feilds = "";
                string strT1String = fnBuildTimePeriod(strT1);
                string strT2String = fnBuildTimePeriod(strT2);

                string strSql = "Select TIMEPERIOD_ID from TRE_TIMEPERIOD WHERE T1='" + strT1String + "' And T2='" + strT2String + "'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else if (Common.iDBType == (int)Enums.DBType.SQl)
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);


                if (dt.Rows.Count > 0)
                {
                    iTimePeriodID = int.Parse(dt.Rows[dt.Rows.Count - 1][0].ToString());
                }
                else
                {
                    strSql = "insert into TRE_TIMEPERIOD (T1,T2) Values ('" + strT1String + "','" + strT2String + "')";

                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                    else
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);

                    strSql = "Select NVL(MAX(TIMEPERIOD_ID), 0) AS TIMEPERIOD_ID from TRE_TIMEPERIOD";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                        iTimePeriodID = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                    else
                        iTimePeriodID = int.Parse(((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                return iTimePeriodID;

            }
          catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool fnSaveTREThreSholdfrmExport(string[] strT1, string[] strT2, string strOppName, string strDropper, string strGrower, string strStopper, int iOpportunityId, string strTableName, string strPtnlFilter, int iProjectid,string strMainFilter, bool bIsONMain = false)
        {
        try
            {
                //((OraDBManager)Common.dbMgr).BeginTrans();
               string  strSql = "alter table TRE_OPPORTUNITYEXPORT add " + strOppName.ToUpper() + "_DELTA NUMBER(18,2) ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                else
                    ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                strSql = "alter table TRE_OPPORTUNITYEXPORT add " + strOppName.ToUpper() + "_STATUS varchar2(50) ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                else
                    ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                strSql = "alter table TRE_OPPORTUNITYEXPORT add " + strOppName.ToUpper() + "_PNTL NUMBER(18,2) ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                else
                    ((DBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                DataTable dtSource = new DataTable();

                 strSql = "Select * from OPPORTUNITY where OPPORTUNITY_ID = " + iOpportunityId;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dtSource = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                else
                    dtSource = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                string strFormula = "";
                       if (dtSource.Rows.Count > 0)
                {
                    strFormula = dtSource.Rows[0]["FORMULA"].ToString();
                    int iCount = 0;
                    strSql = "Select * from TRE_MAPPING where PROJECTID= " + iProjectid + " AND  TYPE=" + ((int)Enums.ColType.Key).ToString() + " OR TYPE=" + ((int)Enums.ColType.Time).ToString();
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
                        int MaxWeek = fnMaxWeek(strTableName);
                        string strKeyString = "";
                        string strKColName = "";
                        string strKACol = "";

                        for (int i = 0; i < dtSource.Rows.Count; i++)
                        {
                            if (strColName != "")
                            {
                                strColName += ",";
                                strUpdateString += " And ";
                                strUpdateMString += " And ";
                            }

                            if (dtSource.Rows[i]["TYPE"].ToString() == ((int)Enums.ColType.Key).ToString())
                            {
                                if (strKeyString != "")
                                    strKeyString += " AND ";

                                if (strKColName != "")
                                    strKColName += ",";

                                if (strKACol != "")
                                    strKACol += ",";

                                strKeyString += "A." + dtSource.Rows[i]["COLNAME"].ToString() + "=B." + dtSource.Rows[i]["COLNAME"].ToString();

                                strKACol += "A." + dtSource.Rows[i]["COLNAME"].ToString();
                                strKColName += dtSource.Rows[i]["COLNAME"].ToString();

                            }

                            strColName += dtSource.Rows[i]["COLNAME"].ToString();
                            strUpdateString += "A." + dtSource.Rows[i]["COLNAME"].ToString() + "=B." + dtSource.Rows[i]["COLNAME"].ToString();

                            strUpdateMString += "OPT." + dtSource.Rows[i]["COLNAME"].ToString() + "=TDN." + dtSource.Rows[i]["COLNAME"].ToString();
                            strUpdateMString += " AND OPT." + dtSource.Rows[i]["COLNAME"].ToString() + "=A." + dtSource.Rows[i]["COLNAME"].ToString();
                           }

                                   
                                   if (Common.iDBType == (int)Enums.DBType.Oracle)
                                   {
                                       ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                                   }
                                   else
                                   {
                                       ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                                   }


                                   strSql = " Select Count(*) from  TRE_OPPORTUNITYEXPORT";
                                   strTableName = strTableName + "_V";
                                   int rowscount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                                   if (rowscount == 0)
                                   {
                                       strSql = " INSERT INTO TRE_OPPORTUNITYEXPORT(CUSTOMER,WEEK," + strOppName.ToUpper() + "_DELTA," + strOppName.ToUpper() + "_STATUS)";
                                       strSql += " Select Z.Customer,Z.WEEK,Z.DELTA,Z.STATUS FROM (select S.Customer,S.WEEK,S.DELTA,S.STATUS from ";
                                       strSql += " (select A.CUSTOMER,B.WEEK, Case When T1=0 then 0 Else Round(T2/T1-1,2) END as DELTA, ";
                                       strSql += " Case When A.T1+B.T2 =0 Then 'NON_USER' ";
                                       strSql += " When A.T1=0 And B.T2>0 Then 'NEW_USER' ";
                                       strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End <= " + Convert.ToDecimal(strStopper) + " Then 'STOPPER' ";
                                       strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End <= " + Convert.ToDecimal(strDropper) + " Then 'DROPPER' ";
                                       strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End >= " + Convert.ToDecimal(strGrower) + " Then 'GROWER' ";
                                       strSql += " ELSE 'FLAT' END as Status From  (Select CUSTOMER, round(avg(" + strFormula.ToUpper() + "),2) T1 from " + strTableName;
                                       strSql += " where " + strBT1 + "  group by CUSTOMER) A ";
                                       strSql += " Left join (Select CUSTOMER," + MaxWeek + " as Week, round(avg(" + strFormula.ToUpper() + "),2) T2 from " + strTableName + " where  " + strBT2 + " group by CUSTOMER) B ";
                                       strSql += " On " + strKeyString + ")S inner join";
                                       strSql += "(Select Distinct CUSTOMER from  " + strTableName;
                                       if (strPtnlFilter != "")
                                           strSql += " where " + strPtnlFilter;
                                       if (strMainFilter != "" && strMainFilter != strPtnlFilter)
                                       { 
                                            if(strPtnlFilter != "")
                                            {
                                           strSql +=  " and " + strMainFilter;
                                            }
                                           else
                                            strSql +=" where " + strMainFilter;
                                       }
                                       strSql += " )G on S.CUSTOMER=G.CUSTOMER)Z";
                                       strSql += " Where Z.Customer not in (Select Customer from TRE_OPPORTUNITYEXPORT)";

                                       ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                       strSql = "INSERT INTO TRE_OPPORTUNITYEXPORT (CUSTOMER,WEEK," + strOppName.ToUpper() + "_DELTA," + strOppName.ToUpper() + "_STATUS)";
                                       strSql += "select Customer,"+MaxWeek+",null,'NA'  from " + strTableName + " A ";
                                       strSql += "Where A.Customer not in (Select Customer from TRE_OPPORTUNITYEXPORT)  Group By Customer";
                                       ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                       //((OraDBManager)Common.dbMgr).CommitTrans();
                                       //((OraDBManager)Common.dbMgr).BeginTrans();
                                   }
                                   else
                                   {
                                       strSql = " Delete From TRE_OPP_TEMP";
                                       ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                       strSql = " INSERT INTO TRE_OPP_TEMP(CUSTOMER,WEEK,DELTA,STATUS) select S.Customer,S.Week,S.DELTA,S.STATUS from (";
                                       strSql += " select A.CUSTOMER,A.Week, Case When T1=0 then 0 Else Round(T2/T1-1,2) END as DELTA, ";
                                       strSql += " Case When A.T1+B.T2 =0 Then 'NON_USER' ";
                                       strSql += " When A.T1=0 And B.T2>0 Then 'NEW_USER' ";
                                       strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End <= " + Convert.ToDecimal(strStopper) + " Then 'STOPPER' ";
                                       strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End <= " + Convert.ToDecimal(strDropper) + " Then 'DROPPER' ";
                                       strSql += " When Case When T1=0 then 0 Else Round(T2/T1-1,2) End >=  " + Convert.ToDecimal(strGrower) + " Then 'GROWER' ";
                                       strSql += " ELSE 'FLAT' END as Status From  (Select CUSTOMER," + MaxWeek + " as Week,round(avg(" + strFormula.ToUpper() + "),2) T1 from " + strTableName;
                                       strSql += " where " + strBT1 + "  group by CUSTOMER) A ";
                                       strSql += " Left join (Select CUSTOMER, round(avg(" + strFormula.ToUpper() + "),2) T2 from " + strTableName + " where  " + strBT2 + " group by CUSTOMER) B ";
                                       strSql += " On " + strKeyString + " )S inner join";

                                       strSql += "(Select Distinct CUSTOMER from " + strTableName;
                                       if (strPtnlFilter != "")
                                           strSql += " where " + strPtnlFilter;
                                       if (strMainFilter != "" && strMainFilter != strPtnlFilter)
                                       {
                                           if (strPtnlFilter != "")
                                           {
                                               strSql += " and " + strMainFilter;
                                           }
                                           else
                                               strSql += " where " + strMainFilter;
                                       }
                                       
                                       strSql += ")G on S.CUSTOMER=G.CUSTOMER";

                                       ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                       strSql = "   INSERT INTO TRE_OPP_TEMP (CUSTOMER, DELTA,STATUS) select Customer,null, 'NA'  from " + strTableName;
                                       strSql += " A Where A.Customer not in (Select Customer from TRE_OPP_TEMP)  Group By Customer";
                                       ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                       //Sravanthi
                                       string strTblCount = "";
                                       strTblCount = "Select count(*) From TRE_OPPORTUNITYEXPORT";
                                       int iTblCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strTblCount));

                                       string strTabCount = iTblCount.ToString();
                                       string strSubCnt = strTabCount.Substring(1, (strTabCount.Length - 1));

                                       int iCalCnt = Convert.ToInt32(strTabCount) - Convert.ToInt32(strSubCnt);

                                       for (int i = 1; i <= iCalCnt / (iCalCnt * 0.1); i++)
                                       {

                                           strSql = "Declare";
                                           strSql += " oppNameDelta string(200);";
                                           strSql += " oppNameStatus string(200);";
                                           strSql+=" minID int;";
                                           strSql+=" maxID int;";
                                           strSql+=" BEGIN";
                                           strSql += " oppNameDelta:='" + strOppName.ToUpper() + "_DELTA'" + ";";
                                           strSql +=" oppNameStatus:='" + strOppName.ToUpper() + "_STATUS'" + ";";
                                           strSql += " minID:=" + (i - 1) * (iCalCnt * 0.1) + ";";
                                           strSql += " maxID:=";
                                           if (i == (iCalCnt / (iCalCnt * 0.1)))
                                           {
                                               strSql += iCalCnt + ";";
                                           }
                                           else
                                           {
                                               strSql += (i) * (iCalCnt * 0.1) + ";";
                                           }
                                           strSql += " STPROC_UPDATE_DELTASTATUS1(oppNameDelta,oppNameStatus,minID,maxID);";
                                           strSql+=" END;";

                                           #region Comments
                                           // strSql = " Update TRE_OPPORTUNITYEXPORT A Set (" + strOppName.ToUpper() + "_DELTA," + strOppName.ToUpper() + "_STATUS )=";
                                           // strSql += " (Select DELTA,STATUS from TRE_OPP_TEMP B where A.CUSTOMER=B.CUSTOMER ) ";
                                           //strSql +=" WHERE A.ID >=" + (i - 1) * (iCalCnt * 0.1) + " AND A.ID < ";

                                           // if (i == (iCalCnt / (iCalCnt * 0.1)))
                                           // {
                                           //     strSql +=   iCalCnt ;
                                           // }
                                           // else
                                           // {
                                           //     strSql +=(i) * (iCalCnt * 0.1);
                                           // }

                                           //  strSql += " AND Exists (select 1 from TRE_OPPORTUNITYEXPORT O , TRE_OPP_TEMP T where O.CUSTOMER=T.CUSTOMER AND O.CUSTOMER=A.CUSTOMER )";
                                          
                                             //

                                           //strSql = " Insert into TRE_OPPORTUNITYEXPORT A Select " + strOppName.ToUpper() + "_DELTA," + strOppName.ToUpper() + "_STATUS ";
                                           //strSql += strOppName.ToUpper() + "_PNTL From TRE_OPP_TEMP B where A.CUSTOMER=B.CUSTOMER ";
                                           //strSql += "WHERE A.ID >=" + (i - 1) * (iCalCnt * 0.1) + " AND A.ID < ";

                                           //if (i == (iCalCnt / (iCalCnt * 0.1)))
                                           //{
                                           //    strSql += iCalCnt;
                                           //}
                                           //else
                                           //{
                                           //    strSql += (i) * (iCalCnt * 0.1);
                                           //}

                                           //strSql += " AND Exists (select 1 from TRE_OPPORTUNITYEXPORT O , TRE_OPP_TEMP T where O.CUSTOMER=T.CUSTOMER AND O.CUSTOMER=A.CUSTOMER )";
                                          
                                           #endregion

                                           ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                       }
                                   
                                   }
                            }
                            else
                            {
                              
                            }
                        }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool fnSaveOPPPotentialfrmExport(string strOppName, int iOpportunityId, string strTableName, string strFormula)
        {
            try
            {
                DataTable dtSource = new DataTable();
                string strSql = "";
                

                string strTblCount = "";
                //((OraDBManager)Common.dbMgr).BeginTrans();
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {

                    strTblCount = "Select count(*) From TRE_OPPORTUNITYEXPORT";
                    int iTblCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strTblCount));

                    string strTabCount = iTblCount.ToString();
                    string strSubCnt=strTabCount.Substring(1, (strTabCount.Length-1));

                    int iCalCnt = Convert.ToInt32(strTabCount) - Convert.ToInt32(strSubCnt);

                    for (int i = 1; i <= iCalCnt / (iCalCnt * 0.1); i++)
                    {
                        strSql="declare";
                        strSql+=" OP_RECHARGE_PNTL string(200);";
                        strSql+=" OP_RECHARGE_STATUS string(200);";
                        strSql+=" formula string(5000);";
                        strSql+=" minID int;";
                        strSql+=" maxID int;";
                        strSql+=" BEGIN";
                        strSql+=" OP_RECHARGE_PNTL:='"+strOppName.ToUpper() + "_PNTL'"+";";
                        strSql+=" OP_RECHARGE_STATUS:='" + strOppName.ToUpper() + "_STATUS'" + ";";
                        strSql+= " formula:='" + strFormula.Replace("'", "''") + "';";
                        strSql+= " minID:="+(i - 1) * (iCalCnt * 0.1)+";";
                        strSql+= " maxID:=";
                        if (i == (iCalCnt / (iCalCnt * 0.1)))
                        {
                            strSql += iCalCnt + ";";
                        }
                        else
                        {
                            strSql += (i) * (iCalCnt * 0.1)+";";
                        }
                        strSql += " stproc_update_PNTLStatus(OP_RECHARGE_PNTL,OP_RECHARGE_STATUS,formula,minID,maxID);";
                        strSql+= " END;";

                        #region Commented

                        /* Commented by Sravanthi
                        strSql = " UPDATE TRE_OPPORTUNITYEXPORT A Set " + strOppName.ToUpper() + "_PNTL=(CASE WHEN " +
                            strOppName.ToUpper() + "_STATUS='NA' THEN 0 ELSE (SELECT " + strFormula + 
                            " from ETS_TRE_BASE2 B where A.CUSTOMER=B.CUSTOMER) END) WHERE A.ID >=" + 
                            (i - 1) * (iCalCnt * 0.1) + " AND A.ID < ";

                        if (i == (iCalCnt / (iCalCnt * 0.1)))
                        {
                            strSql += iCalCnt;
                        }
                        else
                        {
                           strSql +=  (i) * (iCalCnt * 0.1);
                        }
                         * /

                        /* Commented by Sravanthi
                        strSql = " Update TRE_OPPORTUNITYEXPORT  A  Set " + strOppName.ToUpper() + "_PNTL=0";
                        strSql += " WHERE " + strOppName.ToUpper() + "_STATUS='NA'  And A.ID >=" + (i - 1) * (iCalCnt * 0.1) + " AND A.ID < ";

                        if (i == (iCalCnt / (iCalCnt * 0.1)))
                        {
                            strSql += iCalCnt;
                        }
                        else
                        {
                           strSql +=  (i) * (iCalCnt * 0.1);
                        }
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                       
                        strSql = " Update TRE_OPPORTUNITYEXPORT A  Set " + strOppName.ToUpper() + "_PNTL=";
                        strSql += " (Select " + strFormula + " from ETS_TRE_BASE2 B  where A.CUSTOMER=B.CUSTOMER)";
                        strSql += "  Where A.ID >=" + (i - 1) * (iCalCnt * 0.1) + " AND A.ID < ";

                        if (i == (iCalCnt / (iCalCnt * 0.1)))
                        {
                            strSql += iCalCnt;
                        }
                        else
                        {
                            strSql += (i) * (iCalCnt * 0.1);
                        }

                           strSql+= "AND  EXISTS (SELECT 1 from TRE_OPPORTUNITYEXPORT O , ETS_TRE_BASE2 T    where  O.CUSTOMER=T.CUSTOMER AND O.CUSTOMER=A.CUSTOMER AND O." + strOppName.ToUpper() + "_STATUS!='NA' )";
                        */
                        #endregion

                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    }
                }
                else
                {
               
                }
                //((OraDBManager)Common.dbMgr).CommitTrans();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public bool fnDeleteTreOppfrmExport()
        //{
   
        //    string strSql = "select count(*)  from user_tables where table_name = 'TRE_OPPORTUNITYEXPORT'";
        //     int i=  int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql)); 
        //     if (i > 0)
        //     {

        //         strSql = "DROP TABLE TRE_OPPORTUNITYEXPORT";
        //         if (Common.iDBType == (int)Enums.DBType.Oracle)
        //         {
        //             ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
        //         }
        //         else
        //         {
        //             ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
        //         }

        //         strSql = "DROP SEQUENCE tbl_seq";
        //         ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);

        //         //strSql = "DROP TRIGGER tbl_trigr";
        //         //((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);

        //     }
        //    strSql = "CREATE TABLE TRE_OPPORTUNITYEXPORT (ID NUMBER NOT NULL, CUSTOMER varchar2(50)  NULL,";
        //    strSql += "WEEK number(2)  NULL )  NOLOGGING";
            
        //    if (Common.iDBType == (int)Enums.DBType.Oracle)
        //    {
        //        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
        //    }
        //    else
        //    {
        //        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
        //    }

        //    strSql = "ALTER TABLE TRE_OPPORTUNITYEXPORT ADD (CONSTRAINT Id_pk PRIMARY KEY (ID))";

        //    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);

        //    strSql = "CREATE INDEX TRE_OPPOEXPORT_IX ON TRE_OPPORTUNITYEXPORT(CUSTOMER)";

        //    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);

        //     strSql= "CREATE SEQUENCE tbl_seq";

        //     ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
           
        //    strSql = "CREATE OR REPLACE TRIGGER tbl_trigr BEFORE INSERT ON TRE_OPPORTUNITYEXPORT FOR EACH ROW BEGIN SELECT tbl_seq.NEXTVAL INTO :new.ID FROM dual; END;";

        //    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
            
        //    return true;
        //}

        public void fnCreateTableView(string TableName,string Columns, string filter)
        {
            try
            {
                string strSql = "Create Table " + TableName + "_V as Select " + Columns + " from " + TableName;
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                strSql = "CREATE INDEX " + TableName + "_V_IX on " + TableName + "_V" + "(CUSTOMER)";
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //try
            //{
            //    string strSql = "Create OR REPLACE View " + TableName + "_V as Select " + Columns + " from " + TableName;
                //if (filter != "")
                //    strSql += " where " + filter;

             //  ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                //strSql="CREATE INDEX VIEW_"+TableName+"_IX ON "+TableName+"(CUSTOMER)";
                //((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
          //  }
          //  catch (Exception ex)
          //  {
           //     throw ex;
           // }
        
        }

        public void fnCreateTableTab(string TableName, string Columns, string filter)
        {
            try
            {
                 string strSql ="";
                    strSql= "select count(*)  from user_tables where table_name = '"+TableName + "_V'";
                   int i=  int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                   if (i > 0)
                   {

                       strSql = "DROP TABLE  " + TableName + "_V";
                       if (Common.iDBType == (int)Enums.DBType.Oracle)
                       {
                           ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                       }
                       else
                       {
                           ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                       }
                   }
                 strSql = "Create Table " + TableName + "_V as Select " + Columns + " from " + TableName+ " nologging";
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                strSql = "CREATE INDEX " + TableName + "_V_IX on " + TableName+"_V" + "(CUSTOMER)";
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void fnDropTableTab(string TableName)
        {
            try
            {
                string strSql = "DROP Table " + TableName + "_V";
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable fnPreviewExpressionEditor(string TableName)
        {
            try
            {
                DataTable dt = new DataTable();
               // strSql = "Select  " + strString + " from " + strTableName + " Where ROWNUM <=2";
                string strSQL = "Select * from "+TableName+" where ROWNUM<=100";

                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSQL);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}