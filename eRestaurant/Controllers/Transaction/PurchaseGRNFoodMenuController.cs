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
    public class PurchaseGRNFoodMenuController : Controller
    {
        private readonly IPurchaseGRNService _iPurchaseGRNService;
        private readonly IDropDownService _iDropDownService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;
        private readonly ISupplierService _iSupplierService;
        private readonly IEmailService _iEmailService;

        public PurchaseGRNFoodMenuController(IPurchaseGRNService purchaseService,
            IDropDownService idropDownService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer,
            LocService locService,
            ISupplierService supplierService,
            IEmailService emailService)
        {
            _iPurchaseGRNService = purchaseService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
            _iSupplierService = supplierService;
            _iEmailService = emailService;
        }

        // GET: PurchaseGRNFoodMenu
        public ActionResult PurchaseGRNFoodMenuList()
        {
            List<PurchaseGRNViewModel> purchaseList = new List<PurchaseGRNViewModel>();
            purchaseList = _iPurchaseGRNService.GetPurchaseGRNFoodMenuList().ToList();
            return View(purchaseList);
        }

        // GET: PurchaseGRNFoodMenu/Details/5
        [HttpGet]
        public ActionResult GetOrderById(long purchaseId)
        {
            PurchaseGRNModel purchaseModel = new PurchaseGRNModel();
            purchaseModel = _iPurchaseGRNService.GetPurchaseGRNFoodMenuById(purchaseId);
            return View(purchaseModel);
        }

        // GET: PurchaseGRN/Create
        public ActionResult PurchaseGRNFoodMenu(long? id)
        {
            PurchaseGRNModel purchaseModel = new PurchaseGRNModel();
            if (id > 0)
            {
                long purchaseId = Convert.ToInt64(id);
                purchaseModel = _iPurchaseGRNService.GetPurchaseGRNFoodMenuById(purchaseId);
            }
            else
            {
                purchaseModel.PurchaseGRNDate = DateTime.Now;
            }
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            purchaseModel.StoreList = _iDropDownService.GetStoreList();
            //purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();
            return View(purchaseModel);
        }

        // POST: PurchaseGRN/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PurchaseGRNFoodMenu(PurchaseGRNModel purchaseModel, string Cancel)
        {
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            //purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            purchaseModel.StoreList = _iDropDownService.GetStoreList();
            purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();
            string purchaseMessage = string.Empty;
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationPurchaseGRN(purchaseModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    return Json(new { error = true, message = errorString, status = 201 });
                }
            }

            if (purchaseModel.PurchaseGRNDetails != null)
            {
                if (purchaseModel.PurchaseGRNDetails.Count > 0)
                {
                    purchaseModel.InventoryType = 1;
                    if (purchaseModel.Id > 0)
                    {

                        int result = _iPurchaseGRNService.UpdatePurchaseGRNFoodMenu(purchaseModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                            if (purchaseModel.IsSendEmail)
                            {
                                if (!string.IsNullOrEmpty(purchaseModel.SupplierEmail))
                                {
                                  //  _iEmailService.SendEmailToForFoodMenuPurchaseGRN(purchaseModel, purchaseModel.SupplierEmail);
                                }
                            }
                        }
                    }
                    else
                    {
                        purchaseModel.ReferenceNo = _iPurchaseGRNService.ReferenceNumberFoodMenu().ToString();

                        int result = _iPurchaseGRNService.InsertPurchaseGRNFoodMenu(purchaseModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("SaveSuccess") + " Reference No is: " + result.ToString();
                            if (purchaseModel.IsSendEmail)
                            {
                                if (!string.IsNullOrEmpty(purchaseModel.SupplierEmail))
                                {
                                  //  _iEmailService.SendEmailToForFoodMenuPurchaseGRN(purchaseModel, purchaseModel.SupplierEmail);
                                }
                            }
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
                return Json(new { error = true, message = purchaseMessage, status = 201 });
            }
            // return View(purchaseModel);
            return Json(new { error = false, message = purchaseMessage, status = 200 });
            //return View();
        }

        public ActionResult Delete(int id)
        {
            int result = _iPurchaseGRNService.DeletePurchaseGRN(id);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return RedirectToAction(nameof(PurchaseGRNFoodMenuList));
        }

        public ActionResult DeletePurchaseGRNDetails(long purchaseId)
        {
            long result = _iPurchaseGRNService.DeletePurchaseGRNDetails(purchaseId);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return Json(new { error = false, message = string.Empty, status = 200 });
        }

        private string ValidationPurchaseGRN(PurchaseGRNModel purchaseModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(purchaseModel.SupplierId.ToString()) || purchaseModel.SupplierId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSupplier");
                return ErrorString;
            }
            if (purchaseModel.PurchaseGRNDetails == null || purchaseModel.PurchaseGRNDetails.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidPurchaseGRNDetails");
                return ErrorString;
            }

            return ErrorString;
        }

        [HttpGet]
        public ActionResult GetSupplierDetail(int supplierId)
        {
            SupplierModel supplierModel = new SupplierModel();
            PurchaseGRNModel purchaseModel = new PurchaseGRNModel();
            purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuListBySupplier(supplierId);
            supplierModel = _iSupplierService.GetSupplierById(supplierId);
            return Json(new { email = supplierModel.SupplierEmail, purchaseModel.FoodMenuList });
        }

        [HttpGet]
        public ActionResult GetFoodMenuList()
        {
            PurchaseGRNModel purchaseModel = new PurchaseGRNModel();
            purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            return Json(new { purchaseModel.FoodMenuList });
        }

        [HttpGet]
        public ActionResult GetTaxByFoodMenuId(int foodMenuId)
        {
            decimal taxPercentage = 0;
            taxPercentage = _iPurchaseGRNService.GetTaxByFoodMenuId(foodMenuId);
            return Json(new { TaxPercentage = taxPercentage });
        }

        public ActionResult GetFoodMenuLastPrice(int foodMenuId)
        {
            decimal unitPrice = 0;
            unitPrice = _iPurchaseGRNService.GetFoodMenuLastPrice(foodMenuId);
            return Json(new { UnitPrice = unitPrice });
        }
    }
}
