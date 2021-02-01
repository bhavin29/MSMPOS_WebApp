using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RocketPOS.Models
{
    public class PurchaseGRNViewModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
     
        public string Date { get; set; }
        public string SupplierName { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TotalAmount   { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Due { get; set; }
        public int UserId { get; set; }
        public List<PurchaseGRNDetailModel> purchaseGRNDetails { get; set; }
    }
}
