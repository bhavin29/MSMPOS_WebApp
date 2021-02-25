using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface ICommonRepository
    {
        int InsertErrorLog(ErrorModel errorModel);
        ClientModel GetEmailSettings();
        int UpdateEmailSettings(ClientModel clientModel);
    }
}
