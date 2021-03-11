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
    public class UserController : Controller
    {
        private readonly IUserService _iUserService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public UserController(IUserService userService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iUserService = userService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            List<UserModel> userModel = new List<UserModel>();
            userModel = _iUserService.GetUserList().ToList();
            return View(userModel);
        }

        public ActionResult User(int? id)
        {
            UserModel userModel = new UserModel();
            if (id > 0)
            {
                int userId = Convert.ToInt32(id);
                userModel = _iUserService.GetUserById(userId);
            }
            userModel.EmployeeList = _iDropDownService.GetEmployeeList();
            userModel.OutletList = _iDropDownService.GetOutletList();
            userModel.WebRoleList= _iDropDownService.GetWebRoleList();
            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult User(UserModel userModel, string submitButton)
        {
            userModel.EmployeeList = _iDropDownService.GetEmployeeList();
            userModel.OutletList = _iDropDownService.GetOutletList();
            ViewBag.OutletList = userModel.OutletList;

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationUser(userModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(userModel);
                }
            }

            if (userModel.Id > 0)
            {
                var result = _iUserService.UpdateUser(userModel);
                if (result == -1)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(userModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iUserService.InsertUser(userModel);
                if (result == -1)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(userModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "User");
        }

        public ActionResult Delete(int id)
        {
            var deletedid = _iUserService.DeleteUser(id);

            return RedirectToAction(nameof(Index));
        }

        private string ValidationUser(UserModel userModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(userModel.Username))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            //if (string.IsNullOrEmpty(userModel.Price.ToString()) || userModel.Price == 0)
            //{
            //    ErrorString = _locService.GetLocalizedHtmlString("ValidPrice");
            //    return ErrorString;
            //}

            return ErrorString;
        }

    }
}
