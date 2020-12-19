using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IUserRepository
    {

        List<UserModel> GetUserList();

        int InsertUser(UserModel UserModel);

        int UpdateUser(UserModel UserModel);

        int DeleteUser(int UserID);
    }
}
