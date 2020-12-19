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
    public class VarientRepository : IVarientRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public VarientRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<VarientModel> GetVarientList()
        {
            List<VarientModel> VarientModel = new List<VarientModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT V.Id,VarientName,FoodId,FM.FoodMenuName,V.IsActive FROM Varient V INNER JOIN FoodMenu FM ON V.FoodId = FM.Id  WHERE V.IsDeleted = 0 " +
                            "ORDER BY VarientName ";
                VarientModel = con.Query<VarientModel>(query).ToList();
            }

            return VarientModel;
        }

        public int InsertVarient(VarientModel varientModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO Varient (VarientName,FoodId," +
                            "Price, " +
                            "IsActive)" +
                            "VALUES (@VarientName,@FoodId," +
                            "@Price," +
                            "@IsActive); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, varientModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateVarient(VarientModel varientModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE Varient SET VarientName =@VarientName,FoodId=@FoodId," +
                            "Price = @Price, " +
                            "IsActive = @IsActive " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, varientModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int DeleteVarient(int varientId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE Varient SET IsDeleted = 1 WHERE Id = {varientId};";
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
