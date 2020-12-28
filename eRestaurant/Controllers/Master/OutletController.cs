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
        private LocService _locService;

        public OutletController(IOutletService iOutletService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iOutletService = iOutletService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            List<OutletModel> adonsList = new List<OutletModel>();
            adonsList = _iOutletService.GetOutletList().ToList();
  
            return View(adonsList);
        }

        public ActionResult Outlet(int? id)
        {
            OutletModel outletModel = new OutletModel();
            if (id > 0)
            {
                int outletId = Convert.ToInt32(id);
                outletModel = _iOutletService.GetOutletById(outletId);
            }
            outletModel.StoreList = _iDropDownService.GetStoreList();

            return View(outletModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Outlet(OutletModel outletModel, string submitButton)
        {
            outletModel.StoreList = _iDropDownService.GetStoreList();

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationOutlet(outletModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(outletModel);
                }
            }

            if (outletModel.Id > 0)
            {
                var result = _iOutletService.UpdateOutlet(outletModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iOutletService.InsertOutlet(outletModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }
            return RedirectToAction("Index", "Outlet");
        }

            public ActionResult Delete(int id)
        {
            var deletedid = _iOutletService.DeleteOutlet(id);

            return RedirectToAction(nameof(Index));
        }

        private string ValidationOutlet(OutletModel outletModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(outletModel.OutletName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(outletModel.Price.ToString()) || outletModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }



    }
}
