using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface  IAddonAssignRpository
    {
        List<AddonAssignModel> GetAddonAssignList();

        int InsertAddonAssign(AddonAssignModel addonAssignModel);

        int UpdateAddonAssign(AddonAssignModel addonAssignModel);

        int DeleteAddonAssign(int AddonAssignID);

    }
}
