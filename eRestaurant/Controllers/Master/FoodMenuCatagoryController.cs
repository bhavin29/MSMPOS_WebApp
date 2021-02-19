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
    public class FoodMenuCategoryController : Controller
    {
        private readonly IFoodMenuCatagoryService _ifoodMenuCatagoryService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public FoodMenuCategoryController(IFoodMenuCatagoryService ifoodMenuCatagoryService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _ifoodMenuCatagoryService = ifoodMenuCatagoryService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            List<FoodMenuCatagoryModel> foodMenuCategoryModel = new List<FoodMenuCatagoryModel>();
            foodMenuCategoryModel = _ifoodMenuCatagoryService.GetFoodCategoryList().ToList();
            return View(foodMenuCategoryModel);
         }

        public ActionResult FoodMenuCategory(int? id)
        {
            FoodMenuCatagoryModel foodMenuCategoryModel = new FoodMenuCatagoryModel();
            if (id > 0)
            {
                int foodMenuiCateGoryId = Convert.ToInt32(id);
                foodMenuCategoryModel = _ifoodMenuCatagoryService.GetFoodCategoryById(foodMenuiCateGoryId);
            }

            return View(foodMenuCategoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FoodMenuCategory(FoodMenuCatagoryModel foodMenuCategoryModel, string submitButton)
        {

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationFoodMenuCategory(foodMenuCategoryModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(foodMenuCategoryModel);
                }
            }

            if (foodMenuCategoryModel.Id > 0)
            {
                var result = _ifoodMenuCatagoryService.UpdateFoodMenuCatagory(foodMenuCategoryModel);
                if (result == -1)
                {
                    ModelState.AddModelError("FoodMenuCategoryName", "Category already exists");
                    return View(foodMenuCategoryModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _ifoodMenuCatagoryService.InsertFoodMenuCatagory(foodMenuCategoryModel);
                if (result == -1)
                {
                    ModelState.AddModelError("FoodMenuCategoryName", "Category already exists");
                    return View(foodMenuCategoryModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "FoodMenuCategory");
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _ifoodMenuCatagoryService.DeleteFoodMenuCatagory(id);

            return RedirectToAction(nameof(Index));
        }

        private string ValidationFoodMenuCategory(FoodMenuCatagoryModel foodMenuCategoryModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(foodMenuCategoryModel.FoodMenuCategoryName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(foodMenuCategoryModel.Price.ToString()) || foodMenuCategoryModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }

    }
}
