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
    public class InventoryAdjustmentController : Controller
    {

        private readonly IInventoryAdjustmentService _inventoryAdjustmentService;
        private readonly IDropDownService _iDropDownService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;


        public InventoryAdjustmentController(IInventoryAdjustmentService inventoryAdjustmentService,
            IDropDownService idropDownService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _inventoryAdjustmentService = inventoryAdjustmentService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult InventoryAdjustmentList()
        {
            List<InventoryAdjustmentViewModel> inventoyAdjustmentViewModels = new List<InventoryAdjustmentViewModel>();
            inventoyAdjustmentViewModels = _inventoryAdjustmentService.GetInventoryAdjustmentList().ToList();
            return View(inventoyAdjustmentViewModels);
        }

        [HttpGet]
        public JsonResult InventoryAdjustmentListByDate(string fromDate, string toDate)
        {
            List<InventoryAdjustmentViewModel> inventoyAdjustmentViewModels = new List<InventoryAdjustmentViewModel>();
            DateTime newFromDate, newToDate;
            if (fromDate != null)
            {
                newFromDate = fromDate == "01/01/0001 00:00:00" ? DateTime.Now : Convert.ToDateTime(fromDate);
                newToDate = toDate == "01/01/0001 00:00:00" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newFromDate = DateTime.Now;
                newToDate = DateTime.Now;
            }
            inventoyAdjustmentViewModels = _inventoryAdjustmentService.InventoryAdjustmentListByDate(newFromDate, newToDate).ToList();
            return Json(new { InventoryAdjustment = inventoyAdjustmentViewModels });
        }

        [HttpGet]
        public ActionResult GetOrderById(long Id)
        {
            InventoryAdjustmentModel inventoryAdjustmentModel = new InventoryAdjustmentModel();
            inventoryAdjustmentModel = _inventoryAdjustmentService.GetInventoryAdjustmentById(Id);
            return View(inventoryAdjustmentModel);
        }

        public ActionResult InventoryAdjustment(long? id, int? inventoryType)
        {
            InventoryAdjustmentModel inventoryAdjustmentModel = new InventoryAdjustmentModel();
            if (id > 0)
            {
                long purchaseId = Convert.ToInt64(id);
                inventoryAdjustmentModel = _inventoryAdjustmentService.GetInventoryAdjustmentById(purchaseId);

            }
            else
            {
                inventoryAdjustmentModel.Date = DateTime.Now;
                inventoryAdjustmentModel.ReferenceNo = _inventoryAdjustmentService.ReferenceNumber().ToString();
                inventoryAdjustmentModel.InventoryType = Convert.ToInt32(inventoryType);
            }
            inventoryAdjustmentModel.StoreList = _iDropDownService.GetSupplierList();
            inventoryAdjustmentModel.EmployeeList = _iDropDownService.GetEmployeeList();
            inventoryAdjustmentModel.IngredientList = _iDropDownService.GetIngredientList();
            inventoryAdjustmentModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            return View(inventoryAdjustmentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InventoryAdjustment(InventoryAdjustmentModel inventoryAdjustmentModel)
        {
            inventoryAdjustmentModel.StoreList = _iDropDownService.GetStoreList();
            inventoryAdjustmentModel.EmployeeList = _iDropDownService.GetEmployeeList();
            inventoryAdjustmentModel.IngredientList = _iDropDownService.GetIngredientList();
            inventoryAdjustmentModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            string purchaseMessage = string.Empty;

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationInveotryAdjustment(inventoryAdjustmentModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    return Json(new { error = true, message = errorString, status = 201 });
                }
            }

            if (inventoryAdjustmentModel.InventoryAdjustmentDetail != null)
            {
                if (inventoryAdjustmentModel.InventoryAdjustmentDetail.Count > 0)
                {

                    if (inventoryAdjustmentModel.Id > 0)
                    {

                        int result = _inventoryAdjustmentService.UpdateInventoryAdjustment(inventoryAdjustmentModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                        }
                    }
                    else
                    {
                        int result = _inventoryAdjustmentService.InsertInventoryAdjustment(inventoryAdjustmentModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("SaveSuccess") + " Reference No is: " + result.ToString();
                        }
                    }
                }
                else
                {
                    purchaseMessage = _locService.GetLocalizedHtmlString("ValidPurchaseDetails");
                    return Json(new { error = true, message = purchaseMessage, status = 201 });
                }
            }
            else
            {
                purchaseMessage = _locService.GetLocalizedHtmlString("ValidPurchaseDetails");
                return Json(new { error = true, message = purchaseMessage, status = 201 });
            }
            // return View(purchaseModel);
            return Json(new { error = false, message = purchaseMessage, status = 200 });
            //return View();
        }

        public ActionResult Delete(int id)
        {
            int result = _inventoryAdjustmentService.DeleteInventoryAdjustment(id);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return RedirectToAction(nameof(InventoryAdjustmentList));
        }

        public ActionResult DeleteInventoryAdjustmentDetails(long id)
        {
            long result = _inventoryAdjustmentService.DeleteInventoryAdjustmentDetail(id);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return Json(new { error = false, message = string.Empty, status = 200 });
        }

        private string ValidationInveotryAdjustment(InventoryAdjustmentModel inventoryAdjustmentModel)
        {
            string ErrorString = string.Empty;

            if (string.IsNullOrEmpty(inventoryAdjustmentModel.StoreId.ToString()) || inventoryAdjustmentModel.StoreId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidStoreName");
                return ErrorString;
            }
            if (string.IsNullOrEmpty(inventoryAdjustmentModel.EmployeeId.ToString()) || inventoryAdjustmentModel.EmployeeId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSupplier");
                return ErrorString;
            }
            if (inventoryAdjustmentModel.InventoryAdjustmentDetail == null || inventoryAdjustmentModel.InventoryAdjustmentDetail.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidPurchaseDetails");
                return ErrorString;
            }

            return ErrorString;
        }


    }
}
