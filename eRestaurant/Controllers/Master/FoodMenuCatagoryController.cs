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

        public FoodMenuCategoryController(IFoodMenuCatagoryService ifoodMenuCatagoryService, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _ifoodMenuCatagoryService = ifoodMenuCatagoryService;
            _sharedLocalizer = sharedLocalizer;
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
            if (foodMenuCategoryModel.Id > 0)
            {
                var result = _ifoodMenuCatagoryService.UpdateFoodMenuCatagory(foodMenuCategoryModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _ifoodMenuCatagoryService.InsertFoodMenuCatagory(foodMenuCategoryModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _ifoodMenuCatagoryService.DeleteFoodMenuCatagory(id);

            return RedirectToAction(nameof(Index));
        }


    }
}
