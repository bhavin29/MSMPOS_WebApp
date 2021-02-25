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
    public class EmailSettingController : Controller
    {
        private readonly ICommonService _iCommonService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public EmailSettingController(ICommonService iCommonService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public IActionResult Index()
        {
            ClientModel clientModel = new ClientModel();
            clientModel = _iCommonService.GetEmailSettings();
            return View(clientModel);
        }
        [HttpPost]
        public IActionResult EmailSetting(ClientModel clientModel)
        {
            int result = 0;
            result = _iCommonService.UpdateEmailSettings(clientModel);
            return RedirectToAction("Index", "Home");
        }
    }
}
