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
        private readonly ICommonService _iCommonService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public CardTerminalController(ICardTerminalService iCardTerminalService, ICommonService iCommonService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iCardTerminalService = iCardTerminalService;
            _iDropDownService = idropDownService;
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            _iCommonService.GetPageWiseRoleRigths("CardTerminal");
            List<CardTerminalModel> cardTerminalModels = new List<CardTerminalModel>();
            cardTerminalModels = _iCardTerminalService.GetCardTerminalList().ToList();
            return View(cardTerminalModels);
        }

        public ActionResult CardTerminal(int? id)
        {
            CardTerminalModel cardTerminalModel = new CardTerminalModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true)
            {
                if (id > 0)
                {
                    int cardTerminalId = Convert.ToInt32(id);
                    cardTerminalModel = _iCardTerminalService.GetCardTerminalById(cardTerminalId);
                }
                cardTerminalModel.OutletList = _iDropDownService.GetOutletList();

                return View(cardTerminalModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CardTerminal(CardTerminalModel cardTerminalModel, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationCardTerminal(cardTerminalModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(cardTerminalModel);
                }
            }

            if (cardTerminalModel.Id > 0)
            {
                var result = _iCardTerminalService.UpdateCardTerminal(cardTerminalModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iCardTerminalService.InsertCardTerminal(cardTerminalModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }
            cardTerminalModel.OutletList = _iDropDownService.GetOutletList();

            return RedirectToAction("Index", "CardTerminal");
        }

        public ActionResult Delete(int id)
        {
            if (UserRolePermissionForPage.Delete == true)
            {
                var deletedid = _iCardTerminalService.DeleteCardTerminal(id);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        private string ValidationCardTerminal(CardTerminalModel cardTerminalModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(cardTerminalModel.CardTerminalName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(cardTerminalModel.Price.ToString()) || cardTerminalModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }


    }
}
