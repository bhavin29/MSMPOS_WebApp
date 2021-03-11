using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRolePermissionRepository iRolePermissionRepository;

        public RolePermissionService(IRolePermissionRepository rolePermissionRepository)
        {
            iRolePermissionRepository = rolePermissionRepository;
        }

        public List<WebRolePageModel> GetWebRolePermissionList(int webRoleId)
        {
            return iRolePermissionRepository.GetWebRolePermissionList(webRoleId); 
        }

        public int UpdateRolePermissionList(List<WebRolePageModel> webRolePageModels)
        {
            return iRolePermissionRepository.UpdateRolePermissionList(webRolePageModels);
        }
    }
}
