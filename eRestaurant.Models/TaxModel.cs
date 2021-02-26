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
        [DisplayName("Tax Name")]
        [Required(ErrorMessage = "Enter Tax Name")]
        public string TaxName { get; set; }
        public decimal TaxPercentage { get; set; }
        public int TaxType { get; set; }
    }
}
