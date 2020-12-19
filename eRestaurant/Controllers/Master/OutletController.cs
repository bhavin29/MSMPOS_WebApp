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
    public class OutletController : Controller
    {
        private readonly IOutletService _iOutletService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;

        public OutletController(IOutletService iOutletService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _iOutletService = iOutletService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
        }

        public ActionResult Index()
        {
            List<OutletModel> adonsList = new List<OutletModel>();
            adonsList = _iOutletService.GetOutletList().ToList();
            return View(adonsList);
            // return View("../Master/Addons/Index");

        }

        public ActionResult Outlet(int? id)
        {
            OutletModel outletModel = new OutletModel();
            if (id > 0)
            {
                int addonsId = Convert.ToInt32(id);
                outletModel = _iOutletService.GetOutletById(addonsId);
            }
            outletModel.StoreList = _iDropDownService.GetStoreList();

            return View(outletModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Outlet(OutletModel outletModel, string submitButton)
        {
            if (outletModel.Id > 0)
            {
                var result = _iOutletService.UpdateOutlet(outletModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _iOutletService.InsertOutlet(outletModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }
            outletModel.StoreList = _iDropDownService.GetStoreList();
            return View();
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iOutletService.DeleteOutlet(id);

            return RedirectToAction(nameof(Index));
        }


    }
}
