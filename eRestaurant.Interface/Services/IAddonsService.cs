using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IAddonsService
    {
        AddonsModel GetAddonesById(int addonsId);
        List<AddonsModel> GetAddonsList();

        int InsertAddons(AddonsModel addonsModel);

        int UpdateAddons(AddonsModel addonsModel);

        int DeleteAddons(int addonsID);
    }
}
