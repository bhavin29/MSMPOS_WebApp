using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface ILoginRepository
    {
        int GetLogin(string userName, string Password);
    }
}
