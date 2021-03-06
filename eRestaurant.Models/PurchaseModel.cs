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
        public int? AssetItemId { get; set; }
        public List<SelectListItem> AssetItemList { get; set; }
        public int? EmployeeId { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }

        [Required(ErrorMessage = "Select Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal GrandTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TaxAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal DiscountAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Due { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal GrossAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Paid { get; set; }
        public string Message { get; set; }
        public string Notes { get; set; }
        public string SupplierEmail { get; set; }
        public int InventoryType { get; set; }
        public int Status { get; set; }
        public DateTime DateInserted { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress1 { get; set; }
        public string SupplierAddress2 { get; set; }
        public string SupplierPhone { get; set; }
        public bool IsSendEmail { get; set; }
        public List<PurchaseDetailsModel> PurchaseDetails { get; set; }
        public int[] DeletedId { get; set; }
        public string StoreName { get; set; }
    }

    public class PurchaseDetailsModel
    {

        public int RowNumber { get; set; }
        public int ItemType { get; set; }

        public long PurchaseId { get; set; }
        public int ReferenceNo { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal DiscountAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal DiscountPercentage { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TaxAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TaxPercentage { get; set; }
        public decimal Quantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Total { get; set; }
        public string UnitName { get; set; }
        public int AssetItemId { get; set; }
        public string AssetItemName { get; set; }
    }
}
