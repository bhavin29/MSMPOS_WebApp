using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class FoodMenuIngredientModel
    {
        public int? FoodCategoryId { get; set; }
        public List<SelectListItem> FoodCategoryList { get; set; }
        public int? FoodMenuId { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }
        public int? IngredientId { get; set; }
        public List<SelectListItem> IngredientList { get; set; }
        public int[] DeletedId { get; set; }
        public List<FoodMenuIngredientDetails> FoodMenuIngredientDetails { get; set; }
    }
    public class FoodMenuIngredientDetails
    {
        public int Id { get; set; }
        public int FoodMenuId { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public string UnitName { get; set; }
        public decimal Consumption { get; set; }
    }
}
