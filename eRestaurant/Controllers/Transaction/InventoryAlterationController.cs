﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using RocketPOS.Framework;
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
        private readonly ICommonService _iCommonService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;


        public InventoryAlterationController(IInventoryAlterationService inventoryAlterationService,
            IDropDownService idropDownService,
             ICommonService iCommonService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _inventoryAlterationService = inventoryAlterationService;
            _iDropDownService = idropDownService;
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public IActionResult Index(int? storeId, DateTime? fromDate, DateTime? toDate, int? foodMenuId)
        {
            _iCommonService.GetPageWiseRoleRigths("InventoryAlteration");
            InventoryAlterationViewModel inventoryAlterationView = new InventoryAlterationViewModel();

            if (storeId!=null)
            {
                inventoryAlterationView.InventoryAlterationViewList = _inventoryAlterationService.GetInventoryAlterationList(Convert.ToInt32(storeId), Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), Convert.ToInt32(foodMenuId));
            }
            inventoryAlterationView.FromDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            inventoryAlterationView.ToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            inventoryAlterationView.StoreList = _iDropDownService.GetStoreList();
            inventoryAlterationView.FoodMenuList = _iDropDownService.GetFoodMenuList();
            return View(inventoryAlterationView);
        }


        public ActionResult InventoryAlteration(int? id, int? inventoryType)
        {
            _iCommonService.GetPageWiseRoleRigths("InventoryAlteration");
            InventoryAlterationModel inventoryAlterationModel = new InventoryAlterationModel();
            inventoryAlterationModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            inventoryAlterationModel.IngredientList = _iDropDownService.GetIngredientList();
            inventoryAlterationModel.AssetItemList = _iDropDownService.GetAssetItemList();
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

        [HttpGet]
        public JsonResult GetInventoryStockQtyForIngredient(int storeId, int ingredientId)
        {
            decimal stockQty = 0;
            stockQty = _inventoryAlterationService.GetInventoryStockQtyForIngredient(storeId, ingredientId);
            return Json(new { stockQty = stockQty });
        }

        public ActionResult View(long? id)
        {
            InventoryAlterationModel inventoryAlterationModel = new InventoryAlterationModel();
            long purchaseId = Convert.ToInt64(id);
            inventoryAlterationModel = _inventoryAlterationService.GetViewInventoryAlterationById(purchaseId);
            return View(inventoryAlterationModel);
        }
    }
}
