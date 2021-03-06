﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RocketPOS.Models
{
   public  class OutletModel
    {
        public  int Id { get; set;  }
        public int OriginalStoreId { get; set; }
        public int StoreId { get; set; }

        [DisplayName("Store Name")]
        [Required(ErrorMessage = "Select Store")]
        public string StoreName { get; set; }

        public List<SelectListItem> StoreList { get; set; }

        [DisplayName("Outlet Name")]
        [Required(ErrorMessage = "Enter Outlet Name")]
        public string OutletName { get; set;  }
        [DisplayName("Address")]
      
        public string OutletAddress1 { get; set; }
        public string OutletAddress2 { get; set; }
        public string OutletPhone { get; set; }
        public string OutletEmail { get; set; }
        public string InvoiceHeader { get; set; }
        public string InvoiceFooter { get; set; }
        [DisplayName("Collect Tax")]
        [Required]

        public bool IsCollectTax { get; set; }
        [DisplayName("Pre Payment")]
        [Required]
        public bool IsPreorPostPayment { get; set; }
        public bool IsActive { get; set; }
        public bool IsLock { get; set; }
        public int UserId { get; set; }

        public OutletModel()
        {
            IsActive = true;
        }

    }
}
