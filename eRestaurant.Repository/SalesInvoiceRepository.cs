using Dapper;
using Microsoft.Extensions.Options;
using RocketPOS.Framework;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RocketPOS.Repository
{
    public class SalesInvoiceRepository : ISalesInvoiceRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public SalesInvoiceRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }
        //        SalesInvoice

        //SalesId
        //  CustomerId
        //  SalesInvoiceDate

        //       SalesInvoiceDetail

        //SalesInvoiceId
        // SOQty
        // InvoiceQty
        public int DeletePurchaseInvoice(long purchaseInvoiceId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update SalesInvoice set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where id = " + purchaseInvoiceId + ";" +
                    " Update SalesInvoiceDetail set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where SalesInvoiceId = " + purchaseInvoiceId + ";";
                result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);
                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                { sqltrans.Rollback(); }
            }
            return result;
        }
        public int DeletePurchaseInvoiceDetails(long purchaseDetailsId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update SalesInvoiceDetail set IsDeleted = 1 where id = " + purchaseDetailsId + ";";
                result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);
                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                { sqltrans.Rollback(); }
            }
            return result;
        }
        public List<SalesInvoiceModel> GetPurchaseInvoiceFoodMenuById(long purchaseInvoiceId)
        {
            List<SalesInvoiceModel> purchaseModelList = new List<SalesInvoiceModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select SalesInvoice.Id as Id, SalesInvoice.StoreId,SalesInvoice.EmployeeId,ReferenceNumber as ReferenceNo,SalesInvoice.SalesInvoiceDate ,Customer.CustomerName, Customer.Id as CustomerId," +
                      "SalesInvoice.GrossAmount, SalesInvoice.TaxAmount,SalesInvoice.TotalAmount, SalesInvoice.VatableAmount,SalesInvoice.NonVatableAmount , SalesInvoice.DueAmount as Due,SalesInvoice.PaidAmount as Paid,SalesInvoice.DeliveryNoteNumber ,SalesInvoice.DeliveryDate ,SalesInvoice.DriverName ,SalesInvoice.VehicleNumber,SalesInvoice.Notes " +
                      "from SalesInvoice inner join Customer on SalesInvoice.CustomerId = Customer.Id where SalesInvoice.InventoryType=1 And SalesInvoice.Isdeleted = 0 and SalesInvoice.Id = " + purchaseInvoiceId;
                purchaseModelList = con.Query<SalesInvoiceModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public List<SalesInvoiceViewModel> GetPurchaseInvoiceFoodMenuList()
        {
            List<SalesInvoiceViewModel> purchaseViewModelList = new List<SalesInvoiceViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select SalesInvoice.Id as Id, SalesInvoice.ReferenceNumber as ReferenceNo, convert(varchar(12),SalesInvoiceDate, 3) as [Date],Customer.CustomerName," +
                    "SalesInvoice.TotalAMount AS GrandTotal,SalesInvoice.DueAmount as Due,isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as Username, S.Storename  " +
                    ", SalesInvoice.VatableAmount,SalesInvoice.NonVatableAmount  from SalesInvoice inner join Customer on SalesInvoice.CustomerId = Customer.Id inner join [User] U on U.Id=SalesInvoice.UserIdInserted  inner join employee e on e.id = u.employeeid   inner join store S on S.Id = SalesInvoice.StoreId  where SalesInvoice.InventoryType=1 And SalesInvoice.Isdeleted = 0 order by SalesInvoiceDate, SalesId desc";
                purchaseViewModelList = con.Query<SalesInvoiceViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }
        public List<SalesInvoiceDetailModel> GetPurchaseInvoiceFoodMenuDetails(long purchaseInvoiceId)
        {
            List<SalesInvoiceDetailModel> purchaseDetails = new List<SalesInvoiceDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select pin.Id as SalesInvoiceId," +
             " (case when pin.FoodMenuId is null then 1 else 0 end) as ItemType, " +
             " (case when pin.FoodMenuId is null then pin.IngredientId else pin.FoodMenuId end) as FoodMenuId, " +
             " (case when pin.FoodMenuId is null then I.Ingredientname else f.FoodMenuName end) as FoodMenuName, " +
             " pin.UnitPrice as UnitPrice, pin.SOQty,PIN.InvoiceQty , pin.GrossAmount,pin.TaxAmount,pin.TotalAmount,pin.DiscountPercentage,pin.DiscountAmount " +
             " , pin.VatableAmount,pin.NonVatableAmount  from SalesInvoice as P inner join SalesInvoiceDetail as PIN on P.id = pin.SalesInvoiceId " +
             " left join FoodMenu as f on pin.FoodMenuId = f.Id " +
             " left join Ingredient as I on pin.IngredientId = I.Id " +
             " where P.id = " + purchaseInvoiceId + " and pin.isdeleted = 0 and p.isdeleted = 0";

                purchaseDetails = con.Query<SalesInvoiceDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }
        public List<SalesInvoiceViewModel> PurchaseInvoiceFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId)
        {
            List<SalesInvoiceViewModel> purchaseViewModels = new List<SalesInvoiceViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select SalesInvoice.Id as Id, SalesInvoice.ReferenceNumber as ReferenceNo, convert(varchar(12),SalesInvoiceDate, 3) as [Date],Customer.CustomerName," +
                    "SalesInvoice.TotalAMount AS GrandTotal,SalesInvoice.DueAmount as Due,isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as Username, S.Storename " +
                    "from SalesInvoice inner join Customer on SalesInvoice.CustomerId = Customer.Id inner join [User] U on U.Id=SalesInvoice.UserIdInserted inner join employee e on e.id = u.employeeid  inner join store S on S.Id = SalesInvoice.StoreId where SalesInvoice.InventoryType=1 And SalesInvoice.Isdeleted = 0 " +
                    " AND Convert(Date, SalesInvoiceDate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103)  ";

                if (customerId != 0)
                {
                    query += " And SalesInvoice.CustomerId= " + customerId;
                }
                if (storeId != 0)
                {
                    query += " And SalesInvoice.StoreId= " + storeId;
                }
                query += "   order by SalesInvoice.SalesInvoiceDate, SalesInvoice.id desc";

                purchaseViewModels = con.Query<SalesInvoiceViewModel>(query).AsList();
            }

            return purchaseViewModels;
        }
        public int InsertPurchaseInvoiceFoodMenu(SalesInvoiceModel purchaseModel)
        {
            int result = 0;
            int detailResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[SalesInvoice] " +
                             "  ([SalesId] " +
                             "  ,[ReferenceNumber] " +
                             "  ,[InventoryType]  " +
                             "  ,[CustomerId]     " +
                             "  ,[StoreId]        " +
                             "  ,[EmployeeId]        " +
                             "  ,[SalesInvoiceDate]   " +
                             "  ,[GrossAmount]    " +
                             "  ,[TaxAmount]      " +
                             "  ,[TotalAmount]     " +
                              "  ,[VatableAmount]      " +
                             "  ,[NonVatableAmount]      " +
                             " ,[DeliveryNoteNumber] " +
                             "  ,[DeliveryDate] " +
                             "  ,[DriverName]   " +
                             "  ,[VehicleNumber]  " +
                             "  ,[PaidAmount]     " +
                             "  ,[DueAmount]      " +
                             "  ,[Notes]          " +
                             "  ,[UserIdInserted]  " +
                             "  ,[DateInserted]   " +
                             "  ,[IsDeleted]      " +
                             "  ,[SalesDeliveryId] )" +
                             "   VALUES           ";

                if (purchaseModel.Type == "delivery")
                {
                    query = query + "  (NULL,  ";

                }
                else
                {
                    query = query + "  (@SalesId,  ";
                }

                query = query + " @ReferenceNo,  " +
                             "   @InventoryType,      " +
                             "   @CustomerId,      " +
                             "   @StoreId,         " +
                             "   @EmployeeId,         " +
                             "   @SalesInvoiceDate,    " +
                             "   @GrossAmount,     " +
                             "   @TaxAmount,     " +
                             "   @TotalAmount,     " +
                             "   @VatableAmount,      " +
                             "   @NonVatableAmount,      " +
                            "    @DeliveryNoteNumber," +
                             "   @DeliveryDate," +
                             "   @DriverName," +
                             "   @VehicleNumber," +
                             "   @PaidAmount,      " +
                             "   @DueAmount,       " +
                             "   @Notes," +
                             "" + LoginInfo.Userid + "," +
                             "   GetUtcDate(), 0,   ";

                if (purchaseModel.Type == "delivery")
                {
                    query = query + "  @SalesId);  ";

                }
                else
                {
                    query = query + "  NULL);  ";
                }

                query = query + " SELECT CAST(SCOPE_IDENTITY() as int); ";

                result = con.ExecuteScalar<int>(query, purchaseModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    foreach (var item in purchaseModel.SalesInvoiceDetails)
                    {
                        var queryDetails = "INSERT INTO [dbo].[SalesInvoiceDetail]" +
                                             "  ([SalesInvoiceId] " +
                                             " ,[FoodMenuId] " +
                                             " ,[IngredientId] " +
                                             " ,[AssetItemId] " +
                                             " ,[SOQty] " +
                                             " ,[InvoiceQty] " +
                                             " ,[UnitPrice] " +
                                             " ,[GrossAmount] " +
                                             " ,[DiscountPercentage]  " +
                                             " ,[DiscountAmount] " +
                                             " ,[TaxAmount] " +
                                            " ,[TotalAmount]  " +
                                            "  ,[VatableAmount]      " +
                                             "  ,[NonVatableAmount]      " +
                                             " ,[UserIdInserted]" +
                                              " ,[DateInserted]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES (          " + result + ",";
                        if (item.ItemType == 0)
                        {
                            queryDetails = queryDetails + "" + item.FoodMenuId + ",NUll,NUll,";

                        }
                        else if (item.ItemType == 1)
                        {
                            queryDetails = queryDetails + "NULL," + item.FoodMenuId + ",NUll,";

                        }
                        else if (item.ItemType == 2)
                        {
                            queryDetails = queryDetails + "NUll,NULL," + item.FoodMenuId + ",";

                        }
                        queryDetails = queryDetails + "" + item.SOQTY + "," +
                              item.InvoiceQty + "," +
                              item.UnitPrice + "," +
                              item.GrossAmount + "," +
                              item.DiscountPercentage + "," +
                              item.DiscountAmount + "," +
                              item.TaxAmount + "," +
                              item.TotalAmount + "," +
                              item.VatableAmount + "," +
                              item.NonVatableAmount + "," +
                    LoginInfo.Userid + ",GetUtcDate(),0); SELECT CAST(ReferenceNumber as INT) from SalesInvoice where id = " + result + "; ";

                        detailResult = con.ExecuteScalar<int>(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (detailResult > 0)
                    {
                        sqltrans.Commit();

                        int outResult = 0;
                        if (purchaseModel.SalesId > 0)
                            outResult = UpdatePurchaseOrderId(purchaseModel.SalesId);

                        outResult = UpdateDeliveryOrderId(purchaseModel.SalesId);

                        if (purchaseModel.SalesId == 0)
                        {
                            CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                            string sResult = commonRepository.InventoryPush("SalesInvoice", result);
                        }
                        else if (purchaseModel.SalesId > 0 && purchaseModel.SalesStatus != 4)
                        {
                            CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                            string sResult = commonRepository.InventoryPush("SalesInvoice", result);
                        }
                    }
                    else
                    {
                        sqltrans.Rollback();
                    }
                }
                else
                {
                    sqltrans.Rollback();
                }
            }

            return detailResult;
        }
        public int UpdatePurchaseInvoiceFoodMenu(SalesInvoiceModel purchaseModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update [dbo].[SalesInvoice] set " +
                             "   [CustomerId]  = @CustomerId" +
                             "  ,[StoreId]  = @StoreId" +
                             "  ,[EmployeeId]  = @EmployeeId" +
                             "  ,[GrossAmount]  =  @GrossAmount  " +
                             "  ,[TaxAmount]  = @TaxAmount   " +
                             "  ,[TotalAmount]  = @TotalAmount   " +
                             "  ,[VatableAmount] = @VatableAmount     " +
                              "  ,[NonVatableAmount] = @NonVatableAmount      " +
                              " ,[DeliveryNoteNumber] =@DeliveryNoteNumber " +
                              " ,[SalesInvoiceDate] =@SalesInvoiceDate " +
                             "  ,[DeliveryDate]=@DeliveryDate  " +
                             "  ,[DriverName] =@DriverName  " +
                             "  ,[VehicleNumber] =@VehicleNumber  " +
                             "  ,[PaidAmount] = @PaidAmount    " +
                             "  ,[DueAmount] =  @DueAmount    " +
                             "  ,[Notes] =  @Notes    " +
                             "  ,[UserIdUpdated] = " + LoginInfo.Userid + " " +
                             "  ,[DateUpdated]  = GetUtcDate()  where id= " + purchaseModel.Id + ";";
                result = con.Execute(query, purchaseModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    if (purchaseModel.DeletedId != null)
                    {
                        foreach (var item in purchaseModel.DeletedId)
                        {
                            var deleteQuery = $"update SalesInvoiceDetail set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in purchaseModel.SalesInvoiceDetails)
                    {
                        var queryDetails = string.Empty;
                        if (item.SalesInvoiceId > 0)
                        {
                            queryDetails = "Update [dbo].[SalesInvoiceDetail] set " +
                                             "[SalesInvoiceId] = " + purchaseModel.Id;
                            if (item.ItemType == 0)
                            {
                                queryDetails = queryDetails + " ,[FoodMenuId]  = " + item.FoodMenuId + ",[IngredientId] = null,[AssetItemId] = null, ";
                            }
                            else if (item.ItemType == 1)
                            {
                                queryDetails = queryDetails + " [IngredientId]  = " + item.FoodMenuId + ",[FoodMenuId] = null,[AssetItemId] = null, ";

                            }
                            else if (item.ItemType == 2)
                            {
                                queryDetails = queryDetails + " [AssetItemId]  = " + item.FoodMenuId + ",[FoodMenuId] = null,[IngredientId] = null, ";

                            }
                            queryDetails = queryDetails + " [UnitPrice]   = " + item.UnitPrice + "," +
                                              "[SOQty]  = " + item.SOQTY +
                                              ",[InvoiceQty]   = " + item.InvoiceQty +
                                              ",[GrossAmount]   = " + item.GrossAmount +
                                              ",[DiscountPercentage] = " + item.DiscountPercentage +
                                              ",[DiscountAmount]  = " + item.DiscountAmount +
                                              ",[TaxAmount]   = " + item.TaxAmount +
                                              ",[TotalAmount]  = " + item.TotalAmount +
                                             " ,[VatableAmount] = " + item.VatableAmount + "," +
                                             " [NonVatableAmount] = " + item.NonVatableAmount + "," +
                                               " [UserIdUpdated] = " + LoginInfo.Userid + "," +
                                                 " [DateUpdated] = GetUTCDate() " +
                                                 " where id = " + item.SalesInvoiceId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[SalesInvoiceDetail]" +
                                              "  ([SalesInvoiceId] " +
                                              " ,[FoodMenuId] " +
                                              " ,[IngredientId] " +
                                               " ,[AssetItemId] " +
                                              " ,[SOQty] " +
                                              " ,[InvoiceQty] " +
                                              " ,[UnitPrice] " +
                                              " ,[GrossAmount] " +
                                              " ,[DiscountPercentage]  " +
                                              " ,[DiscountAmount] " +
                                              " ,[TaxAmount] " +
                                              " ,[TotalAmount]  " +
                                            "  ,[VatableAmount]      " +
                                             "  ,[NonVatableAmount]      " +
                                              " ,[UserIdInserted]" +
                                               " ,[DateInserted]" +
                                               " ,[IsDeleted])   " +
                                               "VALUES           " +
                                               "(" + purchaseModel.Id + ",";
                            if (item.ItemType == 0)
                            {
                                queryDetails = queryDetails + "" + item.FoodMenuId + ",NUll,NUll,";

                            }
                            else if (item.ItemType == 1)
                            {
                                queryDetails = queryDetails + "NULL," + item.FoodMenuId + ",NUll,";

                            }
                            else if (item.ItemType == 2)
                            {
                                queryDetails = queryDetails + "NUll,NULL," + item.FoodMenuId + ",";

                            }
                            queryDetails = queryDetails + item.SOQTY + "," +
                                               item.InvoiceQty + "," +
                                               item.UnitPrice + "," +
                                               item.GrossAmount + "," +
                                               item.DiscountPercentage + "," +
                                               item.DiscountAmount + "," +
                                               item.TaxAmount + "," +
                                               item.TotalAmount + "," +
                                               "" + item.VatableAmount + "," +
                                                "" + item.NonVatableAmount + "," +
                                     LoginInfo.Userid + ",GetUTCDate(),0); SELECT SCOPE_IDENTITY() ";
                        }
                        detailResult = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (detailResult > 0)
                    {
                        sqltrans.Commit();
                    }
                    else
                    {
                        sqltrans.Rollback();
                    }
                }
                else
                {
                    sqltrans.Rollback();
                }
            }

            return result;
        }
        public string ReferenceNumberFoodMenu()
        {
            string result = string.Empty;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                var query = $"SELECT RIGHT('00' + convert(varchar(8), isnull(count(*), 0) + 1), 12) FROM SalesInvoice where InventoryType = 1 and isdeleted = 0; ";
                result = con.ExecuteScalar<string>(query, null, sqltrans, 0, System.Data.CommandType.Text);
                if (!string.IsNullOrEmpty(result))
                {
                    sqltrans.Commit();
                }
                else
                { sqltrans.Rollback(); }
            }
            return result;
        }
        public decimal GetTaxByFoodMenuId(int foodMenuId)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "select  ISNULL(TaxPercentage,0) AS TaxPercentage from foodmenuRate fmr " +
                            "Inner join tax t on t.Id = fmr.FoodVatTaxId " +
                            "where fmr.FoodMenuId = " + foodMenuId + " And fmr.OutletId = 1"; // Need To Change OutLet Id Dynamic value 
                return con.ExecuteScalar<decimal>(query, null, sqltrans, 0, System.Data.CommandType.Text);
            }
        }
        public List<SalesInvoiceModel> GetPurchaseInvoiceFoodMenuByPurchaseId(long purchaseId, string type)
        {
            List<SalesInvoiceModel> purchaseModelList = new List<SalesInvoiceModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "";
                if (type == "delivery")
                {
                    query = " SELECT 0 as Id,P.Id AS SalesId,P.StoreId,P.EmployeeId,P.ReferenceNumber,P.SalesDeliveryDate AS SalesInvoiceDate,Customer.CustomerName, Customer.Id as CustomerId,P.GrossAmount,P.TaxAmount,P.TotalAmount As TotalAmount, " +
                            " P.DueAmount as Due,P.PaidAmount as Paid,null AS DeliveryNoteNumber,GETDATE() as DeliveryDate,null as DriverName,null as VehicleNumber,P.Notes,P.Status as SalesStatus, p.VatableAmount,p.NonVatableAmount  " +
                            " FROM SalesDelivery P inner join Customer on P.CustomerId = Customer.Id " +
                            " Where P.InventoryType = 1 And P.Isdeleted = 0 And P.Id = " + purchaseId;

                }
                else
                {
                    query = " SELECT 0 as Id,P.Id AS SalesId,P.StoreId,P.EmployeeId,P.ReferenceNo,P.SalesDate AS SalesInvoiceDate,Customer.CustomerName, Customer.Id as CustomerId,P.GrossAmount,P.TaxAmount,P.GrandTotal As TotalAmount, " +
                               " P.DueAmount as Due,P.PaidAmount as Paid,null AS DeliveryNoteNumber,GETDATE() as DeliveryDate,null as DriverName,null as VehicleNumber,P.Notes,P.Status as SalesStatus, p.VatableAmount,p.NonVatableAmount  " +
                               " FROM Sales P inner join Customer on P.CustomerId = Customer.Id " +
                               " Where P.InventoryType = 1 And P.Isdeleted = 0 And P.Id = " + purchaseId;
                }
                purchaseModelList = con.Query<SalesInvoiceModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public List<SalesInvoiceDetailModel> GetPurchaseInvoiceFoodMenuDetailsPurchaseId(long purchaseId, string type)
        {
            List<SalesInvoiceDetailModel> purchaseDetails = new List<SalesInvoiceDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "";
                if (type == "delivery")
                {
                    query = " Select 0 AS SalesInvoiceId," +
                           " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then 2 else 1 end) else 0 end) as ItemType, " +
                           " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then PD.AssetItemId else PD.IngredientId end) else PD.FoodMenuId end) as FoodMenuId, " +
                           " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then AI.AssetItemName else I.IngredientName end) else f.FoodMenuName end) as FoodMenuName,  " +
                           " pd.UnitPrice,PD.SOQty AS SOQty,PD.DeliveryQty AS InvoiceQty,PD.GrossAmount,PD.TaxAmount,PD.TotalAmount,PD.DiscountPercentage,PD.DiscountAmount " +
                           " , pd.VatableAmount,pd.NonVatableAmount " +
                           " From SalesDelivery P Inner join SalesDeliveryDetail PD On P.Id = PD.SalesDeliveryId " +
                           " left join FoodMenu as f on PD.FoodMenuId = f.Id " +
                           " left join Ingredient as I on PD.IngredientId = I.Id " +
                           " left join AssetItem as AI on PD.AssetItemId = AI.Id  " +
                           " Where P.isdeleted = 0 and pd.isdeleted = 0 And P.Id = " + purchaseId;
                }
                else
                {
                    query = " Select 0 AS SalesInvoiceId," +
                           " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then 2 else 1 end) else 0 end) as ItemType, " +
                           " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then PD.AssetItemId else PD.IngredientId end) else PD.FoodMenuId end) as FoodMenuId, " +
                           " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then AI.AssetItemName else I.IngredientName end) else f.FoodMenuName end) as FoodMenuName,  " +
                           " pd.UnitPrice,PD.Qty AS SOQty,PD.Qty AS InvoiceQty,PD.GrossAmount,PD.TaxAmount,PD.TotalAmount,PD.DiscountPercentage,PD.DiscountAmount " +
                           " , pd.VatableAmount,pd.NonVatableAmount " +
                           " From Sales P Inner join SalesDetail PD On P.Id = PD.SalesId " +
                           " left join FoodMenu as f on PD.FoodMenuId = f.Id " +
                           " left join Ingredient as I on PD.IngredientId = I.Id " +
                           " left join AssetItem as AI on PD.AssetItemId = AI.Id  " +
                           " Where P.isdeleted = 0 and pd.isdeleted = 0 And P.Id = " + purchaseId;
                }
                purchaseDetails = con.Query<SalesInvoiceDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }
        public int GetPurchaseIdByPOReference(string poReference)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select Id from Sales Where IsDeleted=0 and Status!=5 and ReferenceNo='" + poReference + "'";
                return con.QueryFirstOrDefault<int>(query);
            }
        }
        public int UpdatePurchaseOrderId(long purchaseId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update Sales set Status = 5 where id = " + purchaseId;
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

        public int UpdateDeliveryOrderId(long purchaseId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update SalesDelivery set Status = 1 where id = " + purchaseId;
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

        public List<SalesInvoiceModel> GetPurchaseInvoiceById(long PurchaseInvoiceId)
        {
            List<SalesInvoiceModel> purchaseModelList = new List<SalesInvoiceModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select SalesInvoice.Id as Id, SalesInvoice.StoreId,SalesInvoice.EmployeeId,PurchaseId as ReferenceNo,SalesInvoice.SalesInvoiceDate as [Date],Customer.SupplierName, Customer.Id as CustomerId," +
                      " SalesInvoice.[GrossAmount] as GrandTotal,SalesInvoice.DueAmount as Due,SalesInvoice.PaidAmount as Paid,SalesInvoice.DeliveryNoteNumber,SalesInvoice.DeliveryDate,SalesInvoice.DriverName,SalesInvoice.VehicleNumber,SalesInvoice.Notes " +
                      " ,[DeliveryNoteNumber], [DeliveryDate],[DriverName],[VehicleNumber], SalesInvoice.VatableAmount,SalesInvoice.NonVatableAmount  " +
                      " from SalesInvoice inner join customer on SalesInvoice.customerId = customer.Id where SalesInvoice.InventoryType=2 And PurchaseInvoice.Isdeleted = 0 and SalesInvoice.Id = " + PurchaseInvoiceId;
                purchaseModelList = con.Query<SalesInvoiceModel>(query).AsList();
            }
            return purchaseModelList;
        }

        public List<SalesInvoiceModel> GetSalesInvoiceReportById(long id)
        {
            List<SalesInvoiceModel> purchaseModelList = new List<SalesInvoiceModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select SI.ReferenceNumber AS ReferenceNo,s.StoreName,si.SalesInvoiceDate,c.CustomerName,c.CustomerAddress1,c.CustomerAddress2,c.CustomerNumber,c.CustomerEmail " +
                            " ,si.Notes,si.GrossAmount ,si.TaxAmount,si.TotalAmount,si.NonVatableAmount,si.VatableAmount,si.DeliveryNoteNumber,si.DeliveryDate,si.DriverName,si.VehicleNumber " +
                            " ,O.OutletAddress1,O.OutletAddress2,O.OutletPhone,O.OutletEmail,O.InvoiceHeader,O.InvoiceFooter, C.TaxInclusive as CustomerTaxInclusive " +
                            " from SalesInvoice SI Inner Join Store S On s.Id=si.StoreId inner join Outlet O on O.Storeid = S.Id " +
                            " inner join Customer c on c.Id=si.CustomerId where si.Id= " + id;
                purchaseModelList = con.Query<SalesInvoiceModel>(query).AsList();
            }
            return purchaseModelList;
        }

        public List<SalesInvoiceDetailModel> GetSalesInvoiceReportFoodMenuDetails(long id)
        {
            List<SalesInvoiceDetailModel> purchaseDetails = new List<SalesInvoiceDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select ROW_NUMBER() over ( Order by PIN.Id) as SrNumber,pin.Id as SalesInvoiceId,  (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then 2 else 1 end) else 0 end) as ItemType,  " +
                     " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then PIN.AssetItemId else pin.IngredientId end) else pin.FoodMenuId end) as FoodMenuId,   " +
                     " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then AI.AssetItemName else I.IngredientName end) else f.FoodMenuName end) as FoodMenuName,   " +
                     " pin.UnitPrice as UnitPrice, pin.SOQty,PIN.InvoiceQty , pin.GrossAmount,pin.TaxAmount,pin.TotalAmount, pin.DiscountPercentage,pin.DiscountAmount  , pin.VatableAmount,pin.NonVatableAmount ," +
                     " (case when PIN.FoodMenuId is null then (case when PIN.IngredientId is null then UA.UnitName else UI.UnitName end) else UF.UnitName end) as UnitName" +
                     " from SalesInvoice as P  inner join SalesInvoiceDetail as PIN on P.id = pin.SalesInvoiceId  " +
                     " left join FoodMenu as f on pin.FoodMenuId = f.Id   left join Ingredient as I on pin.IngredientId = I.Id  left join Units As UI On UI.Id = I.IngredientUnitId  " +
                     " left join AssetItem as AI on PIN.AssetItemId = AI.Id    left join Units As UF On UF.Id = F.UnitsId     left join Units As UA On UA.Id = AI.UnitId  " +
                     " where P.id = " + id + " and pin.isdeleted = 0 and p.isdeleted = 0 ";

                purchaseDetails = con.Query<SalesInvoiceDetailModel>(query).AsList();
            }
            return purchaseDetails;
        }

        public List<SalesInvoiceModel> GetViewSalesInvoiceFoodMenuById(long purchaseInvoiceId)
        {
            List<SalesInvoiceModel> purchaseModelList = new List<SalesInvoiceModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select P.Referenceno as SOReferenceNo, P.SalesDate as SODate, SalesInvoice.Id as Id, SalesInvoice.StoreId,SalesInvoice.EmployeeId,ReferenceNumber as ReferenceNo,SalesInvoiceDate ,Customer.CustomerName, Customer.Id as CustomerId," +
                      "SalesInvoice.GrossAmount, SalesInvoice.TaxAmount,SalesInvoice.TotalAmount,SalesInvoice.DueAmount as Due,SalesInvoice.PaidAmount as Paid,SalesInvoice.DeliveryNoteNumber ,SalesInvoice.DeliveryDate ,SalesInvoice.DriverName ,SalesInvoice.VehicleNumber,SalesInvoice.Notes,S.StoreName " +
                      ", SalesInvoice.VatableAmount,SalesInvoice.NonVatableAmount  from SalesInvoice left JOIN Sales P on P.Id = SalesInvoice.salesid inner join Customer on SalesInvoice.customerId = customer.Id Inner Join  Store S On S.Id=SalesInvoice.StoreId where SalesInvoice.InventoryType=1 And SalesInvoice.Isdeleted = 0 and SalesInvoice.Id = " + purchaseInvoiceId;
                purchaseModelList = con.Query<SalesInvoiceModel>(query).AsList();
            }
            return purchaseModelList;
        }

        public List<SalesInvoiceDetailModel> GetViewSalesInvoiceFoodMenuDetails(long purchaseInvoiceId)
        {
            List<SalesInvoiceDetailModel> purchaseDetails = new List<SalesInvoiceDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select pin.Id as SalesInvoiceId," +
                            " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then 2 else 1 end) else 0 end) as ItemType, " +
                            " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then PIN.AssetItemId else pin.IngredientId end) else pin.FoodMenuId end) as FoodMenuId, " +
                            " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then AI.AssetItemName else I.IngredientName end) else f.FoodMenuName end) as FoodMenuName,  " +
                            " pin.UnitPrice as UnitPrice, pin.SOQty,PIN.InvoiceQty , pin.GrossAmount,pin.TaxAmount,pin.TotalAmount,pin.DiscountPercentage,pin.DiscountAmount, " +
                            "  (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then UA.UnitName else UI.UnitName end) else UF.UnitName end) as UnitName    " +
                            " , pin.VatableAmount,pin.NonVatableAmount from salesInvoice as P inner join SalesInvoiceDetail as PIN on P.id = pin.SalesInvoiceId " +
                            " left join FoodMenu as f on pin.FoodMenuId = f.Id " +
                            " left join Ingredient as I on pin.IngredientId = I.Id " +
                            "   left join AssetItem as AI on PIN.AssetItemId = AI.Id  " +
                            " left join Units As UI On UI.Id = I.IngredientUnitId " +
                            " left join Units As UF On UF.Id = F.UnitsId " +
                            "   left join Units As UA On UA.Id = AI.UnitId " +
                            " where P.id = " + purchaseInvoiceId + " and pin.isdeleted = 0 and p.isdeleted = 0";

                purchaseDetails = con.Query<SalesInvoiceDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }
    }
}
