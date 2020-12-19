using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RocketPOS.Models
{
    public class BankModel
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Branch { get; set; }
        public string SignaturePicture { get; set; }
        public int UserId { get; set; }
    }
}
