using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using RocketPOS.Models.Reports;

namespace RocketPOS.Interface.Services.Reports
{
    public interface IReportService
    {
        List<InventoryReportModel> GetInventoryReport(InventoryReportParamModel inventoryReportParamModel);
        List<InventoryReportModel> GetInventoryStockList(int supplierId, int storeId, int itemType, int active,string reportDate);
        List<InventoryDetailReportModel> GetInventoryDetailReport(InventoryReportParamModel inventoryReportParamModel, int id);
        List<PurchaseReportModel> GetPurchaseReport(DateTime fromDate, DateTime toDate);
        List<OutletRegisterReportModel> GetOutletRegisterReport(int OutletRegisterId);
        PrintReceiptA4 GetPrintReceiptA4Detail(int CustomerOrderId);
        List<DataHistorySyncReportModel> GetDataSyncHistoryReport();
        List<MasterSalesReportModel> GetMasterSaleReport(string fromDate, string toDate, int categoryId, int foodMenuId,int outletId);
        List<DetailedDailyReportModel> GetDetailedDailyByDate(string Fromdate, string Todate, int outletId);
        List<DetailSaleSummaryModel> GetDetailSaleSummaryReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId);
        List<ProductWiseSalesReportModel> GetProductWiseSales(string Fromdate, string Todate, string ReportType, int outletId);
        List<SalesByCategoryProductModel> GetSaleByCategorySectionReport(string fromDate, string toDate, string reportName, int categoryId, int foodMenuId, int outletId);
        List<TableStatisticsModel> GetTableStatisticsReport(string fromDate, string toDate, int outletId);
        List<SalesSummaryModel> GetSalesSummaryByFoodCategoryReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId);
        List<SalesSummaryByFoodCategoryFoodMenuModel> GetSalesSummaryByFoodCategoryFoodMenuReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId);
        List<SalesSummaryBySectionModel> GetSalesSummaryBySectionReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId);
        List<CustomerRewardModel> GetCustomerRewardReport(string fromDate, string toDate, string customerPhone, string customerName, int outletId);
        List<SalesSummaryByWeek> GetSalesSummaryByWeekReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId);
        List<SalesSummaryByHours> GetSalesSummaryByHoursReport(string fromDate, string toDate, int outletId);
        List<TallySalesVoucherModel> GetSalesVoucherData(string fromDate, string toDate, int outletId);
        void GenerateSalesVoucher(string fromDate, string toDate, int outletId, string path);
        void SerializeToXml<T>(T anyobject, string xmlFilePath);
        CessReportModel GetCessReport(string fromDate, string toDate, int outletId,string reporttype);
        CessCategoryReportModel GetCessCategoryReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId);
        List<ModeofPaymentReportModel> GetModOfPaymentReport(string fromDate, string toDate, int outletId);
        List<WasteReportModel> GetWasteReport(string fromDate, string toDate, int storeId, string reporttype);
        List<PurchaseReportModel> GetPurchaseReport(string fromDate, string toDate, int storeId, string reporttype);

    }
}
