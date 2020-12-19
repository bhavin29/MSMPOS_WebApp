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
    public class WasteIngredientRepository : IWasteIngredientRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public WasteIngredientRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<WasteIngredientModel> GetWasteIngredientList()
        {
            List<WasteIngredientModel> WasteIngredientModel = new List<WasteIngredientModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,WasteIngredientName,Price,IsActive FROM WasteIngredient WHERE IsDeleted = 0 " +
                            "ORDER BY WasteIngredientName ";
                WasteIngredientModel = con.Query<WasteIngredientModel>(query).ToList();
            }

            return WasteIngredientModel;
        }

        public int InsertWasteIngredient(WasteIngredientModel WasteIngredientModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO WasteIngredient (WasteIngredientName," +
                            "Price, " +
                            "IsActive)" +
                            "VALUES (@WasteIngredientName," +
                            "@Price," +
                            "@IsActive); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, WasteIngredientModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateWasteIngredient(WasteIngredientModel WasteIngredientModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE WasteIngredient SET WasteIngredientName =@WasteIngredientName," +
                            "Price = @Price, " +
                            "IsActive = @IsActive " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, WasteIngredientModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int DeleteWasteIngredient(int WasteIngredientId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE WasteIngredient SET IsDeleted = 1 WHERE Id = {WasteIngredientId};";
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
    }
}
