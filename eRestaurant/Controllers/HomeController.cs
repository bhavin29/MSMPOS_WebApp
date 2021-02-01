using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Framework;
using RocketPOS.Models.Reports;
using RocketPOS.Interface.Services.Reports;

namespace RocketPOS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICommonService _iCommonService;
        private readonly IReportService _iReportService;

        public HomeController(ILogger<HomeController> logger, ICommonService commonService, IReportService iReportService)
        {
            _logger = logger;
            _iCommonService = commonService;
            _iReportService = iReportService;
        }

        public IActionResult Index()
        {
            if (LoginInfo.Userid == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                List<DataHistorySyncReportModel> dataHistorySyncReportModels = new List<DataHistorySyncReportModel>();

                dataHistorySyncReportModels = _iReportService.GetDataSyncHistoryReport();

                var result = dataHistorySyncReportModels
                            .GroupBy(x => x.Outlet)
                            .Select(x => x.FirstOrDefault()).Where(x => x.ProcessStatus == "Completed").ToList();

    
                ViewData["DataHistorySync"] = result;

                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ErrorModel errorModel = new ErrorModel();
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            errorModel.MethodName = exceptionDetails.GetType().FullName;
            errorModel.ErrorPath = exceptionDetails.Path;
            errorModel.ErrorDetails = exceptionDetails.Error.StackTrace;
            errorModel.UserId = 1;
            int result = _iCommonService.InsertErrorLog(errorModel);
            return View("Error");
        }
    }
}
