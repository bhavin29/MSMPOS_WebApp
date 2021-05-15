using RocketPOS.Framework;
using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class SalesDeliveryService : ISalesDeliveryService
    {
        private readonly ISalesDeliveryRepository _iSalesDeliveryRepository;

        public SalesDeliveryService(ISalesDeliveryRepository iSalesDeliveryRepository)
        {
            _iSalesDeliveryRepository = iSalesDeliveryRepository;
        }

        public int DeletePurchaseGRN(long purchaseId)//
        {
            return _iSalesDeliveryRepository.DeletePurchaseGRN(purchaseId);
        }

        public int DeletePurchaseGRNDetails(long PurchaseGRNDetailsId)//
        {
            return _iSalesDeliveryRepository.DeletePurchaseGRNDetails(PurchaseGRNDetailsId);
        }

        public List<SalesDeliveryViewModel> GetPurchaseGRNFoodMenuList()//
        {
            return _iSalesDeliveryRepository.GetPurchaseGRNFoodMenuList();
        }

        public List<SalesDeliveryViewModel> PurchaseGRNFoodMenuListByDate(string fromDate, string toDate, int customerId, int storeId)//
        {
            return _iSalesDeliveryRepository.PurchaseGRNFoodMenuListByDate(fromDate, toDate, customerId, storeId);
        }
        public int InsertPurchaseGRNFoodMenu(SalesDeliveryModel purchaseModel)//
        {
            return _iSalesDeliveryRepository.InsertPurchaseGRNFoodMenu(purchaseModel);
        }

        public int UpdatePurchaseGRNFoodMenu(SalesDeliveryModel purchaseModel)//
        {
            return _iSalesDeliveryRepository.UpdatePurchaseGRNFoodMenu(purchaseModel);
        }
        public string ReferenceNumberFoodMenu()//
        {
            return _iSalesDeliveryRepository.ReferenceNumberFoodMenu();
        }
        public decimal GetTaxByFoodMenuId(int foodMenuId)//
        {
            return _iSalesDeliveryRepository.GetTaxByFoodMenuId(foodMenuId);
        }
        public decimal GetFoodMenuLastPrice(int itemType, int foodMenuId)//
        {
            return _iSalesDeliveryRepository.GetFoodMenuLastPrice(itemType, foodMenuId);
        }
        public SalesDeliveryModel GetPurchaseGRNFoodMenuByPurchaseId(long purchaseId)//
        {
            var model = (from purchase in _iSalesDeliveryRepository.GetPurchaseGRNFoodMenuByPurchaseId(purchaseId).ToList()
                         select new SalesDeliveryModel()
                         {
                             Id = purchase.Id,
                             SalesId = purchase.SalesId,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             SalesDeliveryDate = purchase.SalesDeliveryDate,
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
                             VatableAmount = purchase.VatableAmount,
                             NonVatableAmount = purchase.NonVatableAmount
                         }).SingleOrDefault();
            if (model != null)
            {
                model.salesDeliveryDetails = (from purchasedetails in _iSalesDeliveryRepository.GetPurchaseGRNFoodMenuDetailsByPurchaseId(purchaseId)
                                            select new SalesDeliveryDetailModel()
                                            {
                                                SalesDeliveryId = purchasedetails.SalesDeliveryId,
                                                FoodMenuId = purchasedetails.FoodMenuId,
                                                SOQTY = purchasedetails.SOQTY,
                                                DeliveryQTY = purchasedetails.DeliveryQTY,
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

        public int GetPurchaseIdByPOReference(string poReference)//
        {
            return _iSalesDeliveryRepository.GetPurchaseIdByPOReference(poReference);
        }

        public SalesDeliveryModel GetPurchaseGRNFoodMenuById(long purchaseGRNId)//
        {
            SalesDeliveryModel purchaseModel = new SalesDeliveryModel();

            var model = (from purchase in _iSalesDeliveryRepository.GetPurchaseGRNFoodMenuById(purchaseGRNId).ToList()
                         select new SalesDeliveryModel()
                         {
                             Id = purchase.Id,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             SalesDeliveryDate = purchase.SalesDeliveryDate,
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
                             CustomerName = purchase.CustomerName,
                             VatableAmount = purchase.VatableAmount,
                             NonVatableAmount = purchase.NonVatableAmount
                         }).SingleOrDefault();
            if (model != null)
            {
                model.salesDeliveryDetails = (from purchasedetails in _iSalesDeliveryRepository.GetPurchaseGRNFoodMenuDetails(purchaseGRNId)
                                            select new SalesDeliveryDetailModel()
                                            {
                                                SalesDeliveryId = purchasedetails.SalesDeliveryId,
                                                FoodMenuId = purchasedetails.FoodMenuId,
                                                SOQTY = purchasedetails.SOQTY,
                                                DeliveryQTY = purchasedetails.DeliveryQTY,
                                                UnitPrice = purchasedetails.UnitPrice,
                                                GrossAmount = purchasedetails.GrossAmount,
                                                DiscountPercentage = purchasedetails.DiscountPercentage,
                                                DiscountAmount = purchasedetails.DiscountAmount,
                                                TaxAmount = purchasedetails.TaxAmount,
                                                TotalAmount = purchasedetails.TotalAmount,
                                                IngredientName = purchasedetails.IngredientName,
                                                FoodMenuName = purchasedetails.FoodMenuName,
                                                VatableAmount = purchasedetails.VatableAmount,
                                                NonVatableAmount = purchasedetails.NonVatableAmount
                                            }).ToList();
            }
            return model;
        }

        public SalesDeliveryModel GetSalesDeliveryReportById(long id)
        {
            SalesDeliveryModel purchaseModel = new SalesDeliveryModel();

            var model = (from purchase in _iSalesDeliveryRepository.GetSalesDeliveryReportById(id).ToList()
                         select new SalesDeliveryModel()
                         {
                             Id = purchase.Id,
                             SalesId = purchase.SalesId,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             SalesDeliveryDate = purchase.SalesDeliveryDate,
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
                             CustomerAddress1 = purchase.CustomerAddress1,
                             CustomerAddress2 = purchase.CustomerAddress2,
                             CustomerEmail = purchase.CustomerEmail,
                             CustomerName = purchase.CustomerName,
                             StoreName = purchase.StoreName,
                             OutletAddress1 = purchase.OutletAddress1,
                             OutletAddress2 = purchase.OutletAddress2,
                             OutletPhone = purchase.OutletPhone,
                             OutletEmail = purchase.OutletEmail,
                             InvoiceHeader = purchase.InvoiceHeader,
                             InvoiceFooter = purchase.InvoiceFooter,
                             CustomerTaxInclusive = purchase.CustomerTaxInclusive

                         }).SingleOrDefault();
            if (model != null)
            {
                model.salesDeliveryDetails = (from purchasedetails in _iSalesDeliveryRepository.GetSalesDeliveryReportFoodMenuDetails(id)
                                             select new SalesDeliveryDetailModel()
                                             {
                                                 SrNumber = purchasedetails.SrNumber,
                                                 SalesDeliveryId = purchasedetails.SalesDeliveryId,
                                                 IngredientId = purchasedetails.IngredientId,
                                                 FoodMenuId = purchasedetails.FoodMenuId,
                                                 SOQTY = purchasedetails.SOQTY,
                                                 DeliveryQTY = purchasedetails.DeliveryQTY,
                                                 UnitPrice = purchasedetails.UnitPrice,
                                                 GrossAmount = purchasedetails.GrossAmount,
                                                 DiscountPercentage = purchasedetails.DiscountPercentage,
                                                 DiscountAmount = purchasedetails.DiscountAmount,
                                                 TaxAmount = purchasedetails.TaxAmount,
                                                 TotalAmount = purchasedetails.TotalAmount,
                                                 IngredientName = purchasedetails.IngredientName,
                                                 FoodMenuName = purchasedetails.FoodMenuName,
                                                 UnitName = purchasedetails.UnitName
                                             }).ToList();
            }
            return model;
        }

        public string GetDeliveryHtmlString(SalesDeliveryModel salesInvoiceModel, string reportName)
        {
            var sb = new StringBuilder();
            decimal total = (decimal)salesInvoiceModel.salesDeliveryDetails.Count;
            int totalRow = (int)total;
            decimal rowCount = total / 15;

            int noOfPages = (int)Math.Ceiling(rowCount);
            int limit = 15, start = 0;
            int logoTop = 35;

            //HTML
            sb.Append(@"<html><body style='font-family:Helvetica; font-size:16px;'>");

            for (int j = 1; j <= noOfPages; j++)
            {
                //Header Start
                string HtmlHeader = @"<table style='border:0px solid black;' width='1280' Height='200'>
                            <tr style='border:0px solid black;' align='center' Height='50'>
	                        <td style='font-size:20px; border:0px solid black; vertical-align:top;padding-top:15px;'>
                            <div style='text-align:center; padding-top:20px; position: rela1tive;'>
                            <img src='http://shayona.rocketpos.uk/img/clientlogo.jpg' width='350' height='140' style='position: absolute; left: 15px; top: " + logoTop + @"px' >" +
                            @"<div style='font-size:28px; font-weigth:bold'><b>" + salesInvoiceModel.InvoiceHeader + @"</b></div>
                            <div style='font-size:22px;'>" + salesInvoiceModel.OutletAddress1 +
                                "</br>" + salesInvoiceModel.OutletAddress2 +
                                "</br>" + salesInvoiceModel.OutletEmail +
                                "</br>" + salesInvoiceModel.OutletPhone +
                                "</div></div></div>" +
                                "</td></tr></table>";

                sb.Append(HtmlHeader);

                //Report Title
                if (reportName == "Note") {
                    sb.Append(@"<table style='border-right:1px solid;border-left:1px solid;border-top:1px solid' width='1280' Height='35'>
                            <tr  align='center'>
                            <td style='font-size:22px;'><b>DELIVERY NOTE <b></td>
                            </tr>
                            </table>");
                }
                else
                {
                    sb.Append(@"<table style='border-right:1px solid;border-left:1px solid;border-top:1px solid' width='1280' Height='35'>
                            <tr  align='center'>
                            <td style='font-size:22px;'><b>PACKING LIST <b></td>
                            </tr>
                            </table>");
                }

                //Buyer name & two empty column
                sb.Append(@"<table style='font-size:21px;border-right:1px solid;border-left:1px solid;border-top:1px solid' width='1280' Height='170'>
                            <tr>
                            <td style='padding-left:5px; border-right:1px solid;' width='398'> <b> Buyer</b></br>  " + salesInvoiceModel.CustomerName + "</br> " + salesInvoiceModel.CustomerAddress1 + "</br> " + salesInvoiceModel.CustomerAddress2 + "</br> " + salesInvoiceModel.CustomerEmail + @"</br></br></td>
                            <td style='padding-left:5px;border-right:1px solid;' width='397'></td>
                            <td width='400'></td>
                            </tr>
                            </table>");

                //Invoice no, Order No., Invoice Date, Page No
                sb.Append(@"<table style='font-size:20px;border-right:1px solid;border-left:1px solid;border-top:1px solid' width='1280' Height='50'>
                            <tr>
                            	<td style='padding-left:5px; border-right:1px solid;' width='400'><b>No.</b></br> " + salesInvoiceModel.ReferenceNo + @"</td>");

                if (salesInvoiceModel.DeliveryNoteNumber != null)
                {
                    if (salesInvoiceModel.DeliveryNoteNumber != "")
                    {
                        sb.Append(@"<td style='padding-left:5px; border-right:1px solid;' width='400'><b>Order No. </b></br> " + salesInvoiceModel.DeliveryNoteNumber + @"</td>");
                    }
                    else
                    {
                        sb.Append(@"<td style='padding-left:5px; border-right:1px solid;' width='400'></td>");
                    }
                }
                else
                {
                    sb.Append(@"<td style='padding-left:5px; border-right:1px solid;' width='400'></td>");
                }

                sb.Append(@"<td width='400' style='padding-left:5px;'><b> Date </b></br><div style='float:left'>" + salesInvoiceModel.SalesDeliveryDate.ToShortDateString() + @"</div><div style='float:right'>" +
                    @"Page " + j.ToString() + @" of " + noOfPages.ToString() + @"</div>
                        </tr> 
                        </table>");

                //Item List
                sb.Append(@"<table style='font-size:20px;border-right:1px solid;border-left:1px solid;border-top:1px solid' width='1280'>
                            <tr Height='42' >
                                    <td style='padding-left:5px;;width:1.5%;text-align: right; padding-right:15px'><b>No</b></td>
                                    <td style='width:66.5%;text-align: left; padding-right:15px'><b>Description of Goods</b></td>
                                    <td style='width:7%;text-align: right; padding-right:40px'><b>Quantity</b></td>
                                    <td style='width:7%;text-align: left; padding-right:20px'><b>UoM</b></td>
                            </tr>");
                //        <td style='width:8%;text-align: right; padding-right:30px'><b>Rate</b></td>
                //        <td style='width:10%;text-align: right; padding-right:15px'><b>Amount</b></td>

                if (noOfPages == 1)
                {
                    limit = 15;
                    if (totalRow < limit)
                    {
                        limit = totalRow;
                    }
                }
                else
                {
                    limit = (j * 15);
                    if (totalRow < limit)
                    {
                        limit = totalRow - ((j - 1) * 15);
                    }

                    if (limit > 15)
                    {
                        limit = 15;
                    }
                }

                int i = 1;
                for (; i <= limit; i++)
                {
                    var item = salesInvoiceModel.salesDeliveryDetails[start];
                    start++;
                    sb.AppendFormat(@"<tr Height='42'>
                                    <td style='padding-left:5px; width:1.5%;text-align: right; padding-right:15px'>{0}</td>
                                    <td style='width:66.5%;text-align: left; padding-right:15px'>{1}</td>
                                    <td style='width:7%;text-align: right; padding-right:40px'>{2}</td>
                                    <td style='width:7%;text-align: left; padding-right:20px'>{3}</td>
                                 </tr>",
                                        item.SrNumber, item.FoodMenuName, item.DeliveryQTY.ToString("0.00"), item.UnitName);
                }

               // <td style='width:8%;text-align: right; padding-right:30px'>{4}</td>
              // <td style='width:10%;text-align: right; padding-right:15px'>{5}</td>
 
                for (; i <= 15; i++)
                {
                    sb.AppendFormat(@"<tr Height='42'>
                                    <td style='padding-left:5px; width:1.5%;text-align: right; padding-right:5px'></td>
                                    <td style='width:58.5%;text-align: left; padding-right:5px'></td>
                                    <td style='width:10%;text-align: right; padding-right:20px'></td>
                                    <td style='width:12%;text-align: left; padding-right:20px'></td>
                                 </tr>");
                }
              //<td style='width:8%;text-align: right; padding-right:20px'></td>
              //<td style='width:10%;text-align: right; padding-right:15px'></td>
 
                //emplty row
                sb.AppendFormat(@"<table style='border-right:1px solid;border-left:1px solid;border-top:0px' Height='210' width = '1280'><tr></tr></table>");

  /*              string strAmountWord = "", VatableAmount = "", NonVatableAmount = "", TaxAmount = "", TotalAmount = "";

                if (j == noOfPages)
                {
                    strAmountWord = NumberToWords.ConvertAmount((double)salesInvoiceModel.TotalAmount) + " KENYAN SHILLINGS";

                    if (salesInvoiceModel.CustomerTaxInclusive)
                    {
                        VatableAmount = salesInvoiceModel.VatableAmount.ToString("0.00");
                        NonVatableAmount = salesInvoiceModel.NonVatableAmount.ToString("0.00");
                        TaxAmount = salesInvoiceModel.TaxAmount.ToString("0.00");
                    }
                    TotalAmount = salesInvoiceModel.TotalAmount.ToString("0.00");
                }

                //Reamrks & Totals
                sb.Append(@"<table  style='font-size:22px;border-right:1px solid;border-left:1px solid;border-top:1px solid' width = '1280' Height = '119' >
                        <tr  Height='40'>
                            <td rowspan=2 style='padding-left:5px; width:76%; vertical-align:top; text-align : left;font-size:12px'>
                                    <b>Remarks : </b></br>" + salesInvoiceModel.Notes + @"</td>
                            <td style='width:12%;  text-align:right; border-left:1px solid; border-right:1px solid; border-bottom:1px solid; padding-right:15px;font-size:20px'><b>Vatable:</b></td >
                            <td style='width:12%;  text-align:right;; border-bottom:1px solid; padding-right:15px;font-size:20px'><b>" + VatableAmount + @"</b></td>
                        </tr>
                        <tr Height='40'>
                            <td style='width:12%;  text-align:right; border-right:1px solid; border-left:1px solid; border-bottom:1px solid; padding-right:15px;font-size:20px'><b>NON-VAT:</b></td>
                            <td style='width:12%;  text-align:right;; border-bottom:1px solid; padding-right:15px;font-size:20px'><b>" + NonVatableAmount + @"</b></td>
                        </tr>
                        <tr Height='40'>
                            <td rowspan=2><div style='vertical-align:bottom;text-align:left;padding-left:5px; '>" + strAmountWord.ToString().ToUpper() + @"</div></td>
                            <td style='width:12%;  text-align:right; border-right:1px solid; border-left:1px solid; border-bottom:1px solid; padding-right:15px;font-size:20px'><b>VAT Total:</b></td>
                            <td style='width:12%;  text-align:right; border-bottom:1px solid; padding-right:15px;font-size:20px'><b>" + TaxAmount + @"</b></td>
                        </tr>
                        <tr Height='40' style='border:0px'>
                            <td style='width:12%;  text-align:right; border-left:1px solid; border-right:1px solid;; padding-right:15px;font-size:20px'><b>Total</b></td>
                            <td style='width:12%;  text-align:right; padding-right:15px;font-size:20px'><b>" + TotalAmount + @"</b></td>
                        </tr>
                    </table>");
  */
                //Terms and condition
                sb.Append(@"<table style='font-size:20px;border:1px solid; ' width = '1280' Height = '220'>
                            <tr Height='50'>
                                <td colspan='7' style='padding-left:5px; vertical-align:top;'><b>Terms and Conditions: </b>");

                string strInvoiceTerms = LoginInfo.InvoiceTerms;
                string[] strSplit = strInvoiceTerms.Split('\n');

                for (i = 0; i < strSplit.Length; i++)
                {
                    sb.Append(@"</br>" + strSplit[i]);
                }

                sb.Append(@"</td>
                        </tr>
                        </table>");

                //emplty row
                sb.AppendFormat(@"<table style='border:0px solid;' Height='40'></table>");

                //logo at header settings
                logoTop = logoTop + 1654 + (j * 14);
            }

            //end of body and html
            sb.Append(@"</body></html>");

            return sb.ToString();
        }

        public SalesDeliveryModel GetViewSalesDeliveryFoodMenuById(long purchaseGRNId)
        {
            SalesDeliveryModel purchaseModel = new SalesDeliveryModel();

            var model = (from purchase in _iSalesDeliveryRepository.GetViewSalesDeliveryFoodMenuById(purchaseGRNId).ToList()
                         select new SalesDeliveryModel()
                         {
                             Id = purchase.Id,
                             ReferenceNo = purchase.ReferenceNo,
                             CustomerId = purchase.CustomerId,
                             EmployeeId = purchase.EmployeeId,
                             StoreId = purchase.StoreId,
                             SalesDeliveryDate = purchase.SalesDeliveryDate,
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
                             CustomerName = purchase.CustomerName,
                             StoreName = purchase.StoreName,
                             SOReferenceNo = purchase.SOReferenceNo,
                             SODate = purchase.SODate,
                             VatableAmount = purchase.VatableAmount,
                             NonVatableAmount = purchase.NonVatableAmount
                         }).SingleOrDefault();
            if (model != null)
            {
                model.salesDeliveryDetails = (from purchasedetails in _iSalesDeliveryRepository.GetViewSalesDeliveryFoodMenuDetails(purchaseGRNId)
                                            select new SalesDeliveryDetailModel()
                                            {
                                                SalesDeliveryId = purchasedetails.SalesDeliveryId,
                                                FoodMenuId = purchasedetails.FoodMenuId,
                                                SOQTY = purchasedetails.SOQTY,
                                                DeliveryQTY = purchasedetails.DeliveryQTY,
                                                UnitPrice = purchasedetails.UnitPrice,
                                                GrossAmount = purchasedetails.GrossAmount,
                                                DiscountPercentage = purchasedetails.DiscountPercentage,
                                                DiscountAmount = purchasedetails.DiscountAmount,
                                                TaxAmount = purchasedetails.TaxAmount,
                                                TotalAmount = purchasedetails.TotalAmount,
                                                IngredientName = purchasedetails.IngredientName,
                                                FoodMenuName = purchasedetails.FoodMenuName,
                                                ItemType = purchasedetails.ItemType,
                                                UnitName = purchasedetails.UnitName,
                                                VatableAmount = purchasedetails.VatableAmount,
                                                NonVatableAmount = purchasedetails.NonVatableAmount
                                            }).ToList();
            }
            return model;
        }

    }
}
