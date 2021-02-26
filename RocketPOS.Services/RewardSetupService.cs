using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class RewardSetupService : IRewardSetupService
    {
        private readonly IRewardSetupRepository _iRewardSetupRepository;

        public RewardSetupService(IRewardSetupRepository iRewardSetupRepository)
        {
            _iRewardSetupRepository = iRewardSetupRepository;
        }

        public int DeleteRewardSetup(int id)
        {
            return _iRewardSetupRepository.DeleteRewardSetup(id);
        }

        public RewardSetupModel GetRewardSetupById(int id)
        {
            return _iRewardSetupRepository.GetRewardSetupList().Where(x => x.Id == id).FirstOrDefault();
        }

        public List<RewardSetupModel> GetRewardSetupList()
        {
            return _iRewardSetupRepository.GetRewardSetupList();
        }

        public int InsertRewardSetup(RewardSetupModel rewardSetupModel)
        {
            return _iRewardSetupRepository.InsertRewardSetup(rewardSetupModel);
        }

        public int UpdateRewardSetup(RewardSetupModel rewardSetupModel)
        {
            return _iRewardSetupRepository.UpdateRewardSetup(rewardSetupModel);
        }
    }
}
