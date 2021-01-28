using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IPurchaseGRNRepository
    {
        List<PurchaseGRNViewModel> GetPurchaseGRNList();
        int InsertPurchaseGRN(PurchaseGRNModel PurchaseGRNModel);
        int UpdatePurchaseGRN(PurchaseGRNModel PurchaseGRNModel);
        int DeletePurchaseGRN(long PurchaseGRNId);
        List<PurchaseGRNDetailModel> GetPurchaseGRNDetails(long PurchaseGRNId);
        List<PurchaseGRNModel> GetPurchaseGRNById(long PurchaseGRNId);
        int DeletePurchaseGRNDetails(long PurchaseGRNDetailsId);
        string ReferenceNumber();
        List<PurchaseGRNDetailModel> GetPurchaseGRNFoodMenuDetails(long PurchaseGRNId);
        List<PurchaseGRNModel> GetPurchaseGRNFoodMenuById(long PurchaseGRNId);
        List<PurchaseGRNViewModel> GetPurchaseGRNFoodMenuList();
        List<PurchaseGRNViewModel> PurchaseGRNFoodMenuListByDate(string fromDate, string toDate);
        int InsertPurchaseGRNFoodMenu(PurchaseGRNModel PurchaseGRNModel);
        int UpdatePurchaseGRNFoodMenu(PurchaseGRNModel PurchaseGRNModel);
        string ReferenceNumberFoodMenu();
        decimal GetTaxByFoodMenuId(int foodMenuId);
        decimal GetFoodMenuLastPrice(int foodMenuId);
        List<PurchaseGRNModel> GetPurchaseGRNFoodMenuByPurchaseId(long purchaseId);
        List<PurchaseGRNDetailModel> GetPurchaseGRNFoodMenuDetailsByPurchaseId(long purchaseId);

        int GetPurchaseIdByPOReference(string poReference);
    }
}
