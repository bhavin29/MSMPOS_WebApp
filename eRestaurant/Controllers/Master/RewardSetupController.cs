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
    public class RewardSetupController : Controller
    {
        private readonly IRewardSetupService _iRewardSetupService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public RewardSetupController(IRewardSetupService iRewardSetupService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iRewardSetupService = iRewardSetupService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            List<RewardSetupModel> rewardSetupModel = new List<RewardSetupModel>();
            rewardSetupModel = _iRewardSetupService.GetRewardSetupList().ToList();
            return View(rewardSetupModel);
        }

        public ActionResult RewardSetup(int? id)
        {
            RewardSetupModel rewardSetupModel = new RewardSetupModel();
            if (id > 0)
            {
                rewardSetupModel = _iRewardSetupService.GetRewardSetupById(Convert.ToInt32(id));
            }

            return View(rewardSetupModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RewardSetup(RewardSetupModel rewardSetupModel)
        {

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationRewardSetup(rewardSetupModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(rewardSetupModel);
                }
            }

            if (rewardSetupModel.Id > 0)
            {
                var result = _iRewardSetupService.UpdateRewardSetup(rewardSetupModel);
                if (result == -1)
                {
                    ModelState.AddModelError("OfferName", "Offer name already exists");
                    return View(rewardSetupModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iRewardSetupService.InsertRewardSetup(rewardSetupModel);
                if (result == -1)
                {
                    ModelState.AddModelError("OfferName", "Offer name already exists");
                    return View(rewardSetupModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "RewardSetup");
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iRewardSetupService.DeleteRewardSetup(id);
            return RedirectToAction(nameof(Index));
        }

        private string ValidationRewardSetup(RewardSetupModel rewardSetupModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(rewardSetupModel.OfferName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidOfferName");
                return ErrorString;
            }
            return ErrorString;
        }
    }
}
