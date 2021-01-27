using RocketPOS.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class InventoryTransferModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        [Required(ErrorMessage = "Select Employee")]
        public int? EmployeeId { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public string FromStoreName { get; set; }
        [Required(ErrorMessage = "Select Store")]
        public int? FromStoreId { get; set; }
        public List<SelectListItem> FromStoreList { get; set; }

        public string ToStoreName { get; set; }
        [Required(ErrorMessage = "Select Store")]
        public int? ToStoreId { get; set; }
        public List<SelectListItem> ToStoreList { get; set; }

        public int? IngredientId { get; set; }
        public List<SelectListItem> IngredientList { get; set; }
        public int? FoodMenuId { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }

        [Required(ErrorMessage = "Select Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public int InventoryType { get; set; }
        public List<InventoryTransferDetailModel> InventoryTransferDetail { get; set; }

        [EnumDataType(typeof(TableStatus))]
        public ConsumpationStatus? ConsumpationStatus { get; set; }
        public int[] DeletedId { get; set; }

    }

    public class InventoryTransferDetailModel
    {
        public long InventoryTransferId { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal CurrentStock { get; set; }
        public string ProductUnit { get; set; }
        [EnumDataType(typeof(TableStatus))]
        public ConsumpationStatus? ConsumpationStatus { get; set; }
    }
}
