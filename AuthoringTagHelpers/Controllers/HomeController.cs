using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthoringTagHelpers.Models;

namespace AuthoringTagHelpers.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(bool approved = false)
        {
            return View(new WebsiteContext
            {
                Approved = approved,
                CopyrightYear = 2017,
                Version = new Version(2, 0, 0, 0),
                TagsToShow = 20
            });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
