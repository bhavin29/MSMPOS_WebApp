using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

using System.Collections.Generic;


namespace RocketPOS.Interface.Services
{
    public interface IVarientService
    {
        VarientModel GetAddonesById(int VarientId);
        List<VarientModel> GetVarientList();

        int InsertVarient(VarientModel VarientModel);

        int UpdateVarient(VarientModel VarientModel);

        int DeleteVarient(int VarientID);
    }
}
