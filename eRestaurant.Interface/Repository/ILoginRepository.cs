using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface ILoginRepository
    {
        LoginModel GetLogin(string userName, string Password);
        List<UserPageRolePermissionModel> GetUserPageRolePermission(int webRoleId);
    }
}
