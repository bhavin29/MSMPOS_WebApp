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
    public class SalesDeliveryController : Controller
    {
        private readonly ISalesDeliveryService _iSalesDeliveryService;
        private readonly IDropDownService _iDropDownService;
        private readonly ICommonService _iCommonService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SalesDeliveryController(IHostingEnvironment hostingEnvironment, ISalesDeliveryService salesDeliveryService,
            IDropDownService idropDownService,
            ICommonService iCommonService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer,
            LocService locService,
            IEmailService emailService)
        {
            _hostingEnvironment = hostingEnvironment;
            _iSalesDeliveryService = salesDeliveryService;
            _iDropDownService = idropDownService;
            _iCommonService = iCommonService;
          //  _iEmailService = emailService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        // GET: PurchaseGRNFoodMenu
        public ActionResult SalesDeliveryFoodMenuList()
        {
            _iCommonService.GetPageWiseRoleRigths("SalesDelivery");
            List<SalesDeliveryViewModel> purchaseList = new List<SalesDeliveryViewModel>();
           // purchaseList = _iSalesDeliveryService.GetPurchaseGRNFoodMenuList().ToList();
            return View(purchaseList);
        }

        public ActionResult SalesDeliveryFoodMenu(long? id, long? purchaseId, string type)
        {
            SalesDeliveryModel purchaseModel = new SalesDeliveryModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true || UserRolePermissionForPage.View == true)
            {
                if (purchaseId > 0)
                {
                    purchaseModel = _iSalesDeliveryService.GetPurchaseGRNFoodMenuByPurchaseId(Convert.ToInt64(purchaseId));
                    if (purchaseModel != null)
                    {
                        purchaseModel.DeliveryDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                        purchaseModel.ReferenceNo = _iSalesDeliveryService.ReferenceNumberFoodMenu().ToString();
                    }
                }
                else
                {
                    if (id > 0)
                    {
                        ViewBag.ActionType = type;
                        long purchaseGRNId = Convert.ToInt64(id);
                        purchaseModel = _iSalesDeliveryService.GetPurchaseGRNFoodMenuById(purchaseGRNId);
                    }
                    else
                    {
                        purchaseModel.SalesDeliveryDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                        purchaseModel.DeliveryDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                        purchaseModel.ReferenceNo = _iSalesDeliveryService.ReferenceNumberFoodMenu().ToString();
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

        // POST: PurchaseGRN/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalesDeliveryFoodMenu(SalesDeliveryModel purchaseModel, string Submit)
        {
            purchaseModel.CustomerList = _iDropDownService.GetCustomerList();
            purchaseModel.StoreList = _iDropDownService.GetStoreList();
            purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();
            purchaseModel.ReferenceNo = _iSalesDeliveryService.ReferenceNumberFoodMenu().ToString();

            string purchaseMessage = string.Empty;
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationPurchaseGRN(purchaseModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    return Json(new { error = true, message = errorString, status = 201 });
                }
            }

            if (purchaseModel.salesDeliveryDetails != null)
            {
                if (purchaseModel.salesDeliveryDetails.Count > 0)
                {
                    purchaseModel.InventoryType = 1;
                    if (purchaseModel.Id > 0)
                    {

                        int result = _iSalesDeliveryService.UpdatePurchaseGRNFoodMenu(purchaseModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                        }
                    }
                    else
                    {
                        purchaseModel.ReferenceNo = _iSalesDeliveryService.ReferenceNumberFoodMenu().ToString();

                        int result = _iSalesDeliveryService.InsertPurchaseGRNFoodMenu(purchaseModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("SaveSuccess") + " Reference No is: " + result.ToString();
                        }
                    }
                }
                else
                {
                    purchaseMessage = _locService.GetLocalizedHtmlString("ValidPurchaseGRNDetails");
                    return Json(new { error = true, message = purchaseMessage, status = 201 });
                }
            }
            else
            {
                purchaseMessage = _locService.GetLocalizedHtmlString("ValidPurchaseGRNDetails");
                return RedirectToAction("PurchaseGRNFoodMenu", "PurchaseGRNFoodMenu");
                // return Json(new { error = true, message = purchaseMessage, status = 201 });
            }
            // return View(purchaseModel);
            return Json(new { error = false, message = purchaseMessage, status = 200 });
            //return View();
        }

        [HttpGet]
        public JsonResult SalesDeliveryFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId)
        {
            List<SalesDeliveryViewModel> purchaseViewModels = new List<SalesDeliveryViewModel>();
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

            purchaseViewModels = _iSalesDeliveryService.PurchaseGRNFoodMenuListByDate(newFromDate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), customerId, storeId).ToList();
            return Json(new { SalesDeliveryFood = purchaseViewModels });
        }
        public ActionResult Delete(int id)
        {
            if (UserRolePermissionForPage.Delete == true)
            {
                int result = _iSalesDeliveryService.DeletePurchaseGRN(id);
                if (result > 0)
                {
                    ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
                }
                return RedirectToAction(nameof(SalesDeliveryFoodMenuList));
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public ActionResult DeleteSalesDeliveryDetails(long purchaseId)
        {
            long result = _iSalesDeliveryService.DeletePurchaseGRNDetails(purchaseId);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return Json(new { error = false, message = string.Empty, status = 200 });
        }

        private string ValidationPurchaseGRN(SalesDeliveryModel purchaseModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(purchaseModel.CustomerId.ToString()) || purchaseModel.CustomerId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidCustomer");
                return ErrorString;
            }
            if (purchaseModel.salesDeliveryDetails == null || purchaseModel.salesDeliveryDetails.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSalesDeliveryDetails");
                return ErrorString;
            }

            return ErrorString;
        }

        [HttpGet]
        public ActionResult GetFoodMenuList()
        {
            PurchaseGRNModel purchaseModel = new PurchaseGRNModel();
            purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuListByFoodmenuType(1);
            return Json(new { purchaseModel.FoodMenuList });
        }

        [HttpGet]
        public ActionResult GetTaxByFoodMenuId(int foodMenuId)
        {
            decimal taxPercentage = 0;
            taxPercentage = _iSalesDeliveryService.GetTaxByFoodMenuId(foodMenuId);
            return Json(new { TaxPercentage = taxPercentage });
        }

        [HttpGet]
        public JsonResult GetPurchaseIdByPOReference(string poReference)
        {
            int purchaseId = 0;
            purchaseId = _iSalesDeliveryService.GetPurchaseIdByPOReference(poReference);
            return Json(new { purchaseId = purchaseId });
        }

        [HttpGet]
        public ActionResult GetDeliveryPrint(int id,string reportName)
        {
            SalesDeliveryModel salesInvoiceModel = new SalesDeliveryModel();
            salesInvoiceModel = _iSalesDeliveryService.GetSalesDeliveryReportById(id);
            string html = _iSalesDeliveryService.GetDeliveryHtmlString(salesInvoiceModel, reportName);

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
            string FileNmae = InvoicePath + "\\SalesDelivery_" + DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset).ToString("MM/dd/yyyy HH:mm").Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + ".pdf";

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
            SalesDeliveryModel purchaseModel = new SalesDeliveryModel();
            if (UserRolePermissionForPage.View == true)
            {
                if (id > 0)
                {
                    long purchaseGRNId = Convert.ToInt64(id);
                    purchaseModel = _iSalesDeliveryService.GetViewSalesDeliveryFoodMenuById(purchaseGRNId);
                }
                else
                {
                    purchaseModel.SalesDeliveryDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                    purchaseModel.ReferenceNo = _iSalesDeliveryService.ReferenceNumberFoodMenu().ToString();
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
