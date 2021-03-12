using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using Dapper;
using RocketPOS.Interface.Repository;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using RocketPOS.Framework;

namespace RocketPOS.Repository
{
    public class PurchaseGRNRepository : IPurchaseGRNRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public PurchaseGRNRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<PurchaseGRNViewModel> GetPurchaseGRNList()
        {
            List<PurchaseGRNViewModel> purchaseViewModelList = new List<PurchaseGRNViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseGRN.Id as Id, PurchaseGRN.ReferenceNumber as ReferenceNo, convert(varchar(12),PurchaseGRNDate, 3) as [PurchaseGRNDate],Supplier.SupplierName," +
                    "PurchaseGRN.GrossAmount,PurchaseGRN.TaxAmount,PurchaseGRN.TotalAmount " +
                    "from PurchaseGRN inner join Supplier on PurchaseGRN.SupplierId = Supplier.Id where PurchaseGRN.InventoryType=2 And PurchaseGRN.Isdeleted = 0 order by PurchaseGRNDate, PurchaseId desc";
                purchaseViewModelList = con.Query<PurchaseGRNViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }

        public List<PurchaseGRNModel> GetPurchaseGRNById(long purchaseId)
        {
            List<PurchaseGRNModel> purchaseModelList = new List<PurchaseGRNModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseGRN.Id as Id, PurchaseGRN.StoreId,PurchaseGRN.EmployeeId,PurchaseId as ReferenceNo,PurchaseGRNDate as [Date],Supplier.SupplierName, Supplier.Id as SupplierId," +
                      " PurchaseGRN.[GrossAmount] as GrandTotal,PurchaseGRN.DueAmount as Due,PurchaseGRN.PaidAmount as Paid,PurchaseGRN.DeliveryNoteNumber,PurchaseGRN.DeliveryDate,PurchaseGRN.DriverName,PurchaseGRN.VehicleNumber,PurchaseGRN.Notes " +
                      " ,[DeliveryNoteNumber], [DeliveryDate],[DriverName],[VehicleNumber], PurchaseGRN.VatableAmount,PurchaseGRN.NonVatableAmount " +
                      " from PurchaseGRN inner join Supplier on PurchaseGRN.SupplierId = Supplier.Id where PurchaseGRN.InventoryType=2 And PurchaseGRN.Isdeleted = 0 and PurchaseGRN.Id = " + purchaseId;
                purchaseModelList = con.Query<PurchaseGRNModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public int InsertPurchaseGRN(PurchaseGRNModel purchaseGRNModel)
        {
            int result = 0;
            int detailResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[PurchaseGRN] " +
                             " [InventoryType] " +
                             " ,[PurchaseOrderId] " +
                             " ,[ReferenceNumber] " +
                             " ,[PurchaseId] " +
                             " ,[PurchaseGRNDate] " +
                             " ,[SupplierId] " +
                             " ,[StoreId] " +
                             " ,[EmployeeId] " +
                             " ,[GrossAmount] " +
                             " ,[TaxAmount] " +
                             " ,[TotalAmount] " +
                            "  ,[VatableAmount]      " +
                             "  ,[NonVatableAmount]      " +
                             " ,[PaidAmount] " +
                             " ,[DueAmount] " +
                             " ,[DeliveryNoteNumber] " +
                             " ,[DeliveryDate] " +
                             " ,[DriverName] " +
                             " ,[VehicleNumber] " +
                             " ,[Notes] " +
                             "  ,[UserIdUpdated]  " +
                             "  ,[DateInserted]   " +
                             "  ,[IsDeleted])     " +
                             "   VALUES           " +
                             "  ( @InventoryType " +
                             " ,@PurchaseOrderId " +
                             " ,@ReferenceNumber " +
                             " ,@PurchaseId " +
                             " ,@PurchaseGRNDate " +
                             " ,@SupplierId " +
                             " ,@StoreId " +
                             " ,@EmployeeId " +
                             " ,@GrossAmount " +
                             " ,@TaxAmount " +
                             " ,@TotalAmount " +
                            "  ,@VatableAmount,     " +
                             " ,@NonVatableAmount     " +
                             " ,@PaidAmount " +
                             " ,@DueAmount " +
                             " ,@DeliveryNoteNumber " +
                             " ,@DeliveryDate " +
                             " ,@DriverName " +
                             " ,@VehicleNumber " +
                             " ,@Notes, " +
                             "" + LoginInfo.Userid + "," +
                             "   GetUtcDate(),    " +
                             "   0); SELECT CAST(SCOPE_IDENTITY() as int); ";
                result = con.ExecuteScalar<int>(query, purchaseGRNModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    foreach (var item in purchaseGRNModel.PurchaseGRNDetails)
                    {
                        var queryDetails = "INSERT INTO [dbo].[PurchaseGRNDetail]" +
                                             "  ,[PurchaseGRNId] " +
                                             " ,[FoodMenuId] " +
                                             " ,[IngredientId] " +
                                             " ,[POQty] " +
                                             " ,[GRNQty] " +
                                             " ,[UnitPrice] " +
                                             " ,[GrossAmount] " +
                                             " ,[DiscountPercentage]  " +
                                             " ,[DiscountAmount] " +
                                             " ,[TaxAmount] " +
                                             " ,[TotalAmount]  " +
                                             " ,[VatableAmount]      " +
                                             " ,[NonVatableAmount]      " +
                                             " ,[UserIdUpdated]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              item.FoodMenuId + "," +
                                              item.IngredientId + "," +
                                              item.POQTY + "," +
                                              item.GRNQTY + "," +
                                              item.UnitPrice + "," +
                                              item.GrossAmount + "," +
                                              item.DiscountPercentage + "," +
                                              item.DiscountAmount + "," +
                                              item.TaxAmount + "," +
                                              item.TotalAmount + "," +
                                              item.VatableAmount + "," +
                                              item.NonVatableAmount + "," +
                                    LoginInfo.Userid + ",0); SELECT CAST(ReferenceNumber as INT) from PurchaseGRN where id = " + result + "; ";
                        detailResult = con.ExecuteScalar<int>(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);

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

            return detailResult;
        }
        public int UpdatePurchaseGRN(PurchaseGRNModel purchaseGRNModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update [dbo].[PurchaseGRN] set " +
                            " InventoryType = @InventoryType " +
                            ",ReferenceNumber = @ReferenceN0 " +
                            ",PurchaseId = @PurchaseId " +
                            ",PurchaseGRNDate = @PurchaseGRNDate " +
                            ",SupplierId = @SupplierId " +
                            ",StoreId = @StoreId " +
                            ",EmployeeId = @EmployeeId " +
                            ",GrossAmount = @GrossAmount " +
                            ",TaxAmount = @TaxAmount " +
                            ",TotalAmount = @TotalAmount " +
                            ",[VatableAmount] = @VatableAmount     " +
                            ",[NonVatableAmount] = @NonVatableAmount      " +
                            ",PaidAmount = @PaidAmount " +
                            ",DueAmount = @DueAmount " +
                            ",DeliveryNoteNumber = @DeliveryNoteNumber " +
                            ",DeliveryDate = @DeliveryDate " +
                            ",DriverName = @DriverName " +
                            ",VehicleNumber = @VehicleNumber " +
                            ",Notes = @Notes " +
                            "  ,[UserIdUpdated] = " + LoginInfo.Userid + " " +
                            "  ,[DateUpdated]  = GetUtcDate()  where id= " + purchaseGRNModel.Id + ";";
                result = con.Execute(query, purchaseGRNModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    if (purchaseGRNModel.DeletedId != null)
                    {
                        foreach (var item in purchaseGRNModel.DeletedId)
                        {
                            var deleteQuery = $"update PurchaseGRNDetail set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in purchaseGRNModel.PurchaseGRNDetails)
                    {
                        var queryDetails = string.Empty;
                        if (item.PurchaseGRNId > 0)
                        {
                            queryDetails = "Update [dbo].[PurchaseGRNDetail] set " +
                                             ",[PurchaseGRNId]		  	 = " + item.PurchaseGRNId +
                                              ",[FoodMenuId]             = " + item.FoodMenuId +
                                              ",[IngredientId]           = " + item.IngredientId +
                                              ",[POQty]                  = " + item.POQTY +
                                              ",[GRNQty]                 = " + item.GRNQTY +
                                              ",[UnitPrice]              = " + item.UnitPrice +
                                              ",[GrossAmount]            = " + item.GrossAmount +
                                              ",[DiscountPercentage]     = " + item.DiscountPercentage +
                                              ",[DiscountAmount]         = " + item.DiscountAmount +
                                              ",[TaxAmount]              = " + item.TaxAmount +
                                              ",[TotalAmount]            = " + item.TotalAmount +
                                             " [VatableAmount] = " + item.VatableAmount + "," +
                                             " [NonVatableAmount] = " + item.NonVatableAmount + "," +
                                               " ,[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                                 " [DateUpdated] = GetUTCDate() " +
                                                 " where id = " + item.PurchaseGRNId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[PurchaseGRNDetail]" +
                                              "[PurchaseGRNId] " +
                                              ",[FoodMenuId] " +
                                              ",[IngredientId] " +
                                              ",[POQty] " +
                                              ",[GRNQty] " +
                                              ",[UnitPrice] " +
                                              ",[GrossAmount] " +
                                              ",[DiscountPercentage] " +
                                              ",[DiscountAmount] " +
                                              ",[TaxAmount] " +
                                              ",[TotalAmount] " +
                                            "  ,[VatableAmount]      " +
                                             "  ,[NonVatableAmount]      " +
                                              " ,[UserIdUpdated] ) " +
                                              " VALUES           " +
                                              "(" + purchaseGRNModel.Id + "," +
                                              item.FoodMenuId + "," +
                                              item.IngredientId + "," +
                                              item.POQTY + "," +
                                              item.GRNQTY + "," +
                                              item.UnitPrice + "," +
                                              item.GrossAmount + "," +
                                              item.DiscountPercentage + "," +
                                              item.DiscountAmount + "," +
                                              item.TaxAmount + "," +
                                              item.TotalAmount + "," +
                                              item.VatableAmount + "," +
                                              item.NonVatableAmount + "," +
                                               LoginInfo.Userid + "); ";
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
        public int DeletePurchaseGRN(long purchaseGRNId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update PurchaseGRN set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where id = " + purchaseGRNId + ";" +
                    " Update PurchaseGRNDetail set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where PurchaseGRNId = " + purchaseGRNId + ";";
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
        public List<PurchaseGRNDetailModel> GetPurchaseGRNDetails(long purchaseId)
        {
            List<PurchaseGRNDetailModel> purchaseDetails = new List<PurchaseGRNDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " SELECT PG.[PurchaseGRNId],PG.[FoodMenuId],PG.[IngredientId],PG.[POQty],PG.[GRNQty],PG.[UnitPrice],PG.[GrossAmount],PG.[DiscountPercentage],PG.[DiscountAmount],PG.[TaxAmount],PG.[TotalAmount], " +
                         " IngredientName, PG.VatableAmount,PG.NonVatableAmount " +
                         " FROM PurchaseGRN PG INNER JOIN PURCHASEGRNDetail PGD" +
                         " inner join Ingredient as i on pin.IngredientId = i.Id" +
                         " INNER JOIN FOODMENU FM ON FM.ID = PGD.FOODMENUID" +
                         " WHERE PG.id = " + purchaseId + " AND PG.InventoryType = 2 and PGD.isdeleted = 0 and PG.isdeleted = 0";
                purchaseDetails = con.Query<PurchaseGRNDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }
        public int DeletePurchaseGRNDetails(long purchaseDetailsId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update PurchaseGRNDetail set IsDeleted = 1 where id = " + purchaseDetailsId + ";";
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
        public string ReferenceNumber()
        {
            string result = string.Empty;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"SELECT RIGHT('' + CONVERT(VARCHAR(8),ISNULL(MAX(PurchaseId),0) + 1), 6) FROM PurchaseGRN where InventoryType=2;";
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
        public List<PurchaseGRNModel> GetPurchaseGRNFoodMenuById(long purchaseGRNId)
        {
            List<PurchaseGRNModel> purchaseModelList = new List<PurchaseGRNModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseGRN.Id as Id, PurchaseGRN.StoreId,PurchaseGRN.EmployeeId,ReferenceNumber as ReferenceNo,PurchaseGRNDate ,Supplier.SupplierName, Supplier.Id as SupplierId," +
                      "PurchaseGRN.GrossAmount, PurchaseGRN.TaxAmount,PurchaseGRN.TotalAmount,PurchaseGRN.VatableAmount,PurchaseGRN.NonVatableAmount ,PurchaseGRN.DueAmount as Due,PurchaseGRN.PaidAmount as Paid,PurchaseGRN.DeliveryNoteNumber ,PurchaseGRN.DeliveryDate ,PurchaseGRN.DriverName ,PurchaseGRN.VehicleNumber,PurchaseGRN.Notes " +
                      "from PurchaseGRN inner join Supplier on PurchaseGRN.SupplierId = Supplier.Id where PurchaseGRN.InventoryType=1 And PurchaseGRN.Isdeleted = 0 and PurchaseGRN.Id = " + purchaseGRNId;
                purchaseModelList = con.Query<PurchaseGRNModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public List<PurchaseGRNViewModel> GetPurchaseGRNFoodMenuList()
        {
            List<PurchaseGRNViewModel> purchaseViewModelList = new List<PurchaseGRNViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseGRN.Id as Id, PurchaseGRN.ReferenceNumber as ReferenceNo, convert(varchar(12),PurchaseGRNDate, 3) as [Date],Supplier.SupplierName,S.Storename," +
                    "PurchaseGRN.TotalAMount,PurchaseGRN.DueAmount as Due ,isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as Username  " +
                    "from PurchaseGRN inner join Supplier on PurchaseGRN.SupplierId = Supplier.Id inner join [User] U on U.Id=PurchaseGRN.UserIdInserted  inner join employee e on e.id = u.employeeid  inner join store S on S.Id = PurchaseGRN.StoreId  where PurchaseGRN.InventoryType=1 And PurchaseGRN.Isdeleted = 0 order by PurchaseGRNDate, PurchaseId desc";
                purchaseViewModelList = con.Query<PurchaseGRNViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }
        public List<PurchaseGRNDetailModel> GetPurchaseGRNFoodMenuDetails(long purchaseGRNId)
        {
            List<PurchaseGRNDetailModel> purchaseDetails = new List<PurchaseGRNDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select pin.Id as PurchaseGRNId," +
                            " (case when pin.FoodMenuId is null then 1 else 0 end) as ItemType, " +
                            " (case when pin.FoodMenuId is null then pin.IngredientId else pin.FoodMenuId end) as FoodMenuId, " +
                            " (case when pin.FoodMenuId is null then I.Ingredientname else f.FoodMenuName end) as FoodMenuName, " +
                            " pin.UnitPrice as UnitPrice, pin.POQty,PIN.GRNQty , pin.GrossAmount,pin.TaxAmount,pin.TotalAmount, pin.VatableAmount,pin.NonVatableAmount , pin.DiscountPercentage,pin.DiscountAmount " +
                            " from purchaseGRN as P inner join PurchaseGRNDetail as PIN on P.id = pin.PurchaseGRNId " +
                            " left join FoodMenu as f on pin.FoodMenuId = f.Id " +
                            " left join Ingredient as I on pin.IngredientId = I.Id " +
                            " where P.id = " + purchaseGRNId + " and pin.isdeleted = 0 and p.isdeleted = 0";
                purchaseDetails = con.Query<PurchaseGRNDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public List<PurchaseGRNViewModel> PurchaseGRNFoodMenuListByDate(string fromDate, string toDate, int supplierId, int storeId)
        {
            List<PurchaseGRNViewModel> purchaseViewModels = new List<PurchaseGRNViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseGRN.Id as Id,PurchaseGRN.PurchaseId, PurchaseGRN.ReferenceNumber as ReferenceNo, convert(varchar(12),PurchaseGRNDate, 3) as [Date],Supplier.SupplierName,S.Storename," +
                    "PurchaseGRN.TotalAMount,PurchaseGRN.DueAmount as Due ,isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as Username  " +
                    "from PurchaseGRN inner join Supplier on PurchaseGRN.SupplierId = Supplier.Id inner join [User] U on U.Id=PurchaseGRN.UserIdInserted  inner join employee e on e.id = u.employeeid inner join store S on S.Id = PurchaseGRN.StoreId  where PurchaseGRN.InventoryType=1 And PurchaseGRN.Isdeleted = 0  " +
                    // " AND Convert(varchar(10), PurchaseGRNDate, 103)  between '" + fromDate + "' and '" + toDate + "'";
                    " AND Convert(Date, PurchaseGRNDate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103)  ";

                if (supplierId != 0)
                {
                    query += " And PurchaseGRN.SupplierId= " + supplierId;
                }

                if (storeId != 0)
                {
                    query += " And PurchaseGRN.StoreId= " + storeId;
                }
                query += "  order by PurchaseGRN.PurchaseGRNDate, PurchaseGRN.id desc";

                purchaseViewModels = con.Query<PurchaseGRNViewModel>(query).AsList();
            }

            return purchaseViewModels;
        }

        public int InsertPurchaseGRNFoodMenu(PurchaseGRNModel purchaseModel)
        {
            int result = 0;
            int detailResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[PurchaseGRN] " +
                             "  ([PurchaseId] " +
                             "  ,[ReferenceNumber] " +
                             "  ,[InventoryType]  " +
                             "  ,[SupplierId]     " +
                             "  ,[StoreId]        " +
                             "  ,[EmployeeId]        " +
                             "  ,[PurchaseGRNDate]   " +
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
                             "  (@PurchaseId,  " +
                             "  @ReferenceNo,  " +
                             "   @InventoryType,      " +
                             "   @SupplierId,      " +
                             "   @StoreId,         " +
                             "   @EmployeeId,         " +
                             "   @PurchaseGRNDate,    " +
                             "   @GrossAmount,     " +
                             "   @TaxAmount,     " +
                             "   @TotalAmount,     " +
                             "   @VatableAmount,      " +
                             "   @NonVatableAmount,      " +
                            "  @DeliveryNoteNumber," +
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

                    foreach (var item in purchaseModel.PurchaseGRNDetails)
                    {
                        var queryDetails = "INSERT INTO [dbo].[PurchaseGRNDetail]" +
                                             "  ([PurchaseGRNId] " +
                                             " ,[FoodMenuId] " +
                                             " ,[IngredientId] " +
                                             " ,[AssetItemId] " +
                                             " ,[POQty] " +
                                             " ,[GRNQty] " +
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
                        queryDetails = queryDetails + "" + item.POQTY + "," +
                                      item.GRNQTY + "," +
                                      item.UnitPrice + "," +
                                      item.GrossAmount + "," +
                                      item.DiscountPercentage + "," +
                                      item.DiscountAmount + "," +
                                      item.TaxAmount + "," +
                                      item.TotalAmount + "," +
                                      item.VatableAmount + "," +
                                      item.NonVatableAmount + "," +
                            LoginInfo.Userid + ",GetUTCDate(),0); SELECT CAST(ReferenceNumber as INT) from PurchaseGRN where id = " + result + "; ";

                        detailResult = con.ExecuteScalar<int>(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (detailResult > 0)
                    {
                        sqltrans.Commit();

                        int outResult = 0;
                        if (purchaseModel.PurchaseId > 0)
                            outResult = UpdatePurchaseOrderId(purchaseModel.PurchaseId);

                        CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                        string sResult = commonRepository.InventoryPush("GRN", result);

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

        public int UpdatePurchaseGRNFoodMenu(PurchaseGRNModel purchaseModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update [dbo].[PurchaseGRN] set " +
                             "   [SupplierId]  = @SupplierId" +
                             "  ,[StoreId]  = @StoreId" +
                             "  ,[EmployeeId]  = @EmployeeId" +
                             "  ,[PurchaseGRNDate] = @PurchaseGRNDate  " +
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
                            var deleteQuery = $"update PurchaseGRNDetail set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in purchaseModel.PurchaseGRNDetails)
                    {
                        var queryDetails = string.Empty;
                        if (item.PurchaseGRNId > 0)
                        {
                            queryDetails = "Update [dbo].[PurchaseGRNDetail] set " +
                                             "[PurchaseGRNId] = " + purchaseModel.Id;
                            if (item.ItemType == 0)
                            {
                                queryDetails = queryDetails + " [FoodMenuId]  = " + item.FoodMenuId + ",[IngredientId] = null,[AssetItemId] = null, ";
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
                              ",[POQty]  = " + item.POQTY +
                              ",[GRNQty]   = " + item.GRNQTY +
                              ",[UnitPrice]   = " + item.UnitPrice +
                              ",[GrossAmount]   = " + item.GrossAmount +
                              ",[DiscountPercentage] = " + item.DiscountPercentage +
                              ",[DiscountAmount]  = " + item.DiscountAmount +
                              ",[TaxAmount]   = " + item.TaxAmount +
                              ",[TotalAmount]  = " + item.TotalAmount +
                             " ,[VatableAmount] = " + item.VatableAmount +
                             " ,[NonVatableAmount] = " + item.NonVatableAmount +
                               " ,[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                 " [DateUpdated] = GetUTCDate() " +
                                 " where id = " + item.PurchaseGRNId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[PurchaseGRNDetail]" +
                                              "  ([PurchaseGRNId] " +
                                              " ,[FoodMenuId] " +
                                              " ,[IngredientId] " +
                                              " ,[AssetItemId] " +
                                              " ,[POQty] " +
                                              " ,[GRNQty] " +
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
                            queryDetails = queryDetails + item.POQTY + "," +
                                               item.GRNQTY + "," +
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

        public string ReferenceNumberFoodMenu()
        {
            string result = string.Empty;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"SELECT ISNULL(MAX(convert(int,ReferenceNumber)),0) + 1 FROM purchaseGRN where InventoryType=1 and ISDeleted=0;";
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

        public decimal GetFoodMenuLastPrice(int itemType, int foodMenuId)
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
                          " (select Id, '' as PDId,PurchasePrice as UnitPrice from foodmenu  where Id = " + foodMenuId +
                          " union " +
                          " select '' as Id, Id asPDId, UnitPrice from PurchaseDetail where FoodMenuId = " + foodMenuId + ") restuls " +
                          " order by PDid desc; ";

                }
                else if (itemType == 1)
                {
                    query = " select top 1 UnitPrice from " +
                                " (select Id, '' as PDId,PurchasePrice as UnitPrice from Ingredient  where Id = " + foodMenuId +
                                " union " +
                                " select '' as Id, Id asPDId, UnitPrice from PurchaseDetail where IngredientId = " + foodMenuId + ") restuls " +
                                " order by PDid desc; ";
                }
                else if (itemType == 2)
                {
                    query = " select top 1 UnitPrice from " +
                          " (select Id, '' as PDId,CostPrice as UnitPrice from AssetItem  where Id = " + foodMenuId +
                          " union " +
                          " select '' as Id, Id asPDId, UnitPrice from PurchaseDetail where AssetItemId = " + foodMenuId + ") restuls " +
                          " order by PDid desc; ";
                }
                return con.ExecuteScalar<decimal>(query, null, sqltrans, 0, System.Data.CommandType.Text);
            }
        }

        public List<PurchaseGRNModel> GetPurchaseGRNFoodMenuByPurchaseId(long purchaseId)
        {
            List<PurchaseGRNModel> purchaseModelList = new List<PurchaseGRNModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT 0 as Id,P.Id AS PurchaseId,P.StoreId,P.EmployeeId,P.ReferenceNo,GETDATE() AS PurchaseGRNDate,Supplier.SupplierName, Supplier.Id as SupplierId,P.GrossAmount,P.TaxAmount,P.GrandTotal As TotalAmount, " +
                            "P.DueAmount,P.PaidAmount,null AS DeliveryNoteNumber,null as DeliveryDate,null as DriverName,null as VehicleNumber,P.Notes, p.VatableAmount,p.NonVatableAmount  FROM Purchase P inner join Supplier on P.SupplierId = Supplier.Id " +
                            "Where P.InventoryType = 1 And P.Isdeleted = 0 And P.Id = " + purchaseId;
                purchaseModelList = con.Query<PurchaseGRNModel>(query).AsList();
            }
            return purchaseModelList;
        }

        public List<PurchaseGRNDetailModel> GetPurchaseGRNFoodMenuDetailsByPurchaseId(long purchaseId)
        {
            List<PurchaseGRNDetailModel> purchaseDetails = new List<PurchaseGRNDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select 0 AS PurchaseGRNId,"+
                             " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then 2 else 1 end) else 0 end) as ItemType, " +
                             " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then PD.AssetItemId else PD.IngredientId end) else PD.FoodMenuId end) as FoodMenuId, " +
                             " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then AI.AssetItemName else I.IngredientName end) else f.FoodMenuName end) as FoodMenuName,  " +
                            "pd.UnitPrice,PD.Qty AS POQty,PD.Qty AS GRNQty,PD.GrossAmount,PD.TaxAmount,PD.TotalAmount,PD.VatableAmount,PD.NonVatableAmount ,PD.DiscountPercentage,PD.DiscountAmount " +
                            "From Purchase P Inner join PurchaseDetail PD On P.Id = PD.PurchaseId " +
                             " left join FoodMenu as f on PD.FoodMenuId = f.Id " +
                             " left join Ingredient as I on PD.IngredientId = I.Id " +
                             "   left join AssetItem as AI on PD.AssetItemId = AI.Id  "+
                            "Where  P.isdeleted = 0 and pd.isdeleted = 0 And P.Id = " + purchaseId;
                purchaseDetails = con.Query<PurchaseGRNDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public int GetPurchaseIdByPOReference(string poReference)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select Id from Purchase Where IsDeleted=0 and Status<4 and ReferenceNo='" + poReference + "' and isDeleted=0";
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
                var query = $"update Purchase set Status = 4 where id = " + purchaseId;
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

        public List<PurchaseGRNDetailModel> GetViewPurchaseGRNFoodMenuDetails(long PurchaseGRNId)
        {
            List<PurchaseGRNDetailModel> purchaseDetails = new List<PurchaseGRNDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select pin.Id as PurchaseGRNId," +
                         " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then 2 else 1 end) else 0 end) as ItemType, " +
                         " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then PIN.AssetItemId else pin.IngredientId end) else pin.FoodMenuId end) as FoodMenuId, " +
                         " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then AI.AssetItemName else I.IngredientName end) else f.FoodMenuName end) as FoodMenuName,  " +
                         "  pin.UnitPrice as UnitPrice, pin.POQty,PIN.GRNQty , pin.GrossAmount,pin.TaxAmount,pin.TotalAmount,pin.DiscountPercentage,pin.DiscountAmount, " +
                         "   (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then UA.UnitName else UI.UnitName end) else UF.UnitName end) as UnitName, pin.VatableAmount,pin.NonVatableAmount  " +
                         " from purchaseGRN as P inner join PurchaseGRNDetail as PIN on P.id = pin.PurchaseGRNId " +
                         " left join FoodMenu as f on pin.FoodMenuId = f.Id " +
                         " left join Ingredient as I on pin.IngredientId = I.Id " +
                         "  left join AssetItem as AI on PIN.AssetItemId = AI.Id " +
                         "  left join Units As UI On UI.Id = I.IngredientUnitId  " +
                         "  left join Units As UF On UF.Id = F.UnitsId "+
                         "   left join Units As UA On UA.Id = AI.UnitId  "+
                         " where P.id = " + PurchaseGRNId + " and pin.isdeleted = 0 and p.isdeleted = 0";

                purchaseDetails = con.Query<PurchaseGRNDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public List<PurchaseGRNModel> GetViewPurchaseGRNFoodMenuById(long PurchaseGRNId)
        {
            List<PurchaseGRNModel> purchaseModelList = new List<PurchaseGRNModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select P.Referenceno as POReferenceNo , P.PurchaseDate as PODate, PurchaseGRN.Id as Id, PurchaseGRN.StoreId,PurchaseGRN.EmployeeId,ReferenceNumber as ReferenceNo,PurchaseGRNDate ,Supplier.SupplierName, Supplier.Id as SupplierId," +
                      "PurchaseGRN.GrossAmount, PurchaseGRN.TaxAmount,PurchaseGRN.TotalAmount, PurchaseGRN.VatableAmount,PurchaseGRN.NonVatableAmount ,PurchaseGRN.DueAmount as Due,PurchaseGRN.PaidAmount as Paid,PurchaseGRN.DeliveryNoteNumber ,PurchaseGRN.DeliveryDate ,PurchaseGRN.DriverName ,PurchaseGRN.VehicleNumber,PurchaseGRN.Notes,S.StoreName " +
                      "from PurchaseGRN  left JOIN PURCHASE P on P.Id = PurchaseGRN.Purchaseid inner join Supplier on PurchaseGRN.SupplierId = Supplier.Id inner join Store S on PurchaseGRN.StoreId = s.Id where PurchaseGRN.Isdeleted = 0 and PurchaseGRN.Id = " + PurchaseGRNId;
                purchaseModelList = con.Query<PurchaseGRNModel>(query).AsList();
            }
            return purchaseModelList;
        }
    }
}
