using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;



namespace RocketPOS.Interface.Services
{
    public interface IOutletService
    {
        OutletModel GetOutletById(int OutletId);
        List<OutletModel> GetOutletList();

        int InsertOutlet(OutletModel OutletModel);

        int UpdateOutlet(OutletModel OutletModel);

        int DeleteOutlet(int OutletID);
    }
}
