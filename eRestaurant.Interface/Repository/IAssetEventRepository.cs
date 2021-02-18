using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IAssetEventRepository
    {
        string ReferenceNumberAssetEvent();
        int UpdateAssetEvent(AssetEventModel assetEventModel);
        int InsertAssetEvent(AssetEventModel assetEventModel);
        List<AssetEventViewModel> GetAssetEventList(bool isHistory);
        int DeleteAssetEven(int id);
        AssetEventModel GetAssetEventById(int id);
        List<AssetEventItemModel> GetAssetEventItemDetails(int assetEventId);
        List<AssetEventFoodmenuModel> GetAssetEventFoodmenuDetails(int assetEventId);
        List<AssetEventIngredientModel> GetAssetIngredientDetails(int assetEventId);
        AssetFoodMenuPriceDetail GetFoodMenuPriceTaxDetailById(int id);
        decimal GetAssetItemPriceById(int id);
        decimal GetIngredientPriceById(int id);

        string GetAssetItemUnitName(int id);
    }
}
