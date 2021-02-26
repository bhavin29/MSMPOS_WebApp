using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface ITaxService
    {
        List<TaxModel> GetTaxList();

        TaxModel GetTaxById(int id);

        int InsertTax(TaxModel taxModel);

        int UpdateTax(TaxModel taxModel);

        int DeleteTax(int id);
    }
}
