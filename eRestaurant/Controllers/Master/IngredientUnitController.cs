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
        private LocService _locService;

        public IngredientUnitController(IIngredientUnitService ingredientUnitService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iIngredientUnitService = ingredientUnitService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
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
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationIngredientUnit(ingredientUniModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(ingredientUniModel);
                }
            }

            if (ingredientUniModel.Id > 0)
            {
                var result = _iIngredientUnitService.UpdateIngredientUnit(ingredientUniModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iIngredientUnitService.InsertIngredientUnit(ingredientUniModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "IngredientUnit");
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iIngredientUnitService.DeleteIngredientUnit(id);

            return RedirectToAction(nameof(Index));
        }

        private string ValidationIngredientUnit(IngredientUnitModel ingredientUnitModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(ingredientUnitModel.IngredientUnitName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(ingredientUnitModel.Price.ToString()) || ingredientUnitModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }



    }
}
