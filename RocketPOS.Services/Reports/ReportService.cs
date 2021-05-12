using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Interface.Services.Reports;
using RocketPOS.Models.Reports;
using RocketPOS.Interface.Repository.Reports;
using RocketPOS.Models;
using System.Xml;
using RocketPOS.Framework;
using System.Xml.Serialization;
using System.IO;

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

        public List<TallySalesVoucherModel> GetSalesVoucherData(string fromDate, string toDate, int outletId)
        {
            return _iReportRepository.GetSalesVoucherData(fromDate, toDate, outletId);
        }

        public void GenerateSalesVoucher(string fromDate, string toDate, int outletId, string path)
        {
            List<TallySetupModel> tallySetupModel = new List<TallySetupModel>();
            List<TallySalesVoucherModel> tallySalesVoucherModels = new List<TallySalesVoucherModel>();
            XmlDocument xmldoc = new XmlDocument();
            List<XmlDocument> xmldocVoucher = new List<XmlDocument>();

            tallySetupModel = _iReportRepository.GetTallySetup(outletId);
            tallySalesVoucherModels = _iReportRepository.GetSalesVoucherData(fromDate, toDate, outletId);
            int i = 1;

            foreach (var salesVoucher in tallySalesVoucherModels)
            {
                var clsSalesFields = new SalesFields();
                clsSalesFields.VoucherType = "Sales";
                clsSalesFields.VoucherUniqueID = salesVoucher.BillDate.Replace("/", "");
                clsSalesFields.VoucherNumber = tallySetupModel.Find(x => x.Keyname.Contains("BillPrefix")).LedgerName + salesVoucher.BillDate.Replace("/", "") + salesVoucher.TallyBillPostfix;
                clsSalesFields.VoucherDate = Convert.ToDateTime(salesVoucher.BillDate);
                clsSalesFields.PartyLedgerName = salesVoucher.TallyLedgerName;
                clsSalesFields.EffectiveDate = Convert.ToDateTime(salesVoucher.BillDate);
                clsSalesFields.IsInvoice = "No";
                clsSalesFields.VoucherNarration = "";

                var pcledgerParty = new ALLLedgerEntries();
                pcledgerParty.LedgerName = salesVoucher.TallyLedgerName;
                pcledgerParty.IsDeemedPositive = "Yes";
                pcledgerParty.LedgerFromItem = "No";
                pcledgerParty.RemoveZeroEntries = "No";
                pcledgerParty.IsPartyLedger = "Yes";
                pcledgerParty.Amount = Convert.ToDouble(salesVoucher.Sales) * -1;
                clsSalesFields.SalesAllLedgerEntriesList.Add(salesVoucher.TallyLedgerName, pcledgerParty);

                pcledgerParty = new ALLLedgerEntries();
                pcledgerParty.LedgerName = salesVoucher.TallyLedgerNamePark; ;
                pcledgerParty.IsDeemedPositive = "No";
                pcledgerParty.LedgerFromItem = "No";
                pcledgerParty.RemoveZeroEntries = "No";
                pcledgerParty.IsPartyLedger = "No";
                pcledgerParty.Amount = Convert.ToDouble(salesVoucher.CashSales);
                clsSalesFields.SalesAllLedgerEntriesList.Add(salesVoucher.TallyLedgerNamePark, pcledgerParty);

                pcledgerParty = new ALLLedgerEntries();
                pcledgerParty.LedgerName = tallySetupModel.Find(x => x.Keyname.Contains("ExemptedSales")).LedgerName;
                pcledgerParty.IsDeemedPositive = "No";
                pcledgerParty.LedgerFromItem = "No";
                pcledgerParty.RemoveZeroEntries = "No";
                pcledgerParty.IsPartyLedger = "No";
                pcledgerParty.Amount = Convert.ToDouble(salesVoucher.ExemptedSales);
                clsSalesFields.SalesAllLedgerEntriesList.Add(tallySetupModel.Find(x => x.Keyname.Contains("ExemptedSales")).LedgerName, pcledgerParty);

                pcledgerParty = new ALLLedgerEntries();
                pcledgerParty.LedgerName = tallySetupModel.Find(x => x.Keyname.Contains("OutputVAT ")).LedgerName;
                pcledgerParty.IsDeemedPositive = "No";
                pcledgerParty.LedgerFromItem = "No";
                pcledgerParty.RemoveZeroEntries = "No";
                pcledgerParty.IsPartyLedger = "No";
                pcledgerParty.Amount = Convert.ToDouble(salesVoucher.OutputVAT);
                clsSalesFields.SalesAllLedgerEntriesList.Add(tallySetupModel.Find(x => x.Keyname.Contains("OutputVAT")).LedgerName, pcledgerParty);

                //var pcBillalloc = new BillAllocation();
                //pcBillalloc.Name = "AccountID";
                //pcBillalloc.BillType = "New Ref";
                //pcBillalloc.Amount = Val("Total") * -1;
                //pcledgerParty.BillAllocationList.Add("PartyLedger", pcBillalloc);
                //clsSalesFields.SalesAllLedgerEntriesList.Add("PartyLedger", pcledgerParty);

                var SalesLedgerCount = new List<string>(new string[] {

                salesVoucher.TallyLedgerName,
                salesVoucher.TallyLedgerNamePark,
                tallySetupModel.Find(x => x.Keyname.Contains("ExemptedSales")).LedgerName,
                tallySetupModel.Find(x => x.Keyname.Contains("OutputVAT ")).LedgerName
                 });

                var clsSalesVoucher = new TallySaleVoucher();
                xmldocVoucher.Add(clsSalesVoucher.CreateSaleVoucherXML(clsSalesFields, SalesLedgerCount, tallySetupModel.Find(x => x.Keyname.Contains("CompanyName")).LedgerName));

                i += 1;
            }
            SerializeToXml(xmldocVoucher, path);
        }

        public void SerializeToXml<T>(T anyobject, string xmlFilePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(anyobject.GetType());

            using (StreamWriter writer = new StreamWriter(xmlFilePath))
            {
                xmlSerializer.Serialize(writer, anyobject);
            }
        }

        public CessReportModel GetCessReport(string fromDate, string toDate, int outletId,string reporttype) 
        {
            return _iReportRepository.GetCessReport(fromDate, toDate, outletId, reporttype);
        }
        public CessCategoryReportModel GetCessCategoryReport(string fromDate, string toDate, int categoryId, int foodMenuId, int outletId)
        {
            return _iReportRepository.GetCessCategoryReport(fromDate, toDate, categoryId,  foodMenuId, outletId);
        }
        public List<ModeofPaymentReportModel> GetModOfPaymentReport(string fromDate, string toDate, int outletId)
        {
            return _iReportRepository.GetModOfPaymentReport(fromDate, toDate, outletId);
        }
        public List<WasteReportModel> GetWasteReport(string fromDate, string toDate, int storeId, string reporttype)
        {
            return _iReportRepository.GetWasteReport(fromDate, toDate, storeId,reporttype);

        }
        public List<PurchaseReportModel> GetPurchaseReport(string fromDate, string toDate, int storeId, string reporttype)
        {
            return _iReportRepository.GetPurchaseReport(fromDate, toDate, storeId, reporttype);
        }

    }
}
