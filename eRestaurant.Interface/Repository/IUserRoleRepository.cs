using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
namespace RocketPOS.Interface.Repository
{
    public interface IUserRoleRepository
    {
        List<UserModel> GetUserList();

        int InsertUser(UserModel userModel);

        int UpdateUser(UserModel userModel);

        int DeleteUser(int userID);

    }
}
