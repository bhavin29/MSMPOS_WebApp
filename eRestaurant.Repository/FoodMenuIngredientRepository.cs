using Dapper;
using Microsoft.Extensions.Options;
using RocketPOS.Framework;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RocketPOS.Repository
{
    public class FoodMenuIngredientRepository : IFoodMenuIngredientRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public FoodMenuIngredientRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public FoodMenuIngredientModel GetFoodMenuIngredientList(int foodMenuId)
        {
            FoodMenuIngredientModel foodMenuIngredientModel = new FoodMenuIngredientModel();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select FMI.Id ,FMI.FoodMenuId,FMI.IngredientId,I.IngredientName,U.UnitName,FMI.Consumption From FoodMenuIngredient FMI " +
                            "Inner Join Ingredient I On I.Id = FMI.IngredientId Inner Join Units U On U.Id = I.IngredientUnitId Where FMI.IsDeleted=0 And FMI.FoodMenuId = " + foodMenuId;
                foodMenuIngredientModel.FoodMenuIngredientDetails = con.Query<FoodMenuIngredientDetails>(query).ToList();
            }
            return foodMenuIngredientModel;
        }

        public string GetUnitNameByIngredientId(int ingredientId)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select U.UnitName from Ingredient I Inner Join Units U On U.Id=I.IngredientUnitId Where I.IsDeleted=0 And I.Id=" + ingredientId;
                return con.QueryFirstOrDefault<string>(query);
            }
        }

        public int InsertUpdateFoodMenuIngredient(FoodMenuIngredientModel foodMenuIngredientModel)
        {
            int result = 0, detailResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                int MaxId = commonRepository.GetMaxId("FoodMenuIngredient");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                int deleteResult = 0;
                if (foodMenuIngredientModel.DeletedId != null)
                {
                    foreach (var item in foodMenuIngredientModel.DeletedId)
                    {
                        var deleteQuery = $"update FoodMenuIngredient set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                        deleteResult = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                    }
                }

                foreach (var item in foodMenuIngredientModel.FoodMenuIngredientDetails)
                {
                    var query = string.Empty;
                    if (item.Id > 0)
                    {

                        query = "update FoodMenuIngredient set " +
                           "FoodMenuId =" + item.FoodMenuId + "," +
                           "IngredientId =" + item.IngredientId + "," +
                           "Consumption = " + item.Consumption + "," +
                           "UserIdUpdated =" + LoginInfo.Userid + "," +
                           "DateUpdated=getutcdate(),IsDeleted = 0 Where Id=" + item.Id;

                    }
                    else
                    {
                        query = "INSERT INTO FoodMenuIngredient" +
                                      "  (Id " +
                                      "  ,FoodMenuId " +
                                      " ,IngredientId " +
                                      " ,Consumption" +
                                      " ,UserIdInserted" +
                                      " ,DateInserted,IsDeleted)   " +
                                       "VALUES           " +
                                       "("
                                       + MaxId.ToString() + "," + 
                                       + item.FoodMenuId + "," +
                                       item.IngredientId + "," +
                                       item.Consumption + "," +
                                       LoginInfo.Userid +
                                       ",GetUtcDate(),0);";

                        MaxId = MaxId + 1;
                    }
                    detailResult = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);
                }

                if (detailResult > 0)
                {
                    result = 1;
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
