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
        private readonly ICommonService _iCommonService;
        private readonly ICustomerService _iCustomerService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public CustomerController(ICustomerService customerService, IStringLocalizer<RocketPOSResources> sharedLocalizer, ICommonService iCommonService, LocService locService)
        {
            _iCustomerService = customerService; _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index(int? noDelete)
        {
            _iCommonService.GetPageWiseRoleRigths("Customer");
            List<CustomerModel> customerList = new List<CustomerModel>();
            customerList = _iCustomerService.GetCustomerList().ToList();
            if (noDelete != null)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Can not delete reference available.");
            }
            return View(customerList);
        }

        public ActionResult Customer(int? id)
        {
            CustomerModel customerModel = new CustomerModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true)
            {
                if (id > 0)
                {
                    int customerId = Convert.ToInt32(id);
                    customerModel = _iCustomerService.GetCustomerById(customerId);
                }

                return View(customerModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Customer(CustomerModel customerModel, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationCustomer(customerModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(customerModel);
                }
            }

            if (customerModel.Id > 0)
            {
                var result = _iCustomerService.UpdateCustomer(customerModel);
                if (result == -1)
                {
                    ModelState.AddModelError("CustomerPhone", "Customer phone already exists");
                    return View(customerModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iCustomerService.InsertCustomer(customerModel);
                if (result == -1)
                {
                    ModelState.AddModelError("CustomerPhone", "Customer phone already exists");
                    return View(customerModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "Customer");
        }

        public ActionResult Delete(int id)
        {
            int result = 0;
            if (UserRolePermissionForPage.Delete == true)
            {
                result = _iCommonService.GetValidateReference("Customer", id.ToString());
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index), new { noDelete = result });
                }
                else
                {
                    var deletedid = _iCustomerService.DeleteCustomer(id);
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        private string ValidationCustomer(CustomerModel customerModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(customerModel.CustomerName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(customerModel.Price.ToString()) || customerModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }

    }
}
