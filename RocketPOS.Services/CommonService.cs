using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _iCommonRepository;
        public CommonService(ICommonRepository icommonRepository)
        {
            _iCommonRepository = icommonRepository;
        }

        public ClientModel GetEmailSettings()
        {
            return _iCommonRepository.GetEmailSettings();
        }

        public int InsertErrorLog(ErrorModel errorModel)
        {
            return _iCommonRepository.InsertErrorLog(errorModel);
        }

        public int UpdateEmailSettings(ClientModel clientModel)
        {
            return _iCommonRepository.UpdateEmailSettings(clientModel);
        }
    }
}
