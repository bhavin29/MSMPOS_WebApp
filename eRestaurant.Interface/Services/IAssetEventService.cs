﻿using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IAssetEventService
    {
        string ReferenceNumberAssetEvent();
        int UpdateAssetEvent(AssetEventModel assetEventModel);
        int InsertAssetEvent(AssetEventModel assetEventModel);

        List<AssetEventViewModel> GetAssetEventList();
        int DeleteAssetEven(int id);

        AssetEventModel GetAssetEventById(int id);
        AssetFoodMenuPriceDetail GetFoodMenuPriceTaxDetailById(int id);
        decimal GetAssetItemPriceById(int id);
        decimal GetIngredientPriceById(int id);

        string GetAssetItemUnitName(int id);

        List<AssetEventViewModel> GetCateringListByStatus(string fromDate, string toDate, int statusId);

        int UpdateStockItemById(List<string> ids);
    }
}
