using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IAssetCategoryRepository
    {
        List<AssetCategoryModel> GetAssetCategoryList();

        int InsertAssetCategory(AssetCategoryModel assetCategoryModel);

        int UpdateAssetCategory(AssetCategoryModel assetCategoryModel);

        int DeleteAssetCategory(int foodCategoryId);
    }
}
