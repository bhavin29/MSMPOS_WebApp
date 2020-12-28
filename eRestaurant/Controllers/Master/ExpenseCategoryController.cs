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
    public class ExpenseCategoryController : Controller
    {

        private readonly IExpsenseCategoryService _iexpsenseCategoryService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public ExpenseCategoryController(IExpsenseCategoryService expsenseCategoryService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iexpsenseCategoryService = expsenseCategoryService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            List<ExpsenseCategoryModel> expenseCategoryModel = new List<ExpsenseCategoryModel>();
            expenseCategoryModel = _iexpsenseCategoryService.GetExpsenseCategoryList().ToList();
            return View(expenseCategoryModel);
            // return View("../Master/ExpenseCategory/Index");

        }

        public ActionResult ExpenseCategory(int? id)
        {
            ExpsenseCategoryModel expenseCategoryModel = new ExpsenseCategoryModel();
            if (id > 0)
            {
                int expenseCAtegoryId = Convert.ToInt32(id);
                expenseCategoryModel = _iexpsenseCategoryService.GetExpsenseCategoryById(expenseCAtegoryId);

            }

            return View(expenseCategoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExpenseCategory(ExpsenseCategoryModel expenseCategoryModel, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationExpenseCategory(expenseCategoryModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(expenseCategoryModel);
                }
            }

            if (expenseCategoryModel.Id > 0)
            {
                var result = _iexpsenseCategoryService.UpdateExpsenseCategory(expenseCategoryModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iexpsenseCategoryService.InsertExpsenseCategory(expenseCategoryModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "ExpenseCategory");
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iexpsenseCategoryService.DeleteExpsenseCategory(id);

            return RedirectToAction(nameof(Index));
        }
        
        private string ValidationExpenseCategory(ExpsenseCategoryModel expenseCategoryModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(expenseCategoryModel.ExpenseCategory))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(expenseCategoryModel.Price.ToString()) || expenseCategoryModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }

    }
}
