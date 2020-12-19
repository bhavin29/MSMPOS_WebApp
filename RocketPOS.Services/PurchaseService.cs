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

        public PurchaseViewModel GetPurchaseById(long PurchaseId)
        {
            return _iPurchaseRepository.GetPurchaseList().Where(x => x.ReferenceNo == PurchaseId).FirstOrDefault();
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
        public int DeletePurchase(int purchaseid)
        {
            return _iPurchaseRepository.DeletePurchase(purchaseid);
        }
    }
}
