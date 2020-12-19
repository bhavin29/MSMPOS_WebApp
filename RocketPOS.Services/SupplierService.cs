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
    public class SupplierService :ISupplierService
    {
        private readonly ISupplierRepository _iSupplierRepository;
        
        public SupplierService(ISupplierRepository iSupplierRepository)
        {
            _iSupplierRepository = iSupplierRepository;
        }

        public int DeleteSupplier(int SupplierID)
        {
            return _iSupplierRepository.DeleteSupplier(SupplierID);
        }

        public SupplierModel GetSupplierById(int SupplierID)
        {
            return _iSupplierRepository.GetSupplierList().Where(x => x.Id == SupplierID).FirstOrDefault();
        }

        public List<SupplierModel> GetSupplierList()
        {
            return _iSupplierRepository.GetSupplierList();
        }

        public int InsertSupplier(SupplierModel SupplierModel)
        {
            return _iSupplierRepository.InsertSupplier(SupplierModel);
        }

        public int UpdateSupplier(SupplierModel SupplierModel)
        {
            return _iSupplierRepository.UpdateSupplier(SupplierModel);
        }
    }
}
