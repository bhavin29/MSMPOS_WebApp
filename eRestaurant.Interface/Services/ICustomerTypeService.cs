using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface ICustomerTypeService
    {
        CustomerTypeModel GetCustomerTypeById(int CustomerTypeId);
        List<CustomerTypeModel> GetCustomerTypeList();

        int InsertCustomerType(CustomerTypeModel CustomerTypeModel);

        int UpdateCustomerType(CustomerTypeModel CustomerTypeModel);

        int DeleteCustomerType(int CustomerTypeID);
    }
}
