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
    public class InventoryTransferController : Controller
    {

        private readonly IInventoryTransferService _inventoryTransferService;
        private readonly IDropDownService _iDropDownService;
        private readonly IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;


        public InventoryTransferController(IInventoryTransferService inventoryTransferService,
            IDropDownService idropDownService,
            IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _inventoryTransferService = inventoryTransferService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult InventoryTransferList()
        {
            List<InventoryTransferViewModel> inventoyTransferViewModels = new List<InventoryTransferViewModel>();
            inventoyTransferViewModels = _inventoryTransferService.GetInventoryTransferList().ToList();
            return View(inventoyTransferViewModels);
        }

        public JsonResult InventoryTransferListByDate(string fromDate, string toDate)
        {
            List<InventoryTransferViewModel> inventoyTransferViewModels = new List<InventoryTransferViewModel>();
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
            inventoyTransferViewModels = _inventoryTransferService.GetInventoryTransferListByDate(newFromDate.ToString("dd/MM/yyyy"), newToDate.ToString("dd/MM/yyyy")).ToList();
            return Json(new { InventoryTransfer = inventoyTransferViewModels });
        }

        [HttpGet]
        public ActionResult GetOrderById(long Id)
        {
            InventoryTransferModel inventoryTransferModel = new InventoryTransferModel();
            inventoryTransferModel = _inventoryTransferService.GetInventoryTransferById(Id);
            return View(inventoryTransferModel);
        }

        public ActionResult InventoryTransfer(long? id, int? inventoryType, string type)
        {
            InventoryTransferModel inventoryTransferModel = new InventoryTransferModel();
            if (id > 0)
            {
                ViewBag.ActionType = type;
                long purchaseId = Convert.ToInt64(id);
                inventoryTransferModel = _inventoryTransferService.GetInventoryTransferById(purchaseId);

            }
            else
            {
                inventoryTransferModel.Date = DateTime.Now;
                inventoryTransferModel.ReferenceNo = _inventoryTransferService.ReferenceNumber().ToString();
                inventoryTransferModel.InventoryType = Convert.ToInt32(inventoryType);
            }
            inventoryTransferModel.FromStoreList = _iDropDownService.GetStoreList();
            inventoryTransferModel.ToStoreList = _iDropDownService.GetStoreList();
            inventoryTransferModel.EmployeeList = _iDropDownService.GetEmployeeList();
            inventoryTransferModel.IngredientList = _iDropDownService.GetIngredientList();
            inventoryTransferModel.FoodMenuList = _iDropDownService.GetFoodMenuListByFoodmenuType(-1);
            return View(inventoryTransferModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InventoryTransfer(InventoryTransferModel inventoryTransferModel)
        {
            inventoryTransferModel.FromStoreList = _iDropDownService.GetStoreList();
            inventoryTransferModel.ToStoreList = _iDropDownService.GetStoreList();
            inventoryTransferModel.EmployeeList = _iDropDownService.GetEmployeeList();
            inventoryTransferModel.IngredientList = _iDropDownService.GetIngredientList();
            inventoryTransferModel.FoodMenuList = _iDropDownService.GetFoodMenuListByFoodmenuType(-1);
            string purchaseMessage = string.Empty;

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationInveotryTransfer(inventoryTransferModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    return Json(new { error = true, message = errorString, status = 201 });
                }
            }

            if (inventoryTransferModel.InventoryTransferDetail != null)
            {
                if (inventoryTransferModel.InventoryTransferDetail.Count > 0)
                {

                    if (inventoryTransferModel.Id > 0)
                    {

                        int result = _inventoryTransferService.UpdateInventoryTransfer(inventoryTransferModel);
                        if (result > 0)
                        {
                            purchaseMessage = _locService.GetLocalizedHtmlString("EditSuccss");
                        }
                    }
                    else
                    {
                        //inventoryTransferModel.Date = DateTime.Now;
                        inventoryTransferModel.ReferenceNo = _inventoryTransferService.ReferenceNumber().ToString();
                        int result = _inventoryTransferService.InsertInventoryTransfer(inventoryTransferModel);
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
            int result = _inventoryTransferService.DeleteInventoryTransfer(id);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return RedirectToAction(nameof(InventoryTransferList));
        }

        public ActionResult DeleteInventoryTransferDetails(long id)
        {
            long result = _inventoryTransferService.DeleteInventoryTransferDetail(id);
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return Json(new { error = false, message = string.Empty, status = 200 });
        }

        private string ValidationInveotryTransfer(InventoryTransferModel inventoryTransferModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(inventoryTransferModel.FromStoreId.ToString()) || inventoryTransferModel.FromStoreId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidFormStore");
                return ErrorString;
            }
            if (string.IsNullOrEmpty(inventoryTransferModel.ToStoreId.ToString()) || inventoryTransferModel.ToStoreId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidToStore");
                return ErrorString;
            }
            
            if (inventoryTransferModel.InventoryTransferDetail == null || inventoryTransferModel.InventoryTransferDetail.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidTransferDetails");
                return ErrorString;
            }

            return ErrorString;
        }

        public ActionResult GetFoodMenuStock(int foodMenuId, int storeId)
        {
            decimal stockQty = 0;
            stockQty = _inventoryTransferService.GetFoodMenuStock(foodMenuId, storeId);
            return Json(new { StockQty = stockQty });
        }

        public ActionResult View(long? id)
        {
            InventoryTransferModel inventoryTransferModel = new InventoryTransferModel();
            long purchaseId = Convert.ToInt64(id);
            inventoryTransferModel = _inventoryTransferService.GetViewInventoryTransferById(purchaseId);
            return View(inventoryTransferModel);
        }
    }
}
