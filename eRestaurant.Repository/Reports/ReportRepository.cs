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
                var query = "";
                 query = " SELECT S.StoreName,INV.Id,F.FoodMenuName as FoodMenuName,FMC.FoodMenuCategoryName ,INV.StockQty, INV.OpeningQty as OpeningQty, " +
                            " S.StoreName,INV.Id,F.FoodMenuCode,F.FoodMenuName as FoodMenuName,FMC.FoodMenuCategoryName ,INV.StockQty, " +
                            " F.PurchasePrice, (INV.StockQty * F.PurchasePrice) as Amount , U.Unitname," +
                            " case  when INV.StockQty < 0 THEN 0 else 1 end as StockQtyText,F.AlterQty" +
                            " FROM inventory INV INNER JOIN FoodMenu F ON INV.FoodMenuId = F.Id" +
                            " INNER JOIN FoodMenuCategory FMC on FMC.Id = F.FoodCategoryId" +
                            " inner join Store S on S.Id = INV.StoreId  inner join Units U on U.Id = F.UnitsId " +
                            " where INV.StockQty <> 0  or ISNULL(OpeningQty,0) <>0";

                //query = "SELECT S.StoreName,INV.Id,I.IngredientName as FoodMenuName,IC.IngredientCategoryName as FoodMenuCategoryName ,INV.StockQty, INV.OpeningQty as OpeningQty, " +
                //         " S.StoreName,INV.Id,I.Code as FoodMenuCode ,INV.StockQty,  " +
                //         " I.PurchasePrice, (INV.StockQty * I.PurchasePrice) as Amount , U.Unitname, " +
                //         " case  when INV.StockQty < 0 THEN 0 else 1 end as StockQtyText,I.AlterQty" +
                //         " FROM inventory INV INNER JOIN Ingredient I ON INV.IngredientId = I.Id " +
                //         " INNER JOIN IngredientCategory IC on IC.Id = I.IngredientCategoryId " +
                //         " inner join Store S on S.Id = INV.StoreId  inner join Units U on U.Id = I.IngredientUnitId " +
                //         " where INV.StockQty <> 0  or ISNULL(OpeningQty,0) <> 0;";

                inventoryReportModel = con.Query<InventoryReportModel>(query).ToList();
            }

            return inventoryReportModel;
        }
        public List<InventoryDetailReportModel> GetInventoryDetailReport(InventoryReportParamModel inventoryReportParamModel, int id)
        {
            List<InventoryDetailReportModel> inventoryReportModel = new List<InventoryDetailReportModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " SELECT Convert(varchar(10), ID.DocDate, 103)as DocDate,ID.StoreId,DocType,DocTable,DocNumber,DocNumberId,DocNumberDetailId,SupplierId, " +
                            " StoreName,SupplierName,ID.FoodMenuId,ID.IngredientId,Reamrks,StockInQty,StockOutQty,BalanceQty " +
                            " FROM InventoryDetail ID inner join Inventory I ON I.StoreId = ID.Storeid and (I.FoodmenuId = ID.FoodMenuid  or  I.IngredientID = ID.IngredientID) " +
                            " inner join Store S ON S.ID = ID.StoreID " +
                            " left join SUPPLIER SP on SP.Id = ID.SupplierId " +
                            " WHERE I.ID=" + id + "Order by ID.DocDate asc";
                //  " where Convert(varchar(10), ID.DocDate, 103) between '01/01/2021' and '01/01/2022'"

                ////  " WHERE I.AlterQty < INV.StockQty  ";

                //if (inventoryReportParamModel.StoreId.ToString().Length != 0)
                //    query = query + " AND INV.StoreId = " + inventoryReportParamModel.StoreId.ToString();

                //if (inventoryReportParamModel.FoodMenuId.ToString().Length != 0)
                //    query = query + " AND FM.Id =" + inventoryReportParamModel.FoodMenuId.ToString();

                //if (inventoryReportParamModel.IngredientCategoryId.ToString().Length != 0)
                //    query = query + " AND I.IngredientCategoryId =" + inventoryReportParamModel.IngredientCategoryId.ToString();

                //if (inventoryReportParamModel.IngredientId.ToString().Length != 0)
                //    query = query + " AND INV.IngredientId = " + inventoryReportParamModel.IngredientId.ToString();

                inventoryReportModel = con.Query<InventoryDetailReportModel>(query).ToList();
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
                customerOrderDetail = "SELECT b.CustomerOrderId,b.Id As BillId,CO.SalesInvoiceNumber,B.BillDateTime,O.OutletName,isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as Username,C.CustomerName,B.GrossAmount,B.TaxAmount,B.VatableAmount,CO.NonVatableAmount, B.Discount,B.ServiceCharge,B.TotalAmount,PM.PaymentMethodName,BD.BillAmount FROM Bill B  " +
                              " INNER JOIN CustomerOrder CO ON B.CustomerOrderId = CO.Id " +
                              " INNER JOIN BillDetail BD ON B.Id = BD.BillId " +
                              " INNER JOIN Outlet O ON O.Id = B.OutletId " +
                              " INNER JOIN[User] U ON U.ID = B.UserIdInserted " +
                              " inner join employee e on e.id = u.employeeid " +
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

        public List<InventoryReportModel> GetInventoryStockList(int supplierId, int storeId,int itemType,int active,string reportDate)
        {
            List<InventoryReportModel> inventoryReportModel = new List<InventoryReportModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                DateTime dt = new DateTime();

                dt = Convert.ToDateTime(reportDate);

                var query = "Exec WebRptCurrentStock " + supplierId + "," + storeId + "," + itemType + "," + active + ",'" + dt.ToString("dd/MM/yyyy") + "'";

                inventoryReportModel = con.Query<InventoryReportModel>(query).ToList();

                return inventoryReportModel;


                if (itemType == 0) 
                {
                        query = " SELECT S.StoreName,INV.Id,F.FoodMenuName as FoodMenuName,FMC.FoodMenuCategoryName ,INV.StockQty, INV.OpeningQty, " +
                                " S.StoreName,INV.Id,F.FoodMenuCode,F.FoodMenuName as FoodMenuName,FMC.FoodMenuCategoryName ,INV.StockQty, " +
                                " F.PurchasePrice, (INV.StockQty * F.PurchasePrice) as Amount , U.UnitShortName as Unitname," +
                                " case  when INV.StockQty < 0 THEN 0 else 1 end as StockQtyText,F.AlterQty" +
                                " FROM inventory INV INNER JOIN FoodMenu F ON INV.FoodMenuId = F.Id" +
                                " INNER JOIN FoodMenuCategory FMC on FMC.Id = F.FoodCategoryId" +
                                " inner join Store S on S.Id = INV.StoreId  inner join Units U on U.Id = F.UnitsId ";
                }
                else if (itemType == 1)
                {
                    query = "SELECT S.StoreName,INV.Id,I.IngredientName as FoodMenuName,IC.IngredientCategoryName as FoodMenuCategoryName ,INV.StockQty, INV.OpeningQty as OpeningQty, " +
                             " S.StoreName,INV.Id,I.Code as FoodMenuCode ,INV.StockQty,  " +
                             " I.PurchasePrice, (INV.StockQty * I.PurchasePrice) as Amount , U.UnitShortName as Unitname, " +
                             " case  when INV.StockQty < 0 THEN 0 else 1 end as StockQtyText,I.AlterQty" +
                             " FROM inventory INV INNER JOIN Ingredient I ON INV.IngredientId = I.Id " +
                             " INNER JOIN IngredientCategory IC on IC.Id = I.IngredientCategoryId " +
                             " inner join Store S on S.Id = INV.StoreId  inner join Units U on U.Id = I.IngredientUnitId ";
                         //    " where INV.StockQty <> 0  or ISNULL(OpeningQty,0) <> 0;";
                }
                else if (itemType == 2)
                {
                    query = "SELECT S.StoreName,INV.Id,I.AssetItemName as FoodMenuName,IC.AssetCategoryName as FoodMenuCategoryName ,INV.StockQty, INV.OpeningQty as OpeningQty, " +
                             " S.StoreName,INV.Id,I.Code as FoodMenuCode ,INV.StockQty,  " +
                             " 0 as PurchasePrice, (INV.StockQty * 1) as Amount , U.UnitShortName as Unitname, " +
                             " case  when INV.StockQty < 0 THEN 0 else 1 end as StockQtyText,0 as AlterQty" +
                             " FROM inventory INV INNER JOIN AssetItem I ON INV.AssetItemId = I.Id " +
                             " INNER JOIN AssetCategory IC on IC.Id = I.AssetCategoryId " +
                             " inner join Store S on S.Id = INV.StoreId  inner join Units U on U.Id = I.UnitId ";
                    //    " where INV.StockQty <> 0  or ISNULL(OpeningQty,0) <> 0;";
                }

                if (itemType == 0 & active==0)
                {
                    query += " left join Outlet O on O.Storeid = S.Id " +
                               " right join Foodmenurate FMR on FMR.Foodmenuid = F.Id and FMR.outletid = O.Id  and FMR.IsActive=1";
                 }


                if (supplierId != 0 && itemType==0)
                {
                    query += "  inner join SupplierItem SI on SI.FoodMenuId = INV.FoodMenuId And SI.SupplierId = " + supplierId;
                }
                 else if (supplierId != 0 && itemType == 1)
                {
                    query += "  inner join SupplierItem SI on SI.IngredientID = INV.IngredientID And SI.SupplierId = " + supplierId;
                }
                else if (supplierId != 0 && itemType == 1)
                {
                    query += "  inner join SupplierItem SI on SI.AssetItemId = INV.AssetItemId And SI.SupplierId = " + supplierId;
                }

                query = query + " where INV.StoreId = " + storeId;

                if (itemType == 0)
               {
                    query += "   ORDER BY  F.Foodmenuname,INV.StockQty desc ";
                }
                else if (itemType == 1)
                {
                    query += "   ORDER BY  I.IngredientName,INV.StockQty desc ";
                }
                else if (itemType == 2)
                {
                    query += "   ORDER BY  I.AssetItemName,INV.StockQty desc ";
                }

                inventoryReportModel = con.Query<InventoryReportModel>(query).ToList();
            }
            return inventoryReportModel;
        }
        public List<DataHistorySyncReportModel> GetDataSyncHistoryReport()
        {
            List<DataHistorySyncReportModel> dataHistorySyncReportModels = new List<DataHistorySyncReportModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " SELECT  [Outlet],[Process],[ProcessDate] ,[ProcessStatus] " +
                            " FROM SyncHistory order by ProcessDate desc";

                dataHistorySyncReportModels = con.Query<DataHistorySyncReportModel>(query).ToList();
            }

            return dataHistorySyncReportModels;
        }

        //POS Reports
        public List<MasterSalesReportModel> GetMasterSaleReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        {
            List<MasterSalesReportModel> masterSalesReportModel = new List<MasterSalesReportModel>();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string Query = string.Empty;

                Query = " select CONVERT(VARCHAR(10),CO.OrderDate,103) As OrderDate,CONVERT(VARCHAR(8),CO.OrderDate,108)  As OrderTime,CO.SalesInvoiceNumber," +
                        " FM.FoodMenuName ,COI.FoodMenuRate,FoodMenuQty,COI.Price,COI.Discount,COI.FoodMenuVat As Tax,COI.GrossAmount,FMC.FoodMenuCategoryName " +
                        " from CustomerOrder CO " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId And COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId " +
                        " Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId " +
                        " Where Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                if (outletId != 0)
                {
                    Query += " And CO.OutletId = " + outletId + " And COI.OutletId = " + outletId;
                }

                if (categoryId != 0)
                {
                    Query += " And FMC.Id = " + categoryId;
                }
                if (foodMenuId != 0)
                {
                    Query += " And FM.Id = " + foodMenuId;
                }

                Query += "order by CO.Orderdate,Salesinvoicenumber";

                masterSalesReportModel = db.Query<MasterSalesReportModel>(Query).ToList();
            }
            return masterSalesReportModel;
        }
        public List<DetailedDailyReportModel> GetDetailedDailyByDate(string Fromdate, string Todate,int outletId)
        {
            List<DetailedDailyReportModel> detailedDailyReportModels = new List<DetailedDailyReportModel>();
            using (var connection = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Exec rptTodaySummary " + outletId + ",'" + Fromdate + "','" + Todate + "'; ";

                detailedDailyReportModels = connection.Query<DetailedDailyReportModel>(query).ToList();
            }

            return detailedDailyReportModels;
        }
        public List<DetailSaleSummaryModel> GetDetailSaleSummaryReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        {
            List<DetailSaleSummaryModel> detailSaleSummaryModel = new List<DetailSaleSummaryModel>();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string Query = string.Empty;

                Query = " select CONVERT(VARCHAR(10),CO.OrderDate,103) As OrderDate,FM.FoodMenuName,sum(COI.FoodMenuQty) AS TotalQty,sum(COI.GrossAmount) AS TotalGrossAmount,sum(COI.Discount)  AS TotalDiscountAmount,sum(COI.Price)  AS TotalNetAmount,sum(T.TaxPercentage) AS TotalTaxPercentage,sum(B.GrossAmount)  AS TotalBillGrossAmount, " +
                        " sum(Case When PM.PaymentMethodName ='CASH' Then BD.BillAmount End) AS CashPayment, " +
                        " sum(Case When PM.PaymentMethodName = 'CREDIT CARD' Or PM.PaymentMethodName = 'DEBIT CARD' Then BD.BillAmount End) AS CardPayment  from Bill B " +
                        " Inner Join CustomerOrder CO On Co.MasterId=B.CustomerOrderId and CO.OutletId = " + outletId + " and B.OutletId = " + outletId +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId " +
                        " inner join BillDetail BD ON BD.BillId=B.MasterId and B.OutletId = " + outletId + " and BD.OutletId = " + outletId + 
                        " Inner join PaymentMethod PM On PM.Id=BD.PaymentMethodId " +
                        " Left Join Tax T On T.Id = COI.foodmenuvattaxid " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                if (categoryId != 0)
                {
                    Query += " And FM.FoodCategoryId = " + categoryId;
                }
                if (foodMenuId != 0)
                {
                    Query += " And FM.Id = " + foodMenuId;
                }

                Query += " group by CONVERT(VARCHAR(10),CO.OrderDate,103),FM.FoodMenuName " +
                        " WITH ROLLUP  " +
                        " order by CONVERT(VARCHAR(10),CO.OrderDate,103)  ";
                detailSaleSummaryModel = db.Query<DetailSaleSummaryModel>(Query).ToList();
                return detailSaleSummaryModel;
            }
        }
        public List<ProductWiseSalesReportModel> GetProductWiseSales(string Fromdate, string Todate, string ReportType, int outletId)
        {
            List<ProductWiseSalesReportModel> productWiseSalesReportModels = new List<ProductWiseSalesReportModel>();
            using (var connection = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Exec [rptProductWiseSumary] " + outletId + ",'" + Fromdate + "','" + Todate + "','" + ReportType + "'; ";

                productWiseSalesReportModels = connection.Query<ProductWiseSalesReportModel>(query).ToList();
            }

            return productWiseSalesReportModels;
        }
        public List<SalesByCategoryProductModel> GetSaleByCategorySectionReport(string fromDate, string toDate, string reportName, int categoryId, int foodMenuId, int outletId)
        {
            List<SalesByCategoryProductModel> salesByCategoryProductModel = new List<SalesByCategoryProductModel>();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string Query = string.Empty;

                if (reportName == "SalesByCategoryProductQtyDesc")
                {
                    Query = " select FMC.FoodMenuCategoryName,FM.FoodMenuName,Sum(COI.FoodMenuRate) As TotalUnitPrice,Sum(COI.FoodMenuQty) As TotalQty,Sum(COI.Price) As TotalPrice,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount, " +
                        "  cast(SUM(COI.GrossAmount) * 100.0 / SUM(SUM(COI.GrossAmount)) OVER ()  as numeric(18,2)) AS ValuePercentage  " +
                        " from CustomerOrder CO " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId " +
                        " Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                     if (categoryId != 0)
                    {
                        Query += " And FMC.Id = " + categoryId;
                    }
                    if (foodMenuId != 0)
                    {
                        Query += " And FM.Id = " + foodMenuId;
                    }

                    Query += " group by FMC.FoodMenuCategoryName,FM.FoodMenuName " +
                        " WITH ROLLUP  " +  // Returns Sub Totals For Group By
                        " Order By Sum(COI.FoodMenuQty) desc ";
                }

                if (reportName == "SalesByCategoryProductQtyAsc")
                {
                    Query = " select FMC.FoodMenuCategoryName,FM.FoodMenuName,Sum(COI.FoodMenuRate) As TotalUnitPrice,Sum(COI.FoodMenuQty) As TotalQty,Sum(COI.Price) As TotalPrice,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount, " +
                        "  cast(SUM(COI.GrossAmount) * 100.0 / SUM(SUM(COI.GrossAmount)) OVER ()  as numeric(18,2)) AS ValuePercentage " +
                        " from CustomerOrder CO " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId " +
                        " Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                     if (categoryId != 0)
                    {
                        Query += " And FMC.Id = " + categoryId;
                    }
                    if (foodMenuId != 0)
                    {
                        Query += " And FM.Id = " + foodMenuId;
                    }

                    Query += " group by FMC.FoodMenuCategoryName,FM.FoodMenuName " +
                        " WITH ROLLUP  " +
                        " Order By Sum(COI.FoodMenuQty) asc ";
                }

                if (reportName == "SalesByCategoryProductAmountDesc")
                {
                    Query = " select FMC.FoodMenuCategoryName,FM.FoodMenuName,Sum(COI.FoodMenuRate) As TotalUnitPrice,Sum(COI.FoodMenuQty) As TotalQty,Sum(COI.Price) As TotalPrice,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount, " +
                        "  cast(SUM(COI.GrossAmount) * 100.0 / SUM(SUM(COI.GrossAmount)) OVER ()  as numeric(18,2)) AS ValuePercentage " +
                        " from CustomerOrder CO " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId + 
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId " +
                        " Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                     if (categoryId != 0)
                    {
                        Query += " And FMC.Id = " + categoryId;
                    }
                    if (foodMenuId != 0)
                    {
                        Query += " And FM.Id = " + foodMenuId;
                    }

                    Query += " group by FMC.FoodMenuCategoryName,FM.FoodMenuName " +
                        " WITH ROLLUP  " +
                        " Order By Sum(COI.Price) desc ";
                }

                if (reportName == "SalesBySectionCategoryProductAmountAsc")
                {
                    Query = " select case when CO.OrderType = 1 then 'DineIN' When CO.OrderType = 2 then 'TakeAway' When CO.OrderType = 2 then 'Delivery' else 'ALL' End As SectionName, " +
                        " FMC.FoodMenuCategoryName,FM.FoodMenuName,Sum(COI.FoodMenuRate) As TotalUnitPrice,Sum(COI.FoodMenuQty) As TotalQty,Sum(COI.Price) As TotalPrice,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount, " +
                        "  cast(SUM(COI.GrossAmount) * 100.0 / SUM(SUM(COI.GrossAmount)) OVER ()  as numeric(18,2)) AS ValuePercentage " +
                        " from CustomerOrder CO " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId " +
                        " Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                     if (categoryId != 0)
                    {
                        Query += " And FMC.Id = " + categoryId;
                    }
                    if (foodMenuId != 0)
                    {
                        Query += " And FM.Id = " + foodMenuId;
                    }

                    Query += " group by CO.OrderType,FMC.FoodMenuCategoryName,FM.FoodMenuName " +
                        " WITH ROLLUP  " +
                        " Order By CO.OrderType,FMC.FoodMenuCategoryName,FM.FoodMenuName,Sum(COI.Price) asc ";
                }

                if (reportName == "SalesBySectionCategoryProductAmountDesc")
                {
                    Query = " select case when CO.OrderType = 1 then 'DineIN' When CO.OrderType = 2 then 'TakeAway' When CO.OrderType = 2 then 'Delivery' else 'ALL' End As SectionName, " +
                        " FMC.FoodMenuCategoryName,FM.FoodMenuName,Sum(COI.FoodMenuRate) As TotalUnitPrice,Sum(COI.FoodMenuQty) As TotalQty,Sum(COI.Price) As TotalPrice,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount, " +
                        "  cast(SUM(COI.GrossAmount) * 100.0 / SUM(SUM(COI.GrossAmount)) OVER ()  as numeric(18,2)) AS ValuePercentage " +
                        " from CustomerOrder CO " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId " +
                        " Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                     if (categoryId != 0)
                    {
                        Query += " And FMC.Id = " + categoryId;
                    }
                    if (foodMenuId != 0)
                    {
                        Query += " And FM.Id = " + foodMenuId;
                    }

                    Query += " group by CO.OrderType,FMC.FoodMenuCategoryName,FM.FoodMenuName " +
                        " WITH ROLLUP  " +
                        " Order By CO.OrderType,FMC.FoodMenuCategoryName,FM.FoodMenuName,Sum(COI.Price) desc ";
                }

                if (reportName == "SalesBySectionCategoryProductQtyAsc")
                {
                    Query = " select case when CO.OrderType = 1 then 'DineIN' When CO.OrderType = 2 then 'TakeAway' When CO.OrderType = 2 then 'Delivery' else 'ALL' End As SectionName, " +
                        " FMC.FoodMenuCategoryName,FM.FoodMenuName,Sum(COI.FoodMenuRate) As TotalUnitPrice,Sum(COI.FoodMenuQty) As TotalQty,Sum(COI.Price) As TotalPrice,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount, " +
                        "  cast(SUM(COI.GrossAmount) * 100.0 / SUM(SUM(COI.GrossAmount)) OVER ()  as numeric(18,2)) AS ValuePercentage " +
                        " from CustomerOrder CO " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId " +
                        " Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                     if (categoryId != 0)
                    {
                        Query += " And FMC.Id = " + categoryId;
                    }
                    if (foodMenuId != 0)
                    {
                        Query += " And FM.Id = " + foodMenuId;
                    }

                    Query += " group by CO.OrderType,FMC.FoodMenuCategoryName,FM.FoodMenuName " +
                        " WITH ROLLUP  " +
                        " Order By CO.OrderType,FMC.FoodMenuCategoryName,FM.FoodMenuName,Sum(COI.FoodMenuQty) asc ";
                }

                if (reportName == "SalesBySectionCategoryProductQtyDesc")
                {
                    Query = " select case when CO.OrderType = 1 then 'DineIN' When CO.OrderType = 2 then 'TakeAway' When CO.OrderType = 2 then 'Delivery' else 'ALL' End As SectionName, " +
                        " FMC.FoodMenuCategoryName,FM.FoodMenuName,Sum(COI.FoodMenuRate) As TotalUnitPrice,Sum(COI.FoodMenuQty) As TotalQty,Sum(COI.Price) As TotalPrice,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount, " +
                        "  cast(SUM(COI.GrossAmount) * 100.0 / SUM(SUM(COI.GrossAmount)) OVER ()  as numeric(18,2)) AS ValuePercentage " +
                        " from CustomerOrder CO " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId " +
                        " Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                     if (categoryId != 0)
                    {
                        Query += " And FMC.Id = " + categoryId;
                    }
                    if (foodMenuId != 0)
                    {
                        Query += " And FM.Id = " + foodMenuId;
                    }

                    Query += " group by CO.OrderType,FMC.FoodMenuCategoryName,FM.FoodMenuName " +
                        " WITH ROLLUP  " +
                        " Order By CO.OrderType,FMC.FoodMenuCategoryName,FM.FoodMenuName,Sum(COI.FoodMenuQty) desc ";
                }

                if (reportName == "SalesBySectionCategory")
                {
                    Query = " select case when CO.OrderType = 1 then 'DineIN' When CO.OrderType = 2 then 'TakeAway' When CO.OrderType = 2 then 'Delivery' else 'ALL' End As SectionName, " +
                        " FMC.FoodMenuCategoryName,Sum(COI.FoodMenuRate) As TotalUnitPrice,Sum(COI.FoodMenuQty) As TotalQty,Sum(COI.Price) As TotalPrice,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount, " +
                        "  cast(SUM(COI.GrossAmount) * 100.0 / SUM(SUM(COI.GrossAmount)) OVER ()  as numeric(18,2)) AS ValuePercentage " +
                        " from CustomerOrder CO " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId " +
                        " Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                     if (categoryId != 0)
                    {
                        Query += " And FMC.Id = " + categoryId;
                    }
                    if (foodMenuId != 0)
                    {
                        Query += " And FM.Id = " + foodMenuId;
                    }

                    Query += " group by CO.OrderType,FMC.FoodMenuCategoryName " +
                        " WITH ROLLUP  " +
                        " Order By CO.OrderType,FMC.FoodMenuCategoryName asc ";
                }

                if (reportName == "SalesBySectionProductAmountDesc")
                {
                    Query = " select case when CO.OrderType = 1 then 'DineIN' When CO.OrderType = 2 then 'TakeAway' When CO.OrderType = 2 then 'Delivery' else 'ALL' End As SectionName, " +
                        " FM.FoodMenuName,Sum(COI.FoodMenuRate) As TotalUnitPrice,Sum(COI.FoodMenuQty) As TotalQty,Sum(COI.Price) As TotalPrice,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount, " +
                        "  cast(SUM(COI.GrossAmount) * 100.0 / SUM(SUM(COI.GrossAmount)) OVER ()  as numeric(18,2)) AS ValuePercentage " +
                        " from CustomerOrder CO " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId " +
                        " Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                     if (categoryId != 0)
                    {
                        Query += " And FMC.Id = " + categoryId;
                    }
                    if (foodMenuId != 0)
                    {
                        Query += " And FM.Id = " + foodMenuId;
                    }

                    Query += " group by CO.OrderType,FM.FoodMenuName " +
                        " WITH ROLLUP  " +
                        " Order By CO.OrderType,FM.FoodMenuName,Sum(COI.Price) desc ";
                }

                if (reportName == "SalesBySectionProductQtyDesc")
                {
                    Query = " select case when CO.OrderType = 1 then 'DineIN' When CO.OrderType = 2 then 'TakeAway' When CO.OrderType = 2 then 'Delivery' else 'ALL' End As SectionName, " +
                        " FM.FoodMenuName,Sum(COI.FoodMenuRate) As TotalUnitPrice,Sum(COI.FoodMenuQty) As TotalQty,Sum(COI.Price) As TotalPrice,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount, " +
                        "  cast(SUM(COI.GrossAmount) * 100.0 / SUM(SUM(COI.GrossAmount)) OVER ()  as numeric(18,2)) AS ValuePercentage " +
                        " from CustomerOrder CO " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId " +
                        " Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                     if (categoryId != 0)
                    {
                        Query += " And FMC.Id = " + categoryId;
                    }
                    if (foodMenuId != 0)
                    {
                        Query += " And FM.Id = " + foodMenuId;
                    }

                    Query += " group by CO.OrderType,FM.FoodMenuName " +
                        " WITH ROLLUP  " +
                        " Order By CO.OrderType,FM.FoodMenuName,Sum(COI.FoodMenuQty) desc ";
                }

                salesByCategoryProductModel = db.Query<SalesByCategoryProductModel>(Query).ToList();
                return salesByCategoryProductModel;
            }
        }
        public List<TableStatisticsModel> GetTableStatisticsReport(string fromDate, string toDate, int outletId)
        {
            List<TableStatisticsModel> tableStatisticsModel = new List<TableStatisticsModel>();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string Query = string.Empty;

                Query = " select  T.TableName,T.PersonCapacity AS ActualCapacity, Sum(T.PersonCapacity) As ExpectedOccupancy,Sum(CO.AllocatedPerson) As Occupancy,((100 * Sum(CO.AllocatedPerson))/Sum(T.PersonCapacity)) As OccupancyPercentage " +
                        " from CustomerOrder CO   " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId   and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join [Tables] T On T.Id=CO.TableId " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) " +
                        " group by  T.TableName,T.PersonCapacity " +
                        " Order By T.TableName asc ";
                tableStatisticsModel = db.Query<TableStatisticsModel>(Query).ToList();
                return tableStatisticsModel;
            }
        }
        public List<SalesSummaryModel> GetSalesSummaryByFoodCategoryReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        {
            List<SalesSummaryModel> salesSummaryModel = new List<SalesSummaryModel>();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string Query = string.Empty;

                Query = " select FMC.FoodMenuCategoryName, Sum(COI.FoodMenuQty) As TotalQty,Sum(COI.VatableAmount) As NetSalesAmount,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount, " +
                        "  cast(SUM(COI.GrossAmount) * 100.0 / SUM(SUM(COI.GrossAmount)) OVER ()  as numeric(18,2)) AS ValuePercentage   " +
                        " from CustomerOrder CO   " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId   Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId   " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                 if (categoryId != 0)
                {
                    Query += " And FMC.Id = " + categoryId;
                }
                if (foodMenuId != 0)
                {
                    Query += " And FM.Id = " + foodMenuId;
                }

                Query += " group by FMC.FoodMenuCategoryName " +
                        " Order By FMC.FoodMenuCategoryName asc ";
                salesSummaryModel = db.Query<SalesSummaryModel>(Query).ToList();
                return salesSummaryModel;
            }
        }
        public List<SalesSummaryByFoodCategoryFoodMenuModel> GetSalesSummaryByFoodCategoryFoodMenuReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        {
            List<SalesSummaryByFoodCategoryFoodMenuModel> salesSummaryByFoodCategoryFoodMenuModel = new List<SalesSummaryByFoodCategoryFoodMenuModel>();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string Query = string.Empty;

                Query = " select FMC.FoodMenuCategoryName,Fm.FoodMenuName, Sum(COI.FoodMenuQty) As TotalQty,Sum(COI.VatableAmount) As NetSalesAmount,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount, " +
                        " SUM(COI.GrossAmount) * 100.0 / SUM(SUM(COI.GrossAmount)) OVER () AS ValuePercentage   " +
                        " from CustomerOrder CO   " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId   Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId   " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                 if (categoryId != 0)
                {
                    Query += " And FMC.Id = " + categoryId;
                }
                if (foodMenuId != 0)
                {
                    Query += " And FM.Id = " + foodMenuId;
                }

                Query += " group by FMC.FoodMenuCategoryName,Fm.FoodMenuName " +
                        " Order By FMC.FoodMenuCategoryName,Fm.FoodMenuName asc ";
                salesSummaryByFoodCategoryFoodMenuModel = db.Query<SalesSummaryByFoodCategoryFoodMenuModel>(Query).ToList();
                return salesSummaryByFoodCategoryFoodMenuModel;
            }
        }
        public List<SalesSummaryBySectionModel> GetSalesSummaryBySectionReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        {
            List<SalesSummaryBySectionModel> salesSummaryBySectionModel = new List<SalesSummaryBySectionModel>();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string Query = string.Empty;

                Query = " select case when CO.OrderType = 1 then 'DineIN' When CO.OrderType = 2 then 'TakeAway' When CO.OrderType = 2 then 'Delivery' else 'ALL' End As SectionName,  " +
                        " convert(varchar, CO.Orderdate, 103) as Orderdate , Count(CO.SalesInvoiceNumber) As TotalInvoice,Sum(COI.VatableAmount) As NetSalesAmount,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount   " +
                        " from CustomerOrder CO   " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId   Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId   " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                 if (categoryId != 0)
                {
                    Query += " And FMC.Id = " + categoryId;
                }
                if (foodMenuId != 0)
                {
                    Query += " And FM.Id = " + foodMenuId;
                }

                Query += " group by CO.OrderType,convert(varchar, CO.Orderdate, 103) " +
                        " Order By CO.OrderType,convert(varchar, CO.Orderdate, 103) ";
                salesSummaryBySectionModel = db.Query<SalesSummaryBySectionModel>(Query).ToList();
                return salesSummaryBySectionModel;
            }
        }
        public List<CustomerRewardModel> GetCustomerRewardReport(string fromDate, string toDate, string customerPhone, string customerName, int outletId)
        {
            List<CustomerRewardModel> customerRewardModel = new List<CustomerRewardModel>();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string Query = string.Empty;

                Query = " select CustomerName,CustomerPhone,Datetime,case when Credit = 0.00 then null else Credit end As Credit,case when Debit = 0.00 then null else Debit end As Debit, Balance " +
                        " from CustomerRedeem CR inner join customer C on C.Id = CR.CustomerId    " +
                        " Where CR.OutletId = " + outletId + " And  Convert(Date, CR.Datetime, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                if (!string.IsNullOrEmpty(customerPhone))
                {
                    Query += " And CustomerPhone like '%" + customerPhone + "%' ";
                }

                if (!string.IsNullOrEmpty(customerName))
                {
                    Query += " And CustomerName like '%" + customerName + "%' ";
                }
                Query += " Order By Datetime desc ";
                customerRewardModel = db.Query<CustomerRewardModel>(Query).ToList();
                return customerRewardModel;
            }
        }
        public List<SalesSummaryByWeek> GetSalesSummaryByWeekReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        {
            List<SalesSummaryByWeek> salesSummaryByWeek = new List<SalesSummaryByWeek>();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string Query = string.Empty;

                Query = " select  convert(varchar(10),DATEADD(DAY, -DATEDIFF(DAY, 0, Convert(Date, CO.Orderdate, 103)) % 7, Convert(Date, CO.Orderdate, 103)),103) AS [WeekStartDate], Count(CO.SalesInvoiceNumber) As TotalInvoice,Sum(COI.VatableAmount) As NetSalesAmount,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount  " +
                        " from CustomerOrder CO   " +
                        " Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId +
                        " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId   Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId   " +
                        " Where CO.OutletId = " + outletId + " And  Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) ";

                 if (categoryId != 0)
                {
                    Query += " And FMC.Id = " + categoryId;
                }
                if (foodMenuId != 0)
                {
                    Query += " And FM.Id = " + foodMenuId;
                }

                Query += " group by DATEADD(DAY, -DATEDIFF(DAY, 0, Convert(Date, CO.Orderdate, 103)) % 7, Convert(Date, CO.Orderdate, 103)) ";// +
                            // " Order By DATEADD(DAY, -DATEDIFF(DAY, 0, Convert(Date, CO.Orderdate, 103)) % 7, Convert(Date, CO.Orderdate, 103)) ";
                           // "order by CO.Orderdate ";
                salesSummaryByWeek = db.Query<SalesSummaryByWeek>(Query).ToList();



                return salesSummaryByWeek;
            }
        }
        public List<SalesSummaryByHours> GetSalesSummaryByHoursReport(string fromDate, string toDate, int outletId)
        {
            List<SalesSummaryByHours> salesSummaryByHours = new List<SalesSummaryByHours>();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string Query = string.Empty;

                Query = " DECLARE @count INT " +
                        " DECLARE @NumDays INT " +
                        " DECLARE @StartDate DATETIME " +
                        " DECLARE @EndDate DATETIME " +
                        " DECLARE @CurrentDay DATE " +
                        " DECLARE @tmp_Transactions TABLE  " +
                        " ( OrderDate varchar(15),StartHour time,EndHour time,TotalInvoice INT,NetSalesAmount numeric(18,2),TotalDiscount numeric(18,2),TotalTax numeric(18,2),TotalGrossAmount numeric(18,2) )   " +
                        " SET @StartDate =  convert(date,'" + fromDate + "',103)" +
                        " SET @EndDate =  convert(date,'" + toDate + "',103)" +
                " SET @count = 0 " +
                " SET @NumDays = DateDiff(Day, @StartDate, @EndDate) " +
                " WHILE @count <= @NumDays " +
                " BEGIN " +
                " SET @CurrentDay = DateAdd(Day, @count, @StartDate) " +
                " INSERT INTO @tmp_Transactions (OrderDate,StartHour,EndHour, TotalInvoice,NetSalesAmount,TotalDiscount,TotalTax,TotalGrossAmount) " +
                " SELECT  convert(varchar,@CurrentDay, 103),h.StartHour,h.EndHour,t.TotalInvoice,t.NetSalesAmount,t.TotalDiscount,t.TotalTax,t.TotalGrossAmount " +
                " FROM    tvfGetDay24Hours(@CurrentDay) AS h " +
                " OUTER APPLY ( SELECT Count(CO.SalesInvoiceNumber) As TotalInvoice,Sum(COI.VatableAmount) As NetSalesAmount,Sum(COI.Discount)  As TotalDiscount,Sum(COI.FoodMenuVat) As TotalTax,Sum(COI.GrossAmount) As TotalGrossAmount " +
                " from CustomerOrder CO  Inner Join CustomerOrderItem COI ON CO.MasterId =COI.CustomerOrderId and CO.OutletId = " + outletId + " and COI.OutletId = " + outletId + " Inner Join FoodMenu FM On FM.Id = COI.FoodMenuId    " +
                " Inner Join FoodMenuCategory FMC ON FMC.Id=FM.FoodCategoryId WHERE CO.OutletId = " + outletId + " And CO.OrderDate BETWEEN h.StartHour AND h.EndHour " +
                " ) AS t " +
                " ORDER BY h.StartHour " +
                " SET @count = @Count + 1 " +
                " END " +
                " SELECT * FROM @tmp_Transactions where TotalInvoice<>0";
                salesSummaryByHours = db.Query<SalesSummaryByHours>(Query).ToList();
                return salesSummaryByHours;
            }
        }

        public CessReportModel GetCessReport(string fromDate, string toDate, int outletId,string reporttype)
        {
            CessReportModel cessReport = new CessReportModel();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string cessSummaryQuery = string.Empty;
                string cessDetailQuery = string.Empty;

                if (reporttype == "cessSummary" || cessSummaryQuery == "All")
                {
                    cessSummaryQuery = " SELECT convert(varchar(10), CO.Orderdate,103) AS BillDate,SUM(isnull(CO.VatableAmount,0.00)+isnull(CO.NonVatableAmount,0.00)) AS NetSales, SUM(isnull(CO.VatableAmount,0.00)) AS Vatable,SUM(isnull(CO.NonVatableAmount,0.00)) AS NonVatable, SUM(isnull(CO.TaxAmount,0.00)) AS TotalTax, SUM(ISNULL(CO.GrossAmount,0.00)) AS GrandTotal " +
                                       " , convert(numeric(18,2),round(((SUM(isnull(CO.VatableAmount,0)+isnull(CO.NonVatableAmount,0)))*2)/100,2)) As CateringLevy " +
                                       " FROM BILL B " +
                                       " INNER JOIN CustomerOrder CO ON CO.MasterID = B.CustomerOrderId AND CO.OutletId = " + outletId + " and B.OutletId = " + outletId +
                                       " Where Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103)  And CO.OrderStatus = 4 AND CO.OutletId = " + outletId +
                                       " GROUP BY convert(varchar(10), CO.Orderdate,103) " +
                                       " ORDER BY convert(varchar(10), CO.Orderdate,103)";
                    cessReport.CessSummaryList = db.Query<CessSummaryModel>(cessSummaryQuery).ToList();
                }

                if (reporttype == "cessDetail" || cessSummaryQuery == "All")
                {
                    cessDetailQuery = " SELECT convert(varchar(10), CO.Orderdate, 103) AS BillDate, CO.SalesInvoiceNumber AS InvoiceNumber,(ISNULL(CO.VatableAmount, 0.00) + ISNULL(CO.NonVatableAmount, 0.00)) AS NetSales, ISNULL(CO.VatableAmount, 0.00) AS Vatable, ISNULL(CO.NonVatableAmount, 0.00) AS NonVatable, ISNULL(CO.TaxAmount, 0.00) AS TotalTax, ISNULL(CO.GrossAmount, 0.00) AS GrandTotal " +
                                   ", convert(numeric(18, 2), round(((isnull(CO.VatableAmount, 0.00) + isnull(CO.NonVatableAmount, 0.00)) * 2) / 100, 2)) As  CateringLevy" +
                                   "  FROM BILL B " +
                                   " INNER JOIN CustomerOrder CO ON CO.MasterID = B.CustomerOrderId AND CO.OutletId = " + outletId + "AND  B.OutletId = " + outletId +
                                   " Where Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103)  And CO.OrderStatus = 4 AND CO.OutletId = " + outletId +
                                   " ORDER BY BillDate";
                    cessReport.CessDetailList = db.Query<CessDetailModel>(cessDetailQuery).ToList();
                }

                return cessReport;
            }
        }

        public CessCategoryReportModel GetCessCategoryReport(string fromDate, string toDate, int categoryId, int foodMenuId,int outletId)
        {
            CessCategoryReportModel cessReport = new CessCategoryReportModel();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string cessSummaryQuery = string.Empty;
                string cessDetailQuery = string.Empty;

                cessSummaryQuery = " SELECT FoodmenucategoryName," +
                                    " SUM(isnull(COI.VatableAmount, 0.00) + isnull(COI.NonVatableAmount, 0.00)) AS NetSales, SUM(isnull(COI.VatableAmount, 0)) as Vatable, SUM(isnull(COI.NonVatableAmount, 0.00)) AS NonVatable," +
                                    " SUM(isnull(COI.FoodMenuVat, 0.00)) AS TotalTax, SUM(ISNULL(COI.Grossamount, 0.00)) AS GrandTotal," +
                                    " convert(numeric(18, 2), round(((SUM(isnull(COI.VatableAmount, 0) + isnull(COI.NonVatableAmount, 0))) * 2) / 100, 2)) As CateringLevy" +
                                    " FROM CustomerOrder CO" +
                                    " INNER join CustomerOrderItem coi on CO.MasterId = COI.CustomerOrderId AND CO.OutletId = " + outletId + " and COI.outletid=" + outletId +
                                    " inner join Foodmenu FM on FM.Id = COI.Foodmenuid" +
                                    " inner join Foodmenucategory FMC on FMC.ID = FM.FoodCAtegoryId" +
                                    " where CO.Orderstatus = 4 AND CO.OutletId = " + outletId;
                if (categoryId != 0)
                {
                    cessSummaryQuery += " And FMC.Id = " + categoryId;
                }
                if (foodMenuId != 0)
                {
                    cessSummaryQuery += " And FM.Id = " + foodMenuId;
                }

                cessSummaryQuery += " AND Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103)  and CO.Isdeleted = 0 AND CO.OutletId = " + outletId +
                                    " group by FoodmenucategoryName ORDER BY FoodmenucategoryName";


                cessDetailQuery = " SELECT convert(varchar(10), CO.Orderdate, 103) AS BillDate, CO.SalesInvoiceNumber AS InvoiceNumber,(ISNULL(CO.VatableAmount, 0.00) + ISNULL(CO.NonVatableAmount, 0.00)) AS NetSales, ISNULL(CO.VatableAmount, 0.00) AS Vatable, ISNULL(CO.NonVatableAmount, 0.00) AS NonVatable, ISNULL(CO.TaxAmount, 0.00) AS TotalTax, ISNULL(CO.GrossAmount, 0.00) AS GrandTotal " +
                                   ", convert(numeric(18, 2), round(((isnull(CO.VatableAmount, 0.00) + isnull(CO.NonVatableAmount, 0.00)) * 2) / 100, 2)) As  CateringLevy" +
                                   "  FROM BILL B " +
                                   " INNER JOIN CustomerOrder CO ON CO.MasterID = B.CustomerOrderId " + " AND CO.OutletId = " + outletId + " AND B.OutletId = " + outletId +
                                   " Where Convert(Date, CO.Orderdate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) And CO.OrderStatus = 4 AND CO.OutletId = " + outletId +
                                   " ORDER BY BillDate";


                cessReport.CessSummaryList = db.Query<CessCategorySummaryModel>(cessSummaryQuery).ToList();
                cessReport.CessDetailList = db.Query<CessDetailModel>(cessDetailQuery).ToList();

                return cessReport;
            }
        }
        public List<ModeofPaymentReportModel> GetModOfPaymentReport(string fromDate, string toDate,int outletId)
        {
            List<ModeofPaymentReportModel> modeofPaymentReportModel = new List<ModeofPaymentReportModel>();
            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string Query = string.Empty;

                Query = "(select convert(varchar(10), BD.BillDate,103) as BillDate,PaymentMethodName,sum(BillAmount) As BillAmount from Bill B " +
                                    " INNER JOIN BillDetail BD ON B.MasterID = BD.BillId and B.OutletId = " + outletId + "and BD.OutletId = " + outletId +
                                    " INNER join PaymentMethod PM ON BD.PaymentMethodId = PM.ID " +
                                    " Where B.IsDeleted = 0 AND " +
                                    " Convert(Date, BD.BillDate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) " + 
                                    " And B.BillStatus = 4 AND B.OutletId = " + outletId +
                                    " Group by convert(varchar(10), BD.BillDate,103),PaymentMethodName )" +
                                        "  union all" +
                                    " (SELECT convert(varchar(10), B.BillDateTime,103) as BillDate, ' SALES' AS PaymentMethodName, SUM(TotalAmount) as Sales " + 
                                    " from Bill B " +
                                    " Where B.IsDeleted = 0  and Convert(Date, B.BillDateTime , 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) " + 
                                    " And B.BillStatus = 4 AND B.OutletId = " + outletId +
                                    " group by convert(varchar(10), B.BillDateTime, 103))" +
                                    " Order by convert(varchar(10), BD.BillDate, 103)";

                modeofPaymentReportModel = db.Query<ModeofPaymentReportModel>(Query).ToList();

                return modeofPaymentReportModel;
            }
        }

        public List<TallySetupModel> GetTallySetup(int outletId)
        {
            List<TallySetupModel> tallySetupModel = new List<TallySetupModel>();

            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                tallySetupModel = db.Query<TallySetupModel>("select KeyName,LedgerName from tallysetup where outletId=" + outletId.ToString()).ToList();
            }
            return tallySetupModel;
        }
        public List<TallySalesVoucherModel> GetSalesVoucherData(string fromDate, string toDate, int outletId)
        {
            List<TallySalesVoucherModel> tallySalesVouchers = new List<TallySalesVoucherModel>();

            using (var db = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string query = "  Select convert(varchar(10), BD.BillDate,103) as BillDate,  sum(round(CO.VatableAmount, 0)) + Sum(round(CO.NonVatableAmount, 0)) + sum(round(CO.TaxAmount, 0)) as Sales,  " +
                                " PML.TallyLedgerName,PML.TallyLedgerNamePark,PML.TallyBillPostfix, " +
                                " sum(round(CO.VatableAmount, 0)) as CashSales, Sum(round(CO.NonVatableAmount, 0)) as ExemptedSales,sum(round(CO.TaxAmount, 0)) as OutputVAT " +
                                " from Billdetail BD " +
                                " INNER join PaymentMethodLedger PML on PML.PaymentMethodId = BD.PaymentMethodId and PML.OutletId = " + outletId +
                                " Inner join Bill B on B.MasterID = BD.BillId and B.OutletId = " + outletId + " and BD.OutletId = " + outletId + 
                                " inner join CustomerOrder CO on CO.MasterID = B.CustomerOrderId  AND CO.OutletId = " + outletId +
                                " Where Convert(Date, BD.BillDate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103) " +
                                " And B.BillStatus = 4 AND BD.OutletId = " + outletId +
                                " group by convert(varchar(10), BD.BillDate, 103),PML.TallyLedgerName,PML.TallyLedgerNamePark,PML.TallyBillPostfix ";

                tallySalesVouchers = db.Query<TallySalesVoucherModel>(query).ToList();
            }
            return tallySalesVouchers;
        }

    }
}
