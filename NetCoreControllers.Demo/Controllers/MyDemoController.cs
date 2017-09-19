using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreControllers.Demo.Controllers
{
    public class MyDemoController : Controller
    {
        // GET: /<controller>/
        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        public IActionResult Index()
        {
            ViewBag.Title = "Index";
            return View();
        }
        //[HttpGet("/products")]
        public IActionResult GetProducts()
        {
            List<string> productNames = new List<string>
            {
                "p1","p2","p3"
            };
            return View(productNames);
        }
    }
}
