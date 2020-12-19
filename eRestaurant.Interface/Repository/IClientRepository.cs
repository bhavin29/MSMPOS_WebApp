using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IClientRepository
    {
        List<ClientModel> GetClintList();

        int InsertClient(ClientModel ClientModel);

        int UpdateClient(ClientModel ClientModel);

        int DeleteClient(int ClientID);

    }
}
