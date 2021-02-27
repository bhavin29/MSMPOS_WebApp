using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class PurchaseInvoiceModel
    {
        public int ItemType { get; set; }
        public int Id { get; set; }
        public int InventoryType { get; set; }
        public int PurchaseOrderId { get; set; }
        public string ReferenceNo { get; set; }
        public long PurchaseId { get; set; }
        [DataType(DataType.Date)]

        public DateTime PurchaseInvoiceDate { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal GrossAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TaxAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TotalAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal PaidAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal DueAmount { get; set; }
        public string DeliveryNoteNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }
        public string DriverName { get; set; }
        public string VehicleNumber { get; set; }
        public  string Notes { get; set; }
        public List<PurchaseInvoiceDetailModel> purchaseInvoiceDetails { get; set; }
        public int[] DeletedId { get; set; }
        public int PurchaseStatus { get; set; }

        public string SupplierName { get; set; }
        public string StoreName { get; set; }
    }
    public class PurchaseInvoiceDetailModel
    {
        public int ItemType { get; set; }
        public int Id { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public string FoodMenuName { get; set; }
        public string IngredientName { get; set; }

        public int FoodMenuId { get; set; }
        public int IngredientId { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal POQTY { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal InvoiceQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal GrossAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal DiscountPercentage { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal DiscountAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TaxAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TotalAmount { get; set; }
    }
}
