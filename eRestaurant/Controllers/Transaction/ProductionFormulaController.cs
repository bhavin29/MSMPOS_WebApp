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
    public class ProductionFormulaController : Controller
    {
        private readonly IProductionFormulaService _iProductionFormulaService;
        private readonly IDropDownService _iDropDownService;
        private readonly ICommonService _iCommonService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public ProductionFormulaController(IProductionFormulaService productionFormulaService,
            IDropDownService idropDownService,
            ICommonService iCommonService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer,
            LocService locService)
        {
            _iProductionFormulaService = productionFormulaService;
            _iDropDownService = idropDownService;
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }
        public IActionResult Index(int? foodMenuType)
        {
            _iCommonService.GetPageWiseRoleRigths("ProductionFormula");
            if (TempData["foodMenuType"] == null)
            {
                TempData["foodMenuType"] = 2;
                foodMenuType = 2;
            }
            else if (foodMenuType != null)
            {
                TempData["foodMenuType"] = foodMenuType;
            }
            else if (foodMenuType == null)
            {
                TempData["foodMenuType"] = 2;
                foodMenuType = 2;
            }
            List<ProductionFormulaViewModel> productionFormulaViewModels = new List<ProductionFormulaViewModel>();
            productionFormulaViewModels = _iProductionFormulaService.GetProductionFormulaList(Convert.ToInt32(foodMenuType));
            return View(productionFormulaViewModels);
        }

        [HttpGet]
        public JsonResult GetProductionFormulaList(int? foodMenuType)
        {
            _iCommonService.GetPageWiseRoleRigths("ProductionFormula");
            TempData["foodMenuType"] = foodMenuType;
            List<ProductionFormulaViewModel> productionFormulaViewModels = new List<ProductionFormulaViewModel>();
            productionFormulaViewModels = _iProductionFormulaService.GetProductionFormulaList(Convert.ToInt32(foodMenuType));
            return Json(new { productionFormulaList = productionFormulaViewModels });
        }

        public ActionResult ProductionFormula(int? id, int? foodMenuType, string type)
        {
            ProductionFormulaModel productionFormulaModel = new ProductionFormulaModel();
            TempData["foodMenuType"] = foodMenuType;
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true || UserRolePermissionForPage.View == true)
            {
                if (id > 0)
                {
                    ViewBag.ActionType = type;
                    productionFormulaModel = _iProductionFormulaService.GetProductionFormulaById(Convert.ToInt32(id));
                }
                else
                {
                    productionFormulaModel.FoodmenuType = (int)foodMenuType;
                    productionFormulaModel.IsActive = true;
                }
                productionFormulaModel.FoodMenuList = _iDropDownService.GetFoodMenuListByFoodmenuType((int)foodMenuType);
                productionFormulaModel.IngredientList = _iDropDownService.GetIngredientList();
                productionFormulaModel.BatchSizeUnitsList = _iDropDownService.GetUnitList();

                return View(productionFormulaModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public JsonResult GetUnitNameByFoodMenuId(int foodMenuId)
        {
            UnitModel unitModel = new UnitModel();
            string unitName = string.Empty;
            if (foodMenuId > 0)
            {
                unitModel = _iProductionFormulaService.GetUnitNameByFoodMenuId(foodMenuId);
            }
            return Json(new { UnitName = unitModel.UnitName, Id = unitModel.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductionFormula(ProductionFormulaModel productionFormulaModel)
        {
            int result = 0;
            string productionFormulaMessage = string.Empty;
            if (productionFormulaModel != null)
            {
                if (productionFormulaModel.productionFormulaFoodMenuModels.Count > 0 && productionFormulaModel.productionFormulaIngredientModels.Count > 0)
                {
                    if (productionFormulaModel.Id > 0)
                    {
                        result = _iProductionFormulaService.UpdateProductionFormula(productionFormulaModel);
                        if (result > 0)
                        {
                            productionFormulaMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                        }
                    }
                    else
                    {
                        result = _iProductionFormulaService.InsertProductionFormula(productionFormulaModel);
                        if (result > 0)
                        {
                            productionFormulaMessage = _locService.GetLocalizedHtmlString("SaveSuccess");
                        }
                        else if (result == -1)
                        {
                            productionFormulaMessage = _locService.GetLocalizedHtmlString("Recipe formula already created for this Menu item ");
                            return Json(new { error = true, message = productionFormulaMessage, status = 201 });
                        }
                    }
                }
                else
                {
                    productionFormulaMessage = _locService.GetLocalizedHtmlString("ValidProductionFormula");
                    return Json(new { error = true, message = productionFormulaMessage, status = 201 });
                }
            }
            else
            {
                productionFormulaMessage = _locService.GetLocalizedHtmlString("ValidProductionFormula");
                return Json(new { error = true, message = productionFormulaMessage, status = 201 });
            }
            return Json(new { error = false, message = productionFormulaMessage, status = 200 });
        }

        public ActionResult Delete(int id)
        {
            if (UserRolePermissionForPage.Delete == true)
            {
                int result = _iProductionFormulaService.DeleteProductionFormulaById(id);
                if (result > 0)
                {
                    ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public ActionResult View(long? id)
        {
            ProductionFormulaModel productionFormulaModel = new ProductionFormulaModel();
            if (UserRolePermissionForPage.View == true)
            {
                productionFormulaModel = _iProductionFormulaService.GetProductionFormulaViewById(Convert.ToInt32(id));
                return View(productionFormulaModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }
    }
}
