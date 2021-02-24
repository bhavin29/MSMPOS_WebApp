using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IAssetCategoryService
    {
        List<AssetCategoryModel> GetAssetCategoryList();

        AssetCategoryModel GetAssetCategoryById(int id);

        int InsertAssetCategory(AssetCategoryModel assetCategoryModel);

        int UpdateAssetCategory(AssetCategoryModel assetCategoryModel);

        int DeleteAssetCategory(int foodCategoryId);
    }
}
