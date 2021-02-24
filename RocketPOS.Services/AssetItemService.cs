using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Services
{
    public class AssetItemService : IAssetItemService
    {
        private readonly IAssetItemRepository iAssetItemRepository;

        public AssetItemService(IAssetItemRepository assetItemRepository)
        {
            iAssetItemRepository = assetItemRepository;
        }

        public int DeleteAssetItem(int id)
        {
            return iAssetItemRepository.DeleteAssetItem(id);
        }

        public AssetItemModel GetAssetItemById(int id)
        {
            return iAssetItemRepository.GetAssetItemById(id);
        }

        public List<AssetItemModel> GetAssetItemList()
        {
            return iAssetItemRepository.GetAssetItemList();
        }

        public int InsertAssetItem(AssetItemModel assetItemModel)
        {
            return iAssetItemRepository.InsertAssetItem(assetItemModel);
        }

        public int UpdateAssetItem(AssetItemModel assetItemModel)
        {
            return iAssetItemRepository.UpdateAssetItem(assetItemModel);
        }
    }
}
