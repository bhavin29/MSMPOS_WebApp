using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models.Reports;

namespace RocketPOS.Interface.Repository.Reports
{
    public interface IReportRepository
    {
        List<InventoryReportModel> GetInventoryReport(InventoryReportParamModel inventoryReportParamModel);
        List<PurchaseReportModel> GetPurchaseReport(DateTime fromDate, DateTime toDate);


    }
}
