using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Services
{
    public class FoodMenuIngredientService : IFoodMenuIngredientService
    {
        private readonly IFoodMenuIngredientRepository iFoodMenuIngredientRepository;

        public FoodMenuIngredientService(IFoodMenuIngredientRepository foodMenuIngredientRepository)
        {
            iFoodMenuIngredientRepository = foodMenuIngredientRepository;
        }

        public FoodMenuIngredientModel GetFoodMenuIngredientList(int foodMenuId)
        {
            return iFoodMenuIngredientRepository.GetFoodMenuIngredientList(foodMenuId);
        }

        public string GetUnitNameByIngredientId(int ingredientId)
        {
            return iFoodMenuIngredientRepository.GetUnitNameByIngredientId(ingredientId);
        }

        public int InsertUpdateFoodMenuIngredient(FoodMenuIngredientModel foodMenuIngredientModel)
        {
            return iFoodMenuIngredientRepository.InsertUpdateFoodMenuIngredient(foodMenuIngredientModel);
        }
    }
}
