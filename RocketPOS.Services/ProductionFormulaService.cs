using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Services
{
    public class ProductionFormulaService : IProductionFormulaService
    {
        private readonly IProductionFormulaRepository iProductionFormulaRepository;

        public ProductionFormulaService(IProductionFormulaRepository productionFormulaRepository)
        {
            iProductionFormulaRepository = productionFormulaRepository;
        }

        public int DeleteProductionFormulaById(int id)
        {
            return iProductionFormulaRepository.DeleteProductionFormulaById(id);
        }

        public ProductionFormulaModel GetProductionFormulaById(int id)
        {
            ProductionFormulaModel productionFormulaModel = new ProductionFormulaModel();
            productionFormulaModel = iProductionFormulaRepository.GetProductionFormulaById(id);
            productionFormulaModel.productionFormulaFoodMenuModels= iProductionFormulaRepository.GetProductionFormulaFoodMenuDetails(id);
            productionFormulaModel.productionFormulaIngredientModels = iProductionFormulaRepository.GetProductionFormulaIngredientDetails(id);
            return productionFormulaModel;
        }

        public List<ProductionFormulaViewModel> GetProductionFormulaList(int foodmenuType)
        {
            return iProductionFormulaRepository.GetProductionFormulaList(foodmenuType);
        }

        public string GetUnitNameByFoodMenuId(int foodMenuId)
        {
            return iProductionFormulaRepository.GetUnitNameByFoodMenuId(foodMenuId);
        }

        public int InsertProductionFormula(ProductionFormulaModel productionFormulaModel)
        {
            return iProductionFormulaRepository.InsertProductionFormula(productionFormulaModel);
        }

        public int UpdateProductionFormula(ProductionFormulaModel productionFormulaModel)
        {
            return iProductionFormulaRepository.UpdateProductionFormula(productionFormulaModel);
        }
    }
}
