﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models.Reports
{
    public class DetailedDailyReportModel
    {
        public string RegisterTitle { get; set; }
        public string RegisterValue { get; set; }
    }

    public class ProductWiseSalesReportModel
    {
        public string FoodMenuCategoryName { get; set; }
        public string Id { get; set; }
        public string RowNumber { get; set; }
        public string FoodMenuName { get; set; }
        public string SalesPrice { get; set; }
        public string FoodMenuQty { get; set; }
        public string Total { get; set; }
    }
    public class DetailSaleSummaryModel
    {
        public string OrderDate { get; set; }
        public string FoodMenuName { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal TotalQty { get; set; }
        public decimal TotalNetAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalTaxPercentage { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalBillGrossAmount { get; set; }
        public decimal CashPayment { get; set; }
        public decimal CardPayment { get; set; }
    }
    public class SalesByCategoryProductModel
    {
        public string SectionName { get; set; }
        public string FoodMenuCategoryName { get; set; }
        public string FoodMenuName { get; set; }
        public decimal TotalUnitPrice { get; set; }
        public decimal TotalQty { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal ValuePercentage { get; set; }
    }
    public class TableStatisticsModel
    {
        public string TableName { get; set; }
        public decimal ActualCapacity { get; set; }
        public decimal ExpectedOccupancy { get; set; }
        public decimal Occupancy { get; set; }
        public decimal OccupancyPercentage { get; set; }
    }
    public class SalesSummaryModel
    {
        public string FoodMenuCategoryName { get; set; }
        public decimal TotalQty { get; set; }
        public decimal NetSalesAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal ValuePercentage { get; set; }
    }

    public class SalesSummaryByFoodCategoryFoodMenuModel
    {
        public string FoodMenuCategoryName { get; set; }
        public string FoodMenuName { get; set; }
        public decimal TotalQty { get; set; }
        public decimal NetSalesAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal ValuePercentage { get; set; }
    }

    public class SalesSummaryBySectionModel
    {
        public string SectionName { get; set; }
        public string Orderdate { get; set; }
        public decimal TotalInvoice { get; set; }
        public decimal NetSalesAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalGrossAmount { get; set; }
    }

    public class SalesSummaryByWeek
    {
        public string WeekStartDate { get; set; }
        public decimal TotalInvoice { get; set; }
        public decimal NetSalesAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalGrossAmount { get; set; }
    }

    public class SalesSummaryByHours
    {
        public string OrderDate { get; set; }
        public TimeSpan? StartHour { get; set; }
        public TimeSpan? EndHour { get; set; }
        public decimal TotalInvoice { get; set; }
        public decimal NetSalesAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalGrossAmount { get; set; }
    }
    public class CustomerRewardModel
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Datetime { get; set; }
        public string Credit { get; set; }
        public string Debit { get; set; }
        public string Balance { get; set; }
    }
    public class ModeofPaymentReportModel
    {
        public string BillDate { get; set; }
        public string PaymentMethodName { get; set; }
        public string Sales { get; set; }
        public decimal BillAmount { get; set; }
        public string Card { get; set; }
        public string Chqeue { get; set; }
        public string Cash { get; set; }
        public string PaisaI { get; set; }

    }
    public class CessReportModel
    {
        public List<CessSummaryModel> CessSummaryList { get; set; }

        public List<CessDetailModel> CessDetailList { get; set; }
    }

    public class CessCategoryReportModel
    {
        public List<CessCategorySummaryModel> CessSummaryList { get; set; }

        public List<CessDetailModel> CessDetailList { get; set; }
    }
    public class CessDetailModel
    {
        public string BillDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal NetSales { get; set; }
        public decimal Vatable { get; set; }
        public decimal NonVatable { get; set; }
        public decimal TotalTax { get; set; }
        public decimal GrandTotal { get; set; }
        public Decimal CateringLevy { get; set; }
    }

    public class CessSummaryModel
    {
        public string BillDate { get; set; }
        public decimal NetSales { get; set; }
        public decimal Vatable { get; set; }
        public decimal NonVatable { get; set; }
        public decimal TotalTax { get; set; }
        public decimal GrandTotal { get; set; }
        public Decimal CateringLevy { get; set; }
    }
    public class CessCategorySummaryModel
    {
        //   public string BillDate { get; set; }
        public string FoodmenucategoryName { get; set; }
        public decimal NetSales { get; set; }
        public decimal Vatable { get; set; }
        public decimal NonVatable { get; set; }
        public decimal TotalTax { get; set; }
        public decimal GrandTotal { get; set; }
        public Decimal CateringLevy { get; set; }
    }
}
