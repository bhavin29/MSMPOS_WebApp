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
    public class CustomerRepository : ICustomerRepository
    {

        private readonly IOptions<ReadConfig> _ConnectionString;

        public CustomerRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<CustomerModel> GetCustomerList()
        {
            List<CustomerModel> supplierModel = new List<CustomerModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id, CustomerTypeId, CustomerNumber, CustomerName,CustomerAddress1, CustomerAddress2,CustomerPhone,CustomerEmail,CustomerImage, IsActive,FavDeliveryAddress from Customer WHERE IsDeleted = 0 " +
                            "ORDER BY CustomerName ";
                supplierModel = con.Query<CustomerModel>(query).ToList();
            }

            return supplierModel;
        }

        public int InsertCustomer(CustomerModel customerModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO Customer (CustomerTypeId, CustomerNumber,CustomerName,CustomerAddress1, CustomerAddress2,CustomerPhone,CustomerEmail,CustomerImage, FavDeliveryAddress,IsActive)" +
                            "VALUES " +
                            "(@CustomerTypeId, @CustomerNumber,@CustomerName,@CustomerAddress1, @CustomerAddress2,@CustomerPhone,@CustomerEmail,@CustomerImage, @FavDeliveryAddress,@IsActive);" +
                            " SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, customerModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateCustomer(CustomerModel customerModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE Customer SET  CustomerTypeId=@CustomerTypeId, CustomerNumber=@CustomerNumber,CustomerName=@CustomerName," +
                            "CustomerAddress1=@CustomerAddress1, CustomerAddress2=@CustomerAddress2,CustomerPhone=@CustomerPhone," +
                            "CustomerEmail =@CustomerEmail,CustomerImage=@CustomerImage, FavDeliveryAddress=@FavDeliveryAddress, IsActive=@IsActive " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, customerModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int DeleteCustomer(int customerId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE Customer SET IsDeleted = 1 WHERE Id = {customerId};";
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
