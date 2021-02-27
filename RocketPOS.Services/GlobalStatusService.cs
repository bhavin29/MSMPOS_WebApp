using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class GlobalStatusService : IGlobalStatusService
    {
        private readonly IGlobalStatusRepository _iGlobalStatusRepository;

        public GlobalStatusService(IGlobalStatusRepository iGlobalStatusRepository)
        {
            _iGlobalStatusRepository = iGlobalStatusRepository;
        }

        public int DeleteGlobalStatus(int id)
        {
            return _iGlobalStatusRepository.DeleteGlobalStatus(id);
        }

        public GlobalStatusModel GetGlobalStatusById(int id)
        {
            return _iGlobalStatusRepository.GetGlobalStatusList().Where(x => x.Id == id).FirstOrDefault();
        }

        public List<GlobalStatusModel> GetGlobalStatusList()
        {
            return _iGlobalStatusRepository.GetGlobalStatusList();
        }

        public int InsertGlobalStatus(GlobalStatusModel globalStatusModel)
        {
            return _iGlobalStatusRepository.InsertGlobalStatus(globalStatusModel);
        }

        public int UpdateGlobalStatus(GlobalStatusModel globalStatusModel)
        {
            return _iGlobalStatusRepository.UpdateGlobalStatus(globalStatusModel);
        }
    }
}
