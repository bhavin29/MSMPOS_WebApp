using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RocketPOS.Models
{
    public class IngredientCategoryModel
    {
        public int Id {get; set;}

        [DisplayName("Ingredient Category Name")]
        [Required]
        public string IngredientCategoryName { get; set; }

        public int RawMaterialType { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

    }
}
