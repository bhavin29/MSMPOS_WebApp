using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IFoodMenuRateService
    {
        List<FoodMenuRate> GetFoodMenuRateList(int foodCategoryId);
        int UpdateFoodMenuRateList(List<FoodMenuRate> foodMenuRates);
    }
}
