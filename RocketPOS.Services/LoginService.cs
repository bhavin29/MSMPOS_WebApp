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
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _ILoginReportsitory;

        public LoginService(ILoginRepository iAddondRepository)
        {
            _ILoginReportsitory = iAddondRepository;
        }

        public int GetLogin(string userNamem, string password)
        {
            return _ILoginReportsitory.GetLogin(userNamem, password);
        }
    }
}
