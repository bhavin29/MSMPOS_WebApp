using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IFoodMenuIngredientService
    {
        string GetUnitNameByIngredientId(int ingredientId);
        int InsertUpdateFoodMenuIngredient(FoodMenuIngredientModel foodMenuIngredientModel);
        FoodMenuIngredientModel GetFoodMenuIngredientList(int foodMenuId);
    }
}
