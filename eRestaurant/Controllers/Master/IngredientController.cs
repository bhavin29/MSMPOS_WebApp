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
        private readonly ICommonService _iCommonService;
        private readonly IIngredientService _iIngredientService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public IngredientController(IIngredientService ingredientService, ICommonService iCommonService, IDropDownService idropDownService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iIngredientService = ingredientService; _iCommonService = iCommonService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        // GET: Ingredient
        public IActionResult Index(int? noDelete)
        {
            _iCommonService.GetPageWiseRoleRigths("Ingredient");
            List<IngredientModel> ingredientList = new List<IngredientModel>();
            ingredientList = _iIngredientService.GetIngredientList().ToList();
            if (noDelete != null)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Can not delete reference available.");
            }
            return View(ingredientList);
        }

        // GET: Ingredient/Ingredient
        public ActionResult Ingredient(int? id)
        {
            IngredientModel ingredientModel = new IngredientModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true)
            {
                if (id > 0)
                {
                    int ingredientId = Convert.ToInt32(id);
                    ingredientModel = _iIngredientService.GetIngredientById(ingredientId);
                }

                ingredientModel.IngredientCategoryList = _iDropDownService.GetIngredientCategoryList();
                ingredientModel.UnitList = _iDropDownService.GetUnitList();
                ingredientModel.TaxList = _iDropDownService.GetTaxList();
                return View(ingredientModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        // POST: Ingredient/Ingredient
        [HttpPost]
        public ActionResult Ingredient(IngredientModel ingredientModel, string submitButton)
        {
            ingredientModel.IngredientCategoryList = _iDropDownService.GetIngredientCategoryList();
            ingredientModel.UnitList = _iDropDownService.GetUnitList();
            ingredientModel.TaxList = _iDropDownService.GetTaxList();
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
                if (result == -1)
                {
                    ModelState.AddModelError("IngredientName", "Stock item already exists");
                    return View(ingredientModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iIngredientService.InsertIngredient(ingredientModel);
                if (result == -1)
                {
                    ModelState.AddModelError("IngredientName", "Stock item already exists");
                    return View(ingredientModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }
            return RedirectToAction("Index", "Ingredient");
        }

        // GET: Ingredient/Delete/5
        public ActionResult Delete(int id)
        {
            int result = 0;
            if (UserRolePermissionForPage.Delete == true)
            {
                result = _iCommonService.GetValidateReference("Ingredient", id.ToString());
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index), new { noDelete = result });
                }
                else
                {
                    var deleteid = _iIngredientService.DeleteIngredient(id);
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
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
            if (string.IsNullOrEmpty(ingredientModel.TaxId.ToString()) || ingredientModel.TaxId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidTax");
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