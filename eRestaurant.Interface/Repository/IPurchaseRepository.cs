﻿using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IPurchaseRepository
    {
        List<PurchaseViewModel> GetPurchaseList();
        int InsertPurchase(PurchaseModel purchaseModel);
        int UpdatePurchase(PurchaseModel purchaseModel);
        int DeletePurchase(long PurchaseId);
        List<PurchaseDetailsModel> GetPurchaseDetails(long purchaseId);
        List<PurchaseModel> GetPurchaseById(long purchaseId);
        int DeletePurchaseDetails(long PurchaseDetailsId);
        long ReferenceNumber();
    }
}
