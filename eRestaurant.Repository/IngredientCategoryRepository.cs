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
                var query = " SELECT IC.Id,IC.IngredientCategoryName,IC.RawMaterialId, R.RawMaterialName as RawMaterialType,IC.Notes,IC.IsActive  " +
                            " FROM IngredientCategory IC INNER JOIN RawMaterial R ON R.ID = IC.RawMaterialId WHERE IC.IsDeleted = 0 " +
                            " ORDER BY R.RawMaterialName,IC.IngredientCategoryName ";

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
                result = commonRepository.GetValidateUnique("IngredientCategory", "IngredientCategoryName", ingredientCategoryModel.IngredientCategoryName, ingredientCategoryModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                int MaxId = commonRepository.GetMaxId("IngredientCategory");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO IngredientCategory (Id,IngredientCategoryName," +
                            "RawMaterialId,Notes, " +
                            "IsActive)" +
                            "VALUES (" + MaxId + ",@IngredientCategoryName," +
                            "@RawMaterialId,@Notes," +
                            "@IsActive); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, ingredientCategoryModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("IngredientCategory");
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
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("IngredientCategory", "IngredientCategoryName", ingredientCategoryModel.IngredientCategoryName, ingredientCategoryModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE IngredientCategory SET IngredientCategoryName =@IngredientCategoryName," +
                            "RawMaterialId=@RawMaterialId,Notes = @Notes, " +
                            "IsActive = @IsActive " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, ingredientCategoryModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("IngredientCategory");
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
