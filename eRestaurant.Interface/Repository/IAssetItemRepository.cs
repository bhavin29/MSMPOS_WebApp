using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IAssetItemRepository
    {
        List<AssetItemModel> GetAssetItemList();

        int InsertAssetItem(AssetItemModel assetItemModel);

        int UpdateAssetItem(AssetItemModel assetItemModel);

        int DeleteAssetItem(int id);

        AssetItemModel GetAssetItemById(int id);
    }
}
