using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
   public interface IIngredientRepository
    {
        List<IngredientModel> GetIngredientList();
        int InsertIngredient(IngredientModel ingredientModel);
        int UpdateIngredient(IngredientModel ingredientModel);
        int DeleteIngredient(int ingredientId);
    }
}
