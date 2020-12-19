using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IWasteRepository
    {
        List<WasteModel> GetWasteList();

         int InsertWaste(WasteModel WasteModel);

        int UpdateWaste(WasteModel WasteModel);

        int DeleteWaste(int WasteId);
    }
}
