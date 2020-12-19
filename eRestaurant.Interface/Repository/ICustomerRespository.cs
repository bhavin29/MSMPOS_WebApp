using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface ICustomerRepository
    {

        List<CustomerModel> GetCustomerList();

        int InsertCustomer(CustomerModel CustomerModel);

        int UpdateCustomer(CustomerModel CustomerModel);

        int DeleteCustomer(int CustomerID);
    }
}
