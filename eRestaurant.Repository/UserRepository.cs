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
    public class UserRepository : IUserRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public UserRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<UserModel> GetUserList()
        {
            List<UserModel> UserModel = new List<UserModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT U.Id,(E.Lastname + ' ' + E.Firstname) as EmployeeName,EmployeeId,OutletId,Username,Password,ThumbToken,RoleTypeId,LastLogin,LastLogout,IPAdress,Counter,U.IsActive,U.WebRoleId "+
                            "FROM [User] U INNER JOIN Employee E ON U.EmployeeId = E.Id WHERE U.IsDeleted = 0 " +
                            "ORDER BY UserName ";
                UserModel = con.Query<UserModel>(query).ToList();
            }

            return UserModel;
        }

        public int InsertUser(UserModel UserModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("[User]", "Username", UserModel.Username, UserModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                UserModel.RoleTypeId = (UserModel.RoleTypeId == 0) ? null : UserModel.RoleTypeId;
                int MaxId = commonRepository.GetMaxId("[User]");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                //LastLogin,LastLogout,

                var query = "INSERT INTO [User] (Id,EmployeeId,OutletId,Username,Password,ThumbToken,RoleTypeId,IPAdress,Counter,IsActive,WebRoleId) " +
                           "Values"+
                           "  (" + MaxId + ",@EmployeeId,@OutletId,@Username,@Password,@ThumbToken,@RoleTypeId,@IPAdress,@Counter,@IsActive,@WebRoleId); " +
                            "SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, UserModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("[User]");
                }
                else
                {
                    sqltrans.Rollback();
                }
            }

            return result;
        }

        public int UpdateUser(UserModel UserModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("[User]", "Username", UserModel.Username, UserModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                //LastLogin=@LastLogin,LastLogout=@LastLogout,
                var query = "UPDATE [User] SET EmployeeId=@EmployeeId,OutletId=@OutletId,Username=@Username,Password=@Password,ThumbToken=@ThumbToken," +
                    "RoleTypeId=@RoleTypeId,IPAdress=@IPAdress,Counter=@Counter,IsActive=@IsActive,WebRoleId=@WebRoleId " +
                     "WHERE Id = @Id;";
                result = con.Execute(query, UserModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("[User]");
                }
                else
                {
                    sqltrans.Rollback();
                }
            }
            return result;
        }

        public int DeleteUser(int UserId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE [User] SET IsDeleted = 1 WHERE Id = {UserId};";
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
