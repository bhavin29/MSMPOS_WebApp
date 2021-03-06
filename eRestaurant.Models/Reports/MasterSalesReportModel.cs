﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models.Reports
{
    public class ReportParameterModel
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
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public List<SelectListItem> StoreList { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int categoryId { get; set; }
        public int foodMenuId { get; set; }
        public int FromStoreId { get; set; }
        public string FromStoreName { get; set; }
        public List<SelectListItem> FromStoreList { get; set; }
        public int ToStoreId { get; set; }
        public string ToStoreName { get; set; }
        public List<SelectListItem> ToStoreList { get; set; }
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
