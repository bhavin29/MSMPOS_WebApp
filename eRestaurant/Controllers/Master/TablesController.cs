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
    public class TablesController : Controller
    {
        private readonly ICommonService _iCommonService;
        private readonly ITablesService _iTablesService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public TablesController(ITablesService tableService, ICommonService iCommonService,IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iTablesService = tableService;
            _iDropDownService = idropDownService;
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index(int? noDelete)
        {
            List<TablesModel> tableList = new List<TablesModel>();
            tableList = _iTablesService.GetTablesList().ToList();
            if (noDelete != null)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Can not delete reference available.");
            }
            return View(tableList);
        }

        public ActionResult Tables(int? id)
        {
            TablesModel tableModel = new TablesModel();
            if (id > 0)
            {
                int tableId = Convert.ToInt32(id);
                tableModel = _iTablesService.GetTablesById(tableId);
            }
            tableModel.OutletList = _iDropDownService.GetOutletList();
            return View(tableModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tables(TablesModel tableModel, string submitButton)
        {
            tableModel.OutletList = _iDropDownService.GetOutletList();

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationTables(tableModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(tableModel);
                }
            }

            if (tableModel.Id > 0)
            {
                var result = _iTablesService.UpdateTables(tableModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iTablesService.InsertTables(tableModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "Tables");
        }

        public ActionResult Delete(int id)
        {
            int result = 0;
            result = _iCommonService.GetValidateReference("Tables", id.ToString());
            if (result > 0)
            {
                return RedirectToAction(nameof(Index), new { noDelete = result });
            }
            else
            {
                var deletedid = _iTablesService.DeleteTables(id);
                return RedirectToAction(nameof(Index));
            }
        }

        private string ValidationTables(TablesModel tablesModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(tablesModel.TableName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(tablesModel.Price.ToString()) || tablesModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }

    }
}
