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
    public class PurchaseInvoiceRepository : IPurchaseInvoiceRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public PurchaseInvoiceRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<PurchaseInvoiceViewModel> GetPurchaseInvoiceList()
        {
            List<PurchaseInvoiceViewModel> purchaseViewModelList = new List<PurchaseInvoiceViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseInvoice.Id as Id, PurchaseInvoiceNumber as ReferenceNo, convert(varchar(12),PurchaseInvoiceDate, 3) as [Date],Supplier.SupplierName," +
                    "PurchaseInvoice.[GrossAmount] as GrandTotal,PurchaseInvoice.DueAmount as Due " +
                    "from PurchaseInvoice inner join Supplier on PurchaseInvoice.SupplierId = Supplier.Id where PurchaseInvoice.InventoryType=2 And PurchaseInvoice.Isdeleted = 0 order by PurchaseInvoiceDate, purchaseNumber desc";
                purchaseViewModelList = con.Query<PurchaseInvoiceViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }

        public List<PurchaseInvoiceModel> GetPurchaseInvoiceById(long purchaseId)
        {
            List<PurchaseInvoiceModel> purchaseModelList = new List<PurchaseInvoiceModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseInvoice.Id as Id, PurchaseInvoice.StoreId,PurchaseInvoice.EmployeeId,PurchaseNumber as ReferenceNo,PurchaseInvoiceDate as [Date],Supplier.SupplierName, Supplier.Id as SupplierId," +
                      " PurchaseInvoice.[GrossAmount] as GrandTotal,PurchaseInvoice.DueAmount as Due,PurchaseInvoice.PaidAmount as Paid,PurchaseInvoice.Notes " +
                      " ,[DeliveryNoteNumber], [DeliveryDate],[DriverName],[VehicleNumber] " +
                      " from PurchaseInvoice inner join Supplier on PurchaseInvoice.SupplierId = Supplier.Id where PurchaseInvoice.InventoryType=2 And PurchaseInvoice.Isdeleted = 0 and PurchaseInvoice.Id = " + purchaseId;
                purchaseModelList = con.Query<PurchaseInvoiceModel>(query).AsList();
            }
            return purchaseModelList;
        }

        public int InsertPurchaseInvoice(PurchaseInvoiceModel purchaseInvoiceModel)
        {
            int result = 0;
            int detailResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[PurchaseInvoice] " +
                             " [InventoryType] " +
                             " ,[PurchaseOrderId] " +
                             " ,[ReferenceNumber] " +
                             " ,[PurchaseNumber] " +
                             " ,[PurchaseInvoiceDate] " +
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
                             " ,@PurchaseInvoiceDate " +
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
                result = con.ExecuteScalar<int>(query, purchaseInvoiceModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    foreach (var item in purchaseInvoiceModel.purchaseInvoiceDetails)
                    {
                        var queryDetails = "INSERT INTO [dbo].[PurchaseInvoiceDetail]" +
                                             "  ,[PurchaseInvoiceId] " +
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
                                              item.InvoiceQty + "," +
                                              item.UnitPrice + "," +
                                              item.GrossAmount + "," +
                                              item.DiscountPercentage + "," +
                                              item.DiscountAmount + "," +
                                              item.TaxAmount + "," +
                                              item.TotalAmount + "," +
                                    LoginInfo.Userid + ",0); SELECT CAST(PurchaseInvoiceNumber as INT) from PurchaseInvoice where id = " + result + "; ";
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
        public int UpdatePurchaseInvoice(PurchaseInvoiceModel purchaseInvoiceModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update [dbo].[PurchaseInvoice] set " +
                            " InventoryType = @InventoryType " +
                            ",ReferenceNumber = @ReferenceN0 " +
                            ",PurchaseNumber = @PurchaseNumber " +
                            ",PurchaseInvoiceDate = @PurchaseInvoiceDate " +
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
                            "  ,[DateUpdated]  = GetUtcDate()  where id= " + purchaseInvoiceModel.Id + ";";
                result = con.Execute(query, purchaseInvoiceModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    if (purchaseInvoiceModel.DeletedId != null)
                    {
                        foreach (var item in purchaseInvoiceModel.DeletedId)
                        {
                            var deleteQuery = $"update PurchaseInvoiceDetail set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in purchaseInvoiceModel.purchaseInvoiceDetails)
                    {
                        var queryDetails = string.Empty;
                        if (item.PurchaseInvoiceId > 0)
                        {
                            queryDetails = "Update [dbo].[PurchaseInvoiceDetail] set " +
                                             ",[PurchaseInvoiceId]		  	 = " + item.PurchaseInvoiceId +
                                              ",[FoodMenuId]             = " + item.FoodMenuId +
                                              ",[IngredientId]           = " + item.IngredientId +
                                              ",[POQty]                  = " + item.POQTY +
                                              ",[GRNQty]                 = " + item.InvoiceQty +
                                              ",[UnitPrice]              = " + item.UnitPrice +
                                              ",[GrossAmount]            = " + item.GrossAmount +
                                              ",[DiscountPercentage]     = " + item.DiscountPercentage +
                                              ",[DiscountAmount]         = " + item.DiscountAmount +
                                              ",[TaxAmount]              = " + item.TaxAmount +
                                              ",[TotalAmount]            = " + item.TotalAmount +
                                               " ,[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                                 " [DateUpdated] = GetUTCDate() " +
                                                 " where id = " + item.PurchaseInvoiceId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[PurchaseInvoiceDetail]" +
                                              "[PurchaseInvoiceId] " +
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
                                              "(" + purchaseInvoiceModel.Id + "," +
                                              item.FoodMenuId + "," +
                                              item.IngredientId + "," +
                                              item.POQTY + "," +
                                              item.InvoiceQty + "," +
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
        public int DeletePurchaseInvoice(long purchaseInvoiceId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update PurchaseInvoice set IsDeleted = 1 where id = " + purchaseInvoiceId + ";" +
                    " Update PurchaseInvoiceDetail set IsDeleted = 1 where PurchaseInvoiceId = " + purchaseInvoiceId + ";";
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
        public List<PurchaseInvoiceDetailModel> GetPurchaseInvoiceDetails(long purchaseId)
        {
            List<PurchaseInvoiceDetailModel> purchaseDetails = new List<PurchaseInvoiceDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " SELECT PG.[PurchaseInvoiceId],PG.[FoodMenuId],PG.[IngredientId],PG.[POQty],PG.[GRNQty],PG.[UnitPrice],PG.[GrossAmount],PG.[DiscountPercentage],PG.[DiscountAmount],PG.[TaxAmount],PG.[TotalAmount], " +
                         " IngredientName" +
                         " FROM PurchaseInvoice PG INNER JOIN PURCHASEGRNDetail PGD" +
                         " inner join Ingredient as i on pin.IngredientId = i.Id" +
                         " INNER JOIN FOODMENU FM ON FM.ID = PGD.FOODMENUID" +
                         " WHERE PG.id = " + purchaseId + " AND PG.InventoryType = 2 and PGD.isdeleted = 0 and PG.isdeleted = 0";
                purchaseDetails = con.Query<PurchaseInvoiceDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }
        public int DeletePurchaseInvoiceDetails(long purchaseDetailsId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update PurchaseInvoiceDetail set IsDeleted = 1 where id = " + purchaseDetailsId + ";";
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
                var query = $"SELECT RIGHT('000000' + CONVERT(VARCHAR(8),ISNULL(MAX(PurchaseNumber),0) + 1), 6) FROM PurchaseInvoice where InventoryType=2;";
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
        public List<PurchaseInvoiceModel> GetPurchaseInvoiceFoodMenuById(long purchaseId)
        {
            List<PurchaseInvoiceModel> purchaseModelList = new List<PurchaseInvoiceModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseInvoice.Id as Id, PurchaseInvoice.StoreId,PurchaseInvoice.EmployeeId,PurchaseInvoiceNumber as ReferenceNo,PurchaseInvoiceDate as [Date],Supplier.SupplierName, Supplier.Id as SupplierId," +
                      "PurchaseInvoice.GrandTotal as GrandTotal,PurchaseInvoice.DueAmount as Due,PurchaseInvoice.PaidAmount as Paid,PurchaseInvoice.Notes " +
                      "from PurchaseInvoice inner join Supplier on PurchaseInvoice.SupplierId = Supplier.Id where PurchaseInvoice.InventoryType=1 And PurchaseInvoice.Isdeleted = 0 and PurchaseInvoice.Id = " + purchaseId;
                purchaseModelList = con.Query<PurchaseInvoiceModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public List<PurchaseInvoiceViewModel> GetPurchaseInvoiceFoodMenuList()
        {
            List<PurchaseInvoiceViewModel> purchaseViewModelList = new List<PurchaseInvoiceViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseInvoice.Id as Id, PurchaseNumber as ReferenceNo, convert(varchar(12),PurchaseInvoiceDate, 3) as [Date],Supplier.SupplierName," +
                    "PurchaseInvoice.GrossAmount as GrandTotal,PurchaseInvoice.DueAmount as Due " +
                    "from PurchaseInvoice inner join Supplier on PurchaseInvoice.SupplierId = Supplier.Id where PurchaseInvoice.InventoryType=1 And PurchaseInvoice.Isdeleted = 0 order by PurchaseInvoiceDate, purchaseNumber desc";
                purchaseViewModelList = con.Query<PurchaseInvoiceViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }
        public List<PurchaseInvoiceDetailModel> GetPurchaseInvoiceFoodMenuDetails(long purchaseId)
        {
            List<PurchaseInvoiceDetailModel> purchaseDetails = new List<PurchaseInvoiceDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select pin.Id as PurchaseInvoiceId,pin.FoodMenuId as FoodMenuId,f.FoodMenuName,pin.UnitPrice as UnitPrice, pin.Qty as Quantity, pin.GrossAmount as Total " +
                            "from purchase as P inner join PurchaseInvoiceIngredient as PIN on P.id = pin.PurchaseInvoiceId " +
                            "inner join FoodMenu as f on pin.FoodMenuId = f.Id where P.id = " + purchaseId + "and P.InventoryType=1 and pin.isdeleted = 0 and p.isdeleted = 0";
                purchaseDetails = con.Query<PurchaseInvoiceDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }
        public int InsertPurchaseInvoiceFoodMenu(PurchaseInvoiceModel purchaseModel)
        {
            int result = 0;
            int detailResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[PurchaseInvoice] " +
                             "  ([PurchaseInvoiceNumber] " +
                             "  ,[InventoryType]  " +
                             "  ,[SupplierId]     " +
                             "  ,[StoreId]        " +
                             "  ,[EmployeeId]        " +
                             "  ,[PurchaseInvoiceDate]   " +
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

                    foreach (var item in purchaseModel.purchaseInvoiceDetails)
                    {
                        var queryDetails = "INSERT INTO [dbo].[PurchaseInvoiceDetail]" +
                                             "  ,[PurchaseInvoiceId] " +
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
                                              item.InvoiceQty + "," +
                                              item.UnitPrice + "," +
                                              item.GrossAmount + "," +
                                              item.DiscountPercentage + "," +
                                              item.DiscountAmount + "," +
                                              item.TaxAmount + "," +
                                              item.TotalAmount + "," +
                                    LoginInfo.Userid + ",0); SELECT CAST(PurchaseInvoiceNumber as INT) from PurchaseInvoice where id = " + result + "; ";

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

        public int UpdatePurchaseInvoiceFoodMenu(PurchaseInvoiceModel purchaseModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update [dbo].[PurchaseInvoice] set " +
                             "   [SupplierId]  = @SupplierId" +
                             "  ,[StoreId]  = @StoreId" +
                             "  ,[EmployeeId]  = @EmployeeId" +
                             "  ,[InventoryType]  = @InventoryType" +
                             "  ,[PurchaseInvoiceDate] = @Date  " +
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
                            var deleteQuery = $"update PurchaseInvoiceDetail set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in purchaseModel.purchaseInvoiceDetails)
                    {
                        var queryDetails = string.Empty;
                        if (item.PurchaseInvoiceId > 0)
                        {
                            queryDetails = "Update [dbo].[PurchaseInvoiceDetail] set " +
                                             ",[PurchaseInvoiceId]		  	 = " + item.PurchaseInvoiceId +
                                              ",[FoodMenuId]             = " + item.FoodMenuId +
                                              ",[IngredientId]           = " + item.IngredientId +
                                              ",[POQty]                  = " + item.POQTY +
                                              ",[GRNQty]                 = " + item.InvoiceQty +
                                              ",[UnitPrice]              = " + item.UnitPrice +
                                              ",[GrossAmount]            = " + item.GrossAmount +
                                              ",[DiscountPercentage]     = " + item.DiscountPercentage +
                                              ",[DiscountAmount]         = " + item.DiscountAmount +
                                              ",[TaxAmount]              = " + item.TaxAmount +
                                              ",[TotalAmount]            = " + item.TotalAmount +
                                               " ,[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                                 " [DateUpdated] = GetUTCDate() " +
                                                 " where id = " + item.PurchaseInvoiceId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[PurchaseInvoiceDetail]" +
                                              "  ,[PurchaseInvoiceId] " +
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
                                               item.InvoiceQty + "," +
                                               item.UnitPrice + "," +
                                               item.GrossAmount + "," +
                                               item.DiscountPercentage + "," +
                                               item.DiscountAmount + "," +
                                               item.TaxAmount + "," +
                                               item.TotalAmount + "," +
                                     LoginInfo.Userid + ",0); SELECT CAST(PurchaseInvoiceNumber as INT) from PurchaseInvoice where id = " + result + "; ";
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
