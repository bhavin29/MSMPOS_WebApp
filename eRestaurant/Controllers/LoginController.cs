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
            try
            {
                if (!String.IsNullOrEmpty(userName))
                {
                    LoginModel loginModel = new LoginModel();
                    loginModel = _iLoginService.GetLogin(userName, password);
                    if (loginModel != null)
                    {
                        List<UserPageRolePermissionModel> userPageRolePermissionModels = _iLoginService.GetUserPageRolePermission(loginModel.WebRoleId);
                        if (userPageRolePermissionModels.Count > 0)
                        {
                            MergeRolePermission(userPageRolePermissionModels);
                        }
                        MergeLogin(loginModel);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "";
                        ViewBag.Validate = "Invalid Username or Password!!!";
                    }
                }
                SystemLogs.Register("Login.Index method called!!!");
            }
            catch (Exception ex)
            {
                SystemLogs.Register(ex.Message);
            }
            return View();
        }

        public void MergeLogin(LoginModel loginModel)
        {
            try
            {
                LoginInfo.Userid = loginModel.Id;
                LoginInfo.Username = loginModel.Username;
                LoginInfo.RoleTypeId = loginModel.RoleTypeId;
                LoginInfo.ClientName = loginModel.ClientName;
                LoginInfo.Address1 = loginModel.Address1;
                LoginInfo.Address2 = loginModel.Address2;
                LoginInfo.Email = loginModel.Email;
                LoginInfo.Phone = loginModel.Phone;
                LoginInfo.Logo = loginModel.Logo;
                LoginInfo.WebSite = loginModel.WebSite;
                LoginInfo.ReceiptPrefix = loginModel.ReceiptPrefix;
                LoginInfo.OrderPrefix = loginModel.OrderPrefix;
                LoginInfo.TimeZone = loginModel.TimeZone;
                LoginInfo.Header = loginModel.Header;
                LoginInfo.Footer = loginModel.Footer;
                LoginInfo.Footer1 = loginModel.Footer1;
                LoginInfo.Footer2 = loginModel.Footer2;
                LoginInfo.Footer3 = loginModel.Footer3;
                LoginInfo.Footer4 = loginModel.Footer4;
                LoginInfo.MainWindowSettings = loginModel.MainWindowSettings;
                LoginInfo.HeaderMarqueeText = loginModel.HeaderMarqueeText;
                LoginInfo.DeliveryList = loginModel.DeliveryList;
                LoginInfo.DiscountList = loginModel.DiscountList;
                LoginInfo.Powerby = loginModel.Powerby;
                LoginInfo.Lastname = loginModel.Lastname;
                LoginInfo.Firstname = loginModel.Firstname;
                LoginInfo.TaxInclusive = loginModel.TaxInclusive;
                LoginInfo.IsItemOverright = loginModel.IsItemOverright;
                LoginInfo.VATLabel = loginModel.VATLabel;
                LoginInfo.PINLabel = loginModel.PINLabel;
                LoginInfo.FromEmailAddress = loginModel.FromEmailAddress;
                LoginInfo.EmailDisplayName = loginModel.EmailDisplayName;
                LoginInfo.FromEmailPassword = loginModel.FromEmailPassword;
                LoginInfo.WebRoleId = loginModel.WebRoleId;
                LoginInfo.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
                LoginInfo.ToDate = DateTime.Now.ToString("dd/MM/yyyy"); 
                LoginInfo.FoodMenuId = -1;
                LoginInfo.CategoryId = -1;
                LoginInfo.OutletId = -1;
                LoginInfo.InvoiceTerms = loginModel.InvoiceTerms;
            }
            catch (Exception ex)
            {
                SystemLogs.Register(ex.Message);
            }
        }

        public void MergeRolePermission(List<UserPageRolePermissionModel> userPageRolePermissionModels)
        {
            UserRolePermissionModel.userRolePermissionModels = new List<UserRolePermissionList>();
            foreach (var item in userPageRolePermissionModels)
            {
                UserRolePermissionList userRolePermissionList = new UserRolePermissionList();
                userRolePermissionList.PageName = item.PageName;
                userRolePermissionList.Add = item.Add;
                userRolePermissionList.Edit = item.Edit;
                userRolePermissionList.Delete = item.Delete;
                userRolePermissionList.View = item.View;
                UserRolePermissionModel.userRolePermissionModels.Add(userRolePermissionList);
            }
        }
    }
}
