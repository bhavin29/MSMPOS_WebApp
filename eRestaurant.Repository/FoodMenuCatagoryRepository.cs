using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using RocketPOS.Interface;
using RocketPOS.Interface.Repository;
using RocketPOS.Framework;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Repository
{
    public class FoodMenuCatagoryRepository : IFoodMenuCatagoryRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public FoodMenuCatagoryRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteFoodMenuCatagory(int foodCategoryId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE FoodMenuCategory SET isDeleted= 1 WHERE Id = {foodCategoryId}";
                result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
            }
            return result;

        }

        public List<FoodMenuCatagoryModel> GetFoodMenuCatagoryList()
        {
            List<FoodMenuCatagoryModel> foodCategoryModel = new List<FoodMenuCatagoryModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,FoodMenuCategoryName,Notes,IsActive  FROM FoodMenuCategory WHERE IsDeleted = 0 " +
               "ORDER BY FoodMenuCategoryName ";

                foodCategoryModel = con.Query<FoodMenuCatagoryModel>(query).ToList();

            }

            return foodCategoryModel;
        }

        public int InsertFoodMenuCatagory(FoodMenuCatagoryModel foodCategoryModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO FoodMenuCategory (FoodMenuCategoryName," +
                            "Notes, " +
                            "IsActive)" +
                            "VALUES (@FoodMenuCategoryName," +
                            "@Notes," +
                            "@IsActive); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, foodCategoryModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public int UpdateFoodMenuCatagory(FoodMenuCatagoryModel foodCategoryModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE FoodMenuCategory SET FoodMenuCategoryName =@FoodMenuCategoryName," +
                            "Notes = @Notes, " +
                            "IsActive = @IsActive " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, foodCategoryModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
            }
            return result;
        }
    }
}
