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
    public class CustomerController : Controller
    {
        private readonly ICustomerService _iCustomerService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;

        public CustomerController(ICustomerService customerService, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _iCustomerService = customerService;
            _sharedLocalizer = sharedLocalizer;
        }

        public ActionResult Index()
        {
            List<CustomerModel> customerList = new List<CustomerModel>();
            customerList = _iCustomerService.GetCustomerList().ToList();
            return View(customerList);
        }

        public ActionResult Customer(int? id)
        {
            CustomerModel customerModel = new CustomerModel();
            if (id > 0)
            {
                int customerId = Convert.ToInt32(id);
                customerModel = _iCustomerService.GetCustomerById(customerId);
            }

            return View(customerModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Customer(CustomerModel customerModel, string submitButton)
        {
            if (customerModel.Id > 0)
            {
                var result = _iCustomerService.UpdateCustomer(customerModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _iCustomerService.InsertCustomer(customerModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iCustomerService.DeleteCustomer(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
