using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface ICommonService
    {
        int InsertErrorLog(ErrorModel errorModel);

        ClientModel GetEmailSettings();
        int UpdateEmailSettings(ClientModel clientModel);

        void GetPageWiseRoleRigths(string pageName);
    }
}
