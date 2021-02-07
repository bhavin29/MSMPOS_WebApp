using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketPOS.Interface.Services;

namespace RocketPOS.Controllers.Transaction
{
    public class SchedulerController : Controller
    {
        private readonly ISchedulerService _schedulerService;

        public SchedulerController(ISchedulerService schedulerService)
        {
            _schedulerService = schedulerService;
        }

        public IActionResult Index()
        {
            int result = 0;
            result = _schedulerService.SalesPOSInventorySync();

            return View();
        }
    }
}
