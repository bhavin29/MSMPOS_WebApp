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
        private readonly ICommonService _iCommonService;
        private readonly IIngredientCategoryService _iIngredientCategoryService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public IngredientCategoryController(IIngredientCategoryService ingredientCategoryService, ICommonService iCommonService,IDropDownService idropDownService,
  IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iIngredientCategoryService = ingredientCategoryService;
            _iDropDownService = idropDownService; _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        // GET: IngredientCategoryController
        public ActionResult Index(int? noDelete)
        {
            List<IngredientCategoryModel> ingredientCategoryList = new List<IngredientCategoryModel>();
            ingredientCategoryList = _iIngredientCategoryService.GetIngredientCategoryList().ToList();
            if (noDelete != null)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Can not delete reference available.");
            }
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
                if (result == -1)
                {
                    ModelState.AddModelError("IngredientCategoryName", "Category already exists");
                    ingredientCategoryModel.RawMaterialList = _iDropDownService.GetRawMaterialList();
                    return View(ingredientCategoryModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iIngredientCategoryService.InsertIngredientCategory(ingredientCategoryModel);
                if (result == -1)
                {
                    ModelState.AddModelError("IngredientCategoryName", "Category already exists");
                    ingredientCategoryModel.RawMaterialList = _iDropDownService.GetRawMaterialList();
                    return View(ingredientCategoryModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }
            ingredientCategoryModel.RawMaterialList = _iDropDownService.GetRawMaterialList();

            return RedirectToAction("Index", "IngredientCategory");
        }

        // GET: IngredientCategory/Delete/5
        public ActionResult Delete(int id)
        {
            int result = 0;
            result = _iCommonService.GetValidateReference("IngredientCategory", id.ToString());
            if (result > 0)
            {
                return RedirectToAction(nameof(Index), new { noDelete = result });
            }
            else
            {
                var deletedid = _iIngredientCategoryService.DeleteIngredientCategory(id);
                return RedirectToAction(nameof(Index));
            }
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
