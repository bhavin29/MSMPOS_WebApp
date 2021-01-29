using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IPurchaseService
    {
        PurchaseModel GetPurchaseById(long purchaseId);
        List<PurchaseViewModel> GetPurchaseList();
        int InsertPurchase(PurchaseModel purchaseModel);
        int UpdatePurchase(PurchaseModel purchaseModel);
        int DeletePurchase(long purchaseId);
        int DeletePurchaseDetails(long purchaseDetailsId);
        string ReferenceNumber();
        PurchaseModel GetPurchaseFoodMenuById(long purchaseId);
        List<PurchaseViewModel> GetPurchaseFoodMenuList();
        List<PurchaseViewModel> PurchaseFoodMenuListByDate(string fromDate, string toDate);
        int InsertPurchaseFoodMenu(PurchaseModel purchaseModel);
        int UpdatePurchaseFoodMenu(PurchaseModel purchaseModel);
        string ReferenceNumberFoodMenu();
        decimal GetTaxByFoodMenuId(int foodMenuId);
        decimal GetFoodMenuLastPrice(int foodMenuId);
        ClientModel GetClientDetail();
    }
}
