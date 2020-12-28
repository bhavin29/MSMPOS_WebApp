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
using RocketPOS.Interface.Services.Reports;
using RocketPOS.Models.Reports;

namespace RocketPOS.Controllers.Reports
{
    public class ReportController : Controller
    {

        private readonly IReportService _iReportService;
        private readonly IDropDownService _iDropDownService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public ReportController(IReportService iReportService, IDropDownService idropDownService,IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iReportService = iReportService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public IActionResult Index()
        {

            return View();
        }

        public ViewResult Inventory()
        {

            List<InventoryReportModel> inventoryReportModel = new List<InventoryReportModel>();
            InventoryReportParamModel inventoryReportParamModel = new InventoryReportParamModel();

            //inventoryReportParamModel.IngredientCategoryList = _iDropDownService.GetIngredientCategoryList();
            //inventoryReportParamModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            //inventoryReportParamModel.IngredientList = _iDropDownService.GetIngredientList();


            inventoryReportModel = _iReportService.GetInventoryReport(inventoryReportParamModel);
            return View(inventoryReportModel);
        }

        [HttpPost]
        public ViewResult Inventory(InventoryReportParamModel inventoryReportParamModel)
        {

            List<InventoryReportModel> inventoryReportModel = new List<InventoryReportModel>();
  
            inventoryReportModel = _iReportService.GetInventoryReport(inventoryReportParamModel);
            return View(inventoryReportModel);
        }


    }
}
