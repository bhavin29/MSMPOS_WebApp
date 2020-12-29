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
                var query = "select Id,IngredientCategoryName as [Name] from IngredientCategory where IsDeleted = 0 Order by IngredientCategoryName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }
        public List<DropDownModel> GetUnitList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,IngredientUnitName as [Name] from IngredientUnit where IsDeleted= 0 Order by IngredientUnitName";
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
                    var query = "select Id,OutletName as [Name] from Outlet where IsDeleted= 0 order by OutletName";
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
                var query = "select Id,SupplierName as [Name] from Supplier where IsDeleted= 0 Order by SupplierName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetStoreList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,StoreName as [Name] from Store where IsDeleted= 0 Order by StoreName";
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
                    var query = "select Id,FoodMenuCategoryName as [Name] from FoodMenuCategory where IsDeleted= 0 Order by FoodMenuCategoryName";
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
                var query = "select Id,IngredientName as [Name] from Ingredient where IsDeleted= 0 Order by IngredientName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

        public List<DropDownModel> GetFoodMenuList()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,FoodMenuName as [Name] from FoodMenu where IsDeleted= 0 Order by FoodMenuName";
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
    }
}
