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
        private readonly IFoodMenuRpository _IFoodMenuReportsitory;

        public FoodMenuService(IFoodMenuRpository iAddondRepository)
        {
            _IFoodMenuReportsitory = iAddondRepository;
        }

        public List<FoodMenuModel> GetFoodMenuList()
        {

            return _IFoodMenuReportsitory.GetFoodMenuList();
        }

        public int InsertFoodMenu(FoodMenuModel FoodMenuModel)
        {
            return _IFoodMenuReportsitory.InsertFoodMenu(FoodMenuModel);
        }

        public int UpdateFoodMenu(FoodMenuModel FoodMenuModel)
        {
            return _IFoodMenuReportsitory.UpdateFoodMenu(FoodMenuModel);
        }

        public int DeleteFoodMenu(int FoodMenuID)
        {
            return _IFoodMenuReportsitory.DeleteFoodMenu(FoodMenuID);
        }

        public FoodMenuModel GetFoodMenueById(int FoodMenuId)
        {
            return _IFoodMenuReportsitory.GetFoodMenuList().Where(x => x.Id == FoodMenuId).FirstOrDefault();
        }
    }
}
