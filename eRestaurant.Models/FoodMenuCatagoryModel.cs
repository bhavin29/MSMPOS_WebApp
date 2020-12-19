using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class FoodMenuCatagoryModel
    {
        public int Id { get; set; }
        public string FoodMenuCategoryName { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

    }
}
