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
    public class SalesController : Controller
    {
        private readonly ISalesService _iSalesService;
        private readonly IDropDownService _iDropDownService;
        private readonly ICommonService _iCommonService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;
        private readonly ISupplierService _iSupplierService;
        private readonly IEmailService _iEmailService;

        public SalesController(ISalesService salesService,
            IDropDownService idropDownService,
            ICommonService iCommonService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer,
            LocService locService,
            ISupplierService supplierService,
            IEmailService emailService)
        {
            _iSalesService = salesService;
            _iDropDownService = idropDownService;
            _iCommonService = iCommonService;
            _iSupplierService = supplierService;
            _iEmailService = emailService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        // GET: PurchaseFoodMenu
        public ActionResult SalesFoodMenuList()
        {
            _iCommonService.GetPageWiseRoleRigths("Sales");
            List<SalesViewModel> purchaseList = new List<SalesViewModel>();
            purchaseList = _iSalesService.GetPurchaseFoodMenuList().ToList();
            return View(purchaseList);
        }

        // GET: PurchaseFoodMenu/Details/5
        [HttpGet]
        public ActionResult GetSalesOrderById(long purchaseId)
        {
            SalesModel purchaseModel = new SalesModel();
            purchaseModel = _iSalesService.GetPurchaseFoodMenuById(purchaseId);
            return View(purchaseModel);
        }

        // GET: Purchase/Create
        public ActionResult SalesFoodMenu(long? id, string type)
        {
            SalesModel purchaseModel = new SalesModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true || UserRolePermissionForPage.View == true)
            {
                if (id > 0)
                {
                    ViewBag.ActionType = type;
                    long purchaseId = Convert.ToInt64(id);
                    purchaseModel = _iSalesService.GetPurchaseFoodMenuById(purchaseId);
                }
                else
                {
                    purchaseModel.Date = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                    purchaseModel.ReferenceNo = _iSalesService.ReferenceNumberFoodMenu().ToString();
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

        // POST: Purchase/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalesFoodMenu(SalesModel purchaseModel, string Cancel)
        {
            purchaseModel.CustomerList = _iDropDownService.GetSupplierList();
            purchaseModel.StoreList = _iDropDownService.GetStoreList();
            purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();

            string purchaseMessage = string.Empty;
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationPurchase(purchaseModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    return Json(new { error = true, message = errorString, status = 201 });
                }
            }

            if (purchaseModel.SalesDetails != null)
            {
                if (purchaseModel.SalesDetails.Count > 0)
                {
                    purchaseModel.InventoryType = 1;
                    if (purchaseModel.Id > 0)
                    {
                        int result = _iSalesService.UpdatePurchaseFoodMenu(purchaseModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                         
                            //ClientModel clientModel = new ClientModel();
                            //clientModel = _iSalesService.GetClientDetail();
                            //_iEmailService.SendEmailToForFoodMenuPurchase(Convert.ToInt32(purchaseModel.Id), clientModel);

                        }
                    }
                    else
                    {
                        purchaseModel.ReferenceNo = _iSalesService.ReferenceNumberFoodMenu().ToString();

                        string result = _iSalesService.InsertPurchaseFoodMenu(purchaseModel);
                        if (result != "")
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("SaveSuccess") + " Reference No is: " + result.ToString();
                         
                            //ClientModel clientModel = new ClientModel();
                            //clientModel = _iSalesService.GetClientDetail();
                            //purchaseId = _iSalesService.GetPurchaseIdByReferenceNo(result);
                            //_iEmailService.SendEmailToForFoodMenuPurchase(purchaseId, clientModel);
                        }
                    }
                }
                else
                {
                    purchaseMessage = _locService.GetLocalizedHtmlString("ValidPurchaseDetails");
                    return Json(new { error = true, message = purchaseMessage, status = 201 });
                }
            }
            else
            {
                purchaseMessage = _locService.GetLocalizedHtmlString("ValidPurchaseDetails");
                return Json(new { error = true, message = purchaseMessage, status = 201 });
            }
            // return View(purchaseModel);
            return Json(new { error = false, message = purchaseMessage, status = 200 });
            //return View();
        }

        public ActionResult Delete(int id)
        {
            if (UserRolePermissionForPage.Delete == true)
            {
                int result = _iSalesService.DeletePurchase(id);
                if (result > 0)
                {
                    ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
                }
                return RedirectToAction(nameof(SalesFoodMenuList));
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public ActionResult DeleteSalesDetails(long purchaseId)
        {
            long result = _iSalesService.DeletePurchaseDetails(purchaseId);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return Json(new { error = false, message = string.Empty, status = 200 });
        }

        private string ValidationPurchase(SalesModel purchaseModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(purchaseModel.CustomerId.ToString()) || purchaseModel.CustomerId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidCustomer");
                return ErrorString;
            }
            if (purchaseModel.SalesDetails == null || purchaseModel.SalesDetails.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSalesDetails");
                return ErrorString;
            }

            return ErrorString;
        }

        //[HttpGet]
        //public ActionResult GetSupplierDetail(int supplierId)
        //{
        //    SupplierModel supplierModel = new SupplierModel();
        //    PurchaseModel purchaseModel = new PurchaseModel();
        //    purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuListBySupplier(supplierId);
        //    supplierModel = _iSupplierService.GetSupplierById(supplierId);
        //    return Json(new { email = supplierModel.SupplierEmail, taxType = supplierModel.TaxType, purchaseModel.FoodMenuList });
        //}

        [HttpGet]
        public ActionResult GetFoodMenuList()
        {
            PurchaseModel purchaseModel = new PurchaseModel();
            purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuListByFoodmenuType(1);
            return Json(new { purchaseModel.FoodMenuList });
        }

        [HttpGet]
        public ActionResult GetIngredientList()
        {
            PurchaseModel purchaseModel = new PurchaseModel();
            purchaseModel.IngredientList = _iDropDownService.GetIngredientList();
            return Json(new { purchaseModel.IngredientList });
        }

        [HttpGet]
        public ActionResult GetAssetItemList()
        {
            PurchaseModel purchaseModel = new PurchaseModel();
            purchaseModel.AssetItemList = _iDropDownService.GetAssetItemList();
            return Json(new { purchaseModel.AssetItemList });
        }

        [HttpGet]
        public JsonResult SalesFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId)
        {
            List<SalesViewModel> purchaseViewModels = new List<SalesViewModel>();
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

            purchaseViewModels = _iSalesService.PurchaseFoodMenuListByDate(newFromDate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), customerId, storeId).ToList();
            return Json(new { PurchaseFoodMenu = purchaseViewModels });
        }

        [HttpGet]
        public ActionResult GetTaxByFoodMenuId(int foodMenuId, int itemType)
        {
            decimal taxPercentage = 0;
            taxPercentage = _iSalesService.GetTaxByFoodMenuId(foodMenuId, itemType);
            return Json(new { TaxPercentage = taxPercentage });
        }

        public ActionResult GetFoodMenuLastPrice(int itemType, int foodMenuId)
        {
            decimal unitPrice = 0;
            unitPrice = _iSalesService.GetFoodMenuLastPrice(itemType, foodMenuId);
            return Json(new { UnitPrice = unitPrice });
        }

        public ActionResult SalesDeliveryBySalesId(int id)
        {
            return RedirectToAction("SalesDeliveryFoodMenu", "SalesDelivery", new { purchaseId = id });
        }

        public ActionResult SalesInvoiceBySalesId(int id)
        {
            return RedirectToAction("SalesInvoiceFoodMenu", "SalesInvoice", new { purchaseId = id });
        }

        public ActionResult PurchaseApproveSuccess(int id)
        {
            int expiredDays = 0;
            SalesModel purchaseModel = new SalesModel();
            purchaseModel = _iSalesService.GetPurchaseFoodMenuById(id);
            ViewBag.PurchaseStatus = "Pending";
            ViewBag.ClientName = LoginInfo.ClientName;
            ViewBag.ClientAddress1 = LoginInfo.Address1;
            ViewBag.ClientAddress2 = LoginInfo.Address2;
            ViewBag.ClientEmail = LoginInfo.Email;
            ViewBag.ClientPhone = LoginInfo.Phone;
            ViewBag.UserName = LoginInfo.Username;
            ViewBag.PrintDate = System.DateTime.Now;

            expiredDays = (DateTime.Now - purchaseModel.DateInserted).Days;
            if (expiredDays > LoginInfo.ExpiryDays)
            {
                ViewBag.PurchaseStatus = "Expired";
            }
            if (purchaseModel.Status == 2)
            {
                ViewBag.PurchaseStatus = "Approved";
            }
            return View(purchaseModel);
        }

        public JsonResult ApprovePurchaseOrder(int id)
        {
            int result = 0;
            result = _iSalesService.ApprovePurchaseOrder(id);
            return Json(new { result = result });
        }

        [HttpGet]
        public JsonResult GetCustomerList()
        {
            SalesViewModel salesViewModel = new SalesViewModel();
            salesViewModel.CustomerList = _iDropDownService.GetCustomerList().ToList();
            return Json(new { CustomerList = salesViewModel.CustomerList });
        }

    }
}
