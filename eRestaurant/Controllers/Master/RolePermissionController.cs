using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Master
{
    public class RolePermissionController : Controller
    {
        private readonly ICommonService _iCommonService;
        private readonly IRolePermissionService _iRolePermissionService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly LocService _locService;

        public RolePermissionController(IRolePermissionService rolePermissionService, ICommonService iCommonService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iRolePermissionService = rolePermissionService;
            _iDropDownService = idropDownService;
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }
        public IActionResult Index(int? webRoleId)
        {
            _iCommonService.GetPageWiseRoleRigths("RolePermission");
            RolePermissionModel rolePermissionModel = new RolePermissionModel();
            rolePermissionModel.WebRoleList = _iDropDownService.GetWebRoleList();
            if (webRoleId!=null)
            {
                rolePermissionModel.WebRolePages = _iRolePermissionService.GetWebRolePermissionList(Convert.ToInt32(webRoleId));
                rolePermissionModel.WebRolesId = Convert.ToInt32(webRoleId);
            }
            return View(rolePermissionModel);
        }

        [HttpPost]
        public JsonResult UpdateRolePermissionList(string rolePermission)
        {
            int result = 0;
            List<WebRolePageModel> webRolePages = new List<WebRolePageModel>();
            webRolePages = JsonConvert.DeserializeObject<List<WebRolePageModel>>(rolePermission);
            result = _iRolePermissionService.UpdateRolePermissionList(webRolePages);
            return Json(new { result = result });
        }

    }
}
