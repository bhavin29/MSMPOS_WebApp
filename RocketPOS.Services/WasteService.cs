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
    public class WasteService : IWasteService
    {
        private readonly IWasteRepository _IWasteReportsitory;

        public WasteService(IWasteRepository iAddondRepository)
        {
            _IWasteReportsitory = iAddondRepository;
        }
        public WasteModel GetWasteById(int WasteId)
        {
            return _IWasteReportsitory.GetWasteList().Where(x => x.Id == WasteId).FirstOrDefault();
        }

        public List<WasteModel> GetWasteList()
        {

            return _IWasteReportsitory.GetWasteList();
        }

        public int InsertWaste(WasteModel WasteModel)
        {
            return _IWasteReportsitory.InsertWaste(WasteModel);
        }

        public int UpdateWaste(WasteModel WasteModel)
        {
            return _IWasteReportsitory.UpdateWaste(WasteModel);
        }

        public int DeleteWaste(int WasteID)
        {
            return _IWasteReportsitory.DeleteWaste(WasteID);
        }

    }
}
