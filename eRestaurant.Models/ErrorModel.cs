using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class ErrorModel
    {
        public string MethodName { get; set; }
        public string ErrorPath { get; set; }
        public string ErrorDetails { get; set; }
        public int UserId { get; set; }
    }
}
