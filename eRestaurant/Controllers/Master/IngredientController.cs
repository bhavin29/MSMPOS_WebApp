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
    public class IngredientController : Controller
    {
        private readonly IIngredientService _iIngredientService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public IngredientController(IIngredientService ingredientService,
            IDropDownService idropDownService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iIngredientService = ingredientService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        // GET: Ingredient
        public IActionResult Index()
        {
            List<IngredientModel> ingredientList = new List<IngredientModel>();
            ingredientList = _iIngredientService.GetIngredientList().ToList();
            return View(ingredientList);
        }

        // GET: Ingredient/Ingredient
        public ActionResult Ingredient(int? id)
        {
            IngredientModel ingredientModel = new IngredientModel();
            if (id > 0)
            {
                int ingredientId = Convert.ToInt32(id);
                ingredientModel = _iIngredientService.GetIngredientById(ingredientId);
                ingredientModel.IngredientCategoryList = _iDropDownService.GetIngredientCategoryList();
                ingredientModel.UnitList = _iDropDownService.GetUnitList();
            }
            else
            {
                ingredientModel.IngredientCategoryList = _iDropDownService.GetIngredientCategoryList();
                ingredientModel.UnitList = _iDropDownService.GetUnitList();
            }
            return View(ingredientModel);
        }

        // POST: Ingredient/Ingredient
        [HttpPost]
        public ActionResult Ingredient(IngredientModel ingredientModel, string submitButton)
        {
            ingredientModel.IngredientCategoryList = _iDropDownService.GetIngredientCategoryList();
            ingredientModel.UnitList = _iDropDownService.GetUnitList();
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationIngredient(ingredientModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(ingredientModel);
                }
            }
            if (ingredientModel.Id > 0)
            {
                var result = _iIngredientService.UpdateIngredient(ingredientModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iIngredientService.InsertIngredient(ingredientModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }
            return View(ingredientModel);
        }

        // GET: Ingredient/Delete/5
        public ActionResult Delete(int id)
        {
            var deleteid = _iIngredientService.DeleteIngredient(id);
            return RedirectToAction(nameof(Index));
        }

        private string ValidationIngredient(IngredientModel ingredientModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(ingredientModel.IngredientName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidIngredientName");
                return ErrorString;
            }
            if (string.IsNullOrEmpty(ingredientModel.CategoryId.ToString()) || ingredientModel.CategoryId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidCategory");
                return ErrorString;
            }
            if (string.IsNullOrEmpty(ingredientModel.UnitId.ToString()) || ingredientModel.UnitId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidUnit");
                return ErrorString;
            }
            if (string.IsNullOrEmpty(ingredientModel.SalesPrice.ToString()) || ingredientModel.SalesPrice == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSalesPrice");
                return ErrorString;
            }

            return ErrorString;
        }
    }
}