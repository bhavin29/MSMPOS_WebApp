using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IRolePermissionRepository
    {
        List<WebRolePageModel> GetWebRolePermissionList(int webRoleId);
        int UpdateRolePermissionList(List<WebRolePageModel> webRolePageModels);
    }
}
