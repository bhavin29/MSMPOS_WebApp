using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using RocketPOS.Framework;

namespace RocketPOS.Models
{
    public class WasteModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Select Outlet")]
        public int? OutletId { get; set; }
        public string OutletName { get; set; }
        public  List<SelectListItem> OutletList { get; set; }

        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public List<SelectListItem> IngredientList { get; set; }

        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }

        [DisplayName("Ref #")]
        [Required]

        public string ReferenceNumber { get; set; }

        [Required(ErrorMessage = "Select Date")]
        [DataType(DataType.Date)]
        public DateTime WasteDateTime { get; set; }
        [Required(ErrorMessage = "Select Resposible Persoin")]
        public int? EmployeeId { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public decimal TotalLossAmount { get; set; }
        [Required(ErrorMessage ="Enter Reason for Waste")]
        public string ReasonForWaste { get; set; }
        public WasteStatus WasteStatus { get; set; }
        public List<WasteDetailModel> WasteDetail { get; set; }
        public List<SelectListItem> FoodMenuListForLostAmount { get; set; }
        public List<SelectListItem> IngredientListForLostAmount { get; set; }
        public decimal FoodMenuIdForLostAmount { get; set; }
        public decimal IngredientIdForLostAmount { get; set; }

        public string[] DeletedId { get; set; }

    }
    public class    WasteDetailModel
    {
        public long WasteId { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        public int IngredientId { get; set; }

        public string IngredientName { get; set; }

        public decimal Qty { get; set; }
        public decimal LossAmount { get; set; }
        public bool IsDeleted { get; set; }

    }
}
