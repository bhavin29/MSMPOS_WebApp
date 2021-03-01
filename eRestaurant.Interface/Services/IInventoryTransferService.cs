using RocketPOS.Models;
using System;
using System.Collections.Generic;

namespace RocketPOS.Interface.Services
{
    public interface IInventoryTransferService
    {
        List<InventoryTransferViewModel> GetInventoryTransferList();
        List<InventoryTransferViewModel> GetInventoryTransferListByDate(string fromDate, string toDate);
        InventoryTransferModel GetInventoryTransferById(long id);
        int InsertInventoryTransfer(InventoryTransferModel inventoryTransferModel);
        int UpdateInventoryTransfer(InventoryTransferModel inventoryTransferModel);
        int DeleteInventoryTransfer(long id);
        List<InventoryTransferDetailModel> GetInventoryTransferDetail(long id);
        int DeleteInventoryTransferDetail(long id);
        long ReferenceNumber();

        decimal GetFoodMenuStock(int foodMenuId, int storeId);

        InventoryTransferModel GetViewInventoryTransferById(long id);
    }
}
