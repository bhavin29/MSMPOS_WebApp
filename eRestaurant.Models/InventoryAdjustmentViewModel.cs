using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class InventoryAdjustmentViewModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public int InventoryType { get; set; }
        public string Date { get; set; }
        public string StoreName { get; set; }
        public string Employee { get; set; }
        public int UserId { get; set; }
        public List<InventoryAdjustmentDetailModel> inventoryAdjustmentDetailModels { get; set; }
    }
}
