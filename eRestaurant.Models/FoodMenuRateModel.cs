using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class FoodMenuRateModel
    {
        public int FoodCategoryId { get; set; }
        public string FoodCategoryName { get; set; }
        public List<SelectListItem> FoodCategoryList { get; set; }
        public List<FoodMenuRate> foodMenuRates { get; set; }
        public int OutletListId { get; set; }
        public string OutletListName { get; set; }
        public List<SelectListItem> OutletList { get; set; }

    }
    public class FoodMenuRate
    {
        //[JsonProperty("id")]
        public int Id { get; set; }
        //[JsonProperty("outletId")]
        public int OutletId { get; set; }
        //[JsonProperty("outletName")]
        public string OutletName { get; set; }
        //[JsonProperty("foodMenuId")]
        public int FoodMenuId { get; set; }
        //[JsonProperty("foodMenuName")]
        public string FoodMenuName { get; set; }
        //[JsonProperty("salesPrice")]
        public decimal SalesPrice { get; set; }
        //[JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}
