using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreFileUploader.Demo.Interfaces;

namespace NetCoreFileUploader.Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDateTime _dateTime;
        //constructor dependency injection
        public HomeController(IDateTime dateTime)
        {
            _dateTime = dateTime;
        }
        public IActionResult Index()
        {
            var serverTime = _dateTime.Now;
            if (serverTime.Hour < 12)
            {
                ViewData["Message"] = "It's morning here - Good Morning!";
            }
            else if (serverTime.Hour < 17)
            {
                ViewData["Message"] = "It's afternoon here - Good Afternoon!";
            }
            else
            {
                ViewData["Message"] = "It's evening here - Good Evening!";
            }
            return View();
        }

        //Action dependency injection
        public IActionResult About([FromServices]IDateTime dateTime)
        {
            ViewData["Message"] = "Currently on the server the time is " + dateTime.Now;
            return Content(ViewData["Message"].ToString());
        }
    }
}