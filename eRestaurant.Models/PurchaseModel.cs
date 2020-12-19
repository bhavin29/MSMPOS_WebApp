using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class PurchaseModel
    {
        public PurchaseModel()
        {
            this.PurchaseDetails = new List<PurchaseDetailsModel>();
            this.SupplierList = new List<SelectListItem>();
            this.IngredientList = new List<SelectListItem>();
        }
        public long ReferenceNo { get; set; }
        public int SupplierId { get; set; }
        public List<SelectListItem> SupplierList { get; set; }
        public int IngredientId { get; set; }
        public List<SelectListItem> IngredientList { get; set; }
        public DateTime Date { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Due { get; set; }
        public decimal Paid { get; set; }
        public List<PurchaseDetailsModel> PurchaseDetails { get; set; }
    }

    public class PurchaseDetailsModel
    {
        public int IngredientCode { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
