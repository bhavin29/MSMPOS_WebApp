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
    public class SalesRepository : ISalesRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public SalesRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeletePurchase(long purchaseId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update Sales set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where id = " + purchaseId + ";" +
                    " Update SalesDetail set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where SalesId = " + purchaseId + ";";
                result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);
                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                { sqltrans.Rollback(); }
            }
            return result;
        }//

        public int DeletePurchaseDetails(long purchaseDetailsId)//
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update SalesDetail set IsDeleted = 1 where id = " + purchaseDetailsId + ";";
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

        public List<SalesModel> GetPurchaseFoodMenuById(long purchaseId)//
        {
            List<SalesModel> purchaseModelList = new List<SalesModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Sales.Id as Id, Sales.StoreId,Sales.EmployeeId,ReferenceNo as ReferenceNo,SalesDate as [Date],Customer.CustomerName, Customer.Id as CustomerId," +
                      " Sales.VatableAmount,Sales.NonVatableAmount," +
                      "Sales.GrossAmount,Sales.GrandTotal as GrandTotal,Sales.DiscountAmount as DiscountAmount,Sales.TaxAmount as TaxAmount,Sales.DueAmount as Due,Sales.PaidAmount as Paid,Sales.Notes,Sales.Status,Sales.DateInserted " +
                      "from Sales inner join Customer on Sales.CustomerId = Customer.Id where Sales.InventoryType=1 And Sales.Isdeleted = 0 and Sales.Id = " + purchaseId;
                purchaseModelList = con.Query<SalesModel>(query).AsList();
            }
            return purchaseModelList;
        }

        public List<SalesDetailsModel> GetPurchaseFoodMenuDetails(long purchaseId)//
        {
            List<SalesDetailsModel> purchaseDetails = new List<SalesDetailsModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select pin.Id as SalesId, ROW_NUMBER() OVER(ORDER BY PIN.Id ASC)-1 AS RowNumber," +
                            " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then 2 else 1 end) else 0 end) as ItemType, " +
                            " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then pin.AssetItemId else pin.IngredientId end) else pin.FoodMenuId end) as FoodMenuId, " +
                            " (case when pin.FoodMenuId is null then (case when pin.IngredientId is null then AI.AssetItemName else I.IngredientName end) else f.FoodMenuName end) as FoodMenuName,  " +
                            " pin.UnitPrice as UnitPrice, pin.Qty as Quantity, pin.TotalAmount as Total, " +
                            " pin.DiscountAmount,pin.DiscountPercentage,pin.TaxPercentage,pin.TaxAmount, pin.VatableAmount,pin.NonVatableAmount  " +
                            " from sales as P inner join salesDetail as PIN on P.id = pin.salesId " +
                            " left join FoodMenu as f on pin.FoodMenuId = f.Id " +
                            " left join Ingredient as I on pin.IngredientId = I.Id " +
                            " left join AssetItem as AI on pin.AssetItemId = AI.Id  " +
                           "where P.id = " + purchaseId + " and pin.isdeleted = 0 and p.isdeleted = 0";
                purchaseDetails = con.Query<SalesDetailsModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public string InsertPurchaseFoodMenu(SalesModel purchaseModel)//
        {
            int result = 0;
            int detailResult = 0;
            string referenceNo = "";

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                //Update Supplier Email
                //if (!string.IsNullOrEmpty(purchaseModel.SupplierEmail))
                //{
                //    var supplierEmailQuery = "Update Supplier set SupplierEmail='" + purchaseModel.SupplierEmail + "' Where Id=" + purchaseModel.SupplierId;
                //    con.Execute(supplierEmailQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                //}

                var query = "INSERT INTO [dbo].[sales] " +
                             "  ([ReferenceNo] " +
                             "  ,[InventoryType]  " +
                             "  ,[CustomerId]     " +
                             "  ,[StoreId]        " +
                             "  ,[EmployeeId]     " +
                             "  ,[SalesDate]   " +
                             "  ,[GrossAmount]    " +
                             "  ,[DiscountAmount] " +
                             "  ,[TaxAmount]      " +
                             "  ,[GrandTotal]     " +
                             "  ,[PaidAmount]     " +
                             "  ,[DueAmount]      " +
                             "  ,[VatableAmount]      " +
                             "  ,[NonVatableAmount]      " +
                             "  ,[Notes]          " +
                             "  ,[Status]          " +
                             "  ,[UserIdInserted]  " +
                             "  ,[DateInserted]   " +
                             "  ,[IsDeleted])     " +
                             "   VALUES           " +
                             "  (@ReferenceNo,  " +
                             "   @InventoryType,   " +
                             "   @CustomerId,      " +
                             "   @StoreId,         " +
                             "   @EmployeeId,         " +
                             "   @Date,    " +
                             "   (@GrandTotal - @TaxAmount),     " +
                             "   @DiscountAmount,     " +
                             "   @TaxAmount,       " +
                             "   @GrandTotal,      " +
                             "   @Paid,      " +
                             "   @Due,       " +
                             "   @VatableAmount,      " +
                             "   @NonVatableAmount,      " +
                             "   @Notes," +
                             "   @Status," +
                             "" + LoginInfo.Userid + "," +
                             "   GetUtcDate(),    " +
                             "   0); SELECT CAST(SCOPE_IDENTITY() as int); ";
                result = con.ExecuteScalar<int>(query, purchaseModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    foreach (var item in purchaseModel.SalesDetails)
                    {
                        //Add Items In SupplierItem

                        if (item.ItemType == 0)
                        {
                            var supplierItemQuery = "BEGIN " +
                                                    "IF NOT EXISTS(SELECT * FROM CustomerItem WHERE CustomerId = " + purchaseModel.CustomerId + " AND FoodMenuId = " + item.FoodMenuId + ") " +
                                                    "BEGIN INSERT INTO CustomerItem(CustomerId, FoodMenuId, IngredientId) VALUES(" + purchaseModel.CustomerId + ", " + item.FoodMenuId + ", NuLL) " +
                                                    "END " +
                                                    "END";
                            con.Execute(supplierItemQuery, null, sqltrans, 0, System.Data.CommandType.Text);

                            //var FoodmenuPurchaePriceUpdate = "" +
                            // " update foodmenu set PurchasePrice = " + item.UnitPrice + " Where id = " + item.FoodMenuId;
                            //con.Execute(FoodmenuPurchaePriceUpdate, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                        else if (item.ItemType == 1)
                        {
                            var supplierItemQuery = "BEGIN " +
                                                    "IF NOT EXISTS(SELECT * FROM CustomerItem WHERE CustomerId = " + purchaseModel.CustomerId + " AND IngredientId = " + item.FoodMenuId + ") " +
                                                    "BEGIN INSERT INTO CustomerItem(CustomerId, FoodMenuId, IngredientId) VALUES(" + purchaseModel.CustomerId + ", NULL," + item.FoodMenuId + ") " +
                                                    "END " +
                                                    "END";
                            con.Execute(supplierItemQuery, null, sqltrans, 0, System.Data.CommandType.Text);

                            //var FoodmenuPurchaePriceUpdate = "" +
                            // " update Ingredient set PurchasePrice = " + item.UnitPrice + " Where id = " + item.FoodMenuId;
                            //con.Execute(FoodmenuPurchaePriceUpdate, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                        else if (item.ItemType == 2)
                        {
                            var supplierItemQuery = "BEGIN " +
                                                    "IF NOT EXISTS(SELECT * FROM CustomerItem WHERE CustomerId = " + purchaseModel.CustomerId + " AND AssetItemId = " + item.FoodMenuId + ") " +
                                                    "BEGIN INSERT INTO CustomerItem(CustomerId, FoodMenuId, IngredientId,AssetItemId) VALUES(" + purchaseModel.CustomerId + ", NULL" + ", NULL," + item.FoodMenuId + ") " +
                                                    "END " +
                                                    "END";
                            con.Execute(supplierItemQuery, null, sqltrans, 0, System.Data.CommandType.Text);

                            //var FoodmenuPurchaePriceUpdate = "" +
                            // " update AssetItem set CostPrice = " + item.UnitPrice + " Where id = " + item.FoodMenuId;
                            //con.Execute(FoodmenuPurchaePriceUpdate, null, sqltrans, 0, System.Data.CommandType.Text);
                        }

                        var queryDetails = "INSERT INTO [dbo].[SalesDetail]" +
                                              " ([SalesId]   " +
                                              " ,[FoodMenuId] " +
                                              " ,[IngredientId] " +
                                              " ,[AssetItemId] " +
                                              " ,[UnitPrice]    " +
                                              " ,[Qty]          " +
                                              " ,[GrossAmount]  " +
                                              " ,[DiscountAmount]  " +
                                              " ,[DiscountPercentage]  " +
                                              " ,[TaxPercentage]  " +
                                              " ,[TaxAmount]    " +
                                              " ,[TotalAmount]  " +
                                             "  ,[VatableAmount]      " +
                                             "  ,[NonVatableAmount]      " +
                                              " ,[UserIdInserted]" +
                                              " ,[DateInserted]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES           " +
                                              "(" + result + ",";
                        if (item.ItemType == 0)
                        {
                            queryDetails = queryDetails + "" + item.FoodMenuId + ",NUll,NUll,";

                        }
                        else if (item.ItemType == 1)
                        {
                            queryDetails = queryDetails + "NULL," + item.FoodMenuId + ",NULL,";

                        }
                        else if (item.ItemType == 2)
                        {
                            queryDetails = queryDetails + "NULL,NULL," + item.FoodMenuId + ",";

                        }
                        queryDetails = queryDetails + "" + item.UnitPrice + "," +
                                                "" + item.Quantity + "," +
                                                "" + (item.Total - item.TaxAmount) + "," +
                                                "" + item.DiscountAmount + "," +
                                                "" + item.DiscountPercentage + "," +
                                                "" + item.TaxPercentage + "," +
                                                "" + item.TaxAmount + "," +
                                                "" + item.Total + "," +
                                                "" + item.VatableAmount + "," +
                                                "" + item.NonVatableAmount + "," +
                                                "" + LoginInfo.Userid + "," +
                                                "   GetUtcDate(),    " +
                                                "0);  SELECT SCOPE_IDENTITY();";

                        detailResult = con.ExecuteScalar<int>(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);

                    }

                    if (detailResult > 0)
                    {
                        sqltrans.Commit();

                        query = "select  ReferenceNo from sales where ID=" + result;
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

        public int UpdatePurchaseFoodMenu(SalesModel purchaseModel)//
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                var query = "Update [dbo].[sales] set " +
                              "   [CustomerId]  = @CustomerId" +
                              "  ,[StoreId]  = @StoreId" +
                              "  ,[EmployeeId]  = @EmployeeId" +
                              "  ,[InventoryType]  = @InventoryType" +
                              "  ,[SalesDate] = @Date  " +
                              "  ,[GrossAmount]  =  (@GrandTotal - @TaxAmount)  " +
                              "  ,[DiscountAmount]  =  @DiscountAmount  " +
                              "  ,[TaxAmount]  =  @TaxAmount  " +
                              "  ,[GrandTotal]  = @GrandTotal   " +
                              "  ,[PaidAmount] = @Paid    " +
                              "  ,[DueAmount] =  @Due    " +
                              "  ,[VatableAmount] = @VatableAmount     " +
                              "  ,[NonVatableAmount] = @NonVatableAmount      " +
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
                            var deleteQuery = $"update SalesDetail set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in purchaseModel.SalesDetails)
                    {
                        var queryDetails = string.Empty;
                        if (item.SalesId > 0)
                        {
                            queryDetails = "Update [dbo].[SalesDetail] set ";
                            if (item.ItemType == 0)
                            {
                                queryDetails = queryDetails + " [FoodMenuId]  = " + item.FoodMenuId + ",[IngredientId] = null, [AssetItemId] =null,";
                            }
                            else if (item.ItemType == 1)
                            {
                                queryDetails = queryDetails + " [IngredientId]  = " + item.FoodMenuId + ",[FoodMenuId] = null,[AssetItemId] =null, ";

                            }
                            else if (item.ItemType == 2)
                            {
                                queryDetails = queryDetails + " [AssetItemId]  = " + item.FoodMenuId + ",[IngredientId] = null,[FoodMenuId] = null, ";

                            }
                            queryDetails = queryDetails + " [UnitPrice]   = " + item.UnitPrice + "," +
                                         " [Qty]        =  " + item.Quantity + "," +
                                         " [GrossAmount] = " + (item.Total - item.TaxAmount) + "," +
                                         " [DiscountAmount] = " + item.DiscountAmount + "," +
                                         " [DiscountPercentage] = " + item.DiscountPercentage + "," +
                                         " [TaxPercentage] = " + item.TaxPercentage + "," +
                                         " [TaxAmount] = " + item.TaxAmount + "," +
                                         " [TotalAmount] = " + item.Total + "," +
                                         " [VatableAmount] = " + item.VatableAmount + "," +
                                         " [NonVatableAmount] = " + item.NonVatableAmount + "," +
                                         " [UserIdUpdated] = " + LoginInfo.Userid + "," +
                                         " [DateUpdated] = GetUTCDate() " +
                                         " where id = " + item.SalesId + ";";
                        }
                        else
                        {

                            if (item.ItemType == 0)
                            {
                                var supplierItemQuery = "BEGIN " +
                                                        "IF NOT EXISTS(SELECT * FROM CustomerItem WHERE CustomerId = " + purchaseModel.CustomerId + " AND FoodMenuId = " + item.FoodMenuId + ") " +
                                                        "BEGIN INSERT INTO CustomerItem(CustomerId, FoodMenuId, IngredientId) VALUES(" + purchaseModel.CustomerId + ", " + item.FoodMenuId + ", NuLL) " +
                                                        "END " +
                                                        "END";
                                con.Execute(supplierItemQuery, null, sqltrans, 0, System.Data.CommandType.Text);

                                //var FoodmenuPurchaePriceUpdate = "" +
                                // " update foodmenu set PurchasePrice = " + item.UnitPrice + " Where id = " + item.FoodMenuId;
                                //con.Execute(FoodmenuPurchaePriceUpdate, null, sqltrans, 0, System.Data.CommandType.Text);
                            }
                            else if (item.ItemType == 1)
                            {
                                var supplierItemQuery = "BEGIN " +
                                                        "IF NOT EXISTS(SELECT * FROM CustomerItem WHERE CustomerId = " + purchaseModel.CustomerId + " AND IngredientId = " + item.FoodMenuId + ") " +
                                                        "BEGIN INSERT INTO CustomerItem(CustomerId, FoodMenuId, IngredientId) VALUES(" + purchaseModel.CustomerId + ", NULL," + item.FoodMenuId + ") " +
                                                        "END " +
                                                        "END";
                                con.Execute(supplierItemQuery, null, sqltrans, 0, System.Data.CommandType.Text);

                                //var FoodmenuPurchaePriceUpdate = "" +
                                // " update Ingredient set PurchasePrice = " + item.UnitPrice + " Where id = " + item.FoodMenuId;
                                //con.Execute(FoodmenuPurchaePriceUpdate, null, sqltrans, 0, System.Data.CommandType.Text);
                            }
                            else if (item.ItemType == 2)
                            {
                                var supplierItemQuery = "BEGIN " +
                                                        "IF NOT EXISTS(SELECT * FROM CustomerItem WHERE CustomerId = " + purchaseModel.CustomerId + " AND AssetItemId = " + item.FoodMenuId + ") " +
                                                        "BEGIN INSERT INTO CustomerItem(CustomerId, FoodMenuId, IngredientId,AssetItemId) VALUES(" + purchaseModel.CustomerId + ", NULL" + ", NULL," + item.FoodMenuId + ") " +
                                                        "END " +
                                                        "END";
                                con.Execute(supplierItemQuery, null, sqltrans, 0, System.Data.CommandType.Text);

                                //var FoodmenuPurchaePriceUpdate = "" +
                                // " update AssetItem set CostPrice = " + item.UnitPrice + " Where id = " + item.FoodMenuId;
                                //con.Execute(FoodmenuPurchaePriceUpdate, null, sqltrans, 0, System.Data.CommandType.Text);
                            }


                            queryDetails = "INSERT INTO [dbo].[SalesDetail]" +
                                              " ([SalesId]   " +
                                              " ,[FoodMenuId] " +
                                              " ,[IngredientId] " +
                                              " ,[AssetItemId] " +
                                              " ,[UnitPrice]    " +
                                              " ,[Qty]          " +
                                              " ,[GrossAmount]  " +
                                              " ,[DiscountAmount]  " +
                                              " ,[DiscountPercentage]  " +
                                              " ,[TaxPercentage]  " +
                                              " ,[TaxAmount]    " +
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
                                queryDetails = queryDetails + "NULL,NULL," + item.FoodMenuId + ",";

                            }
                            queryDetails = queryDetails + "" + item.UnitPrice + "," +
                                                    "" + item.Quantity + "," +
                                                    "" + (item.Total - item.TaxAmount) + "," +
                                                    "" + item.DiscountAmount + "," +
                                                    "" + item.DiscountPercentage + "," +
                                                    "" + item.TaxPercentage + "," +
                                                    "" + item.TaxAmount + "," +
                                                    "" + item.Total + "," +
                                                     "" + item.VatableAmount + "," +
                                                    "" + item.NonVatableAmount + "," +
                                                     "" + LoginInfo.Userid + "," +
                                                    "   GetUtcDate(),    " +
                                                    "0);";
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

        public List<SalesViewModel> PurchaseFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId)//
        {
            List<SalesViewModel> purchaseViewModels = new List<SalesViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select sales.Id as Id,  ReferenceNo, convert(varchar(12),SalesDate, 3) as [Date],customer.customerName," +
                    " sales.GrandTotal as GrandTotal,sales.DueAmount as Due, S.StoreName," +
                    " case when sales.Status = 5 then 'Invoice' when sales.Status = 4 then 'Delivery'  when sales.Status = 3 then 'Rejected' when  sales.Status = 2 then 'Approved' Else 'Created' End AS Status ,isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as Username  " +
                    " from sales inner join customer on sales.customerId = customer.Id inner join [User] U on U.Id=sales.UserIdInserted  inner join employee e on e.id = u.employeeid inner join store S on S.Id = sales.StoreId" +
                    " where  sales.InventoryType=1 And sales.Isdeleted = 0 " +
                    " AND Convert(Date, SalesDate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103)  ";
                if (customerId != 0)
                {
                    query += " And sales.CustomerId =" + customerId;
                }
                if (storeId != 0)
                {
                    query += " And sales.StoreId =" + storeId;
                }
                query += " order by sales.salesDate, sales.id desc;";
                purchaseViewModels = con.Query<SalesViewModel>(query).AsList();
            }

            return purchaseViewModels;
        }
        public string ReferenceNumberFoodMenu()//
        {
            string result = string.Empty;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"SELECT RIGHT('SO-' + convert(varchar(8), isnull(count(*), 0) + 1), 12) FROM sales where InventoryType = 1 and isdeleted = 0; ";
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

        public decimal GetTaxByFoodMenuId(int foodMenuId, int itemType)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "";
                if (itemType == 0)
                {
                    query = "select  ISNULL(TaxPercentage,0) AS TaxPercentage from foodmenuRate fmr " +
                               "Inner join tax t on t.Id = fmr.FoodVatTaxId " +
                               "where fmr.FoodMenuId = " + foodMenuId + " And fmr.OutletId = 1"; // Need To Change OutLet Id Dynamic value
                }
                else if (itemType == 1)
                {
                    query = "select  ISNULL(TaxPercentage,0) AS TaxPercentage from Ingredient I " +
                                "Inner join tax t on t.Id = I.TaxId " +
                                "where I.Id = " + foodMenuId; // Need To Change OutLet Id Dynamic value
                }
                else if (itemType == 2)
                {
                    query = "select  ISNULL(TaxPercentage,0) AS TaxPercentage from AssetItem AI " +
                                "Inner join tax t on t.Id = AI.TaxId " +
                                "where AI.Id = " + foodMenuId; // Need To Change OutLet Id Dynamic value
                }


                return con.ExecuteScalar<decimal>(query, null, sqltrans, 0, System.Data.CommandType.Text);
            }
        }

        public decimal GetFoodMenuLastPrice(int itemType, int foodMenuId)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                // var query = "select top 1 UnitPrice from PurchaseDetail where FoodMenuId="+ foodMenuId + " order by id desc";
                var query = "";

                if (itemType == 0)
                {
                    query = " select top 1 UnitPrice from " +
                             " (select Id, '' as PDId,SalesPrice as UnitPrice from foodmenu  where Id = " + foodMenuId +
                             " union " +
                             " select '' as Id, Id as PDId, UnitPrice from SalesDetail where FoodMenuId = " + foodMenuId + ") restuls " +
                             " order by PDid desc; ";
                }
                else if (itemType == 1)
                {
                    query = " select top 1 UnitPrice from " +
                          " (select Id, '' as PDId,SalesPrice as UnitPrice from Ingredient  where Id = " + foodMenuId +
                          " union " +
                          " select '' as Id, Id as PDId, UnitPrice from SalesDetail where IngredientId = " + foodMenuId + ") restuls " +
                          " order by PDid desc; ";
                }
                else if (itemType == 2)
                {
                    query = " select top 1 UnitPrice from " +
                          " (select Id, '' as PDId,CostPrice as UnitPrice from AssetItem  where Id = " + foodMenuId +
                          " union " +
                          " select '' as Id, Id as PDId, UnitPrice from SalesDetail where AssetItemId = " + foodMenuId + ") restuls " +
                          " order by PDid desc; ";
                }

                return con.ExecuteScalar<decimal>(query, null, sqltrans, 0, System.Data.CommandType.Text);
            }
        }

        public int ApprovePurchaseOrder(int id)//
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "update sales set status=2 where id=" + id;
                return con.Execute(query, null, null, 0, System.Data.CommandType.Text);
            }
        }

        public List<SalesViewModel> GetPurchaseFoodMenuList()//
        {
            List<SalesViewModel> purchaseViewModelList = new List<SalesViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select sales.Id as Id,  ReferenceNo, convert(varchar(12),SalesDate, 3) as [Date],customer.customerName," +
                    "sales.GrandTotal as GrandTotal,sales.DueAmount as Due,S.StoreName, " +
                    "case when sales.Status = 5 then 'Invoice' when sales.Status = 4 then 'Delivery' when sales.Status = 3 then 'Rejected' when  sales.Status = 2 then 'Approved' Else 'Created' End AS Status ,isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as Username  " +
                    "from sales inner join customer on sales.customerId = customer.Id inner join [User] U on U.Id=sales.UserIdInserted  inner join employee e on e.id = u.employeeid  inner join store S on S.Id = sales.StoreId where sales.InventoryType=1 And sales.Isdeleted = 0 order by SalesDate, sales.Id desc";
                purchaseViewModelList = con.Query<SalesViewModel>(query).AsList();
            }
            return purchaseViewModelList;
        }
    }
}
