using Dapper;
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
                var query = "SELECT I.Id,I.StoreId,FMC.Id As FoodCategoryId,FMC.FoodMenuCategoryName,I.FoodMenuId,FM.FoodMenuName,I.PhysicalDatetime AS PhysicalDatetime,I.PhysicalStockQty,I.PhysicalIsLock,I.PhysicalStockINQty,I.PhysicalStockOutQty,I.OpeningQty " +
                            "FROM dbo.Inventory I " +
                            "Inner Join FoodMenu FM On FM.Id = I.FoodMenuId  " +
                            "Inner Join FoodMenuCategory FMC On FMC.Id = FM.FoodCategoryId  " +
                            "Where I.IsDeleted = 0 And FM.Foodmenutype=1 And I.StoreId = " + storeId;

                if (foodCategoryId > 0)
                {
                    query += " And FMC.Id = " + foodCategoryId;
                }
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
                var query = "Exec InventoryPhysicalStock "+ storeId + ", " + foodmenuId;
                result = con.Query(query).ToString();
            }
            return result;
        }

    }
}
