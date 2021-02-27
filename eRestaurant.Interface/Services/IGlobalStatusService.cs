using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IGlobalStatusService
    {
        List<GlobalStatusModel> GetGlobalStatusList();

        GlobalStatusModel GetGlobalStatusById(int id);

        int InsertGlobalStatus(GlobalStatusModel globalStatusModel);

        int UpdateGlobalStatus(GlobalStatusModel globalStatusModel);

        int DeleteGlobalStatus(int id);
    }
}
