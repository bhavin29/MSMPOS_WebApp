using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IOutletRepository
    {

        List<OutletModel> GetOutletList();

        int InsertOutlet(OutletModel OutletModel);

        int UpdateOutlet(OutletModel OutletModel);

        int DeleteOutlet(int OutletID);

    }
}
