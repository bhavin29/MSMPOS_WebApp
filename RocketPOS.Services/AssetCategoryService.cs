using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class AssetCategoryService: IAssetCategoryService
    {
        private readonly IAssetCategoryRepository iAssetCategoryRepository;

        public AssetCategoryService(IAssetCategoryRepository assetCategoryRepository)
        {
            iAssetCategoryRepository = assetCategoryRepository;
        }

        public int DeleteAssetCategory(int id)
        {
            return iAssetCategoryRepository.DeleteAssetCategory(id);
        }

        public AssetCategoryModel GetAssetCategoryById(int id)
        {
            return iAssetCategoryRepository.GetAssetCategoryList().Where(x => x.Id == id).FirstOrDefault();
        }

        public List<AssetCategoryModel> GetAssetCategoryList()
        {
            return iAssetCategoryRepository.GetAssetCategoryList();
        }

        public int InsertAssetCategory(AssetCategoryModel assetCategoryModel)
        {
            return iAssetCategoryRepository.InsertAssetCategory(assetCategoryModel);
        }

        public int UpdateAssetCategory(AssetCategoryModel assetCategoryModel)
        {
            return iAssetCategoryRepository.UpdateAssetCategory(assetCategoryModel);
        }
    }
}
