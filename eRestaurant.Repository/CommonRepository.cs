using Dapper;
using Microsoft.Extensions.Options;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RocketPOS.Repository
{
    public class CommonRepository : ICommonRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public CommonRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }
        public int InsertErrorLog(ErrorModel errorModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO ErrorLog(MethodName," +
                  "ErrorPath, " +
                  "ErrorDetails," +
                  "UserId) " +
                  "VALUES(@MethodName," +
                  " @ErrorPath," +
                  " @ErrorDetails," +
                  "@UserId);" +
                  " SELECT CAST(SCOPE_IDENTITY() as INT); ";
                result = con.Execute(query, errorModel, sqltrans, 0, System.Data.CommandType.Text);
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

        public int GetMaxId(string TableName)
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                string query = "SELECT ISNULL(Max(Id),0)+1 FROM " + TableName.ToString();
                result = con.ExecuteScalar<string>(query);
            }
            return Int16.Parse(result);
        }
    }
}
