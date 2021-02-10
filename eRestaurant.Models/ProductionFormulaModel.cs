using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class ProductionFormulaViewModel
    {
        public int Id { get; set; }
        public string FormulaName { get; set; }
        public string BatchSize { get; set; }
        public string MenuItem { get; set; }
        public string ExpectedOutput { get; set; }
        public string Username { get; set; }
        public string FoodmenuType { get; set; }
        public bool IsActive { get; set; }

    }
    public class ProductionFormulaModel
    {
        public int Id { get; set; }
        public string FormulaName { get; set; }
        public string BatchSize { get; set; }
        public bool IsActive { get; set; }
        public int FoodmenuType { get; set; }        
        public int FoodMenuId { get; set; }
        public string FoodmenuName { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }
        public decimal ExpectedOutput { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public List<SelectListItem> IngredientList { get; set; }
        public decimal IngredientQty { get; set; }
        public List<ProductionFormulaFoodMenuModel> productionFormulaFoodMenuModels { get; set; }
        public List<ProductionFormulaIngredientModel> productionFormulaIngredientModels { get; set; }
        public int[] FoodMenuDeletedId { get; set; }
        public int[] IngredientDeletedId { get; set; }

    }
    public class ProductionFormulaFoodMenuModel
    {
        public int PFFoodMenuId { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        public decimal ExpectedOutput { get; set; }

    }
    public class ProductionFormulaIngredientModel
    {
        public int PFIngredientId { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public decimal IngredientQty { get; set; }

    }
}
