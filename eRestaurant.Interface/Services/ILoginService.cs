﻿using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

using System.Collections.Generic;


namespace RocketPOS.Interface.Services
{
    public interface ILoginService
    {
        int GetLogin(string userName, string Password);
    }
}
