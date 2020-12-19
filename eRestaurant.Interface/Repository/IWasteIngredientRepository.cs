using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IWasteIngredientRepository
    {

        List<WasteIngredientModel> GetWasteIngredientList();

        int InsertWasteIngredient(WasteIngredientModel WasteIngredientModel);

        int UpdateWasteIngredient(WasteIngredientModel WasteIngredientModel);

        int DeleteWasteIngredient(int WasteIngredientID);

    }
}
