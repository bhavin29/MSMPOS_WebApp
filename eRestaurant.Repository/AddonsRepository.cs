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
    public class AddonsRepository : IAddonsRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public AddonsRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<AddonsModel> GetAddonsList()
        {
            List<AddonsModel> addonsModel = new List<AddonsModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,AddonsName,Price,IsActive FROM Addons WHERE IsDeleted = 0 " +
                            "ORDER BY AddonsName ";
                addonsModel = con.Query<AddonsModel>(query).ToList();
            }

            return addonsModel;
        }

        public int InsertAddons(AddonsModel addonsModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO Addons (AddonsName," +
                            "Price, " +
                            "IsActive)" +
                            "VALUES (@AddonsName," +
                            "@Price," +
                            "@IsActive); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, addonsModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateAddons(AddonsModel addonsModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE Addons SET AddonsName =@AddonsName," +
                            "Price = @Price, " +
                            "IsActive = @IsActive " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, addonsModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int DeleteAddons(int addonsId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE Addons SET IsDeleted = 1 WHERE Id = {addonsId};";
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
