using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IPurchaseInvoiceService
    {
        PurchaseInvoiceModel GetPurchaseInvoiceById(long purchaseInvoiceId);
        List<PurchaseInvoiceViewModel> GetPurchaseInvoiceList();
        int InsertPurchaseInvoice(PurchaseInvoiceModel purchaseInvoiceModel);
        int UpdatePurchaseInvoice(PurchaseInvoiceModel purchaseInvoiceModel);
        int DeletePurchaseInvoice(long purchaseId);
        int DeletePurchaseInvoiceDetails(long purchaseDetailsId);
        string ReferenceNumber();
        PurchaseInvoiceModel GetPurchaseInvoiceFoodMenuById(long purchaseInvoiceId);
        List<PurchaseInvoiceViewModel> GetPurchaseInvoiceFoodMenuList();
        int InsertPurchaseInvoiceFoodMenu(PurchaseInvoiceModel purchaseInvoiceModel);
        int UpdatePurchaseInvoiceFoodMenu(PurchaseInvoiceModel purchaseInvoiceModel);
        string ReferenceNumberFoodMenu();
        decimal GetTaxByFoodMenuId(int foodMenuId);
        decimal GetFoodMenuLastPrice(int foodMenuId);
        PurchaseInvoiceModel GetPurchaseInvoiceFoodMenuByPurchaseId(long purchaseId);

        int GetPurchaseIdByPOReference(string poReference);
    }
}
