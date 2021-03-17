using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IProductionEntryService
    {
        ProductionEntryModel GetProductionFormulaById(int id);
        int UpdateProductionEntry(ProductionEntryModel productionEntryModel);
        int InsertProductionEntry(ProductionEntryModel productionEntryModel);

        List<ProductionEntryViewModel> GetProductionEntryList(int foodmenuType, string fromDate, string toDate, int statusId);
        int DeleteProductionEntryById(int id);
        ProductionEntryModel GetProductionEntryById(int id);
        ProductionEntryModel GetProductionEntryViewById(int id);
    }
}
