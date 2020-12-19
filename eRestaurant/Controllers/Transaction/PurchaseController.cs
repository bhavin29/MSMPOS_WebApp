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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Purchase/Create
        public ActionResult Purchase()
        {
            PurchaseModel purchaseModel = new PurchaseModel();
            purchaseModel.SupplierList = _iDropDownService.GetSupplierList();
            purchaseModel.IngredientList = _iDropDownService.GetIngredientList();
            return View(purchaseModel);
        }

        // POST: Purchase/Create
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SavePurchase(PurchaseModel purchaseModel)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(PurchaseList));
            }
            catch
            {
                return View();
            }
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
    }
}