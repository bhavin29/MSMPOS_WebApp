﻿using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using RocketPOS.Interface.Repository;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using RocketPOS.Framework;
namespace RocketPOS.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public LoginRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int GetLogin(string userName, string Password)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"select Id from [User] where [Username] = '"+ userName +"' and [Password] = '"+ Password + "' and RoleTypeId in (1,2);";
                result = con.ExecuteScalar<int>(query, null, sqltrans, 0, System.Data.CommandType.Text);
                if (result > 0)
                {
                    LoginInfo.Userid = result;
                    sqltrans.Commit();
                }
                else
                { sqltrans.Rollback(); }
            }

            return result;
        }
    }
}
