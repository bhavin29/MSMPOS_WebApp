using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class PurchaseInvoiceViewModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }

        public string Date { get; set; }
        public string SupplierName { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Due { get; set; }
        public int UserId { get; set; }
        public List<PurchaseInvoiceDetailModel> purchaseInvoiceDetailModels { get; set; }
    }
}
