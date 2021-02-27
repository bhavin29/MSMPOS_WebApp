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
    public class GlobalStatusController : Controller
    {
        private readonly IGlobalStatusService _iGlobalStatusService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public GlobalStatusController(IGlobalStatusService iGlobalStatusService, IDropDownService iDropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iGlobalStatusService = iGlobalStatusService;
            _iDropDownService = iDropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            List<GlobalStatusModel> globalStatusModel = new List<GlobalStatusModel>();
            globalStatusModel = _iGlobalStatusService.GetGlobalStatusList().ToList();
            return View(globalStatusModel);
        }

        public ActionResult GlobalStatus(int? id)
        {
            GlobalStatusModel globalStatusModel = new GlobalStatusModel();
            if (id > 0)
            {
                globalStatusModel = _iGlobalStatusService.GetGlobalStatusById(Convert.ToInt32(id));
            }
            globalStatusModel.ModuleList = _iDropDownService.GetGlobalStatusList();
            return View(globalStatusModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GlobalStatus(GlobalStatusModel globalStatusModel)
        {

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationGlobalStatus(globalStatusModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(globalStatusModel);
                }
            }

            if (globalStatusModel.Id > 0)
            {
                if (globalStatusModel.ModuleName == "0")
                {
                    ModelState.AddModelError("ModuleName", "Select module");
                    globalStatusModel.ModuleList = _iDropDownService.GetGlobalStatusList();
                    return View(globalStatusModel);
                }
                var result = _iGlobalStatusService.UpdateGlobalStatus(globalStatusModel);
                if (result == -1)
                {
                    ModelState.AddModelError("GlobalStatusName", "GlobalStatus already exists");
                    globalStatusModel.ModuleList = _iDropDownService.GetGlobalStatusList();
                    return View(globalStatusModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                if (globalStatusModel.ModuleName == "0")
                {
                    ModelState.AddModelError("ModuleName", "Select module");
                    globalStatusModel.ModuleList = _iDropDownService.GetGlobalStatusList();
                    return View(globalStatusModel);
                }

                var result = _iGlobalStatusService.InsertGlobalStatus(globalStatusModel);
                if (result == -1)
                {
                    ModelState.AddModelError("GlobalStatusName", "GlobalStatus already exists");
                    globalStatusModel.ModuleList = _iDropDownService.GetGlobalStatusList();
                    return View(globalStatusModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "GlobalStatus");
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iGlobalStatusService.DeleteGlobalStatus(id);
            return RedirectToAction(nameof(Index));
        }

        private string ValidationGlobalStatus(GlobalStatusModel globalStatusModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(globalStatusModel.StatusName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidGlobalStatusName");
                return ErrorString;
            }
            return ErrorString;
        }
    }
}
