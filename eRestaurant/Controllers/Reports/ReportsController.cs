using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using RocketPOS.Interface.Services.Reports;
using RocketPOS.Models.Reports;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using SelectPdf;
using RocketPOS.Framework;

namespace RocketPOS.Controllers.Reports
{
    public class ReportController : Controller
    {

        private readonly IReportService _iReportService;
        private readonly IDropDownService _iDropDownService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public ReportController(IReportService iReportService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iReportService = iReportService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }
        public IActionResult Index()
        {

            return View();
        }
        public ViewResult Inventory()
        {
            List<InventoryReportModel> inventoryReportModel = new List<InventoryReportModel>();
            InventoryReportParamModel inventoryReportParamModel = new InventoryReportParamModel();

            //  inventoryReportModel = _iReportService.GetInventoryReport(inventoryReportParamModel);
            return View(inventoryReportModel);
        }
        [HttpGet]
        public JsonResult GetInventoryStockList(int supplierId, int storeId, int itemType, int active, string reportDate)
        {
            List<InventoryReportModel> inventoryReportModel = new List<InventoryReportModel>();
            InventoryReportParamModel inventoryReportParamModel = new InventoryReportParamModel();
            inventoryReportModel = _iReportService.GetInventoryStockList(supplierId, storeId, itemType, active, reportDate);
            return Json(new { InventoryStockList = inventoryReportModel });
        }
        [HttpPost]
        public ViewResult Inventory(InventoryReportParamModel inventoryReportParamModel)
        {

            List<InventoryReportModel> inventoryReportModel = new List<InventoryReportModel>();

            inventoryReportModel = _iReportService.GetInventoryReport(inventoryReportParamModel);
            return View(inventoryReportModel);
        }
        public ActionResult InventoryDetail(int id, string name, string code, string stock, string opening)
        {
            if (code == "null")
                code = "";

            ViewData["FoodMenuName"] = name;
            ViewData["FoodMenuCode"] = code;
            ViewData["StockQty"] = stock;
            ViewData["OpeningQty"] = opening;

            List<InventoryDetailReportModel> inventoryReportModel = new List<InventoryDetailReportModel>();
            InventoryReportParamModel inventoryReportParamModel = new InventoryReportParamModel();

            inventoryReportModel = _iReportService.GetInventoryDetailReport(inventoryReportParamModel, id);
            if (inventoryReportModel.Count > 0)
                ViewData["StoreName"] = inventoryReportModel[0].StoreName;

            return View(inventoryReportModel);
        }
        public ViewResult Purchase()
        {
            PurchaseReportParamModel purchaseReportModel = new PurchaseReportParamModel();
            purchaseReportModel.FromDate = DateTime.Now;
            purchaseReportModel.ToDate = DateTime.Now;
            //purchaseReportModel = _iReportService.GetPurchaseReport(purchaseReportParamModel);
            return View(purchaseReportModel);
        }
        [HttpGet]
        public ActionResult PurchaseReport(string fromdate, string toDate)
        {
            List<PurchaseReportModel> purchaseReportList = new List<PurchaseReportModel>();
            PurchaseReportParamModel purchaseReportModel = new PurchaseReportParamModel();
            purchaseReportModel.draw = int.Parse(HttpContext.Request.Query["draw"]);
            if (fromdate != null)
            {
                DateTime newfromdate = fromdate == "01/01/0001 00:00:00" ? DateTime.Now : Convert.ToDateTime(fromdate);
                DateTime newToDate = toDate == "01/01/0001 00:00:00" ? DateTime.Now : Convert.ToDateTime(toDate);
                purchaseReportModel.FromDate = newfromdate;
                purchaseReportModel.ToDate = newToDate;
                purchaseReportList = _iReportService.GetPurchaseReport(newfromdate, newToDate);
                purchaseReportModel.data = purchaseReportList.ToArray();
            }
            else
            {
                purchaseReportModel.FromDate = DateTime.Now;
                purchaseReportModel.ToDate = DateTime.Now;
            }


            //string jsonData = JsonConvert.SerializeObject(purchaseReportModel.PurchaseReport);
            var jsonData = purchaseReportList.ToArray();
            //return Json(purchaseReportModel, json);
            return Json(new { draw = purchaseReportModel.draw, recordsFiltered = purchaseReportList.Count, recordsTotal = purchaseReportList.Count, data = jsonData });
        }
        //public ViewResult OutletRegister()
        //{
        //    List<OutletRegisterReportModel> outletRegisterReportModel = new List<OutletRegisterReportModel>();
        //    outletRegisterReportModel= _iReportService.GetOutletRegisterReport(21);
        //    return View(outletRegisterReportModel);
        //}
        public PartialViewResult OutletRegister(int outletRegisterId)
        {
            List<OutletRegisterReportModel> outletRegisterReportModel = new List<OutletRegisterReportModel>();
            outletRegisterReportModel = _iReportService.GetOutletRegisterReport(outletRegisterId);
            return PartialView(outletRegisterReportModel);
        }
        [HttpGet]
        public ActionResult OutletRegisterReport(int outletRegisterId)
        {
            List<OutletRegisterReportModel> outletRegisterModels = new List<OutletRegisterReportModel>();

            //OutletRegisterReportModel outletRegisterModel = new OutletRegisterReportModel();

            outletRegisterModels = _iReportService.GetOutletRegisterReport(outletRegisterId);
            return View(outletRegisterModels);

            //string jsonData = JsonConvert.SerializeObject(purchaseReportModel.PurchaseReport);
            // var jsonData = outletRegisterModels.ToArray();
            //return Json(purchaseReportModel, json);
            //  return Json(new { draw = purchaseReportModel.draw, recordsFiltered = purchaseReportList.Count, recordsTotal = purchaseReportList.Count, data = jsonData });
        }
        public ActionResult PrintReceiptA4(int id, string clientName, string address1, string address2, string phone)
        {
            PrintReceiptA4 printReceiptA4 = new PrintReceiptA4();
            printReceiptA4 = _iReportService.GetPrintReceiptA4Detail(id);
            string pdf_page_size = string.Empty, pdf_orientation = string.Empty;
            var sb = new StringBuilder();
            sb.Append(@"<html>
                    <style type='text/css'>
                        table, tr, td {
                        border: 1px solid;
                    }
                    tr.noBorder td {
                    border: 0;
                    }
                    </style>
                    <body>
                        <table border=1 width='1280' Height='800'>
                        <tr align='center' Height='50'>
	                    <td colspan='6'><div align='right'>");

            sb.Append(clientName + "</br>" + address1 + "</br>" + address2 + "</br>" + phone);
            sb.Append(@"</div></td>
                            </tr>
                            <tr align='center' Height='50'>
                            	<td colspan='6'>INVOICE </td>
                            </tr>
                            <tr Height='50'>
                            <td colspan=3 >Customer Name : " + printReceiptA4.PrintReceiptDetail.CustomerName + @" </td>
                                  	<td colspan=3>Mode of payment : " + printReceiptA4.PrintReceiptDetail.PaymentMethodName + @"</td>
                            </tr>
                            <tr Height='50'>
                            	<td colspan=3>Invoice : " + printReceiptA4.PrintReceiptDetail.SalesInvoiceNumber + @"</td>
                            	<td colspan=3>Date : " + printReceiptA4.PrintReceiptDetail.BillDateTime + @"</td>
                            </tr>
                            <tr Height='30' class='noBorder'>
                            	<td colspan=3 >Item</td>
                            	<td width=50 >Qty</td>
                            	<td >Rate</td>
                            	<td >Amount</td>
                            </tr>");

            foreach (var item in printReceiptA4.PrintReceiptItemList)
            {
                sb.AppendFormat(@"<tr  Height='30' class='noBorder'><td colspan=3 >{0}</td><td width=50 >{1}</td><td>{2}</td><td>{3}</td></tr>", item.FoodMenuName, item.FoodMenuQty, item.FoodMenuRate, item.Price);
            }
            sb.Append(@"<tr Height='30'><td colspan=4></td><td >Gross Total</td><td >" + printReceiptA4.PrintReceiptDetail.GrossAmount + "</td></tr>");
            sb.Append(@"<tr Height='30'><td colspan=4></td><td >Vatable</td><td >" + printReceiptA4.PrintReceiptDetail.VatableAmount + "</td></tr>");
            sb.Append(@"<tr Height='30'><td colspan=4></td><td >Non Vatable</td><td >" + printReceiptA4.PrintReceiptDetail.NonVatableAmount + "</td></tr>");
            sb.Append(@"<tr Height='30'><td colspan=4></td><td >Total Tax</td><td >" + printReceiptA4.PrintReceiptDetail.TaxAmount + "</td></tr>");
            sb.Append(@"<tr Height='30'><td colspan=4></td><td >Grand Total</td><td >" + printReceiptA4.PrintReceiptDetail.TotalAmount + "</td></tr>");
            sb.Append(@" <tr Height='30'>
                            <td colspan='6'>Remarks :  </td>
                            </tr>
                            </body>
                            </table>
                            </html>");
            pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);
            pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);
            //// instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();
            //// set converter options
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            ////converter.Options.WebPageWidth = webPageWidth;
            ////converter.Options.WebPageHeight = webPageHeight;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 20;
            //// create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(sb.ToString());
            //// save pdf document
            //doc.Save("Select.pdf");
            byte[] pdf = doc.Save();
            return new FileContentResult(pdf, "application/pdf")
            {
                FileDownloadName = "Document.pdf"
            };
        }
        [HttpGet]
        public JsonResult GetStoreList()
        {
            InventoryReportModel inventoryReportModel = new InventoryReportModel();
            inventoryReportModel.StoreList = _iDropDownService.GetStoreList().ToList();
            return Json(new { StoreList = inventoryReportModel.StoreList });
        }
        public ViewResult DataSyncHistory()
        {
            List<DataHistorySyncReportModel> dataHistorySyncReportModels = new List<DataHistorySyncReportModel>();

            dataHistorySyncReportModels = _iReportService.GetDataSyncHistoryReport();
            return View(dataHistorySyncReportModels);
        }
        public ViewResult GetMasterSales()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();
            return View(masterSalesReport);
        }
        public ViewResult DetailedDailyByDate()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();
            return View(masterSalesReport);
        }
        public ViewResult DetailSaleSummaryReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();
            return View(masterSalesReport);
        }
        public ViewResult ProductWiseSales()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();
            return View(masterSalesReport);
        }
        public ViewResult SaleByCategorySectionReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();
            return View(masterSalesReport);
        }
        public ViewResult TableStatisticsReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();
            return View(masterSalesReport);
        }
        public ViewResult SalesSummaryByFoodCategoryReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();
            return View(masterSalesReport);
        }
        public ViewResult SalesSummaryByFoodCategoryFoodMenuReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();
            return View(masterSalesReport);
        }
        public ViewResult SalesSummaryBySectionReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();
            return View(masterSalesReport);
        }
        public ViewResult CustomerRewardReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();
            return View(masterSalesReport);
        }
        public ViewResult SalesSummaryByWeekReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();
            return View(masterSalesReport);
        }
        public ViewResult SalesSummaryByHoursReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();
            return View(masterSalesReport);
        }
        public JsonResult GetMasterSalesList(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId)
        {
            List<MasterSalesReportModel> masterSalesReportModel = new List<MasterSalesReportModel>();
            DateTime newfromdate, newToDate;
            if (fromdate != null)
            {
                newfromdate = fromdate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromdate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newfromdate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }
            masterSalesReportModel = _iReportService.GetMasterSaleReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), categoryId, foodMenuId, outletId);
            return Json(new { masterSalesList = masterSalesReportModel });
        }
        public JsonResult DetailedDailyByDate(string fromdate, string toDate, int outletId)
        {
            List<DetailedDailyReportModel> detailedDailyReportModels = new List<DetailedDailyReportModel>();
            DateTime newfromdate, newToDate;
            if (fromdate != null)
            {
                newfromdate = fromdate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromdate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newfromdate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }
            detailedDailyReportModels = _iReportService.GetDetailedDailyByDate(fromdate, toDate, outletId);
            return Json(new { detailedDailyList = detailedDailyReportModels });

        }
        public JsonResult DetailSaleSummaryReport(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId)
            {
            List<DetailSaleSummaryModel> detailSaleSummaryModels = new List<DetailSaleSummaryModel>();
            DateTime newfromdate, newToDate;

            if (fromdate != null)
            {
                newfromdate = fromdate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromdate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newfromdate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }

            detailSaleSummaryModels = _iReportService.GetDetailSaleSummaryReport(fromdate, toDate, categoryId, foodMenuId, outletId);
            return Json(new { detailSaleSummaryList = detailSaleSummaryModels });
        }
        public JsonResult  ProductWiseSales(string fromdate, string toDate, string ReportType, int outletId)
            {
            List<ProductWiseSalesReportModel> productWiseSalesReportModels = new List<ProductWiseSalesReportModel>();
            DateTime newfromdate, newToDate;
            if (fromdate != null)
            {
                newfromdate = fromdate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromdate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newfromdate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }
            productWiseSalesReportModels= _iReportService.GetProductWiseSales(fromdate, toDate, ReportType, outletId);
            return Json(new { productWiseSalesReportList = productWiseSalesReportModels });
        }
        public JsonResult SaleByCategorySectionReport(string fromdate, string toDate, string reportName, int categoryId, int foodMenuId, int outletId)
            {
            List<SalesByCategoryProductModel> salesByCategoryProductModels = new List<SalesByCategoryProductModel>();
            DateTime newfromdate, newToDate;
            if (fromdate != null)
            {
                newfromdate = fromdate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromdate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newfromdate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }
            salesByCategoryProductModels= _iReportService.GetSaleByCategorySectionReport(fromdate, toDate, reportName, categoryId, foodMenuId, outletId);
            return Json(new { salesByCategoryProductList = salesByCategoryProductModels });
        }
        public JsonResult  TableStatisticsReport(string fromdate, string toDate, int outletId)
            {
            List<TableStatisticsModel> tableStatisticsModels = new List<TableStatisticsModel>();
            DateTime newfromdate, newToDate;
            if (fromdate != null)
            {
                newfromdate = fromdate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromdate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newfromdate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }
            tableStatisticsModels= _iReportService.GetTableStatisticsReport(fromdate, toDate, outletId);
            return Json(new { tableStatisticsList = tableStatisticsModels });
        }
        public JsonResult  SalesSummaryByFoodCategoryReport(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId)
            {
            List<SalesSummaryModel> salesSummaryModels = new List<SalesSummaryModel>();
            DateTime newfromdate, newToDate;
            if (fromdate != null)
            {
                newfromdate = fromdate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromdate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newfromdate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }
            salesSummaryModels= _iReportService.GetSalesSummaryByFoodCategoryReport(fromdate, toDate, categoryId, foodMenuId, outletId);
            return Json(new { salesSummaryList = salesSummaryModels });
        }
        public JsonResult  SalesSummaryByFoodCategoryFoodMenuReport(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId)
            {
            List<SalesSummaryByFoodCategoryFoodMenuModel> salesSummaryByFoodCategoryFoodMenuModels = new List<SalesSummaryByFoodCategoryFoodMenuModel>();
            DateTime newfromdate, newToDate;
            if (fromdate != null)
            {
                newfromdate = fromdate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromdate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newfromdate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }
            salesSummaryByFoodCategoryFoodMenuModels= _iReportService.GetSalesSummaryByFoodCategoryFoodMenuReport(fromdate, toDate, categoryId, foodMenuId, outletId);
            return Json(new { salesSummaryByFoodCategoryFoodMenuList = salesSummaryByFoodCategoryFoodMenuModels });
        }
        public JsonResult  SalesSummaryBySectionReport(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId)
            {
            List<SalesSummaryBySectionModel> salesSummaryBySectionModels = new List<SalesSummaryBySectionModel>();
            DateTime newfromdate, newToDate;
            if (fromdate != null)
            {
                newfromdate = fromdate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromdate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newfromdate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }
            salesSummaryBySectionModels= _iReportService.GetSalesSummaryBySectionReport(fromdate, toDate, categoryId, foodMenuId, outletId);
            return Json(new { salesSummaryBySectionList = salesSummaryBySectionModels });
        }
        public JsonResult  CustomerRewardReport(string fromdate, string toDate, string customerPhone, string customerName, int outletId)
            {
            List<CustomerRewardModel> customerRewardModels = new List<CustomerRewardModel>();
            DateTime newfromdate, newToDate;
            if (fromdate != null)
            {
                newfromdate = fromdate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromdate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newfromdate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }
            customerRewardModels= _iReportService.GetCustomerRewardReport(fromdate, toDate, customerPhone, customerName, outletId);
            return Json(new { customerRewardList = customerRewardModels });
        }
        public JsonResult  SalesSummaryByWeekReport(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId)
            {
            List<SalesSummaryByWeek> salesSummaryByWeeks = new List<SalesSummaryByWeek>();
            DateTime newfromdate, newToDate;
            if (fromdate != null)
            {
                newfromdate = fromdate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromdate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newfromdate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }
            salesSummaryByWeeks= _iReportService.GetSalesSummaryByWeekReport(fromdate, toDate, categoryId, foodMenuId, outletId);
            return Json(new { salesSummaryByWeeksList = salesSummaryByWeeks });
        }
        public JsonResult  SalesSummaryByHoursReport(string fromdate, string toDate, int outletId)
            {
            List<SalesSummaryByHours> salesSummaryByHours = new List<SalesSummaryByHours>();
            DateTime newfromdate, newToDate;
            if (fromdate != null)
            {
                newfromdate = fromdate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromdate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newfromdate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }
            salesSummaryByHours= _iReportService.GetSalesSummaryByHoursReport(fromdate, toDate, outletId);
            return Json(new { salesSummaryByHoursList = salesSummaryByHours });
        }

    }
}
