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

namespace RocketPOS.Controllers.Transaction
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseService _iPurchaseService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;


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
        public ActionResult GetOrderById(int purchaseId)
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
                int purchaseId = Convert.ToInt32(id);
                purchaseModel = _iPurchaseService.GetPurchaseById(purchaseId);
                
            }
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            purchaseModel.IngredientList = _iDropDownService.GetIngredientList();
            return View(purchaseModel);
        }

        // POST: Purchase/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Purchase(PurchaseModel purchaseModel)
        {
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            purchaseModel.IngredientList = _iDropDownService.GetIngredientList();
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationPurchase(purchaseModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    purchaseModel.Message = errorString;
                    return View(purchaseModel);
                }
            }

            if (purchaseModel.Id > 0)
            {
                int result = _iPurchaseService.UpdatePurchase(purchaseModel);
                if (result > 0)
                {
                    ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
                }
            }
            else
            {
                int result = _iPurchaseService.InsertPurchase(purchaseModel);
                if (result > 0)
                {
                    ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
                }
            }
            // return View(purchaseModel);
            return Json(new { error = false, message = "Ok" });
            //return View();
        }

        // GET: Purchase/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Purchase/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Purchase/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Purchase/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private string ValidationPurchase(PurchaseModel purchaseModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(purchaseModel.ReferenceNo.ToString()) || purchaseModel.ReferenceNo == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidReferenceNo");
                return ErrorString;
            }
            if (string.IsNullOrEmpty(purchaseModel.SupplierId.ToString()) || purchaseModel.SupplierId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSupplier");
                return ErrorString;
            }
            if (purchaseModel.PurchaseDetails.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidPurchaseDetails");
                return ErrorString;
            }

            return ErrorString;
        }
    }
}