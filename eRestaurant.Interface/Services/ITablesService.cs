using System;
using System.Collections.Generic;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface ITablesService
    {
        TablesModel GetTablesById(int TablesId);
        List<TablesModel> GetTablesList();

        int InsertTables(TablesModel TablesModel);

        int UpdateTables(TablesModel TablesModel);

        int DeleteTables(int TablesId);
    }
}
