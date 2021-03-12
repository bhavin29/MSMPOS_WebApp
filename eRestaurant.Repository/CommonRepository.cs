using Dapper;
using Microsoft.Extensions.Options;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RocketPOS.Repository
{
    public class CommonRepository : ICommonRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public CommonRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }
        public int InsertErrorLog(ErrorModel errorModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO ErrorLog(MethodName," +
                  "ErrorPath, " +
                  "ErrorDetails," +
                  "UserId) " +
                  "VALUES(@MethodName," +
                  " @ErrorPath," +
                  " @ErrorDetails," +
                  "@UserId);" +
                  " SELECT CAST(SCOPE_IDENTITY() as INT); ";
                result = con.Execute(query, errorModel, sqltrans, 0, System.Data.CommandType.Text);
                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
            }

            return result;
        }

        public int GetMaxId(string TableName)
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string query = "SELECT ISNULL(Max(Id),0)+1 FROM " + TableName.ToString();
                result = con.ExecuteScalar<string>(query);
            }
            return Int16.Parse(result);
        }

        public int GetValidateUnique(string TableName,string ColumnName,string Value, string Rowid)
        {
            string result ="";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string query = "SELECT * FROM " + TableName.ToString() + " WHERE IsDeleted=0 AND LTRIM(RTRIM(" + ColumnName + "))= LTRIM(RTRIM('" + Value +"')) AND Id <> " + Rowid ;
                result = con.ExecuteScalar<string>(query);

                result = result != null ? result : "0";
            }
            return Int16.Parse(result);
        }
        public int GetValidateReference(string TableName,string Rowid)
        {
            List<ReferenceTable> referenceTables= new List<ReferenceTable>();
            referenceTables.Add(new ReferenceTable ( "Outlet", "CustomerOrder", "OutletId"));
            referenceTables.Add(new ReferenceTable("Store", "Outlet", "StoreId"));
            referenceTables.Add(new ReferenceTable("Outlet", "Bill", "OutletId"));
            referenceTables.Add(new ReferenceTable("Outlet", "Customer", "OutletId"));
            referenceTables.Add(new ReferenceTable("Outlet", "FoodMenu", "OutletId"));
            referenceTables.Add(new ReferenceTable("Store", "Inventory", "StoreId"));
            referenceTables.Add(new ReferenceTable("Store", "InventoryAdjustment", "StoreId"));
            referenceTables.Add(new ReferenceTable("Store", "PurchaseGRN", "StoreId"));
            referenceTables.Add(new ReferenceTable("Store", "PurchaseInvoice", "StoreId"));
            referenceTables.Add(new ReferenceTable("Store", "Purchase", "StoreId"));

            string result = ""; string query = "";int validate = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                  foreach( var item in referenceTables) 
                {
                    if (item.TableName == TableName)
                    {
                        query = "SELECT Count(*) FROM " + item.ReferenceTableName.ToString() + " WHERE IsDeleted=0 AND " + item.ReferenceColumnName + "= " + Rowid;
                        result = con.ExecuteScalar<string>(query);

                        if (result != null)
                        {
                            validate = 1;
                        }
                    }

                }
            }
            return validate;
        }
        public string InventoryPush(string docType,int id)
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {

                var query = "Exec InventoryPush '" + docType + "'," + id;
                result = con.Query(query).ToString();
            }
            return result;
        }

        public ClientModel GetEmailSettings()
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string query = "Select FromEmailAddress,EmailDisplayName,FromEmailPassword,PurchaseApprovalEmail From Client";
                return con.Query<ClientModel>(query).FirstOrDefault();
            }
        }

        public int UpdateEmailSettings(ClientModel clientModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE Client SET FromEmailAddress =@FromEmailAddress," +
                            "EmailDisplayName = @EmailDisplayName,PurchaseApprovalEmail=@PurchaseApprovalEmail, " +
                            "FromEmailPassword = @FromEmailPassword ";
                result = con.Execute(query, clientModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
            }
            return result;
        }
        public string SyncTableStatus(string tableName)
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "update synctable set IsActive=1 where Tablename ='" + tableName + "'";

                result = con.Query(query).ToString();

            }
            return result;
        }


    }

    public class ReferenceTable
    {
        public string TableName { get; set; }
        public string ReferenceTableName { get; set; }
        public string ReferenceColumnName { get; set; }
        public ReferenceTable(string _TableName, string _ReferenceTableName, string _ReferenceColumnName)
        {
            this.TableName = _TableName;
            this.ReferenceTableName = _ReferenceTableName;
            this.ReferenceColumnName = _ReferenceColumnName;
        }
    }

}
