using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;

namespace RocketPOS.Controllers.Master
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _iSupplierService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;

        public SupplierController(ISupplierService supplierService, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _iSupplierService = supplierService;
            _sharedLocalizer = sharedLocalizer;
        }

        public ActionResult Index()
        {
            List<SupplierModel> supplierList = new List<SupplierModel>();
            supplierList = _iSupplierService.GetSupplierList().ToList();
            return View(supplierList);
        }

        public ActionResult Supplier(int? id)
        {
            SupplierModel supplierModel = new SupplierModel();
            if (id > 0)
            {
                int supplierId = Convert.ToInt32(id);
                supplierModel = _iSupplierService.GetSupplierById(supplierId);
            }

            return View(supplierModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Supplier(SupplierModel supplierModel, string submitButton)
        {
            if (supplierModel.Id > 0)
            {
                var result = _iSupplierService.UpdateSupplier(supplierModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _iSupplierService.InsertSupplier(supplierModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iSupplierService.DeleteSupplier(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
