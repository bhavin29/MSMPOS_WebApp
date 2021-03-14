using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Master
{
    public class RoleController : Controller
    {
        private readonly ICommonService _iCommonService;
        private readonly IRoleService _iRoleService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public RoleController(IRoleService iRoleService, ICommonService iCommonService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iRoleService = iRoleService;
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index(int? noDelete)
        {
            List<RoleModel> roleModel = new List<RoleModel>();
            roleModel = _iRoleService.GetRoleList().ToList();
            if (noDelete != null)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Can not delete reference available.");
            }
            return View(roleModel);
        }

        public ActionResult Role(int? id)
        {
            RoleModel roleModel = new RoleModel();
            if (id > 0)
            {
                roleModel = _iRoleService.GetRoleById(Convert.ToInt32(id));
            }

            return View(roleModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Role(RoleModel roleModel)
        {

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationRole(roleModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(roleModel);
                }
            }

            if (roleModel.Id > 0)
            {
                var result = _iRoleService.UpdateRole(roleModel);
                if (result == -1)
                {
                    ModelState.AddModelError("WebRoleName", "Role Name already exists");
                    return View(roleModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iRoleService.InsertRole(roleModel);
                if (result == -1)
                {
                    ModelState.AddModelError("WebRoleName", "Role Name already exists");
                    return View(roleModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "Role");
        }

        public ActionResult Delete(int id)
        {
            int result = 0;
            result = _iCommonService.GetValidateReference("WebRole", id.ToString());
            if (result > 0)
            {
                return RedirectToAction(nameof(Index), new { noDelete = result });
            }
            else
            {
                var deletedid = _iRoleService.DeleteRole(id);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        private string ValidationRole(RoleModel roleModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(roleModel.WebRoleName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidRoleName");
                return ErrorString;
            }
            return ErrorString;
        }
    }
}
