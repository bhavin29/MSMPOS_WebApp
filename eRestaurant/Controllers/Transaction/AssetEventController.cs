﻿using Microsoft.AspNetCore.Mvc;
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
    public class AssetEventController : Controller
    {
        private readonly IAssetEventService _iAssetEventService;
        private readonly IDropDownService _iDropDownService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;
        public AssetEventController(IAssetEventService assetEventService,
             IDropDownService idropDownService,
             IStringLocalizer<RocketPOSResources> sharedLocalizer,
             LocService locService)
        {
            _iAssetEventService = assetEventService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public IActionResult Index(bool? isHistory)
        {
            TempData["History"] = Convert.ToBoolean(isHistory);
            List<AssetEventViewModel> asssetEventViewModel = new List<AssetEventViewModel>();
            asssetEventViewModel = _iAssetEventService.GetAssetEventList(Convert.ToBoolean(isHistory)).ToList();
            return View(asssetEventViewModel);
        }

        public IActionResult AssetEvent(int? id, string type)
        {
            AssetEventModel assetEventModel = new AssetEventModel();
            if (id > 0)
            {
                ViewBag.ActionType = type;
                assetEventModel = _iAssetEventService.GetAssetEventById(Convert.ToInt32(id));
            }
            else
            {
                assetEventModel.EventDatetime = DateTime.Now;
                assetEventModel.ReferenceNo = _iAssetEventService.ReferenceNumberAssetEvent().ToString();
            }
            assetEventModel.AssetItemList = _iDropDownService.GetAssetItemList();
            assetEventModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            assetEventModel.IngredientList = _iDropDownService.GetIngredientList();
            return View(assetEventModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssetEvent(AssetEventModel assetEventModel)
        {
            int result = 0;
            string assetEventMessage = string.Empty;
            if (assetEventModel != null)
            {
                if (assetEventModel.assetEventItemModels.Count > 0 && assetEventModel.assetEventFoodmenuModels.Count > 0 && assetEventModel.assetEventIngredientModels.Count > 0)
                {
                    if (assetEventModel.Id > 0)
                    {
                        result = _iAssetEventService.UpdateAssetEvent(assetEventModel);
                        if (result > 0)
                        {
                            assetEventMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                        }
                    }
                    else
                    {
                        result = _iAssetEventService.InsertAssetEvent(assetEventModel);
                        if (result > 0)
                        {
                            assetEventMessage = _locService.GetLocalizedHtmlString("SaveSuccess");
                        }
                    }
                }
                else
                {
                    assetEventMessage = _locService.GetLocalizedHtmlString("ValidProductionFormula");
                    return Json(new { error = true, message = assetEventMessage, status = 201 });
                }
            }
            else
            {
                assetEventMessage = _locService.GetLocalizedHtmlString("ValidProductionFormula");
                return Json(new { error = true, message = assetEventMessage, status = 201 });
            }
            return Json(new { error = false, message = assetEventMessage, status = 200 });
        }

        public ActionResult Delete(int id)
        {
            int result = _iAssetEventService.DeleteAssetEven(id);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public JsonResult GetAssetItemPriceById(int id)
        {
            decimal assetItemCostPrice = 0;
            assetItemCostPrice = _iAssetEventService.GetAssetItemPriceById(id);
            return Json(new { assetItemCostPrice = assetItemCostPrice });
        }

        [HttpGet]
        public JsonResult GetIngredientPriceById(int id)
        {
            decimal ingredientPrice = 0;
            ingredientPrice = _iAssetEventService.GetIngredientPriceById(id);
            return Json(new { ingredientPrice = ingredientPrice });
        }

        [HttpGet]
        public JsonResult GetFoodMenuPriceTaxDetailById(int id)
        {
            AssetFoodMenuPriceDetail assetFoodMenuPriceDetail = new AssetFoodMenuPriceDetail();
            assetFoodMenuPriceDetail = _iAssetEventService.GetFoodMenuPriceTaxDetailById(id);
            return Json(new { salesPrice = assetFoodMenuPriceDetail.SalesPrice, taxPercentage = assetFoodMenuPriceDetail.TaxPercentage });
        }
    }
}
