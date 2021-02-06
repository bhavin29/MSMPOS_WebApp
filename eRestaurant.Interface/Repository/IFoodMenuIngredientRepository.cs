using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IFoodMenuIngredientRepository
    {
        string GetUnitNameByIngredientId(int ingredientId);
        int InsertUpdateFoodMenuIngredient(FoodMenuIngredientModel foodMenuIngredientModel);

        FoodMenuIngredientModel GetFoodMenuIngredientList(int foodMenuId);
    }
}
