using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using RocketPOS.Interface.Services;
using RocketPOS.Interface.Repository;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RocketPOS.Services
{
    public class WasteIngredientService : IWasteIngredientService
    {
        private readonly IWasteIngredientRepository _iWasteIngredientRepository;

        public WasteIngredientService(IWasteIngredientRepository iWasteIngredientRepository)
        {
            _iWasteIngredientRepository = iWasteIngredientRepository;
        }

        public WasteIngredientModel GetWasteIngredientById(int WasteIngredientId)
        {
            return _iWasteIngredientRepository.GetWasteIngredientList().Where(x => x.Id == WasteIngredientId).FirstOrDefault();
        }
        public List<WasteIngredientModel> GetWasteIngredientList()
        {
            return _iWasteIngredientRepository.GetWasteIngredientList();
        }
        public int InsertWasteIngredient(WasteIngredientModel WasteIngredientModel)
        {
            return _iWasteIngredientRepository.InsertWasteIngredient(WasteIngredientModel);
        }
        public int UpdateWasteIngredient(WasteIngredientModel WasteIngredientModel)
        {
            return _iWasteIngredientRepository.UpdateWasteIngredient(WasteIngredientModel);
        }
        public int DeleteWasteIngredient(int WasteIngredientId)
        {
            return _iWasteIngredientRepository.DeleteWasteIngredient(WasteIngredientId);
        }

    }
}
