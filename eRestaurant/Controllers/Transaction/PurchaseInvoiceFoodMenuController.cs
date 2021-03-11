using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RocketPOS.Framework;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Transaction
{
    public class PurchaseInvoiceFoodMenuController : Controller
    {
        private readonly IPurchaseInvoiceService _iPurchaseInvoiceService;
        private readonly IDropDownService _iDropDownService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;
        private readonly ISupplierService _iSupplierService;
        private readonly IEmailService _iEmailService;

        public PurchaseInvoiceFoodMenuController(IPurchaseInvoiceService purchaseService,
            IDropDownService idropDownService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer,
            LocService locService,
            ISupplierService supplierService,
            IEmailService emailService)
        {
            _iPurchaseInvoiceService = purchaseService;
            _iDropDownService = idropDownService;
            _iSupplierService = supplierService;
            _iEmailService = emailService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        // GET: PurchaseInvoiceFoodMenu
        public ActionResult PurchaseInvoiceFoodMenuList()
        {
            List<PurchaseInvoiceViewModel> purchaseList = new List<PurchaseInvoiceViewModel>();
            purchaseList = _iPurchaseInvoiceService.GetPurchaseInvoiceFoodMenuList().ToList();
            return View(purchaseList);
        }

        // GET: PurchaseInvoiceFoodMenu/Details/5
        [HttpGet]
        public ActionResult GetOrderById(long purchaseId)
        {
            PurchaseInvoiceModel purchaseModel = new PurchaseInvoiceModel();
            purchaseModel = _iPurchaseInvoiceService.GetPurchaseInvoiceFoodMenuById(purchaseId);
            return View(purchaseModel);
        }

        // GET: PurchaseInvoice/Create
        public ActionResult PurchaseInvoiceFoodMenu(long? id, long? purchaseId, string type)
        {
            PurchaseInvoiceModel purchaseModel = new PurchaseInvoiceModel();
            if (purchaseId > 0)
            {
                purchaseModel = _iPurchaseInvoiceService.GetPurchaseInvoiceFoodMenuByPurchaseId(Convert.ToInt64(purchaseId));
                purchaseModel.DeliveryDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                purchaseModel.ReferenceNo = _iPurchaseInvoiceService.ReferenceNumberFoodMenu().ToString();
            }
            else
            {
                if (id > 0)
                {
                    ViewBag.ActionType = type;
                    long purchaseInvoiceId = Convert.ToInt64(id);
                    purchaseModel = _iPurchaseInvoiceService.GetPurchaseInvoiceFoodMenuById(purchaseInvoiceId);
                }
                else
                {
                    purchaseModel.ReferenceNo = _iPurchaseInvoiceService.ReferenceNumberFoodMenu().ToString();
                    purchaseModel.PurchaseInvoiceDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                    purchaseModel.DeliveryDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                }
            }
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            purchaseModel.StoreList = _iDropDownService.GetStoreList();
            //purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();
            return View(purchaseModel);
        }

        // POST: PurchaseInvoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PurchaseInvoiceFoodMenu(PurchaseInvoiceModel purchaseModel, string Cancel)
        {
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            //purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
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

            if (purchaseModel.purchaseInvoiceDetails != null)
            {
                if (purchaseModel.purchaseInvoiceDetails.Count > 0)
                {
                    purchaseModel.InventoryType = 1;
                    if (purchaseModel.Id > 0)
                    {

                        int result = _iPurchaseInvoiceService.UpdatePurchaseInvoiceFoodMenu(purchaseModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                        }
                    }
                    else
                    {
                        purchaseModel.ReferenceNo = _iPurchaseInvoiceService.ReferenceNumberFoodMenu().ToString();
                        //purchaseModel.PurchaseInvoiceDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);

                        int result = _iPurchaseInvoiceService.InsertPurchaseInvoiceFoodMenu(purchaseModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("SaveSuccess") + " Reference No is: " + result.ToString();
                            //    if (purchaseModel.IsSendEmail)
                            //    {
                            //        if (!string.IsNullOrEmpty(purchaseModel.SupplierEmail))
                            //        {
                            //            //  _iEmailService.SendEmailToForFoodMenuPurchaseInvoice(purchaseModel, purchaseModel.SupplierEmail);
                            //        }
                            //    }
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
                return RedirectToAction("PurchaseInvoiceFoodMenu", "PurchaseInvoiceFoodMenu");

                // return Json(new { error = true, message = purchaseMessage, status = 201 });
            }
            // return View(purchaseModel);
            return Json(new { error = false, message = purchaseMessage, status = 200 });
            //return View();
        }

        [HttpGet]
        public JsonResult PurchaseInvoiceFoodMenuListByDate(string fromDate, string toDate, int supplierId)
        {
            List<PurchaseInvoiceViewModel> purchaseViewModels = new List<PurchaseInvoiceViewModel>();
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

            purchaseViewModels = _iPurchaseInvoiceService.PurchaseInvoiceFoodMenuListByDate(newFromDate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), supplierId).ToList();
            return Json(new { PurchaseInvoiceFoodMenu = purchaseViewModels });
        }
        public ActionResult Delete(int id)
        {
            int result = _iPurchaseInvoiceService.DeletePurchaseInvoice(id);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return RedirectToAction(nameof(PurchaseInvoiceFoodMenuList));
        }

        public ActionResult DeletePurchaseInvoiceDetails(long purchaseId)
        {
            long result = _iPurchaseInvoiceService.DeletePurchaseInvoiceDetails(purchaseId);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return Json(new { error = false, message = string.Empty, status = 200 });
        }

        private string ValidationPurchaseInvoice(PurchaseInvoiceModel purchaseModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(purchaseModel.SupplierId.ToString()) || purchaseModel.SupplierId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSupplier");
                return ErrorString;
            }
            if (purchaseModel.purchaseInvoiceDetails == null || purchaseModel.purchaseInvoiceDetails.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidPurchaseInvoiceDetails");
                return ErrorString;
            }

            return ErrorString;
        }

        [HttpGet]
        public ActionResult GetSupplierDetail(int supplierId)
        {
            SupplierModel supplierModel = new SupplierModel();
            PurchaseInvoiceModel purchaseModel = new PurchaseInvoiceModel();
            purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuListBySupplier(supplierId);
            supplierModel = _iSupplierService.GetSupplierById(supplierId);
            return Json(new { email = supplierModel.SupplierEmail, taxType = supplierModel.TaxType, purchaseModel.FoodMenuList });
        }

        [HttpGet]
        public ActionResult GetFoodMenuList()
        {
            PurchaseInvoiceModel purchaseModel = new PurchaseInvoiceModel();
            purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuListByFoodmenuType(1);
            return Json(new { purchaseModel.FoodMenuList });
        }

        [HttpGet]
        public ActionResult GetTaxByFoodMenuId(int foodMenuId)
        {
            decimal taxPercentage = 0;
            taxPercentage = _iPurchaseInvoiceService.GetTaxByFoodMenuId(foodMenuId);
            return Json(new { TaxPercentage = taxPercentage });
        }

        public ActionResult GetFoodMenuLastPrice(int itemType, int foodMenuId)
        {
            decimal unitPrice = 0;
            unitPrice = _iPurchaseInvoiceService.GetFoodMenuLastPrice(itemType,foodMenuId);
            return Json(new { UnitPrice = unitPrice });
        }

        [HttpGet]
        public JsonResult GetPurchaseIdByPOReference(string poReference)
        {
            int purchaseId = 0;
            purchaseId = _iPurchaseInvoiceService.GetPurchaseIdByPOReference(poReference);
            return Json(new { purchaseId = purchaseId });
        }
        [HttpGet]
        public JsonResult GetSupplierList()
        {
            PurchaseInvoiceViewModel purchaseInvoiceViewModel = new PurchaseInvoiceViewModel();
            purchaseInvoiceViewModel.SupplierList = _iDropDownService.GetSupplierList().ToList();
            return Json(new { SupplierList = purchaseInvoiceViewModel.SupplierList });
        }

        public ActionResult View(long? id)
        {
            PurchaseInvoiceModel purchaseModel = new PurchaseInvoiceModel();

            if (id > 0)
            {
                long purchaseInvoiceId = Convert.ToInt64(id);
                purchaseModel = _iPurchaseInvoiceService.GetViewPurchaseInvoiceFoodMenuById(purchaseInvoiceId);
            }
            else
            {
                purchaseModel.ReferenceNo = _iPurchaseInvoiceService.ReferenceNumberFoodMenu().ToString();
                purchaseModel.PurchaseInvoiceDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            purchaseModel.StoreList = _iDropDownService.GetStoreList();
            purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();
            return View(purchaseModel);
        }
    }
}
