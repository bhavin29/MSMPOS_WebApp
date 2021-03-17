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
        private readonly ICommonService _iCommonService;
        private readonly IAssetCategoryService _iAssetCategoryService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public AssetCategoryController(IAssetCategoryService iAssetCategoryService, ICommonService iCommonService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iAssetCategoryService = iAssetCategoryService; _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }
        public ActionResult Index(int? noDelete)
        {
            _iCommonService.GetPageWiseRoleRigths("AssetCategory");
            List<AssetCategoryModel> assetCategoryModel = new List<AssetCategoryModel>();
            assetCategoryModel = _iAssetCategoryService.GetAssetCategoryList().ToList();
            if (noDelete != null)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Can not delete reference available.");
            }
            return View(assetCategoryModel);
        }

        public ActionResult AssetCategory(int? id)
        {
            AssetCategoryModel assetCategoryModel = new AssetCategoryModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true)
            {
                if (id > 0)
                {
                    assetCategoryModel = _iAssetCategoryService.GetAssetCategoryById(Convert.ToInt32(id));
                }
                return View(assetCategoryModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
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
            int result = 0;
            if (UserRolePermissionForPage.Delete == true)
            {
                result = _iCommonService.GetValidateReference("AssetCategory", id.ToString());
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index), new { noDelete = result });
                }
                else
                {
                    var deletedid = _iAssetCategoryService.DeleteAssetCategory(id);
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }
    }
}
