using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Master
{
    public class TaxController : Controller
    {
        private readonly ICommonService _iCommonService;
        private readonly ITaxService _iTaxService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public TaxController(ITaxService iTaxService, ICommonService iCommonService,IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iTaxService = iTaxService;
            _sharedLocalizer = sharedLocalizer; _iCommonService = iCommonService;
            _locService = locService;
        }

        public ActionResult Index(int? noDelete)
        {
            List<TaxModel> taxModel = new List<TaxModel>();
            taxModel = _iTaxService.GetTaxList().ToList();
            if (noDelete != null)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Can not delete reference available.");
            }
            return View(taxModel);
        }
            
        public ActionResult Tax(int? id)
        {
            TaxModel taxModel = new TaxModel();
            if (id > 0)
            {
                taxModel = _iTaxService.GetTaxById(Convert.ToInt32(id));
            }

            return View(taxModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tax(TaxModel taxModel)
        {

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationTax(taxModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(taxModel);
                }
            }

            if (taxModel.Id > 0)
            {
                var result = _iTaxService.UpdateTax(taxModel);
                if (result == -1)
                {
                    ModelState.AddModelError("TaxName", "Tax name already exists");
                    return View(taxModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iTaxService.InsertTax(taxModel);
                if (result == -1)
                {
                    ModelState.AddModelError("TaxName", "Tax name already exists");
                    return View(taxModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "Tax");
        }

        public ActionResult Delete(int id)
        {
            int result = 0;
            result = _iCommonService.GetValidateReference("Tax", id.ToString());
            if (result > 0)
            {

                return RedirectToAction(nameof(Index), new { noDelete = result });
            }
            else
            {
                var deletedid = _iTaxService.DeleteTax(id);
                return RedirectToAction(nameof(Index));
            }
        }

        private string ValidationTax(TaxModel taxModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(taxModel.TaxName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidTaxName");
                return ErrorString;
            }
            return ErrorString;
        }
    }
}
    