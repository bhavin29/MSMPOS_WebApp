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
    public class WasteService : IWasteService
    {
        private readonly IWasteRepository _iWasteRepository;
       // private IWasteRepository _iWasteRepository;

        public WasteService(IWasteRepository iWasteRepository)
        {
            _iWasteRepository = iWasteRepository;
        }
    
        public List<WasteListModel> GetWasteList()
        {
            return _iWasteRepository.GetWasteList();
        }
        public int InsertWaste(WasteModel wasteModel)
        {
            return _iWasteRepository.InsertWaste(wasteModel);
        }
        public int UpdateWaste(WasteModel wasteModel)
        {
            return _iWasteRepository.UpdateWaste(wasteModel);
        }
        public int DeleteWaste(long wasteId)
        {
            return _iWasteRepository.DeleteWaste(wasteId);
        }

        public int DeleteWasteDetails(long WasteDetailsId)
        {
            return _iWasteRepository.DeleteWasteDetails(WasteDetailsId);
        }

        public long ReferenceNumber()
        {
            return _iWasteRepository.ReferenceNumber();
        }

        public List<WasteDetailModel> GetWasteDetails(long wasteId)
        {
            throw new System.NotImplementedException();
        }

        public WasteModel GetWasteById(long wasteId)
        {
            List<WasteModel> wasteModel = new List<WasteModel>();

            var model = (from waste in _iWasteRepository.GetWasteById(wasteId).ToList()
                         select new WasteModel()
                         {
                             Id = waste.Id,
                             //    ReferenceNo = waste.ReferenceNo,
                             //    SupplierId = waste.SupplierId,
                             //    Date = waste.Date,
                             //    GrandTotal = waste.GrandTotal,
                             //    Due = waste.Due,
                             //    Paid = waste.Paid
                             }).SingleOrDefault();
                if (model != null)
            {
                model.WasteDetail = (from wastedetails in _iWasteRepository.GetWasteDetails(wasteId)
                                      select new WasteDetailModel()
                                      {
                                          WasteId = wastedetails.WasteId,
                                          IngredientId = wastedetails.IngredientId,
                                          //Quantity = wastedetails.Quantity,
                                          //UnitPrice = wastedetails.UnitPrice,
                                          //Total = wastedetails.Total,
                                          //IngredientName = wastedetails.IngredientName
                                      }).ToList();
            }
            return model;

        }
    }
}
