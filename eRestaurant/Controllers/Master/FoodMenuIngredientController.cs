using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Master
{
    public class FoodMenuIngredientController : Controller
    {
        private readonly IFoodMenuIngredientService _iFoodMenuIngredientService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public FoodMenuIngredientController(IFoodMenuIngredientService foodMenuIngredientService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iFoodMenuIngredientService = foodMenuIngredientService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }
        public IActionResult Index(int? foodMenuId)
        {
            FoodMenuIngredientModel foodMenuIngredientModel = new FoodMenuIngredientModel();
            if (Convert.ToInt32(foodMenuId) > 0)
            {
                foodMenuIngredientModel = _iFoodMenuIngredientService.GetFoodMenuIngredientList(Convert.ToInt32(foodMenuId));
            }
            foodMenuIngredientModel.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            foodMenuIngredientModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            foodMenuIngredientModel.IngredientList = _iDropDownService.GetIngredientList();
            return View(foodMenuIngredientModel);
        }

        public JsonResult GetFoodMenuByCategory(int categoryId)
        {
            FoodMenuIngredientModel foodMenuIngredientModel = new FoodMenuIngredientModel();
            foodMenuIngredientModel.FoodMenuList = _iDropDownService.GetFoodMenuListByCategory(categoryId);
            return Json(new { foodMenuIngredientModel.FoodMenuList });
        }

        public JsonResult GetUnitNameByIngredientId(int ingredientId)
        {
            string unitName = string.Empty;
            unitName = _iFoodMenuIngredientService.GetUnitNameByIngredientId(ingredientId);
            return Json(new { UnitName = unitName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FoodMenuIngredient(FoodMenuIngredientModel foodMenuIngredientModel)
        {
            int result = _iFoodMenuIngredientService.InsertUpdateFoodMenuIngredient(foodMenuIngredientModel);
            if (result > 0)
            {
                return Json(new { error = false, message = "Records Saved Successfully.", status = 200 });
            }
            else
            {
                return Json(new { error = true, message = "Records Saved Failed.", status = 201 });
            }
        }
    }
}
