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
                      " ,[DeliveryNoteNumber], [DeliveryDate],[DriverName],[VehicleNumber] " +
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
                         " IngredientName" +
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
                      "PurchaseGRN.GrossAmount, PurchaseGRN.TaxAmount,PurchaseGRN.TotalAmount,PurchaseGRN.DueAmount as Due,PurchaseGRN.PaidAmount as Paid,PurchaseGRN.DeliveryNoteNumber ,PurchaseGRN.DeliveryDate ,PurchaseGRN.DriverName ,PurchaseGRN.VehicleNumber,PurchaseGRN.Notes " +
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
                var query = "select PurchaseGRN.Id as Id, PurchaseGRN.ReferenceNumber as ReferenceNo, convert(varchar(12),PurchaseGRNDate, 3) as [Date],Supplier.SupplierName," +
                    "PurchaseGRN.TotalAMount,PurchaseGRN.DueAmount as Due,U.Username " +
                    "from PurchaseGRN inner join Supplier on PurchaseGRN.SupplierId = Supplier.Id inner join [User] U on U.Id=PurchaseGRN.UserIdInserted where PurchaseGRN.InventoryType=1 And PurchaseGRN.Isdeleted = 0 order by PurchaseGRNDate, PurchaseId desc";
                purchaseViewModelList = con.Query<PurchaseGRNViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }
        public List<PurchaseGRNDetailModel> GetPurchaseGRNFoodMenuDetails(long purchaseGRNId)
        {
            List<PurchaseGRNDetailModel> purchaseDetails = new List<PurchaseGRNDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select pin.Id as PurchaseGRNId,pin.FoodMenuId as FoodMenuId,f.FoodMenuName,pin.UnitPrice as UnitPrice, pin.POQty,PIN.GRNQty , pin.GrossAmount,pin.TaxAmount,pin.TotalAmount,pin.DiscountPercentage,pin.DiscountAmount " +
                            " from purchaseGRN as P inner join PurchaseGRNDetail as PIN on P.id = pin.PurchaseGRNId " +
                            " inner join FoodMenu as f on pin.FoodMenuId = f.Id where P.id = " + purchaseGRNId + "and P.InventoryType=1 and pin.isdeleted = 0 and p.isdeleted = 0";
                purchaseDetails = con.Query<PurchaseGRNDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public List<PurchaseGRNViewModel> PurchaseGRNFoodMenuListByDate(string fromDate, string toDate, int supplierId)
        {
            List<PurchaseGRNViewModel> purchaseViewModels = new List<PurchaseGRNViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseGRN.Id as Id,PurchaseGRN.PurchaseId, PurchaseGRN.ReferenceNumber as ReferenceNo, convert(varchar(12),PurchaseGRNDate, 3) as [Date],Supplier.SupplierName," +
                    "PurchaseGRN.TotalAMount,PurchaseGRN.DueAmount as Due,U.Username " +
                    "from PurchaseGRN inner join Supplier on PurchaseGRN.SupplierId = Supplier.Id inner join [User] U on U.Id=PurchaseGRN.UserIdInserted where PurchaseGRN.InventoryType=1 And PurchaseGRN.Isdeleted = 0  " +
                    // " AND Convert(varchar(10), PurchaseGRNDate, 103)  between '" + fromDate + "' and '" + toDate + "'";
                    " AND Convert(Date, PurchaseGRNDate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103)  ";

                if (supplierId != 0)
                {
                    query += " And PurchaseGRN.SupplierId= " + supplierId;
                }
                query += "  order by PurchaseGRN.id desc";

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
                                             " ,[POQty] " +
                                             " ,[GRNQty] " +
                                             " ,[UnitPrice] " +
                                             " ,[GrossAmount] " +
                                             " ,[DiscountPercentage]  " +
                                             " ,[DiscountAmount] " +
                                             " ,[TaxAmount] " +
                                            " ,[TotalAmount]  " +
                                             " ,[UserIdInserted]" +
                                             " ,[DateInserted]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              item.FoodMenuId + "," +
                                              "null ," +
                                              item.POQTY + "," +
                                              item.GRNQTY + "," +
                                              item.UnitPrice + "," +
                                              item.GrossAmount + "," +
                                              item.DiscountPercentage + "," +
                                              item.DiscountAmount + "," +
                                              item.TaxAmount + "," +
                                              item.TotalAmount + "," +
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
                                             "[PurchaseGRNId] = " + purchaseModel.Id +
                                              ",[FoodMenuId]  = " + item.FoodMenuId +
                                              ",[POQty]  = " + item.POQTY +
                                              ",[GRNQty]   = " + item.GRNQTY +
                                              ",[UnitPrice]   = " + item.UnitPrice +
                                              ",[GrossAmount]   = " + item.GrossAmount +
                                              ",[DiscountPercentage] = " + item.DiscountPercentage +
                                              ",[DiscountAmount]  = " + item.DiscountAmount +
                                              ",[TaxAmount]   = " + item.TaxAmount +
                                              ",[TotalAmount]  = " + item.TotalAmount +
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
                                              " ,[POQty] " +
                                              " ,[GRNQty] " +
                                              " ,[UnitPrice] " +
                                              " ,[GrossAmount] " +
                                              " ,[DiscountPercentage]  " +
                                              " ,[DiscountAmount] " +
                                              " ,[TaxAmount] " +
                                              " ,[TotalAmount]  " +
                                              " ,[UserIdInserted]" +
                                               " ,[DateInserted]" +
                                               " ,[IsDeleted])   " +
                                               "VALUES           " +
                                               "(" + purchaseModel.Id + "," +
                                               item.FoodMenuId + "," +
                                                 "null," +
                                               item.POQTY + "," +
                                               item.GRNQTY + "," +
                                               item.UnitPrice + "," +
                                               item.GrossAmount + "," +
                                               item.DiscountPercentage + "," +
                                               item.DiscountAmount + "," +
                                               item.TaxAmount + "," +
                                               item.TotalAmount + "," +
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

        public decimal GetFoodMenuLastPrice(int foodMenuId)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                //      var query = "select top 1 UnitPrice from PurchaseDetail where FoodMenuId=" + foodMenuId + " order by id desc";
                var query = " select top 1 UnitPrice from " +
                             " (select Id, '' as PDId,PurchasePrice as UnitPrice from foodmenu  where Id = " + foodMenuId +
                             " union " +
                             " select '' as Id, Id asPDId, UnitPrice from PurchaseDetail where FoodMenuId = " + foodMenuId + ") restuls " +
                             " order by PDid desc; ";
                return con.ExecuteScalar<decimal>(query, null, sqltrans, 0, System.Data.CommandType.Text);
            }
        }

        public List<PurchaseGRNModel> GetPurchaseGRNFoodMenuByPurchaseId(long purchaseId)
        {
            List<PurchaseGRNModel> purchaseModelList = new List<PurchaseGRNModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT 0 as Id,P.Id AS PurchaseId,P.StoreId,P.EmployeeId,P.ReferenceNo,GETDATE() AS PurchaseGRNDate,Supplier.SupplierName, Supplier.Id as SupplierId,P.GrossAmount,P.TaxAmount,P.GrandTotal As TotalAmount, " +
                            "P.DueAmount,P.PaidAmount,null AS DeliveryNoteNumber,null as DeliveryDate,null as DriverName,null as VehicleNumber,P.Notes FROM Purchase P inner join Supplier on P.SupplierId = Supplier.Id " +
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
                var query = "Select 0 AS PurchaseGRNId,PD.FoodMenuId,f.FoodMenuName,pd.UnitPrice,PD.Qty AS POQty,PD.Qty AS GRNQty,PD.GrossAmount,PD.TaxAmount,PD.TotalAmount,PD.DiscountPercentage,PD.DiscountAmount " +
                            "From Purchase P Inner join PurchaseDetail PD On P.Id = PD.PurchaseId inner join FoodMenu as f on PD.FoodMenuId = f.Id " +
                            "Where P.InventoryType = 1 and P.isdeleted = 0 and pd.isdeleted = 0 And P.Id = " + purchaseId;
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

    }
}
