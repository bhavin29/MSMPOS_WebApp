using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public static class UserRolePermissionModel
    {
        public static List<UserRolePermissionList> userRolePermissionModels { get; set; }
    }

    public class UserRolePermissionList
    {
        public string PageName = string.Empty;
        public bool Add = false;
        public bool Edit = false;
        public bool Delete = false;
        public bool View = false;
    }

    public static class UserRolePermissionForPage
    {
        public static string PageName = string.Empty;
        public static bool Add = false;
        public static bool Edit = false;
        public static bool Delete = false;
        public static bool View = false;
    }
}
