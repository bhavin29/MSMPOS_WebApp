using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Interface.Services.Reports;
using RocketPOS.Models.Reports;
using RocketPOS.Interface.Repository.Reports;

namespace RocketPOS.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _iReportRepository;

        public ReportService(IReportRepository iReportRepository)
        {
            _iReportRepository = iReportRepository;
        }

        public List<InventoryReportModel> GetInventoryReport(InventoryReportParamModel inventoryReportParamModel)
        {
            return _iReportRepository.GetInventoryReport(inventoryReportParamModel);
        }

        public List<InventoryDetailReportModel> GetInventoryDetailReport(InventoryReportParamModel inventoryReportParamModel, int id)
        {
            return _iReportRepository.GetInventoryDetailReport(inventoryReportParamModel, id);
        }
        public List<OutletRegisterReportModel> GetOutletRegisterReport(int OutletRegisterId)
        {
            return _iReportRepository.GetOutletRegisterReport(OutletRegisterId);
        }

        public PrintReceiptA4 GetPrintReceiptA4Detail(int CustomerOrderId)
        {
            return _iReportRepository.GetPrintReceiptA4Detail(CustomerOrderId);
        }

        public List<PurchaseReportModel> GetPurchaseReport(DateTime fromDate, DateTime toDate)
        {
            return _iReportRepository.GetPurchaseReport(fromDate, toDate);
        }

        public List<InventoryReportModel> GetInventoryStockList(int supplierId, int storeId, int itemType, int active, string reportDate)
        {
            return _iReportRepository.GetInventoryStockList(supplierId, storeId, itemType, active, reportDate);
        }

        public List<DataHistorySyncReportModel> GetDataSyncHistoryReport()
        {
            return _iReportRepository.GetDataSyncHistoryReport();
        }

        public List<MasterSalesReportModel> GetMasterSaleReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        {
            return _iReportRepository.GetMasterSaleReport(fromDate, toDate, categoryId, foodMenuId, outletId);
        }

        public List<DetailedDailyReportModel> GetDetailedDailyByDate(string Fromdate, string Todate, int outletId)
        { 
            return _iReportRepository.GetDetailedDailyByDate(Fromdate, Todate, outletId); 
        }
        public List<DetailSaleSummaryModel> GetDetailSaleSummaryReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        { 
            return _iReportRepository.GetDetailSaleSummaryReport(fromDate, toDate, categoryId, foodMenuId, outletId); 
        }
        public List<ProductWiseSalesReportModel> GetProductWiseSales(string Fromdate, string Todate, string ReportType, int outletId)
        { 
            return _iReportRepository.GetProductWiseSales(Fromdate, Todate, ReportType, outletId);
        }
        public List<SalesByCategoryProductModel> GetSaleByCategorySectionReport(string fromDate, string toDate, string reportName, int categoryId, int foodMenuId, int outletId)
        { 
            return _iReportRepository.GetSaleByCategorySectionReport(fromDate, toDate, reportName, categoryId, foodMenuId, outletId); 
        }
        public List<TableStatisticsModel> GetTableStatisticsReport(string fromDate, string toDate, int outletId)
        { 
            return _iReportRepository.GetTableStatisticsReport(fromDate, toDate, outletId); 
        }
        public List<SalesSummaryModel> GetSalesSummaryByFoodCategoryReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        { 
            return _iReportRepository.GetSalesSummaryByFoodCategoryReport(fromDate, toDate, categoryId, foodMenuId, outletId); 
        }
        public List<SalesSummaryByFoodCategoryFoodMenuModel> GetSalesSummaryByFoodCategoryFoodMenuReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        { 
            return _iReportRepository.GetSalesSummaryByFoodCategoryFoodMenuReport(fromDate, toDate, categoryId, foodMenuId, outletId); 
        }
        public List<SalesSummaryBySectionModel> GetSalesSummaryBySectionReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        { 
            return _iReportRepository.GetSalesSummaryBySectionReport(fromDate, toDate, categoryId, foodMenuId, outletId); 
        }
        public List<CustomerRewardModel> GetCustomerRewardReport(string fromDate, string toDate, string customerPhone, string customerName, int outletId)
        { 
            return _iReportRepository.GetCustomerRewardReport(fromDate, toDate, customerPhone, customerName, outletId); 
        }
        public List<SalesSummaryByWeek> GetSalesSummaryByWeekReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        { 
            return _iReportRepository.GetSalesSummaryByWeekReport(fromDate, toDate, categoryId, foodMenuId, outletId); 
        }
        public List<SalesSummaryByHours> GetSalesSummaryByHoursReport(string fromDate, string toDate, int outletId)
        { 
            return _iReportRepository.GetSalesSummaryByHoursReport(fromDate, toDate, outletId); 
        }
   }
}
