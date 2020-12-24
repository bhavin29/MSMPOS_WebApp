using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RocketPOS.Models
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        public long ReferenceNo { get; set; }
     
        public DateTime Date { get; set; }
        public string SupplierName { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Due { get; set; }
        public int UserId { get; set; }
        public List<PurchaseDetailsModel> purchaseDetails { get; set; }
    }
}
