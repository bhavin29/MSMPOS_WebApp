using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RocketPOS.Models.Reports
{
    public class OutletRegisterReportModel
    {
        public int OutletRegisterId { get; set; }
        [DisplayName("Title")]
        public string RegisterTitle { get; set; }
        [DisplayName("Value")]
        public string RegisterValue { get; set; }
    }
}
