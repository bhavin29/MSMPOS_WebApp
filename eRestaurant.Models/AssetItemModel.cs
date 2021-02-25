using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class AssetItemModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Asset Item")]
        public string AssetItemName { get; set; }
        public string ShortName { get; set; }
        public string Code { get; set; }
        public int AssetSizeId { get; set; }
        public string AssetSizeName { get; set; }
        public List<SelectListItem> AssetSizeList { get; set; }
        public int AssetLocationId { get; set; }
        public string AssetLocationName { get; set; }
        public List<SelectListItem> AssetLocationList { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public List<SelectListItem> UnitList { get; set; }
        public string Brandname { get; set; }
        public string Model { get; set; }
        public string Picture { get; set; }
        public string Notes { get; set; }
        public decimal CostPrice { get; set; }
    }
}
