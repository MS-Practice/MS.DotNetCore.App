using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreControllers.Demo.Filters
{
    public class SampleActionFilterAttribute : TypeFilterAttribute
    {
        public SampleActionFilterAttribute() : base(typeof(SampleActionFilterImp))
        {
        }

        private class SampleActionFilterImp : IActionFilter
        {
            private readonly ILogger _logger;
            public SampleActionFilterImp(ILoggerFactory loggerFactory)
            {
                _logger = loggerFactory.CreateLogger<SampleActionFilterAttribute>();
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                _logger.LogInformation("Business action complete.");
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                _logger.LogInformation("Business action starting...");
            }
        }
    }
}
