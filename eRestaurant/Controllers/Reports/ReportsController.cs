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
using Newtonsoft.Json;

namespace RocketPOS.Controllers.Reports
{
    public class ReportController : Controller
    {

        private readonly IReportService _iReportService;
        private readonly IDropDownService _iDropDownService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public ReportController(IReportService iReportService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
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

        public ViewResult Purchase()
        {
            PurchaseReportParamModel purchaseReportModel = new PurchaseReportParamModel();
            purchaseReportModel.FromDate = DateTime.Now;
            purchaseReportModel.ToDate = DateTime.Now;
            //purchaseReportModel = _iReportService.GetPurchaseReport(purchaseReportParamModel);
            return View(purchaseReportModel);
        }

        [HttpGet]
        public ActionResult PurchaseReport(string fromDate, string toDate)
        {
            List<PurchaseReportModel> purchaseReportList = new List<PurchaseReportModel>();
            PurchaseReportParamModel purchaseReportModel = new PurchaseReportParamModel();
            purchaseReportModel.draw = int.Parse(HttpContext.Request.Query["draw"]);
            if (fromDate != null)
            {
                DateTime newFromDate = fromDate == "01/01/0001 00:00:00" ? DateTime.Now : Convert.ToDateTime(fromDate);
                DateTime newToDate = toDate == "01/01/0001 00:00:00" ? DateTime.Now : Convert.ToDateTime(toDate);
                purchaseReportModel.FromDate = newFromDate;
                purchaseReportModel.ToDate = newToDate;
                purchaseReportList = _iReportService.GetPurchaseReport(newFromDate, newToDate);
                purchaseReportModel.data = purchaseReportList.ToArray();
            }
            else
            {
                purchaseReportModel.FromDate = DateTime.Now;
                purchaseReportModel.ToDate = DateTime.Now;
            }

            //string jsonData = JsonConvert.SerializeObject(purchaseReportModel.PurchaseReport);
            var jsonData = purchaseReportList.ToArray();
            //return Json(purchaseReportModel, json);
            return Json(new { draw = purchaseReportModel.draw, recordsFiltered = purchaseReportList.Count, recordsTotal = purchaseReportList.Count, data = jsonData });
        }

        public ViewResult OutletRegister()
        {
            List<OutletRegisterReportModel> outletRegisterReportModel = new List<OutletRegisterReportModel>();
            outletRegisterReportModel= _iReportService.GetOutletRegisterReport(21);
            return View(outletRegisterReportModel);
        }

        [HttpGet]
        public ActionResult OutletRegisterReport(int outletRegisterId)
        {
            List<OutletRegisterReportModel> outletRegisterModels = new List<OutletRegisterReportModel>();

         //   OutletRegisterReportModel outletRegisterModel = new OutletRegisterReportModel();

            outletRegisterModels = _iReportService.GetOutletRegisterReport(outletRegisterId);
            return View(outletRegisterModels);

            //string jsonData = JsonConvert.SerializeObject(purchaseReportModel.PurchaseReport);
            // var jsonData = outletRegisterModels.ToArray();
            //return Json(purchaseReportModel, json);
            //  return Json(new { draw = purchaseReportModel.draw, recordsFiltered = purchaseReportList.Count, recordsTotal = purchaseReportList.Count, data = jsonData });
        }
    }
}
