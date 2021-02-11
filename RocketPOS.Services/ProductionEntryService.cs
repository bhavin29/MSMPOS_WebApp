using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Services
{
    public class ProductionEntryService : IProductionEntryService
    {
        private readonly IProductionEntryRepository iProductionEntryRepository;

        public ProductionEntryService(IProductionEntryRepository productionEntryRepository)
        {
            iProductionEntryRepository = productionEntryRepository;
        }

        public ProductionEntryModel GetProductionFormulaById(int id)
        {
            ProductionEntryModel productionEntryModel = new ProductionEntryModel();
            productionEntryModel = iProductionEntryRepository.GetProductionFormulaById(id);
            productionEntryModel.productionEntryFoodMenuModels = iProductionEntryRepository.GetProductionFormulaFoodMenuDetails(id);
            productionEntryModel.productionEntryIngredientModels = iProductionEntryRepository.GetProductionFormulaIngredientDetails(id);
            return productionEntryModel;
        }
    }
}
