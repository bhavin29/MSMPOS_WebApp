using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IRoleService
    {
        List<RoleModel> GetRoleList();

        RoleModel GetRoleById(int id);

        int InsertRole(RoleModel roleModel);

        int UpdateRole(RoleModel roleModel);

        int DeleteRole(int id);
    }
}
