using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IWasteIngredientRepository
    {

        List<WasteListModel> GetWasteIngredientList();

        int InsertWasteIngredient(WasteListModel WasteIngredientModel);

        int UpdateWasteIngredient(WasteListModel WasteIngredientModel);

        int DeleteWasteIngredient(int WasteIngredientID);

    }
}
