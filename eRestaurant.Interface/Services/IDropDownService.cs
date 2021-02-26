using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IDropDownService
    {
        List<SelectListItem> GetIngredientCategoryList();
        List<SelectListItem> GetUnitList();
        List<SelectListItem> GetOutletList();
        List<SelectListItem> GetStoreList();
        List<SelectListItem> GetFoodMenuCategoryList();
        List<SelectListItem> GetFoodMenuList();
        List<SelectListItem> GetEmployeeList();
        List<SelectListItem> GetSupplierList();
        List<SelectListItem> GetIngredientList();
        List<SelectListItem> GetUserList();
        List<SelectListItem> GetFoodMenuListBySupplier(int id);
        List<SelectListItem> GetFoodMenuListByFoodmenuType(int foodmenutype);
        List<SelectListItem> GetTaxList();

        List<SelectListItem> GetFoodMenuListByCategory(int id);
        List<SelectListItem> GetProductionFormulaList(int foodmenuType);
        List<SelectListItem> GetRawMaterialList();

        List<SelectListItem> GetAssetItemList();

        List<SelectListItem> GetCateringFoodMenuGlobalStatus();

        List<SelectListItem> GetProductionFormulaFoodMenuList();

        List<SelectListItem> GetAssetSizeList();
        List<SelectListItem> GetAssetLocationList();
        List<SelectListItem> GetGlobalStatusList();

    }
}
