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
        public List<InventoryDetail> GetInventoryDetailList(int storeId, int foodCategoryId, int itemType)
        {
            return _iInventoryRepository.GetInventoryDetailList(storeId,foodCategoryId, itemType);
        }

        public int UpdateInventoryDetailList(List<InventoryDetail> inventoryDetails)
        {
            return _iInventoryRepository.UpdateInventoryDetailList(inventoryDetails);
        }

        public string StockUpdate(int storeId, int foodmenuId, int itemType)
        {
            return _iInventoryRepository.StockUpdate(storeId, foodmenuId, itemType);
        }
        public List<InventoryOpenigStockImport> GetInventoryOpeningStockByStore(int storeId, int foodCategoryId)
        {
            return _iInventoryRepository.GetInventoryOpeningStockByStore(storeId, foodCategoryId);

        }
        int IInventoryService.BulkImport(List<InventoryOpenigStockImport> inventoryOpenigStockImports)
        {
            return _iInventoryRepository.BulkImport(inventoryOpenigStockImports);
        }

    }
}
