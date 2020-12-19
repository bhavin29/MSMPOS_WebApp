using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IClientService
    {
        ClientModel GetAddonesById(int ClientId);
        List<ClientModel> GetClientList();

        int InsertClient(ClientModel ClientModel);

        int UpdateClient(ClientModel ClientModel);

        int DeleteClient(int ClientID);
    }
}
