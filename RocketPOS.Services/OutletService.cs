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
    public class OutletService : IOutletService
    {
        private readonly IOutletRepository _iOutletRepository;

        public OutletService(IOutletRepository iOutletRepostiry)
        {
            _iOutletRepository = iOutletRepostiry;
        }
        public OutletModel GetOutletById(int outletId)
        {
            return _iOutletRepository.GetOutletList().Where(x => x.Id == outletId).FirstOrDefault();
        }

        public int InsertOutlet(OutletModel outletModel)
        {
            return _iOutletRepository.InsertOutlet(outletModel);
        }

        public int UpdateOutlet(OutletModel outletModel)
        {
            return _iOutletRepository.UpdateOutlet(outletModel);
        }

        public int DeleteOutlet(int outletId)
        {
            return _iOutletRepository.DeleteOutlet(outletId);
        }  

        List<OutletModel> IOutletService.GetOutletList()
        {
            return _iOutletRepository.GetOutletList();
        }
    }
}
