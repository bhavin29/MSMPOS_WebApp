using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
namespace RocketPOS.Interface.Services
{
    public interface IFoodMenuCatagoryService
    {
        List<FoodMenuCatagoryModel> GetFoodCategoryList();

        FoodMenuCatagoryModel GetFoodCategoryById(int foodCategoryId);

        int InsertFoodMenuCatagory(FoodMenuCatagoryModel foodCategoryModel);

        int UpdateFoodMenuCatagory(FoodMenuCatagoryModel foodCategoryModel);

        int DeleteFoodMenuCatagory(int foodCategoryId);
    }
}
