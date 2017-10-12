using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreControllers.Demo.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreControllers.Demo.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        [ServiceFilter(typeof(AddHeaderFilterWithDI))]
        public IActionResult Index()
        {
            return Content("");
        }
    }
}
