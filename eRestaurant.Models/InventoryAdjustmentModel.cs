using RocketPOS.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class InventoryAdjustmentModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public int? EmployeeId { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }

        public string StoreName { get; set; }
        [Required(ErrorMessage = "Select Store")]
        public int? StoreId { get; set; }
        public List<SelectListItem> StoreList { get; set; }

        public int? IngredientId { get; set; }
        public List<SelectListItem> IngredientList { get; set; }
        public int? FoodMenuId { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }
        [Required(ErrorMessage = "Select Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public int InventoryType { get; set; }

        [EnumDataType(typeof(TableStatus))]
        public ConsumpationStatus? ConsumpationStatus { get; set; }
        public List<InventoryAdjustmentDetailModel> InventoryAdjustmentDetail { get; set; }

        public int[] DeletedId { get; set; }

    }

    public class InventoryAdjustmentDetailModel
    {
        public long InventoryAdjustmentId { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TotalAmount { get; set; }
        [EnumDataType(typeof(TableStatus))]

        public ConsumpationStatus? ConsumpationStatus { get; set; }
    }
}
