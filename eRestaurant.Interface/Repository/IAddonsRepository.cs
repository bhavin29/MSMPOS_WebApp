using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IAddonsRepository
    {

        List<AddonsModel> GetAddonsList();

        int InsertAddons(AddonsModel addonsModel);

        int UpdateAddons(AddonsModel addonsModel);

        int DeleteAddons(int AddonsID);

    }
}
