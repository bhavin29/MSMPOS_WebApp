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
    public class PaymentMethodController : Controller
    {
        private readonly IPaymentMethodService _iPaymentMethodService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public PaymentMethodController(IPaymentMethodService paymentMethod, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iPaymentMethodService = paymentMethod;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            List<PaymentMethodModel> paymentMethodModel = new List<PaymentMethodModel>();
            paymentMethodModel = _iPaymentMethodService.GetPaymentMethodList().ToList();
            return View(paymentMethodModel);
        }

        public ActionResult PaymentMethod(int? id)
        {
            PaymentMethodModel paymentMethodModel = new PaymentMethodModel();
            if (id > 0)
            {
                int paymentMethodId = Convert.ToInt32(id);
                paymentMethodModel = _iPaymentMethodService.GetAddonesById(paymentMethodId);
            }

            return View(paymentMethodModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentMethod(PaymentMethodModel paymentMethodModel, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationPaymentMethod(paymentMethodModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(paymentMethodModel);
                }
            }

            if (paymentMethodModel.Id > 0)
            {
                var result = _iPaymentMethodService.UpdatePaymentMethod(paymentMethodModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iPaymentMethodService.InsertPaymentMethod(paymentMethodModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "PaymentMethod");
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iPaymentMethodService.DeletePaymentMethod(id);

            return RedirectToAction(nameof(Index));
        }

        private string ValidationPaymentMethod(PaymentMethodModel paymentMethodModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(paymentMethodModel.PaymentMethodName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(paymentMethodModel.Price.ToString()) || paymentMethodModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }

    }
}
