using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IProductionFormulaRepository
    {
        string GetUnitNameByFoodMenuId(int foodMenuId);
        ProductionFormulaModel GetProductionFormulaById(int id);
        List<ProductionFormulaFoodMenuModel> GetProductionFormulaFoodMenuDetails(int productionFormulaId);
        List<ProductionFormulaIngredientModel> GetProductionFormulaIngredientDetails(int productionFormulaId);
        int InsertProductionFormula(ProductionFormulaModel productionFormulaModel);
        int UpdateProductionFormula(ProductionFormulaModel productionFormulaModel);

        List<ProductionFormulaViewModel> GetProductionFormulaList(int foodmenuType);
        int DeleteProductionFormulaById(int id);
    }
}
