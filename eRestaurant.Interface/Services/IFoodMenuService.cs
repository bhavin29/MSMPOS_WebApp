using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IFoodMenuService
    {
        FoodMenuModel GetFoodMenueById(int foodMenuId);
        List<FoodMenuModel> GetFoodMenuList(int categoryid, int foodmenutype);

        int InsertFoodMenu(FoodMenuModel foodMenuModel);

        int UpdateFoodMenu(FoodMenuModel foodMenuModel);

        int DeleteFoodMenu(int foodMenuID);
    }
}
