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

                //var query = "SELECT " + //ROW_NUMBER() OVER(ORDER BY ' + @sort + ') AS ''Index''," +
                //            " INV.Id,I.IngredientName + '(' + I.Code + ')' as IngredientName,IG.IngredientCategoryName,INV.StockQty," +
                //            " case  when INV.StockQty < 0 THEN 0 else 1 end as StockQtyText,I.AlterQty" +
                //            " FROM inventory INV INNER JOIN Ingredient I ON INV.IngredientId = I.Id " +
                //            " INNER JOIN IngredientCategory IG on IG.Id = I.IngredientCategoryId " +
                //            " LEFT OUTER JOIN FoodMenuIngredient FMG on FMG.IngredientId = I.Id " +
                //            " LEFT OUTER JOIN FoodMenu FM ON FM.Id = FMG.FoodMenuId ";
                ////  " WHERE I.AlterQty < INV.StockQty  ";

                var query = " SELECT S.StoreName,INV.Id,F.FoodMenuName as FoodMenuName,FMC.FoodMenuCategoryName ,INV.StockQty, " +
                            " S.StoreName,INV.Id,F.FoodMenuCode,F.FoodMenuName as FoodMenuName,FMC.FoodMenuCategoryName ,INV.StockQty, " +
                            " F.PurchasePrice, (INV.StockQty * F.PurchasePrice) as Amount , U.Unitname," +
                            " case  when INV.StockQty < 0 THEN 0 else 1 end as StockQtyText,F.AlterQty" +
                            " FROM inventory INV INNER JOIN FoodMenu F ON INV.FoodMenuId = F.Id" +
                            " INNER JOIN FoodMenuCategory FMC on FMC.Id = F.FoodCategoryId" +
                            " inner join Store S on S.Id = INV.StoreId  inner join Units U on U.Id = F.UnitsId where INV.StockQty <> 0";
                ////  " WHERE I.AlterQty < INV.StockQty  ";


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

                var query = "select Purchase.Id as Id, PurchaseId as ReferenceNo," +
                             "convert(varchar(12), PurchaseDate, 3) as [Date]," +
                             "Supplier.SupplierName as Supplier," +
                             "Purchase.GrandTotal as GrandTotal," +
                             "Purchase.PaidAmount as Paid," +
                             "Purchase.DueAmount as Due," +
                             "STUFF( (SELECT ', ' + convert(varchar(10), i1.IngredientName, 120)" +
                              " FROM Ingredient i1 inner join PurchaseDetail on PurchaseDetail.IngredientId = i1.Id" +
                              " where Purchase.id = PurchaseDetail.PurchaseId" +
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

        public PrintReceiptA4 GetPrintReceiptA4Detail(int CustomerOrderId)
        {
            PrintReceiptA4 printReceiptA4 = new PrintReceiptA4();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string customerOrderDetail = string.Empty;
                string customerOrderItems = string.Empty;
                customerOrderDetail = "SELECT b.CustomerOrderId,b.Id As BillId,CO.SalesInvoiceNumber,B.BillDateTime,O.OutletName,U.Username,C.CustomerName,B.GrossAmount,B.TaxAmount,B.VatableAmount,CO.NonVatableAmount, B.Discount,B.ServiceCharge,B.TotalAmount,PM.PaymentMethodName,BD.BillAmount FROM Bill B  "+
                              " INNER JOIN CustomerOrder CO ON B.CustomerOrderId = CO.Id " +
                              " INNER JOIN BillDetail BD ON B.Id = BD.BillId " +
                              " INNER JOIN Outlet O ON O.Id = B.OutletId " +
                              " INNER JOIN[User] U ON U.ID = B.UserIdInserted " +
                              " INNER JOIN Customer C ON C.Id = b.CustomerId " +
                              " INNER JOIN PaymentMethod PM ON PM.Id = BD.PaymentMethodId " +
                              " WHERE b.CustomerOrderId =  " + CustomerOrderId;

                customerOrderItems = "SELECT FM.FoodMenuName,COI.FoodMenuQty,COI.FoodMenuRate,COI.Price,   " +
                             " (select case when foodmenutaxtype = 1 then 'V' when foodmenutaxtype = 2 then 'E' when foodmenutaxtype = 3 then 'Z' ELSE '' end) AS FOODVAT " +
                             " FROM CustomerOrderItem  COI " +
                             " INNER JOIN FoodMenu FM ON FM.ID = COI.FoodMenuId " +
                             " WHERE CustomerOrderId =  " + CustomerOrderId;

                printReceiptA4.PrintReceiptDetail = con.Query<PrintReceiptModel>(customerOrderDetail).FirstOrDefault();
                printReceiptA4.PrintReceiptItemList = con.Query<PrintReceiptItemModel>(customerOrderItems).ToList();

                return printReceiptA4;
            }
        }
    }
}
