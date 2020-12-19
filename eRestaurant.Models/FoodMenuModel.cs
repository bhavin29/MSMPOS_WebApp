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

        [DisplayName("Food Category Name")]
        [Required]
        public string FoodCategoryName { get; set; }
        public List<SelectListItem> FoodCategoryList { get; set; }

        [DisplayName("Food Menu Name")]
        [Required]
        public string FoodMenuName { get; set; }
        public string FoodMenuCode { get; set; }
        public string ColourCode { get; set; }
        public string BigThumb { get; set; }
        public string MediumThumb { get; set; }
        public string SmallThumb { get; set; }
        public float SalesPrice { get; set; }
        public string Notes { get; set; }
        public bool IsVegItem { get; set; }
        public bool IsBeverages { get; set; }
        public decimal FoodVat { get; set; }
        public decimal Foodcess { get; set; }
        public bool OfferIsAvailable { get; set; }
        public int Position { get; set; }
        public int OutletId { get; set; }
        public bool IsActive { get; set; }


    }
}

