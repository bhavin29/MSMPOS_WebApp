using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface ISalesDeliveryRepository
    {
        int DeletePurchaseGRN(long PurchaseGRNId);//
        int DeletePurchaseGRNDetails(long PurchaseGRNDetailsId);//
        List<SalesDeliveryDetailModel> GetPurchaseGRNFoodMenuDetails(long PurchaseGRNId);
        List<SalesDeliveryModel> GetPurchaseGRNFoodMenuById(long PurchaseGRNId);//
        List<SalesDeliveryViewModel> GetPurchaseGRNFoodMenuList();//
        List<SalesDeliveryViewModel> PurchaseGRNFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId);//
        int InsertPurchaseGRNFoodMenu(SalesDeliveryModel PurchaseGRNModel);//
        int UpdatePurchaseGRNFoodMenu(SalesDeliveryModel PurchaseGRNModel);//
        string ReferenceNumberFoodMenu();//
        decimal GetTaxByFoodMenuId(int foodMenuId);//
        decimal GetFoodMenuLastPrice(int itemType, int foodMenuId);//
        List<SalesDeliveryModel> GetPurchaseGRNFoodMenuByPurchaseId(long purchaseId);//
        List<SalesDeliveryDetailModel> GetPurchaseGRNFoodMenuDetailsByPurchaseId(long purchaseId);//
        int GetPurchaseIdByPOReference(string poReference);//
        List<SalesDeliveryDetailModel> GetViewSalesDeliveryFoodMenuDetails(long PurchaseGRNId);
        List<SalesDeliveryModel> GetViewSalesDeliveryFoodMenuById(long PurchaseGRNId);
        List<SalesDeliveryModel> GetSalesDeliveryReportById(long id);
        List<SalesDeliveryDetailModel> GetSalesDeliveryReportFoodMenuDetails(long id);
    }
}
