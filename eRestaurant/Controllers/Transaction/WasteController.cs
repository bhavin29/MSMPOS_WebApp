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
        public ActionResult WasteList(int? foodMenuId, int? ingredientId)
        {
            WasteListViewModel wasteListViewModel = new WasteListViewModel();
            wasteListViewModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            wasteListViewModel.IngredientList = _iDropDownService.GetIngredientList();
            wasteListViewModel.WasteListModels = _iWasteService.GetWasteList(Convert.ToInt32(foodMenuId), Convert.ToInt32(ingredientId)).ToList();
            return View(wasteListViewModel);
        }

        // GET: Waste/Details/5
        [HttpGet]
        public ActionResult GetOrderById(long wasteId)
        {
            WasteModel wasteModel = new WasteModel()
            {
                StoreList = _iDropDownService.GetStoreList(),
                FoodMenuList = _iDropDownService.GetFoodMenuList(),
                IngredientList = _iDropDownService.GetIngredientList(),
                //EmployeeList = _iDropDownService.GetEmployeeList(),
                FoodMenuListForLostAmount = _iWasteService.FoodMenuListForLostAmount(),
                IngredientListForLostAmount = _iWasteService.IngredientListForLostAmount(),
            };
            wasteModel = _iWasteService.GetWasteById(wasteId);

            return View(wasteModel);
        }

        // GET: Waste/Create
        public ActionResult Waste(long? id, string type)
        {
            WasteModel wasteModel = new WasteModel();

            if (id > 0)
            {
                long wasteId = Convert.ToInt64(id);
                wasteModel = _iWasteService.GetWasteById(wasteId);
                ViewBag.ActionType = type;
            }
            else
            {
                wasteModel.WasteDateTime = DateTime.Now;
                wasteModel.ReferenceNumber = _iWasteService.ReferenceNumber().ToString();
            }

            wasteModel.StoreList = _iDropDownService.GetStoreList();
            wasteModel.FoodMenuList = _iDropDownService.GetFoodMenuListByFoodmenuType(3);
            wasteModel.IngredientList = _iDropDownService.GetIngredientList();
            wasteModel.FoodMenuListForLostAmount = _iWasteService.FoodMenuListForLostAmount();
            wasteModel.IngredientListForLostAmount = _iWasteService.IngredientListForLostAmount();


            return View(wasteModel);
        }

        // POST: Waste/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Waste(WasteModel wasteModel)
        {
            //wasteModel.FoodMenuList = _iDropDownService.GetFoodMenuList();
            //wasteModel.IngredientList = _iDropDownService.GetIngredientList();
            //wasteModel.EmployeeList = _iDropDownService.GetEmployeeList();
            //wasteModel.StoreList = _iDropDownService.GetStoreList();

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

        public ActionResult DeleteWasteDetails(string wasteId)
        {
            var obj = wasteId.Split("|");
            long result = _iWasteService.DeleteWasteDetails(Convert.ToInt64(obj[0]), Convert.ToInt64(obj[1]), Convert.ToInt64(obj[2]));
            if (result > 0)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Delete");
            }
            return Json(new { error = false, message = string.Empty, status = 200 });
        }

        private string ValidationWaste(WasteModel wasteModel)
        {
            string ErrorString = string.Empty;
            //if (string.IsNullOrEmpty(wasteModel.EmployeeId.ToString()) || wasteModel.EmployeeId == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidEmployee");
            //    return ErrorString;
            //}
            if (string.IsNullOrEmpty(wasteModel.StoreId.ToString()) || wasteModel.StoreId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidStore");
                return ErrorString;
            }
            if (wasteModel.WasteDetail == null || wasteModel.WasteDetail.Count < 1)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidWasteDetails");
                return ErrorString;
            }

            return ErrorString;
        }

        [HttpGet]
        public JsonResult GetIngredientPurchasePrice(int id)
        {
            decimal ingredientPurchasePrice = 0;
            ingredientPurchasePrice = _iWasteService.GetIngredientPurchasePrice(id);
            return Json(new { ingredientPurchasePrice = ingredientPurchasePrice });
        }

        [HttpGet]
        public JsonResult GetFoodMenuPurchasePrice(int id)
        {
            decimal foodMenuPurchasePrice = 0;
            foodMenuPurchasePrice = _iWasteService.GetFoodMenuPurchasePrice(id);
            return Json(new { foodMenuPurchasePrice = foodMenuPurchasePrice });
        }
    }
}