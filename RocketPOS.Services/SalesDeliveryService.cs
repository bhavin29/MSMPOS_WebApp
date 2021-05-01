using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class SalesDeliveryService : ISalesDeliveryService
    {
        private readonly ISalesDeliveryRepository _iSalesDeliveryRepository;

        public SalesDeliveryService(ISalesDeliveryRepository iSalesDeliveryRepository)
        {
            _iSalesDeliveryRepository = iSalesDeliveryRepository;
        }

        public int DeletePurchaseGRN(long purchaseId)//
        {
            return _iSalesDeliveryRepository.DeletePurchaseGRN(purchaseId);
        }

        public int DeletePurchaseGRNDetails(long PurchaseGRNDetailsId)//
        {
            return _iSalesDeliveryRepository.DeletePurchaseGRNDetails(PurchaseGRNDetailsId);
        }

        public List<SalesDeliveryViewModel> GetPurchaseGRNFoodMenuList()//
        {
            return _iSalesDeliveryRepository.GetPurchaseGRNFoodMenuList();
        }

        public List<SalesDeliveryViewModel> PurchaseGRNFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId)//
        {
            return _iSalesDeliveryRepository.PurchaseGRNFoodMenuListByDate(fromDate, toDate, customerId, storeId);
        }
        public int InsertPurchaseGRNFoodMenu(SalesDeliveryModel purchaseModel)//
        {
            return _iSalesDeliveryRepository.InsertPurchaseGRNFoodMenu(purchaseModel);
        }

        public int UpdatePurchaseGRNFoodMenu(SalesDeliveryModel purchaseModel)//
        {
            return _iSalesDeliveryRepository.UpdatePurchaseGRNFoodMenu(purchaseModel);
        }
        public string ReferenceNumberFoodMenu()//
        {
            return _iSalesDeliveryRepository.ReferenceNumberFoodMenu();
        }
        public decimal GetTaxByFoodMenuId(int foodMenuId)//
        {
            return _iSalesDeliveryRepository.GetTaxByFoodMenuId(foodMenuId);
        }
        public decimal GetFoodMenuLastPrice(int itemType, int foodMenuId)//
        {
            return _iSalesDeliveryRepository.GetFoodMenuLastPrice(itemType, foodMenuId);
        }
        public SalesDeliveryModel GetPurchaseGRNFoodMenuByPurchaseId(long purchaseId)//
        {
            var model = (from purchase in _iSalesDeliveryRepository.GetPurchaseGRNFoodMenuByPurchaseId(purchaseId).ToList()
                         select new SalesDeliveryModel()
                         {
                             Id = purchase.Id,
                             SalesId = purchase.SalesId,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             SalesDeliveryDate = purchase.SalesDeliveryDate,
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
                             VatableAmount = purchase.VatableAmount,
                             NonVatableAmount = purchase.NonVatableAmount
                         }).SingleOrDefault();
            if (model != null)
            {
                model.salesDeliveryDetails = (from purchasedetails in _iSalesDeliveryRepository.GetPurchaseGRNFoodMenuDetailsByPurchaseId(purchaseId)
                                            select new SalesDeliveryDetailModel()
                                            {
                                                SalesDeliveryId = purchasedetails.SalesDeliveryId,
                                                FoodMenuId = purchasedetails.FoodMenuId,
                                                SOQTY = purchasedetails.SOQTY,
                                                DeliveryQTY = purchasedetails.DeliveryQTY,
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

        public int GetPurchaseIdByPOReference(string poReference)//
        {
            return _iSalesDeliveryRepository.GetPurchaseIdByPOReference(poReference);
        }

        public SalesDeliveryModel GetPurchaseGRNFoodMenuById(long purchaseGRNId)//
        {
            SalesDeliveryModel purchaseModel = new SalesDeliveryModel();

            var model = (from purchase in _iSalesDeliveryRepository.GetPurchaseGRNFoodMenuById(purchaseGRNId).ToList()
                         select new SalesDeliveryModel()
                         {
                             Id = purchase.Id,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             SalesDeliveryDate = purchase.SalesDeliveryDate,
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
                             CustomerName = purchase.CustomerName,
                             VatableAmount = purchase.VatableAmount,
                             NonVatableAmount = purchase.NonVatableAmount
                         }).SingleOrDefault();
            if (model != null)
            {
                model.salesDeliveryDetails = (from purchasedetails in _iSalesDeliveryRepository.GetPurchaseGRNFoodMenuDetails(purchaseGRNId)
                                            select new SalesDeliveryDetailModel()
                                            {
                                                SalesDeliveryId = purchasedetails.SalesDeliveryId,
                                                FoodMenuId = purchasedetails.FoodMenuId,
                                                SOQTY = purchasedetails.SOQTY,
                                                DeliveryQTY = purchasedetails.DeliveryQTY,
                                                UnitPrice = purchasedetails.UnitPrice,
                                                GrossAmount = purchasedetails.GrossAmount,
                                                DiscountPercentage = purchasedetails.DiscountPercentage,
                                                DiscountAmount = purchasedetails.DiscountAmount,
                                                TaxAmount = purchasedetails.TaxAmount,
                                                TotalAmount = purchasedetails.TotalAmount,
                                                IngredientName = purchasedetails.IngredientName,
                                                FoodMenuName = purchasedetails.FoodMenuName,
                                                VatableAmount = purchasedetails.VatableAmount,
                                                NonVatableAmount = purchasedetails.NonVatableAmount
                                            }).ToList();
            }
            return model;
        }
    }
}
