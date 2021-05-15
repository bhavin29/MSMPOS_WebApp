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
    public class SalesDeliveryRepository : ISalesDeliveryRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public SalesDeliveryRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }
        public int DeletePurchaseGRN(long purchaseGRNId)//
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update SalesDelivery set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where id = " + purchaseGRNId + ";" +
                    " Update SalesDeliveryDetail set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where SalesDeliveryId = " + purchaseGRNId + ";";
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
        public int DeletePurchaseGRNDetails(long purchaseDetailsId)//
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update SalesDeliveryDetail set IsDeleted = 1 where id = " + purchaseDetailsId + ";";
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
        public List<SalesDeliveryModel> GetPurchaseGRNFoodMenuById(long purchaseGRNId)//
        {
            List<SalesDeliveryModel> purchaseModelList = new List<SalesDeliveryModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select SalesDelivery.Id as Id, SalesDelivery.StoreId,SalesDelivery.EmployeeId,ReferenceNumber as ReferenceNo,SalesDelivery.SalesDeliveryDate ,customer.customerName, customer.Id as CustomerId," +
                      "SalesDelivery.GrossAmount, SalesDelivery.TaxAmount,SalesDelivery.TotalAmount,SalesDelivery.VatableAmount,SalesDelivery.NonVatableAmount ,SalesDelivery.DueAmount as Due,SalesDelivery.PaidAmount as Paid,SalesDelivery.DeliveryNoteNumber ,SalesDelivery.DeliveryDate ,SalesDelivery.DriverName ,SalesDelivery.VehicleNumber,SalesDelivery.Notes " +
                      "from SalesDelivery inner join customer on SalesDelivery.customerId = customer.Id where SalesDelivery.InventoryType=1 And SalesDelivery.Isdeleted = 0 and SalesDelivery.Id = " + purchaseGRNId;
                purchaseModelList = con.Query<SalesDeliveryModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public List<SalesDeliveryViewModel> GetPurchaseGRNFoodMenuList()//
        {
            List<SalesDeliveryViewModel> purchaseViewModelList = new List<SalesDeliveryViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select SalesDelivery.Id as Id, SalesDelivery.ReferenceNumber as ReferenceNo, convert(varchar(12),SalesDeliveryDate, 3) as [Date],customer.customerName,S.Storename," +
                    "SalesDelivery.TotalAMount,SalesDelivery.DueAmount as Due ,isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as Username  " +
                    "from SalesDelivery inner join customer on SalesDelivery.customerId  = customer.Id inner join [User] U on U.Id=SalesDelivery.UserIdInserted  inner join employee e on e.id = u.employeeid  inner join store S on S.Id = SalesDelivery.StoreId  where SalesDelivery.InventoryType=1 And SalesDelivery.Isdeleted = 0 order by SalesDeliveryDate, SalesId desc";
                purchaseViewModelList = con.Query<SalesDeliveryViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }
        public List<SalesDeliveryDetailModel> GetPurchaseGRNFoodMenuDetails(long purchaseGRNId)//
        {
            List<SalesDeliveryDetailModel> purchaseDetails = new List<SalesDeliveryDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select pin.Id as SalesDeliveryId," +
                            " (case when pin.FoodMenuId is null then 1 else 0 end) as ItemType, " +
                            " (case when pin.FoodMenuId is null then pin.IngredientId else pin.FoodMenuId end) as FoodMenuId, " +
                            " (case when pin.FoodMenuId is null then I.Ingredientname else f.FoodMenuName end) as FoodMenuName, " +
                            " pin.UnitPrice as UnitPrice, pin.SOQty,PIN.DeliveryQty , pin.GrossAmount,pin.TaxAmount,pin.TotalAmount, pin.VatableAmount,pin.NonVatableAmount , pin.DiscountPercentage,pin.DiscountAmount " +
                            " from SalesDelivery as P inner join SalesDeliveryDetail as PIN on P.id = pin.SalesDeliveryId " +
                            " left join FoodMenu as f on pin.FoodMenuId = f.Id " +
                            " left join Ingredient as I on pin.IngredientId = I.Id " +
                            " where P.id = " + purchaseGRNId + " and pin.isdeleted = 0 and p.isdeleted = 0";
                purchaseDetails = con.Query<SalesDeliveryDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }
        public List<SalesDeliveryViewModel> PurchaseGRNFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId)//
        {
            List<SalesDeliveryViewModel> purchaseViewModels = new List<SalesDeliveryViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select SalesDelivery.Id as Id,SalesDelivery.SalesId, SalesDelivery.ReferenceNumber as ReferenceNo, convert(varchar(12),SalesDeliveryDate, 3) as [Date],Customer.CustomerName,S.Storename," +
                    " SalesDelivery.TotalAMount,SalesDelivery.DueAmount as Due ,isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as Username,  " +
                    " (case when salesdelivery.status=0 then 'Invoice' else '' end ) as Status " +
                    " from SalesDelivery inner join Customer on SalesDelivery.CustomerId = Customer.Id inner join [User] U on U.Id=SalesDelivery.UserIdInserted  inner join employee e on e.id = u.employeeid inner join store S on S.Id = SalesDelivery.StoreId  where SalesDelivery.InventoryType=1 And SalesDelivery.Isdeleted = 0  " +
                    " AND Convert(Date, SalesDeliveryDate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103)  ";

                if (customerId != 0)
                {
                    query += " And SalesDelivery.customerId= " + customerId;
                }

                if (storeId != 0)
                {
                    query += " And SalesDelivery.StoreId= " + storeId;
                }
                query += "  order by SalesDelivery.SalesDeliveryDate, SalesDelivery.id desc";

                purchaseViewModels = con.Query<SalesDeliveryViewModel>(query).AsList();
            }

            return purchaseViewModels;
        }
        public int InsertPurchaseGRNFoodMenu(SalesDeliveryModel purchaseModel)//
        {
            bool taxInclusive = GetCustomerTaxExampt((int)purchaseModel.CustomerId);

            int result = 0;
            int detailResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[SalesDelivery] " +
                             "  ([SalesId] " +
                             "  ,[ReferenceNumber] " +
                             "  ,[InventoryType]  " +
                             "  ,[CustomerId]     " +
                             "  ,[StoreId]        " +
                             "  ,[EmployeeId]        " +
                             "  ,[SalesDeliveryDate]   " +
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
                             "  ,[IsDeleted] )     " +
                             "   VALUES           " +
                             "  (@SalesId,  " +
                             "  @ReferenceNo,  " +
                             "   @InventoryType,      " +
                             "   @CustomerId,      " +
                             "   @StoreId,         " +
                             "   @EmployeeId,         " +
                             "   @SalesDeliveryDate,    ";

                if (taxInclusive == true)
                {
                    query = query + "   @GrossAmount,     " +
                                  "   @TaxAmount,     " +
                                  "   @TotalAmount,     " +
                                  "   @VatableAmount,      " +
                                  "   @NonVatableAmount,      ";

                }
                else
                {
                    query = query + "  @TotalAmount,     " +
                                  "   0,     " +
                                  "   @TotalAmount,     " +
                                  "   0,      " +
                                  "   0,      ";

                }

                query = query + "  @DeliveryNoteNumber," +
                             "  @DeliveryDate," +
                             "  @DriverName," +
                             " @VehicleNumber," +
                             "   @PaidAmount,      " +
                             "   @DueAmount,       " +
                             "   @Notes," +
                             "" + LoginInfo.Userid + "," +
                             "   GetUtcDate(),    " +
                             "   0); SELECT CAST(SCOPE_IDENTITY() as int); ";
                result = con.ExecuteScalar<int>(query, purchaseModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    foreach (var item in purchaseModel.salesDeliveryDetails)
                    {
                        var queryDetails = "INSERT INTO [dbo].[SalesDeliveryDetail]" +
                                             "  ([SalesDeliveryId] " +
                                             " ,[FoodMenuId] " +
                                             " ,[IngredientId] " +
                                             " ,[AssetItemId] " +
                                             " ,[SOQty] " +
                                             " ,[DeliveryQty] " +
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
                                      item.DeliveryQTY + "," +
                                      item.UnitPrice + ",";

                        if (taxInclusive == true)
                        {
                            queryDetails = queryDetails +
                                          item.GrossAmount + "," +
                                          item.DiscountPercentage + "," +
                                          item.DiscountAmount + "," +
                                          item.TaxAmount + "," +
                                          item.TotalAmount + "," +
                                          item.VatableAmount + "," +
                                          item.NonVatableAmount + ",";
                        }
                        else
                        {
                            queryDetails = queryDetails +
                                           item.TotalAmount + "," +
                                           item.DiscountPercentage + "," +
                                           item.DiscountAmount + "," +
                                            "0," +
                                           item.TotalAmount + "," +
                                            "0," +
                                            "0,";
                        }

                        queryDetails = queryDetails + LoginInfo.Userid + ",GetUTCDate(),0); SELECT CAST(ReferenceNumber as INT) from SalesDelivery where id = " + result + "; ";

                        detailResult = con.ExecuteScalar<int>(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (detailResult > 0)
                    {
                        sqltrans.Commit();

                        int outResult = 0;
                        if (purchaseModel.SalesId > 0)
                            outResult = UpdatePurchaseOrderId(purchaseModel.SalesId);

                        CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                        string sResult = commonRepository.InventoryPush("SalesDelivery", result);

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
        public int UpdatePurchaseGRNFoodMenu(SalesDeliveryModel purchaseModel)//
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update [dbo].[SalesDelivery] set " +
                             "   [CustomerId]  = @CustomerId" +
                             "  ,[StoreId]  = @StoreId" +
                             "  ,[EmployeeId]  = @EmployeeId" +
                             "  ,[SalesDeliveryDate] = @SalesDeliveryDate  " +
                             "  ,[GrossAmount]  =  @GrossAmount  " +
                             "  ,[TaxAmount]  = @TaxAmount   " +
                             "  ,[TotalAmount]  = @TotalAmount   " +
                             "  ,[VatableAmount] = @VatableAmount     " +
                              "  ,[NonVatableAmount] = @NonVatableAmount      " +
                             " ,[DeliveryNoteNumber] =@DeliveryNoteNumber " +
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
                            var deleteQuery = $"update SalesDeliveryDetail set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in purchaseModel.salesDeliveryDetails)
                    {
                        var queryDetails = string.Empty;
                        if (item.SalesDeliveryId > 0)
                        {
                            queryDetails = "Update [dbo].[SalesDeliveryDetail] set " +
                                             "[SalesDeliveryId] = " + purchaseModel.Id;
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
                            queryDetails = queryDetails + " [UnitPrice]   = " + item.UnitPrice +
                              ",[SOQTY]  = " + item.SOQTY +
                              ",[DeliveryQTY]   = " + item.DeliveryQTY +
                              ",[GrossAmount]   = " + item.GrossAmount +
                              ",[DiscountPercentage] = " + item.DiscountPercentage +
                              ",[DiscountAmount]  = " + item.DiscountAmount +
                              ",[TaxAmount]   = " + item.TaxAmount +
                              ",[TotalAmount]  = " + item.TotalAmount +
                             " ,[VatableAmount] = " + item.VatableAmount +
                             " ,[NonVatableAmount] = " + item.NonVatableAmount +
                               " ,[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                 " [DateUpdated] = GetUTCDate() " +
                                 " where id = " + item.SalesDeliveryId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[SalesDeliveryDetail]" +
                                              "  ([SalesDeliveryId] " +
                                              " ,[FoodMenuId] " +
                                              " ,[IngredientId] " +
                                              " ,[AssetItemId] " +
                                              " ,[SOQty] " +
                                              " ,[DeliveryQTY] " +
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
                                               item.DeliveryQTY + "," +
                                               item.UnitPrice + "," +
                                               item.GrossAmount + "," +
                                               item.DiscountPercentage + "," +
                                               item.DiscountAmount + "," +
                                               item.TaxAmount + "," +
                                               item.TotalAmount + "," +
                                               item.VatableAmount + "," +
                                               item.NonVatableAmount + "," +
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
        public string ReferenceNumberFoodMenu()//
        {
            string result = string.Empty;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"SELECT ISNULL(MAX(convert(int,ReferenceNumber)),0) + 1 FROM SalesDelivery where InventoryType=1 and ISDeleted=0;";
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
        public decimal GetTaxByFoodMenuId(int foodMenuId)//
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
        public decimal GetFoodMenuLastPrice(int itemType, int foodMenuId)//
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                //      var query = "select top 1 UnitPrice from PurchaseDetail where FoodMenuId=" + foodMenuId + " order by id desc";
                var query = "";

                if (itemType == 0)
                {
                    query = " select top 1 UnitPrice from " +
                          " (select Id, '' as PDId,SalesPrice as UnitPrice from foodmenu  where Id = " + foodMenuId +
                          " union " +
                          " select '' as Id, Id asPDId, UnitPrice from SalesDetail where FoodMenuId = " + foodMenuId + ") restuls " +
                          " order by PDid desc; ";

                }
                else if (itemType == 1)
                {
                    query = " select top 1 UnitPrice from " +
                                " (select Id, '' as PDId,SalesPrice as UnitPrice from Ingredient  where Id = " + foodMenuId +
                                " union " +
                                " select '' as Id, Id asPDId, UnitPrice from SalesDetail where IngredientId = " + foodMenuId + ") restuls " +
                                " order by PDid desc; ";
                }
                else if (itemType == 2)
                {
                    query = " select top 1 UnitPrice from " +
                          " (select Id, '' as PDId,CostPrice as UnitPrice from AssetItem  where Id = " + foodMenuId +
                          " union " +
                          " select '' as Id, Id asPDId, UnitPrice from SalesDetail where AssetItemId = " + foodMenuId + ") restuls " +
                          " order by PDid desc; ";
                }
                return con.ExecuteScalar<decimal>(query, null, sqltrans, 0, System.Data.CommandType.Text);
            }
        }
        public List<SalesDeliveryModel> GetPurchaseGRNFoodMenuByPurchaseId(long purchaseId)//
        {
            List<SalesDeliveryModel> purchaseModelList = new List<SalesDeliveryModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT 0 as Id,P.Id AS SalesId,P.StoreId,P.EmployeeId,P.ReferenceNo,GETDATE() AS SalesDeliveryDate,Customer.CustomerName, Customer.Id as CustomerId,P.GrossAmount,P.TaxAmount,P.GrandTotal As TotalAmount, " +
                            "P.DueAmount,P.PaidAmount,null AS DeliveryNoteNumber,null as DeliveryDate,null as DriverName,null as VehicleNumber,P.Notes, p.VatableAmount,p.NonVatableAmount  FROM Sales P inner join Customer on P.CustomerId = Customer.Id " +
                            "Where P.InventoryType = 1 And P.Isdeleted = 0 And P.Id = " + purchaseId;
                purchaseModelList = con.Query<SalesDeliveryModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public List<SalesDeliveryDetailModel> GetPurchaseGRNFoodMenuDetailsByPurchaseId(long purchaseId)//
        {
            List<SalesDeliveryDetailModel> purchaseDetails = new List<SalesDeliveryDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select 0 AS SalesDeliveryId," +
                             " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then 2 else 1 end) else 0 end) as ItemType, " +
                             " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then PD.AssetItemId else PD.IngredientId end) else PD.FoodMenuId end) as FoodMenuId, " +
                             " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then AI.AssetItemName else I.IngredientName end) else f.FoodMenuName end) as FoodMenuName,  " +
                            "pd.UnitPrice,PD.Qty AS SOQty,PD.Qty AS DeliveryQty,PD.GrossAmount,PD.TaxAmount,PD.TotalAmount,PD.VatableAmount,PD.NonVatableAmount ,PD.DiscountPercentage,PD.DiscountAmount " +
                            "From Sales P Inner join SalesDetail PD On P.Id = PD.SalesId " +
                             " left join FoodMenu as f on PD.FoodMenuId = f.Id " +
                             " left join Ingredient as I on PD.IngredientId = I.Id " +
                             "   left join AssetItem as AI on PD.AssetItemId = AI.Id  " +
                            "Where  P.isdeleted = 0 and pd.isdeleted = 0 And P.Id = " + purchaseId;
                purchaseDetails = con.Query<SalesDeliveryDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }
        public int GetPurchaseIdByPOReference(string poReference)//
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select Id from Sales Where IsDeleted=0 and Status<4 and ReferenceNo='" + poReference + "' and isDeleted=0";
                return con.QueryFirstOrDefault<int>(query);
            }
        }
        public int UpdatePurchaseOrderId(long purchaseId)//
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update sales set Status = 4 where id = " + purchaseId;
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

        public List<SalesDeliveryModel> GetSalesDeliveryReportById(long id)
        {
            List<SalesDeliveryModel> purchaseModelList = new List<SalesDeliveryModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select SI.ReferenceNumber AS ReferenceNo,s.StoreName,si.SalesDeliveryDate,c.CustomerName,c.CustomerAddress1,c.CustomerAddress2,c.CustomerNumber,c.CustomerEmail " +
                            " ,si.Notes,si.GrossAmount ,si.TaxAmount,si.TotalAmount,si.NonVatableAmount,si.VatableAmount,si.DeliveryNoteNumber,si.DeliveryDate,si.DriverName,si.VehicleNumber " +
                            " ,O.OutletAddress1,O.OutletAddress2,O.OutletPhone,O.OutletEmail,O.InvoiceHeader,O.InvoiceFooter, C.TaxInclusive as CustomerTaxInclusive " +
                            " from salesdelivery SI Inner Join Store S On s.Id=si.StoreId inner join Outlet O on O.Storeid = S.Id " +
                            " inner join Customer c on c.Id=si.CustomerId where si.Id= " + id;
                purchaseModelList = con.Query<SalesDeliveryModel>(query).AsList();
            }
            return purchaseModelList;
        }

        public List<SalesDeliveryDetailModel> GetSalesDeliveryReportFoodMenuDetails(long id)
        {
            List<SalesDeliveryDetailModel> purchaseDetails = new List<SalesDeliveryDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select ROW_NUMBER() over ( Order by PIN.Id) as SrNumber,pin.Id as SalesInvoiceId,  (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then 2 else 1 end) else 0 end) as ItemType,  " +
                     " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then PIN.AssetItemId else pin.IngredientId end) else pin.FoodMenuId end) as FoodMenuId,   " +
                     " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then AI.AssetItemName else I.IngredientName end) else f.FoodMenuName end) as FoodMenuName,   " +
                     " pin.UnitPrice as UnitPrice, pin.SOQty,PIN.DeliveryQty , pin.GrossAmount,pin.TaxAmount,pin.TotalAmount, pin.DiscountPercentage,pin.DiscountAmount  , pin.VatableAmount,pin.NonVatableAmount ," +
                     " (case when PIN.FoodMenuId is null then (case when PIN.IngredientId is null then UA.UnitName else UI.UnitName end) else UF.UnitName end) as UnitName" +
                    " from salesdelivery as P  inner join salesdeliveryDetail as PIN on P.id = pin.SalesDeliveryId  " +
                     " left join FoodMenu as f on pin.FoodMenuId = f.Id   left join Ingredient as I on pin.IngredientId = I.Id  left join Units As UI On UI.Id = I.IngredientUnitId  " +
                     " left join AssetItem as AI on PIN.AssetItemId = AI.Id    left join Units As UF On UF.Id = F.UnitsId     left join Units As UA On UA.Id = AI.UnitId  " +
                     " where P.id = " + id + " and pin.isdeleted = 0 and p.isdeleted = 0 ";

                purchaseDetails = con.Query<SalesDeliveryDetailModel>(query).AsList();
            }
            return purchaseDetails;
        }

        public List<SalesDeliveryDetailModel> GetViewSalesDeliveryFoodMenuDetails(long PurchaseGRNId)
        {
            List<SalesDeliveryDetailModel> purchaseDetails = new List<SalesDeliveryDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select pin.Id as SalesDeliveryId," +
                         " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then 2 else 1 end) else 0 end) as ItemType, " +
                         " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then PIN.AssetItemId else pin.IngredientId end) else pin.FoodMenuId end) as FoodMenuId, " +
                         " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then AI.AssetItemName else I.IngredientName end) else f.FoodMenuName end) as FoodMenuName,  " +
                         "  pin.UnitPrice as UnitPrice, pin.SOQty,PIN.DeliveryQty , pin.GrossAmount,pin.TaxAmount,pin.TotalAmount,pin.DiscountPercentage,pin.DiscountAmount, " +
                         "   (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then UA.UnitName else UI.UnitName end) else UF.UnitName end) as UnitName, pin.VatableAmount,pin.NonVatableAmount  " +
                         " from salesdelivery as P inner join salesdeliveryDetail as PIN on P.id = pin.salesdeliveryId " +
                         " left join FoodMenu as f on pin.FoodMenuId = f.Id " +
                         " left join Ingredient as I on pin.IngredientId = I.Id " +
                         "  left join AssetItem as AI on PIN.AssetItemId = AI.Id " +
                         "  left join Units As UI On UI.Id = I.IngredientUnitId  " +
                         "  left join Units As UF On UF.Id = F.UnitsId " +
                         "   left join Units As UA On UA.Id = AI.UnitId  " +
                         " where P.id = " + PurchaseGRNId + " and pin.isdeleted = 0 and p.isdeleted = 0";

                purchaseDetails = con.Query<SalesDeliveryDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public List<SalesDeliveryModel> GetViewSalesDeliveryFoodMenuById(long PurchaseGRNId)
        {
            List<SalesDeliveryModel> purchaseModelList = new List<SalesDeliveryModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select P.Referenceno as SOReferenceNo , P.SalesDate as SODate, salesdelivery.Id as Id, salesdelivery.StoreId,salesdelivery.EmployeeId,ReferenceNumber as ReferenceNo,salesdeliveryDate ,Customer.CustomerName, Customer.Id as CustomerId," +
                      "salesdelivery.GrossAmount, salesdelivery.TaxAmount,salesdelivery.TotalAmount, salesdelivery.VatableAmount,salesdelivery.NonVatableAmount ,salesdelivery.DueAmount as Due,salesdelivery.PaidAmount as Paid,salesdelivery.DeliveryNoteNumber ,salesdelivery.DeliveryDate ,salesdelivery.DriverName ,salesdelivery.VehicleNumber,salesdelivery.Notes,S.StoreName " +
                      "from salesdelivery  left JOIN sales P on P.Id = salesdelivery.salesid inner join Customer on salesdelivery.customerId = customer.Id inner join Store S on salesdelivery.StoreId = s.Id where salesdelivery.Isdeleted = 0 and salesdelivery.Id = " + PurchaseGRNId;
                purchaseModelList = con.Query<SalesDeliveryModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public bool GetCustomerTaxExampt(int customerId)
        {
            bool result;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " Select ISNULL(TaxInclusive,0) from customer where id = " + customerId;
                result = (bool)con.ExecuteScalar(query);
            }
            return result;
        }
    }
}

