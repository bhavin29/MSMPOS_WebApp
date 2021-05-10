using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OpenHtmlToPdf;
using RocketPOS.Framework;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Transaction
{
    public class SalesInvoiceController : Controller
    {
        private readonly ISalesInvoiceService _iSalesInvoiceService;
        private readonly IDropDownService _iDropDownService;
        private readonly ICommonService _iCommonService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SalesInvoiceController(IHostingEnvironment hostingEnvironment, ISalesInvoiceService salesInvoiceService,
            IDropDownService idropDownService,
             ICommonService iCommonService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer,
            LocService locService)
        {
            _hostingEnvironment = hostingEnvironment;
            _iSalesInvoiceService = salesInvoiceService;
            _iDropDownService = idropDownService;
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        // GET: PurchaseInvoiceFoodMenu
        public ActionResult SalesInvoiceFoodMenuList()
        {
            _iCommonService.GetPageWiseRoleRigths("SalesInvoice");
            List<SalesInvoiceViewModel> purchaseList = new List<SalesInvoiceViewModel>();
            //purchaseList = _iSalesInvoiceService.GetPurchaseInvoiceFoodMenuList().ToList();
            return View(purchaseList);
        }

        // GET: PurchaseInvoice/Create
        public ActionResult SalesInvoiceFoodMenu(long? id, long? purchaseId, string type)
        {
            SalesInvoiceModel purchaseModel = new SalesInvoiceModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true || UserRolePermissionForPage.View == true)
            {
                if (purchaseId > 0)
                {
                    purchaseModel = _iSalesInvoiceService.GetPurchaseInvoiceFoodMenuByPurchaseId(Convert.ToInt64(purchaseId));
                    purchaseModel.DeliveryDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                    purchaseModel.ReferenceNo = _iSalesInvoiceService.ReferenceNumberFoodMenu().ToString();
                }
                else
                {
                    if (id > 0)
                    {
                        ViewBag.ActionType = type;
                        long purchaseInvoiceId = Convert.ToInt64(id);
                        purchaseModel = _iSalesInvoiceService.GetPurchaseInvoiceFoodMenuById(purchaseInvoiceId);
                    }
                    else
                    {
                        purchaseModel.ReferenceNo = _iSalesInvoiceService.ReferenceNumberFoodMenu().ToString();
                        purchaseModel.SalesInvoiceDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                        purchaseModel.DeliveryDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                    }
                }
                purchaseModel.CustomerList = _iDropDownService.GetCustomerList();
                purchaseModel.StoreList = _iDropDownService.GetStoreList();
                purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();
                return View(purchaseModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        // POST: PurchaseInvoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalesInvoiceFoodMenu(SalesInvoiceModel purchaseModel, string Cancel)
        {
            purchaseModel.CustomerList = _iDropDownService.GetCustomerList();
            purchaseModel.StoreList = _iDropDownService.GetStoreList();
            purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();
            string purchaseMessage = string.Empty;
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationPurchaseInvoice(purchaseModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    return Json(new { error = true, message = errorString, status = 201 });
                }
            }

            if (purchaseModel.SalesInvoiceDetails != null)
            {
                if (purchaseModel.SalesInvoiceDetails.Count > 0)
                {
                    purchaseModel.InventoryType = 1;
                    if (purchaseModel.Id > 0)
                    {

                        int result = _iSalesInvoiceService.UpdatePurchaseInvoiceFoodMenu(purchaseModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                        }
                    }
                    else
                    {
                        purchaseModel.ReferenceNo = _iSalesInvoiceService.ReferenceNumberFoodMenu().ToString();

                        int result = _iSalesInvoiceService.InsertPurchaseInvoiceFoodMenu(purchaseModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("SaveSuccess") + " Reference No is: " + result.ToString();
                        }
                    }
                }
                else
                {
                    purchaseMessage = _locService.GetLocalizedHtmlString("ValidPurchaseInvoiceDetails");
                    return Json(new { error = true, message = purchaseMessage, status = 201 });
                }
            }
            else
            {
                purchaseMessage = _locService.GetLocalizedHtmlString("ValidPurchaseInvoiceDetails");
                return RedirectToAction("SalesInvoice", "SalesInvoiceFoodMenu");
            }
            return Json(new { error = false, message = purchaseMessage, status = 200 });
        }

        [HttpGet]
        public JsonResult SalesInvoiceFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId)
        {
            List<SalesInvoiceViewModel> purchaseViewModels = new List<SalesInvoiceViewModel>();
            DateTime newFromDate, newToDate;
            if (fromDate != null)
            {
                newFromDate = fromDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromDate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newFromDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }

            purchaseViewModels = _iSalesInvoiceService.PurchaseInvoiceFoodMenuListByDate(newFromDate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), customerId, storeId).ToList();
            return Json(new { salesInvoiceFoodMenu = purchaseViewModels });
        }
        public ActionResult Delete(int id)
        {
            if (UserRolePermissionForPage.Delete == true)
            {
                int result = _iSalesInvoiceService.DeletePurchaseInvoice(id);
                if (result > 0)
                {
                    ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
                }
                return RedirectToAction(nameof(SalesInvoiceFoodMenuList));
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public ActionResult DeletePurchaseInvoiceDetails(long purchaseId)
        {
            long result = _iSalesInvoiceService.DeletePurchaseInvoiceDetails(purchaseId);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return Json(new { error = false, message = string.Empty, status = 200 });
        }

        private string ValidationPurchaseInvoice(SalesInvoiceModel purchaseModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(purchaseModel.CustomerId.ToString()) || purchaseModel.CustomerId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidCustomer");
                return ErrorString;
            }
            if (purchaseModel.SalesInvoiceDetails == null || purchaseModel.SalesInvoiceDetails.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSalesInvoiceDetails");
                return ErrorString;
            }

            return ErrorString;
        }

        [HttpGet]
        public ActionResult GetFoodMenuList()
        {
            SalesInvoiceModel purchaseModel = new SalesInvoiceModel();
            purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuListByFoodmenuType(1);
            return Json(new { purchaseModel.FoodMenuList });
        }

        [HttpGet]
        public ActionResult GetTaxByFoodMenuId(int foodMenuId)
        {
            decimal taxPercentage = 0;
            taxPercentage = _iSalesInvoiceService.GetTaxByFoodMenuId(foodMenuId);
            return Json(new { TaxPercentage = taxPercentage });
        }

        [HttpGet]
        public JsonResult GetPurchaseIdByPOReference(string poReference)
        {
            int purchaseId = 0;
            purchaseId = _iSalesInvoiceService.GetPurchaseIdByPOReference(poReference);
            return Json(new { purchaseId = purchaseId });
        }

        [HttpGet]
        public ActionResult GetInvoicePrint(int id)
        {
            SalesInvoiceModel salesInvoiceModel = new SalesInvoiceModel();
            salesInvoiceModel = _iSalesInvoiceService.GetSalesInvoiceReportById(id);
            string html = _iSalesInvoiceService.GetInvoiceHtmlString(salesInvoiceModel);

            var pdf = Pdf
              .From(html)
              .OfSize(PaperSize.Letter)
              .WithTitle("Title")
              .WithoutOutline()
              .WithMargins(.25.Centimeters())
              .Portrait()
              .Comressed()
              .Content();

            string webRootPath = _hostingEnvironment.WebRootPath;
            string InvoicePath = Path.Combine(webRootPath, "Sales");
            string FileNmae = InvoicePath + "\\SalesInvoice_" + DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset).ToString("MM/dd/yyyy HH:mm").Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + ".pdf";

            Stream myStream = new MemoryStream(pdf);

            using (var fileStream = System.IO.File.Create(FileNmae))
            {
                myStream.Seek(0, SeekOrigin.Begin);
                myStream.CopyTo(fileStream);
            }

            byte[] FileBytes = System.IO.File.ReadAllBytes(FileNmae);
            return File(FileBytes, "application/pdf");
        }

        public ActionResult View(long? id)
        {
            SalesInvoiceModel purchaseModel = new SalesInvoiceModel();
            if (UserRolePermissionForPage.View == true)
            {
                if (id > 0)
                {
                    long purchaseInvoiceId = Convert.ToInt64(id);
                    purchaseModel = _iSalesInvoiceService.GetViewSalesInvoiceFoodMenuById(purchaseInvoiceId);
                }
                else
                {
                    purchaseModel.ReferenceNo = _iSalesInvoiceService.ReferenceNumberFoodMenu().ToString();
                    purchaseModel.SalesInvoiceDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                }
                purchaseModel.CustomerList = _iDropDownService.GetCustomerList();
                purchaseModel.StoreList = _iDropDownService.GetStoreList();
                purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();
                return View(purchaseModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }
    }
}
