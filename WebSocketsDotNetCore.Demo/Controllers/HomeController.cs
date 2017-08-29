using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSocketsDotNetCore.Demo.Models;
using Microsoft.Extensions.Options;
using WebSocketsDotNetCore.Demo.Services;

namespace WebSocketsDotNetCore.Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyOptions _options;
        public HomeController(IOptions<MyOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }
        public IActionResult Index()
        {
            var option1 = _options.Option1;
            var option2 = _options.Option2;
            return Content($"option1 = {option1}, option2 = {option2}");
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
