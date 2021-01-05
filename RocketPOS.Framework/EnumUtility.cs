using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RocketPOS.Framework
{
    public enum Country
    {
        [Display(Name = "India")]
        India,
        [Display(Name = "United States of America")]
        usa,
        [Display(Name = "United Kingdom")]
        uk,
        [Display(Name = "Australia")]
        Australia,
        [Display(Name = "SouthAfrica")]
        SouthAfrica,
    }

    public enum TableStatus
    {
        [Display(Name = "Open")]
        Open = 1,
        [Display(Name = "Occupied")]
        Occupied = 2,
        [Display(Name = "Clean")]
        Clean = 3,
    }
    public enum RoleType
    {
        [Display(Name = "Admin")]
        Admin = 1,
        [Display(Name = "Manager")]
        Manager = 2,
        [Display(Name = "POS")]
        POS = 3,
        [Display(Name = "Waiter")]
        Waiter = 4,
    }
    public enum RawMaterialType
    {
        [Display(Name = "Appletiser")]
        Admin = 1,
        [Display(Name = "Cold Drink")]
        Manager = 2,
        [Display(Name = "ICE Cream")]
        POS = 3,
    }
    public enum AttendanceStatus
    {
        [Display(Name = "Manually")]
        Manually = 1,
        [Display(Name = "Auto")]
        Auto = 2,
    }

    public enum ConsumpationStatus
    {
        [Display(Name = "Stock In")]
        StockIN = 1,
        [Display(Name = "Stock Out")]
        StockOUT = 2,
    }

    public enum WasteStatus
    {
        [Display(Name = "Pending")]
        Pending = 1,
        [Display(Name = "Approved")]
        Approved = 2,
    }
}
