using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RecoEngine_DataLayer;

namespace RecoEngine_BI
{
   public class clsReports
    {

       public DataSet fnGetReports(int iProjectId)
       {
           try
           {
               DataSet ds= new DataSet();
               string strSql = "";
               DataTable dt;

               strSql = "Select OPP_NAME from Opportunity where ISACTIVE=1 And Isonmain=1 And PROJECT_ID="+iProjectId;
                   if (Common.iDBType == (int)Enums.DBType.Oracle)
                       dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                   else
                       dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);

                   foreach (DataRow dr in dt.Rows)
                   {
                       strSql = " Select " +  "'" + dr["OPP_NAME"].ToString()+"' as OppName , "  + dr["OPP_NAME"].ToString() + "_Status as OppStatus, Round(Ratio_To_Report(Count(*)) Over() *100,2)";
                       strSql += " As AccountsPercentage,Round((Round(Sum(" + dr["OPP_NAME"].ToString() + "_Delta),2)/Count(Customer))*100,2)";
                       strSql += " As AverageDelta,Count(Customer) as Accounts,Round(Avg("+dr["OPP_NAME"].ToString()+"_PNTL),2)  as AvgAccountPotential From Tre_OpportunityExport";
                       strSql += " Where " + dr["OPP_NAME"].ToString() +"_Status!='NA' Group By "+ dr["OPP_NAME"].ToString() +"_Status ";
                       if (Common.iDBType == (int)Enums.DBType.Oracle)
                           dt = ((OraDBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                       else
                           dt = ((DBManager)Common.dbMgr).ExecuteDataTable(CommandType.Text, strSql);
                       ds.Tables.Add(dt);
                   }

               return ds;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataTable fnGetRankingReports(int iProjectId,int iRank)
       {
           try
           {
              
               string strSql = "";
               DataTable dt;             
              strSql = " Select A.Rank"+iRank+"_Action,(A.totalpotential) as Rank"+iRank+"Potential,A.Customercount as Accounts,";
              strSql += " Round((A.Customercount)/(T.TotalCustomer),4)*100 as AccountsPercent,Round((A.totalpotential)/R.GrandPotential,6)*100  as Potentialpercent";
              strSql += " from";
              strSql += " (Select Sum(Rank"+iRank+") as totalpotential,Rank"+iRank+"_Action,Count(Customer) as Customercount  from tre_ranking where Rank"+iRank+"=0 Group by Rank"+iRank+"_Action";
              strSql += " Union";
              strSql += " Select Sum(Rank"+iRank+") as totalpotential,Rank"+iRank+"_Action,Count(Customer) as Customercount  from tre_ranking where Rank"+iRank+"!=0 Group by Rank"+iRank+"_Action)A,";
              strSql += " (select Count(Customer) as TotalCustomer From Tre_Ranking) T,";
              strSql += " (select Sum(Rank" + iRank + ") as GrandPotential  From Tre_Ranking where Rank" + iRank + "!=0) R Order by  Rank" + iRank + "Potential desc";
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
    }
}
