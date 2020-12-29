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
        private LocService _locService;

        public EmployeeAttendanceController(IEmployeeAttendanceService employeeAttendanceService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iemployeeAttendanceService = employeeAttendanceService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
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
            else
            {
                employeeAttendanceModel.LogDate = DateTime.Now;
            }
            employeeAttendanceModel.EmployeeList = _iDropDownService.GetEmployeeList();

            return View(employeeAttendanceModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeAttendance(EmployeeAttendanceModel employeeAttendanceModel, string submitButton)
        {
            employeeAttendanceModel.EmployeeList = _iDropDownService.GetEmployeeList();

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationEmployeeAttendance(employeeAttendanceModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(employeeAttendanceModel);
                }
            }

            if (_iemployeeAttendanceService.ValidationEmployeeAttendance(employeeAttendanceModel)==0)
            {
                ViewBag.Validate = "Employee with same log date already exits";

                return View(employeeAttendanceModel);
            }
            else
            {
                if (employeeAttendanceModel.Id > 0)
                {
                    var result = _iemployeeAttendanceService.UpdateEmployeeAttendance(employeeAttendanceModel);
                    ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
                }
                else
                {
                    var result = _iemployeeAttendanceService.InsertEmployeeAttendance(employeeAttendanceModel);
                    ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
                }
            }

            return RedirectToAction("Index", "EmployeeAttendance");
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iemployeeAttendanceService.DeleteEmployeeAttendance(id);

            return RedirectToAction(nameof(Index));
        }

        private string ValidationEmployeeAttendance(EmployeeAttendanceModel employeeAttendanceModel)
        {
            string ErrorString = string.Empty;
            double  timeDiffrent = employeeAttendanceModel.OutTime.TotalMinutes - employeeAttendanceModel.InTime.TotalMinutes;

            if (string.IsNullOrEmpty(employeeAttendanceModel.EmployeeId.ToString()) || employeeAttendanceModel.EmployeeId == 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            if (timeDiffrent <= 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            return ErrorString;
        }

    }
}
