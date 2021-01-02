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
    public class OutletRegisterService :IOutletRegisterService
    {
        private readonly IOutletRegisterRepository _iOutletRegisterRepository;

        public OutletRegisterService(IOutletRegisterRepository iOutletRegisterRepostiry)
        {
            _iOutletRegisterRepository = iOutletRegisterRepostiry;
        }
        public OutletRegisterModel GetOutletRegisterById(int OutletRegisterId)
        {
            return _iOutletRegisterRepository.GetOutletRegisterList().Where(x => x.Id == OutletRegisterId).FirstOrDefault();
        }

        public int InsertOutletRegister(OutletRegisterModel OutletRegisterModel)
        {
            return _iOutletRegisterRepository.InsertOutletRegister(OutletRegisterModel);
        }

        public int UpdateOutletRegister(OutletRegisterModel OutletRegisterModel)
        {
            return _iOutletRegisterRepository.UpdateOutletRegister(OutletRegisterModel);
        }

        public int DeleteOutletRegister(int OutletRegisterId)
        {
            return _iOutletRegisterRepository.DeleteOutletRegister(OutletRegisterId);
        }

        public List<OutletRegisterModel> GetOutletRegisterList()
        {
            return _iOutletRegisterRepository.GetOutletRegisterList();
        }
    }
}
