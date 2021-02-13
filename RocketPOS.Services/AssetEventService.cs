using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Services
{
    public class AssetEventService : IAssetEventService
    {
        private readonly IAssetEventRepository _iAssetEventRepository;

        public AssetEventService(IAssetEventRepository assetEventRepository)
        {
            _iAssetEventRepository = assetEventRepository;
        }

        public int DeleteAssetEven(int id)
        {
            return _iAssetEventRepository.DeleteAssetEven(id);
        }

        public AssetEventModel GetAssetEventById(int id)
        {
            AssetEventModel assetEventModel = new AssetEventModel();
            assetEventModel = _iAssetEventRepository.GetAssetEventById(id);
            assetEventModel.assetEventItemModels = _iAssetEventRepository.GetAssetEventItemDetails(id);
            assetEventModel.assetEventFoodmenuModels = _iAssetEventRepository.GetAssetEventFoodmenuDetails(id);
            return assetEventModel;
        }

        public List<AssetEventFoodmenuModel> GetAssetEventFoodmenuDetails(int assetEventId)
        {
            throw new NotImplementedException();
        }

        public List<AssetEventItemModel> GetAssetEventItemDetails(int assetEventId)
        {
            throw new NotImplementedException();
        }

        public List<AssetEventViewModel> GetAssetEventList()
        {
            return _iAssetEventRepository.GetAssetEventList();
        }

        public int InsertAssetEvent(AssetEventModel assetEventModel)
        {
            return _iAssetEventRepository.InsertAssetEvent(assetEventModel);
        }

        public string ReferenceNumberAssetEvent()
        {
            return _iAssetEventRepository.ReferenceNumberAssetEvent();
        }

        public int UpdateAssetEvent(AssetEventModel assetEventModel)
        {
            return _iAssetEventRepository.UpdateAssetEvent(assetEventModel);
        }
    }
}
