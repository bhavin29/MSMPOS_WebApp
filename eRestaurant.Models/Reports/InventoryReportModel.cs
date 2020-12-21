using System;
using System.Collections.Generic;
using System.Text;

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
    }

    public class InventoryReportParamModel
    {

        public int StoreId { get; set; }
        public int FoodMenuId { get; set; }
        public int IngredientCategoryId { get; set; }
        public int IngredientId { get; set; }

    }

}
