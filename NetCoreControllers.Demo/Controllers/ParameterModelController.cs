using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreControllers.Demo.Controllers
{
    public class ParameterModelController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public string GetById([MustBeInRouteParameterModelConvention]int id)
        {
            return $"Bound to id {id}";
        }
    }
}
