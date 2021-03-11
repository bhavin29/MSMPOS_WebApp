using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IRoleRepository
    {
        List<RoleModel> GetRoleList();

        int InsertRole(RoleModel roleModel);

        int UpdateRole(RoleModel roleModel);

        int DeleteRole(int id);
    }
}
