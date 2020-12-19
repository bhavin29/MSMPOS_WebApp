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
    public class CardTerminalController : Controller
    {
        private readonly ICardTerminalService _iCardTerminalService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;

        public CardTerminalController(ICardTerminalService iCardTerminalService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _iCardTerminalService = iCardTerminalService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
        }

        public ActionResult Index()
        {
            List<CardTerminalModel> cardTerminalModels = new List<CardTerminalModel>();
            cardTerminalModels = _iCardTerminalService.GetCardTerminalList().ToList();
            return View(cardTerminalModels);
          }

        public ActionResult CardTerminal(int? id)
        {
            CardTerminalModel cardTerminalModel = new CardTerminalModel();
            if (id > 0)
            {
                int cardTerminalId = Convert.ToInt32(id);
                cardTerminalModel = _iCardTerminalService.GetCardTerminalById(cardTerminalId);
            }
            cardTerminalModel.OutletList = _iDropDownService.GetOutletList();

            return View(cardTerminalModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CardTerminal(CardTerminalModel cardTerminalModel, string submitButton)
        {
            if (cardTerminalModel.Id > 0)
            {
                var result = _iCardTerminalService.UpdateCardTerminal(cardTerminalModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _iCardTerminalService.InsertCardTerminal(cardTerminalModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }
            cardTerminalModel.OutletList = _iDropDownService.GetOutletList();
            return View();
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iCardTerminalService.DeleteCardTerminal(id);

            return RedirectToAction(nameof(Index));
        }


    }
}
