using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IInventoryRepository
    {
        List<InventoryDetail> GetInventoryDetailList(int storeId, int foodCategoryId);
        int UpdateInventoryDetailList(List<InventoryDetail> inventoryDetails);
    }
}
