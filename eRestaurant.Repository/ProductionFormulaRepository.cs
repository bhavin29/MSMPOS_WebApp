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
    public class ProductionFormulaRepository : IProductionFormulaRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public ProductionFormulaRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteProductionFormulaById(int id)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update ProductionFormula set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where id = " + id + ";" +
                            " update ProductionFormulaFoodmenu set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where ProductionFormulaId = " + id + ";" +
                            " update ProductionFormulaIngredient set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where ProductionFormulaId = " + id + ";";
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

        public ProductionFormulaModel GetProductionFormulaById(int id)
        {
            ProductionFormulaModel productionFormulaModel = new ProductionFormulaModel();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,FormulaName,[BatchSize],[BatchSizeUnitId],FoodmenuType,IsActive  from ProductionFormula Where IsDeleted=0 And Id=" + id;
                productionFormulaModel = con.QueryFirstOrDefault<ProductionFormulaModel>(query);
            }
            return productionFormulaModel;
        }

        public List<ProductionFormulaFoodMenuModel> GetProductionFormulaFoodMenuDetails(int productionFormulaId)
        {
            List<ProductionFormulaFoodMenuModel> productionFormulaFoodMenuDetail = new List<ProductionFormulaFoodMenuModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select PFM.Id AS PFFoodMenuId,PFM.FoodMenuId,FM.FoodMenuName,PFM.ExpectedOutput, U.Unitname as FoodMenuUnitName " +
                            " from ProductionFormulaFoodmenu PFM " +
                            " Inner Join FoodMenu FM On FM.Id = PFM.FoodMenuId inner join Units U ON U.Id = FM.UnitsId " +
                            " Where PFM.IsDeleted = 0 And PFM.ProductionFormulaId =" + productionFormulaId;
                productionFormulaFoodMenuDetail = con.Query<ProductionFormulaFoodMenuModel>(query).AsList();
            }
            return productionFormulaFoodMenuDetail;
        }

        public List<ProductionFormulaIngredientModel> GetProductionFormulaIngredientDetails(int productionFormulaId)
        {
            List<ProductionFormulaIngredientModel> productionFormulaIngredientDetail = new List<ProductionFormulaIngredientModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select PFI.Id AS PFIngredientId,PFI.IngredientId,I.IngredientName,PFI.IngredientQty, U.Unitname as IngredientUnitName " +
                            " from ProductionFormulaIngredient PFI " +
                            " Inner Join Ingredient I On I.Id=PFI.IngredientId inner join Units U ON U.Id = I.ingredientUnitId  " +
                            " Where PFI.IsDeleted=0 And PFI.ProductionFormulaId=" + productionFormulaId;
                productionFormulaIngredientDetail = con.Query<ProductionFormulaIngredientModel>(query).AsList();
            }
            return productionFormulaIngredientDetail;
        }

        public List<ProductionFormulaViewModel> GetProductionFormulaList(int foodmenuType)
        {
            List<ProductionFormulaViewModel> productionFormulaList = new List<ProductionFormulaViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select PF.Id,PF.FormulaName,PF.[BatchSize],PF.FoodmenuType,PF.IsActive ,isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as Username  from ProductionFormula  PF " +
                            " inner join [User] U On U.Id=PF.UserIdInserted  inner join employee e on e.id = u.employeeid  where PF.IsDeleted=0 ";

                if (foodmenuType != 0)
                {
                    query += " And PF.FoodmenuType = " + foodmenuType;
                }
                else
                {
                    query += " And PF.FoodmenuType = 2";
                }
                productionFormulaList = con.Query<ProductionFormulaViewModel>(query).AsList();
            }
            return productionFormulaList;
        }

        public UnitModel GetUnitNameByFoodMenuId(int foodMenuId)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select U.Id,U.UnitShortName AS UnitName from FoodMenu F Inner Join Units U On U.Id=F.UnitsId Where F.IsDeleted=0 And F.Id=" + foodMenuId;
                return con.QueryFirstOrDefault<UnitModel>(query);
            }
        }

        public int InsertProductionFormula(ProductionFormulaModel productionFormulaModel)
        {
            int result = 0;
            if (productionFormulaModel.FoodmenuType == 2)
            {
                result = GeProductionFormulaFoodmenuExists(productionFormulaModel.Id, productionFormulaModel.productionFormulaFoodMenuModels[0].FoodMenuId);
                if (result >0)
                {
                    return -1;
                }
            }
            int foodMenuResult = 0, ingredientResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[ProductionFormula] " +
                             "  ([FormulaName] " +
                             " ,[BatchSize] " +
                             " ,[BatchSizeUnitId] " +
                             " ,[FoodmenuType] " +
                             " ,[IsActive] " +
                             " ,[UserIdInserted]  " +
                             " ,[DateInserted]   " +
                             " ,[IsDeleted])     " +
                             "   VALUES           " +
                             "  (@FormulaName " +
                             " ,@BatchSize " +
                              " ,@BatchSizeUnitId " +
                             " ,@FoodmenuType " +
                             " ,@IsActive, " +
                             "" + LoginInfo.Userid + "," +
                             "   GetUtcDate(),    " +
                             "   0); SELECT CAST(SCOPE_IDENTITY() as int); ";
                result = con.ExecuteScalar<int>(query, productionFormulaModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    foreach (var foodmenu in productionFormulaModel.productionFormulaFoodMenuModels)
                    {
                        var queryDetails = "INSERT INTO [dbo].[ProductionFormulaFoodmenu]" +
                                             "  ([ProductionFormulaId] " +
                                             " ,[FoodMenuId] " +
                                             " ,[ExpectedOutput] " +
                                             " ,[UserIdInserted]" +
                                             " ,[DateInserted]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              foodmenu.FoodMenuId + "," +
                                              foodmenu.ExpectedOutput + "," +
                                    LoginInfo.Userid + ",GetUtcDate(),0);";
                        foodMenuResult = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    foreach (var ingredient in productionFormulaModel.productionFormulaIngredientModels)
                    {

                        var queryDetails = "INSERT INTO [dbo].[ProductionFormulaIngredient]" +
                                             "  ([ProductionFormulaId] " +
                                             " ,[IngredientId] " +
                                             " ,[IngredientQty] " +
                                             " ,[UserIdInserted]" +
                                             " ,[DateInserted]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              ingredient.IngredientId + "," +
                                              ingredient.IngredientQty + "," +
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

        public int UpdateProductionFormula(ProductionFormulaModel productionFormulaModel)
        {
            int result = 0, deleteFoodMenuResult = 0, deleteIngredientResult = 0, foodmenudetails = 0, ingredientdetails = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update [dbo].[ProductionFormula] set " +
                            " FormulaName = @FormulaName " +
                            ",BatchSize = @BatchSize " +
                            ",BatchSizeUnitId = @BatchSizeUnitId " +
                            ",IsActive = @IsActive " +
                            "  ,[UserIdUpdated] = " + LoginInfo.Userid + " " +
                            "  ,[DateUpdated]  = GetUtcDate()  where id= " + productionFormulaModel.Id + ";";
                result = con.Execute(query, productionFormulaModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    if (productionFormulaModel.FoodMenuDeletedId != null)
                    {
                        foreach (var item in productionFormulaModel.FoodMenuDeletedId)
                        {
                            var deleteQuery = $"update ProductionFormulaFoodmenu set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            deleteFoodMenuResult = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }


                    if (productionFormulaModel.IngredientDeletedId != null)
                    {
                        foreach (var item in productionFormulaModel.IngredientDeletedId)
                        {
                            var deleteQuery = $"update ProductionFormulaIngredient set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            deleteIngredientResult = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }

                    foreach (var item in productionFormulaModel.productionFormulaFoodMenuModels)
                    {
                        var queryDetails = string.Empty;
                        if (item.PFFoodMenuId > 0)
                        {
                            queryDetails = "Update [dbo].[ProductionFormulaFoodmenu] set " +
                                             "[FoodMenuId]		  	 = " + item.FoodMenuId +
                                             ",[ExpectedOutput]     = " + item.ExpectedOutput +
                                             " ,[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                             " [DateUpdated] = GetUTCDate() " +
                                             " where id = " + item.PFFoodMenuId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[ProductionFormulaFoodmenu]" +
                                              "  ([ProductionFormulaId] " +
                                              " ,[FoodMenuId] " +
                                              " ,[ExpectedOutput] " +
                                              " ,[UserIdInserted]" +
                                              " ,[DateInserted]" +
                                               " ,[IsDeleted])   " +
                                               "VALUES           " +
                                               "(" + productionFormulaModel.Id + "," +
                                               item.FoodMenuId + "," +
                                               item.ExpectedOutput + "," +
                                     LoginInfo.Userid + ",GetUtcDate(),0);";
                        }
                        foodmenudetails = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    foreach (var item in productionFormulaModel.productionFormulaIngredientModels)
                    {
                        var queryDetails = string.Empty;
                        if (item.PFIngredientId > 0)
                        {
                            queryDetails = "Update [dbo].[ProductionFormulaIngredient] set " +
                                             "[IngredientId]		  	 = " + item.IngredientId +
                                             ",[IngredientQty]     = " + item.IngredientQty +
                                             " ,[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                             " [DateUpdated] = GetUTCDate() " +
                                             " where id = " + item.PFIngredientId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[ProductionFormulaIngredient]" +
                                            "  ([ProductionFormulaId] " +
                                            " ,[IngredientId] " +
                                            " ,[IngredientQty] " +
                                            " ,[UserIdInserted]" +
                                            " ,[DateInserted]" +
                                             " ,[IsDeleted])   " +
                                             "VALUES           " +
                                             "(" + productionFormulaModel.Id + "," +
                                             item.IngredientId + "," +
                                             item.IngredientQty + "," +
                                   LoginInfo.Userid + ",GetUtcDate(),0);";
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

        public int GeProductionFormulaFoodmenuExists(int id, int foodmenuid)
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string query = " SELECT * FROM Productionformulafoodmenu PFFM " +
                               " inner join Productionformula PF on PF.Id = PFFM.ProductionformulaId " +
                               " WHERE PF.IsDeleted=0 AND PF.ID <> " + id + "  and foodmenuid =" + foodmenuid ;
                result = con.ExecuteScalar<string>(query);
            }

             result = result !=null ? result : "0";

            return Int16.Parse(result);
        }

        public ProductionFormulaModel GetProductionFormulaViewById(int id)
        {
            ProductionFormulaModel productionFormulaModel = new ProductionFormulaModel();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select PF.Id,PF.FormulaName,PF.[BatchSize],PF.[BatchSizeUnitId],PF.FoodmenuType,PF.IsActive,U.UnitName from ProductionFormula PF Inner Join Units U On U.Id=PF.BatchSizeUnitId Where PF.IsDeleted=0 And PF.Id=" + id;
                productionFormulaModel = con.QueryFirstOrDefault<ProductionFormulaModel>(query);
            }
            return productionFormulaModel;
        }

        public List<ProductionFormulaFoodMenuModel> GetProductionFormulaFoodMenuDetailsView(int productionFormulaId)
        {
            List<ProductionFormulaFoodMenuModel> productionFormulaFoodMenuDetail = new List<ProductionFormulaFoodMenuModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select PFM.Id AS PFFoodMenuId,PFM.FoodMenuId,FM.FoodMenuName,PFM.ExpectedOutput, U.Unitname as FoodMenuUnitName " +
                            " from ProductionFormulaFoodmenu PFM " +
                            " Inner Join FoodMenu FM On FM.Id = PFM.FoodMenuId inner join Units U ON U.Id = FM.UnitsId " +
                            " Where PFM.IsDeleted = 0 And PFM.ProductionFormulaId =" + productionFormulaId;
                productionFormulaFoodMenuDetail = con.Query<ProductionFormulaFoodMenuModel>(query).AsList();
            }
            return productionFormulaFoodMenuDetail;
        }

        public List<ProductionFormulaIngredientModel> GetProductionFormulaIngredientDetailsView(int productionFormulaId)
        {
            List<ProductionFormulaIngredientModel> productionFormulaIngredientDetail = new List<ProductionFormulaIngredientModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select PFI.Id AS PFIngredientId,PFI.IngredientId,I.IngredientName,PFI.IngredientQty, U.Unitname as IngredientUnitName " +
                            " from ProductionFormulaIngredient PFI " +
                            " Inner Join Ingredient I On I.Id=PFI.IngredientId inner join Units U ON U.Id = I.ingredientUnitId  " +
                            " Where PFI.IsDeleted=0 And PFI.ProductionFormulaId=" + productionFormulaId;
                productionFormulaIngredientDetail = con.Query<ProductionFormulaIngredientModel>(query).AsList();
            }
            return productionFormulaIngredientDetail;
        }
    }
}
