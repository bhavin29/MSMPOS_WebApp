using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _iRoleRepository;

        public RoleService(IRoleRepository iRoleRepository)
        {
            _iRoleRepository = iRoleRepository;
        }

        public int DeleteRole(int id)
        {
            return _iRoleRepository.DeleteRole(id);
        }

        public RoleModel GetRoleById(int id)
        {
            return _iRoleRepository.GetRoleList().Where(x => x.Id == id).FirstOrDefault();
        }

        public List<RoleModel> GetRoleList()
        {
            return _iRoleRepository.GetRoleList();
        }

        public int InsertRole(RoleModel roleModel)
        {
            return _iRoleRepository.InsertRole(roleModel);
        }

        public int UpdateRole(RoleModel roleModel)
        {
            return _iRoleRepository.UpdateRole(roleModel);
        }
    }
}
