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
    public class InventoryTransferService : IInventoryTransferService
    {
        private readonly IInventoryTransferRepository _inventoryTransferRepository;

        public InventoryTransferService(IInventoryTransferRepository inventoryTransferRepository)
        {
            _inventoryTransferRepository = inventoryTransferRepository;
        }

        public int DeleteInventoryTransfer(long invAdjId)
        {
            return _inventoryTransferRepository.DeleteInventoryTransfer(invAdjId);
        }

        public int DeleteInventoryTransferDetail(long invAdjId)
        {
            return _inventoryTransferRepository.DeleteInventoryTransferDetail(invAdjId);
        }

        public InventoryTransferModel GetInventoryTransferById(long id)
        {
            var model = (from inventory in _inventoryTransferRepository.GetInventoryTransferById(id).ToList()
                         select new InventoryTransferModel()
                         {
                             Id = inventory.Id,
                             ReferenceNo = inventory.ReferenceNo,
                             FromStoreId = inventory.FromStoreId,
                             ToStoreId = inventory.ToStoreId,
                             EmployeeId = inventory.EmployeeId,
                             Date = inventory.Date,
                             Notes = inventory.Notes,
                         }).SingleOrDefault();
            if (model != null)
            {
                model.InventoryTransferDetail = (from inventoryAdjDetail in _inventoryTransferRepository.GetInventoryTransferDetail(id)
                                                 select new InventoryTransferDetailModel()
                                                 {
                                                     InventoryTransferId = inventoryAdjDetail.InventoryTransferId,
                                                     IngredientId = inventoryAdjDetail.IngredientId,
                                                     Quantity = inventoryAdjDetail.Quantity,
                                                     ConsumpationStatus = inventoryAdjDetail.ConsumpationStatus,
                                                     IngredientName = inventoryAdjDetail.IngredientName
                                                 }).ToList();
            }
            return model;

        }

        public List<InventoryTransferDetailModel> GetInventoryTransferDetail(long invAdjId)
        {
            return _inventoryTransferRepository.GetInventoryTransferDetail(invAdjId);
        }

        public List<InventoryTransferViewModel> GetInventoryTransferList()
        {
            return _inventoryTransferRepository.GetInventoryTransferList();
        }

        public int InsertInventoryTransfer(InventoryTransferModel inventoryTransferModel)
        {
            return _inventoryTransferRepository.InsertInventoryTransfer(inventoryTransferModel);
        }

        public long ReferenceNumber()
        {
            return _inventoryTransferRepository.ReferenceNumber();
        }

        public int UpdateInventoryTransfer(InventoryTransferModel inventoryTransferModel)
        {
            return _inventoryTransferRepository.UpdateInventoryTransfer(inventoryTransferModel);
        }
    }
}
