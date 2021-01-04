using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IOutletRegisterService
    {
        OutletRegisterModel GetOutletRegisterById(int OutletRegisterID);

        List<OutletRegisterModel> GetOutletRegisterList();

        int InsertOutletRegister(OutletRegisterModel OutletRegisterModel);

        int UpdateOutletRegister(OutletRegisterModel OutletRegisterModel);

        int DeleteOutletRegister(int OutletRegisterID);

    }
}
