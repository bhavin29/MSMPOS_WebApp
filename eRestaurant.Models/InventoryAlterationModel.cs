using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class InventoryAlterationViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? StoreId { get; set; }
        public List<SelectListItem> StoreList { get; set; }
        public int? FoodMenuId { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }

        public List<InventoryAlterationViewListModel> InventoryAlterationViewList = new List<InventoryAlterationViewListModel>();
    }

    public class InventoryAlterationViewListModel
    {
        public int Id { get; set; }
        public int InventoryAlterationId { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime EntryDate { get; set; }
        public string StoreName { get; set; }
        public int StoreId { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        public decimal Qty { get; set; }
        public decimal Amount { get; set; }
    }

    public class InventoryAlterationModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime EntryDate { get; set; }
        public string StoreName { get; set; }
        [Required(ErrorMessage = "Select Store")]
        public int? StoreId { get; set; }
        public List<SelectListItem> StoreList { get; set; }
        public int? FoodMenuId { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }

        public List<InventoryAlterationDetailModel> InventoryAlterationDetails { get; set; }
    }

    public class InventoryAlterationDetailModel
    {
        public int Id { get; set; }
        public int InventoryAlterationId { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        public decimal Qty { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }
    }
}
