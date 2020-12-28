using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace RocketPOS.Models
{
    public class IngredientModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Enter Ingredient Name")]
        public string IngredientName { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        [Required(ErrorMessage = "Select Category")]
        public int? CategoryId { get; set; }
        public List<SelectListItem> IngredientCategoryList { get; set; }
        [Required(ErrorMessage = "Select Unit")]
        public int UnitId { get; set; }
        public List<SelectListItem> UnitList { get; set; }
        public string Unit { get; set; }
        public decimal PurchasePrice { get; set; }
        [Required]
        [RegularExpression(@"[0-9]+(\.[0-9][0-9]?)?", ErrorMessage = "Enter valid amount")]
        public decimal SalesPrice { get; set; }
        public decimal AlterQty { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }

        public IngredientModel()
        {
            IsActive = true;
        }
    }
}
