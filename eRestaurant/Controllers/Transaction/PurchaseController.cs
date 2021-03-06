using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using RocketPOS.Framework;

namespace RocketPOS.Controllers.Transaction
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseService _iPurchaseService;
        private readonly IDropDownService _iDropDownService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;


        public PurchaseController(IPurchaseService purchaseService,
            IDropDownService idropDownService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer,
            LocService locService)
        {
            _iPurchaseService = purchaseService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        // GET: Purchase
        public ActionResult PurchaseList()
        {
            List<PurchaseViewModel> purchaseList = new List<PurchaseViewModel>();
            purchaseList = _iPurchaseService.GetPurchaseList().ToList();
            return View(purchaseList);
        }

        // GET: Purchase/Details/5
        [HttpGet]
        public ActionResult GetOrderById(long purchaseId)
        {
            PurchaseModel purchaseModel = new PurchaseModel();
            purchaseModel = _iPurchaseService.GetPurchaseById(purchaseId);
            return View(purchaseModel);
        }

        // GET: Purchase/Create
        public ActionResult Purchase(long? id)
        {
            PurchaseModel purchaseModel = new PurchaseModel();
            if (id > 0)
            {
                long purchaseId = Convert.ToInt64(id);
                purchaseModel = _iPurchaseService.GetPurchaseById(purchaseId);
            }
            else
            {
                purchaseModel.Date = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                purchaseModel.ReferenceNo = _iPurchaseService.ReferenceNumber().ToString();
            }
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            purchaseModel.StoreList = _iDropDownService.GetStoreList();
            purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();
            purchaseModel.IngredientList = _iDropDownService.GetIngredientList();
            return View(purchaseModel);
        }

        // POST: Purchase/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Purchase(PurchaseModel purchaseModel)
        {
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            purchaseModel.StoreList = _iDropDownService.GetStoreList();
            purchaseModel.EmployeeList = _iDropDownService.GetEmployeeList();
            purchaseModel.IngredientList = _iDropDownService.GetIngredientList();
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
                    purchaseModel.InventoryType = 2;
                    if (purchaseModel.Id > 0)
                    {

                        int result = _iPurchaseService.UpdatePurchase(purchaseModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                        }
                    }
                    else
                    {
                        int result = _iPurchaseService.InsertPurchase(purchaseModel);
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
            return RedirectToAction(nameof(PurchaseList));
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