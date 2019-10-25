using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RecoEngine_DataLayer;


namespace RecoEngine_BI
{
    public class clsDataSource
    {
        public DataTable fnGetTreDetailsSchema(string strTabName)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSql = "";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    strSql = "select * from " + strTabName + " where ROWNUM <= 2";
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
                else
                {
                    strSql = "select top 1 * from " + strTabName;
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
                DataTableReader dr = new DataTableReader(dt);
                DataTable dtSchema = dr.GetSchemaTable();
                dr.Close();
                dr = null;
                return dtSchema;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable fnGetTreDetails(string strTabName)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSql = "";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                  // strSql = "select * from " + strTabName + " where ROWNUM <= 100";
                    if (strTabName == "Tre_Random")
                        strSql = "Select * from " + strTabName;
                    else
                    strSql = "Select * from " + strTabName + "  where ROWNUM <= 100";
                 
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
                else
                {
                    strSql = "select top 100 * from " + strTabName;
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }

                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public void fnDeleteTreMapping(string strTabName)
        {
            try
            {
                string strSql = "delete from TRE_MAPPING where TABLENAME='" + strTabName + "'";
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

        //public bool fnAddColumnToMainTable(string strTabName, string strColName, string strFormula, ref string strMessage)
        //{
        //    try
        //    {
        //        int iCount = 0;
        //        string strSql = " Select count(1) from user_tab_columns where upper(table_name) = '" + strTabName.ToUpper() + "' and upper(column_name) = '" + strColName.ToUpper() + "'";
        //        if (Common.iDBType == (int)Enums.DBType.Oracle)
        //        {
        //            iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
        //        }
        //        if (iCount > 0)
        //        {
        //            strMessage = strColName + " is already there.";
        //            return false;
        //        }
        //        //((OraDBManager)Common.dbMgr).BeginTrans();
        //        strSql = "Insert into TRE_CALCULATED_COLUMNS (COLNAME,COMBINE_COLUMNS) values ('" + strColName.ToUpper() + "','" + strFormula.Replace("'", "''") + "')";

        //        if (Common.iDBType == (int)Enums.DBType.Oracle)
        //        {
        //            ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
        //        }

        //        strSql = " Alter Table " + strTabName + " add " + strColName.ToUpper() + " number (18,2) ";
        //        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

        //        try
        //        {
        //            strSql = " Update " + strTabName + " Set " + strColName.ToUpper() + " = " + strFormula;
        //            ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
        //        }
        //        catch (Exception)
        //        {
        //            strSql = " Alter Table " + strTabName + " DROP COLUMN " + strColName.ToUpper();
        //            ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
        //        }


        //        //((OraDBManager)Common.dbMgr).CommitTrans();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ((OraDBManager)Common.dbMgr).RollbackTrans();
        //        throw ex;
        //    }

        //}
        public bool fnUpdateCalaculatedColumn(string strTabName, string strColName, string strFormula, int ProjectId)
        {
            try
            {
                string strSql = "update TRE_CALCULATED_COLUMNS Set COMBINE_COLUMNS= '" + strFormula + "' Where Project_Id= "+ ProjectId  + " and COLNAME ='" +strColName +"'";
               
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                else
                    ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                       

                return true;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
       }

        public bool fnAddCalaculatedColumn(string strTabName, string strColName, string strFormula, ref string strMessage, int ProjectId)
        {
            try
            {
                DataTable dt= new DataTable();
                int iCount = 0;
                //  strSql = " select Count(1) from   TRE_RANDOM";

                string strSql = " Select count(1) from user_tab_columns where upper(table_name) = '" + strTabName.ToUpper() + "' and upper(column_name) = '" + strColName.ToUpper() + "'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                if (iCount > 0)
                {
                    strMessage = strColName + " is already there.";
                    return false;
                }
                        

                 strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'TEMP_CALACULATED'";

                 if (Common.iDBType == (int)Enums.DBType.Oracle)
                 {
                     iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                 }

                if (iCount >= 1)
                {

                    strSql = " DROP TABLE TEMP_CALACULATED";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }

                
                    strSql = " CREATE TABLE  TEMP_CALACULATED AS select " + strFormula + "  " + strColName + " from " + strTabName + " where rownum<=1";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                strSql = " SELECT COLUMN_NAME, DATA_TYPE FROM user_tab_columns WHERE table_name = 'TEMP_CALACULATED'";
                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                strSql = "Insert into TRE_CALCULATED_COLUMNS (COLNAME,COMBINE_COLUMNS,PROJECT_ID,COLDATATYPE,TABLENAME) values ('" + strColName.ToUpper() + "','" + strFormula.Replace("'", "''") + "', " + ProjectId + " ,'" + dt.Rows[0]["DATA_TYPE"] + "','" + strTabName + "')";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                }

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return true;
        }
        public DataTable fnGetCalaculatedColMappingData(int ProjectId, string strTabName)
        {
            try
            {
                DataTable dt;
                string strSql = "Select * from  TRE_CALCULATED_COLUMNS WHERE TABLENAME= '"+strTabName + "' AND  PROJECT_ID=" + ProjectId;
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
        public DataTable fnGetColMappingData(int ProjectID)
        {
            try
            {
                DataTable dt;
                string strSql = "";

                strSql = "Select * from TRE_MAPPING Where ProjectId ="+ProjectID;

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

        public DataTable fnValidateExpressEditor(string strString, string strTableName, int iExpressionFor)
        {
            try
            {
                DataTable dt;
                string strSql = "";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    if (iExpressionFor == (int)Enums.ExpressionType.AddColumn || iExpressionFor== (int)Enums.ExpressionType.CalaculatedColumn || iExpressionFor == (int)Enums.ExpressionType.Opp_ptnl)
                        strSql = "Select  " + strString + " from " + strTableName + " Where ROWNUM <=2";
                    else
                        strSql = "Select  1 from " + strTableName + " Where " + strString + " and  ROWNUM <=2";
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
                else
                {
                    strSql = "Select Top 1 " + strString + " from " + strTableName;
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
        //public DataTable fnGetExpressionEditorColumns()
        //{
        //    try
        //    {
        //        DataTable dt;
        //        string strSql = "";

        //        strSql = "Select * from TRE_MAPPING";// Where ISREQUIRED=1 AND TYPE=" + (int)Enums.ColType.Input;

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
        public void fnInserFilter(string strFilterCondition, int ProjectId)
        {
            try
            {

                int iCount = 0;
                string strSql = "Select Count(1) FROM FILTER_MAIN WHERE PROJECT_ID=" + ProjectId;

                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                if (iCount > 0)
                {
                    strSql = "DELETE  FROM FILTER_MAIN WHERE PROJECT_ID=" + ProjectId;
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }
                else 
                {
                    strSql = "INSERT  INTO FILTER_MAIN (PROJECT_ID,FILTER) VALUES ( " + ProjectId + ",'" + strFilterCondition.Replace("'","''") + "' )";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                    }           
                
                }

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        public string fnselectFilterCondition(int iProjectid)
        {

            string strSql = "";
            string strFilter = "";
            DataTable dt = new DataTable();
            strSql = "Select FILTER FROM FILTER_MAIN WHERE PROJECT_ID=" + iProjectid;
            dt = (((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql));
            if (dt.Rows.Count > 0)
            {
                strFilter = dt.Rows[0]["FILTER"].ToString();
            }

            return strFilter;
        }
        public void fnInsertTreMApping(DataRow[] drs, string strTabName, string strFilerCondition,int ProjectId)
        {
            try
            {
                string strSql = "";
                // fnDeleteTreMapping(strTabName);
                string strCreateTString = "Create Table ETS_ADM_WEEKLY_A (";
                string strCreateBString = "Create Table ETS_ADM_WEEKLY_B (";
                string strKeyString = "";
                string strSegmentString = "";
                string strCreateTString1 = "";
                string strCreateBString1 = "";
                string strSellPtnlTableString = "";
                string strSellPtnlUserTableString = "";
                string strBaseString = "Create Table ETS_TRE_BASE (";
                string strBase2String = "Create Table ETS_TRE_BASE2 (";
                string strBase3String = "Create Table ETS_TRE_BASE3 (";
                string strBaseString1 = "";
                string strBase2String1 = "";
                string strBase3String1 = "";
                int iETS_ADM_WEEKLYExisits = 0;
                int iETS_TRE_X_SELL_PNTLExisits = 0;
                int iETS_TRE_BASE_Exisits = 0;
                int iETS_TRE_BASE2_Exisits = 0;
                int iETS_TRE_BASE3_Exisits = 0;
                //int iTRE_Random = 0;
                string strRandomColumns = "";
                int iCount = 0;
               
                strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'ETS_ADM_WEEKLY_A'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                if (iCount >= 1)
                {

                    strSql = " DROP TABLE ETS_ADM_WEEKLY_A";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }

                strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'ETS_ADM_WEEKLY_B'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                if (iCount >= 1)
                {

                    strSql = " DROP TABLE ETS_ADM_WEEKLY_B";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }

                strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'ETS_TRE_BASE'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                if (iCount >= 1)
                {

                    strSql = " DROP TABLE ETS_TRE_BASE";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }
                strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'ETS_TRE_BASE2'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                if (iCount >= 1)
                {

                    strSql = " DROP TABLE ETS_TRE_BASE2";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }
                strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'ETS_TRE_BASE3'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                if (iCount >= 1)
                {

                    strSql = " DROP TABLE ETS_TRE_BASE3";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }
                strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'ETS_TRE_X_SELL_PTNL'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                if (iCount >= 1)
                {

                    strSql = " DROP TABLE ETS_TRE_X_SELL_PTNL";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }
                strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'TRE_OPPORTUNITY'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                if (iCount >= 1)
                {

                    strSql = " DROP TABLE TRE_OPPORTUNITY";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }




                strSql = " SELECT count(1) FROM ALL_TABLES where table_name = 'ETS_ADM_WEEKLY_A' ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iETS_ADM_WEEKLYExisits = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }

                //strSql = " SELECT count(1) FROM ALL_TABLES where table_name = 'TRE_RANDOM' ";
                //if (Common.iDBType == (int)Enums.DBType.Oracle)
                //{
                //    iTRE_Random = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                //}
                strSql = " SELECT count(1) FROM ALL_TABLES where table_name = 'ETS_TRE_X_SELL_PNTL' ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iETS_TRE_X_SELL_PNTLExisits = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }

                strSql = " SELECT count(1) FROM ALL_TABLES where table_name = 'ETS_TRE_BASE' ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iETS_TRE_BASE_Exisits = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }

                strSql = " SELECT count(1) FROM ALL_TABLES where table_name = 'ETS_TRE_BASE2' ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iETS_TRE_BASE2_Exisits = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }

                strSql = " SELECT count(1) FROM ALL_TABLES where table_name = 'ETS_TRE_BASE3' ";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iETS_TRE_BASE3_Exisits = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }


              

                   DataTable dtDel= new DataTable();
                   strSql = "SELECT COLNAME FROM TRE_MAPPING Where ProjectId=" + ProjectId;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    dtDel = (((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql));
                    //foreach (DataRow dtrow in dtDel.Rows)
                    //{
                    //    if (dtrow[0].ToString().StartsWith("B_"))
                    //    {
                    //        dtrow[0] = dtrow[0].ToString().Remove(0, 2);
                    //        dtrow.AcceptChanges();
                    //    }
                    //}

                }

                foreach (DataRow dr in drs)
                {
                    if (dtDel.Rows.Count > 0)
                    {
                        dtDel.AsEnumerable().Where(r => r.Field<string>("COLNAME") == dr[0].ToString()).ToList().ForEach(row => row.Delete());
                        dtDel.AcceptChanges();
                    }
                    if (dr["type"].ToString() != ((int)Enums.ColType.None).ToString())
                    {
                      
                        if (dr["Table"] == "C")
                        {
                            DataTable dt;
                            strSql = "select COMBINE_COLUMNS FROM TRE_CALCULATED_COLUMNS WHERE Project_Id = " + ProjectId + "  AND COLNAME='" + dr[0].ToString() +"'";
                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {
                                dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                                strRandomColumns += dt.Rows[0]["COMBINE_COLUMNS"].ToString() + " " + dr[0].ToString();
                            }

                        }
                        else 
                        {
                            strRandomColumns += dr[0].ToString();
                        }
                        strRandomColumns += ",";
                        strSql = " Select count(1) from TRE_MAPPING where TABLENAME = '" + strTabName + "' and COLNAME = '" + dr[0].ToString() + "' and PROJECTID="+ProjectId;
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
                            strSql = "INSERT INTO TRE_MAPPING (COLNAME, ISREQUIRED,TYPE,COLDATATYPE,TABLENAME,PROJECTID) VALUES" + "('" + dr[0].ToString() + "',";
                            if (dr["Required"] != DBNull.Value && (bool)dr["Required"])
                                strSql += "1";
                            else
                                strSql += "0";

                            strSql += "," + dr["type"].ToString() + ",'" + dr["dataType"].ToString() + "','" + strTabName + "',"+ProjectId +")";

                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                                ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                            else
                                ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                        }
                        else
                        {
                            strSql = "update TRE_MAPPING Set ISREQUIRED= ";
                            if (dr["Required"] != DBNull.Value && (bool)dr["Required"])
                                strSql += "1";
                            else
                                strSql += "0";

                            strSql += ",TYPE=" + dr["type"].ToString() + ",COLDATATYPE='" + dr["dataType"].ToString() + "' where TABLENAME = '" + strTabName + "'";
                            strSql += " and COLNAME = '" + dr[0].ToString() + "'";

                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                                ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                            else
                                ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                        }
                        string strDType = "";
                        if (dr["dataType"].ToString() == "System.String")
                            strDType = " varchar(200)";
                        else if (dr["dataType"].ToString() == "System.Decimal")
                            strDType = " Number (18,2)";
                        else if (dr["dataType"].ToString() == "System.DateTime")
                            strDType = " Date ";
                        else
                            strDType = " Number (18,2)";

                        if (dr["type"].ToString() == ((int)Enums.ColType.Input).ToString())
                        {
                            if (dr["dataType"].ToString() != "System.String" && dr["dataType"].ToString() != "System.DateTime")
                            {  
                               
 
                                strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'ETS_ADM_WEEKLY_A' and upper(column_name) = 'A_" + dr[0].ToString().ToUpper() + "'";
                                if (Common.iDBType == (int)Enums.DBType.Oracle)
                                {
                                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                                }
                                if (iCount == 0)
                                {
                                    if (iETS_ADM_WEEKLYExisits == 0)
                                    {
                                        if (strCreateTString1 != "")
                                            strCreateTString1 += ",";

                                        if (strCreateBString1 != "")
                                            strCreateBString1 += ",";


                                        if (strBaseString1 != "")
                                            strBaseString1 += ",";

                                        if (strBase2String1 != "")
                                            strBase2String1 += ",";


                                        if (strBase3String1 != "")
                                            strBase3String1 += ",";

                                        strCreateTString1 += "A_" + dr[0].ToString() + strDType;
                                        strCreateBString1 += "B_" + dr[0].ToString() + strDType;

                                        strBaseString1 += "A_" + dr[0].ToString() + strDType + ",B_" + dr[0].ToString() + strDType + ",X_" + dr[0].ToString() + strDType + ",D_" + dr[0].ToString() + strDType;

                                        strBase2String1 += "A_" + dr[0].ToString() + strDType + ",B_" + dr[0].ToString() + strDType + ",X_" + dr[0].ToString() + strDType + ",D_" + dr[0].ToString() + strDType + ",S_" + dr[0].ToString() + " varchar(200) ";

                                        strBase3String1 += "A_" + dr[0].ToString() + strDType + ",B_" + dr[0].ToString() + strDType + ",X_" + dr[0].ToString() + strDType + ",D_" + dr[0].ToString() + strDType + ",S_" + dr[0].ToString() + " varchar(200) ,P_" + dr[0].ToString() + strDType;

                                    }
                                    else
                                    {


                                        strSql = " Alter Table ETS_ADM_WEEKLY_A add A_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                        strSql = " Alter Table ETS_ADM_WEEKLY_B add B_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                                        //Base
                                        strSql = " Alter Table ETS_TRE_BASE add A_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                        strSql = " Alter Table ETS_TRE_BASE add B_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                        strSql = " Alter Table ETS_TRE_BASE add X_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                        strSql = " Alter Table ETS_TRE_BASE add D_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                        //Base2
                                        strSql = " Alter Table ETS_TRE_BASE2 add A_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                        strSql = " Alter Table ETS_TRE_BASE2 add B_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                        strSql = " Alter Table ETS_TRE_BASE2 add X_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                        strSql = " Alter Table ETS_TRE_BASE2 add D_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                        strSql = " Alter Table ETS_TRE_BASE2 add S_" + dr[0].ToString() + " varchar(200) ";
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                        //Base3
                                        strSql = " Alter Table ETS_TRE_BASE3 add A_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                        strSql = " Alter Table ETS_TRE_BASE3 add B_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                        strSql = " Alter Table ETS_TRE_BASE3 add X_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                        strSql = " Alter Table ETS_TRE_BASE3 add D_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                        strSql = " Alter Table ETS_TRE_BASE3 add S_" + dr[0].ToString() + " varchar(200) ";
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                        strSql = " Alter Table ETS_TRE_BASE3 add P_" + dr[0].ToString() + strDType;
                                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                    }
                                }
                            }
                            strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'ETS_TRE_X_SELL_PNTL' and upper(column_name) = 'X_" + dr[0].ToString().ToUpper() + "'";
                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {
                                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                            }
                            if (iCount == 0)
                            {
                                if (iETS_TRE_X_SELL_PNTLExisits == 0)
                                {
                                    if (strSellPtnlTableString != "")
                                        strSellPtnlTableString += ",";

                                    if (strSellPtnlUserTableString != "")
                                        strSellPtnlUserTableString += ",";

                                    strSellPtnlTableString += "X_" + dr[0].ToString() + strDType;

                                }
                                else
                                {
                                    strSql = " Alter Table ETS_TRE_X_SELL_PNTL add X_" + dr[0].ToString().ToUpper() + strDType;
                                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                }
                            }


                           
                        }
                        else if (dr["type"].ToString() == ((int)Enums.ColType.Segment).ToString())
                        {

                            strSql = " Select count(1) from user_tab_columns where table_name = 'ETS_ADM_WEEKLY_A' and upper(column_name) = '" + dr[0].ToString().ToUpper() + "'";
                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {
                                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                            }
                            if (iCount == 0)
                            {
                                if (iETS_ADM_WEEKLYExisits == 0)
                                {
                                    if (strSegmentString != "")
                                    {
                                        strSegmentString += ",";
                                    }

                                    strSegmentString += dr[0].ToString() + strDType;
                                }
                                else
                                {
                                    strSql = " Alter Table ETS_ADM_WEEKLY_A add " + dr[0].ToString().ToUpper() + strDType;
                                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                    strSql = " Alter Table ETS_TRE_BASE add " + dr[0].ToString().ToUpper() + strDType;
                                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                    strSql = " Alter Table ETS_TRE_BASE2 add " + dr[0].ToString().ToUpper() + strDType;
                                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                    strSql = " Alter Table ETS_TRE_BASE3 add " + dr[0].ToString().ToUpper() + strDType;
                                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                }
                            }
                          

                        }
                        else if (dr["type"].ToString() == ((int)Enums.ColType.Key).ToString())
                        {
                            strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'ETS_ADM_WEEKLY_A' and upper(column_name) = '" + dr[0].ToString().ToUpper() + "'";
                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {
                                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                            }
                            if (iCount == 0)
                            {
                                if (iETS_ADM_WEEKLYExisits == 0)
                                {
                                    if (strKeyString != "")
                                    {
                                        strKeyString += ",";
                                    }

                                    strKeyString += dr[0].ToString() + strDType;
                                }
                                else
                                {
                                    strSql = " Alter Table ETS_ADM_WEEKLY_A add " + dr[0].ToString().ToUpper() + strDType;
                                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                    strSql = " Alter Table ETS_ADM_WEEKLY_B add " + dr[0].ToString().ToUpper() + strDType;
                                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                                }
                            }

                         
                        }

                    }
                    else
                    {
                        strSql = " Delete from TRE_MAPPING where upper(TABLENAME) = '" + strTabName.ToUpper() + "' and upper(COLNAME) = '" + dr[0].ToString().ToUpper() + "'";
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                        if (iETS_ADM_WEEKLYExisits > 0)
                        {
                            strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'ETS_ADM_WEEKLY_A' and upper(column_name) = 'A_" + dr[0].ToString().ToUpper() + "'";
                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {
                                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                            }
                            if (iCount > 0)
                            {
                                strSql = " Alter Table ETS_ADM_WEEKLY_A drop column A_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = " Alter Table ETS_ADM_WEEKLY_B drop column B_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                                strSql = " Alter Table ETS_TRE_BASE drop column A_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = " Alter Table ETS_TRE_BASE drop column B_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = " Alter Table ETS_TRE_BASE drop column D_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = " Alter Table ETS_TRE_BASE drop column X_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);



                                strSql = " Alter Table ETS_TRE_BASE2 drop column A_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = " Alter Table ETS_TRE_BASE2 drop column B_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = " Alter Table ETS_TRE_BASE2 drop column D_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = " Alter Table ETS_TRE_BASE2 drop column X_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = " Alter Table ETS_TRE_BASE2 drop column S_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                                strSql = " Alter Table ETS_TRE_BASE3 drop column A_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = " Alter Table ETS_TRE_BASE3 drop column B_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = " Alter Table ETS_TRE_BASE3 drop column D_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = " Alter Table ETS_TRE_BASE3 drop column X_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                                strSql = " Alter Table ETS_TRE_BASE3 drop column S_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                                strSql = " Alter Table ETS_TRE_BASE3 drop column P_" + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                            }

                            strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'ETS_ADM_WEEKLY_A' and upper(column_name) = '" + dr[0].ToString().ToUpper() + "'";
                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {
                                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                            }
                            if (iCount > 0)
                            {
                                strSql = " Alter Table ETS_ADM_WEEKLY_A drop column " + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                            }

                            strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'ETS_ADM_WEEKLY_B' and upper(column_name) = '" + dr[0].ToString().ToUpper() + "'";
                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {
                                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                            }
                            if (iCount > 0)
                            {
                                strSql = " Alter Table ETS_ADM_WEEKLY_B drop column " + dr[0].ToString().ToUpper();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                            }

                            strSql = " Select count(1) from user_tab_columns where table_name = 'ETS_TRE_BASE' and upper(column_name) = '" + dr[0].ToString().ToUpper() + "'";
                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {
                                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                            }
                            if (iCount > 0)
                            {
                                strSql = " Alter Table ETS_TRE_BASE drop column " + dr[0].ToString();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                            }

                            strSql = " Select count(1) from user_tab_columns where table_name = 'ETS_TRE_BASE2' and upper(column_name) = '" + dr[0].ToString().ToUpper() + "'";
                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {
                                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                            }
                            if (iCount > 0)
                            {
                                strSql = " Alter Table ETS_TRE_BASE2 drop column " + dr[0].ToString();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                            }
                            strSql = " Select count(1) from user_tab_columns where table_name = 'ETS_TRE_BASE3' and upper(column_name) = '" + dr[0].ToString().ToUpper() + "'";
                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {
                                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                            }
                            if (iCount > 0)
                            {
                                strSql = " Alter Table ETS_TRE_BASE3 drop column " + dr[0].ToString();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                            }
                            //strSql = " Select count(1) from user_tab_columns where table_name = 'TRE_RANDOM' and upper(column_name) = '" + dr[0].ToString().ToUpper() + "'";
                            //if (Common.iDBType == (int)Enums.DBType.Oracle)
                            //{
                            //    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                            //}
                            //if (iCount > 0)
                            //{
                            //    strSql = " Alter Table TRE_RANDOM drop column " + dr[0].ToString();
                            //       ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                            //}


                        }

                        if (iETS_TRE_X_SELL_PNTLExisits > 0)
                        {
                            strSql = " Select count(1) from user_tab_columns where table_name = 'ETS_TRE_X_SELL_PNTL' and upper(column_name) = 'X_" + dr[0].ToString().ToUpper() + "'";
                            if (Common.iDBType == (int)Enums.DBType.Oracle)
                            {
                                iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                            }
                            if (iCount > 0)
                            {
                                strSql = " Alter Table ETS_TRE_X_SELL_PNTL drop column X_" + dr[0].ToString();
                                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                            }
                        }
                       

                    }
                }


                foreach (DataRow dr in dtDel.Rows)
                {

                    
                    strSql = " Delete from TRE_MAPPING where upper(TABLENAME) = '" + strTabName.ToUpper() + "' and upper(COLNAME) = '" + dr[0].ToString().ToUpper() + "'";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                    strSql = " Select count(1) from user_tab_columns where table_name = 'ETS_ADM_WEEKLY_A' and upper(column_name) = '" + dr[0].ToString().ToUpper() + "'";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                    }
                    if (iCount > 0)
                    {

                        strSql = " Alter Table ETS_ADM_WEEKLY_A drop column " + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                         }

                    strSql = " Select count(1) from user_tab_columns where table_name = 'ETS_TRE_BASE' and upper(column_name) = '" + dr[0].ToString().ToUpper() + "'";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                    }

                    if (iCount > 0)
                    {

                        strSql = " Alter Table ETS_TRE_BASE drop column " + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                    }
                    strSql = " Select count(1) from user_tab_columns where table_name = 'ETS_TRE_BASE2' and upper(column_name) = '" + dr[0].ToString().ToUpper() + "'";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                    }
                    if (iCount > 0)
                    {

                        strSql = " Alter Table ETS_TRE_BASE2 drop column " + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                         }

                    strSql = " Select count(1) from user_tab_columns where table_name = 'ETS_TRE_BASE3' and upper(column_name) = '" + dr[0].ToString().ToUpper() + "'";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                    }
                        
                    if (iCount > 0)
                    {

                        strSql = " Alter Table ETS_TRE_BASE3 drop column " + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                         }


                    strSql = " Select count(1) from user_tab_columns where table_name = 'ETS_ADM_WEEKLY_A' and upper(column_name) = 'A_" + dr[0].ToString().ToUpper() + "'";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                    }
                    if (iCount > 0)
                    {

                        strSql = " Alter Table ETS_ADM_WEEKLY_A drop column A_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_ADM_WEEKLY_B drop column B_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);


                        strSql = " Alter Table ETS_TRE_BASE drop column A_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                        strSql = " Alter Table ETS_TRE_BASE drop column B_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_TRE_BASE drop column D_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_TRE_BASE drop column X_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);



                        strSql = " Alter Table ETS_TRE_BASE2 drop column A_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                        strSql = " Alter Table ETS_TRE_BASE2 drop column B_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_TRE_BASE2 drop column D_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_TRE_BASE2 drop column X_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_TRE_BASE2 drop column S_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);



                        strSql = " Alter Table ETS_TRE_BASE3 drop column A_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                        strSql = " Alter Table ETS_TRE_BASE3 drop column B_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_TRE_BASE3 drop column D_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_TRE_BASE3 drop column X_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_TRE_BASE3 drop column S_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_TRE_BASE3 drop column P_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                    }

                    strSql = " Select count(1) from user_tab_columns where table_name = 'ETS_TRE_BASE2' and upper(column_name) = 'X_" + dr[0].ToString().ToUpper() + "'";
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                    }
                    if (iCount > 0)
                    {
                        //strSql = " Alter Table ETS_TRE_BASE2 drop column A_" + dr[0].ToString().ToUpper();
                        //((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                        //strSql = " Alter Table ETS_TRE_BASE2 drop column B_" + dr[0].ToString().ToUpper();
                        //((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_TRE_BASE2 drop column D_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_TRE_BASE2 drop column X_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = " Alter Table ETS_TRE_BASE2 drop column S_" + dr[0].ToString().ToUpper();
                        ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

                    }

                }
                                
                strRandomColumns = strRandomColumns.Substring(0, strRandomColumns.Length - 1);
                strSql = " Select count(1) from user_tab_columns where upper(table_name) = 'TRE_RANDOM'";
              //  strSql = " select Count(1) from   TRE_RANDOM";

                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                }
                if (iCount >= 1)
                {

                    strSql = " DROP TABLE TRE_RANDOM";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }

                strSql = "Select count(1) from user_tab_columns where table_name = 'ETS_ADM_WEEKLY_A'";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                } 
                
                strSql = " Select count(1) from ETS_ADM_WEEKLY_A";
                if (iCount > 0)
                {
                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        iCount = int.Parse(((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql));
                    }
                }
                if (iCount != 0)
                {

                    //strSql = "CREATE TABLE TRE_RANDOM AS SELECT " + strRandomColumns + "   from " + strTabName + " WHERE ";
                    //if (strFilerCondition != "")
                    //    strSql += strFilerCondition + " and  ";
                    //strSql += " Customer in( SELECT  Distinct CUSTOMER FROM  ETS_ADM_WEEKLY_A )";
                    //((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);



//                    Select C.CUSTOMER    from TRE_DETAILS_NEW_C  C,
//( SELECT CUSTOMER,week FROM   ( SELECT Distinct CUSTOMER,week FROM TRE_DETAILS_NEW_C Where HS_FLAG='0' 
//ORDER BY DBMS_RANDOM.RANDOM) WHERE  rownum < 5000) K WHERE C.CUSTOMER=K.cUSTOMER AND C.WEEK=K.WEEK

                    strSql = "CREATE TABLE TRE_RANDOM AS SELECT " + strRandomColumns + "   from " + strTabName + " C , ";

                    strSql += " ( SELECT RNDMCUSTOMER FROM   ( SELECT Distinct CUSTOMER as RNDMCUSTOMER FROM " + strTabName;

                    if (strFilerCondition != "")
                        strSql += " Where " + strFilerCondition + "";
                    strSql += " ORDER BY DBMS_RANDOM.RANDOM) WHERE  rownum < 5000)K WHERE C.CUSTOMER=K.RNDMCUSTOMER ";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                    
                }
                else
                {
                    strSql = "CREATE TABLE TRE_RANDOM AS SELECT " + strRandomColumns + "   from " + strTabName + " C , ";

                    strSql += " ( SELECT RNDMCUSTOMER FROM   ( SELECT Distinct CUSTOMER as RNDMCUSTOMER FROM " + strTabName;

                    if (strFilerCondition != "")
                        strSql += " Where " + strFilerCondition + "";
                    strSql += " ORDER BY DBMS_RANDOM.RANDOM) WHERE  rownum < 5000)K WHERE C.CUSTOMER=K.RNDMCUSTOMER ";
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                
                }


                
                if (iETS_ADM_WEEKLYExisits == 0 && strCreateTString1 != "")
                {
                    strCreateTString += "TIMEPERIOD_ID number";

                    if (strKeyString != "")
                        strCreateTString += "," + strKeyString;
                    if (strSegmentString != "")
                        strCreateTString += "," + strSegmentString;

                    strCreateTString += "," + strCreateTString1 + ") NOLOGGING";


                    strCreateBString += "TIMEPERIOD_ID number";
                    if (strKeyString != "")
                        strCreateBString += "," + strKeyString;

                    strCreateBString += "," + strCreateBString1 + ") NOLOGGING";


                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strCreateTString);
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strCreateBString);
                    }
                    else
                    {
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strCreateTString);
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strCreateBString);
                    }
                    //indexes created by Sravanthi

                    strCreateTString = "CREATE INDEX ETS_ADM_WEEKLY_A_IX ON ETS_ADM_WEEKLY_A(CUSTOMER)";
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strCreateTString);

                    strCreateBString = "CREATE INDEX ETS_ADM_WEEKLY_B_IX ON ETS_ADM_WEEKLY_B(CUSTOMER)";
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strCreateBString);
                }
                string strKeySegmentString = "";
                if (iETS_TRE_BASE_Exisits == 0 && strBaseString1 != "")
                {


                    if (strKeyString != "")
                        strKeySegmentString += strKeyString;
                    if (strSegmentString != "")
                        strKeySegmentString += "," + strSegmentString;

                    strBaseString += strKeySegmentString + "," + strBaseString1 + ") NOLOGGING";

                    strBase2String += strKeySegmentString + "," + strBase2String1 + ") NOLOGGING";

                    strBase3String += strKeySegmentString + "," + strBase3String1 + ") NOLOGGING";
                    strSql = "CREATE TABLE TRE_OPPORTUNITY ( CUSTOMER varchar2(50)  NULL,";
                    strSql += "WEEK number(2)  NULL )  NOLOGGING";

                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strBaseString);
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strBase2String);
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strBase3String);
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSql);
                    }
                    else
                    {
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strBaseString);
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strBase2String);
                        ((DBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strBase3String);
                    }

                    //Sravanthi
                    strBaseString = "CREATE INDEX ETS_TRE_BASE_IX ON ETS_TRE_BASE(CUSTOMER)";
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strBaseString);

                    strBase2String = "CREATE INDEX ETS_TRE_BASE2_IX ON ETS_TRE_BASE2(CUSTOMER)";
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strBase2String);

                    strBase3String = "CREATE INDEX ETS_TRE_BAS3E_IX ON ETS_TRE_BASE3(CUSTOMER)";
                    ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strBase3String);

                }

                if (iETS_TRE_X_SELL_PNTLExisits == 0 && strSellPtnlTableString != "")
                {
                    if (strSellPtnlTableString != "")
                        strSellPtnlTableString = "CREATE TABLE ETS_TRE_X_SELL_PNTL(TIMEPERIOD varchar2(50),SEGMENTCOLNAME varchar(50),CURRENTSEGMENT VARCHAR(50), " + strSellPtnlTableString + ") NOLOGGING";

                    if (Common.iDBType == (int)Enums.DBType.Oracle)
                    {
                        ((OraDBManager)Common.dbMgr).ExecuteScalar(CommandType.Text, strSellPtnlTableString);
                    }

                  }
                //dELETING UNSAVED OPPORTUNITITES
                strSql = "select OPPORTUNITY_ID from Opportunity where IsOnMain = 0 and Project_Id=" + ProjectId;
                DataTable dtOpp = new DataTable();
                dtOpp = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                foreach (DataRow dr in dtOpp.Rows)
                {
                    strSql = " Delete from STATUS_BREAKDOWN Where OPPORTUNITY_ID= " + Convert.ToInt32(dr[0]);
                    ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);
                }
                strSql = " Delete from Opportunity where IsOnMain = 0 and Project_Id="+ProjectId;
                ((OraDBManager)Common.dbMgr).ExecuteNonQuery(CommandType.Text, strSql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnCreateTreMApping(DataRow[] drs, string strTabName, string strWherecndtn)
        {
            try
            {
                string strColumnnames = "";
                DataTable dt;
                string strSql = "";
                foreach (DataRow dr in drs)
                {

                    if (dr["type"].ToString() != ((int)Enums.ColType.None).ToString())
                    {

                        strColumnnames += dr[0];
                        strColumnnames += ",";
                    }
                }
                //   SELECT count(*) into nCount FROM dba_tables where table_name = 'EMPLOYEE';
                strColumnnames = strColumnnames.Substring(0, strColumnnames.Length - 1);
                if (strWherecndtn == "")
                    strSql = "create table OPPORTUNITY_RANKING11 as select " + strColumnnames + " from " + strTabName;
                else
                    strSql = "create table OPPORTUNITY_RANKING11 as select " + strColumnnames + " from " + strTabName + " Where " + strWherecndtn;
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
                else
                {
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public DataTable fnGetTableNames()
        {
            try
            {
                DataTable dt;
                string strSql = "";



                if (Common.iDBType == (int)Enums.DBType.Oracle)
                {
                    strSql = "select TName from tab WHERE TABTYPE = 'TABLE' AND  TName not like 'APEX%' AND TName not like 'BIN%' AND TName not like 'DEMO%'";
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
            //
        }
        public DataTable fnCreateColTypes()
        {
            try
            {
                DataTable dtTypes = new DataTable();
                dtTypes.Columns.Add(new DataColumn("TypeId", typeof(int)));
                dtTypes.Columns.Add(new DataColumn("Type", typeof(string)));
                dtTypes.Rows.Add(((int)Enums.ColType.Key).ToString(), "Key");
                dtTypes.Rows.Add(((int)Enums.ColType.Input).ToString(), "Input");
                dtTypes.Rows.Add(((int)Enums.ColType.Time).ToString(), "Time");
                dtTypes.Rows.Add(((int)Enums.ColType.Segment).ToString(), "Segment");
                dtTypes.Rows.Add(((int)Enums.ColType.None).ToString(), "None");
                return dtTypes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool fnIsTREDetailsMapped()
        {
            try
            {
                DataTable dt;
                string strSQl = "Select * from TRE_MAPPING";
                if (Common.iDBType == (int)Enums.DBType.Oracle)
                    dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSQl);
                else
                {
                    dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSQl);
                }
                if (dt.Rows.Count > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
    }
}
