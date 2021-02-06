using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IDropDownRepository
    {
        List<DropDownModel> GetIngredientCategoryList();
        List<DropDownModel> GetUnitList();
        List<DropDownModel> GetSupplierList();
        List<DropDownModel> GetIngredientList();
        List<DropDownModel> GetOutletList();
        List<DropDownModel> GetStoreList();
        List<DropDownModel> GetFoodMenuCategoryList();
        List<DropDownModel> GetFoodMenuList();
        List<DropDownModel> GetEmployeeList();
        List<DropDownModel> GetUserList();
        List<DropDownModel> GetFoodMenuListBySupplier(int id);
        List<DropDownModel> GetTaxList(); 
        List<DropDownModel> GetFoodMenuListByReadymade();

        List<DropDownModel> GetFoodMenuListByCategory(int id);
    }
}
