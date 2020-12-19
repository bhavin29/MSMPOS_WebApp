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
    public class FoodMenuCatagoryService :IFoodMenuCatagoryService
    {
        private readonly IFoodMenuCatagoryRepository iFoodMenuCatagoryRepository;

        public FoodMenuCatagoryService(IFoodMenuCatagoryRepository foodMenuCatagoryRepository)
        {
            iFoodMenuCatagoryRepository = foodMenuCatagoryRepository;
        }

        public int DeleteFoodMenuCatagory(int foodCategoryId)
        {
            return iFoodMenuCatagoryRepository.DeleteFoodMenuCatagory(foodCategoryId);
        }

        public FoodMenuCatagoryModel GetFoodCategoryById(int foodCategoryId)
        {
            return iFoodMenuCatagoryRepository.GetFoodMenuCatagoryList().Where(x => x.Id == foodCategoryId).FirstOrDefault();
        }

        public List<FoodMenuCatagoryModel> GetFoodCategoryList()
        {
            return iFoodMenuCatagoryRepository.GetFoodMenuCatagoryList();
        }

        public int InsertFoodMenuCatagory(FoodMenuCatagoryModel foodCategoryModel)
        {
            return iFoodMenuCatagoryRepository.InsertFoodMenuCatagory(foodCategoryModel);
        }

        public int UpdateFoodMenuCatagory(FoodMenuCatagoryModel foodCategoryModel)
        {
            return iFoodMenuCatagoryRepository.UpdateFoodMenuCatagory(foodCategoryModel);
        }
    }
}
