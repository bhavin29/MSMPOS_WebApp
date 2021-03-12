using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

using System.Collections.Generic;


namespace RocketPOS.Interface.Services
{
    public interface ILoginService
    {
        LoginModel GetLogin(string userName, string Password);
        List<UserPageRolePermissionModel> GetUserPageRolePermission(int webRoleId);
    }
}
