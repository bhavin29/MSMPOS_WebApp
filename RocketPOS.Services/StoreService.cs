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
    public class StoreService :IStoreService
    {
        private readonly IStoreRepository _iStoreRepository;

        public StoreService(IStoreRepository iStoreRepository)
        {
            _iStoreRepository = iStoreRepository;
        }

       
        public List<StoreModel> GetStoreList()
        {

            return _iStoreRepository.GetStoreList();
        }

        public int InsertStore(StoreModel storeModel)
        {
            return _iStoreRepository.InsertStore(storeModel);
        }

        public int UpdateStore(StoreModel storeModel)
        {
            return _iStoreRepository.UpdateStore(storeModel);
        }

        public int DeleteStore(int storeId)
        {
            return _iStoreRepository.DeleteStore(storeId);
        }

        public StoreModel GetStoreById(int storeId)
        {
            return _iStoreRepository.GetStoreList().Where(x => x.Id == storeId).FirstOrDefault();
        }
    }
}
