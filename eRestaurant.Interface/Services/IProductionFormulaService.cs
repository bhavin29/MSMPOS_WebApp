using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IProductionFormulaService
    {
        UnitModel GetUnitNameByFoodMenuId(int foodMenuId);
        ProductionFormulaModel GetProductionFormulaById(int id);
        int InsertProductionFormula(ProductionFormulaModel productionFormulaModel);
        int UpdateProductionFormula(ProductionFormulaModel productionFormulaModel);

        List<ProductionFormulaViewModel> GetProductionFormulaList(int foodmenuType);
        int DeleteProductionFormulaById(int id);
    }
}
