using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IFoodMenuRpository
    {
        List<FoodMenuModel> GetFoodMenuList();

        int InsertFoodMenu(FoodMenuModel foodMenuModel);

        int UpdateFoodMenu(FoodMenuModel foodMenuModel);

        int DeleteFoodMenu(int foodMenuID);

        List<FoodMenuModel> GetFoodMenuById(long foodMenuId);
        List<FoodManuDetailsModel> GetFoodMenuDetails(long foodMenuId);
    }
}
