using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
    
namespace RocketPOS.Interface.Repository
{
    public interface IInventoryTransferRepository
    {
        List<InventoryTransferViewModel> GetInventoryTransferList();
        List<InventoryTransferViewModel> GetInventoryTransferListByDate(string fromDate, string toDate);
        List<InventoryTransferModel> GetInventoryTransferById(long id);
        int InsertInventoryTransfer(InventoryTransferModel inventoryTransferModel);
        int UpdateInventoryTransfer(InventoryTransferModel inventoryTransferModel);
        int DeleteInventoryTransfer(long id);
        List<InventoryTransferDetailModel> GetInventoryTransferDetail(long id);
        int DeleteInventoryTransferDetail(long id);
        long ReferenceNumber();
        decimal GetFoodMenuStock(int foodMenuId, int storeId);
    }
}
