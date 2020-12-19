using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
   public  class OutletModel
    {
        public  int Id { get; set;  }
        public int StoreId { get; set; }
        public string StoreName { get; set; }

        public List<SelectListItem> StoreList { get; set; }
        public string OutletName { get; set;  }
        public string OutletAddress1 { get; set; }
        public string OutletAddress2 { get; set; }
        public string OutletPhone { get; set; }
        public string OutletEmail { get; set; }
        public string InvoiceHeader { get; set; }
        public string InvoiceFooter { get; set; }
        public bool IsCollectTax { get; set; }
        public bool IsPreorPostPayment { get; set; }
        public bool IsActive { get; set; }
        public bool IsLock { get; set; }
        public int UserId { get; set; }

    }
}
