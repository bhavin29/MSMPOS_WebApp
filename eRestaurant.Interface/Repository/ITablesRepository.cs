using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface ITablesRpository
    {
        List<TablesModel> GetTablesList();

        int InsertTables(TablesModel TablesModel);

        int UpdateTables(TablesModel TablesModel);

        int DeleteTables(int TablesID);

    }
}
