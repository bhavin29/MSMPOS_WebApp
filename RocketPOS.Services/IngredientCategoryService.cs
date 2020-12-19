using RocketPOS.Interface.Services;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RocketPOS.Services
{
    public class IngredientCategoryService : IIngredientCategoryService
    {
        private readonly IIngredientCategoryRepository _IIngredientCategoryRepository;

        public IngredientCategoryService(IIngredientCategoryRepository ingredientCategoryRepository)
        {
            _IIngredientCategoryRepository = ingredientCategoryRepository;
        }
        public int DeleteIngredientCategory(int ingredientCategoryId)
        {
            return _IIngredientCategoryRepository.DeleteIngredientCategory(ingredientCategoryId);
        }

        public IngredientCategoryModel GetIngredientCategoryById(int ingredientCategoryId)
        {
            return _IIngredientCategoryRepository.GetIngredientCategoryList().Where(x => x.Id == ingredientCategoryId).FirstOrDefault();
        }

        public List<IngredientCategoryModel> GetIngredientCategoryList()
        {
            return _IIngredientCategoryRepository.GetIngredientCategoryList();
        }

        public int InsertIngredientCategory(IngredientCategoryModel ingredientCategoryModel)
        {
            return _IIngredientCategoryRepository.InsertIngredientCategory(ingredientCategoryModel);
        }

        public int UpdateIngredientCategory(IngredientCategoryModel ingredientCategoryModel)
        {
            return _IIngredientCategoryRepository.UpdateIngredientCategory(ingredientCategoryModel);
        }
    }
}
