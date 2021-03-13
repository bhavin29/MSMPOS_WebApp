using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

using System.Collections.Generic;


namespace RocketPOS.Interface.Services
{
    public interface IUserService
    {
        UserModel GetUserById(int userId);
        List<UserModel> GetUserList();

        int InsertUser(UserModel userModel);

        int UpdateUser(UserModel userModel);

        int DeleteUser(int userID);
        object DeleteOutlet(int id);
    }
}
