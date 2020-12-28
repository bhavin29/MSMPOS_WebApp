using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RocketPOS.Models
{
    public class WasteModel
    {
        public int Id { get; set; }
        public int OutletId { get; set; }

        [DisplayName("Reference Number")]
        [Required]

        public string ReferenceNumber { get; set; }
        public DateTime DateTime { get; set; }
        public int EmployeeID { get; set; }
        decimal TotalLossAmount { get; set; }
        string ReasonForWaste { get; set; }
        int Status { get; set; }

    }
    public class    WasteDetailModel
    {
        public int Id { get; set; }
        public int WasteId { get; set; }
        public int IngredientId { get; set; }
        public decimal Qty { get; set; }
        public decimal LossAmount { get; set; }

    }
}
