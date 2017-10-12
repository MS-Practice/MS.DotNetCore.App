using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace NetCoreControllers.Demo.Filters
{
    public class AddHeaderWithFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new InternalAddHeaderFilter();
        }

        private class InternalAddHeaderFilter : IResultFilter
        {
            public void OnResultExecuted(ResultExecutedContext context)
            {
                context.HttpContext.Response.Headers.Add(
                    "Internal", new string[] { "Header Added" });
            }

            public void OnResultExecuting(ResultExecutingContext context)
            {
                
            }
        }
    }
}
