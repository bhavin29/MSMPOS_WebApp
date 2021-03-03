using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IInventoryAlterationRepository
    {
        string ReferenceNumberInventoryAlteration();
        int InsertInventoryAlteration(InventoryAlterationModel inventoryAlterationModel);

        List<InventoryAlterationViewListModel> GetInventoryAlterationList(int storeId, DateTime fromDate, DateTime toDate, int foodMenuId);
        decimal GetInventoryStockQty(int storeId, int foodMenuId);
        decimal GetInventoryStockQtyForIngredient(int storeId, int ingredientId);

        List<InventoryAlterationModel> GetViewInventoryAlterationById(long invAltId);
        List<InventoryAlterationDetailModel> GetViewInventoryAlterationDetail(long invAltId);
    }
}
