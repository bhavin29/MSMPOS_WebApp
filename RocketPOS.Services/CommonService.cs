using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _iCommonRepository;
        public CommonService(ICommonRepository icommonRepository)
        {
            _iCommonRepository = icommonRepository;
        }

        public ClientModel GetEmailSettings()
        {
            return _iCommonRepository.GetEmailSettings();
        }

        public void GetPageWiseRoleRigths(string pageName)
        {
            UserRolePermissionForPage.Add = UserRolePermissionModel.userRolePermissionModels.SingleOrDefault(x => x.PageName == pageName).Add;
            UserRolePermissionForPage.Edit = UserRolePermissionModel.userRolePermissionModels.SingleOrDefault(x => x.PageName == pageName).Edit;
            UserRolePermissionForPage.Delete = UserRolePermissionModel.userRolePermissionModels.SingleOrDefault(x => x.PageName == pageName).Delete;
            UserRolePermissionForPage.View = UserRolePermissionModel.userRolePermissionModels.SingleOrDefault(x => x.PageName == pageName).View;
        }

        public int GetValidateReference(string TableName, string Rowid)
        {
            return _iCommonRepository.GetValidateReference(TableName, Rowid);
        }

        public int InsertErrorLog(ErrorModel errorModel)
        {
            return _iCommonRepository.InsertErrorLog(errorModel);
        }

        public int UpdateEmailSettings(ClientModel clientModel)
        {
            return _iCommonRepository.UpdateEmailSettings(clientModel);
        }
    }
}
