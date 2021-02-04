using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _iInventoryRepository;
        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _iInventoryRepository = inventoryRepository;
        }
        public List<InventoryDetail> GetInventoryDetailList(int storeId, int foodCategoryId)
        {
            return _iInventoryRepository.GetInventoryDetailList(storeId,foodCategoryId);
        }

        public int UpdateInventoryDetailList(List<InventoryDetail> inventoryDetails)
        {
            return _iInventoryRepository.UpdateInventoryDetailList(inventoryDetails);
        }

        public string StockUpdate(int storeId, int foodmenuId)
        {
            return _iInventoryRepository.StockUpdate(storeId, foodmenuId);
        }
    }
}
