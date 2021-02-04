using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Services
{
    public class FoodMenuRateService : IFoodMenuRateService
    {
        private readonly IFoodMenuRateRepository _iFoodMenuRateRepository;
        public FoodMenuRateService(IFoodMenuRateRepository foodMenuRateRepository)
        {
            _iFoodMenuRateRepository = foodMenuRateRepository;
        }
        public List<FoodMenuRate> GetFoodMenuRateList(int foodCategoryId,int outletId)
        {
            return _iFoodMenuRateRepository.GetFoodMenuRateList(foodCategoryId, outletId);
        }

        public int UpdateFoodMenuRateList(List<FoodMenuRate> foodMenuRates)
        {
            return _iFoodMenuRateRepository.UpdateFoodMenuRateList(foodMenuRates);
        }
    }
}
