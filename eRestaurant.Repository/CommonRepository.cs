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

        public int GetValidateUnique(string TableName, string ColumnName, string Value, string Rowid)
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string query = "SELECT * FROM " + TableName.ToString() + " WHERE IsDeleted=0 AND LTRIM(RTRIM(" + ColumnName + "))= LTRIM(RTRIM('" + Value + "')) AND Id <> " + Rowid;
                result = con.ExecuteScalar<string>(query);

                result = result != null ? result : "0";
            }
            return Int16.Parse(result);
        }
        public int GetValidateReference(string TableName, string Rowid)
        {
            List<ReferenceTable> referenceTables = new List<ReferenceTable>();
            referenceTables.Add(new ReferenceTable("Outlet", "CustomerOrder", "OutletId"));
            referenceTables.Add(new ReferenceTable("Outlet", "Bill", "OutletId"));
            referenceTables.Add(new ReferenceTable("Outlet", "FoodMenuRate", "OutletId"));
 
            referenceTables.Add(new ReferenceTable("Store", "Outlet", "StoreId"));
            referenceTables.Add(new ReferenceTable("Store", "Inventory", "StoreId"));
            referenceTables.Add(new ReferenceTable("Store", "InventoryTransfer", "ToStoreId"));
            referenceTables.Add(new ReferenceTable("Store", "InventoryTransfer", "FromStoreId"));
            referenceTables.Add(new ReferenceTable("Store", "InventoryAdjustment", "StoreId"));
            referenceTables.Add(new ReferenceTable("Store", "PurchaseGRN", "StoreId"));
            referenceTables.Add(new ReferenceTable("Store", "PurchaseInvoice", "StoreId"));
            referenceTables.Add(new ReferenceTable("Store", "Purchase", "StoreId"));

            referenceTables.Add(new ReferenceTable("AssetCategory", "AssetItem", "AssetCategoryId"));

            referenceTables.Add(new ReferenceTable("AssetItem", "InventoryTransferDetail", "AssetItemId"));
            referenceTables.Add(new ReferenceTable("AssetItem", "InventoryAdjustmentDetail", "AssetItemId"));
            referenceTables.Add(new ReferenceTable("AssetItem", "PurchaseGRNDetail", "AssetItemId"));
            referenceTables.Add(new ReferenceTable("AssetItem", "PurchaseInvoiceDetail", "AssetItemId"));
            referenceTables.Add(new ReferenceTable("AssetItem", "PurchaseDetail", "AssetItemId"));

            referenceTables.Add(new ReferenceTable("FoodMenuCategory", "Foodmenu", "FoodCategoryId"));

            referenceTables.Add(new ReferenceTable("Foodmenu", "FoodmenuRate", "FoodmenuId"));
            referenceTables.Add(new ReferenceTable("Foodmenu", "InventoryTransferDetail", "FoodmenuId"));
            referenceTables.Add(new ReferenceTable("Foodmenu", "InventoryAdjustmentDetail", "FoodmenuId"));
            referenceTables.Add(new ReferenceTable("Foodmenu", "PurchaseGRNDetail", "FoodmenuId"));
            referenceTables.Add(new ReferenceTable("Foodmenu", "PurchaseInvoiceDetail", "FoodmenuId"));
            referenceTables.Add(new ReferenceTable("Foodmenu", "PurchaseDetail", "FoodmenuId"));

            referenceTables.Add(new ReferenceTable("IngredientCategory", "Ingredient", "IngredientCategoryId"));

            referenceTables.Add(new ReferenceTable("Ingredient", "InventoryTransferDetail", "IngredientId"));
            referenceTables.Add(new ReferenceTable("Ingredient", "InventoryAdjustmentDetail", "IngredientId"));
            referenceTables.Add(new ReferenceTable("Ingredient", "PurchaseGRNDetail", "IngredientId"));
            referenceTables.Add(new ReferenceTable("Ingredient", "PurchaseInvoiceDetail", "IngredientId"));
            referenceTables.Add(new ReferenceTable("Ingredient", "PurchaseDetail", "IngredientId"));

            referenceTables.Add(new ReferenceTable("IngredientUnit", "Foodmenu", "UnitsId"));
            referenceTables.Add(new ReferenceTable("IngredientUnit", "Ingredient", "IngredientUnitId"));
            referenceTables.Add(new ReferenceTable("IngredientUnit", "AssetItem", "UnitId"));
 
            referenceTables.Add(new ReferenceTable("Supplier", "PurchaseGRN", "SupplierId"));
            referenceTables.Add(new ReferenceTable("Supplier", "PurchaseInvoice", "SupplierId"));
            referenceTables.Add(new ReferenceTable("Supplier", "Purchase", "SupplierId"));

            referenceTables.Add(new ReferenceTable("Tax", "Foodmenu", "FoodVatTaxId"));
            referenceTables.Add(new ReferenceTable("Tax", "Ingredient", "TaxId"));
            referenceTables.Add(new ReferenceTable("Tax", "AssetItem", "TaxId"));

            referenceTables.Add(new ReferenceTable("Customer", "CustomerOrder", "CustomerId"));

            referenceTables.Add(new ReferenceTable("Employee", "[User]", "EmployeeId"));

            referenceTables.Add(new ReferenceTable("GlobalStatus", "Purchase", "StoreId"));

            referenceTables.Add(new ReferenceTable("PaymentMethod", "BillDetail", "PaymentMethodId"));

            referenceTables.Add(new ReferenceTable("RawMaterial", "ingredientcategory", "RawMaterialId"));

            referenceTables.Add(new ReferenceTable("WebRole", "[User]", "WebRoleId"));

            referenceTables.Add(new ReferenceTable("Tables", "CustomerOrder", "TableId"));

            //          referenceTables.Add(new ReferenceTable("RewardSetup", "Purchase", "StoreId"));
            //            referenceTables.Add(new ReferenceTable("Bank", "Purchase", "StoreId"));
            //referenceTables.Add(new ReferenceTable("Section", "Purchase", "StoreId"));

            referenceTables = referenceTables.Where(x => x.TableName.Contains(TableName)).ToList();

            string result = ""; string query = ""; int validate = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                foreach (var item in referenceTables)
                {
                    if (item.TableName == TableName)
                    {
                        query = "SELECT Count(*) FROM " + item.ReferenceTableName.ToString() + " WHERE IsDeleted=0 AND " + item.ReferenceColumnName + "= " + Rowid;
                        result = con.ExecuteScalar<string>(query);

                        if (result != "0")
                        {
                            validate = 1;
                        }
                    }

                }
            }
            return validate;
        }
        public string InventoryPush(string docType, int id)
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
