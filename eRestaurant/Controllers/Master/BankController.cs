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
        private readonly ICommonService _iCommonService;
        private readonly IBankService _iBankService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;
        public BankController(IBankService iBankService, ICommonService iCommonService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iBankService = iBankService;
            _sharedLocalizer = sharedLocalizer; _iCommonService = iCommonService;
            _locService = locService;
        }

        public ActionResult Index(int? noDelete)
        {
            _iCommonService.GetPageWiseRoleRigths("Bank");
            List<BankModel> bankList = new List<BankModel>();
            bankList = _iBankService.GetBankList().ToList();
            if (noDelete != null)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Can not delete reference available.");
            }
            return View(bankList);
        }

        public ActionResult Bank(int? id)
        {
            BankModel bankModel = new BankModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true)
            {
                if (id > 0)
                {
                    int bankId = Convert.ToInt32(id);
                    bankModel = _iBankService.GetBankById(bankId);
                }

                return View(bankModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
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
                if (result == -1)
                {
                    ModelState.AddModelError("Bankname", "Bank name already exists");
                    return View(bankModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iBankService.InsertBank(bankModel);

                if (result == -1)
                {
                    ModelState.AddModelError("Bankname", "Bank name already exists");
                    return View(bankModel);
                }

                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "Bank");
        }

        public ActionResult Delete(int id)
        {
            int result = 0;
            if (UserRolePermissionForPage.Delete == true)
            {
                result = _iCommonService.GetValidateReference("Bank", id.ToString());
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index), new { noDelete = result });
                }
                else
                {
                    var deletedid = _iBankService.DeleteBank(id);
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
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
