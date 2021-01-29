using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Master
{
    public class FoodMenuRateController : Controller
    {
        private readonly IFoodMenuRateService _iFoodMenuRateService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public FoodMenuRateController(IFoodMenuRateService foodMenuRateService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iFoodMenuRateService = foodMenuRateService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }
        public IActionResult Index(int? foodCategoryId)
        {
            FoodMenuRateModel foodMenuRateModel = new FoodMenuRateModel();
            foodMenuRateModel.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            if (Convert.ToInt32(foodCategoryId) > 0)
            {
                foodMenuRateModel.foodMenuRates= _iFoodMenuRateService.GetFoodMenuRateList(Convert.ToInt32(foodCategoryId));
                
            }
            return View(foodMenuRateModel);
        }
        [HttpGet]
        public IActionResult GetFoodMenuRateList(int foodCategoryId)
        {
            List<FoodMenuRate> foodMenuRate = new List<FoodMenuRate>();
            foodMenuRate = _iFoodMenuRateService.GetFoodMenuRateList(foodCategoryId);
            return Json(new { FoodMenuRate = foodMenuRate });
        }

        [HttpPost]
        public JsonResult UpdateFoodMenuRateList(string foodMenuRate)
        {
            int result = 0;
            List<FoodMenuRate> foodMenuRates = new List<FoodMenuRate>();
            foodMenuRates = JsonConvert.DeserializeObject<List<FoodMenuRate>>(foodMenuRate);
            result = _iFoodMenuRateService.UpdateFoodMenuRateList(foodMenuRates);
            return Json(new { result = result });
        }
    }
}
