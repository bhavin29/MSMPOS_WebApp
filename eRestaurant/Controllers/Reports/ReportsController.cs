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
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public ReportController(IReportService iReportService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iReportService = iReportService;
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
