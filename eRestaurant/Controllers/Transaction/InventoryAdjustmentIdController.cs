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
using RocketPOS.Framework;

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

        public ActionResult InventoryAdjustmentList(int consumptionStatus)
        {
            ViewBag.ConsumptionStatus = consumptionStatus;
            List<InventoryAdjustmentViewModel> inventoyAdjustmentViewModels = new List<InventoryAdjustmentViewModel>();
            inventoyAdjustmentViewModels = _inventoryAdjustmentService.GetInventoryAdjustmentList(consumptionStatus).ToList();
            return View(inventoyAdjustmentViewModels);
        }

        [HttpGet]
        public JsonResult InventoryAdjustmentListByDate(string fromDate, string toDate, int consumptionStatus)
        {
            List<InventoryAdjustmentViewModel> inventoyAdjustmentViewModels = new List<InventoryAdjustmentViewModel>();
            DateTime newFromDate, newToDate;
            if (fromDate != null)
            {
                newFromDate = fromDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(fromDate);
                newToDate = toDate == "01/01/0001" ? DateTime.Now : Convert.ToDateTime(toDate);
            }
            else
            {
                newFromDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                newToDate = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
            }

            inventoyAdjustmentViewModels = _inventoryAdjustmentService.InventoryAdjustmentListByDate(newFromDate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy"), consumptionStatus).ToList();
            return Json(new { InventoryAdjustment = inventoyAdjustmentViewModels });
        }

        [HttpGet]
        public ActionResult GetOrderById(long Id)
        {
            InventoryAdjustmentModel inventoryAdjustmentModel = new InventoryAdjustmentModel();
            inventoryAdjustmentModel = _inventoryAdjustmentService.GetInventoryAdjustmentById(Id);
            return View(inventoryAdjustmentModel);
        }

        public ActionResult InventoryAdjustment(long? id, int? inventoryType, int? consumptionStatus, string type)
        {
            InventoryAdjustmentModel inventoryAdjustmentModel = new InventoryAdjustmentModel();
            if (id > 0)
            {
                ViewBag.ActionType = type;
                long purchaseId = Convert.ToInt64(id);
                inventoryAdjustmentModel = _inventoryAdjustmentService.GetInventoryAdjustmentById(purchaseId);

            }
            else
            {
                inventoryAdjustmentModel.Date = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                inventoryAdjustmentModel.ReferenceNo = _inventoryAdjustmentService.ReferenceNumber().ToString();
                inventoryAdjustmentModel.InventoryType = Convert.ToInt32(inventoryType);
                inventoryAdjustmentModel.ConsumptionStatus = Convert.ToInt32(consumptionStatus);
            }
            inventoryAdjustmentModel.StoreList = _iDropDownService.GetStoreList();
            ViewBag.SelectedStore = inventoryAdjustmentModel.StoreList.Where(x => x.Selected == true).Select(x => x.Value).SingleOrDefault();
            inventoryAdjustmentModel.EmployeeList = _iDropDownService.GetEmployeeList();
            inventoryAdjustmentModel.IngredientList = _iDropDownService.GetIngredientList();
            inventoryAdjustmentModel.AssetItemList = _iDropDownService.GetAssetItemList();
            inventoryAdjustmentModel.FoodMenuList = _iDropDownService.GetFoodMenuListByFoodmenuType(-1);
            return View(inventoryAdjustmentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InventoryAdjustment(InventoryAdjustmentModel inventoryAdjustmentModel)
        {
            inventoryAdjustmentModel.StoreList = _iDropDownService.GetStoreList();
            inventoryAdjustmentModel.EmployeeList = _iDropDownService.GetEmployeeList();
            inventoryAdjustmentModel.IngredientList = _iDropDownService.GetIngredientList();
            inventoryAdjustmentModel.FoodMenuList = _iDropDownService.GetFoodMenuListByFoodmenuType(-1);
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
                        //inventoryAdjustmentModel.Date = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset);
                        int result = _inventoryAdjustmentService.InsertInventoryAdjustment(inventoryAdjustmentModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("SaveSuccess");// + " Number is: " + result.ToString();
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
            if (inventoryAdjustmentModel.InventoryAdjustmentDetail == null || inventoryAdjustmentModel.InventoryAdjustmentDetail.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidPurchaseDetails");
                return ErrorString;
            }

            return ErrorString;
        }

        public JsonResult GetFoodMenuPurchasePrice(int foodMenuId)
        {
            decimal purchasePrice = 0;
            purchasePrice = _inventoryAdjustmentService.GetFoodMenuPurchasePrice(foodMenuId);
            return Json(new { purchasePrice = purchasePrice });
        }

        public ActionResult GetFoodMenuList()
        {
            PurchaseModel purchaseModel = new PurchaseModel();
            purchaseModel.FoodMenuList = _iDropDownService.GetFoodMenuListByFoodmenuType(-1);
            return Json(new { purchaseModel.FoodMenuList });
        }
        public ActionResult View(long? id)
        {
            InventoryAdjustmentModel inventoryAdjustmentModel = new InventoryAdjustmentModel();
            long purchaseId = Convert.ToInt64(id);
            inventoryAdjustmentModel = _inventoryAdjustmentService.GetViewInventoryAdjustmentById(purchaseId);
            return View(inventoryAdjustmentModel);
        }
    }
}
