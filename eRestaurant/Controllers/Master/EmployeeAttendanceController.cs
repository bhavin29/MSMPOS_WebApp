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
    public class EmployeeAttendanceController : Controller
    {

        private readonly IEmployeeAttendanceService _iemployeeAttendanceService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;

        public EmployeeAttendanceController(IEmployeeAttendanceService employeeAttendanceService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer)
        {
            _iemployeeAttendanceService = employeeAttendanceService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
        }

        public ActionResult Index()
        {
            List<EmployeeAttendanceModel> userModel = new List<EmployeeAttendanceModel>();
            userModel = _iemployeeAttendanceService.GetEmployeeAttendaceList().ToList();
            return View(userModel);
        }

        public ActionResult EmployeeAttendance(int? id)
        {
            EmployeeAttendanceModel employeeAttendanceModel = new EmployeeAttendanceModel();
            if (id > 0)
            {
                int userId = Convert.ToInt32(id);
                employeeAttendanceModel = _iemployeeAttendanceService.GetEmployeeAttendaceById(userId);
            }
            employeeAttendanceModel.EmployeeList = _iDropDownService.GetEmployeeList();

            return View(employeeAttendanceModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeAttendance(EmployeeAttendanceModel employeeAttendanceModel, string submitButton)
        {
            if (employeeAttendanceModel.Id > 0)
            {
                var result = _iemployeeAttendanceService.UpdateEmployeeAttendance(employeeAttendanceModel);
                ViewBag.Result = _sharedLocalizer["EditSuccss"].Value;
            }
            else
            {
                var result = _iemployeeAttendanceService.InsertEmployeeAttendance(employeeAttendanceModel);
                ViewBag.Result = _sharedLocalizer["SaveSuccess"].Value;
            }
            employeeAttendanceModel.EmployeeList = _iDropDownService.GetEmployeeList();
            return View();
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iemployeeAttendanceService.DeleteEmployeeAttendance(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
