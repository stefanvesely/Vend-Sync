using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Vend_Sync
{
    public partial class frmSyncServer : Form
    {
        public DataTable dtLogTable = new DataTable();

        //public string sConstringServer = "Server=192.168.1.250; Database=DataDemo; User ID=sa; Password=sa@01";
        public string sConstringLocal = @"Data Source=DESKTOP-A5GTSSB\SQLEXPRESS01;Initial Catalog=DataDemo;Integrated Security=True";

        public string sConstringServer = @"Data Source=DESKTOP-A5GTSSB\SQLEXPRESS01;Initial Catalog=DataDemo;Integrated Security=True";
        public string sGetTables = @"SELECT t.name as TableName, SCHEMA_NAME(t.schema_id) As SchemaName,I.rows as RecordCount from sysindexes i INNER JOIN sys.tables t on i.id=t.object_id";
        public string sqlTableCreate = @"CREATE TABLE [dbo].[VLogTable]([ID] [nvarchar](50) NULL,[TableName] [nvarchar](50) NULL,[ColumnName] [nvarchar](50) NULL,[ColValue] [nvarchar](50) NULL,[ColIDValue] [int] NULL,[Date] [datetime] NULL,[UpdatedParties] [nvarchar](max) NULL";
        public string sPCName = "";
        private string sSaveFile = Application.StartupPath.ToString() + @"\PCName.txt";

        public frmSyncServer()
        {
            InitializeComponent();
            GetPCName();
        }

        private void GetPCName()
        {
            try
            {
                ReadPCName();
            }
            catch
            {
                string[] saWords = { "" };
                System.IO.File.WriteAllLines(sSaveFile, saWords);
            }
            if (sPCName.Length == 0)
            {
                MessageBox.Show("Before running any of the functions you have to type a pc name into the box.", "Error");
                tbPCName.Enabled = true;
                try
                {
                    dtLogTable = GetSingleTable("VLogTable", sConstringServer);
                }
                catch
                {
                    CreateLogTable();
                }
            }
            else
            {
                tbPCName.Text = sPCName;
            }
        }

        private void ReadPCName()
        {
            string[] saPCName = System.IO.File.ReadAllLines(sSaveFile);
            sPCName = saPCName[0];
        }

        //§
        private void CreateLogTable()
        {
            SqlConnection sqlconTableCreate = new SqlConnection(sConstringServer);
            sqlconTableCreate.Open();
            SqlCommand sqlCMD = new SqlCommand(sqlTableCreate, sqlconTableCreate);
            sqlCMD.ExecuteNonQuery();
            sqlconTableCreate.Close();
        }

        private List<string> GetTableNames(string sGetConnString)
        {
            DataTable dtTableDataTable = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(sGetConnString);
            sqlConnection.Open();
            SqlDataAdapter sqlOilAdapter = new SqlDataAdapter(sGetTables, sqlConnection);
            sqlOilAdapter.Fill(dtTableDataTable);
            sqlConnection.Close();
            List<string> lstTableNames = new List<string>();
            foreach (DataRow drTable in dtTableDataTable.Rows)
            {
                string sTableName = drTable["TableName"].ToString();
                if (!lstTableNames.Contains(sTableName))
                {
                    lstTableNames.Add(sTableName);
                }
            }
            return lstTableNames;
        }

        private DataTable GetSingleTable(string sTableName, string sGetConnString)
        {
            DataTable dtTabletoReturn = new DataTable();
            string sCommand = "SELECT * FROM " + sTableName;
            SqlConnection sqlConnection = new SqlConnection(sGetConnString);
            sqlConnection.Open();
            SqlDataAdapter sqlOilAdapter = new SqlDataAdapter(sCommand, sqlConnection);
            sqlOilAdapter.Fill(dtTabletoReturn);
            sqlConnection.Close();
            return dtTabletoReturn;
        }

        private bool SuperBulkUploadTable(string sDestinationTable, DataTable dtTableToUpload, string sConnString) // Only use when ulpoading a table with the same columns as dest table
        {
            bool isUploaded = true;
            try
            {
                SqlConnection sqlConn = new SqlConnection(sConnString);
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConn);
                sqlBulkCopy.DestinationTableName = sDestinationTable;
                foreach (DataColumn dcUploadColumn in dtTableToUpload.Columns)
                {
                    sqlBulkCopy.ColumnMappings.Add(dcUploadColumn.ColumnName, dcUploadColumn.ColumnName);
                }
                sqlConn.Open();
                sqlBulkCopy.WriteToServer(dtTableToUpload);
                sqlConn.Close();
                isUploaded = true;
            }
            catch
            {
                isUploaded = false;
            }
            return isUploaded;
        }

        private void CleanTable(string sTableName, string sConnString)
        {
            string sQuery = @"TRUNCATE TABLE " + sTableName;
            SqlConnection sqlCon = new SqlConnection(sConnString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(sQuery, sqlCon);
            cmd.ExecuteNonQuery();
            sqlCon.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SavePCName();
            List<string> lstTableNames = GetTableNames(sConstringServer);
            foreach (string sTableName in lstTableNames)
            {
                if (sTableName.Contains("Vending"))
                {
                    DataTable dtTableToUpload = GetSingleTable(sTableName, sConstringServer);
                    CleanTable(sTableName, sConstringLocal);
                    SuperBulkUploadTable(sTableName, dtTableToUpload, sConstringLocal);
                }
            }
        }

        private void SavePCName()
        {
            sPCName = tbPCName.Text.ToString();
            System.IO.File.Delete(sSaveFile);
            string[] saPCName = new string[1];
            saPCName[0] = sPCName;
            System.IO.File.WriteAllLines(sSaveFile, saPCName);
        }

        private void btnFullUpload_Click(object sender, EventArgs e)
        {
            SavePCName();
            foreach (DataRow drRow in dtLogTable.Rows)
            {
                List<string> lstPCNames = new List<string>();
                string sCompoundNames = drRow["UpdatedParties"].ToString();
                if (!sCompoundNames.Contains(sPCName))
                {
                    string sTable = drRow["TableName"].ToString();
                    string sColumn = drRow["ColumnName"].ToString();
                    string sColumnValue = drRow["ColValue"].ToString();
                    int iColIdentityValue = int.Parse(drRow["ColIDValue"].ToString());
                    UpdateSingleValue("Local", sTable, sColumn, sColumnValue, iColIdentityValue);
                    sColumnValue = sCompoundNames + sPCName + "§";
                    UpdateSingleValue("Local", "VLogTable", "UpdatedParties", sColumnValue, int.Parse(drRow["ID"].ToString()));
                }
            }
            List<string> lstTableNames = GetTableNames(sConstringServer);
            foreach (string sTable in lstTableNames)
            {
                if (sTable.Contains("Vend"))
                {
                    if (sTable.Contains("MachineComments"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndUploadBothChanges(dtLocalTable, dtServerTable, sConstringLocal, sConstringServer, sTable);
                    }
                    if (sTable.Contains("MachineCurrentItemSetup"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("MachineFloatTrace"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("MachineFront"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndUploadBothChanges(dtLocalTable, dtServerTable, sConstringLocal, sConstringServer, sTable);
                    }
                    if (sTable.Contains("VendingMachines1"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("VendingMachineSiteChecklist"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("MachineSiteContacts"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("MachineSites"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("MachineStockTrace"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("MachineTasks"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("MachineTempVisits"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("MachineVisitPlan"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("MachineVisitplanChange"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("VisitPlanSlips"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("VendingMachineVisitPlanStock"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                    if (sTable.Contains("MachineVisitRestock"))
                    {
                        DataTable dtServerTable = GetSingleTable(sTable, sConstringServer);
                        DataTable dtLocalTable = GetSingleTable(sTable, sConstringLocal);
                        CompareAndForceUpdate(dtLocalTable, dtServerTable, sConstringServer, sTable);
                    }
                }
            }
        }

        private void CompareAndForceUpdate(DataTable dtMostUpToDate, DataTable dtComparitor, string sContoUpdate, string sWorkingTable)
        {
            DataTable dtLogUploader = dtLogTable.Copy();
            for (int i = dtLogUploader.Rows.Count - 1; i > -1; i--)
            {
                dtLogUploader.Rows.RemoveAt(i);
            }
            int iColCount = dtMostUpToDate.Columns.Count;
            foreach (DataRow drMU in dtMostUpToDate.Rows)
            {
                foreach (DataRow drTU in dtComparitor.Rows)
                {
                    if (drMU["ID"] == drTU["ID"])
                    {
                        if (drMU != drTU)
                        {
                            int iLogID = int.Parse(dtLogTable.Rows[dtLogTable.Rows.Count - 1][0].ToString());
                            for (int i = 1; i < iColCount; i++)
                            {
                                if (drMU[i] != drTU[i])
                                {
                                    iLogID++;
                                    string sColName = dtComparitor.Columns[i].ColumnName;
                                    string sColVal = drMU[i].ToString();
                                    int iColID = int.Parse(drMU[0].ToString());
                                    DateTime dtNow = DateTime.Now;
                                    string sUpdatedParties = sPCName + "§";
                                    dtLogUploader.Rows.Add(iLogID, sWorkingTable, sColName, sColVal, iColID, dtNow, sUpdatedParties);
                                }
                            }
                        }
                    }
                }
            }
            CleanTable(sWorkingTable, sContoUpdate);
            SuperBulkUploadTable(sWorkingTable, dtMostUpToDate, sContoUpdate);
            SuperBulkUploadTable("VLogTable", dtLogUploader, sConstringServer);
        }

        private void CompareAndUploadBothChanges(DataTable dt1, DataTable dt2, string sCon1, string sCon2, string sWorkingTable)
        {
            DataTable dtUpload1 = new DataTable();
            int iStartingID1 = int.Parse(dt1.Rows[dt1.Rows.Count - 1][0].ToString());
            foreach (DataRow drFirstRow in dt1.Rows)
            {
                int iIndex = 0;
                bool isCanRemove = false;
                for (int i = dt2.Rows.Count - 1; i > 0; i--)
                {
                    if (dt2.Rows[i] == drFirstRow)
                    {
                        isCanRemove = true;
                        iIndex = i;
                    }
                    if (isCanRemove)
                    {
                        dt2.Rows.RemoveAt(iIndex);
                    }
                }
            }
            foreach (DataRow dr1Row in dt1.Rows)
            {
                dtUpload1.Rows.Add(dr1Row);
            }
            foreach (DataRow dr2Row in dt2.Rows)
            {
                iStartingID1++;
                dr2Row["ID"] = iStartingID1;
                dtUpload1.Rows.Add(dr2Row);
            }
            CleanTable(sWorkingTable, sCon1);
            CleanTable(sWorkingTable, sCon2);
            SuperBulkUploadTable(sWorkingTable, dtUpload1, sCon1);
            SuperBulkUploadTable(sWorkingTable, dtUpload1, sCon2);
        }

        private void UpdateSingleValue(string sLS, string sTableName, string sColName, string sColValue, int iID)
        {
            string sCurConString = "";
            string sCommandText = "UPDATE " + sTableName + " SET " + sColName + " = '" + sColValue + "' WHERE ID = '" + iID.ToString() + "'";
            if (sLS == "Local")
            {
                sCurConString = sConstringLocal;
            }
            else
            {
                sCurConString = sConstringServer;
            }
            SqlConnection sqlCon = new SqlConnection(sCurConString);
            SqlCommand sqlUpdateCommand = new SqlCommand(sCommandText, sqlCon);
            sqlCon.Open();
            sqlUpdateCommand.ExecuteNonQuery();
            sqlCon.Close();
        }
    }
}