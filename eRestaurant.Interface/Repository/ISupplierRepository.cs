using System.Collections.Generic;
using RocketPOS.Models;


namespace RocketPOS.Interface.Repository
{
    public interface ISupplierRepository
    {
        List<SupplierModel> GetSupplierList();

        int InsertSupplier(SupplierModel SupplierModel);

        int UpdateSupplier(SupplierModel SupplierModel);

        int DeleteSupplier(int SupplierID);
    }
}
