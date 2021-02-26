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
    public class RawMaterialController : Controller
    {
        private readonly IRawMaterialService _iRawMaterialService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public RawMaterialController(IRawMaterialService iRawMaterialService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iRawMaterialService = iRawMaterialService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            List<RawMaterialModel> rawMaterialModel = new List<RawMaterialModel>();
            rawMaterialModel = _iRawMaterialService.GetRawMaterialList().ToList();
            return View(rawMaterialModel);
        }

        public ActionResult RawMaterial(int? id)
        {
            RawMaterialModel rawMaterialModel = new RawMaterialModel();
            if (id > 0)
            {
                rawMaterialModel = _iRawMaterialService.GetRawMaterialById(Convert.ToInt32(id));
            }

            return View(rawMaterialModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RawMaterial(RawMaterialModel rawMaterialModel)
        {

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationRawMaterial(rawMaterialModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(rawMaterialModel);
                }
            }

            if (rawMaterialModel.Id > 0)
            {
                var result = _iRawMaterialService.UpdateRawMaterial(rawMaterialModel);
                if (result == -1)
                {
                    ModelState.AddModelError("RawMaterialName", "Raw Material already exists");
                    return View(rawMaterialModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iRawMaterialService.InsertRawMaterial(rawMaterialModel);
                if (result == -1)
                {
                    ModelState.AddModelError("RawMaterialName", "Raw Material already exists");
                    return View(rawMaterialModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "RawMaterial");
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iRawMaterialService.DeleteRawMaterial(id);
            return RedirectToAction(nameof(Index));
        }

        private string ValidationRawMaterial(RawMaterialModel rawMaterialModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(rawMaterialModel.RawMaterialName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            return ErrorString;
        }
    }
}
