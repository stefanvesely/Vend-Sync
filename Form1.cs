using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Vend_Sync
{
    public partial class frmSyncServer : Form
    {
        public string sConstringServer = "";
        public string sConstringLocal = "";
        public string sGetTables = @"SELECT t.name as TableName, SCHEMA_NAME(t.schema_id) As SchemaName,I.rows as RecordCount from sysindexes i INNER JOIN sys.tables t on i.id=t.object_id";

        public frmSyncServer()
        {
            InitializeComponent();
        }

        private void brnAddNew_Click(object sender, EventArgs e)
        {
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
        }

        private void btnRetrieveData_Click(object sender, EventArgs e)
        {
        }

        private void RetrieveRemoteData()
        {
        }

        private void RetrieveLocalData()
        {
        }

        private void TransferFromRemote()
        {
        }

        private void TransferToRemote()
        {
        }

        private List<int> ListIndexsOfDiscrepencies(DataTable dtSourceTable, DataTable dtDestinationTable)
        {
            List<int> lstIndexList = new List<int>();
            for (int iRow = 0; iRow < dtSourceTable.Rows.Count; iRow++)
            {
                if (iRow > dtDestinationTable.Rows.Count - 1)
                {
                    lstIndexList.Add(iRow);
                }
                else
                {
                    for (int iCol = 0; iCol < dtSourceTable.Columns.Count; iCol++)
                    {
                        if (dtSourceTable.Rows[iRow][iCol] != dtDestinationTable.Rows[iRow][iCol])
                        {
                            if (!lstIndexList.Contains(iRow))
                            {
                                lstIndexList.Add(iRow);
                            }
                        }
                    }
                }
            }

            return lstIndexList;
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
    }
}