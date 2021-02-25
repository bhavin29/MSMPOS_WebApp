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
    public class AssetCategoryController : Controller
    {
        private readonly IAssetCategoryService _iAssetCategoryService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public AssetCategoryController(IAssetCategoryService iAssetCategoryService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iAssetCategoryService = iAssetCategoryService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }
        public ActionResult Index()
        {
            List<AssetCategoryModel> assetCategoryModel = new List<AssetCategoryModel>();
            assetCategoryModel = _iAssetCategoryService.GetAssetCategoryList().ToList();
            return View(assetCategoryModel);
        }

        public ActionResult AssetCategory(int? id)
        {
            AssetCategoryModel assetCategoryModel = new AssetCategoryModel();
            if (id > 0)
            {
                assetCategoryModel = _iAssetCategoryService.GetAssetCategoryById(Convert.ToInt32(id));
            }
            return View(assetCategoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssetCategory(AssetCategoryModel assetCategoryModel)
        {

            if (assetCategoryModel.Id > 0)
            {
                var result = _iAssetCategoryService.UpdateAssetCategory(assetCategoryModel);
                if (result == -1)
                {
                    ModelState.AddModelError("AssetCategoryName", "Asset category already exists");
                    return View(assetCategoryModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iAssetCategoryService.InsertAssetCategory(assetCategoryModel);
                if (result == -1)
                {
                    ModelState.AddModelError("AssetCategoryName", "Asset category already exists");
                    return View(assetCategoryModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }
            return RedirectToAction("Index", "AssetCategory");
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iAssetCategoryService.DeleteAssetCategory(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
