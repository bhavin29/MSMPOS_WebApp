using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IAssetItemService
    {

        AssetItemModel GetAssetItemById(int id);
        List<AssetItemModel> GetAssetItemList();

        int InsertAssetItem(AssetItemModel assetItemModel);

        int UpdateAssetItem(AssetItemModel assetItemModel);

        int DeleteAssetItem(int id);
    }
}
