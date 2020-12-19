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
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _iemployeeService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;

        public EmployeeController(IEmployeeService employeeService, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _iemployeeService = employeeService;
            _sharedLocalizer = sharedLocalizer;
        }

        public ActionResult Index()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            employeeList = _iemployeeService.GetEmployeeList().ToList();
            return View(employeeList);
        }

        public ActionResult Employee(int? id)
        {
            EmployeeModel employeeModel = new EmployeeModel();
            if (id > 0)
            {
                int employeeId = Convert.ToInt32(id);
                employeeModel = _iemployeeService.GetEmployeeById(employeeId);
            }

            return View(employeeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Employee(EmployeeModel employeeModel, string submitButton)
        {
            if (employeeModel.Id > 0)
            {
                var result = _iemployeeService.UpdateEmployee(employeeModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _iemployeeService.InsertEmployee(employeeModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iemployeeService.DeleteEmployee(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
