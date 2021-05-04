using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface ISalesInvoiceService
    {
        //SalesInvoiceModel GetPurchaseInvoiceById(long purchaseInvoiceId);
        int DeletePurchaseInvoice(long purchaseId);
        int DeletePurchaseInvoiceDetails(long purchaseDetailsId);
        SalesInvoiceModel GetPurchaseInvoiceFoodMenuById(long purchaseInvoiceId);
        List<SalesInvoiceViewModel> GetPurchaseInvoiceFoodMenuList();
        List<SalesInvoiceViewModel> PurchaseInvoiceFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId);
        int InsertPurchaseInvoiceFoodMenu(SalesInvoiceModel purchaseInvoiceModel);
        int UpdatePurchaseInvoiceFoodMenu(SalesInvoiceModel purchaseInvoiceModel);
        string ReferenceNumberFoodMenu();
        decimal GetTaxByFoodMenuId(int foodMenuId);
        SalesInvoiceModel GetPurchaseInvoiceFoodMenuByPurchaseId(long purchaseId);
        int GetPurchaseIdByPOReference(string poReference);
        List<SalesInvoiceModel> GetPurchaseInvoiceById(long PurchaseInvoiceId);

        SalesInvoiceModel GetSalesInvoiceReportById(long id);

        string GetInvoiceHtmlString(SalesInvoiceModel salesInvoiceModel);
    }
}
