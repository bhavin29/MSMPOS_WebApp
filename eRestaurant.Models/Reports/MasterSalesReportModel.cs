using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models.Reports
{
    public class MasterSalesReport
    {
        public int FoodCategoryId { get; set; }
        public string FoodCategoryName { get; set; }
        public List<SelectListItem> FoodCategoryList { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }
        public int OutletId { get; set; }
        public string OutletName { get; set; }
        public List<SelectListItem> OutletList { get; set; }
    }
    public class MasterSalesReportModel
    {
        public string OrderDate { get; set; }
        public string OrderTime { get; set; }
        public string SalesInvoiceNumber { get; set; }
        public string FoodMenuName { get; set; }
        public decimal FoodMenuRate { get; set; }
        public decimal FoodMenuQty { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal GrossAmount { get; set; }
        public string FoodMenuCategoryName { get; set; }
    }
}
