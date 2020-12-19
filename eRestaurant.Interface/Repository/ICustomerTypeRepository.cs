using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface ICustomerTypeRpository
    {
        List<AddonAssignModel> GetCustomerTypeList();

        int InsertCustomerType(AddonAssignModel CustomerTypeModel);

        int UpdateCustomerType(AddonAssignModel CustomerTypeModel);

        int DeleteCustomerType(int CustomerTypeID);
    }
}
