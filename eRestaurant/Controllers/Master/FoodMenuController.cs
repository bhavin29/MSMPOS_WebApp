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
    public class FoodMenuController : Controller
    {
        private readonly IFoodMenuService _iFoodMenuService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public FoodMenuController(IFoodMenuService foodMenuService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iFoodMenuService = foodMenuService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            List<FoodMenuModel> foodMenuModel = new List<FoodMenuModel>();
            foodMenuModel = _iFoodMenuService.GetFoodMenuList().ToList();
            return View(foodMenuModel);
        }

        public ActionResult FoodMenu(int? id)
        {
            FoodMenuModel foodMenuModel = new FoodMenuModel();
            if (id > 0)
            {
                int foodMenuId = Convert.ToInt32(id);
                foodMenuModel = _iFoodMenuService.GetFoodMenueById(foodMenuId);
            }
            foodMenuModel.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            //foodMenuModel.IngredientList = _iDropDownService.GetIngredientList();

            return View(foodMenuModel);
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
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iFoodMenuService.InsertFoodMenu(foodMenuModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "Foodmenu");

        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iFoodMenuService.DeleteFoodMenu(id);

            return RedirectToAction(nameof(Index));
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

    }
}
