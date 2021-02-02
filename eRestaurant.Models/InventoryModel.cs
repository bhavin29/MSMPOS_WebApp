using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class InventoryModel
    {
        public int FoodCategoryId { get; set; }
        public string FoodCategoryName { get; set; }
        public List<SelectListItem> FoodCategoryList { get; set; }
        [Required(ErrorMessage = "Select Store")]
        public int? StoreId { get; set; }
        public List<SelectListItem> StoreList { get; set; }
        public List<InventoryDetail> InventoryDetailList { get; set; }
    }

    public class InventoryDetail
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string FoodMenuCategoryName { get; set; }
        public string FoodMenuName { get; set; }
        public int IngredientId { get; set; }
        public int FoodMenuId { get; set; }
        public decimal OpeningQty { get; set; }
        public decimal StockQty { get; set; }
        public decimal PhysicalStockINQty { get; set; }
        public decimal PhysicalStockOutQty { get; set; }
        public decimal PhysicalStockQty { get; set; }
        public DateTime? PhysicalDatetime { get; set; }
        public bool PhysicalIsLock { get; set; }
    }
}
