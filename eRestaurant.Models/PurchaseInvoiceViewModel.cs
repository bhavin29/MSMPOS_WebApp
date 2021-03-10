using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class PurchaseInvoiceViewModel
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string ReferenceNo { get; set; }
        public string POReferenceNo { get; set; }
        public string Date { get; set; }
        public string SupplierName { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal GrandTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Due { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int? SupplierId { get; set; }
        public List<SelectListItem> SupplierList { get; set; }
        public List<PurchaseInvoiceDetailModel> purchaseInvoiceDetailModels { get; set; }
    }
}
