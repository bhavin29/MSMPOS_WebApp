using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class SalesInvoiceService : ISalesInvoiceService
    {
        private readonly ISalesInvoiceRepository _iSalesInvoiceRepository;

        public SalesInvoiceService(ISalesInvoiceRepository iSalesInvoiceRepository)
        {
            _iSalesInvoiceRepository = iSalesInvoiceRepository;
        }

        public int DeletePurchaseInvoice(long purchaseId)
        {
            return _iSalesInvoiceRepository.DeletePurchaseInvoice(purchaseId);
        }

        public int DeletePurchaseInvoiceDetails(long PurchaseInvoiceDetailsId)
        {
            return _iSalesInvoiceRepository.DeletePurchaseInvoiceDetails(PurchaseInvoiceDetailsId);
        }

        public SalesInvoiceModel GetPurchaseInvoiceFoodMenuById(long purchaseInvoiceId)
        {
            SalesInvoiceModel purchaseModel = new SalesInvoiceModel();

            var model = (from purchase in _iSalesInvoiceRepository.GetPurchaseInvoiceFoodMenuById(purchaseInvoiceId).ToList()
                         select new SalesInvoiceModel()
                         {
                             Id = purchase.Id,
                             SalesId = purchase.SalesId,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             SalesInvoiceDate = purchase.SalesInvoiceDate,
                             GrossAmount = purchase.GrossAmount,
                             TaxAmount = purchase.TaxAmount,
                             TotalAmount = purchase.TotalAmount,
                             PaidAmount = purchase.PaidAmount,
                             DueAmount = purchase.DueAmount,
                             DeliveryNoteNumber = purchase.DeliveryNoteNumber,
                             DeliveryDate = purchase.DeliveryDate,
                             DriverName = purchase.DriverName,
                             VehicleNumber = purchase.VehicleNumber,
                             Notes = purchase.Notes,
                             SOReferenceNo = purchase.SOReferenceNo,
                             SODate = purchase.SODate
                         }).SingleOrDefault();
            if (model != null)
            {
                model.SalesInvoiceDetails = (from purchasedetails in _iSalesInvoiceRepository.GetPurchaseInvoiceFoodMenuDetails(purchaseInvoiceId)
                                                select new SalesInvoiceDetailModel()
                                                {
                                                    SalesInvoiceId = purchasedetails.SalesInvoiceId,
                                                    IngredientId = purchasedetails.IngredientId,
                                                    FoodMenuId = purchasedetails.FoodMenuId,
                                                    SOQTY = purchasedetails.SOQTY,
                                                    InvoiceQty = purchasedetails.InvoiceQty,
                                                    UnitPrice = purchasedetails.UnitPrice,
                                                    GrossAmount = purchasedetails.GrossAmount,
                                                    DiscountPercentage = purchasedetails.DiscountPercentage,
                                                    DiscountAmount = purchasedetails.DiscountAmount,
                                                    TaxAmount = purchasedetails.TaxAmount,
                                                    TotalAmount = purchasedetails.TotalAmount,
                                                    IngredientName = purchasedetails.IngredientName,
                                                    FoodMenuName = purchasedetails.FoodMenuName
                                                }).ToList();
            }
            return model;
        }

        public List<SalesInvoiceViewModel> GetPurchaseInvoiceFoodMenuList()
        {
            return _iSalesInvoiceRepository.GetPurchaseInvoiceFoodMenuList();
        }

        public List<SalesInvoiceViewModel> PurchaseInvoiceFoodMenuListByDate(string fromDate, string toDate, int supplierId, int storeId)
        {
            return _iSalesInvoiceRepository.PurchaseInvoiceFoodMenuListByDate(fromDate, toDate, supplierId, storeId);
        }
        public int InsertPurchaseInvoiceFoodMenu(SalesInvoiceModel purchaseModel)
        {
            return _iSalesInvoiceRepository.InsertPurchaseInvoiceFoodMenu(purchaseModel);
        }

        public int UpdatePurchaseInvoiceFoodMenu(SalesInvoiceModel purchaseModel)
        {
            return _iSalesInvoiceRepository.UpdatePurchaseInvoiceFoodMenu(purchaseModel);
        }

        public string ReferenceNumberFoodMenu()
        {
            return _iSalesInvoiceRepository.ReferenceNumberFoodMenu();
        }
        public decimal GetTaxByFoodMenuId(int foodMenuId)
        {
            return _iSalesInvoiceRepository.GetTaxByFoodMenuId(foodMenuId);
        }

        public SalesInvoiceModel GetPurchaseInvoiceFoodMenuByPurchaseId(long purchaseId)
        {
            var model = (from purchase in _iSalesInvoiceRepository.GetPurchaseInvoiceFoodMenuByPurchaseId(purchaseId).ToList()
                         select new SalesInvoiceModel()
                         {
                             Id = purchase.Id,
                             SalesId = purchase.SalesId,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             SalesInvoiceDate = purchase.SalesInvoiceDate,
                             GrossAmount = purchase.GrossAmount,
                             TaxAmount = purchase.TaxAmount,
                             TotalAmount = purchase.TotalAmount,
                             PaidAmount = purchase.PaidAmount,
                             DueAmount = purchase.DueAmount,
                             DeliveryNoteNumber = purchase.DeliveryNoteNumber,
                             DeliveryDate = purchase.DeliveryDate,
                             DriverName = purchase.DriverName,
                             VehicleNumber = purchase.VehicleNumber,
                             Notes = purchase.Notes,
                             SalesStatus = purchase.SalesStatus,
                             SOReferenceNo = purchase.SOReferenceNo,
                             SODate = purchase.SODate,
                             VatableAmount = purchase.VatableAmount,
                             NonVatableAmount = purchase.NonVatableAmount
                         }).SingleOrDefault();
            if (model != null)
            {
                model.SalesInvoiceDetails = (from purchasedetails in _iSalesInvoiceRepository.GetPurchaseInvoiceFoodMenuDetailsPurchaseId(purchaseId)
                                                select new SalesInvoiceDetailModel()
                                                {
                                                    SalesInvoiceId = purchasedetails.SalesInvoiceId,
                                                    FoodMenuId = purchasedetails.FoodMenuId,
                                                    IngredientId = purchasedetails.IngredientId,
                                                    SOQTY = purchasedetails.SOQTY,
                                                    InvoiceQty = purchasedetails.InvoiceQty,
                                                    UnitPrice = purchasedetails.UnitPrice,
                                                    GrossAmount = purchasedetails.GrossAmount,
                                                    DiscountPercentage = purchasedetails.DiscountPercentage,
                                                    DiscountAmount = purchasedetails.DiscountAmount,
                                                    TaxAmount = purchasedetails.TaxAmount,
                                                    TotalAmount = purchasedetails.TotalAmount,
                                                    IngredientName = purchasedetails.IngredientName,
                                                    FoodMenuName = purchasedetails.FoodMenuName,
                                                    ItemType = purchasedetails.ItemType,
                                                    VatableAmount = purchasedetails.VatableAmount,
                                                    NonVatableAmount = purchasedetails.NonVatableAmount
                                                }).ToList();
            }
            return model;
        }

        public int GetPurchaseIdByPOReference(string poReference)
        {
            return _iSalesInvoiceRepository.GetPurchaseIdByPOReference(poReference);
        }

        public List<SalesInvoiceModel> GetPurchaseInvoiceById(long purchaseInvoiceId)
        {
            return _iSalesInvoiceRepository.GetPurchaseInvoiceById(purchaseInvoiceId);
        }

        public SalesInvoiceModel GetSalesInvoiceReportById(long id)
        {
            SalesInvoiceModel purchaseModel = new SalesInvoiceModel();

            var model = (from purchase in _iSalesInvoiceRepository.GetSalesInvoiceReportById(id).ToList()
                         select new SalesInvoiceModel()
                         {
                             Id = purchase.Id,
                             SalesId = purchase.SalesId,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             SalesInvoiceDate = purchase.SalesInvoiceDate,
                             GrossAmount = purchase.GrossAmount,
                             TaxAmount = purchase.TaxAmount,
                             TotalAmount = purchase.TotalAmount,
                             PaidAmount = purchase.PaidAmount,
                             DueAmount = purchase.DueAmount,
                             VatableAmount = purchase.VatableAmount,
                             NonVatableAmount = purchase.NonVatableAmount,
                             DeliveryNoteNumber = purchase.DeliveryNoteNumber,
                             DeliveryDate = purchase.DeliveryDate,
                             DriverName = purchase.DriverName,
                             VehicleNumber = purchase.VehicleNumber,
                             Notes = purchase.Notes,
                             SOReferenceNo = purchase.SOReferenceNo,
                             SODate = purchase.SODate,
                             CustomerAddress1= purchase.CustomerAddress1,
                             CustomerAddress2 = purchase.CustomerAddress2,
                             CustomerEmail = purchase.CustomerEmail,
                             CustomerName = purchase.CustomerName,
                             StoreName = purchase.StoreName
                         }).SingleOrDefault();
            if (model != null)
            {
                model.SalesInvoiceDetails = (from purchasedetails in _iSalesInvoiceRepository.GetSalesInvoiceReportFoodMenuDetails(id)
                                             select new SalesInvoiceDetailModel()
                                             {
                                                 SalesInvoiceId = purchasedetails.SalesInvoiceId,
                                                 IngredientId = purchasedetails.IngredientId,
                                                 FoodMenuId = purchasedetails.FoodMenuId,
                                                 SOQTY = purchasedetails.SOQTY,
                                                 InvoiceQty = purchasedetails.InvoiceQty,
                                                 UnitPrice = purchasedetails.UnitPrice,
                                                 GrossAmount = purchasedetails.GrossAmount,
                                                 DiscountPercentage = purchasedetails.DiscountPercentage,
                                                 DiscountAmount = purchasedetails.DiscountAmount,
                                                 TaxAmount = purchasedetails.TaxAmount,
                                                 TotalAmount = purchasedetails.TotalAmount,
                                                 IngredientName = purchasedetails.IngredientName,
                                                 FoodMenuName = purchasedetails.FoodMenuName,
                                                 UnitName= purchasedetails.UnitName
                                             }).ToList();
            }
            return model;
        }

        public string GetInvoiceHtmlString(SalesInvoiceModel salesInvoiceModel)
        {
            var sb = new StringBuilder();
            sb.Append(@"<html>
                    <style type='text/css'>
                        table, tr, td {
                        border: 1px solid;
                    }
                    tr.noBorder td {
                    border: 0;
                    }
                    </style>
                    <body>
                        <table border=1 width='1280' Height='800'>
                        <tr align='center' Height='50'>
	                    <td colspan='7'><div align='center'>");

            sb.Append(salesInvoiceModel.StoreName);
            sb.Append(@"</div></td>
                            </tr>
                            <tr align='center' Height='50'>
                            	<td colspan='7'>INVOICE </td>
                            </tr>
                            <tr Height='50'>
                            <td colspan=3 >"+ salesInvoiceModel.CustomerName + "</br>" + salesInvoiceModel.CustomerAddress1 + "</br>" + salesInvoiceModel.CustomerAddress2 + "</br>" + salesInvoiceModel.CustomerEmail + @"</td>
                            <td colspan=2></td>
                            <td colspan=2></td>
                            </tr>
                            <tr Height='50'>
                            	<td colspan=3>Invoice : " + salesInvoiceModel.ReferenceNo + @"</td>
                                <td colspan=2></td>
                            	<td colspan=2>Date : " + salesInvoiceModel.SalesInvoiceDate.ToShortDateString() + @"</td>
                            </tr>
                            <tr Height='30' class='noBorder'>
                            	<td colspan=3 ><b>Item</b></td>
                            	<td width=50 ><b>Qty</b></td>
                                <td ><b>UoM</b></td>
                            	<td ><b>Rate</b></td>
                            	<td ><b>Amount</b></td>
                            </tr>");

            foreach (var item in salesInvoiceModel.SalesInvoiceDetails)
            {
                sb.AppendFormat(@"<tr  Height='30' class='noBorder'><td colspan=3 >{0}</td><td width=50 >{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", item.FoodMenuName, item.InvoiceQty , item.UnitName, item.UnitPrice, item.GrossAmount);
            }
            sb.Append(@"<tr Height='30'><td colspan=5 rowspan=5 style='vertical-align:top; text-align : left;'><b>Remarks : </b>" + salesInvoiceModel.Notes + @"</td><td >Gross Total</td><td >" + salesInvoiceModel.GrossAmount + "</td></tr>");
            sb.Append(@"<tr Height='30'><td >Vatable</td ><td >" + salesInvoiceModel.VatableAmount + "</td></tr>");
            sb.Append(@"<tr Height='30'><td >Non Vatable</td><td >" + salesInvoiceModel.NonVatableAmount + "</td></tr>");
            sb.Append(@"<tr Height='30'><td >Total Tax</td><td >" + salesInvoiceModel.TaxAmount + "</td></tr>");
            sb.Append(@"<tr Height='30'><td >Grand Total</td><td >" + salesInvoiceModel.TotalAmount + "</td></tr>");
            sb.Append(@" <tr Height='30'>
                            <td colspan='7'><b>Terms and Conditions: </b> </br> 1. Terms : Due on demand. 
                            </br> 2. Receive all products in good conditon, goods once sold are not returnable unless prior agreements.
                            </br> 3. Do not pay cash to any sales reps or delivery team, strictly PAYBILL/CHEQUE. 
                            </br> 4. Goods supplied herein remain the sole property of BAPS until fully Paid. 
                            </br> 5. Interest @ 2.5% monthly will be charged for all overdue accounts. 
                            </td>
                            </tr>
                            </body>
                            </table>
                            </html>");
            return sb.ToString();
        }

        public SalesInvoiceModel GetViewSalesInvoiceFoodMenuById(long purchaseId)
        {
            var model = (from purchase in _iSalesInvoiceRepository.GetViewSalesInvoiceFoodMenuById(purchaseId).ToList()
                         select new SalesInvoiceModel()
                         {
                             Id = purchase.Id,
                             SalesId = purchase.SalesId,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             SalesInvoiceDate = purchase.SalesInvoiceDate,
                             GrossAmount = purchase.GrossAmount,
                             TaxAmount = purchase.TaxAmount,
                             TotalAmount = purchase.TotalAmount,
                             PaidAmount = purchase.PaidAmount,
                             DueAmount = purchase.DueAmount,
                             DeliveryNoteNumber = purchase.DeliveryNoteNumber,
                             DeliveryDate = purchase.DeliveryDate,
                             DriverName = purchase.DriverName,
                             VehicleNumber = purchase.VehicleNumber,
                             Notes = purchase.Notes,
                             SalesStatus = purchase.SalesStatus,
                             StoreName = purchase.StoreName,
                             CustomerName = purchase.CustomerName,
                             SOReferenceNo = purchase.SOReferenceNo,
                             SODate = purchase.SODate,
                             VatableAmount = purchase.VatableAmount,
                             NonVatableAmount = purchase.NonVatableAmount
                         }).SingleOrDefault();
            if (model != null)
            {
                model.SalesInvoiceDetails = (from purchasedetails in _iSalesInvoiceRepository.GetViewSalesInvoiceFoodMenuDetails(purchaseId)
                                                select new SalesInvoiceDetailModel()
                                                {
                                                    SalesInvoiceId = purchasedetails.SalesInvoiceId,
                                                    FoodMenuId = purchasedetails.FoodMenuId,
                                                    IngredientId = purchasedetails.IngredientId,
                                                    SOQTY = purchasedetails.SOQTY,
                                                    InvoiceQty = purchasedetails.InvoiceQty,
                                                    UnitPrice = purchasedetails.UnitPrice,
                                                    GrossAmount = purchasedetails.GrossAmount,
                                                    DiscountPercentage = purchasedetails.DiscountPercentage,
                                                    DiscountAmount = purchasedetails.DiscountAmount,
                                                    TaxAmount = purchasedetails.TaxAmount,
                                                    TotalAmount = purchasedetails.TotalAmount,
                                                    IngredientName = purchasedetails.IngredientName,
                                                    FoodMenuName = purchasedetails.FoodMenuName,
                                                    UnitName = purchasedetails.UnitName,
                                                    VatableAmount = purchasedetails.VatableAmount,
                                                    NonVatableAmount = purchasedetails.NonVatableAmount
                                                }).ToList();
            }
            return model;
        }
    }
}
