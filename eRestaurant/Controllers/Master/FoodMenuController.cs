using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using RocketPOS.Framework;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;

namespace RocketPOS.Controllers.Master
{
    public class FoodMenuController : Controller
    {
        private readonly ICommonService _iCommonService;
        private readonly IFoodMenuService _iFoodMenuService;
        private readonly IIngredientUnitService _iIngredientUnitService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public FoodMenuController(IFoodMenuService foodMenuService, ICommonService iCommonService,IIngredientUnitService iIngredientUnitService,IDropDownService idropDownService,  IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iFoodMenuService = foodMenuService; _iCommonService = iCommonService;
            _iIngredientUnitService = iIngredientUnitService;
            _iDropDownService = idropDownService;
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index(int? readymade, int? categoryid, int? foodmenutype)
        {
            _iCommonService.GetPageWiseRoleRigths("FoodMenu");
            List<FoodMenuModel> foodMenuModel = new List<FoodMenuModel>();
            if (categoryid != null && foodmenutype != null)
            {
                foodMenuModel = _iFoodMenuService.GetFoodMenuList(Convert.ToInt32(categoryid), Convert.ToInt32(foodmenutype)).ToList();
            }
            return View(foodMenuModel);
        }

        public ActionResult FoodMenu(int? id)
        {
            FoodMenuModel foodMenuModel = new FoodMenuModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true)
            {
                if (id > 0)
                {
                    int foodMenuId = Convert.ToInt32(id);
                    foodMenuModel = _iFoodMenuService.GetFoodMenueById(foodMenuId);
                }
                foodMenuModel.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
                foodMenuModel.FoodVatTaxList = _iDropDownService.GetTaxList();
                foodMenuModel.UnitsList = _iDropDownService.GetUnitList();
                return View(foodMenuModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FoodMenu(FoodMenuModel foodMenuModel, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationFoodMenu(foodMenuModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(foodMenuModel);
                }
            }

            if (foodMenuModel.Id > 0)
            {
                var result = _iFoodMenuService.UpdateFoodMenu(foodMenuModel);
                if (result == -1)
                {
                    ModelState.AddModelError("FoodMenuName", "Menu Item already exists");

                    foodMenuModel.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
                    foodMenuModel.FoodVatTaxList = _iDropDownService.GetTaxList();
                    foodMenuModel.UnitsList = _iDropDownService.GetUnitList();
                    return View(foodMenuModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iFoodMenuService.InsertFoodMenu(foodMenuModel);
                if (result == -1)
                {
                    ModelState.AddModelError("FoodMenuName", "Menu Item already exists");
                    foodMenuModel.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
                    foodMenuModel.FoodVatTaxList = _iDropDownService.GetTaxList();
                    foodMenuModel.UnitsList = _iDropDownService.GetUnitList();
                    return View(foodMenuModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "Foodmenu");

        }

        public ActionResult Delete(int id)
        {
            if (UserRolePermissionForPage.Delete == true)
            {
                var deletedid = _iFoodMenuService.DeleteFoodMenu(id);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        private string ValidationFoodMenu(FoodMenuModel foodMenuModel)
        {
            string ErrorString = string.Empty;
            //if (string.IsNullOrEmpty(foodMenuModel.FoodMenuName))
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
            //    return ErrorString;
            //}
            //if (string.IsNullOrEmpty(foodMenuModel.Price.ToString()) || foodMenuModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }

        public JsonResult GetFoodMenuCategory()
        {
            FoodMenuModel foodMenuModel = new FoodMenuModel();
            foodMenuModel.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            return Json(new { FoodCategoryList = foodMenuModel.FoodCategoryList });
        }

        [HttpPost]
        public ActionResult AddUnit(IngredientUnitModel ingredientUniModel)
        {
            var result = _iIngredientUnitService.InsertIngredientUnit(ingredientUniModel);
            if (result == -1)
            {
                return Json(new { result = -1 });
            }
            return Json(new { result = true});
        }
    }
}
