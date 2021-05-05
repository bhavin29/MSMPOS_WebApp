using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface ISalesInvoiceRepository
    {
        int DeletePurchaseInvoice(long PurchaseInvoiceId);
        int DeletePurchaseInvoiceDetails(long PurchaseInvoiceDetailsId);
        List<SalesInvoiceDetailModel> GetPurchaseInvoiceFoodMenuDetails(long PurchaseInvoiceId);
        List<SalesInvoiceModel> GetPurchaseInvoiceFoodMenuById(long PurchaseInvoiceId);
        List<SalesInvoiceViewModel> GetPurchaseInvoiceFoodMenuList();
        List<SalesInvoiceViewModel> PurchaseInvoiceFoodMenuListByDate(string fromDate, string toDate, int supplierId, int storeId);

        int InsertPurchaseInvoiceFoodMenu(SalesInvoiceModel PurchaseInvoiceModel);
        int UpdatePurchaseInvoiceFoodMenu(SalesInvoiceModel PurchaseInvoiceModel);
        string ReferenceNumberFoodMenu();
        decimal GetTaxByFoodMenuId(int foodMenuId);
        List<SalesInvoiceModel> GetPurchaseInvoiceFoodMenuByPurchaseId(long purchaseId);
        List<SalesInvoiceDetailModel> GetPurchaseInvoiceFoodMenuDetailsPurchaseId(long purchaseId);
        int GetPurchaseIdByPOReference(string poReference);
        List<SalesInvoiceModel> GetPurchaseInvoiceById(long PurchaseInvoiceId);

        List<SalesInvoiceModel> GetSalesInvoiceReportById(long id);
        List<SalesInvoiceDetailModel> GetSalesInvoiceReportFoodMenuDetails(long id);
    }
}
