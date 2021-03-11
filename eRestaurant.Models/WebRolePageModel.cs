using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class RolePermissionModel
    {
        public int WebRolesId { get; set; }
        public string WebRoleName { get; set; }
        public List<SelectListItem> WebRoleList { get; set; }
        public List<WebRolePageModel> WebRolePages { get; set; }
    }
    public class WebRolePageModel
    {
        public int Id { get; set; }
        public int PagesId { get; set; }
        public string PageName { get; set; }
        public int WebRolesId { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool View { get; set; }
    }
}
