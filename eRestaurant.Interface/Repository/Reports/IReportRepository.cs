using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models.Reports;

namespace RocketPOS.Interface.Repository.Reports
{
    public interface IReportRepository
    {
        List<InventoryReportModel> GetInventoryReport(InventoryReportParamModel inventoryReportParamModel);
        List<InventoryReportModel> GetInventoryStockList(int supplierId, int storeId, int itemType);
        List<InventoryDetailReportModel> GetInventoryDetailReport(InventoryReportParamModel inventoryReportParamModel, int id);
        List<PurchaseReportModel> GetPurchaseReport(DateTime fromDate, DateTime toDate);
        List<OutletRegisterReportModel> GetOutletRegisterReport(int OutletRegisterId);
        PrintReceiptA4 GetPrintReceiptA4Detail(int CustomerOrderId);
        List<DataHistorySyncReportModel> GetDataSyncHistoryReport();
    }
}
