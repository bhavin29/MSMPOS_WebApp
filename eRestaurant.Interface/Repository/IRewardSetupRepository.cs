using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IRewardSetupRepository
    {
        List<RewardSetupModel> GetRewardSetupList();

        int InsertRewardSetup(RewardSetupModel rewardSetupModel);

        int UpdateRewardSetup(RewardSetupModel rewardSetupModel);

        int DeleteRewardSetup(int id);
    }
}
