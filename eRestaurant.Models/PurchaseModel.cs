using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class PurchaseModel
    {

        public long Id { get; set; }
        [Required(ErrorMessage = "Enter Reference No")]
        public string ReferenceNo { get; set; }
        [Required(ErrorMessage = "Select Supplier")]
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
        
        [Required(ErrorMessage = "Select Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Due { get; set; }
        public decimal Paid { get; set; }
        public string Message { get; set; }
        public string Notes { get; set; }
        public int InventoryType { get; set; }
        public List<PurchaseDetailsModel> PurchaseDetails { get; set; }
        public int[] DeletedId { get; set; }
    }
   
    public class PurchaseDetailsModel
    {
        public long PurchaseId { get; set; }
        public int ReferenceNo { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
