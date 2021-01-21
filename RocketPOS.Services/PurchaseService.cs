using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Repository;
using System.Linq;

namespace RocketPOS.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _iPurchaseRepository;

        public PurchaseService(IPurchaseRepository iPurchaseRepository)
        {
            _iPurchaseRepository = iPurchaseRepository;
        }

        public PurchaseModel GetPurchaseById(long purchaseId)
        {
            PurchaseModel purchaseModel = new PurchaseModel();

            var model = (from purchase in _iPurchaseRepository.GetPurchaseById(purchaseId).ToList()
                         select new PurchaseModel()
                         {
                             Id = purchase.Id,
                             ReferenceNo = purchase.ReferenceNo,
                             SupplierId = purchase.SupplierId,
                             StoreId = purchase.StoreId,
                             Date = purchase.Date,
                             GrandTotal = purchase.GrandTotal,
                             Due = purchase.Due,
                             Paid = purchase.Paid,
                             Notes = purchase.Notes
                         }).SingleOrDefault();
            if(model != null)
            {
                model.PurchaseDetails = (from purchasedetails in _iPurchaseRepository.GetPurchaseDetails(purchaseId)
                                      select new PurchaseDetailsModel()
                                      {
                                          PurchaseId = purchasedetails.PurchaseId,
                                          IngredientId = purchasedetails.IngredientId,
                                          Quantity = purchasedetails.Quantity,
                                          UnitPrice = purchasedetails.UnitPrice,
                                          Total = purchasedetails.Total,
                                          IngredientName = purchasedetails.IngredientName
                                      }).ToList();
            }
            return model;
        }
        public List<PurchaseViewModel> GetPurchaseList()
        {
            return _iPurchaseRepository.GetPurchaseList();
        }
        public int InsertPurchase(PurchaseModel purchaseModel)
        {
            return _iPurchaseRepository.InsertPurchase(purchaseModel);
        }
        public int UpdatePurchase(PurchaseModel purchaseModel)
        {
            return _iPurchaseRepository.UpdatePurchase(purchaseModel);
        }
        public int DeletePurchase(long purchaseId)
        {
            return _iPurchaseRepository.DeletePurchase(purchaseId);
        }

        public int DeletePurchaseDetails(long PurchaseDetailsId)
        {
            return _iPurchaseRepository.DeletePurchaseDetails(PurchaseDetailsId);
        }

        public long ReferenceNumber()
        {
            return _iPurchaseRepository.ReferenceNumber();
        }
    }
}
