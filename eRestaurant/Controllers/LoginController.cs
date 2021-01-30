using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using RocketPOS.Framework;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RocketPOS.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _iLoginService;

        public LoginController(ILoginService loginService)
        {
            _iLoginService = loginService;
        }

        public ActionResult Index(string? userName, string? password)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                int result = _iLoginService.GetLogin(userName, password);
                if (result > 0)
                {
                    LoginInfo.Userid = result;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "";
                    ViewBag.Validate = "Invalid Username or Password!!!";
                }
            }
            return View();
        }

        //public ActionResult Login(string message)
        //{
        //    ViewBag.Message = message ?? "";
        //    return RedirectToAction("~/Views/Home/Index.cshtml");
        //}

    }
}
