using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
   public interface IIngredientService
    {
        IngredientModel GetIngredientById(int ingredientId);
        List<IngredientModel> GetIngredientList();
        int InsertIngredient(IngredientModel ingredientModel);
        int UpdateIngredient(IngredientModel ingredientModel);
        int DeleteIngredient(int ingredientId);
    }
}
