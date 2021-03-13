using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Master
{
    public class AssetItemController : Controller
    {
        private readonly ICommonService _iCommonService;
        private readonly IAssetItemService _iAssetItemService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public AssetItemController(IAssetItemService assetItemService, ICommonService iCommonService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iAssetItemService = assetItemService; _iCommonService = iCommonService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index(int? noDelete)
        {
            List<AssetItemModel> assetItemModel = new List<AssetItemModel>();
            assetItemModel = _iAssetItemService.GetAssetItemList().ToList();
            if (noDelete != null)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Can not delete reference available.");
            }
            return View(assetItemModel);
        }

        public ActionResult AssetItem(int? id)
        {
            AssetItemModel assetItemModel = new AssetItemModel();
            if (id > 0)
            {
                assetItemModel = _iAssetItemService.GetAssetItemById(Convert.ToInt32(id));
            }
            assetItemModel.UnitList = _iDropDownService.GetUnitList();
            assetItemModel.TaxList = _iDropDownService.GetTaxList();
            assetItemModel.AssetCategoryList = _iDropDownService.GetAssetCategoryList();
            return View(assetItemModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssetItem(AssetItemModel assetItemModel)
        {
            if (assetItemModel.Id > 0)
            {
                var result = _iAssetItemService.UpdateAssetItem(assetItemModel);
                if (result == -1)
                {
                    ModelState.AddModelError("AssetItemName", "Asset item already exists");
                    return View(assetItemModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iAssetItemService.InsertAssetItem(assetItemModel);
                if (result == -1)
                {
                    ModelState.AddModelError("AssetItemName", "Asset item already exists");
                    return View(assetItemModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }
            return RedirectToAction("Index", "AssetItem");
        }

        public ActionResult Delete(int id)
        {
            int result = 0;
            result = _iCommonService.GetValidateReference("AssetItem", id.ToString());
            if (result > 0)
            {
                return RedirectToAction(nameof(Index), new { noDelete = result });
            }
            else
            {
                var deletedid = _iAssetItemService.DeleteAssetItem(id);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
