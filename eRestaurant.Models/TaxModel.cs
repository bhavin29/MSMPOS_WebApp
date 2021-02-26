using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RocketPOS.Models
{
    public class TaxModel
    {
        public int Id { get; set; }
        [DisplayName("Tax")]
        [Required(ErrorMessage = "Enter Tax Name")]
        public string TaxName { get; set; }
        [DisplayName("Percentage")]
        public decimal TaxPercentage { get; set; }
        [DisplayName("Type")]
        public int TaxType { get; set; }
    }
}
