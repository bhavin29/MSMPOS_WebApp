﻿using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Interface.Services.Reports;
using RocketPOS.Models.Reports;
using RocketPOS.Interface.Repository.Reports;

namespace RocketPOS.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _iReportRepository;

        public ReportService(IReportRepository iReportRepository)
        {
            _iReportRepository = iReportRepository;
        }
    
        public List<InventoryReportModel> GetInventoryReport(InventoryReportParamModel inventoryReportParamModel)
        {
            return _iReportRepository.GetInventoryReport(inventoryReportParamModel);
        }

        public List<PurchaseReportModel> GetPurchaseReport(DateTime fromDate, DateTime toDate)
        {
            return _iReportRepository.GetPurchaseReport(fromDate,toDate);
        }
    }
}
