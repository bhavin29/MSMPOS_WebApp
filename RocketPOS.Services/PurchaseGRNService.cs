using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Repository;
using System.Linq;

namespace RocketPOS.Services
{
    public class PurchaseGRNService : IPurchaseGRNService
    {
        private readonly IPurchaseGRNRepository _iPurchaseGRNRepository;

        public PurchaseGRNService(IPurchaseGRNRepository iPurchaseGRNRepository)
        {
            _iPurchaseGRNRepository = iPurchaseGRNRepository;
        }

        public PurchaseGRNModel GetPurchaseGRNById(long purchaseId)
        {
            PurchaseGRNModel purchaseModel = new PurchaseGRNModel();

            var model = (from purchase in _iPurchaseGRNRepository.GetPurchaseGRNById(purchaseId).ToList()
                         select new PurchaseGRNModel()
                         {
                             Id = purchase.Id,
                             ReferenceNo = purchase.ReferenceNo,
                             SupplierId = purchase.SupplierId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             PurchaseGRNDate = purchase.PurchaseGRNDate,
                             GrossAmount = purchase.GrossAmount,
                             TaxAmount = purchase.TaxAmount,
                             TotalAmount = purchase.TotalAmount,
                             PaidAmount = purchase.PaidAmount,
                             DueAmount = purchase.DueAmount,
                             DeliveryNoteNumber = purchase.DeliveryNoteNumber,
                             DeliveryDate = purchase.DeliveryDate,
                             DriverName = purchase.DriverName,
                             VehicleNumber = purchase.VehicleNumber,
                             Notes = purchase.Notes
                         }).SingleOrDefault();
            if (model != null)
            {
                model.PurchaseGRNDetails = (from purchasedetails in _iPurchaseGRNRepository.GetPurchaseGRNDetails(purchaseId)
                                         select new PurchaseGRNDetailModel()
                                         {
                                             PurchaseGRNId = purchasedetails.PurchaseGRNId,
                                             IngredientId = purchasedetails.IngredientId,
                                             POQTY = purchasedetails.POQTY,
                                             GRNQTY = purchasedetails.GRNQTY,
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
        public List<PurchaseGRNViewModel> GetPurchaseGRNList()
        {
            return _iPurchaseGRNRepository.GetPurchaseGRNList();
        }
        public int InsertPurchaseGRN(PurchaseGRNModel purchaseModel)
        {
            return _iPurchaseGRNRepository.InsertPurchaseGRN(purchaseModel);
        }
        public int UpdatePurchaseGRN(PurchaseGRNModel purchaseModel)
        {
            return _iPurchaseGRNRepository.UpdatePurchaseGRN(purchaseModel);
        }
        public int DeletePurchaseGRN(long purchaseId)
        {
            return _iPurchaseGRNRepository.DeletePurchaseGRN(purchaseId);
        }

        public int DeletePurchaseGRNDetails(long PurchaseGRNDetailsId)
        {
            return _iPurchaseGRNRepository.DeletePurchaseGRNDetails(PurchaseGRNDetailsId);
        }

        public string ReferenceNumber()
        {
            return _iPurchaseGRNRepository.ReferenceNumber();
        }

        public PurchaseGRNModel GetPurchaseGRNFoodMenuById(long purchaseGRNId)
        {
            PurchaseGRNModel purchaseModel = new PurchaseGRNModel();

            var model = (from purchase in _iPurchaseGRNRepository.GetPurchaseGRNFoodMenuById(purchaseGRNId).ToList()
                         select new PurchaseGRNModel()
                         {
                             Id = purchase.Id,
                             ReferenceNo = purchase.ReferenceNo,
                             SupplierId = purchase.SupplierId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             PurchaseGRNDate = purchase.PurchaseGRNDate,
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
                             SupplierName= purchase.SupplierName
                         }).SingleOrDefault();
            if (model != null)
            {
                model.PurchaseGRNDetails = (from purchasedetails in _iPurchaseGRNRepository.GetPurchaseGRNFoodMenuDetails(purchaseGRNId)
                                         select new PurchaseGRNDetailModel()
                                         {
                                             PurchaseGRNId = purchasedetails.PurchaseGRNId,
                                             FoodMenuId = purchasedetails.FoodMenuId,
                                             POQTY = purchasedetails.POQTY,
                                             GRNQTY = purchasedetails.GRNQTY,
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

        public List<PurchaseGRNViewModel> GetPurchaseGRNFoodMenuList()
        {
            return _iPurchaseGRNRepository.GetPurchaseGRNFoodMenuList();
        }

        public List<PurchaseGRNViewModel> PurchaseGRNFoodMenuListByDate(string fromDate, string toDate, int supplierId)
        {
            return _iPurchaseGRNRepository.PurchaseGRNFoodMenuListByDate(fromDate, toDate, supplierId);
        }
        public int InsertPurchaseGRNFoodMenu(PurchaseGRNModel purchaseModel)
        {
            return _iPurchaseGRNRepository.InsertPurchaseGRNFoodMenu(purchaseModel);
        }

        public int UpdatePurchaseGRNFoodMenu(PurchaseGRNModel purchaseModel)
        {
            return _iPurchaseGRNRepository.UpdatePurchaseGRNFoodMenu(purchaseModel);
        }

        public string ReferenceNumberFoodMenu()
        {
            return _iPurchaseGRNRepository.ReferenceNumberFoodMenu();
        }
        public decimal GetTaxByFoodMenuId(int foodMenuId)
        {
            return _iPurchaseGRNRepository.GetTaxByFoodMenuId(foodMenuId);
        }

        public decimal GetFoodMenuLastPrice(int itemType, int foodMenuId)
        {
            return _iPurchaseGRNRepository.GetFoodMenuLastPrice(itemType,foodMenuId);
        }

        public PurchaseGRNModel GetPurchaseGRNFoodMenuByPurchaseId(long purchaseId)
        {
            var model = (from purchase in _iPurchaseGRNRepository.GetPurchaseGRNFoodMenuByPurchaseId(purchaseId).ToList()
                         select new PurchaseGRNModel()
                         {
                             Id = purchase.Id,
                             PurchaseId= purchase.PurchaseId,
                             ReferenceNo = purchase.ReferenceNo,
                             SupplierId = purchase.SupplierId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             PurchaseGRNDate = purchase.PurchaseGRNDate,
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
                model.PurchaseGRNDetails = (from purchasedetails in _iPurchaseGRNRepository.GetPurchaseGRNFoodMenuDetailsByPurchaseId(purchaseId)
                                            select new PurchaseGRNDetailModel()
                                            {
                                                PurchaseGRNId = purchasedetails.PurchaseGRNId,
                                                FoodMenuId = purchasedetails.FoodMenuId,
                                                POQTY = purchasedetails.POQTY,
                                                GRNQTY = purchasedetails.GRNQTY,
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
            return _iPurchaseGRNRepository.GetPurchaseIdByPOReference(poReference);
        }

        public PurchaseGRNModel GetViewPurchaseGRNFoodMenuById(long purchaseGRNId)
        {
            PurchaseGRNModel purchaseModel = new PurchaseGRNModel();

            var model = (from purchase in _iPurchaseGRNRepository.GetViewPurchaseGRNFoodMenuById(purchaseGRNId).ToList()
                         select new PurchaseGRNModel()
                         {
                             Id = purchase.Id,
                             ReferenceNo = purchase.ReferenceNo,
                             SupplierId = purchase.SupplierId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             PurchaseGRNDate = purchase.PurchaseGRNDate,
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
                             SupplierName = purchase.SupplierName,
                             StoreName= purchase.StoreName,
                             POReferenceNo = purchase.POReferenceNo,
                             PODate = purchase.PODate,
                                 VatableAmount = purchase.VatableAmount,
                                 NonVatableAmount = purchase.NonVatableAmount
                             }).SingleOrDefault();
            if (model != null)
            {
                model.PurchaseGRNDetails = (from purchasedetails in _iPurchaseGRNRepository.GetViewPurchaseGRNFoodMenuDetails(purchaseGRNId)
                                            select new PurchaseGRNDetailModel()
                                            {
                                                PurchaseGRNId = purchasedetails.PurchaseGRNId,
                                                FoodMenuId = purchasedetails.FoodMenuId,
                                                POQTY = purchasedetails.POQTY,
                                                GRNQTY = purchasedetails.GRNQTY,
                                                UnitPrice = purchasedetails.UnitPrice,
                                                GrossAmount = purchasedetails.GrossAmount,
                                                DiscountPercentage = purchasedetails.DiscountPercentage,
                                                DiscountAmount = purchasedetails.DiscountAmount,
                                                TaxAmount = purchasedetails.TaxAmount,
                                                TotalAmount = purchasedetails.TotalAmount,
                                                IngredientName = purchasedetails.IngredientName,
                                                FoodMenuName = purchasedetails.FoodMenuName,
                                                ItemType = purchasedetails.ItemType,
                                                UnitName = purchasedetails.UnitName,
                                                VatableAmount = purchasedetails.VatableAmount,
                                                NonVatableAmount = purchasedetails.NonVatableAmount
                                            }).ToList();
            }
            return model;
        }

    }
}
