using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class PurchaseGRNModel
    {
        public int Id { get; set; }
        public int InventoryType { get; set; }
        public int PurchaseOrderID { get; set; }
        public string ReferenceNo { get; set; }
        public string PurchaseNumber { get; set; }
        public DateTime PurchaseGRNDate { get; set; }
        public int SupplierId { get; set; }
        public int StoreId { get; set; }
        public int EmployeeId { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public string DeliveryDate { get; set; }
        public string DriverName { get; set; }
        public string VehicleNumber { get; set; }
        public string Notes { get; set; }
        public List<PurchaseGRNDetailModel> PurchaseGRNDetails { get; set; }

    }

    public class PurchaseGRNDetailModel
    {
        public int Id { get; set; }
        public int PurchaseGRNId { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public decimal POQTY { get; set; }
        public decimal GRNQTY { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
