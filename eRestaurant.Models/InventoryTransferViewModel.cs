using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class InventoryTransferViewModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public string Date { get; set; }
        public string FromStoreName { get; set; }
        public string ToStoreName { get; set; }
        public string Employee { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int InventoryType { get; set; }
    }
}
