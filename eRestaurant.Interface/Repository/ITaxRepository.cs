using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface ITaxRepository
    {
        List<TaxModel> GetTaxList();
        int InsertTax(TaxModel taxModel);
        int UpdateTax(TaxModel taxModel);
        int DeleteTax(int id);
    }
}
