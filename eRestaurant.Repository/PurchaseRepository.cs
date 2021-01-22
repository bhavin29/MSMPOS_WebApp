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
                var query = "select Purchase.Id as Id, PurchaseNumber as ReferenceNo, convert(varchar(12),PurchaseDate, 3) as [Date],Supplier.SupplierName," +
                    "Purchase.GrandTotal as GrandTotal,Purchase.DueAmount as Due " +
                    "from Purchase inner join Supplier on Purchase.SupplierId = Supplier.Id where Purchase.InventoryType=2 And Purchase.Isdeleted = 0 order by PurchaseDate, purchaseNumber desc";
                purchaseViewModelList = con.Query<PurchaseViewModel>(query).AsList();
            }


            return purchaseViewModelList;
        }

        public List<PurchaseModel> GetPurchaseById(long purchaseId)
        {
            List<PurchaseModel> purchaseModelList = new List<PurchaseModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Purchase.Id as Id, Purchase.StoreId,Purchase.EmployeeId,PurchaseNumber as ReferenceNo,PurchaseDate as [Date],Supplier.SupplierName, Supplier.Id as SupplierId," +
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
                             "  ([PurchaseNumber] " +
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
                        var queryDetails = "INSERT INTO [dbo].[PurchaseIngredient]" +
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
                                              "" + LoginInfo.Userid + ",0); SELECT CAST(PurchaseNumber as INT) from Purchase where id = " + result + "; ";
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
                            var deleteQuery = $"update PurchaseIngredient set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in purchaseModel.PurchaseDetails)
                    {
                        var queryDetails = string.Empty;
                        if (item.PurchaseId > 0)
                        {
                            queryDetails = "Update [dbo].[PurchaseIngredient] set " +
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
                            queryDetails = "INSERT INTO [dbo].[PurchaseIngredient]" +
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
                var query = $"update Purchase set IsDeleted = 1 where id = " + purchaseId + ";" +
                    " Update PurchaseIngredient set IsDeleted = 1 where PurchaseId = " + purchaseId + ";";
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
                            "from purchase as P inner join PurchaseIngredient as PIN on P.id = pin.PurchaseId " +
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
                var query = $"update PurchaseIngredient set IsDeleted = 1 where id = " + purchaseDetailsId + ";";
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
                var query = $"SELECT RIGHT('000000' + CONVERT(VARCHAR(8),ISNULL(MAX(PurchaseNumber),0) + 1), 6) FROM purchase where InventoryType=2;";
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
                var query = "select Purchase.Id as Id, Purchase.StoreId,Purchase.EmployeeId,PurchaseNumber as ReferenceNo,PurchaseDate as [Date],Supplier.SupplierName, Supplier.Id as SupplierId," +
                      "Purchase.GrandTotal as GrandTotal,Purchase.DueAmount as Due,Purchase.PaidAmount as Paid,Purchase.Notes " +
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
                var query = "select Purchase.Id as Id, PurchaseNumber as ReferenceNo, convert(varchar(12),PurchaseDate, 3) as [Date],Supplier.SupplierName," +
                    "Purchase.GrandTotal as GrandTotal,Purchase.DueAmount as Due " +
                    "from Purchase inner join Supplier on Purchase.SupplierId = Supplier.Id where Purchase.InventoryType=1 And Purchase.Isdeleted = 0 order by PurchaseDate, purchaseNumber desc";
                purchaseViewModelList = con.Query<PurchaseViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }

        public List<PurchaseDetailsModel> GetPurchaseFoodMenuDetails(long purchaseId)
        {
            List<PurchaseDetailsModel> purchaseDetails = new List<PurchaseDetailsModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select pin.Id as PurchaseId,pin.FoodMenuId as FoodMenuId,f.FoodMenuName,pin.UnitPrice as UnitPrice, pin.Qty as Quantity, pin.GrossAmount as Total " +
                            "from purchase as P inner join PurchaseIngredient as PIN on P.id = pin.PurchaseId " +
                            "inner join FoodMenu as f on pin.FoodMenuId = f.Id where P.id = " + purchaseId + "and P.InventoryType=1 and pin.isdeleted = 0 and p.isdeleted = 0";
                purchaseDetails = con.Query<PurchaseDetailsModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public int InsertPurchaseFoodMenu(PurchaseModel purchaseModel)
        {
            int result = 0;
            int detailResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[Purchase] " +
                             "  ([PurchaseNumber] " +
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
                        var queryDetails = "INSERT INTO [dbo].[PurchaseIngredient]" +
                                              " ([PurchaseId]   " +
                                              " ,[FoodMenuId] " +
                                              " ,[UnitPrice]    " +
                                              " ,[Qty]          " +
                                              " ,[GrossAmount]  " +
                                              " ,[TaxAmount]    " +
                                              " ,[TotalAmount]  " +
                                              " ,[UserIdUpdated]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              "" + item.FoodMenuId + "," +
                                              "" + item.UnitPrice + "," +
                                              "" + item.Quantity + "," +
                                              "" + item.Total + ",0," +
                                              "" + item.Total + "," +
                                              "" + LoginInfo.Userid + ",0); SELECT CAST(PurchaseNumber as INT) from Purchase where id = " + result + "; ";
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

        public int UpdatePurchaseFoodMenu(PurchaseModel purchaseModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update [dbo].[Purchase] set " +
                             "   [SupplierId]  = @SupplierId" +
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
                            var deleteQuery = $"update PurchaseIngredient set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in purchaseModel.PurchaseDetails)
                    {
                        var queryDetails = string.Empty;
                        if (item.PurchaseId > 0)
                        {
                            queryDetails = "Update [dbo].[PurchaseIngredient] set " +
                                                 " [FoodMenuId]  = " + item.FoodMenuId + "," +
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
                            queryDetails = "INSERT INTO [dbo].[PurchaseIngredient]" +
                                                  " ([PurchaseId]   " +
                                                  " ,[FoodMenuId] " +
                                                  " ,[UnitPrice]    " +
                                                  " ,[Qty]          " +
                                                  " ,[GrossAmount]  " +
                                                  " ,[TaxAmount]    " +
                                                  " ,[TotalAmount]  " +
                                                  " ,[UserIdUpdated] ) " +
                                                  " VALUES           " +
                                                  "(" + purchaseModel.Id + "," +
                                                  "" + item.FoodMenuId + "," +
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

        public string ReferenceNumberFoodMenu()
        {
            string result = string.Empty;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"SELECT RIGHT('000000' + CONVERT(VARCHAR(8),ISNULL(MAX(PurchaseNumber),0) + 1), 6) FROM purchase where InventoryType=1;";
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

    }
}
