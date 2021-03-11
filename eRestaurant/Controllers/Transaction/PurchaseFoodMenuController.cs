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
    public class PurchaseFoodMenuController : Controller
    {
        private readonly IPurchaseService _iPurchaseService;
        private readonly IDropDownService _iDropDownService;
        private readonly ICommonService _iCommonService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;
        private readonly ISupplierService _iSupplierService;
        private readonly IEmailService _iEmailService;

        public PurchaseFoodMenuController(IPurchaseService purchaseService,
            IDropDownService idropDownService,
            ICommonService iCommonService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer,
            LocService locService,
            ISupplierService supplierService,
            IEmailService emailService)
        {
            _iPurchaseService = purchaseService;
            _iDropDownService = idropDownService;
            _iCommonService = iCommonService;
            _iSupplierService = supplierService;
            _iEmailService = emailService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        // GET: PurchaseFoodMenu
        public ActionResult PurchaseFoodMenuList()
        {
            _iCommonService.GetPageWiseRoleRigths("PurchaseFoodMenu");
            List<PurchaseViewModel> purchaseList = new List<PurchaseViewModel>();
            purchaseList = _iPurchaseService.GetPurchaseFoodMenuList().ToList();
            return View(purchaseList);
        }

        // GET: PurchaseFoodMenu/Details/5
        [HttpGet]
        public ActionResult GetOrderById(long purchaseId)
        {
            PurchaseModel purchaseModel = new PurchaseModel();
            purchaseModel = _iPurchaseService.GetPurchaseFoodMenuById(purchaseId);
            return View(purchaseModel);
        }

        // GET: Purchase/Create
        public ActionResult PurchaseFoodMenu(long? id, string type)
        {
            PurchaseModel purchaseModel = new PurchaseModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true || UserRolePermissionForPage.View == true)
            {
                if (id > 0)
                {
                    ViewBag.ActionType = type;
                    long purchaseId = Convert.ToInt64(id);
                    purchaseModel = _iPurchaseService.GetPurchaseFoodMenuById(purchaseId);
                }
                else
                {
                    purchaseModel.Date = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                    purchaseModel.ReferenceNo = _iPurchaseService.ReferenceNumberFoodMenu().ToString();
                }
                purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
                purchaseModel.StoreList = _iDropDownService.GetStoreList();
                //purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
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
        public ActionResult PurchaseFoodMenu(PurchaseModel purchaseModel, string Cancel)
        {   
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
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

            if (purchaseModel.PurchaseDetails != null)
            {
                if (purchaseModel.PurchaseDetails.Count > 0)
                {
                    purchaseModel.InventoryType = 1;
                    if (purchaseModel.Id > 0)
                    {
                        int result = _iPurchaseService.UpdatePurchaseFoodMenu(purchaseModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                            ClientModel clientModel = new ClientModel();
                            clientModel = _iPurchaseService.GetClientDetail();
                            _iEmailService.SendEmailToForFoodMenuPurchase(Convert.ToInt32(purchaseModel.Id), clientModel);

                            //    if (purchaseModel.IsSendEmail)
                            //    {
                            //        if (!string.IsNullOrEmpty(purchaseModel.SupplierEmail))
                            //        {
                            //            _iEmailService.SendEmailToForFoodMenuPurchase(purchaseModel, purchaseModel.SupplierEmail);
                            //        }
                            //    }
                        }
                    }
                    else
                    {
                        //purchaseModel.Date = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                        purchaseModel.ReferenceNo = _iPurchaseService.ReferenceNumberFoodMenu().ToString();

                        string result = _iPurchaseService.InsertPurchaseFoodMenu(purchaseModel);
                        if (result != "")
                        {
                            int purchaseId = 0;
                            purchaseMessage = _locService.GetLocalizedHtmlString("SaveSuccess") + " Reference No is: " + result.ToString();
                            ClientModel clientModel = new ClientModel();
                            clientModel = _iPurchaseService.GetClientDetail();
                            purchaseId = _iPurchaseService.GetPurchaseIdByReferenceNo(result);
                            _iEmailService.SendEmailToForFoodMenuPurchase(purchaseId, clientModel);
                            //if (purchaseModel.IsSendEmail)
                            //{
                            //    if (!string.IsNullOrEmpty(purchaseModel.SupplierEmail))
                            //    {
                            //        _iEmailService.SendEmailToForFoodMenuPurchase(purchaseModel, clientModel);
                            //    }
                            //}
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
                int result = _iPurchaseService.DeletePurchase(id);
                if (result > 0)
                {
                    ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
                }
                return RedirectToAction(nameof(PurchaseFoodMenuList));
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public ActionResult DeletePurchaseDetails(long purchaseId)
        {
            long result = _iPurchaseService.DeletePurchaseDetails(purchaseId);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return Json(new { error = false, message = string.Empty, status = 200 });
        }

        private string ValidationPurchase(PurchaseModel purchaseModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(purchaseModel.SupplierId.ToString()) || purchaseModel.SupplierId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSupplier");
                return ErrorString;
            }
            if (purchaseModel.PurchaseDetails == null || purchaseModel.PurchaseDetails.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidPurchaseDetails");
                return ErrorString;
            }

            return ErrorString;
        }

        [HttpGet]
        public ActionResult GetSupplierDetail(int supplierId)
        {
            SupplierModel supplierModel = new SupplierModel();
            PurchaseModel purchaseModel = new PurchaseModel();
            purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuListBySupplier(supplierId);
            supplierModel = _iSupplierService.GetSupplierById(supplierId);
            return Json(new { email = supplierModel.SupplierEmail, taxType = supplierModel.TaxType, purchaseModel.FoodMenuList });
        }

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
        public JsonResult PurchaseFoodMenuListByDate(string fromDate, string toDate, int supplierId)
        {
            List<PurchaseViewModel> purchaseViewModels = new List<PurchaseViewModel>();
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

            purchaseViewModels = _iPurchaseService.PurchaseFoodMenuListByDate(newFromDate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), supplierId).ToList();
            return Json(new { PurchaseFoodMenu = purchaseViewModels });
        }

        [HttpGet]
        public ActionResult GetTaxByFoodMenuId(int foodMenuId,int itemType)
        {
            decimal taxPercentage = 0;
            taxPercentage = _iPurchaseService.GetTaxByFoodMenuId(foodMenuId, itemType);
            return Json(new { TaxPercentage = taxPercentage });
        }

        public ActionResult GetFoodMenuLastPrice(int itemType, int foodMenuId)
        {
            decimal unitPrice = 0;
            unitPrice = _iPurchaseService.GetFoodMenuLastPrice(itemType, foodMenuId);
            return Json(new { UnitPrice = unitPrice });
        }

        public ActionResult PurchaseGRNByPurchaseId(int id)
        {
            return RedirectToAction("PurchaseGRNFoodMenu", "PurchaseGRNFoodMenu", new { purchaseId = id });
        }

        public ActionResult PurchaseInvoiceByPurchaseId(int id)
        {
            return RedirectToAction("PurchaseInvoiceFoodMenu", "PurchaseInvoiceFoodMenu", new { purchaseId = id });
        }

        public ActionResult PurchaseApproveSuccess(int id)
        {
            int expiredDays = 0;
            PurchaseModel purchaseModel = new PurchaseModel();
            purchaseModel = _iPurchaseService.GetPurchaseFoodMenuById(id);
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
            result = _iPurchaseService.ApprovePurchaseOrder(id);
            return Json(new { result = result });
        }

        public ActionResult View(long? id)
        {
            PurchaseModel purchaseModel = new PurchaseModel();
            if (id > 0)
            {
                long purchaseId = Convert.ToInt64(id);
                purchaseModel = _iPurchaseService.GetViewPurchaseFoodMenuById(purchaseId);
            }
            else
            {
                purchaseModel.Date = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                purchaseModel.ReferenceNo = _iPurchaseService.ReferenceNumberFoodMenu().ToString();
            }
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            purchaseModel.StoreList = _iDropDownService.GetStoreList();
            purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();
            return View(purchaseModel);
        }
    }
}
