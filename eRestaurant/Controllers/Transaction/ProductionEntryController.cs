using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RocketPOS.Interface.Services;
using RocketPOS.Resources;
using RocketPOS.Models;
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
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;
        public ProductionEntryController(IProductionEntryService productionEntryService,
             IDropDownService idropDownService,
             IStringLocalizer<RocketPOSResources> sharedLocalizer,
             LocService locService)
        {
            _iProductionEntryService = productionEntryService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public IActionResult Index()
        {
            List<ProductionEntryViewModel> productionEntryViewModels = new List<ProductionEntryViewModel>();
          //  productionFormulaViewModels = _iProductionFormulaService.GetProductionFormulaList(Convert.ToInt32(foodMenuType));
            return View(productionEntryViewModels);
        }
        public ActionResult ProductionEntry(int? id, int? foodMenuType, string type)
        {
            ProductionEntryModel productionEntryModel = new ProductionEntryModel();
            if (id > 0)
            {
                ViewBag.ActionType = type;
               // productionFormulaModel = _iProductionFormulaService.GetProductionFormulaById(Convert.ToInt32(id));
            }
            else
            {
                productionEntryModel.FoodmenuType = 2;
            }
            productionEntryModel.ProductionFormulaList = _iDropDownService.GetProductionFormulaList();
            productionEntryModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            productionEntryModel.IngredientList = _iDropDownService.GetIngredientList();
            return View(productionEntryModel);
        }

        public JsonResult GetProductionFormulaById(int id)
        {
            ProductionEntryModel productionEntryModel = new ProductionEntryModel();
            productionEntryModel = _iProductionEntryService.GetProductionFormulaById(id);
            productionEntryModel.ProductionDate = DateTime.Now;
            return Json(new{ productionEntryModel = productionEntryModel });
        }
    }
}
