using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

using System.Collections.Generic;


namespace RocketPOS.Interface.Services
{
    public interface IUserRoleRoleService
    {
        UserRoleModel GetAddonesById(int UserRoleId);
        List<UserRoleModel> GetUserRoleList();

        int InsertUserRole(UserRoleModel UserRoleModel);

        int UpdateUserRole(UserRoleModel UserRoleModel);

        int DeleteUserRole(int UserRoleID);
    }
}
