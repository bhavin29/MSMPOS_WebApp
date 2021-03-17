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
        private readonly ICommonService _iCommonService;
        private readonly IUserService _iUserService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public UserController(IUserService userService, ICommonService iCommonService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iUserService = userService;
            _iDropDownService = idropDownService; _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index(int? noDelete)
        {
            _iCommonService.GetPageWiseRoleRigths("User");
            List<UserModel> userModel = new List<UserModel>();
            userModel = _iUserService.GetUserList().ToList();
            if (noDelete != null)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Can not delete reference available.");
            }
            return View(userModel);
        }

        public ActionResult User(int? id)
        {
            UserModel userModel = new UserModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true)
            {
                if (id > 0)
                {
                    int userId = Convert.ToInt32(id);
                    userModel = _iUserService.GetUserById(userId);
                }
                userModel.EmployeeList = _iDropDownService.GetEmployeeList();
                userModel.OutletList = _iDropDownService.GetOutletList();
                userModel.WebRoleList = _iDropDownService.GetWebRoleList();
                return View(userModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
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

            int result = 0;
            if (UserRolePermissionForPage.Delete == true)
            {
                result = _iCommonService.GetValidateReference("User", id.ToString());
                if (result > 0)
                {

                    return RedirectToAction(nameof(Index), new { noDelete = result });
                }

                else
                {
                    var deletedid = _iUserService.DeleteOutlet(id);
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
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
