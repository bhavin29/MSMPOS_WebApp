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
using Microsoft.AspNetCore.Hosting;
using RocketPOS.Framework;
using System.Data;
using System.Reflection;

namespace RocketPOS.Controllers.Reports
{
    public class ReportController : Controller
    {

        private readonly IReportService _iReportService;
        private readonly IDropDownService _iDropDownService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ReportController(IHostingEnvironment hostingEnvironment, IReportService iReportService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _hostingEnvironment = hostingEnvironment;
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

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        [HttpGet]
        public ViewResult DetailedDailyByDate()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        [HttpGet]
        public ViewResult DetailSaleSummaryReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        [HttpGet]
        public ViewResult ProductWiseSales()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        [HttpGet]
        public ViewResult SaleByCategorySectionReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        [HttpGet]
        public ViewResult TableStatisticsReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        [HttpGet]
        public ViewResult SalesSummaryByFoodCategoryReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        [HttpGet]
        public ViewResult SalesSummaryByFoodCategoryFoodMenuReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        [HttpGet]
        public ViewResult SalesSummaryBySectionReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        [HttpGet]
        public ViewResult CustomerRewardReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        [HttpGet]
        public ViewResult SalesSummaryByWeekReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        [HttpGet]
        public ViewResult SalesSummaryByHoursReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        public ViewResult TallyVoucher()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        public ViewResult CessReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();

            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        public ViewResult CessDetailReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();

            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        public ViewResult CessCategoryReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();

            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        [HttpGet]
        public ViewResult ModOfPaymentReport()
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();

            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }

        public ViewResult SalesByCategoryProduct(string rname)
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();

            ViewBag.Reportname = rname;

            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        public ViewResult SalesBySectionCategoryProduct(string rname)
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();

