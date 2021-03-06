﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using RocketPOS.Framework;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;

namespace RocketPOS.Controllers.Master
{
    public class SupplierController : Controller
    {
        private readonly ICommonService _iCommonService;
        private readonly ISupplierService _iSupplierService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public SupplierController(ISupplierService supplierService, ICommonService iCommonService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iSupplierService = supplierService;
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index(int? noDelete)
        {
            _iCommonService.GetPageWiseRoleRigths("Supplier");
            List<SupplierModel> supplierList = new List<SupplierModel>();
            supplierList = _iSupplierService.GetSupplierList().ToList();
            if (noDelete != null)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Can not delete reference available.");
            }
            return View(supplierList);
        }

        public ActionResult Supplier(int? id)
        {
            SupplierModel supplierModel = new SupplierModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true)
            {
                if (id > 0)
                {
                    int supplierId = Convert.ToInt32(id);
                    supplierModel = _iSupplierService.GetSupplierById(supplierId);
                }
                ViewData["VatLabel"] = LoginInfo.VATLabel;
                ViewData["PinLabel"] = LoginInfo.PINLabel;
                return View(supplierModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Supplier(SupplierModel supplierModel, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationSupplier(supplierModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(supplierModel);
                }
            }


            if (supplierModel.Id > 0)
            {
                var result = _iSupplierService.UpdateSupplier(supplierModel);
                if (result == -1)
                {
                    ModelState.AddModelError("SupplierName", "Supplier name already exists");
                    return View(supplierModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iSupplierService.InsertSupplier(supplierModel);
                if (result == -1)
                {
                    ModelState.AddModelError("SupplierName", "Supplier name already exists");
                    return View(supplierModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "Supplier");
        }

        public ActionResult Delete(int id)
        {
            int result = 0;
            if (UserRolePermissionForPage.Delete == true)
            {
                result = _iCommonService.GetValidateReference("Supplier", id.ToString());
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index), new { noDelete = result });
                }
                else
                {
                    var deletedid = _iSupplierService.DeleteSupplier(id);
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        private string ValidationSupplier(SupplierModel supplierModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(supplierModel.SupplierName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(supplierModel.Price.ToString()) || supplierModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }

    }
}
