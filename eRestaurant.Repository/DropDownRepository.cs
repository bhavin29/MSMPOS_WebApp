using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using RocketPOS.Interface.Repository;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Repository
{
    public class DropDownRepository : IDropDownRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public DropDownRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }
        public List<DropDownModel> GetIngredientCategoryList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,IngredientCategoryName as [Name] from IngredientCategory where IsActive = 1 AND IsDeleted = 0 Order by IngredientCategoryName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }
        public List<DropDownModel> GetUnitList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,UnitName as [Name] from Units where IsActive = 1 AND IsDeleted= 0 Order by UnitName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }
        public List<DropDownModel> GetOutletList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
                {
                    var query = "select Id,OutletName as [Name] from Outlet where IsActive = 1 AND IsDeleted= 0 order by OutletName";
                    dropDownModels = con.Query<DropDownModel>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetSupplierList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,SupplierName as [Name] from Supplier where IsActive = 1 AND IsDeleted= 0 Order by SupplierName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetStoreList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,StoreName as [Name],IsMainStore As Optional from Store where IsActive = 1 AND IsDeleted= 0 Order by StoreName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }
        public List<DropDownModel> GetFoodMenuCategoryList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
                {
                    var query = "select Id,FoodMenuCategoryName as [Name] from FoodMenuCategory where IsActive = 1 AND IsDeleted= 0 Order by FoodMenuCategoryName";
                    dropDownModels = con.Query<DropDownModel>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetIngredientList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                //var query = "select Id,IngredientName + ' [ ' + CONVERT(VARCHAR(20),SalesPrice ) + ' ]'  as [Name] from Ingredient where IsDeleted= 0 Order by IngredientName";
                var query = "select Id,IngredientName as [Name] from Ingredient where IsActive = 1 AND IsDeleted= 0 Order by IngredientName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetFoodMenuList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,FoodMenuName as [Name], SalesPrice as Optional from FoodMenu where IsActive = 1 AND IsDeleted= 0 Order by FoodMenuName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetEmployeeList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,(LastName + ' ' + FirstName) as [Name] from Employee where IsDeleted= 0 Order by LastName,FirstName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetUserList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,Username as [Name] from [User] where IsActive = 1 AND IsDeleted= 0 Order by Username";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }
        public List<DropDownModel> GetFoodMenuListBySupplier(int id)
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select FM.Id,FM.FoodMenuName as [Name], FM.SalesPrice as Optional from SupplierItem SI " +
                            "Inner Join FoodMenu FM ON FM.Id = SI.FoodMenuId " +
                            "where SI.SupplierId = " + id + " AND FM.IsActive = 1 AND FM.IsDeleted = 0 Order by FM.FoodMenuName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }
        public List<DropDownModel> GetFoodMenuListByFoodmenuType(int foodmenuType)
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,FoodMenuName as [Name], PurchasePrice as Optional from FoodMenu where  FoodmenuType=" + foodmenuType + "  and IsActive = 1 AND IsDeleted= 0 Order by FoodMenuName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }
        public List<DropDownModel> GetTaxList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,TaxName as [Name] from Tax   Order by TaxName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetFoodMenuListByCategory(int id)
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query=string.Empty;
                if (id != 0)
                {
                     query = "select Id,FoodMenuName as [Name], SalesPrice as Optional from FoodMenu where  FoodCategoryId=" + id + " And IsActive = 1 AND IsDeleted= 0 Order by FoodMenuName";
                }
                else
                {
                    query = "select Id,FoodMenuName as [Name], SalesPrice as Optional from FoodMenu where IsActive = 1 AND IsDeleted= 0 Order by FoodMenuName";
                }
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetProductionFormulaList(int foodmenuType)
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select Id,FormulaName AS [Name] From ProductionFormula Where IsDeleted=0 And foodmenuType="+ foodmenuType;
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }
        public List<DropDownModel> GetRawMaterialList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,RawMaterialName as [Name] from RawMaterial   Order by RawMaterialName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetAssetItemList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select Id,AssetItemName AS [Name] From AssetItem Where IsDeleted=0";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetCateringFoodMenuGlobalStatus()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select StatusName As [Name] from GlobalStatus where ModuleName='Catering_FoodMenu'";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetProductionFormulaFoodMenuList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Distinct F.Id ,F.FoodMenuName  As [Name] from ProductionFormulaFoodmenu PFF Inner join FoodMenu F On F.Id=PFF.FoodMenuId Where Foodmenutype=2 and PFF.IsDeleted=0";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }
    }
}
