using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IInventoryAlterationService
    {
        string ReferenceNumberInventoryAlteration();
        int InsertInventoryAlteration(InventoryAlterationModel inventoryAlterationModel);

        List<InventoryAlterationViewListModel> GetInventoryAlterationList(int storeId, DateTime fromDate, DateTime toDate, int foodMenuId);

        decimal GetInventoryStockQty(int storeId, int foodMenuId);
    }
}
