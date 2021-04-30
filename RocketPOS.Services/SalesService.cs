using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class SalesService : ISalesService
    {
        private readonly ISalesRepository _iSalesRepository;

        public SalesService(ISalesRepository iSalesRepository)
        {
            _iSalesRepository = iSalesRepository;
        }

        public int DeletePurchase(long purchaseId)//
        {
            return _iSalesRepository.DeletePurchase(purchaseId);
        }
        public SalesModel GetPurchaseFoodMenuById(long purchaseId)//
        {
            SalesModel purchaseModel = new SalesModel();

            var model = (from purchase in _iSalesRepository.GetPurchaseFoodMenuById(purchaseId).ToList()
                         select new SalesModel()
                         {
                             Id = purchase.Id,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             Date = purchase.Date,
                             GrandTotal = purchase.GrandTotal,
                             DiscountAmount = purchase.DiscountAmount,
                             TaxAmount = purchase.TaxAmount,
                             Due = purchase.Due,
                             Paid = purchase.Paid,
                             Notes = purchase.Notes,
                             Status = purchase.Status,
                             DateInserted = purchase.DateInserted,
                             CustomerName = purchase.CustomerName,
                             SupplierAddress1 = purchase.SupplierAddress1,
                             SupplierAddress2 = purchase.SupplierAddress2,
                             SupplierPhone = purchase.SupplierPhone,
                             SupplierEmail = purchase.SupplierEmail,
                             GrossAmount = purchase.GrossAmount,
                             VatableAmount = purchase.VatableAmount,
                             NonVatableAmount = purchase.NonVatableAmount,
                         }).SingleOrDefault();
            if (model != null)
            {
                model.SalesDetails = (from purchasedetails in _iSalesRepository.GetPurchaseFoodMenuDetails(purchaseId)
                                         select new SalesDetailsModel()
                                         {
                                             SalesId = purchasedetails.SalesId,
                                             FoodMenuId = purchasedetails.FoodMenuId,
                                             Quantity = purchasedetails.Quantity,
                                             UnitPrice = purchasedetails.UnitPrice,
                                             DiscountAmount = purchasedetails.DiscountAmount,
                                             DiscountPercentage = purchasedetails.DiscountPercentage,
                                             TaxAmount = purchasedetails.TaxAmount,
                                             TaxPercentage = purchasedetails.TaxPercentage,
                                             Total = purchasedetails.Total,
                                             FoodMenuName = purchasedetails.FoodMenuName,
                                             ItemType = purchasedetails.ItemType,
                                             RowNumber = purchasedetails.RowNumber,
                                             VatableAmount = purchasedetails.VatableAmount,
                                             NonVatableAmount = purchasedetails.NonVatableAmount,
                                         }).ToList();
            }
            return model;
        }

        public List<SalesViewModel> GetPurchaseFoodMenuList()//
        {
            return _iSalesRepository.GetPurchaseFoodMenuList();
        }
        public int DeletePurchaseDetails(long PurchaseDetailsId)//
        {
            return _iSalesRepository.DeletePurchaseDetails(PurchaseDetailsId);
        }

        public string InsertPurchaseFoodMenu(SalesModel purchaseModel)//
        {
            return _iSalesRepository.InsertPurchaseFoodMenu(purchaseModel);
        }

        public int UpdatePurchaseFoodMenu(SalesModel purchaseModel)//
        {
            return _iSalesRepository.UpdatePurchaseFoodMenu(purchaseModel);
        }

        public string ReferenceNumberFoodMenu()//
        {
            return _iSalesRepository.ReferenceNumberFoodMenu();
        }

        public decimal GetTaxByFoodMenuId(int foodMenuId, int ItemType)
        {
            return _iSalesRepository.GetTaxByFoodMenuId(foodMenuId, ItemType);
        }

        public decimal GetFoodMenuLastPrice(int itemType, int foodMenuId)
        {
            return _iSalesRepository.GetFoodMenuLastPrice(itemType, foodMenuId);
        }

        public int ApprovePurchaseOrder(int id)//
        {
            return _iSalesRepository.ApprovePurchaseOrder(id);
        }

        public List<SalesViewModel> PurchaseFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId)
        {
            return _iSalesRepository.PurchaseFoodMenuListByDate(fromDate,  toDate,customerId, storeId);
        }
    }
}
