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

namespace RocketPOS.Controllers.Transaction
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _iInventoryService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public InventoryController(IInventoryService inventoryService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iInventoryService = inventoryService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }
        public IActionResult Index(int? storeId,int? foodCategoryId)
        {
            InventoryModel inventoryModel = new InventoryModel();
            inventoryModel.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            inventoryModel.StoreList = _iDropDownService.GetStoreList();
            if (Convert.ToInt32(storeId) > 0)
            {
                inventoryModel.InventoryDetailList = _iInventoryService.GetInventoryDetailList(Convert.ToInt32(storeId),Convert.ToInt32(foodCategoryId));
            }
            return View(inventoryModel);
        }

        [HttpPost]
        public JsonResult UpdateInventoryDetailList(string inventoryDetail)
        {
            int result = 0;
            List<InventoryDetail> inventoryDetails = new List<InventoryDetail>();
            inventoryDetails = JsonConvert.DeserializeObject<List<InventoryDetail>>(inventoryDetail);
            result = _iInventoryService.UpdateInventoryDetailList(inventoryDetails);
            return Json(new { result = result });
        }

        [HttpPost]
        public JsonResult SaveInventoryDetailById(string inventoryDetail)
        {
            int result = 0;
            List<InventoryDetail> inventoryDetails = new List<InventoryDetail>();
            inventoryDetails = JsonConvert.DeserializeObject<List<InventoryDetail>>(inventoryDetail);
            result = _iInventoryService.UpdateInventoryDetailList(inventoryDetails);
            return Json(new { result = result });
        }

        [HttpPost]
        public JsonResult StockUpdate(int? storeId, int? foodmenuId)
        {
            string result = "";
            if (Convert.ToInt32(storeId) > 0)
            {
                result = _iInventoryService.StockUpdate(Convert.ToInt32(storeId), Convert.ToInt32(foodmenuId));
            }
            return Json(new { result = result });
        }
    }
}
