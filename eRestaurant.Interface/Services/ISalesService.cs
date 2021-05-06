using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface ISalesService
    {
        int DeletePurchase(long purchaseId);//
        int DeletePurchaseDetails(long purchaseDetailsId);//
        SalesModel GetPurchaseFoodMenuById(long purchaseId);//
        List<SalesViewModel> GetPurchaseFoodMenuList();//
        List<SalesViewModel> PurchaseFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId);//
        string InsertPurchaseFoodMenu(SalesModel purchaseModel);//
        int UpdatePurchaseFoodMenu(SalesModel purchaseModel);//
        string ReferenceNumberFoodMenu();//
        decimal GetTaxByFoodMenuId(int foodMenuId, int ItemType);
        decimal GetFoodMenuLastPrice(int itemType, int foodMenuId);
        int ApprovePurchaseOrder(int id);//

        SalesModel GetViewSalesFoodMenuById(long purchaseId);
    }
}
