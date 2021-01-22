using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;


        public PurchaseFoodMenuController(IPurchaseService purchaseService,
            IDropDownService idropDownService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer,
            LocService locService)
        {
            _iPurchaseService = purchaseService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        // GET: PurchaseFoodMenu
        public ActionResult PurchaseFoodMenuList()
        {
          

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
        public ActionResult PurchaseFoodMenu(long? id)
        {
            PurchaseModel purchaseModel = new PurchaseModel();
            if (id > 0)
            {
                long purchaseId = Convert.ToInt64(id);
                purchaseModel = _iPurchaseService.GetPurchaseFoodMenuById(purchaseId);
            }
            else
            {
                purchaseModel.Date = DateTime.Now;
            }
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            purchaseModel.StoreList = _iDropDownService.GetStoreList();
            purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();
            return View(purchaseModel);
        }

        // POST: Purchase/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PurchaseFoodMenu(PurchaseModel purchaseModel, string Cancel)
        {
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
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
                        }
                    }
                    else
                    {
                        purchaseModel.ReferenceNo = _iPurchaseService.ReferenceNumberFoodMenu().ToString();
                        int result = _iPurchaseService.InsertPurchaseFoodMenu(purchaseModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("SaveSuccess") + " Reference No is: " + result.ToString();
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
            int result = _iPurchaseService.DeletePurchase(id);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return RedirectToAction(nameof(PurchaseFoodMenuList));
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
    }
}
