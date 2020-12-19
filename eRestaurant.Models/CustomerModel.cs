using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public int CustomerTypeId { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerImage { get; set; }
        public string FavDeliveryAddress { get; set; }
        public bool IsActive { get; set; }

    }
}
