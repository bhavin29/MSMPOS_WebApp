using Dapper;
using Microsoft.Extensions.Options;
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
    }
}
