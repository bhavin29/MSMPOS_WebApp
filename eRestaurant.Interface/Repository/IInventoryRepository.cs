using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IInventoryRepository
    {
        List<InventoryDetail> GetInventoryDetailList(int storeId, int foodCategoryId, int itemType);
        int UpdateInventoryDetailList(List<InventoryDetail> inventoryDetails);
        string StockUpdate(int storeId, int foodmenuId, int itemType);
        List<InventoryOpenigStockImport> GetInventoryOpeningStockByStore(int storeId, int foodCategoryId, int itemType);
        int BulkImport(List<InventoryOpenigStockImport> inventoryOpenigStockImports);
    }
}
