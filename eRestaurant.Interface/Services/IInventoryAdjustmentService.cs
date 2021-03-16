using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface  IInventoryAdjustmentService
    {
        List<InventoryAdjustmentViewModel> GetInventoryAdjustmentList(int consumptionStatus);
        List<InventoryAdjustmentViewModel> InventoryAdjustmentListByDate(string fromDate, string toDate, int consumptionStatus);
        InventoryAdjustmentModel GetInventoryAdjustmentById(long invAdjId);
        int InsertInventoryAdjustment(InventoryAdjustmentModel inventoryAdjustmentModel);
        int UpdateInventoryAdjustment(InventoryAdjustmentModel inventoryAdjustmentModel);
        int DeleteInventoryAdjustment(long PurchaseId);
        List<InventoryAdjustmentDetailModel> GetInventoryAdjustmentDetail(long invAdjId);
        int DeleteInventoryAdjustmentDetail(long invAdjId);
        long ReferenceNumber();

        decimal GetFoodMenuPurchasePrice(int foodMenuId);

        InventoryAdjustmentModel GetViewInventoryAdjustmentById(long invAdjId);
    }
}
