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
    public class IngredientRepository : IIngredientRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public IngredientRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<IngredientModel> GetIngredientList()
        {
            List<IngredientModel> ingredientModel = new List<IngredientModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select ing.id,ing.IngredientName as IngredientName, category.IngredientCategoryName as Category," +
                             "unit.UnitName as Unit, ing.IsActive , ing.IngredientCategoryId as CategoryId,ing.IngredientUnitId as UnitId" +
                             ",ing.PurchasePrice, ing.SalesPrice, ing.AlterQty,ing.Code" +
                             " from Ingredient as Ing inner join IngredientCategory as category " +
                             "on ing.IngredientCategoryId = category.id and category.IsDeleted = 0 inner join Units as unit " +
                             "on ing.IngredientUnitId = unit.Id and unit.IsDeleted = 0 where ing.IsDeleted = 0 order by ing.IngredientName asc";
                ingredientModel = con.Query<IngredientModel>(query).ToList();
            }
            return ingredientModel;
        }

        public int InsertIngredient(IngredientModel ingredientModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("Ingredient", "IngredientName", ingredientModel.IngredientName, ingredientModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                int MaxId = commonRepository.GetMaxId("Ingredient");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT into Ingredient(Id,IngredientName," +
                    "Code, " +
                    "IngredientCategoryId," +
                    "IngredientUnitId," +
                    "PurchasePrice," +
                    "SalesPrice," +
                    "AlterQty," +
                    "IsActive) " +
                    "VALUES(" + MaxId + ",@IngredientName," +
                    " @Code," +
                    " @CategoryId," +
                    "@UnitId," +
                    "@PurchasePrice," +
                   "@SalesPrice," +
                   "@AlterQty," + "@IsActive" + " ); SELECT CAST(SCOPE_IDENTITY() as INT); ";
                result = con.Execute(query, ingredientModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateIngredient(IngredientModel ingredientModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("Ingredient", "IngredientName", ingredientModel.IngredientName, ingredientModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update Ingredient set IngredientName = @IngredientName," +
                     "Code = @Code ," +
                     "IngredientCategoryId = @CategoryId," +
                     "IngredientUnitId = @UnitId," +
                     "PurchasePrice = @PurchasePrice," +
                     "SalesPrice = @SalesPrice," +
                     "AlterQty = @AlterQty," +
                     "IsActive = @IsActive WHERE Id = @Id ";
                result = con.Execute(query, ingredientModel, sqltrans, 0, System.Data.CommandType.Text); ;
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

        public int DeleteIngredient(int ingredientId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update Ingredient set IsDeleted = 1 where id = {ingredientId}";
                result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);
                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                { sqltrans.Rollback(); }
            }
            return result;
        }
    }
}
