
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class SalesModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Enter Reference No")]
        public string ReferenceNo { get; set; }
        [Required(ErrorMessage = "Select Customer")]
        public int? CustomerId { get; set; }
        public List<SelectListItem> CustomerList { get; set; }
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
        public decimal VatableAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal NonVatableAmount { get; set; }
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
        public string CustomerEmail { get; set; }
        public int InventoryType { get; set; }
        public int Status { get; set; }
        public DateTime DateInserted { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerPhone { get; set; }
        public bool IsSendEmail { get; set; }
        public List<SalesDetailsModel> SalesDetails { get; set; }
        public int[] DeletedId { get; set; }
        public string StoreName { get; set; }
    }

    public class SalesDetailsModel
    {

        public int RowNumber { get; set; }
        public int ItemType { get; set; }

        public long SalesId { get; set; }
        public int ReferenceNo { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}")]
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
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal VatableAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal NonVatableAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public string UnitName { get; set; }
        public int AssetItemId { get; set; }
        public string AssetItemName { get; set; }
    }

    public class SalesViewModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public string Date { get; set; }
        public string CustomerName { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal GrandTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Due { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        public string StoreName { get; set; }
        public List<SelectListItem> CustomerList { get; set; }
        public List<SalesDetailsModel> salesDetailsModel { get; set; }
    }
}
