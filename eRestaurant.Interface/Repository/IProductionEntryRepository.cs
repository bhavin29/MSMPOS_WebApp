﻿using RocketPOS.Models;
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
        int UpdateProductionEntry(ProductionEntryModel productionEntryModel);
        int InsertProductionEntry(ProductionEntryModel productionEntryModel);

        List<ProductionEntryViewModel> GetProductionEntryList(int foodmenuType, string fromDate, string toDate, int statusId);
        int DeleteProductionEntryById(int id);

        ProductionEntryModel GetProductionEntryById(int id);
        List<ProductionEntryFoodMenuModel> GetProductionEntryFoodMenuDetails(int productionEntryId);
        List<ProductionEntryIngredientModel> GetProductionEntryIngredientDetails(int productionEntryId);
        ProductionEntryModel GetProductionEntryViewById(int id);
        List<ProductionEntryFoodMenuModel> GetProductionEntryFoodMenuDetailsView(int productionEntryId);
        List<ProductionEntryIngredientModel> GetProductionEntryIngredientDetailsView(int productionEntryId);
    }
}