            ViewBag.Reportname = rname;
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
            return View(masterSalesReport);
        }
        public ViewResult SalesBySectionProduct(string rname)
        {
            ReportParameterModel masterSalesReport = new ReportParameterModel();

            ViewBag.Reportname = rname;
            masterSalesReport.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            masterSalesReport.FoodMenuList = _iDropDownService.GetFoodMenuList();
            masterSalesReport.OutletList = _iDropDownService.GetOutletList();

            masterSalesReport.fromDate = Convert.ToDateTime(LoginInfo.FromDate);
            masterSalesReport.toDate = Convert.ToDateTime(LoginInfo.ToDate);
            masterSalesReport.FoodCategoryId = LoginInfo.CategoryId;
            masterSalesReport.FoodMenuId = LoginInfo.FoodMenuId;
            masterSalesReport.OutletId = LoginInfo.OutletId;
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

            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.CategoryId = categoryId;
            LoginInfo.FoodMenuId = foodMenuId;
            LoginInfo.OutletId = outletId;

            masterSalesReportModel = _iReportService.GetMasterSaleReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), categoryId, foodMenuId, outletId);
            return Json(new { masterSalesList = masterSalesReportModel });
        }
        public JsonResult DetailedDailyByDateList(string fromdate, string toDate, int outletId)
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
            DateTime dtFrom = new DateTime();
            DateTime dtTo = new DateTime();

            dtFrom = Convert.ToDateTime(newfromdate);
            dtTo = Convert.ToDateTime(newToDate);

            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.OutletId = outletId;

            detailedDailyReportModels = _iReportService.GetDetailedDailyByDate(dtFrom.ToString("yyyy-MM-dd") + " 00:00:00", dtTo.ToString("yyyy-MM-dd") + " 23:59i:59", outletId);
            detailedDailyReportModels.RemoveAll(p => p.RegisterTitle.Contains("="));
            return Json(new { detailedDailyList = detailedDailyReportModels });

        }
        public JsonResult DetailSaleSummaryList(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId)
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

            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.CategoryId = categoryId;
            LoginInfo.FoodMenuId = foodMenuId;
            LoginInfo.OutletId = outletId;

            detailSaleSummaryModels = _iReportService.GetDetailSaleSummaryReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), categoryId, foodMenuId, outletId);
            return Json(new { detailSaleSummaryList = detailSaleSummaryModels });
        }
        public JsonResult ProductWiseSalesList(string fromdate, string toDate, string ReportType, int outletId)
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
            DateTime dtFrom = new DateTime();
            DateTime dtTo = new DateTime();

            dtFrom = Convert.ToDateTime(newfromdate);
            dtTo = Convert.ToDateTime(newToDate);

            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.OutletId = outletId;

            productWiseSalesReportModels = _iReportService.GetProductWiseSales(dtFrom.ToString("yyyy-MM-dd") + " 00:00:00", dtTo.ToString("yyyy-MM-dd") + " 23:59i:59", "Excel", outletId);
            //productWiseSalesReportModels.RemoveAt(3);
            // productWiseSalesReportModels.RemoveAll(p => p.SalesPrice == "");
            //System.Text.Json.JsonSerializer.Serialize

            productWiseSalesReportModels.RemoveAt(0);
            productWiseSalesReportModels.RemoveAt(1);
            productWiseSalesReportModels.RemoveAt(2);
            productWiseSalesReportModels.RemoveAt(3);
            productWiseSalesReportModels.RemoveAt(4);
            productWiseSalesReportModels.RemoveAt(5);
            productWiseSalesReportModels.RemoveAt(6);
            productWiseSalesReportModels.RemoveAt(7);
            productWiseSalesReportModels.RemoveAt(8);
            productWiseSalesReportModels.RemoveAt(9);
            productWiseSalesReportModels.RemoveAt(10);

            ProductWiseSalesReportModel[] array = productWiseSalesReportModels.ToArray();



            //    string output = new System.Web.Script.JavascriptSerializer.JavaScriptSerializer().Serialize(productWiseSalesReportModels);


            //   var a = JsonConvert.Serialize(productWiseSalesReportModels);

            //          var b = JsonConvert.DeserializeObject(a);
            return Json(new { productWiseSalesList = (array) });


        }
        public JsonResult SaleByCategorySectionList(string fromdate, string toDate, string reportName, int categoryId, int foodMenuId, int outletId)
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

            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.CategoryId = categoryId;
            LoginInfo.FoodMenuId = foodMenuId;
            LoginInfo.OutletId = outletId;

            salesByCategoryProductModels = _iReportService.GetSaleByCategorySectionReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), "SalesBySectionCategory", categoryId, foodMenuId, outletId);
            salesByCategoryProductModels.RemoveAll(p => p.SectionName == "ALL");
            return Json(new { salesByCategoryProductList = salesByCategoryProductModels });
        }
        public JsonResult TableStatisticsList(string fromdate, string toDate, int outletId)
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

            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.OutletId = outletId;

            tableStatisticsModels = _iReportService.GetTableStatisticsReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), outletId);
            return Json(new { tableStatisticsList = tableStatisticsModels });
        }
        public JsonResult SalesSummaryByFoodCategoryList(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId)
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

            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.CategoryId = categoryId;
            LoginInfo.FoodMenuId = foodMenuId;
            LoginInfo.OutletId = outletId;

            salesSummaryModels = _iReportService.GetSalesSummaryByFoodCategoryReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), categoryId, foodMenuId, outletId);
            return Json(new { salesSummaryList = salesSummaryModels });
        }
        public JsonResult SalesSummaryByFoodCategoryFoodMenuList(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId)
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

            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.CategoryId = categoryId;
            LoginInfo.FoodMenuId = foodMenuId;
            LoginInfo.OutletId = outletId;

            salesSummaryByFoodCategoryFoodMenuModels = _iReportService.GetSalesSummaryByFoodCategoryFoodMenuReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), categoryId, foodMenuId, outletId);
            return Json(new { salesSummaryByFoodCategoryFoodMenuList = salesSummaryByFoodCategoryFoodMenuModels });
        }
        public JsonResult SalesSummaryBySectionList(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId)
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

            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.CategoryId = categoryId;
            LoginInfo.FoodMenuId = foodMenuId;
            LoginInfo.OutletId = outletId;

            salesSummaryBySectionModels = _iReportService.GetSalesSummaryBySectionReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), categoryId, foodMenuId, outletId);
            salesSummaryBySectionModels.RemoveAll(p => p.SectionName.Contains("ALL"));
            return Json(new { salesSummaryBySectionList = salesSummaryBySectionModels });
        }
        public JsonResult CustomerRewardList(string fromdate, string toDate, string customerPhone, string customerName, int outletId)
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

            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.OutletId = outletId;

            customerRewardModels = _iReportService.GetCustomerRewardReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), customerPhone, customerName, outletId);
            return Json(new { customerRewardList = customerRewardModels });
        }
        public JsonResult SalesSummaryByWeekList(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId)
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
            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.CategoryId = categoryId;
            LoginInfo.FoodMenuId = foodMenuId;
            LoginInfo.OutletId = outletId;

            salesSummaryByWeeks = _iReportService.GetSalesSummaryByWeekReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), categoryId, foodMenuId, outletId);

            salesSummaryByWeeks = salesSummaryByWeeks.OrderBy(e => Convert.ToDateTime(e.WeekStartDate)).ToList();

            var query = salesSummaryByWeeks
                  .Select(g => new
                  {
                      WeekDate =g.WeekStartDate,
                      WeekStartDate = Convert.ToDateTime(g.WeekStartDate),
                      TotalInvoice = g.TotalInvoice,
                      NetSalesAmount = g.NetSalesAmount,
                      TotalDiscount = g.TotalDiscount,
                      TotalTax = g.TotalTax,
                      TotalGrossAmount = g.TotalGrossAmount
                  });

            var res = from ordr in query orderby Convert.ToDateTime(ordr.WeekStartDate) select ordr;

            return Json(new { salesSummaryByWeeksList = res });
        }
        public JsonResult SalesSummaryByHoursList(string fromdate, string toDate, int outletId)
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
            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.OutletId = outletId;

            salesSummaryByHours = _iReportService.GetSalesSummaryByHoursReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), outletId);
            var salesSummaryByHoursConvert = salesSummaryByHours.Select(x => new { EndHour = x.EndHour.ToString(), x.NetSalesAmount, x.OrderDate, StartHour = x.StartHour.ToString(), x.TotalDiscount, x.TotalGrossAmount, x.TotalInvoice, x.TotalTax }).ToList();
            return Json(new { salesSummaryByHoursList = salesSummaryByHoursConvert });
        }
        public JsonResult GetCessList(string fromdate, string toDate, int outletId)
        {
            CessReportModel cessReportModels = new CessReportModel();
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
            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.OutletId = outletId;

            cessReportModels = _iReportService.GetCessReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), outletId, "cessSummary");
            return Json(new { cessReportList = cessReportModels.CessSummaryList });
        }
        public JsonResult GetCessDetailList(string fromdate, string toDate, int outletId)
        {
            CessReportModel cessReportModels = new CessReportModel();
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
            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.OutletId = outletId;

            cessReportModels = _iReportService.GetCessReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), outletId, "cessDetail");
            return Json(new { cessReportList = cessReportModels.CessDetailList });
        }
        public JsonResult GetModOfPaymentList(string fromdate, string toDate, int outletId)
        {
            List<ModeofPaymentReportModel> modeofPaymentReportModels = new List<ModeofPaymentReportModel>();
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
            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.OutletId = outletId;

            modeofPaymentReportModels = _iReportService.GetModOfPaymentReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), outletId);

            // for linqpad objItems.Dump();

            var result = modeofPaymentReportModels.GroupBy(x => new { x.BillDate, x.PaymentMethodName, x.BillAmount, x.Sales })
                        .Select(g =>
                        {
                            var a = g.ToList();
                            return
                            new
                            {
                                BillDate = a[0].BillDate,
                                PaymentMethodName = a[0].PaymentMethodName,
                                BillAmount = a[0].BillAmount,
                                Sales = a[0].Sales
                            };
                        });

            // for linqpad result.Dump();

            var query = result
             .GroupBy(c => c.BillDate)
             .Select(g => new
             {
                 BillDate = g.Key,
                 Sales = g.Sum(c => Convert.ToDecimal(c.BillAmount)) / 2,
                 //   Sales = g.Where(c => c.PaymentMethodName == "SALES").Sum(c => Convert.ToDecimal(c.Sales)),
                 Cash = g.Where(c => c.PaymentMethodName == "CASH").Sum(c => c.BillAmount),
                 PaisaI = g.Where(c => c.PaymentMethodName == "M-PESA").Sum(c => c.BillAmount),
                 CreditCard = g.Where(c => c.PaymentMethodName == "CREDIT CARD").Sum(c => c.BillAmount),
                 DebitCard = g.Where(c => c.PaymentMethodName == "DEBIT CARD").Sum(c => c.BillAmount)
             });
            //   .ToList();

            //     List<ModeofPaymentReportResultModel> modeofPaymentReportModels1 = query.AsParallel).ToList();


            //var dataTable = ConvertToDataTable(modeofPaymentReportModels);
            //   var dataResult = GetInversedDataTable(dataTable, "PaymentMethodName", "BillDate", "BillAmount", " ", true);

            List<ModeofPaymentReportModel> modeofPaymentReports = new List<ModeofPaymentReportModel>();
            ModeofPaymentReportModel modeofPaymentReportModel = new ModeofPaymentReportModel();
            List<ModeofPaymentReportModel> returnLists = new List<ModeofPaymentReportModel>();
            //  gList.AddRange(DirectCast(DBDataTable.Select(), IEnumerable(Of Guid)))

            //   modeofPaymentReports.AddRang(DirectCast(dataResult.Select().ToList());
            //   modeofPaymentReports = ;

            List<ModeofPaymentReportResultModel> listObject = query.AsEnumerable()
                                  .Select(x => new ModeofPaymentReportResultModel()
                                  {
                                      BillDate = x.BillDate,
                                      Sales = x.Sales,
                                      Cash = x.Cash,
                                      PaisaI = x.PaisaI,
                                      CreditCard = x.CreditCard,
                                      DebitCard = x.DebitCard,
                                  }).ToList();

            /*      List<dynamic> dynamicListReturned = GetListFromDT(typeof(ModeofPaymentReportModel), dataResult);
                  List<ModeofPaymentReportModel> listPersons = dynamicListReturned.Cast<ModeofPaymentReportModel>().ToList();*/


            return Json(new { modeofPaymentReportList = listObject });
        }

        public List<dynamic> GetListFromDT(Type className, DataTable dataTable)
        {
            List<dynamic> list = new List<dynamic>();
            foreach (DataRow row in dataTable.Rows)
            {
                object objClass = Activator.CreateInstance(className);
                Type type = objClass.GetType();
                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = type.GetProperty(column.ColumnName);
                    prop.SetValue(objClass, row[column.ColumnName], null);
                }
                list.Add(objClass);
            }
            return list;
        }

        public JsonResult GetCessCategoryList(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId)
        {
            CessCategoryReportModel cessCategoryReportModels = new CessCategoryReportModel();
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
            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.CategoryId = categoryId;
            LoginInfo.FoodMenuId = foodMenuId;
            LoginInfo.OutletId = outletId;

            cessCategoryReportModels = _iReportService.GetCessCategoryReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), categoryId, foodMenuId, outletId);
            return Json(new { cessCategoryReportList = cessCategoryReportModels.CessSummaryList });
        }
        public JsonResult SalesByCategoryProductList(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId, string rname)
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
            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.CategoryId = categoryId;
            LoginInfo.FoodMenuId = foodMenuId;
            LoginInfo.OutletId = outletId;

            salesByCategoryProductModels = _iReportService.GetSaleByCategorySectionReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), rname, categoryId, foodMenuId, outletId);
           // var salesByCategoryProduct = salesByCategoryProductModels.Select(x => new { x.FoodMenuCategoryName, x.FoodMenuName, x.TotalDiscount, x.TotalGrossAmount, x.TotalPrice, x.TotalQty, x.TotalTax, x.TotalUnitPrice, x.ValuePercentage }).ToList();
            return Json(new { salesByCategoryProductList = salesByCategoryProductModels });
        }
        public JsonResult SalesBySectionCategoryProductList(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId, string rname)
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
            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.CategoryId = categoryId;
            LoginInfo.FoodMenuId = foodMenuId;
            LoginInfo.OutletId = outletId;

            salesByCategoryProductModels = _iReportService.GetSaleByCategorySectionReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), rname, categoryId, foodMenuId, outletId);
            //   var salesByCategoryProduct = salesByCategoryProductModels.Select(x => new { x.FoodMenuCategoryName, x.FoodMenuName, x.TotalDiscount, x.TotalGrossAmount, x.TotalPrice, x.TotalQty, x.TotalTax, x.TotalUnitPrice, x.ValuePercentage }).ToList();
            return Json(new { salesByCategoryProductList = salesByCategoryProductModels });
        }
        public JsonResult SalesBySectionProductList(string fromdate, string toDate, int categoryId, int foodMenuId, int outletId, string rname)
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
            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.CategoryId = categoryId;
            LoginInfo.FoodMenuId = foodMenuId;
            LoginInfo.OutletId = outletId;

            salesByCategoryProductModels = _iReportService.GetSaleByCategorySectionReport(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), rname, categoryId, foodMenuId, outletId);
            var salesByCategoryProduct = salesByCategoryProductModels.Select(x => new { x.SectionName, x.FoodMenuName, x.TotalDiscount, x.TotalGrossAmount, x.TotalPrice, x.TotalQty, x.TotalTax, x.TotalUnitPrice, x.ValuePercentage }).ToList();
            return Json(new { salesByCategoryProductList = salesByCategoryProduct });
        }
        public JsonResult TallyVoucherList(string fromdate, string toDate, int outletId)
        {
            List<TallySalesVoucherModel> tallySalesVoucherModels = new List<TallySalesVoucherModel>();
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

            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.OutletId = outletId;

            tallySalesVoucherModels = _iReportService.GetSalesVoucherData(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), outletId);
            return Json(new { tallySalesVoucherList = tallySalesVoucherModels });
        }
        public async Task<IActionResult> TallyVoucherXMLDownload(string fromdate, string toDate, int outletId, string outletName)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\" + "Download";
            string sFileName = outletName.Trim().ToString() + "_TallySalesVoucher_" + DateTime.Now.ToString("MM-dd-yyyy_HHmmss") + ".XML";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            try
            {
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

                _iReportService.GenerateSalesVoucher(newfromdate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), outletId, sWebRootFolder + "\\" + sFileName);

                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
            }
            catch (Exception ex)
            {
                SystemLogs.Register(ex.Message);
            }

            LoginInfo.FromDate = fromdate;
            LoginInfo.ToDate = toDate;
            LoginInfo.OutletId = outletId;

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
        public DataTable ConvertToDataTable<T>(List<T> models)
        {
            // creating a data table instance and typed it as our incoming model   
            // as I make it generic, if you want, you can make it the model typed you want.  
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties of that model  
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Loop through all the properties              
            // Adding Column name to our datatable  
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names    
                dataTable.Columns.Add(prop.Name);
            }
            // Adding Row and its value to our dataTable  
            foreach (T item in models)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows    
                    values[i] = Props[i].GetValue(item, null);
                }
                // Finally add value to datatable    
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public DataTable GetInversedDataTable(DataTable table, string columnX,
         string columnY, string columnZ, string nullValue, bool sumValues)
        {
            //Create a DataTable to Return
            DataTable returnTable = new DataTable();

            if (columnX == "")
                columnX = table.Columns[0].ColumnName;

            //Add a Column at the beginning of the table
            returnTable.Columns.Add(columnY);


            //Read all DISTINCT values from columnX Column in the provided DataTale
            List<string> columnXValues = new List<string>();

            foreach (DataRow dr in table.Rows)
            {
                string columnXTemp = dr[columnX].ToString();
                if (!columnXValues.Contains(columnXTemp))
                {
                    //Read each row value, if it's different from others provided, add to 
                    //the list of values and creates a new Column with its value.
                    columnXValues.Add(columnXTemp);
                    returnTable.Columns.Add(columnXTemp);
                }
            }

            //Verify if Y and Z Axis columns re provided
            if (columnY != "" && columnZ != "")
            {
                //Read DISTINCT Values for Y Axis Column
                List<string> columnYValues = new List<string>();

                foreach (DataRow dr in table.Rows)
                {
                    if (!columnYValues.Contains(dr[columnY].ToString()))
                        columnYValues.Add(dr[columnY].ToString());
                }

                //Loop all Column Y Distinct Value
                foreach (string columnYValue in columnYValues)
                {
                    //Creates a new Row
                    DataRow drReturn = returnTable.NewRow();
                    drReturn[0] = columnYValue;
                    //foreach column Y value, The rows are selected distincted
                    DataRow[] rows = table.Select(columnY + "='" + columnYValue + "'");

                    //Read each row to fill the DataTable
                    foreach (DataRow dr in rows)
                    {
                        string rowColumnTitle = dr[columnX].ToString();

                        //Read each column to fill the DataTable
                        foreach (DataColumn dc in returnTable.Columns)
                        {
                            if (dc.ColumnName == rowColumnTitle)
                            {
                                //If Sum of Values is True it try to perform a Sum
                                //If sum is not possible due to value types, the value 
                                // displayed is the last one read
                                if (sumValues)
                                {
                                    try
                                    {
                                        drReturn[rowColumnTitle] =
                                             Convert.ToDecimal(drReturn[rowColumnTitle]) +
                                             Convert.ToDecimal(dr[columnZ]);
                                    }
                                    catch
                                    {
                                        drReturn[rowColumnTitle] = dr[columnZ];
                                    }
                                }
                                else
                                {
                                    drReturn[rowColumnTitle] = dr[columnZ];
                                }
                            }
                        }
                    }
                    returnTable.Rows.Add(drReturn);
                }
            }
            else
            {
                throw new Exception("The columns to perform inversion are not provided");
            }

            //if a nullValue is provided, fill the datable with it
            if (nullValue != "")
            {
                foreach (DataRow dr in returnTable.Rows)
                {
                    foreach (DataColumn dc in returnTable.Columns)
                    {
                        if (dr[dc.ColumnName].ToString() == "")
                            dr[dc.ColumnName] = nullValue;
                    }
                }
            }

            return returnTable;
        }

    }
}
