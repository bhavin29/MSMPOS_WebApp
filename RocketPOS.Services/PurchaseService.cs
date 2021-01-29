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
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _iPurchaseRepository;

        public PurchaseService(IPurchaseRepository iPurchaseRepository)
        {
            _iPurchaseRepository = iPurchaseRepository;
        }

        public PurchaseModel GetPurchaseById(long purchaseId)
        {
            PurchaseModel purchaseModel = new PurchaseModel();

            var model = (from purchase in _iPurchaseRepository.GetPurchaseById(purchaseId).ToList()
                         select new PurchaseModel()
                         {
                             Id = purchase.Id,
                             ReferenceNo = purchase.ReferenceNo,
                             SupplierId = purchase.SupplierId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             Date = purchase.Date,
                             GrandTotal = purchase.GrandTotal,
                             Due = purchase.Due,
                             Paid = purchase.Paid,
                             Notes = purchase.Notes
                         }).SingleOrDefault();
            if(model != null)
            {
                model.PurchaseDetails = (from purchasedetails in _iPurchaseRepository.GetPurchaseDetails(purchaseId)
                                      select new PurchaseDetailsModel()
                                      {
                                          PurchaseId = purchasedetails.PurchaseId,
                                          IngredientId = purchasedetails.IngredientId,
                                          Quantity = purchasedetails.Quantity,
                                          UnitPrice = purchasedetails.UnitPrice,
                                          Total = purchasedetails.Total,
                                          IngredientName = purchasedetails.IngredientName
                                      }).ToList();
            }
            return model;
        }
        public List<PurchaseViewModel> GetPurchaseList()
        {
            return _iPurchaseRepository.GetPurchaseList();
        }
        public int InsertPurchase(PurchaseModel purchaseModel)
        {
            return _iPurchaseRepository.InsertPurchase(purchaseModel);
        }
        public int UpdatePurchase(PurchaseModel purchaseModel)
        {
            return _iPurchaseRepository.UpdatePurchase(purchaseModel);
        }
        public int DeletePurchase(long purchaseId)
        {
            return _iPurchaseRepository.DeletePurchase(purchaseId);
        }

        public int DeletePurchaseDetails(long PurchaseDetailsId)
        {
            return _iPurchaseRepository.DeletePurchaseDetails(PurchaseDetailsId);
        }

        public string ReferenceNumber()
        {
            return _iPurchaseRepository.ReferenceNumber();
        }

        public PurchaseModel GetPurchaseFoodMenuById(long purchaseId)
        {
            PurchaseModel purchaseModel = new PurchaseModel();

            var model = (from purchase in _iPurchaseRepository.GetPurchaseFoodMenuById(purchaseId).ToList()
                         select new PurchaseModel()
                         {
                             Id = purchase.Id,
                             ReferenceNo = purchase.ReferenceNo,
                             SupplierId = purchase.SupplierId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             Date = purchase.Date,
                             GrandTotal = purchase.GrandTotal,
                             DiscountAmount = purchase.DiscountAmount,
                             TaxAmount = purchase.TaxAmount,
                             Due = purchase.Due,
                             Paid = purchase.Paid,
                             Notes = purchase.Notes,
                             Status = purchase.Status
                         }).SingleOrDefault();
            if (model != null)
            {
                model.PurchaseDetails = (from purchasedetails in _iPurchaseRepository.GetPurchaseFoodMenuDetails(purchaseId)
                                         select new PurchaseDetailsModel()
                                         {
                                             PurchaseId = purchasedetails.PurchaseId,
                                             FoodMenuId = purchasedetails.FoodMenuId,
                                             Quantity = purchasedetails.Quantity,
                                             UnitPrice = purchasedetails.UnitPrice,
                                             DiscountAmount = purchasedetails.DiscountAmount,
                                             DiscountPercentage = purchasedetails.DiscountPercentage,
                                             TaxAmount = purchasedetails.TaxAmount,
                                             TaxPercentage = purchasedetails.TaxPercentage,
                                             Total = purchasedetails.Total,
                                             FoodMenuName = purchasedetails.FoodMenuName
                                         }).ToList();
            }
            return model;
        }

        public List<PurchaseViewModel> GetPurchaseFoodMenuList()
        {
            return _iPurchaseRepository.GetPurchaseFoodMenuList();
        }

        public List<PurchaseViewModel> PurchaseFoodMenuListByDate(string fromDate, string toDate)
        {
            return _iPurchaseRepository.PurchaseFoodMenuListByDate(fromDate, toDate);
        }
        public int InsertPurchaseFoodMenu(PurchaseModel purchaseModel)
        {
            return _iPurchaseRepository.InsertPurchaseFoodMenu(purchaseModel);
        }

        public int UpdatePurchaseFoodMenu(PurchaseModel purchaseModel)
        {
            return _iPurchaseRepository.UpdatePurchaseFoodMenu(purchaseModel);
        }

        public string ReferenceNumberFoodMenu()
        {
            return _iPurchaseRepository.ReferenceNumberFoodMenu();
        }

        public decimal GetTaxByFoodMenuId(int foodMenuId)
        {
            return _iPurchaseRepository.GetTaxByFoodMenuId(foodMenuId);
        }

        public decimal GetFoodMenuLastPrice(int foodMenuId)
        {
            return _iPurchaseRepository.GetFoodMenuLastPrice(foodMenuId);
        }

        public ClientModel GetClientDetail()
        {
            return _iPurchaseRepository.GetClientDetail();
        }
    }
}
