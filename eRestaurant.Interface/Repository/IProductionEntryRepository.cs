using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IProductionEntryRepository
    {
        ProductionEntryModel GetProductionFormulaById(int id);
        List<ProductionEntryFoodMenuModel> GetProductionFormulaFoodMenuDetails(int productionFormulaId);
        List<ProductionEntryIngredientModel> GetProductionFormulaIngredientDetails(int productionFormulaId);
    }
}
