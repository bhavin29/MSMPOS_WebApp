using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using RocketPOS.Models;
namespace RocketPOS.Interface.Repository
{
    public interface IWasteRepository
    {
        List<WasteListModel> GetWasteList();
        int InsertWaste(WasteModel purchaseModel);
        int UpdateWaste(WasteModel purchaseModel);
        int DeleteWaste(long WasteId);
        List<WasteDetailModel> GetWasteDetails(long purchaseId);
        List<WasteModel> GetWasteById(long purchaseId);
        int DeleteWasteDetails(long wasteId, long foodManuId, long ingredientId);
        long ReferenceNumber();
        List<DropDownModel> FoodMenuListForLostAmount();
        List<DropDownModel> IngredientListForLostAmount();

        decimal GetFoodMenuPurchasePrice(int id);
        decimal GetIngredientPurchasePrice(int id);
    }
}
