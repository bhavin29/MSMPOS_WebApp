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
        private readonly ITablesService _iTablesService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;

        public TablesController(ITablesService tableService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _iTablesService = tableService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
        }

        public ActionResult Index()
        {
            List<TablesModel> tableList = new List<TablesModel>();
            tableList = _iTablesService.GetTablesList().ToList();
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
            if (tableModel.Id > 0)
            {
                var result = _iTablesService.UpdateTables(tableModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _iTablesService.InsertTables(tableModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }
            tableModel.OutletList = _iDropDownService.GetOutletList();
            return View();
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iTablesService.DeleteTables(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
