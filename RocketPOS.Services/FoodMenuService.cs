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
    public class FoodMenuService : IFoodMenuService
    {
        private readonly IFoodMenuRpository _iFoodMenuReportsitory;

        public FoodMenuService(IFoodMenuRpository iAddondRepository)
        {
            _iFoodMenuReportsitory = iAddondRepository;
        }

        public List<FoodMenuModel> GetFoodMenuList()
        {

            return _iFoodMenuReportsitory.GetFoodMenuList();
        }

        public int InsertFoodMenu(FoodMenuModel FoodMenuModel)
        {
            return _iFoodMenuReportsitory.InsertFoodMenu(FoodMenuModel);
        }

        public int UpdateFoodMenu(FoodMenuModel FoodMenuModel)
        {
            return _iFoodMenuReportsitory.UpdateFoodMenu(FoodMenuModel);
        }

        public int DeleteFoodMenu(int FoodMenuID)
        {
            return _iFoodMenuReportsitory.DeleteFoodMenu(FoodMenuID);
        }

        public FoodMenuModel GetFoodMenueById(int foodMenuId)
        {
            List<FoodMenuModel> foodMenuModel = new List<FoodMenuModel>();
            FoodMenuModel model = new FoodMenuModel();

            model = _iFoodMenuReportsitory.GetFoodMenuById(foodMenuId).ToList().SingleOrDefault();
            if (model != null)
            {
                model.FoodMenuDetails = _iFoodMenuReportsitory.GetFoodMenuDetails(foodMenuId);
            }
            return model;
        }
    }
}
