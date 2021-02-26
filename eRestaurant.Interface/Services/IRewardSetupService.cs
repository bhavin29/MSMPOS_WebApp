using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IRewardSetupService
    {
        List<RewardSetupModel> GetRewardSetupList();

        RewardSetupModel GetRewardSetupById(int id);

        int InsertRewardSetup(RewardSetupModel rewardSetupModel);

        int UpdateRewardSetup(RewardSetupModel rewardSetupModel);

        int DeleteRewardSetup(int id);
    }
}
