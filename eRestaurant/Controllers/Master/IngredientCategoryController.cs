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

namespace RocketPOS.Controllers.Master
{
    public class IngredientCategoryController : Controller
    {
        private readonly IIngredientCategoryService _iIngredientCategoryService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;

        public IngredientCategoryController(IIngredientCategoryService ingredientCategoryService, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _iIngredientCategoryService = ingredientCategoryService;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: AddonsController
        public ActionResult Index()
        {
            List<IngredientCategoryModel> ingredientCategoryList = new List<IngredientCategoryModel>();
            ingredientCategoryList = _iIngredientCategoryService.GetIngredientCategoryList().ToList();
            return View(ingredientCategoryList);
            // return View("../Master/Addons/Index");

        }

        // GET: AddonsController/Details/5
        public ActionResult IngredientCategory(int? id)
        {
            IngredientCategoryModel ingredientCategoryModel = new IngredientCategoryModel();
            if (id > 0)
            {
                int ingredientCategroyId = Convert.ToInt32(id);
                ingredientCategoryModel = _iIngredientCategoryService.GetIngredientCategoryById(ingredientCategroyId);
            }

            return View(ingredientCategoryModel);
        }

        // POST: Addons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IngredientCategory(IngredientCategoryModel ingredientCategoryModel, string submitButton)
        {
            if (ingredientCategoryModel.Id > 0)
            {
                var result = _iIngredientCategoryService.UpdateIngredientCategory(ingredientCategoryModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _iIngredientCategoryService.InsertIngredientCategory(ingredientCategoryModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }

            return View();
        }

        // GET: Addons/Delete/5
        public ActionResult Delete(int id)
        {
            var deletedid =_iIngredientCategoryService.DeleteIngredientCategory(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
