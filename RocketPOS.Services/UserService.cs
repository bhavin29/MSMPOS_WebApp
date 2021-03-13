using RocketPOS.Interface.Services;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RocketPOS.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _IUserReportsitory;

        public UserService(IUserRepository iAddondRepository)
        {
            _IUserReportsitory = iAddondRepository;
        }
        public UserModel GetUserById(int UserId)
        {
            return _IUserReportsitory.GetUserList().Where(x => x.Id == UserId).FirstOrDefault();
        }

        public List<UserModel> GetUserList()
        {

            return _IUserReportsitory.GetUserList();
        }

        public int InsertUser(UserModel UserModel)
        {
            return _IUserReportsitory.InsertUser(UserModel);
        }

        public int UpdateUser(UserModel UserModel)
        {
            return _IUserReportsitory.UpdateUser(UserModel);
        }

        public int DeleteUser(int UserID)
        {
            return _IUserReportsitory.DeleteUser(UserID);
        }

        public object DeleteOutlet(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
