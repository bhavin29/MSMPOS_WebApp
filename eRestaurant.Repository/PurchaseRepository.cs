using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using Dapper;
using RocketPOS.Interface.Repository;
using Microsoft.Extensions.Options;

namespace RocketPOS.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public PurchaseRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<PurchaseViewModel> GetPurchaseList()
        {
            List<PurchaseViewModel> purchaseViewModelList = new List<PurchaseViewModel>();
            return purchaseViewModelList;
        }
        public int InsertPurchase(PurchaseModel purchaseModel)
        {
            int result = 0;
            return result;

        }
        public int UpdatePurchase(PurchaseModel purchaseModel)
        {
            int result = 0;
            return result;
        }
        public int DeletePurchase(int Purchaseid)
        {
            int result = 0;
            return result;
        }
    }
}
