using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Transaction
{
    public class ProductionEntryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
