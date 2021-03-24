using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models.Reports;

namespace RocketPOS.Interface.Repository.Reports
{
    public interface IReportRepository
    {
        List<InventoryReportModel> GetInventoryReport(InventoryReportParamModel inventoryReportParamModel);
        List<InventoryReportModel> GetInventoryStockList(int supplierId, int storeId, int itemType, int active,string reportDate);
        List<InventoryDetailReportModel> GetInventoryDetailReport(InventoryReportParamModel inventoryReportParamModel, int id);
        List<PurchaseReportModel> GetPurchaseReport(DateTime fromDate, DateTime toDate);
        List<OutletRegisterReportModel> GetOutletRegisterReport(int OutletRegisterId);
        PrintReceiptA4 GetPrintReceiptA4Detail(int CustomerOrderId);
        List<DataHistorySyncReportModel> GetDataSyncHistoryReport();

        List<MasterSalesReportModel> GetMasterSaleReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId);
    }
}
