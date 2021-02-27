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
    public class PurchaseInvoiceService : IPurchaseInvoiceService
    {
        private readonly IPurchaseInvoiceRepository _iPurchaseInvoiceRepository;

        public PurchaseInvoiceService(IPurchaseInvoiceRepository iPurchaseRepository)
        {
            _iPurchaseInvoiceRepository = iPurchaseRepository;
        }

        public PurchaseInvoiceModel GetPurchaseInvoiceById(long purchaseId)
        {
            PurchaseInvoiceModel purchaseModel = new PurchaseInvoiceModel();

            var model = (from purchase in _iPurchaseInvoiceRepository.GetPurchaseInvoiceById(purchaseId).ToList()
                         select new PurchaseInvoiceModel()
                         {
                             Id = purchase.Id,
                             ReferenceNo = purchase.ReferenceNo,
                             SupplierId = purchase.SupplierId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             PurchaseInvoiceDate = purchase.PurchaseInvoiceDate,
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
                model.purchaseInvoiceDetails = (from purchasedetails in _iPurchaseInvoiceRepository.GetPurchaseInvoiceDetails(purchaseId)
                                         select new PurchaseInvoiceDetailModel()
                                         {
                                             PurchaseInvoiceId = purchasedetails.PurchaseInvoiceId,
                                             IngredientId = purchasedetails.IngredientId,
                                             POQTY = purchasedetails.POQTY,
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
        public List<PurchaseInvoiceViewModel> GetPurchaseInvoiceList()
        {
            return _iPurchaseInvoiceRepository.GetPurchaseInvoiceList();
        }
        public int InsertPurchaseInvoice(PurchaseInvoiceModel purchaseModel)
        {
            return _iPurchaseInvoiceRepository.InsertPurchaseInvoice(purchaseModel);
        }
        public int UpdatePurchaseInvoice(PurchaseInvoiceModel purchaseModel)
        {
            return _iPurchaseInvoiceRepository.UpdatePurchaseInvoice(purchaseModel);
        }
        public int DeletePurchaseInvoice(long purchaseId)
        {
            return _iPurchaseInvoiceRepository.DeletePurchaseInvoice(purchaseId);
        }

        public int DeletePurchaseInvoiceDetails(long PurchaseInvoiceDetailsId)
        {
            return _iPurchaseInvoiceRepository.DeletePurchaseInvoiceDetails(PurchaseInvoiceDetailsId);
        }

        public string ReferenceNumber()
        {
            return _iPurchaseInvoiceRepository.ReferenceNumber();
        }

        public PurchaseInvoiceModel GetPurchaseInvoiceFoodMenuById(long purchaseInvoiceId)
        {
            PurchaseInvoiceModel purchaseModel = new PurchaseInvoiceModel();

            var model = (from purchase in _iPurchaseInvoiceRepository.GetPurchaseInvoiceFoodMenuById(purchaseInvoiceId).ToList()
                         select new PurchaseInvoiceModel()
                         {
                             Id = purchase.Id,
                             PurchaseId= purchase.PurchaseId,
                             ReferenceNo = purchase.ReferenceNo,
                             SupplierId = purchase.SupplierId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             PurchaseInvoiceDate = purchase.PurchaseInvoiceDate,
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
                model.purchaseInvoiceDetails = (from purchasedetails in _iPurchaseInvoiceRepository.GetPurchaseInvoiceFoodMenuDetails(purchaseInvoiceId)
                                         select new PurchaseInvoiceDetailModel()
                                         {
                                             PurchaseInvoiceId = purchasedetails.PurchaseInvoiceId,
                                             IngredientId = purchasedetails.IngredientId,
                                             FoodMenuId = purchasedetails.FoodMenuId,
                                             POQTY = purchasedetails.POQTY,
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

        public List<PurchaseInvoiceViewModel> GetPurchaseInvoiceFoodMenuList()
        {
            return _iPurchaseInvoiceRepository.GetPurchaseInvoiceFoodMenuList();
        }

        public List<PurchaseInvoiceViewModel> PurchaseInvoiceFoodMenuListByDate(string fromDate, string toDate, int supplierId)
        {
            return _iPurchaseInvoiceRepository.PurchaseInvoiceFoodMenuListByDate(fromDate, toDate, supplierId);
        }
        public int InsertPurchaseInvoiceFoodMenu(PurchaseInvoiceModel purchaseModel)
        {
            return _iPurchaseInvoiceRepository.InsertPurchaseInvoiceFoodMenu(purchaseModel);
        }

        public int UpdatePurchaseInvoiceFoodMenu(PurchaseInvoiceModel purchaseModel)
        {
            return _iPurchaseInvoiceRepository.UpdatePurchaseInvoiceFoodMenu(purchaseModel);
        }

        public string ReferenceNumberFoodMenu()
        {
            return _iPurchaseInvoiceRepository.ReferenceNumberFoodMenu();
        }
        public decimal GetTaxByFoodMenuId(int foodMenuId)
        {
            return _iPurchaseInvoiceRepository.GetTaxByFoodMenuId(foodMenuId);
        }

        public decimal GetFoodMenuLastPrice(int itemType, int foodMenuId)
        {
            return _iPurchaseInvoiceRepository.GetFoodMenuLastPrice(itemType,foodMenuId);
        }

        public PurchaseInvoiceModel GetPurchaseInvoiceFoodMenuByPurchaseId(long purchaseId)
        {
            var model = (from purchase in _iPurchaseInvoiceRepository.GetPurchaseInvoiceFoodMenuByPurchaseId(purchaseId).ToList()
                         select new PurchaseInvoiceModel()
                         {
                             Id = purchase.Id,
                             PurchaseId = purchase.PurchaseId,
                             ReferenceNo = purchase.ReferenceNo,
                             SupplierId = purchase.SupplierId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             PurchaseInvoiceDate = purchase.PurchaseInvoiceDate,
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
                             PurchaseStatus = purchase.PurchaseStatus
                         }).SingleOrDefault();
            if (model != null)
            {
                model.purchaseInvoiceDetails = (from purchasedetails in _iPurchaseInvoiceRepository.GetPurchaseInvoiceFoodMenuDetailsPurchaseId(purchaseId)
                                                select new PurchaseInvoiceDetailModel()
                                                {
                                                    PurchaseInvoiceId = purchasedetails.PurchaseInvoiceId,
                                                    FoodMenuId = purchasedetails.FoodMenuId,
                                                    IngredientId = purchasedetails.IngredientId,
                                                    POQTY = purchasedetails.POQTY,
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

        public int GetPurchaseIdByPOReference(string poReference)
        {
            return _iPurchaseInvoiceRepository.GetPurchaseIdByPOReference(poReference);
        }

        public PurchaseInvoiceModel GetViewPurchaseInvoiceFoodMenuById(long purchaseId)
        {
            var model = (from purchase in _iPurchaseInvoiceRepository.GetViewPurchaseInvoiceFoodMenuById(purchaseId).ToList()
                         select new PurchaseInvoiceModel()
                         {
                             Id = purchase.Id,
                             PurchaseId = purchase.PurchaseId,
                             ReferenceNo = purchase.ReferenceNo,
                             SupplierId = purchase.SupplierId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             PurchaseInvoiceDate = purchase.PurchaseInvoiceDate,
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
                             PurchaseStatus = purchase.PurchaseStatus,
                             StoreName=purchase.StoreName,
                             SupplierName=purchase.SupplierName
                         }).SingleOrDefault();
            if (model != null)
            {
                model.purchaseInvoiceDetails = (from purchasedetails in _iPurchaseInvoiceRepository.GetViewPurchaseInvoiceFoodMenuDetails(purchaseId)
                                                select new PurchaseInvoiceDetailModel()
                                                {
                                                    PurchaseInvoiceId = purchasedetails.PurchaseInvoiceId,
                                                    FoodMenuId = purchasedetails.FoodMenuId,
                                                    IngredientId = purchasedetails.IngredientId,
                                                    POQTY = purchasedetails.POQTY,
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
    }
}
