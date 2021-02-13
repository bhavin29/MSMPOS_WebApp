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
    public class IngredientCategoryController : Controller
    {
        private readonly IIngredientCategoryService _iIngredientCategoryService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public IngredientCategoryController(IIngredientCategoryService ingredientCategoryService, IDropDownService idropDownService,
  IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iIngredientCategoryService = ingredientCategoryService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        // GET: IngredientCategoryController
        public ActionResult Index()
        {
            List<IngredientCategoryModel> ingredientCategoryList = new List<IngredientCategoryModel>();
            ingredientCategoryList = _iIngredientCategoryService.GetIngredientCategoryList().ToList();
            return View(ingredientCategoryList);
        }

        // GET: IngredientCategoryController/Details/5
        public ActionResult IngredientCategory(int? id)
        {
            IngredientCategoryModel ingredientCategoryModel = new IngredientCategoryModel();
            if (id > 0)
            {
                int ingredientCategroyId = Convert.ToInt32(id);
                ingredientCategoryModel = _iIngredientCategoryService.GetIngredientCategoryById(ingredientCategroyId);
            }
            ingredientCategoryModel.RawMaterialList = _iDropDownService.GetRawMaterialList();

            return View(ingredientCategoryModel);
        }

        // POST: IngredientCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IngredientCategory(IngredientCategoryModel ingredientCategoryModel, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationIngredientCategory(ingredientCategoryModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(ingredientCategoryModel);
                }
            }

            if (ingredientCategoryModel.Id > 0)
            {
                var result = _iIngredientCategoryService.UpdateIngredientCategory(ingredientCategoryModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iIngredientCategoryService.InsertIngredientCategory(ingredientCategoryModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }
            ingredientCategoryModel.RawMaterialList = _iDropDownService.GetRawMaterialList();

            return RedirectToAction("Index", "IngredientCategory");
        }

        // GET: IngredientCategory/Delete/5
        public ActionResult Delete(int id)
        {
            var deletedid = _iIngredientCategoryService.DeleteIngredientCategory(id);

            return RedirectToAction(nameof(Index));
        }

        private string ValidationIngredientCategory(IngredientCategoryModel ingredientCategoryModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(ingredientCategoryModel.IngredientCategoryName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(ingredientCategoryModel.Price.ToString()) || ingredientCategoryModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }

    }
}
