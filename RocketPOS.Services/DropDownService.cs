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
    public class DropDownService : IDropDownService
    {
        private readonly IDropDownRepository _dropDownRepository;

        public DropDownService(IDropDownRepository dropDownRepository)
        {
            _dropDownRepository = dropDownRepository;
        }

        public List<SelectListItem> GetIngredientCategoryList()
        {
            List<SelectListItem> lstCategory = new List<SelectListItem>();

            lstCategory.Add(new SelectListItem { Text = "Select", Value = String.Empty });
            List<DropDownModel> lstCategoryResult = _dropDownRepository.GetIngredientCategoryList().ToList();
            if (lstCategoryResult != null && lstCategoryResult.Count > 0)
            {
                foreach (var item in lstCategoryResult)
                {
                    lstCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }

            return lstCategory;
        }

        public List<SelectListItem> GetUnitList()
        {
            List<SelectListItem> lstCategory = new List<SelectListItem>();

            lstCategory.Add(new SelectListItem { Text = "Select", Value = String.Empty });
            List<DropDownModel> lstCategoryResult = _dropDownRepository.GetUnitList().ToList();
            if (lstCategoryResult != null && lstCategoryResult.Count > 0)
            {
                foreach (var item in lstCategoryResult)
                {
                    lstCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }

            return lstCategory;
        }

        public List<SelectListItem> GetOutletList()
        {
            List<SelectListItem> lstCategory = new List<SelectListItem>();

            lstCategory.Add(new SelectListItem { Text = "Select", Value = String.Empty });
            List<DropDownModel> lstCategoryResult = _dropDownRepository.GetOutletList().ToList();
            if (lstCategoryResult != null && lstCategoryResult.Count > 0)
            {
                foreach (var item in lstCategoryResult)
                {
                    lstCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }

            return lstCategory;
        }

        public List<SelectListItem> GetStoreList()
        {
            List<SelectListItem> lstCategory = new List<SelectListItem>();

            lstCategory.Add(new SelectListItem { Text = "Select", Value = String.Empty });
            List<DropDownModel> lstCategoryResult = _dropDownRepository.GetStoreList().ToList();
            if (lstCategoryResult != null && lstCategoryResult.Count > 0)
            {
                foreach (var item in lstCategoryResult)
                {
                    if (Convert.ToInt32(item.Optional) == 1)
                    {
                        lstCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        lstCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString()});
                    }
                    
                }
            }

            return lstCategory;
        }
      
        public List<SelectListItem> GetFoodMenuList()
        {
            List<SelectListItem> lstCategory = new List<SelectListItem>();

            lstCategory.Add(new SelectListItem { Text = "Select", Value = "0" });
            List<DropDownModel> lstCategoryResult = _dropDownRepository.GetFoodMenuList().ToList();
            if (lstCategoryResult != null && lstCategoryResult.Count > 0)
            {
                foreach (var item in lstCategoryResult)
                {
                    lstCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }

            return lstCategory;
        }

        public List<SelectListItem> GetFoodMenuCategoryList()
        {
            List<SelectListItem> lstCategory = new List<SelectListItem>();

            lstCategory.Add(new SelectListItem { Text = "Select", Value = String.Empty });
            List<DropDownModel> lstCategoryResult = _dropDownRepository.GetFoodMenuCategoryList().ToList();
            if (lstCategoryResult != null && lstCategoryResult.Count > 0)
            {
                foreach (var item in lstCategoryResult)
                {
                    lstCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }

            return lstCategory;
        }

        public List<SelectListItem> GetEmployeeList()
        {
            List<SelectListItem> lstCategory = new List<SelectListItem>();

            lstCategory.Add(new SelectListItem { Text = "Select", Value = String.Empty });
            List<DropDownModel> lstCategoryResult = _dropDownRepository.GetEmployeeList();
            if (lstCategoryResult != null && lstCategoryResult.Count > 0)
            {
                foreach (var item in lstCategoryResult)
                {
                    lstCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }

            return lstCategory;
        }

        public List<SelectListItem> GetSupplierList()
        {
            List<SelectListItem> supplierList = new List<SelectListItem>();

            supplierList.Add(new SelectListItem { Text = "Select", Value = String.Empty });
            List<DropDownModel> supplierListResult = _dropDownRepository.GetSupplierList().ToList();
            if (supplierListResult != null && supplierListResult.Count > 0)
            {
                foreach (var item in supplierListResult)
                {
                    supplierList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }
            return supplierList;
        }

        public List<SelectListItem> GetIngredientList()
        {
            List<SelectListItem> ingredientList = new List<SelectListItem>();

            ingredientList.Add(new SelectListItem { Text = "Select", Value = "0" });
            List<DropDownModel> ingredientListResult = _dropDownRepository.GetIngredientList().ToList();
            if (ingredientListResult != null && ingredientListResult.Count > 0)
            {
                foreach (var item in ingredientListResult)
                {
                    ingredientList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }
            return ingredientList;
        }

        public List<SelectListItem> GetUserList()
        {
            List<SelectListItem> ingredientList = new List<SelectListItem>();

            ingredientList.Add(new SelectListItem { Text = "Select", Value = String.Empty });
            List<DropDownModel> ingredientListResult = _dropDownRepository.GetUserList().ToList();
            if (ingredientListResult != null && ingredientListResult.Count > 0)
            {
                foreach (var item in ingredientListResult)
                {
                    ingredientList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }
            return ingredientList;
        }

        public List<SelectListItem> GetFoodMenuListBySupplier(int id)
        {
            List<SelectListItem> lstFoodMenu = new List<SelectListItem>();
            lstFoodMenu.Add(new SelectListItem { Text = "Select", Value = "0" });
            List<DropDownModel> lstFoodMenuResult = _dropDownRepository.GetFoodMenuListBySupplier(id).ToList();
            if (lstFoodMenuResult != null && lstFoodMenuResult.Count > 0)
            {
                foreach (var item in lstFoodMenuResult)
                {
                    lstFoodMenu.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }
            return lstFoodMenu;
        }
        
            public List<SelectListItem> GetFoodMenuListByReadymade()
        {
            List<SelectListItem> lstCategory = new List<SelectListItem>();

            lstCategory.Add(new SelectListItem { Text = "Select", Value = "0" });
            List<DropDownModel> lstCategoryResult = _dropDownRepository.GetFoodMenuListByReadymade().ToList();
            if (lstCategoryResult != null && lstCategoryResult.Count > 0)
            {
                foreach (var item in lstCategoryResult)
                {
                    lstCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }

            return lstCategory;
        }
        public List<SelectListItem> GetTaxList()
        {
            List<SelectListItem> lstCategory = new List<SelectListItem>();

            lstCategory.Add(new SelectListItem { Text = "Select", Value = "0" });
            List<DropDownModel> lstCategoryResult = _dropDownRepository.GetTaxList().ToList();
            if (lstCategoryResult != null && lstCategoryResult.Count > 0)
            {
                foreach (var item in lstCategoryResult)
                {
                    lstCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }

            return lstCategory;
        }

        public List<SelectListItem> GetFoodMenuListByCategory(int id)
        {
            List<SelectListItem> lstFoodMenu = new List<SelectListItem>();

            lstFoodMenu.Add(new SelectListItem { Text = "Select", Value = "0" });
            List<DropDownModel> lstFoodMenuResult = _dropDownRepository.GetFoodMenuListByCategory(id).ToList();
            if (lstFoodMenuResult != null && lstFoodMenuResult.Count > 0)
            {
                foreach (var item in lstFoodMenuResult)
                {
                    lstFoodMenu.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }

            return lstFoodMenu;
        }

        public List<SelectListItem> GetProductionFormulaList(int foodmenuType)
        {
            List<SelectListItem> lstProductionFormula = new List<SelectListItem>();

            lstProductionFormula.Add(new SelectListItem { Text = "Select", Value = "0" });
            List<DropDownModel> lstProductionFormulaResult = _dropDownRepository.GetProductionFormulaList(foodmenuType).ToList();
            if (lstProductionFormulaResult != null && lstProductionFormulaResult.Count > 0)
            {
                foreach (var item in lstProductionFormulaResult)
                {
                    lstProductionFormula.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }
            return lstProductionFormula;
        }
        public List<SelectListItem> GetRawMaterialList()
        {
            List<SelectListItem> lstCategory = new List<SelectListItem>();

            lstCategory.Add(new SelectListItem { Text = "Select", Value = "0" });
            List<DropDownModel> lstCategoryResult = _dropDownRepository.GetRawMaterialList().ToList();
            if (lstCategoryResult != null && lstCategoryResult.Count > 0)
            {
                foreach (var item in lstCategoryResult)
                {
                    lstCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }

            return lstCategory;
        }
    }
}
