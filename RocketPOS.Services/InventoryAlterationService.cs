using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class InventoryAlterationService : IInventoryAlterationService
    {
        private readonly IInventoryAlterationRepository _inventoryAlterationRepository;

        public InventoryAlterationService(IInventoryAlterationRepository inventoryAlterationRepository)
        {
            _inventoryAlterationRepository = inventoryAlterationRepository;
        }

        public List<InventoryAlterationViewListModel> GetInventoryAlterationList(int storeId, DateTime fromDate, DateTime toDate, int foodMenuId)
        {
            return _inventoryAlterationRepository.GetInventoryAlterationList(storeId, fromDate, toDate, foodMenuId);
        }

        public decimal GetInventoryStockQty(int storeId, int foodMenuId)
        {
            return _inventoryAlterationRepository.GetInventoryStockQty(storeId, foodMenuId);
        }

        public decimal GetInventoryStockQtyForIngredient(int storeId, int ingredientId)
        {
            return _inventoryAlterationRepository.GetInventoryStockQtyForIngredient(storeId, ingredientId);
        }

        public InventoryAlterationModel GetViewInventoryAlterationById(long invAltId)
        {
            var model = (from inventory in _inventoryAlterationRepository.GetViewInventoryAlterationById(invAltId).ToList()
                         select new InventoryAlterationModel()
                         {
                             Id = inventory.Id,
                             ReferenceNo = inventory.ReferenceNo,
                             StoreId = inventory.StoreId,
                             EntryDate = inventory.EntryDate,
                             Notes = inventory.Notes,
                             InventoryType = inventory.InventoryType,
                             StoreName = inventory.StoreName
                         }).SingleOrDefault();
            if (model != null)
            {
                model.InventoryAlterationDetails = (from inventoryAltDetail in _inventoryAlterationRepository.GetViewInventoryAlterationDetail(invAltId)
                                                   select new InventoryAlterationDetailModel()
                                                   {
                                                       InventoryAlterationId = inventoryAltDetail.InventoryAlterationId,
                                                       Qty = inventoryAltDetail.Qty,
                                                       InventoryStockQty = inventoryAltDetail.InventoryStockQty,
                                                       Amount = inventoryAltDetail.Amount,
                                                       FoodMenuName = inventoryAltDetail.FoodMenuName,
                                                       UnitName = inventoryAltDetail.UnitName
                                                   }).ToList();
            }
            return model;
        }

        public int InsertInventoryAlteration(InventoryAlterationModel inventoryAlterationModel)
        {
            return _inventoryAlterationRepository.InsertInventoryAlteration(inventoryAlterationModel);
        }

        public string ReferenceNumberInventoryAlteration()
        {
            return _inventoryAlterationRepository.ReferenceNumberInventoryAlteration();
        }
    }
}
