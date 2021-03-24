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
            return _iReportRepository.GetInventoryDetailReport(inventoryReportParamModel,id);
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
            return _iReportRepository.GetPurchaseReport(fromDate,toDate);
        }

        public List<InventoryReportModel> GetInventoryStockList(int supplierId, int storeId, int itemType, int active,string reportDate)
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
    }
}
