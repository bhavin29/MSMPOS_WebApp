using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RocketPOS.Models
{
    public class FoodMenuModel
    {
        public int Id { get; set; }
        public int FoodCategoryId { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Select Category")]
        public string FoodCategoryName { get; set; }
        public List<SelectListItem> FoodCategoryList { get; set; }

        [DisplayName("Food Menu")]
        [Required(ErrorMessage = "Enter Food Menu")]
        public string FoodMenuName { get; set; }

        [DisplayName("Code")]
        public string FoodMenuCode { get; set; }
        public string ColourCode { get; set; }
        public decimal PurchasePrice {get; set;}
        public bool Readymade { get; set; }
        public string BigThumb { get; set; }
        public string MediumThumb { get; set; }
        public string SmallThumb { get; set; }
        public float SalesPrice { get; set; }
        public string Notes { get; set; }
        public bool IsVegItem { get; set; }
        public bool IsBeverages { get; set; }
        public decimal FoodVat { get; set; }
        public int FoodVatTaxId { get; set; }
        public string FoodVatTaxName { get; set; }
        public List<SelectListItem> FoodVatTaxList { get; set; }
        public List<SelectListItem> UnitsList { get; set; }
        public int UnitsId { get; set; }
        public string UnitName { get; set; }
        public decimal Foodcess { get; set; }
        public bool OfferIsAvailable { get; set; }
        public int Position { get; set; }
        public string OutletId { get; set; }
        public bool IsActive { get; set; }
        public decimal Consumption { get; set; }
        public int? IngredientId { get; set; }
        public List<SelectListItem> IngredientList { get; set; }
    //    public List<FoodManuDetailsModel> FoodMenuDetails { get; set; }
        public int[] DeletedId { get; set; }

        public FoodMenuModel()
        {
            IsActive = true;
        }
    }
    public class FoodManuDetailsModel
    {
        public long FoodMenuId { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public decimal Consumption { get; set; }
    }
}

