using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class SalesInvoiceService : ISalesInvoiceService
    {
        private readonly ISalesInvoiceRepository _iSalesInvoiceRepository;

        public SalesInvoiceService(ISalesInvoiceRepository iSalesInvoiceRepository)
        {
            _iSalesInvoiceRepository = iSalesInvoiceRepository;
        }

        public int DeletePurchaseInvoice(long purchaseId)
        {
            return _iSalesInvoiceRepository.DeletePurchaseInvoice(purchaseId);
        }

        public int DeletePurchaseInvoiceDetails(long PurchaseInvoiceDetailsId)
        {
            return _iSalesInvoiceRepository.DeletePurchaseInvoiceDetails(PurchaseInvoiceDetailsId);
        }

        public SalesInvoiceModel GetPurchaseInvoiceFoodMenuById(long purchaseInvoiceId)
        {
            SalesInvoiceModel purchaseModel = new SalesInvoiceModel();

            var model = (from purchase in _iSalesInvoiceRepository.GetPurchaseInvoiceFoodMenuById(purchaseInvoiceId).ToList()
                         select new SalesInvoiceModel()
                         {
                             Id = purchase.Id,
                             SalesId = purchase.SalesId,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             SalesInvoiceDate = purchase.SalesInvoiceDate,
                             GrossAmount = purchase.GrossAmount,
                             TaxAmount = purchase.TaxAmount,
                             TotalAmount = purchase.TotalAmount,
                             PaidAmount = purchase.PaidAmount,
                             DueAmount = purchase.DueAmount,
                             DeliveryNoteNumber = purchase.DeliveryNoteNumber,
                             DeliveryDate = purchase.DeliveryDate,
                             DriverName = purchase.DriverName,
                             VehicleNumber = purchase.VehicleNumber,
                             Notes = purchase.Notes,
                             SOReferenceNo = purchase.SOReferenceNo,
                             SODate = purchase.SODate
                         }).SingleOrDefault();
            if (model != null)
            {
                model.SalesInvoiceDetails = (from purchasedetails in _iSalesInvoiceRepository.GetPurchaseInvoiceFoodMenuDetails(purchaseInvoiceId)
                                                select new SalesInvoiceDetailModel()
                                                {
                                                    SalesInvoiceId = purchasedetails.SalesInvoiceId,
                                                    IngredientId = purchasedetails.IngredientId,
                                                    FoodMenuId = purchasedetails.FoodMenuId,
                                                    SOQTY = purchasedetails.SOQTY,
                                                    InvoiceQty = purchasedetails.InvoiceQty,
                                                    UnitPrice = purchasedetails.UnitPrice,
                                                    GrossAmount = purchasedetails.GrossAmount,
                                                    DiscountPercentage = purchasedetails.DiscountPercentage,
                                                    DiscountAmount = purchasedetails.DiscountAmount,
                                                    TaxAmount = purchasedetails.TaxAmount,
                                                    TotalAmount = purchasedetails.TotalAmount,
                                                    IngredientName = purchasedetails.IngredientName,
                                                    FoodMenuName = purchasedetails.FoodMenuName
                                                }).ToList();
            }
            return model;
        }

        public List<SalesInvoiceViewModel> GetPurchaseInvoiceFoodMenuList()
        {
            return _iSalesInvoiceRepository.GetPurchaseInvoiceFoodMenuList();
        }

        public List<SalesInvoiceViewModel> PurchaseInvoiceFoodMenuListByDate(string fromDate, string toDate, int supplierId, int storeId)
        {
            return _iSalesInvoiceRepository.PurchaseInvoiceFoodMenuListByDate(fromDate, toDate, supplierId, storeId);
        }
        public int InsertPurchaseInvoiceFoodMenu(SalesInvoiceModel purchaseModel)
        {
            return _iSalesInvoiceRepository.InsertPurchaseInvoiceFoodMenu(purchaseModel);
        }

        public int UpdatePurchaseInvoiceFoodMenu(SalesInvoiceModel purchaseModel)
        {
            return _iSalesInvoiceRepository.UpdatePurchaseInvoiceFoodMenu(purchaseModel);
        }

        public string ReferenceNumberFoodMenu()
        {
            return _iSalesInvoiceRepository.ReferenceNumberFoodMenu();
        }
        public decimal GetTaxByFoodMenuId(int foodMenuId)
        {
            return _iSalesInvoiceRepository.GetTaxByFoodMenuId(foodMenuId);
        }

        public SalesInvoiceModel GetPurchaseInvoiceFoodMenuByPurchaseId(long purchaseId)
        {
            var model = (from purchase in _iSalesInvoiceRepository.GetPurchaseInvoiceFoodMenuByPurchaseId(purchaseId).ToList()
                         select new SalesInvoiceModel()
                         {
                             Id = purchase.Id,
                             SalesId = purchase.SalesId,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             SalesInvoiceDate = purchase.SalesInvoiceDate,
                             GrossAmount = purchase.GrossAmount,
                             TaxAmount = purchase.TaxAmount,
                             TotalAmount = purchase.TotalAmount,
                             PaidAmount = purchase.PaidAmount,
                             DueAmount = purchase.DueAmount,
                             DeliveryNoteNumber = purchase.DeliveryNoteNumber,
                             DeliveryDate = purchase.DeliveryDate,
                             DriverName = purchase.DriverName,
                             VehicleNumber = purchase.VehicleNumber,
                             Notes = purchase.Notes,
                             SalesStatus = purchase.SalesStatus,
                             SOReferenceNo = purchase.SOReferenceNo,
                             SODate = purchase.SODate,
                             VatableAmount = purchase.VatableAmount,
                             NonVatableAmount = purchase.NonVatableAmount
                         }).SingleOrDefault();
            if (model != null)
            {
                model.SalesInvoiceDetails = (from purchasedetails in _iSalesInvoiceRepository.GetPurchaseInvoiceFoodMenuDetailsPurchaseId(purchaseId)
                                                select new SalesInvoiceDetailModel()
                                                {
                                                    SalesInvoiceId = purchasedetails.SalesInvoiceId,
                                                    FoodMenuId = purchasedetails.FoodMenuId,
                                                    IngredientId = purchasedetails.IngredientId,
                                                    SOQTY = purchasedetails.SOQTY,
                                                    InvoiceQty = purchasedetails.InvoiceQty,
                                                    UnitPrice = purchasedetails.UnitPrice,
                                                    GrossAmount = purchasedetails.GrossAmount,
                                                    DiscountPercentage = purchasedetails.DiscountPercentage,
                                                    DiscountAmount = purchasedetails.DiscountAmount,
                                                    TaxAmount = purchasedetails.TaxAmount,
                                                    TotalAmount = purchasedetails.TotalAmount,
                                                    IngredientName = purchasedetails.IngredientName,
                                                    FoodMenuName = purchasedetails.FoodMenuName,
                                                    ItemType = purchasedetails.ItemType,
                                                    VatableAmount = purchasedetails.VatableAmount,
                                                    NonVatableAmount = purchasedetails.NonVatableAmount
                                                }).ToList();
            }
            return model;
        }

        public int GetPurchaseIdByPOReference(string poReference)
        {
            return _iSalesInvoiceRepository.GetPurchaseIdByPOReference(poReference);
        }

        public List<SalesInvoiceModel> GetPurchaseInvoiceById(long purchaseInvoiceId)
        {
            return _iSalesInvoiceRepository.GetPurchaseInvoiceById(purchaseInvoiceId);
        }
    }
}
