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
    public class ProductionEntryRepository: IProductionEntryRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public ProductionEntryRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteProductionEntryById(int id)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update ProductionEntry set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where id = " + id + ";" +
                            " update ProductionEntryFoodmenu set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where ProductionEntryId = " + id + ";" +
                            " update ProductionEntryIngredient set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where ProductionEntryId = " + id + ";";
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

        public ProductionEntryModel GetProductionEntryById(int id)
        {
            ProductionEntryModel productionEntryModel = new ProductionEntryModel();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PE.Id, PE.ProductionFormulaId,PF.FormulaName AS ProductionFormulaName, convert(varchar(12),ProductionDate, 3) as ProductionDate,PE.ActualBatchSize,PE.VariationNotes,PE.Notes,PE.Status, PF.[BatchSize],PF.[BatchSizeUnitId], " +
                            "PE.FoodmenuType,U.UnitName from ProductionEntry PE Inner Join ProductionFormula PF On PF.Id=PE.ProductionFormulaId Inner Join [Units] U On U.Id=PF.BatchSizeUnitId Where PE.IsDeleted=0 And PE.Id=" + id;
                productionEntryModel = con.QueryFirstOrDefault<ProductionEntryModel>(query);
            }
            return productionEntryModel;
        }

        public List<ProductionEntryFoodMenuModel> GetProductionEntryFoodMenuDetails(int productionEntryId)
        {
            List<ProductionEntryFoodMenuModel> productionEntryFoodMenuModel = new List<ProductionEntryFoodMenuModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "  select PEM.Id AS PEFoodMenuId, PEM.FoodMenuId,FM.FoodMenuName,PEM.ExpectedOutput,PEM.AllocationOutput,PEM.ActualOutput  from ProductionEntryFoodmenu PEM  " +
                            " Inner Join FoodMenu FM On FM.Id = PEM.FoodMenuId Where PEM.IsDeleted = 0 And PEM.ProductionEntryId =" + productionEntryId;
                productionEntryFoodMenuModel = con.Query<ProductionEntryFoodMenuModel>(query).AsList();
            }
            return productionEntryFoodMenuModel;
        }

        public List<ProductionEntryIngredientModel> GetProductionEntryIngredientDetails(int productionEntryId)
        {
            List<ProductionEntryIngredientModel> productionEntryIngredientModel = new List<ProductionEntryIngredientModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "  select PEI.Id AS PEIngredientId, PEI.IngredientId,I.IngredientName,PEI.IngredientQty,PEI.ActualIngredientQty  from ProductionEntryIngredient PEI    " +
                            "  Inner Join Ingredient I On I.Id=PEI.IngredientId Where PEI.IsDeleted=0 And PEI.ProductionEntryId=" + productionEntryId;
                productionEntryIngredientModel = con.Query<ProductionEntryIngredientModel>(query).AsList();
            }
            return productionEntryIngredientModel;
        }

        public List<ProductionEntryViewModel> GetProductionEntryList(int foodmenuType)
        {
            List<ProductionEntryViewModel> productionEntryList = new List<ProductionEntryViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select PE.Id,PE.ProductionFormulaId,PF.FormulaName As ProductionFormulaName,PE.ReferenceNo,PE.ActualBatchSize,convert(varchar(12),ProductionDate, 3) as ProductionDate,PE.ProductionCompletionDate,PE.Status, PE.FoodmenuType,U.Username from ProductionEntry  PE  " +
                            " inner join ProductionFormula  PF On PF.Id=PE.ProductionFormulaId  inner join [User] U On U.Id=PE.UserIdInserted where PE.IsDeleted=0 ";

                if (foodmenuType != 0)
                {
                    query += " And PE.FoodmenuType = " + foodmenuType;
                }
                else
                {
                    query += " And PE.FoodmenuType = 2";
                }
                productionEntryList = con.Query<ProductionEntryViewModel>(query).AsList();
            }
            return productionEntryList;
        }

        public ProductionEntryModel GetProductionFormulaById(int id)
        {
            ProductionEntryModel productionEntryModel = new ProductionEntryModel();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PF.Id AS ProductionFormulaId,PF.FormulaName AS ProductionFormulaName,PF.[BatchSize],PF.[BatchSizeUnitId],PF.FoodmenuType,PF.IsActive,U.UnitName from ProductionFormula PF " +
                            "Inner Join [Units] U On U.Id=PF.BatchSizeUnitId Where PF.IsDeleted=0 And PF.Id=" + id;
                productionEntryModel = con.QueryFirstOrDefault<ProductionEntryModel>(query);
            }
            return productionEntryModel;
        }

        public List<ProductionEntryFoodMenuModel> GetProductionFormulaFoodMenuDetails(int productionFormulaId)
        {
            List<ProductionEntryFoodMenuModel> productionEntryFoodMenuModel = new List<ProductionEntryFoodMenuModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select PFM.FoodMenuId,FM.FoodMenuName,PFM.ExpectedOutput  from ProductionFormulaFoodmenu PFM " +
                            " Inner Join FoodMenu FM On FM.Id = PFM.FoodMenuId Where PFM.IsDeleted = 0 And PFM.ProductionFormulaId =" + productionFormulaId;
                productionEntryFoodMenuModel = con.Query<ProductionEntryFoodMenuModel>(query).AsList();
            }
            return productionEntryFoodMenuModel;
        }

        public List<ProductionEntryIngredientModel> GetProductionFormulaIngredientDetails(int productionFormulaId)
        {
            List<ProductionEntryIngredientModel> productionEntryIngredientModel = new List<ProductionEntryIngredientModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select PFI.IngredientId,I.IngredientName,PFI.IngredientQty  from ProductionFormulaIngredient PFI  " +
                            " Inner Join Ingredient I On I.Id=PFI.IngredientId Where PFI.IsDeleted=0 And PFI.ProductionFormulaId=" + productionFormulaId;
                productionEntryIngredientModel = con.Query<ProductionEntryIngredientModel>(query).AsList();
            }
            return productionEntryIngredientModel;
        }

        public int InsertProductionEntry(ProductionEntryModel productionEntryModel)
        {
            int result = 0;
            int foodMenuResult = 0, ingredientResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                var refNoQuery = $"SELECT ISNULL(MAX(convert(int,ReferenceNo)),0) + 1  FROM ProductionEntry where isdeleted = 0; ";
                var referenceNo = con.ExecuteScalar<string>(refNoQuery, null, sqltrans, 0, System.Data.CommandType.Text);

                var query = "INSERT INTO [dbo].[ProductionEntry] " +
                             "  ([ProductionFormulaId] " +
                             " ,[FoodmenuType] " +
                             " ,[ReferenceNo] " +
                             " ,[ProductionDate] " + 
                             " ,[ProductionCompletionDate] " +
                             " ,[ActualBatchSize] " +
                             " ,[Status] " +
                             " ,[VariationNotes] " +
                             " ,[Notes] " +
                             " ,[UserIdInserted]  " +
                             " ,[DateInserted]   " +
                             " ,[IsDeleted])     " +
                             "   VALUES           " +
                             "  (@ProductionFormulaId " +
                             " ,@FoodmenuType " +
                              " ,'"+ referenceNo + "' "+
                             " ,@ProductionDate " +
                              " ,GetUtcDate() " +
                             " ,@ActualBatchSize " +
                              " ,@Status " +
                               " ,@VariationNotes " +
                                " ,@Notes, " +
                             "" + LoginInfo.Userid + "," +
                             "   GetUtcDate(),    " +
                             "   0); SELECT CAST(SCOPE_IDENTITY() as int); ";
                result = con.ExecuteScalar<int>(query, productionEntryModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    foreach (var foodmenu in productionEntryModel.productionEntryFoodMenuModels)
                    {
                        var queryDetails = "INSERT INTO [dbo].[ProductionEntryFoodmenu]" +
                                             "  ([ProductionEntryId] " +
                                             " ,[FoodMenuId] " +
                                             " ,[ExpectedOutput] " +
                                             " ,[AllocationOutput] " +
                                             " ,[ActualOutput] " +
                                             " ,[UserIdInserted]" +
                                             " ,[DateInserted]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              foodmenu.FoodMenuId + "," +
                                              foodmenu.ExpectedOutput + "," +
                                               foodmenu.AllocationOutput + "," +
                                                foodmenu.ActualOutput + "," +
                                    LoginInfo.Userid + ",GetUtcDate(),0);";
                        foodMenuResult = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    foreach (var ingredient in productionEntryModel.productionEntryIngredientModels)
                    {

                        var queryDetails = "INSERT INTO [dbo].[ProductionEntryIngredient]" +
                                             "  ([ProductionEntryId] " +
                                             " ,[IngredientId] " +
                                             " ,[IngredientQty] " +
                                             " ,[ActualIngredientQty] " +
                                             " ,[UserIdInserted]" +
                                             " ,[DateInserted]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              ingredient.IngredientId + "," +
                                              ingredient.IngredientQty + "," +
                                               ingredient.ActualIngredientQty + "," +
                                    LoginInfo.Userid + ",GetUtcDate(),0);";
                        ingredientResult = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (foodMenuResult > 0 && ingredientResult > 0)
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

        public int UpdateProductionEntry(ProductionEntryModel productionEntryModel)
        {
            int result = 0, deleteFoodMenuResult = 0, deleteIngredientResult = 0, foodmenudetails = 0, ingredientdetails = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update [dbo].[ProductionEntry] set " +
                            " ProductionDate = @ProductionDate " +
                            ",ProductionCompletionDate = @ProductionCompletionDate " +
                            ",ActualBatchSize = @ActualBatchSize " +
                            ",Status = @Status " +
                            ",VariationNotes = @VariationNotes " +
                            ",Notes = @Notes " +
                            "  ,[UserIdUpdated] = " + LoginInfo.Userid + " " +
                            "  ,[DateUpdated]  = GetUtcDate()  where id= " + productionEntryModel.Id + ";";
                result = con.Execute(query, productionEntryModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    if (productionEntryModel.FoodMenuDeletedId != null)
                    {
                        foreach (var item in productionEntryModel.FoodMenuDeletedId)
                        {
                            var deleteQuery = $"update ProductionEntryFoodmenu set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            deleteFoodMenuResult = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }


                    if (productionEntryModel.IngredientDeletedId != null)
                    {
                        foreach (var item in productionEntryModel.IngredientDeletedId)
                        {
                            var deleteQuery = $"update ProductionEntryIngredient set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            deleteIngredientResult = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }

                    foreach (var item in productionEntryModel.productionEntryFoodMenuModels)
                    {
                        var queryDetails = string.Empty;
                        if (item.PEFoodMenuId > 0)
                        {
                            queryDetails = "Update [dbo].[ProductionEntryFoodmenu] set " +
                                             "[FoodMenuId]		  	 = " + item.FoodMenuId +
                                             ",[ExpectedOutput]     = " + item.ExpectedOutput +
                                              ",[AllocationOutput]     = " + item.AllocationOutput +
                                               ",[ActualOutput]     = " + item.ActualOutput +
                                             " ,[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                             " [DateUpdated] = GetUTCDate() " +
                                             " where id = " + item.PEFoodMenuId + ";";
                        }
                        foodmenudetails = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    foreach (var item in productionEntryModel.productionEntryIngredientModels)
                    {
                        var queryDetails = string.Empty;
                        if (item.PEIngredientId > 0)
                        {
                            queryDetails = "Update [dbo].[ProductionEntryIngredient] set " +
                                             "[IngredientId]		  	 = " + item.IngredientId +
                                             ",[IngredientQty]     = " + item.IngredientQty +
                                                ",[ActualIngredientQty]     = " + item.ActualIngredientQty +
                                             " ,[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                             " [DateUpdated] = GetUTCDate() " +
                                             " where id = " + item.PEIngredientId + ";";
                        }
                        ingredientdetails = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (foodmenudetails > 0 && ingredientdetails > 0)
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
    }
}
