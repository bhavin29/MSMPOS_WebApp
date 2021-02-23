using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
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
