﻿using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IPurchaseGRNService
    {
        PurchaseGRNModel GetPurchaseGRNById(long purchaseGRNId);
        List<PurchaseGRNViewModel> GetPurchaseGRNList();
        int InsertPurchaseGRN(PurchaseGRNModel purchaseGRNModel);
        int UpdatePurchaseGRN(PurchaseGRNModel purchaseGRNModel);
        int DeletePurchaseGRN(long purchaseGRNId);
        int DeletePurchaseGRNDetails(long purchaseDetailsId);
        string ReferenceNumber();
        PurchaseGRNModel GetPurchaseGRNFoodMenuById(long purchaseGRNId);
        List<PurchaseGRNViewModel> GetPurchaseGRNFoodMenuList();
        List<PurchaseGRNViewModel> PurchaseGRNFoodMenuListByDate(string fromDate, string toDate, int supplierId, int storeId);
        int InsertPurchaseGRNFoodMenu(PurchaseGRNModel purchaseGRNModel);
        int UpdatePurchaseGRNFoodMenu(PurchaseGRNModel purchaseGRNModel);
        string ReferenceNumberFoodMenu();
        decimal GetTaxByFoodMenuId(int foodMenuId);
        decimal GetFoodMenuLastPrice(int itemType, int foodMenuId);
        PurchaseGRNModel GetPurchaseGRNFoodMenuByPurchaseId(long purchaseId);

        int GetPurchaseIdByPOReference(string poReference);

        PurchaseGRNModel GetViewPurchaseGRNFoodMenuById(long purchaseGRNId);
    }
}
