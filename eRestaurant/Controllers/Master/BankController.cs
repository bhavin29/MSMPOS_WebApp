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
    public class BankController : Controller
    {
        private readonly IBankService _iBankService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public BankController(IBankService iBankService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iBankService = iBankService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            List<BankModel> bankList = new List<BankModel>();
            bankList = _iBankService.GetBankList().ToList();
            return View(bankList);
        }

        public ActionResult Bank(int? id)
        {
            BankModel bankModel = new BankModel();
            if (id > 0)
            {
                int bankId = Convert.ToInt32(id);
                bankModel = _iBankService.GetBankById(bankId);
            }

            return View(bankModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bank(BankModel bankModel, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationBank(bankModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(bankModel);
                }
            }

            if (bankModel.Id > 0)
            {
                var result = _iBankService.UpdateBank(bankModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iBankService.InsertBank(bankModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "Bank");
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iBankService.DeleteBank(id);

            return RedirectToAction(nameof(Index));
        }

        private string ValidationBank(BankModel bankModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(bankModel.BankName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidBankName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(BankModel.Price.ToString()) || BankModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }

    }
}
