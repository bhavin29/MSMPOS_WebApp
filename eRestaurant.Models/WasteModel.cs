using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class WasteModel
    {
        public int Id { get; set; }
        public int OutletId { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime DateTime { get; set; }
        public int EmployeeID { get; set; }
        decimal TotalLossAmount { get; set; }
        string ReasonForWaste { get; set; }
        int Status { get; set; }

    }
}
