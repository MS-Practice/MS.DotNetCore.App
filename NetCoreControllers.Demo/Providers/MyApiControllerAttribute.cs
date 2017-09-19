using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreControllers.Demo.Providers
{
    public class MyApiControllerAttribute : Attribute, IRouteTemplateProvider
    {
        public string Template => "api/[controller]";

        public int? Order { get; set; }

        public string Name { get; set; }
    }
}
