using System;
using System.ComponentModel.DataAnnotations;

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
}
