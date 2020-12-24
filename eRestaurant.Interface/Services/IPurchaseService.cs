using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IPurchaseService
    {
        PurchaseModel GetPurchaseById(long PurchaseId);
        List<PurchaseViewModel> GetPurchaseList();
        int InsertPurchase(PurchaseModel purchaseModel);
        int UpdatePurchase(PurchaseModel purchaseModel);
        int DeletePurchase(int Purchaseid);
    }
}
