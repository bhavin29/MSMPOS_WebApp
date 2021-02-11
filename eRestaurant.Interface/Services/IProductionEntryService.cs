using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IProductionEntryService
    {
        ProductionEntryModel GetProductionFormulaById(int id);
    }
}
