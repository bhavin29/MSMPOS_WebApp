using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class WasteListModel
    {
        public int Id { get; set; }

        public string ReferenceNumber { get; set; }

        public DateTime Wastedatetime { get; set; }

        public string OutlutName { get; set; }

        public string EmployeeName { get; set; }

        public decimal TotalLossAmount { get; set; }

        public string ReasonForWaste { get; set; }

        public string WasteStatus { get; set; }

       }
}
