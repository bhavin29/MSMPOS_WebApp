using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RocketPOS.Models
{
    public class AssetCategoryModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Asset Category")]
        public string AssetCategoryName { get; set; }
        public string Notes { get; set; }
    }
}
