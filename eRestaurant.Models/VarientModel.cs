using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RocketPOS.Models
{
    public class VarientModel
    {
        public int Id { get; set; }
        public int FoodId { get; set; }

        public string FoodMenuName { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }

        [DisplayName("Varient Name")]
        [Required]
        public string VarientName { get; set; }
        public decimal Price { get; set; }

        public bool IsActive { get; set;}

    }
}
