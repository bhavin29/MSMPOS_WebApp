using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RocketPOS.Models
{
    public class PaymentMethodModel
    {
        public int Id { get; set; }

        [DisplayName("Payment Method")]
        [Required]
        public string PaymentMethodName { get; set; }
        [DisplayName("Bank")]
        public bool IsBank { get; set; }
        [DisplayName("Integration")]
        public bool IsIntegration { get; set; }
        public bool IsActive { get; set; }
    }
}
