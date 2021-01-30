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

            inventoryReportModel = _iReportService.GetInventoryReport(inventoryReportParamModel);
            return View(inventoryReportModel);
        }
        [HttpGet]
        public JsonResult GetInventoryStockList(int supplierId, int storeId)
        {
            List<InventoryReportModel> inventoryReportModel = new List<InventoryReportModel>();
            InventoryReportParamModel inventoryReportParamModel = new InventoryReportParamModel();
            inventoryReportModel = _iReportService.GetInventoryStockList(supplierId, storeId);
            return Json(new { InventoryStockList = inventoryReportModel });
        }

        [HttpPost]
        public ViewResult Inventory(InventoryReportParamModel inventoryReportParamModel)
        {

            List<InventoryReportModel> inventoryReportModel = new List<InventoryReportModel>();

            inventoryReportModel = _iReportService.GetInventoryReport(inventoryReportParamModel);
            return View(inventoryReportModel);
        }

        public ActionResult InventoryDetail(int id, string name, string code,string stock)
        {
            ViewData["FoodMenuName"] = name;
            ViewData["FoodMenuCode"] = code;
            ViewData["StockQty"] = stock;
            

            List<InventoryDetailReportModel> inventoryReportModel = new List<InventoryDetailReportModel>();
            InventoryReportParamModel inventoryReportParamModel = new InventoryReportParamModel();

            inventoryReportModel = _iReportService.GetInventoryDetailReport(inventoryReportParamModel, id);
            if (inventoryReportModel.Count>0 )
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
        public ActionResult PurchaseReport(string fromDate, string toDate)
        {
            List<PurchaseReportModel> purchaseReportList = new List<PurchaseReportModel>();
            PurchaseReportParamModel purchaseReportModel = new PurchaseReportParamModel();
            purchaseReportModel.draw = int.Parse(HttpContext.Request.Query["draw"]);
            if (fromDate != null)
            {
                DateTime newFromDate = fromDate == "01/01/0001 00:00:00" ? DateTime.Now : Convert.ToDateTime(fromDate);
                DateTime newToDate = toDate == "01/01/0001 00:00:00" ? DateTime.Now : Convert.ToDateTime(toDate);
                purchaseReportModel.FromDate = newFromDate;
                purchaseReportModel.ToDate = newToDate;
                purchaseReportList = _iReportService.GetPurchaseReport(newFromDate, newToDate);
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

        public ActionResult PrintReceiptA4(int id,string clientName,string address1, string address2, string phone)
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

                sb.Append(clientName +"</br>"+ address1+"</br>" + address2+ "</br>" + phone);
                sb.Append(@"</div></td>
                            </tr>
                            <tr align='center' Height='50'>
                            	<td colspan='6'>INVOICE </td>
                            </tr>
                            <tr Height='50'>
                            <td colspan=3 >Customer Name : "+ printReceiptA4.PrintReceiptDetail.CustomerName + @" </td>
                                  	<td colspan=3>Mode of payment : "+ printReceiptA4.PrintReceiptDetail.PaymentMethodName + @"</td>
                            </tr>
                            <tr Height='50'>
                            	<td colspan=3>Invoice : "+ printReceiptA4 .PrintReceiptDetail.SalesInvoiceNumber+ @"</td>
                            	<td colspan=3>Date : "+ printReceiptA4.PrintReceiptDetail.BillDateTime + @"</td>
                            </tr>
                            <tr Height='30' class='noBorder'>
                            	<td colspan=3 >Item</td>
                            	<td width=50 >Qty</td>
                            	<td >Rate</td>
                            	<td >Amount</td>
                            </tr>");

            foreach (var item in printReceiptA4.PrintReceiptItemList)
            {
                sb.AppendFormat(@"<tr  Height='30' class='noBorder'><td colspan=3 >{0}</td><td width=50 >{1}</td><td>{2}</td><td>{3}</td></tr>", item.FoodMenuName,item.FoodMenuQty,item.FoodMenuRate,item.Price);
            }
            sb.Append(@"<tr Height='30'><td colspan=4></td><td >Gross Total</td><td >" + printReceiptA4.PrintReceiptDetail.GrossAmount + "</td></tr>");
            sb.Append(@"<tr Height='30'><td colspan=4></td><td >Vatable</td><td >"+ printReceiptA4 .PrintReceiptDetail.VatableAmount+ "</td></tr>");
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
    }
}
