using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class PaymentMethodModel
    {
        public int Id { get; set; }
        public string PaymentMethodName { get; set; }
        public bool IsBank { get; set; }
        public bool IsIntegration { get; set; }
        public bool IsActive { get; set; }
    }
}
