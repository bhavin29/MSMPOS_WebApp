using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IGlobalStatusRepository
    {
        List<GlobalStatusModel> GetGlobalStatusList();

        int InsertGlobalStatus(GlobalStatusModel globalStatusModel);

        int UpdateGlobalStatus(GlobalStatusModel globalStatusModel);

        int DeleteGlobalStatus(int id);
    }
}
