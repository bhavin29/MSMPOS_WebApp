using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using Dapper;
using RocketPOS.Interface.Repository;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

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
                var query = "select Purchase.Id as Id, PurchaseNumber as ReferenceNo,PurchaseDate as [Date],Supplier.SupplierName," +
                    "Purchase.GrandTotal as GrandTotal,Purchase.DueAmount as Due " +
                    "from Purchase inner join Supplier on Purchase.SupplierId = Supplier.Id";
                purchaseViewModelList = con.Query<PurchaseViewModel>(query).AsList();
            }


            return purchaseViewModelList;
        }

        public List<PurchaseModel> GetPurchaseById(long purhcaseId)
        {
            List<PurchaseModel> purchaseModelList = new List<PurchaseModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Purchase.Id as Id, PurchaseNumber as ReferenceNo,PurchaseDate as [Date],Supplier.SupplierName, Supplier.Id as SupplierId," +
                      "Purchase.GrandTotal as GrandTotal,Purchase.DueAmount as Due,Purchase.PaidAmount as Paid " +
                      "from Purchase inner join Supplier on Purchase.SupplierId = Supplier.Id where Purchase.Id = " + purhcaseId;
                purchaseModelList = con.Query<PurchaseModel>(query).AsList();
            }


            return purchaseModelList;
        }
        public int InsertPurchase(PurchaseModel purchaseModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[Purchase] " +
                             "  ([PurchaseNumber] " +
                             "  ,[SupplierId]     " +
                             "  ,[StoreId]        " +
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
                             "   @SupplierId,      " +
                             "   1,         " +
                             "   @Date,    " +
                             "   @GrandTotal,     " +
                             "   0,       " +
                             "   @GrandTotal,      " +
                             "   @Paid,      " +
                             "   @Due,       " +
                             "   'Notes'," +
                             "   1,   " +
                             "   GetUtcDate(),    " +
                             "   0); SELECT CAST(@ReferenceNo as int); ";
                result = con.ExecuteScalar<int>(query, purchaseModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
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
                                              "" + item.Total + "," +
                                              "0," +
                                              "" + item.Total + ",1,0); SELECT CAST(SCOPE_IDENTITY() as INT); ";
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

            return result;

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
                             "  ,[PurchaseDate] = @Date  " +
                             "  ,[GrossAmount]  =  @GrandTotal  " +
                             "  ,[GrandTotal]  = @GrandTotal   " +
                             "  ,[PaidAmount] = @Paid    " +
                             "  ,[DueAmount] =  @Due    " +
                             "  ,[UserIdUpdated] = 1 " +
                             "  ,[DateUpdated]  = GetUtcDate()  where id= " + purchaseModel.Id + ";";
                result = con.Execute(query, purchaseModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
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
                                                 " [TotalAmount] = " + item.Total + " where id = "+ item.PurchaseId +";";
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
                                                  " ,[UserIdUpdated]" +
                                                  " ,[IsDeleted])   " +
                                                  "VALUES           " +
                                                  "(" + purchaseModel.Id + "," +
                                                  "" + item.IngredientId + "," +
                                                  "" + item.UnitPrice + "," +
                                                  "" + item.Quantity + "," +
                                                  "" + item.Total + "," +
                                                  "0," +
                                                  "" + item.Total + ",1,0); ";
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
        public int DeletePurchase(long PurchaseId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update Purchase set IsDeleted = 1 where id = " + PurchaseId + ";" +
                    " Update PurchaseIngredient set IsDeleted = 1 where PurchaseId = " + PurchaseId + ";";
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
                            "inner join Ingredient as i on pin.IngredientId = i.Id where P.id = " + purchaseId;
                purchaseDetails = con.Query<PurchaseDetailsModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public int DeletePurchaseDetails(long PurchaseDetailsId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update PurchaseIngredient set IsDeleted = 1 where id = " + PurchaseDetailsId + ";";
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

        public long ReferenceNumber()
        {
            long result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"SELECT ISNULL(MAX(PurchaseNumber),0) + 1 FROM purchase ;";
                result = con.ExecuteScalar<long>(query, null, sqltrans, 0, System.Data.CommandType.Text);
                if (result > 0)
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
