﻿using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IPurchaseInvoiceRepository
    {
        List<PurchaseInvoiceViewModel> GetPurchaseInvoiceList();
        int InsertPurchaseInvoice(PurchaseInvoiceModel PurchaseInvoiceModel);
        int UpdatePurchaseInvoice(PurchaseInvoiceModel PurchaseInvoiceModel);
        int DeletePurchaseInvoice(long PurchaseInvoiceId);
        List<PurchaseInvoiceDetailModel> GetPurchaseInvoiceDetails(long PurchaseInvoiceId);
        List<PurchaseInvoiceModel> GetPurchaseInvoiceById(long PurchaseInvoiceId);
        int DeletePurchaseInvoiceDetails(long PurchaseInvoiceDetailsId);
        string ReferenceNumber();
        List<PurchaseInvoiceDetailModel> GetPurchaseInvoiceFoodMenuDetails(long PurchaseInvoiceId);
        List<PurchaseInvoiceModel> GetPurchaseInvoiceFoodMenuById(long PurchaseInvoiceId);
        List<PurchaseInvoiceViewModel> GetPurchaseInvoiceFoodMenuList();
        int InsertPurchaseInvoiceFoodMenu(PurchaseInvoiceModel PurchaseInvoiceModel);
        int UpdatePurchaseInvoiceFoodMenu(PurchaseInvoiceModel PurchaseInvoiceModel);
        string ReferenceNumberFoodMenu();
        decimal GetTaxByFoodMenuId(int foodMenuId);
        decimal GetFoodMenuLastPrice(int foodMenuId);
    }
}