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
                int addonsId = Convert.ToInt32(id);
                varientModel = _iVarientService.GetAddonesById(addonsId);
            }
            varientModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            return View(varientModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Varient(VarientModel varientModel, string submitButton)
        {
            if (varientModel.Id > 0)
            {
                var result = _iVarientService.UpdateVarient(varientModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _iVarientService.InsertVarient(varientModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }
            varientModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            return View();
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iVarientService.DeleteVarient(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
