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
                var query = "select PurchaseGRN.Id as Id, PurchaseGRNNumber as ReferenceNo, convert(varchar(12),PurchaseGRNDate, 3) as [Date],Supplier.SupplierName," +
                    "PurchaseGRN.[GrossAmount] as GrandTotal,PurchaseGRN.DueAmount as Due " +
                    "from PurchaseGRN inner join Supplier on PurchaseGRN.SupplierId = Supplier.Id where PurchaseGRN.InventoryType=2 And PurchaseGRN.Isdeleted = 0 order by PurchaseGRNDate, purchaseNumber desc";
                purchaseViewModelList = con.Query<PurchaseGRNViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }

        public List<PurchaseGRNModel> GetPurchaseGRNById(long purchaseId)
        {
            List<PurchaseGRNModel> purchaseModelList = new List<PurchaseGRNModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseGRN.Id as Id, PurchaseGRN.StoreId,PurchaseGRN.EmployeeId,PurchaseNumber as ReferenceNo,PurchaseGRNDate as [Date],Supplier.SupplierName, Supplier.Id as SupplierId," +
                      " PurchaseGRN.[GrossAmount] as GrandTotal,PurchaseGRN.DueAmount as Due,PurchaseGRN.PaidAmount as Paid,PurchaseGRN.Notes " +
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
                             " ,[PurchaseNumber] " +
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
                             " ,@PurchaseNumber " +
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
                                    LoginInfo.Userid + ",0); SELECT CAST(PurchaseGRNNumber as INT) from PurchaseGRN where id = " + result + "; ";
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
                            ",PurchaseNumber = @PurchaseNumber " +
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
                var query = $"update PurchaseGRN set IsDeleted = 1 where id = " + purchaseGRNId + ";" +
                    " Update PurchaseGRNDetail set IsDeleted = 1 where PurchaseGRNId = " + purchaseGRNId + ";";
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
               var query=" SELECT PG.[PurchaseGRNId],PG.[FoodMenuId],PG.[IngredientId],PG.[POQty],PG.[GRNQty],PG.[UnitPrice],PG.[GrossAmount],PG.[DiscountPercentage],PG.[DiscountAmount],PG.[TaxAmount],PG.[TotalAmount], " +
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
                var query = $"SELECT RIGHT('000000' + CONVERT(VARCHAR(8),ISNULL(MAX(PurchaseNumber),0) + 1), 6) FROM PurchaseGRN where InventoryType=2;";
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
        public List<PurchaseGRNModel> GetPurchaseGRNFoodMenuById(long purchaseId)
        {
            List<PurchaseGRNModel> purchaseModelList = new List<PurchaseGRNModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseGRN.Id as Id, PurchaseGRN.StoreId,PurchaseGRN.EmployeeId,PurchaseGRNNumber as ReferenceNo,PurchaseGRNDate as [Date],Supplier.SupplierName, Supplier.Id as SupplierId," +
                      "PurchaseGRN.GrandTotal as GrandTotal,PurchaseGRN.DueAmount as Due,PurchaseGRN.PaidAmount as Paid,PurchaseGRN.Notes " +
                      "from PurchaseGRN inner join Supplier on PurchaseGRN.SupplierId = Supplier.Id where PurchaseGRN.InventoryType=1 And PurchaseGRN.Isdeleted = 0 and PurchaseGRN.Id = " + purchaseId;
                purchaseModelList = con.Query<PurchaseGRNModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public List<PurchaseGRNViewModel> GetPurchaseGRNFoodMenuList()
        {
            List<PurchaseGRNViewModel> purchaseViewModelList = new List<PurchaseGRNViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseGRN.Id as Id, PurchaseNumber as ReferenceNo, convert(varchar(12),PurchaseGRNDate, 3) as [Date],Supplier.SupplierName," +
                    "PurchaseGRN.GrossAmount as GrandTotal,PurchaseGRN.DueAmount as Due " +
                    "from PurchaseGRN inner join Supplier on PurchaseGRN.SupplierId = Supplier.Id where PurchaseGRN.InventoryType=1 And PurchaseGRN.Isdeleted = 0 order by PurchaseGRNDate, purchaseNumber desc";
                purchaseViewModelList = con.Query<PurchaseGRNViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }
        public List<PurchaseGRNDetailModel> GetPurchaseGRNFoodMenuDetails(long purchaseId)
        {
            List<PurchaseGRNDetailModel> purchaseDetails = new List<PurchaseGRNDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select pin.Id as PurchaseGRNId,pin.FoodMenuId as FoodMenuId,f.FoodMenuName,pin.UnitPrice as UnitPrice, pin.Qty as Quantity, pin.GrossAmount as Total " +
                            "from purchase as P inner join PurchaseGRNIngredient as PIN on P.id = pin.PurchaseGRNId " +
                            "inner join FoodMenu as f on pin.FoodMenuId = f.Id where P.id = " + purchaseId + "and P.InventoryType=1 and pin.isdeleted = 0 and p.isdeleted = 0";
                purchaseDetails = con.Query<PurchaseGRNDetailModel>(query).AsList();
            }

            return purchaseDetails;
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
                             "  ([PurchaseGRNNumber] " +
                             "  ,[InventoryType]  " +
                             "  ,[SupplierId]     " +
                             "  ,[StoreId]        " +
                             "  ,[EmployeeId]        " +
                             "  ,[PurchaseGRNDate]   " +
                             "  ,[GrossAmount]    " +
                             "  ,[TaxAmount]      " +
                             "  ,[GrandTotal]     " +
                             "  ,[PaidAmount]     " +
                             "  ,[DueAmount]      " +
                             "  ,[Notes]          " +
                             "  ,[UserIdUpdated]  " +
                             "  ,[DateInserted]   " +
                             "  ,[IsDeleted])     " +
                             "   VALUES           " +
                             "  (@ReferenceNo,  " +
                             "   @InventoryType,      " +
                             "   @SupplierId,      " +
                             "   @StoreId,         " +
                             "   @EmployeeId,         " +
                             "   @Date,    " +
                             "   @GrandTotal,     " +
                             "   0,       " +
                             "   @GrandTotal,      " +
                             "   @Paid,      " +
                             "   @Due,       " +
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
                                    LoginInfo.Userid + ",0); SELECT CAST(PurchaseGRNNumber as INT) from PurchaseGRN where id = " + result + "; ";

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
                             "  ,[InventoryType]  = @InventoryType" +
                             "  ,[PurchaseGRNDate] = @Date  " +
                             "  ,[GrossAmount]  =  @GrandTotal  " +
                             "  ,[GrandTotal]  = @GrandTotal   " +
                             "  ,[PaidAmount] = @Paid    " +
                             "  ,[DueAmount] =  @Due    " +
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
                                      LoginInfo.Userid + ",0); SELECT CAST(PurchaseGRNNumber as INT) from PurchaseGRN where id = " + result + "; ";
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
                var query = $"SELECT RIGHT('000000' + CONVERT(VARCHAR(8),ISNULL(MAX(PurchasNumber),0) + 1), 6) FROM purchase where InventoryType=1;";
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
                var query = "select top 1 UnitPrice from PurchaseDetail where FoodMenuId=" + foodMenuId + " order by id desc";
                return con.ExecuteScalar<decimal>(query, null, sqltrans, 0, System.Data.CommandType.Text);
            }
        }
    }
}
