using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecoEngine_DataLayer;
using RecoEngine_BI;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using NhibernateclassGeneration;
using NhibernateclassGeneration.Domain;
using NhibernateclassGeneration.Reader;
using System.Configuration;
using System.IO;

namespace RecoEngine
{
    public partial class CntrlDataFields : UserControl
    {
        private IMetadataReader metadataReader;
        clsDataSource clsDSOBJ = new clsDataSource();
        public event EventHandler DataFieldsNxtBtnClick;
        bool bIsLoaded = false;
        DataTable dtmain = new DataTable();
        public CntrlDataFields()
        {
            InitializeComponent();

        }
        private void CntrlDataFields_Load(object sender, EventArgs e)
        {

            try
            {
                gbMain.Padding = new Padding(0, 0, 0, 0);
                dataschemaGrid.DoubleClick += new EventHandler(dataschemaGrid_DoubleClick);
                fnLoadTableName();
                dataschemaGridbinding();
                bIsLoaded = true;
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                Common.LogMessage(ex);
            }
        }

        void dataschemaGrid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dataschemaGrid.CurrentRow != null)
                {
                    GridViewRowInfo drRow = dataschemaGrid.CurrentRow;
                    if (drRow.Cells["Table"].Value.ToString() == "C")
                    {

                        DataTable dtcalaculated = clsDSOBJ.fnGetCalaculatedColMappingData(Common.iProjectID,Common.strTableName);
                        string str=drRow.Cells["ColumnName"].Value.ToString() ;
                        DataRow[] dtrows = dtcalaculated.Select("COLNAME = '" + str +"'");
                        bindingExpressionEditor((int)Enums.ExpressionType.CalaculatedColumn, dtrows[0]["COMBINE_COLUMNS"].ToString(), dtrows[0]["COLNAME"].ToString());
            
        
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }
        void fnLoadTableName()
        {
            try
            {
                DataTable dtTbl = new DataTable();

                dtTbl = clsDSOBJ.fnGetTableNames();

                ddlTableName.DataSource = dtTbl;
                ddlTableName.ValueMember = "TNAME";
                ddlTableName.DisplayMember = "TNAME";

                if (Common.strTableName != "")
                    ddlTableName.SelectedValue = Common.strTableName;
                else
                    ddlTableName.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void dataschemaGridbinding()
        {
            try
            {
                DataTable dt = clsDSOBJ.fnGetTreDetailsSchema(ddlTableName.SelectedValue.ToString());
                DataTable dtcalaculated = clsDSOBJ.fnGetCalaculatedColMappingData(Common.iProjectID,Common.strTableName);
                DataTable dt1 = clsDSOBJ.fnGetColMappingData(Common.iProjectID);
                DataRow row;
             
                dt.Columns.Add(new DataColumn("Table", typeof(string)));
                dt.Columns.Add(new DataColumn("Type", typeof(int)));
                dt.Columns.Add(new DataColumn("Required", typeof(bool)));
                foreach (DataRow drcal in dtcalaculated.Rows)
                { 
                    row = dt.NewRow();
                    row["ColumnName"] = drcal["COLNAME"].ToString();
                    row["Table"] = "C";
                    //if (drcal["COLDATATYPE"].ToString() == "NUMBER")
                    //{
                    //    row["DataType"] = typeof(decimal);
                    //}
                    row["DataType"] = drcal["COLDATATYPE"].ToString() == "NUMBER" ? typeof(Decimal) : drcal["COLDATATYPE"].ToString() == "DATE" ? typeof(DateTime) : typeof(String);
                    dt.Rows.Add(row);
                }
               
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt1.Rows.Count > 0)
                    {
                        DataRow[] dr = dt1.Select("COLNAME='" + dt.Rows[i][0].ToString() + "'");
                        if (dr.Length > 0 && dr[0]["TABLENAME"].ToString().ToLower() == ddlTableName.SelectedValue.ToString().ToLower())
                        {
                            dt.Rows[i]["Type"] = dr[0]["Type"];
                            dt.Rows[i]["Required"] = dr[0]["ISREQUIRED"];
                            if (dt.Rows[i]["Table"].ToString() != "C")
                            dt.Rows[i]["Table"] = "M";
                        }
                        else
                        {
                            dt.Rows[i]["Type"] = (int)Enums.ColType.None;
                            if (dt.Rows[i]["Table"] != "C")
                               dt.Rows[i]["Table"] = "M";
                            dt.Rows[i]["Required"] = false;
                        }
                    }
                    else
                    {
                        dt.Rows[i]["Type"] = (int)Enums.ColType.None;
                        if (dt.Rows[i]["Table"] != "C")
                            dt.Rows[i]["Table"] = "M";
                        dt.Rows[i]["Required"] = false;
                    }
                }

                dataschemaGrid.DataSource = dt;
             ////   DataTable dtmain = (DataTable)dataschemaGrid.DataSource;
             //   if (dtmain.Rows.Count > 0)
             //   {
             //       //dtmain.Merge(dt, true);
             //       if (dtmain.Rows.Count != dt.Rows.Count)
             //       {
             //           DataRow dr = dtmain.NewRow();
             //         dr = dt.Rows[dt.Rows.Count - 1] as DataRow;
             //           dt.Rows[dt.Rows.Count - 1].Delete();
             //           dtmain.Rows.Add(dr.ItemArray);
             //           dataschemaGrid.DataSource = dtmain;
             //           dtmain = null;
             //       }
             //   }
             //   else
             //   {
             //       dataschemaGrid.DataSource = dt;
             //   }
                GridViewComboBoxColumn categoryColumn = new GridViewComboBoxColumn();
                categoryColumn.HeaderText = "Type";
                categoryColumn.ValueMember = "TypeId";
                categoryColumn.DisplayMember = "Type";
                categoryColumn.FieldName = "Type";
                categoryColumn.DataSource = clsDSOBJ.fnCreateColTypes();
                categoryColumn.Width = 200;
                if (dataschemaGrid.MasterTemplate.Columns.Contains("Type1"))
                 dataschemaGrid.MasterTemplate.Columns.Remove("Type1");
                dataschemaGrid.MasterTemplate.Columns.Add(categoryColumn);
                dataschemaGrid.Columns["ColumnName"].ReadOnly = true;
                dataschemaGrid.Columns["DataType"].ReadOnly = true;
                dataschemaGrid.Columns["Type"].ReadOnly = true;
                dataschemaGrid.Columns["Type"].IsVisible = false;
                dataschemaGrid.Columns["Table"].IsVisible = true;
                dataschemaGrid.Columns[0].Width = 200;
               // dataschemaGrid.Columns[1].AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                dataschemaGrid.Columns["DataType"].Width = 300;
                dataschemaGrid.Columns["Required"].Width = 50;
                dataschemaGrid.Columns["Table"].Width = 30;
                dataschemaGrid.Columns.Remove("NumericPrecision");
                dataschemaGrid.Columns.Remove("NumericScale");
                dataschemaGrid.Columns.Remove("ProviderType");
                dataschemaGrid.Columns.Remove("IsLong");
                dataschemaGrid.Columns.Remove("AllowDBNull");
                dataschemaGrid.Columns.Remove("IsReadOnly");
                dataschemaGrid.Columns.Remove("IsUnique");
                dataschemaGrid.Columns.Remove("IsKey");
                dataschemaGrid.Columns.Remove("IsAutoIncrement");
                dataschemaGrid.Columns.Remove("IsRowVersion");
                dataschemaGrid.Columns.Remove("ColumnMapping");
                dataschemaGrid.Columns.Remove("AutoIncrementSeed");
                dataschemaGrid.Columns.Remove("AutoIncrementStep");
                dataschemaGrid.Columns.Remove("BaseCatalogName");
                dataschemaGrid.Columns.Remove("BaseSchemaName");
                dataschemaGrid.Columns.Remove("BaseTableName");
                dataschemaGrid.Columns.Remove("BaseTableNameSpace");
                dataschemaGrid.Columns.Remove("BaseColumnName");
                dataschemaGrid.Columns.Remove("BaseColumnNameSpace");
                dataschemaGrid.Columns.Remove("Expression");
                dataschemaGrid.Columns.Remove("DefaultValue");
                dataschemaGrid.Columns.Remove("ColumnOrdinal");
                dataschemaGrid.Columns.Remove("ColumnSize");
                dataschemaGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                //dataschemaGrid.MasterTemplate.BestFitColumns(); 
                dataschemaGrid.CellValueChanged += new GridViewCellEventHandler(dataschemaGrid_CellValueChanged);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void dataschemaGrid_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (dataschemaGrid.CurrentRow != null)
                {
                    GridViewRowInfo drRow = dataschemaGrid.CurrentRow;
                   // drRow.Cells["Flag"].Value = "Y";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
       
            //if ((e.Column.Name.ToLower() == "type1"))
            //{
            //    if (Convert.ToInt16(e.Value) == (int)Enums.ColType.None)
            //    {
            //        drRow.Cells["Flag"].Value = "N";
            //    }
            //    else
            //    {
            //        drRow.Cells["Flag"].Value = "Y";
            //    }

            //}
            //else if (e.Column.Name.ToLower() == "required")
            //{
            //    if (Convert.ToInt16(drRow.Cells["type1"].Value) == (int)Enums.ColType.None)
            //    {
            //        drRow.Cells["Flag"].Value = "N";
            //    }
            //    else
            //        drRow.Cells["Flag"].Value = "Y";
            //}

        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                generateMapclass();
                string strTabName = ddlTableName.SelectedValue.ToString().ToUpper();
                //strTabName = "tre_random".ToUpper();;
                dataschemaGrid.Refresh();
                DataTable dt = (DataTable)dataschemaGrid.DataSource;
                DataRow[] drs = dt.Select("TYPE=" + (int)Enums.ColType.Key);
                if (drs.Length == 0)
                {
                    Telerik.WinControls.RadMessageBox.Show(this, "Should map 'Key' to at least one column.", "Information", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                    return;
                }
                
                drs = dt.Select("TYPE=" + (int)Enums.ColType.Time);

                if (drs.Length > 1)
                {
                   // Telerik.WinControls.RadMessageBox.Show(this, "Should not map 'Time' to more than one column.", "Information", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                }
                else if (drs.Length == 0)
                {
                    Telerik.WinControls.RadMessageBox.Show(this, "Should map 'Time' to at least one column.", "Information", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                    return;
                }

             
                drs = dt.Select("Required='True'");
                if (drs.Length > 0)
                {
                    Common.strfiltertxt = clsDSOBJ.fnselectFilterCondition(Common.iProjectID);
                    clsDSOBJ.fnInsertTreMApping(drs, strTabName, Common.strfiltertxt,Common.iProjectID);
                   // clsDSOBJ.fnCreateTreMApping(drs, strTabName,Common.strfiltertxt);
                    Common.strTableName = ddlTableName.SelectedValue.ToString();
                  //  Common.strTableName = "tre_random";
                    Common.bIsTableMapped = true;
                    Common.SetLoginDetailsToRegistry();
                    dataschemaGridbinding();
                }

                
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void ddlTableName_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {

                if (bIsLoaded)
                {
                    dataschemaGridbinding();
                    Common.strTableName = ddlTableName.SelectedValue.ToString();
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void generateMapclass()
        {
           
            var Mapclasssettings = new Connection();
           string mapTempPath = Path.GetTempPath();
           if (mapTempPath.EndsWith(@"\") == false)
           { mapTempPath += @"\"; }
           mapTempPath += @"Mapping\";
           if (Directory.Exists(mapTempPath) == false)
           {
               Directory.CreateDirectory(mapTempPath);
           }
            Mapclasssettings.Domainfolderpath = mapTempPath;
            Mapclasssettings.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"]; 
            Mapclasssettings.EntityName = "Entity";
            if ((RecoEngine_BI.Common.iDBType) == (int)Enums.DBType.Oracle)
            {
                Mapclasssettings.Type = ServerType.Oracle;
            }
            metadataReader = MetadataFactory.GetReader(Mapclasssettings.Type, Mapclasssettings.ConnectionString);
            var owners = metadataReader.GetOwners();
            IList<Table> tblList = metadataReader.GetTables("RECOUSR");
            int tableindex = 0;
            for (int i = 0; i < tblList.Count; i++)
            {
                if (tblList[i].Name == "OPPORTUNITY")
                {
                    tableindex = i;
                    break;
                }
            }
            Table table = tblList[tableindex];
            table.PrimaryKey = metadataReader.DeterminePrimaryKeys(table);
            metadataReader.GetTableDetails(table, "RECOUSR");
            var applicationPreferences = GetApplicationPreferences(table, false, Mapclasssettings);
            new ApplicationController(applicationPreferences, table).Generate();
        }

        private void Generate(Table table, bool generateAll, Connection appSettings)
        {
            try
            {
                var applicationPreferences = GetApplicationPreferences(table, generateAll, appSettings);
                new ApplicationController(applicationPreferences, table).Generate();
            }
            catch (Exception ex)
            {
                
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
         
            }
            
        }
        private ApplicationPreferences GetApplicationPreferences(Table tableName, bool all, Connection appSettings)
        {
            
                var applicationPreferences = new ApplicationPreferences
                {
                    ServerType = appSettings.Type,
                    DomainFolderPath = appSettings.Domainfolderpath,
                    TableName = tableName.Name,
                    NameSpace = appSettings.EntityName,
                    FieldNamingConvention = FieldNamingConvention.SameAsDatabase,
                    FieldGenerationConvention = FieldGenerationConvention.AutoProperty,
                    ConnectionString = appSettings.ConnectionString,
                };

                return applicationPreferences;

            
        }

        private void btnAddColumn_Click(object sender, EventArgs e)
        {
            bindingExpressionEditor((int)Enums.ExpressionType.AddColumn);
            
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {

            bindingExpressionEditor((int)Enums.ExpressionType.Filter);

       }
        void bindingExpressionEditor(int iExpressionType, string CalaculatedColumnValue="", string strColname="")
        {
            try
            {
                clsDataSource clsObj = new clsDataSource();
                DataTable dtSource = clsObj.fnGetColMappingData(Common.iProjectID);
                DataTable dt = clsDSOBJ.fnGetTreDetails(Common.strTableName);
                DataTableReader dr = new DataTableReader(dt);
                DataTable dtSchema = dr.GetSchemaTable();

                string strExpression ="";
                if (iExpressionType == (int)Enums.ExpressionType.Filter)
                {
                    strExpression = clsObj.fnselectFilterCondition(Common.iProjectID);
                }
                else
                {
                    strExpression = "";
                }
                if (iExpressionType == (int)Enums.ExpressionType.CalaculatedColumn)
                {
                    strExpression = CalaculatedColumnValue;

                }
                using (var frm = new frmExpressEditor(iExpressionType, Common.strTableName, strExpression))
                {
                    frm._fieldDict = Common.GetDict(dt);
                    frm.AvailableFields = frm._fieldDict.ToList<KeyValuePair<string, Type>>();
                  frm.dtSource = dtSchema;
                    var res = frm.ShowDialog();
               if (res == System.Windows.Forms.DialogResult.OK)
                {
                    if (iExpressionType == (int)Enums.ExpressionType.Filter)
                        clsObj.fnInserFilter(frm.strExpression, Common.iProjectID);
                    else if (iExpressionType == (int)Enums.ExpressionType.AddColumn)
                    {
                        //  Common.strfiltertxt = "";
                        string strColName = frm.strColName;

                        string strMsg = "";
                        strExpression = frm.strExpression;
                        if (!clsObj.fnAddCalaculatedColumn(Common.strTableName, strColName, strExpression, ref  strMsg, Common.iProjectID))
                        {
                            if (strMsg != "")
                                Telerik.WinControls.RadMessageBox.Show(this, strMsg, "Information", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                            return;
                        }
                      
                    }
                    else if (iExpressionType == (int)Enums.ExpressionType.CalaculatedColumn)
                    {
                        if (!clsObj.fnUpdateCalaculatedColumn(Common.strTableName, strColname, frm.strExpression, Common.iProjectID))
                        {
                            
                        }

                    }
                    dtmain = (DataTable)dataschemaGrid.DataSource;
                    dataschemaGridbinding();
                }
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(this, ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (DataFieldsNxtBtnClick != null)
                DataFieldsNxtBtnClick(this, e);
        }

    }
}
