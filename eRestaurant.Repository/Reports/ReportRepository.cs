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
        public List<PurchaseReportModel> GetPurchaseReport(DateTime fromDate, DateTime toDate)
        {
            List<PurchaseReportModel> purchaseReportModel = new List<PurchaseReportModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {

                var query = "select Purchase.Id as Id, PurchaseNumber as ReferenceNo," +
                             "convert(varchar(12), PurchaseDate, 3) as [Date]," +
                             "Supplier.SupplierName as Supplier," +
                             "Purchase.GrandTotal as GrandTotal," +
                             "Purchase.PaidAmount as Paid," +
                             "Purchase.DueAmount as Due," +
                             "STUFF( (SELECT ', ' + convert(varchar(10), i1.IngredientName, 120)" +
                              " FROM Ingredient i1 inner join PurchaseIngredient on PurchaseIngredient.IngredientId = i1.Id" +
                              " where Purchase.id = PurchaseIngredient.PurchaseId" +
                              " FOR XML PATH(''))" +
                              " , 1, 2, '')  AS Ingredients," +
                             " 'Rocket Admin' as PurchaseBy" +
                             " from Purchase inner join Supplier on Purchase.SupplierId = Supplier.Id " +
                             "where Purchase.Isdeleted = 0 and PurchaseDate between convert(datetime, '" + fromDate + "',103) and convert(datetime, '" + toDate + "',103)";

                purchaseReportModel = con.Query<PurchaseReportModel>(query).ToList();
            }

            return purchaseReportModel;
        }

        public List<OutletRegisterReportModel> GetOutletRegisterReport(int OutletRegisterId)
        {
            List<OutletRegisterReportModel> outletRegisterReportModels = new List<OutletRegisterReportModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {

                var query = "Exec rptUserRegister " + OutletRegisterId + ";";
                    outletRegisterReportModels = con.Query<OutletRegisterReportModel>(query).ToList();
            }

            return outletRegisterReportModels;
        }

    }
}
