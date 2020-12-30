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
        [Required(ErrorMessage = "Select Employee")]
        public int? EmployeeId { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }

        public string StoreName { get; set; }
        [Required(ErrorMessage = "Select Store")]
        public int? StoreId { get; set; }
        public List<SelectListItem> StoreList { get; set; }

        public int? IngredientId { get; set; }
        public List<SelectListItem> IngredientList { get; set; }
        [Required(ErrorMessage = "Select Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Notes { get; set; }

        [EnumDataType(typeof(TableStatus))]
        public ConsumpationStatus? ConsumpationStatus { get; set; }
        public List<InventoryAdjustmentDetailModel> InventoryAdjustmentDetail { get; set; }

    }

    public class InventoryAdjustmentDetailModel
    {
        public long InventoryAdjustmentId { get; set; }
        public int ReferenceNo { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public decimal Quantity { get; set; }

        [EnumDataType(typeof(TableStatus))]
        public ConsumpationStatus? ConsumpationStatus { get; set; }
    }
}
