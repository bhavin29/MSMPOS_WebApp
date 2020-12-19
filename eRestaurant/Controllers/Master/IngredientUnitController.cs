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
    public class IngredientUnitController : Controller
    {
        private readonly IIngredientUnitService _iIngredientUnitService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;

        public IngredientUnitController(IIngredientUnitService ingredientUnitService, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _iIngredientUnitService = ingredientUnitService;
            _sharedLocalizer = sharedLocalizer;
        }

        public ActionResult Index()
        {
            List<IngredientUnitModel> ingredientUniList = new List<IngredientUnitModel>();
            ingredientUniList = _iIngredientUnitService.GetIngredientUnitList().ToList();
            return View(ingredientUniList);
 
        }

        public ActionResult IngredientUnit(int? id)
        {
            IngredientUnitModel ingredientUniList = new IngredientUnitModel();
            if (id > 0)
            {
                int ingredientUnitId = Convert.ToInt32(id);
                ingredientUniList = _iIngredientUnitService.GetIngredientUnitById(ingredientUnitId);
            }

            return View(ingredientUniList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IngredientUnit(IngredientUnitModel ingredientUniModel, string submitButton)
        {
            if (ingredientUniModel.Id > 0)
            {
                var result = _iIngredientUnitService.UpdateIngredientUnit(ingredientUniModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _iIngredientUnitService.InsertIngredientUnit(ingredientUniModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iIngredientUnitService.DeleteIngredientUnit(id);

            return RedirectToAction(nameof(Index));
        }


    }
}
