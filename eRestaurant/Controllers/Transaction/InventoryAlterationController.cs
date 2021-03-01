using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Transaction
{
    public class InventoryAlterationController : Controller
    {
        private readonly IInventoryAlterationService _inventoryAlterationService;
        private readonly IDropDownService _iDropDownService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;


        public InventoryAlterationController(IInventoryAlterationService inventoryAlterationService,
            IDropDownService idropDownService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _inventoryAlterationService = inventoryAlterationService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public IActionResult Index(int? storeId, DateTime? fromDate, DateTime? toDate, int? foodMenuId)
        {
            InventoryAlterationViewModel inventoryAlterationView = new InventoryAlterationViewModel();

            if (storeId!=null)
            {
                inventoryAlterationView.InventoryAlterationViewList = _inventoryAlterationService.GetInventoryAlterationList(Convert.ToInt32(storeId), Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), Convert.ToInt32(foodMenuId));
            }
            inventoryAlterationView.FromDate = DateTime.Now;
            inventoryAlterationView.ToDate = DateTime.Now;
            inventoryAlterationView.StoreList = _iDropDownService.GetStoreList();
            inventoryAlterationView.FoodMenuList = _iDropDownService.GetFoodMenuList();
            return View(inventoryAlterationView);
        }


        public ActionResult InventoryAlteration(int? id, int? inventoryType)
        {
            InventoryAlterationModel inventoryAlterationModel = new InventoryAlterationModel();
            inventoryAlterationModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            inventoryAlterationModel.IngredientList = _iDropDownService.GetIngredientList();
            inventoryAlterationModel.StoreList = _iDropDownService.GetStoreList();
            inventoryAlterationModel.ReferenceNo = _inventoryAlterationService.ReferenceNumberInventoryAlteration();
            inventoryAlterationModel.InventoryType = Convert.ToInt32(inventoryType);
            return View(inventoryAlterationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InventoryAlteration(InventoryAlterationModel inventoryAlterationModel)
        {
            int result = 0;
            string InventoryAlterationMessage = string.Empty;
            if (inventoryAlterationModel != null)
            {
                if (inventoryAlterationModel.InventoryAlterationDetails.Count > 0)
                {

                    result = _inventoryAlterationService.InsertInventoryAlteration(inventoryAlterationModel);
                    if (result > 0)
                    {
                        InventoryAlterationMessage = _locService.GetLocalizedHtmlString("SaveSuccess");
                    }
                    else
                    {
                        InventoryAlterationMessage = _locService.GetLocalizedHtmlString("SaveFailed");
                    }
                }
                else
                {
                    InventoryAlterationMessage = _locService.GetLocalizedHtmlString("ValidInventoryAlteration");
                    return Json(new { error = true, message = InventoryAlterationMessage, status = 201 });
                }
            }
            else
            {
                InventoryAlterationMessage = _locService.GetLocalizedHtmlString("ValidInventoryAlteration");
                return Json(new { error = true, message = InventoryAlterationMessage, status = 201 });
            }
            return Json(new { error = false, message = InventoryAlterationMessage, status = 200 });
        }

        [HttpGet]
        public JsonResult GetInventoryStockQty(int storeId,int foodMenuId)
        {
            decimal stockQty = 0;
            stockQty = _inventoryAlterationService.GetInventoryStockQty(storeId, foodMenuId);
            return Json(new { stockQty = stockQty });
        }
    }
}
