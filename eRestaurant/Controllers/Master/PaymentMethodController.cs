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

        public PaymentMethodController(IPaymentMethodService paymentMethod, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _iPaymentMethodService = paymentMethod;
            _sharedLocalizer = sharedLocalizer;
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
            if (paymentMethodModel.Id > 0)
            {
                var result = _iPaymentMethodService.UpdatePaymentMethod(paymentMethodModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _iPaymentMethodService.InsertPaymentMethod(paymentMethodModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iPaymentMethodService.DeletePaymentMethod(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
