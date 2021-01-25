using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class PurchaseGRNModel
    {
        public int Id { get; set; }
        public int InventoryType { get; set; }
        public int PurchaseOrderID { get; set; }
        public string ReferenceNo { get; set; }
        public string PurchaseId { get; set; }
        [Required(ErrorMessage = "Select Date")]
        [DataType(DataType.Date)]
        public DateTime PurchaseGRNDate { get; set; }
 
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
        public string DeliveryDate { get; set; }
        public string DriverName { get; set; }
        public string VehicleNumber { get; set; }
        public string Notes { get; set; }
        public List<PurchaseGRNDetailModel> PurchaseGRNDetails { get; set; }
        public int[] DeletedId { get; set; }
        public bool IsSendEmail { get; set; }
        public string SupplierEmail { get; set; }


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
        public decimal TaxPercentage { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
