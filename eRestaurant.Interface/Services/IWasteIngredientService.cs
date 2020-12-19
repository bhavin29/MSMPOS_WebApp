using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IWasteIngredientService
    {
        WasteIngredientModel GetWasteIngredientById(int WasteIngredientId);
        List<WasteIngredientModel> GetWasteIngredientList();

        int InsertWasteIngredient(WasteIngredientModel WasteIngredientModel);

        int UpdateWasteIngredient(WasteIngredientModel WasteIngredientModel);

        int DeleteWasteIngredient(int WasteIngredientID);
    }
}
