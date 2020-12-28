using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RocketPOS.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public int CustomerTypeId { get; set; }
        public string CustomerNumber { get; set; }

        [DisplayName("Customer Name")]
        [Required(ErrorMessage = "Enter Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("Email")]
        public string CustomerEmail { get; set; }
        [DisplayName("Address")]
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerImage { get; set; }
        public string FavDeliveryAddress { get; set; }
        public bool IsActive { get; set; }

        public CustomerModel()
        {
            IsActive = true;
        }
    }
}
