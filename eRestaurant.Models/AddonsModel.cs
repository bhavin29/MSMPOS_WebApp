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

        [Required(ErrorMessage = "Enter Addons Name")]

        public string AddonsName { get; set; }

        [Required(ErrorMessage = "Enter Price")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

        public AddonsModel()
        {
            IsActive = true;
        }

    }
}
