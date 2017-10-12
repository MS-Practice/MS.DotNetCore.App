using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreControllers.Demo.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreControllers.Demo.Controllers
{
    [AddHeader("Author","Marson Shine@163.com")]
    public class SampleController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return Content("Examine the headers using developer tools.");
        }

        public IActionResult SomeResource()
        {
            return Content("Successful access to resource - header should be set");
        }
        [ShortCircuitingResourceFilter]
        public IActionResult CompareSomeResource()
        {
            return Content("Successful access to resource - header should be set");
        }
    }
}
