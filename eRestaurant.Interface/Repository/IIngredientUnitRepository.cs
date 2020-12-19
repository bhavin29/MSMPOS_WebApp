using System.Collections.Generic;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IIngredientUnitRepository
    {
        List<IngredientUnitModel> GetIngredientUnitList();

        int InsertIngredientUnit(IngredientUnitModel ingredientUnitModel);

        int UpdateIngredientUnit(IngredientUnitModel ingredientUnitModel);

        int DeleteIngredientUnit(int ingredientUnitId);
    }
}
