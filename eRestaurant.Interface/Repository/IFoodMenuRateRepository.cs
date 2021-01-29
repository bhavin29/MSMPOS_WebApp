using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IFoodMenuRateRepository
    {
        List<FoodMenuRate> GetFoodMenuRateList(int foodCategoryId);
        int UpdateFoodMenuRateList(List<FoodMenuRate> foodMenuRates);
    }
}
