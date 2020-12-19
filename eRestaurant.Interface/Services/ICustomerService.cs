using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface ICustomerService
    {
        CustomerModel GetCustomerById(int CustomerId);
        List<CustomerModel> GetCustomerList();

        int InsertCustomer(CustomerModel CustomerModel);

        int UpdateCustomer(CustomerModel CustomerModel);

        int DeleteCustomer(int CustomerID);
    }
}
