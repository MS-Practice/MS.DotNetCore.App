using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreControllers.Demo.Controllers
{
    [ControllerDescription("Controller Description")]
    public class DescriptionAttributesController : Controller
    {
        // GET: /<controller>/
        public string Index()
        {
            return "Description: " + ControllerContext.ActionDescriptor.Properties["description"];
        }

        [ActionDescription("Action Description")]
        public string UseActionDescriptionAttribute()
        {
            return "Description: " + ControllerContext.ActionDescriptor.Properties["description"];
        }
    }
}
