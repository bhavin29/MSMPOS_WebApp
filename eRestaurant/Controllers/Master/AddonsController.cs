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

namespace RocketPOS.Controllers
{
    public class AddonsController : Controller
    {
        private readonly IAddonsService _iAddonsService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public AddonsController(IAddonsService addonsService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iAddonsService = addonsService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            List<AddonsModel> adonsList = new List<AddonsModel>();
            adonsList = _iAddonsService.GetAddonsList().ToList();
            return View(adonsList);
        }

        public ActionResult Addons(int? id)
        {
            AddonsModel addonsModel = new AddonsModel();
            if (id > 0)
            {
                int addonsId = Convert.ToInt32(id);
                addonsModel = _iAddonsService.GetAddonesById(addonsId);
            }

            return View(addonsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addons(AddonsModel addonsModel, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationAddons(addonsModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(addonsModel);
                }
            }

            if (addonsModel.Id > 0)
            {
                var result = _iAddonsService.UpdateAddons(addonsModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iAddonsService.InsertAddons(addonsModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "Addons");

            //return View(addonsModel);
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iAddonsService.DeleteAddons(id);

            return RedirectToAction(nameof(Index));
        }

        private string ValidationAddons(AddonsModel addonsModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(addonsModel.AddonsName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(addonsModel.Price.ToString()) || addonsModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}
  
            return ErrorString;
        }

    }
}
