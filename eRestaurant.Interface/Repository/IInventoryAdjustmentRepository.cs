using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IInventoryAdjustmentRepository
    {
        List<InventoryAdjustmentViewModel> GetInventoryAdjustmentList();
        List<InventoryAdjustmentViewModel> InventoryAdjustmentListByDate(string fromDate, string toDate);
        List<InventoryAdjustmentModel> GetInventoryAdjustmentById(long invAdjId);
        int InsertInventoryAdjustment(InventoryAdjustmentModel inventoryAdjustmentModel);
        int UpdateInventoryAdjustment(InventoryAdjustmentModel inventoryAdjustmentModel);
        int DeleteInventoryAdjustment(long PurchaseId);
        List<InventoryAdjustmentDetailModel> GetInventoryAdjustmentDetail(long invAdjId);
        int DeleteInventoryAdjustmentDetail(long invAdjId);
        long ReferenceNumber();
    }
}
