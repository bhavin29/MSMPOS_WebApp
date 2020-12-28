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

        [HttpGet]
        public ActionResult GetOrderById(long Id)
        {
            InventoryTransferModel inventoryTransferModel = new InventoryTransferModel();
            inventoryTransferModel = _inventoryTransferService.GetInventoryTransferById(Id);
            return View(inventoryTransferModel);
        }

        public ActionResult InventoryTransfer(long? id)
        {
            InventoryTransferModel inventoryTransferModel = new InventoryTransferModel();
            if (id > 0)
            {
                long purchaseId = Convert.ToInt64(id);
                inventoryTransferModel = _inventoryTransferService.GetInventoryTransferById(purchaseId);

            }
            else
            {
                inventoryTransferModel.Date = DateTime.Now;
                inventoryTransferModel.ReferenceNo = _inventoryTransferService.ReferenceNumber().ToString();
            }
            inventoryTransferModel.FromStoreList = _iDropDownService.GetStoreList();
            inventoryTransferModel.ToStoreList = _iDropDownService.GetStoreList();
            inventoryTransferModel.EmployeeList = _iDropDownService.GetEmployeeList();
            inventoryTransferModel.IngredientList = _iDropDownService.GetIngredientList();

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
            //if (string.IsNullOrEmpty(purchaseModel.ReferenceNo.ToString()) || purchaseModel.ReferenceNo == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidReferenceNo");
            //    return ErrorString;
            //}
            if (string.IsNullOrEmpty(inventoryTransferModel.FromStoreId.ToString()) || inventoryTransferModel.FromStoreId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSupplier");
                return ErrorString;
            }
            if (string.IsNullOrEmpty(inventoryTransferModel.EmployeeId.ToString()) || inventoryTransferModel.EmployeeId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSupplier");
                return ErrorString;
            }
            if (inventoryTransferModel.InventoryTransferDetail == null || inventoryTransferModel.InventoryTransferDetail.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidPurchaseDetails");
                return ErrorString;
            }

            return ErrorString;
        }


    }
}
