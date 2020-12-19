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
    public class VarientService : IVarientService
    {
        private readonly IVarientRepository _IVarientReportsitory;

        public VarientService(IVarientRepository iAddondRepository)
        {
            _IVarientReportsitory = iAddondRepository;
        }
        public VarientModel GetAddonesById(int VarientId)
        {
            return _IVarientReportsitory.GetVarientList().Where(x => x.Id == VarientId).FirstOrDefault();
        }

        public List<VarientModel> GetVarientList()
        {

            return _IVarientReportsitory.GetVarientList();
        }

        public int InsertVarient(VarientModel VarientModel)
        {
            return _IVarientReportsitory.InsertVarient(VarientModel);
        }

        public int UpdateVarient(VarientModel VarientModel)
        {
            return _IVarientReportsitory.UpdateVarient(VarientModel);
        }

        public int DeleteVarient(int VarientID)
        {
            return _IVarientReportsitory.DeleteVarient(VarientID);
        }

    }
}
