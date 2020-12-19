using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class AddonAssignModel
    {
        public int Id { get; set; }
        public int FoodId { get; set; }
        public int AddonId { get; set; }
        public bool IsActive { get; set; }

    }
}
