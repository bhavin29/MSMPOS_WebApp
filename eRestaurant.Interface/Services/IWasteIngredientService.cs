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
        WasteListModel GetWasteIngredientById(int WasteIngredientId);
        List<WasteListModel> GetWasteIngredientList();

        int InsertWasteIngredient(WasteListModel WasteIngredientModel);

        int UpdateWasteIngredient(WasteListModel WasteIngredientModel);

        int DeleteWasteIngredient(int WasteIngredientID);
    }
}
