using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace RocketPOS.Models.Reports
{
    public class InventoryReportModel
    {

        public int Index { get; set; }
        public int Id { get; set; }
        public string IngredientName { get; set; }
        public string IngredientCategoryName { get; set; }
        public float StockQty { get; set; }
        public string StockQtyText { get; set; }
        public float AlterQty { get; set; }

        public string IngredientCategory { get; set; }
        [Required(ErrorMessage = "Select Category")]
        public int? IngredientCategoryId { get; set; }
        public List<SelectListItem> IngredientCategoryList { get; set; }

        public string Ingredient { get; set; }
        [Required(ErrorMessage = "Select Category")]
        public int? IngredientId { get; set; }
        public List<SelectListItem> IngredientList { get; set; }

        public string FoodMenu { get; set; }
        [Required(ErrorMessage = "Select Category")]
        public int? FoodMenuId { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }

    }

    public class InventoryReportParamModel
    {

        public int StoreId { get; set; }
        public int FoodMenuId { get; set; }
        public int IngredientCategoryId { get; set; }
        public int IngredientId { get; set; }

    }

}
