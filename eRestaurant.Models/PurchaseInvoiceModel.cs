using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class PurchaseInvoiceModel
    {
        public int Id { get; set; }
        public int InventoryType { get; set; }
        public int PurchaseOrderId { get; set; }
        public string ReferenceNo { get; set; }
        public string PurchaseId { get; set; }
        public string PurchaseInvoiceDate { get; set; }
        public int? SupplierId { get; set; }
        public List<SelectListItem> SupplierList { get; set; }
        public int? StoreId { get; set; }
        public List<SelectListItem> StoreList { get; set; }
        public int? IngredientId { get; set; }
        public List<SelectListItem> IngredientList { get; set; }
        public int? FoodMenuId { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }
        public int? EmployeeId { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DriverName { get; set; }
        public string VehicleNumber { get; set; }
        public  string Notes { get; set; }
        public List<PurchaseInvoiceDetailModel> purchaseInvoiceDetails { get; set; }
        public int[] DeletedId { get; set; }
    }
    public class PurchaseInvoiceDetailModel
    {
        public int Id { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public string FoodMenuName { get; set; }
        public string IngredientName { get; set; }

        public int FoodMenuId { get; set; }
        public int IngredientId { get; set; }
        public decimal POQTY { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
