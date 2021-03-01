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
        public IActionResult Index(int? outletListId, int? foodCategoryId,int? flag)
        {
            FoodMenuRateModel foodMenuRateModel = new FoodMenuRateModel();
            foodMenuRateModel.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            foodMenuRateModel.OutletList = _iDropDownService.GetOutletList();
            if (Convert.ToInt32(flag) > 0)
            {
                foodMenuRateModel.foodMenuRates = _iFoodMenuRateService.GetFoodMenuRateList(Convert.ToInt32(foodCategoryId), Convert.ToInt32(outletListId));
            }
            return View(foodMenuRateModel);
        }


        public ActionResult GetFoodMenuRateList(int foodCategoryId, int outletListId)
        {
            FoodMenuRateModel foodMenuRateModel = new FoodMenuRateModel();
            List<FoodMenuRate> foodMenuRate = new List<FoodMenuRate>();
            foodMenuRateModel.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            foodMenuRateModel.OutletList = _iDropDownService.GetOutletList();

            foodMenuRateModel.foodMenuRates = _iFoodMenuRateService.GetFoodMenuRateList(Convert.ToInt32(foodCategoryId), Convert.ToInt32(outletListId));

            return View(foodMenuRateModel);
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

        public ActionResult FoodMenubyId(int id)
        {
            return RedirectToAction("FoodMenu", "FoodMenu", new { id = id });
        }


    }
}
