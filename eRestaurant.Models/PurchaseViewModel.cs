using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RocketPOS.Models
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public string Date { get; set; }
        public string SupplierName { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal GrandTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Due { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        public string StoreName { get; set; }
        public List<PurchaseDetailsModel> purchaseDetails { get; set; }
    }
}
