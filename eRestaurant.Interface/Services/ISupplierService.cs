using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface ISupplierService
    {
        List<SupplierModel> GetSupplierList();

        SupplierModel GetSupplierById(int SupplierID);
        int InsertSupplier(SupplierModel SupplierModel);

        int UpdateSupplier(SupplierModel SupplierModel);

        int DeleteSupplier(int SupplierID);

    }
}
