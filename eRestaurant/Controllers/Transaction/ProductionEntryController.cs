using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RocketPOS.Interface.Services;
using RocketPOS.Resources;
using RocketPOS.Models;
using RocketPOS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Transaction
{
    public class ProductionEntryController : Controller
    {
        private readonly IProductionEntryService _iProductionEntryService;
        private readonly IDropDownService _iDropDownService;
        private readonly ICommonService _iCommonService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;
        public ProductionEntryController(IProductionEntryService productionEntryService,
             IDropDownService idropDownService,
             ICommonService iCommonService,
             IStringLocalizer<RocketPOSResources> sharedLocalizer,
             LocService locService)
        {
            _iProductionEntryService = productionEntryService;
            _iDropDownService = idropDownService;
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public IActionResult Index(int? foodMenuType)
        {
            _iCommonService.GetPageWiseRoleRigths("ProductionEntry");
            if (TempData["foodMenuType"] == null)
            {
                TempData["foodMenuType"] = 2;
            }
            else if (foodMenuType != null)
            {
                TempData["foodMenuType"] = foodMenuType;
            }
            List<ProductionEntryViewModel> productionEntryViewModels = new List<ProductionEntryViewModel>();
            productionEntryViewModels = _iProductionEntryService.GetProductionEntryList(Convert.ToInt32(foodMenuType));
            return View(productionEntryViewModels);
        }

        [HttpGet]
        public JsonResult GetProductionEntryList(int? foodMenuType)
        {
            _iCommonService.GetPageWiseRoleRigths("ProductionEntry");
            TempData["foodMenuType"] = foodMenuType;
            List<ProductionEntryViewModel> productionEntryViewModels = new List<ProductionEntryViewModel>();
            productionEntryViewModels = _iProductionEntryService.GetProductionEntryList(Convert.ToInt32(foodMenuType));
            return Json(new { productionEntryList = productionEntryViewModels });
        }
        public ActionResult ProductionEntry(int? id, int? foodMenuType, int? productionFormulaId, string type)
        {
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true || UserRolePermissionForPage.View == true)
            {
                ProductionEntryModel productionEntryModel = new ProductionEntryModel();
                TempData["foodMenuType"] = foodMenuType;

            if (id > 0)
            {
               ViewBag.ActionType = type;
                productionEntryModel = _iProductionEntryService.GetProductionEntryById(Convert.ToInt32(id));
            }
            else
            {
                if (Convert.ToInt32(productionFormulaId) > 0)
                {
                    productionEntryModel = _iProductionEntryService.GetProductionFormulaById(Convert.ToInt32(productionFormulaId));
                }
                productionEntryModel.ProductionDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                productionEntryModel.FoodmenuType = Convert.ToInt32(foodMenuType);
            }
            productionEntryModel.ProductionFormulaList = _iDropDownService.GetProductionFormulaList(Convert.ToInt32(foodMenuType));
            productionEntryModel.FoodMenuList = _iDropDownService.GetFoodMenuListByFoodmenuType(3);
            productionEntryModel.IngredientList = _iDropDownService.GetIngredientList();
            productionEntryModel.StoreList = _iDropDownService.GetStoreList();
            ViewBag.SelectedStore = productionEntryModel.StoreList.Where(x => x.Selected == true).Select(x => x.Value).SingleOrDefault();

                return View(productionEntryModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public JsonResult GetProductionFormulaById(int id)
        {
            ProductionEntryModel productionEntryModel = new ProductionEntryModel();
            productionEntryModel = _iProductionEntryService.GetProductionFormulaById(id);
            //  productionEntryModel.ProductionDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            return Json(new { productionEntryModel = productionEntryModel });
        }

        public JsonResult GetProductionEntryById(int id)
        {
            ProductionEntryModel productionEntryModel = new ProductionEntryModel();
            productionEntryModel = _iProductionEntryService.GetProductionEntryById(id);
            return Json(new { productionEntryModel = productionEntryModel });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductionEntry(ProductionEntryModel productionEntryModel)
        {
            int result = 0;
            string productionEntryMessage = string.Empty;
            if (productionEntryModel != null)
            {
                if (productionEntryModel.productionEntryFoodMenuModels.Count > 0 && productionEntryModel.productionEntryIngredientModels.Count > 0)
                {
                    if (productionEntryModel.Id > 0)
                    {
                        if (productionEntryModel.Status == 3)
                        {
                            productionEntryModel.ProductionCompletionDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                        }
                        else
                        {
                            productionEntryModel.ProductionCompletionDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                        }
                        result = _iProductionEntryService.UpdateProductionEntry(productionEntryModel);
                        if (result > 0)
                        {
                            productionEntryMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                        }
                    }
                    else
                    {
                        result = _iProductionEntryService.InsertProductionEntry(productionEntryModel);
                        if (result > 0)
                        {
                            productionEntryMessage = _locService.GetLocalizedHtmlString("SaveSuccess");
                        }
                    }
                }
                else
                {
                    productionEntryMessage = _locService.GetLocalizedHtmlString("ValidProductionFormula");
                    return Json(new { error = true, message = productionEntryMessage, status = 201 });
                }
            }
            else
            {
                productionEntryMessage = _locService.GetLocalizedHtmlString("ValidProductionFormula");
                return Json(new { error = true, message = productionEntryMessage, status = 201 });
            }
            return Json(new { error = false, message = productionEntryMessage, status = 200 });
        }

        public ActionResult Delete(int id)
        {
            if (UserRolePermissionForPage.Delete == true)
            {
                int result = _iProductionEntryService.DeleteProductionEntryById(id);
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
    }
}
