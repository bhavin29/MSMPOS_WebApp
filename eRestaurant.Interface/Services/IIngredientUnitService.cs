using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IIngredientUnitService
    {
        IngredientUnitModel GetIngredientUnitById(int ingredientUnitId);

        List<IngredientUnitModel> GetIngredientUnitList();

        int InsertIngredientUnit(IngredientUnitModel ingredientUnitModel);

        int UpdateIngredientUnit(IngredientUnitModel ingredientUnitModel);

        int DeleteIngredientUnit(int ingredientUnitId);
    }
}
