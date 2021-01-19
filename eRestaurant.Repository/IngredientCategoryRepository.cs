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
    public class IngredientCategoryRepository : IIngredientCategoryRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public IngredientCategoryRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteIngredientCategory(int ingredientCategoryId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE ingredientCategory SET isDeleted= 1 WHERE Id = {ingredientCategoryId}";
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

        public List<IngredientCategoryModel> GetIngredientCategoryList()
        {
            List<IngredientCategoryModel> ingredientUnitModel = new List<IngredientCategoryModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,IngredientCategoryName,RawMaterialType,Notes,IsActive  FROM IngredientCategory WHERE IsDeleted = 0 " +
               "ORDER BY IngredientCategoryName ";

                ingredientUnitModel = con.Query<IngredientCategoryModel>(query).ToList();
            }

            return ingredientUnitModel;
        }

        public int InsertIngredientCategory(IngredientCategoryModel ingredientCategoryModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                int MaxId = commonRepository.GetMaxId("IngredientCategory");

                ingredientCategoryModel.RawMaterialType = (ingredientCategoryModel.RawMaterialType == 0) ? null : ingredientCategoryModel.RawMaterialType;
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO IngredientCategory (Id,IngredientCategoryName," +
                            "RawMaterialType,Notes, " +
                            "IsActive)" +
                            "VALUES (" + MaxId + ",@IngredientCategoryName," +
                            "@RawMaterialType,@Notes," +
                            "@IsActive); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, ingredientCategoryModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateIngredientCategory(IngredientCategoryModel ingredientCategoryModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE IngredientCategory SET IngredientCategoryName =@IngredientCategoryName," +
                            "RawMaterialType=@RawMaterialType,Notes = @Notes, " +
                            "IsActive = @IsActive " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, ingredientCategoryModel, sqltrans, 0, System.Data.CommandType.Text);

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
