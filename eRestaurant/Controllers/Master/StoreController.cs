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

        public StoreController(IStoreService storeService, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _istoreService = storeService;
            _sharedLocalizer = sharedLocalizer;
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
                int addonsId = Convert.ToInt32(id);
                storeModel = _istoreService.GetStoreById(addonsId);
            }

            return View(storeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Store(StoreModel storeModel, string submitButton)
        {
            if (storeModel.Id > 0)
            {
                var result = _istoreService.UpdateStore(storeModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _istoreService.InsertStore(storeModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _istoreService.DeleteStore(id);

            return RedirectToAction(nameof(Index));
        }


    }
}
