using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RocketPOS.Models
{
    public class AddonsModel
    {
        public int Id { get; set; }
       
        [DisplayName("Addons Name")]
       
        public string AddonsName { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

    }
}
