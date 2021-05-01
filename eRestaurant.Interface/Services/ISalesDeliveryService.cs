using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface ISalesDeliveryService
    {
        int DeletePurchaseGRN(long purchaseGRNId);//
        int DeletePurchaseGRNDetails(long purchaseDetailsId);//
        SalesDeliveryModel GetPurchaseGRNFoodMenuById(long purchaseGRNId);//
        List<SalesDeliveryViewModel> GetPurchaseGRNFoodMenuList();//
        List<SalesDeliveryViewModel> PurchaseGRNFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId);//
        int InsertPurchaseGRNFoodMenu(SalesDeliveryModel purchaseGRNModel);//
        int UpdatePurchaseGRNFoodMenu(SalesDeliveryModel purchaseGRNModel);//
        string ReferenceNumberFoodMenu();//
        decimal GetTaxByFoodMenuId(int foodMenuId);//
        decimal GetFoodMenuLastPrice(int itemType, int foodMenuId);//
        SalesDeliveryModel GetPurchaseGRNFoodMenuByPurchaseId(long purchaseId);//
        int GetPurchaseIdByPOReference(string poReference);//
    }
}
