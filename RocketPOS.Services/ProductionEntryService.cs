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

        public int DeleteProductionEntryById(int id)
        {
            return iProductionEntryRepository.DeleteProductionEntryById(id);
        }

        public ProductionEntryModel GetProductionEntryById(int id)
        {
            ProductionEntryModel productionEntryModel = new ProductionEntryModel();
 
            productionEntryModel = iProductionEntryRepository.GetProductionEntryById(id);
            productionEntryModel.productionEntryFoodMenuModels = iProductionEntryRepository.GetProductionEntryFoodMenuDetails(id);
            productionEntryModel.productionEntryIngredientModels = iProductionEntryRepository.GetProductionEntryIngredientDetails(id);

            return productionEntryModel;
        }

        public List<ProductionEntryViewModel> GetProductionEntryList(int foodmenuType)
        {
            return iProductionEntryRepository.GetProductionEntryList(foodmenuType);
        }

        public ProductionEntryModel GetProductionFormulaById(int id)
        {
            ProductionEntryModel productionEntryModel = new ProductionEntryModel();
            productionEntryModel = iProductionEntryRepository.GetProductionFormulaById(id);
            productionEntryModel.productionEntryFoodMenuModels = iProductionEntryRepository.GetProductionFormulaFoodMenuDetails(id);
            productionEntryModel.productionEntryIngredientModels = iProductionEntryRepository.GetProductionFormulaIngredientDetails(id);
            return productionEntryModel;
        }

        public int InsertProductionEntry(ProductionEntryModel productionEntryModel)
        {
            return iProductionEntryRepository.InsertProductionEntry(productionEntryModel);
        }

        public int UpdateProductionEntry(ProductionEntryModel productionEntryModel)
        {
            return iProductionEntryRepository.UpdateProductionEntry(productionEntryModel);
        }
    }
}
