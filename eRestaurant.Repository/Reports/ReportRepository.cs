using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using RocketPOS.Models;
using RocketPOS.Interface.Repository;
using System.Data.SqlClient;
using System.Linq;
using RocketPOS.Models.Reports;
using RocketPOS.Interface.Repository.Reports;
using System;

namespace RocketPOS.Repository.Reports
{
    public class ReportRepository : IReportRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public ReportRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<InventoryReportModel> GetInventoryReport(InventoryReportParamModel inventoryReportParamModel)
        {
            List<InventoryReportModel> inventoryReportModel = new List<InventoryReportModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {

                var query = "SELECT " + //ROW_NUMBER() OVER(ORDER BY ' + @sort + ') AS ''Index''," +
                            " INV.Id,I.IngredientName + '(' + I.Code + ')' as IngredientName,IG.IngredientCategoryName,INV.StockQty," +
                            " case  when INV.StockQty < 0 THEN 0 else 1 end as StockQtyText,I.AlterQty" +
                            " FROM inventory INV INNER JOIN Ingredient I ON INV.IngredientId = I.Id " +
                            " INNER JOIN IngredientCategory IG on IG.Id = I.IngredientCategoryId " +
                            " LEFT OUTER JOIN FoodMenuIngredient FMG on FMG.IngredientId = I.Id " +
                            " LEFT OUTER JOIN FoodMenu FM ON FM.Id = FMG.FoodMenuId ";
                          //  " WHERE I.AlterQty < INV.StockQty  ";

                //if (inventoryReportParamModel.StoreId.ToString().Length != 0)
                //    query = query + " AND INV.StoreId = " + inventoryReportParamModel.StoreId.ToString();

                //if (inventoryReportParamModel.FoodMenuId.ToString().Length != 0)
                //    query = query + " AND FM.Id =" + inventoryReportParamModel.FoodMenuId.ToString();

                //if (inventoryReportParamModel.IngredientCategoryId.ToString().Length != 0)
                //    query = query + " AND I.IngredientCategoryId =" + inventoryReportParamModel.IngredientCategoryId.ToString();

                //if (inventoryReportParamModel.IngredientId.ToString().Length != 0)
                //    query = query + " AND INV.IngredientId = " + inventoryReportParamModel.IngredientId.ToString();

                inventoryReportModel = con.Query<InventoryReportModel>(query).ToList();
            }

            return inventoryReportModel;
        }
    }
}
