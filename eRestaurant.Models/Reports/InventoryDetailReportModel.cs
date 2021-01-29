using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models.Reports
{
    public class InventoryDetailReportModel
    {
        public int Index { get; set; }
        public int Id { get; set; }
        public string StoreName { get; set; }
        public int StoreId { get; set; }

        public string DocType { get; set; }
        public string DocTable { get; set; }
        public string DocNumber { get; set; }
        public int DocNumberId { get; set; }
        public int DocNumberDetailId { get; set; }

        public string DocDate { get; set; }

        public string SupplierName { get; set; }
        public int SupplierId { get; set; }

        public string Remakrs { get; set; }
        public string IngredientName { get; set; }
        public string IngredientCategoryName { get; set; }
        public string FoodMenuCode { get; set; }
        public string FoodMenuName { get; set; }
        public string FoodMenuCategoryName { get; set; }
        public float StockInQty { get; set; }
        public float StockOutQty { get; set; }
        public float BalanceQty { get; set; }
        public string StockQtyText { get; set; }
 
        public string Unitname { get; set; }

        public string IngredientCategory { get; set; }
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
}
