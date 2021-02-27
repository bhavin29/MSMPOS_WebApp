using System;
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
        int DeletePurchase(long purchaseId);
        List<PurchaseDetailsModel> GetPurchaseDetails(long purchaseId);
        List<PurchaseModel> GetPurchaseById(long purchaseId);
        int DeletePurchaseDetails(long purchaseDetailsId);
        string ReferenceNumber();
        List<PurchaseDetailsModel> GetPurchaseFoodMenuDetails(long purchaseId);
        List<PurchaseModel> GetPurchaseFoodMenuById(long purchaseId);
        List<PurchaseViewModel> GetPurchaseFoodMenuList();
        List<PurchaseViewModel> PurchaseFoodMenuListByDate(string fromDate, string toDate,int supplierId);
        string  InsertPurchaseFoodMenu(PurchaseModel purchaseModel);
        int UpdatePurchaseFoodMenu(PurchaseModel purchaseModel);
        string ReferenceNumberFoodMenu();
        decimal GetTaxByFoodMenuId(int foodMenuId);
        decimal GetFoodMenuLastPrice(int itemType,int foodMenuId);
        ClientModel GetClientDetail();
        int GetPurchaseIdByReferenceNo(string referenceNo);
        int ApprovePurchaseOrder(int id);

        List<PurchaseModel> GetViewPurchaseFoodMenuById(long purchaseId);
        List<PurchaseDetailsModel> GetViewPurchaseFoodMenuDetails(long purchaseId);
    }
}
