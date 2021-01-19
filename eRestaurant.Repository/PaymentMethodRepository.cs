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

namespace RocketPOS.Repository
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public PaymentMethodRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<PaymentMethodModel> GetPaymentMethodList()
        {
            List<PaymentMethodModel> PaymentMethodModel = new List<PaymentMethodModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,PaymentMethodName,IsBank,IsIntegration,IsActive FROM PaymentMethod WHERE IsDeleted = 0 " +
                            "ORDER BY PaymentMethodName ";
                PaymentMethodModel = con.Query<PaymentMethodModel>(query).ToList();
            }

            return PaymentMethodModel;
        }

        public int InsertPaymentMethod(PaymentMethodModel PaymentMethodModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                int MaxId = commonRepository.GetMaxId("PaymentMethod");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO PaymentMethod (Id,PaymentMethodName,IsBank,IsIntegration,IsActive)" +
                            "VALUES (" + MaxId + ",@PaymentMethodName, @IsBank,@IsIntegration,@IsActive); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, PaymentMethodModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
            }

            return result;
        }

        public int UpdatePaymentMethod(PaymentMethodModel PaymentMethodModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE PaymentMethod SET PaymentMethodName =@PaymentMethodName, IsBank=@IsBank,IsIntegration=@IsIntegration," +
                            "IsActive = @IsActive " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, PaymentMethodModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
            }
            return result;
        }

        public int DeletePaymentMethod(int paymentMethodId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE PaymentMethod SET IsDeleted = 1 WHERE Id = {paymentMethodId};";
                result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
            }
            return result;
        }
    }
}
