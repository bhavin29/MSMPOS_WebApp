using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IFoodMenuCatagoryRepository
    {
        List<FoodMenuCatagoryModel> GetFoodMenuCatagoryList();

        int InsertFoodMenuCatagory(FoodMenuCatagoryModel foodCategoryModel);

        int UpdateFoodMenuCatagory(FoodMenuCatagoryModel foodCategoryModel);

        int DeleteFoodMenuCatagory(int foodCategoryId);
    }
}
