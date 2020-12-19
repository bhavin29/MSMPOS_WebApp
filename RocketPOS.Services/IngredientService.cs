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
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _iIngredientRepository;

        public IngredientService(IIngredientRepository iIngredientRepository)
        {
            _iIngredientRepository = iIngredientRepository;
        }
        
        public IngredientModel GetIngredientById(int ingredientId)
        {
            return _iIngredientRepository.GetIngredientList().Where(x=>x.Id == ingredientId).FirstOrDefault();
        }
        public List<IngredientModel> GetIngredientList()
        {
            return _iIngredientRepository.GetIngredientList();
        }
        public int InsertIngredient(IngredientModel ingredientModel)
        {
            return _iIngredientRepository.InsertIngredient(ingredientModel);
        }
        public int UpdateIngredient(IngredientModel ingredientModel)
        {
            return _iIngredientRepository.UpdateIngredient(ingredientModel);
        }
        public int DeleteIngredient(int ingredientId)
        {
            return _iIngredientRepository.DeleteIngredient(ingredientId);
        }

    }
}
