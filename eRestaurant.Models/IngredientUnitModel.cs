using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RocketPOS.Models
{
    public class IngredientUnitModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Unit name")]
        [DisplayName("Ingredient Unit")]
        public string IngredientUnitName { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

        public IngredientUnitModel()
        {
            IsActive = true;
        }
    }
}
