using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IWasteService
    {
        WasteModel GetWasteById(int wasteId);
        List<WasteModel> GetWasteList();

        int InsertWaste(WasteModel wasteModel);

        int UpdateWaste(WasteModel wasteModel);

        int DeleteWaste(int wasteId);
    }
}
