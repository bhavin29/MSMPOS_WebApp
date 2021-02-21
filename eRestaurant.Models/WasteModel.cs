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
        [Required(ErrorMessage = "Select Store")]
        public int? StoreId { get; set; }
        public string StoreName { get; set; }
        public  List<SelectListItem> StoreList { get; set; }

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
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TotalLossAmount { get; set; }
        [Required(ErrorMessage ="Enter Reason for Waste")]
        public string ReasonForWaste { get; set; }
        public WasteStatus WasteStatus { get; set; }
        public List<WasteDetailModel> WasteDetail { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public List<SelectListItem> FoodMenuListForLostAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public List<SelectListItem> IngredientListForLostAmount { get; set; }
        public decimal FoodMenuIdForLostAmount { get; set; }
        public decimal IngredientIdForLostAmount { get; set; }

        public string[] DeletedId { get; set; }

        public DateTime CreatedDatetime { get; set; }
        public DateTime ApprovedDateTime { get; set; }

        public string CreatedUserName { get; set; }

        public string ApprovedUserName { get; set; }

    }
    public class WasteDetailModel
    {
        public long WasteIngredientId { get; set; }
        public long WasteId { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        public int IngredientId { get; set; }

        public string IngredientName { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Qty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal LossAmount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
