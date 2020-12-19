using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketPOS.Models;

namespace RocketPOS.Controllers.Reports
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Inventory(int? id)
        {
            AddonsModel addonsModel = new AddonsModel();

            //return View(addonsModel);
            return View("../Reports/Inventory");

        }
    }
}
