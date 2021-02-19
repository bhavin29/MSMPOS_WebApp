using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class AssetEventViewModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public string EventName { get; set; }
        public string Username { get; set; }
        public int Status { get; set; }
        public string EventDateTime { get; set; }
        public string ClosedDatetime { get; set; }



    }
    public class AssetEventModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public int EventType { get; set; }
        public string EventName { get; set; }
        public DateTime EventDatetime { get; set; }
        public string EventPlace { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonNumber { get; set; }
        public DateTime AllocationDatetime { get; set; }
        public DateTime ReturnDatetime { get; set; }
        public DateTime ClosedDatetime { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public Decimal FoodGrossAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public Decimal FoodDiscountAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public Decimal FoodNetAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public Decimal FoodVatAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public Decimal FoodTaxAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public Decimal IngredientNetAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public Decimal AssetItemNetAmount { get; set; }
        public int Status { get; set; }
        public DateTime DateInserted { get; set; }

        public int FoodMenuId { get; set; }
        public string FoodmenuName { get; set; }
        public List<SelectListItem> FoodMenuList { get; set; }

        public int AssetItemId { get; set; }
        public string AssetItemName { get; set; }
        public List<SelectListItem> AssetItemList { get; set; }

        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public List<SelectListItem> IngredientList { get; set; }
        public List<SelectListItem> MissingNoteList { get; set; }

        public List<AssetEventItemModel> assetEventItemModels { get; set; }
        public List<AssetEventFoodmenuModel> assetEventFoodmenuModels { get; set; }
        public List<AssetEventIngredientModel> assetEventIngredientModels { get; set; }
        public int[] AssetEventItemDeletedId { get; set; }
        public int[] AssetEventFoodmenuDeletedId { get; set; }
        public int[] AssetEventIngredientDeletedId { get; set; }
    }
    public class AssetEventItemModel
    {
        public int AssetEventItemId { get; set; }
        public int AssetItemId { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public string AssetItemName { get; set; }
        public string AssetItemUnitName { get; set; }
        public decimal StockQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal EventQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal AllocatedQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ReturnQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal MissingQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal CostPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TotalAmount { get; set; }
        public string MissingNote { get; set; }

    }
    public class AssetEventFoodmenuModel
    {
        public int AssetEventFoodmenuId { get; set; }
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        public string FoodMenuUnitName { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal SalesPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Qunatity { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal FoodVatAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal FoodTaxAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TotalPrice { get; set; }
    }
    public class AssetEventIngredientModel
    {
        public int AssetEventIngredientId { get; set; }
        public int IngredientId { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public string IngredientName { get; set; }
        public string IngredientUnitName { get; set; }
        public decimal StockQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal EventQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ReturnQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ActualQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal CostPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TotalAmount { get; set; }
    }
    public class AssetFoodMenuPriceDetail
    {
        public decimal SalesPrice { get; set; }
        public decimal TaxPercentage { get; set; }
        public int FoodVatTaxId { get; set; }
    }

    public class ProductionAutoEntry
    {
        public int ProductionFormulaId { get; set; }
        public int FoodmenuType { get; set; }
        public int FoodMenuId { get; set; }
        public int Status { get; set; }
        public decimal Qunatity { get; set; }
        public int UserIdInserted { get; set; }
        public int AssetEventId { get; set; }
        public int AssetEventFoodMenuId { get; set; }
        public decimal BatchSize { get; set; }
        public decimal ExpectedOutput { get; set; }
       

    }

}
