using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IOutletRegisterRepository
    {
        List<OutletRegisterModel> GetOutletRegisterList();

        int InsertOutletRegister(OutletRegisterModel OutletRegisterModel);

        int UpdateOutletRegister(OutletRegisterModel OutletRegisterModel);

        int DeleteOutletRegister(int OutletRegisterID);
    }
}
