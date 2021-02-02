using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IInventoryService
    {
        List<InventoryDetail> GetInventoryDetailList(int storeId, int foodCategoryId);
        int UpdateInventoryDetailList(List<InventoryDetail> inventoryDetails);
        string StockUpdate();
    }
}
