using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class ProductionEntryViewModel
    {
        public int Id { get; set; }
        public string ProductionFormulaId { get; set; }
        public string ProductionFormulaName { get; set; }
        public int FoodmenuType { get; set; }
        public string ReferenceNo { get; set; }
        public string ProductionDate { get; set; }
        public DateTime ProductionCompletionDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public string ActualBatchSize { get; set; }
        public int Status { get; set; }
        public string Username { get; set; }

    }
    public class ProductionEntryModel
    {
        public int Id { get; set; }
        public string ProductionFormulaId { get; set; }
        public string ProductionFormulaName { get; set; }
        public List<SelectListItem> ProductionFormulaList { get; set; }
        public int FoodmenuType { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ProductionCompletionDate { get; set; }
        public string BatchSize { get; set; }
        public int BatchSizeUnitId { get; set; }
        public string UnitName { get; set; }        
        public string ActualBatchSize { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodmenuName { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ExpectedOutput { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ActualOutput { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public string Notes { get; set; }
        public string VariationNotes { get; set; }        
        public List<SelectListItem> IngredientList { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal IngredientQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ActualIngredientQty { get; set; }
        public int Status { get; set; }
        public List<ProductionEntryFoodMenuModel> productionEntryFoodMenuModels { get; set; }
        public List<ProductionEntryIngredientModel> productionEntryIngredientModels { get; set; }
        public int[] FoodMenuDeletedId { get; set; }
        public int[] IngredientDeletedId { get; set; }

    }
    public class ProductionEntryFoodMenuModel
    {
        public int PEFoodMenuId { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ExpectedOutput { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal AllocationOutput { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ActualOutput { get; set; }

    }
    public class ProductionEntryIngredientModel
    {
        public int PEIngredientId { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal IngredientQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ActualIngredientQty { get; set; }
    }
}
