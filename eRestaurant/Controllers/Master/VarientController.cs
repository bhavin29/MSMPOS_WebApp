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
    public class VarientController : Controller
    {
        private readonly IVarientService _iVarientService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public VarientController(IVarientService varientService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _iVarientService = varientService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
        }

        public ActionResult Index()
        {
            List<VarientModel> varientModel = new List<VarientModel>();
            varientModel = _iVarientService.GetVarientList().ToList();
            return View(varientModel);
        }

        public ActionResult Varient(int? id)
        {
            VarientModel varientModel = new VarientModel();
            if (id > 0)
            {
                int varientId = Convert.ToInt32(id);
                varientModel = _iVarientService.GetAddonesById(varientId);
            }
            varientModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            return View(varientModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Varient(VarientModel varientModel, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationVarient(varientModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(varientModel);
                }
            }

            if (varientModel.Id > 0)
            {
                var result = _iVarientService.UpdateVarient(varientModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iVarientService.InsertVarient(varientModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }
            varientModel.FoodMenuList = _iDropDownService.GetFoodMenuList();

            return RedirectToAction("Index", "Varient");
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iVarientService.DeleteVarient(id);

            return RedirectToAction(nameof(Index));
        }

        private string ValidationVarient(VarientModel varientModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(varientModel.VarientName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(varientModel.Price.ToString()) || varientModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }

    }
}
