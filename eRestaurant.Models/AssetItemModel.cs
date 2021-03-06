﻿using System;
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
        [Required(ErrorMessage = "Select Unit")]
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public List<SelectListItem> UnitList { get; set; }

        [Required(ErrorMessage = "Select Tax")]
        public int TaxId { get; set; }
        public string Tax { get; set; }
        public List<SelectListItem> TaxList { get; set; }

        [Required(ErrorMessage = "Select Asset Category")]
        public int AssetCategoryId { get; set; }
        public string AssetCategoryName { get; set; }
        public List<SelectListItem> AssetCategoryList { get; set; }
        public string Brandname { get; set; }
        public string Model { get; set; }
        public string Picture { get; set; }
        public string Notes { get; set; }
        public decimal CostPrice { get; set; }
    }
}
