using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IWasteService
    {
        List<WasteListModel> GetWasteList();
        int InsertWaste(WasteModel purchaseModel);
        int UpdateWaste(WasteModel purchaseModel);
        int DeleteWaste(long WasteId);
        List<WasteDetailModel> GetWasteDetails(long purchaseId);
        WasteModel GetWasteById(long purchaseId);
        int DeleteWasteDetails(long wasteId, long foodManuId,  long ingredientId);
        long ReferenceNumber();
        List<SelectListItem> FoodMenuListForLostAmount();
        List<SelectListItem> IngredientListForLostAmount();

        decimal GetFoodMenuPurchasePrice(int id);
        decimal GetIngredientPurchasePrice(int id);
    }
}
