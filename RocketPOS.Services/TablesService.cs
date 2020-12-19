using RocketPOS.Interface.Services;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RocketPOS.Services
{
    public class TablesService : ITablesService
    {
        private readonly ITablesRpository _ITablesReportsitory;

        public TablesService(ITablesRpository iAddondRepository)
        {
            _ITablesReportsitory = iAddondRepository;
        }
        public List<TablesModel> GetTablesList()
        {

            return _ITablesReportsitory.GetTablesList();
        }

        public int InsertTables(TablesModel TablesModel)
        {
            return _ITablesReportsitory.InsertTables(TablesModel);
        }

        public int UpdateTables(TablesModel TablesModel)
        {
            return _ITablesReportsitory.UpdateTables(TablesModel);
        }

        public int DeleteTables(int TablesID)
        {
            return _ITablesReportsitory.DeleteTables(TablesID);
        }

        public TablesModel GetTablesById(int TablesId)
        {

            return _ITablesReportsitory.GetTablesList().Where(x => x.Id == TablesId).FirstOrDefault();
        }
    }
}
