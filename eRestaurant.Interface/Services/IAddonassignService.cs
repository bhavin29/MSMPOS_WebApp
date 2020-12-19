using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IAddonassignService
    {
        AddonAssignModel GetAddonesById(int AddonassignId);
        List<AddonAssignModel> GetAddonassignList();

        int InsertAddonassign(AddonAssignModel AddonassignModel);

        int UpdateAddonassign(AddonAssignModel AddonassignModel);

        int DeleteAddonassign(int AddonassignID);
    }
}