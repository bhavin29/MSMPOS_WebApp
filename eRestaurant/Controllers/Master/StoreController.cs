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
    public class StoreController : Controller
    {

        private readonly IStoreService _istoreService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public StoreController(IStoreService storeService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _istoreService = storeService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;

        }

        public ActionResult Index()
        {
            List<StoreModel> storeModel = new List<StoreModel>();
            storeModel = _istoreService.GetStoreList().ToList();
            return View(storeModel);
          }

        public ActionResult Store(int? id)
        {
            StoreModel storeModel = new StoreModel();
            if (id > 0)
            {
                int storeId = Convert.ToInt32(id);
                storeModel = _istoreService.GetStoreById(storeId);
            }

            return View(storeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Store(StoreModel storeModel, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationStore(storeModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(storeModel);
                }
            }

            if (storeModel.Id > 0)
            {
                var result = _istoreService.UpdateStore(storeModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _istoreService.InsertStore(storeModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "Store");

        }

        public ActionResult Delete(int id)
        {
            var deletedid = _istoreService.DeleteStore(id);

            return RedirectToAction(nameof(Index));
        }

        private string ValidationStore(StoreModel storeModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(storeModel.StoreName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidStoreName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(storeModel.Price.ToString()) || storeModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }

    }
}
