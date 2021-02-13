using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace RocketPOS.Models
{
    public class IngredientCategoryModel
    {
        public int Id {get; set;}

        [Required(ErrorMessage = "Enter Category")]
        public string IngredientCategoryName { get; set; }

        public string RawMaterialType { get; set; }

        [Required(ErrorMessage = "Select Raw Material")]
        [Range(1, int.MaxValue, ErrorMessage = "Select Raw Material")]
       
        public int RawMaterialId { get; set; }
        public List<SelectListItem> RawMaterialList { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Select Unit")]
        public int UnitId { get; set; }

        public IngredientCategoryModel()
        {
            IsActive = true;
        }
    }
}
