using RocketPOS.Interface.Services;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RocketPOS.Services
{
    public class AddonsService : IAddonsService
    {
        private readonly IAddonsRepository _IAddonsReportsitory;

        public AddonsService(IAddonsRepository iAddondRepository)
        {
            _IAddonsReportsitory = iAddondRepository;
        }
        public AddonsModel GetAddonesById(int addonsId)
        {
            return _IAddonsReportsitory.GetAddonsList().Where(x => x.Id == addonsId).FirstOrDefault();
        }

        public int InsertAddons(AddonsModel addonsModel)
        {
            return _IAddonsReportsitory.InsertAddons(addonsModel);
        }

        public int UpdateAddons(AddonsModel addonsModel)
        {
            return _IAddonsReportsitory.UpdateAddons(addonsModel);
        }

        public int DeleteAddons(int addonsID)
        {
            return _IAddonsReportsitory.DeleteAddons(addonsID);
        }

        List<AddonsModel> IAddonsService.GetAddonsList()
        {
            return _IAddonsReportsitory.GetAddonsList();
        }
    }
}
