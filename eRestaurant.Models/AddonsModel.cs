using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RocketPOS.Models
{
    public class AddonsModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Enter addons name")]
        public string AddonsName { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

    }
}
