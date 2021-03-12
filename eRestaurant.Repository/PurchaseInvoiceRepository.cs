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
                      " ,[DeliveryNoteNumber], [DeliveryDate],[DriverName],[VehicleNumber], PurchaseInvoice.VatableAmount,PurchaseInvoice.NonVatableAmount  " +
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
                             " ,@PurchaseInvoiceDate " +
                             " ,@SupplierId " +
                             " ,@StoreId " +
                             " ,@EmployeeId " +
                             " ,@GrossAmount " +
                             " ,@TaxAmount " +
                             " ,@TotalAmount " +
                            "   @VatableAmount,      " +
                             "   @NonVatableAmount,      " +
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
                                            "  ,[VatableAmount]      " +
                                             "  ,[NonVatableAmount]      " +
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
                                              item.VatableAmount + "," +
                                              item.NonVatableAmount + "," +
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
                            ",[VatableAmount] = @VatableAmount     " +
                           " ,[NonVatableAmount] = @NonVatableAmount      " +
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
                                             " [VatableAmount] = " + item.VatableAmount + "," +
                                             " [NonVatableAmount] = " + item.NonVatableAmount + "," +
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
                                            "  ,[VatableAmount]      " +
                                             "  ,[NonVatableAmount]      " +
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
        public int DeletePurchaseInvoice(long purchaseInvoiceId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update PurchaseInvoice set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where id = " + purchaseInvoiceId + ";" +
                    " Update PurchaseInvoiceDetail set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where PurchaseInvoiceId = " + purchaseInvoiceId + ";";
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
                         " IngredientName, PG.VatableAmount,PG.NonVatableAmount " +
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
                      "PurchaseInvoice.GrossAmount, PurchaseInvoice.TaxAmount,PurchaseInvoice.TotalAmount, PurchaseInvoice.VatableAmount,PurchaseInvoice.NonVatableAmount , PurchaseInvoice.DueAmount as Due,PurchaseInvoice.PaidAmount as Paid,PurchaseInvoice.DeliveryNoteNumber ,PurchaseInvoice.DeliveryDate ,PurchaseInvoice.DriverName ,PurchaseInvoice.VehicleNumber,PurchaseInvoice.Notes " +
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
                    "PurchaseInvoice.TotalAMount AS GrandTotal,PurchaseInvoice.DueAmount as Due,isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as Username, S.Storename  " +
                    ", PurchaseInvoice.VatableAmount,PurchaseInvoice.NonVatableAmount  from PurchaseInvoice inner join Supplier on PurchaseInvoice.SupplierId = Supplier.Id inner join [User] U on U.Id=PurchaseInvoice.UserIdInserted  inner join employee e on e.id = u.employeeid   inner join store S on S.Id = PurchaseInvoice.StoreId  where PurchaseInvoice.InventoryType=1 And PurchaseInvoice.Isdeleted = 0 order by PurchaseInvoiceDate, PurchaseId desc";
                purchaseViewModelList = con.Query<PurchaseInvoiceViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }
        public List<PurchaseInvoiceDetailModel> GetPurchaseInvoiceFoodMenuDetails(long purchaseInvoiceId)
        {
            List<PurchaseInvoiceDetailModel> purchaseDetails = new List<PurchaseInvoiceDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select pin.Id as PurchaseInvoiceId," +
             " (case when pin.FoodMenuId is null then 1 else 0 end) as ItemType, " +
             " (case when pin.FoodMenuId is null then pin.IngredientId else pin.FoodMenuId end) as FoodMenuId, " +
             " (case when pin.FoodMenuId is null then I.Ingredientname else f.FoodMenuName end) as FoodMenuName, " +
             " pin.UnitPrice as UnitPrice, pin.POQty,PIN.InvoiceQty , pin.GrossAmount,pin.TaxAmount,pin.TotalAmount,pin.DiscountPercentage,pin.DiscountAmount " +
             " , pin.VatableAmount,pin.NonVatableAmount  from purchaseInvoice as P inner join PurchaseInvoiceDetail as PIN on P.id = pin.PurchaseInvoiceId " +
             " left join FoodMenu as f on pin.FoodMenuId = f.Id " +
             " left join Ingredient as I on pin.IngredientId = I.Id " +
             " where P.id = " + purchaseInvoiceId + " and pin.isdeleted = 0 and p.isdeleted = 0";

                purchaseDetails = con.Query<PurchaseInvoiceDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }
        public List<PurchaseInvoiceViewModel> PurchaseInvoiceFoodMenuListByDate(string fromDate, string toDate, int supplierId, int storeId)
        {
            List<PurchaseInvoiceViewModel> purchaseViewModels = new List<PurchaseInvoiceViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PurchaseInvoice.Id as Id, PurchaseInvoice.ReferenceNumber as ReferenceNo, convert(varchar(12),PurchaseInvoiceDate, 3) as [Date],Supplier.SupplierName," +
                    "PurchaseInvoice.TotalAMount AS GrandTotal,PurchaseInvoice.DueAmount as Due,isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as Username, S.Storename " +
                    "from PurchaseInvoice inner join Supplier on PurchaseInvoice.SupplierId = Supplier.Id inner join [User] U on U.Id=PurchaseInvoice.UserIdInserted inner join employee e on e.id = u.employeeid  inner join store S on S.Id = PurchaseInvoice.StoreId where PurchaseInvoice.InventoryType=1 And PurchaseInvoice.Isdeleted = 0 " +
                    // " AND Convert(varchar(10), PurchaseInvoiceDate, 103)  between '" + fromDate + "' and '" + toDate + "'";
                    " AND Convert(Date, PurchaseInvoiceDate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103)  ";

                if (supplierId != 0)
                {
                    query += " And PurchaseInvoice.SupplierId= " + supplierId;
                }
                if (storeId != 0)
                {
                    query += " And PurchaseInvoice.StoreId= " + storeId;
                }
                query += "   order by PurchaseInvoice.PurchaseInvoiceDate, PurchaseInvoice.id desc";

                purchaseViewModels = con.Query<PurchaseInvoiceViewModel>(query).AsList();
            }

            return purchaseViewModels;
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
                             "   @PurchaseInvoiceDate,    " +
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

                    foreach (var item in purchaseModel.purchaseInvoiceDetails)
                    {
                        var queryDetails = "INSERT INTO [dbo].[PurchaseInvoiceDetail]" +
                                             "  ([PurchaseInvoiceId] " +
                                             " ,[FoodMenuId] " +
                                             " ,[IngredientId] " +
                                             " ,[AssetItemId] " +
                                             " ,[POQty] " +
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
                        queryDetails = queryDetails + "" + item.POQTY + "," +
                              item.InvoiceQty + "," +
                              item.UnitPrice + "," +
                              item.GrossAmount + "," +
                              item.DiscountPercentage + "," +
                              item.DiscountAmount + "," +
                              item.TaxAmount + "," +
                              item.TotalAmount + "," +
                              item.VatableAmount + "," +
                              item.NonVatableAmount + "," +
                    LoginInfo.Userid + ",GetUtcDate(),0); SELECT CAST(ReferenceNumber as INT) from PurchaseInvoice where id = " + result + "; ";

                        detailResult = con.ExecuteScalar<int>(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (detailResult > 0)
                    {
                        sqltrans.Commit();

                        int outResult = 0;
                        if (purchaseModel.PurchaseId > 0)
                            outResult = UpdatePurchaseOrderId(purchaseModel.PurchaseId);

                        if (purchaseModel.PurchaseId == 0)
                        {
                            CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                            string sResult = commonRepository.InventoryPush("PI", result);
                        }
                        else if (purchaseModel.PurchaseId > 0 && purchaseModel.PurchaseStatus != 4)
                        {
                            CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                            string sResult = commonRepository.InventoryPush("PI", result);
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
                             "  ,[VatableAmount] = @VatableAmount     " +
                              "  ,[NonVatableAmount] = @NonVatableAmount      " +
                              " ,[DeliveryNoteNumber] =@DeliveryNoteNumber " +
                              " ,[PurchaseInvoiceDate] =@PurchaseInvoiceDate " +
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
                                             "[PurchaseInvoiceId] = " + purchaseModel.Id;
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
                                              ",[InvoiceQty]   = " + item.InvoiceQty +
                                              ",[UnitPrice]   = " + item.UnitPrice +
                                              ",[GrossAmount]   = " + item.GrossAmount +
                                              ",[DiscountPercentage] = " + item.DiscountPercentage +
                                              ",[DiscountAmount]  = " + item.DiscountAmount +
                                              ",[TaxAmount]   = " + item.TaxAmount +
                                              ",[TotalAmount]  = " + item.TotalAmount +
                                             " [VatableAmount] = " + item.VatableAmount + "," +
                                             " [NonVatableAmount] = " + item.NonVatableAmount + "," +
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
                                               " ,[AssetItemId] " +
                                              " ,[POQty] " +
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
                            queryDetails = queryDetails + item.POQTY + "," +
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
                var query = $"SELECT ISNULL(MAX(convert(int,ReferenceNumber)),0) + 1  FROM purchaseInvoice where InventoryType=1 and ISDeleted=0;";
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
                //               var query = "select top 1 UnitPrice from PurchaseDetail where FoodMenuId=" + foodMenuId + " order by id desc";
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
        public List<PurchaseInvoiceModel> GetPurchaseInvoiceFoodMenuByPurchaseId(long purchaseId)
        {
            List<PurchaseInvoiceModel> purchaseModelList = new List<PurchaseInvoiceModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT 0 as Id,P.Id AS PurchaseId,P.StoreId,P.EmployeeId,P.ReferenceNo,P.PurchaseDate AS PurchaseInvoiceDate,Supplier.SupplierName, Supplier.Id as SupplierId,P.GrossAmount,P.TaxAmount,P.GrandTotal As TotalAmount, " +
                            "P.DueAmount as Due,P.PaidAmount as Paid,null AS DeliveryNoteNumber,GETDATE() as DeliveryDate,null as DriverName,null as VehicleNumber,P.Notes,P.Status as PurchaseStatus, p.VatableAmount,p.NonVatableAmount  FROM Purchase P inner join Supplier on P.SupplierId = Supplier.Id " +
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
                var query = " Select 0 AS PurchaseInvoiceId," +
                             " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then 2 else 1 end) else 0 end) as ItemType, " +
                             " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then PD.AssetItemId else PD.IngredientId end) else PD.FoodMenuId end) as FoodMenuId, " +
                             " (case when PD.FoodMenuId is null then (case when PD.IngredientId is null then AI.AssetItemName else I.IngredientName end) else f.FoodMenuName end) as FoodMenuName,  " +
                            " pd.UnitPrice,PD.Qty AS POQty,PD.Qty AS InvoiceQty,PD.GrossAmount,PD.TaxAmount,PD.TotalAmount,PD.DiscountPercentage,PD.DiscountAmount " +
                            " , pd.VatableAmount,pd.NonVatableAmount From Purchase P Inner join PurchaseDetail PD On P.Id = PD.PurchaseId " +
                             " left join FoodMenu as f on PD.FoodMenuId = f.Id " +
                             " left join Ingredient as I on PD.IngredientId = I.Id " +
                             "   left join AssetItem as AI on PD.AssetItemId = AI.Id  "+
                            " Where P.isdeleted = 0 and pd.isdeleted = 0 And P.Id = " + purchaseId;
                purchaseDetails = con.Query<PurchaseInvoiceDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public int GetPurchaseIdByPOReference(string poReference)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select Id from Purchase Where IsDeleted=0 and Status!=5 and ReferenceNo='" + poReference + "'";
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
                var query = $"update Purchase set Status = 5 where id = " + purchaseId;
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

        public List<PurchaseInvoiceModel> GetViewPurchaseInvoiceFoodMenuById(long purchaseInvoiceId)
        {
            List<PurchaseInvoiceModel> purchaseModelList = new List<PurchaseInvoiceModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select P.Referenceno as POReferenceNo, P.PurchaseDate as PODate, PurchaseInvoice.Id as Id, PurchaseInvoice.StoreId,PurchaseInvoice.EmployeeId,ReferenceNumber as ReferenceNo,PurchaseInvoiceDate ,Supplier.SupplierName, Supplier.Id as SupplierId," +
                      "PurchaseInvoice.GrossAmount, PurchaseInvoice.TaxAmount,PurchaseInvoice.TotalAmount,PurchaseInvoice.DueAmount as Due,PurchaseInvoice.PaidAmount as Paid,PurchaseInvoice.DeliveryNoteNumber ,PurchaseInvoice.DeliveryDate ,PurchaseInvoice.DriverName ,PurchaseInvoice.VehicleNumber,PurchaseInvoice.Notes,S.StoreName " +
                      ", PurchaseInvoice.VatableAmount,PurchaseInvoice.NonVatableAmount  from PurchaseInvoice left JOIN PURCHASE P on P.Id = PurchaseInvoice.Purchaseid inner join Supplier on PurchaseInvoice.SupplierId = Supplier.Id Inner Join  Store S On S.Id=PurchaseInvoice.StoreId where PurchaseInvoice.InventoryType=1 And PurchaseInvoice.Isdeleted = 0 and PurchaseInvoice.Id = " + purchaseInvoiceId;
                purchaseModelList = con.Query<PurchaseInvoiceModel>(query).AsList();
            }
            return purchaseModelList;
        }

        public List<PurchaseInvoiceDetailModel> GetViewPurchaseInvoiceFoodMenuDetails(long purchaseInvoiceId)
        {
            List<PurchaseInvoiceDetailModel> purchaseDetails = new List<PurchaseInvoiceDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select pin.Id as PurchaseInvoiceId," +
                            " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then 2 else 1 end) else 0 end) as ItemType, " +
                            " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then PIN.AssetItemId else pin.IngredientId end) else pin.FoodMenuId end) as FoodMenuId, " +
                            " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then AI.AssetItemName else I.IngredientName end) else f.FoodMenuName end) as FoodMenuName,  " +
                            " pin.UnitPrice as UnitPrice, pin.POQty,PIN.InvoiceQty , pin.GrossAmount,pin.TaxAmount,pin.TotalAmount,pin.DiscountPercentage,pin.DiscountAmount, " +
                            "  (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then UA.UnitName else UI.UnitName end) else UF.UnitName end) as UnitName    " +
                            " , pin.VatableAmount,pin.NonVatableAmount from purchaseInvoice as P inner join PurchaseInvoiceDetail as PIN on P.id = pin.PurchaseInvoiceId " +
                            " left join FoodMenu as f on pin.FoodMenuId = f.Id " +
                            " left join Ingredient as I on pin.IngredientId = I.Id " +
                            "   left join AssetItem as AI on PIN.AssetItemId = AI.Id  "+
                            " left join Units As UI On UI.Id = I.IngredientUnitId " +
                            " left join Units As UF On UF.Id = F.UnitsId " +
                            "   left join Units As UA On UA.Id = AI.UnitId "+
                            " where P.id = " + purchaseInvoiceId + " and pin.isdeleted = 0 and p.isdeleted = 0";

                purchaseDetails = con.Query<PurchaseInvoiceDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }
    }
}
