using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RocketPOS.Models
{
    public class FoodMenuCatagoryModel
    {
        public int Id { get; set; }

        [DisplayName("FoodMenu Category")]
        [Required(ErrorMessage = "Enter FoodMenu Category")]
        public string FoodMenuCategoryName { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

    }
}
