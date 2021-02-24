using Dapper;
using FastMember;
using Microsoft.Extensions.Options;
using RocketPOS.Framework;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RocketPOS.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public InventoryRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<InventoryDetail> GetInventoryDetailList(int storeId, int foodCategoryId)
        {
            List<InventoryDetail> inventoryDetail = new List<InventoryDetail>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT I.Id,I.StoreId,FMC.Id As FoodCategoryId,FMC.FoodMenuCategoryName,I.FoodMenuId,FM.FoodMenuName," +
                            " (Case when I.PhysicalDatetime  is  null then Getdate() else I.PhysicalDatetime end) as PhysicalDatetime," +
                            " I.PhysicalStockQty,I.PhysicalIsLock,I.PhysicalStockINQty,I.PhysicalStockOutQty,I.OpeningQty " +
                            "FROM dbo.Inventory I " +
                            "Inner Join FoodMenu FM On FM.Id = I.FoodMenuId  " +
                            "Inner Join FoodMenuCategory FMC On FMC.Id = FM.FoodCategoryId  " +
                            "Where I.IsDeleted = 0  And I.StoreId = " + storeId;

                if (foodCategoryId > 0)
                {
                    query += " And FMC.Id = " + foodCategoryId;
                }

                query += " Order by FMC.FoodMenuCategoryName,FM.FoodMenuName";

                inventoryDetail = con.Query<InventoryDetail>(query).ToList();
            }
            return inventoryDetail;
        }

        public int UpdateInventoryDetailList(List<InventoryDetail> inventoryDetails)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                foreach (var item in inventoryDetails)
                {
                    var query = "Update Inventory set PhysicalDatetime = @PhysicalDatetime, PhysicalStockQty = @PhysicalStockQty, PhysicalIsLock = @PhysicalIsLock, UserIdUpdated = " + LoginInfo.Userid + ", DateUpdated = GETUTCDATE() Where Id = @Id";
                    result = con.Execute(query, item, sqltrans, 0, System.Data.CommandType.Text);
                }
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

        public string StockUpdate(int storeId, int foodmenuId)
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Exec InventoryPhysicalStock " + storeId + ", " + foodmenuId;
                result = con.Query(query).ToString();
            }
            return result;
        }

        public List<InventoryOpenigStockImport> GetInventoryOpeningStockByStore(int storeId, int foodCategoryId)
        {
            List<InventoryOpenigStockImport> inventoryOpenigStockImports = new List<InventoryOpenigStockImport>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select StoreId,FoodmenuId,foodmenucategoryname,foodmenuname,PhysicalDatetime,PhysicalIsLock,PhysicalLastCalcDatetime,PhysicalStockINQty,PhysicalStockOutQty,PhysicalStockQty,ImportBatch from inventory i " +
                            " inner join foodmenu f on f.id = i.foodmenuid " +
                            " inner join foodmenucategory fc on fc.id = f.foodcategoryid " +
                            " Where I.IsDeleted = 0  And I.StoreId = " + storeId;

                if (foodCategoryId > 0)
                {
                    query += " And FC.Id = " + foodCategoryId;
                }

                query += " Order by FoodMenuCategoryName,FoodMenuName";

                inventoryOpenigStockImports = con.Query<InventoryOpenigStockImport>(query).ToList();
            }
            return inventoryOpenigStockImports;
        }

        public int BulkImport(List<InventoryOpenigStockImport> inventoryOpenigStockImports)
        {
   
            var copyParameters = new[]
             {
                        nameof(InventoryOpenigStockImport.StoreId),
                        nameof(InventoryOpenigStockImport.FoodmenuId),
                        nameof(InventoryOpenigStockImport.PhysicalStockQty),
                        nameof(InventoryOpenigStockImport.ImportBatch)
                    };

            using (var sqlCopy = new SqlBulkCopy(_ConnectionString.Value.ConnectionString))
            {
                sqlCopy.DestinationTableName = "[TempBulkOpeningStock]";
                sqlCopy.BatchSize = 700;
                using (var reader = ObjectReader.Create(inventoryOpenigStockImports, copyParameters))
                {
                    sqlCopy.WriteToServer(reader);
                }
            }

           int result =  BulkImportUpdate(inventoryOpenigStockImports[0].ImportBatch);

           string strResult= BulkImportOpeningReset();

            return result;
        }
        public int BulkImportUpdate(string importbatch)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = " update I Set I.PhysicalStockQty = T.PhysicalStockQty " +
                            " from inventory I inner join [TempBulkOpeningStock] T  on I.Storeid = T.Storeid and I.Foodmenuid = T.foodmenuid " +
                            " where T.importbatch = '" + importbatch + "' ";

                result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);
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

        public string BulkImportOpeningReset()
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {

                var query = "Exec InventoryPhysicalStockScheduler ";
                result = con.Query(query).ToString();
            }
            return result;
        }
    }
}
