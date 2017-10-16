using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreControllers.Demo.Filters;
using System.Globalization;

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
        [TypeFilter(typeof(AddHeaderAttribute), Arguments = new string[] { "Author", "MarsonShine@163.com" })]
        public IActionResult Hi(string name)
        {
            return Content($"Hi {name}");
        }
        [SampleActionFilter]
        public IActionResult NoConstructorParameters()
        {
            return Content("NoConstructorParameters");
        }
        [MiddlewareFilter(typeof(LocalizationPipeline))]
        public IActionResult Pipline() {
            return Content($"CurrentCulture:{CultureInfo.CurrentCulture.Name},CurrentUICulture:{CultureInfo.CurrentUICulture.Name}");
        }
    }
}
