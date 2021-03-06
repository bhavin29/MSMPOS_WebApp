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
    public class GlobalStatusRepository : IGlobalStatusRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public GlobalStatusRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteGlobalStatus(int id)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE GlobalStatus SET isDeleted= 1,DateDeleted=GetUtcDate(),UserIdDeleted= " + LoginInfo.Userid + " WHERE Id = " + id;
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

        public List<GlobalStatusModel> GetGlobalStatusList()
        {
            List<GlobalStatusModel> GlobalStatusModel = new List<GlobalStatusModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,ModuleName,StatusName,StatusCode  FROM GlobalStatus WHERE IsDeleted = 0 " +
               "ORDER BY ModuleName ";

                GlobalStatusModel = con.Query<GlobalStatusModel>(query).ToList();

            }

            return GlobalStatusModel;
        }

        public int InsertGlobalStatus(GlobalStatusModel GlobalStatusModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("GlobalStatus", "StatusName", GlobalStatusModel.StatusName, GlobalStatusModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                int MaxId = commonRepository.GetMaxId("GlobalStatus");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO GlobalStatus (Id,ModuleName," +
                            "StatusName, " +
                            "StatusCode,UserIdInserted,DateInserted,IsDeleted)" +
                            "VALUES (" + MaxId + ",@ModuleName," +
                            "@StatusName," +
                            "@StatusCode," + LoginInfo.Userid + ",GetUtcDate(),0); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, GlobalStatusModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("GlobalStatus");
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public int UpdateGlobalStatus(GlobalStatusModel GlobalStatusModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("GlobalStatus", "StatusName", GlobalStatusModel.StatusName, GlobalStatusModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE GlobalStatus SET ModuleName =@ModuleName," +
                            "StatusName = @StatusName, " +
                            "StatusCode = @StatusCode, " +
                            "UserIdUpdated =  " + LoginInfo.Userid +
                            ",DateUpdated = GetUtcDate() " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, GlobalStatusModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("GlobalStatus");
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
