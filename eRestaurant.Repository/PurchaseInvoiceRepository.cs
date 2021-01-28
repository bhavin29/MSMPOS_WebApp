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
                var query = "select PurchaseInvoice.Id as Id, PurchaseInvoice.ReferenceNumber as ReferenceNo, convert(varchar(12),PurchaseInvoiceDate, 3) as [PurchaseInvoiceDate],Supplier.SupplierName," +
                    "PurchaseInvoice.GrossAmount,PurchaseInvoice.TaxAmount,PurchaseInvoice.TotalAmount " +
                    "from PurchaseInvoice inner join Supplier on PurchaseInvoice.SupplierId = Supplier.Id where PurchaseInvoice.InventoryType=2 And PurchaseInvoice.Isdeleted = 0 order by PurchaseInvoiceDate, PurchaseId desc";
                purchaseViewModelList = con.Query<PurchaseInvoiceViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }

        public List<PurchaseInvoiceModel> GetPurchaseInvoiceById(long purchaseId)
        {
            List<PurchaseInvoiceModel> purchaseModelList = new List<PurchaseInvoiceModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseInvoice.Id as Id, PurchaseInvoice.StoreId,PurchaseInvoice.EmployeeId,PurchaseId as ReferenceNo,PurchaseInvoiceDate as [Date],Supplier.SupplierName, Supplier.Id as SupplierId," +
                      " PurchaseInvoice.[GrossAmount] as GrandTotal,PurchaseInvoice.DueAmount as Due,PurchaseInvoice.PaidAmount as Paid,PurchaseInvoice.DeliveryNoteNumber,PurchaseInvoice.DeliveryDate,PurchaseInvoice.DriverName,PurchaseInvoice.VehicleNumber,PurchaseInvoice.Notes " +
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
                             " ,[PurchaseId] " +
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
                             " ,@PurchaseId " +
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
                                             " ,[InvoiceQty] " +
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
                                    LoginInfo.Userid + ",0); SELECT CAST(ReferenceNumber as INT) from PurchaseInvoice where id = " + result + "; ";
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
                            ",PurchaseId = @PurchaseId " +
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
                                              ",[InvoiceQty]                 = " + item.InvoiceQty +
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
                                              ",[InvoiceQty] " +
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
                var query = " SELECT PG.[PurchaseInvoiceId],PG.[FoodMenuId],PG.[IngredientId],PG.[POQty],PG.[InvoiceQty],PG.[UnitPrice],PG.[GrossAmount],PG.[DiscountPercentage],PG.[DiscountAmount],PG.[TaxAmount],PG.[TotalAmount], " +
                         " IngredientName" +
                         " FROM PurchaseInvoice PG INNER JOIN PURCHASEInvoiceDetail PGD" +
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
                var query = $"SELECT RIGHT('' + CONVERT(VARCHAR(8),ISNULL(MAX(PurchaseId),0) + 1), 6) FROM PurchaseInvoice where InventoryType=2;";
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
        public List<PurchaseInvoiceModel> GetPurchaseInvoiceFoodMenuById(long purchaseInvoiceId)
        {
            List<PurchaseInvoiceModel> purchaseModelList = new List<PurchaseInvoiceModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseInvoice.Id as Id, PurchaseInvoice.StoreId,PurchaseInvoice.EmployeeId,ReferenceNumber as ReferenceNo,PurchaseInvoiceDate ,Supplier.SupplierName, Supplier.Id as SupplierId," +
                      "PurchaseInvoice.GrossAmount, PurchaseInvoice.TaxAmount,PurchaseInvoice.TotalAmount,PurchaseInvoice.DueAmount as Due,PurchaseInvoice.PaidAmount as Paid,PurchaseInvoice.DeliveryNoteNumber ,PurchaseInvoice.DeliveryDate ,PurchaseInvoice.DriverName ,PurchaseInvoice.VehicleNumber,PurchaseInvoice.Notes " +
                      "from PurchaseInvoice inner join Supplier on PurchaseInvoice.SupplierId = Supplier.Id where PurchaseInvoice.InventoryType=1 And PurchaseInvoice.Isdeleted = 0 and PurchaseInvoice.Id = " + purchaseInvoiceId;
                purchaseModelList = con.Query<PurchaseInvoiceModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public List<PurchaseInvoiceViewModel> GetPurchaseInvoiceFoodMenuList()
        {
            List<PurchaseInvoiceViewModel> purchaseViewModelList = new List<PurchaseInvoiceViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseInvoice.Id as Id, PurchaseInvoice.ReferenceNumber as ReferenceNo, convert(varchar(12),PurchaseInvoiceDate, 3) as [Date],Supplier.SupplierName," +
                    "PurchaseInvoice.TotalAMount,PurchaseInvoice.DueAmount as Due " +
                    "from PurchaseInvoice inner join Supplier on PurchaseInvoice.SupplierId = Supplier.Id where PurchaseInvoice.InventoryType=1 And PurchaseInvoice.Isdeleted = 0 order by PurchaseInvoiceDate, PurchaseId desc";
                purchaseViewModelList = con.Query<PurchaseInvoiceViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }
        public List<PurchaseInvoiceDetailModel> GetPurchaseInvoiceFoodMenuDetails(long purchaseInvoiceId)
        {
            List<PurchaseInvoiceDetailModel> purchaseDetails = new List<PurchaseInvoiceDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select pin.Id as PurchaseInvoiceId,pin.FoodMenuId as FoodMenuId,f.FoodMenuName,pin.UnitPrice as UnitPrice, pin.POQty,PIN.InvoiceQty , pin.GrossAmount,pin.TaxAmount,pin.TotalAmount,pin.DiscountPercentage,pin.DiscountAmount " +
                            " from purchaseInvoice as P inner join PurchaseInvoiceDetail as PIN on P.id = pin.PurchaseInvoiceId " +
                            " inner join FoodMenu as f on pin.FoodMenuId = f.Id where P.id = " + purchaseInvoiceId + "and P.InventoryType=1 and pin.isdeleted = 0 and p.isdeleted = 0";
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
                             "  ([PurchaseId] " +
                             "  ,[ReferenceNumber] " +
                             "  ,[InventoryType]  " +
                             "  ,[SupplierId]     " +
                             "  ,[StoreId]        " +
                             "  ,[EmployeeId]        " +
                             "  ,[PurchaseInvoiceDate]   " +
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
                             "  ,[UserIdUpdated]  " +
                             "  ,[DateInserted]   " +
                             "  ,[IsDeleted] )     " +
                             "   VALUES           " +
                             "  (@PurchaseId,  " +
                             "  @ReferenceNo,  " +
                             "   @InventoryType,      " +
                             "   @SupplierId,      " +
                             "   @StoreId,         " +
                             "   @EmployeeId,         " +
                             "   @PurchaseInvoiceDate,    " +
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

                    foreach (var item in purchaseModel.purchaseInvoiceDetails)
                    {
                        var queryDetails = "INSERT INTO [dbo].[PurchaseInvoiceDetail]" +
                                             "  ([PurchaseInvoiceId] " +
                                             " ,[FoodMenuId] " +
                                             " ,[IngredientId] " +
                                             " ,[POQty] " +
                                             " ,[InvoiceQty] " +
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
                                              "null ," +
                                              item.POQTY + "," +
                                              item.InvoiceQty + "," +
                                              item.UnitPrice + "," +
                                              item.GrossAmount + "," +
                                              item.DiscountPercentage + "," +
                                              item.DiscountAmount + "," +
                                              item.TaxAmount + "," +
                                              item.TotalAmount + "," +
                                    LoginInfo.Userid + ",0); SELECT CAST(ReferenceNumber as INT) from PurchaseInvoice where id = " + result + "; ";

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
                                             "[PurchaseInvoiceId] = " + purchaseModel.Id +
                                              ",[FoodMenuId]  = " + item.FoodMenuId +
                                              ",[POQty]  = " + item.POQTY +
                                              ",[InvoiceQty]   = " + item.InvoiceQty +
                                              ",[UnitPrice]   = " + item.UnitPrice +
                                              ",[GrossAmount]   = " + item.GrossAmount +
                                              ",[DiscountPercentage] = " + item.DiscountPercentage +
                                              ",[DiscountAmount]  = " + item.DiscountAmount +
                                              ",[TaxAmount]   = " + item.TaxAmount +
                                              ",[TotalAmount]  = " + item.TotalAmount +
                                               " ,[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                                 " [DateUpdated] = GetUTCDate() " +
                                                 " where id = " + item.PurchaseInvoiceId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[PurchaseInvoiceDetail]" +
                                              "  ([PurchaseInvoiceId] " +
                                              " ,[FoodMenuId] " +
                                              " ,[IngredientId] " +
                                              " ,[POQty] " +
                                              " ,[InvoiceQty] " +
                                              " ,[UnitPrice] " +
                                              " ,[GrossAmount] " +
                                              " ,[DiscountPercentage]  " +
                                              " ,[DiscountAmount] " +
                                              " ,[TaxAmount] " +
                                              " ,[TotalAmount]  " +
                                              " ,[UserIdUpdated]" +
                                               " ,[IsDeleted])   " +
                                               "VALUES           " +
                                               "(" + purchaseModel.Id + "," +
                                               item.FoodMenuId + "," +
                                                 "null," +
                                               item.POQTY + "," +
                                               item.InvoiceQty + "," +
                                               item.UnitPrice + "," +
                                               item.GrossAmount + "," +
                                               item.DiscountPercentage + "," +
                                               item.DiscountAmount + "," +
                                               item.TaxAmount + "," +
                                               item.TotalAmount + "," +
                                     LoginInfo.Userid + ",0); SELECT SCOPE_IDENTITY() ";
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
                var query = $"SELECT RIGHT('' + CONVERT(VARCHAR(8),ISNULL(MAX(ReferenceNumber),0) + 1), 6) FROM purchaseInvoice where InventoryType=1 and ISDeleted=0;";
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
                //               var query = "select top 1 UnitPrice from PurchaseDetail where FoodMenuId=" + foodMenuId + " order by id desc";
                var query = " select top 1 UnitPrice from " +
                        " (select Id, '' as PDId,PurchasePrice as UnitPrice from foodmenu  where Id = " + foodMenuId +
                        " union " +
                        " select '' as Id, Id asPDId, UnitPrice from PurchaseDetail where FoodMenuId = " + foodMenuId + ") restuls " +
                        " order by PDid desc; ";
                return con.ExecuteScalar<decimal>(query, null, sqltrans, 0, System.Data.CommandType.Text);
            }
        }

        public List<PurchaseInvoiceModel> GetPurchaseInvoiceFoodMenuByPurchaseId(long purchaseId)
        {
            List<PurchaseInvoiceModel> purchaseModelList = new List<PurchaseInvoiceModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT 0 as Id,P.Id AS PurchaseId,P.StoreId,P.EmployeeId,P.ReferenceNo,GETDATE() AS PurchaseInvoiceDate,Supplier.SupplierName, Supplier.Id as SupplierId,P.GrossAmount,P.TaxAmount,P.GrandTotal As TotalAmount, "+
                            "P.DueAmount as Due,P.PaidAmount as Paid,null AS DeliveryNoteNumber,null as DeliveryDate,null as DriverName,null as VehicleNumber,P.Notes FROM Purchase P inner join Supplier on P.SupplierId = Supplier.Id " +
                            "Where P.InventoryType = 1 And P.Isdeleted = 0 And P.Id = " + purchaseId;
                purchaseModelList = con.Query<PurchaseInvoiceModel>(query).AsList();
            }
            return purchaseModelList;
        }

        public List<PurchaseInvoiceDetailModel> GetPurchaseInvoiceFoodMenuDetailsPurchaseId(long purchaseId)
        {
            List<PurchaseInvoiceDetailModel> purchaseDetails = new List<PurchaseInvoiceDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select 0 AS PurchaseInvoiceId,PD.FoodMenuId,f.FoodMenuName,pd.UnitPrice,PD.Qty AS POQty,PD.Qty AS InvoiceQty,PD.GrossAmount,PD.TaxAmount,PD.TotalAmount,PD.DiscountPercentage,PD.DiscountAmount "+
                            "From Purchase P Inner join PurchaseDetail PD On P.Id = PD.PurchaseId inner join FoodMenu as f on PD.FoodMenuId = f.Id "+
                            "Where P.InventoryType = 1 and P.isdeleted = 0 and pd.isdeleted = 0 And P.Id = "+ purchaseId;
                purchaseDetails = con.Query<PurchaseInvoiceDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public int GetPurchaseIdByPOReference(string poReference)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select Id from Purchase Where ReferenceNo='" + poReference + "'";
                return con.QueryFirstOrDefault<int>(query);
            }
        }
    }
}
