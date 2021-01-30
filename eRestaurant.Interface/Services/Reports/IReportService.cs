﻿using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models.Reports;

namespace RocketPOS.Interface.Services.Reports
{
    public interface IReportService
    {
        List<InventoryReportModel> GetInventoryReport(InventoryReportParamModel inventoryReportParamModel);
        List<InventoryReportModel> GetInventoryStockList(int supplierId, int storeId);
        List<InventoryDetailReportModel> GetInventoryDetailReport(InventoryReportParamModel inventoryReportParamModel, int id);
        List<PurchaseReportModel> GetPurchaseReport(DateTime fromDate, DateTime toDate);
        List<OutletRegisterReportModel> GetOutletRegisterReport(int OutletRegisterId);

        PrintReceiptA4 GetPrintReceiptA4Detail(int CustomerOrderId);
    }
}
