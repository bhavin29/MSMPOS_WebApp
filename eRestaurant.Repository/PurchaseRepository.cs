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
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public PurchaseRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<PurchaseViewModel> GetPurchaseList()
        {
            List<PurchaseViewModel> purchaseViewModelList = new List<PurchaseViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Purchase.Id as Id, PurchaseId as ReferenceNo, convert(varchar(12),PurchaseDate, 3) as [Date],Supplier.SupplierName," +
                    "Purchase.GrandTotal as GrandTotal,Purchase.DueAmount as Due " +
                    "from Purchase inner join Supplier on Purchase.SupplierId = Supplier.Id where Purchase.InventoryType=2 And Purchase.Isdeleted = 0 order by PurchaseDate, PurchaseId desc";
                purchaseViewModelList = con.Query<PurchaseViewModel>(query).AsList();
            }


            return purchaseViewModelList;
        }

        public List<PurchaseModel> GetPurchaseById(long purchaseId)
        {
            List<PurchaseModel> purchaseModelList = new List<PurchaseModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Purchase.Id as Id, Purchase.StoreId,Purchase.EmployeeId,PurchaseId as ReferenceNo,PurchaseDate as [Date],Supplier.SupplierName, Supplier.Id as SupplierId," +
                      "Purchase.GrandTotal as GrandTotal,Purchase.DueAmount as Due,Purchase.PaidAmount as Paid,Purchase.Notes " +
                      "from Purchase inner join Supplier on Purchase.SupplierId = Supplier.Id where Purchase.InventoryType=2 And Purchase.Isdeleted = 0 and Purchase.Id = " + purchaseId;
                purchaseModelList = con.Query<PurchaseModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public int InsertPurchase(PurchaseModel purchaseModel)
        {
            int result = 0;
            int detailResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[Purchase] " +
                             "  ([PurchaseId] " +
                             "  ,[InventoryType]  " +
                             "  ,[SupplierId]     " +
                             "  ,[StoreId]        " +
                             "  ,[EmployeeId]        " +
                             "  ,[PurchaseDate]   " +
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

                    foreach (var item in purchaseModel.PurchaseDetails)
                    {
                        var queryDetails = "INSERT INTO [dbo].[PurchaseDetail]" +
                                              " ([PurchaseId]   " +
                                              " ,[IngredientId] " +
                                              " ,[UnitPrice]    " +
                                              " ,[Qty]          " +
                                              " ,[GrossAmount]  " +
                                              " ,[TaxAmount]    " +
                                              " ,[TotalAmount]  " +
                                              " ,[UserIdUpdated]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              "" + item.IngredientId + "," +
                                              "" + item.UnitPrice + "," +
                                              "" + item.Quantity + "," +
                                              "" + item.Total + ",0," +
                                              "" + item.Total + "," +
                                              "" + LoginInfo.Userid + ",0); SELECT CAST(PurchaseId as INT) from Purchase where id = " + result + "; ";
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
        public int UpdatePurchase(PurchaseModel purchaseModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update [dbo].[Purchase] set " +
                             "  [SupplierId]  = @SupplierId" +
                             "  ,[StoreId]  = @StoreId" +
                             "  ,[EmployeeId]  = @EmployeeId" +
                             "  ,[InventoryType]  = @InventoryType" +
                             "  ,[PurchaseDate] = @Date  " +
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
                            var deleteQuery = $"update PurchaseDetail set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in purchaseModel.PurchaseDetails)
                    {
                        var queryDetails = string.Empty;
                        if (item.PurchaseId > 0)
                        {
                            queryDetails = "Update [dbo].[PurchaseDetail] set " +
                                                 " [IngredientId]  = " + item.IngredientId + "," +
                                                 " [UnitPrice]   = " + item.UnitPrice + "," +
                                                 " [Qty]        =  " + item.Quantity + "," +
                                                 " [GrossAmount] = " + item.Total + "," +
                                                 " [TotalAmount] = " + item.Total + "," +
                                                 " [UserIdUpdated] = " + LoginInfo.Userid + "," +
                                                 " [DateUpdated] = GetUTCDate() " +
                                                 " where id = " + item.PurchaseId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[PurchaseDetail]" +
                                                  " ([PurchaseId]   " +
                                                  " ,[IngredientId] " +
                                                  " ,[UnitPrice]    " +
                                                  " ,[Qty]          " +
                                                  " ,[GrossAmount]  " +
                                                  " ,[TaxAmount]    " +
                                                  " ,[TotalAmount]  " +
                                                  " ,[UserIdUpdated] ) " +
                                                  " VALUES           " +
                                                  "(" + purchaseModel.Id + "," +
                                                  "" + item.IngredientId + "," +
                                                  "" + item.UnitPrice + "," +
                                                  "" + item.Quantity + "," +
                                                  "" + item.Total + "," +
                                                  "0," +
                                                  "" + item.Total + "," +
                                                  "" + LoginInfo.Userid + "); ";
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
        public int DeletePurchase(long purchaseId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update Purchase set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where id = " + purchaseId + ";" +
                    " Update PurchaseDetail set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where PurchaseId = " + purchaseId + ";";
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

        public List<PurchaseDetailsModel> GetPurchaseDetails(long purchaseId)
        {
            List<PurchaseDetailsModel> purchaseDetails = new List<PurchaseDetailsModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select pin.Id as PurchaseId,pin.IngredientId as IngredientId,i.IngredientName,pin.UnitPrice as UnitPrice, pin.Qty as Quantity, pin.GrossAmount as Total " +
                            "from purchase as P inner join PurchaseDetail as PIN on P.id = pin.PurchaseId " +
                            "inner join Ingredient as i on pin.IngredientId = i.Id where P.id = " + purchaseId + "and P.InventoryType=2 and pin.isdeleted = 0 and p.isdeleted = 0";
                purchaseDetails = con.Query<PurchaseDetailsModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public int DeletePurchaseDetails(long purchaseDetailsId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update PurchaseDetail set IsDeleted = 1 where id = " + purchaseDetailsId + ";";
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
                var query = $"SELECT RIGHT('PO-' + CONVERT(VARCHAR(8),ISNULL(MAX(Cast(PurchaseId as int)),0) + 1), 12) FROM purchase where InventoryType=2;";
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

        public List<PurchaseModel> GetPurchaseFoodMenuById(long purchaseId)
        {
            List<PurchaseModel> purchaseModelList = new List<PurchaseModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Purchase.Id as Id, Purchase.StoreId,Purchase.EmployeeId,ReferenceNo as ReferenceNo,PurchaseDate as [Date],Supplier.SupplierName, Supplier.Id as SupplierId," +
                      " Supplier.SupplierAddress1,Supplier.SupplierAddress2,Supplier.SupplierPhone,Supplier.SupplierEmail," +
                      "Purchase.GrossAmount,Purchase.GrandTotal as GrandTotal,Purchase.DiscountAmount as DiscountAmount,Purchase.TaxAmount as TaxAmount,Purchase.DueAmount as Due,Purchase.PaidAmount as Paid,Purchase.Notes,Purchase.Status,Purchase.DateInserted " +
                      "from Purchase inner join Supplier on Purchase.SupplierId = Supplier.Id where Purchase.InventoryType=1 And Purchase.Isdeleted = 0 and Purchase.Id = " + purchaseId;
                purchaseModelList = con.Query<PurchaseModel>(query).AsList();
            }
            return purchaseModelList;
        }

        public List<PurchaseViewModel> GetPurchaseFoodMenuList()
        {
            List<PurchaseViewModel> purchaseViewModelList = new List<PurchaseViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Purchase.Id as Id,  ReferenceNo, convert(varchar(12),PurchaseDate, 3) as [Date],Supplier.SupplierName," +
                    "Purchase.GrandTotal as GrandTotal,Purchase.DueAmount as Due, " +
                    "case when Purchase.Status = 3 then 'Rejected' when  Purchase.Status = 2 then 'Approved' Else 'Created' End AS Status,U.Username " +
                    "from Purchase inner join Supplier on Purchase.SupplierId = Supplier.Id inner join [User] U on U.Id=Purchase.UserIdInserted where Purchase.InventoryType=1 And Purchase.Isdeleted = 0 order by PurchaseDate, Purchase.Id desc";
                purchaseViewModelList = con.Query<PurchaseViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }

        public List<PurchaseDetailsModel> GetPurchaseFoodMenuDetails(long purchaseId)
        {
            List<PurchaseDetailsModel> purchaseDetails = new List<PurchaseDetailsModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select pin.Id as PurchaseId,pin.FoodMenuId as FoodMenuId,f.FoodMenuName,pin.UnitPrice as UnitPrice, pin.Qty as Quantity, pin.GrossAmount as Total, " +
                            "pin.DiscountAmount,pin.DiscountPercentage,pin.TaxPercentage,pin.TaxAmount " +
                            "from purchase as P inner join PurchaseDetail as PIN on P.id = pin.PurchaseId " +
                            "inner join FoodMenu as f on pin.FoodMenuId = f.Id where P.id = " + purchaseId + "and P.InventoryType=1 and pin.isdeleted = 0 and p.isdeleted = 0";
                purchaseDetails = con.Query<PurchaseDetailsModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public string InsertPurchaseFoodMenu(PurchaseModel purchaseModel)
        {
            int result = 0;
            int detailResult = 0;
            string referenceNo = "";

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                //Update Supplier Email
                if (!string.IsNullOrEmpty(purchaseModel.SupplierEmail))
                {
                    var supplierEmailQuery = "Update Supplier set SupplierEmail='" + purchaseModel.SupplierEmail + "' Where Id=" + purchaseModel.SupplierId;
                    con.Execute(supplierEmailQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                }

                var query = "INSERT INTO [dbo].[Purchase] " +
                             "  ([ReferenceNo] " +
                             "  ,[InventoryType]  " +
                             "  ,[SupplierId]     " +
                             "  ,[StoreId]        " +
                             "  ,[EmployeeId]     " +
                             "  ,[PurchaseDate]   " +
                             "  ,[GrossAmount]    " +
                             "  ,[DiscountAmount] " +
                             "  ,[TaxAmount]      " +
                             "  ,[GrandTotal]     " +
                             "  ,[PaidAmount]     " +
                             "  ,[DueAmount]      " +
                             "  ,[Notes]          " +
                             "  ,[Status]          " +
                             "  ,[UserIdInserted]  " +
                             "  ,[DateInserted]   " +
                             "  ,[IsDeleted])     " +
                             "   VALUES           " +
                             "  (@ReferenceNo,  " +
                             "   @InventoryType,   " +
                             "   @SupplierId,      " +
                             "   @StoreId,         " +
                             "   @EmployeeId,         " +
                             "   @Date,    " +
                             "   @GrandTotal,     " +
                             "   @DiscountAmount,     " +
                             "   @TaxAmount,       " +
                             "   @GrandTotal,      " +
                             "   @Paid,      " +
                             "   @Due,       " +
                             "   @Notes," +
                             "   @Status," +
                             "" + LoginInfo.Userid + "," +
                             "   GetUtcDate(),    " +
                             "   0); SELECT CAST(SCOPE_IDENTITY() as int); ";
                result = con.ExecuteScalar<int>(query, purchaseModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    foreach (var item in purchaseModel.PurchaseDetails)
                    {
                        //Add Items In SupplierItem
                        var supplierItemQuery = "BEGIN " +
                                                "IF NOT EXISTS(SELECT * FROM SupplierItem WHERE SupplierId = " + purchaseModel.SupplierId + " AND FoodMenuId = " + item.FoodMenuId + ") " +
                                                "BEGIN INSERT INTO SupplierItem(SupplierId, FoodMenuId, IngredientId) VALUES(" + purchaseModel.SupplierId + ", " + item.FoodMenuId + ", NuLL) " +
                                                "END " +
                                                "END";
                        con.Execute(supplierItemQuery, null, sqltrans, 0, System.Data.CommandType.Text);

                        var FoodmenuPurchaePriceUpdate = ""+
                         " update foodmenu set PurchasePrice = " + item.UnitPrice +" Where id = " + item.FoodMenuId;
                        con.Execute(FoodmenuPurchaePriceUpdate, null, sqltrans, 0, System.Data.CommandType.Text);

                        var queryDetails = "INSERT INTO [dbo].[PurchaseDetail]" +
                                              " ([PurchaseId]   " +
                                              " ,[FoodMenuId] " +
                                              " ,[UnitPrice]    " +
                                              " ,[Qty]          " +
                                              " ,[GrossAmount]  " +
                                              " ,[DiscountAmount]  " +
                                              " ,[DiscountPercentage]  " +
                                              " ,[TaxPercentage]  " +
                                              " ,[TaxAmount]    " +
                                              " ,[TotalAmount]  " +
                                              " ,[UserIdInserted]" +
                                              " ,[DateInserted]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              "" + item.FoodMenuId + "," +
                                              "" + item.UnitPrice + "," +
                                              "" + item.Quantity + "," +
                                              "" + item.Total + "," +
                                              "" + item.DiscountAmount + "," +
                                              "" + item.DiscountPercentage + "," +
                                              "" + item.TaxPercentage + "," +
                                              "" + item.TaxAmount + "," +
                                              "" + item.Total + "," +
                                              "" + LoginInfo.Userid +","+
                                              "   GetUtcDate(),    " +
                                              "0);  SELECT SCOPE_IDENTITY();";

                        detailResult = con.ExecuteScalar<int>(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);

                    }

                    if (detailResult > 0)
                    {
                        sqltrans.Commit();

                        query = "select  ReferenceNo from Purchase where ID=" + result;
                        referenceNo = con.ExecuteScalar<string>(query, null, sqltrans, 0, System.Data.CommandType.Text);
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

            return referenceNo;
        }

        public int UpdatePurchaseFoodMenu(PurchaseModel purchaseModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                //Update Supplier Email
                if (!string.IsNullOrEmpty(purchaseModel.SupplierEmail))
                {
                    var supplierEmailQuery = "Update Supplier set SupplierEmail='" + purchaseModel.SupplierEmail + "' Where Id=" + purchaseModel.SupplierId;
                    con.Execute(supplierEmailQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                }

                var query = "Update [dbo].[Purchase] set " +
                              "   [SupplierId]  = @SupplierId" +
                              "  ,[StoreId]  = @StoreId" +
                              "  ,[EmployeeId]  = @EmployeeId" +
                              "  ,[InventoryType]  = @InventoryType" +
                              "  ,[PurchaseDate] = @Date  " +
                              "  ,[GrossAmount]  =  @GrandTotal  " +
                              "  ,[DiscountAmount]  =  @DiscountAmount  " +
                              "  ,[TaxAmount]  =  @TaxAmount  " +
                              "  ,[GrandTotal]  = @GrandTotal   " +
                              "  ,[PaidAmount] = @Paid    " +
                              "  ,[DueAmount] =  @Due    " +
                              "  ,[Notes] =  @Notes    " +
                              "  ,[Status] =  @Status    " +
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
                            var deleteQuery = $"update PurchaseDetail set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in purchaseModel.PurchaseDetails)
                    {
                        var queryDetails = string.Empty;
                        if (item.PurchaseId > 0)
                        {
                            queryDetails = "Update [dbo].[PurchaseDetail] set " +
                                                 " [FoodMenuId]  = " + item.FoodMenuId + "," +
                                                 " [UnitPrice]   = " + item.UnitPrice + "," +
                                                 " [Qty]        =  " + item.Quantity + "," +
                                                 " [GrossAmount] = " + item.Total + "," +
                                                 " [DiscountAmount] = " + item.DiscountAmount + "," +
                                                 " [DiscountPercentage] = " + item.DiscountPercentage + "," +
                                                 " [TaxPercentage] = " + item.TaxPercentage + "," +
                                                 " [TaxAmount] = " + item.TaxAmount + "," +
                                                 " [TotalAmount] = " + item.Total + "," +
                                                 " [UserIdUpdated] = " + LoginInfo.Userid + "," +
                                                 " [DateUpdated] = GetUTCDate() " +
                                                 " where id = " + item.PurchaseId + ";";
                        }
                        else
                        {
                            //Add Items In SupplierItem
                            var supplierItemQuery = "BEGIN " +
                                                    "IF NOT EXISTS(SELECT * FROM SupplierItem WHERE SupplierId = " + purchaseModel.SupplierId + " AND FoodMenuId = " + item.FoodMenuId + ") " +
                                                    "BEGIN INSERT INTO SupplierItem(SupplierId, FoodMenuId, IngredientId) VALUES(" + purchaseModel.SupplierId + ", " + item.FoodMenuId + ", NuLL) " +
                                                    "END " +
                                                    "END";
                            con.Execute(supplierItemQuery, null, sqltrans, 0, System.Data.CommandType.Text);

                            queryDetails = "INSERT INTO [dbo].[PurchaseDetail]" +
                                                  " ([PurchaseId]   " +
                                                  " ,[FoodMenuId] " +
                                                  " ,[UnitPrice]    " +
                                                  " ,[Qty]          " +
                                                  " ,[GrossAmount]  " +
                                                  " ,[DiscountAmount]  " +
                                                  " ,[DiscountPercentage]  " +
                                                  " ,[TaxPercentage]  " +
                                                  " ,[TaxAmount]    " +
                                                  " ,[TotalAmount]  " +
                                                  " ,[UserIdInserted] ,[DateInserted]) " +
                                                  " VALUES           " +
                                                  "(" + purchaseModel.Id + "," +
                                                  "" + item.FoodMenuId + "," +
                                                  "" + item.UnitPrice + "," +
                                                  "" + item.Quantity + "," +
                                                  "" + item.Total + "," +
                                                  "" + item.DiscountAmount + "," +
                                                  "" + item.DiscountPercentage + "," +
                                                  "" + item.TaxPercentage + "," +
                                                  "" + item.TaxAmount + "," +
                                                  "" + item.Total + "," +
                                                  "" + LoginInfo.Userid + ",GetUtcDate());";
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

        public List<PurchaseViewModel> PurchaseFoodMenuListByDate(string fromDate, string toDate, int supplierId)
        {
            List<PurchaseViewModel> purchaseViewModels = new List<PurchaseViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select Purchase.Id as Id,  ReferenceNo, convert(varchar(12),PurchaseDate, 3) as [Date],Supplier.SupplierName," +
                    " Purchase.GrandTotal as GrandTotal,Purchase.DueAmount as Due, " +
                    " case when Purchase.Status = 3 then 'Rejected' when  Purchase.Status = 2 then 'Approved' Else 'Created' End AS Status,U.Username " +
                    " from Purchase inner join Supplier on Purchase.SupplierId = Supplier.Id inner join [User] U on U.Id=Purchase.UserIdInserted " +
                    " where  Purchase.InventoryType=1 And Purchase.Isdeleted = 0 " +
                    " AND Convert(Date, PurchaseDate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103)  ";
                if (supplierId != 0)
                {
                    query += " And Purchase.SupplierId =" + supplierId;
                }
                query += " order by Purchase.id desc;";
                purchaseViewModels = con.Query<PurchaseViewModel>(query).AsList();
            }

            return purchaseViewModels;
        }
        public string ReferenceNumberFoodMenu()
        {
            string result = string.Empty;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"SELECT RIGHT('PO-' + convert(varchar(8), isnull(count(*), 0) + 1), 12) FROM purchase where InventoryType = 1 and isdeleted = 0; ";
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
                // var query = "select top 1 UnitPrice from PurchaseDetail where FoodMenuId="+ foodMenuId + " order by id desc";

                var query = " select top 1 UnitPrice from " +
                         " (select Id, '' as PDId,PurchasePrice as UnitPrice from foodmenu  where Id = " + foodMenuId +
                         " union " +
                         " select '' as Id, Id asPDId, UnitPrice from PurchaseDetail where FoodMenuId = " + foodMenuId + ") restuls " +
                         " order by PDid desc; ";
                return con.ExecuteScalar<decimal>(query, null, sqltrans, 0, System.Data.CommandType.Text);
            }
        }

        public ClientModel GetClientDetail()
        {
            ClientModel clientModel = new ClientModel();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select PurchaseApprovalEmail,WebAppUrl from client";
                clientModel = con.QueryFirstOrDefault<ClientModel>(query);
                return clientModel;
            }
        }

        public int GetPurchaseIdByReferenceNo(string referenceNo)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id from Purchase where ReferenceNo='"+ referenceNo+"'";
                return con.QueryFirstOrDefault<int>(query);
            }
        }

        public int ApprovePurchaseOrder(int id)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "update Purchase set status=2 where id="+id;
                return con.Execute(query, null, null, 0, System.Data.CommandType.Text);
            }
        }
    }
}
