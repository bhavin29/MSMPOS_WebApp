using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IIngredientCategoryRepository
    {
        List<IngredientCategoryModel> GetIngredientCategoryList();

        int InsertIngredientCategory(IngredientCategoryModel ingredientCategoryModel);

        int UpdateIngredientCategory(IngredientCategoryModel ingredientCategoryModel);

        int DeleteIngredientCategory(int ingredientCategoryId);
    }
}
