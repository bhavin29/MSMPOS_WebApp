using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace RocketPOS.Models.Reports
{
    public class PurchaseReportParamModel
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public PurchaseReportModel[] data { get; set; }
    }

    //public class PurchaseReportModel
    //{
    //    public long ReferenceNo { get; set; }
    //    public string Date { get; set; }
    //    public string Supplier { get; set; }
    //    public decimal GrandTotal { get; set; }
    //    public decimal Paid { get; set; }
    //    public decimal Due { get; set; }
    //    public string Ingredients { get; set; }
    //    public string PurchaseBy { get; set; }
    //}
   
}
