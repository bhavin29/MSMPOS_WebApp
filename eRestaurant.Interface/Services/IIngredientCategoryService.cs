using System;
using System.Collections.Generic;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IIngredientCategoryService
    {
        IngredientCategoryModel GetIngredientCategoryById(int ingredientCategoryId);
        List<IngredientCategoryModel> GetIngredientCategoryList();

        int InsertIngredientCategory(IngredientCategoryModel ingredientCategoryModel);

        int UpdateIngredientCategory(IngredientCategoryModel ingredientCategoryModel);

        int DeleteIngredientCategory(int ingredientCategoryId);
    }
}
