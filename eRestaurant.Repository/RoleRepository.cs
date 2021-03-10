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
    public class RoleRepository : IRoleRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public RoleRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteRole(int id)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE WebRole SET isDeleted= 1,DateDeleted=GetUtcDate(),UserIdDeleted= " + LoginInfo.Userid + " WHERE Id = " + id;
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

        public List<RoleModel> GetRoleList()
        {
            List<RoleModel> roleModel = new List<RoleModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,WebRoleName,IsActive  FROM WebRole WHERE IsDeleted = 0 " +
               "ORDER BY WebRoleName ";

                roleModel = con.Query<RoleModel>(query).ToList();

            }

            return roleModel;
        }

        public int InsertRole(RoleModel roleModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("WebRole", "WebRoleName", roleModel.WebRoleName, roleModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                int MaxId = commonRepository.GetMaxId("WebRole");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO WebRole (Id,WebRoleName," +
                            "IsActive,UserIdInserted,DateInserted,IsDeleted)" +
                            "VALUES (" + MaxId + ",@WebRoleName," +
                            "@IsActive," + LoginInfo.Userid + ",GetUtcDate(),0); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, roleModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("WebRole");
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public int UpdateRole(RoleModel roleModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("WebRole", "WebRoleName", roleModel.WebRoleName, roleModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE WebRole SET WebRoleName =@WebRoleName," +
                            "IsActive = @IsActive, " +
                            "UserIdUpdated =  " + LoginInfo.Userid +
                            ",DateUpdated = GetUtcDate() " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, roleModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("WebRole");
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
