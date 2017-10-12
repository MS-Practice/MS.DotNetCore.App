using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace NetCoreControllers.Demo.Filters
{
    public class AddHeaderFilterWithDI : IResultFilter
    {
        private ILogger _logger;
        public AddHeaderFilterWithDI(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AddHeaderFilterWithDI>();
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var headerName = "OnResultExecuting";
            context.HttpContext.Response.Headers.Add(
                headerName, new string[] { "ResultExecutingSuccessfully" });
            _logger.LogInformation($"Header added :{headerName}");
        }
    }
}
