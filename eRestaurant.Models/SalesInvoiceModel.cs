using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class SalesInvoiceModel
    {
        public int ItemType { get; set; }
        public int Id { get; set; }
        public int InventoryType { get; set; }
        public int SalesInvoiceId { get; set; }
        public string ReferenceNo { get; set; }
        public string SOReferenceNo { get; set; }
        public long SalesId { get; set; }
        [DataType(DataType.Date)]

        public DateTime SalesInvoiceDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime SODate { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal VatableAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal NonVatableAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
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
        public string Notes { get; set; }
        public List<SalesInvoiceDetailModel> SalesInvoiceDetails { get; set; }
        public int[] DeletedId { get; set; }
        public int SalesStatus { get; set; }

        public string CustomerName { get; set; }
        public string StoreName { get; set; }
    }
    public class SalesInvoiceDetailModel
    {
        public int ItemType { get; set; }
        public int Id { get; set; }
        public int SalesInvoiceId { get; set; }
        public string FoodMenuName { get; set; }
        public string IngredientName { get; set; }

        public int FoodMenuId { get; set; }
        public int IngredientId { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal SOQTY { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal VatableAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal NonVatableAmount { get; set; }
        public string UnitName { get; set; }
        public int AssetItemId { get; set; }
        public string AssetItemName { get; set; }
    }

    public class SalesInvoiceViewModel
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string ReferenceNo { get; set; }
        public string POReferenceNo { get; set; }
        public string Date { get; set; }
        public string CustomerName { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal GrandTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Due { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int? CustomerId { get; set; }
        public List<SelectListItem> CustomerList { get; set; }
        public List<SalesInvoiceDetailModel> salesInvoiceDetailModel { get; set; }
    }
}
