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
using RocketPOS.Framework;
namespace RocketPOS.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public LoginRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public LoginModel GetLogin(string userName, string Password)
        {
            LoginModel loginModel = new LoginModel();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"select U.Id,U.Username,U.RoletypeId,E.LastName,E.FirstName,C.ClientName,C.Address1,C.Address2,C.Email,C.Phone,C.Logo,C.WebSite,C.ReceiptPrefix,C.OrderPrefix,C.OpenTime, "+
                            "C.CloseTime,C.CurrencyId,C.TimeZone,C.Header,C.Footer,C.Footer1,C.Footer2,C.Footer3,C.Footer4, C.MainWindowSettings,C.HeaderMarqueeText,C.DeliveryList,C.DiscountList,C.Powerby,C.TaxInclusive, C.IsItemOverright,C.VATLabel,C.PINLabel " +
                            "From[User] U INNER JOIN Employee E On E.Id = U.EmployeeId CROSS JOIN Client C " +
                            "where[Username] = '" + userName +"' and[Password] = '"+ Password + "' and RoleTypeId in (1, 2); ";
                loginModel = con.QueryFirstOrDefault<LoginModel>(query, null, sqltrans, 0, System.Data.CommandType.Text);
                if (loginModel!=null)
                {
                    sqltrans.Commit();
                }
                else
                { sqltrans.Rollback(); }
            }
            return loginModel;
        }
    }
}
