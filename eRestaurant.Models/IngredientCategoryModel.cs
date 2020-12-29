using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using RocketPOS.Framework;

namespace RocketPOS.Models
{
    public class IngredientCategoryModel
    {
        public int Id {get; set;}

        [DisplayName("Ingredient Category")]
        [Required(ErrorMessage = "Enter Ingredient Category")]
        public string IngredientCategoryName { get; set; }

        [DisplayName("Raw Material Type")]
        public RawMaterialType? RawMaterialType { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

        public IngredientCategoryModel()
        {
            IsActive = true;
        }
    }
}
