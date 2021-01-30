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
    public class InventoryAdjustmentService : IInventoryAdjustmentService
    {
        private readonly IInventoryAdjustmentRepository _inventoryAdjustmentRepository;

        public InventoryAdjustmentService(IInventoryAdjustmentRepository inventoryAdjustmentRepository)
        {
            _inventoryAdjustmentRepository = inventoryAdjustmentRepository;
        }

        public int DeleteInventoryAdjustment(long invAdjId)
        {
            return _inventoryAdjustmentRepository.DeleteInventoryAdjustment(invAdjId);
        }

        public int DeleteInventoryAdjustmentDetail(long invAdjId)
        {
            return _inventoryAdjustmentRepository.DeleteInventoryAdjustmentDetail(invAdjId);
        }

        public decimal GetFoodMenuPurchasePrice(int foodMenuId)
        {
            return _inventoryAdjustmentRepository.GetFoodMenuPurchasePrice(foodMenuId);
        }

        public InventoryAdjustmentModel GetInventoryAdjustmentById(long invAdjId)
        {
            var model = (from inventory in _inventoryAdjustmentRepository.GetInventoryAdjustmentById(invAdjId).ToList()
                         select new InventoryAdjustmentModel()
                         {
                             Id = inventory.Id,
                             ReferenceNo = inventory.ReferenceNo,
                             StoreId = inventory.StoreId,
                             EmployeeId = inventory.EmployeeId,
                             Date = inventory.Date,
                             Notes = inventory.Notes,
                             InventoryType = inventory.InventoryType,
                         }).SingleOrDefault();
            if (model != null)
            {
                model.InventoryAdjustmentDetail = (from inventoryAdjDetail in _inventoryAdjustmentRepository.GetInventoryAdjustmentDetail(invAdjId)
                                         select new InventoryAdjustmentDetailModel()
                                         {
                                             InventoryAdjustmentId = inventoryAdjDetail.InventoryAdjustmentId,
                                             IngredientId = inventoryAdjDetail.IngredientId,
                                             Quantity = inventoryAdjDetail.Quantity,
                                             Price = inventoryAdjDetail.Price,
                                             TotalAmount = inventoryAdjDetail.TotalAmount,
                                             ConsumpationStatus = inventoryAdjDetail.ConsumpationStatus,
                                             IngredientName = inventoryAdjDetail.IngredientName,
                                             FoodMenuId = inventoryAdjDetail.FoodMenuId,
                                             FoodMenuName = inventoryAdjDetail.FoodMenuName
                                         }).ToList();
            }
            return model;
        }

        public List<InventoryAdjustmentDetailModel> GetInventoryAdjustmentDetail(long invAdjId)
        {
            return _inventoryAdjustmentRepository.GetInventoryAdjustmentDetail(invAdjId);
        }

        public List<InventoryAdjustmentViewModel> GetInventoryAdjustmentList()
        {
            return _inventoryAdjustmentRepository.GetInventoryAdjustmentList();
        }

        public int InsertInventoryAdjustment(InventoryAdjustmentModel inventoryAdjustmentModel)
        {
            return _inventoryAdjustmentRepository.InsertInventoryAdjustment(inventoryAdjustmentModel);
        }

        public List<InventoryAdjustmentViewModel> InventoryAdjustmentListByDate(string fromDate, string toDate)
        {
            return _inventoryAdjustmentRepository.InventoryAdjustmentListByDate(fromDate, toDate);
        }

        public long ReferenceNumber()
        {
            return _inventoryAdjustmentRepository.ReferenceNumber();
        }

        public int UpdateInventoryAdjustment(InventoryAdjustmentModel inventoryAdjustmentModel)
        {
            return _inventoryAdjustmentRepository.UpdateInventoryAdjustment(inventoryAdjustmentModel);
        }
    }
}
