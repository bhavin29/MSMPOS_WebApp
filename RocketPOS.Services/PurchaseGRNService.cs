﻿using System;
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

        public PurchaseGRNModel GetPurchaseGRNFoodMenuById(long purchaseId)
        {
            PurchaseGRNModel purchaseModel = new PurchaseGRNModel();

            var model = (from purchase in _iPurchaseGRNRepository.GetPurchaseGRNFoodMenuById(purchaseId).ToList()
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
                model.PurchaseGRNDetails = (from purchasedetails in _iPurchaseGRNRepository.GetPurchaseGRNFoodMenuDetails(purchaseId)
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

        public decimal GetFoodMenuLastPrice(int foodMenuId)
        {
            return _iPurchaseGRNRepository.GetFoodMenuLastPrice(foodMenuId);
        }
    }
}