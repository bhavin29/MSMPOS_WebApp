using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;

namespace RocketPOS.Controllers.Transaction
{
    public class WasteController : Controller
    {
        private readonly IWasteService _iWasteService;
        private readonly IDropDownService _iDropDownService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;


        public WasteController(IWasteService wasteService,
            IDropDownService idropDownService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iWasteService = wasteService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        // GET: Waste
        public ActionResult WasteList()
        {
            List<WasteListModel> wasteLists = new List<WasteListModel>();
            wasteLists = _iWasteService.GetWasteList().ToList();
            return View(wasteLists);
        }

        // GET: Waste/Details/5
        [HttpGet]
        public ActionResult GetOrderById(long wasteId)
        {
            WasteModel wasteModel = new WasteModel();
            wasteModel = _iWasteService.GetWasteById(wasteId);
            return View(wasteModel);
        }

        // GET: Waste/Create
        public ActionResult Waste(long? id)
        {
            WasteModel wasteModel = new WasteModel();
            if (id > 0)
            {
                long wasteId = Convert.ToInt64(id);
                wasteModel = _iWasteService.GetWasteById(wasteId);

            }
            else
            {
                wasteModel.WasteDateTime = DateTime.Now;
                wasteModel.ReferenceNumber = _iWasteService.ReferenceNumber().ToString();
            }
            wasteModel.OutletList = _iDropDownService.GetOutletList();
            wasteModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            wasteModel.IngredientList = _iDropDownService.GetIngredientList();
            wasteModel.EmployeeList = _iDropDownService.GetEmployeeList();
            return View(wasteModel);
        }

        // POST: Waste/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Waste(WasteModel wasteModel)
        {
            wasteModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            wasteModel.IngredientList = _iDropDownService.GetIngredientList();
            wasteModel.EmployeeList = _iDropDownService.GetEmployeeList();
            wasteModel.OutletList = _iDropDownService.GetOutletList();

            string wasteMessage = string.Empty;
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationWaste(wasteModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    return Json(new { error = true, message = errorString, status = 201 });
                }
            }

            if (wasteModel.WasteDetail != null)
            {
                if (wasteModel.WasteDetail.Count > 0)
                {

                    if (wasteModel.Id > 0)
                    {

                        int result = _iWasteService.UpdateWaste(wasteModel);
                        if (result > 0)
                        {
                            wasteMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                        }
                    }
                    else
                    {
                        int result = _iWasteService.InsertWaste(wasteModel);
                        if (result > 0)
                        {
                            wasteMessage = _locService.GetLocalizedHtmlString("SaveSuccess") + " Reference No is: " + result.ToString();
                        }
                    }
                }
                else
                {
                    wasteMessage = _locService.GetLocalizedHtmlString("ValidWasteDetails");
                    return Json(new { error = true, message = wasteMessage, status = 201 });
                }
            }
            else
            {
                wasteMessage = _locService.GetLocalizedHtmlString("ValidWasteDetails");
                return Json(new { error = true, message = wasteMessage, status = 201 });
            }
            // return View(wasteModel);
            return Json(new { error = false, message = wasteMessage, status = 200 });
            //return View();
        }

        public ActionResult Delete(int id)
        {
            int result = _iWasteService.DeleteWaste(id);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return RedirectToAction(nameof(WasteList));
        }

        public ActionResult DeleteWasteDetails(long wasteId)
        {
            long result = _iWasteService.DeleteWasteDetails(wasteId);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return Json(new { error = false, message = string.Empty, status = 200 });
        }

        private string ValidationWaste(WasteModel wasteModel)
        {
            string ErrorString = string.Empty;
            //if (string.IsNullOrEmpty(wasteModel.ReferenceNo.ToString()) || wasteModel.ReferenceNo == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidReferenceNo");
            //    return ErrorString;
            //}
            if (string.IsNullOrEmpty(wasteModel.EmployeeId.ToString()) || wasteModel.EmployeeId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSupplier");
                return ErrorString;
            }
            if (wasteModel.WasteDetail == null || wasteModel.WasteDetail.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidWasteDetails");
                return ErrorString;
            }

            return ErrorString;
        }
    }
}